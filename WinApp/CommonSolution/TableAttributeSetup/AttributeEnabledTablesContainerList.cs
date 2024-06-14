/********************************************************************************************
 * Project Name - TableAttributeSetup  
 * Description  - AttributeEnabledTablesContainerList class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      24-Aug-2021      Fiona                     Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.TableAttributeSetup
{
    public class AttributeEnabledTablesContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, AttributeEnabledTablesContainer> attributeEnabledTablesContainerCache = new Cache<int, AttributeEnabledTablesContainer>();
        private static Timer refreshTimer;
        static AttributeEnabledTablesContainerList()
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
            var uniqueKeyList = attributeEnabledTablesContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                AttributeEnabledTablesContainer attributeEnabledTablesContainer;
                if (attributeEnabledTablesContainerCache.TryGetValue(uniqueKey, out attributeEnabledTablesContainer))
                {
                    attributeEnabledTablesContainerCache[uniqueKey] = attributeEnabledTablesContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        
        private static AttributeEnabledTablesContainer GetAttributeEnabledTablesContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            AttributeEnabledTablesContainer result = attributeEnabledTablesContainerCache.GetOrAdd(siteId, (k) => new AttributeEnabledTablesContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static AttributeEnabledTablesContainerDTOCollection GetAttributeEnabledTablesContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            AttributeEnabledTablesContainer container = GetAttributeEnabledTablesContainer(siteId);
            AttributeEnabledTablesContainerDTOCollection result = container.GetPaymentModeContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }
        public static AttributeEnabledTablesContainerDTO GetAttributeEnabledTablesContainerDTO(ExecutionContext executionContext, int Id)
        {
            log.LogMethodEntry(executionContext, Id);
            log.LogMethodExit();
            return GetAttributeEnabledTablesContainerDTO(executionContext.SiteId, Id);
        }
        public static AttributeEnabledTablesContainerDTO GetAttributeEnabledTablesContainerDTO(int siteId, int Id)
        {
            log.LogMethodEntry(siteId, Id);
            AttributeEnabledTablesContainer container = GetAttributeEnabledTablesContainer(siteId);
            var result = container.GetAttributeEnabledTablesContainerDTO(Id);
            log.LogMethodExit(result);
            return result;
        }
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry();
            AttributeEnabledTablesContainer attributeEnabledTablesContainer = GetAttributeEnabledTablesContainer(siteId);
            attributeEnabledTablesContainerCache[siteId] = attributeEnabledTablesContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
