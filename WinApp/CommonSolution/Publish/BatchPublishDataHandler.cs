/********************************************************************************************
 * Project Name - Batch Publish DataAccess Handler
 * Description  - Setting up SQL Connection and managing batch SQL operations
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************  
 *2.130.0     31-Aug-2021   Guru S A        Enable Serial number based card load
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Publish
{
    internal class BatchPublishDataHandler
    {
        private readonly DataAccessHandler dataAccessHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SqlTransaction sqlTransaction;

        public BatchPublishDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            dataAccessHandler.CommandTimeOut = 0;
            log.LogMethodExit();
        }

        public List<Column> GetColumns(string tableName)
        {
            log.LogMethodEntry(tableName);
            List<Column> result = new List<Column>();
            string query = @"select c.name ColumnName, tParent.name ReferencedTable, 
                                    cParent.name ReferencedTablePrimaryKey, c.is_identity IsPrimaryKey,
                                    ty.name TypeName
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
                             inner join	sys.types ty 
							 on  ty.user_type_id = c.user_type_id
                             where c.object_id = t.object_id
                             and t.name = @TableName";
            result = dataAccessHandler.GetDataFromReader(query, new SqlParameter[] { dataAccessHandler.GetSQLParameter("@TableName", tableName) }, sqlTransaction, CreateColumnList);
            log.LogMethodExit(result);
            return result;
        }

        public void ExecuteQuery(string query, List<int> pkIds, List<int> siteIds)
        {
            log.LogMethodEntry(query, pkIds, siteIds);
            SqlParameter primaryKeyIdListParameter = dataAccessHandler.GetListSqlParameter("@pkIdList", pkIds);
            SqlParameter siteIdListParameter = dataAccessHandler.GetListSqlParameter("@siteIdList", siteIds);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { primaryKeyIdListParameter, siteIdListParameter }, sqlTransaction);
            log.LogMethodExit();
        }

        private List<Column> CreateColumnList(SqlDataReader reader)
        {
            log.LogMethodEntry(reader);
            List<Column> columns = new List<Column>();
            int columnName = reader.GetOrdinal("ColumnName");
            int referencedTable = reader.GetOrdinal("ReferencedTable");
            int referencedTablePrimaryKey = reader.GetOrdinal("ReferencedTablePrimaryKey");
            int isPrimaryKey = reader.GetOrdinal("IsPrimaryKey");
            int typeName = reader.GetOrdinal("TypeName");
            while (reader.Read())
            {
                Column column;
                string columnNameValue = reader.GetString(columnName);
                string typeNameValue = reader.GetString(typeName);
                if (reader.IsDBNull(referencedTable) == false)
                {
                    string tableName = reader.GetString(referencedTable);
                    if (tableName.ToLower() == "membership")
                    {
                        column = new Column(columnNameValue);
                    }
                    else
                    {
                        column = new ForeignKeyColumn(columnNameValue, reader.GetString(referencedTable), reader.GetString(referencedTablePrimaryKey));
                    }
                }
                else if (reader.GetBoolean(isPrimaryKey))
                {
                    column = new PrimaryKeyColumn(columnNameValue);
                }
                else if(columnNameValue.ToLower() == "site_id")
                {
                    column = new SiteIdColumn(columnNameValue);
                }
                else if (columnNameValue.ToLower() == "masterentityid")
                {
                    column = new MasterEntityIdColumn(columnNameValue);
                }
                else if (IsIgnoreColumn(columnNameValue))
                {
                    column = new IgnoreColumn(columnNameValue);
                }
                else if (typeNameValue.ToLower() == "datetime")
                {
                    column = new DateTimeColumn(columnNameValue);
                }
                else
                {
                    column = new Column(columnNameValue);
                }
                columns.Add(column);
            }
            log.LogMethodExit(columns);
            return columns;
        }

        private bool IsIgnoreColumn(string columnName)
        {
            log.LogMethodEntry(columnName);
            bool result = false;
            if(columnName.ToLower() == "guid" || columnName.ToLower() == "synchstatus")
            {
                result = true;
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<int> CreateIntergerList(SqlDataReader reader)
        {
            log.LogMethodEntry(reader);
            List<int> integerList = new List<int>();
            try
            {
                int id = reader.GetOrdinal("Id");
                while (reader.Read())
                {
                    if (reader.IsDBNull(id) == false)
                    {
                        int value;
                        try
                        {
                            value = reader.GetInt32(id);
                        }
                        catch (Exception)
                        {
                            value = reader.GetInt16(id);
                        }
                        integerList.Add(value);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(integerList);
            return integerList;
        }

        public List<int> GetList(string query, List<int> pkIds)
        {
            log.LogMethodEntry(query, pkIds);
            SqlParameter parameter = dataAccessHandler.GetListSqlParameter("@pkIdList", pkIds);
            List<int> result = dataAccessHandler.GetDataFromReader(query, new SqlParameter[] { parameter } ,sqlTransaction, CreateIntergerList);
            if(result == null)
            {
                result = new List<int>();
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
