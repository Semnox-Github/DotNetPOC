/********************************************************************************************
 * Project Name - Reports
 * Description  - ReportServerRequest class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.110.0     04-Jan-2021      Laster Menezes     Removed ReportServerRequest parameterized method - Obsolete method
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Semnox.Parafait.Reports
{

    /// <summary>
    /// ReportServerRequest Class
    /// </summary>
    public class ReportServerRequest
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// default constructor
        /// </summary>
        public ReportServerRequest()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// SendRequest(string url) method
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>returns  status bool </returns>
        public bool SendRequest(string url)
        {
            log.Debug("Begin SendRequest");
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                // after the Ignore call i can do what ever i want...

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Timeout =  9900000 ;
                request.KeepAlive = true;
                using (WebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    log.Debug("Ends -SendRequest() method Success    ");
                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends -SendRequest() Exception  method " + ex.Message);
                return false;
            }                   
        }

        private static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

    }
}
