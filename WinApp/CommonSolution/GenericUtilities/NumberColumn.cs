/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - NumberColumn 
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
    /// int, float, number, decimal column
    /// </summary>
    public class NumberColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="browsable"></param>
        public NumberColumn(string name, string displayName = null, string defaultValue = null, bool browsable = true) : base(name, displayName, defaultValue, browsable)
        {
            log.LogMethodEntry(name, displayName, defaultValue, browsable);
            log.LogMethodExit();
        }
        /// <summary>
        /// returns the column type
        /// </summary>
        /// <returns>columnType</returns>
        public override ColumnType GetColumnType()
        {
            log.LogMethodEntry();
            ColumnType columnType = new ColumnType();
            columnType = ColumnType.NUMBER;
            log.LogMethodExit(columnType);
            return columnType;
        }
        /// <summary>
        /// returns the valid operators applicable operators on the column
        /// </summary>
        /// <returns>operators</returns>
        public override List<Operator> GetApplicableOperators()
        {
            log.LogMethodEntry();
            List<Operator> operators = new List<Operator>();
            operators = new List<Operator>() { Operator.BETWEEN, Operator.EQUAL_TO, Operator.GREATER_THAN, Operator.GREATER_THAN_OR_EQUAL_TO, Operator.IN, Operator.LESSER_THAN, Operator.LESSER_THAN_OR_EQUAL_TO, Operator.NOT_EQUAL_TO, Operator.IS_NULL, Operator.IS_NOT_NULL };
            log.LogMethodExit(operators);
            return operators;
        }
        /// <summary>
        /// returns the formatted parameter string
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>formattedValue</returns>
        protected override string GetFormatedString(object parameter)
        {
            log.LogMethodEntry(parameter);
            string formattedValue = string.Empty;
            if (parameter is int || parameter is long || parameter is float || parameter is double || parameter is decimal)
            {
                formattedValue = parameter.ToString();
            }
            log.LogMethodExit(formattedValue);
            return formattedValue;
        }
    }
}
