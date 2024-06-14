/********************************************************************************************
 * Project Name - DateTimeValueConverter
 * Description  - Data object of DecimalValueConverter
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's.
 **********************************************************************************************/
using System;

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Converts decimal value from/to string
    /// </summary>
    public class DecimalValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string format;

        /// <summary>
        /// default constructor
        /// </summary>
        public DecimalValueConverter()
        {
            log.LogMethodEntry("Empty Constructor for DecimalValueConverter ");
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="format"></param>
        public DecimalValueConverter(string format)
        {
            log.LogMethodEntry(format);
            this.format = format;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the decimal value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            decimal convertedValue;
            if (decimal.TryParse(stringValue.Trim(), out convertedValue))
            {
                log.LogMethodExit(convertedValue);
                return convertedValue;
            }
            log.Error("Decimal Conversion Failed");
            log.LogMethodExit(null, "Throwing Exception");
            throw new Exception("Unable to convert '" + (stringValue == null ? "null" : stringValue) + "' to a decimal.");
        }
        /// <summary>
        /// Converts decimal value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            string result = string.Empty;
            if (value != null)
            {
                if (value is string)
                {
                    decimal i;
                    if (decimal.TryParse(value as string, out i))
                    {
                        result = value as string;
                    }
                    else
                    {
                        throw new Exception("Invalid value. Please pass a decimal value. value: " + value == null ? "null" : value.ToString());
                    }
                }
                else if (value is decimal)
                {
                    result = ((decimal)value).ToString(format);
                }
                else if (value is decimal? && (value as decimal?).HasValue)
                {
                    result = (value as decimal?).Value.ToString(format);
                }
                else
                {
                    throw new Exception("Invalid value. Please pass a decimal value. value: " + value == null ? "null" : value.ToString());
                }
            }
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
