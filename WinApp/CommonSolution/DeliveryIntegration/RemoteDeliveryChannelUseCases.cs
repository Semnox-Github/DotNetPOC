/********************************************************************************************
 * Project Name - DeliveryIntegration
 * Description  - RemoteDeliveryChannelUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.120.0      15-Mar-2021       Prajwal S          Created :Inventory UI/POS UI re-design with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient; 
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// RemoteDeliveryChannelUseCases
    /// </summary>
    public class RemoteDeliveryChannelUseCases : RemoteUseCases, IDeliveryChannelUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string DeliveryChannel_URL = "api/Organization/DeliveryChannels";

        /// <summary>
        /// RemoteDeliveryChannelUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteDeliveryChannelUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// GetDeliveryChannel
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<DeliveryChannelDTO>> GetDeliveryChannel(List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>>
                          parameters, SqlTransaction sqlTransaction = null)
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
                List<DeliveryChannelDTO> result = await Get<List<DeliveryChannelDTO>>(DeliveryChannel_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<DeliveryChannelDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case DeliveryChannelDTO.SearchByParameters.CHANNEL_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("channelName".ToString(), searchParameter.Value));
                        }
                        break;
                    case DeliveryChannelDTO.SearchByParameters.DELIVERY_CHANNEL_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("deliveryChannelId".ToString(), searchParameter.Value));
                        }
                        break;
                    case DeliveryChannelDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        /// <summary>
        /// SaveDeliveryChannel
        /// </summary>
        /// <param name="deliveryChannelDTOList"></param>
        /// <returns></returns>
        public async Task<List<DeliveryChannelDTO>> SaveDeliveryChannel(List<DeliveryChannelDTO> deliveryChannelDTOList)
        {
            log.LogMethodEntry(deliveryChannelDTOList);
            try
            {
                List<DeliveryChannelDTO> responseString = await Post<List<DeliveryChannelDTO>>(DeliveryChannel_URL, deliveryChannelDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

    }
}
