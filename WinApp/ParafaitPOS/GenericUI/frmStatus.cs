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
using ParafaitUtils;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Parafait_POS.Redemption;
using Semnox.Parafait.CustomerFeedBackSurvey;

namespace Semnox.Parafait.GenericUI
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
        ParafaitUtils.Utilities Utilities = POSStatic.Utilities;
        frmWaiverSignature frmSignature;
        frmCapillaryRedemption frmRedemption;
        public string responseMsg = POSStatic.MessageUtils.getMessage(1008, WARNING);
        bool isWaiverCall = false;
        Form frmLocal;   
        //Begin: Added for logger function on 15-May-2016
        Semnox.Core.Logger log = new Semnox.Core.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Added for logger function on 15-May-2016
       
        public frmStatus(frmWaiverSignature signaturefrm)
        {
            log.Debug("Starts-frmStatus()");
            InitializeComponent();
            isWaiverCall = true;
            frmSignature = signaturefrm;
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
            if (frmSignature != null)
            {
                if (frmSignature.Visible == true)
                {
                    frmSignature.Invoke(new Action(() => { frmSignature.Close(); }));
                }
                if(!frmSignature.IsSigWindowOpen)
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
            if(isWaiverCall)
                frmSignature.raiseEvent = userEvent;
        }
    }
}

