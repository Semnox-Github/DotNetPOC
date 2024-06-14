/********************************************************************************************
 * Project Name - Parafait_Kiosk - frmCustomerSignatureConfirmation
 * Description  - frmCustomerSignatureConfirmation 
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By       Remarks          
 ********************************************************************************************* 
 *2.100       19-Oct-2020      Guru S A           Created for Enabling minor signature option for waiver
 *2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
 *2.150.1     22-Feb-2023      Vignesh Bhat       Kiosk Cart Enhancements
 ********************************************************************************************/

using System; 
using System.Windows.Forms;
using Semnox.Parafait.Languages;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk.Waiver
{
    public partial class frmCustomerSignatureConfirmation : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineExecutionContext;
        public frmCustomerSignatureConfirmation(ExecutionContext executionContext, string customerName, bool guardian)
        {
            log.LogMethodEntry(executionContext, customerName, guardian);
            InitializeComponent();
            this.machineExecutionContext = executionContext;
            SetCustomerNameLabel(customerName, guardian);
            KioskStatic.setDefaultFont(this);
            ResetKioskTimer();
            SetCustomizedFontColors();
            KioskStatic.Utilities.setLanguage(this);
            KioskStatic.logToFile("Loading signature confirmation form");
            log.Info("Loading signature confirmation form");
            log.LogMethodExit();
        }

        private void SetCustomerNameLabel(string customerName, bool guardian)
        {
            log.LogMethodEntry(customerName, guardian);
            lblCustomerName.Text = string.Empty;
            lblCustomerName.Text = (guardian ? MessageContainerList.GetMessage(machineExecutionContext, "Guardian: ")
                                             : MessageContainerList.GetMessage(machineExecutionContext, "Participant: "));
            lblCustomerName.Text = lblCustomerName.Text + customerName;
            log.LogMethodExit();
        }

        private void chkSignConfirm_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (chkSignConfirm.Checked)
            {
                pbCheckBox.Image = Properties.Resources.tick_box_checked;
            }
            else
            {
                pbCheckBox.Image = Properties.Resources.tick_box_unchecked;
            }
            log.LogMethodExit();
        }


        private void pbCheckBox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            chkSignConfirm.Checked = !chkSignConfirm.Checked;
            log.LogMethodExit();
        }
        

        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("user clicked Okay button in signature confirmation screen"); 
            log.Info("user clicked Okay button in signature confirmation screen");
            try
            {
                ResetKioskTimer();
                if (chkSignConfirm.Checked)
                {                   
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    using (frmOKMsg frmOK = new frmOKMsg(MessageContainerList.GetMessage(machineExecutionContext, 1206), true))
                    {
                        frmOK.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex); 
                KioskStatic.logToFile(ex.Message); 
            } 
            log.LogMethodExit();

        }

        private void frmCustomerSignatureConfirmation_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Signature confirmation screen closed, user action is "+ this.DialogResult.ToString());
            log.Info("Signature confirmation screen closed, user action is "+ this.DialogResult.ToString());
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmCustomerSignatureConfirmation");
            try
            {
                this.lblCustomerName.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationLblCustomerNameTextForeColor;
                this.chkSignConfirm.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationChkSignConfirmTextForeColor;
                this.pbCheckBox.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationPbCheckBoxTextForeColor;
                this.btnOkay.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationBtnOkayTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmCustSignConfirmationBtnCancelTextForeColor;
                this.BackgroundImage = ThemeManager.GetPopupBackgroundImage(ThemeManager.CurrentThemeImages.CustomerSignatureConfirmationBox);
                btnOkay.BackgroundImage =
                    btnCancel.BackgroundImage = ThemeManager.GetBackButtonBackgroundImage(ThemeManager.CurrentThemeImages.CustomerSignatureConfirmationButtons);
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmCustomerSignatureConfirmation: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
