/********************************************************************************************
 * Project Name - BooleanValueConverter
 * Description  - Data object of BooleanValueConverter
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
    ///  Converts boolean value from/to string
    /// </summary>
    public class BooleanValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Boolean the double value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            bool result = false;
            if(string.IsNullOrWhiteSpace(stringValue) == false)
            {
                if (stringValue.ToUpper() == "Y" || stringValue.ToUpper() == "TRUE" || stringValue.ToUpper() == "1" || stringValue.ToUpper() == "YES")
                {
                    result = true;
                }
                
                else if (stringValue.ToUpper() == "N" || stringValue.ToUpper() == "FALSE" || stringValue.ToUpper() == "0" || stringValue.ToUpper() == "NO")
                {
                    result = false;
                }
                else
                {
                    log.Error("Conversion Error");
                    throw new Exception("Unable to convert '" + (stringValue == null ? "null" : stringValue) + "' to a boolean.");
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts boolean value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string result = string.Empty;
            if(value is bool)
            {
                result = GetString((bool)value);
            }
            else if(value is bool? && (value as bool?).HasValue)
            {
                result = GetString((value as bool?).Value);
            }
            else
            {
                log.Error("Invalid parameter");
                throw new Exception("Invalid parameter please pass boolean. value: " + value == null? "null" : value.ToString());
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Converts boolean value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string GetString(bool value)
        {
            log.LogMethodEntry(value);
            string result = "N";
            if(value)
            {
                result = "Y";
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
