/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - EnabledAttributesViewContainer
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      20-Aug-2021      fiona                     Created 
 ********************************************************************************************/

using Semnox.Parafait.TableAttributeSetup;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// EnabledAttributesViewContainer
    /// </summary>
    public class EnabledAttributesViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly EnabledAttributesContainerDTOCollection enabledAttributesContainerDTOCollection;
        private readonly ConcurrentDictionary<int, EnabledAttributesContainerDTO> enabledAttributesContainerDTODictionary = new ConcurrentDictionary<int, EnabledAttributesContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="siteId"></param>
        public EnabledAttributesViewContainer(int siteId)
           : this(siteId, GetEnabledAttributesContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }

        private static EnabledAttributesContainerDTOCollection GetEnabledAttributesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            EnabledAttributesContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IEnabledAttributesUseCases paymentModeUseCases = EnabledAttributesUseCaseFactory.GetEnabledAttributesUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<EnabledAttributesContainerDTOCollection> task = paymentModeUseCases.GetEnabledAttributesContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving EnabledAttributesContainerDTOCollection.", ex);
                result = new EnabledAttributesContainerDTOCollection();
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// EnabledAttributesViewContainer
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="enabledAttributesContainerDTOCollection"></param>
        public EnabledAttributesViewContainer(int siteId, EnabledAttributesContainerDTOCollection enabledAttributesContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, enabledAttributesContainerDTOCollection);
            this.siteId = siteId;
            this.enabledAttributesContainerDTOCollection = enabledAttributesContainerDTOCollection;
            if (enabledAttributesContainerDTOCollection != null &&
                enabledAttributesContainerDTOCollection.EnabledAttributesContainerDTOList != null &&
                enabledAttributesContainerDTOCollection.EnabledAttributesContainerDTOList.Any())
            {
                foreach (var enabledAttributesContainerDTO in enabledAttributesContainerDTOCollection.EnabledAttributesContainerDTOList)
                {
                    AddToEnabledAttributesDictionary(enabledAttributesContainerDTO);
                }
            }
            log.LogMethodExit();
        }

        private void AddToEnabledAttributesDictionary(EnabledAttributesContainerDTO enabledAttributesContainerDTO)
        {
            log.LogMethodEntry();
            enabledAttributesContainerDTODictionary[enabledAttributesContainerDTO.EnabledAttibuteId] = enabledAttributesContainerDTO;
            log.LogMethodExit();
        }
        internal List<EnabledAttributesContainerDTO> GetEnabledAttributesContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(enabledAttributesContainerDTOCollection.EnabledAttributesContainerDTOList);
            return enabledAttributesContainerDTOCollection.EnabledAttributesContainerDTOList;
        }
        /// <summary>
        /// GetEnabledAttributesContainerDTOCollection
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public EnabledAttributesContainerDTOCollection GetEnabledAttributesContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (enabledAttributesContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(enabledAttributesContainerDTOCollection);
            return enabledAttributesContainerDTOCollection;
        }
        internal EnabledAttributesViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            EnabledAttributesContainerDTOCollection latestEnabledAttributesContainerDTOCollection = GetEnabledAttributesContainerDTOCollection(siteId, enabledAttributesContainerDTOCollection.Hash, rebuildCache);
            if (latestEnabledAttributesContainerDTOCollection == null ||
                latestEnabledAttributesContainerDTOCollection.EnabledAttributesContainerDTOList == null ||
                latestEnabledAttributesContainerDTOCollection.EnabledAttributesContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            EnabledAttributesViewContainer result = new EnabledAttributesViewContainer(siteId, latestEnabledAttributesContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
