/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - ValueCrieteria 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// computes comparision on value instead on column
    /// </summary>
    public class ValueCriteria : Criteria
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object leftValue;
        private readonly object rightValue;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        /// <param name="sqlParameterNameProvider"></param>
        /// <param name="leftValue"></param>
        /// <param name="operator"></param>
        /// <param name="rightValue"></param>
        public ValueCriteria(ColumnProvider columnProvider, SqlParameterNameProvider sqlParameterNameProvider, object leftValue, Operator @operator, object rightValue) : base(columnProvider, sqlParameterNameProvider)
        {
            log.LogMethodEntry(columnProvider, sqlParameterNameProvider, leftValue, @operator, rightValue);
            this.leftValue = leftValue;
            this.rightValue = rightValue;
            log.LogMethodExit();
        }
        /// <summary>
        /// returns the where clause
        /// </summary>
        /// <returns></returns>
        public override string GetWhereClause()
        {
            log.LogMethodEntry();
            BuildCriteriaParameters();
            string returnValue = criteriaParameters[0].Name + OperatorFactory.GetOperator(@operator).GetQueryString(this.column, new List<CriteriaParameter>(){criteriaParameters[1]});
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the sql parameters
        /// </summary>
        /// <returns></returns>
        protected override void BuildCriteriaParameters()
        {
            log.LogMethodEntry();
            if (criteriaParameters != null)
            {
                log.LogMethodExit();
                return;
            }

            criteriaParameters = new List<CriteriaParameter>
            {
                new CriteriaParameter(sqlParameterNameProvider.GetSqlParameterName("LEFT_VALUE_CRITERIA"), leftValue),
                new CriteriaParameter(sqlParameterNameProvider.GetSqlParameterName("RIGHT_VALUE_CRITERIA"), rightValue)
            };
        }
    }
}
