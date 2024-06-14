/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Kiosk age gate form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       12-Sep-2018      Archana            Created 
 *2.70.2        10-Aug-2019      Girish kundar      Modified :Removed Unused namespace's.
 ********************************************************************************************/
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer
{
    public partial class FrmAgeGate : FrmKioskBaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected DateTime BirthDate = DateTime.MinValue;
        protected CustomerDTO customerDTO = null;
        private string cardNumber;
        private ExecutionContext executionContext;
        protected AlphaNumericKeyPad keypad;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="machineExecutionContext">ExecutionContext</param>
        /// <param name="CardNumber">string</param>
        public FrmAgeGate(ExecutionContext machineExecutionContext, Utilities utils, string cardNumber = "")
        {
            log.LogMethodEntry(machineExecutionContext, cardNumber);
            InitializeComponent();
            this.cardNumber = cardNumber;
            utils.setLanguage(this);
            executionContext = machineExecutionContext;
            pbCheckBox.Visible = chkReadConfirm.Visible = lnkTerms.Visible = false;
            txtDate1.Clear();
            txtDate2.Clear();
            txtDate3.Clear();
            log.LogMethodExit();
        }

        protected virtual void frmAgeGate_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit();
        }

        protected void txtBirthDate_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            TextBox txt = sender as TextBox;
            if (keypad == null || keypad.IsDisposed)
            {
                keypad = new AlphaNumericKeyPad(this, txt);
                keypad.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - keypad.Width) / 2, this.Height - keypad.Height - 30);
            }
            else
                keypad.currentTextBox = txt;

            keypad.Show();
            Application.DoEvents();
            txt.SelectAll();
            log.LogMethodExit();
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            TextBox txt = sender as TextBox;
            if (txt.Text.Length == txt.MaxLength)
            {
                Control c = this.GetNextControl(txt, true);
                if (c != null)
                    c.Focus();
            }
            log.LogMethodExit();
        }

        protected virtual void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null)
                keypad.Hide();
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        protected virtual void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            log.LogMethodExit();
        }

        private void frmAgeGater_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            log.LogMethodExit();
        }
    }
}
