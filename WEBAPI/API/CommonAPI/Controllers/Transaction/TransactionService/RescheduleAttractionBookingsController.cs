/********************************************************************************************
 * Project Name - Transaction Services - RescheduleAttractionBookingBL
 * Description  - Controller to reschedule attraction bookings
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.100      24-Sep-2020   Nitin Pai                Created
 ***************************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.Controllers.Transaction.TransactionService
{
    public class RescheduleAttractionBookingsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// The source and target attraction bookings are sent in the same list. All the source bookings have booking id > 0.
        /// All target attraction bookings have booking id = -1. The source is mapped to target using transaction id and line id.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Transaction/TransactionService/RescheduleAttractionBookings")]
        public HttpResponseMessage Post([FromBody] List<AttractionBookingDTO> reschedulAttractionBookingDTOList)
        {
            try
            {
                log.LogMethodEntry(reschedulAttractionBookingDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (reschedulAttractionBookingDTOList != null && reschedulAttractionBookingDTOList.Any())
                {
                    List<AttractionBookingDTO> sourceATBDTOList = reschedulAttractionBookingDTOList.Where(x => x.BookingId > -1).ToList();
                    List<AttractionBookingDTO> targetATBDTOList = reschedulAttractionBookingDTOList.Where(x => x.BookingId == -1).ToList();

                    RescheduleAttractionBookingsBL rescheduleAttractionBookingBL = new RescheduleAttractionBookingsBL(executionContext, sourceATBDTOList, targetATBDTOList);
                    rescheduleAttractionBookingBL.RescheduleAttractionBookings();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = reschedulAttractionBookingDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
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
