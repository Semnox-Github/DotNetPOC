/********************************************************************************************
* Project Name - NotificationTagManualEventsDataHandler
* Description - BL for NotificationTagManualEvents 
*
**************
**Version Log 
**************
*Version    Date        Modified By     Remarks
*********************************************************************************************
*2.110.0    07-Jan-2021  Fiona          Created 
*********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    internal class NotificationTagManualEventsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<NotificationTagManualEventsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<NotificationTagManualEventsDTO.SearchByParameters, string>
               {
                    {NotificationTagManualEventsDTO.SearchByParameters.NOTIFICATION_TAG_EVENT_ID, "nti.NotificationTagMEventId"},
                    {NotificationTagManualEventsDTO.SearchByParameters.NOTIFICATION_TAG_ID, "nti.NotificationTagId"},
                    {NotificationTagManualEventsDTO.SearchByParameters.MASTER_ENTITY_ID, "nti.MasterEntityId"},
                    {NotificationTagManualEventsDTO.SearchByParameters.SITE_ID,"nti.site_id"},
                    {NotificationTagManualEventsDTO.SearchByParameters.IS_ACTIVE,"nti.IsActive"},
                    {NotificationTagManualEventsDTO.SearchByParameters.PROCESSING_STATUS,"nti.ProcessingStatus"},
                    {NotificationTagManualEventsDTO.SearchByParameters.TIMESTAMP,"nti.Timestamp"}
               };
        private const string SELECT_QUERY = @"select * from NotificationTagManualEvents AS nti ";
        /// <summary>
        /// Default constructor of NotificationTagManualEventsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public NotificationTagManualEventsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }
        private NotificationTagManualEventsDTO GetNotificationTagManualEventsDTO(DataRow notificationTagManualEventsDTO)
        {
            log.LogMethodEntry(notificationTagManualEventsDTO);
            NotificationTagManualEventsDTO notificationTagManualEventsDataObject = new NotificationTagManualEventsDTO(Convert.ToInt32(notificationTagManualEventsDTO["NotificationTagMEventId"]),
                                            notificationTagManualEventsDTO["NotificationTagId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagManualEventsDTO["NotificationTagId"]),
                                            notificationTagManualEventsDTO["Command"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagManualEventsDTO["Command"]),
                                            notificationTagManualEventsDTO["NotificationTagProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagManualEventsDTO["NotificationTagProfileId"]),
                                            notificationTagManualEventsDTO["LastSessionAlertTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagManualEventsDTO["LastSessionAlertTime"]),
                                            notificationTagManualEventsDTO["LastAlertTimeBeforeExpiry"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagManualEventsDTO["LastAlertTimeBeforeExpiry"]),
                                            notificationTagManualEventsDTO["LastAlertTimeOnExpiry"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagManualEventsDTO["LastAlertTimeOnExpiry"]),
                                            notificationTagManualEventsDTO["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagManualEventsDTO["Timestamp"]),
                                            notificationTagManualEventsDTO["ProcessingStatus"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagManualEventsDTO["ProcessingStatus"]),
                                            notificationTagManualEventsDTO["ProcessDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagManualEventsDTO["ProcessDate"]),
                                            notificationTagManualEventsDTO["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagManualEventsDTO["IsActive"]),
                                            notificationTagManualEventsDTO["Remarks"].ToString(),
                                            notificationTagManualEventsDTO["CreatedBy"].ToString(),
                                            notificationTagManualEventsDTO["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagManualEventsDTO["CreationDate"]),
                                            notificationTagManualEventsDTO["LastUpdatedBy"].ToString(),
                                            notificationTagManualEventsDTO["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagManualEventsDTO["LastUpdateDate"]),
                                            notificationTagManualEventsDTO["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagManualEventsDTO["site_id"]),
                                            notificationTagManualEventsDTO["Guid"].ToString(), 
                                            notificationTagManualEventsDTO["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(notificationTagManualEventsDTO["SynchStatus"]),
                                            notificationTagManualEventsDTO["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagManualEventsDTO["MasterEntityId"])
                                            );
            log.LogMethodExit();
            return notificationTagManualEventsDataObject;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating NotificationTagManualEvents Record.
        /// </summary>
        /// <param name="notificationTagManualEventsDTO">NotificationTagManualEventsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(NotificationTagManualEventsDTO notificationTagManualEventsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagManualEventsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@NotificationTagMEventId", notificationTagManualEventsDTO.NotificationTagMEventId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NotificationTagId", notificationTagManualEventsDTO.NotificationTagId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Command", notificationTagManualEventsDTO.Command));
            parameters.Add(dataAccessHandler.GetSQLParameter("@NotificationTagProfileId", notificationTagManualEventsDTO.NotificationTagProfileId,true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastSessionAlertTime", notificationTagManualEventsDTO.LastSessionAlertTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastAlertTimeBeforeExpiry", notificationTagManualEventsDTO.LastAlertTimeBeforeExpiry, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@LastAlertTimeOnExpiry", notificationTagManualEventsDTO.LastAlertTimeOnExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Timestamp", notificationTagManualEventsDTO.Timestamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProcessingStatus", notificationTagManualEventsDTO.ProcessingStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ProcessDate", notificationTagManualEventsDTO.ProcessDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", notificationTagManualEventsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@Remarks", notificationTagManualEventsDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", notificationTagManualEventsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private void RefreshNotificationTagManualEventsDTO(NotificationTagManualEventsDTO notificationTagManualEventsDTO, DataTable dt)
        {
            log.LogMethodEntry(notificationTagManualEventsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                notificationTagManualEventsDTO.NotificationTagMEventId = Convert.ToInt32(dt.Rows[0]["NotificationTagMEventId"]);
                notificationTagManualEventsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                notificationTagManualEventsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                notificationTagManualEventsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                notificationTagManualEventsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                notificationTagManualEventsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                notificationTagManualEventsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Inserts the NotificationTagManualEvents record to the database
        /// </summary>
        /// <param name="notificationTagManualEventsDTO">NotificationTagManualEventsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public NotificationTagManualEventsDTO InsertNotificationTagManualEvents(NotificationTagManualEventsDTO notificationTagManualEventsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagManualEventsDTO, userId, siteId);
            string insertQuery = @"insert into NotificationTagManualEvents 
                                                        (
                                                            NotificationTagId,
                                                            Command,
                                                            NotificationTagProfileId,
                                                            LastSessionAlertTime,
                                                            LastAlertTimeBeforeExpiry,
                                                            LastAlertTimeOnExpiry,
                                                            Timestamp,
                                                            ProcessingStatus,
                                                            ProcessDate,
                                                            IsActive,
                                                            Remarks,
                                                            CreationDate,
                                                            CreatedBy,
                                                            LastUpdateDate,
                                                            LastUpdatedBy, 
                                                            Site_id,
                                                            GUID,
                                                            MasterEntityId
                                                        ) 
                                                values 
                                                        ( 
                                                           @NotificationTagId,
                                                           @Command,
                                                           @NotificationTagProfileId,
                                                           @LastSessionAlertTime,
                                                           @LastAlertTimeBeforeExpiry,
                                                           @LastAlertTimeOnExpiry,
                                                           @Timestamp,
                                                           @ProcessingStatus,
                                                           @ProcessDate,
                                                           @isActive,
                                                           @Remarks,
                                                           GetDate(),
                                                           @createdBy,
                                                           GetDate(),
                                                           @lastUpdatedBy,
                                                           @site_id,
                                                           NewId(),
                                                           @masterEntityId
                                                        )SELECT * FROM NotificationTagManualEvents WHERE NotificationTagMEventId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(notificationTagManualEventsDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagManualEventsDTO(notificationTagManualEventsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagManualEventsDTO);
            return notificationTagManualEventsDTO;
        }
        /// <summary>
        /// Updates the NotificationTagManualEvents record
        /// </summary>
        /// <param name="notificationTagManualEventsDTO">NotificationTagManualEventsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public NotificationTagManualEventsDTO UpdateNotificationTagManualEvents(NotificationTagManualEventsDTO notificationTagManualEventsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagManualEventsDTO, userId, siteId);
            string updateQuery = @"update NotificationTagManualEvents  set 
                                                            NotificationTagId=          @NotificationTagId,
                                                            Command=                    @Command,
                                                            NotificationTagProfileId=   @NotificationTagProfileId,
                                                            LastSessionAlertTime=       @LastSessionAlertTime,
                                                            LastAlertTimeBeforeExpiry=  @LastAlertTimeBeforeExpiry,
                                                            LastAlertTimeOnExpiry=      @LastAlertTimeOnExpiry,
                                                            Timestamp=                  @Timestamp,
                                                            ProcessingStatus=           @ProcessingStatus,
                                                            ProcessDate=                @ProcessDate,
                                                            IsActive=                   @isActive,
                                                            Remarks =                   @Remarks,
                                                            LastUpdateDate=             GetDate(),
                                                            LastUpdatedBy=              @lastUpdatedBy,
                                                            MasterEntityId=             @masterEntityId
                                       where NotificationTagMEventId =                  @NotificationTagMEventId
                                       select * from NotificationTagManualEvents where  NotificationTagMEventId = @NotificationTagMEventId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(notificationTagManualEventsDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagManualEventsDTO(notificationTagManualEventsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagManualEventsDTO);
            return notificationTagManualEventsDTO;
        }

        /// <summary>
        /// Deletes the NotificationTagManualEvents record of passed  NotificationTagMEventId
        /// </summary>
        /// <param name="notificationTagMEventId">integer type parameter</param>
        public void DeleteNotificationTagManualEvents(int notificationTagMEventId)
        {
            log.LogMethodEntry(notificationTagMEventId);
            string query = @"DELETE  
                             FROM NotificationTagManualEvents
                             WHERE NotificationTagMEventId = @NotificationTagMEventId";
            SqlParameter parameter = new SqlParameter("@NotificationTagMEventId", notificationTagMEventId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the NotificationTagManualEvents data of passed NotificationTagMEventId
        /// </summary>
        /// <param name="NotificationTagMEventsId">integer type parameter</param>
        /// <returns>Returns NotificationTagManualEventsDTO</returns>
        public NotificationTagManualEventsDTO GetNotificationTagManualEventsDTO(int notificationTagMEventId)
        {
            log.LogMethodEntry(notificationTagMEventId);
            NotificationTagManualEventsDTO notificationTagManualEventsDTO = null;
            string selectQuery = SELECT_QUERY + "  where nti.NotificationTagMEventId = @NotificationTagMEventId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@NotificationTagMEventId", notificationTagMEventId);
            DataTable notificationTagManualEvents = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters, sqlTransaction);
            if (notificationTagManualEvents.Rows.Count > 0)
            {
                DataRow dataRow = notificationTagManualEvents.Rows[0];
                notificationTagManualEventsDTO = GetNotificationTagManualEventsDTO(dataRow);
            }
            log.LogMethodExit(notificationTagManualEventsDTO);
            return notificationTagManualEventsDTO;
        }
        
        /// <summary>
        /// Gets the NotificationTagManualEventsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of NotificationTagManualEventsDTO matching the search criteria</returns>
        public List<NotificationTagManualEventsDTO> GetNotificationTagManualEventsDTOList(List<KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<NotificationTagManualEventsDTO> notificationTagManualEventsDTOList = new List<NotificationTagManualEventsDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<NotificationTagManualEventsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(NotificationTagManualEventsDTO.SearchByParameters.NOTIFICATION_TAG_EVENT_ID) ||
                            searchParameter.Key.Equals(NotificationTagManualEventsDTO.SearchByParameters.NOTIFICATION_TAG_ID) ||
                            searchParameter.Key.Equals(NotificationTagManualEventsDTO.SearchByParameters.MASTER_ENTITY_ID)))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagManualEventsDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagManualEventsDTO.SearchByParameters.PROCESSING_STATUS))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ", 'P') = " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagManualEventsDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagManualEventsDTO.SearchByParameters.TIMESTAMP))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + " >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagManualEventsDTO.SearchByParameters.GUID))
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

            DataTable NotificationTagManualEventsDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (NotificationTagManualEventsDataTable.Rows.Count > 0)
            {
                foreach (DataRow NotificationTagManualEventsDataRow in NotificationTagManualEventsDataTable.Rows)
                {
                    NotificationTagManualEventsDTO notificationTagManualEventsDataObject = GetNotificationTagManualEventsDTO(NotificationTagManualEventsDataRow);
                    notificationTagManualEventsDTOList.Add(notificationTagManualEventsDataObject);
                }
            }
            log.LogMethodExit(notificationTagManualEventsDTOList);
            return notificationTagManualEventsDTOList;
        }
    }
}
