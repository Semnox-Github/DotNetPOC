/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - EncryptedTextColumn 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// varchar, nvarchar, char column
    /// </summary>
    public class EncryptedTextColumn : TextColumn
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string hashName;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="browsable"></param>
        public EncryptedTextColumn(string name, string hashedName, string displayName = null, string defaultValue = null, bool browsable = true) : base(name, displayName, defaultValue, browsable)
        {
            log.LogMethodEntry(name, hashedName, displayName, defaultValue, browsable);
            this.hashName = hashedName;
            log.LogMethodExit();
        }

        /// <summary>
        /// returns the query strings
        /// </summary>
        /// <returns></returns>
        public override string GetQueryString(Operator @operator)
        {
            log.LogMethodEntry();
            string returnValue = string.Empty;
            if (@operator != Operator.EQUAL_TO && @operator != Operator.NOT_EQUAL_TO && @operator != Operator.IN)
            {
                log.Info("calling base class");
                returnValue = base.GetQueryString(@operator);
            }
            else
            {
                if (defaultValue != null)
                {
                    returnValue = "Isnull(" + hashName + ", " + defaultValue + ")";
                }
                else
                {
                    returnValue = hashName;
                }
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

    }
}
