/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - ComplexCriteria 
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
    /// Complex logical operator Criteria
    /// </summary>
    public abstract class ComplexCriteria : Criteria
    {
        /// <summary>
        /// child criteria list
        /// </summary>
        protected List<Criteria> criteriaList;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        /// <param name="sqlParameterNameProvider"></param>
        public ComplexCriteria(ColumnProvider columnProvider, SqlParameterNameProvider sqlParameterNameProvider) : base(columnProvider, sqlParameterNameProvider)
        {
            log.LogMethodEntry(columnProvider, sqlParameterNameProvider);
            criteriaList = new List<Criteria>();
            log.LogMethodExit();
        }

        /// <summary>
        /// returns the where clause
        /// </summary>
        /// <returns></returns>
        public override string GetWhereClause()
        {
            log.LogMethodEntry();
            string queryString = "( ";
            string seperator = "";
            string logicalOperator = " " + GetLogialOperator() + " ";
            foreach (var criteria in criteriaList)
            {
                queryString += seperator + criteria.GetWhereClause();
                seperator = logicalOperator;
            }
            queryString += " )";
            log.LogMethodExit(queryString);
            return queryString;
        }

        /// <summary>
        /// returns the criteria list
        /// </summary>
        /// <returns></returns>
        public List<Criteria> GetCriteriaList()
        {
            log.LogMethodEntry();
            log.LogMethodExit(criteriaList);
            return criteriaList;
        }

        /// <summary>
        /// Returns the sql parameters
        /// </summary>
        /// <returns></returns>
        public override List<CriteriaParameter> GetCriteriaParameters()
        {
            log.LogMethodEntry();
            List<CriteriaParameter> result = new List<CriteriaParameter>();
            foreach (Criteria criteria in criteriaList)
            {
                result.AddRange(criteria.GetCriteriaParameters());
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// returns the logical operator
        /// </summary>
        /// <returns></returns>
        public abstract string GetLogialOperator();

        /// <summary>
        /// Replaces the existing criteria with new criteria
        /// </summary>
        /// <param name="originalCriteria"></param>
        /// <param name="replaceCriteria"></param>
        public override void ReplaceCriteria(Criteria originalCriteria, Criteria replaceCriteria)
        {
            log.LogMethodEntry(originalCriteria, replaceCriteria);
            int index = -1;
            for (int i = 0; i < criteriaList.Count; i++)
            {
                if (criteriaList[i] == originalCriteria)
                {
                    index = i;
                }
            }
            if (index >= 0)
            {
                criteriaList.RemoveAt(index);
                criteriaList.Insert(index, replaceCriteria);
            }
            else
            {
                foreach (var criteriaItem in criteriaList)
                {
                    criteriaItem.ReplaceCriteria(originalCriteria, replaceCriteria);
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
            Criteria returnValue = null;
            if (criteriaList.Contains(criteria))
            {
                criteriaList.Remove(criteria);
            }
            else
            {
                foreach (var criteriaItem in criteriaList)
                {
                    Criteria replaceValue = criteriaItem.RemoveCriteria(criteria);
                    if (replaceValue != null)
                    {
                        ReplaceCriteria(criteriaItem, replaceValue);
                    }
                }
            }
            if (criteriaList.Count == 1)
            {
                returnValue = criteriaList[0];
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// Updates the sqlParameterNameProvider
        /// </summary>
        /// <param name="value"></param>
        internal override void UpdateSqlParameterNameProvider(SqlParameterNameProvider value)
        {
            log.LogMethodEntry(sqlParameterNameProvider);
            sqlParameterNameProvider = value;
            if (criteriaList != null)
            {
                foreach (Criteria criteria in criteriaList)
                {
                    criteria.UpdateSqlParameterNameProvider(value);
                }
            }
            log.LogMethodExit();
        }

    }
}
