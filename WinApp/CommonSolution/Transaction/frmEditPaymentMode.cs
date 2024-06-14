using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    public partial class frmEditPaymentMode : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities ParafaitUtilities;
        int paymentId = -1;
        bool isIntialBinding = false;
        int trxId = -1;
        string oldPaymentMode = "";
        TextBox CurrentTextBox;
        TextBox PreviousTexBox;
        
        public frmEditPaymentMode(int trxId, int paymentId, Utilities _Utilities)
        {
            log.LogMethodEntry(trxId, paymentId, _Utilities);

            InitializeComponent();
            ParafaitUtilities = _Utilities;
            
            DataTable dt = ParafaitUtilities.executeDataTable(@"select pm.PaymentModeId, pm.PaymentMode from TrxPayments tp
                                                                  inner join PaymentModes pm on pm.PaymentModeId = tp.PaymentModeId
                                                                  where tp.PaymentId = @paymentModeId", new SqlParameter("paymentModeId", paymentId));

            log.LogVariableState("paymentModeId", paymentId);

           if (dt!= null && dt.Rows.Count > 0)
           {
               oldPaymentMode = dt.Rows[0][1].ToString();
               LoadPayments(dt.Rows[0][0]);
           }
            
            this.paymentId = paymentId;
            this.trxId = trxId;

            log.LogMethodExit(null);
        }

        //Load the payment modes
        void LoadPayments(object paymentModeId)
        {
            log.LogMethodEntry(paymentModeId);

            DataTable dt = ParafaitUtilities.executeDataTable(@"select PaymentModeId, PaymentMode from PaymentModes where POSAvailable = 1 and PaymentModeId != @paymentModeId and Gateway is null
                                                                and isDebitCard ='N'", new SqlParameter("@paymentModeId", paymentModeId));

            isIntialBinding = true;
            if(dt != null && dt.Rows.Count > 0)
            {
                cmbPaymentMode.DataSource = dt;
                cmbPaymentMode.DisplayMember = "PaymentMode";
                cmbPaymentMode.ValueMember = "PaymentModeId";
            }

            isIntialBinding = false;
            cmbPaymentMode_SelectedIndexChanged(null, null);

            log.LogVariableState("isIntialBinding", isIntialBinding);
            log.LogMethodExit(null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            this.Close();

            log.LogMethodExit(null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            #region Validation code
            if (ParafaitUtilities.getParafaitDefaults("CREDITCARD_DETAILS_MANDATORY") == "Y" && GetPaymentMode(cmbPaymentMode.SelectedValue) == "CREDIT")
            {
                if (string.IsNullOrEmpty(txtCardNumber.Text.Trim()))
                {
                    txtMessage.Text = ParafaitUtilities.MessageUtils.getMessage(179);
                    log.LogMethodExit(null);
                    return;
                }

                if (string.IsNullOrEmpty(txtCardExpiry.Text.Trim()))
                {
                    txtMessage.Text = ParafaitUtilities.MessageUtils.getMessage(180);
                    log.LogMethodExit(null);
                    return;
                }

                if (string.IsNullOrEmpty(txtNameOnCC.Text.Trim()))
                {
                    txtMessage.Text = ParafaitUtilities.MessageUtils.getMessage(181);
                    log.LogMethodExit(null);
                    return;
                }
            }
            #endregion

            SqlCommand cmd = ParafaitUtilities.getCommand();

            try
            {
                #region Update Trxpayment
                if (paymentId > 0 && cmbPaymentMode.SelectedValue != DBNull.Value)
                {
                    cmd.CommandText = @"Update TrxPayments set PaymentModeId =@paymentModeId, CreditCardNumber = @creditCardNumber, NameOnCreditCard = @nameOnCreditCard, CreditCardName = @creditCardName,
				                                           CreditCardExpiry = @creditCardExpiry, CreditCardAuthorization = @creditCardAuthorization, Reference =@reference
				                                           where PaymentId = @paymentId";

                    cmd.Parameters.AddWithValue("@paymentId", paymentId);
                    cmd.Parameters.AddWithValue("@paymentModeId", cmbPaymentMode.SelectedValue);

                    log.LogVariableState("@paymentId", paymentId);
                    log.LogVariableState("@paymentModeId", cmbPaymentMode.SelectedValue);

                    if (string.IsNullOrEmpty(txtCardNumber.Text.Trim()))
                    {
                        cmd.Parameters.AddWithValue("@creditCardNumber", DBNull.Value);
                        log.LogVariableState("@creditCardNumber", DBNull.Value);
                    }
                    else
                    {
                        string creditCardNumber = txtCardNumber.Text;

                        if (creditCardNumber.Length > 4)
                            creditCardNumber = creditCardNumber.Substring(creditCardNumber.Length - 4);
                        creditCardNumber = new String('X', 12) + creditCardNumber;
                        cmd.Parameters.AddWithValue("@creditCardNumber", creditCardNumber);

                        log.LogVariableState("@creditCardNumber", creditCardNumber);
                    }

                    if (string.IsNullOrEmpty(txtNameOnCC.Text.Trim()))
                    {
                        cmd.Parameters.AddWithValue("@nameOnCreditCard", DBNull.Value);
                        log.LogVariableState("@nameOnCreditCard", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@nameOnCreditCard", txtNameOnCC.Text.ToString());
                        log.LogVariableState("@nameOnCreditCard", txtNameOnCC.Text.ToString());
                    }

                    if (string.IsNullOrEmpty(txtCCName.Text.Trim()))
                    {
                        cmd.Parameters.AddWithValue("@creditCardName", DBNull.Value);
                        log.LogVariableState("@creditCardName", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@creditCardName", txtCCName.Text.ToString());
                        log.LogVariableState("@creditCardName", txtCCName.Text.ToString());
                    }

                    if (string.IsNullOrEmpty(txtCardExpiry.Text.Trim()))
                    {
                        cmd.Parameters.AddWithValue("@creditCardExpiry", DBNull.Value);
                        log.LogVariableState("@creditCardExpiry", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@creditCardExpiry", txtCardExpiry.Text.ToString());
                        log.LogVariableState("@creditCardExpiry", txtCardExpiry.Text.ToString());
                    }

                    if (string.IsNullOrEmpty(txtAuthorization.Text.Trim()))
                    {
                        cmd.Parameters.AddWithValue("@creditCardAuthorization", DBNull.Value);
                        log.LogVariableState("@creditCardAuthorization", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@creditCardAuthorization", txtAuthorization.Text.ToString());
                        log.LogVariableState("@creditCardAuthorization", txtAuthorization.Text.ToString());
                    }

                    if (string.IsNullOrEmpty(txtReference.Text.Trim()))
                    {
                        cmd.Parameters.AddWithValue("@reference", DBNull.Value);
                        log.LogVariableState("@reference", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@reference", txtReference.Text.ToString());
                        log.LogVariableState("@reference", txtReference.Text.ToString());
                    }

                    cmd.ExecuteNonQuery();
                }
                #endregion

                #region update trx_header 
                cmd.CommandText = @"update trx_header 
                                set status = 'CLOSED', CashAmount = pCashAmount, CreditCardAmount = pCreditCardAmount, 
                                    GameCardAmount = pGameCardAmount, OtherPaymentModeAmount = pOtherPaymentModeAmount,
                                    payment_mode = ISNULL((select case when pm.isCash = 'Y' then 1 
					                                    else case when pm.isCreditCard = 'Y' then 2 
						                                    else case when pm.isDebitCard = 'Y' then 3 
							                                    else 4 end end end
                                    from (select AVG(distinct tp.PaymentModeId) pmId
		                                    from TrxPayments tp, PaymentModes pm
		                                    where pm.PaymentModeId = tp.PaymentModeId
		                                    and pm.isRoundOff = 'N'
		                                    and tp.ParentPaymentId is null
		                                    and not exists (select 1 
						                                    from TrxPayments tp1
						                                    where tp1.TrxId = tp.TrxId
						                                    and tp.PaymentId = tp1.ParentPaymentId)
		                                    and trxid = @trxId
		                                    having count(distinct tp.PaymentModeId) = 1) v, paymentModes pm
                                    where pm.PaymentModeId = v.pmId), 5)
                                    from (select isnull(SUM(case when pm.isCash = 'Y' then amount else 0 end), 0) pCashAmount, 
			                                     isnull(SUM(case when pm.isDebitCard = 'Y' then amount else 0 end), 0) pGameCardAmount,
			                                     isnull(SUM(case when pm.isCreditCard = 'Y' then amount else 0 end), 0) pCreditCardAmount,
			                                     isnull(SUM(case when pm.isCash+pm.isCreditCard+pm.isDebitCard = 'NNN' then amount else 0 end), 0) pOtherPaymentModeAmount
		                                    from TrxPayments tp, PaymentModes pm
		                                    where tp.PaymentModeId = pm.PaymentModeId
		                                    and TrxId = @trxId) v
                                where trxId = @trxId";
                cmd.Parameters.AddWithValue("@trxId", trxId);

                log.LogVariableState("@trxId", trxId);

                cmd.ExecuteNonQuery();
                #endregion

                ParafaitUtilities.EventLog.logEvent("POSTransactionPayments", 'D', ParafaitUtilities.ParafaitEnv.LoginID,
                                                    oldPaymentMode + " To " + cmbPaymentMode.Text.ToString() + " For trxId " + trxId.ToString() + " and PaymentId " + paymentId.ToString(), "PaymentModeChange", 0);//Added to log the user on 16-Mar-2016//
                txtMessage.Text = ParafaitUtilities.MessageUtils.getMessage(452);
                oldPaymentMode = cmbPaymentMode.Text; //assign new payment to old paymentmode 
            }
            catch(Exception ex)
            {
                log.Error("Unable to update transaction payment and transaction header", ex);
                txtMessage.Text = ex.Message;
            }
        }

        //Check PaymnetMode 
        string GetPaymentMode(Object paymentModeId)
        {
            log.LogMethodEntry(paymentModeId);

            Object o = ParafaitUtilities.executeScalar(@"SELECT (CASE WHEN isCreditCard = 'Y' THEN 'CREDIT'
			                                                          WHEN isCash = 'Y' THEN 'CASH' 
			                                                          ELSE 'OTHER' END) PaymentType 
                                                                 FROM PaymentModes WHERE PaymentModeId = @paymnetModeId",
                                                                 new SqlParameter("@paymnetModeId", paymentModeId));

          log.LogVariableState("@paymnetModeId", paymentModeId);


            if (o == DBNull.Value)
          {
              o = "NONE";
          }

          log.LogMethodExit(o.ToString());
          return o.ToString();
        }

        //Enable or Disbale controls
        void EnableDisableControls(bool val)
        {
            log.LogMethodEntry(val);

            //Clear the texbox
            txtAuthorization.Text = string.Empty;
            txtCardExpiry.Text = string.Empty;
            txtCardNumber.Text = string.Empty;
            txtCCName.Text = string.Empty;
            txtNameOnCC.Text = string.Empty;
            txtReference.Text = string.Empty;

            //Update controls
            txtAuthorization.Enabled = val;
            txtCardExpiry.Enabled = val;
            txtCardNumber.Enabled = val;
            txtCCName.Enabled = val;
            txtNameOnCC.Enabled = val;

            log.LogMethodExit(null);
        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            txtMessage.Text = "";
            if (!isIntialBinding && cmbPaymentMode.SelectedValue != DBNull.Value)
            {
                string paymentMode = GetPaymentMode(cmbPaymentMode.SelectedValue);
           
                if (paymentMode == "CREDIT")
                {
                    EnableDisableControls(true);
                    txtReference.Enabled = false;
                }
                else if (paymentMode == "OTHER")
                {
                    EnableDisableControls(false);
                    txtReference.Enabled = true;
                }
                else // CASH
                {
                    EnableDisableControls(false);
                    txtReference.Enabled = false;
                }
            }

            log.LogMethodExit(null);
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            showNumberPadForm('-');

            log.LogMethodExit(null);
        }

        AlphaNumericKeyPad keypad;
        void showNumberPadForm(char firstKey)
        {
            log.LogMethodEntry(firstKey);

            this.Location = new Point(this.Location.X, 10);
            bool isAlphanumeric = false;
            isAlphanumeric = CurrentTextBox.Equals(txtCardNumber)
                | CurrentTextBox.Equals(txtCardExpiry)
                | CurrentTextBox.Equals(txtNameOnCC)
                | CurrentTextBox.Equals(txtCCName)
                | CurrentTextBox.Equals(txtAuthorization)
                | CurrentTextBox.Equals(txtReference);

            if (isAlphanumeric)
            {
                if (keypad == null || keypad.IsDisposed)
                {
                    PreviousTexBox = CurrentTextBox;
                    keypad = new AlphaNumericKeyPad(this, CurrentTextBox);
                    //keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, this.Location.Y + this.Height + 4);
                    keypad.Location = new Point(this.PointToScreen(btnShowNumPad.Location).X - keypad.Width + 52, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                    keypad.Show();
                }
                else if (keypad.Visible)
                {
                    PreviousTexBox = CurrentTextBox;
                    keypad.Hide();
                }
                else
                {
                    PreviousTexBox = CurrentTextBox;
                    keypad = new AlphaNumericKeyPad(this, CurrentTextBox);
                    keypad.Location = new Point(this.PointToScreen(btnShowNumPad.Location).X - keypad.Width + 52, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                    keypad.Show();
                }
            }

            log.LogMethodExit(null);
        }

        //Refresh and Show NumbePad for textbox enter
        void ShowNumberPad()
        {
            log.LogMethodEntry();

            if (keypad != null && !keypad.IsDisposed && PreviousTexBox != null)
            {
                if (keypad.Visible && PreviousTexBox != CurrentTextBox)
                {
                    keypad.Hide();
                    keypad = new AlphaNumericKeyPad(this, CurrentTextBox);
                    keypad.Location = new Point(this.PointToScreen(btnShowNumPad.Location).X - keypad.Width + 52, Screen.PrimaryScreen.WorkingArea.Height - keypad.Height);
                    PreviousTexBox = CurrentTextBox; 
                    keypad.Show();
                }
            }
            log.LogMethodExit(null);
        }

        private void txtCardNumber_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            CurrentTextBox = txtCardNumber;
            ShowNumberPad();

            log.LogMethodExit(null);
        }

        private void txtCardExpiry_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            CurrentTextBox = txtCardExpiry;
            ShowNumberPad();

            log.LogVariableState("CurrentTextBox ", CurrentTextBox);
            log.LogMethodExit(null);
        }

        private void txtNameOnCC_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            CurrentTextBox = txtNameOnCC;
            ShowNumberPad();

            log.LogVariableState("CurrentTextBox ", CurrentTextBox);
            log.LogMethodExit(null);
        }

        private void txtCCName_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            CurrentTextBox = txtCCName;
            ShowNumberPad();

            log.LogVariableState("CurrentTextBox ", CurrentTextBox);
            log.LogMethodExit(null);
        }

        private void txtAuthorization_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            CurrentTextBox = txtAuthorization;
            ShowNumberPad();

            log.LogVariableState("CurrentTextBox ", CurrentTextBox);
            log.LogMethodExit(null);
        }

        private void txtReference_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            CurrentTextBox = txtReference;
            ShowNumberPad();

            log.LogVariableState("CurrentTextBox ", CurrentTextBox);
            log.LogMethodExit(null);
        }

        private void frmEditPaymentMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (keypad != null)
            {
                keypad.Dispose();
            }

            log.LogMethodExit(null);
        }
    }
}
