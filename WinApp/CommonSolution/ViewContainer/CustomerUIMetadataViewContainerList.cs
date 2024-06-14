/********************************************************************************************
 * Project Name - ContainerView
 * Description  - CustomerUIMetadataViewContainerList holds multiple  CustomerUIMetadataView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    09-Jul-2021      Roshan Devadiga           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// CustomerUIMetadataViewContainerList
    /// </summary>
    public class CustomerUIMetadataViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CustomerUIMetadataViewContainer> customerUIMetadataViewContainerCache = new Cache<int, CustomerUIMetadataViewContainer>();
        private static Timer refreshTimer;

        static CustomerUIMetadataViewContainerList()
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
            var uniqueKeyList = customerUIMetadataViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CustomerUIMetadataViewContainer customerUIMetadataViewContainer;
                if (customerUIMetadataViewContainerCache.TryGetValue(uniqueKey, out customerUIMetadataViewContainer))
                {
                    customerUIMetadataViewContainerCache[uniqueKey] = customerUIMetadataViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static CustomerUIMetadataViewContainer GetCustomerUIMetadataViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = customerUIMetadataViewContainerCache.GetOrAdd(siteId, (k) => new CustomerUIMetadataViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<CustomerUIMetadataContainerDTO> GetCustomerUIMetadataContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            CustomerUIMetadataViewContainer customerUIMetadataViewContainer = GetCustomerUIMetadataViewContainer(executionContext.SiteId);
            List<CustomerUIMetadataContainerDTO> result = customerUIMetadataViewContainer.GetCustomerUIMetadataContainerDTOList();
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public static CustomerUIMetadataContainerDTOCollection GetCustomerUIMetadataContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            CustomerUIMetadataViewContainer container = GetCustomerUIMetadataViewContainer(siteId);
            CustomerUIMetadataContainerDTOCollection customerUIMetadataContainerDTOCollection = container.GetCustomerUIMetadataContainerDTOCollection(hash);
            return customerUIMetadataContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CustomerUIMetadataViewContainer container = GetCustomerUIMetadataViewContainer(siteId);
            customerUIMetadataViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
