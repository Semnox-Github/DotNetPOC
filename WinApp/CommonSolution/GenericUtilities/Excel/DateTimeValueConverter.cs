/********************************************************************************************
 * Project Name - DateTimeValueConverter
 * Description  - Data object of DateTimeValueConverter
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's.
 **********************************************************************************************/
using System;
using System.Globalization;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Converts DateTime value from/to string
    /// </summary>
    public class DateTimeValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string format;
        /// <summary>
        /// Default constructor
        /// </summary>
        public DateTimeValueConverter()
        {
            log.LogMethodEntry("Empty Constructor for DateTimeValueConverter ");
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="format"></param>
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
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            if (string.IsNullOrWhiteSpace(stringValue) == false)
            {
                DateTime convertedValue;
                if (DateTime.TryParseExact(stringValue.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out convertedValue))
                {
                    return convertedValue;
                }
                double d;
                if(double.TryParse(stringValue, out d))
                {
                    try
                    {
                        convertedValue = DateTime.FromOADate(d);
                        return convertedValue;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Conversion Failed",ex);
                        log.LogMethodExit(null, "Throwing Exception"+ ex.Message);
                        throw new Exception("Unable to convert '" + (stringValue == null ? "null" : stringValue) + "' to a datetime.", ex);
                    }
                }
            }
            log.Error("Conversion Failed");
            throw new Exception("Invalid value can't convert :" + stringValue + " to DateTime");
        }

        /// <summary>
        /// Converts datetime value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string result = string.Empty;
            if (value != null)
            {
                if (value is string)
                {
                    DateTime convertedValue;
                    if (DateTime.TryParseExact((value as string), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out convertedValue))
                    {
                        result = value as string;
                    }
                    else
                    {
                        log.Error("Date Conversion Failed");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new Exception("Invalid value. Please pass a DateTime value. value: " + value == null? "null" : value.ToString());
                    }
                }
                else if (value is DateTime)
                {
                    result = ((DateTime)value).ToString(format);
                }
                else if (value is DateTime? && (value as DateTime?).HasValue)
                {
                    result = ((DateTime?)value).Value.ToString(format);
                }
                else
                {
                    log.Error("Date Conversion Failed");
                    log.LogMethodExit(null, "Throwing Exception");
                    throw new Exception("Invalid value. Please pass a DateTime value. value: " + value == null ? "null" : value.ToString());
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get/Set Methods of format field
        /// </summary>
        public string Format
        {
            get
            {
                return format;
            }

            set
            {
                this.format = value;
            }
        }
    }
}
