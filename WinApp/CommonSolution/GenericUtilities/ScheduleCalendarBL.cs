/********************************************************************************************
 * Project Name - Schedule
 * Description  - Bussiness logic of schedule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        12-Jan-2016   Raghuveera          Created 
 *1.00        28-Apr-2017   Lakshminarayana     Modified 
 *2.60        08-Jan-2019   Jagan Mohana Rao    Added new constructor GetAllScheduleList(),SaveSchedule() 
 *2.70        08-Mar-2019   Guru S A            Renamed Schedule as ScheduleCalendarBL
 *2.70        03-May-2019   Mehraj              Added SaveSchedule() method 
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Log method entries/exits, Save method.
 *2.90        07-Aug-2020      Mushahid Faizan     Modified : Constructor, Save() method, Added Validate, Build() to get child records and 
 *                                                 List class changes as per 3 tier standards.
*2.150.0      03-May-2021      Abhishek             Modified : added GetScheduleCalendarDTO property,fetch data using Idlist  
*2.130.4     02-Mar-2022    Abhishek           WMS Fix : Added GetAllSchedules() To display the all signage schdeules                                                
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Schedule creates and modifies the schedule details 
    /// </summary>
    public class ScheduleCalendarBL
    {
        private ScheduleCalendarDTO scheduleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// This class is inherited by other class , so not made as private
        public ScheduleCalendarBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ScheduleCalendarBL id as the parameter
        /// Would fetch the scheduleDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public ScheduleCalendarBL(ExecutionContext executionContext, int id, bool loadChildRecords = false,
                                  bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ScheduleCalendarDataHandler scheduleCalendarDataHandler = new ScheduleCalendarDataHandler(executionContext,sqlTransaction);
            scheduleDTO = scheduleCalendarDataHandler.GetScheduleCalendarDTO(id);
            scheduleDTO = SiteContainerList.FromSiteDateTime(executionContext, scheduleDTO, "", "Siteid");
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Generate scheduleDTO list
        /// </summary>
        /// <param name="activeChildRecords">Bool for active only records</param>
        /// <param name="sqlTransaction">sql transaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            JobScheduleListBL jobScheduleListBL = new JobScheduleListBL(executionContext);
            List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>> jobScheduleSearchParams = new List<KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>>();
            if (activeChildRecords)
            {
                jobScheduleSearchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.IS_ACTIVE, "1"));
            }
            jobScheduleSearchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            jobScheduleSearchParams.Add(new KeyValuePair<JobScheduleDTO.SearchByJobScheduleDTOParameters, string>(JobScheduleDTO.SearchByJobScheduleDTOParameters.SCHEDULE_ID, scheduleDTO.ScheduleId.ToString()));
            scheduleDTO.JobScheduleDTOList = jobScheduleListBL.GetAllJobScheduleDTOList(jobScheduleSearchParams, true, true);

            List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>> scheduleCalendarExclusionSearchParameters = new List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>>();
            scheduleCalendarExclusionSearchParameters.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            scheduleCalendarExclusionSearchParameters.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SCHEDULE_ID, scheduleDTO.ScheduleId.ToString()));
            if (activeChildRecords)
            {
                scheduleCalendarExclusionSearchParameters.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.IS_ACTIVE, "1"));
            }
            ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executionContext);
            scheduleDTO.ScheduleCalendarExclusionDTOList = scheduleExclusionList.GetAllScheduleExclusions(scheduleCalendarExclusionSearchParameters);

            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="scheduleDTO"></param>
        public ScheduleCalendarBL(ExecutionContext executionContext, ScheduleCalendarDTO scheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(scheduleDTO, executionContext);
            this.scheduleDTO = scheduleDTO;
            if (scheduleDTO.ScheduleId < 0)
            {
                ValidateScheduleName(scheduleDTO.ScheduleName);
                ValidateScheduleConstraints();
            }
            log.LogMethodExit();
        }

        public void Update(ScheduleCalendarDTO parameterScheduleCalendarDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parameterScheduleCalendarDTO, sqlTransaction);
            ChangeScheduleName(parameterScheduleCalendarDTO.ScheduleName);
            ChangeScheduleTime(parameterScheduleCalendarDTO.ScheduleTime);
            ChangeScheduleEndDate(parameterScheduleCalendarDTO.ScheduleEndDate);
            ChangeRecurEndDate(parameterScheduleCalendarDTO.RecurEndDate);
            ChangeIsActive(parameterScheduleCalendarDTO.IsActive);
            ChangeRecurFrequency(parameterScheduleCalendarDTO.RecurFrequency);
            ChangeRecurFlag(parameterScheduleCalendarDTO.RecurFlag);
            ChangeRecurType(parameterScheduleCalendarDTO.RecurType);
            ChangeIsValidateRequired(parameterScheduleCalendarDTO.IsValidateRequired);

            Dictionary<int, JobScheduleDTO> jobScheduleDTOListDictionary = new Dictionary<int, JobScheduleDTO>();
            if (scheduleDTO.JobScheduleDTOList != null &&
                scheduleDTO.JobScheduleDTOList.Any())
            {
                foreach (var jobScheduleDTO in scheduleDTO.JobScheduleDTOList)
                {
                    jobScheduleDTOListDictionary.Add(jobScheduleDTO.JobScheduleId, jobScheduleDTO);
                }
            }
            if (parameterScheduleCalendarDTO.JobScheduleDTOList != null &&
                parameterScheduleCalendarDTO.JobScheduleDTOList.Any())
            {
                foreach (var parameterJobScheduleDTO in parameterScheduleCalendarDTO.JobScheduleDTOList)
                {
                    if (jobScheduleDTOListDictionary.ContainsKey(parameterJobScheduleDTO.JobScheduleId))
                    {
                        JobScheduleBL jobScheduleBL = new JobScheduleBL(executionContext, jobScheduleDTOListDictionary[parameterJobScheduleDTO.JobScheduleId]);
                        jobScheduleBL = new JobScheduleBL(executionContext, parameterJobScheduleDTO);
                    }
                    else if (parameterJobScheduleDTO.JobScheduleId > -1)
                    {
                        JobScheduleBL jobScheduleBL = new JobScheduleBL(executionContext, parameterJobScheduleDTO.JobScheduleId, sqlTransaction);
                        if (scheduleDTO.JobScheduleDTOList == null)
                        {
                            scheduleDTO.JobScheduleDTOList = new List<JobScheduleDTO>();
                        }
                        scheduleDTO.JobScheduleDTOList.Add(jobScheduleBL.JobScheduleDTO);
                        jobScheduleBL = new JobScheduleBL(executionContext, parameterJobScheduleDTO);
                    }
                    else
                    {
                        JobScheduleBL jobScheduleListBL = new JobScheduleBL(executionContext, parameterJobScheduleDTO);
                        if (scheduleDTO.JobScheduleDTOList == null)
                        {
                            scheduleDTO.JobScheduleDTOList = new List<JobScheduleDTO>();
                        }
                        scheduleDTO.JobScheduleDTOList.Add(jobScheduleListBL.JobScheduleDTO);
                    }
                }
            }

            Dictionary<int, ScheduleCalendarExclusionDTO> scheduleCalendarExclusionDTOListDictionary = new Dictionary<int, ScheduleCalendarExclusionDTO>();
            if (scheduleDTO.ScheduleCalendarExclusionDTOList != null &&
                scheduleDTO.ScheduleCalendarExclusionDTOList.Any())
            {
                foreach (var scheduleCalendarExclusionDTO in scheduleDTO.ScheduleCalendarExclusionDTOList)
                {
                    scheduleCalendarExclusionDTOListDictionary.Add(scheduleCalendarExclusionDTO.ScheduleExclusionId, scheduleCalendarExclusionDTO);
                }
            }
            if (parameterScheduleCalendarDTO.ScheduleCalendarExclusionDTOList != null &&
                parameterScheduleCalendarDTO.ScheduleCalendarExclusionDTOList.Any())
            {
                foreach (var parameterScheduleCalendarExclusionDTO in parameterScheduleCalendarDTO.ScheduleCalendarExclusionDTOList)
                {
                    if (scheduleCalendarExclusionDTOListDictionary.ContainsKey(parameterScheduleCalendarExclusionDTO.ScheduleExclusionId))
                    {
                        ScheduleCalendarExclusionBL scheduleCalendarExclusionBL = new ScheduleCalendarExclusionBL(executionContext, scheduleCalendarExclusionDTOListDictionary[parameterScheduleCalendarExclusionDTO.ScheduleExclusionId]);
                        scheduleCalendarExclusionBL = new ScheduleCalendarExclusionBL(executionContext, parameterScheduleCalendarExclusionDTO);
                    }
                    else if (parameterScheduleCalendarExclusionDTO.ScheduleExclusionId > -1)
                    {
                        ScheduleCalendarExclusionBL ScheduleCalendarExclusionBL = new ScheduleCalendarExclusionBL(executionContext, parameterScheduleCalendarExclusionDTO.ScheduleExclusionId, sqlTransaction);
                        if (scheduleDTO.ScheduleCalendarExclusionDTOList == null)
                        {
                            scheduleDTO.ScheduleCalendarExclusionDTOList = new List<ScheduleCalendarExclusionDTO>();
                        }
                        scheduleDTO.ScheduleCalendarExclusionDTOList.Add(ScheduleCalendarExclusionBL.ScheduleCalendarExclusionDTO);
                        ScheduleCalendarExclusionBL = new ScheduleCalendarExclusionBL(executionContext, parameterScheduleCalendarExclusionDTO);
                    }
                    else
                    {
                        ScheduleCalendarExclusionBL ScheduleCalendarExclusionListBL = new ScheduleCalendarExclusionBL(executionContext, parameterScheduleCalendarExclusionDTO);
                        if (scheduleDTO.ScheduleCalendarExclusionDTOList == null)
                        {
                            scheduleDTO.ScheduleCalendarExclusionDTOList = new List<ScheduleCalendarExclusionDTO>();
                        }
                        scheduleDTO.ScheduleCalendarExclusionDTOList.Add(ScheduleCalendarExclusionListBL.ScheduleCalendarExclusionDTO);
                    }
                }
            }
            ValidateScheduleConstraints();
            log.LogMethodExit();

        }

        private void ValidateScheduleName(string name)
        {
            log.LogMethodEntry(name);
            if (string.IsNullOrWhiteSpace(name))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Name"));
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("ScheduleName is empty.", "ScheduleCalendar", "ScheduleName", errorMessage);
            }
            if (name.Length > 100)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2197, MessageContainerList.GetMessage(executionContext, "Name"), 100);
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("ScheduleName greater than 100 characters.", "ScheduleCalendar", "ScheduleName", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateScheduleConstraints()
        {
            log.LogMethodEntry();
            //if (scheduleDTO.ScheduleTime.Date > scheduleDTO.ScheduleEndDate.Date)
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 571);
            //    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
            //    throw new ValidationException("End Time should be greater than Start Time.", "ScheduleCalendar", "ScheduleEndDate", errorMessage);
            //}
            if (string.Equals(scheduleDTO.RecurFlag, "Y"))
            {
                if (scheduleDTO.ScheduleTime >= scheduleDTO.RecurEndDate)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 606);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("Invalid Recurrence Pattern. Check the Recur End Date.", "ScheduleCalendar", "RecurEndDate", errorMessage);
                }
                decimal scheduleTime = new decimal(scheduleDTO.ScheduleTime.Hour + (((double)scheduleDTO.ScheduleTime.Minute) / 100));
                decimal scheduleEndTime = new decimal(scheduleDTO.ScheduleEndDate.Hour + (((double)scheduleDTO.ScheduleEndDate.Minute) / 100));
                if (scheduleTime >= scheduleEndTime)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 571);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new ValidationException("End Time should be greater than Start Time.", "ScheduleCalendar", "ScheduleTime", errorMessage);
                }
            }
            else
            {
                if (scheduleDTO.ScheduleTime.Date == scheduleDTO.ScheduleEndDate.Date)
                {
                    if (scheduleDTO.ScheduleTime > scheduleDTO.ScheduleEndDate)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 571);
                        log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                        throw new ValidationException("End Time should be greater than Start Time.", "ScheduleCalendar", "ScheduleTime", errorMessage);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void ChangeIsValidateRequired(bool isValidateRequired)
        {
            log.LogMethodEntry(isValidateRequired);
            if (scheduleDTO.IsValidateRequired == isValidateRequired)
            {
                log.LogMethodExit(null, "No changes to isValidateRequired");
                return;
            }
            scheduleDTO.IsValidateRequired = isValidateRequired;
            log.LogMethodExit();
        }

        private void ChangeRecurType(string recurType)
        {
            log.LogMethodEntry(recurType);
            if (scheduleDTO.RecurType == recurType)
            {
                log.LogMethodExit(null, "No changes to recurType");
                return;
            }
            scheduleDTO.RecurType = recurType;
            log.LogMethodExit();
        }

        private void ChangeRecurFlag(string recurFlag)
        {
            log.LogMethodEntry(recurFlag);
            if (scheduleDTO.RecurFlag == recurFlag)
            {
                log.LogMethodExit(null, "No changes to RecurFlag");
                return;
            }
            scheduleDTO.RecurFlag = recurFlag;
            log.LogMethodExit();
        }

        private void ChangeRecurFrequency(string recurFrequency)
        {
            log.LogMethodEntry(recurFrequency);
            if (scheduleDTO.RecurFrequency == recurFrequency)
            {
                log.LogMethodExit(null, "No changes to recurFrequency");
                return;
            }
            scheduleDTO.RecurFrequency = recurFrequency;
            log.LogMethodExit();
        }

        private void ChangeRecurEndDate(DateTime recurEndDate)
        {
            log.LogMethodEntry(recurEndDate);
            if (scheduleDTO.RecurEndDate == recurEndDate)
            {
                log.LogMethodExit(null, "No changes to RecurEndDate");
                return;
            }
            scheduleDTO.RecurEndDate = recurEndDate;
            log.LogMethodExit();
        }

        private void ChangeScheduleTime(DateTime scheduleTime)
        {
            log.LogMethodEntry(scheduleTime);
            if (scheduleDTO.ScheduleTime == scheduleTime)
            {
                log.LogMethodExit(null, "No changes to ScheduleTime");
                return;
            }
            scheduleDTO.ScheduleTime = scheduleTime;
            log.LogMethodExit();
        }

        private void ChangeScheduleName(string scheduleName)
        {
            log.LogMethodEntry(scheduleName);
            if (scheduleDTO.ScheduleName == scheduleName)
            {
                log.LogMethodExit(null, "No changes to scheduleName");
                return;
            }
            ValidateScheduleName(scheduleName);
            scheduleDTO.ScheduleName = scheduleName;
            log.LogMethodExit();
        }

        public void ChangeIsActive(bool isActive)
        {
            log.LogMethodEntry(isActive);
            if (scheduleDTO.IsActive == isActive)
            {
                log.LogMethodExit(null, "No changes to user isActive");
                return;
            }
            scheduleDTO.IsActive = isActive;
            log.LogMethodExit();
        }

        public void ChangeScheduleEndDate(DateTime scheduleEndDate)
        {
            log.LogMethodEntry(scheduleEndDate);
            if (scheduleDTO.ScheduleEndDate == scheduleEndDate)
            {
                log.LogMethodExit(null, "No changes to scheduleEndDate");
                return;
            }
            scheduleDTO.ScheduleEndDate = scheduleEndDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ScheduleCalendarDTO ScheduleCalendarDTO
        {
            get { return scheduleDTO; }
        }

        /// <summary>
        /// Saves the Schedule record
        /// Checks if the schedule id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public virtual void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            DateTime currentTime = SiteContainerList.CurrentDateTime(executionContext);
            ScheduleCalendarDataHandler scheduleDataHandler = new ScheduleCalendarDataHandler(executionContext,sqlTransaction);
            if (scheduleDTO.IsValidateRequired)
            {
                IsRelevantValidation(currentTime);
            }
            if (scheduleDTO.ScheduleId < 0)
            {
                SiteContainerList.ToSiteDateTime(executionContext, scheduleDTO);
                scheduleDTO = scheduleDataHandler.Insert(scheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                SiteContainerList.FromSiteDateTime(executionContext, scheduleDTO, "", "Siteid");
                scheduleDTO.AcceptChanges();
            }
            else
            {
                if (scheduleDTO.IsChanged)
                {
                    SiteContainerList.ToSiteDateTime(executionContext, scheduleDTO);
                    scheduleDTO = scheduleDataHandler.Update(scheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    SiteContainerList.FromSiteDateTime(executionContext, scheduleDTO, "", "Siteid");
                    scheduleDTO.AcceptChanges();
                }
            }
            SaveChild(sqlTransaction);
            scheduleDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveChild(SqlTransaction sqlTransaction)
        {
            if (scheduleDTO.JobScheduleDTOList != null &&
                scheduleDTO.JobScheduleDTOList.Any())
            {
                List<JobScheduleDTO> updatedJobScheduleDTOList = new List<JobScheduleDTO>();
                foreach (var jobScheduleDTO in scheduleDTO.JobScheduleDTOList)
                {
                    if (jobScheduleDTO.ScheduleId != scheduleDTO.ScheduleId)
                    {
                        jobScheduleDTO.ScheduleId = scheduleDTO.ScheduleId;
                    }
                    if (jobScheduleDTO.IsChanged)
                    {
                        updatedJobScheduleDTOList.Add(jobScheduleDTO);
                    }
                }
                if (updatedJobScheduleDTOList.Any())
                {
                    JobScheduleListBL jobScheduleListBL = new JobScheduleListBL(executionContext, updatedJobScheduleDTOList);
                    jobScheduleListBL.Save(sqlTransaction);
                }
            }

            if (scheduleDTO.ScheduleCalendarExclusionDTOList != null &&
                scheduleDTO.ScheduleCalendarExclusionDTOList.Any())
            {
                List<ScheduleCalendarExclusionDTO> updatedScheduleCalendarExclusionDTOList = new List<ScheduleCalendarExclusionDTO>();
                foreach (var scheduleCalendarExclusionDTO in scheduleDTO.ScheduleCalendarExclusionDTOList)
                {
                    if (scheduleCalendarExclusionDTO.ScheduleId != scheduleDTO.ScheduleId)
                    {
                        scheduleCalendarExclusionDTO.ScheduleId = scheduleDTO.ScheduleId;
                    }
                    if (scheduleCalendarExclusionDTO.IsChanged)
                    {
                        updatedScheduleCalendarExclusionDTOList.Add(scheduleCalendarExclusionDTO);
                    }
                }
                if (updatedScheduleCalendarExclusionDTOList.Any())
                {
                    ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executionContext, updatedScheduleCalendarExclusionDTOList);
                    scheduleExclusionList.Save(sqlTransaction);
                }
            }
        }

        /// <summary>
        /// Validate the scheduleDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        /// <summary>
        /// Returns the daywise record 
        /// </summary>
        /// <param name="dtFrom">From Date Parameter</param>
        /// <param name="dtToDate">To date parameter</param>
        /// <param name="siteId">siteId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ScheduleCalendarDTO> GetScheduleDayList(DateTime dtFrom, DateTime dtToDate, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(dtFrom, dtToDate, siteId);
            ScheduleCalendarDataHandler scheduleDataHandler = new ScheduleCalendarDataHandler(executionContext,sqlTransaction);
            List<ScheduleCalendarDTO> scheduleDTO = scheduleDataHandler.GetScheduleDayWeekList(dtFrom, dtToDate, siteId);
            scheduleDTO = SiteContainerList.FromSiteDateTime(executionContext, scheduleDTO, "", "Siteid");
            log.LogMethodExit(scheduleDTO);
            return scheduleDTO;
        }

        /// <summary>
        /// Returns whether the schedule is relevent at the specified date and time.
        /// </summary>
        /// <param name="dateTime">date</param>
        /// <returns></returns>
        public bool IsRelevant(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            ScheduleCalendarCalculator scheduleCalendarCalculator = new ScheduleCalendarCalculator(scheduleDTO);
            bool result = scheduleCalendarCalculator.IsRelevant(dateTime);
            log.LogMethodExit(result);
            return result;
        }

        private bool CheckRecurFrequency(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            bool valid = false;
            switch (scheduleDTO.RecurFrequency)
            {
                case "D":
                    {
                        valid = true;
                        break;
                    }
                case "W":
                    {
                        if (scheduleDTO.ScheduleTime.DayOfWeek <= scheduleDTO.ScheduleEndDate.DayOfWeek)
                        {
                            if (scheduleDTO.ScheduleTime.DayOfWeek <= dateTime.DayOfWeek && scheduleDTO.ScheduleEndDate.DayOfWeek >= dateTime.DayOfWeek)
                            {
                                valid = true;
                            }
                        }
                        else
                        {
                            if (scheduleDTO.ScheduleTime.DayOfWeek <= dateTime.DayOfWeek || scheduleDTO.ScheduleEndDate.DayOfWeek >= dateTime.DayOfWeek)
                            {
                                valid = true;
                            }
                        }

                        break;
                    }
                case "M":
                    {
                        if (string.Equals(scheduleDTO.RecurType, "W"))
                        {
                            GregorianCalendar gregorianCalendar = new GregorianCalendar();
                            int scheduleWeekNo = gregorianCalendar.GetWeekOfYear(scheduleDTO.ScheduleTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) - gregorianCalendar.GetWeekOfYear(new DateTime(scheduleDTO.ScheduleTime.Year, scheduleDTO.ScheduleTime.Month, 1), CalendarWeekRule.FirstDay, DayOfWeek.Sunday) + 1;
                            if (new DateTime(scheduleDTO.ScheduleTime.Year, scheduleDTO.ScheduleTime.Month, 1).DayOfWeek > scheduleDTO.ScheduleTime.DayOfWeek)
                            {
                                scheduleWeekNo--;
                            }
                            int dateTimeWeekNo = gregorianCalendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) - gregorianCalendar.GetWeekOfYear(new DateTime(dateTime.Year, dateTime.Month, 1), CalendarWeekRule.FirstDay, DayOfWeek.Sunday) + 1;
                            if (new DateTime(dateTime.Year, dateTime.Month, 1).DayOfWeek > dateTime.DayOfWeek)
                            {
                                dateTimeWeekNo--;
                            }
                            if (scheduleWeekNo == dateTimeWeekNo && scheduleDTO.ScheduleTime.DayOfWeek == dateTime.DayOfWeek)
                            {
                                valid = true;
                            }
                        }
                        else
                        {
                            if (scheduleDTO.ScheduleTime.Day == dateTime.Day)
                            {
                                valid = true;
                            }
                        }
                        break;
                    }
            }
            log.LogMethodExit(valid);
            return valid;
        }

        private string CheckInclusionExclusion(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            string returnValue = "";
            List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOList = null;
            ScheduleCalendarExclusionListBL scheduleExclusionListBL = new ScheduleCalendarExclusionListBL(executionContext);
            List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>> searchParameter = new List<KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>>();
            searchParameter.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.SCHEDULE_ID, scheduleDTO.ScheduleId.ToString()));
            searchParameter.Add(new KeyValuePair<ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters, string>(ScheduleCalendarExclusionDTO.SearchByScheduleCalendarExclusionParameters.IS_ACTIVE, "1"));
            scheduleExclusionDTOList = scheduleExclusionListBL.GetAllScheduleExclusions(searchParameter);
            if (scheduleExclusionDTOList != null)
            {
                foreach (ScheduleCalendarExclusionDTO scheduleExclusionDTO in scheduleExclusionDTOList)
                {
                    if (scheduleExclusionDTO.Day > 0)
                    {
                        if ((int)dateTime.DayOfWeek == (scheduleExclusionDTO.Day - 1))
                        {
                            returnValue = (scheduleExclusionDTO.IncludeDate == true ? "Y" : "N");
                        }
                    }
                }
                foreach (ScheduleCalendarExclusionDTO scheduleExclusionDTO in scheduleExclusionDTOList)
                {
                    if (!string.IsNullOrWhiteSpace(scheduleExclusionDTO.ExclusionDate))
                    {
                        DateTime exclusionDate = Convert.ToDateTime(scheduleExclusionDTO.ExclusionDate);
                        if (DateTime.Compare(new DateTime(exclusionDate.Year, exclusionDate.Month, exclusionDate.Day), new DateTime(dateTime.Year, dateTime.Month, dateTime.Day)) == 0)
                        {
                            returnValue = (scheduleExclusionDTO.IncludeDate == true ? "Y" : "N");
                        }
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Save the Schedule based on validation and retrun the scheduleId
        /// </summary>
        /// <returns></returns>
        //public int SaveSchedule(bool isValidateRequired = true)
        //{
        //    int scheduleId = 0;
        //    LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
        //    DateTime currentTime = serverTimeObject.GetServerDateTime();
        //    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
        //    {
        //        parafaitDBTrx.BeginTransaction();
        //        try
        //        {
        //            if (isValidateRequired)
        //            {
        //                IsRelevantValidation(currentTime);
        //            }
        //            Save(parafaitDBTrx.SQLTrx);
        //            parafaitDBTrx.EndTransaction();
        //            scheduleId = scheduleDTO.ScheduleId;
        //        }
        //        catch (Exception ex)
        //        {
        //            log.Error(ex);
        //            parafaitDBTrx.RollBack();
        //            log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
        //            throw;
        //        }
        //    }
        //    log.LogMethodExit(scheduleId);
        //    return scheduleId;
        //}

        /// <summary>
        /// Returns whether the schedule is relevent at the specified date and time.
        /// </summary>
        /// <param name="dateTime">date</param>
        /// <returns></returns>
        public void IsRelevantValidation(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            bool recurFrequencyCheckPassed = true;
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (scheduleDTO == null)
            {
                log.Info("scheduleDTO == null");
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "ScheduleDTO"), MessageContainerList.GetMessage(executionContext, 1800));//"Schedule details are not set"
                validationErrorList.Add(validationError);
            }
            if (DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleTime.Date) > 0)
            {
                log.Info("DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleTime.Date) > 0");
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "Date"), MessageContainerList.GetMessage(executionContext, 1801));//"Date is before with schedule date"
                validationErrorList.Add(validationError);
            }
            decimal scheduleTime = new decimal(scheduleDTO.ScheduleTime.Hour + (((double)scheduleDTO.ScheduleTime.Minute) / 100));
            decimal scheduleEndTime = new decimal(scheduleDTO.ScheduleEndDate.Hour + (((double)scheduleDTO.ScheduleEndDate.Minute) / 100));
            decimal currentTime = new decimal(dateTime.Hour + (((double)dateTime.Minute) / 100));
            if (scheduleDTO.ScheduleId > -1 && scheduleTime < currentTime && DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleTime.Date) == 0)
            {
                scheduleDTO.ScheduleTime = dateTime;
            }
            if (scheduleTime < currentTime && DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleEndDate.Date) == 0)
            {
                log.Info("scheduleTime < currentTime");
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "ScheduleTime"), MessageContainerList.GetMessage(executionContext, 1803));//"Schedule start time is greater than date time"
                validationErrorList.Add(validationError);
            }
            if (scheduleEndTime <= currentTime && DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleEndDate.Date) == 0)
            {
                log.Info("scheduleEndTime <= currentTime");
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "ScheduleTime"), MessageContainerList.GetMessage(executionContext, 1804));// "Schedule end time is less than date time"
                validationErrorList.Add(validationError);
            }
            if (scheduleTime >= scheduleEndTime && DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleEndDate.Date) == 0)
            {
                log.Info("scheduleTime >= scheduleEndTime");
                ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "ScheduleTime"), MessageContainerList.GetMessage(executionContext, 571)); //"End Time should be greater than Start Time"
                validationErrorList.Add(validationError);
            }
            if (string.Equals(scheduleDTO.RecurFlag, "Y"))
            {
                if (DateTime.Compare(dateTime.Date, scheduleDTO.RecurEndDate.Date) > 0)
                {
                    log.Info("DateTime.Compare(dateTime.Date, scheduleDTO.RecurEndDate.Date) > 0");
                    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "Date"), MessageContainerList.GetMessage(executionContext, 1802));// "Date is greater than with recurring end date"
                    validationErrorList.Add(validationError);
                }
                recurFrequencyCheckPassed = CheckRecurFrequency(dateTime);
                if (recurFrequencyCheckPassed)
                {
                    if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
                    {
                        log.Info("string.Equals(CheckInclusionExclusion(dateTime), 'N')");
                        ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "Check Inclusion Exclusion"), MessageContainerList.GetMessage(executionContext, 1805));
                        validationErrorList.Add(validationError);
                    }
                }
                else
                {
                    if (string.Equals(CheckInclusionExclusion(dateTime), "Y") == false)
                    {
                        log.Info("string.Equals(CheckInclusionExclusion(dateTime), 'Y') == false");
                        ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "Check Inclusion Exclusion"), MessageContainerList.GetMessage(executionContext, 1805));//"Date fails schedule inclusion exclusion check"
                        validationErrorList.Add(validationError);
                    }
                }
            }
            else
            {
                if (DateTime.Compare(dateTime, scheduleDTO.ScheduleTime) > 0)
                {
                    log.Info("DateTime.Compare(dateTime, scheduleDTO.ScheduleTime) > 0");
                    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "Date"), MessageContainerList.GetMessage(executionContext, 1806)); //"Date time is before schedule time"
                    validationErrorList.Add(validationError);
                }
                if (DateTime.Compare(dateTime, scheduleDTO.ScheduleEndDate) >= 0)
                {
                    log.Info("DateTime.Compare(dateTime, scheduleDTO.ScheduleEndDate) >= 0");
                    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "Date"), MessageContainerList.GetMessage(executionContext, 1807));//"Date time is greater than schedule end time"
                    validationErrorList.Add(validationError);
                }
                if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
                {
                    log.Info("string.Equals(CheckInclusionExclusion(dateTime), 'N')");
                    ValidationError validationError = new ValidationError(MessageContainerList.GetMessage(executionContext, "Schedule"), MessageContainerList.GetMessage(executionContext, "Check Inclusion Exclusion"), MessageContainerList.GetMessage(executionContext, 1805)); //"Date fails schedule inclusion exclusion check"
                    validationErrorList.Add(validationError);
                }
            }

            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1808), validationErrorList);// "Datetime passed fails relavancy checks"
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ScheduleCalendarDTO GetScheduleCalendarDTO
        {
            get { return scheduleDTO; }
        }
    }

    /// <summary>
    /// Manages the list of Schedule
    /// </summary>
    public class ScheduleCalendarListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ScheduleCalendarDTO> scheduleCalendarDTOList;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ScheduleCalendarListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="scheduleCalendarDTOList">scheduleCalendarDTOList</param>
        public ScheduleCalendarListBL(ExecutionContext executionContext, List<ScheduleCalendarDTO> scheduleCalendarDTOList) 
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scheduleCalendarDTOList);
            this.scheduleCalendarDTOList = scheduleCalendarDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Schedule list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="loadActiveChildRecord">loadActiveChildRecord</param>
        /// <returns></returns>
        public List<ScheduleCalendarDTO> GetAllSchedule(List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters,
                                                        bool loadChildRecords = false, bool loadActiveChildRecord = false,
                                                        SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecord);
            ScheduleCalendarDataHandler scheduleDataHandler = new ScheduleCalendarDataHandler(executionContext,sqlTransaction);
            List<ScheduleCalendarDTO> scheduleDTOsList = scheduleDataHandler.GetScheduleCalendarDTOList(searchParameters);
            scheduleDTOsList = SiteContainerList.FromSiteDateTime(executionContext, scheduleDTOsList,"", "Siteid");
            if (scheduleDTOsList != null && scheduleDTOsList.Count != 0 && loadChildRecords)
            {
                Build(scheduleDTOsList, loadActiveChildRecord, sqlTransaction);
            }
            log.LogMethodExit(scheduleDTOsList);
            return scheduleDTOsList;
        }

        /// <summary>
        /// Returns the Schedule list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadChildRecords">loadChildRecords</param>
        /// <param name="fromDate">fromDate</param>
        /// <param name="loadActiveChildRecord">loadActiveChildRecord</param>
        /// <returns></returns>
        public List<ScheduleCalendarDTO> GetAllSchedules(List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters, DateTime? fromDate = null,
                                                        bool loadChildRecords = false, bool loadActiveChildRecord = false, 
                                                        SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecord);
            ScheduleCalendarDataHandler scheduleDataHandler = new ScheduleCalendarDataHandler(executionContext,sqlTransaction);
            List<ScheduleCalendarDTO> scheduleDTOsList = scheduleDataHandler.GetScheduleCalendarDTOList(searchParameters);
            scheduleDTOsList = SiteContainerList.FromSiteDateTime(executionContext, scheduleDTOsList, "", "Siteid");
            if (scheduleDTOsList != null && scheduleDTOsList.Count != 0 && loadChildRecords)
            {
                Build(scheduleDTOsList, loadActiveChildRecord, sqlTransaction);
            }
            List<ScheduleCalendarDTO> scheduleCalendarDTOList = new List<ScheduleCalendarDTO>();
            if (scheduleDTOsList != null && scheduleDTOsList.Any())
            {
                foreach (ScheduleCalendarDTO scheduleCalendarDTO in scheduleDTOsList)
                {
                    bool show = true;
                    if (string.IsNullOrEmpty(scheduleCalendarDTO.ScheduleName))
                    {
                        show = false;
                    }
                    if (scheduleCalendarDTO.IsActive && fromDate != null)
                    {
                        DateTime startDate = Convert.ToDateTime(fromDate.ToString());
                        if (startDate == DateTime.MinValue)
                        {
                            throw new ValidationException("Invalid date format, expected format is yyyy-mm-dd hh:mm:ss");
                        }

                        if (DateTime.Compare(new DateTime(scheduleCalendarDTO.ScheduleTime.Year, scheduleCalendarDTO.ScheduleTime.Month, scheduleCalendarDTO.ScheduleTime.Day), new DateTime(startDate.Year, startDate.Month, startDate.Day)) > 0)
                        {
                            show = false;
                        }
                        if (string.Equals(scheduleCalendarDTO.RecurFlag, "Y"))
                        {
                            if (DateTime.Compare(new DateTime(scheduleCalendarDTO.RecurEndDate.Year, scheduleCalendarDTO.RecurEndDate.Month, scheduleCalendarDTO.RecurEndDate.Day), new DateTime(startDate.Year, startDate.Month, startDate.Day)) < 0)
                            {
                                show = false;
                            }
                        }
                        else
                        {
                            if (scheduleCalendarDTO.ScheduleEndDate.Date != DateTime.MinValue)
                            {
                                if (DateTime.Compare(new DateTime(scheduleCalendarDTO.ScheduleEndDate.Year, scheduleCalendarDTO.ScheduleEndDate.Month, scheduleCalendarDTO.ScheduleEndDate.Day), new DateTime(startDate.Year, startDate.Month, startDate.Day)) < 0)
                                {
                                    show = false;
                                }
                            }
                        }
                    }
                    if (show)
                    {
                        scheduleCalendarDTOList.Add(scheduleCalendarDTO);
                    }
                }
            }
            log.LogMethodExit(scheduleCalendarDTOList);
            return scheduleCalendarDTOList;
        }

        private void Build(List<ScheduleCalendarDTO> scheduleCalendarDTOList, bool activeChildRecords, SqlTransaction sqlTransaction)
        {
            Dictionary<int, ScheduleCalendarDTO> scheduleCalendarDTODictionary = new Dictionary<int, ScheduleCalendarDTO>();
            List<int> scheduleCalendarIdList = new List<int>();
            for (int i = 0; i < scheduleCalendarDTOList.Count; i++)
            {
                if (scheduleCalendarDTODictionary.ContainsKey(scheduleCalendarDTOList[i].ScheduleId))
                {
                    continue;
                }
                scheduleCalendarDTODictionary.Add(scheduleCalendarDTOList[i].ScheduleId, scheduleCalendarDTOList[i]);
                scheduleCalendarIdList.Add(scheduleCalendarDTOList[i].ScheduleId);
            }

            // Child 1 : JobScheduleDTO
            JobScheduleListBL jobScheduleListBL = new JobScheduleListBL(executionContext);
            List<JobScheduleDTO> jobScheduleDTOListOnDisplay = jobScheduleListBL.GetAllJobScheduleDTOList(scheduleCalendarIdList, true, activeChildRecords, sqlTransaction);

            if (jobScheduleDTOListOnDisplay != null && jobScheduleDTOListOnDisplay.Any())
            {
                for (int i = 0; i < jobScheduleDTOListOnDisplay.Count; i++)
                {
                    if (scheduleCalendarDTODictionary.ContainsKey(jobScheduleDTOListOnDisplay[i].ScheduleId) == false)
                    {
                        continue;
                    }
                    ScheduleCalendarDTO scheduleCalendarDTO = scheduleCalendarDTODictionary[jobScheduleDTOListOnDisplay[i].ScheduleId];
                    if (scheduleCalendarDTO.JobScheduleDTOList == null)
                    {
                        scheduleCalendarDTO.JobScheduleDTOList = new List<JobScheduleDTO>();
                    }
                    scheduleCalendarDTO.JobScheduleDTOList.Add(jobScheduleDTOListOnDisplay[i]);
                }
            }

            // Child 2 : ScheduleCalendarExclusionDTO
            ScheduleCalendarExclusionListBL scheduleExclusionList = new ScheduleCalendarExclusionListBL(executionContext);
            List<ScheduleCalendarExclusionDTO> scheduleCalendarExclusionDTOList = scheduleExclusionList.GetScheduleCalendarExclusionDTOList(scheduleCalendarIdList, activeChildRecords, sqlTransaction);
            if (scheduleCalendarExclusionDTOList != null && scheduleCalendarExclusionDTOList.Any())
            {
                for (int i = 0; i < scheduleCalendarExclusionDTOList.Count; i++)
                {
                    if (scheduleCalendarDTODictionary.ContainsKey(scheduleCalendarExclusionDTOList[i].ScheduleId) == false)
                    {
                        continue;
                    }
                    ScheduleCalendarDTO scheduleCalendarDTO = scheduleCalendarDTODictionary[scheduleCalendarExclusionDTOList[i].ScheduleId];
                    if (scheduleCalendarDTO.ScheduleCalendarExclusionDTOList == null)
                    {
                        scheduleCalendarDTO.ScheduleCalendarExclusionDTOList = new List<ScheduleCalendarExclusionDTO>();
                    }
                    scheduleCalendarDTO.ScheduleCalendarExclusionDTOList.Add(scheduleCalendarExclusionDTOList[i]);
                }
            }
        }

        /// <summary>
        /// Returns the Schedule list based on filtering
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="scheduleIdList">scheduleIdList</param>
        /// <param name="isActive">isActive</param>
        /// <returns></returns>
        public List<ScheduleCalendarDTO> GetAllScheduleList(List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters, List<int> scheduleIdList, string isActive)
        {
            log.LogMethodEntry(searchParameters, scheduleIdList, isActive);
            try
            {
                ScheduleCalendarListBL scheduleListBL = new ScheduleCalendarListBL(executionContext);
                List<ScheduleCalendarDTO> scheduleDTOList = new List<ScheduleCalendarDTO>();
                List<ScheduleCalendarDTO> bindingSortingScheduleDTOList = new List<ScheduleCalendarDTO>();
                foreach (int scheduleId in scheduleIdList)
                {
                    List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchScheduleParameters = new List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>>();
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>(ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID, scheduleId.ToString()));
                    List<ScheduleCalendarDTO> list = scheduleListBL.GetAllSchedule(searchScheduleParameters);
                    if (list != null && list.Count > 0)
                    {
                        scheduleDTOList.Add(list[0]);
                    }
                }
                bool show = true;
                DateTime dtpSchedule = DateTime.Today;
                foreach (ScheduleCalendarDTO scheduleDTO in scheduleDTOList)
                {
                    show = true;
                    if (DateTime.Compare(new DateTime(scheduleDTO.ScheduleTime.Year, scheduleDTO.ScheduleTime.Month, scheduleDTO.ScheduleTime.Day), new DateTime(dtpSchedule.Year, dtpSchedule.Month, dtpSchedule.Day)) > 0)
                    {
                        show = false;
                    }
                    if (string.Equals(scheduleDTO.RecurFlag, "Y"))
                    {
                        if (DateTime.Compare(new DateTime(scheduleDTO.RecurEndDate.Year, scheduleDTO.RecurEndDate.Month, scheduleDTO.RecurEndDate.Day), new DateTime(dtpSchedule.Year, dtpSchedule.Month, dtpSchedule.Day)) < 0)
                        {
                            show = false;
                        }
                    }
                    else
                    {
                        if (scheduleDTO.ScheduleEndDate.Date != DateTime.MinValue)
                        {
                            if (DateTime.Compare(new DateTime(scheduleDTO.ScheduleEndDate.Year, scheduleDTO.ScheduleEndDate.Month, scheduleDTO.ScheduleEndDate.Day), new DateTime(dtpSchedule.Year, dtpSchedule.Month, dtpSchedule.Day)) < 0)
                            {
                                show = false;
                            }
                        }
                    }
                    if ((isActive.ToString() == "1" ? true : false) && !scheduleDTO.IsActive)
                    {
                        show = false;
                    }
                    if (show)
                    {
                        bindingSortingScheduleDTOList.Add(scheduleDTO);
                    }
                }
                log.LogMethodExit();
                return bindingSortingScheduleDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Gets the ScheduleCalendarDTO List for Schedule Id List
        /// </summary>
        /// <param name="scheduleIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ScheduleCalendarDTO</returns>
        public List<ScheduleCalendarDTO> GetScheduleDTOListOfSchedules(List<int> scheduleIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scheduleIdList);
            ScheduleCalendarDataHandler scheduleCalendarDataHandler = new ScheduleCalendarDataHandler(executionContext,sqlTransaction);
            List<ScheduleCalendarDTO> scheduleCalendarDTOList = scheduleCalendarDataHandler.GetScheduleDTOListOfSchedules(scheduleIdList, activeRecords);
            scheduleCalendarDTOList = SiteContainerList.FromSiteDateTime(scheduleCalendarDTOList, "", "Siteid");
            log.LogMethodExit(scheduleCalendarDTOList);
            return scheduleCalendarDTOList;
        }


        /// <summary>
        /// Save Schedules
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (scheduleCalendarDTOList == null ||
                scheduleCalendarDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < scheduleCalendarDTOList.Count; i++)
            {
                var scheduleCalendarDTO = scheduleCalendarDTOList[i];
                if (scheduleCalendarDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    ScheduleCalendarBL scheduleCalendarBL = new ScheduleCalendarBL(executionContext, scheduleCalendarDTO);
                    scheduleCalendarBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving scheduleCalendarDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("scheduleCalendarDTO", scheduleCalendarDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ScheduleCalendarDTO List for CampaignDefinitionList
        /// </summary>
        /// <param name="scheduleIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ScheduleCalendarDTO</returns>
        public List<ScheduleCalendarDTO> GetScheduleCalendarDTOList(List<int> scheduleIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(scheduleIdList, activeRecords, sqlTransaction);
            ScheduleCalendarDataHandler scheduleCalendarDataHandler = new ScheduleCalendarDataHandler(executionContext,sqlTransaction);
            List<ScheduleCalendarDTO> scheduleCalendarDTOList = scheduleCalendarDataHandler.GetScheduleCalendarDTOList(scheduleIdList, activeRecords);
            scheduleCalendarDTOList = SiteContainerList.FromSiteDateTime(executionContext, scheduleCalendarDTOList, "", "Siteid");
            if (scheduleCalendarDTOList != null && scheduleCalendarDTOList.Count != 0)
            {
                Build(scheduleCalendarDTOList, activeRecords, sqlTransaction);
            }
            log.LogMethodExit(scheduleCalendarDTOList);
            return scheduleCalendarDTOList;
        }
    }
}
