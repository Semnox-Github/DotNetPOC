/********************************************************************************************
* Project Name - CommnonAPI - POS Redemption Module 
* Description  - API for the TicketReceiptController.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.110.0     17-Nov-2020     Vikas Dwivedi       Created
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
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Redemption;

namespace Semnox.CommonAPI.POS.Redemption
{
    public class TicketReceiptController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Redemption/TicketReceipts")]
        public async Task<HttpResponseMessage> Get(int ticketReceiptId = -1, int redemptionId = -1, string manualTicketReceiptNo = null, int balanceTickets = -1, 
                                    bool isSuspected = false, DateTime? issueFromDate = null, DateTime? issueToDate = null, int sourceRedemptionId = -1)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(ticketReceiptId, redemptionId, manualTicketReceiptNo, balanceTickets, isSuspected, issueFromDate, issueToDate, sourceRedemptionId);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>> ticketReceiptSearchParams = new List<KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>>();
                ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (ticketReceiptId > 0)
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ID, ticketReceiptId.ToString()));
                }
                if (redemptionId > 0)
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.REDEMPTION_ID, redemptionId.ToString()));
                }
                if (sourceRedemptionId > 0)
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.SOURCE_REDEMPTION_ID, sourceRedemptionId.ToString()));
                }
                if (!string.IsNullOrEmpty(manualTicketReceiptNo))
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.MANUAL_TICKET_RECEIPT_NO, manualTicketReceiptNo));
                }
                if (balanceTickets > 0)
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.BALANCE_TICKETS, balanceTickets.ToString()));
                }
                if (isSuspected)
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.IS_SUSPECTED, isSuspected.ToString()));
                }
                if (issueFromDate != null)
                {
                    DateTime fromDate = Convert.ToDateTime(issueFromDate);
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_FROM_DATE, fromDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (issueToDate != null)
                {
                    DateTime toDate = Convert.ToDateTime(issueToDate);
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_TO_DATE, toDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (issueFromDate == null || issueToDate == null)
                {
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_FROM_DATE, DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    ticketReceiptSearchParams.Add(new KeyValuePair<TicketReceiptDTO.SearchByTicketReceiptParameters, string>(TicketReceiptDTO.SearchByTicketReceiptParameters.ISSUE_TO_DATE, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }

                ITicketReceiptUseCases ticketReceiptUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(executionContext);
                List<TicketReceiptDTO> ticketReceiptDTOList = await ticketReceiptUseCases.GetTicketReceipts(ticketReceiptSearchParams);

                log.LogMethodExit(ticketReceiptDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = ticketReceiptDTOList });
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }



        /// <summary>
        /// Performs a Put operation 
        /// </summary>
        /// <param name="TicketReceipt"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("api/Redemption/TicketReceipts")]
        public async Task<HttpResponseMessage> Put([FromBody] List<TicketReceiptDTO> ticketReceiptDTOList) 
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(ticketReceiptDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                if (ticketReceiptDTOList != null && ticketReceiptDTOList.Any(a => a.Id > 0)) 
                {
                    ITicketReceiptUseCases ticketReceiptUseCases = RedemptionUseCaseFactory.GetTicketReceiptUseCases(executionContext);
                    var result = await ticketReceiptUseCases.UpdateTicketReceipts(ticketReceiptDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (UnauthorizedException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Unauthorized, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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
