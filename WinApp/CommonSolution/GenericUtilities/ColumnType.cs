using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// Type of db columns
    /// </summary>
    public enum ColumnType
    {
        /// <summary>
        /// DateTime Column
        /// </summary>
        DATE_TIME,
        /// <summary>
        /// int, float, number and decimal column
        /// </summary>
        NUMBER,
        /// <summary>
        /// char, nvarchar, varchar column
        /// </summary>
        TEXT,
        /// <summary>
        /// bit column
        /// </summary>
        BIT,
        /// <summary>
        /// unique identifier column
        /// </summary>
        UNIQUE_IDENTIFIER
    }
}
