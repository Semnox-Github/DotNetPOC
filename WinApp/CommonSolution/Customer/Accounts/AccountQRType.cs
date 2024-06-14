/******************************************************************************************************
 * Project Name - AccountQRType
 * Description  - Enum for QR Type. Has value Gameplay and Transaction
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ******************************************************************************************************
 *2.100.0     20-Nov-2020      Mathew Ninan   Created 
 ********************************************************************************************************/using System;

namespace Semnox.Parafait.Customer.Accounts
{
    /// <summary>
    /// AccountQRType enum defines value for QR type - Gameplay or Transaction
    /// </summary>
    public enum AccountQRType
    {
        /// <summary>
        /// Transaction represented as T
        /// </summary>
        TRANSACTION,
        /// <summary>
        /// Gameplay represented as G
        /// </summary>
        GAMEPLAY
    }

    /// <summary>
    /// Converts CreditPlusType from/to string
    /// </summary>
    public class AccountQRTypeConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Converts Account QR Type from string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AccountQRType FromString(string value)
        {
            log.LogMethodEntry("value");
            switch (value.ToUpper())
            {
                case "T":
                    {
                        return AccountQRType.TRANSACTION;
                    }
                case "G":
                    {
                        return AccountQRType.GAMEPLAY;
                    }
                default:
                    {
                        return AccountQRType.TRANSACTION;
                    }
            }
        }
        /// <summary>
        /// Converts Account QR Type to string
        /// </summary>
        /// <param name="value"></param>
        /// <returns>G or T</returns>
        public static string ToString(AccountQRType value)
        {
            log.LogMethodEntry("value");
            switch (value)
            {
                case AccountQRType.TRANSACTION:
                    {
                        return "T";
                    }
                case AccountQRType.GAMEPLAY:
                    {
                        return "G";
                    }
                default:
                    {
                        return "T";
                    }
            }
        }
    }
}
