/********************************************************************************************
 * Project Name - Communications
 * Description  - Controller for MessagingRequests class.
 *  
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.80.0      24-Apr-2020     Girish Kundar     Created
 *2.100.0     15-Sep-2020     Nitin Pai         Push Notification; Added a get method for messaging request
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.CommonAPI.Controllers.Communication
{
    public class MessagingRequestsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Performs a Post operation on messagingTriggerDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Communication/MessagingRequests")]
        public HttpResponseMessage Post([FromBody]List<MessagingRequestDTO> messagingRequestDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(messagingRequestDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (messagingRequestDTOList != null && messagingRequestDTOList.Any())
                {
                    MessagingRequestListBL messagingRequestListBL = new MessagingRequestListBL(executionContext, messagingRequestDTOList);
                    messagingRequestListBL.SaveMessagingRequest();
                    log.LogMethodExit(messagingRequestDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingRequestDTOList });
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
        /// Performs a Get operation on messagingTriggerDTOList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Communication/MessagingRequests")]
        public HttpResponseMessage Get(int messageId = -1, int customerId = -1, int cardId = -1, string messageType = null, DateTime? fromDate = null, DateTime? toDate = null, DateTime? sendDate = null, string status = null, string statusNotIn = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(messageId, customerId, cardId, messageType, fromDate, toDate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now.AddDays(1);
                DateTime messageSentDate = DateTime.Now;

                List<KeyValuePair<MessagingRequestDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MessagingRequestDTO.SearchByParameters, string>>();

                if (String.IsNullOrEmpty(status))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.STATUS, "Success"));
                }
                else
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.STATUS, status));
                }

                if (!String.IsNullOrEmpty(statusNotIn))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.STATUS_NOT_EQ, statusNotIn));
                }

                if (messageId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.ID, messageId.ToString()));
                }

                if (customerId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.CUSTOMER_ID, customerId.ToString()));
                }

                if (cardId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.CARD_ID, cardId.ToString()));
                }

                if (!String.IsNullOrEmpty(messageType))
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.MESSAGE_TYPE, messageType.ToString()));
                }

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-ddThh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                    if (toDate == null)
                        toDate = startDate.AddDays(30);
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-ddThh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }
                }

                if (fromDate != null || toDate != null)
                {
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.FROM_DATE, startDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.TO_DATE, endDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                if (sendDate != null)
                {
                    messageSentDate = Convert.ToDateTime(sendDate.ToString());
                    if (messageSentDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
                    }

                    searchParameters.Add(new KeyValuePair<MessagingRequestDTO.SearchByParameters, string>(MessagingRequestDTO.SearchByParameters.SEND_DATE, messageSentDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                MessagingRequestListBL messagingRequestListBL = new MessagingRequestListBL(executionContext);
                List<MessagingRequestDTO> messagingRequestDTOList = messagingRequestListBL.GetAllMessagingRequestList(searchParameters);

                if (messagingRequestDTOList != null && messagingRequestDTOList.Any())
                {
                    foreach (MessagingRequestDTO msgDTO in messagingRequestDTOList)
                    {
                        if (msgDTO.MessageType.Equals("A") && !String.IsNullOrEmpty(msgDTO.Body) && msgDTO.Body.StartsWith("{"))
                        {
                            try
                            {
                                log.Debug(msgDTO.Id + ":" + msgDTO.Body);
                                dynamic data = JsonConvert.DeserializeObject(msgDTO.Body);
                                log.Debug(data);
                                if (data != null)
                                {
                                    object body = data["data"];
                                    if (body != null)
                                    {
                                        log.Debug("3" + body);
                                        msgDTO.Body = body.ToString();// JsonConvert.DeserializeObject<String>(body.ToString());
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                log.Debug("Error" + ex.Message);
                            }
                        }
                    }
                }
                log.LogMethodExit(messagingRequestDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingRequestDTOList });
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
        /// Updates the message Read Status
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Communication/MessagingRequests/{messagingRequestId}/MessageRead")]
        public HttpResponseMessage StatusPost(int messagingRequestId, [FromBody]MessagingRequestDTO messagingRequestDTO)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(messagingRequestDTO);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (messagingRequestDTO != null && messagingRequestId > -1)
                {
                    MessagingRequestBL messagingRequestBL = new MessagingRequestBL(executionContext, messagingRequestId);
                    if (messagingRequestBL.GetMessagingRequestDTO != null && messagingRequestBL.GetMessagingRequestDTO.Id == messagingRequestId)
                    {
                        messagingRequestBL.GetMessagingRequestDTO.MessageRead = messagingRequestDTO.MessageRead;
                        messagingRequestBL.Save();
                        log.LogMethodExit(messagingRequestBL.GetMessagingRequestDTO);
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = messagingRequestBL.GetMessagingRequestDTO });
                    }
                    else
                    {
                        log.LogMethodExit();
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "MessageId is invalid" });
                    }
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
        /// Updates the message Read Status
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Communication/MessagingRequests/Resend")]
        public async Task<HttpResponseMessage> ResendPost([FromUri]string messageIdList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(messageIdList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (!string.IsNullOrWhiteSpace(messageIdList))
                {
                    IMessagingRequestsUseCases messagingRequestsUseCases = CommunicationUseCaseFactory.GetMessagingRequestsUseCases(executionContext);
                    string result = await messagingRequestsUseCases.SaveMessagingRequestDTO(messageIdList);
                    log.LogMethodExit(result);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
