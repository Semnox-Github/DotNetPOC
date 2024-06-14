/********************************************************************************************
 * Project Name - Products
 * Description  - API for the Order Type details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        19-Mar-2019   Jagan Mohana Rao          Created 
 *2.60        18-Apr-2019   Mushahid Faizan           Added log Method Entry & Exit &
                                                      Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                      Added isActive SearchParameter in HttpGet Method.
                                                      Changed Get method from GetOrderTypeDTOList to GetAllOrderTypeList[To Access Child records.]
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Transaction
{
    public class OrderTypeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object OrderType List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/OrderTypes")]
        public HttpResponseMessage Get(string isActive)
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

                OrderTypeListBL OrderTypeListBL = new OrderTypeListBL(executionContext);
                List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<OrderTypeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                bool loadChildActiveRecords = false;
                if (isActive == "1")
                {
                    loadChildActiveRecords = true;
                    searchParameters.Add(new KeyValuePair<OrderTypeDTO.SearchByParameters, string>(OrderTypeDTO.SearchByParameters.ACTIVE_FLAG, isActive));
                }
                var content = OrderTypeListBL.GetOrderTypeDTOList(searchParameters, true, loadChildActiveRecords);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Performs a Post operation on OrderTypeDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Transaction/OrderTypes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<OrderTypeDTO> orderTypeDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(orderTypeDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (orderTypeDTOList != null)
                {
                    // if orderTypeDTOsList.id is less than zero then insert or else update
                    OrderTypeListBL OrderTypeListBL = new OrderTypeListBL(executionContext, orderTypeDTOList);
                    OrderTypeListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        ///// <summary>
        ///// Performs a Delete operation on OrderTypeDTOsList details
        ///// </summary>        
        ///// <returns>HttpResponseMessage</returns>
        //[HttpDelete]
        //[Route("api/Transaction/OrderTypes")]
        //[Authorize]
        //public HttpResponseMessage Delete([FromBody] List<OrderTypeDTO> orderTypeDTOList)
        //{

        //    log.LogMethodEntry(orderTypeDTOList);
        //    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        //    securityTokenBL.GenerateJWTToken();
        //    SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
        //    ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
        //    try
        //    {
        //        if (orderTypeDTOList != null)
        //        {
        //            // if orderTypeDTOsList.id is less than zero then insert or else update
        //            OrderTypeListBL OrderTypeListBL = new OrderTypeListBL(executionContext, orderTypeDTOList);
        //            OrderTypeListBL.SaveUpdateOrderTypeList();
        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
        //        }
        //        else
        //        {
        //            log.LogMethodExit();
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
        //        }
        //    }
        //    catch (ValidationException vexp)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
        //    }
        //    catch (Exception ex)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
        //    }
        //}
    }
}