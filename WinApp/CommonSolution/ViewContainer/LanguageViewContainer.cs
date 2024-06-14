
/********************************************************************************************
 * Project Name - Utilities
 * Description  - LanguageViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      17-Nov-2020      Lakshminarayana           Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// LanguageViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
    /// </summary>
    public class LanguageViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly LanguageContainerDTOCollection languageContainerDTOCollection;
        private readonly ConcurrentDictionary<int, LanguageContainerDTO> languageContainerDTODictionary = new ConcurrentDictionary<int, LanguageContainerDTO>();
        private readonly int siteId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="languageContainerDTOCollection">languageContainerDTOCollection</param>
        internal LanguageViewContainer(int siteId, LanguageContainerDTOCollection languageContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, languageContainerDTOCollection);
            this.siteId = siteId;
            this.languageContainerDTOCollection = languageContainerDTOCollection;
            if (languageContainerDTOCollection != null &&
                languageContainerDTOCollection.LanguageContainerDTOList != null &&
                languageContainerDTOCollection.LanguageContainerDTOList.Any())
            {
                foreach (var languageContainerDTO in languageContainerDTOCollection.LanguageContainerDTOList)
                {
                    languageContainerDTODictionary[languageContainerDTO.LanguageId] = languageContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal LanguageViewContainer(int siteId)
            :this(siteId, GetLanguageContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static LanguageContainerDTOCollection GetLanguageContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            LanguageContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ILanguageUseCases languageUseCases = LanguageUseCaseFactory.GetLanguageUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<LanguageContainerDTOCollection> task = languageUseCases.GetLanguageContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving LanguageContainerDTOCollection.", ex);
                result = new LanguageContainerDTOCollection();
            }
            
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the LanguageContainerDTO for the languageId
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public LanguageContainerDTO GetLanguageContainerDTO(int languageId)
        {
            log.LogMethodEntry(languageId);
            if (languageContainerDTODictionary.ContainsKey(languageId) == false)
            {
                string errorMessage = "Language with language Id :" + languageId + " doesn't exists.";
                log.LogMethodExit("Throwing Exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            LanguageContainerDTO result = languageContainerDTODictionary[languageId];
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the latest in LanguageContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal LanguageContainerDTOCollection GetLanguageContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (languageContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(languageContainerDTOCollection);
            return languageContainerDTOCollection;
        }

        internal LanguageViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if(LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            LanguageContainerDTOCollection latestLanguageContainerDTOCollection = GetLanguageContainerDTOCollection(siteId, languageContainerDTOCollection.Hash, rebuildCache);
            if (latestLanguageContainerDTOCollection == null || 
                latestLanguageContainerDTOCollection.LanguageContainerDTOList == null ||
                latestLanguageContainerDTOCollection.LanguageContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            LanguageViewContainer result = new LanguageViewContainer(siteId, latestLanguageContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
