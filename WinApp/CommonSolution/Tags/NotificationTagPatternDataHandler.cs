/********************************************************************************************
 * Project Name - Tags
 * Description  - Data Handler of the NotificationTagPattern class
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
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagPatternDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataAccessHandler dataAccessHandler;
        private SqlTransaction sqlTransaction;
        private static readonly Dictionary<NotificationTagPatternDTO.SearchByParameters, string> DBSearchParameters = new Dictionary<NotificationTagPatternDTO.SearchByParameters, string>
               {
                    {NotificationTagPatternDTO.SearchByParameters.NOTIFICATIONTAGPATTERNID, "ntp.NotificationTagPatternId"},
                    {NotificationTagPatternDTO.SearchByParameters.NOTIFICATIONTAGPATTERNNAME, "ntp.NotificationTagPatternName"},
                    {NotificationTagPatternDTO.SearchByParameters.LEDPATTERNNUMBER, "ntp.LEDPatternNumber"},
                    {NotificationTagPatternDTO.SearchByParameters.BUZZERPATTERNNUMBER, "ntp.BuzzerPatternNumber"},
                    {NotificationTagPatternDTO.SearchByParameters.MASTER_ENTITY_ID, "ntp.MasterEntityId"},
                    {NotificationTagPatternDTO.SearchByParameters.SITE_ID,"ntp.site_id"},
                    {NotificationTagPatternDTO.SearchByParameters.IS_ACTIVE,"ntp.IsActive"}
               };
        private const string SELECT_QUERY = @"select * from NotificationTagPattern AS ntp ";
        /// <summary>
        /// Default constructor of NotificationTagPatternDataHandler class
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public NotificationTagPatternDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to NotificationTagPatternDTO class type
        /// </summary>
        /// <param name="notificationTagPatternDataRow">NotificationTagPatternDTO DataRow</param>
        /// <returns>Returns NotificationTagPatternDTO</returns>
        private NotificationTagPatternDTO GetNotificationTagPatternDTO(DataRow notificationTagPatternDataRow)
        {
            log.LogMethodEntry(notificationTagPatternDataRow);
            NotificationTagPatternDTO notificationTagPatternDataObject = new NotificationTagPatternDTO(Convert.ToInt32(notificationTagPatternDataRow["NotificationTagPatternId"]),
                                            notificationTagPatternDataRow["NotificationTagPatternName"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagPatternDataRow["NotificationTagPatternName"]),
                                            notificationTagPatternDataRow["PatternDurationInSeconds"] == DBNull.Value ? Convert.ToInt32(null) : Convert.ToInt32(notificationTagPatternDataRow["PatternDurationInSeconds"]),
                                            notificationTagPatternDataRow["LEDPatternNumber"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagPatternDataRow["LEDPatternNumber"]),
                                            notificationTagPatternDataRow["VibrationPatternNumber"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagPatternDataRow["VibrationPatternNumber"]),
                                            notificationTagPatternDataRow["BuzzerPatternNumber"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagPatternDataRow["BuzzerPatternNumber"]),
                                            notificationTagPatternDataRow["CustomColor"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagPatternDataRow["CustomColor"]),
                                            notificationTagPatternDataRow["CustomAnimation"] == DBNull.Value ? string.Empty : Convert.ToString(notificationTagPatternDataRow["CustomAnimation"]),
                                            notificationTagPatternDataRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(notificationTagPatternDataRow["IsActive"]),
                                            notificationTagPatternDataRow["CreatedBy"].ToString(),
                                            notificationTagPatternDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagPatternDataRow["CreationDate"]),
                                            notificationTagPatternDataRow["LastUpdatedBy"].ToString(),
                                            notificationTagPatternDataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(notificationTagPatternDataRow["LastUpdateDate"]),
                                            notificationTagPatternDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagPatternDataRow["site_id"]),
                                            notificationTagPatternDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(notificationTagPatternDataRow["MasterEntityId"]),
                                            notificationTagPatternDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(notificationTagPatternDataRow["SynchStatus"]),
                                            notificationTagPatternDataRow["Guid"].ToString()
                                            );
            log.LogMethodExit();
            return notificationTagPatternDataObject;
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating NotificationTagPatternDTO Record.
        /// </summary>
        /// <param name="notificationTagPatternDTO">NotificationTagPatternDTO type object</param>
        /// <param name="userId">User performing the operation</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>List of SQL parameters</returns>
        private List<SqlParameter> GetSQLParameters(NotificationTagPatternDTO notificationTagPatternDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagPatternDTO, userId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagPatternId", notificationTagPatternDTO.NotificationTagPatternId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@notificationTagPatternName", notificationTagPatternDTO.NotificationTagPatternName));
            parameters.Add(dataAccessHandler.GetSQLParameter("@patternDurationInSeconds", notificationTagPatternDTO.PatternDurationInSeconds));
            parameters.Add(dataAccessHandler.GetSQLParameter("@ledPatternNumber", notificationTagPatternDTO.LEDPatternNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@vibrationPatternNumber", notificationTagPatternDTO.VibrationPatternNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@buzzerPatternNumber", notificationTagPatternDTO.BuzzerPatternNumber));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customColor", notificationTagPatternDTO.CustomColor));
            parameters.Add(dataAccessHandler.GetSQLParameter("@customAnimation", notificationTagPatternDTO.CustomAnimation));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", notificationTagPatternDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", userId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@site_id", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", notificationTagPatternDTO.MasterEntityId, true));
            log.LogMethodExit(parameters);
            return parameters;
        }
        private void RefreshNotificationTagPatternDTO(NotificationTagPatternDTO notificationTagPatternDTO, DataTable dt)
        {
            log.LogMethodEntry(notificationTagPatternDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                notificationTagPatternDTO.NotificationTagPatternId = Convert.ToInt32(dt.Rows[0]["NotificationTagPatternId"]);
                notificationTagPatternDTO.LastUpdateDate = dataRow["LastUpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdateDate"]);
                notificationTagPatternDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                notificationTagPatternDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                notificationTagPatternDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                notificationTagPatternDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                notificationTagPatternDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Inserts the NotificationTagPattern record to the database
        /// </summary>
        /// <param name="notificationTagPatternDTO">NotificationTagPatternDTO type object</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public NotificationTagPatternDTO InsertNotificationTagPattern(NotificationTagPatternDTO notificationTagPatternDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagPatternDTO, userId, siteId);
            string insertQuery = @"insert into NotificationTagPattern 
                                                        (
                                                           NotificationTagPatternName,
                                                           PatternDurationInSeconds,
                                                           LEDPatternNumber,
                                                           VibrationPatternNumber,
                                                           BuzzerPatternNumber,
                                                           CustomColor,
                                                           CustomAnimation,
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
                                                           @notificationTagPatternName,
                                                           @patternDurationInSeconds,
                                                           @ledPatternNumber,
                                                           @vibrationPatternNumber,
                                                           @buzzerPatternNumber,
                                                           @customColor,
                                                           @customAnimation,
                                                           @isActive,
                                                           @createdBy,
                                                           GetDate(),
                                                           @lastUpdatedBy,
                                                           GetDate(),
                                                           @site_id,
                                                           NewId(),
                                                           @masterEntityId
                                                        )SELECT * FROM NotificationTagPattern WHERE NotificationTagPatternId = scope_identity() ";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertQuery, GetSQLParameters(notificationTagPatternDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagPatternDTO(notificationTagPatternDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(ex, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagPatternDTO);
            return notificationTagPatternDTO;
        }

        /// <summary>
        /// Updates the NotificationTagPatternDTO record
        /// </summary>
        /// <param name="notificationTagPatternDTO">NotificationTagPatternDTO type parameter</param>
        /// <param name="userId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public NotificationTagPatternDTO UpdateNotificationTagPattern(NotificationTagPatternDTO notificationTagPatternDTO, string userId, int siteId)
        {
            log.LogMethodEntry(notificationTagPatternDTO, userId, siteId);
            string updateQuery = @"update NotificationTagPattern  set 
                                              NotificationTagPatternName= @notificationTagPatternName,
                                              PatternDurationInSeconds  = @patternDurationInSeconds,
                                              LEDPatternNumber          = @ledPatternNumber,
                                              VibrationPatternNumber    = @vibrationPatternNumber,
                                              BuzzerPatternNumber       = @buzzerPatternNumber,
                                              CustomColor               = @customColor,
                                              CustomAnimation           = @customAnimation,
                                              IsActive                  = @isActive,
                                              LastUpdatedBy             = @lastUpdatedBy,
                                              LastUpdateDate            = GetDate(),
                                              MasterEntityId            = @masterEntityId
                                       where NotificationTagPatternId   = @notificationTagPatternId
                                       select * from NotificationTagPattern WHERE NotificationTagPatternId  = @notificationTagPatternId";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateQuery, GetSQLParameters(notificationTagPatternDTO, userId, siteId).ToArray(), sqlTransaction);
                RefreshNotificationTagPatternDTO(notificationTagPatternDTO, dt);
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw new Exception(ex.Message, ex);
            }
            log.LogMethodExit(notificationTagPatternDTO);
            return notificationTagPatternDTO;
        }

        /// <summary>
        /// Deletes the NotificationTagPattern record of passed  notificationTagPatternId
        /// </summary>
        /// <param name="notificationTagPatternId">integer type parameter</param>
        public void Delete(int notificationTagPatternId)
        {
            log.LogMethodEntry(notificationTagPatternId);
            string query = @"DELETE  
                             FROM NotificationTagPattern
                             WHERE NotificationTagPatternId = @notificationTagPatternId";
            SqlParameter parameter = new SqlParameter("@notificationTagPatternId", notificationTagPatternId);
            dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the NotificationTagPattern data of passed notificationTagPatternId
        /// </summary>
        /// <param name="notificationTagPatternId">integer type parameter</param>
        /// <returns>Returns NotificationTagPatternDTO</returns>
        public NotificationTagPatternDTO GetNotificationTagPatternDTO(int notificationTagPatternId)
        {
            log.LogMethodEntry(notificationTagPatternId);
            NotificationTagPatternDTO notificationTagPatternDTO = null;
            string selectQuery = SELECT_QUERY + " where ntp.NotificationTagPatternId = @notificationTagPatternId";
            SqlParameter[] selectParameters = new SqlParameter[1];
            selectParameters[0] = new SqlParameter("@notificationTagPatternId", notificationTagPatternId);
            DataTable notificationTagPattern = dataAccessHandler.executeSelectQuery(selectQuery, selectParameters, sqlTransaction);
            if (notificationTagPattern.Rows.Count > 0)
            {
                DataRow dataRow = notificationTagPattern.Rows[0];
                notificationTagPatternDTO = GetNotificationTagPatternDTO(dataRow);
            }
            log.LogMethodExit(notificationTagPatternDTO);
            return notificationTagPatternDTO;
        }

        /// <summary>
        /// Gets the NotificationTagPatternDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of NotificationTagPatternDTO matching the search criteria</returns>
        public List<NotificationTagPatternDTO> GetNotificationTagPatternDTOList(List<KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<NotificationTagPatternDTO> notificationTagPatternDTOList = new List<NotificationTagPatternDTO>();
            List<SqlParameter> parameters = new List<SqlParameter>();

            string selectQuery = SELECT_QUERY;
            if (searchParameters != null)
            {
                string joiner;
                StringBuilder query = new StringBuilder(" where ");
                foreach (KeyValuePair<NotificationTagPatternDTO.SearchByParameters, string> searchParameter in searchParameters)
                {
                    joiner = count == 0 ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if ((searchParameter.Key.Equals(NotificationTagPatternDTO.SearchByParameters.BUZZERPATTERNNUMBER) ||
                            searchParameter.Key.Equals(NotificationTagPatternDTO.SearchByParameters.LEDPATTERNNUMBER) ||
                            searchParameter.Key.Equals(NotificationTagPatternDTO.SearchByParameters.NOTIFICATIONTAGPATTERNID)))

                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagPatternDTO.SearchByParameters.SITE_ID))
                        {
                            query.Append(joiner + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagPatternDTO.SearchByParameters.IS_ACTIVE))
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",1)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), (searchParameter.Value == "1" || searchParameter.Value == "Y" ? true : false)));
                        }
                        else if (searchParameter.Key.Equals(NotificationTagPatternDTO.SearchByParameters.NOTIFICATIONTAGPATTERNNAME))
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

            DataTable notificationTagPatternDataTable = dataAccessHandler.executeSelectQuery(selectQuery, parameters.ToArray(), sqlTransaction);
            if (notificationTagPatternDataTable.Rows.Count > 0)
            {
                foreach (DataRow notificationTagPatternDataRow in notificationTagPatternDataTable.Rows)
                {
                    NotificationTagPatternDTO notificationTagPatternDataObject = GetNotificationTagPatternDTO(notificationTagPatternDataRow);
                    notificationTagPatternDTOList.Add(notificationTagPatternDataObject);
                }
            }
            log.LogMethodExit(notificationTagPatternDTOList);
            return notificationTagPatternDTOList;
        }

    }
}
