/********************************************************************************************
 * Project Name - Batch Job Request Data Handler
 * Description  - Data handler of the Batch Job Request class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        23-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
 *2.100.0     31-Aug-2020   Mushahid Faizan   siteId changes in GetSQLParameters().
 *2.140       14-Sep-2021      Fiona          Modified: Issue fixes
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// This is the BatchJobRequest datahandler object class. This acts as data handler for the BatchJobRequest business object
    /// </summary>
    public class BatchJobRequestDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT a.* FROM BatchJobRequest as a ";

        /// <summary>
        /// Dictionary for searching Parameters for the BatchJobRequest object.
        /// </summary>
        private static readonly Dictionary<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string> DBSearchParameters = new Dictionary<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>
               {
                    {BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBREQUEST_ID, "a.BatchJobRequestId"},
                    {BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBACTIVITY_ID, "a.BatchJobActivityID"},
                    {BatchJobRequestDTO.SearchByBatchJobRequestParameters.ENTITYGUID, "a.EntityGuid"},
                    {BatchJobRequestDTO.SearchByBatchJobRequestParameters.ENTITYCOLUMN_VALUE, "a.EntityColumnValue"},
                    {BatchJobRequestDTO.SearchByBatchJobRequestParameters.PROCESSE_FLAG, "a.ProcesseFlag"},
                    {BatchJobRequestDTO.SearchByBatchJobRequestParameters.SITE_ID,"a.site_id"},
                    {BatchJobRequestDTO.SearchByBatchJobRequestParameters.MASTER_ENTITY_ID,"a.MasterEntityId"}
               };
        //Utilities utilities;

        /// <summary>
        /// Default constructor of BatchJobRequestDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public BatchJobRequestDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            //utilities = new Utilities();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating BatchJobRequest parameters Record.
        /// </summary>
        /// <param name="batchJobRequestDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(BatchJobRequestDTO batchJobRequestDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobRequestDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@batchJobRequestId", batchJobRequestDTO.BatchJobRequestId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@batchJobActivityID", batchJobRequestDTO.BatchJobActivityID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@entityGuid", batchJobRequestDTO.EntityGuid));
            parameters.Add(dataAccessHandler.GetSQLParameter("@entityColumnValue", batchJobRequestDTO.EntityColumnValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@processeFlag", batchJobRequestDTO.ProcesseFlag));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", batchJobRequestDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastModUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            return parameters;
        }

        /// <summary>
        /// Inserts the BatchJobRequest record to the database
        /// </summary>
        /// <param name="batchJobRequestDTO">BatchJobRequestDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns>Returns inserted record id</returns>
        public BatchJobRequestDTO InsertBatchJobRequest(BatchJobRequestDTO batchJobRequestDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobRequestDTO, loginId, siteId, sqlTransaction);
            string InsertBatchJobRequestQuery = @"INSERT INTO [dbo].[BatchJobRequest]  
                                                        (
                                                          BatchJobActivityID,
                                                          EntityGuid,
                                                          EntityColumnValue,
                                                          ProcesseFlag,
                                                          Guid,
                                                          site_id,
                                                          MasterEntityId,
                                                          LastModUserId,
                                                          LastModDttm,
                                                          CreatedBy,
                                                          CreationDate
                                                        ) 
                                                values 
                                                        (
                                                          @batchJobActivityID,
                                                          NewId(),
                                                          @entityColumnValue,
                                                          @processeFlag,
                                                          NewId(),
                                                          @siteId,
                                                          @MasterEntityId,
                                                          @lastModUserId,
                                                          getdate(),
                                                          @createdBy,
                                                          Getdate()
                                                        )SELECT * FROM BatchJobRequest WHERE BatchJobRequestId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertBatchJobRequestQuery, GetSQLParameters(batchJobRequestDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBatchJobRequestDTO(batchJobRequestDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting BatchJobRequest", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(batchJobRequestDTO);
            return batchJobRequestDTO;
        }

        /// <summary>
        /// Updates the BatchJobRequest record
        /// </summary>
        /// <param name="batchJobRequestDTO">BatchJobRequestDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public BatchJobRequestDTO UpdateBatchJobRequest(BatchJobRequestDTO batchJobRequestDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobRequestDTO, loginId, siteId, sqlTransaction);
            string updateBatchJobRequestQuery = @"update BatchJobRequest 
                                                          set  BatchJobActivityID = @batchJobActivityID,
                                                          EntityColumnValue = @entityColumnValue,
                                                          ProcesseFlag = @processeFlag,
                                                          -- site_id=@siteId,                                                          
                                                          MasterEntityId=@MasterEntityId,
                                                          LastModUserId=@lastModUserId,
                                                          LastModDttm=getdate()
                                       where BatchJobRequestId = @batchJobRequestId
                                       SELECT* FROM BatchJobRequest WHERE  BatchJobRequestId = @batchJobRequestId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateBatchJobRequestQuery, GetSQLParameters(batchJobRequestDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBatchJobRequestDTO(batchJobRequestDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating BatchJobRequestDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(batchJobRequestDTO);
            return batchJobRequestDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="batchJobRequestDTO">batchJobRequestDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshBatchJobRequestDTO(BatchJobRequestDTO batchJobRequestDTO, DataTable dt)
        {
            log.LogMethodEntry(batchJobRequestDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                batchJobRequestDTO.BatchJobRequestId = Convert.ToInt32(dt.Rows[0]["BatchJobRequestId"]);
                batchJobRequestDTO.LastModDttm = dataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastModDttm"]);
                batchJobRequestDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                batchJobRequestDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                batchJobRequestDTO.EntityGuid = dataRow["EntityGuid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["EntityGuid"]);
                batchJobRequestDTO.LastModUserId = dataRow["LastModUserId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastModUserId"]);
                batchJobRequestDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                batchJobRequestDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Updates the UpdateEntityColumn for UpdateEntity
        /// </summary>
        /// <param name="batchJobRequestDTO">BatchJobRequestDTO type parameter</param>
        /// <param name="entityName">Name of the entity</param>
        /// <param name="entityColumn">Name of the entity Column</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public int UpdateEntityColumn(BatchJobRequestDTO batchJobRequestDTO, string jobActionName, string entityName, string entityColumn, string userId, int siteId)
        {
            log.LogMethodEntry(batchJobRequestDTO, userId, siteId, sqlTransaction);
            StringBuilder queryString = new StringBuilder("");
            switch (jobActionName)
            {
                case "UPDATE":
                    queryString.Append("Update " + entityName + " set " + entityColumn + " = @entityColumnValue where Guid = @entityGuid");
                    break;
                default: break;
            }

            List<SqlParameter> updateRequestParameters = new List<SqlParameter>();
            updateRequestParameters.Add(new SqlParameter("@entityGuid", batchJobRequestDTO.EntityGuid));
            updateRequestParameters.Add(new SqlParameter("@entityColumnValue", batchJobRequestDTO.EntityColumnValue));

            int rowsUpdated = dataAccessHandler.executeUpdateQuery(queryString.ToString(), updateRequestParameters.ToArray(), sqlTransaction);
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Converts the Data row object to batchJobRequestDTO class type
        /// </summary>
        /// <param name="batchJobRequestDataRow">batchJobRequestDTO DataRow</param>
        /// <returns>Returns batchJobRequestDTO</returns> 
        private BatchJobRequestDTO GetBatchJobRequestDTO(DataRow batchJobRequestDataRow)
        {
            log.LogMethodEntry(batchJobRequestDataRow);
            BatchJobRequestDTO BatchJobRequestDataObject = new BatchJobRequestDTO(Convert.ToInt32(batchJobRequestDataRow["BatchJobRequestId"]),
                                                          Convert.ToInt32(batchJobRequestDataRow["BatchJobActivityID"]),
                                                          batchJobRequestDataRow["EntityGuid"].ToString(),
                                                          batchJobRequestDataRow["EntityColumnValue"].ToString(),
                                                          Convert.ToBoolean(batchJobRequestDataRow["ProcesseFlag"]),
                                                          batchJobRequestDataRow["Guid"].ToString(),
                                                          batchJobRequestDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(batchJobRequestDataRow["site_id"]),
                                                          batchJobRequestDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(batchJobRequestDataRow["SynchStatus"]),
                                                          batchJobRequestDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(batchJobRequestDataRow["MasterEntityId"]),
                                                          batchJobRequestDataRow["LastModUserId"].ToString(),
                                                          batchJobRequestDataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(batchJobRequestDataRow["LastModDttm"]),
                                                          batchJobRequestDataRow["CreatedBy"].ToString(),
                                                          batchJobRequestDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(batchJobRequestDataRow["CreationDate"])
                                                         );
            log.LogMethodExit(BatchJobRequestDataObject);
            return BatchJobRequestDataObject;
        }

        /// <summary>
        /// Gets the BatchJobRequest data of passed patch asset application id
        /// </summary>
        /// <param name="batchJobRequestID">integer type parameter</param>
        /// <returns>Returns batchJobRequestDTO</returns>
        public BatchJobRequestDTO GetBatchJobRequest(int batchJobRequestID)
        {
            log.LogMethodEntry(batchJobRequestID);
            string selectBatchJobRequestQuery = SELECT_QUERY + " where a.BatchJobRequestId = @batchJobRequestId";
            BatchJobRequestDTO batchJobRequestDataObject = null;
            SqlParameter[] selectProductParameters = new SqlParameter[1];
            selectProductParameters[0] = new SqlParameter("@batchJobRequestId", batchJobRequestID);
            DataTable batchJobRequest = dataAccessHandler.executeSelectQuery(selectBatchJobRequestQuery, selectProductParameters, sqlTransaction);
            if (batchJobRequest.Rows.Count > 0)
            {
                DataRow batchJobRequesttRow = batchJobRequest.Rows[0];
                batchJobRequestDataObject = GetBatchJobRequestDTO(batchJobRequesttRow);
            }
            log.LogMethodExit(batchJobRequestDataObject);
            return batchJobRequestDataObject;
        }

        /// <summary>
        /// Gets the BatchJobRequestDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of batchJobRequestDTO matching the search criteria</returns>
        public List<BatchJobRequestDTO> GetAllBatchJobRequestList(List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<BatchJobRequestDTO> batchJobRequestDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBREQUEST_ID
                             || searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBACTIVITY_ID
                             || searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.MASTER_ENTITY_ID
                             )
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.PROCESSE_FLAG)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                batchJobRequestDTOList = new List<BatchJobRequestDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    BatchJobRequestDTO batchJobRequestDTO = GetBatchJobRequestDTO(dataRow);
                    batchJobRequestDTOList.Add(batchJobRequestDTO);
                }
            }
            log.LogMethodExit(batchJobRequestDTOList);
            return batchJobRequestDTOList;
        }

        /// <summary>
        /// Gets the Latest Pending BatchJobRequestDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of batchJobRequestDTO matching the search criteria</returns>
        public List<BatchJobRequestDTO> GetLatestPendingBatchJobRequestList(List<KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<BatchJobRequestDTO> batchJobRequestDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = @"select bjr.*
                                        from BatchJobRequest bjr
                                       where bjr.ProcesseFlag=0 
                                         and bjr.LastModDttm = (SELECT MAX(bjrin.LastModDttm) 
                                                                  FROM BatchJobRequest bjrin 
                                                                 WHERE bjrin.BatchJobActivityID = bjr.BatchJobActivityID 
                                                                   AND bjrin.EntityGuid = bjr.EntityGuid
                                                                   AND bjrin.ProcesseFlag = 0)";
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" and ");
                foreach (KeyValuePair<BatchJobRequestDTO.SearchByBatchJobRequestParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBREQUEST_ID
                             || searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.BATCHJOBACTIVITY_ID
                             || searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + "bjr." + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.PROCESSE_FLAG)
                        {
                            query.Append(joiner + "bjr." + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == BatchJobRequestDTO.SearchByBatchJobRequestParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + "bjr." + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + "bjr." + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                    counter++;
                }
                selectQuery = selectQuery + query;
            }
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                batchJobRequestDTOList = new List<BatchJobRequestDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    BatchJobRequestDTO batchJobRequestDTO = GetBatchJobRequestDTO(dataRow);
                    batchJobRequestDTOList.Add(batchJobRequestDTO);
                }
            }
            log.LogMethodExit(batchJobRequestDTOList);
            return batchJobRequestDTOList;
        }
    }
}
