/********************************************************************************************
 * Project Name - Site Setup
 * Description  - API for the Order Type Group details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By           Remarks          
 *********************************************************************************************
 *2.60        19-Mar-2019   Jagan Mohana Rao      Created 
 *2.60        23-Apr-2019   Mushahid Faizan       Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                  Added isActive SearchParameter in HttpGet Method.
*2.90         14-07-2020     Girish Kundar        Modified : Moved to Transaction resource folder and REST API staandard changes
*2.120.00     16-Jun-2021    Roshan Devadiga      Modified Get,Post and Added Put method
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Transaction
{
    public class OrderTypeGroupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object OrderTypeGroup List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/OrderTypesGroups")]
        public async Task<HttpResponseMessage> Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>(OrderTypeGroupDTO.SearchByParameters.ACTIVE_FLAG, isActive));
                }
                IOrderTypeGroupUseCases orderTypeGroupUseCases = ProductsUseCaseFactory.GetOrderTypeGroupUseCases(executionContext);
                List<OrderTypeGroupDTO> orderTypeGroupDTOList = await orderTypeGroupUseCases.GetOrderTypeGroups(searchParameters);
                log.LogMethodExit(orderTypeGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = orderTypeGroupDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation on orderTypeGroupDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/OrderTypesGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<OrderTypeGroupDTO> orderTypeGroupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(orderTypeGroupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (orderTypeGroupDTOList == null)
                {
                    log.LogMethodExit(orderTypeGroupDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IOrderTypeGroupUseCases orderTypeGroupUseCases = ProductsUseCaseFactory.GetOrderTypeGroupUseCases(executionContext);
                await orderTypeGroupUseCases.SaveOrderTypeGroups(orderTypeGroupDTOList);
                log.LogMethodExit(orderTypeGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Delete operation on orderTypeGroupDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Transaction/OrderTypesGroups")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<OrderTypeGroupDTO> orderTypeGroupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(orderTypeGroupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (orderTypeGroupDTOList != null)
                {
                    OrderTypeGroupListBL orderTypeGroupListBL = new OrderTypeGroupListBL(executionContext, orderTypeGroupDTOList);
                    orderTypeGroupListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the OrderTypeGroupDTOList collection
        /// <param name="orderTypeGroupDTOList">OrderTypeGroupDTOList</param>
        [HttpPut]
        [Route("api/Transaction/OrderTypesGroups")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<OrderTypeGroupDTO> orderTypeGroupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(orderTypeGroupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (orderTypeGroupDTOList == null || orderTypeGroupDTOList.Any(a => a.Id < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IOrderTypeGroupUseCases orderTypeGroupUseCases = ProductsUseCaseFactory.GetOrderTypeGroupUseCases(executionContext);
                await orderTypeGroupUseCases.SaveOrderTypeGroups(orderTypeGroupDTOList);
                log.LogMethodExit(orderTypeGroupDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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