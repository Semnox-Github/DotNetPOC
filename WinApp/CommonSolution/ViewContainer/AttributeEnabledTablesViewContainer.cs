/********************************************************************************************
 * Project Name - ViewContainer
 * Description  - AttributeEnabledTablesViewContainer class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      24-Aug-2021      Fiona                    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Semnox.Core.Utilities;
using Semnox.Parafait.TableAttributeSetup;

namespace Semnox.Parafait.ViewContainer
{
    /// <summary>
    /// AttributeEnabledTablesViewContainer
    /// </summary>
    public class AttributeEnabledTablesViewContainer : AbstractViewContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly AttributeEnabledTablesContainerDTOCollection AttributeEnabledTablesContainerDTOCollection;
        private readonly ConcurrentDictionary<int, AttributeEnabledTablesContainerDTO> AttributeEnabledTablesContainerDTODictionary = new ConcurrentDictionary<int, AttributeEnabledTablesContainerDTO>();
        private readonly int siteId;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public AttributeEnabledTablesViewContainer(int siteId)//changed from internal to public
           : this(siteId, GetAttributeEnabledTablesContainerDTOCollection(siteId, null, false))
        {
            log.LogMethodEntry(siteId);
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="attributeEnabledTablesContainerDTOCollection"></param>
        public AttributeEnabledTablesViewContainer(int siteId, AttributeEnabledTablesContainerDTOCollection attributeEnabledTablesContainerDTOCollection)
        {
            log.LogMethodEntry(siteId, attributeEnabledTablesContainerDTOCollection);
            this.siteId = siteId;
            this.AttributeEnabledTablesContainerDTOCollection = attributeEnabledTablesContainerDTOCollection;
            if (attributeEnabledTablesContainerDTOCollection != null &&
                attributeEnabledTablesContainerDTOCollection.AttributeEnabledTablesContainerDTOList != null &&
                attributeEnabledTablesContainerDTOCollection.AttributeEnabledTablesContainerDTOList.Any())
            {
                foreach (var AttributeEnabledTablesContainerDTO in attributeEnabledTablesContainerDTOCollection.AttributeEnabledTablesContainerDTOList)
                {
                    AddToAttributeEnabledTablesDictionary(AttributeEnabledTablesContainerDTO);
                }
            }
            log.LogMethodExit();
        }
        private void AddToAttributeEnabledTablesDictionary(AttributeEnabledTablesContainerDTO attributeEnabledTablesContainerDTO)
        {
            log.LogMethodEntry();
            AttributeEnabledTablesContainerDTODictionary[attributeEnabledTablesContainerDTO.AttributeEnabledTableId] = attributeEnabledTablesContainerDTO;
            log.LogMethodExit();
        }

        internal List<AttributeEnabledTablesContainerDTO> GetAttributeEnabledTablesContainerDTOList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(AttributeEnabledTablesContainerDTOCollection.AttributeEnabledTablesContainerDTOList);
            return AttributeEnabledTablesContainerDTOCollection.AttributeEnabledTablesContainerDTOList;
        }

        /// <summary>
        /// GetAttributeEnabledTablesContainerDTOCollection
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public AttributeEnabledTablesContainerDTOCollection GetAttributeEnabledTablesContainerDTOCollection(string hash)
        {
            log.LogMethodEntry(hash);
            if (AttributeEnabledTablesContainerDTOCollection.Hash == hash)
            {
                log.LogMethodExit(null, "No changes to the cache");
                return null;
            }
            log.LogMethodExit(AttributeEnabledTablesContainerDTOCollection);
            return AttributeEnabledTablesContainerDTOCollection;
        }
        internal AttributeEnabledTablesViewContainer Refresh(bool rebuildCache)
        {
            log.LogMethodEntry();
            if (LastRefreshTime.AddMinutes(MinimimViewContainerRefreshWaitPeriod.GetValueInMinutes()) > DateTime.Now)
            {
                log.LogMethodExit(this, "Last Refresh Time is " + LastRefreshTime);
                return this;
            }
            LastRefreshTime = DateTime.Now;
            AttributeEnabledTablesContainerDTOCollection latestAttributeEnabledTablesContainerDTOCollection = GetAttributeEnabledTablesContainerDTOCollection(siteId, AttributeEnabledTablesContainerDTOCollection.Hash, rebuildCache);
            if (latestAttributeEnabledTablesContainerDTOCollection == null ||
                latestAttributeEnabledTablesContainerDTOCollection.AttributeEnabledTablesContainerDTOList == null ||
                latestAttributeEnabledTablesContainerDTOCollection.AttributeEnabledTablesContainerDTOList.Any() == false)
            {
                log.LogMethodExit(this, "No changes to the cache");
                return this;
            }
            AttributeEnabledTablesViewContainer result = new AttributeEnabledTablesViewContainer(siteId, latestAttributeEnabledTablesContainerDTOCollection);
            log.LogMethodExit(result);
            return result;
        }

        private static AttributeEnabledTablesContainerDTOCollection GetAttributeEnabledTablesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId);
            AttributeEnabledTablesContainerDTOCollection result;
            try
            {
                ExecutionContext executionContext = GetSystemUserExecutionContext();
                IAttributeEnabledTablesUseCases paymentModeUseCases = AttributeEnabledTablesUseCaseFactory.GetAttributeEnabledTablesUseCases(executionContext);
                using (NoSynchronizationContextScope.Enter())
                {
                    Task<AttributeEnabledTablesContainerDTOCollection> task = paymentModeUseCases.GetAttributeEnabledTablesContainerDTOCollection(siteId, hash, rebuildCache);
                    task.Wait();
                    result = task.Result;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while retrieving AttributeEnabledTablesContainerDTOCollection.", ex);
                result = new AttributeEnabledTablesContainerDTOCollection();
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
