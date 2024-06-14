using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

public enum HttpVerb
{
    GET,
    POST,
    PUT,
    // DELETE
}

namespace Semnox.Core.HttpUtils
{
    /// <summary>
    /// Summary description for Connection
    /// </summary>
    public class WebRequestClient
    {
        public string EndPoint { get; set; }
        public HttpVerb Method { get; set; }

        public string ContentType { get; set; }
        public string PostData { get; set; }

        public Boolean UseProxy { get; set; }
        public String ProxyHost { get; set; }
        public String ProxyUser { get; set; }
        public String ProxyPassword { get; set; }
        public String ProxyDomain { get; set; }
        public Boolean Debug { get; set; }
        public Boolean UseSsl { get; set; }
        public bool IgnoreSslErrors { get; set; }

        // Authentication
        public String Username { get; set; }
        public String Password { get; set; }
        public bool IsBasicAuthentication { get; set; }

        public List<System.Security.Cryptography.X509Certificates.X509Certificate2> ClientCertificates { get; set; }

        public string ResponseToken { get; set; }

        public WebRequestClient()
        {
            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = "text/xml";
            PostData = "";
            this.ResponseToken = string.Empty;
            this.ClientCertificates = new List<System.Security.Cryptography.X509Certificates.X509Certificate2>();

        }

        public WebRequestClient(string endpoint, HttpVerb method)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "application/json; charset=iso-8859-1";
            PostData = "";
            this.ClientCertificates = new List<System.Security.Cryptography.X509Certificates.X509Certificate2>();

        }

        public WebRequestClient(string endpoint, HttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "application/json; charset=iso-8859-1";
            PostData = postData;
            this.ClientCertificates = new List<System.Security.Cryptography.X509Certificates.X509Certificate2>();

        }


        public String MakeRequest(List<KeyValuePair<string, string>> kvpHeaderList = null)
        {
            var responseFields = new Dictionary<string, string>();

            string body = String.Empty;

            HttpWebRequest request = WebRequest.Create(EndPoint) as HttpWebRequest;
            request.Method = Method.ToString();
            request.ContentType = ContentType;

            // [Snippet] howToSetCredentials - start
            string credentials;
            if (Username != "")
            {
                if (IsBasicAuthentication)
                {
                    credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Username + ":" + Password));
                    request.Headers.Add("Authorization", "Basic " + credentials);
                }
                else
                {
                    request.Headers.Add("Authorization", Password);
                }

            }

            if (kvpHeaderList != null)
            {
                foreach (KeyValuePair<string, string> param in kvpHeaderList)
                {
                    request.Headers.Add(param.Key, param.Value);
                }
            }

            if (this.ClientCertificates.Count() > 0)
            {
                request.ClientCertificates.Add(this.ClientCertificates[0]);
            }

            // Create a byte array of the data we want to send
            byte[] utf8bytes = Encoding.UTF8.GetBytes(PostData);
            byte[] iso8859bytes = Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("iso-8859-1"), utf8bytes);

            // Set the content length in the request headers
            request.ContentLength = iso8859bytes.Length;

            // Ignore format error checks before sending body
            request.ServicePoint.Expect100Continue = false;

            try
            {
                // [Snippet] executeSendTransaction - start
                // Write data
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(iso8859bytes, 0, iso8859bytes.Length);
                }
                // [Snippet] executeSendTransaction - end

                // Get response
                try
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        // Get the response stream
                        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"));
                        body = reader.ReadToEnd();
                        if (response.Headers.GetValues("Authorization") != null)
                        {
                            this.ResponseToken = response.Headers.GetValues("Authorization").FirstOrDefault();
                        }

                        this.ResponseToken = this.ResponseToken == null ? "" : this.ResponseToken;
                    }
                }
                catch (WebException wex)
                {
                    StreamReader reader = new StreamReader(wex.Response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"));
                    body = reader.ReadToEnd();
                }
                return body;
            }
            catch (Exception ex)
            {
                return ex.Message + "\n\naddress:\n" + request.Address.ToString() + "\n\nheader:\n" + request.Headers.ToString() + "data submitted:\n" + PostData;
            }
        }

        public String GetResponse()
        {
            //InitialiseClient();

            var responseFields = new Dictionary<string, string>();
            string body = String.Empty;

            // Create the web request
            HttpWebRequest request = WebRequest.Create(EndPoint) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json; charset=iso-8859-1";

            if (Username != "")
            {
                if (IsBasicAuthentication)
                {
                    string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Username + ":" + Password));
                    request.Headers.Add("Authorization", "Basic " + credentials);
                }
                else
                {
                    request.Headers.Add("Authorization", Password);
                }
            }


            request.ServicePoint.Expect100Continue = false;
            try
            {
                try
                {
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        // Get the response stream
                        StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"));
                        body = reader.ReadToEnd();
                    }
                }
                catch (WebException wex)
                {
                    StreamReader reader = new StreamReader(wex.Response.GetResponseStream(), Encoding.GetEncoding("iso-8859-1"));
                    body = reader.ReadToEnd();
                }
                return body;
            }
            catch (Exception ex)
            {
                return ex.Message + "\n\naddress:\n" + request.Address.ToString() + "\n\nheader:\n" + request.Headers.ToString();
            }
        }

        public string MakeXMLRequest()
        {
            try
            {
                string receivedResponse = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(EndPoint);

                byte[] requestInFormOfBytes = System.Text.Encoding.ASCII.GetBytes(PostData);
                request.Method = "POST";

                request.ContentType = "text/xml;charset=utf-8";
                request.ContentLength = requestInFormOfBytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(requestInFormOfBytes, 0, requestInFormOfBytes.Length);
                requestStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                receivedResponse = respStream.ReadToEnd();
                respStream.Close();
                response.Close();

                return receivedResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
