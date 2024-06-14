/********************************************************************************************
 * Project Name - Promotions
 * Description  - Helper class to calculate the promotion calendar
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      06-May-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Semnox.Parafait.Promotions
{
    public class PromotionCalendarCalculator
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly PromotionDTO promotionDTO;

        enum DateInterval
        {
            Month,
            Week,
            Day,
        }

        enum DateParts
        {
            Day,
            WeekDay,
        }

        public PromotionCalendarCalculator(PromotionDTO promotionDTO)
        {
            log.LogMethodEntry();
            if (promotionDTO == null)
            {
                log.LogMethodExit("Throwing Argument Null Exception");
                throw new ArgumentNullException("promotionDTO");
            }
            this.promotionDTO = promotionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns whether the promotion exist at a given date time
        /// </summary>
        /// <param name="dateTime">date</param>
        /// <returns></returns>
        public bool IsPromotionExistOn(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            if (DateTime.Compare(dateTime.Date, promotionDTO.TimeFrom.Date) < 0)
            {
                log.LogMethodExit(false);
                return false;
            }
            if (InclusionExclusionDateExists(dateTime))
            {
                bool included = IsIncludedDate(dateTime);
                log.LogMethodExit(included, "InclusionDateExists");
                return included;
            }
            if (char.Equals(promotionDTO.RecurFlag, 'Y'))
            {
                double duration = promotionDTO.TimeTo.Subtract(promotionDTO.TimeFrom).TotalMinutes;
                if (IsDateBetween(dateTime, promotionDTO.TimeFrom, promotionDTO.TimeTo))
                {
                    log.LogMethodExit(true, "IsDateBetween(dateTime, promotionDTO.TimeFrom, promotionDTO.TimeTo)");
                    return true;
                }
                
                if (DateTime.Compare(promotionDTO.RecurEndDate.Value.Date, dateTime.Date) < 0)
                {
                    log.LogMethodExit(false);
                    return false;
                }
                if (promotionDTO.RecurFrequency.HasValue && char.Equals(promotionDTO.RecurFrequency.Value, 'D'))
                {
                    DateTime promotionFromDaily = DateAdd(DateInterval.Day, DateDiff(DateInterval.Day, promotionDTO.TimeFrom, dateTime), promotionDTO.TimeFrom);
                    if (IsDateBetween(dateTime, promotionFromDaily, promotionFromDaily.AddMinutes(duration)))
                    {
                        log.LogMethodExit(true, "IsDateBetween(dateTime, promotionFromDaily, promotionFromDaily.AddMinutes(duration))");
                        return true;
                    }
                    //DateTime previousDay = DateAdd(DateInterval.Day, -1, promotionFromDaily);
                    //if (IsDateBetween(dateTime, previousDay, previousDay.AddMinutes(duration)))
                    //{
                    //    log.LogMethodExit(true, "IsDateBetween(dateTime, previousDay, previousDay.AddMinutes(duration))");
                    //    return true;
                    //}
                }
                if (promotionDTO.RecurFrequency.HasValue && char.Equals(promotionDTO.RecurFrequency.Value, 'W'))
                {
                    DateTime promotionFromForWeekly = DateAdd(DateInterval.Week, DateDiff(DateInterval.Week, promotionDTO.TimeFrom, dateTime), promotionDTO.TimeFrom);
                    if (IsDateBetween(dateTime, promotionFromForWeekly, promotionFromForWeekly.AddMinutes(duration)))
                    {
                        log.LogMethodExit(true, "IsDateBetween(dateTime, promotionFromForWeekly, promotionFromForWeekly.AddMinutes(duration))");
                        return true;
                    }
                    //DateTime previousWeek = DateAdd(DateInterval.Day, -7, promotionFromForWeekly);
                    //if (IsDateBetween(dateTime, previousWeek, previousWeek.AddMinutes(duration)))
                    //{
                    //    log.LogMethodExit(true, "IsDateBetween(dateTime, previousWeek, previousWeek.AddMinutes(duration))");
                    //    return true;
                    //}
                }
                if (promotionDTO.RecurFrequency.HasValue && char.Equals(promotionDTO.RecurFrequency.Value, 'M'))
                {
                    DateTime promotionFromForMonthly;
                    if (char.Equals(promotionDTO.RecurType, 'D'))
                    {
                        promotionFromForMonthly = DateAdd(DateInterval.Month, DateDiff(DateInterval.Month, promotionDTO.TimeFrom, dateTime), promotionDTO.TimeFrom);
                    }
                    else if (char.Equals(promotionDTO.RecurType, 'W'))
                    {
                        log.LogMethodExit(false, "RecurFlag is 'W' is not supported");
                        return false;
                    }
                    else
                    {
                        log.LogMethodExit(false, "RecurFlag is undefined");
                        return false;
                    }
                    if (IsDateBetween(dateTime, promotionFromForMonthly, promotionFromForMonthly.AddMinutes(duration)))
                    {
                        log.LogMethodExit(true, "IsDateBetween(dateTime, promotionFromForWeekly, promotionFromForWeekly.AddMinutes(duration))");
                        return true;
                    }
                }
            }   
            else
            {
                if (IsDateBetween(dateTime, promotionDTO.TimeFrom, promotionDTO.TimeTo))
                {
                    log.LogMethodExit(true, "IsDateBetween(dateTime, promotionDTO.TimeFrom, promotionDTO.TimeTo)");
                    return true;
                }
            }
            log.LogMethodExit(false);
            return false;
        }

        private bool InclusionExclusionDateExists(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            bool returnValue = false;
            List<PromotionExclusionDateDTO> promotionExclusionDateDTOList = promotionDTO.PromotionExclusionDateDTOLists;
            if (promotionExclusionDateDTOList == null ||
                promotionExclusionDateDTOList.Any() == false)
            {
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            foreach (PromotionExclusionDateDTO promotionExclusionDateDTO in promotionExclusionDateDTOList)
            {
                if (promotionExclusionDateDTO.ExclusionDate.HasValue)
                {
                    DateTime exclusionDate = promotionExclusionDateDTO.ExclusionDate.Value;
                    if (DateTime.Compare(new DateTime(exclusionDate.Year, exclusionDate.Month, exclusionDate.Day), new DateTime(dateTime.Year, dateTime.Month, dateTime.Day)) == 0)
                    {
                        returnValue = true;
                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                }
            }
            foreach (PromotionExclusionDateDTO promotionExclusionDateDTO in promotionExclusionDateDTOList)
            {
                if (promotionExclusionDateDTO.Day > 0)
                {
                    if ((int)dateTime.DayOfWeek == (promotionExclusionDateDTO.Day - 1))
                    {
                        returnValue = true;
                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private bool IsIncludedDate(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            bool returnValue = false;
            List<PromotionExclusionDateDTO> promotionExclusionDateDTOList = promotionDTO.PromotionExclusionDateDTOLists;
            foreach (PromotionExclusionDateDTO promotionExclusionDateDTO in promotionExclusionDateDTOList)
            {
                if (promotionExclusionDateDTO.ExclusionDate.HasValue)
                {
                    DateTime exclusionDate = promotionExclusionDateDTO.ExclusionDate.Value;
                    if (DateTime.Compare(new DateTime(exclusionDate.Year, exclusionDate.Month, exclusionDate.Day), new DateTime(dateTime.Year, dateTime.Month, dateTime.Day)) == 0)
                    {
                        returnValue = char.Equals(promotionExclusionDateDTO.IncludeDate.Value, 'Y');
                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                }
            }
            foreach (PromotionExclusionDateDTO promotionExclusionDateDTO in promotionExclusionDateDTOList)
            {
                if (promotionExclusionDateDTO.Day > 0)
                {
                    if ((int)dateTime.DayOfWeek == (promotionExclusionDateDTO.Day - 1))
                    {
                        returnValue = char.Equals(promotionExclusionDateDTO.IncludeDate.Value, 'Y');
                        log.LogMethodExit(returnValue);
                        return returnValue;
                    }
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        private int DateDiff(DateInterval interval, DateTime date1, DateTime date2)
        {
            int result;
            switch (interval)
            {
                case DateInterval.Month:
                    {
                        result = (date2.Month - date1.Month) + (12 * (date2.Year - date1.Year));
                    }
                    break;
                case DateInterval.Week:
                    {
                        result = ((int)date2.Date.Subtract(date1.Date).TotalDays) / 7;
                    }
                    break;
                case DateInterval.Day:
                    {
                        result = (int)date2.Date.Subtract(date1.Date).TotalDays;
                    }
                    break;
                default:
                    {
                        throw new ArgumentException("interval");
                    }
            }
            log.LogMethodExit(result);
            return result;
        }

        private DateTime DateAdd(DateInterval interval, int increment, DateTime date)
        {
            log.LogMethodEntry(interval, increment, date);
            DateTime result;
            switch (interval)
            {
                case DateInterval.Month:
                    {
                        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                        result = calendar.AddMonths(date, increment);
                    }
                    break;
                case DateInterval.Week:
                    {
                        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                        result = calendar.AddWeeks(date, increment);
                    }
                    break;
                case DateInterval.Day:
                    {
                        Calendar calendar = CultureInfo.InvariantCulture.Calendar;
                        result = calendar.AddDays(date, increment);
                    }
                    break;
                default:
                    {
                        throw new ArgumentException("interval");
                    }
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool IsDateBetween(DateTime dateTime, DateTime fromDate, DateTime toDate)
        {
            log.LogMethodEntry(dateTime, fromDate, toDate);
            bool result = false;
            if (DateTime.Compare(dateTime, fromDate) < 0)
            {
                log.LogMethodExit(result);
                return result;
            }
            if (DateTime.Compare(toDate, dateTime) < 0)
            {
                log.LogMethodExit(result);
                return result;
            }
            log.LogMethodExit(true);
            return true;
        }

        public List<DateTime> GetSignificantDateTimesInRange(ContinuousDateTimeRanges continuousDateTimeRanges, TimeSpan increment)
        {
            log.LogMethodEntry(continuousDateTimeRanges, increment);
            List<DateTime> significantDateTimes = new List<DateTime>();
            bool previousPromotionAvailable = IsPromotionExistOn(continuousDateTimeRanges.StartDateTime);
            foreach (var dateTime in continuousDateTimeRanges.GetDateTimesInRange(increment))
            {
                bool promotionAvailable = IsPromotionExistOn(dateTime);
                if (previousPromotionAvailable != promotionAvailable)
                {
                    significantDateTimes.Add(dateTime);
                    previousPromotionAvailable = promotionAvailable;
                }
            }
            log.LogMethodExit(significantDateTimes);
            return significantDateTimes;
        }

    }
}
