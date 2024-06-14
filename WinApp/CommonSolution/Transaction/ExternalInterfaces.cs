/********************************************************************************************
 * Project Name - ExternalInterfaces
 * Description  - External Interfaces
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        27-Jun-2019   Mathew Ninan            Modified Switch on/off method to use FacilityTableDTO 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.IO.Ports;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Transaction
{
    public static class ExternalInterfaces
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static class KMTronic
        {
            public static int SwitchLightOn(uint PortNumber, uint LightNumber)
            {
                log.LogMethodEntry(PortNumber, LightNumber);

                SerialPort sp = new SerialPort("COM" + PortNumber.ToString());
                if (sp.IsOpen)
                {
                    try
                    {
                        sp.Close();
                    }
                    catch(Exception ex)
                    {
                        log.Error("Unable to close the Serial Port - COM" + PortNumber.ToString(), ex);
                    }
                }

                try
                {
                    sp.Open();
                    sp.Write(new byte[] { 0xFF, Convert.ToByte(LightNumber), 0x01 }, 0, 3);

                    log.LogMethodExit(0);
                    return 0;
                }
                catch (Exception ex)
                {
                    log.Error("Unable to Open and Write in the Serial Port ", ex);
                    log.LogMethodExit("Throwing Exception " + ex);    
                    throw ex;
                }
                finally
                {
                    sp.Close();
                }
            }


            public static int SwitchLightOff(uint PortNumber, uint LightNumber)
            {
                log.LogMethodEntry(PortNumber, LightNumber);

                SerialPort sp = new SerialPort("COM" + PortNumber.ToString());
                if (sp.IsOpen)
                {
                    try
                    {
                        sp.Close();
                    }
                    catch(Exception ex)
                    {
                        log.Error("Unable to Close the Serial Port - COM "+PortNumber.ToString(), ex);
                    }
                }

                try
                {
                    sp.Open();
                    sp.Write(new byte[] { 0xFF, Convert.ToByte(LightNumber), 0x00 }, 0, 3);

                    log.LogMethodExit(0);
                    return 0;
                }
                catch (Exception ex)
                {
                    log.Error("Unable to Open and Write in the Serial Port ", ex);
                    log.LogMethodExit("Throwing Exception " + ex);
                    throw ex;
                }
                finally
                {
                    sp.Close();
                }
            }
        }

        static class SNKSnookerControl
        {
            private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


            [DllImport("LightControl.dll")]
            extern static int SetLight(uint Comm, uint lno, uint on_off);
            [DllImport("LightControl.dll")]
            extern static int SetLight2400(uint Comm, uint lno, uint on_off);

            public static int SwitchLightOn(uint PortNumber, uint LightNumber)
            {
                log.LogMethodEntry(PortNumber, LightNumber);

                int returnValueNew = SetLight2400(PortNumber, LightNumber, 1);
                log.LogMethodExit(returnValueNew);
                return returnValueNew; // returns 0 on success
            }

            public static int SwitchLightOff(uint PortNumber, uint LightNumber)
            {
                log.LogMethodEntry(PortNumber, LightNumber);

                int returnValueNew = SetLight2400(PortNumber, LightNumber, 0);
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
        }

        static class SNKKaraokeControl
        {
            public static int SwitchOn(string IPAddress, int Portnumber, string TableNo, ref string message)
            {
                log.LogMethodEntry(IPAddress, Portnumber, TableNo, message);

                try
                {
                    Socket m_socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(IPAddress);
                    System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, Portnumber);
                    m_socClient.Connect(remoteEndPoint);
                    String szData = TableNo + ";01";
                    byte[] byData = System.Text.Encoding.ASCII.GetBytes(szData);
                    m_socClient.SendTimeout = 5000;
                    m_socClient.ReceiveTimeout = 5000;
                    m_socClient.Send(byData);

                    byte[] bytes = new byte[1024];

                    int bytesRec = m_socClient.Receive(bytes);

                    message = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                }
                catch (Exception ex)
                {
                    log.Error("Error when creating a Socket", ex);
                    message = "Karaoke SwitchOn: " + ex.Message;

                    log.LogVariableState("Message ,", message);
                    log.LogMethodExit(1);
                    return 1;
                }

                log.LogVariableState("Message ,", message);
                log.LogMethodExit(0);
                return 0;
            }

            public static int SwitchOff(string IPAddress, int Portnumber, string TableNo, ref string message)
            {
                log.LogMethodEntry(IPAddress, Portnumber, TableNo, message);

                try
                {
                    Socket m_socClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(IPAddress);
                    System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, Portnumber);
                    m_socClient.Connect(remoteEndPoint);
                    String szData = TableNo + ";02";
                    byte[] byData = System.Text.Encoding.ASCII.GetBytes(szData);
                    m_socClient.SendTimeout = 5000;
                    m_socClient.ReceiveTimeout = 5000;
                    m_socClient.Send(byData);

                    byte[] bytes = new byte[1024];

                    int bytesRec = m_socClient.Receive(bytes);

                    message = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                }
                catch (Exception ex)
                {
                    log.Error("Unable to Create Socket! ", ex);
                    message = "Karaoke SwitchOff: " + ex.Message;

                    log.LogVariableState("Message ,", message);
                    log.LogMethodExit(1);
                    return 1;
                }

                log.LogVariableState("message ,", message);
                log.LogMethodExit(0);
                return 0;
            }
        }

        //public static bool SwitchOn(System.Data.DataRow drTableInfo)
        public static bool SwitchOn(FacilityTableDTO faclityTableDTO)
        {
            log.LogMethodEntry(faclityTableDTO);

            switch (faclityTableDTO.TableType)
            {
                case CheckInDTO.PoolTableType:
                    {
                        uint portNo;
                        if (uint.TryParse(faclityTableDTO.InterfaceInfo1, out portNo))
                        {
                            if (portNo >= 0)
                            {
                                uint lightNo;
                                if (uint.TryParse(faclityTableDTO.InterfaceInfo2, out lightNo))
                                {
                                    Semnox.Core.Utilities.Utilities utils = new Semnox.Core.Utilities.Utilities();
                                    string intType = utils.getParafaitDefaults("RELAY_BOARD_INTERFACE");
                                    if (intType.Equals(CheckInDTO.SnKInterface))
                                        ExternalInterfaces.SNKSnookerControl.SwitchLightOn(portNo, lightNo);
                                    else if (intType.Equals(CheckInDTO.KMTronicInterface))
                                        ExternalInterfaces.KMTronic.SwitchLightOn(portNo, lightNo);

                                    StaticUtils.logPOSDebug("Table " + faclityTableDTO.TableName + " switched on");
                                }
                            }
                        }
                        break;
                    }
                case CheckInDTO.KaraokeTableType:
                    {
                        string ipAddress = faclityTableDTO.InterfaceInfo1.Trim();
                        if (string.IsNullOrEmpty(ipAddress) == false)
                        {
                            int portNo;
                            if (int.TryParse(faclityTableDTO.InterfaceInfo2, out portNo))
                            {
                                if (portNo >= 0)
                                {
                                    string tableNo = faclityTableDTO.InterfaceInfo3.Trim();
                                    if (string.IsNullOrEmpty(tableNo) == false)
                                    {
                                        string message = "";
                                        if (0 != ExternalInterfaces.SNKKaraokeControl.SwitchOn(ipAddress, portNo, tableNo, ref message))
                                        {
                                            log.LogMethodExit(null, "Throwing ApplicationException - "+message);
                                            throw new ApplicationException(message);
                                        }
                                        else
                                            StaticUtils.logPOSDebug("Table " + faclityTableDTO.TableName + " switched on");
                                    }
                                }
                            }
                        }
                        break;
                    }
                default: break;
            }

            log.LogMethodExit(true);
            return true;
        }

        //public static bool SwitchOff(System.Data.DataRow drTableInfo)
        public static bool SwitchOff(FacilityTableDTO faclityTableDTO)
        {
            log.LogMethodEntry(faclityTableDTO);

            switch (faclityTableDTO.TableType)
            {
                case CheckInDTO.PoolTableType:
                    {
                        uint portNo;
                        if (uint.TryParse(faclityTableDTO.InterfaceInfo1, out portNo))
                        {
                            if (portNo >= 0)
                            {
                                uint lightNo;
                                if (uint.TryParse(faclityTableDTO.InterfaceInfo2, out lightNo))
                                {
                                    Semnox.Core.Utilities.Utilities utils = new Semnox.Core.Utilities.Utilities();
                                    string intType = utils.getParafaitDefaults("RELAY_BOARD_INTERFACE");
                                    if (intType.Equals(CheckInDTO.SnKInterface))
                                        ExternalInterfaces.SNKSnookerControl.SwitchLightOff(portNo, lightNo);
                                    else if (intType.Equals(CheckInDTO.KMTronicInterface))
                                        ExternalInterfaces.KMTronic.SwitchLightOff(portNo, lightNo);

                                    StaticUtils.logPOSDebug("Table " + faclityTableDTO.TableName + " switched off");
                                }
                            }
                        }
                        break;
                    }
                case CheckInDTO.KaraokeTableType:
                    {
                        string ipAddress = faclityTableDTO.InterfaceInfo1.Trim();
                        if (string.IsNullOrEmpty(ipAddress) == false)
                        {
                            int portNo;
                            if (int.TryParse(faclityTableDTO.InterfaceInfo2, out portNo))
                            {
                                if (portNo >= 0)
                                {
                                    string tableNo = faclityTableDTO.InterfaceInfo3.Trim();
                                    if (string.IsNullOrEmpty(tableNo) == false)
                                    {
                                        string Message = "";
                                        if (0 != ExternalInterfaces.SNKKaraokeControl.SwitchOff(ipAddress, portNo, tableNo, ref Message))
                                        {
                                            throw new ApplicationException(Message);
                                        }
                                        else
                                            StaticUtils.logPOSDebug("Table " + faclityTableDTO.TableName + " switched off");
                                    }
                                }
                            }
                        }
                        break;
                    }
                default: break;
            }

            log.LogMethodExit(true);
            return true;
        }
    }
}
