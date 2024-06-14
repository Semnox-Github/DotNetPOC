using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// To handle all Customer details and Customer coupon details activities
    /// </summary>
    public interface IUserQuery
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
        /// To Get customer details and Coupons details by calling Merkle API's
        /// </summary>
        /// <param name="phoneNumber">customer phone number passes to API, to get details</param>
        /// <param name="userCode">QRCode or userCode is also another key to find the customers</param>
        /// <returns>Returns the customer details or Coupons details based the calls</returns>
        DataTable QueryResult(string phoneNumber, string userCode);
    }
}
