/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API to Shutdown the PC. Digital Signage --> Setup --> Panel --> ShutDown PC
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *2.90        13-Aug-2020    Mushahid Faizan  Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.DigitalSignage;
using Semnox.Core.GenericUtilities;

namespace Semnox.CommonAPI.Controllers.DigitalSignage
{
    public class ShutdownPCController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Post operation on panel details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/DigitalSignage/ShutdownPC")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<DisplayPanelDTO> displayPanelDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(displayPanelDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (displayPanelDTOList != null && displayPanelDTOList.Any())
                {
                    DisplayPanelListBL displayPanelBL = new DisplayPanelListBL(executionContext, displayPanelDTOList);
                    displayPanelBL.DisplayPanelListShutdownPC();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }
    }
}
