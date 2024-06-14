/********************************************************************************************
 * Project Name - Country Params Programs 
 * Description  - Criteria
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        09-Aug-2019     Deeksha            Added logger Methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Criteria class
    /// </summary>
    public class Criteria
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// column provider
        /// </summary>
        protected readonly ColumnProvider columnProvider;

        /// <summary>
        /// sql parameter name provider
        /// </summary>
        protected SqlParameterNameProvider sqlParameterNameProvider;
        /// <summary>
        /// dbcolumn
        /// </summary>
        protected readonly Column column;

        /// <summary>
        /// column identifier
        /// </summary>
        protected readonly Enum columnIdentifier;

        /// <summary>
        /// operator
        /// </summary>
        protected readonly Operator @operator;
        /// <summary>
        /// parameters to the criteria
        /// </summary>
        protected readonly object[] parameters;

        /// <summary>
        /// sql parameters
        /// </summary>
        protected List<CriteriaParameter> criteriaParameters;

        /// <summary>
        /// orderby conditions
        /// </summary>
        protected List<OrderBy> orderByList;
        /// <summary>
        /// pagination condition
        /// </summary>
        protected Page page;

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        /// <param name="sqlParameterNameProvider"></param>
        protected Criteria(ColumnProvider columnProvider, SqlParameterNameProvider sqlParameterNameProvider)
        {
            log.LogMethodEntry(columnProvider);
            this.columnProvider = columnProvider;
            this.sqlParameterNameProvider = sqlParameterNameProvider;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor
        /// </summary>
        /// <param name="columnProvider"></param>
        /// <param name="sqlParameterNameProvider"></param>
        /// <param name="columnIdentifier"></param>
        /// <param name="operator"></param>
        /// <param name="parameters"></param>
        public Criteria(ColumnProvider columnProvider, SqlParameterNameProvider sqlParameterNameProvider, Enum columnIdentifier, Operator @operator, params object[] parameters)
        {
            log.LogMethodEntry(columnProvider, sqlParameterNameProvider, columnIdentifier, @operator, parameters);
            this.columnProvider = columnProvider;
            this.sqlParameterNameProvider = sqlParameterNameProvider;
            this.column = columnProvider.GetColumn(columnIdentifier);
            this.columnIdentifier = columnIdentifier;
            this.@operator = @operator;
            this.parameters = parameters;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates a new complex criteria with logical and operator
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual Criteria And(Criteria criteria)
        {
            log.LogMethodEntry(criteria);
            log.LogMethodExit();
            return new AndCriteria(columnProvider, sqlParameterNameProvider, this, criteria);
        }
        /// <summary>
        /// Creates a new complex criteria with logical and operator
        /// </summary>
        /// <param name="columnIdentifier"></param>
        /// <param name="operator"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Criteria And(Enum columnIdentifier, Operator @operator, params object[] parameters)
        {
            log.LogMethodEntry(columnIdentifier, @operator, parameters);
            log.LogMethodExit();
            return And(new Criteria(columnProvider, sqlParameterNameProvider, columnIdentifier, @operator, parameters));
        }
        /// <summary>
        /// Creates a new complex criteria with logical and operator
        /// </summary>
        /// <param name="leftValue"></param>
        /// <param name="operator"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        public virtual Criteria And(object leftValue, Operator @operator, object rightValue)
        {
            log.LogMethodEntry(leftValue, @operator, rightValue);
            log.LogMethodExit();
            return And(new ValueCriteria(columnProvider, sqlParameterNameProvider, leftValue, @operator, rightValue));
        }
        /// <summary>
        /// Creates a new complex criteria with logical or operator
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual Criteria Or(Criteria criteria)
        {
            log.LogMethodEntry(criteria);
            log.LogMethodExit();
            return new OrCriteria(columnProvider, sqlParameterNameProvider, this, criteria);
        }
        /// <summary>
        /// Creates a new complex criteria with logical or operator
        /// </summary>
        /// <param name="columnIdentifier"></param>
        /// <param name="operator"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual Criteria Or(Enum columnIdentifier, Operator @operator, params object[] parameters)
        {
            log.LogMethodEntry(columnIdentifier, @operator, parameters);
            log.LogMethodExit();
            return Or(new Criteria(columnProvider, sqlParameterNameProvider, columnIdentifier, @operator, parameters));
        }
        /// <summary>
        /// Creates a new complex criteria with logical or operator
        /// </summary>
        /// <param name="leftValue"></param>
        /// <param name="operator"></param>
        /// <param name="rightValue"></param>
        /// <returns></returns>
        public virtual Criteria Or(object leftValue, Operator @operator, object rightValue)
        {
            log.LogMethodEntry(leftValue, @operator, rightValue);
            log.LogMethodExit();
            return Or(new ValueCriteria(columnProvider, sqlParameterNameProvider, leftValue, @operator, rightValue));
        }
        /// <summary>
        /// Creates a new complex criteria with logical not operator
        /// </summary>
        /// <returns></returns>
        public virtual Criteria Not()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
            return new NotCriteria(columnProvider, sqlParameterNameProvider, this);
        }
        /// <summary>
        /// adds pagination infomration to the query
        /// </summary>
        /// <param name="columnIdentifier"></param>
        /// <param name="orderByType"></param>
        /// <returns></returns>
        public Criteria OrderBy(Enum columnIdentifier, OrderByType orderByType = OrderByType.ASC)
        {
            log.LogMethodEntry(columnIdentifier, orderByType);
            if (orderByList == null)
            {
                orderByList = new List<OrderBy>();
            }
            if (orderByList.FirstOrDefault((x) => x.ColumnIdentifier.Equals(columnIdentifier)) == null)
            {
                orderByList.Add(new OrderBy(columnProvider, columnIdentifier, orderByType));
            }
            log.LogMethodExit();
            return this;
        }
        /// <summary>
        /// returns the where clause
        /// </summary>
        /// <returns></returns>
        public virtual string GetWhereClause()
        {
            log.LogMethodEntry();
            if (column != null)
            {
                log.LogMethodExit();
                string returnValue = column.GetQueryString(@operator) + OperatorFactory.GetOperator(@operator).GetQueryString(column, GetCriteriaParameters());
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                log.LogMethodExit();
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the sql parameters
        /// </summary>
        /// <returns></returns>
        protected virtual void BuildCriteriaParameters()
        {
            log.LogMethodEntry();
            if (criteriaParameters != null)
            {
                log.LogMethodExit();
                return;
            }
            if (parameters == null)
            {
                criteriaParameters = new List<CriteriaParameter>();
                return;
            }

            object[] parameterArray;
            if (parameters.Length == 0 && parameters[0] is Array)
            {
                parameterArray = parameters[0] as object[];
            }
            else
            {
                parameterArray = parameters;
            }
            criteriaParameters = new List<CriteriaParameter>();
            if (parameterArray != null)
            {
                foreach (object parameter in parameterArray)
                {
                    criteriaParameters.Add(new CriteriaParameter(sqlParameterNameProvider.GetSqlParameterName(columnIdentifier.ToString()), parameter));
                }
            }
        }

        /// <summary>
        /// Returns the sql parameters
        /// </summary>
        /// <returns></returns>
        public virtual List<CriteriaParameter> GetCriteriaParameters()
        {
            log.LogMethodEntry();
            BuildCriteriaParameters();
            log.LogMethodExit(criteriaParameters);
            return criteriaParameters;
        }

        /// <summary>
        /// Returns the sql parameters
        /// </summary>
        /// <returns></returns>
        public List<SqlParameter> GetSqlParameters()
        {
            log.LogMethodEntry();
            List<CriteriaParameter> criteriaParameterList = GetCriteriaParameters();
            List<SqlParameter> returnValue = new List<SqlParameter>();
            foreach (CriteriaParameter parameter in criteriaParameterList)
            {
                returnValue.Add(new SqlParameter(parameter.Name, parameter.Value == null ? DBNull.Value : parameter.Value));
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        /// <summary>
        /// returns the where clause
        /// </summary>
        /// <returns></returns>
        public virtual string GetDisplayClause()
        {
            log.LogMethodEntry();
            if (column != null)
            {
                string returnValue = column.DisplayName + OperatorFactory.GetOperator(@operator).GetDisplayString(column.GetFormatedString(parameters));
                log.LogMethodExit(returnValue);
                return returnValue;
            }
            else
            {
                log.LogMethodExit();
                return string.Empty;
            }
        }
        /// <summary>
        /// returns the order by clause
        /// </summary>
        /// <returns></returns>
        public string GetOrderByClause()
        {
            log.LogMethodEntry();
            string orderByString = string.Empty;
            if (orderByList != null && orderByList.Count > 0)
            {
                orderByString = " ORDER BY";
                string seperator = "";
                foreach (var orderBy in orderByList)
                {
                    orderByString += seperator + orderBy.ToString();
                    seperator = ",";
                }
            }
            log.LogMethodExit(orderByString);
            return orderByString;
        }
        /// <summary>
        /// returns the pagination clause
        /// </summary>
        /// <returns></returns>
        public string GetPaginationClause()
        {
            log.LogMethodEntry();
            string paginationString = string.Empty;
            if (page != null)
            {
                paginationString = page.ToString();
            }
            log.LogMethodExit(paginationString);
            return paginationString;
        }
        /// <summary>
        /// returns the query. contains where, orderby and pagination clauses
        /// </summary>
        /// <returns></returns>
        public virtual string GetQuery()
        {
            log.LogMethodEntry();
            string queryString = " WHERE ";
            queryString += GetWhereClause() + GetOrderByClause() + GetPaginationClause();
            log.LogMethodExit(queryString);
            return queryString;
        }
        /// <summary>
        /// adds pagination infomration to the query
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Criteria Paginate(int pageNumber, int pageSize)
        {
            log.LogMethodEntry(pageNumber, pageSize);
            if (orderByList != null && orderByList.Count > 0)
            {
                page = new Page(pageNumber, pageSize);
            }
            else
            {
                throw new Exception("Order by clause is mandatory for pagination");
            }
            log.LogMethodExit(this);
            return this;
        }

        /// <summary>
        /// Replaces the existing criteria with new criteria
        /// </summary>
        /// <param name="originalCriteria"></param>
        /// <param name="replaceCriteria"></param>
        public virtual void ReplaceCriteria(Criteria originalCriteria, Criteria replaceCriteria)
        {
            log.LogMethodEntry(originalCriteria, replaceCriteria);
            log.LogMethodExit();
        }

        /// <summary>
        /// removes the existing criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual Criteria RemoveCriteria(Criteria criteria)
        {
            log.LogMethodEntry(criteria);
            log.LogMethodExit();
            return null;
        }

        /// <summary>
        /// Updates the sqlParameterNameProvider
        /// </summary>
        /// <param name="value"></param>
        internal virtual void UpdateSqlParameterNameProvider(SqlParameterNameProvider value)
        {
            log.LogMethodEntry(sqlParameterNameProvider);
            sqlParameterNameProvider = value;
            log.LogMethodExit();
        }
    }
}
