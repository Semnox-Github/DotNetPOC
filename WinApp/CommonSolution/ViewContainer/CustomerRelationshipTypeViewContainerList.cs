/********************************************************************************************
* Project Name - ViewContainer
* Description  - CustomerRelationshipTypeViewContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    31-Aug-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System.Timers;
using System.Collections.Generic;
using Semnox.Parafait.Customer;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerRelationshipTypeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CustomerRelationshipTypeViewContainer> customerRelationshipTypeViewContainerCache = new Cache<int, CustomerRelationshipTypeViewContainer>();
        private static Timer refreshTimer;

        static CustomerRelationshipTypeViewContainerList()
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
            var uniqueKeyList = customerRelationshipTypeViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CustomerRelationshipTypeViewContainer customerRelationshipTypeViewContainer;
                if (customerRelationshipTypeViewContainerCache.TryGetValue(uniqueKey, out customerRelationshipTypeViewContainer))
                {
                    customerRelationshipTypeViewContainerCache[uniqueKey] = customerRelationshipTypeViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static CustomerRelationshipTypeViewContainer GetCustomerRelationshipTypeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = customerRelationshipTypeViewContainerCache.GetOrAdd(siteId, (k) => new CustomerRelationshipTypeViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the CustomerRelationshipTypeContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<CustomerRelationshipTypeContainerDTO> GetCustomerRelationshipTypeContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            CustomerRelationshipTypeViewContainer customerRelationshipTypeViewContainer = GetCustomerRelationshipTypeViewContainer(executionContext.SiteId);
            List<CustomerRelationshipTypeContainerDTO> result = customerRelationshipTypeViewContainer.GetCustomerRelationshipTypeContainerDTOList();
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
        public static CustomerRelationshipTypeContainerDTOCollection GetCustomerRelationshipTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            CustomerRelationshipTypeViewContainer container = GetCustomerRelationshipTypeViewContainer(siteId);
            CustomerRelationshipTypeContainerDTOCollection customerRelationshipTypeContainerDTOCollection = container.GetCustomerRelationshipTypeContainerDTOCollection(hash);
            return customerRelationshipTypeContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CustomerRelationshipTypeViewContainer container = GetCustomerRelationshipTypeViewContainer(siteId);
            customerRelationshipTypeViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
