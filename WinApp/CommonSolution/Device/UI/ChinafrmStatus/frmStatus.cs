/********************************************************************************************
 * Project Name - Status Form UI
 * Description  - This is the UI which process the request
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Apr-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 using Semnox.Core.Utilities;
using System.Threading;
using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace Semnox.Parafait.Device.PaymentGateway.ChinaUMSUI
{
    internal partial class frmStatus : Form
    {
        [DllImport(@"posinf.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"bankall")]//
        static extern long bankall(StringBuilder request, StringBuilder response);

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string responseString;
        public string requestLRC = "";
        long returnCode;
        public StringBuilder requestStringSB;
        public ChinaUMSTransactionResponse transactionResponse;
        StringBuilder responseStringSB = new StringBuilder();
        Utilities utilities;
        Thread myThread;
        int exitCount = 90;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities"></param>
        public frmStatus(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);

            InitializeComponent();
            this.Size = new Size(0, 0);
            utilities = _Utilities;
            responseString = utilities.MessageUtils.getMessage("Processing Payment...");
            this.TopMost = true;

            log.LogMethodExit(null);
        }
        public void refreshMessage()
        {
            log.LogMethodEntry();

            txtStatus.Text = responseString;
            txtStatus.Refresh();

            log.LogMethodExit(null);
        }

        private void ThreadTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (exitCount <= 0)
            {
                responseString = utilities.MessageUtils.getMessage("Gateway is not responding...");
                refreshMessage();
                ThreadTimer.Stop();
                this.DialogResult = DialogResult.Cancel;
                KillTheThread();
                Thread.Sleep(3000);
                this.Close();
            }
            if ((responseStringSB != null && !string.IsNullOrEmpty(responseStringSB.ToString())) || returnCode != 0)
            {
                string response = responseStringSB.ToString();
                if (!string.IsNullOrEmpty(response))
                {
                    transactionResponse = ConvertResponse(response.ToString());
                    transactionResponse.ReturnCode = returnCode;                    
                    if (transactionResponse.ResponseCode.Equals("00"))
                    {
                        responseString = utilities.MessageUtils.getMessage("APPROVED");
                    }
                    else
                    {
                        responseString = utilities.MessageUtils.getMessage("ERROR");
                    }
                }
                else
                {
                    responseString = utilities.MessageUtils.getMessage("Response is invalid.");
                    transactionResponse = null;
                }
                refreshMessage();
                ThreadTimer.Stop();
                this.DialogResult = DialogResult.OK;
                KillTheThread();
                Thread.Sleep(3000);
                this.Close();
            }
            exitCount--;

            log.LogMethodExit(null);
        }

        private void SendPaymentRequest()
        {
            log.LogMethodEntry();

            try
            {
                returnCode = bankall(requestStringSB, responseStringSB);
                log.Debug("Ends- SendPaymentRequest() thread");
            }
            catch(Exception e)
            {
                log.Error("Error occured while sending payment request", e);           
                exitCount = -1;
            }

            //Thread.Sleep(120 * 1000);
            log.LogMethodExit(null);
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        private void KillTheThread()
        {
            log.LogMethodEntry();

            myThread.Abort();

            log.LogMethodExit(null);
        }

        private void frmStatus_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);            
            myThread = new Thread(SendPaymentRequest);
            ThreadTimer.Interval = 1000;
            myThread.Start();
            ThreadTimer.Start();

            log.LogMethodExit(null);
        }

        private void frmStatus_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            ThreadTimer.Stop();
            KillTheThread();

            log.LogMethodExit(null);
        }
        private void StringToByte(string cmdstring, ref byte[] CmdArray)
        {
            log.LogMethodEntry(cmdstring, CmdArray);

            CmdArray = new byte[cmdstring.Length];            
            CmdArray = Encoding.Default.GetBytes(cmdstring);

            log.LogMethodExit(null);
        }
        private ChinaUMSTransactionResponse ConvertResponse(string umsResponse)
        {
            log.LogMethodEntry(umsResponse);

            ChinaUMSTransactionResponse transactionResponse = new ChinaUMSTransactionResponse();
            try
            {
                byte[] byteResponse=null;
                StringToByte(umsResponse, ref byteResponse);
                //if(System.IO.File.Exists(".\\test.txt"))//This is to see the response in test .txt file
                // System.IO.File.Delete(".\\test.txt");
                //string[] s=new string[1]{umsResponse} ;
                //System.IO.File.WriteAllLines(".\\test.txt", s);
                if (string.IsNullOrEmpty(umsResponse))
                {
                    log.LogMethodExit(null);
                    return null;
                }

                transactionResponse.ResponseCode = ByteTostring(byteResponse.Take(2).ToArray());//umsResponse.Substring(0, 2);//EndIndex=1, EI=StartIndex+lenght-1
                transactionResponse.BankId = ByteTostring(byteResponse.Skip(2).Take(5).ToArray());//umsResponse.Substring(2, 4);//EI=5
                transactionResponse.CardNo = ByteTostring(byteResponse.Skip(6).Take(20).ToArray());//umsResponse.Substring(6, 20);//EI=25
                transactionResponse.DraftNo = ByteTostring(byteResponse.Skip(26).Take(6).ToArray());//umsResponse.Substring(26, 6);//EI=31
                long.TryParse(ByteTostring(byteResponse.Skip(32).Take(12).ToArray()), out transactionResponse.TransactionAmount);//umsResponse.Substring(32, 12)//EI=43
                transactionResponse.MistakesExplanation = ByteTostring(byteResponse.Skip(44).Take(40).ToArray());//umsResponse.Substring(44, 40);//EI=83
                transactionResponse.MerchantId = ByteTostring(byteResponse.Skip(84).Take(15).ToArray());//umsResponse.Substring(84, 15);//EI=98
                transactionResponse.TerminalID = ByteTostring(byteResponse.Skip(99).Take(8).ToArray());//umsResponse.Substring(99, 8);//EI=106
                transactionResponse.BatchNo = ByteTostring(byteResponse.Skip(107).Take(6).ToArray());//umsResponse.Substring(107, 6);//EI=112
                transactionResponse.TransactionDate = ByteTostring(byteResponse.Skip(113).Take(4).ToArray());//umsResponse.Substring(113, 4);//EI=116
                transactionResponse.TransactionTime = ByteTostring(byteResponse.Skip(117).Take(6).ToArray());//umsResponse.Substring(117, 6);//EI=122
                transactionResponse.ReferenceNo = ByteTostring(byteResponse.Skip(123).Take(12).ToArray());//umsResponse.Substring(123, 12);//EI=134
                transactionResponse.AuthorizationNo = ByteTostring(byteResponse.Skip(135).Take(6).ToArray());//umsResponse.Substring(135, 6);//EI=140
                transactionResponse.SettlementDate = ByteTostring(byteResponse.Skip(141).Take(4).ToArray());//umsResponse.Substring(141, 4);//EI=144
                transactionResponse.CardType = ByteTostring(byteResponse.Skip(145).Take(3).ToArray());//umsResponse.Substring(145, 3);//EI=147
                transactionResponse.LRC = ByteTostring(byteResponse.Skip(148).Take(3).ToArray());//umsResponse.Substring(148, 3);//EI=150
                log.Debug("Ends-ConvertResponse() method");
            }
            catch (Exception ex)
            {
                log.Error("Error occured while converting response", ex);
            }

            log.LogMethodExit(transactionResponse);
            return transactionResponse;
        }

        private string ByteTostring(byte[] byteData)
        {
            log.LogMethodEntry(byteData);

            string stringData="";
            stringData=Encoding.Default.GetString(byteData);

            log.LogMethodExit(stringData);        
            return stringData;
        }
    }
}
