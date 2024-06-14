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

namespace Semnox.Parafait.Device.PaymentGateway.Mercury
{
    public partial class frmStatus : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private bool _retvalue = false;
        System.Timers.Timer _timer = new System.Timers.Timer();
        private ClsResponseMessageAttributes _lasttranResponseMsg;
        private ClsResponseMessageAttributes objresponse;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="InitialResponseeventargs"></param>
        public frmStatus(ErrorResponseEventArgs InitialResponseeventargs)
        {
            log.LogMethodEntry(InitialResponseeventargs);

            InitializeComponent();
            UpdateStatus(InitialResponseeventargs);

            log.LogMethodExit(null);            
        }

        /// <summary>
        /// Get/Set Property for returnval
        /// </summary>
        public bool returnval
        {
            get
            {
                return _retvalue;
            }
        }

        /// <summary>
        /// Get Property for lasttranResponseMsg
        /// </summary>
        public ClsResponseMessageAttributes lasttranResponseMsg
        {
            get
            {
                return _lasttranResponseMsg;
            }
        }

        private void Updatelabel(string StatusMessage)
        {
            log.LogMethodEntry(StatusMessage);

            if (lbl_Status_Errors.InvokeRequired)
                lbl_Status_Errors.BeginInvoke(new Action(() => lbl_Status_Errors.Text = StatusMessage));
            else
                lbl_Status_Errors.Text = StatusMessage;

            log.LogMethodExit(null);
        }

        /// <summary>
        /// UpdateStatus Method
        /// </summary>
        /// <param name="responseeventargs"></param>
        public void UpdateStatus(ErrorResponseEventArgs responseeventargs)
        {
            log.LogMethodEntry(responseeventargs);

            string StatusMessage = responseeventargs.StatusMessage.ToString();
            Updatelabel(StatusMessage);
            objresponse = new ClsResponseMessageAttributes();
            objresponse = responseeventargs.ResponseMessage;
            if (objresponse != null)
            {
                enumCommandStatus CmdStatus = (enumCommandStatus)Enum.Parse(typeof(enumCommandStatus), objresponse.CmdStatus);
                _timer.Interval = double.Parse(ConfigurationManager.AppSettings["PoupcloseTimeout"], CultureInfo.InvariantCulture);
                _timer.Elapsed += _timer_Tick;
                if (objresponse.TextResponse != null)
                    Updatelabel((objresponse.CmdStatus == "None" ? "" : objresponse.CmdStatus + "    ") + objresponse.TextResponse);
                if ((CmdStatus == enumCommandStatus.Error) || (CmdStatus == enumCommandStatus.Declined) || (CmdStatus == enumCommandStatus.IPADCancel) || (CmdStatus == enumCommandStatus.WSError) || (CmdStatus == enumCommandStatus.None))
                {
                    if (btn_Cancel.InvokeRequired)
                    {
                        btn_Cancel.BeginInvoke(new Action(() => btn_Cancel.Enabled = true));
                        _timer.Enabled = false;
                    }
                    else
                        btn_Cancel.Enabled = true;
                }
                else
                {
                    if (btn_Cancel.InvokeRequired)
                        _timer.Enabled = true;
                    else
                        _timer.Enabled = true;
                }
            }

            log.LogMethodExit(null);
        }

        void _timer_Tick(object sender, ElapsedEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            _timer.Enabled = false;
            _retvalue = true;
            this.DialogResult = DialogResult.Cancel;
            _lasttranResponseMsg = objresponse;
            if (this.InvokeRequired)
                this.BeginInvoke(new Action(() => this.Close()));

            log.LogMethodExit(null);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            _retvalue = false;
            _lasttranResponseMsg = objresponse;
            this.DialogResult = DialogResult.Cancel;
            this.Close();

            log.LogMethodExit(null);
        }
    }
}

