using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Semnox.Parafait.Communication
{
    public class NetUtils
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

        public class IPMacEntry
        {
            public string MacAddress;
            public IPAddress ipAddress;

            public IPMacEntry(string mac, IPAddress ip)
            {
                log.LogMethodEntry(mac, ip);
                MacAddress = mac;
                ipAddress = ip;
                log.LogMethodExit(null);
            }
        }
        List<IPAddress> IPList = new List<IPAddress>();
        public List<IPMacEntry> IPMacTable = new List<IPMacEntry>();
        int _pingTimeOut = 200;

        public IPAddress getIPAddress(string macAddress)
        {
            log.LogMethodEntry(macAddress);
            try
            {
                macAddress = macAddress.Trim().Replace('-', ':').ToUpper();
                IPMacEntry ipMacFound = IPMacTable.Find(delegate(IPMacEntry ipMac) { return (ipMac.MacAddress == macAddress); });
                if (ipMacFound != null)
                {
                    log.LogMethodExit((ipMacFound.ipAddress));
                    return (ipMacFound.ipAddress);
                }
                    
                else
                {
                    log.LogMethodExit(null);
                    return null;
                }
                   
            }
            catch(Exception ex)
            {
                log.Error("Error occured while calculating ip address", ex);
                log.LogMethodExit(null);
                return null;
            }
        }

        void addEntry(string MacAddress, IPAddress ipAddress)
        {
            log.LogMethodEntry(MacAddress, ipAddress);
            IPMacEntry ipMac = new IPMacEntry(MacAddress, ipAddress);
            IPMacTable.Add(ipMac);
            log.LogMethodExit(null);
        }

        void updateEntry(string MacAddress, IPAddress ipAddress)
        {
            log.LogMethodEntry(MacAddress, ipAddress);
            lock (IPMacTable)
            {
                IPMacEntry ipMacFound = IPMacTable.Find(delegate(IPMacEntry ipMac) { return (ipMac.MacAddress == MacAddress); });
                if (ipMacFound == null)
                    addEntry(MacAddress, ipAddress);
                else
                    ipMacFound.ipAddress = ipAddress;
            }
            log.LogMethodExit(null);
        }

        public void clearMacIPTable()
        {
            log.LogMethodEntry();
            IPMacTable.Clear();
            log.LogMethodExit(null);
        }

        public void ScanNetwork()
        {
            log.LogMethodEntry();
            ScanNetwork("");
            log.LogMethodExit(null);
        }

        public void ScanNetwork(string Mask, int PingTimeOut = 200)
        {
            log.LogMethodEntry(Mask, PingTimeOut);
            _pingTimeOut = PingTimeOut;
            if (IPMacTable == null)
                IPMacTable = new List<IPMacEntry>();

            if (IPList.Count == 0)
            {
                System.Net.IPAddress[] TempAd;

                if (Mask != "")
                {
                    string[] masks = Mask.Split('|');
                    TempAd = new IPAddress[masks.Length];
                    int i = 0;
                    foreach (string mask in masks)
                        TempAd[i++] = IPAddress.Parse(mask.ToLower().Replace('x', '1'));
                }
                else
                {
                    IPHostEntry server = Dns.GetHostEntry(Dns.GetHostName());
                    TempAd = server.AddressList;
                }

                for (int ipIndex = 0; ipIndex < TempAd.Length; ipIndex++)
                {
                    if (TempAd[ipIndex].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        string ipBase = TempAd[ipIndex].ToString();
                        try
                        {
                            ipBase = ipBase.Substring(0, ipBase.LastIndexOf('.'));
                        }
                        catch(Exception  ex)
                        {
                            log.Error("Error while calculating ipBase", ex);
                            log.LogMethodExit(null, "Throwing Application exception-ScanNetwork(): Invalid IP Mask" + ipBase);
                            throw new ApplicationException("ScanNetwork(): Invalid IP Mask " + ipBase);
                        }

                        for (int i = 0; i < 256; i++)
                        {
                            IPList.Add(IPAddress.Parse(ipBase + "." + i.ToString()));
                        }
                    }
                }
            }

            using (var finished = new CountdownEvent(1))
            {
                foreach (IPAddress ip in IPList)
                {
                    while (finished.CurrentCount > 128)
                        Thread.Sleep(100);
                    
                    finished.AddCount();

                    Thread t = new Thread(() =>
                    {
                        try
                        {
                            findAndAdd(ip);
                            finished.Signal();
                        }
                        catch(Exception ex)
                        {
                            log.Error("Error occured while scanning the network", ex);
                        }
                    });
                    t.Start();
                }

                finished.Signal();
                finished.Wait();
            }
            log.LogMethodExit(null);
        }

        void findAndAdd(object ipO)
        {
            log.LogMethodEntry(ipO);
            try
            {
                IPAddress ip = ipO as IPAddress;
                using (Ping p = new Ping())
                {
                    int pingTryCount = 30;
                    int pingFailCount = 0;
                    while (pingTryCount-- > 0)
                    {
                        PingReply pr = p.Send(ip, _pingTimeOut);
                        if (pr.Status == IPStatus.Success)
                        {
                            pingFailCount = 0;
                            string macAddr = GetMacAddress(ip);
                            if (string.IsNullOrEmpty(macAddr) == false)
                            {
                                macAddr = macAddr.Trim().Replace('-', ':').ToUpper();
                                updateEntry(macAddr, ip);
                                break;
                            }
                        }
                        else
                        {
                            if (++pingFailCount > 6) // if 6 pings fail, remove the ip entry from table
                            {
                                IPMacTable.RemoveAll(delegate(IPMacEntry ipMac) { return (ipMac.ipAddress == ip); });
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing find and add", ex);
            }
            log.LogMethodExit(null);
        }

        public static string GetMacAddress(string hostName)
        {
            log.LogMethodEntry(hostName);
            string s = string.Empty;
            System.Net.IPHostEntry Tempaddr = null;
            Tempaddr = (System.Net.IPHostEntry)Dns.GetHostEntry(hostName);
            System.Net.IPAddress[] TempAd = Tempaddr.AddressList;
            string[] Ipaddr = new string[3];
            foreach (IPAddress TempA in TempAd)
            {
                Ipaddr[1] = TempA.ToString();
                byte[] ab = new byte[6];
                int len = ab.Length;
                if (SendARP(BitConverter.ToInt32(TempA.GetAddressBytes(), 0), 0, ab, ref len) == 0)
                {
                    string sMAC = BitConverter.ToString(ab, 0, 6);
                    Ipaddr[2] = sMAC;
                    s = sMAC;
                    break;
                }
            }
            log.LogMethodExit(s);
            return s;
        }

        public static string GetMacAddress(IPAddress ipAddress)
        {
            log.LogMethodEntry(ipAddress);
            if (ipAddress == null)
            {
                log.LogMethodExit("");
                return "";
            }
               
            string s = string.Empty;
            byte[] ab = new byte[6];
            int len = ab.Length;
            if (SendARP(BitConverter.ToInt32(ipAddress.GetAddressBytes(), 0), 0, ab, ref len) == 0)
            {
                s = BitConverter.ToString(ab, 0, 6);
            }
            log.LogMethodExit(s);
            return s;
        }
    }
}
