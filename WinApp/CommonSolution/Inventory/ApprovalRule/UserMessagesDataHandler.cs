/********************************************************************************************
 * Project Name - User Messages Data Handler
 * Description  - Data handler of the user messages class
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
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// User Messages Data Handler - Handles insert, update and select of User Messages objects
    /// </summary>
    public class UserMessagesDataHandler
    {
        private readonly SqlTransaction sqlTransaction;
        private const string SELECT_QUERY = @"SELECT * FROM UserMessages AS um ";
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<UserMessagesDTO.SearchByUserMessagesParameters, string> DBSearchParameters = new Dictionary<UserMessagesDTO.SearchByUserMessagesParameters, string>
            {
                {UserMessagesDTO.SearchByUserMessagesParameters.MESSAGE_ID, "um.MessageID"},
                {UserMessagesDTO.SearchByUserMessagesParameters.APPROVAL_RULE_ID, "um.ApprovalRuleID"},
                {UserMessagesDTO.SearchByUserMessagesParameters.MESSAGE_TYPE, "um.MessageType"},
                {UserMessagesDTO.SearchByUserMessagesParameters.ROLE_ID, "um.role_id"},
                {UserMessagesDTO.SearchByUserMessagesParameters.USER_ID, "um.user_id"},
                {UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "um.ModuleType"},
                {UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, "um.ObjectGUID"},
                {UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, "um.ObjectType"},
                {UserMessagesDTO.SearchByUserMessagesParameters.ACTED_BY_USER, "um.ActedByUser"},
                {UserMessagesDTO.SearchByUserMessagesParameters.STATUS, "um.Status"},
                {UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "um.IsActive"},
                {UserMessagesDTO.SearchByUserMessagesParameters.LAST_UPDATED_DATE_TILL, "um.LastUpdatedDate"},
                {UserMessagesDTO.SearchByUserMessagesParameters.MASTER_ENTITY_ID,"um.MasterEntityId"},
                {UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID, "um.site_id"}
            };
        private DataAccessHandler dataAccessHandler;

        /// <summary>
        /// Default constructor of UserMessagesDataHandler class
        /// </summary>
        public UserMessagesDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            this.sqlTransaction = sqlTransaction;
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Builds the SQL Parameter list used for inserting and updating InventoryReceipt Record.
        /// </summary>
        /// <param name="UserMessagesDTO">UserMessagesDTO object is passed as Parameter</param>
        /// <param name="loginId">login Id</param>
        /// <param name="siteId">site Id</param>
        /// <returns>Returns the list of SQL parameter</returns>
        private List<SqlParameter> GetSQLParameters(UserMessagesDTO userMessagesDTO, string loginId, int siteId)
        {
            log.LogMethodEntry(userMessagesDTO, loginId, siteId);
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(dataAccessHandler.GetSQLParameter("@messageID", userMessagesDTO.MessageId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@approvalRuleID", userMessagesDTO.ApprovalRuleID, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@roleId", userMessagesDTO.RoleId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@actedByUser", userMessagesDTO.ActedByUser, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@messageType", string.IsNullOrEmpty(userMessagesDTO.MessageType) ? DBNull.Value:(object) userMessagesDTO.MessageType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@message", string.IsNullOrEmpty(userMessagesDTO.Message) ? DBNull.Value:(object) userMessagesDTO.Message));
            parameters.Add(dataAccessHandler.GetSQLParameter("@moduleType", string.IsNullOrEmpty(userMessagesDTO.ModuleType) ? DBNull.Value:(object) userMessagesDTO.ModuleType));
            parameters.Add(dataAccessHandler.GetSQLParameter("@objectType", string.IsNullOrEmpty(userMessagesDTO.ObjectType) ? DBNull.Value:(object) userMessagesDTO.ObjectType));
            parameters.Add(new SqlParameter("@objectGUID", string.IsNullOrEmpty(userMessagesDTO.ObjectGUID) ? DBNull.Value:(object) userMessagesDTO.ObjectGUID));
            parameters.Add(dataAccessHandler.GetSQLParameter("@status", string.IsNullOrEmpty(userMessagesDTO.Status) ? DBNull.Value:(object) userMessagesDTO.Status));
            parameters.Add(dataAccessHandler.GetSQLParameter("@userId", userMessagesDTO.UserId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@level", userMessagesDTO.Level, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@isActive", userMessagesDTO.IsActive));
            parameters.Add(dataAccessHandler.GetSQLParameter("@masterEntityId", userMessagesDTO.MasterEntityId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@siteId", siteId, true));
            parameters.Add(dataAccessHandler.GetSQLParameter("@createdBy", loginId));
            parameters.Add(dataAccessHandler.GetSQLParameter("@lastUpdatedBy", loginId));
            log.LogMethodExit(parameters);
            return parameters;
        }

        /// <summary>
        /// Inserts the user messages record to the database
        /// </summary>
        /// <param name="userMessages">UserMessagesDTO type object</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns inserted record id</returns>
        public UserMessagesDTO InsertUserMessages(UserMessagesDTO userMessages, string loginId, int siteId)
        {
            log.LogMethodEntry(userMessages, loginId, siteId, sqlTransaction);
            string insertUserMessagesQuery = @"insert into UserMessages 
                                                        (
                                                        ApprovalRuleID,
                                                        MessageType,
                                                        Message,
                                                        role_id,
                                                        user_id,
                                                        Level,
                                                        ModuleType,
                                                        ObjectType,
                                                        ObjectGUID,
                                                        Status,
                                                        ActedByUser,
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
                                                         @approvalRuleID,
                                                         @messageType,
                                                         @message,
                                                         @roleId,
                                                         @userId,
                                                         @level,
                                                         @moduleType,
                                                         @objectType,
                                                         @objectGUID,
                                                         @status,
                                                         @actedByUser,
                                                         @masterEntityId,
                                                         @isActive,
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         @createdBy,
                                                         Getdate(),                                                         
                                                         NEWID(),
                                                         @siteid
                                                        ) SELECT * FROM UserMessages WHERE MessageID = scope_identity()";


            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(insertUserMessagesQuery, GetSQLParameters(userMessages, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserMessagesDTO(userMessages, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while inserting userMessages", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userMessages);
            return userMessages;
        }

        /// <summary>
        /// Updates the user messages record
        /// </summary>
        /// <param name="userMessages">UserMessagesDTO type parameter</param>
        /// <param name="loginId">User inserting the record</param>
        /// <param name="siteId">Site to which the record belongs</param>
        /// <returns>Returns the count of updated rows</returns>
        public UserMessagesDTO UpdateUserMessages(UserMessagesDTO userMessages, string loginId, int siteId)
        {
            log.LogMethodEntry(userMessages, loginId, siteId, sqlTransaction);
            string updateUserMessagesQuery = @"update UserMessages 
                                         set ApprovalRuleID = @approvalRuleID,
                                             MessageType = @messageType,
                                             Message = @message,
                                             role_id = @roleId,
                                             user_id = @userId,
                                             Level = @level,
                                             ModuleType = @moduleType,
                                             ObjectType = @objectType,
                                             ObjectGUID = @objectGUID,
                                             Status = @status,
                                             ActedByUser = @actedByUser,
                                             MasterEntityId=@masterEntityId,
                                             IsActive = @isActive, 
                                             LastUpdatedBy = @lastUpdatedBy, 
                                             LastupdatedDate = Getdate()
                                             --site_id=@siteid                                            
                                       where MessageID = @messageID
                             SELECT * FROM UserMessages WHERE MessageID = @messageID";
            try
            {
                DataTable dt = dataAccessHandler.executeSelectQuery(updateUserMessagesQuery, GetSQLParameters(userMessages, loginId, siteId).ToArray(), sqlTransaction);
                RefreshUserMessagesDTO(userMessages, dt);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while updating userMessages", ex);
                log.LogMethodExit(null, "Throwing exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit(userMessages);
            return userMessages;
        }

        /// <summary>
        /// Delete the record from the userMessages database based on MessageID
        /// </summary>
        /// <param name="MessageID">MessageID</param>
        /// <returns>return the int </returns>
        internal int Delete(int messageID)
        {
            log.LogMethodEntry(messageID);
            string query = @"DELETE  
                             FROM UserMessages
                             WHERE UserMessages.MessageID = @messageID";
            SqlParameter parameter = new SqlParameter("@messageID", messageID);
            int id = dataAccessHandler.executeUpdateQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            log.LogMethodExit(id);
            return id;
        }

        /// <summary>
        /// Used to update the current DTO with auto generated Database column values.
        /// So that after insert / update ,DTO can be accessed anywhere with full data captured.
        /// </summary>
        /// <param name="UserMessagesDTO">UserMessagesDTO object passed as a parameter.</param>
        /// <param name="dt">dt is an object of DataTable </param>
        private void RefreshUserMessagesDTO(UserMessagesDTO userMessagesDTO, DataTable dt)
        {
            log.LogMethodEntry(userMessagesDTO, dt);
            if (dt.Rows.Count > 0)
            {
                DataRow dataRow = dt.Rows[0];
                userMessagesDTO.ApprovalRuleID = Convert.ToInt32(dt.Rows[0]["MessageID"]);
                userMessagesDTO.LastUpdatedDate = dataRow["LastUpdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["LastUpdatedDate"]);
                userMessagesDTO.CreationDate = dataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(dataRow["CreationDate"]);
                userMessagesDTO.Guid = dataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["Guid"]);
                userMessagesDTO.CreatedBy = dataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["CreatedBy"]);
                userMessagesDTO.LastUpdatedBy = dataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(dataRow["LastUpdatedBy"]);
                userMessagesDTO.SiteId = dataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(dataRow["site_id"]);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Converts the Data row object to UserMessagesDTO class type
        /// </summary>
        /// <param name="userMessagesDataRow">UserMessages DataRow</param>
        /// <returns>Returns UserMessages</returns>
        private UserMessagesDTO GetUserMessagesDTO(DataRow userMessagesDataRow)
        {
            log.LogMethodEntry(userMessagesDataRow);
            UserMessagesDTO userMessagesDataObject = new UserMessagesDTO(Convert.ToInt32(userMessagesDataRow["MessageID"]),
                            userMessagesDataRow["ApprovalRuleID"] == DBNull.Value ? -1 : Convert.ToInt32(userMessagesDataRow["ApprovalRuleID"]),
                            userMessagesDataRow["MessageType"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["MessageType"]),
                            userMessagesDataRow["Message"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["Message"]),
                            userMessagesDataRow["role_id"] == DBNull.Value ? -1 : Convert.ToInt32(userMessagesDataRow["role_id"]),
                            userMessagesDataRow["user_id"] == DBNull.Value ? -1 : Convert.ToInt32(userMessagesDataRow["user_id"]),
                            userMessagesDataRow["Level"] == DBNull.Value ? 0 : Convert.ToInt32(userMessagesDataRow["Level"]),
                            userMessagesDataRow["ModuleType"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["ModuleType"]),
                            userMessagesDataRow["ObjectType"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["ObjectType"]),
                            userMessagesDataRow["ObjectGUID"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["ObjectGUID"]),
                            userMessagesDataRow["Status"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["Status"]),
                            userMessagesDataRow["ActedByUser"] == DBNull.Value ? -1 : Convert.ToInt32(userMessagesDataRow["ActedByUser"]),
                            userMessagesDataRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(userMessagesDataRow["MasterEntityId"]),
                            userMessagesDataRow["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(userMessagesDataRow["IsActive"]),
                            userMessagesDataRow["CreatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["CreatedBy"]),
                            userMessagesDataRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userMessagesDataRow["CreationDate"]),
                            userMessagesDataRow["LastUpdatedBy"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["LastUpdatedBy"]),
                            userMessagesDataRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(userMessagesDataRow["LastupdatedDate"]),
                            userMessagesDataRow["Guid"] == DBNull.Value ? string.Empty : Convert.ToString(userMessagesDataRow["Guid"]),
                            userMessagesDataRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(userMessagesDataRow["site_id"]),
                            userMessagesDataRow["SynchStatus"] == DBNull.Value ? false : Convert.ToBoolean(userMessagesDataRow["SynchStatus"])
                            );
            log.LogMethodExit(userMessagesDataObject);
            return userMessagesDataObject;
        }

        /// <summary>
        /// Gets the user messages data of passed user messages Id
        /// </summary>
        /// <param name="userMessagesId">integer type parameter</param>
        /// <returns>Returns UserMessagesDTO</returns>
        public UserMessagesDTO GetUserMessages(int userMessagesId)
        {
            log.LogMethodEntry(userMessagesId);
            UserMessagesDTO result = null;
            string query = SELECT_QUERY + @" WHERE um.MessageID= @messageID";
            SqlParameter parameter = new SqlParameter("@messageID", userMessagesId);
            DataTable dataTable = dataAccessHandler.executeSelectQuery(query, new SqlParameter[] { parameter }, sqlTransaction);
            if (dataTable.Rows.Count > 0)
            {
                result = GetUserMessagesDTO(dataTable.Rows[0]);
            }
            log.LogMethodExit(result);
            return result;
        }

        public List<UserMessagesDTO> GetPendingApprovalUserMessage(string moduleType, string objectType, string objectGUID, int roleId,int userId, int siteId)
        {
            log.LogMethodEntry(moduleType, objectType, objectGUID, roleId, userId, siteId);
            string selectUserMessagesQuery = @"select * from UserMessages 
                                                          where (role_id = @roleId or  @roleId=-1) and (user_id = @userId or @userId = -1)
		                                                        and ModuleType = @moduleType and  ObjectType = @objectType and (site_id = @siteId or @siteId = -1)
				                                                and ObjectGUID = @objectGUID and Isnull(Status,'" + UserMessagesDTO.UserMessagesStatus.PENDING.ToString()+"') = '"+UserMessagesDTO.UserMessagesStatus.PENDING.ToString()+"' and IsActive = 1 order by level ";
            SqlParameter[] selectUserMessagesParameters = new SqlParameter[6];            
            selectUserMessagesParameters[0] = new SqlParameter("@roleId", roleId);
            selectUserMessagesParameters[1] = new SqlParameter("@moduleType", moduleType);
            selectUserMessagesParameters[2] = new SqlParameter("@objectType", objectType);
            selectUserMessagesParameters[3] = new SqlParameter("@objectGUID", objectGUID);
            selectUserMessagesParameters[4] = new SqlParameter("@userId", userId);
            selectUserMessagesParameters[5] = new SqlParameter("@siteId", siteId);
            DataTable userMessagesData = dataAccessHandler.executeSelectQuery(selectUserMessagesQuery, selectUserMessagesParameters, sqlTransaction);
            if (userMessagesData.Rows.Count > 0)
            {
                List<UserMessagesDTO> userMessagesList = new List<UserMessagesDTO>();
                foreach (DataRow userMessagesDataRow in userMessagesData.Rows)
                {
                    UserMessagesDTO userMessagesDataObject = GetUserMessagesDTO(userMessagesDataRow);
                    userMessagesList.Add(userMessagesDataObject);
                }
                log.LogMethodExit(userMessagesList);
                return userMessagesList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        public List<UserMessagesDTO> GetHistoryUserMessage(int roleId, int userId, string moduleType, string loginId, int siteId, string status)
        {
            log.LogMethodEntry(roleId, userId, moduleType, loginId, siteId);
            string selectUserMessagesQuery = @"select * from UserMessages um where (um.Site_id = @siteId or @siteId=-1) and (select max(level) from UserMessages umh where um.ObjectGUID=umh.ObjectGUID  and um.ObjectType = umh.ObjectType and ((status='" + status + "'and ActedByUser=@userId) or 'NONE'= '" + status + "'))=level and "+
                                                  "ModuleType = @moduleType and  (status='" + status + "' or 'NONE'= '" + status + "') and "
                                                   + "exists(select * from UserMessages ums where (role_id = @roleId or ActedByUser = @userId or CreatedBy = @loginid) and status<>'" + UserMessagesDTO.UserMessagesStatus.PENDING.ToString() + "' and ums.IsActive = 1  and um.ObjectGUID=ums.ObjectGUID  and um.ObjectType = ums.ObjectType) order by LastupdatedDate desc";
            SqlParameter[] selectUserMessagesParameters = new SqlParameter[5];
            selectUserMessagesParameters[0] = new SqlParameter("@userId", userId);
            selectUserMessagesParameters[1] = new SqlParameter("@moduleType", moduleType);
            selectUserMessagesParameters[2] = new SqlParameter("@loginId", loginId);
            selectUserMessagesParameters[3] = new SqlParameter("@siteId", siteId);
            selectUserMessagesParameters[4] = new SqlParameter("@roleId", roleId);
            DataTable userMessagesData = dataAccessHandler.executeSelectQuery(selectUserMessagesQuery, selectUserMessagesParameters, sqlTransaction);
            if (userMessagesData.Rows.Count > 0)
            {
                List<UserMessagesDTO> userMessagesList = new List<UserMessagesDTO>();
                foreach (DataRow userMessagesDataRow in userMessagesData.Rows)
                {
                    UserMessagesDTO userMessagesDataObject = GetUserMessagesDTO(userMessagesDataRow);
                    userMessagesList.Add(userMessagesDataObject);
                }
                log.LogMethodExit(userMessagesList);
                return userMessagesList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        public List<UserMessagesDTO> GetApprovedUserMessage(int approvalRuleId,int roleId, int userId, string moduleType, string objectType, string objectGUID, int siteId)
        {
            log.LogMethodEntry(approvalRuleId, roleId, userId, moduleType, objectType, objectGUID, siteId);
            string selectUserMessagesQuery = @"select * from UserMessages 
                                                          where ApprovalRuleID = @approvalRuleID and (user_id = @userId or @userId  = -1) and (role_id = @roleId or  @roleId=-1) and (Site_id = @siteId or  @siteId=-1)
		                                                        and ModuleType = @moduleType and  ObjectType = @objectType 
				                                                and ObjectGUID = @objectGUID and Status = '" + UserMessagesDTO.UserMessagesStatus.APPROVED + "' and IsActive = 1 order by level desc";
            SqlParameter[] selectUserMessagesParameters = new SqlParameter[7];
            selectUserMessagesParameters[0] = new SqlParameter("@approvalRuleID", approvalRuleId);
            selectUserMessagesParameters[1] = new SqlParameter("@userId", userId);
            selectUserMessagesParameters[2] = new SqlParameter("@moduleType", moduleType);
            selectUserMessagesParameters[3] = new SqlParameter("@objectType", objectType);
            selectUserMessagesParameters[4] = new SqlParameter("@objectGUID", objectGUID);
            selectUserMessagesParameters[5] = new SqlParameter("@siteId", siteId);
            selectUserMessagesParameters[6] = new SqlParameter("@roleId", roleId);
            DataTable userMessagesData = dataAccessHandler.executeSelectQuery(selectUserMessagesQuery, selectUserMessagesParameters, sqlTransaction);
            if (userMessagesData.Rows.Count > 0)
            {
                List<UserMessagesDTO> userMessagesList = new List<UserMessagesDTO>();
                foreach (DataRow userMessagesDataRow in userMessagesData.Rows)
                {
                    UserMessagesDTO userMessagesDataObject = GetUserMessagesDTO(userMessagesDataRow);
                    userMessagesList.Add(userMessagesDataObject);
                }


                log.Debug("Ends-GetApprovedUserMessage(approvalRuleId, roleId , userId, moduleType, objectType, guid, siteId) Method by returning userMessagesList.");
                return userMessagesList;
            }
            else
            {
                log.Debug("Ends-GetApprovedUserMessage(approvalRuleId, roleId, moduleType, objectType, guid, siteId) Method by returning null.");
                return null;
            }
        }

        public List<UserMessagesDTO> GetAllMyPendingApprovalUserMessage(int roleId, string moduleType, int siteId)
        {
            log.LogMethodEntry(roleId);
            string selectUserMessagesQuery = @"select * from(
                                                   select * from UserMessages um 
                                                        where role_id = @roleId and ModuleType = @moduleType 
                                                              and (Site_id=@siteId or @siteId=-1) and um.Status= @status
                                                              and um.IsActive=1 
                                                              and not exists(select * from UserMessages u 
                                                                                      where um.Level<u.Level and  u.ObjectType = um.ObjectType 
                                                                                            and u.ObjectGUID = um.ObjectGUID and u.ModuleType=um.ModuleType 
                                                                                            and um.ApprovalRuleID=u.ApprovalRuleID 
                                                                                            and u.Status!=@status
                                                                             )  
                                                   union all
                                                   select * from UserMessages um where um.role_id<>@roleId and ModuleType = @moduleType 
                                                                        and (Site_id=@siteId or @siteId=-1) and um.Status= @status                                    
									                                    and @roleId in (select role_id from user_roles where manager_flag = 'Y') 
									                                    and exists (select objectGuid from usermessages umd where um.MessageID = umd.MessageID group by umd.ObjectGUID having count(umd.ObjectGUID) = 1 )
									                                    and (exists (select * from PurchaseOrder where guid = um.ObjectGUID and ToSiteId = (case when(select count(*) from site)>1 then @siteId else (select Site_id from site) end))
								                                             or @siteId =(select top 1 Master_Site_Id from Company))
                                               ) a order by CreationDate";
            SqlParameter[] selectUserMessagesParameters = new SqlParameter[4];            
            selectUserMessagesParameters[0] = new SqlParameter("@roleId", roleId);
            selectUserMessagesParameters[1] = new SqlParameter("@moduleType", moduleType);
            selectUserMessagesParameters[2] = new SqlParameter("@status", UserMessagesDTO.UserMessagesStatus.PENDING.ToString());            
            selectUserMessagesParameters[3] = new SqlParameter("@siteId", siteId);
            DataTable userMessagesData = dataAccessHandler.executeSelectQuery(selectUserMessagesQuery, selectUserMessagesParameters, sqlTransaction);
            if (userMessagesData.Rows.Count > 0)
            {
                List<UserMessagesDTO> userMessagesList = new List<UserMessagesDTO>();
                foreach (DataRow userMessagesDataRow in userMessagesData.Rows)
                {
                    UserMessagesDTO userMessagesDataObject = GetUserMessagesDTO(userMessagesDataRow);
                    userMessagesList.Add(userMessagesDataObject);
                }
                log.LogMethodExit(userMessagesList);
                return userMessagesList;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }

        /// <summary>
        /// Gets the UserMessagesDTO list matching the search key
        /// </summary>
        /// <param name="searchParameters">List of search parameters</param>
        /// <returns>Returns the list of UserMessagesDTO matching the search criteria</returns>
        public List<UserMessagesDTO> GetUserMessagesList(List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            List<UserMessagesDTO> userMessagesList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectUserMessagesQuery = SELECT_QUERY;
            if ((searchParameters != null) && (searchParameters.Count > 0))
            {
                string joiner;
                int count = 0;
                StringBuilder query = new StringBuilder(" WHERE ");
                foreach (KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string> searchParameter in searchParameters)
                {
                    joiner = (count == 0) ? string.Empty : " and ";
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        if (searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.MESSAGE_ID
                            || searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.ACTED_BY_USER
                            || searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.APPROVAL_RULE_ID
                            || searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.ROLE_ID                            
                            || searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.MASTER_ENTITY_ID)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE
                            || searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID
                            || searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE
                            || searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.STATUS)
                        {
                            query.Append(joiner + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else if (searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG)
                        {
                            query.Append(joiner + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",0)=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value == "1"));
                        }
                        else if (searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.LAST_UPDATED_DATE_TILL)
                        {
                            query.Append(joiner + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) < " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID)
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
                    selectUserMessagesQuery = selectUserMessagesQuery + query;
            }
            DataTable userMessagesData = dataAccessHandler.executeSelectQuery(selectUserMessagesQuery, parameters.ToArray(), sqlTransaction);
            if (userMessagesData.Rows.Count > 0)
            {
                userMessagesList = new List<UserMessagesDTO>();
                foreach (DataRow userMessagesDataRow in userMessagesData.Rows)
                {
                    UserMessagesDTO userMessagesDataObject = GetUserMessagesDTO(userMessagesDataRow);
                    userMessagesList.Add(userMessagesDataObject);
                }
                
            }
            log.LogMethodExit(userMessagesList);
            return userMessagesList;
        }

        /// <summary>
        /// Returns the no of UserMessages matching the search parameters
        /// </summary>
        /// <param name="roleId">roleId</param>
        /// <param name="moduleType">moduleType</param>
        /// <param name="siteId">siteId</param>
        /// <returns>no of UserMessages matching the criteria</returns>
        public int GetUserMessagesCount(int roleId, string moduleType, int siteId)
        {
            log.LogMethodEntry(roleId, moduleType, siteId);
            int userMessagesCount = 0;
            string selectUserMessagesQuery = @"select * from(
                                                   select * from UserMessages um 
                                                        where role_id = @roleId and ModuleType = @moduleType 
                                                              and (Site_id=@siteId or @siteId=-1) and um.Status= @status
                                                              and um.IsActive=1 
                                                              and not exists(select * from UserMessages u 
                                                                                      where um.Level<u.Level and  u.ObjectType = um.ObjectType 
                                                                                            and u.ObjectGUID = um.ObjectGUID and u.ModuleType=um.ModuleType 
                                                                                            and um.ApprovalRuleID=u.ApprovalRuleID 
                                                                                            and u.Status!=@status
                                                                             )  
                                                   union all
                                                   select * from UserMessages um where um.role_id<>@roleId and ModuleType = @moduleType 
                                                                        and (Site_id=@siteId or @siteId=-1) and um.Status= @status                                    
									                                    and @roleId in (select role_id from user_roles where manager_flag = 'Y') 
									                                    and exists (select objectGuid from usermessages umd where um.MessageID = umd.MessageID group by umd.ObjectGUID having count(umd.ObjectGUID) = 1 )
									                                    and (exists (select * from PurchaseOrder where guid = um.ObjectGUID and ToSiteId = (case when(select count(*) from site)>1 then @siteId else (select Site_id from site) end))
								                                             or @siteId =(select top 1 Master_Site_Id from Company))
                                               ) a order by CreationDate";
            SqlParameter[] selectUserMessagesParameters = new SqlParameter[4];
            selectUserMessagesParameters[0] = new SqlParameter("@roleId", roleId);
            selectUserMessagesParameters[1] = new SqlParameter("@moduleType", moduleType);
            selectUserMessagesParameters[2] = new SqlParameter("@status", UserMessagesDTO.UserMessagesStatus.PENDING.ToString());
            selectUserMessagesParameters[3] = new SqlParameter("@siteId", siteId);
            DataTable userMessagesData = dataAccessHandler.executeSelectQuery(selectUserMessagesQuery, selectUserMessagesParameters, sqlTransaction);
            if (userMessagesData.Rows.Count > 0)
            {
                userMessagesCount = Convert.ToInt32(userMessagesData.Rows.Count);
            }
            log.LogMethodExit(userMessagesCount);
            return userMessagesCount;
        }
    }
}
