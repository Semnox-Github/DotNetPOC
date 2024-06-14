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
 *2.70.2        26-Jul-2019   Dakshakh raj        Modified : Log method entries/exits, Save method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Schedule creates and modifies the schedule details 
    /// </summary>
    public class Schedule
    {
        private ScheduleDTO scheduleDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public Schedule(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.scheduleDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="scheduleDTO">scheduleDTO</param>
        public Schedule(ExecutionContext executionContext, ScheduleDTO scheduleDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(scheduleDTO);
            this.scheduleDTO = scheduleDTO;
            log.LogMethodExit();
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
            ScheduleDataHandler scheduleDataHandler = new ScheduleDataHandler(sqlTransaction);
            if (scheduleDTO.ScheduleId < 0)
            {
                scheduleDTO = scheduleDataHandler.InsertSchedule(scheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                scheduleDTO.AcceptChanges();
            }
            else
            {
                if (scheduleDTO.IsChanged)
                {
                    scheduleDTO = scheduleDataHandler.UpdateSchedule(scheduleDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    scheduleDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the daywise record 
        /// </summary>
        /// <param name="dtFrom">From Date Parameter</param>
        /// <param name="dtToDate">To date parameter</param>
        /// <param name="siteId">siteId</param>
        /// <returns>daywise record </returns>
        public List<ScheduleDTO> GetScheduleDayList(DateTime dtFrom, DateTime dtToDate, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(dtFrom, dtToDate, siteId, sqlTransaction);
            ScheduleDataHandler scheduleDataHandler = new ScheduleDataHandler(sqlTransaction);
            List<ScheduleDTO> scheduleDTO = scheduleDataHandler.GetScheduleDayWeekList(dtFrom, dtToDate, siteId);
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
            bool recurFrequencyCheckPassed = true;
            if (scheduleDTO == null)
            {
                log.Debug("Ends-IsRelevant() Method.");
                return false;
            }
            if (DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleTime.Date) < 0)
            {
                log.Debug("Ends-IsRelevant() Method.");
                return false;
            }
            if (string.Equals(scheduleDTO.RecurFlag, "Y"))
            {
                if (DateTime.Compare(dateTime.Date, scheduleDTO.RecurEndDate.Date) > 0)
                {
                    log.Debug("Ends-IsRelevant() Method.");
                    return false;
                }
                decimal scheduleTime = new decimal(scheduleDTO.ScheduleTime.Hour + (((double)scheduleDTO.ScheduleTime.Minute) / 100));
                decimal scheduleEndTime = new decimal(scheduleDTO.ScheduleEndDate.Hour + (((double)scheduleDTO.ScheduleEndDate.Minute) / 100));
                decimal currentTime = new decimal(dateTime.Hour + (((double)dateTime.Minute) / 100));
                if (scheduleTime > currentTime)
                {
                    log.Debug("Ends-IsRelevant() Method.");
                    return false;
                }
                if (scheduleEndTime <= currentTime)
                {
                    log.Debug("Ends-IsRelevant() Method.");
                    return false;
                }
                recurFrequencyCheckPassed = CheckRecurFrequency(dateTime);
                if (recurFrequencyCheckPassed)
                {
                    if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
                    {
                        log.Debug("Ends-IsRelevant() Method.");
                        recurFrequencyCheckPassed = false;
                    }
                }
                else
                {
                    if (string.Equals(CheckInclusionExclusion(dateTime), "Y"))
                    {
                        log.Debug("Ends-IsRelevant() Method.");
                        recurFrequencyCheckPassed = true;
                    }
                }
                log.Debug("Ends-IsRelevant() Method.");
                return recurFrequencyCheckPassed;
            }
            else
            {
                if (DateTime.Compare(dateTime, scheduleDTO.ScheduleTime) < 0)
                {
                    log.Debug("Ends-IsRelevant() Method.");
                    return false;
                }
                if (DateTime.Compare(dateTime, scheduleDTO.ScheduleEndDate) >= 0)
                {
                    log.Debug("Ends-IsRelevant() Method.");
                    return false;
                }
                if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
                {
                    log.Debug("Ends-IsRelevant() Method.");
                    return false;
                }
                else
                {
                    log.Debug("Ends-IsRelevant() Method.");
                    return true;
                }
            }
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

        /// <summary>
        /// Check Inclusion Exclusion
        /// </summary>
        /// <param name="dateTime">dateTime</param>
        /// <returns></returns>
        private string CheckInclusionExclusion(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            string returnValue = "";
            List<ScheduleExclusionDTO> scheduleExclusionDTOList = null;
            ScheduleExclusionList scheduleExclusionListBL = new ScheduleExclusionList();
            List<KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>> searchParameter = new List<KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>>();
            searchParameter.Add(new KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>(ScheduleExclusionDTO.SearchByScheduleExclusionParameters.SCHEDULE_ID, scheduleDTO.ScheduleId.ToString()));
            searchParameter.Add(new KeyValuePair<ScheduleExclusionDTO.SearchByScheduleExclusionParameters, string>(ScheduleExclusionDTO.SearchByScheduleExclusionParameters.IS_ACTIVE, "1"));
            scheduleExclusionDTOList = scheduleExclusionListBL.GetAllScheduleExclusions(searchParameter);
            if (scheduleExclusionDTOList != null)
            {
                foreach (ScheduleExclusionDTO scheduleExclusionDTO in scheduleExclusionDTOList)
                {
                    if (scheduleExclusionDTO.Day > 0)
                    {
                        if ((int)dateTime.DayOfWeek == (scheduleExclusionDTO.Day - 1))
                        {
                            returnValue = scheduleExclusionDTO.IncludeDate;
                        }
                    }
                }
                foreach (ScheduleExclusionDTO scheduleExclusionDTO in scheduleExclusionDTOList)
                {
                    if (!string.IsNullOrWhiteSpace(scheduleExclusionDTO.ExclusionDate))
                    {
                        DateTime exclusionDate = Convert.ToDateTime(scheduleExclusionDTO.ExclusionDate);
                        if (DateTime.Compare(new DateTime(exclusionDate.Year, exclusionDate.Month, exclusionDate.Day), new DateTime(dateTime.Year, dateTime.Month, dateTime.Day)) == 0)
                        {
                            returnValue = scheduleExclusionDTO.IncludeDate;
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
        public int SaveSchedule()
        {
            try
            {
                int scheduleId = 0;
                IsRelevantValidation(DateTime.Now);
                Save();
                scheduleId = scheduleDTO.ScheduleId;
                log.LogMethodExit(scheduleId);
                return scheduleId;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
        }

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
                ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "ScheduleDTO"), MessageContainer.GetMessage(executionContext, 1800));//"Schedule details are not set"
                validationErrorList.Add(validationError);
            }
            if (DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleTime.Date) < 0)
            {
                log.Info("DateTime.Compare(dateTime.Date, scheduleDTO.ScheduleTime.Date) < 0");
                ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "Date"), MessageContainer.GetMessage(executionContext, 1801));//"Date is before with schedule date"
                validationErrorList.Add(validationError);
            }
            if (string.Equals(scheduleDTO.RecurFlag, "Y"))
            {
                if (DateTime.Compare(dateTime.Date, scheduleDTO.RecurEndDate.Date) > 0)
                {
                    log.Info("DateTime.Compare(dateTime.Date, scheduleDTO.RecurEndDate.Date) > 0");
                    ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "Date"), MessageContainer.GetMessage(executionContext, 1802));// "Date is greater than with recurring end date"
                    validationErrorList.Add(validationError);
                }
                decimal scheduleTime = new decimal(scheduleDTO.ScheduleTime.Hour + (((double)scheduleDTO.ScheduleTime.Minute) / 100));
                decimal scheduleEndTime = new decimal(scheduleDTO.ScheduleEndDate.Hour + (((double)scheduleDTO.ScheduleEndDate.Minute) / 100));
                decimal currentTime = new decimal(dateTime.Hour + (((double)dateTime.Minute) / 100));
                if (scheduleTime > currentTime)
                {
                    log.Info("scheduleTime > currentTime");
                    ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "ScheduleTime"), MessageContainer.GetMessage(executionContext, 1803));//"Schedule start time is greater than date time"
                    validationErrorList.Add(validationError);
                }
                if (scheduleEndTime <= currentTime)
                {
                    log.Info("scheduleEndTime <= currentTime");
                    ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "ScheduleTime"), MessageContainer.GetMessage(executionContext, 1804));// "Schedule end time is less than date time"
                    validationErrorList.Add(validationError);
                }
                recurFrequencyCheckPassed = CheckRecurFrequency(dateTime);
                if (recurFrequencyCheckPassed)
                {
                    if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
                    {
                        log.Info("string.Equals(CheckInclusionExclusion(dateTime), 'N')");
                        ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "Check Inclusion Exclusion"), MessageContainer.GetMessage(executionContext, 1805));
                        validationErrorList.Add(validationError);
                    }
                }
                else
                {
                    if (string.Equals(CheckInclusionExclusion(dateTime), "Y") == false)
                    {
                        log.Info("string.Equals(CheckInclusionExclusion(dateTime), 'Y') == false");
                        ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "Check Inclusion Exclusion"), MessageContainer.GetMessage(executionContext, 1805));//"Date fails schedule inclusion exclusion check"
                        validationErrorList.Add(validationError);
                    }
                }
            }
            else
            {
                if (DateTime.Compare(dateTime, scheduleDTO.ScheduleTime) < 0)
                {
                    log.Info("DateTime.Compare(dateTime, scheduleDTO.ScheduleTime) < 0");
                    ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "Date"), MessageContainer.GetMessage(executionContext, 1806)); //"Date time is before schedule time"
                    validationErrorList.Add(validationError);
                }
                if (DateTime.Compare(dateTime, scheduleDTO.ScheduleEndDate) >= 0)
                {
                    log.Info("DateTime.Compare(dateTime, scheduleDTO.ScheduleEndDate) >= 0");
                    ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "Date"), MessageContainer.GetMessage(executionContext, 1807));//"Date time is greater than schedule end time"
                    validationErrorList.Add(validationError);
                }
                if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
                {
                    log.Info("string.Equals(CheckInclusionExclusion(dateTime), 'N')");
                    ValidationError validationError = new ValidationError(MessageContainer.GetMessage(executionContext, "Schedule"), MessageContainer.GetMessage(executionContext, "Check Inclusion Exclusion"), MessageContainer.GetMessage(executionContext, 1805)); //"Date fails schedule inclusion exclusion check"
                    validationErrorList.Add(validationError);
                }
            }

            if (validationErrorList != null && validationErrorList.Count > 0)
            {
                throw new ValidationException(MessageContainer.GetMessage(executionContext, 1808), validationErrorList);// "Datetime passed fails relavancy checks"
            }
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of Schedule
    /// </summary>
    public class ScheduleList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;


        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ScheduleList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Schedule list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ScheduleDTO> GetAllSchedule(List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ScheduleDataHandler scheduleDataHandler = new ScheduleDataHandler(sqlTransaction);
            List<ScheduleDTO> scheduleDTOsList = scheduleDataHandler.GetScheduleList(searchParameters);
            return scheduleDTOsList;
        }

        /// <summary>
        /// Returns the Schedule list based on filtering
        /// </summary>
        public List<ScheduleDTO> GetAllScheduleList(List<int> scheduleIdList,string isActive)
        {
            log.LogMethodEntry(scheduleIdList);
            try
            {
                ScheduleList scheduleListBL = new ScheduleList(executionContext);
                List<ScheduleDTO> scheduleDTOList = new List<ScheduleDTO>();
                List<ScheduleDTO> bindingSortingScheduleDTOList = new List<ScheduleDTO>();
                foreach (int scheduleId in scheduleIdList)
                {
                    List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>> searchScheduleParameters = new List<KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>>();
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchScheduleParameters.Add(new KeyValuePair<ScheduleDTO.SearchByScheduleParameters, string>(ScheduleDTO.SearchByScheduleParameters.SCHEDULE_ID, scheduleId.ToString()));
                    List<ScheduleDTO> list = scheduleListBL.GetAllSchedule(searchScheduleParameters);
                    if (list != null && list.Count > 0)
                    {
                        scheduleDTOList.Add(list[0]);
                    }
                }
                bool show = true;
                DateTime dtpSchedule = DateTime.Today;
                foreach (ScheduleDTO scheduleDTO in scheduleDTOList)
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
    }
}
