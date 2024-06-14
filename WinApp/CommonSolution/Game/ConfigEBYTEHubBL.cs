/********************************************************************************************
 * Project Name - ConfigEBYTE Hub
 * Description  - This code has been taken from Parafit application.
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By                       Remarks          
 *********************************************************************************************
 *2.70.2        15-Oct-2019           Rakesh Kumar                     Created 
 *            16-Oct-2019           Jagan Mohana                     Modified - exceptions has been modified. Messages number updated
 *                                                                              ParafaitDefaultContainerList class used to get the default values insted of using utilities.
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
using System.Data;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    public class ConfigEBYTEHubBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = null;
        private static NetScan NetScan = NetScan.Instance;
        private HubDTO hubDTO;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="hubDTO"></param>
        public ConfigEBYTEHubBL(ExecutionContext executionContext, HubDTO hubDTO)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            this.hubDTO = hubDTO;
            log.LogMethodExit();
        }
        public enum Parameters
        {
            ADDRESSH,
            ADDRESSL,
            UART_PARITY,
            UART_BAUD_RATE,
            DATA_RATE,
            CHANNEL,
            FIXED_TRANSMISSION,
            IO_DRIVE_MODE,
            WAKEUP_TIME,
            FEC_SWITCH,
            OUTPUT_POWER
        };

        public static Dictionary<Parameters, byte> ParameterMap = new Dictionary<Parameters, byte>()
            {
                {Parameters.ADDRESSH, 0x00},
                {Parameters.ADDRESSL, 0x00},
                {Parameters.UART_PARITY, 0x00},
                {Parameters.UART_BAUD_RATE, 0x03},
                {Parameters.DATA_RATE, 0x02},
                {Parameters.CHANNEL, 0x06},
                {Parameters.FIXED_TRANSMISSION, 0x01},
                {Parameters.IO_DRIVE_MODE, 0x01},
                {Parameters.WAKEUP_TIME, 0x00},
                {Parameters.FEC_SWITCH, 0x01},
                {Parameters.OUTPUT_POWER, 0x00}
            };

        public static class Commands
        {
            public static byte[] ConfigWrite = { 0xC0 };
            public static byte[] ConfigRead = { 0xC1, 0xC1, 0xC1 };
            public static byte[] GetVersion = { 0xC3, 0xC3, 0xC3 };
            public static byte[] Reset = { 0xC4, 0xC4, 0xC4 };
        }

        public class ConfigValues
        {
            public Dictionary<Parameters, byte> UserSettableValues = new Dictionary<Parameters, byte>();

            public void SetConfigValue(Parameters Register, byte Value)
            {
                UserSettableValues[Register] = Value;
            }
        }

        SemnoxTCPSocket getSocket()
        {
            string MacAddress = hubDTO.MACAddress;
            string IPAddress = hubDTO.IPAddress;
            int TCPPort = hubDTO.TCPPort;

            System.Net.IPAddress lclIPAddress = null;
            if (!string.IsNullOrEmpty(IPAddress.Trim()))
            {
                try
                {
                    lclIPAddress = System.Net.IPAddress.Parse(IPAddress);
                }
                catch (Exception ex)
                {
                    throw ex;
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
                    throw new ApplicationException(message);
                }
            }
            else
            {
                string message = MessageContainerList.GetMessage(executionContext, 1861);                
                log.LogMethodExit(null, "Throwing Validation Exception - " + message);
                throw new ValidationException(message);
            }
            if (TCPPort <= 0)
            {
                try
                {
                    TCPPort = Convert.ToInt32(double.Parse(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DEFAULT_TCP_PORT")));
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
            }
            SemnoxTCPSocket sock = new SemnoxTCPSocket(lclIPAddress, TCPPort);
            log.LogMethodExit(sock);
            return sock;
        }
        public void Configure(ConfigValues configValues)
        {
            log.LogMethodEntry(configValues);
            SemnoxTCPSocket socket = null;
            try
            {
                socket = getSocket();
                byte[] command = makeHubCommand(Commands.ConfigWrite, configValues);
                byte[] data = socket.SendPacket(command, command.Length);
                string dataString = BitConverter.ToString(data, 0, 6);
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
                catch { }
            }
        }
        byte[] makeHubCommand(byte[] command, ConfigValues values)
        {
            log.LogMethodEntry(command, values);
            if (command.Equals(Commands.ConfigWrite))
            {
                byte[] instruction = new byte[6];
                Array.Copy(command, instruction, command.Length);
                int index = command.Length;

                instruction[index++] = values.UserSettableValues[Parameters.ADDRESSH];
                instruction[index++] = values.UserSettableValues[Parameters.ADDRESSL];

                byte data = new byte();
                data = values.UserSettableValues[Parameters.UART_PARITY];
                data <<= 3;
                data |= values.UserSettableValues[Parameters.UART_BAUD_RATE];
                data <<= 3;
                data |= values.UserSettableValues[Parameters.DATA_RATE];
                instruction[index++] = data;

                instruction[index++] = values.UserSettableValues[Parameters.CHANNEL];

                data = 0x00;
                data = values.UserSettableValues[Parameters.FIXED_TRANSMISSION];
                data <<= 1;
                data |= values.UserSettableValues[Parameters.IO_DRIVE_MODE];
                data <<= 3;
                data |= values.UserSettableValues[Parameters.WAKEUP_TIME];
                data <<= 1;
                data |= values.UserSettableValues[Parameters.FEC_SWITCH];
                data <<= 2;
                data |= values.UserSettableValues[Parameters.OUTPUT_POWER];
                instruction[index++] = data;
                log.LogMethodExit(instruction);
                return instruction;
            }
            else
            {
                log.LogMethodExit(command);
                return command;
            }
        }
        public List<string> ReadAllConfigs()
        {
            log.LogMethodEntry();
            SemnoxTCPSocket socket = null;
            try
            {
                socket = getSocket();
                byte[] data;
                string dataString = string.Empty;
                byte[] command = makeHubCommand(Commands.ConfigRead, null);
                data = socket.SendPacket(command, command.Length);
                dataString = BitConverter.ToString(data, 0, 6);
                List<string> configValues = new List<string>();
                ParameterMap[Parameters.ADDRESSH] = data[1];
                ParameterMap[Parameters.UART_PARITY] = Convert.ToByte((data[3] & 0xC0) >> 6);
                ParameterMap[Parameters.UART_BAUD_RATE] = Convert.ToByte((data[3] & 0x38) >> 3);
                ParameterMap[Parameters.DATA_RATE] = Convert.ToByte((data[3] & 0x07));
                ParameterMap[Parameters.CHANNEL] = data[4];
                ParameterMap[Parameters.FIXED_TRANSMISSION] = Convert.ToByte((data[5] & 0x80) >> 7);
                ParameterMap[Parameters.IO_DRIVE_MODE] = Convert.ToByte((data[5] & 0x40) >> 6);
                ParameterMap[Parameters.WAKEUP_TIME] = Convert.ToByte((data[5] & 0x38) >> 3);
                ParameterMap[Parameters.FEC_SWITCH] = Convert.ToByte((data[5] & 0x04) >> 2);
                ParameterMap[Parameters.OUTPUT_POWER] = Convert.ToByte((data[5] & 0x03));
                command = makeHubCommand(Commands.GetVersion, null);
                data = socket.SendPacket(command, command.Length);
                dataString = BitConverter.ToString(data, 0, 4);
                configValues.Add("Module: " + data[1].ToString() + " Version: " + data[2].ToString() + "." + data[3].ToString());
                foreach (Parameters register in Enum.GetValues(typeof(Parameters)))
                {
                    configValues.Add(register.ToString() + ": " + ParameterMap[register].ToString());
                }
                log.LogMethodExit(configValues);
                return configValues;
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
            }
        }

        public static int getBaudRateIndex(int baudRate)
        {
            switch (baudRate)
            {
                case 1200: return 0;
                case 2400: return 1;
                case 4800: return 2;
                case 9600: return 3;
                case 19200: return 4;
                case 38400: return 5;
                case 57600: return 6;
                case 115200: return 7;
                default: return 7;
            }
        }
        public void ConfigureEbyte()
        {
            log.LogMethodEntry();
            try
            {
                ConfigValues configValues = new ConfigValues();
                configValues.SetConfigValue(Parameters.CHANNEL, (byte)(Convert.ToInt32(hubDTO.Frequency)));
                configValues.SetConfigValue(Parameters.ADDRESSH, (byte)(Convert.ToInt32(hubDTO.Address)));
                configValues.SetConfigValue(Parameters.ADDRESSL, 0x00);
                configValues.SetConfigValue(Parameters.DATA_RATE, (byte)hubDTO.EBYTEDTO.DataRate);
                configValues.SetConfigValue(Parameters.FEC_SWITCH, (byte)hubDTO.EBYTEDTO.FecSwitch);
                configValues.SetConfigValue(Parameters.FIXED_TRANSMISSION, (byte)hubDTO.EBYTEDTO.TransmissionMode);
                configValues.SetConfigValue(Parameters.IO_DRIVE_MODE, (byte)hubDTO.EBYTEDTO.IODriveMode);
                configValues.SetConfigValue(Parameters.OUTPUT_POWER, (byte)hubDTO.EBYTEDTO.OutputPower);
                configValues.SetConfigValue(Parameters.UART_BAUD_RATE, (byte)getBaudRateIndex(hubDTO.BaudRate));
                configValues.SetConfigValue(Parameters.UART_PARITY, (byte)hubDTO.EBYTEDTO.UARTParity);
                configValues.SetConfigValue(Parameters.WAKEUP_TIME, (byte)hubDTO.EBYTEDTO.WakeupTime);
                Configure(configValues);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
    }
}