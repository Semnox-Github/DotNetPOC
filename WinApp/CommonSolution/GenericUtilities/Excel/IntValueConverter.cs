/********************************************************************************************
 * Project Name - IntValueConverter
 * Description  - Data object of IntValueConverter
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
    /// Converts int value from/to string
    /// </summary>
    public class IntValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the int value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int convertedValue;
            if (int.TryParse(stringValue.Trim(), out convertedValue))
            {
                log.LogMethodExit(convertedValue);
                return convertedValue;
            }
            log.Error("Conversion Failed");
            log.LogMethodExit(null, "Throwing Exception");
            throw new Exception("Unable to convert '" + (stringValue == null ? "null" : stringValue) + "' to an integer.");
        }

        /// <summary>
        /// Converts int value to string
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
                    int i;
                    if (int.TryParse(value as string, out i))
                    {
                        result = value as string;
                    }
                    else
                    {
                        log.Error("Conversion Failed");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new Exception("Invalid value. Please pass a integer value. value: " + value == null ? "null" : value.ToString());
                    }
                }
                else if (value is int)
                {
                    result = value.ToString();
                }
                else if (value is int? && (value as int?).HasValue)
                {
                    result = (value as int?).Value.ToString();
                }
                else
                {
                    log.Error("Conversion Failed");
                    log.LogMethodExit(null, "Throwing Exception");
                    throw new Exception("Invalid value. Please pass a integer value. value: " + value == null ? "null" : value.ToString());
                }
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
