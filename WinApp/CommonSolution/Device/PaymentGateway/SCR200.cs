using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Net;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Drawing;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public partial class SCR200 : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int _serialPortNo;
        private SerialPort _serialPort;
        public bool _debug = true;
        int cmdSequence = 1;
        const string LogDirectory = ".\\log\\";

        public PaymentExpClsResponseMessageAttributes objresponse = null;
        private ClsStatusMessageAttributes objStatus = null;
        private ClsBasicMessageAttributes basicResponse = null;
        private PaymentExpClsRequestMessageAttributes requestMessage = null;

        Thread MainMessageThread = null;

        List<string> SerialData = new List<string>();
        string LastSentData, PerformAction = "";
        bool cancelPressed = false;
        private Label lblDebugSCR;
        private Label lblStatus;
        private Label lblDebugPOS;

        System.Windows.Forms.Timer SerialActivityTimer = new System.Windows.Forms.Timer();
        public SCR200(int SerialPortNumber, bool Debug = false)
        {
            log.LogMethodEntry(SerialPortNumber, Debug);
            InitializeComponent();

            _debug = Debug;

            _serialPortNo = SerialPortNumber;
            _serialPort = new SerialPort();
            _serialPort.DataReceived += _serialPort_DataReceived;

            SerialActivityTimer.Interval = 5000;
            SerialActivityTimer.Tick += SerialActivityTimer_Tick;
            log.LogMethodExit(null);
        }

        void SerialActivityTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            (sender as System.Windows.Forms.Timer).Stop();
            UpdateMessage("Device not responding. Check connection.");
            if (objresponse != null && cancelPressed)
                objresponse.TrxStatus = "CANCELLED";
            log.LogMethodExit(null);
        }

        public void CloseComm()
        {
            log.LogMethodEntry();
            if (MainMessageThread != null && MainMessageThread.IsAlive)
            {
                PerformAction = "EXIT";
                MainMessageThread.Join();
            }

            if (_serialPort.IsOpen)
                _serialPort.Close();
            log.LogMethodExit(null);
        }

        private void UpdateDebugPOS(string debugMessage)
        {
            log.LogMethodEntry(debugMessage);
            if (lblDebugPOS.InvokeRequired)
                lblDebugPOS.BeginInvoke(new Action(() => lblDebugPOS.Text = debugMessage));
            else
                lblDebugPOS.Text = debugMessage;
            log.LogMethodExit(null);
        }

        private void UpdateDebugSCR(string debugMessage)
        {
            log.LogMethodEntry(debugMessage);
            if (lblDebugSCR.InvokeRequired)
                lblDebugSCR.BeginInvoke(new Action(() => lblDebugSCR.Text = debugMessage));
            else
                lblDebugSCR.Text = debugMessage;
            log.LogMethodExit(null);
        }

        private void UpdateMessage(string StatusMessage)
        {
            log.LogMethodEntry(StatusMessage);
            if (string.IsNullOrEmpty(StatusMessage))
            {
                log.LogMethodExit(null);
                return;
            }


            if (lblMessage.InvokeRequired)
                lblMessage.BeginInvoke(new Action(() => lblMessage.Text = StatusMessage));
            else
                lblMessage.Text = StatusMessage;
            log.LogMethodExit(null);
        }

        public void UpdateStatus(string StatusMessage)
        {
            log.LogMethodEntry(StatusMessage);
            if (lblStatus.InvokeRequired)
                lblStatus.BeginInvoke(new Action(() => lblStatus.Text = StatusMessage));
            else
                lblStatus.Text = StatusMessage;
            log.LogMethodExit(null);
        }

        public bool Initialize()
        {
            log.LogMethodEntry();
            this.Show();

            if (MainMessageThread != null && MainMessageThread.IsAlive)
            {
                log.LogMethodExit(true);
                return true;
            }


            if (_debug)
            {
                lblDebugSCR.Visible =
                lblDebugPOS.Visible = true;
                WindowState = FormWindowState.Normal;
            }
            else
            {
                lblDebugPOS.Visible = lblDebugSCR.Visible = false;
                this.Height = 240 - (lblDebugPOS.Height + lblDebugSCR.Height);
                WindowState = FormWindowState.Minimized;
            }

            UpdateMessage("Initializing...");

            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            foreach (string f in Directory.GetFiles(LogDirectory))
            {
                if (File.GetCreationTime(f) < ServerDateTime.Now.AddDays(-7))
                    File.Delete(f);
            }

            MainMessageLoop();
            log.LogMethodExit(true);
            return true;
        }

        void MainMessageLoop()
        {
            log.LogMethodEntry();
            bool HostReachable = false;

            ThreadStart thr = delegate
            {

                if (_serialPort.IsOpen == false)
                {
                    _serialPort.PortName = "COM" + _serialPortNo.ToString();
                    _serialPort.Parity = Parity.None;
                    _serialPort.DataBits = 8;
                    _serialPort.StopBits = StopBits.One;
                    _serialPort.DtrEnable = true;
                    _serialPort.Handshake = Handshake.None;
                    _serialPort.BaudRate = 115200;

                    int loopCount = 1000;
                    while (loopCount-- > 0)
                    {
                        try
                        {
                            UpdateStatus("Opening Serial Port: " + _serialPortNo.ToString());
                            Application.DoEvents();
                            _serialPort.Open();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured while opening of serial port", ex);
                            UpdateStatus(ex.Message);

                            if (ex.Message.StartsWith("Access to the port")
                                && ex.Message.EndsWith("denied"))
                            {
                                Thread.Sleep(100);
                                continue;
                            }
                        }
                    }
                }

                if (_serialPort.IsOpen == false)
                {
                    log.LogMethodExit(null);
                    return;
                }


                UpdateStatus("Serial port open");
                _serialPort.DiscardInBuffer();
                _serialPort.DiscardOutBuffer();

                bool setdSuccess = false;
                bool txenSuccess = false;
                int retryCount = 1;
                while (true) // main message loop
                {
                    Application.DoEvents();
                    if (PerformAction.Equals("EXIT"))
                    {
                        Application.DoEvents();
                        break;
                    }

                    if (!HostReachable)
                    {
                        if (cancelPressed)
                        {
                            PerformAction = "";
                            TransactionInProgress = false;
                            if (objresponse != null)
                                objresponse.TrxStatus = "CANCELLED";
                            continue;
                        }

                        UpdateMessage("Attempting to reach Host..");
                        HostReachable = Utils.IsReachableUrl("https://www.google.com");
                        if (HostReachable)
                            retryCount = 1;
                    }

                    if (!HostReachable)
                    {
                        UpdateStatus("Host not reachable. Retrying...[" + retryCount++.ToString() + "]");
                        if (retryCount > 100)
                            break;
                        Thread.Sleep(5000);
                        continue;
                    }
                    else
                        Thread.Sleep(800);

                    if (HostReachable && !txenSuccess && SerialData.Count == 0)
                    {
                        EnableTransaction();
                        Thread.Sleep(1000);
                    }
                    else if (!setdSuccess && SerialData.Count == 0)
                    {
                        SetDevice();
                        Thread.Sleep(1000);
                    }

                    Application.DoEvents();

                    if (SerialData.Count == 0)
                    {
                        StatusRequest();
                        continue;
                    }

                    if (_debug)
                        UpdateDebugSCR(SerialData[0]);

                    string[] splitdata = DataSplitter(SerialData[0]);
                    lock (SerialData)
                    {
                        SerialData.RemoveAt(0);
                    }

                    if (splitdata.Length > 1)
                    {
                        basicResponse = new ClsBasicMessageAttributes();
                        if (splitdata.Length > 0)
                            basicResponse.Object = splitdata[0];
                        if (splitdata.Length > 1)
                            basicResponse.Action = splitdata[1];
                        if (splitdata.Length > 2)
                            basicResponse.CmdSeq = splitdata[2];
                        if (splitdata.Length > 3)
                            basicResponse.ResponseCode = splitdata[3];

                        switch (basicResponse.Action)
                        {
                            case "VG":
                                {
                                    writeSerialData(LastSentData); continue;
                                }
                            case "gs1":
                                {
                                    process_gs1(splitdata);
                                    if (objStatus.ResponseCode.Equals("00"))
                                    {
                                        switch (objStatus.Status)
                                        {
                                            case "0": txenSuccess = false; continue;
                                            case "1": setdSuccess = false; continue;
                                            case "2": // idle / ready
                                                {
                                                    if (cancelPressed)
                                                    {
                                                        UpdateMessage("Cancelling...");
                                                        UpdateStatus("Please wait...");
                                                        if (objresponse != null)
                                                            objresponse.TrxStatus = "CANCELLED";
                                                        PerformAction = "";
                                                        continue;
                                                    }

                                                    if (PerformAction.Equals("PURCHASE"))
                                                    {
                                                        PerformAction = "";
                                                        RequestPurchase();

                                                        continue;
                                                    }
                                                    else if (PerformAction.Equals("VOID"))
                                                    {
                                                        VoidLastSale();
                                                        PerformAction = "";
                                                        continue;
                                                    }
                                                    else if (PerformAction.Equals("GETLASTSALE"))
                                                    {
                                                        GetLastSale();
                                                        PerformAction = "";
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        if (objStatus.CardPresent)
                                                            UpdateMessage("REMOVE CARD");
                                                    }
                                                }
                                                break;
                                            case "3": // busy
                                                {
                                                    if (cancelPressed)
                                                    {
                                                        SendCancel();
                                                        UpdateMessage("Cancelling...");
                                                        UpdateStatus("Please wait...");
                                                        continue;
                                                    }
                                                    else
                                                        UpdateStatus("Processing. Please wait...");

                                                    break;
                                                }
                                            default: break;
                                        }
                                    }
                                    break;
                                }
                            case "rx":
                                {
                                    break;
                                }
                            case "tx":
                                {
                                    UpdateMessage("Please wait...");
                                    if (!cancelPressed)
                                    {
                                        writeSerialData("MSG~TX~" + splitdata[2] + "~00~" + (char)13);
                                        if (!SendToHost(splitdata[4], splitdata[2]))
                                        {
                                            HostReachable = false;
                                        }
                                    }
                                    continue;
                                }
                            case "txen":
                                {
                                    txenSuccess = true;
                                    break;
                                }
                            case "setd":
                                {
                                    process_setd(splitdata);
                                    if (basicResponse.ResponseCode.Equals("00"))
                                        setdSuccess = true;
                                    break;
                                }
                            case "pur":
                                {
                                    process_pur(splitdata);
                                    switch (basicResponse.ResponseCode)
                                    {
                                        case "00": GetLastSale(); continue;
                                        case "76": objresponse.TrxStatus = "DECLINED"; break;
                                        case "VB": if (!cancelPressed) objresponse.TrxStatus = "TIMEOUT"; break; //time out
                                        case "VE": // setd not done
                                        case "V6": // read error
                                        case "VZ": if (!cancelPressed) PerformAction = "PURCHASE"; break; // txen not done
                                    }
                                    break;
                                }
                            case "void":
                                {
                                    process_void(splitdata);
                                    switch (basicResponse.ResponseCode)
                                    {
                                        case "00": objresponse.TrxStatus = "APPROVED"; continue;
                                        case "VF": objresponse.TrxStatus = "DECLINED"; continue;
                                    }
                                    break;
                                }
                            case "get1":
                                {
                                    process_get1(splitdata); break;
                                }
                            case "getr":
                                {
                                    process_getr(splitdata); break;
                                }
                            case "pdsp":
                                {
                                    process_pdsp(splitdata); continue;
                                }
                        }

                        if (basicResponse.ResponseCode != null)
                        {
                            switch (basicResponse.ResponseCode)
                            {
                                case "VE": UpdateStatus("Not initialized"); setdSuccess = false; break;
                                case "VL": UpdateStatus("Configuration update needed"); txenSuccess = false; break;
                                case "VZ": UpdateStatus("Cannot execute command – TXEN is not enabled"); txenSuccess = false; break;
                                default: break;
                            }
                        }
                    }
                }
            };
            MainMessageThread = new Thread(thr);
            MainMessageThread.Start();
            log.LogMethodExit(null);
        }

        void writeSerialData(string _msg)
        {

            log.LogMethodEntry(_msg);
            try
            {
                _serialPort.Write(_msg);
                LastSentData = _msg;
                cmdSequence++;
                if (cmdSequence > 899999)
                    cmdSequence = 1;

                if (_debug)
                {
                    UpdateDebugPOS(_msg);
                    logToFile(_msg);
                }
                SerialActivityTimer.Start();
            }
            catch (Exception ex)
            {
                log.Error("Error occured because of serial port write", ex);
                UpdateStatus(ex.Message);
                TransactionInProgress = false;
                try
                {
                    if (MainMessageThread.IsAlive)
                        MainMessageThread.Abort();
                }
                catch (Exception exe)
                {
                    log.Error("Error occured because of main message thread", exe);
                }
            }
            log.LogMethodExit(null);
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SerialActivityTimer.Stop();
            string _msg = "";

            _msg = _serialPort.ReadExisting();
            while (_msg[_msg.Length - 1] != 13)
            {
                _msg = _msg + _serialPort.ReadExisting();
            }

            string[] data = _msg.Split((char)13);

            lock (SerialData)
            {
                foreach (string d in data)
                {
                    if (string.IsNullOrEmpty(d.Trim()) == false)
                    {
                        SerialData.Add(d);
                        if (_debug)
                            logToFile(d);
                    }
                }
            }
            log.LogMethodExit(null);
        }

        void process_setd(string[] responseData)
        {
            log.LogMethodEntry(responseData);
            UpdateStatus(Utils.getReturnMessage(responseData[3]));
            log.LogMethodExit(null);
        }

        void process_gs1(string[] responseData)
        {
            log.LogMethodEntry(responseData);
            if (responseData[3].Equals("00") && _debug == false)
                UpdateStatus("");
            else
                UpdateStatus(Utils.getReturnMessage(responseData[3]));
            objStatus = new ClsStatusMessageAttributes();
            objStatus.ResponseCode = responseData[3];
            objStatus.MessageCount = responseData[4];
            objStatus.CardPresent = responseData[5].Equals("1");
            objStatus.Status = responseData[6];
            objStatus.TxnState = responseData[7];
            objStatus.FirmwarePending = responseData[11];

            if (objStatus.Status != "3")
            {
                string s = Utils.getTxnState(objStatus.TxnState);
                if (!string.IsNullOrEmpty(s))
                    UpdateMessage(s);
                else
                {
                    switch (objStatus.Status)
                    {
                        case "0": UpdateMessage("Configuration data is not downloaded"); break;
                        case "1": UpdateMessage("Device is not initialized (CFG+SETD is needed)"); break;
                        case "2": UpdateMessage("Idle/Ready"); break;
                        case "3": UpdateMessage("Busy. SCR is busy handling other requests."); break;
                        case "4": UpdateMessage("Offline limit exceeded"); break;
                    }
                }
            }
            log.LogMethodExit(null);
        }

        void process_pdsp(string[] responseData)
        {
            log.LogMethodEntry(responseData);
            UpdateMessage(responseData[3] + ' ' + responseData[4]);

            string msg = "DSP" + "~" + "PDSP" + "~" + cmdSequence.ToString() + "~00~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(null);
        }

        void process_pur(string[] responseData)
        {
            log.LogMethodEntry(responseData);
            UpdateMessage(Utils.getReturnMessage(responseData[3]));
            log.LogMethodExit(null);
        }

        void process_void(string[] responseData)
        {
            log.LogMethodEntry(responseData);
            UpdateStatus(Utils.getReturnMessage(responseData[3]));
            log.LogMethodExit(null);
        }

        void process_get1(string[] responseData)
        {
            log.LogMethodEntry(responseData);
            UpdateStatus(Utils.getReturnMessage(responseData[3]));
            if (responseData[3].Equals("00"))
            {
                objresponse.ResponseCode = responseData[3];
                objresponse.TrxStatus = "APPROVED";
                objresponse.CardSuffix = responseData[4];
                objresponse.CardID = responseData[5];
                objresponse.AmountRequested = responseData[6];
                objresponse.AmountAuthorized = responseData[7];
                //objresponse.TxnState = responseData[8];
                objresponse.TxnState = objStatus.TxnState;
                objresponse.Cardnumber2 = responseData[9];
                objresponse.DPSBillingId = responseData[10];
                objresponse.AuthCode = responseData[13];
                objresponse.CardHolderName = responseData[14];
                objresponse.DpsTxnRef = responseData[15];
                objresponse.InvoiceNo = responseData[17];
            }
            log.LogMethodExit(null);
        }

        void process_getr(string[] responseData)
        {
            log.LogMethodEntry(responseData);
            if (responseData[3].Equals("00"))
            {
                PrintReceipt(responseData[4], 30);
            }
            else
            {
                UpdateStatus("GETR error: " + responseData[3]);
            }
            log.LogMethodExit(null);
        }

        void PrintReceipt(string ReceiptText, int Width)
        {
            log.LogMethodEntry(ReceiptText, Width);
            if (string.IsNullOrEmpty(ReceiptText))
            {
                log.LogMethodExit(null);
                return;
            }


            try
            {
                string formattedReceipt = "*-----------EFTPOS-----------*" + Environment.NewLine;
                while (true)
                {
                    if (ReceiptText.Length > Width)
                    {
                        formattedReceipt += ReceiptText.Substring(0, Width) + Environment.NewLine;
                        ReceiptText = ReceiptText.Substring(Width);
                    }
                    else
                    {
                        formattedReceipt += ReceiptText + Environment.NewLine;
                        break;
                    }
                }
                formattedReceipt += "*----------------------------*" + Environment.NewLine;

                System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                printDocument.PrintPage += (sender, args) =>
                {
                    args.Graphics.DrawString(formattedReceipt, new System.Drawing.Font("Courier New", 9), System.Drawing.Brushes.Black, 12, 20);
                };
                printDocument.Print();
            }
            catch (Exception ex)
            {
                log.Error("Error while printing the recipt document", ex);
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            log.LogMethodExit(null);
        }

        public bool SendToHost(string msg, string txnref)
        {
            log.LogMethodEntry(msg, txnref);
            UpdateStatus("Sending To Host...");
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)System.Net.WebRequest.Create(ConfigurationManager.AppSettings["PaymentExpressURL"]);
                webRequest.ContentType = "text/plain";
                webRequest.Method = "POST";
                webRequest.KeepAlive = true;
                webRequest.Timeout = 15000;
                webRequest.UserAgent = "SKScrApp";
                string scrData = "<MifXmlMessage action=\"doScr\"><ScrData>" + msg + "</ScrData><ScrTxnRef>" + txnref + "</ScrTxnRef></MifXmlMessage>";
                webRequest.ContentLength = scrData.Length;
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(scrData);
                Stream requeststream = webRequest.GetRequestStream();
                requeststream.Write(bytes, 0, bytes.Length);
                requeststream.Close();

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                StreamReader postreqreader = new StreamReader(webResponse.GetResponseStream());
                string data = postreqreader.ReadToEnd();

                webResponse.Close();
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(data);
                string fromHost = "MSG~RX~" + xml.GetElementsByTagName("ScrTxnRef")[0].InnerText + "~" + xml.GetElementsByTagName("ScrData")[0].InnerText + "~" + (char)13;

                string _msg = fromHost;

                writeSerialData(fromHost);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while http web request", ex);
                UpdateStatus(ex.Message);
                log.LogMethodExit(false);
                return false;
            }
        }

        private string[] DataSplitter(string msg)
        {
            log.LogMethodEntry(msg);
            string[] returnvalue = msg.Split('~');
            log.LogMethodExit(returnvalue);
            return returnvalue;
        }

        void SendCancel()
        {
            log.LogMethodEntry();
            string msg = "STS" + "~" + "BTN" + "~" + cmdSequence.ToString() + "~X~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(null);
        }

        void StatusRequest()
        {
            log.LogMethodEntry();
            string msg = "STS" + "~" + "GS1" + "~" + cmdSequence.ToString() + "~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(null);
        }

        void RequestPurchase()
        {
            log.LogMethodEntry();
            string msg = "TXN" + "~" + "PUR" + "~" + requestMessage.InvoiceNo + "~" + requestMessage.Amount + "~" + requestMessage.InvoiceNo + "~" + requestMessage.InvoiceNo + "~~" + "0" + "~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(null);
        }

        void SetDevice()
        {
            log.LogMethodEntry();
            string eventMask = "3";
            string enableOffline = "0";
            string msg = "CFG" + "~" + "SETD" + "~" + cmdSequence.ToString() + "~"
                + ConfigurationManager.AppSettings["DeviceID"] + "~"
                + ConfigurationManager.AppSettings["CurrencyCode"] + "~"
                + ConfigurationManager.AppSettings["POSProtocol"] + "~"
                + ConfigurationManager.AppSettings["VendorID"] + "~"
                + eventMask + "~"
                + enableOffline + "~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(null);
        }

        void EnableTransaction()
        {
            log.LogMethodEntry();
            string msg = "MSG" + "~" + "TXEN" + "~" + cmdSequence.ToString() + "~" + "1" + "~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(null);
        }

        bool VoidLastSale()
        {
            log.LogMethodEntry();
            string msg = "TXN" + "~" + "VOID" + "~" + cmdSequence.ToString() + "~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(true);
            return true;
        }

        void GetLastSale()
        {
            log.LogMethodEntry();
            string msg = "TXN" + "~" + "GET1" + "~" + cmdSequence.ToString() + "~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(null);
        }

        void GetReceipt()
        {
            log.LogMethodEntry();
            string msg = "TXN" + "~" + "GETR" + "~" + cmdSequence.ToString() + "~1~20~~" + (char)13;

            writeSerialData(msg);
            log.LogMethodExit(null);
        }

        bool TransactionInProgress = false;
        public bool PerformSale(object TrxRef, double Amount)
        {
            log.LogMethodEntry(TrxRef, Amount);
            if (MainMessageThread == null || MainMessageThread.IsAlive == false)
                Initialize();

            if (MainMessageThread == null || MainMessageThread.IsAlive == false)
            {
                log.LogMethodExit(false);
                return false;
            }


            TransactionInProgress = true;
            WindowState = FormWindowState.Normal;
            this.Activate();
            requestMessage = new PaymentExpClsRequestMessageAttributes();
            requestMessage.Amount = Convert.ToInt32(Amount * 100).ToString();
            requestMessage.InvoiceNo = TrxRef.ToString();
            PerformAction = "PURCHASE";
            objresponse = new PaymentExpClsResponseMessageAttributes();
            objresponse.TrxStatus = "";

            bool retVal = false;
            int loopCount = 1;
            while (true) // wait until status is obtained
            {
                if (objresponse.TrxStatus.Equals("APPROVED"))
                {
                    retVal = true;
                    GetReceipt();
                    break;
                }
                else if (objresponse.TrxStatus.Equals("DECLINED"))
                {
                    retVal = false;
                    GetReceipt();
                    break;
                }
                else if (objresponse.TrxStatus.Equals("CANCELLED"))
                {
                    retVal = false;
                    break;
                }
                else if (objresponse.TrxStatus.Equals("TIMEOUT"))
                {
                    retVal = false;
                    break;
                }
                else if (MainMessageThread.IsAlive == false)
                {
                    objresponse.TrxStatus = "Not Initialized";
                    retVal = false;
                    break;
                }

                loopCount++;
                if (loopCount > 1000)
                    break;

                Application.DoEvents();
                Thread.Sleep(100);
            }

            TransactionInProgress = false;
            if (loopCount > 1000)
            {
                objresponse.TxnState = "TIMEOUT";
                if (GetLastTransaction())
                {
                    if (objresponse.InvoiceNo.Equals(TrxRef.ToString()))
                    {
                        if (objresponse.TxnState.Equals("APPROVED"))
                        {
                            log.LogMethodExit(true);
                            return true;
                        }

                        else
                        {
                            UpdateMessage("Transaction failed");
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                    else
                    {
                        log.LogMethodExit(false);
                        return false;
                    }

                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }

            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                log.LogMethodExit(retVal);
                return retVal;
            }

        }

        public bool VoidSale()
        {
            log.LogMethodEntry();
            if (MainMessageThread == null || MainMessageThread.IsAlive == false)
                Initialize();

            if (MainMessageThread == null || MainMessageThread.IsAlive == false)
            {
                log.LogMethodExit(false);
                return false;
            }


            TransactionInProgress = true;
            WindowState = FormWindowState.Normal;
            this.Activate();
            PerformAction = "VOID";
            objresponse = new PaymentExpClsResponseMessageAttributes();
            objresponse.TrxStatus = "";
            bool retVal = false;

            int loopCount = 1;
            while (true) // wait until status is obtained
            {
                if (objresponse.TrxStatus.Equals("APPROVED"))
                {
                    retVal = true;
                    GetReceipt();
                    break;
                }
                else if (objresponse.TrxStatus.Equals("DECLINED"))
                {
                    retVal = false;
                    break;
                }
                else if (objresponse.TrxStatus.Equals("CANCELLED"))
                {
                    retVal = false;
                    break;
                }
                else if (objresponse.TrxStatus.Equals("TIMEOUT"))
                {
                    retVal = false;
                    break;
                }
                else if (MainMessageThread.IsAlive == false)
                {
                    objresponse.TrxStatus = "Not Initialized";
                    retVal = false;
                    break;
                }

                loopCount++;
                if (loopCount > 1000)
                    break;

                Application.DoEvents();
                Thread.Sleep(100);
            }

            TransactionInProgress = false;
            if (loopCount > 1000)
            {
                objresponse.TxnState = "TIMEOUT";
                if (GetLastTransaction())
                {
                    if (objresponse.TxnState.Equals("7"))
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        UpdateMessage("Transaction failed");
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                log.LogMethodExit(retVal);
                return retVal;
            }
        }

        public bool GetLastTransaction()
        {
            log.LogMethodEntry();
            if (MainMessageThread == null || MainMessageThread.IsAlive == false)
                Initialize();

            if (MainMessageThread == null || MainMessageThread.IsAlive == false)
            {
                log.LogMethodExit(false);
                return false;
            }


            TransactionInProgress = true;
            requestMessage = new PaymentExpClsRequestMessageAttributes();
            PerformAction = "GETLASTSALE";
            objresponse = new PaymentExpClsResponseMessageAttributes();
            objresponse.TrxStatus = "";

            int loopCount = 1;
            bool retVal = false;
            while (true) // wait until status is obtained
            {
                if (objresponse.TrxStatus.Equals("APPROVED"))
                {
                    retVal = true;
                    break;
                }
                else if (objresponse.TrxStatus.Equals("DECLINED"))
                {
                    retVal = false;
                    break;
                }
                else if (objresponse.TrxStatus.Equals("CANCELLED"))
                {
                    retVal = false;
                    break;
                }
                else if (objresponse.TrxStatus.Equals("TIMEOUT"))
                {
                    retVal = false;
                    break;
                }
                else if (MainMessageThread.IsAlive == false)
                {
                    objresponse.TrxStatus = "Not Initialized";
                    retVal = false;
                    break;
                }

                loopCount++;
                if (loopCount > 1000)
                    break;

                Application.DoEvents();
                Thread.Sleep(100);
            }

            TransactionInProgress = false;
            if (loopCount > 1000)
            {
                objresponse.TxnState = "TIMEOUT";
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                log.LogMethodExit(retVal);
                return retVal;
            }

        }

        #region UI
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            log.LogMethodEntry(disposing);
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            log.LogMethodExit(null);
        }

        private void InitializeComponent()
        {
            log.LogMethodEntry();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.lblDebugPOS = new System.Windows.Forms.Label();
            this.lblDebugSCR = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Black;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(560, 46);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Message";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.Black;
            this.btn_Cancel.Location = new System.Drawing.Point(200, 160);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(160, 36);
            this.btn_Cancel.TabIndex = 66;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // lblDebugPOS
            // 
            this.lblDebugPOS.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDebugPOS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDebugPOS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebugPOS.ForeColor = System.Drawing.Color.Black;
            this.lblDebugPOS.Location = new System.Drawing.Point(0, 92);
            this.lblDebugPOS.Name = "lblDebugPOS";
            this.lblDebugPOS.Size = new System.Drawing.Size(560, 30);
            this.lblDebugPOS.TabIndex = 67;
            this.lblDebugPOS.Text = "DebugPOS";
            this.lblDebugPOS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDebugPOS.Visible = false;
            // 
            // lblDebugSCR
            // 
            this.lblDebugSCR.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblDebugSCR.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDebugSCR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDebugSCR.ForeColor = System.Drawing.Color.Black;
            this.lblDebugSCR.Location = new System.Drawing.Point(0, 122);
            this.lblDebugSCR.Name = "lblDebugSCR";
            this.lblDebugSCR.Size = new System.Drawing.Size(560, 30);
            this.lblDebugSCR.TabIndex = 68;
            this.lblDebugSCR.Text = "DebugSCR";
            this.lblDebugSCR.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDebugSCR.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.Black;
            this.lblStatus.Location = new System.Drawing.Point(0, 47);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(560, 46);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "Status";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SCR200
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 201);
            this.ControlBox = false;
            this.Controls.Add(this.lblDebugSCR);
            this.Controls.Add(this.lblDebugPOS);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btn_Cancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SCR200";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - this.Width / 2, 100);
            ; this.Text = "PaymentExpress Status";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SCR200_FormClosing);
            this.ResumeLayout(false);
            log.LogMethodExit(null);
        }

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btn_Cancel;

        #endregion

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ThreadStart thr = delegate
            {
                if (cancelPressed)
                {
                    log.LogMethodExit(null);
                    return;
                }

                cancelPressed = true;
                UpdateMessage("Cancelling...");
                while (TransactionInProgress)
                    Thread.Sleep(100);

                cancelPressed = false;

                this.Invoke(new Action(() => this.WindowState = FormWindowState.Minimized));
            };

            new Thread(thr).Start();
            log.LogMethodExit(null);
        }

        private void SCR200_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (MainMessageThread != null && MainMessageThread.IsAlive)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
            log.LogMethodExit(null);
        }

        string logFile = LogDirectory + "SCR200-" + ServerDateTime.Now.ToString("dd-MMM-yyyy") + ".log";
        public void logToFile(string logText)
        {
            log.LogMethodEntry(logText);
            lock (LogDirectory)
                File.AppendAllText(logFile, ServerDateTime.Now.ToString("h:mm:ss tt") + ": " + logText + Environment.NewLine);
            log.LogMethodExit(null);
        }
    }
}
