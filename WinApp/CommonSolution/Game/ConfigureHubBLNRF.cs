/********************************************************************************************
 * Project Name - ConfigureHub BLNRF
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date                    Modified By                       Remarks          
 *********************************************************************************************
 *2.60        20-Feb-2019           Indrajeet Kumar                     Created 
 *2.60        12-Apr-2019           Akshay Gulaganji                    modified 
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Communication;
using System.Net;
using Semnox.Parafait.Device;
using System.Timers;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    public class ConfigureHubBLNRF
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = null;
        private HubDataHandler hubDataHandler = null;
        private static NetScan NetScan = NetScan.Instance;
        private HubDTO hubDTO;

        /// <summary>
        /// parameterized Constructor with hubDTO and ExecutionContext
        /// </summary>
        /// <param name="hubDTO"></param>
        /// <param name="executionContext"></param>
        public ConfigureHubBLNRF(ExecutionContext executionContext, HubDTO hubDTO)
        {
            log.LogMethodEntry(hubDTO, executionContext);
            this.executionContext = executionContext;
            this.hubDTO = hubDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ConfigureHubBLNRF(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.hubDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the hubDTO based on hubId
        /// </summary>
        /// <param name="hubId"></param>
        /// <returns>hubDTO</returns>
        public HubDTO GetHubList(int hubId)
        {
            log.LogMethodEntry(hubId);
            hubDataHandler = new HubDataHandler();
            hubDTO = hubDataHandler.GetHub(hubId);
            log.LogMethodExit(hubId);
            return hubDTO;
        }

        /// <summary>
        /// Gets the HubDto List based on searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<HubDTO> GetHubSearchList(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            HubDataHandler hubDataHandler = new HubDataHandler();            
            NetScan.ScanNetwork(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "IP_MASK_FOR_NETWORK_SCAN"), 1000, 1000000);
            var result = hubDataHandler.GetHubList(searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Configures the Hub
        /// </summary>
        public void ConfigureHub()
        {
            log.LogMethodEntry();
            try
            {
                ConfigureHubBLNRF configureHubBL = new ConfigureHubBLNRF(executionContext, hubDTO);
                configureHubBL.ConfigureNrfHub();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Socket
        /// </summary>
        /// <returns></returns>
        SemnoxTCPSocket getSocket()
        {
            log.LogMethodEntry();
            IPAddress lclIPAddress = null;
            if (!string.IsNullOrEmpty(hubDTO.IPAddress.Trim()))
            {
                try
                {
                    lclIPAddress = IPAddress.Parse(hubDTO.IPAddress);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
            }
            else if (!string.IsNullOrEmpty(hubDTO.MACAddress.Trim()))
            {
                int wait = 100; // 5 seconds
                while (wait-- > 0)
                {
                    lclIPAddress = NetScan.getIPAddress(hubDTO.MACAddress);
                    if (lclIPAddress != null)
                        break;
                    System.Threading.Thread.Sleep(50);
                }

                if (lclIPAddress == null)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 729);
                    log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                    throw new ValidationException(message);
                }
            }
            else
            {
                string message = "Neither Mac Address nor IP address specified";
                log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                throw new ValidationException(message);
            }
            if (hubDTO.TCPPort <= 0)
            {
                try
                {                
                    hubDTO.TCPPort = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_TCP_PORT")));
                }
                catch(Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
            }
            SemnoxTCPSocket sock = new SemnoxTCPSocket(lclIPAddress, hubDTO.TCPPort);
            log.LogMethodExit(sock);
            return sock;
        }

        /// <summary>
        /// Configures the NrfHub
        /// </summary>
        public void ConfigureNrfHub()
        {
            log.LogMethodEntry();
            if ((hubDTO.PortNumber == null ||  hubDTO.PortNumber <= 0) && string.IsNullOrEmpty(hubDTO.MACAddress) && string.IsNullOrEmpty(hubDTO.IPAddress))
            {
                var message = MessageContainerList.GetMessage(executionContext, 650, MessageContainerList.GetMessage(executionContext, "Hub Configure"));
                log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                throw new ValidationException(message);
            }

            //Required The Confirmation Message From the UI.
            //Message is -- Are your sure you want to configure this hub? Address and Frequency will be reconfigured.

            //if (MessageBox.Show(Common.MessageUtils.getMessage(541), Common.MessageUtils.getMessage("Hub Configure"), MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
            //{
            //    return;
            //}

            if (hubDTO.PortNumber > 0)
            {
                COMPort Comport = null;
                try
                {
                    Comport = new COMPort(Convert.ToInt32(hubDTO.PortNumber), hubDTO.BaudRate);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }

                Comport.setReceiveAction = delegate ()
                {
                    if (Comport.ReceivedData.Contains("SUCCESS"))
                    {
                        var message = MessageContainerList.GetMessage(executionContext, 555);
                        log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                        throw new ValidationException(message);
                    }
                    Comport.ReceivedData = "";
                    Comport.Close();
                    Comport.comPort.Dispose();
                };

                if (!Comport.Open())
                {
                    var message = MessageContainerList.GetMessage(executionContext, 732, hubDTO.PortNumber.ToString());
                    log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                    throw new ValidationException(message);
                }

                try
                {
                    string configString = "BEGIN_CONFIG_" + hubDTO.Address.ToString().PadLeft(2, '0') + "_" + hubDTO.Frequency.ToString().PadLeft(2, '0') + "_END";
                    configString = configString.PadRight(32, '0');
                    Comport.WriteData(configString);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
            }
            else
            {
                SemnoxTCPSocket socket = null;
                try
                {
                    socket = getSocket();

                    if (socket.SocketValid)
                    {
                        string configString = "BEGIN_CONFIG_" + hubDTO.Address.ToString().PadLeft(2, '0') + "_" + hubDTO.Frequency.ToString().PadLeft(2, '0') + "_END";
                        configString = configString.PadRight(32, '0');
                        byte[] data = socket.SendPacket(configString);
                        string dataString = Encoding.UTF8.GetString(data, 0, data.Length);
                        if (dataString.Contains("SUCCESS"))
                        {
                            var message = MessageContainerList.GetMessage(executionContext, 555);
                            log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                            throw new ValidationException(message);
                        }
                        else
                        {
                            var message = MessageContainerList.GetMessage(executionContext, dataString);
                            log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                            throw new ValidationException(message);
                        }
                    }
                    else
                    {
                        var message = MessageContainerList.GetMessage(executionContext, 733);
                        log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                        throw new ValidationException(message);
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
                finally
                {
                    try
                    {
                        socket.TCPSocket.Disconnect(false);
                        socket.TCPSocket.Close();
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}