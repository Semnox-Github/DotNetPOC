/********************************************************************************************
 * Project Name - DeliveryIntegration
 * Description  - LocalDeliveryChannelUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      8-Mar-2021       Prajwal S                  Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// LocalOnlineOrderDeliveryIntegrationUseCases
    /// </summary>
    public class LocalOnlineOrderDeliveryIntegrationUseCases : IOnlineOrderDeliveryIntegrationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalOnlineOrderDeliveryIntegrationUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalOnlineOrderDeliveryIntegrationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetOnlineOrderDeliveryIntegration
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="loadActiveChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<OnlineOrderDeliveryIntegrationDTO>> GetOnlineOrderDeliveryIntegration(
                            List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false,
                            bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<OnlineOrderDeliveryIntegrationDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecords, sqlTransaction);
                OnlineOrderDeliveryIntegrationListBL deliveryIntegrationListBL = new OnlineOrderDeliveryIntegrationListBL(executionContext);
                List<OnlineOrderDeliveryIntegrationDTO> deliveryIntegrationDTOList = deliveryIntegrationListBL.GetOnlineOrderDeliveryIntegrationDTOList(
                    searchParameters, loadChildRecords, loadActiveChildRecords, sqlTransaction);
                log.LogMethodExit(deliveryIntegrationDTOList);
                return deliveryIntegrationDTOList;
            });
        }

        /// <summary>
        /// SaveOnlineOrderDeliveryIntegration
        /// </summary>
        /// <param name="deliveryIntegrationDTOList"></param>
        /// <returns></returns>
        public async Task<List<OnlineOrderDeliveryIntegrationDTO>> SaveOnlineOrderDeliveryIntegration(List<OnlineOrderDeliveryIntegrationDTO> deliveryIntegrationDTOList)
        {
            return await Task<List<OnlineOrderDeliveryIntegrationDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    OnlineOrderDeliveryIntegrationListBL deliveryIntegrationListBL = new OnlineOrderDeliveryIntegrationListBL(executionContext);
                    List<OnlineOrderDeliveryIntegrationDTO> result = deliveryIntegrationListBL.Save(deliveryIntegrationDTOList);
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        /// <summary>
        /// Gets OnlineOrderDeliveryIntegration Container Data
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public async Task<OnlineOrderDeliveryIntegrationContainerDTOCollection> GetOnlineOrderDeliveryIntegrationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<OnlineOrderDeliveryIntegrationContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    OnlineOrderDeliveryIntegrationContainerList.Rebuild(siteId);
                }
                OnlineOrderDeliveryIntegrationContainerDTOCollection result = OnlineOrderDeliveryIntegrationContainerList.GetOnlineOrderDeliveryIntegrationContainerDTOCollection(siteId);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }

    }
}
