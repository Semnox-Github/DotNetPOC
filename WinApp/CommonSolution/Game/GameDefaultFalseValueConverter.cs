/********************************************************************************************
 * Project Name - Machine                                                                          
 * Description  - Value converter class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       16-Mar-2019   Muhammed Mehraj  Created 
 *2.70.2       12-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using System;

namespace Semnox.Parafait.Game
{
    public class GameDefaultFalseValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// This method default returns N if string is empty or null
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {

            log.LogMethodEntry(stringValue);
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return "N";
            }
            string returnValue = stringValue.Trim();
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// This method returns an empty string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            if (value == null)
            {
                log.LogMethodExit();
                return string.Empty;
            }
            if (value is string == false)
            {
                throw new Exception("Invalid value. please pass a string. value: " + value == null ? "null" : value.ToString());
            }
            if (value is string && string.IsNullOrWhiteSpace(value as string))
            {
                log.LogMethodExit();
                return string.Empty;
            }
            string returnValue = (value as string).Trim().ToString();
            log.LogMethodExit(returnValue);
            return returnValue;
        }

    }
}
