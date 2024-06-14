using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    //This form is created on 09-Oct-2015 for adding tip feature
    public partial class frmTip : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public double TipAmount = 0;
        public string Mode;
        bool isPaymentmodeChangesAllowed = false;
        NumberPad numPad;
        double _Amount;

        DataTable dTable;//Added to enable user in tip on 04-Feb-2016//
        public List<int> lstUserId = new List<int>(); //Added for populate selected user ids on 04-Feb-2016//

        public frmTip(Utilities _utilities, double TipAmount, double Amount, string PaymentMode, bool allowChanges = true)//Modified to add enableUserOption on 25-Jan-2016
        {
            log.LogMethodEntry(_utilities, TipAmount, Amount, PaymentMode, allowChanges);
            InitializeComponent();
            Mode = PaymentMode;
            _Amount = Amount;
           
            isPaymentmodeChangesAllowed = allowChanges;
            string[] denoms = _utilities.getParafaitDefaults("PAYMENT_DENOMINATIONS").Split('|');

            if (PaymentMode.Equals("Cash"))
            {
                btnCash.BackgroundImage =  Semnox.Parafait.Transaction.Properties.Resources.checked_checkbox4;
                btnCreditCard.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.checkbox_unchecked;
                gbCreditCard.Visible = false;
            }
            else
            {
                btnCash.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.checkbox_unchecked;
                btnCreditCard.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.checked_checkbox4;
                gbCreditCard.Visible = true;
                if(!isPaymentmodeChangesAllowed)
                {
                    gbCash.Visible = false;
                }
                
            }
            //Begin: Modification done for showing user in Checklistbox on 04-Feb-2016//                    
            dTable = _utilities.executeDataTable(@"SELECT u.user_id,u.username,u.UserStatus,ur.role,ur.role_description,u.active_flag FROM users u, user_roles ur
                                                    WHERE ur.role_id=u.role_id
                                                    AND (ur.allow_pos_access='Y'
                                                    OR ur.manager_flag='Y'
                                                    OR ur.EnablePOSClockIn = 1)
                                                    AND u.UserStatus = 'ACTIVE'
                                                    AND ur.role NOT IN ('Semnox Admin','System Administrator')");                                
            cbxlUser.DataSource = dTable;
            cbxlUser.DisplayMember = "username";
            cbxlUser.ValueMember = "user_id";
            //End: Modification done for showing user in Checklistbox on 04-Feb-2016//

            numPad = new NumberPad(_utilities.ParafaitEnv.AMOUNT_FORMAT, _utilities.ParafaitEnv.RoundingPrecision);
            numPad.handleaction(TipAmount.ToString());
            numPad.NewEntry = true;

            Panel NumberPadVarPanel = numPad.NumPadPanel();
            NumberPadVarPanel.Location = new System.Drawing.Point(2, 2);
            this.Controls.Add(NumberPadVarPanel);
            numPad.setReceiveAction = EventnumPadOKReceived;
            numPad.setKeyAction = EventnumPadKeyPressReceived;

            this.KeyPreview = true;

            this.KeyPress += new KeyPressEventHandler(FormNumPad_KeyPress);
            this.FormClosing += new FormClosingEventHandler(FormNumPad_FormClosing);

            log.LogMethodExit(null);
        }
       

        void FormNumPad_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (this.DialogResult == DialogResult.Cancel)
                TipAmount = 0;

            log.LogMethodExit(null);
        }

        void FormNumPad_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (e.KeyChar == (char)Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else
                numPad.GetKey(e.KeyChar);

            log.LogMethodExit(null);
        }

        private void EventnumPadOKReceived()
        {
            log.LogMethodEntry();

            TipAmount = numPad.ReturnNumber;
            this.DialogResult = DialogResult.OK;

            //Begin: Added for Add tip amount selected user on 25-Jan-2016//
            if (TipAmount != 0)
            {
                // MessageBox.Show("TipAmount is available");
                for (int i = 0; i < cbxlUser.CheckedItems.Count; i++)
                {
                    DataRowView drv = (DataRowView)cbxlUser.CheckedItems[i];
                    int name = Convert.ToInt32(drv[0].ToString());
                    lstUserId.Add(name);
                }
            }
            //else
            //{
            //    MessageBox.Show("no TipAmount ");
            //}
            //End: Added for Add tip amount selected user on 25-Jan-2016//

            this.Close();

            log.LogMethodExit(null);
        }

        void EventnumPadKeyPressReceived()
        {
            log.LogMethodEntry(); 

            TipAmount = numPad.ReturnNumber;

            log.LogMethodExit(null);
        }
       

        private void btnCancel_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            (sender as Button).BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.customer_button_pressed;

            log.LogMethodExit(null);
        }

        private void btnCancel_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            (sender as Button).BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.customer_button_normal;

            log.LogMethodExit(null);
        }
        

        private void btnCash_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            Mode = "Cash";
            btnCash.BackgroundImage = Properties.Resources.checked_checkbox4;
            btnCreditCard.BackgroundImage = Properties.Resources.checkbox_unchecked;

            log.LogMethodExit(null);
        }

        private void btnCreditCard_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            Mode = "CreditCard";
            btnCash.BackgroundImage = Properties.Resources.checkbox_unchecked;
            btnCreditCard.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.checked_checkbox4;

            log.LogMethodExit(null);
        }
        
    }
}
