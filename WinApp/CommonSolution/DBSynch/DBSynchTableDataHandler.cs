/********************************************************************************************
 * Project Name - DBSynchTableDataHandler
 * Description  - Get and Insert or update methods for db synch data.
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        24-Mar-2019   Jagan Mohana         Created 
 *2.60        09-Apr-2019   Mushahid Faizan      Modified -  GetSQLParameters() & GetDbSynchDTO(), Update method.
 *2.70.2        21-Oct-2019   Rakesh               Implemented the CreateDBSynchTables() & GetAllTables() & CreateMasterDataFromMasterSite() methods
 *2.110.0     28-Dec-2020   Mathew Ninan    Fixed logic of pulling dependencies in GETALLTables 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Publish;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.DBSynch
{
    public class DBSynchTableDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<DBSynchTableDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<DBSynchTableDTO.SearchByParameters, string>
        {
            { DBSynchTableDTO.SearchByParameters.DBSYNCH_ID,"Id"},
            { DBSynchTableDTO.SearchByParameters.TABLE_NAME, "TableName"},
            { DBSynchTableDTO.SearchByParameters.SITE_ID, "site_id"}
        };

        /// <summary>
        /// Default constructor of DBSynchDataHandler class
        /// </summary>
        public DBSynchTableDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            this.sqlTransaction = sqlTransaction;
            this.dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit(null);
        }

        /// <summary>
        ///  Converts the Data row object to DBSynchDTO class type
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        /// Modified SiteId to site_id column on 09-Apr 2019 by Mushahid Faizan
        private DBSynchTableDTO GetDbSynchDTO(DataRow dataRow)
        {
            log.LogMethodEntry();
            DBSynchTableDTO dBSynchDTO = new DBSynchTableDTO(Convert.ToInt32(dataRow["Id"]),
                                            dataRow["TableName"] == DBNull.Value ? string.Empty : dataRow["TableName"].ToString(),
                                            dataRow["UploadOnly"] == DBNull.Value ? string.Empty : dataRow["UploadOnly"].ToString(),
                                            dataRow["Synchronize"] == DBNull.Value ? string.Empty : dataRow["Synchronize"].ToString(),
                                            dataRow["InsertOnly"] == DBNull.Value ? string.Empty : dataRow["InsertOnly"].ToString(),
                                            dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]),
                                            dataRow["Guid"] == DBNull.Value ? string.Empty : dataRow["Guid"].ToString(),
                                            dataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(dataRow["SynchStatus"]),
                                            dataRow["InitialLoadDone"] == DBNull.Value ? string.Empty : dataRow["InitialLoadDone"].ToString(),
                                            dataRow["IgnoreColumnsOnRoaming"] == DBNull.Value ? string.Empty : dataRow["IgnoreColumnsOnRoaming"].ToString(),
                                            dataRow["IgnoreOnError"] == DBNull.Value ? string.Empty : dataRow["IgnoreOnError"].ToString(),
                                            dataRow["SynchDeletes"] == DBNull.Value ? string.Empty : dataRow["SynchDeletes"].ToString(),
                                            dataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["MasterEntityId"]),
                                            dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]),
                                            dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]),
                                            dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]),
                                            dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]),
                                            dataRow["DBSynchLogName"] == DBNull.Value ? string.Empty : (dataRow["DBSynchLogName"]).ToString(),
                                            dataRow["BatchNumber"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["BatchNumber"]),
                                            dataRow["UploadFrequency"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["UploadFrequency"]),
                                            dataRow["BatchStartTime"] == DBNull.Value ? 0.0M : Convert.ToDecimal(dataRow["BatchStartTime"].ToString()),
                                            dataRow["BatchEndTime"] == DBNull.Value ? 0.0M : Convert.ToDecimal(dataRow["BatchEndTime"].ToString()),
                                            dataRow["UploadBatchMaxHours"] == DBNull.Value ? 0.0M : Convert.ToDecimal(dataRow["UploadBatchMaxHours"].ToString())
                                            );
            log.LogMethodExit(dBSynchDTO);
            return dBSynchDTO;
        }

        /// <summary>
        /// Gets the DBSynchDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of DBSynchDTO matching the search criteria</returns>
        public List<DBSynchTableDTO> GetAllDBSynchList(List<KeyValuePair<DBSynchTableDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            string selectQuery = @"select * from DBSynchTables";
            if (searchParameters != null && (searchParameters.Count > 0))
            {
                string joiner = "";
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<DBSynchTableDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        joiner = (count == 0) ? " " : " and ";
                        {
                            if (searchParameter.Key.Equals(DBSynchTableDTO.SearchByParameters.DBSYNCH_ID))
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = " + searchParameter.Value);
                            }
                            else if (searchParameter.Key.Equals(DBSynchTableDTO.SearchByParameters.SITE_ID))
                            {
                                query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + searchParameter.Value + " or " + searchParameter.Value + "=-1)");
                            }
                            else
                            {
                                query.Append(joiner + DBSearchParameters[searchParameter.Key] + " = '" + searchParameter.Value + "'");
                            }
                        }
                        count++;
                    }
                    else
                    {
                        log.Error("Ends-GetAllDbSynch(searchParameters) Method by throwing manual exception\"The query parameter does not exist \"" + searchParameter.Key + "\".");
                        throw new Exception();
                    }
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable companyData = dataAccessHandler.executeSelectQuery(selectQuery, null, sqlTransaction);
            if (companyData.Rows.Count > 0)
            {
                List<DBSynchTableDTO> dbSynchDTOList = new List<DBSynchTableDTO>();
                foreach (DataRow securityPolicyDataRow in companyData.Rows)
                {
                    DBSynchTableDTO dbSynchDataObject = GetDbSynchDTO(securityPolicyDataRow);
                    dbSynchDTOList.Add(dbSynchDataObject);
                }
                log.LogMethodExit(dbSynchDTOList);
                return dbSynchDTOList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }


        /// <summary>
        /// Delete the record from the database based on Id
        /// </summary>
        /// <returns>return the int </returns>
        public int DeleteDbSynch(int dbSynchId)
        {
            log.LogMethodEntry(dbSynchId);
            try
            {
                string deleteQuery = @"delete  
                                          from DBSynchTables
                                          where Id = @dbSynchId";

                SqlParameter[] deleteParameters = new SqlParameter[1];
                deleteParameters[0] = new SqlParameter("@dbSynchId", dbSynchId);

                int deleteStatus = dataAccessHandler.executeUpdateQuery(deleteQuery, deleteParameters);
                log.LogMethodExit(deleteStatus);
                return deleteStatus;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
        }
        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating db synch Record.
        /// </summary>
        /// <param name="dBSynchDTO">dBSynchDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        /// Modified to access dataAccessHandler.GetSQLParameter() method by Mushahid Faizan on 09-Apr-2019
        private List<SqlParameter> GetSQLParameters(DBSynchTableDTO dBSynchDTO, string userId, int siteId)
        {
            log.LogMethodEntry(dBSynchDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@Id", dBSynchDTO.DbSynchId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@TableName", dBSynchDTO.TableName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UploadOnly", dBSynchDTO.UploadOnly));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Synchronize", dBSynchDTO.Synchronize));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InsertOnly", dBSynchDTO.InsertOnly));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@InitialLoadDone", dBSynchDTO.InitialLoadDone));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IgnoreColumnsOnRoaming", dBSynchDTO.IgnoreColumnsOnRoaming));
            parameters.Add(dataAccessHandler.GetSQLParameter("@IgnoreOnError", dBSynchDTO.IgnoreOnError));
            parameters.Add(dataAccessHandler.GetSQLParameter("@SynchDeletes", dBSynchDTO.SynchDeletes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", dBSynchDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@DbSynchLogName", dBSynchDTO.DbSynchLogName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BatchNumber", dBSynchDTO.BatchNumber, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UploadFrequency", dBSynchDTO.UploadFrequency, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BatchStartTime", dBSynchDTO.BatchStartTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@BatchEndTime", dBSynchDTO.BatchEndTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@UploadBatchMaxHours", dBSynchDTO.UploadBatchMaxHours));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the db synch record to the database
        /// </summary>
        /// <param name="dBSynchDTO">SecurityPolicyDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public int InsertDbSynch(DBSynchTableDTO dBSynchDTO, string userId, int siteId)
        {
            log.LogMethodEntry(dBSynchDTO, userId, siteId);
            int idOfRowInserted;
            string query = @"insert into DBSynchTables
                                                       (
                                                        TableName,
                                                        UploadOnly,
                                                        Synchronize,
                                                        InsertOnly,
                                                        site_id,
                                                        Guid,
                                                        SynchStatus,
                                                        InitialLoadDone,
                                                        IgnoreColumnsOnRoaming,
                                                        IgnoreOnError,
                                                        SynchDeletes,
                                                        MasterEntityId,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdateDate,
                                                        DBSynchLogName,
                                                        BatchNumber,
                                                        UploadFrequency,
                                                        BatchStartTime,
                                                        BatchEndTime,
                                                        UploadBatchMaxHours
                                                        ) 
                                                values 
                                                        (                                                        
                                                        @tableName,
                                                        @uploadOnly,
                                                        @synchronize,
                                                        @insertOnly,
                                                        @siteId,
                                                        NewId(),
                                                        NULL,
                                                        @initialLoadDone,
                                                        @ignoreColumnsOnRoaming,
                                                        @ignoreOnError,
                                                        @synchDeletes,
                                                        @masterEntityId,
                                                        @createdBy,
                                                        GETDATE(),                                                        
                                                        @lastUpdatedBy,                                                        
                                                        GetDate(),
                                                        @DbSynchLogName,
                                                        @BatchNumber, 
                                                        @UploadFrequency,
                                                        @BatchStartTime,
                                                        @BatchEndTime,
                                                        @UploadBatchMaxHours
                                            )SELECT CAST(scope_identity() AS int)";
            try
            {
                idOfRowInserted = dataAccessHandler.executeInsertQuery(query, GetSQLParameters(dBSynchDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(idOfRowInserted);
            return idOfRowInserted;
        }

        /// <summary>
        /// Updates the db synch record
        /// </summary>
        /// <param name="dBSynchDTO">dBSynchDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        /// Tablename mismatch: modified by Mushahid Faizan on 09-Apr-2019
        public int UpdateDbSynch(DBSynchTableDTO dBSynchDTO, string userId, int siteId)
        {
            log.LogMethodEntry(dBSynchDTO, userId, siteId);
            int rowsUpdated;
            string query = @"update  DBSynchTables
                                         set TableName=@tableName,
                                             UploadOnly = @uploadOnly,
                                             Synchronize = @synchronize,
                                             InsertOnly = @insertOnly,
                                             site_id = @siteId,
                                             InitialLoadDone = @initialLoadDone,
                                             IgnoreColumnsOnRoaming = @ignoreColumnsOnRoaming,
                                             IgnoreOnError = @ignoreOnError,
                                             SynchDeletes = @synchDeletes,                                             
                                             MasterEntityId = @masterEntityId,
                                             LastUpdatedBy = @lastUpdatedBy,
                                             LastUpdateDate = GETDATE(),
                                             DBSynchLogName = @DbSynchLogName,
                                             BatchNumber = @BatchNumber,
                                             UploadFrequency = @UploadFrequency,
                                             BatchStartTime = @BatchStartTime,
                                             BatchEndTime = @BatchEndTime,
                                             UploadBatchMaxHours = @UploadBatchMaxHours
                                       where Id = @Id";

            try
            {
                rowsUpdated = dataAccessHandler.executeUpdateQuery(query, GetSQLParameters(dBSynchDTO, userId, siteId).ToArray(), sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }
        /// <summary>
        ///  Deletes the DBSynchTables record based on Id 
        /// </summary>
        /// <param name="id">id is passed as parameter</param>
        internal void Delete(int id)
        {
            log.LogMethodEntry(id);
            string query = @"DELETE  
                             FROM DBSynchTables
                             WHERE DBSynchTables.Id = @Id";
            SqlParameter parameter = new SqlParameter("@Id", id);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }
        private void CreateDBSynchTables(int siteId)
        {
            log.LogMethodEntry(siteId);
            string query = @"insert into DBSynchTables (TableName, UploadOnly, Synchronize, InsertOnly, site_id)
                                        select name, 'N', 'Y', 'N', @site_id
                                        from sys.tables
                                        where not exists (select 1 
                                                            from DBSynchTables dt 
                                                            where TableName = name 
                                                            and site_id = @site_id)
                                        order by name;
                                        update DBSynchTables set Synchronize = 'N'
                                        where TableName in ('sysdiagrams', 'mCashXref', 
					                                        'DirectSQL', 'DBSynchLog', 'CommunicationLog',
					                                        'Organization', 'OrgStructure', 'Company', 'Site', 'RoamingSites',
					                                        'ExSysSynchLog')
                                        and Synchronize ='Y'";
            SqlParameter[] dbSynchTableParameters = new SqlParameter[1];
            dbSynchTableParameters[0] = new SqlParameter("@site_id", siteId);
            try
            {
                dataAccessHandler.executeUpdateQuery(query, dbSynchTableParameters, sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit();
        }
        public DataTable GetAllTables(int siteId)
        {
            log.LogMethodEntry(siteId);
            DataTable allTable = new DataTable();
            CreateDBSynchTables(siteId);
            string query = @"WITH n(referenced_object_id, parent_object_id, sort) AS 
                                       (SELECT referenced_object_id, parent_object_id, 0 sort
                                        FROM sys.foreign_keys where referenced_object_id != parent_object_id
                                            UNION ALL
                                        SELECT nplus1.referenced_object_id, nplus1.parent_object_id, sort + 1
                                        FROM sys.foreign_keys as nplus1, n
                                        WHERE n.referenced_object_id = nplus1.parent_object_id
                                        and nplus1.referenced_object_id != nplus1.parent_object_id)
                                    SELECT t.name, max(sort) sort 
                                    FROM n, sys.tables t 
                                        inner join DBSynchTables tbls 
		                                on tbls.TableName = t.name
		                                and tbls.site_id = @site_id
                                    where t.object_id = n.referenced_object_id
                                    and Synchronize = 'Y'
                                    group by t.name
                                    union
                                    select t.name, -1 
                                    from sys.tables t 
                                        inner join DBSynchTables tbls 
		                                on tbls.TableName = t.name
		                                and tbls.site_id = @site_id
                                    where not exists (select 1 
					                                    from sys.foreign_keys k, n
					                                    where k.referenced_object_id = t.object_id
														  and n.referenced_object_id = k.referenced_object_id
                                                     )
                                    and Synchronize = 'Y'
                                    order by sort desc";
            SqlParameter[] dbSynchTableParameters = new SqlParameter[1];
            dbSynchTableParameters[0] = new SqlParameter("@site_id", siteId);
            try
            {
                allTable = dataAccessHandler.executeSelectQuery(query, dbSynchTableParameters, sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "throwing exception");
                throw ex;
            }
            log.LogMethodExit(allTable);
            return allTable;
        }
        public void CreateMasterDataFromMasterSite(int siteId, int masterSiteId, string userId)
        {
            log.LogMethodEntry(siteId, masterSiteId);
            PublishDataHandler publishDataHandler = new PublishDataHandler();
            DataTable dtTables = GetAllTables(siteId);
            List<DBSynchTrxTable> trxTables = new DBSynchTrxTable().GetDBSynchTrxTables();
            foreach (DataRow drTable in dtTables.Rows)
            {
                string tableName = drTable["Name"].ToString();
                if (trxTables.Find(x => x.TableName.ToLower().Equals(tableName.ToLower())) != null)
                {
                    continue;
                }
                object pkCol = publishDataHandler.getPKColName(tableName);
                if (pkCol == null)
                {
                    continue;
                }
                // update site id to master site id where site id is null
                try
                {
                    SqlParameter[] tableParameters = new SqlParameter[1];
                    tableParameters[0] = new SqlParameter("@tableName", tableName);
                    DataTable dt = dataAccessHandler.executeSelectQuery("select Guid from "+ tableName + " where site_id is null", null , sqlTransaction);
                    foreach (DataRow dr in dt.Rows)
                    {
                        dataAccessHandler.executeUpdateQuery("update " + tableName + " set site_id = @masterSiteId where Guid = @guid",
                                                new SqlParameter[] { new SqlParameter("@masterSiteId", masterSiteId), new SqlParameter("@guid", dr[0]) }, sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw new Exception(ex.Message, ex);
                }
                DataTable dtTablePK = dataAccessHandler.executeSelectQuery("select " + pkCol.ToString() + " from " + tableName + " where site_id = @masterSiteId", new[] { new SqlParameter("@masterSiteId", masterSiteId) });
                PublishDataHandler.Entity entity = new PublishDataHandler.Entity(tableName);
                foreach (DataRow drRow in dtTablePK.Rows)
                {
                    publishDataHandler.Publish(drRow[0], siteId, entity, sqlTransaction, userId);
                }
            }
            log.LogMethodExit();
        }
    }
}