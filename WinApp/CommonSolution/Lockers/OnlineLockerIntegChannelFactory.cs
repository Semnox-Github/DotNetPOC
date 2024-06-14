/********************************************************************************************
 * Project Name - Parafait Locker OnlineLockerIntegChannelFactory
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        18-Sep-2019   Dakshakh raj   modified : added logs
 ********************************************************************************************/
using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Semnox.Parafait.Device.Lockers
{
    class OnlineLockerIntegChannelFactory
    {
        private bool isSSLRequired;
        private ChannelFactory<IOnlineLockerIntegrationInterface> httpFactory;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// OnlineLockerIntegChannelFactory
        /// </summary>
        /// <param name="wcfServerName">wcfServerName</param>
        /// <param name="isSSL">isSSL</param>
        public OnlineLockerIntegChannelFactory(String wcfServerName, bool isSSL)
        {
            log.LogMethodEntry(wcfServerName, isSSL);
            isSSLRequired = isSSL;

            if ((wcfServerName == null) || (wcfServerName.CompareTo("") == 0))
                throw new Exception("WCF Server name not set");

            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (se, cert, chain, sslerror) =>
                    {
                        return true;
                    };
            BasicHttpBinding binding = new BasicHttpBinding();
            if (isSSLRequired == true)
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
                ServicePointManager.ServerCertificateValidationCallback += EasyCertCheck;
            }
            httpFactory = new ChannelFactory<IOnlineLockerIntegrationInterface>(
                                    binding,
                                    new EndpointAddress(wcfServerName + "/OnlineLockerIntegrationService"));

            log.LogMethodExit();
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            log.LogMethodEntry();
            if (httpFactory != null)
                httpFactory.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// CreateChannel
        /// </summary>
        /// <returns></returns>
        public IOnlineLockerIntegrationInterface CreateChannel()
        {
            log.LogMethodEntry();
            if (httpFactory != null)
                log.LogMethodExit(httpFactory.CreateChannel());
                return httpFactory.CreateChannel();
            throw new Exception("Channel factory not setup. Please check..");
        }

        /// <summary>
        /// EasyCertCheck
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="cert">cert</param>
        /// <param name="chain">chain</param>
        /// <param name="error">error</param>
        /// <returns></returns>
        bool EasyCertCheck(object sender, X509Certificate cert, X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            log.LogMethodEntry(sender, cert, chain, error);
            log.LogMethodExit(true);
            return true;
        }
    }
}
