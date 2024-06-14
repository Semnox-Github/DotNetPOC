using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ProductPrice
{
    /// <summary>
    /// Different hierarchy of prices
    /// </summary>
    public class PriceType
    {

        /// <summary>
        /// Price based on the membership
        /// </summary>
        public const string BASE = "B";
        /// <summary>
        /// Price based on the membership
        /// </summary>
        public const string MEMBERSHIP = "M";

        /// <summary>
        /// Price based on the transaction profile
        /// </summary>
        public const string TRANSACTION_PROFILE = "T";

        /// <summary>
        /// Price based on the promotion
        /// </summary>
        public const string PROMOTION = "P";

        /// <summary>
        /// Price based on the promotion
        /// </summary>
        public const string USER_ROLE = "U";

        /// <summary>
        /// user overriden price
        /// </summary>
        public const string USER_OVERRIDEN = "O";

        /// <summary>
        /// system overriden price
        /// </summary>
        public const string SYSTEM_OVERRIDEN = "S";

    }
}
