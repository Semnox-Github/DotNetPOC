using Nst;
//using Semnox.Parafait.TransactionPayments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.logging;
namespace Semnox.Parafait.Device.PaymentGateway.Menories
{
    internal partial class frmStatus : Form
    {       
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        

        public string errorMessage;
        public bool isExitTrigged = false;
                
        public frmStatus()
        {
            log.LogMethodEntry();

            InitializeComponent();
            isExitTrigged = false;

            log.LogMethodExit(null);
        }
        ~frmStatus()
        {
            log.LogMethodEntry();
            //isExitTrigged = true;
            messageDisplayTimer.Stop();
            log.LogMethodExit(null);
        }

        //private void RunDisplayMessage()
        //{
        //    while (!isExitTrigged)
        //    {
        //        DisplayMessage(errorMessage);
        //        Thread.Sleep(500);
        //    }
        //}
        
        delegate void DisplayText(string message);
        private void DisplayMessage(string message)
        {
            log.LogMethodEntry(message);

            if (this.InvokeRequired)
            {
                DisplayText initializeReader = new DisplayText(DisplayMessage);
                log.Debug("Ends-InitializeReader() method.");
                this.Invoke(initializeReader, new object[] { message });
            }
            else
            {
                if (!this.IsDisposed)
                {
                    txtStatus.Text = message;                    
                }
            }

            log.LogMethodExit(null);
        }
       

        //[SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        //private void KillTheThread()
        //{
        //    log.Error("Starts-KillTheThread() method.");
        //    if (processCommand != null && processCommand.IsAlive)
        //        processCommand.Abort();            
        //    CloseMe();
        //    Thread.Sleep(2000);
        //}
        private delegate void CloseForm();
        public void CloseMe()
        {
            log.LogMethodEntry();
            isExitTrigged = true;
            if (this.InvokeRequired)
            {
                CloseForm closefrm = new CloseForm(CloseMe);
                this.Invoke(closefrm);
            }
            else
            {
                this.Close();                
            }
            log.LogMethodExit(null); 
        }

        private void messageDisplayTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DisplayMessage(errorMessage);
            log.LogMethodExit(null);
        }

        private void frmStatus_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            messageDisplayTimer.Stop();
            //isExitTrigged = true;
            log.LogMethodExit(null);
        }

        private void frmStatus_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            messageDisplayTimer.Start();
            //isExitTrigged = false;
            errorMessage = "Processing..Please follow the instructions on Credit Card device.";
            DisplayMessage("Processing..Please follow the instructions on Credit Card device.");
            log.LogMethodEntry(null);
        }
    }
}
