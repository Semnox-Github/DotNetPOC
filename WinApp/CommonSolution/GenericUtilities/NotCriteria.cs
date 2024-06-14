/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - NoCriteria 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/

using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// complex criteria with logical not operator
    /// </summary>
    public class NotCriteria : Criteria
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Criteria criteria;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        /// <param name="sqlParameterNameProvider"></param>
        /// <param name="criteria"></param>
        public NotCriteria(ColumnProvider columnProvider, SqlParameterNameProvider sqlParameterNameProvider, Criteria criteria) : base(columnProvider, sqlParameterNameProvider)
        {
            log.LogMethodEntry(columnProvider, sqlParameterNameProvider,  criteria);
            this.criteria = criteria;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates a new complex criteria with logical not operator
        /// </summary>
        /// <returns></returns>
        public override Criteria Not()
        {
            log.LogMethodEntry();
            log.LogMethodExit(criteria);
            return criteria;
        }
        /// <summary>
        /// returns the where clause
        /// </summary>
        /// <returns></returns>
        public override string GetWhereClause()
        {
            log.LogMethodEntry();
            string returnvalue = " NOT( " + criteria.GetWhereClause() + ") ";
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        /// <summary>
        /// return the inner criteria
        /// </summary>
        /// <returns></returns>
        public Criteria GetCriteria()
        {
            log.LogMethodEntry();
            log.LogMethodExit(criteria);
            return criteria;
        }

        /// <summary>
        /// Returns the sql parameters
        /// </summary>
        /// <returns></returns>
        public override List<CriteriaParameter> GetCriteriaParameters()
        {
            log.LogMethodEntry();
            List<CriteriaParameter> result = criteria == null ? new List<CriteriaParameter>() : criteria.GetCriteriaParameters();
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Updates the sqlParameterNameProvider
        /// </summary>
        /// <param name="value"></param>
        internal override void UpdateSqlParameterNameProvider(SqlParameterNameProvider value)
        {
            log.LogMethodEntry(sqlParameterNameProvider);
            sqlParameterNameProvider = value;
            if (criteria != null)
            {
                criteria.UpdateSqlParameterNameProvider(value);
            }
            log.LogMethodExit();
        }
    }
}
