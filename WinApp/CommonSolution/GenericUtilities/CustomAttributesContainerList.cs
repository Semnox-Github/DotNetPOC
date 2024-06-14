/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - CustomAttributesContainerList class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.130.0      27-Jul-2021       Mushahid Faizan    Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Timers;

namespace Semnox.Core.GenericUtilities
{
    public class CustomAttributesContainerList
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, CustomAttributesContainers> customAttributesContainerDictionary = new Cache<int, CustomAttributesContainers>();
        private static Timer refreshTimer;

        static CustomAttributesContainerList()
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
            var uniqueKeyList = customAttributesContainerDictionary.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CustomAttributesContainers customAttributesContainer;
                if (customAttributesContainerDictionary.TryGetValue(uniqueKey, out customAttributesContainer))
                {
                    customAttributesContainerDictionary[uniqueKey] = customAttributesContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static CustomAttributesContainers GetCustomAttributesContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomAttributesContainers result = customAttributesContainerDictionary.GetOrAdd(siteId, (k) => new CustomAttributesContainers(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static List<CustomAttributesContainerDTO> GetCustomAttributesContainerDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomAttributesContainers container = GetCustomAttributesContainer(siteId);
            List<CustomAttributesContainerDTO> customAttributesContainerDTOList = container.GetCustomAttributesContainerDTOList();
            log.LogMethodExit(customAttributesContainerDTOList);
            return customAttributesContainerDTOList;
        }
        /// <summary>
        /// rebulds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CustomAttributesContainers customAttributesContainer = GetCustomAttributesContainer(siteId);
            customAttributesContainerDictionary[siteId] = customAttributesContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
