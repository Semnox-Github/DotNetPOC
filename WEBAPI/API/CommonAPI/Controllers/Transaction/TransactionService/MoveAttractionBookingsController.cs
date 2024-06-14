/********************************************************************************************
 * Project Name - Transaction Services - RescheduleAttractionBookingBL
 * Description  - Controller to move attraction bookings from one slot to another
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.100      24-Sep-2020   Nitin Pai                Created
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Transaction.TransactionService
{
    public class MoveAttractionBookingsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Move attraction bookings. Source Day Attraction Schedule DTO is sent in 0th index and target in 1st  
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Transaction/TransactionService/MoveAttractionBookings")]
        public HttpResponseMessage Post([FromBody] List<DayAttractionScheduleDTO> moveAttractionBookingDTOList)
        {
            try
            {
                log.LogMethodEntry(moveAttractionBookingDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (moveAttractionBookingDTOList != null && moveAttractionBookingDTOList.Count == 2)
                {
                    RescheduleAttractionBookingsBL rescheduleAttractionBookingBL = new RescheduleAttractionBookingsBL(executionContext,moveAttractionBookingDTOList[0], moveAttractionBookingDTOList[1]);
                    rescheduleAttractionBookingBL.MoveAttractionBookings();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = moveAttractionBookingDTOList });
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
