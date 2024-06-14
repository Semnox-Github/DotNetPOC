/********************************************************************************************
 * Project Name - Device  
 * Description  - EnabledAttributesContainerList class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      20-Aug-2021      Fiona                    Created 
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
    public class EnabledAttributesContainerList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Cache<int, EnabledAttributesContainer> enabledAttributesContainerCache = new Cache<int, EnabledAttributesContainer>();
        private static Timer refreshTimer;
        static EnabledAttributesContainerList()
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
            var uniqueKeyList = enabledAttributesContainerCache.Keys;
            foreach (var uniqueKey in uniqueKeyList)
            {
                EnabledAttributesContainer enabledAttributesContainer;
                if (enabledAttributesContainerCache.TryGetValue(uniqueKey, out enabledAttributesContainer))
                {
                    enabledAttributesContainerCache[uniqueKey] = enabledAttributesContainer.Refresh();
                }
            }
            log.LogMethodExit();
        }
        private static EnabledAttributesContainer GetEnabledAttributesContainer(int siteId)
        {
            log.LogMethodEntry(siteId);
            EnabledAttributesContainer result = enabledAttributesContainerCache.GetOrAdd(siteId, (k) => new EnabledAttributesContainer(siteId));
            log.LogMethodExit(result);
            return result;
        }
        public static EnabledAttributesContainerDTOCollection GetEnabledAttributesContainerDTOCollection(int siteId)
        {
            log.LogMethodEntry(siteId);
            EnabledAttributesContainer container = GetEnabledAttributesContainer(siteId);
            EnabledAttributesContainerDTOCollection result = container.GetEnabledAttributesContainerDTOCollection();
            log.LogMethodExit(result);
            return result;
        }
        public static EnabledAttributesContainerDTO GetEnabledAttributesContainerDTO(ExecutionContext executionContext, int Id)
        {
            log.LogMethodEntry(executionContext, Id);
            log.LogMethodExit();
            return GetEnabledAttributesContainerDTO(executionContext.SiteId, Id);
        }
        public static EnabledAttributesContainerDTO GetEnabledAttributesContainerDTO(int siteId, int Id)
        {
            log.LogMethodEntry(siteId, Id);
            EnabledAttributesContainer container = GetEnabledAttributesContainer(siteId);
            var result = container.GetEnabledAttributesContainerDTO(Id);
            log.LogMethodExit(result);
            return result;
        }
        public static void Rebuild(int siteId)
        {
            log.LogMethodEntry(siteId);
            EnabledAttributesContainer enabledAttributesContainer = GetEnabledAttributesContainer(siteId);
            enabledAttributesContainerCache[siteId] = enabledAttributesContainer.Refresh();
            log.LogMethodExit();
        }
    }
}
