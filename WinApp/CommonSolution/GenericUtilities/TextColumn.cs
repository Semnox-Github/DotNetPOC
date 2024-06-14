/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - TextColumn 
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
    public class TextColumn : Column
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="browsable"></param>
        public TextColumn(string name, string displayName = null, string defaultValue = null, bool browsable = true) : base(name, displayName, defaultValue, browsable)
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
            ColumnType columnType = ColumnType.TEXT;
            log.LogMethodExit(columnType);
            return columnType;
        }
        /// <summary>
        /// returns the valid operators applicable operators on the column
        /// </summary>
        /// <returns></returns>
        public override List<Operator> GetApplicableOperators()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            List<Operator> operators = new List<Operator>();
            operators = new List<Operator>() { Operator.BETWEEN, Operator.LIKE, Operator.NOT_LIKE, Operator.IS_NOT_NULL, Operator.IS_NULL,Operator.EQUAL_TO };
            log.LogMethodExit(operators);
            return operators;
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
            if (parameter is string)
            {
                formattedValue = "N'" + parameter + "'";
            }
            log.LogMethodExit(formattedValue);
            return formattedValue;
        }
    }
}
