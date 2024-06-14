/********************************************************************************************
* Project Name - ViewContainer
* Description  - ApplicationContentViewContainer class 
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
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.ViewContainer
{
    class ApplicationContentViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ApplicationContentContainerDTOCollection applicationContentContainerDTOCollection;
        private readonly ConcurrentDictionary<int, ApplicationContentContainerDTO> applicationContentContainerDTODictionary = new ConcurrentDictionary<int, ApplicationContentContainerDTO>();
        private readonly int siteId;
        private readonly int languageId;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="applicationContentContainerDTOCollection">applicationContentContainerDTOCollection</param>
        //internal ApplicationContentViewContainer(int siteId,  ApplicationContentContainerDTOCollection applicationContentContainerDTOCollection)
        //{
        //    log.LogMethodEntry(siteId, applicationContentContainerDTOCollection);
        //    this.siteId = siteId;
        //   // this.languageId = languageId;
        //    this.applicationContentContainerDTOCollection = applicationContentContainerDTOCollection;
        //    if (applicationContentContainerDTOCollection != null &&
        //        applicationContentContainerDTOCollection.ApplicationContentContainerDTOList != null &&
        //       applicationContentContainerDTOCollection.ApplicationContentContainerDTOList.Any())
        //    {
        //        foreach (var applicationContentContainerDTO in applicationContentContainerDTOCollection.ApplicationContentContainerDTOList)
        //        {
        //            applicationContentContainerDTODictionary[applicationContentContainerDTO.ApplicationContentId] = applicationContentContainerDTO;
        //        }
        //    }
        //    log.LogMethodExit();
        //}


        internal ApplicationContentViewContainer(int siteId, int languageId, ApplicationContentContainerDTOCollection applicationContentContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, applicationContentContainerDTOCollection);
            this.siteId = siteId;
            this.languageId = languageId;
            this.applicationContentContainerDTOCollection = applicationContentContainerDTOCollection;
            if (applicationContentContainerDTOCollection != null &&
                applicationContentContainerDTOCollection.ApplicationContentContainerDTOList != null &&
               applicationContentContainerDTOCollection.ApplicationContentContainerDTOList.Any())
            {
                foreach (var applicationContentContainerDTO in applicationContentContainerDTOCollection.ApplicationContentContainerDTOList)
                {
                    applicationContentContainerDTODictionary[applicationContentContainerDTO.ApplicationContentId] = applicationContentContainerDTO;
                }
            }
            log.LogMethodExit();
        }

        internal ApplicationContentViewContainer(int siteId, int languageId)
      : this(siteId, languageId, GetApplicationContentContainerDTOCollection(siteId, languageId, null, false))
        {
            log.LogMethodEntry(siteId, languageId);
            log.LogMethodExit();
        }


      //  internal ApplicationContentViewContainer(int siteId)
      //: this(siteId,  GetApplicationContentContainerDTOCollection(siteId,  null, false))
      //  {
      //      log.LogMethodEntry(siteId);
      //      log.LogMethodExit();
      //  }

        //private static ApplicationContentContainerDTOCollection GetApplicationContentContainerDTOCollection(int siteId,  string hash, bool rebuildCache)
        //{
        //    log.LogMethodEntry(siteId);
        //    ApplicationContentContainerDTOCollection result;
        //    try
        //    {
        //        ExecutionContext executionContext = GetSystemUserExecutionContext();
        //        IApplicationContentUseCases applicationContentUseCases = GenericUtilitiesUseCaseFactory.GetApplicationContents(executionContext);
        //        using (NoSynchronizationContextScope.Enter())
        //        {
        //            Task<ApplicationContentContainerDTOCollection> task = applicationContentUseCases.GetApplicationContentContainerDTOCollection(siteId,  hash, rebuildCache);
        //            task.Wait();
        //            result = task.Result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Error occurred while retrieving ApplicationContentContainerDTOCollection.", ex);
        //        result = new ApplicationContentContainerDTOCollection();
        //    }

        //    log.LogMethodExit(result);
        //    return result;
        //}
        private static ApplicationContentContainerDTOCollection GetApplicationContentContainerDTOCollection(int siteId, int languageId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            ApplicationContentContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IApplicationContentUseCases applicationContentUseCases = GenericUtilitiesUseCaseFactory.GetApplicationContents(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<ApplicationContentContainerDTOCollection> task = applicationContentUseCases.GetApplicationContentContainerDTOCollection(siteId, languageId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving ApplicationContentContainerDTOCollection.", ex);
                result = new ApplicationContentContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in ApplicationContentContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal ApplicationContentContainerDTOCollection GetApplicationContentContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (applicationContentContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(applicationContentContainerDTOCollection);
            return applicationContentContainerDTOCollection;
        }
        internal List<ApplicationContentContainerDTO> GetApplicationContentContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(applicationContentContainerDTOCollection.ApplicationContentContainerDTOList);
            return applicationContentContainerDTOCollection.ApplicationContentContainerDTOList;
        }
        internal ApplicationContentViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            ApplicationContentContainerDTOCollection latestApplicationContentContainerDTOCollection = GetApplicationContentContainerDTOCollection(siteId, languageId, applicationContentContainerDTOCollection.Hash, rebuildCache);
            if (latestApplicationContentContainerDTOCollection == null ||
                latestApplicationContentContainerDTOCollection.ApplicationContentContainerDTOList == null ||
                latestApplicationContentContainerDTOCollection.ApplicationContentContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            ApplicationContentViewContainer result = new ApplicationContentViewContainer(siteId, languageId, latestApplicationContentContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
