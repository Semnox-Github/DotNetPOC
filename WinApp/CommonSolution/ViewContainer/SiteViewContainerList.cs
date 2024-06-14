/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - Holds the site container object
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
  2.150.0     09-Mar-2022       Lakshminarayana         Modified : Added GetTimeZoneName() as a part of SiteDateTime Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Timers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the site container object
    /// </summary>
    public class SiteViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static SiteViewContainer siteViewContainer;
        private static Timer refreshTimer;
        static SiteViewContainerList()
        {
            log.LogMethodEntry();
            siteViewContainer = new SiteViewContainer();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            siteViewContainer = siteViewContainer.Refresh(false);
            log.LogMethodExit();
        }

        /// <summary>
        /// return the current site container DTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static SiteContainerDTO GetCurrentSiteContainerDTO(ExecutionContext executionContext)
        {
            return GetCurrentSiteContainerDTO(executionContext.SiteId);
        }

        /// <summary>
        /// return the current site container DTO
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static SiteContainerDTO GetCurrentSiteContainerDTO(int siteId)
        {
            log.LogMethodEntry();
            SiteContainerDTO siteContainerDTO = siteViewContainer.GetSiteContainerDTO(siteId);
            log.LogMethodExit(siteContainerDTO);
            return siteContainerDTO;
        }

        /// <summary>
        /// return the all site container DTO s
        /// </summary>
        /// <returns></returns>
        public static List<SiteContainerDTO> GetSiteContainerDTOList()
        {
            log.LogMethodEntry();
            List<SiteContainerDTO> siteContainerDTOList = siteViewContainer.GetSiteContainerDTOList();
            log.LogMethodExit(siteContainerDTOList);
            return siteContainerDTOList;
        }

        /// <summary>
        /// Returns whether this is a HQ environment
        /// </summary>
        /// <returns></returns>
        public static bool IsCorporate()
        {
            log.LogMethodEntry();
            var result = siteViewContainer.IsCorporate();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the site id of the master site. 
        /// </summary>
        /// <returns></returns>
        public static int GetMasterSiteId()
        {
            log.LogMethodEntry();
            int result = siteViewContainer.GetMasterSiteId();
            log.LogMethodExit(result);
            return result;
        }


        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild()
        {
            log.LogMethodEntry();
            siteViewContainer = siteViewContainer.Refresh(true);
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the site time zone
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <returns>siteTimeZone</returns>
        internal static string GetTimeZoneName(int siteId)
        {
            log.LogMethodEntry(siteId);
            SiteContainerDTO siteContainerDTO = GetCurrentSiteContainerDTO(siteId);
            string result = siteContainerDTO.TimeZoneName;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the site time zone
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <returns>siteTimeZone</returns>
        internal static string GetTimeZoneName(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            SiteContainerDTO siteContainerDTO = GetCurrentSiteContainerDTO(executionContext.GetSiteId());
            string result = siteContainerDTO.TimeZoneName;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the SiteContainerDTOCollection for a given hash
        /// If the hash matches then no data is returned as the caller is holding the latest data
        /// </summary>
        /// <param name="hash">hash</param>
        /// <param name="onlineEnabledOnly"></param>
        /// <param name="fnBEnabledOnly"></param>
        /// <returns></returns>
        public static SiteContainerDTOCollection GetSiteContainerDTOCollection(string hash, bool onlineEnabledOnly, bool fnBEnabledOnly)
        {
            log.LogMethodEntry(hash, onlineEnabledOnly, fnBEnabledOnly);
            SiteContainerDTOCollection result = siteViewContainer.GetSiteContainerDTOCollection(hash, onlineEnabledOnly, fnBEnabledOnly);
            log.LogMethodExit(result);
            return result;
        }
    }
}
