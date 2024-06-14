
/********************************************************************************************
 * Project Name - Utilities
 * Description  - UserRoleViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
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
    /// UserRoleViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
    /// </summary>
    public class UserRoleViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly UserRoleContainerDTOCollection userRoleContainerDTOCollection;
        private readonly ConcurrentDictionary<int, UserRoleContainerDTO> roleIdUserRoleContainerDTODictionary = new ConcurrentDictionary<int, UserRoleContainerDTO>();
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="userRoleContainerDTOCollection">userRoleContainerDTOCollection</param>
        internal UserRoleViewContainer(int siteId, UserRoleContainerDTOCollection userRoleContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, userRoleContainerDTOCollection);
            this.siteId = siteId;
            this.userRoleContainerDTOCollection = userRoleContainerDTOCollection;
            if (userRoleContainerDTOCollection != null &&
                userRoleContainerDTOCollection.UserRoleContainerDTOList != null &&
                userRoleContainerDTOCollection.UserRoleContainerDTOList.Any())
            {
                foreach (var userRoleContainerDTO in userRoleContainerDTOCollection.UserRoleContainerDTOList)
                {
                    roleIdUserRoleContainerDTODictionary[userRoleContainerDTO.RoleId] = userRoleContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal UserRoleViewContainer(int siteId)
            :this(siteId, GetUserRoleContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static UserRoleContainerDTOCollection GetUserRoleContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            //log.Error("Start UserRoleViewContainer " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            UserRoleContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IUserRoleUseCases userRoleUseCases = UserUseCaseFactory.GetUserRoleUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<UserRoleContainerDTOCollection> task = userRoleUseCases.GetUserRoleContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
                //log.Error("End UserRoleViewContainer " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving UserRoleContainerDTOCollection.", ex);
                result = new UserRoleContainerDTOCollection();
            }
            
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in UserRoleContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal UserRoleContainerDTOCollection GetUserRoleContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (userRoleContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(userRoleContainerDTOCollection);
            return userRoleContainerDTOCollection;
        }

        internal UserRoleViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            UserRoleContainerDTOCollection latestUserRoleContainerDTOCollection = GetUserRoleContainerDTOCollection(siteId, userRoleContainerDTOCollection.Hash, rebuildCache);
            if (latestUserRoleContainerDTOCollection == null || 
                latestUserRoleContainerDTOCollection.UserRoleContainerDTOList == null ||
                latestUserRoleContainerDTOCollection.UserRoleContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            UserRoleViewContainer result = new UserRoleViewContainer(siteId, latestUserRoleContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

        internal List<UserRoleContainerDTO> GetUserRoleContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(userRoleContainerDTOCollection.UserRoleContainerDTOList);
            return userRoleContainerDTOCollection.UserRoleContainerDTOList;
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
            if(userRoleContainerDTO.AssignedManagerRoleIdList != null &&
                userRoleContainerDTO.AssignedManagerRoleIdList.Contains(managerRoleId))
            {
                result = true;
                log.LogMethodExit(result, "userRoleContainerDTO.AssignedManagerRoleIdList.Contains(managerRoleId)");
                return result;
            }
            if((userRoleContainerDTO.AssignedManagerRoleIdList == null ||
                userRoleContainerDTO.AssignedManagerRoleIdList.Any() == false) &&
                managerUserRoleContainerDTO.IsManagerRole)
            {
                result = true;
                log.LogMethodExit(result, "userRoleContainerDTO.AssignedManagerRoleIdList.Any() == false && managerUserRoleContainerDTO.IsManagerRole");
                return result;
            }
            if((userRoleContainerDTO.AssignedManagerRoleIdList == null ||
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
        internal bool CheckAccess(int userRoleId, string formName, string menuName=null)
        {
            log.LogMethodEntry();
            bool result = false;
            if (roleIdUserRoleContainerDTODictionary.ContainsKey(userRoleId) == false)
            {
                string errorMessage = "UserRole with role Id :" + userRoleId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            UserRoleContainerDTO userRoleContainerDTO = roleIdUserRoleContainerDTODictionary[userRoleId];
            if (userRoleContainerDTO.ManagementFormAccessContainerDTOList != null && userRoleContainerDTO.ManagementFormAccessContainerDTOList.Any())
            {
                if (menuName!=null)
                {
                    if (userRoleContainerDTO.ManagementFormAccessContainerDTOList.Any(x => x.FormName.ToLower() == formName.ToLower() && x.MainMenu.ToLower() == menuName.ToLower()))
                    {
                        result = true;
                    }
                }
                else
                {
                    if (userRoleContainerDTO.ManagementFormAccessContainerDTOList.Any(x => x.FormName.ToLower() == formName.ToLower()))
                    {
                        result = true;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        internal UserRoleContainerDTO GetUserRoleContainerDTO(int roleId)
        {
            log.LogMethodEntry(roleId);
            if (roleIdUserRoleContainerDTODictionary.ContainsKey(roleId) == false)
            {
                string errorMessage = "UserRoles with role Id :" + roleId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            var result = roleIdUserRoleContainerDTODictionary[roleId];
            log.LogMethodExit(result);
            return result;
        }
    }
}
