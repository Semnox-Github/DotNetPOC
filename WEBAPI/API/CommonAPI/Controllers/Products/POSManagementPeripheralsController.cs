/********************************************************************************************
 * Project Name - POSPeripherals Controller
 * Description  - Created to fetch, update and insert POSPeripherals  in the product details.
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        23-Jan-2019   Indrajeet Kumar         Created to get, insert, update and Delete Methods.
 **********************************************************************************************
 *2.60        20-Mar-2019   Akshay Gulaganji        Added customGenericException 
 *********************************************************************************************
 *2.70        29-June-2019  Indrajeet Kumar         Modified Delete - Implemented Hard Deletion
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Peripherals;
using System.Web;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Products
{
    public class POSManagementPeripheralsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the JSON POS Peripherals.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/POSManagementPeripherals")]
        public HttpResponseMessage Get(string isActive, string posMachineId)
        {
            try
            {
                log.LogMethodEntry(isActive, posMachineId);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PeripheralsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                searchParameters.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.POS_MACHINE_ID, posMachineId));
                if (isActive.ToString() == "1")
                {
                    searchParameters.Add(new KeyValuePair<PeripheralsDTO.SearchByParameters, string>(PeripheralsDTO.SearchByParameters.ACTIVE, isActive));
                }
                PeripheralsListBL peripheralsListBL = new PeripheralsListBL(executionContext);
                var content = peripheralsListBL.GetPeripheralsDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
            }
        }

        /// <summary>
        /// Post the JSON POS Peripherals
        /// </summary>
        /// <param name="peripheralsList">peripheralsList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/POSManagementPeripherals")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<PeripheralsDTO> peripheralsList)
        {
            try
            {
                log.LogMethodEntry(peripheralsList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (peripheralsList != null || peripheralsList.Count != 0)
                {
                    PeripheralsListBL peripherals = new PeripheralsListBL(executionContext, peripheralsList);
                    peripherals.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
            }
        }
        /// <summary>
        /// Delete the JSON POS Peripherals
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/POSManagementPeripherals")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<PeripheralsDTO> peripheralsList)
        {
            try
            {
                log.LogMethodEntry(peripheralsList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (peripheralsList != null || peripheralsList.Count != 0)
                {
                    PeripheralsListBL peripherals = new PeripheralsListBL(executionContext, peripheralsList);
                    peripherals.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
            }
        }
    }
}