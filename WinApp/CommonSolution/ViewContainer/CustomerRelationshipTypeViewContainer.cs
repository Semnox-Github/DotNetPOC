/********************************************************************************************
* Project Name - ViewContainer
* Description  - CustomerRelationshipTypeViewContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    31-Aug-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomerRelationshipTypeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CustomerRelationshipTypeContainerDTOCollection customerRelationshipTypeContainerDTOCollection;
        private readonly ConcurrentDictionary<int, CustomerRelationshipTypeContainerDTO> customerRelationshipTypeContainerDTODictionary = new ConcurrentDictionary<int, CustomerRelationshipTypeContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="customerRelationshipTypeContainerDTOCollection">customerRelationshipTypeContainerDTOCollection</param>
        internal CustomerRelationshipTypeViewContainer(int siteId, CustomerRelationshipTypeContainerDTOCollection customerRelationshipTypeContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, customerRelationshipTypeContainerDTOCollection);
            this.siteId = siteId;
            this.customerRelationshipTypeContainerDTOCollection = customerRelationshipTypeContainerDTOCollection;
            if (customerRelationshipTypeContainerDTOCollection != null &&
                customerRelationshipTypeContainerDTOCollection.CustomerRelationshipTypeContainerDTOList != null &&
               customerRelationshipTypeContainerDTOCollection.CustomerRelationshipTypeContainerDTOList.Any())
            {
                foreach (var customerRelationshipTypeContainerDTO in customerRelationshipTypeContainerDTOCollection.CustomerRelationshipTypeContainerDTOList)
                {
                    customerRelationshipTypeContainerDTODictionary[customerRelationshipTypeContainerDTO.CustomerRelationshipTypeId] = customerRelationshipTypeContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal CustomerRelationshipTypeViewContainer(int siteId)
              : this(siteId, GetCustomerRelationshipTypeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static CustomerRelationshipTypeContainerDTOCollection GetCustomerRelationshipTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            CustomerRelationshipTypeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ICustomerRelationshipTypeUseCases customerRelationshipTypeUseCases = CustomerUseCaseFactory.GetCustomerRelationshipTypeUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<CustomerRelationshipTypeContainerDTOCollection> task = customerRelationshipTypeUseCases.GetCustomerRelationshipTypeContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving CustomerRelationshipTypeContainerDTOCollection.", ex);
                result = new CustomerRelationshipTypeContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in CustomerRelationshipTypeContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal CustomerRelationshipTypeContainerDTOCollection GetCustomerRelationshipTypeContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (customerRelationshipTypeContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(customerRelationshipTypeContainerDTOCollection);
            return customerRelationshipTypeContainerDTOCollection;
        }
        internal List<CustomerRelationshipTypeContainerDTO> GetCustomerRelationshipTypeContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(customerRelationshipTypeContainerDTOCollection.CustomerRelationshipTypeContainerDTOList);
            return customerRelationshipTypeContainerDTOCollection.CustomerRelationshipTypeContainerDTOList;
        }
        internal CustomerRelationshipTypeViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            CustomerRelationshipTypeContainerDTOCollection latestCustomerRelationshipTypeContainerDTOCollection = GetCustomerRelationshipTypeContainerDTOCollection(siteId, customerRelationshipTypeContainerDTOCollection.Hash, rebuildCache);
            if (latestCustomerRelationshipTypeContainerDTOCollection == null ||
                latestCustomerRelationshipTypeContainerDTOCollection.CustomerRelationshipTypeContainerDTOList == null ||
                latestCustomerRelationshipTypeContainerDTOCollection.CustomerRelationshipTypeContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            CustomerRelationshipTypeViewContainer result = new CustomerRelationshipTypeViewContainer(siteId, latestCustomerRelationshipTypeContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
