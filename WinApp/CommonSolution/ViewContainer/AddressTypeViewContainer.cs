/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - AddressTypeViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    08-Jul-2021      Roshan Devadiga          Created : POS UI Redesign with REST API
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
    class AddressTypeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AddressTypeContainerDTOCollection addressTypeContainerDTOCollection;
        private readonly ConcurrentDictionary<int, AddressTypeContainerDTO> addressTypeContainerDTODictionary = new ConcurrentDictionary<int, AddressTypeContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="addressTypeContainerDTOCollection">addressTypeContainerDTOCollection</param>
        internal AddressTypeViewContainer(int siteId, AddressTypeContainerDTOCollection addressTypeContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, addressTypeContainerDTOCollection);
            this.siteId = siteId;
            this.addressTypeContainerDTOCollection = addressTypeContainerDTOCollection;
            if (addressTypeContainerDTOCollection != null &&
                addressTypeContainerDTOCollection.AddressTypeContainerDTOList != null &&
               addressTypeContainerDTOCollection.AddressTypeContainerDTOList.Any())
            {
                foreach (var addressTypeContainerDTO in addressTypeContainerDTOCollection.AddressTypeContainerDTOList)
                {
                    addressTypeContainerDTODictionary[addressTypeContainerDTO.Id] = addressTypeContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal AddressTypeViewContainer(int siteId)
              : this(siteId, GetAddressTypeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static AddressTypeContainerDTOCollection GetAddressTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            AddressTypeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IAddressTypeUseCases addressTypeUseCases = CustomerUseCaseFactory.GetAddressTypeUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<AddressTypeContainerDTOCollection> task = addressTypeUseCases.GetAddressTypeContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving AddressTypeContainerDTOCollection.", ex);
                result = new AddressTypeContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
            /// <summary>
            /// returns the latest in AddressTypeContainerDTOCollection
            /// </summary>
            /// <returns></returns>
            internal AddressTypeContainerDTOCollection GetAddressTypeContainerDTOCollection(string hash)
            {
                log.LogMethodEntry(hash);
                if (addressTypeContainerDTOCollection.Hash == hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(addressTypeContainerDTOCollection);
                return addressTypeContainerDTOCollection;
            }

            internal List<AddressTypeContainerDTO> GetAddressTypeContainerDTOList()
            {
                log.LogMethodEntry();
                log.LogMethodExit(addressTypeContainerDTOCollection.AddressTypeContainerDTOList);
                return addressTypeContainerDTOCollection.AddressTypeContainerDTOList;
            }
            internal AddressTypeViewContainer Refresh(bool rebuildCache)
            {
                log.LogMethodEntry();
                if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
                {
                    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                    return this;
                }
                LastRefreshTime = DateTime.Now;
            AddressTypeContainerDTOCollection latestAddressTypeContainerDTOCollection = GetAddressTypeContainerDTOCollection(siteId, addressTypeContainerDTOCollection.Hash, rebuildCache);
                if (latestAddressTypeContainerDTOCollection == null ||
                    latestAddressTypeContainerDTOCollection.AddressTypeContainerDTOList == null ||
                    latestAddressTypeContainerDTOCollection.AddressTypeContainerDTOList.Any() == false)
                {
                    log.LogMethodExit(this, "No changes to the cache");
                    return this;
                }
                AddressTypeViewContainer result = new AddressTypeViewContainer(siteId, latestAddressTypeContainerDTOCollection);
                log.LogMethodExit(result);
                return result;
            }
        }
    }