/********************************************************************************************
 * Project Name - Utilities
 * Description  - UserRoleMasterList class holds multiple user role container instances
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020   Lakshminarayana     Created: POS Redesign
 *2.130.0        12-Jul-2021    Lakshminarayana      Modified : Static menu enhancement
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public static class UserRoleContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, UserRoleContainer> userRoleContainerDictionary = new Cache<int, UserRoleContainer>();
        private static Timer refreshTimer;

        static UserRoleContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e) 
        {
            log.LogMethodEntry();
            var uniqueKeyList = userRoleContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                UserRoleContainer userRoleContainer;
                if (userRoleContainerDictionary.TryGetValue(uniqueKey, out userRoleContainer))
                {
                    userRoleContainerDictionary[uniqueKey] = userRoleContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static UserRoleContainer GetUserRoleContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserRoleContainer result = userRoleContainerDictionary.GetOrAdd(siteId, (k)=>new UserRoleContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static UserRoleContainerDTOCollection GetUserRoleContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserRoleContainer container = GetUserRoleContainer(siteId);
            UserRoleContainerDTOCollection result = container.GetUserRoleContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        public static List<UserRoleContainerDTO> GetUserRoleContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserRoleContainer container = GetUserRoleContainer(siteId);
            var result = container.GetUserRoleContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }

        public static UserRoleContainerDTO GetUserRoleContainerDTOOrDefault(int siteId, int userRoleId)
        {
            log.LogMethodEntry(siteId);
            UserRoleContainer container = GetUserRoleContainer(siteId);
            var result = container.GetUserRoleContainerDTOOrDefault(userRoleId);
            log.LogMethodExit(result);
            return result;
        }

        public static UserRoleContainerDTO GetUserRoleContainerDTO(int siteId, int userRoleId)
        {
            log.LogMethodEntry(siteId);
            UserRoleContainer container = GetUserRoleContainer(siteId);
            var result = container.GetUserRoleContainerDTO(userRoleId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            UserRoleContainer userRoleContainer = GetUserRoleContainer(siteId);
            userRoleContainerDictionary[siteId] = userRoleContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the SecurityPolicyId of the given role
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <returns></returns>
        public static int GetSecurityPolicyId(int siteId, int roleId)
        {
            log.LogMethodEntry(roleId);
            UserRoleContainer userRoleContainer = GetUserRoleContainer(siteId);
            int result = userRoleContainer.GetSecurityPolicyId(roleId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the SecurityPolicyId of the given role
        /// </summary>
        /// <param name="roleId">Role Id</param>
        /// <returns></returns>
        public static int GetSecurityPolicyId(ExecutionContext executionContext, int roleId)
        {
            return GetSecurityPolicyId(executionContext.SiteId, roleId);
        }

        /// <summary>
        /// Returns the whether self approval allowed for the role
        /// </summary>
        /// <param name="siteId">Site Id</param>
        /// <param name="roleId">Role Id</param>
        /// <returns></returns>
        public static bool IsSelfApprovalAllowedFor(int siteId, int roleId)
        {
            log.LogMethodEntry(roleId);
            UserRoleContainer userRoleContainer = GetUserRoleContainer(siteId);
            var result = userRoleContainer.IsSelfApprovalAllowedFor(roleId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the data access allowed role list for a given role
        /// </summary>
        /// <param name="siteId">Site Id</param>
        /// <param name="roleId">Role Id</param>
        /// <returns></returns>
        public static List<int> GetDataAccessRoleIdList(int siteId, int roleId)
        {
            log.LogMethodEntry(roleId);
            UserRoleContainer userRoleContainer = GetUserRoleContainer(siteId);
            var result = userRoleContainer.GetDataAccessRoleIdList(roleId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Validates whether the manager role can approve for the given user role
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="userRoleId">user role id</param>
        /// <param name="managerRoleId">manager role id</param>
        /// <returns></returns>
        public static bool CanApproveFor(int siteId, int userRoleId, int managerRoleId)
        {
            log.LogMethodEntry(siteId, userRoleId, managerRoleId);
            UserRoleContainer userRoleContainer = GetUserRoleContainer(siteId);
            var result = userRoleContainer.CanApproveFor(userRoleId, managerRoleId);
            log.LogMethodExit(result);
            return result;
        }

        
    }
}
