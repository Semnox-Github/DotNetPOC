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
    /// LocalDeliveryChannelUseCases
    /// </summary>
    public class LocalDeliveryChannelUseCases : IDeliveryChannelUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalDeliveryChannelUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalDeliveryChannelUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetDeliveryChannel
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<DeliveryChannelDTO>> GetDeliveryChannel(List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<DeliveryChannelDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                DeliveryChannelListBL deliveryChannelListBL = new DeliveryChannelListBL(executionContext);
                List<DeliveryChannelDTO> deliveryChannelDTOList = deliveryChannelListBL.GetDeliveryChannels(searchParameters, sqlTransaction);

                log.LogMethodExit(deliveryChannelDTOList);
                return deliveryChannelDTOList;
            });
        }

        /// <summary>
        /// SaveDeliveryChannel
        /// </summary>
        /// <param name="deliveryChannelDTOList"></param>
        /// <returns></returns>
        public async Task<List<DeliveryChannelDTO>> SaveDeliveryChannel(List<DeliveryChannelDTO> deliveryChannelDTOList)
        {
            return await Task<List<DeliveryChannelDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    DeliveryChannelListBL deliveryChannelList = new DeliveryChannelListBL(executionContext);
                    List<DeliveryChannelDTO> result = deliveryChannelList.Save(deliveryChannelDTOList);
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
