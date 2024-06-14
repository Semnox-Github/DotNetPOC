/********************************************************************************************
 * Project Name - Customer
 * Description  - CustomerRelationshipTypeContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      19-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Parafait.Customer
{
    class CustomerRelationshipTypeContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CustomerRelationshipTypeContainer> customerRelationshipTypeContainerDictionary = new Cache<int, CustomerRelationshipTypeContainer>();
        private static Timer refreshTimer;

        static CustomerRelationshipTypeContainerList()
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
            var uniqueKeyList = customerRelationshipTypeContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CustomerRelationshipTypeContainer customerRelationshipTypeContainer;
                if (customerRelationshipTypeContainerDictionary.TryGetValue(uniqueKey, out customerRelationshipTypeContainer))
                {
                    customerRelationshipTypeContainerDictionary[uniqueKey] = customerRelationshipTypeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static CustomerRelationshipTypeContainer GetCustomerRelationshipTypeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomerRelationshipTypeContainer result = customerRelationshipTypeContainerDictionary.GetOrAdd(siteId, (k) => new CustomerRelationshipTypeContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<CustomerRelationshipTypeContainerDTO> GetCustomerRelationshipTypeContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomerRelationshipTypeContainer container = GetCustomerRelationshipTypeContainer(siteId);
            List<CustomerRelationshipTypeContainerDTO> customerRelationshipTypeContainerDTOList = container.GetCustomerRelationshipTypeContainerDTOList();
            log.LogMethodExit(customerRelationshipTypeContainerDTOList);
            return customerRelationshipTypeContainerDTOList;
        }

        public static CustomerRelationshipTypeContainerDTOCollection GetCustomerRelationshipTypeContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomerRelationshipTypeContainer container = GetCustomerRelationshipTypeContainer(siteId);
            CustomerRelationshipTypeContainerDTOCollection result = container.GetCustomerRelationshipTypeContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the CustomerRelationshipTypeContainerDTO based on the site and customerRelationshipTypeId
        /// </summary>
        /// <param name="siteId">site Id</param>
        /// <param name="customerRelationshipTypeId">option value customerRelationshipTypeId</param>
        /// <returns></returns>
        public static CustomerRelationshipTypeContainerDTO GetCustomerRelationshipTypeContainerDTO(int siteId, int customerRelationshipTypeId)
        {
            log.LogMethodEntry(siteId, customerRelationshipTypeId);
            CustomerRelationshipTypeContainer customerRelationshipTypeContainer = GetCustomerRelationshipTypeContainer(siteId);
            CustomerRelationshipTypeContainerDTO result = customerRelationshipTypeContainer.GetCustomerRelationshipTypeContainerDTO(customerRelationshipTypeId);
            log.LogMethodExit();
            return result;
        }

        /// <summary>
        /// Returns the CustomerRelationshipTypeContainerDTO based on the execution context
        /// </summary>
        /// <param name="executionContext">current application execution context</param>
        /// <param name="customerRelationshipTypeId">customerRelationshipTypeId</param>
        /// <returns></returns>
        public static CustomerRelationshipTypeContainerDTO GetCustomerRelationshipTypeContainerDTO(ExecutionContext executionContext, int customerRelationshipTypeId)
        {
            log.LogMethodEntry(executionContext, customerRelationshipTypeId);
            CustomerRelationshipTypeContainerDTO customerRelationshipTypeContainerDTO = GetCustomerRelationshipTypeContainerDTO(executionContext.GetSiteId(), customerRelationshipTypeId);
            log.LogMethodExit(customerRelationshipTypeContainerDTO);
            return customerRelationshipTypeContainerDTO;
        }

        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CustomerRelationshipTypeContainer customerRelationshipTypeContainer = GetCustomerRelationshipTypeContainer(siteId);
            customerRelationshipTypeContainerDictionary[siteId] = customerRelationshipTypeContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
