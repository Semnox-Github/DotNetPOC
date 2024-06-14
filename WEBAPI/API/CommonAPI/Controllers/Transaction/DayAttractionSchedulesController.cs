/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - Controller for DayAttractionSchedules entity
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
    public class DayAttractionSchedulesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Controller for Day Attraction Schedule entity. This is called for Blocking or Reserving a day attraction schedule.
        /// </summary>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Transaction/DayAttractionSchedulesController")]
        public HttpResponseMessage Post([FromBody] List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList)
        {
            try
            {
                log.LogMethodEntry(dayAttractionScheduleDTOList);
                securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (dayAttractionScheduleDTOList != null && dayAttractionScheduleDTOList.Any())
                {

                    foreach (DayAttractionScheduleDTO dasDTO in dayAttractionScheduleDTOList)
                    {
                        // Offset calculation is not being performed as DayAttractionSchedule cannot be created independently and is not accessible as a Web Site use case
                        DayAttractionScheduleBL dasBL = new DayAttractionScheduleBL(executionContext, dasDTO);
                        dasBL.Save();
                        //reverse the offset duration
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = dayAttractionScheduleDTOList });
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
        /// Gets the DayAttractionSchedules Collection based on Get parameters.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Transaction/DayAttractionSchedulesController")]
        public HttpResponseMessage Get(int dayAttractionScheduleId = -1, int scheduleId = -1, bool loadChildRecords = false, int attractionPlayId = -1, 
                                       int facilityMapId = -1, string externalReference = null, DateTime? scheduleDate = null, DateTime? toDate = null,
                                       DateTime? scheduleFromDateTime = null, DateTime? scheduleToDateTime = null, bool? activeRecordsOnly = null, bool? unexpiredRecordsOnly = null)
        {
            log.LogMethodEntry(dayAttractionScheduleId, scheduleId, loadChildRecords, attractionPlayId, facilityMapId, externalReference,
                               scheduleDate, scheduleFromDateTime, scheduleToDateTime);
            try
            {
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime currentDateTime = serverTimeObject.GetServerDateTime();

                List<DayAttractionScheduleDTO> dayAttractionScheduleDTOList = new List<DayAttractionScheduleDTO>();
                DayAttractionScheduleListBL dayAttractionScheduleListBL = new DayAttractionScheduleListBL(executionContext);
                List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (dayAttractionScheduleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.DAY_ATTRACTION_SCHEDULE_ID, dayAttractionScheduleId.ToString()));
                }
                if (scheduleId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_SCHEDULE_ID, scheduleId.ToString()));
                }
                if (facilityMapId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                }
                if (attractionPlayId > -1)
                {
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.ATTRACTION_PLAY_ID, attractionPlayId.ToString()));
                }
                if (scheduleDate != null)
                {
                    DateTime dateTime = Convert.ToDateTime(scheduleDate);
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_DATE, dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (scheduleToDateTime != null)
                {
                    DateTime dateTime = Convert.ToDateTime(scheduleToDateTime);
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_TO_DATE_TIME, dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (scheduleFromDateTime != null)
                {
                    DateTime dateTime = Convert.ToDateTime(scheduleFromDateTime);
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.SCHEDULE_FROM_DATE_TIME, dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (string.IsNullOrEmpty(externalReference) == false)
                {
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.EXTERNAL_SYSTEM_REFERENCE, externalReference));
                }
                if (activeRecordsOnly != null)
                {
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_ACTIVE, activeRecordsOnly.ToString()));
                }
                if (unexpiredRecordsOnly != null)
                {
                    searchParameters.Add(new KeyValuePair<DayAttractionScheduleDTO.SearchByParameters, string>(DayAttractionScheduleDTO.SearchByParameters.IS_UN_EXPIRED, currentDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                dayAttractionScheduleDTOList = dayAttractionScheduleListBL.GetAllDayAttractionScheduleList(searchParameters, null, loadChildRecords);
                log.LogMethodExit(dayAttractionScheduleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = dayAttractionScheduleDTOList });

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
