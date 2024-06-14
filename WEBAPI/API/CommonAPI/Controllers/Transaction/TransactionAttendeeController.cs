/********************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Controller for Booking Attendee
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.80         18-Mar-2020   Mushahid Faizan      Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Customer;
using System.Linq;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class TransactionAttendeeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;

        [HttpGet]
        [Route("api/Transaction/TransactionAttendees")]
        [Authorize]
        public HttpResponseMessage Get(int transactionId = -1)
        {
            try
            {
                log.LogMethodEntry(transactionId);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                BookingAttendeeList bookingAttendeeList = new BookingAttendeeList(executionContext);
                List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>> searchParam = new List<KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>>();
                searchParam.Add(new KeyValuePair<BookingAttendeeDTO.SearchByParameters, string>(BookingAttendeeDTO.SearchByParameters.TRX_ID, transactionId.ToString()));
                var content = bookingAttendeeList.GetAllBookingAttendeeList(searchParam);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
        }

        [HttpPost]
        [Route("api/Transaction/TransactionAttendees")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<BookingAttendeeDTO> bookingAttendeeDTOList)
        {
            try
            {
                log.LogMethodEntry(bookingAttendeeDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (bookingAttendeeDTOList != null)
                {
                    foreach (BookingAttendeeDTO bookingAttendeeDTO in bookingAttendeeDTOList)
                    {
                        BookingAttendee bookingAttendee = new BookingAttendee(executionContext, bookingAttendeeDTO);
                        bookingAttendee.Save();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "Input not found" });
                }
                
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.Forbidden, new { data = customException });
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
