/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Custom Attributes
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        03-Oct-2018   Jagan Mohana Rao        Created 
 *2.100.0     27-Aug-2020   Girish Kundar   Modified : Moved helper class files
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.CommonServices
{

    public class CustomAttributesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Events List Details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>        
        [HttpGet]
        [Route("api/CommonServices/CustomAttributes")]
        [Authorize]
        public HttpResponseMessage Get(string entityName = null, string entityId = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(entityName, entityId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                // Applicability : CUSTOMER,PRODUCT,MACHINE,GAMES,GAME_PROFILE,CARDGAMES,INVPRODUCT,LOCATION                
                CustomAttributesWrapperBL customAttributesBL = new CustomAttributesWrapperBL(executionContext);
                var result = customAttributesBL.GetCustomeAttributes(entityName, Convert.ToInt32(entityId));
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result  });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message  });
            }
        }

        /// <summary>
        /// Post the JSON Object CustomAttributesWrapperDTO Details
        /// </summary>
        /// <param name="customAttributesWrapperValues">customAttributesWrapperValues</param>
        /// <param name="applicabilityId">id</param>
        /// <returns>HttpMessgae</returns>     
        [HttpPost]
        [Route("api/CommonServices/CustomAttributes")]
        [Authorize]
        public HttpResponseMessage Post(string applicabilityId, [FromBody]List<CustomAttributesWrapperDTO> customAttributesWrapperValues)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(applicabilityId, customAttributesWrapperValues);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (customAttributesWrapperValues.Count != 0)
                {
                    CustomAttributesWrapperBL customAttributesBL = new CustomAttributesWrapperBL(executionContext);
                    customAttributesBL.SaveUpdateCustomAttributeWrapperList(customAttributesWrapperValues, applicabilityId);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.Debug("CustomAttributesDataController-Post() Method.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message  });
            }
        }
    }
}