using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// POS payment gateway status form 
    /// </summary>
    public partial class frmPOSPaymentStatusUI : Form, IDisplayStatusUI
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        delegate void DisplayWindow();
        delegate void DisplayTextBox(string text);
        delegate void EnableCancel(bool isEnable);
        delegate void EnableCheckNow(bool isEnable);
        delegate void CloseWindow();
        /// <summary>
        /// Cancel clicked
        /// </summary>
        public event EventHandler CancelClicked;
        public event EventHandler CheckNowClicked;
        bool isCancelButtonEnabled;
        private ExecutionContext executionContext;

        /// <summary>
        /// Enable of Disable the cancel button
        /// </summary>
        public bool IsCancelButtonEnabled
        {
            get { return isCancelButtonEnabled; }
            set
            {
                isCancelButtonEnabled = value;
                btnCancel.Enabled = isCancelButtonEnabled;
            }
        }
        /// <summary>
        /// Constructor to for creating the ui object
        /// </summary>
        /// <param name="titleText">string value to display in the title</param>
        public frmPOSPaymentStatusUI(ExecutionContext executionContext, string titleText, string amountChargeText)
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.executionContext = executionContext;
            this.Text = titleText;
            //if (string.IsNullOrEmpty(amountChargeText))
            //{
            lblCardCharged.Visible = false;
            //}
            //lblCardCharged.Text = amountChargeText;

            this.TopMost = true;
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //log.LogMethodEntry(sender, e);
            CancelClicked(sender, e);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //this.Close();
            //log.LogMethodExit(null);
        }
        /// <summary>
        /// Load the form
        /// </summary>
        public void ShowStatusWindow()
        {
            log.LogMethodEntry();
            try
            {
                if (this.InvokeRequired)
                {
                    DisplayWindow delegateFunction = new DisplayWindow(ShowStatusWindow);
                    BeginInvoke(delegateFunction, new object[] { });
                }
                else
                {
                    this.ShowDialog();
                }
            }
            catch { }
            log.LogMethodExit();
        }
            /// <summary>
            /// Display the text
            /// </summary>
            /// <param name="text"></param>
            public void DisplayText(string text)
        {
            try
            {
                if (lblStatus.InvokeRequired)
                {
                    DisplayTextBox delegateFunction = new DisplayTextBox(DisplayText);
                    BeginInvoke(delegateFunction, new object[] { text });
                }
                else
                {
                    lblStatus.Text = text;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Enable cancel button
        /// </summary>
        /// <param name="isEnable"></param>
        public void EnableCancelButton(bool isEnable)
        {
            log.LogMethodEntry();
            try
            {
                if (btnCancel.InvokeRequired)
                {
                    EnableCancel delegateFunction = new EnableCancel(EnableCancelButton);
                    BeginInvoke(delegateFunction, new object[] { isEnable });
                }
                else
                {
                    btnCancel.Visible = isEnable;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        public void EnableCheckNowButton(bool isEnable)
        {
            log.LogMethodEntry();
            try
            {
                if (btnCheckNow.InvokeRequired)
                {
                    EnableCheckNow delegateFunction = new EnableCheckNow(EnableCheckNowButton);
                    BeginInvoke(delegateFunction, new object[] { isEnable });
                }
                else
                {
                    btnCheckNow.Visible = isEnable;
                    if (isEnable)
                    {
                        btnCheckNow.Location = new System.Drawing.Point(344, 80);
                        btnCancel.Location = new System.Drawing.Point(144, 80);
                    }
                    else
                    {
                        btnCancel.Location = new System.Drawing.Point(244, 80);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Close window
        /// </summary>        
        public void CloseStatusWindow()
        {
            log.LogMethodEntry();
            try
            {

                if (this.InvokeRequired)
                {
                    CloseWindow delegateFunction = new CloseWindow(CloseStatusWindow);
                    BeginInvoke(delegateFunction, new object[] { });
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void btnCheckNow_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnCheckNow.Enabled = false;
            CheckNowClicked(sender, e);
            log.LogMethodExit();
        }
    }
}
