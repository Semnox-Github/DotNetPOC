/********************************************************************************************
 * Project Name - Utilities
 * Description  - UserMasterList class holds multiple user role container instances
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.110.0     27-NOV-2020      Lakshminarayana     Created: POS Redesign
 *2.120.0     18-Mar-2021      Guru S A            For Subscription phase 2 changes
 ********************************************************************************************/

using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.User
{
    public static class UserContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, UserContainer> userContainerCache = new Cache<int, UserContainer>();
        private static Timer refreshTimer;

        static UserContainerList()
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
            var uniqueKeyList = userContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                UserContainer userContainer;
                if (userContainerCache.TryGetValue(uniqueKey, out userContainer))
                {
                    userContainerCache[uniqueKey] = userContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        internal static UserContainerDTO GetUserContainerDTO(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            UserContainer userContainer = GetUserContainer(executionContext.SiteId);
            var result = userContainer.GetUserContainerDTO(executionContext.UserId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get User Container DTO by site id and user id
        /// </summary>
        public static UserContainerDTO GetUserContainerDTO(ExecutionContext executionContext, int userPKId)
        {
            log.LogMethodEntry(executionContext, userPKId);
            var result = GetUserContainerDTO(executionContext.SiteId, userPKId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get User Container DTO by site id and user id
        /// </summary>
        public static UserContainerDTO GetUserContainerDTO(ExecutionContext executionContext, string loginId)
        {
            log.LogMethodEntry(executionContext, loginId);
            var result = GetUserContainerDTO(executionContext.SiteId, loginId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get User Container DTO by site id and user id
        /// </summary> 
        public static UserContainerDTO GetUserContainerDTO(int siteId, int userPKId)
        {
            log.LogMethodEntry(siteId, userPKId);
            UserContainer userContainer = GetUserContainer(siteId);
            var result = userContainer.GetUserContainerDTO(userPKId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get User Container DTO by site id and user id
        /// </summary>
        public static UserContainerDTO GetUserContainerDTO(int siteId, string loginId)
        {
            log.LogMethodEntry(siteId, loginId);
            UserContainer userContainer = GetUserContainer(siteId);
            var result = userContainer.GetUserContainerDTO(loginId);
            log.LogMethodExit(result);
            return result;
        }

        private static UserContainer GetUserContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserContainer result = userContainerCache.GetOrAdd(siteId, (k)=>new UserContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static UserContainerDTO GetUserContainerDTOOrDefault(string loginId, string tagNumber, int siteId)
        {
            log.LogMethodEntry(loginId, tagNumber, siteId);
            UserContainerDTO result = null;
            if (siteId > -1)
            {
                UserContainer userContainer = GetUserContainer(siteId);
                result = userContainer.GetUserContainerDTOOrDefault(loginId, tagNumber);
            }
            else
            {
                if (SiteContainerList.IsCorporate())
                {
                    foreach (var siteContainerDTO in SiteContainerList.GetSiteContainerDTOList())
                    {
                        UserContainer userContainer = GetUserContainer(siteContainerDTO.SiteId);
                        result = userContainer.GetUserContainerDTOOrDefault(loginId, tagNumber);
                        if(result != null)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    UserContainer userContainer = GetUserContainer(-1);
                    result = userContainer.GetUserContainerDTOOrDefault(loginId, tagNumber);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the UserContainerDTOCollection for the given site
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static UserContainerDTOCollection GetUserContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserContainer container = GetUserContainer(siteId);
            UserContainerDTOCollection result = container.GetUserContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            UserContainer userContainer = GetUserContainer(siteId);
            userContainerCache[siteId] = userContainer.Refresh();
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns whether user can self approve the task which requires manager approval 
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static bool IsSelfApprovalAllowed(ExecutionContext executionContext)
        {
            return IsSelfApprovalAllowed(executionContext.SiteId, executionContext.UserPKId);
        }

        /// <summary>
        /// Returns whether user can self approve the task which requires manager approval 
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="userId">user primary key id</param>
        /// <returns></returns>
        public static bool IsSelfApprovalAllowed(int siteId, int userId)
        {
            log.LogMethodEntry(siteId, userId);
            UserContainer userViewContainer = GetUserContainer(siteId);
            var result = userViewContainer.IsSelfApprovalAllowed(userId);
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// Returns whether user can self approve the task which requires manager approval 
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="managerId">manager id</param>
        /// <returns></returns>
        public static bool CanApproveFor(ExecutionContext executionContext, int managerId)
        {
            return CanApproveFor(executionContext.SiteId, executionContext.UserPKId, managerId);
        }

        /// <summary>
        /// Validates whether the manager can approve for the given user 
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="userId">user id</param>
        /// <param name="managerId">manager id</param>
        /// <returns></returns>
        public static bool CanApproveFor(int siteId, int userId, int managerId)
        {
            log.LogMethodEntry(siteId, userId, managerId);
            UserContainer userViewContainer = GetUserContainer(siteId);
            var result = userViewContainer.CanApproveFor(userId, managerId);
            log.LogMethodExit(result);
            return result;
        }

    }
}
