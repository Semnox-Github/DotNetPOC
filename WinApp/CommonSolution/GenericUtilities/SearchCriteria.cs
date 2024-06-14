/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - Searchcriteria 
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019            Deeksha        Added logger methods.
 *2.100.0        28-Oct-2020            Mushahid Faizan   Added GetAndQuery method.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Serach criteria base class 
    /// </summary>
    public class SearchCriteria : Criteria
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Criteria criteria;
        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        protected SearchCriteria(ColumnProvider columnProvider) : base(columnProvider, new SqlParameterNameProvider())
        {
            log.LogMethodEntry(columnProvider);
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        /// <param name="columnIdentifier"></param>
        /// <param name="operator"></param>
        /// <param name="parameters"></param>
        protected SearchCriteria(ColumnProvider columnProvider, Enum columnIdentifier, Operator @operator, params object[] parameters)
            : base(columnProvider, new SqlParameterNameProvider())
        {
            log.LogMethodEntry(columnProvider, columnIdentifier, @operator, parameters);
            criteria = new Criteria(columnProvider, sqlParameterNameProvider, columnIdentifier, @operator, parameters);
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates a new complex criteria with logical and operator
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public override Criteria And(Criteria criteria)
        {
            log.LogMethodEntry(criteria);
            criteria.UpdateSqlParameterNameProvider(sqlParameterNameProvider);
            if (ContainsCondition)
            {
                this.criteria = this.criteria.And(criteria);
            }
            else
            {
                this.criteria = criteria;
            }
            log.LogMethodExit();
            return this;
        }
        /// <summary>
        /// returns the query. contains where, orderby and pagination clauses
        /// </summary>
        /// <returns></returns>
        public override string GetQuery()
        {
            log.LogMethodEntry();
            string returnValue = (criteria != null ? " WHERE " + criteria.GetWhereClause() : "") + GetOrderByClause() + GetPaginationClause();
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// returns the query. contains where, orderby and pagination clauses
        /// </summary>
        /// <returns></returns>
        public string GetAndQuery()
        {
            log.LogMethodEntry();
            string returnValue = (criteria != null ? " AND " + criteria.GetWhereClause() : "") + GetOrderByClause() + GetPaginationClause();
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        /// <summary>
        /// returns the where clause
        /// </summary>
        /// <returns></returns>
        public override string GetWhereClause()
        {
            log.LogMethodEntry();
            string returnValue = (criteria != null ? criteria.GetWhereClause() : "");
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Returns the sql parameters
        /// </summary>
        /// <returns></returns>
        public override List<CriteriaParameter> GetCriteriaParameters()
        {
            log.LogMethodEntry();
            List<CriteriaParameter> returnValue = (criteria != null ? criteria.GetCriteriaParameters() : new List<CriteriaParameter>());
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Creates a new complex criteria with logical not operator
        /// </summary>
        /// <returns></returns>
        public override Criteria Not()
        {
            log.LogMethodEntry();
            if (ContainsCondition)
            {
                this.criteria = this.criteria.Not();
            }
            log.LogMethodExit();
            return this;
        }

        /// <summary>
        /// Creates a new complex criteria with logical or operator
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public override Criteria Or(Criteria criteria)
        {
            log.LogMethodEntry(criteria);
            criteria.UpdateSqlParameterNameProvider(sqlParameterNameProvider);
            if (ContainsCondition)
            {
                this.criteria = this.criteria.Or(criteria);

            }
            else
            {
                this.criteria = criteria;
            }
            log.LogMethodExit();
            return this;
        }

        /// <summary>
        /// returns the search criteria contains any conditions
        /// </summary>
        public bool ContainsCondition
        {
            get
            {
                return criteria != null;
            }
        }

        /// <summary>
        /// returns the criteria
        /// </summary>
        /// <returns></returns>
        public Criteria GetCriteria()
        {
            log.LogMethodEntry();
            log.LogMethodExit(criteria);
            return criteria;
        }

        /// <summary>
        /// Returns the column provider
        /// </summary>
        /// <returns></returns>
        public ColumnProvider GetColumnProvider()
        {
            log.LogMethodEntry();
            log.LogMethodExit(columnProvider);
            return columnProvider;
        }

        /// <summary>
        /// Replaces the existing criteria with new criteria
        /// </summary>
        /// <param name="originalCriteria"></param>
        /// <param name="replaceCriteria"></param>
        public override void ReplaceCriteria(Criteria originalCriteria, Criteria replaceCriteria)
        {
            log.LogMethodEntry(originalCriteria, replaceCriteria);
            if (criteria != null)
            {
                if (criteria == originalCriteria)
                {
                    criteria = replaceCriteria;
                }
                else
                {
                    criteria.ReplaceCriteria(originalCriteria, replaceCriteria);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// removes the existing criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public override Criteria RemoveCriteria(Criteria criteria)
        {
            log.LogMethodEntry(criteria);
            if (this.criteria != null)
            {
                if (this.criteria == criteria)
                {
                    this.criteria = null;
                }
                else
                {
                    Criteria replaceValue = this.criteria.RemoveCriteria(criteria);
                    if (replaceValue != null)
                    {
                        this.criteria = replaceValue;
                    }
                }
            }
            log.LogMethodExit();
            return null;
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
