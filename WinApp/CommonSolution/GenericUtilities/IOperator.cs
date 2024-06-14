using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Sql Operators
    /// </summary>
    public interface IOperator
    {
        /// <summary>
        /// returns the query string
        /// </summary>
        /// <param name="criteriaParameters"></param>
        /// <returns></returns>
        string GetQueryString(Column column, List<CriteriaParameter> criteriaParameters);

        /// <summary>
        /// returns the query string
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        string GetDisplayString(params string[] values);

        /// <summary>
        /// returns the display name
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Whether can be used in the UI
        /// </summary>
        bool Browsable { get; }

    }
}
