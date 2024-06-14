/********************************************************************************************
 * Class Name - Generic Utilities                                                                         
 * Description - RemotingClient
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        30-Oct-2019            Guru S A       Waiver phase 2 changes
 *2.70.2         26-Nov-2019   Lakshminarayana      Virtual store enhancement
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;


namespace Semnox.Core.GenericUtilities
{
    public class RemotingClient
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ParafaitRemotingClient client;
        Service clientWS;
        string ServerMachine;
        string remotingURL;
        public RemotingClient()
        {
            log.LogMethodEntry();
            int pos1, pos2, pos3;

            string ConnectionString = Settings.Default.ParafaitConnectionString;
            pos1 = ConnectionString.IndexOf("data source", StringComparison.OrdinalIgnoreCase);
            pos2 = ConnectionString.IndexOf('=', pos1) + 1;
            pos3 = ConnectionString.IndexOf('\\', pos2);
            if (pos3 == -1)
                pos3 = ConnectionString.IndexOf(';', pos2);

            if (pos3 == -1)
                ServerMachine = ConnectionString.Substring(pos2);
            else
                ServerMachine = ConnectionString.Substring(pos2, pos3 - pos2);

            ServerMachine = ServerMachine.Trim();
            if (ServerMachine == "" || ServerMachine == ".")
                ServerMachine = "localhost";

            Semnox.Core.Utilities.Utilities utils = new Semnox.Core.Utilities.Utilities();
            remotingURL = utils.getParafaitDefaults("ONDEMAND_REMOTING_SERVER_URL");
            GenerateRemotingUrl();

            log.LogMethodExit();
        }
        // Generating Remote URL for web service directly and using for API
        // Jagan Mohana Rao Added on 29-Nov-2018
        public RemotingClient(bool generateWSremotingClient)
        {
            log.LogMethodEntry(generateWSremotingClient);
            int pos1, pos2, pos3;

            string ConnectionString = Settings.Default.ParafaitConnectionString;
            pos1 = ConnectionString.IndexOf("data source", StringComparison.OrdinalIgnoreCase);
            pos2 = ConnectionString.IndexOf('=', pos1) + 1;
            pos3 = ConnectionString.IndexOf('\\', pos2);
            if (pos3 == -1)
                pos3 = ConnectionString.IndexOf(';', pos2);

            if (pos3 == -1)
                ServerMachine = ConnectionString.Substring(pos2);
            else
                ServerMachine = ConnectionString.Substring(pos2, pos3 - pos2);

            ServerMachine = ServerMachine.Trim();
            if (ServerMachine == "" || ServerMachine == ".")
                ServerMachine = "localhost";

            Semnox.Core.Utilities.Utilities utils = new Semnox.Core.Utilities.Utilities();
            remotingURL = utils.getParafaitDefaults("WEBSERVICE_UPLOAD_URL");

            if (string.IsNullOrEmpty(remotingURL.Trim()))
            {
                remotingURL = "http://" + ServerMachine + ":8000";
            }            
            clientWS = new Service(remotingURL);
            log.LogMethodExit();
        }

        public RemotingClient(string connstring)
        {
            log.LogMethodEntry(connstring);
            int pos1, pos2, pos3;

            pos1 = connstring.IndexOf("data source", StringComparison.OrdinalIgnoreCase);
            pos2 = connstring.IndexOf('=', pos1) + 1;
            pos3 = connstring.IndexOf('\\', pos2);
            if (pos3 == -1)
                pos3 = connstring.IndexOf(';', pos2);

            if (pos3 == -1)
                ServerMachine = connstring.Substring(pos2);
            else
                ServerMachine = connstring.Substring(pos2, pos3 - pos2);

            ServerMachine = ServerMachine.Trim();
            if (ServerMachine == "" || ServerMachine == ".")
                ServerMachine = "localhost";

            Semnox.Core.Utilities.Utilities utils = new Semnox.Core.Utilities.Utilities(connstring);
            remotingURL = utils.getParafaitDefaults("ONDEMAND_REMOTING_SERVER_URL");

            GenerateRemotingUrl();
            log.LogMethodExit();
        }

        void GenerateRemotingUrl()
        {
            log.LogMethodEntry();
            if (string.IsNullOrEmpty(remotingURL.Trim()))
                remotingURL = "http://" + ServerMachine + ":8000";
            Semnox.Core.Utilities.Utilities utils = new Semnox.Core.Utilities.Utilities();
            string timeoutValue = utils.getParafaitDefaults("ONDEMAND_REMOTING_SERVICE_TIMEOUT");
            int timeoutInSeconds = 20;
            if (int.TryParse(timeoutValue, out timeoutInSeconds) == false)
            {
                timeoutInSeconds = 20;
            }
            //Step 1: Create an endpoint address and an instance of the WCF Client.
            //client = new ParafaitRemotingClient("WSHttpBinding_IParafaitRemoting", "http://" + ServerMachine + ":8000/ParafaitRemoting/Service/ParafaitRemoting");
            System.ServiceModel.WSHttpBinding binding = new System.ServiceModel.WSHttpBinding(System.ServiceModel.SecurityMode.None);
            binding.SendTimeout = new TimeSpan(0, 0, timeoutInSeconds);
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MaxBufferPoolSize = int.MaxValue;
            client = new ParafaitRemotingClient(binding, new System.ServiceModel.EndpointAddress(remotingURL + "/ParafaitRemoting/Service/ParafaitRemoting"));
            log.LogMethodExit();
        }

        public string GetServerCard(string cardNumber, int SiteId, ref string message)
        {
            log.LogMethodEntry(cardNumber, SiteId, message);
            int cardId = client.getServerCard(cardNumber, SiteId, ref message);
            if (cardId == -1)
            {
                log.LogVariableState("Message", message);
                log.LogMethodExit("NOT FOUND");
                return "NOTFOUND";
            }

            else if (cardId < 0)
            {
                log.LogVariableState("Message", message);
                log.LogMethodExit(message);
                return message;
            }

            else
            {
                message = "SUCCESS";
                log.LogVariableState("Message", message);
                log.LogMethodExit(cardNumber);
                return cardNumber;
            }
        }
        
        public DataSet GetServerCardActivity(string cardNumber, int SiteId)
        {
            log.LogMethodEntry(cardNumber, SiteId);
            DataSet returnvalue = (client.getServerCardActivity(cardNumber, SiteId));
            log.LogMethodExit(returnvalue);
            return (returnvalue);

        }

        public Semnox.Core.GenericUtilities.WebApiResponse Post(string url, List<KeyValuePair<string, string>> headers, string content, string contentType)
        {
            log.LogMethodEntry(url, headers, content, contentType);
            Semnox.Core.GenericUtilities.WebApiResponse returnvalue = (client.Post(url, headers, content, contentType));
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }

        // This function hit the web service and get the consolidated data of GamePlays for particular CardNumber
        // The difference between the GetServerCardActivity() and GetServerCardActivityFromWS() is WCF call and WS calls. 
        // Other difference is input parameters inside the function
        // Jagan Mohana Rao Added on 29-Nov-2018
        public DataSet GetServerCardActivityFromWS(string companyKey, string cardNumber, int SiteId)
        {
            log.LogMethodEntry(companyKey, cardNumber, SiteId);
            string message = string.Empty;
            DataSet returnvalue = clientWS.GetServerCardActivity(companyKey, cardNumber, SiteId, ref message);
            log.LogMethodExit(returnvalue);
            return (returnvalue);
        }
        /// <summary>
        /// Download Server Files
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="fileName"></param>
        public void DownloadServerFiles(int siteId, string fileName)
        {
            log.LogMethodEntry(siteId, fileName);
            try
            {
                client.DownloadServerFiles(siteId, fileName);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
        }

        ~RemotingClient()
        {
            log.LogMethodEntry();
            //Step 3: Closing the client gracefully closes the connection and cleans up resources.
            if (client != null)
            {
                try
                {
                    client.Close();
                }
                catch (Exception ex)
                {
                    log.Error("Error occured when the client tried to close", ex);
                }
            }
            log.LogMethodExit();
        }
    }
}
