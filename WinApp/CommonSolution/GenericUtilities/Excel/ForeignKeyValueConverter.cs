/********************************************************************************************
 * Project Name - ForeignKeyValueConverter
 * Description  - Data object of ForeignKeyValueConverter
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
    /// Converts Foreignkey value from/to string
    /// </summary>
    public class ForeignKeyValueConverter : IntValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        ///  Returns the int value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            object result = -1;
            if (string.IsNullOrWhiteSpace(stringValue) == false)
            {
                result = base.FromString(stringValue);
            }
            log.LogMethodExit(result);
            return result;
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
            if (!((value is int) && (int)value < 0))
            {
                result = base.ToString(value);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
