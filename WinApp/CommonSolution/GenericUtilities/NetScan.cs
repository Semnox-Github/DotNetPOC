using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;

namespace Semnox.Core.GenericUtilities
{
    public sealed class NetScan
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        static extern int SendARP(int DestIP, int SrcIP, [Out] byte[] pMacAddr, ref int PhyAddrLen);

        private static readonly NetScan instance = new NetScan();
        static NetScan()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        private NetScan()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public static NetScan Instance
        {
            get
            {
                return instance;
            }
        }

        public List<IPMacEntry> IPMacTable = null;
        static int _pingTimeOut;
        static int _scanFrequency;

        public class IPMacEntry
        {
            public string MacAddress;
            public IPAddress ipAddress;
            public bool PingSuccess = false;
            bool _running = true;

            public IPMacEntry(IPAddress ip)
            {
                log.LogMethodEntry("ip");
                ipAddress = ip;

                new Thread(() =>
                {
                    Thread.Sleep(100 * ipAddress.GetAddressBytes()[3]); // for delayed start for each ip thread, to reduce concurrent overload

                    const int IPChangeCheckFrequency = 10 * 60 * 1000; // 10 minutes
                    int ipChangeCounter = IPChangeCheckFrequency;
                    while (_running)
                    {
                        if (PingSuccess) // valid ip address
                        {
                            if (string.IsNullOrEmpty(MacAddress)) // mac not yet populated
                            {
                                int retries = 30;
                                do // attempt to get mac address through sendARP. repeat if not got, along with pings
                                {
                                    string macAddr = GetMacAddress(ipAddress);
                                    if (string.IsNullOrEmpty(macAddr) == false)
                                    {
                                        MacAddress = macAddr;
                                        ipChangeCounter = IPChangeCheckFrequency;
                                        break;
                                    }
                                    pingTest(ipAddress);
                                } while (retries-- > 0);
                            }
                            else // mac is already present. check connectivity through ping 
                            {
                                for (int i = 0; i < _scanFrequency & _running; i += 100)
                                    Thread.Sleep(100);
                                pingTest(ipAddress, 6);
                                ipChangeCounter -= _scanFrequency;
                                if (ipChangeCounter <= 0 && PingSuccess) // check if ip has changed once in 10 minutes
                                {
                                    ipChangeCounter = IPChangeCheckFrequency;
                                    string macAddr = GetMacAddress(ipAddress);
                                    if (string.IsNullOrEmpty(macAddr) == false)
                                    {
                                        MacAddress = macAddr;
                                    }
                                }
                            }
                        }
                        else
                        {
                            pingTest(ipAddress);
                            if (!PingSuccess)
                            {
                                for (int i = 0; i < _scanFrequency & _running; i += 100)
                                    Thread.Sleep(100);
                            }
                        }
                    }

                }).Start();
                log.LogMethodExit();
            }

            public void Stop()
            {
                log.LogMethodEntry();
                _running = false;
                log.LogMethodExit();
            }

            ~IPMacEntry()
            {
                log.LogMethodEntry();
                Stop();
                log.LogMethodExit();
            }

            string GetMacAddress(IPAddress ipAddress)
            {
                log.LogMethodEntry("ipAddress");
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
                    s = s.Trim().Replace('-', ':').ToUpper();
                }
                log.LogMethodExit(s);
                return s;
            }

            void pingTest(object ipO, int pingTryCount = 1)
            {
                log.LogMethodEntry(ipO, pingTryCount);
                PingSuccess = false;
                try
                {
                    IPAddress ip = ipO as IPAddress;
                    using (Ping p = new Ping())
                    {
                        while (pingTryCount-- > 0)
                        {
                            PingReply pr = p.Send(ip, NetScan._pingTimeOut);
                            if (pr.Status == IPStatus.Success)
                            {
                                PingSuccess = true;
                                log.LogMethodExit();
                                return;
                            }
                        }

                        this.MacAddress = null;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error while calculating ping test", ex);
                }
                log.LogMethodExit();
            }
        }

        public IPAddress getIPAddress(string macAddress)
        {
            log.LogMethodEntry("macAddress");
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
                    log.LogMethodExit();
                    return null;
                }
                    
            }
            catch(Exception ex)
            {
                log.Error("Error while calculating ip address", ex);
                log.LogMethodExit();
                return null;
            }
          
        }

        public void StopScan()
        {
            log.LogMethodEntry();
            if (IPMacTable != null)
            {
                foreach (IPMacEntry ipMac in IPMacTable)
                    ipMac.Stop();

                IPMacTable.Clear();
            }
            log.LogMethodExit();
        }

        public void ScanNetwork(string Mask, int PingTimeOut, int ScanFrequency)
        {
            log.LogMethodEntry(Mask, PingTimeOut, ScanFrequency);
            _pingTimeOut = Math.Max(PingTimeOut, 100);
            _scanFrequency = Math.Max(ScanFrequency * 1000, 30000);

            if (IPMacTable == null)
                IPMacTable = new List<IPMacEntry>();
            else
            {
                StopScan();
                int sleep = Mask.Split('|').Length * (PingTimeOut + 2000);
                while (sleep > 0)
                {
                    Thread.Sleep(100);
                    sleep -= 100;
                }
            }

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
                    catch(Exception ex)
                    {
                        log.Error("Error occured while calculating ipBase", ex);
                        log.LogMethodExit("Throwing application exception -ScanNetwork(): Invalid IP Mask" + ipBase);
                        throw new ApplicationException("ScanNetwork(): Invalid IP Mask " + ipBase);
                    }

                    for (int i = 1; i < 255; i++)
                    {
                        IPMacEntry ipMac = new IPMacEntry(IPAddress.Parse(ipBase + "." + i.ToString()));
                        IPMacTable.Add(ipMac);
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
