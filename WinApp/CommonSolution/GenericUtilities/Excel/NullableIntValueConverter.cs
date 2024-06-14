/********************************************************************************************
 * Project Name - NullableIntValueConverter
 * Description  - Data object of NullableIntValueConverter
 *  
 **************
 * Version Log
 **************
 * Version    Date             Modified By         Remarks          
 *********************************************************************************************
 *2.60.2      12-Jun-2019      Akshay Gulaganji    Code merge from Development to WebManagementStudio
 *2.70.2        10-Aug-2019      Girish kundar       Modified :Added Logger methods and Removed Unused namespace's.
 **********************************************************************************************/

namespace Semnox.Core.GenericUtilities.Excel
{
    /// <summary>
    /// Converts int value from/to string
    /// </summary>
    public class NullableIntValueConverter : IntValueConverter
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
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                object value = base.FromString(stringValue);
                log.LogMethodExit(value);
                return value;
            }
        }

        /// <summary>
        /// Converts int value to string
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
            string returnValue = base.ToString(value);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
