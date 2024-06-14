/********************************************************************************************
 * Project Name -TET 
 * Description  - TETController
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By                    Remarks          
 *********************************************************************************************
 *2.140.2       26-Feb-2022   Girish Kundar                Created 
 ********************************************************************************************/

using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ThirdParty.TET.Controllers
{
    /// <summary>
    /// This API is called by TET team to get transaction details like cutomer name and the number of the visitors
    /// </summary>
    public class TETController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [Route("api/TET/Visitors/{orderId}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage Get (string orderId)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(orderId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                //Get the header content as api key
                log.Debug("inside API Call GetVisitorId ");
                if (!string.IsNullOrEmpty(orderId))
                { 
                    GetVisitor Visitor = new GetVisitor(executionContext, orderId);                    
                    JObject VisitorResponse = Visitor.GetVisitorResponse();
                    log.LogVariableState("VisitorResponse" , VisitorResponse);
                    if (VisitorResponse != new JObject())
                    {
                        log.LogMethodExit("HttpStatusCode.OK");
                        return Request.CreateResponse(HttpStatusCode.OK, VisitorResponse);
                    }
                    else
                    {
                        log.LogMethodExit("HttpStatusCode.BadRequest");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "Visitor Response Null");
                    }
                }
                else
                {
                    log.LogMethodExit("HttpStatusCode.BadRequest:  OrderId null");
                    log.Error("Error - webhook request reference can not be null or Empty" + orderId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,"OrderId null");
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,"Exception"+ex.Message);
            }
        }
        //Creating the execution context
        private ExecutionContext ExecutionContext()
        {
            log.LogMethodEntry("Create context");
            int siteId = Convert.ToInt32(ConfigurationManager.AppSettings["SiteId"]);
            int posMachineId = Convert.ToInt32(ConfigurationManager.AppSettings["POSMachineId"]);
            string loginId = ConfigurationManager.AppSettings["LoginID"];
            ExecutionContext executionContext = new ExecutionContext(loginId, siteId, posMachineId, -1, true, 2);
            log.LogMethodExit(executionContext);
            return executionContext;
        }
        //Check for the valid api key
        private bool ValidateRequest(ExecutionContext executionContext, string incommingApiKey)
        {
            log.LogMethodEntry(executionContext, "incommingApiKey");
            bool result = false;
            try
            {
                //Need to change Redeam API Keys
                string optionType = TETConstants.SYSTEM_OPTION_TYPE;
                string optionName = TETConstants.SYSTEM_OPTION_NAME;
                SystemOptionsBL systemOptionsBL = new SystemOptionsBL(executionContext, optionType, optionName);
                if (systemOptionsBL.GetSystemOptionsDTO != null)
                {
                    log.Debug("Getting TET API keys");
                    string parafaitTETAPIKey = systemOptionsBL.GetSystemOptionsDTO.OptionValue;
                    if (incommingApiKey == parafaitTETAPIKey)
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Debug(" Exception caaught in  ValidateRequest()  method Getting TET API keys");
                log.LogVariableState("message", ex.Message);
                return false;
            }
        }
    }
}
