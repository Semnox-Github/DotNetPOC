/********************************************************************************************
 * Project Name - Product
 * Description  - TaxList class to get the List of Tax from the container API
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0     19-Jan-2022       Prajwal S                Created :
 ********************************************************************************************/

using System;
using System.Timers;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Product
{
    public static class TaxContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, TaxContainer> taxContainerCache = new Cache<int, TaxContainer>();
        private static Timer refreshTimer;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        static TaxContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            refreshTimer.Start();
            log.LogMethodExit();
        }

        /// <summary>
        /// Container Refresh after elapsing the time set for refresh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            var uniqueKeyList = taxContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                TaxContainer taxContainer;
                if (taxContainerCache.TryGetValue(uniqueKey, out taxContainer))
                {
                    taxContainerCache[uniqueKey] = taxContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Container Data for the Site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static TaxContainer GetTaxContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            TaxContainer result = taxContainerCache.GetOrAdd(siteId, (k) => new TaxContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }

        public static TaxContainerDTOCollection GetTaxContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            TaxContainer container = GetTaxContainer(siteId);
            TaxContainerDTOCollection result = container.GetTaxContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the TaxContainerDTO based on the site and taxId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="taxId">option value taxId</param>
        /// <returns></returns>
        public static TaxContainerDTO GetTaxContainerDTO(int siteId, int taxId)
        {
            log.LogMethodEntry(siteId, taxId);
            TaxContainer taxContainer = GetTaxContainer(siteId);
            TaxContainerDTO result = taxContainer.GetTaxContainerDTO(taxId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the TaxContainerDTO based on the execution context and taxId
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="taxId">option value taxId</param>
        /// <returns></returns>
        public static TaxContainerDTO GetTaxContainerDTO(ExecutionContext executionContext, int taxId)
        {
            log.LogMethodEntry(executionContext, taxId);
            TaxContainerDTO result = GetTaxContainerDTO(executionContext.SiteId, taxId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            TaxContainer taxContainer = GetTaxContainer(siteId);
            taxContainerCache[siteId] = taxContainer.Refresh();
            log.LogMethodExit();
        }

    }
}
