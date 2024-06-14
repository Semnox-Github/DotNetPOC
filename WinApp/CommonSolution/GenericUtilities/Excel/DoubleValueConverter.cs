/********************************************************************************************
 * Project Name - DoubleValueConverter
 * Description  - Data object of DoubleValueConverter
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

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Converts double value from/to string
    /// </summary>
    public class DoubleValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string format;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        public DoubleValueConverter()
        {
            log.LogMethodEntry("Default constructor");
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="format"></param>
        public DoubleValueConverter(string format)
        {
            log.LogMethodEntry(format);
            this.format = format;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the double value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            double convertedValue;
            if (double.TryParse(stringValue.Trim(), out convertedValue))
            {
                log.LogMethodExit(convertedValue);
                return convertedValue;
            }
            log.Error("Conversion Failed");
            log.LogMethodExit(null, "Throwing Exception");
            throw new Exception("Unable to convert '" + (stringValue == null ? "null" : stringValue) + "' to a double.");
        }
        /// <summary>
        /// Converts double value to string
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
                    double i;
                    if (double.TryParse(value as string, out i))
                    {
                        result = value as string;
                    }
                    else
                    {
                        log.Error("Invalid value");
                        throw new Exception("Invalid value. Please pass a double value. value: " + value == null ? "null" : value.ToString());
                    }
                }
                else if (value is double)
                {
                    result = ((double)value).ToString(format);
                }
                else if (value is double? && (value as double?).HasValue)
                {
                    result = (value as double?).Value.ToString(format);
                }
                else
                {
                    log.Error("Conversion Failed");
                    log.LogMethodExit(null, "Throwing Exception");
                    throw new Exception("Invalid value. Please pass a double value. value: " + value == null ? "null" : value.ToString());
                }
            }
            log.LogMethodExit();
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
