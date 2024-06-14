using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public partial class frmFinalizeTransaction : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public delegate bool AuthenticateManager(ref int managerId);
        Utilities utilities;
        decimal transactionAmount = 0;
        decimal enteredTipAmount = 0;
        decimal currentTransactionAmount = 0;
        ShowMessageDelegate messageDelegate;
        decimal currentTipAmount = 0;
        
        public decimal TipAmount { get { return currentTipAmount; } }
                
        string limit;
        long tipLimit;
        /// <summary>
        /// This form is to show the summerized amounts before doing the settlement transaction
        /// </summary>
        /// <param name="utilities">utilitis which holds the environment settings</param>
        /// <param name="OverallTransactionAmount">Including other payment modes and the remaining transaction amount in that transaction</param>
        /// <param name="enteredTipAmount"> The sum of all the tip amount paid through all payment mode</param>
        /// <param name="currentTransactionAmount"> Current payment mode transaction amount </param>
        /// <param name="currentTrasactionTipAmount">Current payment mode transaction tip amount</param>
        /// <param name="cardNumber"> Credit/Debit card number using which the payment is done</param>
        public frmFinalizeTransaction(Utilities utilities, decimal OverallTransactionAmount, decimal enteredTipAmount, decimal currentTransactionAmount,decimal currentTrasactionTipAmount, string creditCardNumber,ShowMessageDelegate showMessage)
        {
            log.LogMethodEntry(utilities, OverallTransactionAmount, enteredTipAmount, currentTransactionAmount, currentTrasactionTipAmount, creditCardNumber, showMessage);
            InitializeComponent();
            utilities.setLanguage(this);
            this.utilities = utilities;
            //this.saveTransactionLog = saveTransactionLog;
            messageDelegate = (showMessage==null)? MessageBox.Show: showMessage;
            transactionAmount = OverallTransactionAmount;
            this.enteredTipAmount = (enteredTipAmount >= currentTrasactionTipAmount) ? enteredTipAmount- currentTrasactionTipAmount:0;
            this.currentTransactionAmount = currentTransactionAmount;
            limit = utilities.getParafaitDefaults("MAX_TIP_AMOUNT_PERCENTAGE");
            tipLimit = Convert.ToInt64(string.IsNullOrEmpty(limit) ? "200" : limit);
            lblCardNumber.Text = creditCardNumber;
            lblTipAmount.Text = currentTrasactionTipAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            lblTrxnAmount.Text = currentTransactionAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            lblTotalAmount.Text = (currentTransactionAmount+ currentTrasactionTipAmount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);            
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void btnEditTip_Click(object sender, System.EventArgs e)
        {
            log.LogMethodEntry();
            showNumberPadForm('-');            
            log.LogMethodExit();
        }
        void showNumberPadForm(char firstKey)
        {
            log.LogMethodEntry();
            double varAmount = NumberPadForm.ShowNumberPadForm(utilities.MessageUtils.getMessage("Enter tip amount"), firstKey, utilities);
            if (varAmount < 0)
            {
                return;
            }
            else
            {                
                if (tipLimit > 0 && ((transactionAmount * tipLimit) / 100) < (Convert.ToDecimal(varAmount)+ enteredTipAmount))
                {
                    messageDelegate(utilities.MessageUtils.getMessage("Total Transaction Amount: ") + transactionAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)+"\n"
                        + utilities.MessageUtils.getMessage("Tip Amount Limit: ")+ ((transactionAmount * tipLimit) / 100).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL)+"\n"
                        + utilities.MessageUtils.getMessage("Tip Amount Settled: ") + enteredTipAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + "\n"
                        + utilities.MessageUtils.getMessage("Balance Tip Amount Applicable: ") + (((transactionAmount * tipLimit) / 100) - enteredTipAmount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL) + "\n"
                        + utilities.MessageUtils.getMessage("Tip amount is higher than the tip limit for this transaction. Please enter lower tip amount."),"Tip limit validation", MessageBoxButtons.OK);
                    showNumberPadForm('-');
                    log.LogMethodExit();
                    return;
                }
                lblTipAmount.Text = varAmount.ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
                currentTipAmount = Convert.ToDecimal(varAmount);
                lblTotalAmount.Text = (currentTransactionAmount + currentTipAmount).ToString(utilities.ParafaitEnv.AMOUNT_WITH_CURRENCY_SYMBOL);
            }            
            log.LogMethodExit();
        }        
        private void btnComplete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.OK;
            Close();
            log.LogMethodExit();
        }
    }
}
