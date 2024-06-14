/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - CustomerUIMetadataViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    09-Jul-2021      Roshan Devadiga          Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    class CustomerUIMetadataViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CustomerUIMetadataContainerDTOCollection customerUIMetadataContainerDTOCollection;
        private readonly ConcurrentDictionary<int, CustomerUIMetadataContainerDTO> customerUIMetadataContainerDTODictionary = new ConcurrentDictionary<int, CustomerUIMetadataContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="customerUIMetadataContainerDTOCollection">customerUIMetadataContainerDTOCollection</param>
        internal CustomerUIMetadataViewContainer(int siteId, CustomerUIMetadataContainerDTOCollection customerUIMetadataContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, customerUIMetadataContainerDTOCollection);
            this.siteId = siteId;
            this.customerUIMetadataContainerDTOCollection = customerUIMetadataContainerDTOCollection;
            if (customerUIMetadataContainerDTOCollection != null &&
                customerUIMetadataContainerDTOCollection.CustomerUIMetadataContainerDTOList != null &&
               customerUIMetadataContainerDTOCollection.CustomerUIMetadataContainerDTOList.Any())
            {
                foreach (var customerUIMetadataContainerDTO in customerUIMetadataContainerDTOCollection.CustomerUIMetadataContainerDTOList)
                {
                    customerUIMetadataContainerDTODictionary[customerUIMetadataContainerDTO.CustomAttributeId] = customerUIMetadataContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal CustomerUIMetadataViewContainer(int siteId)
              : this(siteId, GetCustomerUIMetadataContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static CustomerUIMetadataContainerDTOCollection GetCustomerUIMetadataContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            CustomerUIMetadataContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ICustomerUIMetadataUseCases customerUIMetadataUseCases = CustomerUseCaseFactory.GetCustomerUIMetadataUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<CustomerUIMetadataContainerDTOCollection> task = customerUIMetadataUseCases.GetCustomerUIMetadataContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving CustomerUIMetadataontainerDTOCollection.", ex);
                result = new CustomerUIMetadataContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in CustomerUIMetadataContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal CustomerUIMetadataContainerDTOCollection GetCustomerUIMetadataContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (customerUIMetadataContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(customerUIMetadataContainerDTOCollection);
            return customerUIMetadataContainerDTOCollection;
        }

        internal List<CustomerUIMetadataContainerDTO> GetCustomerUIMetadataContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(customerUIMetadataContainerDTOCollection.CustomerUIMetadataContainerDTOList);
            return customerUIMetadataContainerDTOCollection.CustomerUIMetadataContainerDTOList;
        }
        internal CustomerUIMetadataViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            CustomerUIMetadataContainerDTOCollection latestCustomerUIMetadataContainerDTOCollection = GetCustomerUIMetadataContainerDTOCollection(siteId, customerUIMetadataContainerDTOCollection.Hash, rebuildCache);
            if (latestCustomerUIMetadataContainerDTOCollection == null ||
                latestCustomerUIMetadataContainerDTOCollection.CustomerUIMetadataContainerDTOList == null ||
                latestCustomerUIMetadataContainerDTOCollection.CustomerUIMetadataContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            CustomerUIMetadataViewContainer result = new CustomerUIMetadataViewContainer(siteId, latestCustomerUIMetadataContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}