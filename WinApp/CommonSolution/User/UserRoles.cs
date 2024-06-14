/********************************************************************************************
 * Project Name - User Roles
 * Description  - Business logic of user roles
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        28-JUN-2016   Raghuveera          Created 
 *2.60        08-May-2019   Mushahid Faizan     Added log Method Entry & Exit in Save() & SaveUpdateUserRolesList()
 *2.70.2        15-Jul-2019   Girish Kundar       Modified : Save() method. Now Insert/Update method returns the DTO instead of Id.
 *            04-Nov-2019   Jagan Mohana        Added new method GetFormAccessRoles()
 *2.70.3      27-Mar-2020   Girish Kundar       Modified : GetUIFieldsToHide() method to build child records
 *2.80.0      05-Jun-2020   Girish Kundar   Modified : Renamed list save method as Save() and added sqlTransaction logic
 *2.100.0     05-Sep-2020   Girish Kundar   Modified : POS UI redesign related changes 
 *2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
  *2.150.3     27-APR-2023   Sweedol             Modified: Management form access new architecture changes
 ********************************************************************************************/
//using Semnox.Core.Security.DataAccessControl;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.PriceList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// User roles business logic
    /// </summary>
    public class UserRoles
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserRolesDTO userRolesDTO;
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of UserRoles class having ExecutionContext
        /// </summary>
        private UserRoles(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="userRolesDTO"></param>
        public UserRoles(ExecutionContext executionContext, UserRolesDTO userRolesDTO)
         : this(executionContext)
        {
            log.LogMethodEntry(executionContext, userRolesDTO);
            this.userRolesDTO = userRolesDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the user role details on role id
        /// </summary>
        /// <param name="roleId">integer type parameter</param>   
        /// <param name="activeChildRecords">To load only active records</param>
        /// <param name="loadChildRecords">To load the child DTO Records</param>
        public UserRoles(ExecutionContext executionContext, int roleId, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        : this(executionContext)
        {
            log.LogMethodEntry(roleId, loadChildRecords, activeChildRecords, sqlTransaction);
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
            userRolesDTO = userRolesDataHandler.GetUserRoles(roleId);
            if (loadChildRecords && this.userRolesDTO != null)
            {
                List<UsersDTO> UsersDTOBL = new List<UsersDTO>();
                UsersList usersList = new UsersList(executionContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_ID, roleId.ToString()));
                if (activeChildRecords)
                    searchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                else
                    searchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "N"));
                this.userRolesDTO.UsersDTO = usersList.GetAllUsers(searchParams, loadChildRecords);

                ProductMenuPanelExclusionListBL productMenuPanelExclusionListBL = new ProductMenuPanelExclusionListBL();
                List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionList = productMenuPanelExclusionListBL.GetProductMenuPanelExclusionDTOListForUserRoles(new List<int>() { userRolesDTO.RoleId }, activeChildRecords, sqlTransaction);
                if (productMenuPanelExclusionList != null)
                {
                    userRolesDTO.ProductMenuPanelExclusionDTOList = productMenuPanelExclusionList;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the true if approver is of higher level
        /// </summary>
        /// <param name="workerRoleId">Role id of the task performer</param>
        /// <param name="approverRoleId">Role id of the task approver</param>
        /// <param name="siteId">Site id of the task approver</param>
        /// <param name="sqlTransaction">SqlTransaction object </param>
        /// <returns>returns true </returns>
        public bool IsApproverIsHeigherLevel(int workerRoleId, int approverRoleId, int siteId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(workerRoleId, approverRoleId, siteId, sqlTransaction);
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
            log.Debug("Starts-IsApproverIsHeigherLevel(" + workerRoleId + ", " + approverRoleId + "," + siteId + ") method.");
            DataTable dTable = userRolesDataHandler.GetLowerLevelsRoleData(workerRoleId, siteId);
            if (dTable != null && dTable.Rows.Count > 0)//if assigned managed id is configured in user roles
            {
                foreach (DataRow dr in dTable.Rows)
                {
                    if (Convert.ToInt32(dr["roleId"]) == approverRoleId)
                    {
                        log.Debug("Ends-IsApproverIsHeigherLevel(workerRoleId, approverRoleId,siteId) method. The approver is not of higher level");
                        return false;
                    }
                }
            }
            else
            {
                UserRolesDTO workerRoleDTO = userRolesDataHandler.GetUserRoles(workerRoleId);//task creator role
                if (workerRoleDTO == null)
                {
                    throw new Exception("Ends-IsApproverIsHeigherLevel(workerRoleId, approverRoleId,siteId) method.Creator role id(" + workerRoleId + ") details failed to load. ");
                }
                else
                {
                    if (workerRoleDTO.ManagerFlag.Equals("Y"))
                    {
                        log.Debug("Ends-IsApproverIsHeigherLevel(workerRoleId, approverRoleId,siteId) method. The approver is not of higher level because the creator is a manager.");
                        return false;
                    }
                    else if (workerRoleDTO.ManagerFlag.Equals("N"))
                    {
                        UserRolesDTO approverRoleDTO = userRolesDataHandler.GetUserRoles(approverRoleId);//approver in hq
                        if (approverRoleDTO == null)
                        {
                            log.Debug("Ends-IsApproverIsHeigherLevel(workerRoleId, approverRoleId,siteId) method. The approver details failed to load.");
                            return false;
                            //throw new Exception("Ends-IsApproverIsHeigherLevel(workerRoleId, approverRoleId,siteId) method.Creator role id(" + workerRoleId + ") details failed to load. ");
                        }
                        else
                        {
                            if (approverRoleDTO.ManagerFlag.Equals("Y"))
                            {
                                log.Debug("Ends-IsApproverIsHeigherLevel(workerRoleId, approverRoleId,siteId) method. The approver is of higher level because he is a manager.");
                                return true;
                            }
                            else
                            {
                                log.Debug("Ends-IsApproverIsHeigherLevel(workerRoleId, approverRoleId,siteId) method. Setup conflict and failed to identify the level of approver.");
                                return false;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// Check is editable for ui or not
        /// </summary>
        /// <param name="uiName"> Ui name how we used in the lookups</param>
        /// <returns>Boolean</returns>
        public bool IsEditable(string uiName)
        {
            log.LogMethodEntry(uiName);
            if (userRolesDTO == null)
            {
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                if (userRolesDTO.DataAccessRuleId != -1)
                {
                    DataAccessRule dataAccessRule = new DataAccessRule(executionContext, userRolesDTO.DataAccessRuleId);
                    log.LogMethodExit("Result");
                    return dataAccessRule.IsEditable(uiName);
                }
                else
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
        }

        /// <summary>
        /// Check is editable for ui or not
        /// </summary>
        /// <param name="uiName"> Ui name how we used in the lookups</param>
        /// <returns>Boolean</returns>
        public List<EntityExclusionDetailDTO> GetUIFieldsToHide(string uiName, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(uiName, sqlTransaction);
            List<EntityExclusionDetailDTO> listOfUIFieldsToHide = new List<EntityExclusionDetailDTO>();

            if (userRolesDTO != null && userRolesDTO.DataAccessRuleId != -1)
            {
                DataAccessRule dataAccessRule = new DataAccessRule(executionContext, userRolesDTO.DataAccessRuleId, sqlTransaction, true, true);
                listOfUIFieldsToHide = dataAccessRule.GetUIFieldsToHide(uiName);
            }
            log.LogMethodExit(listOfUIFieldsToHide);
            return listOfUIFieldsToHide;
        }
        /// <summary>
        /// Saves the User Roles
        /// Checks if the Role id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Validate(sqlTransaction);
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
            if (userRolesDTO.RoleId < 0)
            {
                userRolesDTO = userRolesDataHandler.InsertUserRole(userRolesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(userRolesDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("user_roles", userRolesDTO.Guid, sqlTransaction);
                }
                userRolesDTO.AcceptChanges();
                AddManagementFormAccess(sqlTransaction);
            }
            else
            {
                if (userRolesDTO.IsChanged)
                {
                    UserRolesDTO existingUserRolesDTO = new UserRoles(executionContext, userRolesDTO.RoleId).getUserRolesDTO;
                    userRolesDTO = userRolesDataHandler.UpdateUserRole(userRolesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(userRolesDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("user_roles", userRolesDTO.Guid, sqlTransaction);
                    }
                    userRolesDTO.AcceptChanges();
                    if (existingUserRolesDTO.Role.ToLower().ToString() != userRolesDTO.Role.ToLower().ToString())
                    {
                        RenameManagementFormAccess(existingUserRolesDTO.Role, sqlTransaction);
                    }
                    if (existingUserRolesDTO.IsActive != userRolesDTO.IsActive)
                    {
                        UpdateManagementFormAccess(userRolesDTO.Role, userRolesDTO.IsActive, userRolesDTO.Guid,sqlTransaction);
                    }
                }
            }

            SaveUserRolePriceList(sqlTransaction);
            SaveUserRoleDisplayGroupExclusion(sqlTransaction);
            SaveProductMenuPanelExclusions(sqlTransaction);
            log.LogMethodExit();
        }
        private void AddManagementFormAccess(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userRolesDTO.RoleId > -1)
            {
                UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
                userRolesDataHandler.AddManagementFormAccess(userRolesDTO.Role, userRolesDTO.Guid, executionContext.GetSiteId(), userRolesDTO.IsActive);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void RenameManagementFormAccess(string existingFormName, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userRolesDTO.RoleId > -1)
            {
                UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
                userRolesDataHandler.RenameManagementFormAccess(userRolesDTO.Role, existingFormName, executionContext.GetSiteId(), userRolesDTO.Guid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void UpdateManagementFormAccess(string formName, bool updatedIsActive, string functionGuid, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userRolesDTO.RoleId > -1)
            {
                UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
                userRolesDataHandler.UpdateManagementFormAccess(formName,executionContext.GetSiteId(), updatedIsActive, functionGuid);
                log.LogMethodExit();
            }
            log.LogMethodExit();
        }
        private void Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry();
            if (userRolesDTO.IsChanged && userRolesDTO.IsActive == false)
            {
                try
                {
                    UserContainerDTOCollection result = UserContainerList.GetUserContainerDTOCollection(executionContext.SiteId);
                    if (result != null && result.UserContainerDTOList != null && result.UserContainerDTOList.Any())
                    {
                        bool activeUserExists = result.UserContainerDTOList.Exists(x => x.RoleId == userRolesDTO.RoleId);
                        if (activeUserExists)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, 1143);
                            log.LogMethodExit(null, "Throwing Exception - " + message);
                            throw new Core.Utilities.ForeignKeyException(message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Unable to find the UserRoleContainerDTO", ex);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        private void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            try
            {
                if (userRolesDTO.UserRolePriceListDTOList != null && userRolesDTO.UserRolePriceListDTOList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SaveUserRolePriceList(sqlTransaction);
                if (userRolesDTO.UserRoleDisplayGroupExclusionsDTOList != null && userRolesDTO.UserRoleDisplayGroupExclusionsDTOList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SaveUserRoleDisplayGroupExclusion(sqlTransaction);
                if (userRolesDTO.ProductMenuPanelExclusionDTOList != null && userRolesDTO.ProductMenuPanelExclusionDTOList.Any(x => x.IsActive))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new Core.Utilities.ForeignKeyException(message);
                }
                SaveProductMenuPanelExclusions(sqlTransaction);
                if (userRolesDTO.RoleId >= 0)
                {
                    UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
                    userRolesDataHandler.Delete(userRolesDTO.RoleId);
                }
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }
        private void SaveUserRolePriceList(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userRolesDTO.UserRolePriceListDTOList != null &&
                userRolesDTO.UserRolePriceListDTOList.Any())
            {
                List<UserRolePriceListDTO> userRolePriceListDTOList = new List<UserRolePriceListDTO>();
                foreach (UserRolePriceListDTO userRolePriceListDTO in userRolesDTO.UserRolePriceListDTOList)
                {
                    if (userRolePriceListDTO.Roleid != userRolesDTO.RoleId)
                    {
                        userRolePriceListDTO.Roleid = userRolesDTO.RoleId;
                    }
                    if (userRolePriceListDTO.IsChanged)
                    {
                        userRolePriceListDTOList.Add(userRolePriceListDTO);
                    }
                }
                if (userRolePriceListDTOList.Any())
                {
                    UserRolePriceListBL userRolePriceListBL = new UserRolePriceListBL(executionContext, userRolePriceListDTOList);
                    userRolePriceListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }
        private void SaveUserRoleDisplayGroupExclusion(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userRolesDTO.UserRoleDisplayGroupExclusionsDTOList != null &&
                userRolesDTO.UserRoleDisplayGroupExclusionsDTOList.Any())
            {
                List<UserRoleDisplayGroupExclusionsDTO> UpdateduserRoleDisplayGroupExclusionsDTOList = new List<UserRoleDisplayGroupExclusionsDTO>();
                foreach (UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusionsDTO in userRolesDTO.UserRoleDisplayGroupExclusionsDTOList)
                {
                    if (userRoleDisplayGroupExclusionsDTO.RoleId != userRolesDTO.RoleId)
                    {
                        userRoleDisplayGroupExclusionsDTO.RoleId = userRolesDTO.RoleId;
                    }
                    if (userRoleDisplayGroupExclusionsDTO.IsChanged)
                    {
                        UpdateduserRoleDisplayGroupExclusionsDTOList.Add(userRoleDisplayGroupExclusionsDTO);
                    }
                }
                if (UpdateduserRoleDisplayGroupExclusionsDTOList.Any())
                {
                    UserRoleDisplayGroupExclusionsList userRoleDisplayGroupExclusionsList = new UserRoleDisplayGroupExclusionsList(executionContext, UpdateduserRoleDisplayGroupExclusionsDTOList);
                    userRoleDisplayGroupExclusionsList.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        private void SaveProductMenuPanelExclusions(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (userRolesDTO.ProductMenuPanelExclusionDTOList != null &&
                userRolesDTO.ProductMenuPanelExclusionDTOList.Any())
            {
                List<ProductMenuPanelExclusionDTO> updatedProductMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
                foreach (var ProductMenuPanelExclusionDTO in userRolesDTO.ProductMenuPanelExclusionDTOList)
                {
                    if (ProductMenuPanelExclusionDTO.UserRoleId != userRolesDTO.RoleId)
                    {
                        ProductMenuPanelExclusionDTO.UserRoleId = userRolesDTO.RoleId;
                    }
                    if (ProductMenuPanelExclusionDTO.IsChanged)
                    {
                        updatedProductMenuPanelExclusionDTOList.Add(ProductMenuPanelExclusionDTO);
                    }
                }
                if (updatedProductMenuPanelExclusionDTOList.Any())
                {
                    ProductMenuPanelExclusionListBL ProductMenuPanelExclusionListBL = new ProductMenuPanelExclusionListBL(executionContext, updatedProductMenuPanelExclusionDTOList);
                    ProductMenuPanelExclusionListBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Filter in the controller based on security policy and loginid
        /// the users and userroles based on the roleId
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public string GetFormAccessRoles(int roleId)
        {
            log.LogMethodEntry(roleId);
            string roles = string.Empty;
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler();
            roles = userRolesDataHandler.GetFormAccessRoles(roleId);
            log.LogMethodExit(roles);
            return roles;
        }

        public string GetDataAccessRuleLookup(int userId)
        {
            log.LogMethodEntry(userId);
            string roles = string.Empty;
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler();
            roles = userRolesDataHandler.GetDataAccessRuleLookup(userId, executionContext.GetSiteId());
            log.LogMethodExit(roles);
            return roles;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public UserRolesDTO getUserRolesDTO { get { return userRolesDTO; } }

    }
    /// <summary>
    /// Manages the list of user roles
    /// </summary>
    public class UserRolesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<UserRolesDTO> userRolesList;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserRolesList()
        {
            log.LogMethodEntry();
            this.userRolesList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public UserRolesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.userRolesList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public UserRolesList(ExecutionContext executionContext, List<UserRolesDTO> userRolesList)
        {
            log.LogMethodEntry(executionContext, userRolesList);
            this.userRolesList = userRolesList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the user roles based on the id passed
        /// </summary>
        /// <param name="RoleId">Integer value</param>
        /// <returns>UserRolesDTO </returns>
        public UserRolesDTO GetUserRole(int RoleId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(RoleId, sqlTransaction);
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
            UserRolesDTO userRolesDTO = userRolesDataHandler.GetUserRoles(RoleId);
            log.LogMethodExit(userRolesDTO);
            return userRolesDTO;
        }

        /// <summary>
        /// Returns the user roles based on the guid passed
        /// </summary>
        /// <param name="roleGuid">unique identifier value value</param>
        /// <returns>UserRolesDTO </returns>
        public UserRolesDTO GetUserRole(string roleGuid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(roleGuid, sqlTransaction);
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
            UserRolesDTO userRolesDTO = userRolesDataHandler.GetUserRoles(roleGuid);
            log.LogMethodExit(userRolesDTO);
            return userRolesDTO;
        }
        /// <summary>
        /// Returns the user roles list
        /// </summary>
        public List<UserRolesDTO> GetAllUserRoles(List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters,
                                                  bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler(sqlTransaction);
            List<UserRolesDTO> userRolesDTOList = userRolesDataHandler.GetUserRolesList(searchParameters);
            if (userRolesDTOList != null && userRolesDTOList.Any() && loadChildRecords)
            {
                Build(userRolesDTOList, activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit(userRolesDTOList);
            return userRolesDTOList;
        }

        private void Build(List<UserRolesDTO> userRolesDTOList, bool activeChildRecords, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(userRolesDTOList, sqlTransaction);
            Dictionary<int, UserRolesDTO> userIdIdUserRoleDTODictionary = new Dictionary<int, UserRolesDTO>();
            string userRoleIdSet;
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < userRolesDTOList.Count; i++)
            {
                if (userRolesDTOList[i].RoleId == -1 ||
                    userIdIdUserRoleDTODictionary.ContainsKey(userRolesDTOList[i].RoleId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(userRolesDTOList[i].RoleId);
                userIdIdUserRoleDTODictionary.Add(userRolesDTOList[i].RoleId, userRolesDTOList[i]);
            }
            userRoleIdSet = sb.ToString();
            UserRoleDisplayGroupExclusionsList userRoleDisplayGroupExclusionsList = new UserRoleDisplayGroupExclusionsList();
            List<KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>> searchParameters = new List<KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>>();
            searchParameters.Add(new KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>(UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ROLE_ID_LIST, userRoleIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters, string>(UserRoleDisplayGroupExclusionsDTO.SearchByDisplayGroupsParameters.ISACTIVE, "1"));
            }
            List<UserRoleDisplayGroupExclusionsDTO> userRoleDisplayGroupExclusionsDTOList = userRoleDisplayGroupExclusionsList.GetAllUserRoleDisplayGroupExclusionsList(searchParameters);
            if (userRoleDisplayGroupExclusionsDTOList != null && userRoleDisplayGroupExclusionsDTOList.Any())
            {
                log.LogVariableState("userRoleDisplayGroupExclusionsDTOList", userRoleDisplayGroupExclusionsDTOList);
                foreach (UserRoleDisplayGroupExclusionsDTO userRoleDisplayGroupExclusionsDTO in userRoleDisplayGroupExclusionsDTOList)
                {
                    if (userIdIdUserRoleDTODictionary.ContainsKey(userRoleDisplayGroupExclusionsDTO.RoleId))
                    {
                        if (userIdIdUserRoleDTODictionary[userRoleDisplayGroupExclusionsDTO.RoleId].UserRoleDisplayGroupExclusionsDTOList == null)
                        {
                            userIdIdUserRoleDTODictionary[userRoleDisplayGroupExclusionsDTO.RoleId].UserRoleDisplayGroupExclusionsDTOList = new List<UserRoleDisplayGroupExclusionsDTO>();
                        }
                        userIdIdUserRoleDTODictionary[userRoleDisplayGroupExclusionsDTO.RoleId].UserRoleDisplayGroupExclusionsDTOList.Add(userRoleDisplayGroupExclusionsDTO);
                    }
                }
            }
            List<KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>> priceListsearchParameters = new List<KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>>();
            priceListsearchParameters.Add(new KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>(UserRolePriceListDTO.SearchByUserRolePriceListParameters.ROLE_ID_LIST, userRoleIdSet.ToString()));
            if (activeChildRecords)
            {
                priceListsearchParameters.Add(new KeyValuePair<UserRolePriceListDTO.SearchByUserRolePriceListParameters, string>(UserRolePriceListDTO.SearchByUserRolePriceListParameters.ISACTIVE, "1"));
            }
            UserRolePriceListBL userRolePriceListList = new UserRolePriceListBL();
            List<UserRolePriceListDTO> userRolePriceListDTOList = userRolePriceListList.GetAllUserRolePriceList(priceListsearchParameters, sqlTransaction);
            if (userRolePriceListDTOList != null && userRolePriceListDTOList.Any())
            {
                log.LogVariableState("userRolePriceListDTOList", userRolePriceListDTOList);
                foreach (UserRolePriceListDTO userRolePriceListDTO in userRolePriceListDTOList)
                {
                    if (userIdIdUserRoleDTODictionary.ContainsKey(userRolePriceListDTO.Roleid))
                    {
                        if (userIdIdUserRoleDTODictionary[userRolePriceListDTO.Roleid].UserRolePriceListDTOList == null)
                        {
                            userIdIdUserRoleDTODictionary[userRolePriceListDTO.Roleid].UserRolePriceListDTOList = new List<UserRolePriceListDTO>();
                        }
                        userIdIdUserRoleDTODictionary[userRolePriceListDTO.Roleid].UserRolePriceListDTOList.Add(userRolePriceListDTO);
                    }
                }
            }
            UsersList usersList = new UsersList();
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
            searchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_ID_LIST, userRoleIdSet.ToString()));
            if (activeChildRecords)
            {
                searchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
            }
            List<UsersDTO> usersDTOList = usersList.GetAllUsers(searchParams, false, true, sqlTransaction);
            if (usersDTOList != null && usersDTOList.Any())
            {
                log.LogVariableState("usersDTOList", usersDTOList);
                foreach (UsersDTO usersDTO in usersDTOList)
                {
                    if (userIdIdUserRoleDTODictionary.ContainsKey(usersDTO.RoleId))
                    {
                        if (userIdIdUserRoleDTODictionary[usersDTO.RoleId].UsersDTO == null)
                        {
                            userIdIdUserRoleDTODictionary[usersDTO.RoleId].UsersDTO = new List<UsersDTO>();
                        }
                        userIdIdUserRoleDTODictionary[usersDTO.RoleId].UsersDTO.Add(usersDTO);
                    }
                }
            }
            // Loads the Management formaccess details for Users
            List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> formAccesssearchParameters = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
            formAccesssearchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID_LIST, userRoleIdSet.ToString()));
            if(activeChildRecords)
            {
                formAccesssearchParameters.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE,"1"));
            }
            ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL();
            List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessListBL.GetManagementFormAccessDTOList(formAccesssearchParameters);
            if (managementFormAccessDTOList != null && managementFormAccessDTOList.Any())
            {
                log.LogVariableState("managementFormAccessDTOList", managementFormAccessDTOList);
                foreach (ManagementFormAccessDTO managementFormAccessDTO in managementFormAccessDTOList)
                {
                    if (userIdIdUserRoleDTODictionary.ContainsKey(managementFormAccessDTO.RoleId))
                    {
                        if (userIdIdUserRoleDTODictionary[managementFormAccessDTO.RoleId].ManagementFormAccessDTOList == null)
                        {
                            userIdIdUserRoleDTODictionary[managementFormAccessDTO.RoleId].ManagementFormAccessDTOList = new List<ManagementFormAccessDTO>();
                        }
                        userIdIdUserRoleDTODictionary[managementFormAccessDTO.RoleId].ManagementFormAccessDTOList.Add(managementFormAccessDTO);
                    }
                }
            }

            //load product menu panel exclusion for user role list
            ProductMenuPanelExclusionListBL ProductMenuPanelExclusionListBL = new ProductMenuPanelExclusionListBL();
            List<ProductMenuPanelExclusionDTO> productMenuPanelExclusionDTOList = ProductMenuPanelExclusionListBL.GetProductMenuPanelExclusionDTOListForUserRoles(userIdIdUserRoleDTODictionary.Keys.ToList(), activeChildRecords, sqlTransaction);
            if (productMenuPanelExclusionDTOList != null && productMenuPanelExclusionDTOList.Any())
            {
                for (int i = 0; i < productMenuPanelExclusionDTOList.Count; i++)
                {
                    if (userIdIdUserRoleDTODictionary.ContainsKey(productMenuPanelExclusionDTOList[i].UserRoleId) == false)
                    {
                        continue;
                    }
                    UserRolesDTO userRolesDTO = userIdIdUserRoleDTODictionary[productMenuPanelExclusionDTOList[i].UserRoleId];
                    if (userRolesDTO.ProductMenuPanelExclusionDTOList == null)
                    {
                        userRolesDTO.ProductMenuPanelExclusionDTOList = new List<ProductMenuPanelExclusionDTO>();
                    }
                    userRolesDTO.ProductMenuPanelExclusionDTOList.Add(productMenuPanelExclusionDTOList[i]);
                }
            }

            log.LogMethodExit();
        }


        /// <summary>
        /// This method should be used to Save and Update the User Roles details for Web Management Studio.
        /// </summary>
        public List<UserRolesDTO> Save()
        {
            log.LogMethodEntry();
            List<UserRolesDTO> savedUserRoleDTOList = new List<UserRolesDTO>();
            if (userRolesList != null && userRolesList.Any())
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (UserRolesDTO userRoleDto in userRolesList)
                        {
                            UserRoles userRoles = new UserRoles(executionContext, userRoleDto);
                            userRoles.Save(parafaitDBTrx.SQLTrx);
                            savedUserRoleDTOList.Add(userRoles.getUserRolesDTO);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (SqlException sqlEx)
                    {
                        log.Error(sqlEx);
                        parafaitDBTrx.RollBack();
                        log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                        if (sqlEx.Number == 547)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                        }
                        if (sqlEx.Number == 2601)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                        }
                        else
                        {
                            throw;
                        }
                    }
                    catch (ValidationException valEx)
                    {
                        log.Error(valEx);
                        parafaitDBTrx.RollBack();
                        log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        parafaitDBTrx.RollBack();
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
            return savedUserRoleDTOList;
        }

        public DateTime? GetUserRoleModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserRolesDataHandler userRolesDataHandler = new UserRolesDataHandler();
            DateTime? result = userRolesDataHandler.GetUserRoleModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
    }
}