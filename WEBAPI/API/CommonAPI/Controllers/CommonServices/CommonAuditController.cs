/********************************************************************************************
 * Project Name - Coomon  Audit Controller.                                                                          
 * Description  - COntroller class for the Audit.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.40        18-Sep-2018   Rajiv          Created.
 *********************************************************************************************
 *2.50        11-Mar-2019   Akshay Gulaganji    Added executionContext 
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using System.Web.Http;
using System.Web;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.CommonAPI.CommonServices
{
    public class CommonAuditController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the Audit values for machine, game, game profile ,Products,ProductGames, ProductCreditPlus, ProductCreditPlusConsumption  List.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/CommonServices/CommonAudit/")]
        public HttpResponseMessage Get(int auditId, string tablename)
        {
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.LogMethodEntry(auditId, tablename);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<List<List<string>>> auditList = new List<List<List<string>>>();
                AuditLog auditLog = new AuditLog(executionContext);
                auditList = auditLog.GetAuditList(auditId, tablename);
                if (auditList.Count != 0)
                {
                    log.LogMethodExit(auditList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = auditList, token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
    }
}