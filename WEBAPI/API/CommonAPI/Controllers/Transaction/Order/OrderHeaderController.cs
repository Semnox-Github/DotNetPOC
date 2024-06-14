/********************************************************************************************
* Project Name - CommnonAPI - Transaction
* Description  - API for the OrderHeader Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     11-Mar-2021    Prajwal S           Created
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Transaction
{
    public class OrderHeaderController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/OrderHeader")]
        public async Task<HttpResponseMessage> Get(int orderId = -1, int tableId = -1, string customerName = null, string tableNumber = null, string cardNumber = null, string isActive = null, bool buildChildRecords = false, bool loadActiveChild = false)
        {                              
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(orderId, tableId, customerName, tableNumber, cardNumber, isActive, buildChildRecords, loadActiveChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> orderHeaderSearchParameters = new List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>>();
                orderHeaderSearchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (orderId > -1)
                {
                    orderHeaderSearchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.ORDER_ID, orderId.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(cardNumber))
                {
                    orderHeaderSearchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.CARD_NUMBER, cardNumber.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(customerName))
                {
                    orderHeaderSearchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.CUSTOMER_NAME, customerName.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(tableNumber))
                {
                    orderHeaderSearchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.TABLE_NUMBER, tableNumber.ToString()));
                }
                if (tableId > -1)
                {
                    orderHeaderSearchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.TABLE_ID, tableId.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        orderHeaderSearchParameters.Add(new KeyValuePair<OrderHeaderDTO.SearchByParameters, string>(OrderHeaderDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IOrderHeaderUseCases orderHeaderUseCases = TransactionUseCaseFactory.GetOrderHeaderUseCases(executionContext);
                List<OrderHeaderDTO> orderHeaderDTOLists = await orderHeaderUseCases.GetOrderHeader(orderHeaderSearchParameters, buildChildRecords, loadActiveChild);
                log.LogMethodExit(orderHeaderDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = orderHeaderDTOLists,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="orderHeaderDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Transaction/OrderHeader")]
        public async Task<HttpResponseMessage> Post([FromBody] List<OrderHeaderDTO> orderHeaderDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(orderHeaderDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                IOrderHeaderUseCases orderHeaderUseCases = TransactionUseCaseFactory.GetOrderHeaderUseCases(executionContext);
                List<OrderHeaderDTO> orderHeaderDTOLists = await orderHeaderUseCases.SaveOrderHeader(orderHeaderDTOList);
                log.LogMethodExit(orderHeaderDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = orderHeaderDTOLists,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
