using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Interface defined to do all data query transaction
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Property to hold filename
        /// </summary>
        string FileName { get; set; }
        /// <summary>
        /// Executes a query and return the result.
        /// </summary>
        /// <returns>Returns datatable with result set</returns>
        DataTable GetQueryResult();
    }
}
