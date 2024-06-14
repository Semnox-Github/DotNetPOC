using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
//using Semnox.Mercury.PaymentGateway;
using System.Configuration;
using System.Timers;
using System.Globalization;
using System.IO;
using iTextSharp.text.pdf;
using Parafait_POS.Redemption;
using Semnox.Core.Utilities;

namespace Parafait_POS
{
    public partial class frmStatus : Form
    {
        public bool isSigned;

        public bool IsSigned
        {
            get { return isSigned; }
        }

        public bool isCancelled;

        public bool IsCancelled
        {
            get { return isCancelled; }
        }
       
        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";
        Utilities Utilities = POSStatic.Utilities;
        frmWaiverSignature_DTU1031 FrmWaiverSignature_DTU1031;
        frmWaiverSignature_DTH1152 frmWaiverSignature_DTH1152;
        frmCapillaryRedemption frmRedemption;
        public string responseMsg = POSStatic.MessageUtils.getMessage(1008, WARNING);
        bool isWaiverCall = false;
        Form frmLocal;   
        //Begin: Added for logger function on 15-May-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Added for logger function on 15-May-2016
       
        public frmStatus(frmWaiverSignature_DTU1031 frmWaiverSignature_DTU1031, frmWaiverSignature_DTH1152 frmWaiverSignature_DTH1152)
        {
            log.Debug("Starts-frmStatus()");
            InitializeComponent();
            isWaiverCall = true;
            FrmWaiverSignature_DTU1031 = frmWaiverSignature_DTU1031;
            this.frmWaiverSignature_DTH1152 = frmWaiverSignature_DTH1152;
            Updatelabel(responseMsg);
            log.Debug("Ends-frmStatus()");
        }

        public frmStatus(frmCapillaryRedemption redemptinFrm)
        {
            log.Debug("Starts-frmStatus() from Capillary API call");
            InitializeComponent();
            this.Text = "Capillary API Integration"; 
            frmRedemption = redemptinFrm;
            Updatelabel(responseMsg);

            log.Debug("Ends-frmStatus() from Capillary API call");
        }

        //Added for Merkle Integartion
        public frmStatus(string messsage)
        {
            log.Debug("Starts-frmStatus() from other API call");
            InitializeComponent();
            this.Text = messsage;
            Updatelabel(responseMsg);

            log.Debug("Ends-frmStatus() from other API call");
        }
        public frmStatus(Form frmpassed, string applicability)//Generic constructor
        {
            log.Debug("Starts-frmStatus() from Capillary API call");
            InitializeComponent();
            switch (applicability)
            {
                case "CustomerFeedback": this.Text = "Customer Survey Questionnaire";
                    break;
            }
            frmLocal = frmpassed;
            Updatelabel(responseMsg);
            log.Debug("Ends-frmStatus() from Capillary API call");
        }
        void userEvent(object sender, EventArgs ev)
        {
            isSigned = Convert.ToBoolean((ev as eventArgs).value1);
           // MessageBox.Show("Event receved from " + (ev as eventArgs).value1);
            //if ((ev as eventArgs).value1 == "true")
            //{
            //    btn_Cancel.Enabled = false;
            //}
            //else
            //{
            //    btn_Cancel.Enabled = true;
            //}
           
        }

        //To display status text into label
        private void Updatelabel(string StatusMessage)
        {
            log.Debug("Starts-Updatelabel()");
            if (lbl_Status_Errors.InvokeRequired)
                lbl_Status_Errors.BeginInvoke(new Action(() => lbl_Status_Errors.Text = StatusMessage));
            else
                lbl_Status_Errors.Text = StatusMessage;
            log.Debug("End-Updatelabel()");
        }        
        
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            //waiver call
            if (FrmWaiverSignature_DTU1031 != null)
            {
                if (FrmWaiverSignature_DTU1031.Visible == true)
                {
                    FrmWaiverSignature_DTU1031.Invoke(new Action(() => { FrmWaiverSignature_DTU1031.Close(); }));
                }
                if (!FrmWaiverSignature_DTU1031.IsSigWindowOpen)
                {
                    this.Close();
                }
            }
            else if (frmWaiverSignature_DTH1152 != null)
            {
                if (frmWaiverSignature_DTH1152.Visible == true)
                {
                    frmWaiverSignature_DTH1152.Invoke(new Action(() => { frmWaiverSignature_DTH1152.Close(); }));
                }
                if (!frmWaiverSignature_DTH1152.IsSigWindowOpen)
                {
                    this.Close();
                }
            }

            //for Capillary call
            isCancelled = true;
            if (frmRedemption != null && frmRedemption.Visible == true)
            {
                frmRedemption.Invoke(new Action(() => { frmRedemption.Close(); }));
                this.Close();
            } 
            else
            {
                this.Close();
            }
            if (frmLocal != null)
            {
                if (frmLocal.Visible == true)
                {
                    frmLocal.Invoke(new Action(() => { frmLocal.Close(); }));
                }
                this.Close();
            }
        }
        
        private void frmStatus_Load(object sender, EventArgs e)
        {
            if(isWaiverCall && FrmWaiverSignature_DTU1031 != null)
                FrmWaiverSignature_DTU1031.raiseEvent = userEvent;
            if (isWaiverCall && frmWaiverSignature_DTH1152 != null)
                frmWaiverSignature_DTH1152.raiseEvent = userEvent;
        }
    }
}

