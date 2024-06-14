/********************************************************************************************
 * Project Name - CustomerDetails
 * Description  - Customer Details class
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
using Newtonsoft.Json.Linq;
using Semnox.Core.Utilities;
using System;
using System.Data;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// To handle API calls for getting customer details
    /// </summary>
    public class CustomerDetails : IUserQuery
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities Utilities;
        ///// <summary>
        ///// to hold api information
        ///// </summary>
        //private string apiUrl;
        ///// <summary>
        ///// to hold secret id 
        ///// </summary>
        //private string secretId;
        ///// <summary>
        ///// to hold uuid for the api connection
        ///// </summary>
      //  private string uuid;
        
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
        /// customer details
        /// </summary>
        /// <param name="_Utilities">utils to get all config values</param>
        public CustomerDetails(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            Utilities = _Utilities;
            APIUrl = Utilities.getParafaitDefaults("MERKLE_API_URL").ToString();
            SecretId = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MERKLE_API_SECRETID");//Utilities.getParafaitDefaults("MERKLE_API_SECRETID").ToString();
            UUID = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "MERKLE_API_UUID");//Utilities.getParafaitDefaults("MERKLE_API_UUID").ToString();
            log.LogMethodExit();
        }

        /// <summary>
        /// To get customer details
        /// </summary>
        /// <param name="phoneNumber">customer phone number as a key to find customer</param>
        /// <param name="userCode">userCode is another key to finding customer</param>
        /// <returns>return customer dataTable, if not found returns null</returns>
        public DataTable QueryResult(string phoneNumber, string userCode)
        {
            log.LogMethodEntry(phoneNumber, userCode);
            DataTable custDt= new DataTable();
            Customer custObj = new Customer();
            
            Parser jsonParse = new Parser();
            MD5HashEncryption md5Encryption = new MD5HashEncryption(Utilities);
            string signature = string.Empty;
            string endPoint = string.Empty;
            string folderAppend = APIUrl.Substring(APIUrl.LastIndexOf("/"));

            if (!string.IsNullOrEmpty(userCode))
            {
                if (!string.IsNullOrEmpty(folderAppend))
                {
                    signature = md5Encryption.EncryptKey(SecretId + folderAppend + "/data/customer/show.json?uuid=" + UUID + "&secure_customer_id=" + userCode + "&include=detail");
                    //signature = md5Encryption.EncryptKey(SecretId + folderAppend + "/data/customer/show.json?uuid=" + UUID + "&secure_customer_id=" + userCode + "&include=detail");
                }
                else
                    signature = md5Encryption.EncryptKey(SecretId + "/2016-12-01/data/customer/show.json?uuid=" + UUID + "&secure_customer_id=" + userCode + "&include=detail");

                endPoint = APIUrl + "/data/customer/show.json?uuid=" + UUID + "&secure_customer_id=" + userCode + "&include=detail&sig=" + signature;
            }
            else if (!string.IsNullOrEmpty(phoneNumber))
            {
                if (!string.IsNullOrEmpty(folderAppend))
                    signature = md5Encryption.EncryptKey(SecretId + folderAppend + "/data/customer/show.json?uuid=" + UUID + "&external_customer_id=" + phoneNumber + "&include=detail");
                else
                    signature = md5Encryption.EncryptKey(SecretId + "/2016-12-01/data/customer/show.json?uuid=" + UUID + "&external_customer_id=" + phoneNumber + "&include=detail");

                endPoint = APIUrl + "/data/customer/show.json?uuid=" + UUID + "&external_customer_id=" + phoneNumber + "&include=detail&sig=" + signature;
            }

            var client = new RestClient(endPoint);
            var json = client.MakeRequest();
            Object jObject = JsonConvert.DeserializeObject<JObject>(json);
            custObj = (Customer)JsonConvert.DeserializeObject(jObject.ToString(), (typeof(Customer)));
            custDt = jsonParse.ConvertToDataTable(custObj.data);

            if(!Convert.ToBoolean(custObj.success))
            {
                Utilities.EventLog.logEvent("ParafaitDataTransfer", 'I', "customer details ,API Response and Code: " + custObj.data.code + "Errro Message: " + custObj.data.message, "", "MerkleAPIIntegration", 1, "", "", Utilities.ParafaitEnv.LoginID, Utilities.ParafaitEnv.POSMachine, null);
            }
            log.LogMethodExit(custDt);
            return custDt; 
        }

        /// <summary>
        /// To hold the customer object
        /// </summary>
        public class Customer
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
            /// the engagement channel – i.e. web vs. instore vs. mobile – where the customer’s enrollment occurred
            /// </summary>
            public string channel { get; set; }
            /// <summary>
            /// created at 
            /// </summary>
            public string created_at { get; set; }
            /// <summary>
            /// customer email address
            /// </summary>
            public string email { get; set; }
            /// <summary>
            /// Might be different from created_at in the case of feed enrollment.
            /// </summary>
            public string enrolled_at { get; set; }
            /// <summary>
            /// Cell phone
            /// </summary>
            public string external_customer_id { get; set; }
            /// <summary>
            /// date/time at which the most recent customer event was recorded
            /// </summary>
            public string last_activity { get; set; }
            /// <summary>
            /// last_reward_date
            /// </summary>
            public string last_reward_date { get; set; }
            /// <summary>
            /// last_reward_event_id
            /// </summary>
            public string last_reward_event_id { get; set; }
            /// <summary>
            /// active or inactive
            /// </summary>
            public string status { get; set; }
            /// <summary>
            /// additional details about the channel, such as store ID, store location, website section, etc.
            /// </summary>
            public string sub_channel { get; set; }
            /// <summary>
            /// additional details about the sub_channel, such as Sales Rep or Employee ID Number
            /// </summary>
            public string sub_channel_detail { get; set; }
            /// <summary>
            /// subscription_type
            /// </summary>
            public string subscription_type { get; set; }
            /// <summary>
            /// unsubscribed
            /// </summary>
            public string unsubscribed { get; set; }
            /// <summary>
            /// mobile communication subscription status
            /// </summary>
            public string unsubscribed_sms { get; set; }
            /// <summary>
            /// updated_at
            /// </summary>
            public string updated_at { get; set; }
            /// <summary>
            /// same as email if name is not available
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// current point balance
            /// </summary>
            public string balance { get; set; }
            /// <summary>
            /// total number of points earned since joining
            /// </summary>
            public string lifetime_balance { get; set; }
            /// <summary>
            /// facebook or twitter profile image url (returned if customer connected his/her social accounts)
            /// </summary>
            public string image_url { get; set; }
            /// <summary>
            /// current tier
            /// </summary>
            public string top_tier_name { get; set; }
            /// <summary>
            /// expiration date/time for the current tier
            /// </summary>
            public string top_tier_expiration_date { get; set; }
            /// <summary>
            /// birthdate
            /// </summary>
            public string birthdate { get; set; }
            /// <summary>
            /// city
            /// </summary>
            public string city { get; set; }
            /// <summary>
            /// country
            /// </summary>
            public string country { get; set; }
            /// <summary>
            /// first_name
            /// </summary>
            public string first_name { get; set; }
            /// <summary>
            /// home_phone
            /// </summary>
            public string home_phone { get; set; }
            /// <summary>
            /// home_store
            /// </summary>
            public string home_store { get; set; }
            /// <summary>
            /// last_name
            /// </summary>
            public string last_name { get; set; }
            /// <summary>
            /// last_visit_date
            /// </summary>
            public string last_visit_date { get; set; }
            /// <summary>
            /// middle_name
            /// </summary>
            public string middle_name { get; set; }
            /// <summary>
            /// mobile_phone
            /// </summary>
            public string mobile_phone { get; set; }
            /// <summary>
            /// postal_code
            /// </summary>
            public string postal_code { get; set; }
            /// <summary>
            /// state
            /// </summary>
            public string state { get; set; }
            /// <summary>
            /// work_phone
            /// </summary>
            public string work_phone { get; set; }
            /// <summary>
            /// address_line_1
            /// </summary>
            public string address_line_1 { get; set; }
            /// <summary>
            /// address_line_2
            /// </summary>
            public string address_line_2 { get; set; }
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
