/********************************************************************************************
 * Project Name - NullableBooleanValueConverter
 * Description  - Data object of NullableBooleanValueConverter
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
    /// Converts nullable boolean value from/to string
    /// </summary>
    public class NullableBooleanValueConverter : BooleanValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public NullableBooleanValueConverter() : base()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the nullable boolean value from string
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
                object returnValue = base.FromString(stringValue);
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            
        }

        /// <summary>
        /// Converts nullable boolean value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            if (value == null)
            {
                log.LogMethodExit(string.Empty);
                return string.Empty;
            }
            string returnValue = base.ToString(value);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

    }
}
