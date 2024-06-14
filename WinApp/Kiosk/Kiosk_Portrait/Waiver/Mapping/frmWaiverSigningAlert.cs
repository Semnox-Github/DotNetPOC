/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - Waiver Mapping
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.151.2     29-Dec-2023      Sathyavathi        Created for Waiver Mapping Enhancement
 ********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Collections.Generic;

namespace Parafait_Kiosk
{
    public partial class frmWaiverSigningAlert : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public KioskTransaction GetKioskTransaction { get { return kioskTransaction; } }

        public frmWaiverSigningAlert(KioskTransaction kioskTransaction)
        {
            log.LogMethodEntry("kioskTransaction");
            InitializeComponent();
            this.kioskTransaction = kioskTransaction;

            DisplaybtnHome(false);
            DisplaybtnCart(false);
            DisplaybtnCancel(true);
            DisplaybtnPrev(true);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            KioskStatic.setDefaultFont(this);
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            KioskStatic.Utilities.setLanguage(this);

            DisplayProductsDemandingWaiverSigning();
            SetDisplayElements();

            log.LogMethodExit();
        } 

        private void frmWaiverSigningAlert_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void DisplayProductsDemandingWaiverSigning()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            fLPProducts.Controls.Clear();
            fLPProducts.SuspendLayout();
            try
            {
                //Get all the lines that require waiver signing and then group by product id 
                List<Semnox.Parafait.Transaction.Transaction.TransactionLine> linesRequiringWaiverSigning = kioskTransaction.WaiverRequiredLines();
                if(linesRequiringWaiverSigning != null && linesRequiringWaiverSigning.Count == 0)
                {
                    btnStartSigning.Enabled = false;
                    txtMessage.Text = string.Empty;
                    //5467 - Waivers have been signed by each participant. Proceeding..
                    string alertMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5467);
                    string buttonText = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "OK");
                    frmOKMsg.ShowShortUserMessage(alertMsg, buttonText);
                }
                else if (linesRequiringWaiverSigning != null && linesRequiringWaiverSigning.Any())
                {
                    List<Semnox.Parafait.Transaction.Transaction.TransactionLine> trxLinesWithUniqueProducts = linesRequiringWaiverSigning.GroupBy
                        (t => new { t.ProductID }).Select(t => t.FirstOrDefault()).ToList();
                    foreach (Semnox.Parafait.Transaction.Transaction.TransactionLine trxLine in trxLinesWithUniqueProducts)
                    {
                        int quantity = kioskTransaction.GetCountOfParticipantsRequireWiaverSigning(trxLine.ProductID);
                        UsrCtrlWaiverAlert usrCtrl = new UsrCtrlWaiverAlert(KioskStatic.Utilities.ExecutionContext, trxLine.ProductName, quantity);
                        fLPProducts.Controls.Add(usrCtrl);
                    }
                }
                Application.DoEvents();
                fLPProducts.ResumeLayout();
            }
            catch (Exception ex)
            {
                string errMsg = "Error occurred while executing DisplayProductsDemandingWaiverSigning() in frmWaiverSigningAlert";
                log.Error(errMsg, ex);
                KioskStatic.logToFile(errMsg + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }
        
        private void btnStartSigning_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 1448);//Loading... Please wait...
                Application.DoEvents();
                using (frmMapAttendees frmMapAttendees = new frmMapAttendees(kioskTransaction))
                {
                    DialogResult dr = frmMapAttendees.ShowDialog();
                    kioskTransaction = frmMapAttendees.GetKioskTransaction;
                    if (dr != DialogResult.No)
                    {
                        //this.DialogResult = dr;
                        DialogResult = dr;
                        Close();
                    }
                    else
                    {
                        DisplayProductsDemandingWaiverSigning();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing btnStartSigning_Click", ex);
                KioskStatic.logToFile("Error on btnStartSigning_Click Click: " + ex.Message);
                string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Sorry unexpected error");
                frmOKMsg.ShowUserMessage(errMsg);
            }
            finally
            {
                //5462 - 'The items below in your cart require a waiver to be signed by each participant'
                txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Waiver Signing Required"); //Literal
            }
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Cancel Button Pressed : Triggering Home Button Action ");
                base.btnHome_Click(sender, e);
            }
            catch (Exception ex)
            {
                log.Error("Error in btnCancel_Click", ex);
            }
            log.LogMethodExit();
        } 
        private void SetOnScreenMessages()
        {
            log.LogMethodEntry();

            lblGreeting.Text = txtMessage.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Waiver Signing Required"); //Literal
            //5462 - 'The items below in your cart require a waiver to be signed by each participant'
            lblMsg.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5462);
            btnStartSigning.Text = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "Start Signing"); //Literal
            log.LogMethodExit();
        }

        private void SetDisplayElements()
        {
            log.LogMethodEntry();
            try
            {
                SetOnScreenMessages();
                SetCustomImages();
                SetCustomizedFontColors();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error setting display elements in frmWaiveSigningrAlert" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void SetCustomImages()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.WaiverSigningAlertScreenBackground);
                pnlProducts.BackgroundImage = ThemeManager.CurrentThemeImages.TablePurchaseSummary;
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                btnStartSigning.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                string msg = "Error while Setting Customized background images for frmWaiverSigningAlert: ";
                log.Error(msg, ex);
                KioskStatic.logToFile(msg + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            try
            {
                this.lblGreeting.ForeColor = KioskStatic.CurrentTheme.WaiverSigningAlertLblGreetingTextForeColor;
                this.lblMsg.ForeColor = KioskStatic.CurrentTheme.WaiverSigningAlertLblMsgTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.WaiverSigningAlertBackButtonTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.WaiverSigningAlertCancelButtonTextForeColor;
                this.btnStartSigning.ForeColor = KioskStatic.CurrentTheme.WaiverSigningAlertSignButtonTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.WaiverSigningAlertFooterTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors in frmWaiverSigningAlert", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmWaiverSigningAlert: " + ex.Message);
            }
            log.LogMethodExit();
        }        
    }
}
