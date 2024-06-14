
/********************************************************************************************
* Project Name - GenericUtilities
* Description  - CustomAttributeContainerList
* 
**************
**Version Log
**************
*Version      Date            Modified By         Remarks          
*********************************************************************************************
*2.130.0     28-Jul-2020      Girish Kundar              Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System.Timers;

namespace Semnox.Core.GenericUtilities
{
    public class CustomAttributeContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static Timer refreshTimer;
        private static readonly Cache<int, CustomAttributeContainer> CustomAttributeContainerCache = new Cache<int, CustomAttributeContainer>();


        static CustomAttributeContainerList()
        {
            log.LogMethodEntry();
            refreshTimer = new Timer(DataRefreshFrequency.GetValue());
            refreshTimer.Elapsed += OnRefreshTimer;
            log.LogMethodExit();
        }

        private static void OnRefreshTimer(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry();
            var uniqueKeyList = CustomAttributeContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                CustomAttributeContainer CustomAttributeContainer;
                if (CustomAttributeContainerCache.TryGetValue(uniqueKey, out CustomAttributeContainer))
                {
                    CustomAttributeContainerCache[uniqueKey] = CustomAttributeContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private static CustomAttributeContainer GetCustomAttributeContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomAttributeContainer result = CustomAttributeContainerCache.GetOrAdd(siteId, (k)=> new CustomAttributeContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        
        public static CustomAttributeContainerDTOCollection GetCustomAttributeContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            CustomAttributeContainer container = GetCustomAttributeContainer(siteId);
            CustomAttributeContainerDTOCollection result = container.GetCustomAttributesContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }

        public static CustomAttributesContainerDTO GetCustomAttributeContainerDTO(ExecutionContext executionContext, int attributeId)
        {
            log.LogMethodEntry(executionContext, attributeId);
            log.LogMethodExit();
            return GetCustomAttributeContainerDTO(executionContext.SiteId, attributeId);
        }

        public static CustomAttributesContainerDTO GetCustomAttributeContainerDTO(int siteId, int attributeId)
        {
            log.LogMethodEntry(siteId, attributeId);
            CustomAttributeContainer container = GetCustomAttributeContainer(siteId);
            var result = container.GetCustomAttributesContainerDTO(attributeId);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// rebuilds the container
        /// </summary>
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            CustomAttributeContainer CustomAttributeContainer = GetCustomAttributeContainer(siteId);
            CustomAttributeContainerCache[siteId] = CustomAttributeContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
