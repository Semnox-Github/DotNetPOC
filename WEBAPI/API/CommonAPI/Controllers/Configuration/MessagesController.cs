/********************************************************************************************
 * Project Name - Site Setup
 * Description  - API for the Messages details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Mushahid Faizan   Created 
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Configuration
{
    public class MessagesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object Values List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/Messages")]
        public async Task<HttpResponseMessage> Get(string activityType = null, int currentPage = 0, int pageSize = 5, string messages = null)
        {
            try
            {
                log.LogMethodEntry(activityType, currentPage, pageSize);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>> searchParameters = new List<KeyValuePair<MessagesDTO.SearchByMessageParameters, string>>();
                searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                bool messageFilter = false;
                int MessageNo = 10000;

                if (!string.IsNullOrEmpty(messages))
                {
                    messageFilter = true;
                    searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.MESSAGE, messages.ToString()));
                }
                if (!string.IsNullOrEmpty(activityType) && activityType.ToUpper().ToString() == "M") // If type =="M" it will check for messages
                {
                    searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.MESSAGE_NO, MessageNo.ToString()));
                }
                else if (!string.IsNullOrEmpty(activityType) && activityType.ToUpper().ToString() == "L") // If type =="L" it will check for Literals
                {
                    searchParameters.Add(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.LITERAL_MESSAGE_NO, MessageNo.ToString()));
                }

                int totalNoOfPages = 0;
                MessageListBL messagesList = new MessageListBL(executionContext);
                int totalNoOfMessages = await Task<int>.Factory.StartNew(() => { return messagesList.GetMessagesCount(searchParameters); });
                log.LogVariableState("totalNoOfMessages", totalNoOfMessages);
                totalNoOfPages = (totalNoOfMessages / pageSize) + ((totalNoOfMessages % pageSize) > 0 ? 1 : 0);

                int maxMessageNo = 0;
                IList<MessagesDTO> messagesDTOList = null;
                messagesDTOList = messagesList.GetMessagesDTOList(searchParameters, currentPage, pageSize, true);
                if (messagesDTOList != null && messagesDTOList.Count != 0)
                {

                }

                if (totalNoOfPages > 0)
                {
                    if (messageFilter)
                    {
                        searchParameters.Remove(new KeyValuePair<MessagesDTO.SearchByMessageParameters, string>(MessagesDTO.SearchByMessageParameters.MESSAGE, messages.ToString()));
                    }
                    maxMessageNo = messagesList.GetMaxMessageNo(searchParameters);
                }
                bool isProtected = false;
                if (securityTokenDTO.LoginId.ToLower() != "semnox")
                {
                    isProtected = true;
                }
                log.LogMethodExit(messagesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = messagesDTOList, isProtectedReadonly = isProtected, currentPageNo = currentPage, TotalCount = totalNoOfMessages, MaxMessageNo = maxMessageNo  });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message  });
            }
        }
        /// <summary>
        /// Performs a Post operation on MessageDTO List details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Configuration/Messages")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MessagesDTO> messageDTOList)
        {
            try
            {
                log.LogMethodEntry(messageDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (messageDTOList != null && messageDTOList.Any())
                {
                    // if messageDTOList.MessageId is less than zero then insert or else update
                    MessageListBL messageListBL = new MessageListBL(executionContext, messageDTOList);
                    messageListBL.SaveUpdateMessages();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }
        /// <summary>
        /// Performs a Delete operation on MessageDTO List details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/Configuration/Messages")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<MessagesDTO> messageDTOList)
        {
            try
            {
                log.LogMethodEntry(messageDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (messageDTOList != null)
                {
                    MessageListBL messageListBL = new MessageListBL(executionContext, messageDTOList);
                    messageListBL.SaveUpdateMessages();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }
    }
}
