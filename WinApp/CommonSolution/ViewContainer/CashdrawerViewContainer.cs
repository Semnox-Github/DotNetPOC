/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - CashdrawerViewContainer holds the parafait default values for a given siteId, userId and POSMachineId
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.140.00    27-Jul-2021       Girish Kundar            Created : Multicash drawer enhancement
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer.Cashdrawers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// 
    /// </summary>
    public class CashdrawerViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CashdrawerContainerDTOCollection cashdrawerContainerDTOCollection;
        private readonly ConcurrentDictionary<int, CashdrawerContainerDTO> cashdrawerContainerDTOContainerDTODictionary = new ConcurrentDictionary<int, CashdrawerContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="cashdrawerContainerDTOCollection">CashdrawerContainerDTOCollection</param>
        internal CashdrawerViewContainer(int siteId, CashdrawerContainerDTOCollection cashdrawerContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, cashdrawerContainerDTOCollection);
            this.siteId = siteId;
            this.cashdrawerContainerDTOCollection = cashdrawerContainerDTOCollection;
            if (cashdrawerContainerDTOCollection != null &&
                cashdrawerContainerDTOCollection.CashdrawerContainerDTOList != null &&
               cashdrawerContainerDTOCollection.CashdrawerContainerDTOList.Any())
            {
                foreach (var cashdrawerContainerDTO in cashdrawerContainerDTOCollection.CashdrawerContainerDTOList)
                {
                    cashdrawerContainerDTOContainerDTODictionary[cashdrawerContainerDTO.CashdrawerId] = cashdrawerContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal CashdrawerViewContainer(int siteId)
              : this(siteId, GetCashdrawerContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static CashdrawerContainerDTOCollection GetCashdrawerContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            CashdrawerContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ICashdrawerUseCases cashdrawerUseCase = CashdrawerUseCaseFactory.GetCashdrawerUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<CashdrawerContainerDTOCollection> task = cashdrawerUseCase.GetCashdrawerContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving CashdrawerContainerDTOCollection.", ex);
                result = new CashdrawerContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in CashdrawerContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal CashdrawerContainerDTOCollection GetCashdrawerContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (cashdrawerContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(cashdrawerContainerDTOCollection);
            return cashdrawerContainerDTOCollection;
        }
        internal List<CashdrawerContainerDTO> GetCashdrawerContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(cashdrawerContainerDTOCollection.CashdrawerContainerDTOList);
            return cashdrawerContainerDTOCollection.CashdrawerContainerDTOList;
        }
        internal CashdrawerViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            CashdrawerContainerDTOCollection latestCashdrawerContainerDTOCollection = GetCashdrawerContainerDTOCollection(siteId, cashdrawerContainerDTOCollection.Hash, rebuildCache);
            if (latestCashdrawerContainerDTOCollection == null ||
                latestCashdrawerContainerDTOCollection.CashdrawerContainerDTOList == null ||
                latestCashdrawerContainerDTOCollection.CashdrawerContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            CashdrawerViewContainer result = new CashdrawerViewContainer(siteId, latestCashdrawerContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
 }
