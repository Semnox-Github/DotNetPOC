/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - frmRemoteDebugInput
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By          Remarks          
 ********************************************************************************************* 
 *2.130.7.0   26-Apr-2022       Guru S A             Enable remote debug option    
 *********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Redemption_Kiosk
{
    public partial class frmRemoteDebugInput : frmRedemptionKioskBaseForm
    {
        private new static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext;
        private AlphaNumericKeyPad keypad;
        private TextBox currentActiveTextBox;
        public frmRemoteDebugInput(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.machineUserContext = executionContext;
            InitializeComponent();
            this.Text = MessageContainerList.GetMessage(machineUserContext, "Remote Input");
            this.lblCardNumber.Text = MessageContainerList.GetMessage(machineUserContext, "Card Number") + ": ";
            this.lblTicketReceiptNo.Text = MessageContainerList.GetMessage(machineUserContext, "Ticket Receipt Number") + ": ";
            this.btnOkay.Text = MessageContainerList.GetMessage(machineUserContext, "Ok");
            this.btnClose.Text = MessageContainerList.GetMessage(machineUserContext, "Close");
            currentActiveTextBox = txtCardNumber;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            log.LogMethodExit();
        }

        
        private void btnKeyPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Clear();
                ResetTimeOut();
                if (keypad == null || keypad.IsDisposed)
                {
                    keypad = new AlphaNumericKeyPad(this, currentActiveTextBox);
                    int x = this.Location.X;
                    int y = this.Location.Y + this.Height - 10;
                    keypad.Location = new Point(x, y); 
                    keypad.Show();
                }
                else if (keypad.Visible)
                {
                    keypad.Hide();
                }
                else
                {
                    keypad.Show();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            } 
            log.LogMethodExit();
        }

        private void TextBoxEnter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Clear(); 
                ResetTimeOut(); 
                currentActiveTextBox = sender as TextBox;
                if (keypad != null)
                {
                    keypad.currentTextBox = currentActiveTextBox;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }  

        private void TextBoxLeave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        } 
        private void TextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Clear();
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtMessage.Text = ex.Message;
            }
            log.LogMethodExit();
        }

        private void TextBoxMouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                ResetTimeOut();
            }
            catch (Exception ex)
            {
                log.Error(ex); 
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try { 
            ResetTimeOut();
            txtMessage.Clear();
            this.txtCardNumber.Clear();
            this.txtTicketReceiptNo.Clear();
            this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                txtMessage.Clear();
                ResetTimeOut();
                if (string.IsNullOrWhiteSpace(this.txtCardNumber.Text) && string.IsNullOrWhiteSpace(this.txtTicketReceiptNo.Text))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(machineUserContext, 4423));
                    //Please enter Card Number or Ticket Receipt Number to proceed
                }
                this.Close();
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.Message;
                log.Error(ex);
            } 
            log.LogMethodExit();
        }

        internal string GetEnteredCardNumber()
        {
            log.LogMethodEntry();
            string retValue = txtCardNumber.Text;
            log.LogMethodExit(retValue);
            return retValue;
        }

        internal string GetEnteredTicketReceiptNumber()
        {
            log.LogMethodEntry();
            string retValue = txtTicketReceiptNo.Text;
            log.LogMethodExit(retValue);
            return retValue;
        }

        private void frmRemoteDebugInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (keypad != null)
                {
                    keypad.Hide();
                    keypad.Dispose();
                }
            }
            catch (Exception ex)
            { 
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
