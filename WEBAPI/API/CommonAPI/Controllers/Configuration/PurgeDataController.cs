/********************************************************************************************
 * Project Name - Site Setup
 * Description  - API for the Purge Data in site Setup module.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.60        08-May-2019   Mushahid Faizan           Created 
*2.90.0      14-Jun-2020    Girish Kundar             Modified : REST API phase 2 changes/standard  
 ********************************************************************************************/

using System;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class PurgeDataController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object of Purge Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/PurgeData")]
        public HttpResponseMessage Get()
        {
            log.LogMethodEntry();
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                PurgeDataDTO purgeDataDTO = new PurgeDataDTO();
                purgeDataDTO.Date = Convert.ToDateTime("01" + DateTime.Now.AddMonths(-1).Date.ToString("MMM-yyyy"));
                purgeDataDTO.CardsDate = purgeDataDTO.Gameplaydate = purgeDataDTO.TransactionsDate = purgeDataDTO.LogsDate = purgeDataDTO.Date;
                var content = purgeDataDTO;
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = purgeDataDTO });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on Purge details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Configuration/PurgeData")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] PurgeDataDTO purgeDataDTO)
        {
                log.LogMethodEntry(purgeDataDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                try
                {
                    if (purgeDataDTO != null)
                {
                    PurgeDataBL purgeDataBL = new PurgeDataBL(executionContext, purgeDataDTO);
                    purgeDataBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
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
    }
}
