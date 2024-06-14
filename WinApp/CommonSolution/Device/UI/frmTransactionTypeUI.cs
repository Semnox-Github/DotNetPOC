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

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Form is used to select the transaction type 
    /// </summary>
    public partial class frmTransactionTypeUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// string TransactionType
        /// </summary>
        public string TransactionType;
        /// <summary>
        /// double TipAmount;
        /// </summary>
        public double TipAmount;
        private double trxAmount;
        ShowMessageDelegate showMessageDelegate;
        Utilities utilities;
        /// <summary>
        /// Constructor for transaction type selection UI 
        /// </summary>
        /// <param name="_Utilities"> parameter of the type parafait utilities </param>
        /// <param name="trxnType"> Can be passed as TATokenRequest(if the tokenization is enabled)
        /// <param name="trxAmount"> Can be passed as TATokenRequest(if the tokenization is enabled)
        /// <param name="showMessageDelegate"> showMessageDelegate
        /// Authorization, Sale and Completion</param>
        public frmTransactionTypeUI(Utilities _Utilities, string trxnType,double trxAmount, ShowMessageDelegate showMessageDelegate)
        {
            log.LogMethodEntry(_Utilities, trxnType, trxAmount, showMessageDelegate);

            InitializeComponent();
            utilities = _Utilities;
            this.trxAmount = trxAmount;
            this.showMessageDelegate = showMessageDelegate;
            utilities.setLanguage(this);
            grpboxTipEntry.Visible = false;
            this.Height = grpboxTipEntry.Bottom + 20;
            if (string.IsNullOrEmpty(trxnType))
            {
                log.LogMethodExit(null);
                return;
            }
            else
            {
                if (trxnType.Equals("TATokenRequest"))
                {
                    btnAuth.Enabled = btnPreauth.Enabled = btnsettlement.Enabled = true;
                }
                else if (trxnType.Equals("Authorization"))
                {
                    btnAuth.Enabled = btnsettlement.Enabled = true;
                    btnPreauth.Enabled = false;
                }
                else if (trxnType.Equals("Sale") || trxnType.Equals("Completion"))
                {
                    btnsettlement.Enabled = true;
                    btnAuth.Enabled = btnPreauth.Enabled = false;
                    if (trxnType.Equals("Completion"))
                    {
                        grpboxTipEntry.Visible = true;
                        this.Height = 285;
                    }
                }
            }
            log.LogMethodExit(null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            this.DialogResult = DialogResult.Cancel;
            Close();

            log.LogMethodEntry(null);
        }

        private void btnPreauth_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            TransactionType = "TATokenRequest";
            this.DialogResult = DialogResult.OK;

            log.LogMethodExit(null);
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            TransactionType = "Authorization";
            this.DialogResult = DialogResult.OK;

            log.LogMethodExit(null);
        }

        private void btnsettlement_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (btnAuth.Enabled && btnsettlement.Enabled)
            {
                TransactionType = "Sale";
            }
            else
            {
                TransactionType = "Completion";
            }
            TipAmount = string.IsNullOrEmpty(txtTip.Text) ? 0.0 : double.Parse(txtTip.Text);
            this.DialogResult = DialogResult.OK;

            log.LogMethodExit(null);
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            showNumberPadForm('-');
            log.LogMethodExit(null);
        }
        void showNumberPadForm(char firstKey)
        {
            log.LogMethodEntry();
            double varAmount = NumberPadForm.ShowNumberPadForm(utilities.MessageUtils.getMessage("Enter tip amount"), firstKey, utilities);
            if (varAmount < 0)
            {
                btnCancel.PerformClick();
                return;
            }
            else
            {
                string limit = utilities.getParafaitDefaults("MAX_TIP_AMOUNT_PERCENTAGE");
                long tipLimit = Convert.ToInt64(string.IsNullOrEmpty(limit)?"200":limit);
                if (tipLimit > 0 && ((trxAmount * tipLimit) / 100) < varAmount)
                {
                    if (showMessageDelegate == null)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1832, tipLimit), "Tip validation", MessageBoxButtons.OK);
                    }
                    else
                    {
                        showMessageDelegate(utilities.MessageUtils.getMessage(1832, tipLimit), "Tip validation", MessageBoxButtons.OK);
                    }
                    showNumberPadForm('-');
                    log.LogMethodExit();
                    return;
                }
                txtTip.Text = varAmount.ToString("0.00");
            }                
            btnsettlement.PerformClick();
            log.LogMethodExit();
        }

        private void txtTip_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            showNumberPadForm('-');
            log.LogMethodExit();
        }

        private void frmTransactionTypeUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if(grpboxTipEntry.Visible)
            {
                showNumberPadForm('-');
            }
            log.LogMethodExit();
        }
    }
}
