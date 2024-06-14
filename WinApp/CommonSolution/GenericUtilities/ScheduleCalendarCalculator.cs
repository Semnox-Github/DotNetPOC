/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - class of ScheduleCalendarCalculator
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      06-May-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class ScheduleCalendarCalculator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ScheduleCalendarDTO scheduleCalendarDTO;

        public ScheduleCalendarCalculator(ScheduleCalendarDTO scheduleCalendarDTO)
        {
            this.scheduleCalendarDTO = scheduleCalendarDTO;
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
            if (scheduleCalendarDTO == null)
            {
                //log.Debug("Ends-IsRelevant() Method.");
                log.LogMethodExit();
                return false;
            }
            if (DateTime.Compare(dateTime.Date, scheduleCalendarDTO.ScheduleTime.Date) < 0)
            {
                //log.Debug("Ends-IsRelevant() Method.");
                log.LogMethodExit();
                return false;
            }
            if (string.Equals(scheduleCalendarDTO.RecurFlag, "Y"))
            {
                if (DateTime.Compare(dateTime.Date, scheduleCalendarDTO.RecurEndDate.Date) > 0)
                {
                    log.LogMethodExit();
                    //log.Debug("Ends-IsRelevant() Method.");
                    return false;
                }
                decimal scheduleTime = new decimal(scheduleCalendarDTO.ScheduleTime.Hour + (((double)scheduleCalendarDTO.ScheduleTime.Minute) / 100));
                decimal scheduleEndTime = new decimal(scheduleCalendarDTO.ScheduleEndDate.Hour + (((double)scheduleCalendarDTO.ScheduleEndDate.Minute) / 100));
                decimal currentTime = new decimal(dateTime.Hour + (((double)dateTime.Minute) / 100));
                if (scheduleTime > currentTime)
                {
                    log.LogMethodExit();
                    //log.Debug("Ends-IsRelevant() Method.");
                    return false;
                }
                if (scheduleEndTime <= currentTime)
                {
                    //log.Debug("Ends-IsRelevant() Method.");
                    log.LogMethodExit();
                    return false;
                }
                recurFrequencyCheckPassed = CheckRecurFrequency(dateTime);
                if (recurFrequencyCheckPassed)
                {
                    if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
                    {
                        //log.Debug("Ends-IsRelevant() Method.");
                        log.LogMethodExit();
                        recurFrequencyCheckPassed = false;
                    }
                }
                else
                {
                    if (string.Equals(CheckInclusionExclusion(dateTime), "Y"))
                    {
                        //log.Debug("Ends-IsRelevant() Method.");
                        log.LogMethodExit();
                        recurFrequencyCheckPassed = true;
                    }
                }
                log.LogMethodExit();
                //log.Debug("Ends-IsRelevant() Method.");
                return recurFrequencyCheckPassed;
            }
            else
            {
                if (DateTime.Compare(dateTime, scheduleCalendarDTO.ScheduleTime) < 0)
                {
                    //log.Debug("Ends-IsRelevant() Method.");
                    log.LogMethodExit();
                    return false;
                }
                if (DateTime.Compare(dateTime, scheduleCalendarDTO.ScheduleEndDate) >= 0)
                {
                    //log.Debug("Ends-IsRelevant() Method.");
                    log.LogMethodExit();
                    return false;
                }
                if (string.Equals(CheckInclusionExclusion(dateTime), "N"))
                {
                    //log.Debug("Ends-IsRelevant() Method.");
                    log.LogMethodExit();
                    return false;
                }
                else
                {
                    //log.Debug("Ends-IsRelevant() Method.");
                    log.LogMethodExit();
                    return true;
                }
            }
        }

        private bool CheckRecurFrequency(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            bool valid = false;
            switch (scheduleCalendarDTO.RecurFrequency)
            {
                case "D":
                    {
                        valid = true;
                        break;
                    }
                case "W":
                    {
                        if (scheduleCalendarDTO.ScheduleTime.DayOfWeek <= scheduleCalendarDTO.ScheduleEndDate.DayOfWeek)
                        {
                            if (scheduleCalendarDTO.ScheduleTime.DayOfWeek <= dateTime.DayOfWeek && scheduleCalendarDTO.ScheduleEndDate.DayOfWeek >= dateTime.DayOfWeek)
                            {
                                valid = true;
                            }
                        }
                        else
                        {
                            if (scheduleCalendarDTO.ScheduleTime.DayOfWeek <= dateTime.DayOfWeek || scheduleCalendarDTO.ScheduleEndDate.DayOfWeek >= dateTime.DayOfWeek)
                            {
                                valid = true;
                            }
                        }

                        break;
                    }
                case "M":
                    {
                        if (string.Equals(scheduleCalendarDTO.RecurType, "W"))
                        {
                            GregorianCalendar gregorianCalendar = new GregorianCalendar();
                            int scheduleWeekNo = gregorianCalendar.GetWeekOfYear(scheduleCalendarDTO.ScheduleTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) - gregorianCalendar.GetWeekOfYear(new DateTime(scheduleCalendarDTO.ScheduleTime.Year, scheduleCalendarDTO.ScheduleTime.Month, 1), CalendarWeekRule.FirstDay, DayOfWeek.Sunday) + 1;
                            if (new DateTime(scheduleCalendarDTO.ScheduleTime.Year, scheduleCalendarDTO.ScheduleTime.Month, 1).DayOfWeek > scheduleCalendarDTO.ScheduleTime.DayOfWeek)
                            {
                                scheduleWeekNo--;
                            }
                            int dateTimeWeekNo = gregorianCalendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) - gregorianCalendar.GetWeekOfYear(new DateTime(dateTime.Year, dateTime.Month, 1), CalendarWeekRule.FirstDay, DayOfWeek.Sunday) + 1;
                            if (new DateTime(dateTime.Year, dateTime.Month, 1).DayOfWeek > dateTime.DayOfWeek)
                            {
                                dateTimeWeekNo--;
                            }
                            if (scheduleWeekNo == dateTimeWeekNo && scheduleCalendarDTO.ScheduleTime.DayOfWeek == dateTime.DayOfWeek)
                            {
                                valid = true;
                            }
                        }
                        else
                        {
                            if (scheduleCalendarDTO.ScheduleTime.Day == dateTime.Day)
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
            List<ScheduleCalendarExclusionDTO> scheduleExclusionDTOList = scheduleCalendarDTO.ScheduleCalendarExclusionDTOList;
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

    }
}
