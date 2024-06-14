/********************************************************************************************
 * Project Name - Tags
 * Description  - NotificationTagViewDataHandler 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.120       04-Mar-2021   Girish Kundar          Created - Is Radian change
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagViewDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<NotificationTagViewDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<NotificationTagViewDTO.SearchByParameters, string>
               {
                    {NotificationTagViewDTO.SearchByParameters.TAG_NUMBER, "ntp.TagNumber"},
                    {NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS, "ntp.TagNotificationStatus"},
                    {NotificationTagViewDTO.SearchByParameters.TAG_ID, "ntp.NotificationTagId"},
                    {NotificationTagViewDTO.SearchByParameters.MARKED_FOR_STORAGE, "ntp.MarkedForStorage"},
                    {NotificationTagViewDTO.SearchByParameters.IS_IN_STORAGE, "ntp.IsInStorage"},
                    {NotificationTagViewDTO.SearchByParameters.PING_STATUS,"PingStatus"},
                    {NotificationTagViewDTO.SearchByParameters.SIGNAL_STRENGTH,"SignalStrength"},
                    {NotificationTagViewDTO.SearchByParameters.DEVICE_STATUS,"DeviceStatus"},
                    {NotificationTagViewDTO.SearchByParameters.SITE_ID,"ntp.site_id"},
                    {NotificationTagViewDTO.SearchByParameters.EXPIRED,"ExpiryDate"},
                    {NotificationTagViewDTO.SearchByParameters.ISSUED_TODAY,"IssueDate"},
                    {NotificationTagViewDTO.SearchByParameters.EXPIRING_IN_X_MINUTES,"ExpiryDate"},
                    {NotificationTagViewDTO.SearchByParameters.IS_RETURNED,"IsReturned"},
                    {NotificationTagViewDTO.SearchByParameters.EXPIRING_TODAY,"ExpiryDate"},
                    {NotificationTagViewDTO.SearchByParameters.BATTERY_PERCENTAGE,"BatteryStatusPercentage"}
               };
        private const string SELECT_QUERY = @"select ntp.NotificationTagId ,ntp.IsInStorage, ntp.TagNumber , t.TimeStamp , t.DeviceStatus,t.PingStatus,
      t.BatteryStatusPercentage,t.SignalStrength,v.ExpiryDate,v.IsReturned,v.issueDate,
         ntp.TagNotificationStatus,  datediff(minute, getdate(), v.ExpiryDate),  datediff(minute, v.ExpiryDate, getdate() )
     from (select * from NotificationTags where isActive = 1) ntp
     left outer join
        (select nts.NotificationTagId NotificationTagId,nts.Timestamp Timestamp ,nts.DeviceStatus DeviceStatus,
                nts.PingStatus  PingStatus,nts.BatteryStatusPercentage BatteryStatusPercentage ,nts.SignalStrength  SignalStrength,          
               dense_rank() OVER (partition by notificationTagId order by timestamp desc) rnk
                 from NotificationTagStatus nts) t
                on t.NotificationTagId = ntp.NotificationTagId and rnk = 1
                 left outer join (Select t.notificationTagId, nti.CardId,nti.IsReturned,nti.ExpiryDate, nti.issueDate,
              dense_rank() OVER (partition by t.notificationTagId order by nti.creationDate desc, nti.lastUpdateDate desc, nti.expirydate desc) rnk
                           from NotificationTagIssued nti, cards c, notificationTags t
                                    where c.card_id = nti.CardId
                                      and t.TagNumber = c.card_number) v on
                            v.notificationTagId = ntp.NotificationTagId and v.rnk = 1 ";
        /// <summary>
        /// Default constructor of NotificationTagPatternDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public NotificationTagViewDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        private NotificationTagViewDTO GetNotificationTagViewDTO(DataRow notificationTagPatternDataRow)
        {
            log.LogMethodEntry(notificationTagPatternDataRow);
            NotificationTagViewDTO notificationTagPatternDataObject = new NotificationTagViewDTO(
                                            notificationTagPatternDataRow["NotificationTagId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagPatternDataRow["NotificationTagId"]),
                                            notificationTagPatternDataRow["IsInStorage"] == DBNull.Value ? false : Convert.ToBoolean(notificationTagPatternDataRow["IsInStorage"]),
                                            notificationTagPatternDataRow["TagNumber"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagPatternDataRow["TagNumber"]),
                                            notificationTagPatternDataRow["TimeStamp"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(notificationTagPatternDataRow["TimeStamp"]),
                                            notificationTagPatternDataRow["DeviceStatus"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagPatternDataRow["DeviceStatus"]),
                                            notificationTagPatternDataRow["PingStatus"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagPatternDataRow["PingStatus"]),
                                            notificationTagPatternDataRow["BatteryStatusPercentage"] == DBNull.Value ? "0%" : Decimal.Truncate(Convert.ToDecimal(notificationTagPatternDataRow["BatteryStatusPercentage"])).ToString() + "%",
                                            notificationTagPatternDataRow["SignalStrength"] == DBNull.Value ? string.Empty : GetSignalStrengthFormattedString(Convert.ToString(notificationTagPatternDataRow["SignalStrength"])),
                                            notificationTagPatternDataRow["ExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(notificationTagPatternDataRow["ExpiryDate"]));
            log.LogMethodExit();
            notificationTagPatternDataObject.AcceptChanges();
            return notificationTagPatternDataObject;
        }

        private string GetSignalStrengthFormattedString(string signalStrength)
        {
            log.LogMethodEntry();
            string result = signalStrength.Substring(0, signalStrength.IndexOf("_") < 0 ? signalStrength.Length : signalStrength.IndexOf("_"));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Gets the GetNotificationTagViewDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of GetNotificationTagViewDTO matching the search criteria</returns>
        public List<NotificationTagViewDTO> GetNotificationTagViewDTOList(List<KeyValuePair<NotificationTagViewDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<NotificationTagViewDTO> notificationTagViewDTOList = new List<NotificationTagViewDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" Where ");
                foreach (KeyValuePair<NotificationTagViewDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    string joiner = (count == 0) ? " " : " and ";

                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.TAG_NUMBER) ||
                            searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.TAG_NOTIFICATION_STATUS) ||
                            searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.PING_STATUS) ||
                            searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.CHANNEL) ||
                            searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.DEVICE_STATUS))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.EXPIRING_TODAY))
                        {
                            string businessStartTime = searchParameter.Value;
                            string dateQuery = @" ExpiryDate > getdate()  and Expirydate between  case when datepart(hour, getdate())
                                                  between 0 and " + businessStartTime + " then dateadd(hour, 6,convert(datetime, convert(date, getdate() - 1)))" +
                                                  " else dateadd(hour, " + businessStartTime + ", convert(datetime, convert(date, getdate()))) end and case when " +
                                                  "datepart(hour, getdate()) between 0 and " + businessStartTime + " then dateadd(hour," + businessStartTime + ",convert(datetime, convert(date, getdate())))" +
                                                  " else dateadd(hour, " + businessStartTime + ", convert(datetime, convert(date, getdate() + 1))) end";
                            query.Append(joiner + dateQuery);
                            //query.Append(joiner + "CAST(" + DBSearchParameters[searchParameter.Key] + " as date) = CAST(getdate() as date)");
                            //parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ServerDateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.ISSUED_TODAY))
                        {
                            string businessStartTime = searchParameter.Value;
                            string dateQuery = @" IssueDate between  case when datepart(hour, getdate())
                                                  between 0 and " + businessStartTime + " then dateadd(hour, 6,convert(datetime, convert(date, getdate() - 1)))" +
                                                  " else dateadd(hour, " + businessStartTime + ", convert(datetime, convert(date, getdate()))) end and case when " +
                                                  "datepart(hour, getdate()) between 0 and " + businessStartTime + " then dateadd(hour," + businessStartTime + ",convert(datetime, convert(date, getdate())))" +
                                                  " else dateadd(hour, " + businessStartTime + ", convert(datetime, convert(date, getdate() + 1))) end";
                            query.Append(joiner + dateQuery);
                            //query.Append(joiner + "CAST(" + DBSearchParameters[searchParameter.Key] + " as date) = CAST(getdate() as date)");
                            //parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ServerDateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.EXPIRED))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "<" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.BATTERY_PERCENTAGE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.EXPIRING_IN_X_MINUTES))
                        {
                            query.Append(joiner + " DATEDIFF(minute,GETDATE()," + DBSearchParameters[searchParameter.Key] + ") between 0 and " + searchParameter.Value);
                        }
                        else if (searchParameter.Key == NotificationTagViewDTO.SearchByParameters.SITE_ID)
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.IS_IN_STORAGE) ||
                                 searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.MARKED_FOR_STORAGE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.IS_RETURNED))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagViewDTO.SearchByParameters.TAG_ID))
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
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
            DataTable dataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow notificationTagPatternDataRow in dataTable.Rows)
                {
                    NotificationTagViewDTO notificationTagViewDTO = GetNotificationTagViewDTO(notificationTagPatternDataRow);
                    notificationTagViewDTOList.Add(notificationTagViewDTO);
                }
            }
            log.LogMethodExit(notificationTagViewDTOList);
            return notificationTagViewDTOList;
        }
    }
}
