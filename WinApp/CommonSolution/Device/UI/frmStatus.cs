/********************************************************************************************
 * Project Name - Status Form UI
 * Description  - This is the UI which process the request
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Aug-2017   Raghuveera          Created 
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
namespace Semnox.Parafait.Device.PaymentGateway
{
    internal partial class frmStatus : Form
    {
        [DllImport(@"KeeperClient.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Ansi, EntryPoint = @"misposTrans")]//
        static extern int misposTrans(ref ChinaICBCTransactionRequest.ICBCInputStruct input, out ChinaICBCTransactionResponse.ICBCOutputStruct output);

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string responseString;        
        int returnCode = -10;
        ChinaICBCTransactionRequest transactionRequest;
        public ChinaICBCTransactionResponse transactionResponse;        
        Utilities utilities;
        Thread myThread;
        int exitCount = 240;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities">Utilities class object.</param>
        /// <param name="transRequest">ChinaICBCTransactionRequest class object.</param>
        public frmStatus(Utilities _Utilities, ChinaICBCTransactionRequest transRequest)
        {
            log.LogMethodEntry(_Utilities, transRequest);

            InitializeComponent();
            utilities = _Utilities;
            transactionRequest = transRequest;
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
                log.LogVariableState("exitCount", exitCount);
                responseString = utilities.MessageUtils.getMessage("Gateway is not responding...");
                refreshMessage();
                ThreadTimer.Stop();
                this.DialogResult = DialogResult.Cancel;
                transactionResponse = null;
                KillTheThread();
                Thread.Sleep(3000);
                this.Close();
            }
            if (transactionResponse != null && returnCode != -10)
            {
                if (returnCode == 0)
                {
                    transactionResponse.GetClass();
                    if (!string.IsNullOrEmpty(transactionResponse.RspCode))
                    {
                        log.LogVariableState("transactionResponse", transactionResponse);
                        if (transactionResponse.RspCode.Equals("00"))
                        {
                            log.Debug("Entered APPROVED.");
                            responseString = utilities.MessageUtils.getMessage("APPROVED");
                        }
                        else
                        {
                            log.Debug("Entered ERROR.");
                            responseString = utilities.MessageUtils.getMessage("ERROR");
                        }
                    }
                    else
                    {
                        responseString = utilities.MessageUtils.getMessage("Response is invalid.");
                        transactionResponse = null;
                    }
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    transactionResponse.GetClass();
                    responseString = transactionResponse.RspMessage;
                    this.DialogResult = DialogResult.Cancel;
                }
                refreshMessage();
                ThreadTimer.Stop();

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
                transactionRequest.SetStructure();
                transactionResponse = new ChinaICBCTransactionResponse();
                returnCode = misposTrans(ref transactionRequest.requestStruct, out transactionResponse.responseStruct);
                log.LogVariableState("transactionResponse.responseStruct", transactionResponse.responseStruct);
            }
            catch(Exception ex)
            {
                log.Error("Error occured while sending the payment request", ex);      
                exitCount = -1;
            }

            log.LogMethodExit(null);
            //Thread.Sleep(120 * 1000);
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
    }
}
