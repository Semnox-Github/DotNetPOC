/********************************************************************************************
 * Project Name - StringValueConverter
 * Description  - Data object of StringValueConverter
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
    /// Converts string value from/to string
    /// </summary>
    public class StringValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the string value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                log.LogMethodExit(string.Empty);
                return string.Empty;
            }
            log.LogMethodExit(stringValue);
            return stringValue.Trim();
        }

        /// <summary>
        /// Converts string value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            if (value == null)
            {
                return string.Empty;
            }
            if (value is string == false)
            {
                log.Error("Invalid value");
                log.LogMethodExit(null, "Throwing Exception");
                throw new Exception("Invalid value. please pass a string. value: " + value == null ? "null" : value.ToString());
            }
            if (value is string && string.IsNullOrWhiteSpace(value as string))
            {
                log.LogMethodExit(string.Empty);
                return string.Empty;
            }
            log.LogMethodExit(value);
            return (value as string).Trim().ToString();
        }
    }
}
