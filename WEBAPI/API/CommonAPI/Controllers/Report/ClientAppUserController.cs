/********************************************************************************************
 * Project Name - ClientAppUserController
 * Description  - Controller for Client App User workflow - Register, Sign In, Sign Out
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.110       20-Dec-2020   Nitin Pai         Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ClientApp;

namespace Semnox.CommonAPI.Controllers
{
    public class ClientAppUserController:ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAppUserId"></param>
        /// <param name="clientAppId"></param>
        /// <param name="loginId"></param>
        /// <param name="customerId"></param>
        /// <param name="deviceGuid"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/ClientApp/ClientAppUsers")]
        public HttpResponseMessage Get(int clientAppUserId = -1, int clientAppId = -1, string loginId = null, int customerId = -1, string deviceGuid = null)
        {
            log.LogMethodEntry(clientAppUserId, loginId, customerId, deviceGuid);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));


                ClientAppUserList clientAppUserList = new ClientAppUserList(executionContext);
                List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>> searchParameters = new List<KeyValuePair<ClientAppUserDTO.SearchParameters, string>>();
                if(!string.IsNullOrEmpty(deviceGuid))
                    searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.DEVICE_GUID, deviceGuid));

                if(!string.IsNullOrEmpty(loginId))
                    searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.LOGINID, loginId));

                if(clientAppId > -1)
                    searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.CLIENT_APP_ID, clientAppId.ToString()));

                if (clientAppUserId > -1)
                    searchParameters.Add(new KeyValuePair<ClientAppUserDTO.SearchParameters, string>(ClientAppUserDTO.SearchParameters.CLIENT_APP_USER_ID, clientAppUserId.ToString()));

                List<ClientAppUserDTO> clientAppUserDTOList = clientAppUserList.GetAllClientAppUsers(searchParameters);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = clientAppUserDTOList});
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAppUserDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/ClientApp/ClientAppUsers")]
        public HttpResponseMessage Post([FromBody] List<ClientAppUserDTO> clientAppUserDTOList)
        {
            log.LogMethodEntry(clientAppUserDTOList);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(clientAppUserDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (clientAppUserDTOList != null && clientAppUserDTOList.Any())
                {
                    ClientAppUserList clientAppUserList = new ClientAppUserList(executionContext, clientAppUserDTOList);
                    clientAppUserList.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAppUserDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/ClientApp/ClientAppUser/Register")]
        public HttpResponseMessage Register([FromBody] ClientAppUserDTO clientAppUserDTO)
        {
            log.LogMethodEntry(clientAppUserDTO);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(clientAppUserDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (clientAppUserDTO != null)
                {
                    clientAppUserDTO.ClientAppUserId = -1;
                    ClientAppUser clientAppUserBL = new ClientAppUser(executionContext, clientAppUserDTO);
                    clientAppUserBL.Register();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = clientAppUserBL.GetClientAppUserDTO });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "ClientAppUserDTO not found." });
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAppUserDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/ClientApp/ClientAppUser/Login")]
        public HttpResponseMessage Login([FromBody] ClientAppUserDTO clientAppUserDTO)
        {
            log.LogMethodEntry(clientAppUserDTO);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(clientAppUserDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (clientAppUserDTO != null)
                {
                    ClientAppUser clientAppUserBL = new ClientAppUser(executionContext, clientAppUserDTO);
                    clientAppUserBL.LoginUser();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = clientAppUserBL.GetClientAppUserDTO });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAppUserDTO"></param>
        /// <param name="allDevices"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/ClientApp/ClientAppUser/Logout")]
        public HttpResponseMessage Logout([FromBody] ClientAppUserDTO clientAppUserDTO, [FromUri] Boolean? allDevices = null)
        {
            log.LogMethodEntry(clientAppUserDTO);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(clientAppUserDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (clientAppUserDTO != null && clientAppUserDTO.ClientAppUserId > -1)
                {
                    clientAppUserDTO.UserSignedIn = false;
                    ClientAppUser clientAppUserBL = new ClientAppUser(executionContext, clientAppUserDTO);
                    clientAppUserBL.LoginOutUser(allDevices);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "User signed out successfully" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

    }
}