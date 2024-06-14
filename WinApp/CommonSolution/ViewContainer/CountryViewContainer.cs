/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - CountryViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    08-Jul-2021      Roshan Devadiga          Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    class CountryViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CountryContainerDTOCollection countryContainerDTOCollection;
        private readonly ConcurrentDictionary<int, CountryContainerDTO> countryContainerDTODictionary = new ConcurrentDictionary<int, CountryContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="countryContainerDTOCollection">countryContainerDTOCollection</param>
        internal CountryViewContainer(int siteId, CountryContainerDTOCollection countryContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, countryContainerDTOCollection);
            this.siteId = siteId;
            this.countryContainerDTOCollection = countryContainerDTOCollection;
            if (countryContainerDTOCollection != null &&
                countryContainerDTOCollection.CountryContainerDTOList != null &&
               countryContainerDTOCollection.CountryContainerDTOList.Any())
            {
                foreach (var countryContainerDTO in countryContainerDTOCollection.CountryContainerDTOList)
                {
                    countryContainerDTODictionary[countryContainerDTO.CountryId] = countryContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal CountryViewContainer(int siteId)
              : this(siteId, GetCountryContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static CountryContainerDTOCollection GetCountryContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            CountryContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ICountryUseCases countryUseCases = GenericUtilitiesUseCaseFactory.GetCountries(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<CountryContainerDTOCollection> task = countryUseCases.GetCountryContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving CountryContainerDTOCollection.", ex);
                result = new CountryContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in CountryContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal CountryContainerDTOCollection GetCountryContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (countryContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(countryContainerDTOCollection);
            return countryContainerDTOCollection;
        }
        internal List<CountryContainerDTO> GetCountryContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(countryContainerDTOCollection.CountryContainerDTOList);
            return countryContainerDTOCollection.CountryContainerDTOList;
        }
        internal CountryViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            CountryContainerDTOCollection latestCountryContainerDTOCollection = GetCountryContainerDTOCollection(siteId, countryContainerDTOCollection.Hash, rebuildCache);
            if (latestCountryContainerDTOCollection == null ||
                latestCountryContainerDTOCollection.CountryContainerDTOList == null ||
                latestCountryContainerDTOCollection.CountryContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            CountryViewContainer result = new CountryViewContainer(siteId, latestCountryContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
 }
