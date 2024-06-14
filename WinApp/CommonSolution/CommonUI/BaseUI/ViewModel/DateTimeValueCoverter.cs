/********************************************************************************************
 * Project Name - GenericUtilities
 * Description  - Helper class to convert datetime from string 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 ********************************************************************************************* 
 *2.130.0     19-Jul-2021      Lakshminarayana     Created
 ********************************************************************************************/
using System;
using System.Globalization;

namespace Semnox.Parafait.CommonUI.BaseUI.ViewModel
{
    public class DateTimeValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string format;

        public DateTimeValueConverter(string format)
        {
            log.LogMethodEntry(format);
            this.format = format;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the datetime value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public DateTime? FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            DateTime? result = null;
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                log.LogMethodExit(result, "stringValue is empty");
                return result;
            }
            DateTime convertedValue;
            if (DateTime.TryParseExact(stringValue.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out convertedValue) == false)
            {
                log.LogMethodExit(result, "TryParseExact failed");
                return result;
            }
            result = convertedValue;
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts datetime value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string ToString(DateTime? value)
        {
            log.LogMethodEntry(value);
            string result = string.Empty;
            if(value.HasValue == false || value.Value == DateTime.MinValue)
            {
                log.LogMethodExit(result, "value.HasValue == false || value.Value == DateTime.MinValue`");
                return result;
            }
            result = value.Value.ToString(format, CultureInfo.InvariantCulture);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Returns the format used by the date time converter
        /// </summary>
        public string Format
        {
            get
            {
                return format;
            }
        }
    }
}
