/********************************************************************************************
 * Project Name - Batch Job Log Data Handler
 * Description  - Data handler of the  Batch Job Log class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        23-Jul-2019   Dakshakh raj        Modified : added GetSQLParameters(),
 *                                                         SQL injection Issue Fix
 *2.70.2        09-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
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
    /// This is the BatchJobLog datahandler object class. This acts as data handler for the BatchJobLog business object
    /// </summary>
    public class BatchJobLogDatahandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT a.* FROM BatchJobLog as a ";

        /// <summary>
        /// Dictionary for searching Parameters for the BatchJobLog object.
        /// </summary>
        private static readonly Dictionary<BatchJobLogDTO.SearchByBatchJobLogParameters, string> DBSearchParameters = new Dictionary<BatchJobLogDTO.SearchByBatchJobLogParameters, string>
               {
                    {BatchJobLogDTO.SearchByBatchJobLogParameters.BATCHJOBLOG_ID, "a.BatchJobLogId"},
                    {BatchJobLogDTO.SearchByBatchJobLogParameters.BATCHJOBREQUEST_ID, "a.BatchJobRequestId"},
                    {BatchJobLogDTO.SearchByBatchJobLogParameters.Log_Key, "a.LogKey"},
                    {BatchJobLogDTO.SearchByBatchJobLogParameters.Log_Value, "a.LogValue"},
                    {BatchJobLogDTO.SearchByBatchJobLogParameters.Log_Text, "a.LogText"},
                    {BatchJobLogDTO.SearchByBatchJobLogParameters.SITE_ID,"a.site_id"},
                    {BatchJobLogDTO.SearchByBatchJobLogParameters.MASTER_ENTITY_ID,"a.MasterEntityId"}
               };
        Utilities utilities;

        /// <summary>
        /// Default constructor of BatchJobLogDatahandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public BatchJobLogDatahandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            utilities = new Utilities();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating BatchJobLog parameters Record.
        /// </summary>
        /// <param name="batchJobLogDTO"></param>
        /// <param name="loginId"></param>
        /// <param name="siteId"></param>
        /// <returns>Returns the list of SQL parameter </returns>
        private List<SqlParameter> GetSQLParameters(BatchJobLogDTO batchJobLogDTO , string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@batchJobLogId", batchJobLogDTO.BatchJobLogId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@batchJobRequestId", batchJobLogDTO.BatchJobRequestId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@logKey", batchJobLogDTO.LogKey));
            parameters.Add(dataAccessHandler.GetSQLParameter("@logValue", string.IsNullOrEmpty(batchJobLogDTO.LogValue) ? DBNull.Value : (object)batchJobLogDTO.LogValue));
            parameters.Add(dataAccessHandler.GetSQLParameter("@logText", string.IsNullOrEmpty(batchJobLogDTO.LogText) ? DBNull.Value : (object)batchJobLogDTO.LogText));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", batchJobLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastModUserId", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            return parameters;
        }

        /// <summary>
        /// Inserts the BatchJobLog record to the database
        /// </summary>
        /// <param name="batchJobLogDTO">BatchJobLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns>Returns inserted record id</returns>
        public BatchJobLogDTO InsertBatchJobLog(BatchJobLogDTO batchJobLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobLogDTO, loginId, siteId);
            string InsertBatchJobLogQuery = @"INSERT INTO [dbo].[BatchJobLog]  
                                                        (
                                                          BatchJobRequestId,
	                                                      LogKey,
	                                                      LogValue,
	                                                      LogText,
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
                                                          @batchJobRequestID,
                                                          @logKey,
                                                          @logValue,
                                                          @logText,
                                                          NewId(),
                                                          @siteId,
                                                          @MasterEntityId,
                                                          @lastModUserId,
                                                          getdate(),
                                                          @createdBy,
                                                          Getdate()
                                                        )SELECT * FROM BatchJobLog WHERE BatchJobLogId = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(InsertBatchJobLogQuery, GetSQLParameters(batchJobLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBatchJobLogDTO(batchJobLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting BatchJobLogDTO", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(batchJobLogDTO);
            return batchJobLogDTO;
        }

        /// <summary>
        /// Updates the BatchJobLog record
        /// </summary>
        /// <param name="batchJobLogDTO">BatchJobLogDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <param name="SQLTrx">sql transaction</param>
        /// <returns>Returns the count of updated rows</returns>
        public BatchJobLogDTO UpdateBatchJobLog(BatchJobLogDTO batchJobLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(batchJobLogDTO, loginId, siteId);
            string updateBatchJobLogQuery = @"update BatchJobLog 
                                                          set  BatchJobRequestId = @batchJobRequestId,
                                                          LogKey =  @logKey,
                                                          LogValue = @logValue,
                                                          LogText = @logText,
                                                          -- site_id=@siteId,                                                          
                                                          MasterEntityId=@MasterEntityId,
                                                          LastModUserId=@lastModUserId,
                                                          LastModDttm=getdate()
                                       where BatchJobLogId = @batchJobLogId
                                       SELECT* FROM BatchJobLog WHERE BatchJobLogId = @batchJobLogId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateBatchJobLogQuery, GetSQLParameters(batchJobLogDTO, loginId, siteId).ToArray(), sqlTransaction);
                RefreshBatchJobLogDTO(batchJobLogDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while Updating BatchJobLog", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(batchJobLogDTO);
            return batchJobLogDTO;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="locationDTO">locationDTO</param>
        /// <param name="dt">dt</param>
        private void RefreshBatchJobLogDTO(BatchJobLogDTO batchJobLogDTO, DataTable dt)
        {
            log.LogMethodEntry(batchJobLogDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                batchJobLogDTO.BatchJobLogId = Convert.ToInt32(dt.Rows[0]["BatchJobLogId"]);
                batchJobLogDTO.LastModDttm = dataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastModDttm"]);
                batchJobLogDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                batchJobLogDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                batchJobLogDTO.LastModUserId = dataRow["LastModUserId"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastModUserId"]);
                batchJobLogDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                batchJobLogDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to batchJobLogDTO class type
        /// </summary>
        /// <param name="batchJobLogDataRow">batchJobLogDTO DataRow</param>
        /// <returns>Returns batchJobLogDTO</returns>
        private BatchJobLogDTO GetBatchJobLogDTO(DataRow batchJobLogDataRow)
        {
            log.LogMethodEntry(batchJobLogDataRow);
            BatchJobLogDTO BatchJobLogDataObject = new BatchJobLogDTO(Convert.ToInt32(batchJobLogDataRow["BatchJobLogId"]),
                                                          Convert.ToInt32(batchJobLogDataRow["BatchJobRequestId"]),
                                                          batchJobLogDataRow["LogKey"].ToString(),
                                                          batchJobLogDataRow["LogValue"].ToString(),
                                                          batchJobLogDataRow["LogText"].ToString(),
                                                          batchJobLogDataRow["Guid"].ToString(),
                                                          batchJobLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(batchJobLogDataRow["site_id"]),
                                                          batchJobLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(batchJobLogDataRow["SynchStatus"]),
                                                          batchJobLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(batchJobLogDataRow["MasterEntityId"]),
                                                          batchJobLogDataRow["LastModUserId"].ToString(),
                                                          batchJobLogDataRow["LastModDttm"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(batchJobLogDataRow["LastModDttm"]),
                                                          batchJobLogDataRow["CreatedBy"].ToString(),
                                                          batchJobLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(batchJobLogDataRow["CreationDate"])
                                                         );
            log.LogMethodExit(BatchJobLogDataObject);
            return BatchJobLogDataObject;
        }

        /// <summary>
        /// Gets the BatchJobLog data of passed patch asset application id
        /// </summary>
        /// <param name="batchJobLogID">integer type parameter</param>
        /// <returns>Returns batchJobLogDTO</returns>
        public BatchJobLogDTO GetBatchJobLog(int batchJobLogID)
        {
            log.LogMethodEntry(batchJobLogID);
            string selectBatchJobLogQuery = SELECT_QUERY + " where a.BatchJobLogId = @batchJobLogId";
            SqlParameter[] selectBatchJobLogParameters = new SqlParameter[1];
            selectBatchJobLogParameters[0] = new SqlParameter("@batchJobLogId", batchJobLogID);
            BatchJobLogDTO batchJobLogDataObject = null;
            DataTable batchJobLog = dataAccessHandler.executeSelectQuery(selectBatchJobLogQuery, selectBatchJobLogParameters, sqlTransaction);
            if (batchJobLog.Rows.Count > 0)
            {
                DataRow batchJobLogtRow = batchJobLog.Rows[0];
                batchJobLogDataObject = GetBatchJobLogDTO(batchJobLogtRow);
                
            }
            log.LogMethodExit(batchJobLogDataObject);
            return batchJobLogDataObject;
        }

        /// <summary>
        /// Gets the BatchJobLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of batchJobLogDTO matching the search criteria</returns>
        public List<BatchJobLogDTO> GetAllBatchJobLogList(List<KeyValuePair<BatchJobLogDTO.SearchByBatchJobLogParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<BatchJobLogDTO> batchJobLogDTOList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int counter = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<BatchJobLogDTO.SearchByBatchJobLogParameters, string> searchParameter in searchParameters)
                {
                    joiner = counter == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == BatchJobLogDTO.SearchByBatchJobLogParameters.BATCHJOBLOG_ID
                             || searchParameter.Key == BatchJobLogDTO.SearchByBatchJobLogParameters.BATCHJOBREQUEST_ID
                             || searchParameter.Key == BatchJobLogDTO.SearchByBatchJobLogParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        
                        else if (searchParameter.Key == BatchJobLogDTO.SearchByBatchJobLogParameters.SITE_ID)
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
                batchJobLogDTOList = new List<BatchJobLogDTO>();
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    BatchJobLogDTO batchJobLogDTO  = GetBatchJobLogDTO(dataRow);
                    batchJobLogDTOList.Add(batchJobLogDTO);
                }
            }
            log.LogMethodExit(batchJobLogDTOList);
            return batchJobLogDTOList;
        }
    }
}
