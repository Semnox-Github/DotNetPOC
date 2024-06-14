
/********************************************************************************************
 * Project Name - Utilities
 * Description  - UserViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// UserViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
    /// </summary>
    public class UserViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly UserContainerDTOCollection userContainerDTOCollection;
        private readonly ConcurrentDictionary<int, UserContainerDTO> userIdUserContainerDTODictionary = new ConcurrentDictionary<int, UserContainerDTO>();
        private readonly ConcurrentDictionary<string, UserContainerDTO> loginIdUserContainerDTODictionary = new ConcurrentDictionary<string, UserContainerDTO>();
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="userContainerDTOCollection">userContainerDTOCollection</param>
        internal UserViewContainer(int siteId, UserContainerDTOCollection userContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, userContainerDTOCollection);
            this.siteId = siteId;
            this.userContainerDTOCollection = userContainerDTOCollection;
            if (userContainerDTOCollection != null &&
                userContainerDTOCollection.UserContainerDTOList != null &&
                userContainerDTOCollection.UserContainerDTOList.Any())
            {
                foreach (var userContainerDTO in userContainerDTOCollection.UserContainerDTOList)
                {
                    userIdUserContainerDTODictionary[userContainerDTO.UserId] = userContainerDTO;
                    loginIdUserContainerDTODictionary[userContainerDTO.LoginId.ToLower()] = userContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal UserViewContainer(int siteId)
            :this(siteId, GetUserContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static UserContainerDTOCollection GetUserContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            UserContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IUserUseCases userUseCases = UserUseCaseFactory.GetUserUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<UserContainerDTOCollection> task = userUseCases.GetUserContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving UserContainerDTOCollection.", ex);
                result = new UserContainerDTOCollection();
            }
            
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in UserContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal UserContainerDTOCollection GetUserContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (userContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(userContainerDTOCollection);
            return userContainerDTOCollection;
        }

        internal UserContainerDTO GetUserContainerDTO(string loginId)
        {
            log.LogMethodEntry(loginId);
            if (loginIdUserContainerDTODictionary.ContainsKey(loginId.ToLower()) == false)
            {
                string errorMessage = "User with login Id :" + loginId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = loginIdUserContainerDTODictionary[loginId.ToLower()];
            log.LogMethodExit(result);
            return result;
        }
        internal UserContainerDTO GetUserContainerDTO(int userId)
        {
            log.LogMethodEntry(userId);
            if (userIdUserContainerDTODictionary.ContainsKey(userId) == false)
            {
                string errorMessage = "User with user Id :" + userId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = userIdUserContainerDTODictionary[userId];
            log.LogMethodExit(result);
            return result;
        }
        internal UserViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            UserContainerDTOCollection latestUserContainerDTOCollection = GetUserContainerDTOCollection(siteId, userContainerDTOCollection.Hash, rebuildCache);
            if (latestUserContainerDTOCollection == null || 
                latestUserContainerDTOCollection.UserContainerDTOList == null ||
                latestUserContainerDTOCollection.UserContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            UserViewContainer result = new UserViewContainer(siteId, latestUserContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

        internal List<UserContainerDTO> GetUserContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(userContainerDTOCollection.UserContainerDTOList);
            return userContainerDTOCollection.UserContainerDTOList;
        }

        internal bool IsSelfApprovalAllowed(int userId)
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

        internal bool CanApproveFor(int userId, int managerId)
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
            bool result = UserRoleViewContainerList.CanApproveFor(siteId, userRoleContainerDTO.RoleId, managerUserRoleContainerDTO.RoleId);
            log.LogMethodExit(result);
            return result;
        }


        

    }
}
