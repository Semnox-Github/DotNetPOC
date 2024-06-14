/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler of the NotificationTagProfile class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public class NotificationTagProfileDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<NotificationTagProfileDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<NotificationTagProfileDTO.SearchByParameters, string>
               {
                    {NotificationTagProfileDTO.SearchByParameters.NOTIFICATIONTAGPROFILEID, "ntp.NotificationTagProfileId"},
                    {NotificationTagProfileDTO.SearchByParameters.NOTIFICATIONTAGPROFILENAME, "ntp.NotificationTagProfileName"},
                    {NotificationTagProfileDTO.SearchByParameters.MASTER_ENTITY_ID, "ntp.MasterEntityId"},
                    {NotificationTagProfileDTO.SearchByParameters.SITE_ID,"ntp.site_id"},
                    {NotificationTagProfileDTO.SearchByParameters.IS_ACTIVE,"ntp.IsActive"}
               };
        private const string SELECT_QUERY = @"select * from NotificationTagProfile AS ntp ";
        /// <summary>
        /// Default constructor of NotificationTagProfileDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public NotificationTagProfileDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to NotificationTagProfileDTO class type
        /// </summary>
        /// <param name="notificationTagProfileDataRow">NotificationTagProfileDTO DataRow</param>
        /// <returns>Returns NotificationTagProfileDTO</returns>
        private NotificationTagProfileDTO GetNotificationTagProfileDTO(DataRow notificationTagProfileDataRow)
        {
            log.LogMethodEntry(notificationTagProfileDataRow);
            NotificationTagProfileDTO notificationTagProfileDataObject = new NotificationTagProfileDTO(Convert.ToInt32(notificationTagProfileDataRow["NotificationTagProfileId"]),
                                            notificationTagProfileDataRow["NotificationTagProfileName"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagProfileDataRow["NotificationTagProfileName"]),
                                            notificationTagProfileDataRow["SessionAlertFrequencyInMinutes"] == DBNull.Value ? Convert.ToDecimal(null) : Convert.ToDecimal(notificationTagProfileDataRow["SessionAlertFrequencyInMinutes"]),
                                            notificationTagProfileDataRow["SessionAlertNotificationPatternId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagProfileDataRow["SessionAlertNotificationPatternId"]),
                                            notificationTagProfileDataRow["AlertTimeInMinsBeforeExpiry"] == DBNull.Value ? Convert.ToDecimal(null) : Convert.ToDecimal(notificationTagProfileDataRow["AlertTimeInMinsBeforeExpiry"]),
                                            notificationTagProfileDataRow["AlertFrequencySecsBeforeExpiry"] == DBNull.Value ? Convert.ToDecimal(null) : Convert.ToDecimal(notificationTagProfileDataRow["AlertFrequencySecsBeforeExpiry"]),
                                            notificationTagProfileDataRow["AlertPatternIdBeforeExpiry"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagProfileDataRow["AlertPatternIdBeforeExpiry"]),
                                            notificationTagProfileDataRow["AlertPatternIdOnExpiry"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagProfileDataRow["AlertPatternIdOnExpiry"]),
                                            notificationTagProfileDataRow["AlertFrequencySecsOnExpiry"] == DBNull.Value ? Convert.ToDecimal(null) : Convert.ToDecimal(notificationTagProfileDataRow["AlertFrequencySecsOnExpiry"]),
                                            notificationTagProfileDataRow["AlertDurationMinsAfterExpiry"] == DBNull.Value ? Convert.ToDecimal(null) : Convert.ToDecimal(notificationTagProfileDataRow["AlertDurationMinsAfterExpiry"]),
                                            notificationTagProfileDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagProfileDataRow["IsActive"]),
                                            notificationTagProfileDataRow["CreatedBy"].ToString(),
                                            notificationTagProfileDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagProfileDataRow["CreationDate"]),
                                            notificationTagProfileDataRow["LastUpdatedBy"].ToString(),
                                            notificationTagProfileDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagProfileDataRow["LastUpdateDate"]),
                                            notificationTagProfileDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagProfileDataRow["site_id"]),
                                            notificationTagProfileDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagProfileDataRow["MasterEntityId"]),
                                            notificationTagProfileDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(notificationTagProfileDataRow["SynchStatus"]),
                                            notificationTagProfileDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit();
            return notificationTagProfileDataObject;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating NotificationTagProfileDTO Record.
        /// </summary>
        /// <param name="notificationTagProfileDTO">NotificationTagProfileDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(NotificationTagProfileDTO notificationTagProfileDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagProfileDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagProfileId", notificationTagProfileDTO.NotificationTagProfileId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagProfileName", notificationTagProfileDTO.NotificationTagProfileName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sessionAlertFrequencyInMinutes", notificationTagProfileDTO.SessionAlertFrequencyInMinutes));
            parameters.Add(dataAccessHandler.GetSQLParameter("@sessionAlertNotificationPatternId", notificationTagProfileDTO.SessionAlertNotificationPatternId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@alertTimeInMinsBeforeExpiry", notificationTagProfileDTO.AlertTimeInMinsBeforeExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@alertFrequencySecsBeforeExpiry", notificationTagProfileDTO.AlertFrequencySecsBeforeExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@alertPatternIdBeforeExpiry", notificationTagProfileDTO.AlertPatternIdBeforeExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@alertPatternIdOnExpiry", notificationTagProfileDTO.AlertPatternIdOnExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@alertFrequencySecsOnExpiry", notificationTagProfileDTO.AlertFrequencySecsOnExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@alertDurationMinsAfterExpiry", notificationTagProfileDTO.AlertDurationMinsAfterExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", notificationTagProfileDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", notificationTagProfileDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private void RefreshNotificationTagProfileDTO(NotificationTagProfileDTO notificationTagProfileDTO, DataTable dt)
        {
            log.LogMethodEntry(notificationTagProfileDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                notificationTagProfileDTO.NotificationTagProfileId = Convert.ToInt32(dt.Rows[0]["NotificationTagProfileId"]);
                notificationTagProfileDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                notificationTagProfileDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                notificationTagProfileDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                notificationTagProfileDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                notificationTagProfileDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                notificationTagProfileDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the NotificationTagProfile record to the database
        /// </summary>
        /// <param name="notificationTagProfileDTO">NotificationTagProfileDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public NotificationTagProfileDTO InsertNotificationTagProfile(NotificationTagProfileDTO notificationTagProfileDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagProfileDTO, userId, siteId);
            string insertQuery = @"insert into NotificationTagProfile 
                                                        (
                                                           NotificationTagProfileName,
                                                           SessionAlertFrequencyInMinutes,
                                                           SessionAlertNotificationPatternId,
                                                           AlertTimeInMinsBeforeExpiry,
                                                           AlertFrequencySecsBeforeExpiry,
                                                           AlertPatternIdBeforeExpiry,
                                                           AlertPatternIdOnExpiry,
                                                           AlertFrequencySecsOnExpiry,
                                                           AlertDurationMinsAfterExpiry,
                                                           IsActive,
                                                           CreatedBy,
                                                           CreationDate,
                                                           LastUpdatedBy,
                                                           LastUpdateDate,
                                                           site_id,
                                                           Guid,
                                                           MasterEntityId
                                                        ) 
                                                values 
                                                        ( 
                                                           @notificationTagProfileName,
                                                           @sessionAlertFrequencyInMinutes,
                                                           @sessionAlertNotificationPatternId,
                                                           @alertTimeInMinsBeforeExpiry,
                                                           @alertFrequencySecsBeforeExpiry,
                                                           @alertPatternIdBeforeExpiry,
                                                           @alertPatternIdOnExpiry,
                                                           @alertFrequencySecsOnExpiry,
                                                           @alertDurationMinsAfterExpiry,
                                                           @isActive,
                                                           @createdBy,
                                                           GetDate(),
                                                           @lastUpdatedBy,
                                                           GetDate(),
                                                           @site_id,
                                                           NewId(),
                                                           @masterEntityId
                                                        )SELECT * FROM NotificationTagProfile WHERE NotificationTagProfileId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(notificationTagProfileDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagProfileDTO(notificationTagProfileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagProfileDTO);
            return notificationTagProfileDTO;
        }

        /// <summary>
        /// Updates the NotificationTagProfileDTO record
        /// </summary>
        /// <param name="notificationTagProfileDTO">NotificationTagProfileDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public NotificationTagProfileDTO UpdateNotificationTagProfile(NotificationTagProfileDTO notificationTagProfileDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagProfileDTO, userId, siteId);
            string updateQuery = @"update NotificationTagProfile  set 
                                              NotificationTagProfileName        = @notificationTagProfileName,
                                              SessionAlertFrequencyInMinutes    = @sessionAlertFrequencyInMinutes,
                                              SessionAlertNotificationPatternId = @sessionAlertNotificationPatternId,
                                              AlertTimeInMinsBeforeExpiry       = @alertTimeInMinsBeforeExpiry,
                                              AlertFrequencySecsBeforeExpiry    = @alertFrequencySecsBeforeExpiry,
                                              AlertPatternIdBeforeExpiry        = @alertPatternIdBeforeExpiry,
                                              AlertPatternIdOnExpiry            = @alertPatternIdOnExpiry,
                                              AlertFrequencySecsOnExpiry        = @alertFrequencySecsOnExpiry,
                                              AlertDurationMinsAfterExpiry      = @alertDurationMinsAfterExpiry,
                                              IsActive                  = @isActive,
                                              LastUpdatedBy             = @lastUpdatedBy,
                                              LastUpdateDate            = GetDate(),
                                              MasterEntityId            = @masterEntityId
                                       where NotificationTagProfileId   = @notificationTagProfileId
                                       select * from NotificationTagProfile WHERE NotificationTagProfileId  = @notificationTagProfileId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(notificationTagProfileDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagProfileDTO(notificationTagProfileDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagProfileDTO);
            return notificationTagProfileDTO;
        }

        /// <summary>
        /// Deletes the NotificationTagProfile record of passed  notificationTagProfileId
        /// </summary>
        /// <param name="notificationTagProfileId">integer type parameter</param>
        public void Delete(int notificationTagProfileId)
        {
            log.LogMethodEntry(notificationTagProfileId);
            string query = @"DELETE  
                             FROM NotificationTagProfile
                             WHERE NotificationTagProfileId = @notificationTagProfileId";
            SqlParameter parameter = new SqlParameter("@notificationTagProfileId", notificationTagProfileId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the NotificationTagProfile data of passed notificationTagProfileId
        /// </summary>
        /// <param name="notificationTagProfileId">integer type parameter</param>
        /// <returns>Returns NotificationTagProfileDTO</returns>
        public NotificationTagProfileDTO GetNotificationTagProfileDTO(int notificationTagProfileId)
        {
            log.LogMethodEntry(notificationTagProfileId);
            NotificationTagProfileDTO notificationTagProfileDTO = null;
            string selectQuery = SELECT_QUERY + " where ntp.NotificationTagProfileId = @notificationTagProfileId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@notificationTagProfileId", notificationTagProfileId);
            DataTable notificationTagProfile = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters, sqlTransaction);
            if (notificationTagProfile.Rows.Count > 0)
            {
                DataRow dataRow = notificationTagProfile.Rows[0];
                notificationTagProfileDTO = GetNotificationTagProfileDTO(dataRow);
            }
            log.LogMethodExit(notificationTagProfileDTO);
            return notificationTagProfileDTO;
        }

        /// <summary>
        /// Gets the NotificationTagProfileDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of NotificationTagProfileDTO matching the search criteria</returns>
        public List<NotificationTagProfileDTO> GetNotificationTagProfileDTOList(List<KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<NotificationTagProfileDTO> notificationTagProfileDTOList = new List<NotificationTagProfileDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<NotificationTagProfileDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(NotificationTagProfileDTO.SearchByParameters.NOTIFICATIONTAGPROFILEID) ||
                            searchParameter.Key.Equals(NotificationTagProfileDTO.SearchByParameters.MASTER_ENTITY_ID)))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagProfileDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagProfileDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagProfileDTO.SearchByParameters.GUID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }
                if (searchParameters.Count > 0)
                    selectQuery = selectQuery + query;
            }

            DataTable notificationTagProfileDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (notificationTagProfileDataTable.Rows.Count > 0)
            {
                foreach (DataRow notificationTagProfileDataRow in notificationTagProfileDataTable.Rows)
                {
                    NotificationTagProfileDTO notificationTagProfileDataObject = GetNotificationTagProfileDTO(notificationTagProfileDataRow);
                    notificationTagProfileDTOList.Add(notificationTagProfileDataObject);
                }
            }
            log.LogMethodExit(notificationTagProfileDTOList);
            return notificationTagProfileDTOList;
        }

    }
}
