/********************************************************************************************
 * Project Name - CouponDetails
 * Description  - Coupon Details class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value 
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using Newtonsoft.Json;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// To handle coupon activities
    /// </summary>
    public class CouponDetails : IUserQuery
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;

        /// <summary>
        /// to hold API Url
        /// </summary>
        public string APIUrl
        {
            get;
            set;
        }

        /// <summary>
        /// to hold secret Id to calculate Signature
        /// </summary>
        public string SecretId
        {
            get;
            set;
        }

        /// <summary>
        /// to hold uuid for api url
        /// </summary>
        public string UUID
        {
            get;
            set;
        }

        /// <summary>
        /// coupons details
        /// </summary>
        /// <param name="_Utilities">utils to get all config values</param>
        public CouponDetails(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            APIUrl = Utilities.getParafaitDefaults("MERKLE_API_URL").ToString();
            SecretId = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MERKLE_API_SECRETID");//Utilities.getParafaitDefaults("MERKLE_API_SECRETID").ToString();
            UUID = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MERKLE_API_UUID");//Utilities.getParafaitDefaults("MERKLE_API_UUID").ToString();
            log.LogMethodExit();
        }

        /// <summary>
        /// To get Coupons details for the registered customer
        /// </summary>
        /// <param name="phoneNumber">customer phone number as a key to find customer</param>
        /// <param name="userCode">userCode is another key to finding customer</param>
        /// <returns>return customer dataTable, if not found returns null</returns>
        public DataTable QueryResult(string phoneNumber, string userCode)
        {
            log.LogMethodEntry(phoneNumber, userCode);
            DataTable couponsDT = new DataTable() ;
            Coupons couponsObj = new Coupons();
            MD5HashEncryption md5Encryption = new MD5HashEncryption(Utilities);
            string signature = string.Empty;
            string endPoint = string.Empty;
            string folderAppend = APIUrl.Substring(APIUrl.LastIndexOf("/"));

            if (!string.IsNullOrEmpty(userCode))
            {
                if (!string.IsNullOrEmpty(folderAppend))
                {
                    signature = md5Encryption.EncryptKey(SecretId + folderAppend + "/data/customer/coupons.json?uuid=" + UUID + "&secure_customer_id=" + userCode + "&reward_group=nbaplayzone");
                }
                else
                {
                    signature = md5Encryption.EncryptKey(SecretId + "/2016-12-01/data/customer/coupons.json?uuid=" + UUID + "&secure_customer_id=" + userCode + "&reward_group=nbaplayzone");
                }
                endPoint = APIUrl + "/data/customer/coupons.json?uuid=" + UUID + "&secure_customer_id=" + userCode + "&reward_group=nbaplayzone&sig=" + signature;
            }
            else if (!string.IsNullOrEmpty(phoneNumber))
            {
                if (!string.IsNullOrEmpty(folderAppend))
                {
                    signature = md5Encryption.EncryptKey(SecretId + folderAppend + "/data/customer/coupons.json?uuid=" + UUID + "&external_customer_id=" + phoneNumber + "&reward_group=nbaplayzone");
                }
                else
                {
                    signature = md5Encryption.EncryptKey(SecretId + "/2016-12-01/data/customer/coupons.json?uuid=" + UUID + "&external_customer_id=" + phoneNumber + "&reward_group=nbaplayzone");
                }
                endPoint = APIUrl + "/data/customer/coupons.json?uuid=" + UUID + "&external_customer_id=" + phoneNumber + "&reward_group=nbaplayzone&sig=" + signature;
            }

            var client = new RestClient(endPoint);
            var json = client.MakeRequest();
            couponsObj = JsonConvert.DeserializeObject<Coupons>(json);

            if (couponsObj != null)
            {
                couponsDT.Columns.AddRange(new DataColumn[3] { new DataColumn("code", typeof(string)), new DataColumn("expires_at", typeof(string)), new DataColumn("description", typeof(string)) });
                if (couponsObj.data.Count > 0)
                {
                    for (int i = 0; i < couponsObj.data.Count; i++)
                    {
                        if (couponsObj.data[i].code != null && (couponsObj.data[i].status == "redeemed" || couponsObj.data[i].status == "reissued"))
                        {
                            couponsDT.Rows.Add(couponsObj.data[i].code, DBNull.Value.Equals(couponsObj.data[i].expires_at) ? "" : couponsObj.data[i].expires_at, couponsObj.data[i].reward.description);
                        }
                    }
                }
            }

            if (!Convert.ToBoolean(couponsObj.success))
            {
                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "Coupons details ,API Response and Code: " + couponsObj.data[0].code + "Errro Message: " + couponsObj.data[0].message, "", "MerkleAPIIntegration", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            }
            log.LogMethodExit(couponsDT);
            return couponsDT;
        }

        /// <summary>
        /// To hold the customers coupons object
        /// </summary>
        public class Coupons
        {
            /// <summary>
            /// to hold customer property, it is defined as it is return json from merkle
            /// </summary>
            public List<Data> data { get; set; }
            /// <summary>
            /// to hold json return true or false
            /// </summary>
            public string success { get; set; }
        }

        /// <summary>
        /// An array of information for the reward associated with the coupon
        /// </summary>
        public class Reward
        {
            /// <summary>
            /// coupon code
            /// </summary>
            public string cost { get; set; }
            /// <summary>
            /// date/time of code creation
            /// </summary>
            public string description { get; set; }
            /// <summary>
            /// date/time code expires
            /// </summary>
            public string image_url { get; set; }
            /// <summary>
            /// date/time code expires
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// date/time of code invalidation
            /// </summary>
            public string points { get; set; }
            /// <summary>
            /// date/time of code invalidation
            /// </summary>
            public string redeem_instructions { get; set; }
            /// <summary>
            /// date/time of code invalidation
            /// </summary>
            public string reward_type { get; set; }
            /// <summary>
            /// date/time of code invalidation
            /// </summary>
            public string expiration_date { get; set; }
            /// <summary>
            /// date/time of code invalidation
            /// </summary>
            public string reward_id { get; set; }
        }

        /// <summary>
        /// to hold customer coupons details which defined as merkle json response
        /// </summary>
        public class Data
        {
            /// <summary>
            /// coupon code
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// date/time of code creation
            /// </summary>
            public string created_at { get; set; }
            /// <summary>
            /// date/time code expires
            /// </summary>
            public string expires_at { get; set; }
            /// <summary>
            /// date/time of code invalidation
            /// </summary>
            public string invalidated_at { get; set; }
            /// <summary>
            /// date/time of reward redemption, when the code was attached to the customer
            /// </summary>
            public string redeemed_at { get; set; }
            /// <summary>
            /// date/time the coupon becomes available for customer use
            /// </summary>
            public string start_at { get; set; }
            /// <summary>
            /// the time zone used for start_at
            /// </summary>
            public string start_at_time_zone { get; set; }
            /// <summary>
            /// the code has been assigned to a customer, but not yet used
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// date/time of code use, when the customer used the code to receive the discount / product
            /// </summary>
            public string used_at { get; set; }
            /// <summary>
            /// email of the customer attached to the coupon code
            /// </summary>
            public string email { get; set; }
            /// <summary>
            /// Cell phone number
            /// </summary>
            public string external_customer_id { get; set; }
            /// <summary>
            /// An array of information for the reward or deal associated with the coupon. If this is a reward, the array will be called "reward"; if it's a deal, the array will be called "deal". The contents of the array are the same as returned with a rewards or deals end point.
            /// </summary>
            public Reward reward { get; set; }
            /// <summary>
            /// error message 
            /// </summary>

            public string message { get; set; }

        }
    }
}
