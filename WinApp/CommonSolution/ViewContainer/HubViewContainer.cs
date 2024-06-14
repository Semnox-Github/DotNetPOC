/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - HubViewContainer class
 *
 **************
 ** Version Log
  **************
  * Version        Date Modified     By                  Remarks
 *********************************************************************************************
 2.110.0           10-Nov-2020       Prajwal S           Created : POS UI Redesign with REST API
 ********************************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the list of Hubs
    /// </summary>
    public class HubViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly HubContainerDTOCollection hubDTOCollection;
        private readonly ConcurrentDictionary<int, HubContainerDTO> hubContainerDTODictionary = new ConcurrentDictionary<int, HubContainerDTO>();
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal HubViewContainer(int siteId, HubContainerDTOCollection hubDTOCollection)
        {
            log.LogMethodEntry(siteId, hubDTOCollection);
            this.siteId = siteId;
            this.hubDTOCollection = hubDTOCollection;
            //lastRefreshTime = DateTime.Now;
            if (hubDTOCollection != null &&
               hubDTOCollection.HubContainerDTOList != null &&
               hubDTOCollection.HubContainerDTOList.Any())
            {
                foreach (var hubContainerDTO in hubDTOCollection.HubContainerDTOList)
                {
                    hubContainerDTODictionary[hubContainerDTO.MasterId] = hubContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal HubViewContainer(int siteId) :
            this(siteId, GetHubContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static HubContainerDTOCollection GetHubContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            HubContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IHubUseCases hubUseCases = GameUseCaseFactory.GetHubUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<HubContainerDTOCollection> hubViewDTOCollectionTask = hubUseCases.GetHubContainerDTOCollection(siteId, hash, rebuildCache);
                    hubViewDTOCollectionTask.Wait();
                    result = hubViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving HubContainerDTOCollection.", ex);
                result = new HubContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in HubDTOCollection
        /// </summary>
        /// <returns></returns>
        internal HubContainerDTOCollection GetHubDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (hubDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(hubDTOCollection);
            return hubDTOCollection;
        }

        internal List<HubContainerDTO> GetHubContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(hubDTOCollection.HubContainerDTOList);
            return hubDTOCollection.HubContainerDTOList;
        }

        /// <summary>
        /// returns the HubContainerDTO for the HubId
        /// </summary>
        /// <param name="HubId"></param>
        /// <returns></returns>
        public HubContainerDTO GetHubContainerDTO(int HubId)
        {
            log.LogMethodEntry(HubId);
            if (hubContainerDTODictionary.ContainsKey(HubId) == false)
            {
                string errorMessage = "Hub with Hub Id :" + HubId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            HubContainerDTO result = hubContainerDTODictionary[HubId];
            log.LogMethodExit(result);
            return result;
        }


        internal HubViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            HubContainerDTOCollection latestHubDTOCollection = GetHubContainerDTOCollection(siteId, hubDTOCollection.Hash, true);
            if (latestHubDTOCollection == null ||
                latestHubDTOCollection.HubContainerDTOList == null ||
                latestHubDTOCollection.HubContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            HubViewContainer result = new HubViewContainer(siteId, latestHubDTOCollection);
            log.LogMethodExit(result);
            return result;
        }


    }
}

