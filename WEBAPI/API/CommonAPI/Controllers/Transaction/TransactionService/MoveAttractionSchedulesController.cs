/********************************************************************************************
 * Project Name - Transaction Services - RescheduleAttractionBookingBL
 * Description  - Controller to move attraction schedule from one slot to another with cascaded impact
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
using Semnox.Parafait.Product;
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
    public class MoveAttractionSchedulesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        
        /// <summary>
        /// Move attraction schedule from one slot to another. Source Day attraction Schedule DTO is in 0th index, target in 1st 
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Transaction/TransactionService/MoveAttractionSchedules")]
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
                    RescheduleAttractionBookingsBL rescheduleAttractionBookingBL = new RescheduleAttractionBookingsBL(executionContext, moveAttractionBookingDTOList[0], moveAttractionBookingDTOList[1]);
                    rescheduleAttractionBookingBL.MoveAttractionSchedules();
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

        /// <summary>
        /// Get the list of schedules impacted due to a "move schedule"
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/TransactionService/MoveAttractionSchedules")]
        public HttpResponseMessage Get(DateTime scheduleDate, int facilityMapId, int attractionScheduleId, int sourceAttractionScheduleId)
        {
            try
            {
                log.LogMethodEntry(scheduleDate, facilityMapId, attractionScheduleId);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                RescheduleAttractionBookingsBL rescheduleAttractionBookingBL = new RescheduleAttractionBookingsBL(executionContext);
                List<ScheduleDetailsDTO> impactedScheduleDetailsDTO = rescheduleAttractionBookingBL.MoveAttractionSchedulesImpactedSlots(scheduleDate, facilityMapId, attractionScheduleId, sourceAttractionScheduleId);

                log.LogMethodExit(impactedScheduleDetailsDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = impactedScheduleDetailsDTO });
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
