/********************************************************************************************
* Project Name - Customer
* Description  - AddressTypeContainerList class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    08-Jul-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
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
    public class AddressTypeContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, AddressTypeContainer> addressTypeContainerDictionary = new Cache<int, AddressTypeContainer>();
        private static Timer refreshTimer;

        static AddressTypeContainerList()
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
            var uniqueKeyList = addressTypeContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                AddressTypeContainer addressTypeContainer;
                if (addressTypeContainerDictionary.TryGetValue(uniqueKey, out addressTypeContainer))
                {
                    addressTypeContainerDictionary[uniqueKey] = addressTypeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static AddressTypeContainer GetAddressTypeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            AddressTypeContainer result = addressTypeContainerDictionary.GetOrAdd(siteId, (k) => new AddressTypeContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<AddressTypeContainerDTO> GetAddressTypeContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            AddressTypeContainer container = GetAddressTypeContainer(siteId);
            List<AddressTypeContainerDTO> addressTypeContainerDTOList = container.GetAddressTypeContainerDTOList();
            log.LogMethodExit(addressTypeContainerDTOList);
            return addressTypeContainerDTOList;
        }
        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            AddressTypeContainer addressTypeContainer = GetAddressTypeContainer(siteId);
            addressTypeContainerDictionary[siteId] = addressTypeContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
