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
 2.120.0      18-Mar-2021      Guru S A                  For Subscription phase 2 changes
*2.140.0      01-Jun-2021      Fiona Lishal              Modified for Delivery Order enhancements for F&B and UrbanPiper
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// Container class caches user roles for a given site
    /// </summary>
    public class UserContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<int, UsersDTO> userIdUsersDTODictionary = new Dictionary<int, UsersDTO>();
        private readonly Dictionary<int, UserContainerDTO> userIdUserContainerDTODictionary = new Dictionary<int, UserContainerDTO>();
        private readonly Dictionary<string, UserContainerDTO> loginIdUserContainerDTODictionary = new Dictionary<string, UserContainerDTO>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, UserContainerDTO> tagNumberUserContainerDTODictionary = new Dictionary<string, UserContainerDTO>(StringComparer.OrdinalIgnoreCase);
        private readonly List<UsersDTO> usersDTOList;
        private readonly UserContainerDTOCollection userContainerDTOCollection;
        private readonly DateTime? userModuleLastUpdateTime;
        private readonly int siteId;

        public UserContainer(int siteId) :
            this(siteId, GetUserDTOList(siteId), GetUserModuleLastUpdateTime(siteId))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        public UserContainer(int siteId, List<UsersDTO> usersList, DateTime? userModuleLastUpdateTime)
        {
            log.LogMethodEntry(siteId);
            this.siteId = siteId;
            this.usersDTOList = usersList;
            this.userModuleLastUpdateTime = userModuleLastUpdateTime;
            List<UserContainerDTO> userContainerDTOList = new List<UserContainerDTO>();
            foreach (UsersDTO usersDTO in usersDTOList)
            {
                userIdUsersDTODictionary.Add(usersDTO.UserId, usersDTO);

                UserContainerDTO userContainerDTO = CreateUserContainerDTO(usersDTO);
                userContainerDTOList.Add(userContainerDTO);
                loginIdUserContainerDTODictionary.Add(usersDTO.LoginId.ToLower(), userContainerDTO);
                userIdUserContainerDTODictionary.Add(usersDTO.UserId, userContainerDTO);
                if (usersDTO.UserIdentificationTagsDTOList != null &&
                    usersDTO.UserIdentificationTagsDTOList.Any())
                {
                    foreach (var userIdentificationTagsDTO in usersDTO.UserIdentificationTagsDTOList)
                    {
                        if (string.IsNullOrWhiteSpace(userIdentificationTagsDTO.CardNumber))
                        {
                            continue;
                        }
                        if (tagNumberUserContainerDTODictionary.ContainsKey(userIdentificationTagsDTO.CardNumber.ToLower()) ||
                            (userIdentificationTagsDTO.StartDate != DateTime.MinValue && userIdentificationTagsDTO.StartDate > ServerDateTime.Now) ||
                            (userIdentificationTagsDTO.EndDate != DateTime.MinValue && userIdentificationTagsDTO.EndDate < ServerDateTime.Now))
                        {
                            continue;
                        }
                        tagNumberUserContainerDTODictionary.Add(userIdentificationTagsDTO.CardNumber.ToLower(), userContainerDTO);
                    }
                }
            }
            userContainerDTOCollection = new UserContainerDTOCollection(userContainerDTOList);
            log.LogMethodExit();
        }

        private static List<UsersDTO> GetUserDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<UsersDTO> result = null;
            try
            {
                UsersList usersList = new UsersList();
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "1"));
                searchParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteId.ToString()));
                result = usersList.GetAllUsers(searchParameters, true);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the user DTO List.", ex);
            }
            if(result == null)
            {
                result = new List<UsersDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }

        private static DateTime? GetUserModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            DateTime? result = null;
            try
            {
                UsersList usersList = new UsersList();
                result = usersList.GetUserModuleLastUpdateTime(siteId);
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving the user module last update time.", ex);
                result = null;
            }
            log.LogMethodExit(result);
            return result;
        }

        public UserContainerDTO GetUserContainerDTO(string loginId)
        {
            log.LogMethodEntry(loginId);
            if (loginIdUserContainerDTODictionary.ContainsKey(loginId.ToLower()) == false)
            {
                string errorMessage = "User with logiId :" + loginId + " is not found.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = loginIdUserContainerDTODictionary[loginId.ToLower()];
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Get User Container DTO by Id
        /// </summary>
        /// <param name="userPKId"></param>
        /// <returns></returns>
        public UserContainerDTO GetUserContainerDTO(int userPKId)
        {
            log.LogMethodEntry(userPKId);
            if (userIdUserContainerDTODictionary.ContainsKey(userPKId) == false)
            {
                string errorMessage = "User with userPKId :" + userPKId + " is not found.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = userIdUserContainerDTODictionary[userPKId];
            log.LogMethodExit(result);
            return result;
        }

        private UserContainerDTO CreateUserContainerDTO(UsersDTO usersDTO)
        {
            log.LogMethodEntry(usersDTO);
            bool selfApprovalAllowed = IsSelfApprovalAllowedFor(usersDTO);
            UserContainerDTO result = new UserContainerDTO(usersDTO.UserId, usersDTO.UserName, usersDTO.EmpLastName,usersDTO.LoginId,
                                                            usersDTO.RoleId, usersDTO.ManagerId, usersDTO.SiteId, usersDTO.PosTypeId,
                                                            usersDTO.Guid, selfApprovalAllowed,usersDTO.PhoneNumber);
            if (usersDTO.UserIdentificationTagsDTOList != null &&
                usersDTO.UserIdentificationTagsDTOList.Any())
            {
                foreach (var userIdentificationTagsDTO in usersDTO.UserIdentificationTagsDTOList)
                {
                    result.UserIdentificationTagContainerDTOList.Add(new UserIdentificationTagContainerDTO(userIdentificationTagsDTO.Id,
                                                                                                           userIdentificationTagsDTO.CardNumber,
                                                                                                           userIdentificationTagsDTO.CardId,
                                                                                                           userIdentificationTagsDTO.StartDate,
                                                                                                           userIdentificationTagsDTO.EndDate));
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool IsSelfApprovalAllowedFor(UsersDTO usersDTO)
        {
            log.LogMethodEntry();
            var result = UserRoleContainerList.IsSelfApprovalAllowedFor(siteId, usersDTO.RoleId);
            log.LogMethodExit(result);
            return result;
        }

        public UserContainerDTO GetUserContainerDTOOrDefault(string loginId, string tagNumber)
        {
            log.LogMethodEntry(loginId, tagNumber);
            UserContainerDTO result = null;
            if (string.IsNullOrWhiteSpace(loginId) == false && loginIdUserContainerDTODictionary.ContainsKey(loginId.ToLower()))
            {
                result = loginIdUserContainerDTODictionary[loginId.ToLower()];
            }
            else if (string.IsNullOrWhiteSpace(tagNumber) == false && tagNumberUserContainerDTODictionary.ContainsKey(tagNumber.ToLower()))
            {
                result = tagNumberUserContainerDTODictionary[tagNumber.ToLower()];
            }

            log.LogMethodExit(result);
            return result;
        }

        public UserContainerDTOCollection GetUserContainerDTOCollection()
        {
            log.LogMethodEntry();
            log.LogMethodExit(userContainerDTOCollection);
            return userContainerDTOCollection;
        }

        public UserContainer Refresh()
        {
            log.LogMethodEntry();
            DateTime? updateTime = GetUserModuleLastUpdateTime(siteId);
            if (userModuleLastUpdateTime.HasValue
                && userModuleLastUpdateTime >= updateTime)
            {
                log.LogMethodExit(this, "No changes in user since " + updateTime.Value.ToString(CultureInfo.InvariantCulture));
                return this;
            }
            UserRoleContainerList.Rebuild(siteId);
            UserContainer result = new UserContainer(siteId);
            log.LogMethodExit(result);
            return result;
        }

        public bool IsSelfApprovalAllowed(int userId)
        {
            log.LogMethodEntry(userId);
            if (userIdUserContainerDTODictionary.ContainsKey(userId) == false)
            {
                string errorMessage = "User with user Id :" + userId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            bool result = userIdUserContainerDTODictionary[userId].SelfApprovalAllowed;
            log.LogMethodExit(result);
            return result;
        }

        public bool CanApproveFor(int userId, int managerId)
        {
            log.LogMethodEntry(userId);
            if (userIdUserContainerDTODictionary.ContainsKey(userId) == false)
            {
                string errorMessage = "User with user Id :" + userId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            if (userIdUserContainerDTODictionary.ContainsKey(managerId) == false)
            {
                string errorMessage = "Manager with user Id :" + managerId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            UserContainerDTO userRoleContainerDTO = userIdUserContainerDTODictionary[userId];
            UserContainerDTO managerUserRoleContainerDTO = userIdUserContainerDTODictionary[managerId];
            bool result = UserRoleContainerList.CanApproveFor(siteId, userRoleContainerDTO.RoleId, managerUserRoleContainerDTO.RoleId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
