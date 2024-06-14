/********************************************************************************************
 * Project Name - Transaction
 * Description  - Created to fetch, update and insert NotificationTagIssued Details
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.2     05-Dec-2022   Abhishek              Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Tags
{
    public class NotificationTagIssuedController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON NotificationTagIssued
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/NotificationTagIssued")]
        public async Task<HttpResponseMessage> Get(string isActive = null, int notificationTagIssuedId = -1, int cardId = -1, int transactionId = -1,
                                                   int lineId = -1, bool isReturned = false)
        {
            log.LogMethodEntry(isActive, notificationTagIssuedId, cardId, transactionId, lineId, isReturned);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                if (notificationTagIssuedId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.NOTIFICATIONTAGISSUEDID, notificationTagIssuedId.ToString()));
                }
                if (cardId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.CARDID, cardId.ToString()));
                }
                if (transactionId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.TRANSACTIONID, transactionId.ToString()));
                }
                if (lineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.LINEID, lineId.ToString()));
                }
                if (isReturned)
                {
                    searchParameters.Add(new KeyValuePair<NotificationTagIssuedDTO.SearchByParameters, string>(NotificationTagIssuedDTO.SearchByParameters.ISRETURNED, "1"));
                }
                INotificationTagIssuedUseCases notificationTagIssuedUseCases = TransactionUseCaseFactory.GetNotificationTagIssuedUseCases(executionContext);
                List<NotificationTagIssuedDTO> notificationTagsDTOList = await notificationTagIssuedUseCases.GetNotificationTagIssued(searchParameters);
                log.LogMethodExit(notificationTagsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = notificationTagsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON NotificationTagIssuedDTO
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>        
        [HttpPost]
        [Route("api/Transaction/NotificationTagIssued")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<NotificationTagIssuedDTO> notificationTagIssuedDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(notificationTagIssuedDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (notificationTagIssuedDTOList != null)
                {
                    INotificationTagIssuedUseCases notificationTagIssuedUseCases = TransactionUseCaseFactory.GetNotificationTagIssuedUseCases(executionContext);
                    List<NotificationTagIssuedDTO> response = await notificationTagIssuedUseCases.SaveNotificationTagIssued(notificationTagIssuedDTOList);
                    log.LogMethodExit(response);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
