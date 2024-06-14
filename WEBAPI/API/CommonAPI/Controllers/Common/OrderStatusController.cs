/********************************************************************************************
* Project Name - CommnonAPI - Transaction Module
* Description  - API for the OrderStatus Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.120.0     09-Mar-2021     Prajwal S           Created
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
using Semnox.Parafait.GenericUtilities;
namespace Semnox.CommonAPI.Transaction
{
    public class OrderStatusController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Common/OrderStatus")]
        public async Task<HttpResponseMessage> Get(int orderStatusId = -1, string orderStatus = null, string isActive = null)
        {                              
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(orderStatusId, orderStatus, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>> orderStatusSearchParameters = new List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>>();
                orderStatusSearchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (orderStatusId > -1)
                {
                    orderStatusSearchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.ORDER_STATUS_ID, orderStatusId.ToString()));
                }
                if (!string.IsNullOrWhiteSpace(orderStatus))
                {
                    orderStatusSearchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.ORDER_STATUS, orderStatus.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        orderStatusSearchParameters.Add(new KeyValuePair<OrderStatusDTO.SearchByParameters, string>(OrderStatusDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IOrderStatusUseCases orderStatusUseCases = OrderStatusUseCaseFactory.GetOrderStatusUseCases(executionContext);
                List<OrderStatusDTO> orderStatusDTOLists = await orderStatusUseCases.GetOrderStatus(orderStatusSearchParameters);
                log.LogMethodExit(orderStatusDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = orderStatusDTOLists,
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
        /// <param name="orderStatusDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Common/OrderStatus")]
        public async Task<HttpResponseMessage> Post([FromBody] List<OrderStatusDTO> orderStatusDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(orderStatusDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                IOrderStatusUseCases orderStatusUseCases = OrderStatusUseCaseFactory.GetOrderStatusUseCases(executionContext);
                List<OrderStatusDTO> orderStatusDTOLists = await orderStatusUseCases.SaveOrderStatus(orderStatusDTOList);
                log.LogMethodExit(orderStatusDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = orderStatusDTOLists,
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
