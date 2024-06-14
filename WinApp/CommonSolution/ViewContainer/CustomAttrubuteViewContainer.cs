/********************************************************************************************
* Project Name - ViewContainer
* Description  - CustomAttributeViewContainer class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    27-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.GenericUtilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semnox.Parafait.ViewContainer
{
    public class CustomAttributeViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CustomAttributeContainerDTOCollection CustomAttributeContainerDTOCollection;
        private readonly ConcurrentDictionary<int, CustomAttributesContainerDTO> customAttributesContainerDTODictionary = new ConcurrentDictionary<int, CustomAttributesContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="siteId">site id</param>
        /// <param name="CustomAttributeContainerDTOCollection">CustomAttributeContainerDTOCollection</param>
        internal CustomAttributeViewContainer(int siteId, CustomAttributeContainerDTOCollection CustomAttributeContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, CustomAttributeContainerDTOCollection);
            this.siteId = siteId;
            this.CustomAttributeContainerDTOCollection = CustomAttributeContainerDTOCollection;
            if (CustomAttributeContainerDTOCollection != null &&
                CustomAttributeContainerDTOCollection.CustomAttributesContainerDTOList != null &&
               CustomAttributeContainerDTOCollection.CustomAttributesContainerDTOList.Any())
            {
                foreach (var customAttributesContainerDTO in CustomAttributeContainerDTOCollection.CustomAttributesContainerDTOList)
                {
                    customAttributesContainerDTODictionary[customAttributesContainerDTO.CustomAttributeId] = customAttributesContainerDTO;
                }
            }
            log.LogMethodExit();
        }
        internal CustomAttributeViewContainer(int siteId)
              : this(siteId, GetCustomAttributeContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        private static CustomAttributeContainerDTOCollection GetCustomAttributeContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            CustomAttributeContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                ICustomAttributesUseCases customAttributesUseCases = GenericUtilitiesUseCaseFactory.GetCustomAttributes(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<CustomAttributeContainerDTOCollection> task = customAttributesUseCases.GetCustomAttributesContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving CustomAttributeContainerDTOCollection.", ex);
                result = new CustomAttributeContainerDTOCollection();
            }

            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// returns the latest in CustomAttributeContainerDTOCollection
        /// </summary>
        /// <returns></returns>
        internal CustomAttributeContainerDTOCollection GetCustomAttributeContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (CustomAttributeContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(CustomAttributeContainerDTOCollection);
            return CustomAttributeContainerDTOCollection;
        }
        internal List<CustomAttributesContainerDTO> GetCustomAttributesContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(CustomAttributeContainerDTOCollection.CustomAttributesContainerDTOList);
            return CustomAttributeContainerDTOCollection.CustomAttributesContainerDTOList;
        }
        internal CustomAttributeViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            CustomAttributeContainerDTOCollection latestCustomAttributeContainerDTOCollection = GetCustomAttributeContainerDTOCollection(siteId, CustomAttributeContainerDTOCollection.Hash, rebuildCache);
            if (latestCustomAttributeContainerDTOCollection == null ||
                latestCustomAttributeContainerDTOCollection.CustomAttributesContainerDTOList == null ||
                latestCustomAttributeContainerDTOCollection.CustomAttributesContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            CustomAttributeViewContainer result = new CustomAttributeViewContainer(siteId, latestCustomAttributeContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }
    }
}
