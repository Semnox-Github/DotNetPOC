/********************************************************************************************
 * Project Name - ContainerView
 * Description  - AddressTypeViewContainerList holds multiple  AddressTypeView containers based on siteId, userId and POSMachineId
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    08-Jul-2021      Roshan Devadiga           Created : POS UI Redesign with REST API
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
    /// AddressTypeViewContainerList
    /// </summary>
    public class AddressTypeViewContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, AddressTypeViewContainer> addressTypeViewContainerCache = new Cache<int, AddressTypeViewContainer>();
        private static Timer refreshTimer;

        static AddressTypeViewContainerList()
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
            var uniqueKeyList = addressTypeViewContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                AddressTypeViewContainer addressTypeViewContainer;
                if (addressTypeViewContainerCache.TryGetValue(uniqueKey, out addressTypeViewContainer))
                {
                    addressTypeViewContainerCache[uniqueKey] = addressTypeViewContainer.Refresh(false);
                }
            }
            log.LogMethodExit();
        }
        private static AddressTypeViewContainer GetAddressTypeViewContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            var result = addressTypeViewContainerCache.GetOrAdd(siteId, (k) => new AddressTypeViewContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the POSMachineContainerDTOList for a given context
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <returns></returns>
        public static List<AddressTypeContainerDTO> GetAddressTypeContainerDTOList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            AddressTypeViewContainer addressTypeViewContainer = GetAddressTypeViewContainer(executionContext.SiteId);
            List<AddressTypeContainerDTO> result = addressTypeViewContainer.GetAddressTypeContainerDTOList();
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
        public static AddressTypeContainerDTOCollection GetAddressTypeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            if (rebuildCache)
            {
                Rebuild(siteId);
            }
            AddressTypeViewContainer container = GetAddressTypeViewContainer(siteId);
            AddressTypeContainerDTOCollection addressTypeContainerDTOCollection = container.GetAddressTypeContainerDTOCollection(hash);
            return addressTypeContainerDTOCollection;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            AddressTypeViewContainer container = GetAddressTypeViewContainer(siteId);
            addressTypeViewContainerCache[siteId] = container.Refresh(true);
            log.LogMethodExit();
        }
    }
}
