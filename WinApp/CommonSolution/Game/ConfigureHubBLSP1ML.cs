/********************************************************************************************
 * Project Name - ConfigureHub BL - SP1ML
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date                    Modified By                       Remarks          
 *********************************************************************************************
 *2.60        22-Feb-2019           Indrajeet Kumar                     Created 
 *2.60        12-Apr-2019           Akshay Gulaganji                    modified and added parameterized constructor with ExecutionContext in HubConfig Class
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
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    public class ConfigureHubBLSP1ML
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private static NetScan NetScan = NetScan.Instance;
        private HubDTO hubDTO;
        int hubAddress, frequency;

        /// <summary>
        /// parameterized Constructor with executionContext
        /// </summary>
        /// <param name="executionContext"></param>
        public ConfigureHubBLSP1ML(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.hubDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// parameterized constructor with hubDto and executionContext
        /// </summary>
        /// <param name="hubDTO"></param>
        /// <param name="executionContext"></param>
        public ConfigureHubBLSP1ML(ExecutionContext executionContext, HubDTO hubDTO)
        {
            log.LogMethodEntry(hubDTO, executionContext);
            this.executionContext = executionContext;
            this.hubDTO = hubDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the Hub based on searchParameters
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        public List<HubDTO> GetHubSearchList(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            HubDataHandler hubDataHandler = new HubDataHandler();
            log.LogMethodExit();
            return hubDataHandler.GetHubList(searchParameters);
        }

        /// <summary>
        /// Configures the Spirit
        /// </summary>
        public void ConfigureSpirit()
        {
            log.LogMethodEntry();
            try
            {
                HubConfig configureHubBLspirit1 = new HubConfig(executionContext);
                Int32.TryParse(hubDTO.Address, out hubAddress);
                Int32.TryParse(hubDTO.Frequency, out frequency);
                configureHubBLspirit1.SetParameters(hubAddress, frequency, hubDTO.BaudRate, hubDTO.IPAddress, hubDTO.MACAddress, hubDTO.TCPPort, hubDTO.DataRate);
                configureHubBLspirit1.Configure();
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
        /// Sets the Register
        /// </summary>
        public void SetRegister()
        {
            log.LogMethodEntry();
            string registerValue = hubDTO.RegisterValue;
            string registeredDDvalue = hubDTO.RegisterId.ToString();
            string registeredName = string.Empty;

            foreach(var registerMap in AdvancedRegister.RegisterMap)
            {
                if (registerMap.Value == registeredDDvalue.ToString())
                {
                    registeredName = registerMap.Key.ToString();
                    break;
                }
            }
            
            if (AdvancedRegister.RegisterMap.Count >= 0 && string.IsNullOrEmpty(registerValue) == false)
            {
                try
                {
                    if ((Registers)Enum.Parse(typeof(Registers), registeredName.ToString()) == Registers.BAUD_RATE)
                    {
                        //if (MessageBox.Show("Are you sure you want to change the baud rate? Module may be incommunicable if not set properly.", "Baud Rate Change", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == System.Windows.Forms.DialogResult.No)
                        //return;
                        int baudRate;
                        Int32.TryParse(registerValue, out baudRate);

                        if (baudRate != 9600
                            && baudRate != 19200
                            && baudRate != 38400
                            && baudRate != 57600
                            && baudRate != 115200
                            && baudRate != 230400
                            && baudRate != 460800
                            && baudRate != 921600)
                        {
                            string message = MessageContainerList.GetMessage(executionContext, "Invalid Baud Rate");
                            log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                            throw new ValidationException(message);
                        }
                    }
                    HubConfig hubConfig = new HubConfig(executionContext);
                    Int32.TryParse(hubDTO.Address, out hubAddress);
                    Int32.TryParse(hubDTO.Frequency, out frequency);
                    hubConfig.SetParameters(hubAddress, frequency, hubDTO.BaudRate, hubDTO.IPAddress, hubDTO.MACAddress, hubDTO.TCPPort, hubDTO.DataRate);
                    hubConfig.SetRegister(registeredName, registerValue);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Reads the Config
        /// </summary>
        /// <returns></returns>
        public List<string> ReadConfig()
        {
            try
            {
                log.LogMethodEntry();
                HubConfig hubConfig = new HubConfig(executionContext);
                Int32.TryParse(hubDTO.Address, out hubAddress);
                Int32.TryParse(hubDTO.Frequency, out frequency);
                hubConfig.SetParameters(hubAddress, frequency, hubDTO.BaudRate, hubDTO.IPAddress, hubDTO.MACAddress, hubDTO.TCPPort, hubDTO.DataRate);
                List<string> configList = hubConfig.ReadAllConfigs();
                log.LogMethodExit(configList);
                return configList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the Socket
        /// </summary>
        /// <returns></returns>
        public SemnoxTCPSocket getSocket()
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
        /// HubConfig class
        /// </summary>
        public class HubConfig
        {
            public delegate void LogEventHandeler(string logText);
            public event LogEventHandeler LogEvent;
            private ExecutionContext executionContext;

            private string MacAddress;
            private string IPAddress;
            private int TCPPort;
            private int HubAddress, Frequency, BaudRate, DataRate;

            private const int SP1ML_868_BASE_FREQ = 863000000;
            private const int SP1ML_915_BASE_FREQ = 902000000;

            /// <summary>
            /// parameterized constructor with ExecutionContext
            /// </summary>
            /// <param name="executionContext"></param>
            public HubConfig(ExecutionContext executionContext)
            {
                log.LogMethodEntry();
                this.executionContext = executionContext;
                log.LogMethodExit();
            }
            /// <summary>
            /// Writes to Log
            /// </summary>
            /// <param name="logText"></param>
            private void WriteToLog(string logText)
            {
                log.LogMethodEntry(logText);
                if (LogEvent != null)
                    LogEvent(logText);
                log.LogMethodExit();
            }
            /// <summary>
            /// Gets Spirit1Frequency
            /// </summary>
            /// <param name="HubFrequencyIndex"></param>
            /// <returns></returns>
            public static int getSpirit1Frequency(int HubFrequencyIndex)
            {
                log.LogMethodEntry(HubFrequencyIndex);
                int spirit1Frequency;
                if (HubFrequencyIndex <= 16)
                {
                    spirit1Frequency = SP1ML_868_BASE_FREQ + HubFrequencyIndex * 500000;
                    log.LogMethodExit(spirit1Frequency);
                    return spirit1Frequency;
                }
                else if (HubFrequencyIndex >= 20)
                {
                    spirit1Frequency = SP1ML_915_BASE_FREQ + (HubFrequencyIndex - 20) * 500000;
                    log.LogMethodExit(spirit1Frequency);
                    return spirit1Frequency;
                }
                else
                { 
                    var message = "Invalid Hub Frequency: " + HubFrequencyIndex.ToString();
                    log.LogMethodExit(null, "Throwing Application Exception - " + message);
                    throw new ApplicationException(message);
                }
            }

            /// <summary>
            /// Gets spirit1DataRate
            /// </summary>
            /// <param name="dataRateIndex"></param>
            /// <returns></returns>
            public static int getSpirit1DataRate(int dataRateIndex)
            {
                log.LogMethodEntry(dataRateIndex);
                int spirit1DataRate;
                if (dataRateIndex < 10)
                {
                    spirit1DataRate = 1000 * (dataRateIndex + 1);
                }
                else
                {
                    spirit1DataRate = 10000 + 5000 * (dataRateIndex - 10 + 1);
                }
                log.LogMethodExit(spirit1DataRate);
                return spirit1DataRate;
            }

            public class ConfigValues
            {
                /// <summary>
                /// Sets the config Value
                /// </summary>
                /// <param name="register"></param>
                /// <param name="value"></param>
                public void SetConfigValue(Registers register, string value)
                {
                    log.LogMethodEntry(register,value);
                    AdvancedRegister.UserSettableValues[register] = value;
                    log.LogMethodExit();
                }
            }

            /// <summary>
            /// Sets Parameters
            /// </summary>
            /// <param name="inHubAddress"></param>
            /// <param name="inFrequency"></param>
            /// <param name="inBaudRate"></param>
            /// <param name="inIPAddress"></param>
            /// <param name="inMacAddress"></param>
            /// <param name="inTCPPort"></param>
            /// <param name="inDataRate"></param>
            public void SetParameters(int inHubAddress, int inFrequency, int inBaudRate, string inIPAddress, string inMacAddress, int inTCPPort, int inDataRate)
            {
                log.LogMethodEntry(inHubAddress, inFrequency, inBaudRate, inIPAddress, inMacAddress, inTCPPort, inDataRate);
                HubAddress = inHubAddress;
                Frequency = inFrequency;
                BaudRate = inBaudRate;
                IPAddress = inIPAddress;
                TCPPort = inTCPPort;
                MacAddress = inMacAddress;
                DataRate = inDataRate;
                log.LogMethodExit();
            }
            /// <summary>
            /// Gets Socket
            /// </summary>
            /// <returns></returns>
            SemnoxTCPSocket getSocket()
            {
                log.LogMethodEntry();
                WriteToLog("Get Socket()");
                IPAddress lclIPAddress = null;
                if (!string.IsNullOrEmpty(IPAddress.Trim()))
                {
                    try
                    {
                        lclIPAddress = System.Net.IPAddress.Parse(IPAddress);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        throw;
                    }
                }
                else if (!string.IsNullOrEmpty(MacAddress.Trim()))
                {
                    int wait = 100; // 5 seconds
                    while (wait-- > 0)
                    {
                        lclIPAddress = NetScan.getIPAddress(MacAddress);
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

                WriteToLog("IP Address is " + lclIPAddress.ToString());

                if (TCPPort <= 0)
                {
                    try
                    {
                        TCPPort = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_TCP_PORT")));
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        throw;
                    }
                }
                WriteToLog("TCP Port: " + TCPPort.ToString());
                SemnoxTCPSocket sock = new SemnoxTCPSocket(lclIPAddress, TCPPort);
                log.LogMethodExit(sock);
                return sock;
            }
            /// <summary>
            /// Checks Reply
            /// </summary>
            /// <param name="data"></param>
            void checkReply(byte[] data)
            {
                log.LogMethodEntry(data);
                string dataString = Encoding.UTF8.GetString(data, 0, data.Length);
                WriteToLog(dataString);
                if (dataString.Contains("ERROR"))
                {
                    string message = "Error setting register value";
                    log.LogMethodExit(null, "Throwing Application Exception - " + message);
                    throw new ApplicationException(message);
                }
            }
            /// <summary>
            /// Configures
            /// </summary>
            public void Configure()
            {
                log.LogMethodEntry();
                WriteToLog("Set configuration");

                SemnoxTCPSocket socket = null;
                try
                {
                    socket = getSocket();

                    ConfigValues configValues = new ConfigValues();
                    configValues.SetConfigValue(Registers.FREQUENCY, getSpirit1Frequency(Frequency).ToString());
                    configValues.SetConfigValue(Registers.DATA_RATE, getSpirit1DataRate(DataRate).ToString());
                    configValues.SetConfigValue(Registers.SOURCE_ADDR, string.Format("0x{0:X2}", HubAddress));

                    checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxCommandModeEnter)));
                    try
                    {
                        foreach (KeyValuePair<Registers, string> config in AdvancedRegister.UserSettableValues)
                        {
                            WriteToLog("Setting " + config.Key.ToString());
                            checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxConfigWrite, AdvancedRegister.RegisterMap[config.Key], Commands.WriteConfigRegister, config.Value)));
                        }

                        foreach (KeyValuePair<Registers, string> config in AdvancedRegister.SystemDefaultValues)
                        {
                            WriteToLog("Setting " + config.Key.ToString());
                            checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxConfigWrite, AdvancedRegister.RegisterMap[config.Key], Commands.WriteConfigRegister, config.Value)));
                        }

                        checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxConfigWrite, Commands.StoreConfig)));
                        checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxConfigWrite, Commands.Restart)));
                    }
                    finally
                    {
                        checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxCommandModeExit)));
                    }

                    WriteToLog("Set configuration success");
                }
                finally
                {
                    try
                    {
                        if (socket != null)
                        {
                            socket.TCPSocket.Disconnect(false);
                            socket.TCPSocket.Close();
                        }
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        throw;
                    }
                }
                log.LogMethodExit();
            }

            /// <summary>
            /// Sets Register
            /// </summary>
            /// <param name="Register"></param>
            /// <param name="Value"></param>
            public void SetRegister(string register, string value)
            {
                log.LogMethodEntry(register, value);
                WriteToLog("Set Register");

                SemnoxTCPSocket socket = null;
                try
                {
                    socket = getSocket();

                    checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxCommandModeEnter)));

                    try
                    {
                        WriteToLog("Setting " + register.ToString());
                        checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxConfigWrite, register, Commands.WriteConfigRegister, value)));
                        checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxConfigWrite, Commands.StoreConfig)));
                        checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxConfigWrite, Commands.Restart)));
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        throw;
                    }

                    finally
                    {
                        checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxCommandModeExit)));
                    }

                    WriteToLog("Set register success");
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
                        if (socket != null)
                        {
                            socket.TCPSocket.Disconnect(false);
                            socket.TCPSocket.Close();
                        }
                    }
                    catch(Exception ex)
                    {
                        log.Error(ex);
                    }
                }
            }

            /// <summary>
            /// makes the Hub Command
            /// </summary>
            /// <param name="commands"></param>
            /// <returns></returns>
            string makeHubCommand(params string[] commands)
            {
                log.LogMethodEntry(commands);
                string command = (string.Join("", commands) + ";").PadRight(32, '0');
                WriteToLog(command);
                log.LogMethodExit(command);
                return command;
            }

            /// <summary>
            /// Reads all Configs
            /// </summary>
            /// <returns></returns>
            public List<string> ReadAllConfigs()
            {
                log.LogMethodEntry();
                WriteToLog("Reading all configs");

                SemnoxTCPSocket socket = null;
                try
                {
                    socket = getSocket();

                    byte[] data;
                    string dataString = "";

                    checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxCommandModeEnter)));

                    try
                    {
                        List<string> configValues = new List<string>();
                        foreach (Registers register in Enum.GetValues(typeof(Registers)))
                        {
                            data = socket.SendPacket(makeHubCommand(Commands.SemnoxConfigRead, AdvancedRegister.RegisterMap[register], Commands.ReadConfigRegister));
                            dataString = System.Text.Encoding.UTF8.GetString(data, 0, data.Length);
                            WriteToLog(dataString);
                            configValues.Add(dataString);
                        }
                        log.LogMethodExit(configValues);
                        return configValues;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                        throw;
                    }
                    finally
                    {
                        checkReply(socket.SendPacket(makeHubCommand(Commands.SemnoxCommandModeExit)));
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
                        if (socket != null)
                        {
                            socket.TCPSocket.Disconnect(false);
                            socket.TCPSocket.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                    log.LogMethodExit();
                }
            }
        }
    }
}

