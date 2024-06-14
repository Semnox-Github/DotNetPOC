/********************************************************************************************
 * Project Name - RemoteOrderHeaderUseCases
 * Description  - RemoteOrderHeaderUseCases class 
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// RemoteOrderHeaderUseCases
    /// </summary>
    public class RemoteOrderHeaderUseCases : RemoteUseCases, IOrderHeaderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string OrderHeader_URL = "api/Transaction/OrderHeader";

        /// <summary>
        /// RemoteOrderHeaderUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteOrderHeaderUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetOrderHeader
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public async Task<List<OrderHeaderDTO>> GetOrderHeader(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>>
                          parameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameters);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("activeChildRecords".ToString(), activeChildRecords.ToString()));
            if (parameters != null)
                if (parameters != null)
                {
                    searchParameterList.AddRange(BuildSearchParameter(parameters));
                }
            try
            {
                RemoteConnectionCheckContainer.GetInstance.ThrowIfNoConnection();
                List<OrderHeaderDTO> result = await Get<List<OrderHeaderDTO>>(OrderHeader_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<OrderHeaderDTO.SearchByParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case OrderHeaderDTO.SearchByParameters.CARD_NUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("cardNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case OrderHeaderDTO.SearchByParameters.CUSTOMER_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("customerName".ToString(), searchParameter.Value));
                        }
                        break;
                    case OrderHeaderDTO.SearchByParameters.ORDER_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("orderId".ToString(), searchParameter.Value));
                        }
                        break;
                    case OrderHeaderDTO.SearchByParameters.TABLE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("tableId".ToString(), searchParameter.Value));
                        }
                        break;
                    case OrderHeaderDTO.SearchByParameters.TABLE_NUMBER:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("tableNumber".ToString(), searchParameter.Value));
                        }
                        break;
                    case OrderHeaderDTO.SearchByParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("isActive".ToString(), searchParameter.Value));
                        }
                        break;
                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }

        public async Task<List<OrderHeaderDTO>> SaveOrderHeader(List<OrderHeaderDTO> orderHeaderDTOList)
        {
            log.LogMethodEntry(orderHeaderDTOList);
            try
            {
                List<OrderHeaderDTO> responseString = await Post<List<OrderHeaderDTO>>(OrderHeader_URL, orderHeaderDTOList);
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
