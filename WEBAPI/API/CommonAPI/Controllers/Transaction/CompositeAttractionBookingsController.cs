/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the AttractionBookings
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By                 Remarks          
 *********************************************************************************************
 *2.80        08-Apr-2020           Girish Kundar               Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
namespace Semnox.CommonAPI.Controllers.Transaction
{
    public class CompositeAttractionBookingsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Post the JSON Object of AttractionBookings List
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Transaction/CompositeAttractionBookings")]
        public HttpResponseMessage Post([FromBody] List<AttractionBookingDTO> attractionBookingDTOList)
        {
            try
            {
                log.LogMethodEntry(attractionBookingDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (attractionBookingDTOList != null && attractionBookingDTOList.Any())
                {
                    double businessDayStartTime = !String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(executionContext,"BUSINESS_DAY_START_TIME")) ? ParafaitDefaultContainerList.GetParafaitDefault<double>(executionContext, "BUSINESS_DAY_START_TIME") : 6;
                    TimeZoneUtil timeZoneUtil = new TimeZoneUtil();

                    foreach (AttractionBookingDTO atbDTO in attractionBookingDTOList)
                    {
                        DateTime scheduleDate = atbDTO.ScheduleFromDate;
                        int offSetDuration = 0;
                        scheduleDate = scheduleDate.Date.AddHours(businessDayStartTime);
                        offSetDuration = timeZoneUtil.GetOffSetDuration(atbDTO.SiteId, scheduleDate);
                        atbDTO.ScheduleFromDate = atbDTO.ScheduleFromDate.AddSeconds(offSetDuration);
                        atbDTO.ScheduleToDate = atbDTO.ScheduleToDate.AddSeconds(offSetDuration);

                        AttractionBooking attractionBooking = new AttractionBooking(executionContext, atbDTO);
                        if (atbDTO.BookingId != -1)
                        {
                            attractionBooking.Expire();
                        }
                        else
                        {
                            attractionBooking.Save(-1);
                            atbDTO.BookingId = attractionBooking.AttractionBookingDTO.BookingId;
                            atbDTO.ExpiryDate = attractionBooking.AttractionBookingDTO.ExpiryDate;
                            //reverse the offset duration
                            atbDTO.ScheduleFromDate = atbDTO.ScheduleFromDate.AddSeconds(-1 * offSetDuration);
                            atbDTO.ScheduleToDate = atbDTO.ScheduleToDate.AddSeconds(-1 * offSetDuration);
                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = attractionBookingDTOList });
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
        /// Gets the AttractionBookings Collection based on Get parameters.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/CompositeAttractionBookings")]
        public HttpResponseMessage Get(int bookingId = -1, int scheduleId = -1, bool loadChildRecords = false,
                                       int attractionPlayId = -1, int trxId = -1, int facilityMapId = -1, string externalReference = null,
                                       DateTime? attractionFromDate = null)
        {
            log.LogMethodEntry(facilityMapId, bookingId, scheduleId, loadChildRecords,attractionPlayId, trxId, externalReference, attractionFromDate);
            try
            {
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<AttractionBookingDTO> attractionBookingDTOList = new List<AttractionBookingDTO>();
                AttractionBookingList attractionBookingList = new AttractionBookingList(executionContext);
                List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AttractionBookingDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (bookingId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.ATTRACTION_BOOKING_ID, bookingId.ToString()));
                }
                if (trxId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.TRX_ID, trxId.ToString()));
                }
                if (facilityMapId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                }
                if (attractionPlayId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.ATTRACTION_PLAY_ID, attractionPlayId.ToString()));
                }
                if (scheduleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.SCHEDULE_ID, scheduleId.ToString()));
                }
                if (attractionFromDate != null)
                {
                    DateTime dateTime = Convert.ToDateTime(attractionFromDate);
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.ATTRACTION_FROM_DATE, dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (string.IsNullOrEmpty(externalReference) == false)
                {
                    searchParameters.Add(new KeyValuePair<AttractionBookingDTO.SearchByParameters, string>(AttractionBookingDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, externalReference));
                }
                attractionBookingDTOList = attractionBookingList.GetAttractionBookingDTOList(searchParameters, loadChildRecords);
                log.LogMethodExit(attractionBookingDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = attractionBookingDTOList });

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
