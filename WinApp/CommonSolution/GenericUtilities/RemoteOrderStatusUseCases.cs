/********************************************************************************************
 * Project Name - RemoteOrderStatusUseCases
 * Description  - RemoteOrderStatusUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 2.120.0      17-Mar-2021       Prajwal S          Created :Inventory UI/POS UI re-design with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// RemoteOrderStatusUseCases
    /// </summary>
    public class RemoteOrderStatusUseCases : RemoteUseCases , IOrderStatusUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string OrderStatus_URL = "api/Common/OrderStatus";

        /// <summary>
        /// RemoteOrderStatusUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteOrderStatusUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetOrderStatus
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<OrderStatusDTO>> GetOrderStatus(List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>>
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
                List<OrderStatusDTO> result = await Get<List<OrderStatusDTO>>(OrderStatus_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<OrderStatusDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case OrderStatusDTO.SearchByParameters.ORDER_STATUS:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderStatus".ToString(), searchParameter.Value));
                        }
                        break;
                    case OrderStatusDTO.SearchByParameters.ORDER_STATUS_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderStatusId".ToString(), searchParameter.Value));
                        }
                        break;
                    case OrderStatusDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<OrderStatusDTO>> SaveOrderStatus(List<OrderStatusDTO> orderStatusDTOList)
        {
            log.LogMethodEntry(orderStatusDTOList);
            try
            {
                List<OrderStatusDTO> responseString = await Post<List<OrderStatusDTO>>(OrderStatus_URL, orderStatusDTOList);
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
