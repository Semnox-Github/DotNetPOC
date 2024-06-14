/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - UserRoleViewContainerList holds multiple  UserRoleView containers based on siteId
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
    /// UserRoleViewContainerList holds multiple  UserRoleView containers based on siteId
    /// </summary>
    public class UserRoleViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, UserRoleViewContainer> userRoleViewContainerCache = new Cache<int, UserRoleViewContainer>();
        private static Timer refreshTimer;

        static UserRoleViewContainerList()
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
            var uniqueKeyList = userRoleViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                UserRoleViewContainer userRoleViewContainer;
                if (userRoleViewContainerCache.TryGetValue(uniqueKey, out userRoleViewContainer))
                {
                    userRoleViewContainerCache[uniqueKey] = userRoleViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }

        private static UserRoleViewContainer GetUserRoleViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserRoleViewContainer result = userRoleViewContainerCache.GetOrAdd(siteId, (k)=> new UserRoleViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the UserRoleContainerDTOCollection for a given siteId and hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="hash">hash</param>
        /// <returns></returns>
        public static UserRoleContainerDTOCollection GetUserRoleContainerDTOCollection(int siteId, string hash)
        {
            log.LogMethodEntry(siteId, hash);
            UserRoleViewContainer userRoleViewContainer = GetUserRoleViewContainer(siteId); 
            UserRoleContainerDTOCollection result = userRoleViewContainer.GetUserRoleContainerDTOCollection(hash);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            UserRoleViewContainer userRoleViewContainer = GetUserRoleViewContainer(siteId);
            userRoleViewContainerCache[siteId] = userRoleViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the user role container DTO list 
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <returns></returns>
        public static List<UserRoleContainerDTO> GetUserRoleContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            UserRoleViewContainer userRoleViewContainer = GetUserRoleViewContainer(executionContext.SiteId);
            List<UserRoleContainerDTO> result = userRoleViewContainer.GetUserRoleContainerDTOList();
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
            UserRoleViewContainer userRoleViewContainer = GetUserRoleViewContainer(siteId);
            var result = userRoleViewContainer.CanApproveFor(userRoleId, managerRoleId);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// CheckAccess
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userRoleId"></param>
        /// <param name="formName"></param>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public static bool CheckAccess(int siteId, int userRoleId, string formName, string menuName=null)
        {
            log.LogMethodEntry(userRoleId, formName, menuName);
            UserRoleViewContainer userRoleViewContainer = GetUserRoleViewContainer(siteId);
            var result = userRoleViewContainer.CheckAccess(userRoleId, formName, menuName);
            log.LogMethodEntry();
            return result;
        }



        /// <summary>
        /// Returns the UserRoleContainerDTO for given siteId and roleId
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static UserRoleContainerDTO GetUserRoleContainerDTO(int siteId, int roleId)
        {
            log.LogMethodEntry(siteId, roleId);
            UserRoleViewContainer userRoleViewContainer = GetUserRoleViewContainer(siteId);
            UserRoleContainerDTO result = userRoleViewContainer.GetUserRoleContainerDTO(roleId);
            log.LogMethodExit(result);
            return result;
        }
    }
}
