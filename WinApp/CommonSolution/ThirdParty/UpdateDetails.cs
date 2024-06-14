/********************************************************************************************
 * Project Name - UpdateDetails
 * Description  - Update Details class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value 
 *2.70.2        16-Aug-2019       Deeksha        Added Logger Methods.
 ********************************************************************************************/
using Newtonsoft.Json;
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.JobUtils;

namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// Update details into merkle server
    /// </summary>
    public class UpdateDetails : IUpdateDetails
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
        /// constructs to initialize the variables
        /// </summary>
        public UpdateDetails(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            APIUrl = Utilities.getParafaitDefaults("MERKLE_API_URL");
            SecretId = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MERKLE_API_SECRETID");//Utilities.getParafaitDefaults("MERKLE_API_SECRETID");
            UUID = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MERKLE_API_UUID");// Utilities.getParafaitDefaults("MERKLE_API_UUID");
            log.LogMethodExit();
        }

        /// <summary>
        /// Update coupons status into merkle
        /// </summary>
        /// <param name="status">input status parameter is used-Used coupon, reissued - for reversetransaction</param>
        /// <param name="couponCode">coupon code to be update</param>
        /// <returns>returns the true on update success, false on failure</returns>
        public bool Update(string couponCode, string status)
        {
            log.LogMethodEntry(couponCode, status);
            bool updateStatus = false;
            UpdateCoupon updateResponseObj = new UpdateCoupon();
            MD5HashEncryption md5Encryption = new MD5HashEncryption(Utilities);
            string folderAppend = APIUrl.Substring(APIUrl.LastIndexOf("/"));
            string signature = string.Empty;

            if (!string.IsNullOrEmpty(folderAppend))
                signature = md5Encryption.EncryptKey(SecretId + folderAppend + "/data/coupon/update.json?uuid=" + UUID + "&code=" + couponCode + "&status=" + status);
            else
                signature = md5Encryption.EncryptKey(SecretId + "/data/coupon/update.json?uuid=" + UUID + "&code=" + couponCode + "&status=" + status);

            string endPoint = APIUrl + "/data/coupon/update.json?uuid=" + UUID + "&code=" + couponCode + "&status=" + status + "&sig=" + signature;

            var client = new RestClient(endPoint);
            var json = client.MakeRequest();
            updateResponseObj = JsonConvert.DeserializeObject<UpdateCoupon>(json);

            if (updateResponseObj != null)
            {
                updateStatus = Convert.ToBoolean(updateResponseObj.success);
            }
            if (!Convert.ToBoolean(updateResponseObj.success))
            {
                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "Update Coupon details ,API Response and Code: " + updateResponseObj.data.code + "Errro Message: " + updateResponseObj.data.message, "", "MerkleAPIIntegration", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            }
            log.LogMethodExit(updateStatus);
            return updateStatus;
        }
        /// <summary>
        /// To hold the customer object
        /// </summary>
        public class UpdateCoupon
        {
            /// <summary>
            /// to hold customer property, it is defined as it is return json from merkle
            /// </summary>
            public Data data { get; set; }
            /// <summary>
            /// to hold json return true or false
            /// </summary>
            public string success { get; set; }
        }

        /// <summary>
        /// to hold customer details which defined as merkle json response
        /// </summary>
        public class Data
        {
            /// <summary>
            /// error code 
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// error message
            /// </summary>
            public string message { get; set; }
            
        }
    }
}
