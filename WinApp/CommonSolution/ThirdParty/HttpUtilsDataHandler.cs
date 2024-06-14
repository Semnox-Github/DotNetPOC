/********************************************************************************************
 * Project Name - Http Utils Data Handler
 * Description  - Data handler of the  Loyalty RedemptionBL  class
 * 
 **************
 **Version Logs
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        1-July-2016   Amaresh          Created 
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;


namespace Semnox.Parafait.ThirdParty
{
    /// <summary>
    /// HttpVerb enum
    /// </summary>
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    /// <summary>
    /// HttpUtilsDataHandler - Request the API
    /// </summary>
   public class HttpUtilsDataHandler
    {
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       Utilities utilities = new Utilities();
       string CapillaryURL = string.Empty;

        /// <summary>
        /// HttpUtilsDataHandler constructor
        /// </summary>
        public HttpUtilsDataHandler()
        {
            log.LogMethodEntry();

            CapillaryURL = string.IsNullOrWhiteSpace(utilities.getParafaitDefaults("CAPILLARY_INTEGRATION_URL")) ? string.Empty : utilities.getParafaitDefaults("CAPILLARY_INTEGRATION_URL");

            log.LogMethodExit();
        }

        /// <summary>
        /// RestClient Class
        /// </summary>
        public class RestClient
        {
            Utilities _utilities = new Utilities();
            public string EndPoint { get; set; }
            public HttpVerb Method { get; set; }
            public string ContentType { get; set; }
            public string PostData { get; set; }

            /// <summary>
            /// Constructor without parameters
            /// </summary>
            public RestClient()
            {
                log.LogMethodEntry();

                EndPoint = "";
                Method = HttpVerb.GET;
                ContentType = "text/xml";
                PostData = "";

                log.LogMethodExit();
            }

            /// <summary>
            /// Constructor with endpoint string parameter
            /// </summary>
            public RestClient(string endpoint)
            {
                log.LogMethodEntry(endpoint);

                EndPoint = endpoint;
                Method = HttpVerb.GET;
                ContentType = "text/xml";
                PostData = "";

                log.LogMethodExit();
            }

            /// <summary>
            /// Constructor with endpoint and method parameter
            /// </summary>
            public RestClient(string endpoint, HttpVerb method)
            {
                log.LogMethodEntry(endpoint, method);

                EndPoint = endpoint;
                Method = method;
                ContentType = "text/xml";
                PostData = "";

                log.LogMethodExit();
            }

            /// <summary>
            /// Constructor with endpoint, method and postData parameter
            /// </summary>
            public RestClient(string endpoint, HttpVerb method, string postData)
            {
                log.LogMethodEntry(endpoint, method, postData);

                EndPoint = endpoint;
                Method = method;
                ContentType = "text/xml";
                PostData = postData;

                log.LogMethodExit();
            }

            /// Calling for webService
            public string MakeRequest()
            {
                log.LogMethodEntry();

                string returnValueNew = MakeRequest("");
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }

            /// Calling for webService returns string
            public string MakeRequest(string parameters)
            {
                log.LogMethodEntry(parameters);

                var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);
                request.Method = Method.ToString();
                request.PreAuthenticate = true;
                request.ContentLength = 0;
                request.Timeout = 20000; // 20 sec
                request.ContentType = ContentType;
                string userName = string.Empty;
                string pwd = string.Empty;

                userName = _utilities.getParafaitDefaults("CAPILLARY_INTEGRATION_USERNAME");
                pwd = EncryptMD5(ParafaitDefaultContainerList.GetParafaitDefault(_utilities.ExecutionContext, "CAPILLARY_INTEGRATION_PASSWORD"));//_utilities.getParafaitDefaults("CAPILLARY_INTEGRATION_PASSWORD"));
                request.Credentials = new NetworkCredential(userName.Trim(), pwd);
                //"ed21585b56ad2974c7dce89007ad0331");

                // request.Credentials = CredentialCache.DefaultNetworkCredentials;
                if (!string.IsNullOrEmpty(PostData) && Method == HttpVerb.POST)
                {
                    var encoding = new UTF8Encoding();
                    var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseValue = string.Empty;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);

                        log.LogMethodExit(null, "Throwing ApplicationException - " + message);
                        throw new ApplicationException(message);
                    }

                    // grab the response
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                    }

                    log.LogMethodExit(responseValue);
                    return responseValue;
                }
            }

            /// Used for Serializating of object to XML
            public string Serialize(object dataToSerialize)
            {
                log.LogMethodEntry(dataToSerialize);

                StringWriter stringwriter = new System.IO.StringWriter();

                XmlSerializer s = new XmlSerializer(dataToSerialize.GetType());
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                s.Serialize(stringwriter, dataToSerialize, ns);

                String xml = stringwriter.ToString();
                xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");

                log.LogMethodExit(xml);
                return xml;
            }

            /// Used for Encryption
            public string EncryptMD5(string Metin)
            {
                log.LogMethodEntry(Metin);

                MD5CryptoServiceProvider MD5Code = new MD5CryptoServiceProvider();
                byte[] byteDizisi = Encoding.UTF8.GetBytes(Metin);
                byteDizisi = MD5Code.ComputeHash(byteDizisi);
                StringBuilder sb = new StringBuilder();
                foreach (byte ba in byteDizisi)
                {
                    sb.Append(ba.ToString("x2").ToLower());
                }

                string returnValueNew = sb.ToString();
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
        } // class

        /// get the responce for valid customer
        public string  ValidateCustomer(string phoneNo)
        {
           log.LogMethodEntry(phoneNo);

           string endPoint = CapillaryURL+"/customer/get?format=xml&mobile=" + phoneNo;
           var client = new RestClient(endPoint);
           var json = client.MakeRequest();

           string returnValueNew = json.ToString();
           log.LogMethodEntry(returnValueNew);
           return returnValueNew;
        }

        /// get the response after adding customer details
        public string AddCustomerDetails(string xml)
        {
            log.LogMethodEntry(xml);

            string endPoint = CapillaryURL+"/customer/add?format=xml";
            var Rclient = new RestClient(endPoint, HttpVerb.POST, xml);
            var jsonResult = Rclient.MakeRequest();

            string returnValueNew = jsonResult.ToString();
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

       /// get the response after updating customer Details
       public string UpdateCustomerDetails(string xml)
        {
            log.LogMethodEntry(xml);

            string endPoint = CapillaryURL+"/customer/update?format=xml ";
            var Rclient = new RestClient(endPoint, HttpVerb.POST, xml);
            var jsonResult = Rclient.MakeRequest();

            string returnValueNew = jsonResult.ToString();
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }
        
        /// get the response for Validating couponRedeemable
        public string ValidateCouponRedeemable(string couponNumber, string phoneNo1)
        {
            log.LogMethodEntry(couponNumber, phoneNo1);

            string endPoint = CapillaryURL+"/coupon/isredeemable?format=xml&details=true&code=" + couponNumber + "&mobile=" + phoneNo1;
            var Rclient = new HttpUtilsDataHandler.RestClient(endPoint);
            var Rjson = Rclient.MakeRequest();

            string returnValueNew = Rjson.ToString();
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        /// get the response for redeem coupon
        public string RedeemCouponInCapillary(string xml)
        {
            log.LogMethodEntry(xml);

            string endPoint = CapillaryURL+"/coupon/redeem?format=xml";
            var Rclient = new RestClient(endPoint, HttpVerb.POST, xml);
            var json = Rclient.MakeRequest();

            string returnValueNew = json.ToString();
            log.LogMethodExit(returnValueNew);
            return returnValueNew;         
        }
     
       /// <summary>
       ///  Receive the validationCode 
       /// </summary>
       /// <param name="redeemPoints">points</param>
       /// <param name="phoneNo">Phone number </param>
       public string ReceiveValidationCode(string redeemPoints, string phoneNo)
       {
           log.LogMethodEntry(redeemPoints, phoneNo);

           string endPoint = CapillaryURL+"/points/validationcode?format=xml&<query" + "&mobile=" + phoneNo + "&points=" + redeemPoints;
           var Rclient = new RestClient(endPoint);
           var Rjson = Rclient.MakeRequest();

           string returnValueNew = Rjson.ToString();
           log.LogMethodExit(returnValueNew);
           return returnValueNew;
       }

        /// <summary>
        ///  Validate the points redeemable? 
        /// </summary>
        /// <param name="points">points</param>
        /// <param name="validationCode"> Validation code </param>
        /// <param name="phoneNo">Phone number </param>
        public string  ValidatePointsRedeemable(string points, string validationCode, string phoneNo)
       {
           log.LogMethodEntry(points, validationCode, phoneNo);

           string endPoint = CapillaryURL+"/points/isredeemable?format=xml" + "&points=" + points + "&validation_code=" + validationCode + "&mobile=" + phoneNo;
           var Rclient = new RestClient(endPoint);
           var Rjson = Rclient.MakeRequest();

           string returnValueNew = Rjson.ToString();
           log.LogMethodExit(returnValueNew);
           return returnValueNew;
       }

       /// <summary>
       ///  Redeem the points
       /// </summary>
       /// <param name="xml">xml file</param>
       /// <returns>Returns responce </returns>
       public string RedeemPointsInCapillary(string xml)
       {
           log.LogMethodEntry(xml);
           
           string endPoint = CapillaryURL+"/points/redeem?format=xml";
           var Rclient = new RestClient(endPoint, HttpVerb.POST, xml);
           var Rjson = Rclient.MakeRequest();

           string returnValueNew = Rjson.ToString();
           log.LogMethodExit(returnValueNew);
           return returnValueNew;
       }
    }
}
