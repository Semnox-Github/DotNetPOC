/********************************************************************************************
 * Project Name - User
 * Description  - Container class caches user roles for a given site
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 2.110.0      12-Jan-2021      Deeksha                   Modified : POS UI Redesign with REST API
 2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 ********************************************************************************************/

using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Container class caches user roles for a given site
    /// </summary>
    public class UserRoleContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, UserRolesDTO> roleIdUserRolesDTODictionary = new Dictionary<int, UserRolesDTO>();
        private readonly Dictionary<string, UserRolesDTO> roleGuidUserRolesDTODictionary = new Dictionary<string, UserRolesDTO>();
        private readonly Dictionary<int, HashSet<int>> userRoleIdAssignedManagerRoleIdHashSetDictionary = new Dictionary<int, HashSet<int>>();
        private readonly HashSet<int> distinctAssignedManagerRoleIdHashSet = new HashSet<int>();
        private readonly Dictionary<int, bool> roleIdSelfApprovalAllowedDictionary = new Dictionary<int, bool>();
        private readonly Dictionary<int, List<int>> userRoleIdDataAccessRoleIdListDictionary = new Dictionary<int, List<int>>();
        private readonly Dictionary<int, UserRoleContainerDTO> roleIdUserRoleContainerDTODictionary = new Dictionary<int, UserRoleContainerDTO>();
        private readonly List<UserRolesDTO> userRolesDTOList;
        private readonly UserRoleContainerDTOCollection userRoleContainerDTOCollection;
        private readonly DateTime? userRoleModuleLastUpdateTime;
        private readonly int siteId;

        public UserRoleContainer(int siteId) : this(siteId, GetUserRolesDTOList(siteId), GetUserModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public UserRoleContainer(int siteId, List<UserRolesDTO> userRolesDTOList, DateTime? userRoleModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.userRolesDTOList = userRolesDTOList;
            List<UserRoleContainerDTO> userRoleContainerDTOList = new List<UserRoleContainerDTO>();
            this.userRoleModuleLastUpdateTime = userRoleModuleLastUpdateTime;
            foreach (UserRolesDTO userRolesDTO in userRolesDTOList)
            {
                if (userRolesDTO.AssignedManagerRoleId > -1)
                {
                    distinctAssignedManagerRoleIdHashSet.Add(userRolesDTO.AssignedManagerRoleId);
                }
                roleIdUserRolesDTODictionary.Add(userRolesDTO.RoleId, userRolesDTO);
            }
            foreach (UserRolesDTO userRolesDTO in userRolesDTOList)
            {
                HashSet<int> assignedManagerRoleIdList = new HashSet<int>();
                BuildAssignedManagerRoleIdListRecursively(assignedManagerRoleIdList, userRolesDTO);
                if (userRoleIdAssignedManagerRoleIdHashSetDictionary.ContainsKey(userRolesDTO.RoleId) == false)
                {
                    userRoleIdAssignedManagerRoleIdHashSetDictionary.Add(userRolesDTO.RoleId, assignedManagerRoleIdList);
                }
                if (roleIdSelfApprovalAllowedDictionary.ContainsKey(userRolesDTO.RoleId) == false)
                {
                    bool selfApprovalAllowed = IsSelfApprovalAllowedForRole(userRolesDTO, assignedManagerRoleIdList);
                    roleIdSelfApprovalAllowedDictionary.Add(userRolesDTO.RoleId, selfApprovalAllowed);
                }
                UserRoleContainerDTO userRoleContainerDTO = CreateUserRoleContainerDTO(userRolesDTO, assignedManagerRoleIdList);
                userRoleContainerDTOList.Add(userRoleContainerDTO);
                if (roleIdUserRoleContainerDTODictionary.ContainsKey(userRolesDTO.RoleId) == false)
                {
                    roleIdUserRoleContainerDTODictionary.Add(userRolesDTO.RoleId, userRoleContainerDTO);
                }
                if (roleGuidUserRolesDTODictionary.ContainsKey(userRolesDTO.Guid) == false)
                {
                    roleGuidUserRolesDTODictionary.Add(userRolesDTO.Guid, userRolesDTO);
                }
            }

            foreach (UserRolesDTO userRolesDTO in userRolesDTOList)
            {
                List<int> dataAccessRoleIdList = new List<int>() { };
                if (userRolesDTO.ManagementFormAccessDTOList != null)
                {
                    var managementFormAccessDTOList = userRolesDTO.ManagementFormAccessDTOList.Where(x => x.MainMenu == "User Roles" && x.FunctionGroup == "Data Access" && x.AccessAllowed);
                    foreach (var managementFormAccessDTO in managementFormAccessDTOList)
                    {
                        if (roleGuidUserRolesDTODictionary.ContainsKey(managementFormAccessDTO.FunctionGUID))
                        {
                            dataAccessRoleIdList.Add(roleGuidUserRolesDTODictionary[managementFormAccessDTO.FunctionGUID].RoleId);
                        }
                    }
                }
                userRoleIdDataAccessRoleIdListDictionary.Add(userRolesDTO.RoleId, dataAccessRoleIdList);
            }
            userRoleContainerDTOCollection = new UserRoleContainerDTOCollection(userRoleContainerDTOList);
            log.LogMethodExit();
        }

        

        private static List<UserRolesDTO> GetUserRolesDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserRolesList userRolesList = new UserRolesList();
            List<UserRolesDTO> userRolesDTOList = null;
            try
            {
                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> searchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ISACTIVE, "1"));
                searchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, siteId.ToString()));
                userRolesDTOList = userRolesList.GetAllUserRoles(searchParameters, true, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the User Roles.", ex);
            }
            if (userRolesDTOList == null)
            {
                userRolesDTOList = new List<UserRolesDTO>();
            }
            log.LogMethodExit(userRolesDTOList);
            return userRolesDTOList;
        }

        private static DateTime? GetUserModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                UserRolesList userRolesList = new UserRolesList();
                result = userRolesList.GetUserRoleModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the system option max last update date.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool IsSelfApprovalAllowedForRole(UserRolesDTO userRolesDTO, HashSet<int> assignedManagerRoleIdList)
        {
            log.LogMethodEntry(userRolesDTO, assignedManagerRoleIdList);
            bool result = assignedManagerRoleIdList.Count == 0 &&
                (userRolesDTO.ManagerFlag == "Y" ||
                distinctAssignedManagerRoleIdHashSet.Contains(userRolesDTO.RoleId));
            return result;
        }

        public UserRoleContainerDTO GetUserRoleContainerDTO(int userRoleId)
        {
            log.LogMethodEntry(userRoleId);
            if (roleIdUserRoleContainerDTODictionary.ContainsKey(userRoleId) == false)
            {
                string errorMessage = "User role with roleId : " + userRoleId + " doesn't exist.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = roleIdUserRoleContainerDTODictionary[userRoleId];
            return result;
        }

        public UserRoleContainerDTO GetUserRoleContainerDTOOrDefault(int userRoleId)
        {
            log.LogMethodEntry(userRoleId);
            if (roleIdUserRoleContainerDTODictionary.ContainsKey(userRoleId) == false)
            {
                string message = "User role with roleId : " + userRoleId + " doesn't exist.";
                log.LogMethodExit(null, message);
                return null;
            }
            var result = roleIdUserRoleContainerDTODictionary[userRoleId];
            return result;
        }

        public bool IsSelfApprovalAllowedFor(int roleId)
        {
            log.LogMethodEntry(roleId);
            if (roleIdSelfApprovalAllowedDictionary.ContainsKey(roleId) == false)
            {
                string errorMessage = "SelfApprovalAllowed for roleId :" + roleId + " not defined.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            bool result = roleIdSelfApprovalAllowedDictionary[roleId];
            log.LogMethodExit(result);
            return result;
        }

        public List<int> GetDataAccessRoleIdList(int roleId)
        {
            log.LogMethodEntry(roleId);
            if (userRoleIdDataAccessRoleIdListDictionary.ContainsKey(roleId) == false)
            {
                string errorMessage = "Data access roleId List for roleId :" + roleId + " is not defined.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            List<int> result = userRoleIdDataAccessRoleIdListDictionary[roleId];
            log.LogMethodExit(result);
            return result;
        }

        private void BuildAssignedManagerRoleIdListRecursively(HashSet<int> assignedManagerRoleIdList, UserRolesDTO userRolesDTO)
        {
            log.LogMethodEntry(assignedManagerRoleIdList, userRolesDTO, userRolesDTOList);
            if (userRolesDTO.AssignedManagerRoleId <= -1)
            {
                log.LogMethodExit("userRolesDTO.AssignedManagerRoleId <= -1.");
                return;
            }
            if (assignedManagerRoleIdList.Contains(userRolesDTO.AssignedManagerRoleId))
            {
                log.LogMethodExit("assignedManagerRoleIdList already contains the AssignedManagerRoleId. tree has been already traversed. possible circular reference.");
                return;
            }
            assignedManagerRoleIdList.Add(userRolesDTO.AssignedManagerRoleId);
            if (roleIdUserRolesDTODictionary.ContainsKey(userRolesDTO.AssignedManagerRoleId) == false)
            {
                log.Error("AssignedManagerRoleId doesn't exist in the roleIdUserRolesDTODictionary. Possible wrong cross site references in HQ DB");
                log.LogMethodExit("AssignedManagerRoleId doesn't exist in the roleIdUserRolesDTODictionary.");
                return;
            }
            BuildAssignedManagerRoleIdListRecursively(assignedManagerRoleIdList, roleIdUserRolesDTODictionary[userRolesDTO.AssignedManagerRoleId]);
        }

        private UserRoleContainerDTO CreateUserRoleContainerDTO(UserRolesDTO userRolesDTO, HashSet<int> assignedManagerRoleIdList)
        {
            log.LogMethodEntry(userRolesDTO);
            int priceListId = -1;
            if(userRolesDTO.UserRolePriceListDTOList != null)
            {
                priceListId = userRolesDTO.UserRolePriceListDTOList.First().PriceListId;
            }
            UserRoleContainerDTO result = new UserRoleContainerDTO(userRolesDTO.RoleId, userRolesDTO.Role, userRolesDTO.Description,
                                                                    userRolesDTO.ManagerFlag == "Y", userRolesDTO.AllowPosAccess,
                                                                    userRolesDTO.DataAccessLevel, userRolesDTO.EnablePOSClockIn,
                                                                    userRolesDTO.AllowShiftOpenClose, distinctAssignedManagerRoleIdHashSet.Contains(userRolesDTO.RoleId), priceListId);
            List<ManagementFormAccessContainerDTO> managementFormAccessContainerDTOList = new List<ManagementFormAccessContainerDTO>();
            if (userRolesDTO.ManagementFormAccessDTOList != null &&
                userRolesDTO.ManagementFormAccessDTOList.Any())
            {
                foreach (var managementFormAccessDTO in userRolesDTO.ManagementFormAccessDTOList.Where(x => x.AccessAllowed == true))
                {
                    ManagementFormAccessContainerDTO managementFormAccessContainerDTO = new ManagementFormAccessContainerDTO(managementFormAccessDTO.ManagementFormAccessId, managementFormAccessDTO.RoleId, managementFormAccessDTO.MainMenu, managementFormAccessDTO.FormName, managementFormAccessDTO.FunctionGroup);
                    managementFormAccessContainerDTOList.Add(managementFormAccessContainerDTO);
                }
            }
            if (userRolesDTO.UserRoleDisplayGroupExclusionsDTOList != null && userRolesDTO.UserRoleDisplayGroupExclusionsDTOList.Any())
            {
                result.ExcludedProductIdList = ProductsContainerList.GetProductContainerDTOListOfDisplayGroups(siteId, userRolesDTO.UserRoleDisplayGroupExclusionsDTOList.Select(x => x.ProductDisplayGroupId).ToList()).Select(x => x.ProductId).ToList();
            }
            if(userRolesDTO.ProductMenuPanelExclusionDTOList != null &&
               userRolesDTO.ProductMenuPanelExclusionDTOList.Any())
            {
                result.ExcludedProductMenuPanelIdList = userRolesDTO.ProductMenuPanelExclusionDTOList.Select(x => x.PanelId).ToList();
            }
            result.ManagementFormAccessContainerDTOList = managementFormAccessContainerDTOList;
            result.AssignedManagerRoleIdList.AddRange(assignedManagerRoleIdList);
            log.LogMethodExit(result);
            return result;
        }


        public UserRoleContainerDTOCollection GetUserRoleContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(userRoleContainerDTOCollection);
            return userRoleContainerDTOCollection;
        }

        public List<UserRoleContainerDTO> GetUserRoleContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(userRoleContainerDTOCollection.UserRoleContainerDTOList);
            return userRoleContainerDTOCollection.UserRoleContainerDTOList;
        }

        public int GetSecurityPolicyId(int roleId)
        {
            log.LogMethodEntry(roleId);
            int result = -1;
            if (roleIdUserRolesDTODictionary.ContainsKey(roleId))
            {
                result = roleIdUserRolesDTODictionary[roleId].SecurityPolicyId;
            }
            log.LogMethodExit(result);
            return result;
        }

        public UserRoleContainer Refresh()
        {
            log.LogMethodEntry();
            DateTime? updateTime= GetUserRoleModuleLastUpdateTime(siteId);
            if (userRoleModuleLastUpdateTime.HasValue
                && userRoleModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in user role since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            UserRoleContainer result = new UserRoleContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }
        private static DateTime? GetUserRoleModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                UserRolesList userRolesList = new UserRolesList();
                result = userRolesList.GetUserRoleModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the user role module last update time.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        internal bool CanApproveFor(int userRoleId, int managerRoleId)
        {
            log.LogMethodEntry(userRoleId, managerRoleId);
            if (roleIdUserRoleContainerDTODictionary.ContainsKey(userRoleId) == false)
            {
                string errorMessage = "UserRole with role Id :" + userRoleId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            if (roleIdUserRoleContainerDTODictionary.ContainsKey(managerRoleId) == false)
            {
                string errorMessage = "Manager Role with role Id :" + managerRoleId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            UserRoleContainerDTO userRoleContainerDTO = roleIdUserRoleContainerDTODictionary[userRoleId];
            UserRoleContainerDTO managerUserRoleContainerDTO = roleIdUserRoleContainerDTODictionary[managerRoleId];
            bool result = false;
            if (userRoleContainerDTO.AssignedManagerRoleIdList != null &&
                userRoleContainerDTO.AssignedManagerRoleIdList.Contains(managerRoleId))
            {
                result = true;
                log.LogMethodExit(result, "userRoleContainerDTO.AssignedManagerRoleIdList.Contains(managerRoleId)");
                return result;
            }
            if ((userRoleContainerDTO.AssignedManagerRoleIdList == null ||
                userRoleContainerDTO.AssignedManagerRoleIdList.Any() == false) &&
                managerUserRoleContainerDTO.IsManagerRole)
            {
                result = true;
                log.LogMethodExit(result, "userRoleContainerDTO.AssignedManagerRoleIdList.Any() == false && managerUserRoleContainerDTO.IsManagerRole");
                return result;
            }
            if ((userRoleContainerDTO.AssignedManagerRoleIdList == null ||
                userRoleContainerDTO.AssignedManagerRoleIdList.Any() == false) &&
                managerUserRoleContainerDTO.Manager)
            {
                result = true;
                log.LogMethodExit(result, "userRoleContainerDTO.AssignedManagerRoleIdList.Any() == false && managerUserRoleContainerDTO.Manager");
                return result;
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
