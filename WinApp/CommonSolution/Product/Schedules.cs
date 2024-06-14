/* Project Name - Semnox.Parafait.Booking.Schedules 
* Description  - Business call object of the Schedules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
*********************************************************************************************
*1.00        25-Dec-2016    Rakshith             Created 
*2.50        26-Nov-2018    Guru S A             Booking enhancement changes 
*2.70        23-Mar-2019    Guru S A             Booking Phase 2 changes 
*2.70        27-Jun-2019    Akshay G             Added DeleteSchedules() method
*2.80.0      21-Feb-2020     Girish Kundar        Modified : 3 tier Changes for REST API
*2.90.0      08- Sep-2020     Girish Kundar        Modified : Added validation for from time and to time
*2.130.11    20-Oct-2022    Nitin Pai            Modified: if multiple rules are present, select the date based rule first
********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{

    /// <summary>
    ///  Schedules
    /// </summary>
    public class SchedulesBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SchedulesDTO scheduleDTO;
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor of AttractionSchedulesBL class
        /// </summary>
        /// </summary>
        public SchedulesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            scheduleDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the attractionSchedule id as the parameter
        /// Would fetch the attractionSchedule object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">optional sql transaction</param>
        public SchedulesBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null, bool buildChildRecords = false)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            SchedulesDataHandler scheduleDataHandler = new SchedulesDataHandler(sqlTransaction);
            scheduleDTO = scheduleDataHandler.GetScheduleDTO(id);
            if (buildChildRecords)
            {
                BuildChildRecordDetails(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates SchedulesBL object using the ScheduleDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="scheduleDTO">scheduleDTO object</param>
        public SchedulesBL(ExecutionContext executionContext, SchedulesDTO scheduleDTO, bool buildChildRecords = false)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scheduleDTO);
            this.scheduleDTO = scheduleDTO;
            if (buildChildRecords)
            {
                BuildChildRecordDetails();
            }
            log.LogMethodExit();
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            if (string.IsNullOrWhiteSpace(scheduleDTO.ScheduleName))
            {
                validationErrorList.Add(new ValidationError("MasterSchedule", "ScheduleName", MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Schedule Name"))));
            }

            if (!string.IsNullOrWhiteSpace(scheduleDTO.ScheduleName) && scheduleDTO.ScheduleName.Length > 50)
            {
                validationErrorList.Add(new ValidationError("MasterSchedule", "ScheduleName", MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Schedule Name"), 50)));
            }

            if (scheduleDTO.ScheduleToTime <= scheduleDTO.ScheduleTime)
            {
                validationErrorList.Add(new ValidationError("MasterSchedule", "ScheduleTime", MessageContainerList.GetMessage(executionContext, 2772, scheduleDTO.ScheduleName))); //Checks if to time is less than from time
                log.LogMethodExit("Schedule ToTime should be greater than Schedule FromTime");
            }
            if (scheduleDTO.ScheduleRulesDTOList != null && scheduleDTO.ScheduleRulesDTOList.Count > 0)
            {
                foreach (var scheduleRulesDTO in scheduleDTO.ScheduleRulesDTOList)
                {
                    if (scheduleRulesDTO.IsChanged)
                    {
                        ScheduleRulesBL scheduleRulesBL = new ScheduleRulesBL(executionContext, scheduleRulesDTO);
                        validationErrorList.AddRange(scheduleRulesBL.Validate(sqlTransaction));
                    }
                }
            }
            ValidateOverlaps();
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private DateTime GetValidDate(decimal hour)
        {
            log.LogMethodEntry(hour);
            string hr = hour.ToString();
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            DateTime date = serverTimeObject.GetServerDateTime().Date.AddHours(hr.IndexOf(".") == -1 ? Convert.ToInt32(hour) : Convert.ToInt32(hr.Substring(0, hr.IndexOf("."))));

            try
            {
                if (hour.ToString().Contains("."))
                {
                    string min = String.Format("{0:0.00}", hr.Substring(hr.IndexOf(".") + 1, hr.Length - hr.IndexOf(".") - 1));
                    date = date.AddMinutes(Convert.ToInt32(min));
                }
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit(date);
            return date;
        }

        void ValidateOverlaps()
        {
            log.LogMethodEntry();
            List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID, scheduleDTO.MasterScheduleId.ToString()));
            MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext);
            List<MasterScheduleDTO> masterScheduleDTOList = masterScheduleList.GetMasterScheduleDTOsList(searchParameters, true, true, -1);
            List<SchedulesDTO> list = new List<SchedulesDTO>();
            if (masterScheduleDTOList != null && masterScheduleDTOList.Any())
            {
                list = masterScheduleDTOList[0].SchedulesDTOList;
            }
            foreach (SchedulesDTO dto in list)
            {
                decimal timeFrom1, timeTo1;
                if (dto.ScheduleTime == 0)
                    timeFrom1 = 0;
                else
                    timeFrom1 = Convert.ToDecimal(String.Format("{0:0.00}", dto.ScheduleTime));

                if (dto.ScheduleToTime == 0)
                    timeTo1 = 24;
                else
                {
                    timeTo1 = Convert.ToDecimal(String.Format("{0:0.00}", dto.ScheduleToTime));
                }

                if (scheduleDTO.ScheduleId != dto.ScheduleId && scheduleDTO.ActiveFlag)
                {
                    decimal timeFrom2, timeTo2;
                    if (scheduleDTO.ScheduleTime == 0)
                        timeFrom2 = 0;
                    else
                        timeFrom2 = Convert.ToDecimal(String.Format("{0:0.00}", scheduleDTO.ScheduleTime));

                    if (scheduleDTO.ScheduleToTime == 0)
                        timeTo2 = 24;
                    else
                    {
                        timeTo2 = Convert.ToDecimal(String.Format("{0:0.00}", scheduleDTO.ScheduleToTime));
                    }

                    bool timeOverlap = false;

                    DateTime dateFrom1 = (timeFrom1 >= 0 && timeFrom1 <= 6) ? GetValidDate(timeFrom1).AddDays(1) : GetValidDate(timeFrom1);
                    DateTime dateTo1 = (timeTo1 >= 0 && timeTo1 <= 6) ? GetValidDate(timeTo1).AddDays(1) : GetValidDate(timeTo1);
                    DateTime dateFrom2 = (timeFrom2 >= 0 && timeFrom2 <= 6) ? GetValidDate(timeFrom2).AddDays(1) : GetValidDate(timeFrom2);
                    DateTime dateTo2 = (timeTo2 >= 0 && timeTo2 <= 6) ? GetValidDate(timeTo2).AddDays(1) : GetValidDate(timeTo2);

                    if (dateFrom1 < dateTo2 && dateFrom2 < dateTo1)
                    {
                        timeOverlap = true;
                    }

                    if (timeOverlap)
                    {
                        log.LogMethodExit(true);
                        string msg = MessageContainerList.GetMessage(executionContext, 936, scheduleDTO.ScheduleName, dto.ScheduleName);
                        log.Info(msg);
                        throw new ValidationException(msg);
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the Schedule
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (scheduleDTO.IsChangedRecursive == false
                 && scheduleDTO.ScheduleId >-1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            SchedulesDataHandler scheduleDataHandler = new SchedulesDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (scheduleDTO.ScheduleId < 0)
            {
                scheduleDTO = scheduleDataHandler.InsertSchedule(scheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                if (!string.IsNullOrEmpty(scheduleDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(executionContext);
                    auditLog.AuditTable("AttractionSchedules", scheduleDTO.Guid, sqlTransaction);
                }
                scheduleDTO.AcceptChanges();
            }
            else
            {
                if (scheduleDTO.IsChanged)
                {
                    scheduleDTO = scheduleDataHandler.UpdateSchedule(scheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    if (!string.IsNullOrEmpty(scheduleDTO.Guid))
                    {
                        AuditLog auditLog = new AuditLog(executionContext);
                        auditLog.AuditTable("AttractionSchedules", scheduleDTO.Guid, sqlTransaction);
                    }
                    scheduleDTO.AcceptChanges();
                }
            }
            SaveChildList(sqlTransaction);
            log.LogMethodExit();
        }

        private void SaveChildList(SqlTransaction sqlTransaction)
        {
            if (scheduleDTO.ScheduleRulesDTOList != null && scheduleDTO.ScheduleRulesDTOList.Count > 0)
            {
                foreach (ScheduleRulesDTO scheduleRulesDTO in scheduleDTO.ScheduleRulesDTOList)
                {
                    if (scheduleRulesDTO.ScheduleId != scheduleDTO.ScheduleId)
                    {
                        scheduleRulesDTO.ScheduleId = scheduleDTO.ScheduleId;
                    }
                    if (scheduleRulesDTO.IsChanged)
                    {
                        ScheduleRulesBL scheduleRulesBL = new ScheduleRulesBL(executionContext, scheduleRulesDTO);
                        scheduleRulesBL.Save(sqlTransaction);
                    }
                }
            }
        }

        /// <summary>
        /// Delete the Schedules Details based on scheduleId
        /// </summary>
        /// <param name="scheduleId">scheduleId</param>        
        /// <param name="sqlTransaction">sqlTransaction</param>        
        public void DeleteSchedules(SchedulesDTO scheduleDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scheduleDTO, sqlTransaction);
            try
            {
                SchedulesDataHandler scheduleDataHandler = new SchedulesDataHandler(sqlTransaction);
                if (scheduleDTO.ScheduleRulesDTOList != null &&
                            scheduleDTO.ScheduleRulesDTOList.Any(x => x.IsActive == true))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                scheduleDataHandler.DeleteSchedules(scheduleDTO.ScheduleId);
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public SchedulesDTO ScheduleDTO
        {
            get
            {
                return scheduleDTO;
            }
        }


        public ScheduleDetailsDTO GetEligibleScheduleDetails(DateTime scheduleDate, decimal fromTime, decimal toTime, int facilityMapId)
        {
            log.LogMethodEntry(scheduleDate, fromTime, toTime, facilityMapId);
            scheduleDate = scheduleDate.Date;
            log.LogVariableState("ScheduleDate: After date conv", scheduleDate);
            ScheduleDetailsDTO scheduleDetailsDTO = null;
            if (this.scheduleDTO != null)
            {
                bool eligibleSchedule = false;
                TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                int offSetDuration = timeZoneUtil.GetOffSetDuration(executionContext.GetSiteId(), scheduleDate);
                log.LogVariableState("offSetDuration", offSetDuration);
                if (offSetDuration != 0)
                {
                    offSetDuration = offSetDuration * -1;
                    log.LogVariableState("offSetDuration", offSetDuration);
                    DateTime fromDate = scheduleDate.Date.AddMinutes((int)fromTime * 60 + (double)fromTime % 1 * 100);
                    DateTime toDate = scheduleDate.Date.AddMinutes((int)toTime * 60 + (double)toTime % 1 * 100);
                    fromDate = fromDate.AddSeconds(offSetDuration);
                    toDate = toDate.AddSeconds(offSetDuration);
                    fromTime = Convert.ToDecimal(fromDate.Hour + fromDate.Minute / 100.0);
                    toTime = Convert.ToDecimal(toDate.Hour + toDate.Minute / 100.0);
                }
                log.LogVariableState("fromTime", fromTime);
                log.LogVariableState("toTime", toTime);
                if (this.scheduleDTO.FixedSchedule)
                {
                    eligibleSchedule = (this.scheduleDTO.ScheduleTime >= fromTime && this.scheduleDTO.ScheduleTime <= toTime);
                }
                else
                {
                    //    eligibleSchedule = ((this.scheduleDTO.ScheduleTime >= fromTime && this.scheduleDTO.ScheduleTime <= toTime && this.scheduleDTO.ScheduleToTime >= toTime)
                    //                      || (fromTime >= this.scheduleDTO.ScheduleTime && fromTime <= this.scheduleDTO.ScheduleToTime && this.scheduleDTO.ScheduleToTime >= toTime));
                    eligibleSchedule = (fromTime >= this.scheduleDTO.ScheduleTime && fromTime < this.scheduleDTO.ScheduleToTime && toTime > this.scheduleDTO.ScheduleTime && toTime <= this.scheduleDTO.ScheduleToTime);
                }
                if (eligibleSchedule)
                {
                    if (facilityMapId > -1)
                    {
                        FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, facilityMapId);
                        FacilityMapDTO facDTO = facilityMapBL.FacilityMapDTO;
                        scheduleDetailsDTO = BuildScheduleDetails(facDTO, scheduleDate);
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2128));//Facility information is required

                    }
                }
            }
            log.LogMethodExit(scheduleDetailsDTO);
            return scheduleDetailsDTO;
        }

        private void BuildChildRecordDetails(SqlTransaction sqltrx = null)
        {
            log.LogMethodEntry();
            if (this.scheduleDTO != null && this.scheduleDTO.ScheduleId > -1)
            {
                ScheduleRulesList scheduleRulesList = new ScheduleRulesList(executionContext);
                List<KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>> srSearchParams = new List<KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>>();
                srSearchParams.Add(new KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>(ScheduleRulesDTO.SearchByParameters.SCHEDULE_ID, this.scheduleDTO.ScheduleId.ToString()));
                srSearchParams.Add(new KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>(ScheduleRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                srSearchParams.Add(new KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>(ScheduleRulesDTO.SearchByParameters.IS_ACTIVE, "1"));
                List<ScheduleRulesDTO> scheduleRulesDTOList = scheduleRulesList.GetScheduleRulesDTOList(srSearchParams, sqltrx);
                this.scheduleDTO.ScheduleRulesDTOList = scheduleRulesDTOList;
            }
            log.LogMethodExit();
        }
        public ScheduleDetailsDTO BuildScheduleDetails(FacilityMapDTO facDTO, DateTime scheduleDate)
        {
            log.LogMethodEntry(scheduleDate, facDTO);
            ScheduleDetailsDTO scheduleDetailsDTO = null;
            MasterScheduleBL masterScheduleBL = new MasterScheduleBL(executionContext, this.scheduleDTO.MasterScheduleId);
            DateTime scheduleFromDate = scheduleDate.Date.AddMinutes((int)this.scheduleDTO.ScheduleTime * 60 + (double)this.scheduleDTO.ScheduleTime % 1 * 100);//scheduleDate.AddHours((double)this.scheduleDTO.ScheduleTime)
            DateTime scheduleToDate = scheduleDate.Date.AddMinutes((int)this.scheduleDTO.ScheduleToTime * 60 + (double)this.scheduleDTO.ScheduleToTime % 1 * 100);
            if (scheduleDTO.ScheduleToTime < scheduleDTO.ScheduleTime)
                scheduleToDate = scheduleToDate.AddDays(1);

            log.LogVariableState("scheduleFromDate", scheduleFromDate);
            log.LogVariableState("scheduleToDate", scheduleToDate);
            //FacilityMapBL facilityMapBL = new FacilityMapBL(executionContext, facilityMapId); 
            //FacilityMapDTO facDTO = facilityMapBL.FacilityMapDTO;

            if (facDTO != null && facDTO.ProductsAllowedInFacilityDTOList != null && facDTO.ProductsAllowedInFacilityDTOList.Any())
            {
                int count = facDTO.ProductsAllowedInFacilityDTOList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (facDTO.ProductsAllowedInFacilityDTOList[i].ProductsDTO != null)
                    {
                        //and @Fromdate BETWEEN ISNULL(p.StartDate,@Fromdate) and ISNULL(p.ExpiryDate,@Fromdate+1)
                        DateTime prodStartDate = (facDTO.ProductsAllowedInFacilityDTOList[i].ProductsDTO.StartDate == DateTime.MinValue ? scheduleDate : facDTO.ProductsAllowedInFacilityDTOList[i].ProductsDTO.StartDate);
                        DateTime prodExpireDate = (facDTO.ProductsAllowedInFacilityDTOList[i].ProductsDTO.ExpiryDate == DateTime.MinValue ? scheduleDate.AddDays(1) : facDTO.ProductsAllowedInFacilityDTOList[i].ProductsDTO.ExpiryDate);
                        if ((scheduleDate >= prodStartDate && scheduleDate <= prodExpireDate) == false)
                        {
                            facDTO.ProductsAllowedInFacilityDTOList.RemoveAt(i);
                            i = i - 1;
                            count = count - 1;
                        }
                    }
                }
            }
            if (this.scheduleDTO.ScheduleRulesDTOList != null && this.scheduleDTO.ScheduleRulesDTOList.Any()) //Facility has rules
            {
                TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                int offSetDuration = timeZoneUtil.GetOffSetDuration(executionContext.GetSiteId(), scheduleDate);
                offSetDuration = offSetDuration * -1;
                log.LogVariableState("offSetDuration", offSetDuration);

                List<ScheduleRulesDTO> facilityScheduleRulesDTOList = this.scheduleDTO.ScheduleRulesDTOList.Where(sRule => sRule.FacilityMapId == facDTO.FacilityMapId
                                                                                                                    && (
                                                                                                                        (sRule.FromDate != null && sRule.ToDate != null && scheduleDate >= ((DateTime)sRule.FromDate).AddSeconds(offSetDuration) && scheduleDate < ((DateTime)sRule.ToDate).AddDays(1).AddSeconds(offSetDuration))
                                                                                                                        || sRule.Day != null && sRule.Day == (int)scheduleDate.DayOfWeek
                                                                                                                        || sRule.Day != null && sRule.Day - 1000 == scheduleDate.Day
                                                                                                                        || sRule.Day != null && sRule.Day == -1
                                                                                                                        )
                                                                                                                    ).ToList();
                if (facilityScheduleRulesDTOList != null && facilityScheduleRulesDTOList.Any()) //Foiund applicable rule info. User first entry
                {
                    // if multiple rules are present, select the date based rule first
                    if (facilityScheduleRulesDTOList.Count > 1)
                    {
                        // Set null as Max walue and then order by desc to get following list
                        // Date Rule (null)
                        // Day Rule (0 to 6)
                        // Every Day Rule (-1)
                        facilityScheduleRulesDTOList = facilityScheduleRulesDTOList.OrderByDescending(x => x.Day ?? int.MaxValue).ToList();
                    }

                    scheduleDetailsDTO = new ScheduleDetailsDTO(facDTO.FacilityMapId, facDTO.FacilityMapName, this.scheduleDTO.MasterScheduleId, masterScheduleBL.MasterScheduleDTO.MasterScheduleName,
                                                                this.scheduleDTO.ScheduleId, this.scheduleDTO.ScheduleName, scheduleFromDate, scheduleToDate, this.scheduleDTO.ScheduleTime,
                                                                this.scheduleDTO.ScheduleToTime, this.scheduleDTO.FixedSchedule, this.scheduleDTO.AttractionPlayId,
                                                                this.scheduleDTO.AttractionPlayName, -1, "", null, facDTO.FacilityCapacity, facilityScheduleRulesDTOList[0].Units, (facilityScheduleRulesDTOList[0].Units != null ? (int)facilityScheduleRulesDTOList[0].Units : 0),
                                                                null, facilityScheduleRulesDTOList[0].Units, null, this.scheduleDTO.AttractionPlayExpiryDate, -1, -1, null, facilityScheduleRulesDTOList[0].SiteId, this.scheduleDTO.AttractionPlayPrice, facDTO);
                    log.LogMethodExit(scheduleDetailsDTO);
                    return scheduleDetailsDTO;
                }
            }
            //No rules for the facility 

            scheduleDetailsDTO = new ScheduleDetailsDTO(facDTO.FacilityMapId, facDTO.FacilityMapName, this.scheduleDTO.MasterScheduleId, masterScheduleBL.MasterScheduleDTO.MasterScheduleName,
                                                        this.scheduleDTO.ScheduleId, this.scheduleDTO.ScheduleName, scheduleFromDate, scheduleToDate, this.scheduleDTO.ScheduleTime,
                                                        this.scheduleDTO.ScheduleToTime, this.scheduleDTO.FixedSchedule, this.scheduleDTO.AttractionPlayId,
                                                        this.scheduleDTO.AttractionPlayName, -1, "", null, facDTO.FacilityCapacity, null, (facDTO.FacilityCapacity != null ? (int)facDTO.FacilityCapacity : 0),
                                                        null, facDTO.FacilityCapacity, null, this.scheduleDTO.AttractionPlayExpiryDate, -1, -1, null, this.scheduleDTO.SiteId, this.scheduleDTO.AttractionPlayPrice, facDTO);


            log.LogMethodExit(scheduleDetailsDTO);
            return scheduleDetailsDTO;
        }


    }

    /// <summary>
    /// Manages the list of Schedules
    /// </summary>
    public class SchedulesListBL
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<SchedulesDTO> schedulesDTOList;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public SchedulesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.schedulesDTOList = new List<SchedulesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="schedulesDTOList"></param>
        public SchedulesListBL(ExecutionContext executionContext, List<SchedulesDTO> schedulesDTOList)
        {
            log.LogMethodEntry(executionContext, schedulesDTOList);
            this.executionContext = executionContext;
            this.schedulesDTOList = schedulesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetScheduleDTOList
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns>List<ScheduleDTO></returns>
        public List<SchedulesDTO> GetScheduleDTOList(List<KeyValuePair<SchedulesDTO.SearchByParameters, string>> searchParameters,
                                           bool buildChildDetails = false, int facilityMapId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, buildChildDetails, sqlTransaction);
            SchedulesDataHandler scheduleDataHandler = new SchedulesDataHandler(sqlTransaction);
            List<SchedulesDTO> scheduleDTOList = scheduleDataHandler.GetScheduleDTOList(searchParameters);
            if (buildChildDetails && scheduleDTOList != null && scheduleDTOList.Count > 0)
            {
                List<int> masterScheduleIdList = scheduleDTOList.Select(sch => sch.MasterScheduleId).Distinct().ToList();
                if (masterScheduleIdList != null && masterScheduleIdList.Count > 0)
                {
                    foreach (int masterScheduleId in masterScheduleIdList)
                    {
                        ScheduleRulesList scheduleRulesList = new ScheduleRulesList(executionContext);
                        List<KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>> srSearchParams = new List<KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>>();
                        srSearchParams.Add(new KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>(ScheduleRulesDTO.SearchByParameters.MASTER_SCHEDULE_ID, masterScheduleId.ToString()));
                        srSearchParams.Add(new KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>(ScheduleRulesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        if (facilityMapId > -1)
                        {
                            srSearchParams.Add(new KeyValuePair<ScheduleRulesDTO.SearchByParameters, string>(ScheduleRulesDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                        }
                        List<ScheduleRulesDTO> scheduleRulesDTOList = scheduleRulesList.GetScheduleRulesDTOList(srSearchParams);
                        if (scheduleRulesDTOList != null && scheduleRulesDTOList.Count > 0)
                        {
                            foreach (SchedulesDTO scheduleDTO in scheduleDTOList)
                            {
                                if (scheduleDTO.MasterScheduleId == masterScheduleId)
                                {
                                    foreach (ScheduleRulesDTO scheduleRulesDTO in scheduleRulesDTOList)
                                    {
                                        if (scheduleRulesDTO.ScheduleId == scheduleDTO.ScheduleId)
                                        {
                                            if (scheduleDTO.ScheduleRulesDTOList == null)
                                            {
                                                scheduleDTO.ScheduleRulesDTOList = new List<ScheduleRulesDTO>();
                                            }
                                            scheduleDTO.ScheduleRulesDTOList.Add(scheduleRulesDTO);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit(scheduleDTOList);
            return scheduleDTOList;
        }

        /// <summary>
        /// Saves the schedulesDTOList
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (schedulesDTOList == null ||
                schedulesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < schedulesDTOList.Count; i++)
            {
                var schedulesDTO = schedulesDTOList[i];
                try
                {
                    SchedulesBL schedulesBL = new SchedulesBL(executionContext, schedulesDTO);
                    schedulesBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving schedulesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("schedulesDTO", schedulesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        ///// <summary>
        ///// GetAttractionSchedule
        ///// </summary>
        ///// <param name="bookingproductId"></param>
        ///// <param name="reservationDate"></param>
        ///// <param name="facilityId"></param>
        ///// <returns>List<scheduleDTO></returns>
        //public List<SchedulesDTO> GetScheduleDTOList(int bookingproductId, DateTime reservationDate, int facilityId)
        //{
        //    log.LogMethodEntry(bookingproductId, reservationDate, facilityId);
        //    SchedulesDataHandler schedulesDataHandler = new SchedulesDataHandler(null);
        //    List<SchedulesDTO> scheduleDTOList = schedulesDataHandler.GetAttractionSchedule(bookingproductId, reservationDate, facilityId);
        //    log.LogMethodExit(scheduleDTOList);
        //    return scheduleDTOList;
        //}

        ///// <summary>
        ///// GetBookingScheduleList
        ///// </summary>
        ///// <param name="sqlSearchParams"></param>
        ///// <returns></returns>
        //public DataTable GetBookingScheduleList(List<SqlParameter> sqlSearchParams)
        //{
        //    log.LogMethodEntry(sqlSearchParams);
        //    SchedulesDataHandler schedulesDataHandler = new SchedulesDataHandler(null);
        //    DataTable scheduleDTOList = schedulesDataHandler.GetBookingScheduleList(sqlSearchParams);
        //    log.LogMethodExit(scheduleDTOList);
        //    return scheduleDTOList;
        //}

    }

}
