/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of CustomerTypeConverter      s
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Converts CustomerType from/to string
    /// </summary>
    public class CustomerTypeConverter  : ValueConverter
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts CustomerType from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CustomerType FromStringValue(string value)
        {
            log.LogMethodEntry("value");
            switch(value.ToUpper())
            {
                case "R":
                    {
                        return CustomerType.REGISTERED;
                    }
                case "U":
                    {
                        return CustomerType.UNREGISTERED;
                    }
                default:
                    {
                        return CustomerType.REGISTERED;
                    }
            }
        }
        /// <summary>
        /// Converts CustomerType to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToString(CustomerType value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case CustomerType.REGISTERED:
                    {
                        return "R";
                    }
                case CustomerType.UNREGISTERED:
                    {
                        return "U";
                    }
                default:
                    {
                        log.Error("Error :Not a valid customer type ");
                        log.LogMethodExit(null, "Throwing Exception");
                        throw new ArgumentException("Not a valid customer type");
                    }
            }
        }

        public override object FromString(string stringValue)
        {
            return FromStringValue(stringValue);
        }

        public override string ToString(object value)
        {
            if (value is CustomerType == false)
            {
                return string.Empty;
            }
            return ToString((CustomerType)value);
        }
    }
}
