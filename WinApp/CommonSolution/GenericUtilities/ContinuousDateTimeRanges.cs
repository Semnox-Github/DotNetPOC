/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Represents a continuous date time ranges.
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      07-Aug-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    public class ContinuousDateTimeRanges : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly List<DateTimeRange> values = new List<DateTimeRange>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        public ContinuousDateTimeRanges(DateTime startDateTime, DateTime endDateTime)
            :this(new List<DateTimeRange>(){ new DateTimeRange(startDateTime, endDateTime) })
        {
            log.LogMethodEntry(startDateTime, endDateTime);
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="dateTimeRanges"></param>
        public ContinuousDateTimeRanges(IEnumerable<DateTimeRange> dateTimeRanges)
        {
            log.LogMethodEntry(dateTimeRanges);
            if(dateTimeRanges == null)
            {
                throw new ArgumentNullException("dateTimeRanges");
            }
            values = dateTimeRanges.OrderBy(x => x.StartDateTime).ToList();
            for (int i = 0; i < values.Count - 1; i++)
            {
                if (values[i].EndDateTime != values[i + 1].StartDateTime)
                {
                    throw new ArgumentException("Invalid dateTimeRanges date time range should be continuous.");
                }
            }
            log.LogMethodExit();
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            log.LogMethodEntry();
            foreach (var value in values)
            {
                yield return value;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the startDate continuous range
        /// </summary>
        public DateTime StartDateTime
        {
            get { return values[0].StartDateTime; }
        }

        /// <summary>
        /// Get method of the endDate continuous range
        /// </summary>
        public DateTime EndDateTime
        {
            get { return values[values.Count - 1].EndDateTime; }
        }

        /// <summary>
        /// Returns whether the give date time is within  the continuous date time range
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(DateTime value)
        {
            log.LogMethodEntry(value);
            bool result = false;
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
            {
                return result;
            }
            result = value >= StartDateTime && value <= EndDateTime;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the date time values with in the range, separated by given increment 
        /// </summary>
        /// <param name="increment"></param>
        /// <returns></returns>
        public IEnumerable<DateTime> GetDateTimesInRange(TimeSpan increment)
        {
            log.LogMethodEntry(increment);
            DateTime value = StartDateTime;
            while (value < EndDateTime)
            {
                yield return value;
                value = value.Add(increment);
            }
            yield return EndDateTime;
            log.LogMethodExit();

        }

        /// <summary>
        /// Get method of sub date time range of this continuous date time range
        /// </summary>
        public IEnumerable<DateTimeRange> DateTimeRanges
        {
            get
            {
                foreach (var value in values)
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Spits the continuous date time ranges at a given date time.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public ContinuousDateTimeRanges Split(DateTime dateTime)
        {
            log.LogMethodEntry(dateTime);
            if (Contains(dateTime) == false)
            {
                log.LogMethodExit(this, "The date time doesn't falls withing the range hence no splitting is required.");
                return this;
            }
            if(values.Any(x => x.StartDateTime == dateTime || x.EndDateTime == dateTime))
            {
                log.LogMethodExit(this, "The range is already split at given date time.");
                return this;
            }
            List<DateTimeRange> dateTimeRanges = new List<DateTimeRange>();
            foreach (var value in values)
            {
                if(value.Contains(dateTime) == false)
                {
                    dateTimeRanges.Add(value);
                    continue;
                }

                dateTimeRanges.Add(new DateTimeRange(value.StartDateTime, dateTime));
                dateTimeRanges.Add(new DateTimeRange(dateTime, value.EndDateTime));
            }
            ContinuousDateTimeRanges result = new ContinuousDateTimeRanges(dateTimeRanges);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Spits the continuous date time ranges at a given date time with precision.
        /// </summary>
        /// <param name="dateTime">Split the continuous date time range at given date time</param>
        /// <param name="margin">round up to nearest margin</param>
        /// <returns></returns>
        public ContinuousDateTimeRanges Split(DateTime dateTime, TimeSpan margin)
        {
            log.LogMethodEntry(dateTime);
            DateTime adjustedDateTime = new DateTime((dateTime.Ticks + margin.Ticks - 1) / margin.Ticks * margin.Ticks, dateTime.Kind);
            if (Contains(adjustedDateTime) == false)
            {
                log.LogMethodExit(this, "The date time doesn't falls withing the range hence no splitting is required.");
                return this;
            }
            ContinuousDateTimeRanges result = Split(adjustedDateTime);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns a string that represents the current constructor fields
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return "Start Date time: " + StartDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + " - End Date time: " + EndDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public static implicit operator ContinuousDateTimeRanges(DateTimeRange dateTimeRange)
        {
            return new ContinuousDateTimeRanges(dateTimeRange.StartDateTime, dateTimeRange.EndDateTime);
        }
    }
}
