/********************************************************************************************
* Project Name - ViewContainer
* Description  - WaiverSetViewContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    20-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Parafait.Waiver;

namespace Semnox.Parafait.ViewContainer
{
    class WaiverSetViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly WaiverSetContainerDTOCollection waiverSetContainerDTOCollection;
        private readonly ConcurrentDictionary<int, WaiverSetContainerDTO> waiverSetContainerDTODictionary = new ConcurrentDictionary<int, WaiverSetContainerDTO>();
        private readonly int siteId;
        private readonly int languageId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="languageId">languageId</param>
        /// <param name="waiverSetContainerDTOCollection">waiverSetContainerDTOCollection</param>
        internal WaiverSetViewContainer(int siteId, int languageId, WaiverSetContainerDTOCollection waiverSetContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, waiverSetContainerDTOCollection);
            this.siteId = siteId;
            this.languageId = languageId;
            this.waiverSetContainerDTOCollection = waiverSetContainerDTOCollection;
            if (waiverSetContainerDTOCollection != null &&
                waiverSetContainerDTOCollection.WaiverSetContainerDTOList != null &&
               waiverSetContainerDTOCollection.WaiverSetContainerDTOList.Any())
            {
                foreach (var waiverSetContainerDTO in waiverSetContainerDTOCollection.WaiverSetContainerDTOList)
                {
                    waiverSetContainerDTODictionary[waiverSetContainerDTO.WaiverSetId] = waiverSetContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal WaiverSetViewContainer(int siteId, int languageId)
              : this(siteId, languageId,GetWaiverSetContainerDTOCollection(siteId, languageId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static WaiverSetContainerDTOCollection GetWaiverSetContainerDTOCollection(int siteId, int languageId,string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            WaiverSetContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IWaiverSetUseCases waiverSetUseCases = WaiverUseCaseFactory.GetWaiverSetUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<WaiverSetContainerDTOCollection> task = waiverSetUseCases.GetWaiverSetContainerDTOCollection(siteId, languageId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving WaiverSetContainerDTOCollection.", ex);
                result = new WaiverSetContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in WaiverSetContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal WaiverSetContainerDTOCollection GetWaiverSetContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (waiverSetContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(waiverSetContainerDTOCollection);
            return waiverSetContainerDTOCollection;
        }
        internal List<WaiverSetContainerDTO> GetWaiverSetContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(waiverSetContainerDTOCollection.WaiverSetContainerDTOList);
            return waiverSetContainerDTOCollection.WaiverSetContainerDTOList;
        }
        internal WaiverSetViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            WaiverSetContainerDTOCollection latestWaiverSetContainerDTOCollection = GetWaiverSetContainerDTOCollection(siteId, languageId,waiverSetContainerDTOCollection.Hash, rebuildCache);
            if (latestWaiverSetContainerDTOCollection == null ||
                latestWaiverSetContainerDTOCollection.WaiverSetContainerDTOList == null ||
                latestWaiverSetContainerDTOCollection.WaiverSetContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            WaiverSetViewContainer result = new WaiverSetViewContainer(siteId, languageId,latestWaiverSetContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
