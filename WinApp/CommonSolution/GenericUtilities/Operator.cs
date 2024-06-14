using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// applicable sql operators
    /// </summary>
    public enum Operator
    {
        /// <summary>
        /// Equal to operator
        /// </summary>
        EQUAL_TO,
        /// <summary>
        /// not equal operator
        /// </summary>
        NOT_EQUAL_TO,
        /// <summary>
        /// like operator
        /// </summary>
        LIKE,
        /// <summary>
        /// not like operator
        /// </summary>
        NOT_LIKE,
        /// <summary>
        /// greater than operator
        /// </summary>
        GREATER_THAN,
        /// <summary>
        /// lesser than operator
        /// </summary>
        LESSER_THAN,
        /// <summary>
        /// greater than equal to
        /// </summary>
        GREATER_THAN_OR_EQUAL_TO,
        /// <summary>
        /// lesser than equal to
        /// </summary>
        LESSER_THAN_OR_EQUAL_TO,
        /// <summary>
        /// between operator
        /// </summary>
        BETWEEN,
        /// <summary>
        /// in operator
        /// </summary>
        IN,
        /// <summary>
        /// is null operator
        /// </summary>
        IS_NULL,
        /// <summary>
        /// is not null operator
        /// </summary>
        IS_NOT_NULL
    }
}
