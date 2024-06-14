/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - DateTimeColumn 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Datetime column
    /// </summary>
    public class DateTimeColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="browsable"></param>
        public DateTimeColumn(string name, string displayName = null, string defaultValue = null, bool browsable = true) 
            : base(name, displayName, defaultValue, browsable)
        {
            log.LogMethodEntry(name, displayName, defaultValue, browsable);
            log.LogMethodExit();
        }
        /// <summary>
        /// returns the column type
        /// </summary>
        /// <returns></returns>
        public override ColumnType GetColumnType()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return ColumnType.DATE_TIME;
        }
        /// <summary>
        /// returns the valid operators applicable operators on the column
        /// </summary>
        /// <returns></returns>
        public override List<Operator> GetApplicableOperators()
        {
            log.LogMethodEntry();
            List<Operator> returnValue= new List<Operator>() { Operator.BETWEEN, Operator.EQUAL_TO, Operator.GREATER_THAN, Operator.GREATER_THAN_OR_EQUAL_TO, Operator.IN, Operator.LESSER_THAN, Operator.LESSER_THAN_OR_EQUAL_TO, Operator.NOT_EQUAL_TO, Operator.IS_NOT_NULL, Operator.IS_NULL };
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// returns the formatted parameter string
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override string GetFormatedString(object parameter)
        {
            log.LogMethodEntry(parameter);
            string formattedValue = string.Empty;
            if (parameter is DateTime)
            {
                formattedValue = "'" + ((DateTime)parameter).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            log.LogMethodExit(formattedValue);
            return formattedValue;
        }
    }
}
