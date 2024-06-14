using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// To handle update coupon details activities
    /// </summary>
    public interface IUpdateDetails
    {
        /// <summary>
        /// Merkle API Url
        /// </summary>
        string APIUrl { get; set; }

        /// <summary>
        /// Merkle server UUID to connect
        /// </summary>
        string UUID { get; set; }
        /// <summary>
        /// Merkle SecretId code to connect URL
        /// </summary>
        string SecretId { get; set; }
        /// <summary>
        /// To update the coupon used status
        /// </summary>
        /// <param name="status">parameters, status code</param>
        /// <param name="couponCode">parameters, coupon code</param>
        /// <returns>returns true on success update, false on failure</returns>
        bool Update(string couponCode, string status);
    }

}
