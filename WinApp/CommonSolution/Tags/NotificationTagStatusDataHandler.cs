/********************************************************************************************
 * Project Name - Tags
 * Description  - Data Handler of the NotificationTagStatus class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        21-jul-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagStatusDataHandler
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<NotificationTagStatusDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<NotificationTagStatusDTO.SearchByParameters, string>
               {
                    {NotificationTagStatusDTO.SearchByParameters.NOTIFICATIONTAGID, "nts.NotificationTagId"},
                    {NotificationTagStatusDTO.SearchByParameters.NOTIFICATIONTAGSTATUSID, "nts.NotificationTagStatusId"},
                    {NotificationTagStatusDTO.SearchByParameters.SITE_ID,"nts.site_id"},
                    {NotificationTagStatusDTO.SearchByParameters.CHANNEL,"nts.Channel"},
                    {NotificationTagStatusDTO.SearchByParameters.MASTER_ENTITY_ID,"nts.MasterEntityId"},
                    {NotificationTagStatusDTO.SearchByParameters.IS_ACTIVE,"nts.IsActive"},
                    {NotificationTagStatusDTO.SearchByParameters.TIMESTAMP,"nts.Timestamp"}
               };

        private const string SELECT_QUERY = @"select * from NotificationTagStatus AS nts ";
        /// <summary>
        /// Default constructor of NotificationTagStatusDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public NotificationTagStatusDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to NotificationTagStatusDTO class type
        /// </summary>
        /// <param name="notificationTagStatusDataRow">NotificationTagStatusDTO DataRow</param>
        /// <returns>Returns NotificationTagStatusDTO</returns>
        private NotificationTagStatusDTO GetNotificationTagStatusDTO(DataRow notificationTagStatusDataRow)
        {
            log.LogMethodEntry(notificationTagStatusDataRow);
            NotificationTagStatusDTO notificationTagStatusDataObject = new NotificationTagStatusDTO(Convert.ToInt32(notificationTagStatusDataRow["NotificationTagStatusId"]),
                                            notificationTagStatusDataRow["NotificationTagId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagStatusDataRow["NotificationTagId"]),
                                            notificationTagStatusDataRow["Timestamp"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagStatusDataRow["Timestamp"]),
                                            notificationTagStatusDataRow["PingStatus"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagStatusDataRow["PingStatus"]),
                                            notificationTagStatusDataRow["BatteryStatusPercentage"] == DBNull.Value ? Convert.ToDecimal(null) : Convert.ToDecimal(notificationTagStatusDataRow["BatteryStatusPercentage"]),
                                            notificationTagStatusDataRow["SignalStrength"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagStatusDataRow["SignalStrength"]),
                                            notificationTagStatusDataRow["DeviceStatus"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagStatusDataRow["DeviceStatus"]),
                                            notificationTagStatusDataRow["Channel"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagStatusDataRow["Channel"]),
                                            notificationTagStatusDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagStatusDataRow["IsActive"]),
                                            notificationTagStatusDataRow["CreatedBy"].ToString(),
                                            notificationTagStatusDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagStatusDataRow["CreationDate"]),
                                            notificationTagStatusDataRow["LastUpdatedBy"].ToString(),
                                            notificationTagStatusDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagStatusDataRow["LastUpdateDate"]),
                                            notificationTagStatusDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagStatusDataRow["site_id"]),
                                            notificationTagStatusDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagStatusDataRow["MasterEntityId"]),
                                            notificationTagStatusDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(notificationTagStatusDataRow["SynchStatus"]),
                                            notificationTagStatusDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit();
            return notificationTagStatusDataObject;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating NotificationTagStatus Record.
        /// </summary>
        /// <param name="NotificationTagStatusDTO">NotificationTagStatusDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(NotificationTagStatusDTO notificationTagStatusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagStatusDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagStatusId", notificationTagStatusDTO.NotificationTagStatusId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagId", notificationTagStatusDTO.NotificationTagId, true));
            //parameters.Add(dataAccessHandler.GetSQLParameter("@timestamp", notificationTagStatusDTO.TimeStamp));
            parameters.Add(dataAccessHandler.GetSQLParameter("@pingStatus", notificationTagStatusDTO.PingStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@batteryStatusPercentage", notificationTagStatusDTO.BatteryStatusPercentage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@signalStrength", notificationTagStatusDTO.SignalStrength));
            parameters.Add(dataAccessHandler.GetSQLParameter("@deviceStatus", notificationTagStatusDTO.DeviceStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@channel", notificationTagStatusDTO.Channel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", notificationTagStatusDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", notificationTagStatusDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private void RefreshNotificationTagStatusDTO(NotificationTagStatusDTO notificationTagStatusDTO, DataTable dt)
        {
            log.LogMethodEntry(notificationTagStatusDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                notificationTagStatusDTO.NotificationTagStatusId = Convert.ToInt32(dt.Rows[0]["NotificationTagStatusId"]);
                notificationTagStatusDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                notificationTagStatusDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                notificationTagStatusDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                notificationTagStatusDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                notificationTagStatusDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                notificationTagStatusDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Inserts the NotificationTagStatus record to the database
        /// </summary>
        /// <param name="notificationTagStatusDTO">NotificationTagStatusDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public NotificationTagStatusDTO InsertNotificationTagStatus(NotificationTagStatusDTO notificationTagStatusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagStatusDTO, userId, siteId);
            string insertQuery = @"insert into NotificationTagStatus 
                                                        (
                                                           NotificationTagId,
                                                           Timestamp,
                                                           PingStatus,
                                                           BatteryStatusPercentage,
                                                           SignalStrength,
                                                           DeviceStatus,
                                                           Channel,
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
                                                           @notificationTagId,
                                                           GetDate(),
                                                           @pingStatus,
                                                           @batteryStatusPercentage,
                                                           @signalStrength,
                                                           @deviceStatus,
                                                           @channel,
                                                           @isActive,
                                                           @createdBy,
                                                           GetDate(),
                                                           @lastUpdatedBy,
                                                           GetDate(),
                                                           @site_id,
                                                           NewId(),
                                                           @masterEntityId
                                                        )SELECT * FROM NotificationTagStatus WHERE NotificationTagStatusId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(notificationTagStatusDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagStatusDTO(notificationTagStatusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagStatusDTO);
            return notificationTagStatusDTO;
        }

        /// <summary>
        /// Updates the NotificationTagStatus record
        /// </summary>
        /// <param name="notificationTagStatusDTO">notificationTagStatusDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public NotificationTagStatusDTO UpdateNotificationTagStatus(NotificationTagStatusDTO notificationTagStatusDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagStatusDTO, userId, siteId);
            string updateQuery = @"update NotificationTagStatus  set 
                                              NotificationTagId = @notificationTagId,
                                              Timestamp         = GetDate(),
                                              PingStatus        = @pingStatus,
                                              BatteryStatusPercentage = @batteryStatusPercentage,
                                              SignalStrength    = @signalStrength,
                                              DeviceStatus      = @deviceStatus,
                                              Channel           = @channel,
                                              IsActive                  = @isActive,
                                              LastUpdatedBy             = @lastUpdatedBy,
                                              LastUpdateDate            = GetDate(),
                                              MasterEntityId            = @masterEntityId
                                       where NotificationTagStatusId = @notificationTagStatusId
                                       select * from NotificationTagStatus where  NotificationTagStatusId = @notificationTagStatusId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(notificationTagStatusDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagStatusDTO(notificationTagStatusDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagStatusDTO);
            return notificationTagStatusDTO;
        }

        /// <summary>
        /// Deletes the NotificationTagStatus record of passed  NotificationTagStatusId
        /// </summary>
        /// <param name="notificationTagStatusId">integer type parameter</param>
        public void DeleteNotificationTagStatus(int notificationTagStatusId)
        {
            log.LogMethodEntry(notificationTagStatusId);
            string query = @"DELETE  
                             FROM NotificationTagStatus
                             WHERE NotificationTagStatusId = @notificationTagStatusId";
            SqlParameter parameter = new SqlParameter("@notificationTagStatusId", notificationTagStatusId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the NotificationTagStatus data of passed NotificationTagStatusId
        /// </summary>
        /// <param name="notificationTagStatusId">integer type parameter</param>
        /// <returns>Returns NotificationTagStatusDTO</returns>
        public NotificationTagStatusDTO GetNotificationTagStatusDTO(int notificationTagStatusId)
        {
            log.LogMethodEntry(notificationTagStatusId);
            NotificationTagStatusDTO notificationTagStatusDTO = null;
            string selectQuery = SELECT_QUERY + " where nts.NotificationTagStatusId = @notificationTagStatusId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@notificationTagStatusId", notificationTagStatusId);
            DataTable notificationTagStatus = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters, sqlTransaction);
            if (notificationTagStatus.Rows.Count > 0)
            {
                DataRow dataRow = notificationTagStatus.Rows[0];
                notificationTagStatusDTO = GetNotificationTagStatusDTO(dataRow);
            }
            log.LogMethodExit(notificationTagStatusDTO);
            return notificationTagStatusDTO;
        }

        /// <summary>
        /// Gets the NotificationTagStatusDTO List for NotificationTagStatus Id List
        /// </summary>
        /// <param name="notificationTagStatusIdList">integer list parameter</param>
        /// <returns>Returns List of NotificationTagStatusDTOList</returns>
        public List<NotificationTagStatusDTO> GetNotificationTagStatusDTOList(List<int> notificationTagStatusIdList, bool activeRecords)
        {
            log.LogMethodEntry(notificationTagStatusIdList);
            List<NotificationTagStatusDTO> list = new List<NotificationTagStatusDTO>();
            string query = @"SELECT NotificationTagStatus.*
                            FROM NotificationTagStatus, @notificationTagStatusIdList List
                            WHERE NotificationTagStatusId = List.id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@notificationTagStatusIdList", notificationTagStatusIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetNotificationTagStatusDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }


        /// <summary>
        /// Gets the NotificationTagStatusDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of NotificationTagStatusDTO matching the search criteria</returns>
        public List<NotificationTagStatusDTO> GetNotificationTagStatusDTOList(List<KeyValuePair<NotificationTagStatusDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<NotificationTagStatusDTO> notificationTagStatusDTOList = new List<NotificationTagStatusDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<NotificationTagStatusDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(NotificationTagStatusDTO.SearchByParameters.NOTIFICATIONTAGID) ||
                            searchParameter.Key.Equals(NotificationTagStatusDTO.SearchByParameters.NOTIFICATIONTAGSTATUSID) ||
                            searchParameter.Key.Equals(NotificationTagStatusDTO.SearchByParameters.MASTER_ENTITY_ID) ||
                            searchParameter.Key.Equals(NotificationTagStatusDTO.SearchByParameters.CHANNEL))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagStatusDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagStatusDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagStatusDTO.SearchByParameters.GUID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == NotificationTagStatusDTO.SearchByParameters.TIMESTAMP)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
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

            DataTable notificationTagStatusDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (notificationTagStatusDataTable.Rows.Count > 0)
            {
                foreach (DataRow notificationTagStatusDataRow in notificationTagStatusDataTable.Rows)
                {
                    NotificationTagStatusDTO notificationTagStatusDataObject = GetNotificationTagStatusDTO(notificationTagStatusDataRow);
                    notificationTagStatusDTOList.Add(notificationTagStatusDataObject);
                }
            }
            log.LogMethodExit(notificationTagStatusDTOList);
            return notificationTagStatusDTOList;
        }

    }
}
