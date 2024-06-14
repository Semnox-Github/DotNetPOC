/********************************************************************************************
 * Project Name - Transaction
 * Description  - Data Handler of the NotificationTagIssued class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
  *2.140.0     09-Sep-2021   Girish Kundar  Modified: Check In/Check out changes
 ********************************************************************************************/
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
    public class NotificationTagIssuedDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<NotificationTagIssuedDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<NotificationTagIssuedDTO.SearchByParameters, string>
               {
                    {NotificationTagIssuedDTO.SearchByParameters.CARDID, "nti.CardId"},
                    {NotificationTagIssuedDTO.SearchByParameters.EXPIRYDATE, "nti.ExpiryDate"},
                    {NotificationTagIssuedDTO.SearchByParameters.ISSUEDATE, "nti.IssueDate"},
                    {NotificationTagIssuedDTO.SearchByParameters.LINEID, "nti.LineId"},
                    {NotificationTagIssuedDTO.SearchByParameters.MASTER_ENTITY_ID, "nti.MasterEntityId"},
                    {NotificationTagIssuedDTO.SearchByParameters.NOTIFICATIONTAGISSUEDID, "nti.NotificationTagIssuedId"},
                    {NotificationTagIssuedDTO.SearchByParameters.STARTDATE, "nti.StartDate"},
                    {NotificationTagIssuedDTO.SearchByParameters.TRANSACTIONID, "nti.TrxId"},
                    {NotificationTagIssuedDTO.SearchByParameters.SITE_ID,"nti.site_id"},
                    {NotificationTagIssuedDTO.SearchByParameters.ISRETURNED,"nti.IsReturned"},
                    {NotificationTagIssuedDTO.SearchByParameters.LASTUPDATEDATE_AFTER,"nti.LastUpdateDate"},
                    {NotificationTagIssuedDTO.SearchByParameters.IS_ACTIVE,"nti.IsActive"},
                    {NotificationTagIssuedDTO.SearchByParameters.LASTUPDATE_STARTDATE_AFTER,"IIF(nti.LastUpdateDate >= nti.StartDate, nti.LastUpdateDate, nti.StartDate)"},
                    {NotificationTagIssuedDTO.SearchByParameters.EXPIRY_DATE_NULL_AFTER,"ISNULL(nti.ExpiryDate, getdate())"}
               };
        private const string SELECT_QUERY = @"select * from NotificationTagIssued AS nti ";
        /// <summary>
        /// Default constructor of NotificationTagIssuedDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public NotificationTagIssuedDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Converts the Data row object to NotificationTagIssuedDTO class type
        /// </summary>
        /// <param name="notificationTagIssuedDataRow">NotificationTagIssuedDTO DataRow</param>
        /// <returns>Returns NotificationTagIssuedDTO</returns>
        private NotificationTagIssuedDTO GetNotificationTagIssuedDTO(DataRow notificationTagIssuedDataRow)
        {
            log.LogMethodEntry(notificationTagIssuedDataRow);
            NotificationTagIssuedDTO notificationTagIssuedDataObject = new NotificationTagIssuedDTO(Convert.ToInt32(notificationTagIssuedDataRow["NotificationTagIssuedId"]),
                                            notificationTagIssuedDataRow["CardId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagIssuedDataRow["CardId"]),
                                            notificationTagIssuedDataRow["IssueDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["IssueDate"]),
                                            notificationTagIssuedDataRow["StartDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["StartDate"]),
                                            notificationTagIssuedDataRow["ExpiryDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["ExpiryDate"]),
                                            notificationTagIssuedDataRow["TrxId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagIssuedDataRow["TrxId"]),
                                            notificationTagIssuedDataRow["LineId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagIssuedDataRow["LineId"]),
                                            notificationTagIssuedDataRow["IsReturned"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagIssuedDataRow["IsReturned"]),
                                            notificationTagIssuedDataRow["ReturnDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["ReturnDate"]),
                                            notificationTagIssuedDataRow["NotificationTagProfileId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagIssuedDataRow["NotificationTagProfileId"]),
                                            notificationTagIssuedDataRow["LastSessionAlertTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["LastSessionAlertTime"]),
                                            notificationTagIssuedDataRow["LastAlertTimeBeforeExpiry"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["LastAlertTimeBeforeExpiry"]),
                                            notificationTagIssuedDataRow["LastAlertTimeOnExpiry"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["LastAlertTimeOnExpiry"]),
                                            notificationTagIssuedDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagIssuedDataRow["IsActive"]),
                                            notificationTagIssuedDataRow["CreatedBy"].ToString(),
                                            notificationTagIssuedDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["CreationDate"]),
                                            notificationTagIssuedDataRow["LastUpdatedBy"].ToString(),
                                            notificationTagIssuedDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagIssuedDataRow["LastUpdateDate"]),
                                            notificationTagIssuedDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagIssuedDataRow["site_id"]),
                                            notificationTagIssuedDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagIssuedDataRow["MasterEntityId"]),
                                            notificationTagIssuedDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(notificationTagIssuedDataRow["SynchStatus"]),
                                            notificationTagIssuedDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit();
            return notificationTagIssuedDataObject;
        }

        internal NotificationTagIssuedDTO UpdateTagStartTime(NotificationTagIssuedDTO notificationTagIssuedDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagIssuedDTO, userId, siteId);
            string updateQuery = @"update NotificationTagIssued  set 
                                              StartDate                 = GetDate(),
                                              LastUpdatedBy             = @lastUpdatedBy,
                                              LastUpdateDate            = GetDate()
                                       where NotificationTagIssuedId = @notificationTagIssuedId
                                       select * from NotificationTagIssued where  NotificationTagIssuedId = @notificationTagIssuedId";
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagIssuedId", notificationTagIssuedDTO.NotificationTagIssuedId));
               // parameters.Add(dataAccessHandler.GetSQLParameter("@startDate", notificationTagIssuedDTO.StartDate == DateTime.MinValue ? DBNull.Value : (object)notificationTagIssuedDTO.StartDate));
                parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, parameters.ToArray(), sqlTransaction);
                RefreshNotificationTagIssuedDTO(notificationTagIssuedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagIssuedDTO);
            return notificationTagIssuedDTO;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating NotificationTagIssued Record.
        /// </summary>
        /// <param name="NotificationTagIssuedDTO">NotificationTagIssuedDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(NotificationTagIssuedDTO notificationTagIssuedDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagIssuedDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagIssuedId", notificationTagIssuedDTO.NotificationTagIssuedId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@cardId", notificationTagIssuedDTO.CardId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@issueDate", notificationTagIssuedDTO.IssueDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@startDate", notificationTagIssuedDTO.StartDate == DateTime.MinValue ? DBNull.Value : (object) notificationTagIssuedDTO.StartDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@expiryDate", notificationTagIssuedDTO.ExpiryDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@trxId", notificationTagIssuedDTO.TransactionId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lineId", notificationTagIssuedDTO.LineId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isReturned", notificationTagIssuedDTO.IsReturned));
            parameters.Add(dataAccessHandler.GetSQLParameter("@returnDate", notificationTagIssuedDTO.ReturnDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagProfileId", notificationTagIssuedDTO.NotificationTagProfileId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastSessionAlertTime", notificationTagIssuedDTO.LastSessionAlertTime));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastAlertTimeBeforeExpiry", notificationTagIssuedDTO.LastAlertTimeBeforeExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastAlertTimeOnExpiry", notificationTagIssuedDTO.LastAlertTimeOnExpiry));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", notificationTagIssuedDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", notificationTagIssuedDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private void RefreshNotificationTagIssuedDTO(NotificationTagIssuedDTO notificationTagIssuedDTO, DataTable dt)
        {
            log.LogMethodEntry(notificationTagIssuedDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                notificationTagIssuedDTO.NotificationTagIssuedId = Convert.ToInt32(dt.Rows[0]["NotificationTagIssuedId"]);
                notificationTagIssuedDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                notificationTagIssuedDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                notificationTagIssuedDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                notificationTagIssuedDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                notificationTagIssuedDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                notificationTagIssuedDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Inserts the NotificationTagIssued record to the database
        /// </summary>
        /// <param name="notificationTagIssuedDTO">NotificationTagIssuedDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public NotificationTagIssuedDTO InsertNotificationTagIssued(NotificationTagIssuedDTO notificationTagIssuedDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagIssuedDTO, userId, siteId);
            string insertQuery = @"insert into NotificationTagIssued 
                                                        (
                                                           CardId,
                                                           IssueDate,
                                                           StartDate,
                                                           ExpiryDate,
                                                           TrxId,
                                                           LineId,
                                                           IsReturned,
                                                           ReturnDate,
                                                           NotificationTagProfileId,
                                                           LastSessionAlertTime,
                                                           LastAlertTimeBeforeExpiry,
                                                           LastAlertTimeOnExpiry,
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
                                                           @cardId,
                                                           @issueDate,
                                                           @startDate,
                                                           @expiryDate,
                                                           @trxId,
                                                           @lineId,
                                                           @isReturned,
                                                           @returnDate,
                                                           @notificationTagProfileId,
                                                           @lastSessionAlertTime,
                                                           @lastAlertTimeBeforeExpiry,
                                                           @lastAlertTimeOnExpiry,
                                                           @isActive,
                                                           @createdBy,
                                                           GetDate(),
                                                           @lastUpdatedBy,
                                                           GetDate(),
                                                           @site_id,
                                                           NewId(),
                                                           @masterEntityId
                                                        )SELECT * FROM NotificationTagIssued WHERE NotificationTagIssuedId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(notificationTagIssuedDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagIssuedDTO(notificationTagIssuedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagIssuedDTO);
            return notificationTagIssuedDTO;
        }

        /// <summary>
        /// Updates the NotificationTagIssued record
        /// </summary>
        /// <param name="notificationTagIssuedDTO">notificationTagIssuedDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public NotificationTagIssuedDTO UpdateNotificationTagIssued(NotificationTagIssuedDTO notificationTagIssuedDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagIssuedDTO, userId, siteId);
            string updateQuery = @"update NotificationTagIssued  set 

                                              CardId                    = @cardId,
                                              IssueDate                 = @issueDate,
                                              StartDate                 = @startDate,
                                              ExpiryDate                = @expiryDate,
                                              TrxId                     = @trxId,
                                              LineId                    = @lineId,
                                              IsReturned                = @isReturned,
                                              ReturnDate                = @returnDate,
                                              NotificationTagProfileId  = @notificationTagProfileId,
                                              LastSessionAlertTime      = @lastSessionAlertTime,
                                              LastAlertTimeBeforeExpiry = @lastAlertTimeBeforeExpiry,
                                              LastAlertTimeOnExpiry     = @lastAlertTimeOnExpiry,
                                              IsActive                  = @isActive,
                                              LastUpdatedBy             = @lastUpdatedBy,
                                              LastUpdateDate            = GetDate(),
                                              MasterEntityId            = @masterEntityId
                                       where NotificationTagIssuedId = @notificationTagIssuedId
                                       select * from NotificationTagIssued where  NotificationTagIssuedId = @notificationTagIssuedId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(notificationTagIssuedDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagIssuedDTO(notificationTagIssuedDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagIssuedDTO);
            return notificationTagIssuedDTO;
        }

        /// <summary>
        /// Deletes the NotificationTagIssued record of passed  NotificationTagIssuedId
        /// </summary>
        /// <param name="notificationTagIssuedId">integer type parameter</param>
        public void DeleteNotificationTagIssued(int notificationTagIssuedId)
        {
            log.LogMethodEntry(notificationTagIssuedId);
            string query = @"DELETE  
                             FROM NotificationTagIssued
                             WHERE NotificationTagIssuedId = @notificationTagIssuedId";
            SqlParameter parameter = new SqlParameter("@notificationTagIssuedId", notificationTagIssuedId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the NotificationTagIssued data of passed NotificationTagIssuedId
        /// </summary>
        /// <param name="notificationTagIssuedId">integer type parameter</param>
        /// <returns>Returns NotificationTagIssuedDTO</returns>
        public NotificationTagIssuedDTO GetNotificationTagIssuedDTO(int notificationTagIssuedId)
        {
            log.LogMethodEntry(notificationTagIssuedId);
            NotificationTagIssuedDTO notificationTagIssuedDTO = null;
            string selectQuery = SELECT_QUERY + "  where nti.NotificationTagIssuedId = @notificationTagIssuedId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@notificationTagIssuedId", notificationTagIssuedId);
            DataTable notificationTagIssued = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters, sqlTransaction);
            if (notificationTagIssued.Rows.Count > 0)
            {
                DataRow dataRow = notificationTagIssued.Rows[0];
                notificationTagIssuedDTO = GetNotificationTagIssuedDTO(dataRow);
            }
            log.LogMethodExit(notificationTagIssuedDTO);
            return notificationTagIssuedDTO;
        }

        /// <summary>
        /// Gets the NotificationTagIssuedDTO List for NotificationTagIssued Id List
        /// </summary>
        /// <param name="notificationTagIssuedIdList">integer list parameter</param>
        /// <returns>Returns List of NotificationTagIssuedDTOList</returns>
        public List<NotificationTagIssuedDTO> GetNotificationTagIssuedDTOList(List<int> notificationTagIssuedIdList, bool activeRecords)
        {
            log.LogMethodEntry(notificationTagIssuedIdList);
            List<NotificationTagIssuedDTO> list = new List<NotificationTagIssuedDTO>();
            string query = @"SELECT NotificationTagIssued.*
                            FROM NotificationTagIssued, @notificationTagIssuedIdList List
                            WHERE NotificationTagIssuedId = List.Id ";
            if (activeRecords)
            {
                query += " AND isActive = '1' ";
            }
            DataTable table = dataAccessHandler.BatchSelect(query, "@notificationTagIssuedIdList", notificationTagIssuedIdList, null, sqlTransaction);
            if (table != null && table.Rows.Cast<DataRow>().Any())
            {
                list = table.Rows.Cast<DataRow>().Select(x => GetNotificationTagIssuedDTO(x)).ToList();
            }
            log.LogMethodExit(list);
            return list;
        }


        /// <summary>
        /// Gets the NotificationTagIssuedDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of NotificationTagIssuedDTO matching the search criteria</returns>
        public List<NotificationTagIssuedDTO> GetNotificationTagIssuedDTOList(List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<NotificationTagIssuedDTO> notificationTagIssuedDTOList = new List<NotificationTagIssuedDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectQuery = SELECT_QUERY ;
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.CARDID) ||
                            searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.LINEID) ||
                            searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.TRANSACTIONID) ||
                            searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.NOTIFICATIONTAGISSUEDID) ||
                            searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.MASTER_ENTITY_ID)))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.IS_ACTIVE) ||
                                searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.ISRETURNED))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key == NotificationTagIssuedDTO.SearchByParameters.EXPIRYDATE ||
                                searchParameter.Key == NotificationTagIssuedDTO.SearchByParameters.ISSUEDATE ||
                                searchParameter.Key == NotificationTagIssuedDTO.SearchByParameters.STARTDATE ||
                                searchParameter.Key == NotificationTagIssuedDTO.SearchByParameters.LASTUPDATEDATE_AFTER ||
                                searchParameter.Key == NotificationTagIssuedDTO.SearchByParameters.LASTUPDATE_STARTDATE_AFTER ||
                                searchParameter.Key == NotificationTagIssuedDTO.SearchByParameters.EXPIRY_DATE_NULL_AFTER)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagIssuedDTO.SearchByParameters.GUID))
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

            DataTable notificationTagIssuedDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (notificationTagIssuedDataTable.Rows.Count > 0)
            {
                foreach (DataRow notificationTagIssuedDataRow in notificationTagIssuedDataTable.Rows)
                {
                    NotificationTagIssuedDTO notificationTagIssuedDataObject = GetNotificationTagIssuedDTO(notificationTagIssuedDataRow);
                    notificationTagIssuedDTOList.Add(notificationTagIssuedDataObject);
                }
            }
            log.LogMethodExit(notificationTagIssuedDTOList);
            return notificationTagIssuedDTOList;
        }

    }
}
