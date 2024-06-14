/********************************************************************************************
 * Project Name - ApplicationContent
 * Description  - ApplicationContentContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      20-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Core.GenericUtilities
{
    public class ApplicationContentContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, ApplicationContentContainer> applicationContentContainerDictionary = new Cache<int, ApplicationContentContainer>();
        private static Timer refreshTimer;

        static ApplicationContentContainerList()
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
            var uniqueKeyList = applicationContentContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                ApplicationContentContainer applicationContentContainer;
                if (applicationContentContainerDictionary.TryGetValue(uniqueKey, out applicationContentContainer))
                {
                    applicationContentContainerDictionary[uniqueKey] = applicationContentContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static ApplicationContentContainer GetApplicationContentContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            ApplicationContentContainer result = applicationContentContainerDictionary.GetOrAdd(siteId, (k) => new ApplicationContentContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static ApplicationContentContainerDTOCollection GetApplicationContentContainerDTOCollection(int siteId, int languageId)
        {
            ApplicationContentContainer container = GetApplicationContentContainer(siteId);
            return container.GetApplicationContentContainerDTOCollection(languageId);
        }

        public static List<ApplicationContentContainerDTO> GetApplicationContentContainerDTOList(int siteId, int languageId)
        {
            log.LogMethodEntry(siteId);
            ApplicationContentContainer container = GetApplicationContentContainer(siteId);
            List<ApplicationContentContainerDTO> applicationContentContainerDTOList = container.GetApplicationContentContainerDTOList(languageId);
            log.LogMethodExit(applicationContentContainerDTOList);
            return applicationContentContainerDTOList;
        }
        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            ApplicationContentContainer applicationContentContainer = GetApplicationContentContainer(siteId);
            applicationContentContainerDictionary[siteId] = applicationContentContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ApplicationContentContainerDTO based on the site and applicationContentId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="applicationContentId">option value applicationContentId</param>
        /// <returns></returns>
        public static ApplicationContentContainerDTO GetApplicationContentContainerDTO(int siteId, int applicationContentId)
        {
            log.LogMethodEntry(siteId, applicationContentId);
            ApplicationContentContainer applicationContentContainer = GetApplicationContentContainer(siteId);
            ApplicationContentContainerDTO result = applicationContentContainer.GetApplicationContentContainerDTO(applicationContentId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the ApplicationContentContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="applicationContentId">applicationContentId</param>
        /// <returns></returns>
        public static ApplicationContentContainerDTO GetApplicationContentContainerDTO(ExecutionContext executionContext, int applicationContentId)
        {
            log.LogMethodEntry(executionContext, applicationContentId);
            ApplicationContentContainerDTO applicationContentContainerDTO = GetApplicationContentContainerDTO(executionContext.GetSiteId(), applicationContentId);
            log.LogMethodExit(applicationContentContainerDTO);
            return applicationContentContainerDTO;
        }
    }
}
