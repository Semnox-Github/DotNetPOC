/********************************************************************************************
* Project Name - Customer
* Description  - CustomerUIMetadataContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    09-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Semnox.Parafait.Customer
{
    public class CustomerUIMetadataContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CustomerUIMetadataContainer> customerUIMetadataContainerDictionary = new Cache<int, CustomerUIMetadataContainer>();
        private static Timer refreshTimer;

        static CustomerUIMetadataContainerList()
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
            var uniqueKeyList = customerUIMetadataContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CustomerUIMetadataContainer customerUIMetadataContainer;
                if (customerUIMetadataContainerDictionary.TryGetValue(uniqueKey, out customerUIMetadataContainer))
                {
                    customerUIMetadataContainerDictionary[uniqueKey] = customerUIMetadataContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static CustomerUIMetadataContainer GetCustomerUIMetadataContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomerUIMetadataContainer result = customerUIMetadataContainerDictionary.GetOrAdd(siteId, (k) => new CustomerUIMetadataContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<CustomerUIMetadataContainerDTO> GetCustomerUIMetadataContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomerUIMetadataContainer container = GetCustomerUIMetadataContainer(siteId);
            List<CustomerUIMetadataContainerDTO> customerUIMetadataContainerDTOList = container.GetCustomerUIMetadataContainerDTOList();
            log.LogMethodExit(customerUIMetadataContainerDTOList);
            return customerUIMetadataContainerDTOList;
        }
        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CustomerUIMetadataContainer customerUIMetadataContainer = GetCustomerUIMetadataContainer(siteId);
            customerUIMetadataContainerDictionary[siteId] = customerUIMetadataContainer.Refresh();
            log.LogMethodExit();
        }
    }
}

