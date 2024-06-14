/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - SiteViewContainer class
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified By Remarks
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 2.150.2     20-Mar-2023       Abhishek                Modified : Added GetMasterSiteId method to get MasterSiteId
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// Holds the list of sites
    /// </summary>
    public class SiteViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly SiteContainerDTOCollection siteContainerDTOCollection;
        private readonly Dictionary<string, SiteContainerDTOCollection> subSiteContainerDTOCollectionDictionary = new Dictionary<string, SiteContainerDTOCollection>();
        private readonly ConcurrentDictionary<int, SiteContainerDTO> siteDictionary;
        private readonly bool isCorporate = false;
        private readonly int masterSiteId = -1;

        internal SiteViewContainer(SiteContainerDTOCollection siteContainerDTOCollection)
        {
            log.LogMethodEntry();
            this.siteContainerDTOCollection = siteContainerDTOCollection;
            siteDictionary = new ConcurrentDictionary<int, SiteContainerDTO>();
            if (siteContainerDTOCollection != null &&
                siteContainerDTOCollection.SiteContainerDTOList != null &&
                siteContainerDTOCollection.SiteContainerDTOList.Any())
            {
                isCorporate = siteContainerDTOCollection.SiteContainerDTOList.Count > 1;
                foreach (var siteContainerDTO in siteContainerDTOCollection.SiteContainerDTOList)
                {
                    siteDictionary[siteContainerDTO.SiteId] = siteContainerDTO;
                    if (siteContainerDTO.IsMasterSite == true)
                    {
                        if (isCorporate)
                        {
                            masterSiteId = siteContainerDTO.SiteId;
                        }
                    }
                }

                string key = GetKey(false, false);
                subSiteContainerDTOCollectionDictionary.Add(key, siteContainerDTOCollection);

                key = GetKey(true, false);
                List<SiteContainerDTO> onlineEnabledSiteContainerDTOList = siteContainerDTOCollection.SiteContainerDTOList.Where(x => x.OnlineEnabled && (x.SiteDeliveryDetailsDTOList == null || x.SiteDeliveryDetailsDTOList.All(y => y.DeliveryChannelId < 0))).ToList();
                SiteContainerDTOCollection onlineEnabledSiteContainerDTOCollection = new SiteContainerDTOCollection(onlineEnabledSiteContainerDTOList);
                subSiteContainerDTOCollectionDictionary.Add(key, onlineEnabledSiteContainerDTOCollection);

                key = GetKey(false, true);
                List<SiteContainerDTO> fnbEnabledSiteContainerDTOList = siteContainerDTOCollection.SiteContainerDTOList.Where(x => x.OnlineEnabled && x.SiteDeliveryDetailsDTOList != null && x.SiteDeliveryDetailsDTOList.Any(y => y.DeliveryChannelId > -1) && x.SiteDeliveryDetailsDTOList.All(y => y.OrderDeliveryType != "None") && x.SiteDeliveryDetailsDTOList.All(y => y.OrderDeliveryType != "") && x.SiteDeliveryDetailsDTOList.All(y => y.OrderDeliveryType != string.Empty)).ToList();
                SiteContainerDTOCollection fnbEnabledSiteContainerDTOCollection = new SiteContainerDTOCollection(fnbEnabledSiteContainerDTOList);
                subSiteContainerDTOCollectionDictionary.Add(key, fnbEnabledSiteContainerDTOCollection);

                key = GetKey(true, true);
                List<SiteContainerDTO> fnbAndOnlineEnabledSiteContainerDTOList = siteContainerDTOCollection.SiteContainerDTOList.Where(x => x.OnlineEnabled && x.SiteDeliveryDetailsDTOList != null && x.SiteDeliveryDetailsDTOList.Any(y => y.DeliveryChannelId > -1) && x.SiteDeliveryDetailsDTOList.All(y => y.OrderDeliveryType != "None") && x.SiteDeliveryDetailsDTOList.All(y => y.OrderDeliveryType != "" || y.OrderDeliveryType != string.Empty)).ToList();
                SiteContainerDTOCollection fnbAndOnlineEnabledSiteContainerDTOCollection = new SiteContainerDTOCollection(fnbAndOnlineEnabledSiteContainerDTOList);
                subSiteContainerDTOCollectionDictionary.Add(key, fnbAndOnlineEnabledSiteContainerDTOCollection);

            }
            log.LogMethodExit();
        }

        internal SiteViewContainer()
            : this(GetSiteContainerDTOCollection(null, false, false, false))
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private static SiteContainerDTOCollection GetSiteContainerDTOCollection(string hash, bool onlineEnabledOnly, bool fnBEnabledOnly, bool rebuildCache)
        {
            log.LogMethodEntry();
            SiteContainerDTOCollection siteContainerDTOCollection;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ISiteUseCases siteUseCases = SiteUseCaseFactory.GetSiteUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<SiteContainerDTOCollection> siteContainerDTOCollectionTask = siteUseCases.GetSiteContainerDTOCollection(hash, onlineEnabledOnly, fnBEnabledOnly, rebuildCache);
                    siteContainerDTOCollectionTask.Wait();
                    siteContainerDTOCollection = siteContainerDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving SiteContainerDTOCollection.", ex);
                siteContainerDTOCollection = new SiteContainerDTOCollection();
            }

            log.LogMethodExit(siteContainerDTOCollection);
            return siteContainerDTOCollection;
        }

        internal bool IsCorporate()
        {
            log.LogMethodEntry();
            log.LogMethodExit(isCorporate);
            return isCorporate;
        }

        /// <summary>
        /// returns the MasterSiteId
        /// </summary>
        /// <returns>MasterSiteId</returns>
        internal int GetMasterSiteId()
        {
            log.LogMethodEntry();
            log.LogMethodExit(masterSiteId);
            return masterSiteId;
        }

        /// <summary>
        /// returns the latest in SiteViewDTOCollection
        /// </summary>
        /// <returns></returns>
        internal SiteContainerDTOCollection GetSiteContainerDTOCollection(string hash, bool onlineEnabledOnly, bool fnBEnabledOnly)
        {
            log.LogMethodEntry(hash);
            string key = GetKey(onlineEnabledOnly, fnBEnabledOnly);
            SiteContainerDTOCollection result;
            if (subSiteContainerDTOCollectionDictionary.ContainsKey(key) == false)
            {
                result = new SiteContainerDTOCollection();
                log.LogMethodExit(result, "subSiteContainerDTOCollectionDictionary.ContainsKey(key) == false");
                return result; 
            }
            result = subSiteContainerDTOCollectionDictionary[key];
            if (result.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(result);
            return result;
        }
        
        private string GetKey(bool onlineEnabledOnly, bool fnBEnabledOnly)
        {
            log.LogMethodEntry(onlineEnabledOnly, fnBEnabledOnly);
            string result = "onlineEnabledOnly" + onlineEnabledOnly + "fnBEnabledOnly" + fnBEnabledOnly;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the site container DTO for a given siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public SiteContainerDTO GetSiteContainerDTO(int siteId)
        {
            log.LogMethodEntry(siteId);
            if (isCorporate == false && siteContainerDTOCollection.SiteContainerDTOList.Count == 1)
            {
                log.LogMethodExit(siteContainerDTOCollection.SiteContainerDTOList[0], "isCorporate == false && SiteContainerDTOList.Count == 1");
                return siteContainerDTOCollection.SiteContainerDTOList[0];
            }
            if (siteDictionary.ContainsKey(siteId) == false)
            {
                string errorMessage = "Site with siteId :" + siteId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            SiteContainerDTO result = siteDictionary[siteId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the list of site container DTO
        /// </summary>
        /// <returns></returns>
        public List<SiteContainerDTO> GetSiteContainerDTOList()
        {
            log.LogMethodEntry();
            List<SiteContainerDTO> result = siteContainerDTOCollection.SiteContainerDTOList;
            log.LogMethodExit(result);
            return result;
        }


        internal SiteViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            SiteContainerDTOCollection latestSiteContainerDTOCollection = GetSiteContainerDTOCollection(siteContainerDTOCollection.Hash, false, false, rebuildCache);
            if (latestSiteContainerDTOCollection == null ||
                latestSiteContainerDTOCollection.SiteContainerDTOList == null ||
                latestSiteContainerDTOCollection.SiteContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            SiteViewContainer result = new SiteViewContainer(latestSiteContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
