using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;


namespace Semnox.Parafait.Device.PaymentGateway
{
    public class HttpService
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            log.LogMethodEntry(sender, certificate, chain, errors);
            log.LogMethodExit(true);
            return true;
        }

        public static string Send(string xml, string url, bool isUseCert, int timeout,WeChatConfiguration weChatConfiguration)
        {
            log.LogMethodEntry(xml, url, isUseCert, timeout, weChatConfiguration);
            System.GC.Collect(); 

            string result = ""; 

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                ServicePointManager.DefaultConnectionLimit = 200;
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase)||
                    url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                            new RemoteCertificateValidationCallback(CheckValidationResult);
                }

                request = (HttpWebRequest)WebRequest.Create(url);

                request.Method = "POST";
                request.Timeout = timeout * 1000;

                //WebProxy proxy = new WebProxy();                          // 
                //proxy.Address = new Uri(WxPayConfig.PROXY_URL);              // : 
                //request.Proxy = proxy;

                request.ContentType = "text/xml";
                byte[] data = System.Text.Encoding.UTF8.GetBytes(xml);
                request.ContentLength = data.Length;

                if (isUseCert)
                {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                string path = Path.Combine(currentDirectory, Path.GetFileName("apiclient_cert.p12"));
                    string pass= weChatConfiguration.SSLCERT_PASSWORD;
                X509Certificate2 cert = new X509Certificate2(path,pass );
                    request.ClientCertificates.Add(cert);
                }

                reqStream = request.GetRequestStream();
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();

                response = (HttpWebResponse)request.GetResponse();

                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (System.Threading.ThreadAbortException e)
            {
                log.Error("Error while sending ", e);
                System.Threading.Thread.ResetAbort();
            }
            catch (WebException e)
            {
                log.LogVariableState("StatusCode", ((HttpWebResponse)e.Response).StatusCode);
                log.LogVariableState("StatusDescription", ((HttpWebResponse)e.Response).StatusDescription);
                log.Error("Error occured because of web exceeption", e);
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    //Log.Error("HttpService", "StatusCode : " + ((HttpWebResponse)e.Response).StatusCode);
                    //Log.Error("HttpService", "StatusDescription : " + ((HttpWebResponse)e.Response).StatusDescription);
                }
                 
            }
            catch (Exception ex)
            {
                log.Error("Error occured communicating with the server", ex);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }
            log.LogMethodExit(result);
            return result;
        }

    }
}
