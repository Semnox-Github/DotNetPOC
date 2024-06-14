/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - class of DateTimeRange
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      06-May-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// DateTime Range
    /// </summary>
    public class DateTimeRange : ValueObject
    {
        private static readonly Semnox.Parafait.logging.Logger log =
            new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly DateTime startDateTime;
        private readonly DateTime endDateTime;

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        public DateTimeRange(DateTime startDateTime, DateTime endDateTime)
        {
            log.LogMethodEntry(startDateTime, endDateTime);
            if(startDateTime == DateTime.MinValue)
            {
                throw new ArgumentException("Invalid " + "startDateTime", "startDateTime");
            }
            if (endDateTime == DateTime.MinValue)
            {
                throw new ArgumentException("Invalid " + "endDateTime", "endDateTime");
            }
            if(startDateTime >= endDateTime)
            {
                throw new ArgumentException("start date should be less than end date");
            }
            this.startDateTime = startDateTime;
            this.endDateTime = endDateTime;
            log.LogMethodExit();

        }

        /// <summary>
        /// Returns the startDate and endDate
        /// </summary>
        protected override IEnumerable<object> GetAtomicValues()
        {
            log.LogMethodEntry();
            yield return startDateTime;
            yield return endDateTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get method of the startDate field
        /// </summary>
        public DateTime StartDateTime
        {
            get { return startDateTime; }
        }

        /// <summary>
        /// Get method of the endDate field
        /// </summary>
        public DateTime EndDateTime
        {
            get { return endDateTime; }
        }

        public IEnumerable<DateTime> GetDateTimesInRange(TimeSpan increment)
        {
            log.LogMethodEntry(increment);
            DateTime value = startDateTime;
            while(value < endDateTime)
            {
                yield return value;
                value = value.Add(increment);
            }
            yield return endDateTime;
            log.LogMethodExit();

        }

        public bool Contains(DateTime value)
        {
            log.LogMethodEntry(value);
            bool result = false;
            if (value == DateTime.MinValue || value == DateTime.MaxValue)
            {
                return result;
            }
            result = value >= startDateTime && value <= endDateTime;
            log.LogMethodExit(result);
            return result;
        }

        public bool Contains(DateTimeRange range)
        {
            log.LogMethodEntry(range);
            bool result = Contains(range.StartDateTime) && Contains(range.endDateTime);
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
            return "Start Date time: " + startDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) + " - End Date time: " + endDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }
    }
}
