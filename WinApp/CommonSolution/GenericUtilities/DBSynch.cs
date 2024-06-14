/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - DBSynch 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 *2.140.0     01-Oct-2021            Mathew Ninan   Added support for nvarchar
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace Semnox.Core.GenericUtilities
{
    public static class DBSynch
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static readonly object lockObject = new object();

        public static void SynchronizeTableData(DataTable Table, int SiteId, SqlTransaction SQLTrx, ref string Message, ref int UpdatedRows, ref int InsertedRows)
        {
            log.LogMethodEntry(Table, SiteId, SQLTrx, Message, UpdatedRows, InsertedRows);
            SynchronizeTableData(Table, SiteId, SQLTrx.Connection, SQLTrx, ref Message, ref UpdatedRows, ref InsertedRows);
            log.LogVariableState("Message", Message);
            log.LogVariableState("Updated rows", UpdatedRows);
            log.LogVariableState("Inserted rows", InsertedRows);
            log.LogMethodExit();
        }

        public static void SynchronizeTableData(DataTable Table, int SiteId, SqlConnection SQLConnection, SqlTransaction SQLTrx, ref string Message, ref int UpdatedRows, ref int InsertedRows)
        {
            log.LogMethodEntry(Table, SiteId, SQLConnection, SQLTrx, Message, UpdatedRows, InsertedRows);
            Message = "";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = SQLConnection;
            cmd.Transaction = SQLTrx;
            cmd.CommandTimeout = 300;

            cmd.CommandText = "select OBJECT_ID('" + Table.TableName + "')";
            if (cmd.ExecuteScalar() == DBNull.Value) // table not found
            {
                log.LogVariableState("Message", Message);
                log.LogVariableState("Updated rows", UpdatedRows);
                log.LogVariableState("Inserted rows", InsertedRows);
                log.LogMethodExit();
                return;
            }
              

            string tempTablename = Table.TableName + "_" + SiteId.ToString();
            DataTable DBColumnType = SQLGetType(Table.TableName, cmd);
            cmd.CommandText = "if OBJECT_ID('" + tempTablename + "') is not null drop table " + tempTablename + "; " + GetCreateFromDataTableSQL(tempTablename, Table, DBColumnType);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "create index IX_" + tempTablename + "_guid on " + tempTablename + "(guid)";
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();

            SqlBulkCopy sqlBulk = new SqlBulkCopy(cmd.Connection, SqlBulkCopyOptions.Default, SQLTrx);
            sqlBulk.DestinationTableName = tempTablename;
            sqlBulk.BulkCopyTimeout = 300;
            sqlBulk.WriteToServer(Table);
            sqlBulk.Close();

            cmd.CommandText = "update " + tempTablename + " set site_id = @siteId where site_id is null";
            cmd.Parameters.AddWithValue("@siteId", SiteId);
            cmd.ExecuteNonQuery();
            log.LogVariableState("@siteId", SiteId);

            cmd.CommandText = "select top 1 InsertOnly, IgnoreColumnsOnRoaming, IgnoreOnError from DBSynchTables where TableName = '" + Table.TableName + "'";
            SqlDataAdapter daSynch = new SqlDataAdapter(cmd);
            DataTable dtSynch = new DataTable();
            daSynch.Fill(dtSynch);

            bool InsertOnly = false;
            bool IgnoreError = false;
            string IgnoreColumnsOnRoaming = "";
            if (dtSynch.Rows.Count > 0)
            {
                object o = dtSynch.Rows[0][0];
                if (o.ToString() == "N" || o.ToString() == "")
                    InsertOnly = false;
                else
                    InsertOnly = true;

                IgnoreColumnsOnRoaming = dtSynch.Rows[0][1].ToString().Trim().ToLower();

                o = dtSynch.Rows[0][2];
                if (o.ToString() == "N" || o.ToString() == "")
                    IgnoreError = false;
                else
                    IgnoreError = true;
            }

            cmd.CommandText = @"select c.name column_name, tParent.name parent_table, cParent.name parent_column
		                        from sys.tables t,
		                        sys.columns c left outer join
		                        sys.foreign_key_columns f 
		                        on f.parent_object_id = c.object_id
		                        and f.parent_column_id = c.column_id
		                        left outer join sys.columns cParent
		                        on f.referenced_object_id = cParent.object_id
		                        and f.referenced_column_id = cParent.column_id
		                        left outer join sys.tables tParent
		                        on  f.referenced_object_id = tParent.object_id
		                        where c.object_id = t.object_id
		                        and c.is_identity != 1
		                        and t.name = @TableName";
            cmd.Parameters.AddWithValue("@TableName", Table.TableName);
            log.LogVariableState("@TableName", Table.TableName);

            string column_name, parent_table, parent_column;
            string columnList, fromClause;
            string query;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cmd.CommandText = "select count(1) from site";
            //bool isHQ = (Convert.ToInt32(cmd.ExecuteScalar()) > 1);

            //List<string> RoamingTables = new List<string>() { "cards", "customers", "cardtype", "cardcreditplus", "cardcreditplusconsumption" };
            if (!InsertOnly)
            {
                columnList = "update tbl set ";
             //   if (isHQ)
               //     fromClause = "from " + Table.TableName + " as tbl, " + tempTablename + " src where src.Guid = tbl.Guid and tbl.Guid = @Guid and tbl.site_id = @site_id";
                //else
                    fromClause = "from " + Table.TableName + " as tbl, " + tempTablename + " src where src.Guid = tbl.Guid and tbl.Guid = @Guid";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    column_name = dt.Rows[i]["column_name"].ToString();
                    if (!columnFoundInTable(column_name, Table))
                        continue;

                    parent_table = dt.Rows[i]["parent_table"].ToString();
                    parent_column = dt.Rows[i]["parent_column"].ToString();

                    if (parent_table == "")
                    {
                        columnList = columnList + "tbl." + column_name + " = src." + column_name + ", ";
                    }
                    else
                    {
                        if (IgnoreColumnsOnRoaming.Contains(column_name.ToLower()))
                        {
                            //if (isHQ && !RoamingTables.Contains(Table.TableName.ToLower()))
                            //    columnList = columnList + "tbl." + column_name + " = case when src.site_id = @site_id then (select top 1 " + parent_table + "." + parent_column + " from " + parent_table + " where guid = src." + column_name + ") else tbl." + column_name + " end, ";
                            //else
                                columnList = columnList + "tbl." + column_name + " = case when src.site_id = @site_id then (select top 1 " + parent_table + "." + parent_column + " from " + parent_table + " where guid = src." + column_name + ") else tbl." + column_name + " end, ";
                        }
                        else
                        {
                          //  if (isHQ && !RoamingTables.Contains(Table.TableName.ToLower()))
                            //    columnList = columnList + "tbl." + column_name + " = (select top 1 " + parent_table + "." + parent_column + " from " + parent_table + " where guid = src." + column_name + " and site_id = @site_id), ";
                          //  else
                                columnList = columnList + "tbl." + column_name + " = (select top 1 " + parent_table + "." + parent_column + " from " + parent_table + " where guid = src." + column_name + "), ";
                        }
                    }
                }
                columnList = columnList.TrimEnd(',', ' ');

                query = columnList + ' ' + fromClause;
                cmd.CommandText = query + "; select @@ROWCOUNT";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Guid", DBNull.Value);
                cmd.Parameters.AddWithValue("@site_id", SiteId);
                log.LogVariableState("@Guid", DBNull.Value);
                log.LogVariableState("@site_id", SiteId);
                SqlCommand cmdData = new SqlCommand();
                cmdData.Connection = cmd.Connection;
                cmdData.Transaction = cmd.Transaction;

                cmdData.CommandText = "select Guid from " + tempTablename;
                SqlDataAdapter daData = new SqlDataAdapter(cmdData);
                DataTable dtData = new DataTable();
                daData.Fill(dtData);
                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    cmd.Parameters["@Guid"].Value = dtData.Rows[i]["Guid"];
                    try
                    {
                        UpdatedRows += Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error in updating the rows", ex);
                        if (IgnoreError)
                        {
                            Message += Table.TableName + ":" + dtData.Rows[i]["Guid"].ToString() + " - " + ex.Message + Environment.NewLine;
                            continue;
                        }
                        else
                        {
                            log.LogMethodExit(null, "Throwing Exception-"+Table.TableName + ":" + dtData.Rows[i]["Guid"].ToString() + " - " + ex.Message + cmd.CommandText);
                            throw new Exception(Table.TableName + ":" + dtData.Rows[i]["Guid"].ToString() + " - " + ex.Message + cmd.CommandText);
                        }
                           
                    }
                }
            }

            columnList = "insert into " + Table.TableName + "(";
           // if (isHQ && !RoamingTables.Contains(Table.TableName.ToLower()))
          //      fromClause = "from " + tempTablename + " src where not exists (select 1 from " + Table.TableName + " a where a.Guid = src.Guid and a.site_id = @site_id) and src.Guid = @Guid";
          //  else
                fromClause = "from " + tempTablename + " src where not exists (select 1 from " + Table.TableName + " a where a.Guid = src.Guid) and src.Guid = @Guid";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                column_name = dt.Rows[i]["column_name"].ToString();
                if (!columnFoundInTable(column_name, Table))
                    continue;

                columnList = columnList + column_name + ", ";
            }
            columnList = columnList.TrimEnd(',', ' ');
            columnList += ") select ";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                column_name = dt.Rows[i]["column_name"].ToString();
                if (!columnFoundInTable(column_name, Table))
                    continue;

                parent_table = dt.Rows[i]["parent_table"].ToString();
                parent_column = dt.Rows[i]["parent_column"].ToString();

                if (parent_table == "")
                {
                    columnList = columnList + column_name + ", ";
                }
                else
                {
                 //   if (isHQ && !RoamingTables.Contains(Table.TableName.ToLower()))
                //        columnList = columnList + " (select top 1 " + parent_table + "." + parent_column + " from " + parent_table + " where guid = src." + column_name + " and site_id = @site_id), ";
                 //   else
                        columnList = columnList + " (select top 1 " + parent_table + "." + parent_column + " from " + parent_table + " where guid = src." + column_name + "), ";
                }
            }
            columnList = columnList.TrimEnd(',', ' ');

            query = columnList + ' ' + fromClause;
            cmd.CommandText = query + "; select @@ROWCOUNT";

            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Guid", DBNull.Value);
            cmd.Parameters.AddWithValue("@site_id", SiteId);
            log.LogVariableState("@Guid", DBNull.Value);
            log.LogVariableState("@site_id", SiteId);
            SqlCommand cmdInsData = new SqlCommand();
            cmdInsData.Connection = cmd.Connection;
            cmdInsData.Transaction = cmd.Transaction;

            cmdInsData.CommandText = "select Guid from " + tempTablename;
            SqlDataAdapter daInsData = new SqlDataAdapter(cmdInsData);
            DataTable dtInsData = new DataTable();
            daInsData.Fill(dtInsData);
            for (int i = 0; i < dtInsData.Rows.Count; i++)
            {
                cmd.Parameters["@Guid"].Value = dtInsData.Rows[i]["Guid"];
                try
                {
                    InsertedRows += Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    log.Error("Error occured while inserting rows", ex);
                    if (IgnoreError)
                    {
                        Message += Table.TableName + ":" + dtInsData.Rows[i]["Guid"].ToString() + " - " + ex.Message + Environment.NewLine;
                        continue;
                    }
                    else
                    {
                        log.LogMethodExit(null, "Throwing exception-" + Table.TableName + ":" + dtInsData.Rows[i]["Guid"].ToString() + " - " + ex.Message + cmd.CommandText);
                        throw new Exception(Table.TableName + ":" + dtInsData.Rows[i]["Guid"].ToString() + " - " + ex.Message + cmd.CommandText);
                    }
                        
                }
            }
            
            cmd.CommandText = "drop table " + tempTablename;
            cmd.ExecuteNonQuery();
            log.LogVariableState("Message", Message);
            log.LogVariableState("Updated rows", UpdatedRows);
            log.LogVariableState("Inserted rows", InsertedRows);
            log.LogMethodExit();
        }

        static bool columnFoundInTable(string column, DataTable Table)
        {
            log.LogMethodEntry(column, Table);
            foreach (DataColumn dc in Table.Columns)
            {
                if (dc.ColumnName.Equals(column, StringComparison.CurrentCultureIgnoreCase))
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        static string GetCreateFromDataTableSQL(string tableName, DataTable table, DataTable DBColumnType)
        {
            log.LogMethodEntry(tableName, table, DBColumnType);
            string sql = "CREATE TABLE [" + tableName + "] (\n";

            // columns
            foreach (DataColumn column in table.Columns)
            {
                if (column.DataType.ToString() == "System.Guid")
                    sql += "[" + column.ColumnName + "] uniqueidentifier,\n";
                else
                    sql += "[" + column.ColumnName + "] " + SQLGetType(column, DBColumnType) + ",\n";
            }
            sql = sql.TrimEnd(new char[] { ',', '\n' }) + "\n";
            log.LogMethodExit(sql + ")");
            
            return sql + ")";
        }

        // Return T-SQL data type definition, based on schema definition for a column
        public static string SQLGetType(object type, int columnSize, int numericPrecision, int numericScale)
        {
            log.LogMethodEntry();
            switch (type.ToString())
            {
                case "System.String":
                    {
                        string returnvalue= ("NVARCHAR(" + ((columnSize == -1) ? 255 : columnSize) + ")");
                        log.LogMethodExit(returnvalue);
                        return returnvalue;
                    }

                case "System.Decimal":
                    if (numericScale > 0)
                    {
                        log.LogMethodExit("REAL");
                        return "REAL";
                    }
                       
                    else if (numericPrecision > 10)
                    {
                        log.LogMethodExit("BIGINT");
                        return "BIGINT";
                    }
                        
                    else
                    {
                        log.LogMethodExit("INT");
                        return "INT";
                    }
                      

                case "System.Double":
                case "System.Single":
                    {
                        log.LogMethodExit("REAL");
                        return "REAL";
                    }
                    

                case "System.Int64":
                    {
                        log.LogMethodExit("BIGINT");
                        return "BIGINT";
                    }

                case "System.Int16":
                case "System.Int32":
                    {
                        log.LogMethodExit("INT");
                        return "INT";
                    }
                   

                case "System.DateTime":
                    {
                        log.LogMethodExit("DATETIME");
                        return "DATETIME";
                    }
                 

                case "System.Guid":
                    {
                        log.LogMethodExit("uniqueidentifier");
                        return "uniqueidentifier";
                    }
                
                case "System.Byte[]":
                    {
                        log.LogMethodExit("image");
                        return "image";
                    }

                case "System.Boolean":
                    {
                        log.LogMethodExit("bit");
                        return "bit";
                    }

                default:
                    {
                        log.LogMethodExit(null, "Throwing Exception"+type.ToString() + " not implemented.");
                        throw new Exception(type.ToString() + " not implemented.");
                    }
                    
            }
        
        }

        // Overload based on DataColumn from DataTable type
        static string SQLGetType(DataColumn column)
        {
            log.LogMethodEntry(column);
            string returnvalue = (SQLGetType(column.DataType, column.MaxLength, 10, 2));
            log.LogMethodExit();
            return (returnvalue);
        }

        // Overload based on DataColumn and table name
        static DataTable SQLGetType(string TableName, SqlCommand cmd)
        {
            log.LogMethodEntry(TableName);
            cmd.CommandText = @"select c.name, 
	                                case ty.name 
		                                when 'char' then ty.name + '(' + maxLength + ')'
		                                when 'nchar' then ty.name + '(' + maxLength + ')'
		                                when 'varchar' then ty.name + '(' + maxLength + ')'
		                                when 'nvarchar' then ty.name + '(' + case when c.max_length = -1 then 'MAX' 
										  else cast(c.max_length/2 as varchar) end + ')'
		                                when 'varbinary' then ty.name + '(' + maxLength + ')'
		                                when 'numeric' then ty.name + '(' + cast(c.precision as varchar) + ', ' + cast(c.scale as varchar) + ')'
		                                when 'decimal' then ty.name + '(' + cast(c.precision as varchar) + ', ' + cast(c.scale as varchar) + ')'
		                                else ty.name end type
                                 from (select name, max_length, case when max_length = -1 then 'MAX' else cast(max_length as varchar) end maxLength, precision, scale,
										system_type_id, user_type_id
                                        from sys.columns
                                        where object_id = object_id(@tableName)) c,
                                sys.types ty
                                where ty.system_type_id = c.system_type_id
                                and ty.user_type_id = c.user_type_id";

            cmd.Parameters.AddWithValue("@tableName", TableName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            log.LogMethodExit();
            return (dt);
        }

        // Overload based on DataColumn and table name
        static string SQLGetType(DataColumn column, DataTable DBColumnType)
        {
            log.LogMethodEntry(column, DBColumnType);
            for (int i = 0; i < DBColumnType.Rows.Count; i++)
            {
                if (column.ColumnName.Equals(DBColumnType.Rows[i]["name"].ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    string returnvalue = (DBColumnType.Rows[i]["type"].ToString());
                    log.LogMethodExit(returnvalue);
                    return(returnvalue);
                }
            }
            string returnvalue1 = SQLGetType(column);

            log.LogMethodExit(returnvalue1);
            return (returnvalue1);
        }

        public static void CreateRoamingData(DataTable dtRoamTable, int SiteId, string RoamingSites, DateTime UploadEndTime, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(dtRoamTable, SiteId, RoamingSites, UploadEndTime, SQLTrx);
            CreateRoamingData(dtRoamTable, SiteId, RoamingSites, UploadEndTime, SQLTrx.Connection, SQLTrx);
            log.LogMethodExit();
        }

        public static void CreateRoamingData(DataTable dtRoamTable, int SiteId, string RoamingSites, DateTime UploadEndTime, SqlConnection SQLConnection, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(dtRoamTable, SiteId, RoamingSites, UploadEndTime, SQLConnection, SQLTrx);
            // create roaming data in auto roam sites and original site 
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = SQLConnection;
            cmd.Transaction = SQLTrx;
            cmd.CommandText = @"insert into DBSynchLog (Operation, Guid, TableName, TimeStamp, site_id)
                              select 'I', @guid, @tableName, @timeStamp, s.site_id 
                                from site s 
                               where (active_flag = 'Y' OR active_flag IS NULL)
                                and site_id != @site_id 
                                and (site_id in " + RoamingSites + " or site_id = @tableDataSiteId)";
            cmd.Parameters.AddWithValue("@guid", DBNull.Value);
            cmd.Parameters.AddWithValue("@site_id", SiteId);
            cmd.Parameters.AddWithValue("@tableDataSiteId", DBNull.Value);
            cmd.Parameters.AddWithValue("@tableName", dtRoamTable.TableName);
            log.LogVariableState("@guid", DBNull.Value);
            log.LogVariableState("@site_id", SiteId);
            log.LogVariableState("@tableDataSiteId", DBNull.Value);
            log.LogVariableState("@tableName", dtRoamTable.TableName);
            if (UploadEndTime == DateTime.MinValue)
            {
                cmd.Parameters.AddWithValue("@timeStamp", DBNull.Value);
                log.LogVariableState("@timeStamp", DBNull.Value);
            }
            
            else
            {
                cmd.Parameters.AddWithValue("@timeStamp", UploadEndTime);
                log.LogVariableState("@timeStamp", UploadEndTime);
            }
                

            foreach (DataRow dr in dtRoamTable.Rows)
            {
                cmd.Parameters["@guid"].Value = dr["guid"];
                log.LogVariableState("@guid", dr["guid"]);
                cmd.Parameters["@tableDataSiteId"].Value = dr["site_id"];
                log.LogVariableState("@tableDataSiteId", dr["site_id"]);
                if (UploadEndTime == DateTime.MinValue)
                    cmd.Parameters["@timeStamp"].Value = dr[1];
                cmd.ExecuteNonQuery();
            }
            log.LogMethodExit();
        }
        public static void CreateRoamingData(string tablename, Guid guid, int SiteId, string RoamingSites, DateTime UploadEndTime, SqlConnection SQLConnection, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(tablename, guid, SiteId, RoamingSites, UploadEndTime, SQLConnection, sqlTransaction);
            List<SqlParameter> sqlParameterList = new List<SqlParameter>();
            string sqlQuery = @"insert into DBSynchLog (Operation, Guid, TableName, TimeStamp, site_id)
                              select 'I', @guid, @tableName, @timeStamp, s.site_id 
                                from site s 
                               where (active_flag = 'Y' OR active_flag IS NULL)
                                and site_id != @site_id 
                                and (site_id in " + RoamingSites + " or site_id = @tableDataSiteId)";
           
            
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = SQLConnection;
            cmd.Transaction = sqlTransaction;
            cmd.CommandText = sqlQuery;
            cmd.Parameters.Add(new SqlParameter("@guid", guid));
            cmd.Parameters.Add(new SqlParameter("@site_id", SiteId));
            cmd.Parameters.Add(new SqlParameter("@tableDataSiteId", DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@tableName", tablename));
            log.LogVariableState("@guid", guid);
            log.LogVariableState("@site_id", SiteId);
            log.LogVariableState("@tableDataSiteId", DBNull.Value);
            log.LogVariableState("@tableName", tablename);
            if (UploadEndTime == DateTime.MinValue)
            {
                cmd.Parameters.Add(new SqlParameter("@timeStamp", DBNull.Value));
                log.LogVariableState("@timeStamp", DBNull.Value);
            }
            else
            {
                cmd.Parameters.Add(new SqlParameter("@timeStamp", UploadEndTime));
                log.LogVariableState("@timeStamp", UploadEndTime);
            }
            cmd.ExecuteScalar();            
            log.LogMethodExit();
        }
        public static string getRoamingSitesForEntity(string entityName, int site_id, Guid entityRowGuid, SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(entityName, site_id, entityRowGuid, sqlConnection, sqlTransaction);
            string roamingSites = "(-1";
            DataTable dtOrg = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.Transaction = sqlTransaction;
            cmd.CommandText = @"SELECT SITE_ID
                                  FROM " + entityName + @"
                                 WHERE MASTERENTITYID = (SELECT MASTERENTITYID
                                                           FROM " + entityName + @"
                                                          WHERE GUID = @id
                                                        )
                                   AND SITE_ID != @site_id";
            cmd.Parameters.Add(new SqlParameter("@site_id", site_id));
            cmd.Parameters.Add(new SqlParameter("@id", entityRowGuid));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dtOrg);
            if (dtOrg == null)
            {
                roamingSites += ")";
                log.LogMethodExit(roamingSites);
                return roamingSites;
            }
            for (int i = 0; i < dtOrg.Rows.Count; i++)
            {
                roamingSites += ", " + dtOrg.Rows[i]["site_id"].ToString();
            }
            roamingSites += ")";
            log.LogMethodExit(roamingSites);
            return roamingSites;
        }
        public static string getRoamingSites(SqlConnection DBConnection, SqlTransaction SQLTrx, int SiteId, out int TopmostAutoRoamOrg)
        {
            log.LogMethodEntry(DBConnection, SQLTrx, SiteId);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = DBConnection;
            if (SQLTrx != null)
                cmd.Transaction = SQLTrx;

            string RoamingSites = "(-1";
            cmd.CommandText = @"WITH n(OrgId, ParentOrgId, OrgName, StructureId, level1) AS 
                                   (SELECT OrgId, ParentOrgId, OrgName, StructureId, 0
                                    FROM Organization
                                    WHERE OrgId = (select OrgId from Site where site_id = @site_id)
                                        UNION ALL
                                    SELECT nplus1.OrgId, nplus1.ParentOrgId, nplus1.OrgName, nplus1.StructureId, level1 + 1
                                    FROM Organization as nplus1, n
                                    WHERE nplus1.OrgId = n.ParentOrgId)
                                SELECT OrgId, OrgName, ors.StructureName, isnull(ors.AutoRoam, 'N') AutoRoam, level1
                                  FROM n, OrgStructure ors
                                 where n.StructureId = ors.StructureId
                                 order by ors.AutoRoam desc, level1 desc";

            cmd.Parameters.AddWithValue("@site_id", SiteId);
            log.LogVariableState("@site_id", SiteId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtOrg = new DataTable();
            da.Fill(dtOrg);
            object orgId;

            if (dtOrg.Rows.Count > 0 && dtOrg.Rows[0]["AutoRoam"].ToString().Equals("Y"))
            {
                orgId = dtOrg.Rows[0][0]; // highest level auto roam org
                cmd.CommandText = @"WITH n(OrgId, OrgName) AS 
                                       (SELECT OrgId, OrgName
                                        FROM Organization
                                        WHERE OrgId = @OrgId
                                        UNION ALL
                                        SELECT nplus1.OrgId, nplus1.OrgName
                                        FROM Organization as nplus1, n
                                        WHERE n.OrgId = nplus1.ParentOrgId)
                                    SELECT s.site_name, s.site_id, s.last_upload_time
                                      FROM n, site s
                                     where s.OrgId = n.orgId";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@OrgId", orgId);
                log.LogVariableState("@OrgId", orgId);
                SqlDataAdapter daRoaming = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                daRoaming.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    RoamingSites += ", " + dt.Rows[i]["site_id"].ToString();
                }
            }
            else
                orgId = -1;

            RoamingSites += ")";

            TopmostAutoRoamOrg = Convert.ToInt32(orgId);
            log.LogMethodExit(RoamingSites);
            return RoamingSites;
        }

        //07-Nov-2016 :: Allow customer to roam across all sites
        public static string getAllSites(SqlConnection DBConnection, SqlTransaction SQLTrx, int siteId)
        {
            log.LogMethodEntry(DBConnection, SQLTrx, siteId);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = DBConnection;
            if (SQLTrx != null)
                cmd.Transaction = SQLTrx;

            string AllSites = "(-1";
            cmd.CommandText = @"SELECT SITE_ID
                                  FROM SITE
                                 WHERE SITE_ID != (SELECT MASTER_SITE_ID FROM COMPANY)
                                   AND (ACTIVE_FLAG = 'Y' OR ACTIVE_FLAG IS NULL)
                                   AND SITE_ID != @site_id";

            cmd.Parameters.AddWithValue("@site_id", siteId);
            log.LogVariableState("@site_id", siteId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtOrg = new DataTable();
            da.Fill(dtOrg);
 
            for (int i = 0; i < dtOrg.Rows.Count; i++)
            {
                AllSites += ", " + dtOrg.Rows[i]["site_id"].ToString();
            }

            AllSites += ")";
            log.LogMethodExit(AllSites);

            return AllSites;
        }
        //07-Nov-2016 :: Allow customer to roam across all sites
    }
}
