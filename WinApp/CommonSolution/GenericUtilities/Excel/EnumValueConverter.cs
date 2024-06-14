/********************************************************************************************
 * Project Name - EnumValueConverter
 * Description  - Data object of EnumValueConverter
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
    /// Converts Enum value from/to string
    /// </summary>
    public class EnumValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Type enumType;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EnumValueConverter()
        {
            log.LogMethodEntry("Default constructor");
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="enumType"></param>
        public EnumValueConverter(Type enumType)
        {
            log.LogMethodEntry(enumType);
            this.enumType = enumType;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Enum value from string
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            object enumResult = Activator.CreateInstance(enumType);
            if (string.IsNullOrWhiteSpace(stringValue) == false)
            {
                try
                {
                    enumResult = Enum.Parse(enumType, stringValue.Trim(), true);
                }
                catch (Exception ex)
                {
                    log.Error("Conversion Failed",ex);
                    log.LogMethodExit(null, "Throwing Exception"+ex.Message);
                    throw new Exception("Unable to convert '" + (stringValue == null ? "null" : stringValue) + "' to an enum of type " + enumType.Name + ".", ex);
                }
            }
            log.LogMethodExit(enumResult);
            return enumResult;
        }

        /// <summary>
        /// Converts Enum value to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string result = string.Empty;
            if (value != null && value.GetType() == enumType)
            {
                result = value.ToString();
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Get/Set Methods for enumType field
        /// Used for xml serialization
        /// </summary>
        public string EnumTypeString
        {
            get
            {
                return enumType.FullName + ", " + enumType.Assembly.GetName().Name;
            }
            set
            {
                enumType = Type.GetType(value);
            }
        }
    }
}
