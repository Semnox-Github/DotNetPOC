/********************************************************************************************
 * Project Name - Tags
 * Description  - Data Handler of the NotificationTag class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.90        20-jul-2020   Mushahid Faizan         Created.
 *2.130.0     31-Aug-2021   Guru S A                Enable Serial number based card load
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagsDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<NotificationTagsDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<NotificationTagsDTO.SearchByParameters, string>
               {
                    {NotificationTagsDTO.SearchByParameters.NOTIFICATIONTAGID, "nt.NotificationTagId"},
                    {NotificationTagsDTO.SearchByParameters.ISINSTORAGE, "nt.IsInStorage"},
                    {NotificationTagsDTO.SearchByParameters.MARKED_FOR_STORAGE, "nt.MarkedForStorage"},
                    {NotificationTagsDTO.SearchByParameters.TAGNUMBER, "nt.TagNumber"},
                    {NotificationTagsDTO.SearchByParameters.TAGNOTIFICATIONSTATUS, "nt.TagNotificationStatus"},
                    {NotificationTagsDTO.SearchByParameters.TAG_NOTIFICATION_STATUS_LIST, "nt.TagNotificationStatus"},
                    {NotificationTagsDTO.SearchByParameters.SITE_ID,"nt.site_id"},
                    {NotificationTagsDTO.SearchByParameters.DEFAULT_CHANNEL,"nt.DefaultChannel"},
                    {NotificationTagsDTO.SearchByParameters.IS_ACTIVE,"nt.IsActive"}
               };
        private const string SELECT_QUERY = @"select * from NotificationTags AS nt ";
        /// <summary>
        /// Default constructor of NotificationTagsDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public NotificationTagsDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to NotificationTagsDTO class type
        /// </summary>
        /// <param name="notificationTagsDataRow">NotificationTagsDTO DataRow</param>
        /// <returns>Returns NotificationTagsDTO</returns>
        private NotificationTagsDTO GetNotificationTagsDTO(DataRow notificationTagsDataRow)
        {
            log.LogMethodEntry(notificationTagsDataRow);
            NotificationTagsDTO notificationTagsDataObject = new NotificationTagsDTO(Convert.ToInt32(notificationTagsDataRow["NotificationTagId"]),
                                            notificationTagsDataRow["TagNumber"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagsDataRow["TagNumber"]),
                                            notificationTagsDataRow["TagNotificationStatus"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagsDataRow["TagNotificationStatus"]),
                                            notificationTagsDataRow["MarkedForStorage"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagsDataRow["MarkedForStorage"]),
                                            notificationTagsDataRow["LastStorageMarkedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagsDataRow["LastStorageMarkedDate"]),
                                            notificationTagsDataRow["IsInStorage"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagsDataRow["IsInStorage"]),
                                            notificationTagsDataRow["Remarks"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagsDataRow["Remarks"]),
                                            notificationTagsDataRow["DefaultChannel"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagsDataRow["DefaultChannel"]),
                                            notificationTagsDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagsDataRow["IsActive"]),
                                            notificationTagsDataRow["CreatedBy"].ToString(),
                                            notificationTagsDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagsDataRow["CreationDate"]),
                                            notificationTagsDataRow["LastUpdatedBy"].ToString(),
                                            notificationTagsDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagsDataRow["LastUpdateDate"]),
                                            notificationTagsDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagsDataRow["site_id"]),
                                            notificationTagsDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagsDataRow["MasterEntityId"]),
                                            notificationTagsDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(notificationTagsDataRow["SynchStatus"]),
                                            notificationTagsDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit();
            return notificationTagsDataObject;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating NotificationTagsDTO Record.
        /// </summary>
        /// <param name="notificationTagsDTO">NotificationTagsDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(NotificationTagsDTO notificationTagsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagsDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagId", notificationTagsDTO.NotificationTagId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tagNumber", notificationTagsDTO.TagNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@tagNotificationStatus", notificationTagsDTO.TagNotificationStatus));
            parameters.Add(dataAccessHandler.GetSQLParameter("@markedForStorage", notificationTagsDTO.MarkedForStorage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastStorageMarkedDate", notificationTagsDTO.LastStorageMarkedDate));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isInStorage", notificationTagsDTO.IsInStorage));
            parameters.Add(dataAccessHandler.GetSQLParameter("@remarks", notificationTagsDTO.Remarks));
            parameters.Add(dataAccessHandler.GetSQLParameter("@defaultChannel", notificationTagsDTO.DefaultChannel));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", notificationTagsDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", notificationTagsDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private void RefreshNotificationTagsDTO(NotificationTagsDTO notificationTagsDTO, DataTable dt)
        {
            log.LogMethodEntry(notificationTagsDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                notificationTagsDTO.NotificationTagId = Convert.ToInt32(dt.Rows[0]["NotificationTagId"]);
                notificationTagsDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                notificationTagsDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                notificationTagsDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                notificationTagsDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                notificationTagsDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                notificationTagsDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the NotificationTags record to the database
        /// </summary>
        /// <param name="notificationTagsDTO">NotificationTagsDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public NotificationTagsDTO InsertNotificationTags(NotificationTagsDTO notificationTagsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagsDTO, userId, siteId);
            string insertQuery = @"insert into NotificationTags 
                                                        (
                                                           TagNumber,
                                                           TagNotificationStatus,
                                                           MarkedForStorage,
                                                           LastStorageMarkedDate,
                                                           IsInStorage,
                                                           Remarks,
                                                           DefaultChannel,
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
                                                           @tagNumber,
                                                           @tagNotificationStatus,
                                                           @markedForStorage,
                                                           @lastStorageMarkedDate,
                                                           @isInStorage,
                                                           @remarks,
                                                           @defaultChannel,
                                                           @isActive,
                                                           @createdBy,
                                                           GetDate(),
                                                           @lastUpdatedBy,
                                                           GetDate(),
                                                           @site_id,
                                                           NewId(),
                                                           @masterEntityId
                                                        )SELECT * FROM NotificationTags WHERE NotificationTagId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(notificationTagsDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagsDTO(notificationTagsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagsDTO);
            return notificationTagsDTO;
        }

        /// <summary>
        /// Updates the NotificationTagsDTO record
        /// </summary>
        /// <param name="notificationTagsDTO">NotificationTagsDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public NotificationTagsDTO UpdateNotificationTags(NotificationTagsDTO notificationTagsDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagsDTO, userId, siteId);
            string updateQuery = @"update NotificationTags  set 

                                              TagNumber             = @tagNumber,
                                              TagNotificationStatus = @tagNotificationStatus,
                                              MarkedForStorage      = @markedForStorage,
                                              LastStorageMarkedDate = @lastStorageMarkedDate,
                                              IsInStorage           = @isInStorage,
                                              Remarks               = @remarks,
                                              DefaultChannel        = @defaultChannel,
                                              IsActive                  = @isActive,
                                              LastUpdatedBy             = @lastUpdatedBy,
                                              LastUpdateDate            = GetDate(),
                                              MasterEntityId            = @masterEntityId
                                       where NotificationTagId   = @notificationTagId
                                       select * from NotificationTags WHERE NotificationTagId  = @notificationTagId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(notificationTagsDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagsDTO(notificationTagsDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagsDTO);
            return notificationTagsDTO;
        }

        internal List<string> GetNotificationTagColumns()
        {
            log.LogMethodEntry();
            List<string> result = new List<string>();
            string query = @"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'NotificationTags' ORDER BY ORDINAL_POSITION";
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, null, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dataRow = dataTable.Rows[i];
                    result.Add(dataRow[0].ToString());
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Deletes the NotificationTags record of passed  notificationTagId
        /// </summary>
        /// <param name="notificationTagsId">integer type parameter</param>
        public void Delete(int notificationTagsId)
        {
            log.LogMethodEntry(notificationTagsId);
            string query = @"DELETE  
                             FROM NotificationTags
                             WHERE NotificationTagId = @notificationTagId";
            SqlParameter parameter = new SqlParameter("@notificationTagId", notificationTagsId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the NotificationTags data of passed notificationTagsId
        /// </summary>
        /// <param name="notificationTagsId">integer type parameter</param>
        /// <returns>Returns NotificationTagsDTO</returns>
        public NotificationTagsDTO GetNotificationTagsDTO(int notificationTagsId)
        {
            log.LogMethodEntry(notificationTagsId);
            NotificationTagsDTO notificationTagsDTO = null;
            string selectQuery = SELECT_QUERY + " where nt.NotificationTagId = @notificationTagId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@notificationTagId", notificationTagsId);
            DataTable notificationTags = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters, sqlTransaction);
            if (notificationTags.Rows.Count > 0)
            {
                DataRow dataRow = notificationTags.Rows[0];
                notificationTagsDTO = GetNotificationTagsDTO(dataRow);
            }
            log.LogMethodExit(notificationTagsDTO);
            return notificationTagsDTO;
        }

        /// <summary>
        /// Gets the NotificationTagsDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of NotificationTagsDTO matching the search criteria</returns>
        internal List<NotificationTagsDTO> GetNotificationTagsDTOList(List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<NotificationTagsDTO> notificationTagsDTOList = new List<NotificationTagsDTO>();
            KeyValuePair<string, List<SqlParameter>> qryAndParameterList = BuildWhereClause(searchParameters, SELECT_QUERY);
            log.Debug(qryAndParameterList);
            if (string.IsNullOrWhiteSpace(qryAndParameterList.Key) == false)
            {
                DataTable notificationTagsDataTable = dataAccessHandler.executeSelectQuery(qryAndParameterList.Key, qryAndParameterList.Value.ToArray(), sqlTransaction);
                if (notificationTagsDataTable.Rows.Count > 0)
                {
                    foreach (DataRow notificationTagsDataRow in notificationTagsDataTable.Rows)
                    {
                        NotificationTagsDTO notificationTagsDataObject = GetNotificationTagsDTO(notificationTagsDataRow);
                        notificationTagsDTOList.Add(notificationTagsDataObject);
                    }
                }
            }
            log.LogMethodExit(notificationTagsDTOList);
            return notificationTagsDTOList;
        }

        private KeyValuePair<string, List<SqlParameter>> BuildWhereClause(List<KeyValuePair<NotificationTagsDTO.SearchByParameters,
                                                                         string>> searchParameters, string selectQuery)
        {
            log.LogMethodEntry(searchParameters, selectQuery);
            int count = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            KeyValuePair<string, List<SqlParameter>> outPutValue = new KeyValuePair<string, List<SqlParameter>>(string.Empty,new List<SqlParameter>());
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<NotificationTagsDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.NOTIFICATIONTAGID)))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.TAGNUMBER) ||
                                 (searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.DEFAULT_CHANNEL)) ||
                                 (searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.TAGNOTIFICATIONSTATUS)))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.ISINSTORAGE) ||
                            searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.MARKED_FOR_STORAGE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ", 1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagsDTO.SearchByParameters.TAG_NOTIFICATION_STATUS_LIST))
                        {
                            query.Append(joiner + "( " + DBSearchParameters[searchParameter.Key] + ") IN (" + dataAccessHandler.GetParameterName(searchParameter.Key) + " )");
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
                {
                    outPutValue = new KeyValuePair<string, List<SqlParameter>>(selectQuery + query, parameters);
                }
            }
            log.LogMethodExit(outPutValue);
            return outPutValue;
        }

        internal List<NotificationTagsDTO> GetNotificationTagsDTOList(List<string> cardNumberList,
            List<KeyValuePair<NotificationTagsDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(cardNumberList, searchParameters);
            List<NotificationTagsDTO> notificationTagsDTOList = new List<NotificationTagsDTO>();
            string query = ((cardNumberList!= null && cardNumberList.Any()) ? SELECT_QUERY + @" inner join @CardNumberList List on nt.TagNumber = List.Value "
                                                                            : SELECT_QUERY);
            KeyValuePair<string, List<SqlParameter>> qryAndParameterList = BuildWhereClause(searchParameters, query);
            List<SqlParameter> parameters = null;
            log.Debug(qryAndParameterList);
            if (string.IsNullOrWhiteSpace(qryAndParameterList.Key) == false)
            {
                query = qryAndParameterList.Key;
                parameters = qryAndParameterList.Value;
            }
            DataTable notificationTagsDataTable = dataAccessHandler.BatchSelect(query, "@CardNumberList", cardNumberList, parameters.ToArray(), sqlTransaction);
            //DataTable notificationTagsDataTable = dataAccessHandler.executeSelectQuery(qryAndParameterList.Key, parameters.ToArray(), sqlTransaction);
            if (notificationTagsDataTable.Rows.Count > 0)
            {
                foreach (DataRow notificationTagsDataRow in notificationTagsDataTable.Rows)
                {
                    NotificationTagsDTO notificationTagsDataObject = GetNotificationTagsDTO(notificationTagsDataRow);
                    notificationTagsDTOList.Add(notificationTagsDataObject);
                }
            }
            log.LogMethodExit(notificationTagsDTOList);
            return notificationTagsDTOList;
        }
    }
}
