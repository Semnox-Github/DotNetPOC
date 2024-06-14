using Semnox.Core.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    public class LookupsViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LookupsContainerDTOCollection lookupsDTOCollection;
        private readonly ConcurrentDictionary<string, LookupsContainerDTO> lookupsContainerDTODictionary = new ConcurrentDictionary<string, LookupsContainerDTO>();
        private readonly ConcurrentDictionary<int, LookupValuesContainerDTO> lookupValuesContainerDTODictionary = new ConcurrentDictionary<int, LookupValuesContainerDTO>();
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal LookupsViewContainer(int siteId, LookupsContainerDTOCollection lookupsDTOCollection)
        {
            log.LogMethodEntry(siteId, lookupsDTOCollection);
            this.siteId = siteId;
            this.lookupsDTOCollection = lookupsDTOCollection;
            //lastRefreshTime = DateTime.Now;
            if (lookupsDTOCollection != null &&
               lookupsDTOCollection.LookupsContainerDTOList != null &&
               lookupsDTOCollection.LookupsContainerDTOList.Any())
            {
                foreach (var lookupsContainerDTO in lookupsDTOCollection.LookupsContainerDTOList)
                {
                    lookupsContainerDTODictionary[lookupsContainerDTO.LookupName] = lookupsContainerDTO;
                    foreach (LookupValuesContainerDTO lookupValuesContainerDTO in lookupsContainerDTO.LookupValuesContainerDTOList)
                    {
                        lookupValuesContainerDTODictionary[lookupValuesContainerDTO.LookupValueId] = lookupValuesContainerDTO;
                    }
                }
            }
            log.LogMethodExit();
        }

        internal LookupsViewContainer(int siteId) :
            this(siteId, GetLookupsContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static LookupsContainerDTOCollection GetLookupsContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            LookupsContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ILookupsUseCases lookupsUseCases = LookupsUseCaseFactory.GetLookupsUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<LookupsContainerDTOCollection> lookupsViewDTOCollectionTask = lookupsUseCases.GetLookupsContainerDTOCollection(siteId, hash, rebuildCache);
                    lookupsViewDTOCollectionTask.Wait();
                    result = lookupsViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving LookupsContainerDTOCollection.", ex);
                result = new LookupsContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in LookupsDTOCollection
        /// </summary>
        /// <returns></returns>
        internal LookupsContainerDTOCollection GetLookupsDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (lookupsDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(lookupsDTOCollection);
            return lookupsDTOCollection;
        }

        internal List<LookupsContainerDTO> GetLookupsContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(lookupsDTOCollection.LookupsContainerDTOList);
            return lookupsDTOCollection.LookupsContainerDTOList;
        }


        /// <summary>
        /// returns the LookupsContainerDTO for the LookupsId
        /// </summary>
        /// <param name="lookupName"></param>
        /// <returns></returns>
        public LookupsContainerDTO GetLookupsContainerDTO(string lookupName)
        {
            log.LogMethodEntry(lookupName);
            if (lookupsContainerDTODictionary.ContainsKey(lookupName) == false)
            {
                string errorMessage = "Lookups with Lookups Name :" + lookupName + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            LookupsContainerDTO result = lookupsContainerDTODictionary[lookupName];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the LookupsContainerDTO for the LookupsId
        /// </summary>
        /// <param name="lookupName"></param>
        /// <returns></returns>
        public LookupValuesContainerDTO GetLookupValuesContainerDTO(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            if (lookupValuesContainerDTODictionary.ContainsKey(lookupValueId) == false)
            {
                string errorMessage = "Lookups with Lookups Name :" + lookupValueId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            LookupValuesContainerDTO result = lookupValuesContainerDTODictionary[lookupValueId];
            log.LogMethodExit(result);
            return result;
        }

        internal LookupsViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            LookupsContainerDTOCollection latestLookupsDTOCollection = GetLookupsContainerDTOCollection(siteId, lookupsDTOCollection.Hash, true);
            if (latestLookupsDTOCollection == null ||
                latestLookupsDTOCollection.LookupsContainerDTOList == null ||
                latestLookupsDTOCollection.LookupsContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            LookupsViewContainer result = new LookupsViewContainer(siteId, latestLookupsDTOCollection);
            log.LogMethodExit(result);
            return result;
        }


    }
}
