/********************************************************************************************
 * Project Name - DeliveryIntegration
 * Description  - RemoteOnlineOrderDeliveryIntegrationUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.150.0      13-Jul-2022       Guru S A          Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient; 
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// RemoteOnlineOrderDeliveryIntegrationUseCases
    /// </summary>
    public class RemoteOnlineOrderDeliveryIntegrationUseCases : RemoteUseCases, IOnlineOrderDeliveryIntegrationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DeliveryIntegration_URL = "api/Organization/DeliveryIntegration";

        /// <summary>
        /// RemoteOnlineOrderDeliveryIntegrationUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteOnlineOrderDeliveryIntegrationUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetOnlineOrderDeliveryIntegration
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="loadActiveChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<OnlineOrderDeliveryIntegrationDTO>> GetOnlineOrderDeliveryIntegration(
                                    List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false,
                                    bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            if (parameters != null)
                if (parameters != null)
                {
                    searchParameterList.AddRange(BuildSearchParameter(parameters));
                }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<OnlineOrderDeliveryIntegrationDTO> result = await Get<List<OnlineOrderDeliveryIntegrationDTO>>(DeliveryIntegration_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case OnlineOrderDeliveryIntegrationDTO.SearchByParameters.INTEGRATION_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IntegrationName".ToString(), searchParameter.Value));
                        }
                        break;
                    case OnlineOrderDeliveryIntegrationDTO.SearchByParameters.DELIVERY_INTEGRATION_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("DeliveryIntegrationId".ToString(), searchParameter.Value));
                        }
                        break;
                    case OnlineOrderDeliveryIntegrationDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case OnlineOrderDeliveryIntegrationDTO.SearchByParameters.SITE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("site_id".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        /// <summary>
        /// SaveOnlineOrderDeliveryIntegration
        /// </summary>
        /// <param name="deliveryIntegrationDTOList"></param>
        /// <returns></returns>
        public async Task<List<OnlineOrderDeliveryIntegrationDTO>> SaveOnlineOrderDeliveryIntegration(List<OnlineOrderDeliveryIntegrationDTO> deliveryIntegrationDTOList)
        {
            log.LogMethodEntry(deliveryIntegrationDTOList);
            try
            {
                List<OnlineOrderDeliveryIntegrationDTO> responseString = await Post<List<OnlineOrderDeliveryIntegrationDTO>>(DeliveryIntegration_URL, deliveryIntegrationDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        } 
        /// <summary>
        /// Get the Container Data for OnlineOrderDeliveryIntegration remote case.
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        public async Task<OnlineOrderDeliveryIntegrationContainerDTOCollection> GetOnlineOrderDeliveryIntegrationContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            log.LogMethodEntry(siteId, hash, rebuildCache);
            OnlineOrderDeliveryIntegrationContainerDTOCollection result
                = await Get<OnlineOrderDeliveryIntegrationContainerDTOCollection>(DeliveryIntegration_URL,
                                                          new WebApiGetRequestParameterCollection("siteId", siteId, "hash", hash, "rebuildCache", rebuildCache));
            log.LogMethodExit(result);
            return result;
        }
    }
}
