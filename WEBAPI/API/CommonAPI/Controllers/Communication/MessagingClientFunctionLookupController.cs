/********************************************************************************************
* Project Name - CommnonAPI - Communication Module 
* Description  - API for the MessagingClientFunctionLookup Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.100.0     07-Aug-2020     Vikas Dwivedi       Created
*2.110.0     22-Jan-2021     Guru S A            Subscription changes
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

namespace Semnox.CommonAPI.Games.Controllers.Communication
{
    public class MessagingClientFunctionLookupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Communication/MessagingClientFunctionLookups")]
        public HttpResponseMessage Get(int clientLookupFunctionId = -1, int clientId = -1, string messageType = null, int functionEventId = -1,  string isActive = null, bool buildChildRecords = false, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(clientLookupFunctionId,clientId, messageType, functionEventId, isActive, buildChildRecords, loadActiveChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>> messagingClientFunctionLookupSearchParameters = new List<KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>>();
                messagingClientFunctionLookupSearchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (clientLookupFunctionId > -1)
                {
                    messagingClientFunctionLookupSearchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.ID, clientLookupFunctionId.ToString()));
                }
                if (clientId > -1)
                {
                    messagingClientFunctionLookupSearchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.CLIENT_ID, clientId.ToString()));
                }
                if (!string.IsNullOrEmpty(messageType))
                {
                    messagingClientFunctionLookupSearchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.MESSAGE_TYPE, messageType.ToString()));
                }
                //if (lookupId > -1)
                //{
                //    messagingClientFunctionLookupSearchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.LOOKUP_ID, lookupId.ToString()));
                //}
                //if (lookupValueId > -1)
                //{
                //    messagingClientFunctionLookupSearchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.LOOKUP_VALUE_ID, lookupValueId.ToString()));
                //}
                if (functionEventId > -1)
                {
                    messagingClientFunctionLookupSearchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.PARAFAIT_FUNCTION_EVENT_ID, functionEventId.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        messagingClientFunctionLookupSearchParameters.Add(new KeyValuePair<MessagingClientFunctionLookUpDTO.SearchByParameters, string>(MessagingClientFunctionLookUpDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext);
                List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList = messagingClientFunctionLookUpListBL.GetAllMessagingClientFunctionLookUpList(messagingClientFunctionLookupSearchParameters, buildChildRecords, loadActiveChild, null);
                log.LogMethodExit(messagingClientFunctionLookUpDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingClientFunctionLookUpDTOList });
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
        /// <param name="messagingClientFunctionLookUpDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/Communication/MessagingClientFunctionLookups")]
        public HttpResponseMessage Post([FromBody] List<MessagingClientFunctionLookUpDTO> messagingClientFunctionLookUpDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(messagingClientFunctionLookUpDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (messagingClientFunctionLookUpDTOList != null && messagingClientFunctionLookUpDTOList.Any())
                {
                    MessagingClientFunctionLookUpListBL messagingClientFunctionLookUpListBL = new MessagingClientFunctionLookUpListBL(executionContext, messagingClientFunctionLookUpDTOList);
                    messagingClientFunctionLookUpListBL.SaveMessagingClientFunctionLookUp();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingClientFunctionLookUpDTOList });
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
