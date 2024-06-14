/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  -LoginController : Does the login function and returns the token on successful login
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *2.110.0    28-Sept-2020           Girish Kundar          Created 
 *******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.ThirdParty.CenterEdge;

namespace Semnox.CommonAPI.ThirdParty.CenterEdge
{
    public class AuthenticateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [HttpPost]
        [Route("api/ThirdParty/CenterEdge/Login")]
        public HttpResponseMessage Post([FromBody] LoginDTO login)
        {
            log.LogMethodEntry(login);
            log.LogVariableState("LoginDTO:", login);
            SecurityTokenDTO securityTokenDTO = null;
            try
            {
                log.Debug("login.requestTimestamp : " + login.requestTimestamp);
                if (login == null)
                {
                    log.Error("Login details are not valid");
                    throw new ValidationException("Invalid login");
                }
                else if(login.requestTimestamp == null)
                {
                    log.Error("login.requestTimestamp == null - Invalid timestamp. Please re-login");
                    throw new ValidationException("Invalid timestamp. Please re-login");
                }

                DateTime serverDateTime = DateTime.UtcNow;
                log.Debug("serverDateTime in UTC : " + serverDateTime);
                DateTime requestTimestampDate = Convert.ToDateTime(login.requestTimestamp).ToUniversalTime();
                log.Debug("login.requestTimestamp Date in UTC: " + requestTimestampDate);
                TimeSpan span = serverDateTime.Subtract(requestTimestampDate);
                log.Debug("userRequestedTime span: " + span);

                if (span.Hours> 0 || (span.Minutes > 5 || span.Minutes < -5))
                {
                    log.Error("span.Minutes > 5 - Invalid timestamp. Please re-login");
                    throw new ValidationException("Invalid timestamp. Please re-login");
                }
               
                Security.User user = null;
                bool isCorporate = Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]);
                try
                {   
                    LoginBL loginBL = new LoginBL(login);
                    user = loginBL.Login();
                }
                catch (Security.SecurityException se)
                {
                    log.Error(se.Message);
                    Request.Headers.Authorization = new AuthenticationHeaderValue(new Guid().ToString());
                    return Request.CreateResponse(HttpStatusCode.Forbidden, new { code = "invalidLogin" , message= "Incorrect username or password" });
                }

                if (user != null && user.UserId != 0)
                {
                    int defaultLanguageId = -1;
                    int posMachineId = -1;
                 
                    POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(user.SiteId, "CenterEdge", "", -1);
                    if (pOSMachineContainerDTO != null)
                    {
                        posMachineId = pOSMachineContainerDTO.POSMachineId;
                    }
                    int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(user.SiteId, "JWT_TOKEN_LIFE_TIME", 0);
                    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                    securityTokenBL.GenerateNewJWTToken(user.LoginId, user.GUID, user.SiteId.ToString(), defaultLanguageId.ToString(), user.RoleId.ToString(),"User", posMachineId.ToString(), Guid.NewGuid().ToString(), tokenLifeTime); // Machine Id need to add or from get execution context API
                    securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                    log.LogMethodExit(securityTokenDTO);
                    Request.Headers.Authorization = new AuthenticationHeaderValue(securityTokenDTO.Token);
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, new { bearerToken =  securityTokenDTO.Token });
                    return response;
                }
                else
                {
                    String message = MessageContainerList.GetMessage(ExecutionContext.GetExecutionContext(), 735);
                    log.Error(message);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = ErrorCode.invalidLogin.ToString(), message = "Unauthorized" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, ExecutionContext.GetExecutionContext());
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = ErrorCode.invalidLogin.ToString(), message = "Unauthorized" });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { code = ErrorCode.invalidLogin.ToString(), message = "Unauthorized" });
            }
        }
    }
}

