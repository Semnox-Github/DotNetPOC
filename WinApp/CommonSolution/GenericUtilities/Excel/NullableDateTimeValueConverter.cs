/********************************************************************************************
 * Project Name - NullableDateTimeValueConverter
 * Description  - Data object of NullableDateTimeValueConverter
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
    /// Converts DateTime value from/to string
    /// </summary>
    public class NullableDateTimeValueConverter : DateTimeValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="format"></param>
        public NullableDateTimeValueConverter(string format) : base(format)
        {
            log.LogMethodEntry(format);
            log.LogMethodExit();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public NullableDateTimeValueConverter() : base()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the datetime value from string
        /// </summary>
        /// <param name="stringValue"></param>
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
        /// Converts datetime value to string
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
