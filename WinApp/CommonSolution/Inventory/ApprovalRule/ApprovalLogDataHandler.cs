/********************************************************************************************
 * Project Name - Approval Log Data Handler
 * Description  - Data handler of the approval log class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        18-OCT-2017   Raghuveera          Created 
 *2.70.2        13-Aug-2019   Deeksha             modifications as per 3 tier standards
 *2.70.2        09-Dec-2019   Jinto Thomas        Removed siteid from update query 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Approval Log Data Handler - Handles insert, update and select of approval log objects
    /// </summary>
    public class ApprovalLogDataHandler
    {
        private const string SELECT_QUERY = @"SELECT * FROM ApprovalLog AS al ";
        private readonly SqlTransaction sqlTransaction;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<ApprovalLogDTO.SearchByApprovalLogParameters, string> DBSearchParameters = new Dictionary<ApprovalLogDTO.SearchByApprovalLogParameters, string>
            {
                {ApprovalLogDTO.SearchByApprovalLogParameters.APPROVAL_LOG_ID, "al.ApprovalLogID"},
                {ApprovalLogDTO.SearchByApprovalLogParameters.DOCUMENT_TYPE_ID, "al.DocumentTypeID"},
                {ApprovalLogDTO.SearchByApprovalLogParameters.STATUS, "al.status"},
                {ApprovalLogDTO.SearchByApprovalLogParameters.APPROVAL_LEVELS, "al.ApprovalLevel"},
                {ApprovalLogDTO.SearchByApprovalLogParameters.ACTIVE_FLAG, "al.IsActive"},
                {ApprovalLogDTO.SearchByApprovalLogParameters.MASTER_ENTITY_ID,"al.MasterEntityId"},
                {ApprovalLogDTO.SearchByApprovalLogParameters.SITE_ID, "al.site_id"}
            };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of ApprovalLogDataHandler class
        /// </summary>
        public ApprovalLogDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating ApprovalLog Record.
        /// </summary>
        /// <param name="ApprovalLogDTO">ApprovalLogDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(ApprovalLogDTO approvalLogDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(approvalLogDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvalLogID", approvalLogDTO.ApprovalLogID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@documentTypeID", approvalLogDTO.DocumentTypeID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvalLevel", approvalLogDTO.ApprovalLevel, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@objectGUID", string.IsNullOrEmpty(approvalLogDTO.ObjectGUID) ? DBNull.Value : (object)approvalLogDTO.ObjectGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(approvalLogDTO.Status) ? DBNull.Value : (object)approvalLogDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", string.IsNullOrEmpty(approvalLogDTO.Remarks) ? DBNull.Value : (object)approvalLogDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", approvalLogDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", approvalLogDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteid", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the approval log record to the database
        /// </summary>
        /// <param name="approvalLog">ApprovalLogDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public ApprovalLogDTO InsertApprovalLog(ApprovalLogDTO approvalLog, string loginId, int siteId)
        {
            log.LogMethodEntry(approvalLog, loginId, siteId);
            string insertApprovalLogQuery = @"insert into ApprovalLog 
                                                        (
                                                        DocumentTypeID,
                                                        ObjectGUID,
                                                        ApprovalLevel,
                                                        Status,
                                                        Remarks,
                                                        MasterEntityId,
                                                        IsActive,
                                                        CreatedBy,
                                                        CreationDate,
                                                        LastUpdatedBy,
                                                        LastUpdatedDate,
                                                        Guid,
                                                        site_id
                                                        ) 
                                                values 
                                                        (
                                                         @documentTypeID,
                                                         @objectGUID,
                                                         @approvalLevel,
                                                         @status,
                                                         @remarks,
                                                         @masterEntityId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @lastUpdatedBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid
                                                        )SELECT * FROM ApprovalLog WHERE ApprovalLogID = scope_identity()";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertApprovalLogQuery, GetSQLParameters(approvalLog, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApprovalLogDTO(approvalLog, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting approvalLog", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(approvalLog);
            return approvalLog;
        }


        /// <summary>
        /// Updates the approval log record
        /// </summary>
        /// <param name="approvalLog">ApprovalLogDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public ApprovalLogDTO UpdateApprovalLog(ApprovalLogDTO approvalLog, string loginId, int siteId)
        {
            log.LogMethodEntry(approvalLog, loginId, siteId);
            string updateApprovalLogQuery = @"update ApprovalLog 
                                         set DocumentTypeID = @documentTypeID,
                                             ObjectGUID = @objectGUID,
                                             ApprovalLevel = @approvalLevel,
                                             Status = @status,
                                             Remarks = @remarks,
                                             MasterEntityId=@masterEntityId,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate()
                                             --site_id=@siteid                                          
                                       where ApprovalLogID = @approvalLogID
                             SELECT * FROM ApprovalLog WHERE ApprovalLogID = @approvalLogID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateApprovalLogQuery, GetSQLParameters(approvalLog, loginId, siteId).ToArray(), sqlTransaction);
                RefreshApprovalLogDTO(approvalLog, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating approvalLog", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(approvalLog);
            return approvalLog;
        }

        /// <summary>
        /// Delete the record from the approvalLog database based on approvalLogID
        /// </summary>
        /// <param name="approvalLogID">approvalLogID</param>
        /// <returns>return the int </returns>
        internal int Delete(int approvalLogID)
        {
            log.LogMethodEntry(approvalLogID);
            string query = @"DELETE  
                             FROM ApprovalLog
                             WHERE ApprovalLog.ApprovalLogID = @approvalLogID";
            SqlParameter parameter = new SqlParameter("@approvalLogID", approvalLogID);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="approvalLog">approvalLog object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshApprovalLogDTO(ApprovalLogDTO approvalLog, DataTable dt)
        {
            log.LogMethodEntry(approvalLog, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                approvalLog.ApprovalLogID = Convert.ToInt32(dt.Rows[0]["ApprovalLogID"]);
                approvalLog.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                approvalLog.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                approvalLog.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                approvalLog.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                approvalLog.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                approvalLog.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to ApprovalLogDTO class type
        /// </summary>
        /// <param name="approvalLogDataRow">ApprovalLog DataRow</param>
        /// <returns>Returns ApprovalLog</returns>
        private ApprovalLogDTO GetApprovalLogDTO(DataRow approvalLogDataRow)
        {
            log.LogMethodEntry(approvalLogDataRow);
            ApprovalLogDTO approvalLogDataObject = new ApprovalLogDTO(Convert.ToInt32(approvalLogDataRow["ApprovalLogID"]),
                            approvalLogDataRow["DocumentTypeID"] == DBNull.Value ? -1 : Convert.ToInt32(approvalLogDataRow["DocumentTypeID"]),
                            approvalLogDataRow["ObjectGUID"] == DBNull.Value ? string.Empty : Convert.ToString(approvalLogDataRow["ObjectGUID"]),
                            approvalLogDataRow["ApprovalLevel"] == DBNull.Value ? 0 : Convert.ToInt32(approvalLogDataRow["ApprovalLevel"]),
                            approvalLogDataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(approvalLogDataRow["Status"]),
                            approvalLogDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(approvalLogDataRow["Remarks"]),
                            approvalLogDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(approvalLogDataRow["MasterEntityId"]),
                            approvalLogDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(approvalLogDataRow["IsActive"]),
                            approvalLogDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(approvalLogDataRow["CreatedBy"]),
                            approvalLogDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(approvalLogDataRow["CreationDate"]),
                            approvalLogDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(approvalLogDataRow["LastupdatedDate"]),
                            approvalLogDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(approvalLogDataRow["LastupdatedDate"]),
                            approvalLogDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(approvalLogDataRow["Guid"]),
                            approvalLogDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(approvalLogDataRow["site_id"]),
                            approvalLogDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(approvalLogDataRow["SynchStatus"])
                            );
            log.LogMethodExit(approvalLogDataObject);
            return approvalLogDataObject;
        }

        /// <summary>
        /// Gets the approval log data of passed approval log Id
        /// </summary>
        /// <param name="approvalLogId">integer type parameter</param>
        /// <returns>Returns ApprovalLogDTO</returns>
        public ApprovalLogDTO GetApprovalLog(int approvalLogId)
        {
            log.LogMethodEntry(approvalLogId);
            ApprovalLogDTO result = null;
            string selectApprovalLogQuery = SELECT_QUERY + @" WHERE al.ApprovalLogID= @approvalLogID";
            SqlParameter parameter = new SqlParameter("@approvalLogID", approvalLogId);
            DataTable approvalLog = dataAccessHandler.executeSelectQuery(selectApprovalLogQuery, new SqlParameter[] { parameter }, sqlTransaction);
            if (approvalLog.Rows.Count > 0)
            {

                result = GetApprovalLogDTO(approvalLog.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Gets the ApprovalLogDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of ApprovalLogDTO matching the search criteria</returns>
        public List<ApprovalLogDTO> GetApprovalLogList(List<KeyValuePair<ApprovalLogDTO.SearchByApprovalLogParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<ApprovalLogDTO> approvalLogList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectApprovalLogQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<ApprovalLogDTO.SearchByApprovalLogParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == ApprovalLogDTO.SearchByApprovalLogParameters.APPROVAL_LOG_ID
                            || searchParameter.Key == ApprovalLogDTO.SearchByApprovalLogParameters.APPROVAL_LEVELS
                            || searchParameter.Key == ApprovalLogDTO.SearchByApprovalLogParameters.DOCUMENT_TYPE_ID
                            || searchParameter.Key == ApprovalLogDTO.SearchByApprovalLogParameters.SITE_ID
                            || searchParameter.Key == ApprovalLogDTO.SearchByApprovalLogParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ApprovalLogDTO.SearchByApprovalLogParameters.SITE_ID)
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
                    count++;
                }
                if (searchParameters.Count > 0)
                    selectApprovalLogQuery = selectApprovalLogQuery + query;
            }
            DataTable approvalLogData = dataAccessHandler.executeSelectQuery(selectApprovalLogQuery, parameters.ToArray(), sqlTransaction);
            if (approvalLogData.Rows.Count > 0)
            {
                approvalLogList = new List<ApprovalLogDTO>();
                foreach (DataRow approvalLogDataRow in approvalLogData.Rows)
                {
                    ApprovalLogDTO approvalLogDataObject = GetApprovalLogDTO(approvalLogDataRow);
                    approvalLogList.Add(approvalLogDataObject);
                }
                
            }
            log.LogMethodExit(approvalLogList);
            return approvalLogList;

        }
    }
}
