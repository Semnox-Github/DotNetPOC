using System;
using Semnox.Parafait.Game;
using System.Collections.Concurrent;
using System.Linq;
using Semnox.Core.Utilities;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{

    public class ReaderConfigurationViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ReaderConfigurationContainerDTOCollection readerConfigurationDTOCollection;
        private readonly ConcurrentDictionary<int, ReaderConfigurationContainerDTO> readerConfigurationContainerDTODictionary = new ConcurrentDictionary<int, ReaderConfigurationContainerDTO>();
        private readonly int siteId;
        //private DateTime lastRefreshTime;
        //private readonly object locker = new object();

        internal ReaderConfigurationViewContainer(int siteId, ReaderConfigurationContainerDTOCollection readerConfigurationDTOCollection)
        {
            log.LogMethodEntry(siteId, readerConfigurationDTOCollection);
            this.siteId = siteId;
            this.readerConfigurationDTOCollection = readerConfigurationDTOCollection;
            //lastRefreshTime = DateTime.Now;
            if (readerConfigurationDTOCollection != null &&
               readerConfigurationDTOCollection.ReaderConfigurationContainerDTOList != null &&
               readerConfigurationDTOCollection.ReaderConfigurationContainerDTOList.Any())
            {
                foreach (var readerConfigurationContainerDTO in readerConfigurationDTOCollection.ReaderConfigurationContainerDTOList)
                {
                    readerConfigurationContainerDTODictionary[readerConfigurationContainerDTO.AttributeId] = readerConfigurationContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal ReaderConfigurationViewContainer(int siteId) :
            this(siteId, GetReaderConfigurationContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static ReaderConfigurationContainerDTOCollection GetReaderConfigurationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {

            log.LogMethodEntry(siteId, hash, rebuildCache);
            ReaderConfigurationContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IReaderConfigurationUseCases readerConfigurationUseCases = GameUseCaseFactory.GetReaderConfigurationUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ReaderConfigurationContainerDTOCollection> readerConfigurationViewDTOCollectionTask = readerConfigurationUseCases.GetMachineAttributeContainerDTOCollection(siteId, hash, rebuildCache);
                    readerConfigurationViewDTOCollectionTask.Wait();
                    result = readerConfigurationViewDTOCollectionTask.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ReaderConfigurationContainerDTOCollection.", ex);
                result = new ReaderConfigurationContainerDTOCollection();
            }

            return result;
        }

        /// <summary>
        /// returns the latest in ReaderConfigurationDTOCollection
        /// </summary>
        /// <returns></returns>
        internal ReaderConfigurationContainerDTOCollection GetReaderConfigurationDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (readerConfigurationDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(readerConfigurationDTOCollection);
            return readerConfigurationDTOCollection;
        }

        /// <summary>
        /// returns the ReaderConfigurationContainerDTO for the ReaderConfigurationId
        /// </summary>
        /// <param name="readerConfigurationId"></param>
        /// <returns></returns>
        public ReaderConfigurationContainerDTO GetReaderConfigurationContainerDTO(int readerConfigurationId)
        {
            log.LogMethodEntry(readerConfigurationId);
            if (readerConfigurationContainerDTODictionary.ContainsKey(readerConfigurationId) == false)
            {
                string errorMessage = "ReaderConfiguration with ReaderConfiguration Id :" + readerConfigurationId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ReaderConfigurationContainerDTO result = readerConfigurationContainerDTODictionary[readerConfigurationId];
            log.LogMethodExit(result);
            return result;
        }


        internal ReaderConfigurationViewContainer Refresh()
        {
            log.LogMethodEntry();
            //if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            //{
            //    log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
            //    return this;
            //}
            LastRefreshTime = DateTime.Now;
            ReaderConfigurationContainerDTOCollection latestReaderConfigurationDTOCollection = GetReaderConfigurationContainerDTOCollection(siteId, readerConfigurationDTOCollection.Hash, true);
            if (latestReaderConfigurationDTOCollection == null ||
                latestReaderConfigurationDTOCollection.ReaderConfigurationContainerDTOList == null ||
                latestReaderConfigurationDTOCollection.ReaderConfigurationContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ReaderConfigurationViewContainer result = new ReaderConfigurationViewContainer(siteId, latestReaderConfigurationDTOCollection);
            log.LogMethodExit(result);
            return result;
        }


    }
}


