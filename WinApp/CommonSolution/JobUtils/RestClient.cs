/********************************************************************************************
 * Project Name - Job Utils
 * Description  - Rest Client
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019      Deeksha        Added logger methods.
 ********************************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Semnox.Parafait.JobUtils
{

    /// <summary>
    /// RestClient Class
    /// </summary>
    public class RestClient
    {
        /// <summary>
        /// HttpVerb enum
        /// </summary>
        public enum HttpVerb
        {
            /// <summary>
            /// http request type GET, when only requesting some response
            /// </summary>
            GET,
            /// <summary>
            /// http request type POST, handle request with passing some data
            /// </summary>
            POST,
            /// <summary>
            /// http request type GET, when updating in server
            /// </summary>
            PUT,
            /// <summary>
            /// http request type GET, when deleting activities in server
            /// </summary>
            DELETE
        }

       // ParafaitUtils.Utilities utilities;

        string MerkelURL = string.Empty;
        
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private string endPoint;
        private HttpVerb method;
        private string contentType;
        private string postData;

        /// <summary>
        /// Enpoint string
        /// </summary>
        public string EndPoint
        {
            get
            {
                return endPoint;
            }

            set
            {
                endPoint = value;
            }
        }

        /// <summary>
        /// http method type
        /// </summary>
        public HttpVerb Method
        {
            get
            {
                return method;
            }

            set
            {
                method = value;
            }
        }

        /// <summary>
        /// çontent type json/xml
        /// </summary>
        public string ContentType
        {
            get
            {
                return contentType;
            }

            set
            {
                contentType = value;
            }
        }

        /// <summary>
        /// data to be send 
        /// </summary>
        public string PostData
        {
            get
            {
                return postData;
            }

            set
            {
                postData = value;
            }
        }

        /// <summary>
        /// Constructor withot parameters
        /// </summary>
        public RestClient()
        {
            log.LogMethodEntry();

            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = "text/json";
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
            ContentType = "text/json";
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
            ContentType = "text/json";
            PostData = "";

            log.LogMethodExit();
        }

        /// Calling for webService
        public string MakeRequest()
        {
            log.LogMethodEntry();
            string returnvalue = MakeRequest("");
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        /// Calling for webService returns string
        public string MakeRequest(string parameters)
        {
            log.LogMethodEntry(parameters);
            string responseText = string.Empty;
            //System.Net.ServicePointManager.DefaultConnectionLimit = 100; // Just selected a random number for testing greater than 2
            //System.Net.ServicePointManager.SetTcpKeepAlive(true, 30, 30); // 30 is based on my server i'm hitting
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EndPoint);
            //    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            //    request.Timeout = 60000;
            //    request.ReadWriteTimeout = 60000;
            //    //request.Proxy = new WebProxy("http://" + proxyUsed + "/", true);
            ////    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.01; Windows NT 5.0)";//ahem! :)
            //    DateTime giveUp = DateTime.UtcNow.AddSeconds(60);
            //    using (WebResponse myResponse = request.GetResponse())
            //    {
            //        //httpLink = myResponse.ResponseUri.AbsoluteUri;
            //        using (Stream s = myResponse.GetResponseStream())
            //        {
            //            s.ReadTimeout = 60000;
            //            s.WriteTimeout = 60000;
            //            char[] buffer = new char[4096];
            //            StringBuilder sb = new StringBuilder();
            //            using (StreamReader sr = new StreamReader(s, System.Text.Encoding.UTF8))
            //            {
            //                for (int read = sr.Read(buffer, 0, 4096); read != 0; read = sr.Read(buffer, 0, 4096))
            //                {
            //                    if (DateTime.UtcNow > giveUp)
            //                        throw new TimeoutException();
            //                    sb.Append(buffer, 0, read);
            //                }
            //                responseText = sb.ToString();
            //            }
            //        }
            //    }

            // request.Method = "GET";
            //request.Timeout = 30000;
            //request.KeepAlive = false;
            ////request.ServicePoint.ConnectionLeaseTimeout = 5000;
            ////request.ServicePoint.MaxIdleTime = 5000;
            //request.Proxy = null;
            //using (WebResponse response = request.GetResponse())
            //{
            //    using (Stream responseStream = response.GetResponseStream())
            //    {
            //        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            //        responseText = reader.ReadToEnd();
            //        reader.Close();
            //    }

            //}

            //  return responseText;
            //}
            //catch (WebException ex)
            //{
            //    WebResponse errorResponse = ex.Response;
            //    using (Stream responseStream = errorResponse.GetResponseStream())
            //    {
            //        StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            //        return reader.ReadToEnd();
            //        // log errorText
            //    }
            //    //throw;
            //}
            var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);
            request.Method = Method.ToString();
            request.PreAuthenticate = true;
            request.ContentLength = 0;
            request.Timeout = 30000;
            request.ContentType = ContentType;
            string userName = string.Empty;
            string pwd = string.Empty;

            userName = "Username";
            pwd = "Passowod";
            request.Credentials = new NetworkCredential(userName.Trim(), pwd);

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
    }
}
