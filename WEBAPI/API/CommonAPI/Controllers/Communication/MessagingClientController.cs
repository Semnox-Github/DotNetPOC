/********************************************************************************************
* Project Name - CommnonAPI - Communication Module 
* Description  - API for the MessagingClient Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.100.0     07-Aug-2020     Vikas Dwivedi       Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.CommonAPI.Communication
{
    public class MessagingClientController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Communication/MessagingClients")]
        public HttpResponseMessage Get(int clientId = -1, string clientName = null, string messagingChanelCode = null, string isActive = null, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(clientId, clientName, messagingChanelCode, isActive, loadActiveChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>> messagingClientSearchParameters = new List<KeyValuePair<MessagingClientDTO.SearchByParameters, string>>();
                messagingClientSearchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (clientId > -1)
                {
                    messagingClientSearchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.CLIENT_ID, clientId.ToString()));
                }
                if (!string.IsNullOrEmpty(clientName))
                {
                    messagingClientSearchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.CLIENT_NAME, clientName.ToString()));
                }
                if (!string.IsNullOrEmpty(messagingChanelCode))
                {
                    messagingClientSearchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.MESSAGING_CHANNEL_CODE, messagingChanelCode.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        messagingClientSearchParameters.Add(new KeyValuePair<MessagingClientDTO.SearchByParameters, string>(MessagingClientDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                MessagingClientListBL messagingClientListBL = new MessagingClientListBL(executionContext);
                List<MessagingClientDTO> messagingClientDTOList = messagingClientListBL.GetMessagingClientDTOList(messagingClientSearchParameters, null);
                log.LogMethodExit(messagingClientDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingClientDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="messagingClientDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Communication/MessagingClients")]
        public HttpResponseMessage Post([FromBody] List<MessagingClientDTO> messagingClientDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(messagingClientDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (messagingClientDTOList != null && messagingClientDTOList.Any())
                {
                    MessagingClientListBL messagingClientListBL = new MessagingClientListBL(executionContext, messagingClientDTOList);
                    messagingClientListBL.SaveMessagingClient();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingClientDTOList });
                }
                else
                {
                    log.LogMethodExit();
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
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}