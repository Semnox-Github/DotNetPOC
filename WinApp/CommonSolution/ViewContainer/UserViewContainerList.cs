/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - UserViewContainerList holds multiple  UserView containers based on siteId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// UserViewContainerList holds multiple  UserView containers based on siteId
    /// </summary>
    public class UserViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, UserViewContainer> userViewContainerDictionary = new Cache<int, UserViewContainer>();
        private static Timer refreshTimer;

        static UserViewContainerList()
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
            var uniqueKeyList = userViewContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                UserViewContainer userViewContainer;
                if (userViewContainerDictionary.TryGetValue(uniqueKey, out userViewContainer))
                {
                    userViewContainerDictionary[uniqueKey] = userViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static UserViewContainer GetUserViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserViewContainer result = userViewContainerDictionary.GetOrAdd(siteId, (k)=>new UserViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the UserContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static UserContainerDTOCollection GetUserContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            UserViewContainer userViewContainer = GetUserViewContainer(siteId); 
            UserContainerDTOCollection result = userViewContainer.GetUserContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the user container dto for the given siteId and loginId
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="loginId"></param>
        /// <returns></returns>
        public static UserContainerDTO GetUserContainerDTO(int siteId, string loginId)
        {
            log.LogMethodEntry(siteId, loginId);
            UserViewContainer userViewContainer = GetUserViewContainer(siteId); 
            var result = userViewContainer.GetUserContainerDTO(loginId);
            log.LogMethodExit(result);
            return result;
        }
        public static UserContainerDTO GetUserContainerDTO(int siteId, int userId)
        {
            log.LogMethodEntry(siteId, userId);
            UserViewContainer userViewContainer = GetUserViewContainer(siteId);
            var result = userViewContainer.GetUserContainerDTO(userId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserViewContainer userViewContainer = GetUserViewContainer(siteId);
            userViewContainerDictionary[siteId] = userViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the user role container DTO list 
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static List<UserContainerDTO> GetUserContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            UserViewContainer userViewContainer = GetUserViewContainer(executionContext.SiteId);
            List<UserContainerDTO> result = userViewContainer.GetUserContainerDTOList();
            log.LogMethodExit(result);
            return result;
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
            UserViewContainer userViewContainer = GetUserViewContainer(siteId);
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
            UserViewContainer userViewContainer = GetUserViewContainer(siteId);
            var result = userViewContainer.CanApproveFor(userId, managerId);
            log.LogMethodExit(result);
            return result;
        }
        
    }
}
