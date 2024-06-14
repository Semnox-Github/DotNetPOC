/********************************************************************************************
 * Project Name - Promotions
 * Description  - Created to insert, update MessageManagement details in Promotions
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.0      03-Sep-2019   Mushahid Faizan  Created 
 *2.80.0      06-Apr-2020   Mushahid Faizan  Renamed Controller from MessageManagement to MessagesTriggerController and
 *                                           Modified as per the Rest Api phase 1 changes.
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

namespace Semnox.CommonAPI.Controllers.Communication
{
    public class MessagingTriggerController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Object of MessageManagement List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Communication/MessagingTriggers")]
        public HttpResponseMessage Get(string isActive = null, bool loadActiveChild = false, bool buildChildRecords = false, string triggerName = null, string typeCode = null,
                                       int triggerId = -1, int receiptTemplateId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(isActive, loadActiveChild, buildChildRecords, triggerName, typeCode, triggerId, receiptTemplateId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                MessagingTriggerListBL messagingTriggerList = new MessagingTriggerListBL(executionContext);
                List<KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>(MessagingTriggerDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (!string.IsNullOrEmpty(triggerName))
                {
                    searchParameters.Add(new KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>(MessagingTriggerDTO.SearchByParameters.TRIGGER_NAME, triggerName.ToString()));
                }
                if (receiptTemplateId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>(MessagingTriggerDTO.SearchByParameters.RECEIPT_TEMPLATE_ID, receiptTemplateId.ToString()));
                }
                if (triggerId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>(MessagingTriggerDTO.SearchByParameters.TRIGGER_ID, triggerId.ToString()));
                }
                if (!string.IsNullOrEmpty(typeCode))
                {
                    searchParameters.Add(new KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>(MessagingTriggerDTO.SearchByParameters.TYPE_CODE, typeCode.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MessagingTriggerDTO.SearchByParameters, string>(MessagingTriggerDTO.SearchByParameters.ACTIVE_FLAG, isActive));
                    }
                }
                var content = messagingTriggerList.GetAllMessagingTriggerList(searchParameters, buildChildRecords, loadActiveChild, null);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Performs a Post operation on messagingTriggerDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Communication/MessagingTriggers")]
        public HttpResponseMessage Post([FromBody]List<MessagingTriggerDTO> messagingTriggerDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(messagingTriggerDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (messagingTriggerDTOList != null && messagingTriggerDTOList.Any())
                {
                    MessagingTriggerListBL messagingTriggerList = new MessagingTriggerListBL(executionContext, messagingTriggerDTOList);
                    messagingTriggerList.Save();
                    log.LogMethodExit(messagingTriggerDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingTriggerDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
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

        /// <summary>
        /// Performs a Delete operation on messagingTriggerDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Authorize]
        [Route("api/Communication/MessagingTriggers")]
        public HttpResponseMessage Delete([FromBody]List<MessagingTriggerDTO> messagingTriggerDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(messagingTriggerDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (messagingTriggerDTOList != null && messagingTriggerDTOList.Any())
                {
                    MessagingTriggerListBL messagingTriggerList = new MessagingTriggerListBL(executionContext, messagingTriggerDTOList);
                    messagingTriggerList.Delete();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }

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
