/********************************************************************************************
 * Project Name - Site
 * Description  - RoamingSiteMasterList holds the roamingSite container
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 *2.110.0     21-Dec-2020      Lakshminarayana           Created for POS UI Redesign.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.Site
{
    /// <summary>
    /// Holds the roamingSite container object
    /// </summary>
    public class RoamingSiteMasterList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static RoamingSiteContainer roamingSiteContainer;
        private static Timer refreshTimer;

        static RoamingSiteMasterList()
        {
            log.LogMethodEntry();
            roamingSiteContainer = new RoamingSiteContainer();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            roamingSiteContainer = roamingSiteContainer.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the whether the give site id is roaming site
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <returns></returns>
        public static bool IsRoamingSite(int siteId)
        {
            log.LogMethodEntry();
            var result = roamingSiteContainer.IsRoamingSite(siteId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild()
        {
            log.LogMethodEntry();
            roamingSiteContainer = roamingSiteContainer.Refresh();
            log.LogMethodExit();
        }

    }
}
