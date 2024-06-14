/********************************************************************************************
 * Project Name - User Messages BL
 * Description  - BL of the user messages class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019   Deeksha             modifications as per 3 tier standards
 *2.70.2        17-Dec-2019   Jinto Thomas        Added parameter executioncontext for userrolebl declaration with userid 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    public class UserMessages
    {
        private UserMessagesDTO userMessagesDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext; //= ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default constructor of UserMessages class
        /// </summary>
        public UserMessages(ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(machineUserContext);
            userMessagesDTO = null;
            this.machineUserContext = machineUserContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the UserMessages DTO based on the userMessages id passed 
        /// </summary>
        /// <param name="userMessagesId">UserMessages id</param>
        public UserMessages(int userMessagesId, ExecutionContext machineUserContext, SqlTransaction sqlTransaction = null)
            : this(machineUserContext)
        {
            log.LogMethodEntry(userMessagesId, machineUserContext, sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            userMessagesDTO = userMessagesDataHandler.GetUserMessages(userMessagesId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Contructor which gets the UserMessagesDTO of passed guid
        /// </summary>
        /// <param name="guid"> guid of the purchaseOrder</param>
        /// <param name="executionUserContext"> ExecutionContext</param>
        /// <param name="sqltrxn"> sqlTransction object</param>
        public UserMessages(string guid, ExecutionContext executionUserContext, SqlTransaction sqlTransaction = null)
            : this(executionUserContext)
        {
            log.LogMethodEntry(guid, executionUserContext, sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> searchParameters = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
            searchParameters.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, guid));
            List<UserMessagesDTO> userMessagesDTOList = userMessagesDataHandler.GetUserMessagesList(searchParameters);
            if (userMessagesDTOList == null || (userMessagesDTOList != null && userMessagesDTOList.Count == 0))
            {
                userMessagesDTO = new UserMessagesDTO();
            }
            else
            {
                userMessagesDTO = userMessagesDTOList[0];
            }
            log.LogMethodExit();

        }

        /// <summary>
        /// Creates userMessages object using the UserMessagesDTO
        /// </summary>
        /// <param name="userMessages">UserMessagesDTO object</param>
        public UserMessages(UserMessagesDTO userMessages, ExecutionContext machineUserContext)
            : this(machineUserContext)
        {
            log.LogMethodEntry(userMessages, machineUserContext);
            this.userMessagesDTO = userMessages;
            this.machineUserContext = machineUserContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the userMessages data object of highest approved
        /// </summary>
        /// <param name="approvalRuleId"> The approval rule id for task</param>
        /// <param name="roleId"> RoleId of the user who created the task</param>
        /// <param name="userId"> UserId of the user who created the task</param>
        /// <param name="moduleType">Module type should be POS, Inventory, HR etc</param>
        /// <param name="objectType">Object type should be AJIS, DIIS</param>
        /// <param name="guid">integer type parameter</param>
        /// <param name="siteId">integer type parameter</param>
        /// <returns>Returns UserMessagesDTO which has highest approval details</returns>
        public UserMessagesDTO GetHighestApprovedUserMessage(int approvalRuleId, int roleId, int userId, string moduleType, string objectType,
                                                             string guid, int siteId, SqlTransaction sqlTransaction = null)
        {
            List<UserMessagesDTO> userMessagesDTOList;
            log.LogMethodEntry(approvalRuleId, roleId, userId, moduleType, objectType, guid, siteId, sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            userMessagesDTOList = userMessagesDataHandler.GetApprovedUserMessage(approvalRuleId, roleId, userId, moduleType, objectType, guid, siteId);
            if (userMessagesDTOList != null && userMessagesDTOList.Count > 0)
            {
                log.LogMethodExit(userMessagesDTO);
                userMessagesDTO = userMessagesDTOList[0];
                return userMessagesDTO;
            }
            else
            {
                log.LogMethodExit();
                return null;
            }
        }
        /// <summary>
        /// Saves the user messages record
        /// Checks if the message id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            if (userMessagesDTO == null || userMessagesDTO.IsChanged == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(machineUserContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation Failed", validationErrors);
            }
            if (userMessagesDTO.MessageId < 0)
            {
                userMessagesDTO = userMessagesDataHandler.InsertUserMessages(userMessagesDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                userMessagesDTO.AcceptChanges();
            }
            else
            {
                if (userMessagesDTO.IsChanged)
                {
                    userMessagesDTO = userMessagesDataHandler.UpdateUserMessages(userMessagesDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    userMessagesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }


        public List<ValidationError> Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if ((string.IsNullOrWhiteSpace(userMessagesDTO.Status) || userMessagesDTO.Status == "REJECTED") && (string.IsNullOrWhiteSpace(userMessagesDTO.Message)))
            {
                log.Debug("Enter Remarks ");
                validationErrorList.Add(new ValidationError("UserMessage", "Message", MessageContainerList.GetMessage(machineUserContext, 201, MessageContainerList.GetMessage(machineUserContext, "Message"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        public void CreateUserMessages(ApprovalRuleDTO approvalRuleDTO, string moduleType, string objectType, string objectGUID, int userId, string messageType, string message, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(approvalRuleDTO, moduleType, objectType, objectGUID, userId, messageType, message, sqlTransaction);
            UserMessagesList userMessagesList = new UserMessagesList();
            List<UserMessagesDTO> userMessagesDTOList;
            List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, objectGUID));
            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, objectType));
            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, moduleType));
            userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);
            if ((userMessagesDTOList == null || (userMessagesDTOList != null && userMessagesDTOList.Count == 0)) && approvalRuleDTO.NumberOfApprovalLevels != 0)
            {
                SaveUserMessages(approvalRuleDTO, moduleType, objectType, objectGUID, userId, messageType, message, sqlTransaction);

            }
            else if (userMessagesDTOList.Count == approvalRuleDTO.NumberOfApprovalLevels)
            {
                log.LogMethodExit();
                return;
            }
            else if (userMessagesDTOList.Count < approvalRuleDTO.NumberOfApprovalLevels)
            {
                SaveUserMessages(approvalRuleDTO, moduleType, objectType, objectGUID, userId, messageType, message, sqlTransaction);
            }
            log.LogMethodExit();
        }
        private void SaveUserMessages(ApprovalRuleDTO approvalRuleDTO, string moduleType, string objectType, string objectGUID, int userId, string messageType, string message, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(approvalRuleDTO, moduleType, objectType, objectGUID, userId, messageType, message, sqlTransaction);
            UserMessagesList userMessagesList = new UserMessagesList();
            int roleId = -1;
            List<UserMessagesDTO> userMessagesDTOList;
            List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
            UserRoles userRoles = new UserRoles(machineUserContext, approvalRuleDTO.RoleId);
            for (int i = 0; i < approvalRuleDTO.NumberOfApprovalLevels; i++)
            {
                if (userRoles.getUserRolesDTO != null && userRoles.getUserRolesDTO.AssignedManagerRoleId > -1)
                {
                    roleId = userRoles.getUserRolesDTO.AssignedManagerRoleId;
                    userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, objectGUID));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, objectType));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, moduleType));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ROLE_ID, userRoles.getUserRolesDTO.AssignedManagerRoleId.ToString()));
                    userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);
                    if (userMessagesDTOList == null || (userMessagesDTOList != null && userMessagesDTOList.Count == 0))
                    {
                        userMessagesDTO = new UserMessagesDTO();
                        userMessagesDTO.ApprovalRuleID = approvalRuleDTO.ApprovalRuleID;
                        userMessagesDTO.Level = i + 1;
                        userMessagesDTO.Message = message;
                        userMessagesDTO.MessageType = messageType;
                        userMessagesDTO.ModuleType = moduleType;
                        userMessagesDTO.ObjectType = objectType;
                        userMessagesDTO.ObjectGUID = objectGUID;
                        userMessagesDTO.RoleId = userRoles.getUserRolesDTO.AssignedManagerRoleId;
                        userMessagesDTO.UserId = userId;
                        Save(sqlTransaction);
                    }
                    userRoles = new UserRoles(machineUserContext, roleId);
                }
                else
                {
                    break;
                }
            }
            log.LogMethodExit();
        }

        public void UpdateStatus(UserMessagesDTO userMessagesDTO, UserMessagesDTO.UserMessagesStatus userStatus, int documentTypeId, int userId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userMessagesDTO, userStatus, documentTypeId, sqlTransaction);
            ApprovalLog approvalLog = new ApprovalLog(machineUserContext);
            try
            {
                List<UserMessagesDTO> userMessagesDTOList = new List<UserMessagesDTO>();
                UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
                this.userMessagesDTO = userMessagesDataHandler.GetUserMessages(userMessagesDTO.MessageId);
                if (userMessagesDTO.LastUpdatedDate.CompareTo(this.userMessagesDTO.LastUpdatedDate) == 0 && userMessagesDTO.Status == this.userMessagesDTO.Status)
                {
                    this.userMessagesDTO.Status = userStatus.ToString();
                    this.userMessagesDTO.ActedByUser = userId;
                    Save(sqlTransaction);
                    approvalLog.InsertApprovalLog(documentTypeId, this.userMessagesDTO.ObjectGUID, this.userMessagesDTO.Level, this.userMessagesDTO.Status, this.userMessagesDTO.ObjectType + " task as been " + this.userMessagesDTO.Status.ToLower() + " by " + machineUserContext.GetUserId() + " of approval level " + this.userMessagesDTO.Level + " from POSMachine " + Environment.MachineName + ".", sqlTransaction);
                    if (userStatus.Equals(UserMessagesDTO.UserMessagesStatus.REJECTED))
                    {
                        UserMessagesList userMessagesList = new UserMessagesList();
                        userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage(this.userMessagesDTO.ModuleType, this.userMessagesDTO.ObjectType, this.userMessagesDTO.ObjectGUID, -1, -1, machineUserContext.GetSiteId(), sqlTransaction);
                        if (userMessagesDTOList != null)
                        {
                            foreach (UserMessagesDTO umDTO in userMessagesDTOList)
                            {
                                umDTO.Status = UserMessagesDTO.UserMessagesStatus.CANCELLED.ToString();
                                this.userMessagesDTO = umDTO;
                                Save(sqlTransaction);
                                approvalLog.InsertApprovalLog(documentTypeId, this.userMessagesDTO.ObjectGUID, this.userMessagesDTO.Level, this.userMessagesDTO.Status, this.userMessagesDTO.ObjectType + " task as been rejected by " + machineUserContext.GetUserId() + " of approval level " + this.userMessagesDTO.Level + " from POSMachine " + Environment.MachineName + ".", sqlTransaction);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing UpdateStatus()" + ex.Message);
                log.Fatal("Ends-UpdateStatus(userMessagesDTO, userStatus,documentTypeId,sqlTransaction) method. Exception:" + ex.ToString());
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public UserMessagesDTO getUserMessagesDTO { get { return userMessagesDTO; } }
    }

    /// <summary>
    /// Manages the list of userMessagess
    /// </summary>
    public class UserMessagesList
    {
        private ExecutionContext executionContext;
        private List<UserMessagesDTO> userMessagesDTOList;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor with no parameter.
        /// </summary>
        public UserMessagesList() //added
        {
            log.LogMethodEntry();
            this.userMessagesDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Default constructor with executionContext
        /// </summary>
        public UserMessagesList(ExecutionContext executionContext) //added
            : this()
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the userMessages list
        /// </summary>
        public List<UserMessagesDTO> GetAllUserMessages(List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            List<UserMessagesDTO> userMessagesDTOs = new List<UserMessagesDTO>();
            userMessagesDTOs = userMessagesDataHandler.GetUserMessagesList(searchParameters);
            log.LogMethodExit(userMessagesDTOs);
            return userMessagesDTOs;
        }

        /// <summary>
        /// Returns the userMessages of all pending approvals for the passed object reference guid list
        /// </summary>
        public List<UserMessagesDTO> GetPendingApprovalUserMessage(string moduleType, string objectType, string objectGUID, int roleId, int userId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(moduleType, objectType, objectGUID, roleId, userId, siteId, sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            List<UserMessagesDTO> userMessagesDTOs = new List<UserMessagesDTO>();
            userMessagesDTOs = userMessagesDataHandler.GetPendingApprovalUserMessage(moduleType, objectType, objectGUID, roleId, userId, siteId);
            log.LogMethodExit(userMessagesDTOs);
            return userMessagesDTOs;
        }
        /// <summary>
        /// Returns the userMessages of all passed role id
        /// </summary>
        public List<UserMessagesDTO> GetAllMyPendingApprovalUserMessage(int roleId, string moduleType, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(roleId, moduleType, siteId, sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            List<UserMessagesDTO> userMessagesDTOs = new List<UserMessagesDTO>();
            userMessagesDTOs = userMessagesDataHandler.GetAllMyPendingApprovalUserMessage(roleId, moduleType, siteId);
            log.LogMethodExit(userMessagesDTOs);
            return userMessagesDTOs;
        }

        /// <summary>
        /// Returns the userMessages of all passed role id
        /// </summary>
        public List<UserMessagesDTO> GetHistoryUserMessage(int roleId, int userId, string moduleType, string loginId, int siteId, string status, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(roleId, moduleType, siteId, status, sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            List<UserMessagesDTO> userMessagesDTOs = new List<UserMessagesDTO>();
            userMessagesDTOs = userMessagesDataHandler.GetHistoryUserMessage(roleId, userId, moduleType, loginId, siteId, status);
            log.LogMethodExit(userMessagesDTOs);
            return userMessagesDTOs;
        }

        /// <summary>
        /// Returns the no of UserMessages matching the search Parameters
        /// </summary>
        /// <param name="roleId"> roleId</param>
        /// <param name="moduleType"> moduleType</param>
        /// <param name="siteId"> siteId</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetUserMessagesCount(int roleId, string moduleType, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(roleId, moduleType, siteId, sqlTransaction);
            UserMessagesDataHandler userMessagesDataHandler = new UserMessagesDataHandler(sqlTransaction);
            int userMessagesCount = userMessagesDataHandler.GetUserMessagesCount(roleId, moduleType, siteId);
            log.LogMethodExit(userMessagesCount);
            return userMessagesCount;
        }
    }
}