/********************************************************************************************
* Project Name - Parafait_Kiosk - frmAdmin
* Description  - frmAdmin.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using Semnox.Parafait.KioskCore;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace Parafait_Kiosk
{
    public partial class frmAdmin : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool managerCardFlag = false;
        ExecutionContext executionContext = null;
        private string staffCardNumber;

        public frmAdmin(ExecutionContext executionContext, string cardNumber, bool isManagerCard = false)
        {
            log.LogMethodEntry(cardNumber, isManagerCard);
            this.executionContext = executionContext;
            InitializeComponent();
            staffCardNumber = cardNumber;
            managerCardFlag = isManagerCard;
            KioskStatic.setDefaultFont(this);
            SetCustomizedFontColors();
            DisplaybtnCancel(true);
            DisplaybtnHome(false);
            KioskStatic.Utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("SetUp pressed");
                using (SetUp sf = new SetUp())
                {
                    sf.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                KioskStatic.logToFile("Exit pressed");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnPrintSummary_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("Print Summary pressed");

                using (frmKioskPrintSummary frm = new frmKioskPrintSummary(staffCardNumber))
                {
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error printing Kiosk Summary: " + ex.Message);
                using (frmOKMsg frm = new frmOKMsg(ex.Message, true))
                {
                    frm.ShowDialog();
                }
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                flowLayoutPanel1.SuspendLayout();
                this.SuspendLayout();
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_LOAD_BONUS_IN_ADMIN_SCREEN", false))
                {
                    btnLoadBonus.Enabled = true;
                }
                else
                {
                    btnLoadBonus.Enabled = false;
                }
                if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "ENABLE_TRX_VIEW_IN_ADMIN_SCREEN", false))
                {
                    btnTrxView.Enabled = true;
                }
                else
                {
                    btnTrxView.Enabled = false;
                }
                flowLayoutPanel1.ResumeLayout(true);
                this.ResumeLayout(true);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error loading Admin form: " + ex.Message);
            }
            KioskStatic.logToFile("Admin Screen opened");
            log.LogMethodExit();
        }

        private void btnLoadBonus_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("Load Bonus pressed");
                using (frmTapCard ftc = new frmTapCard())
                {
                    ftc.ShowDialog();
                    if (ftc.Card == null)
                    {
                        ftc.Dispose();
                        log.LogMethodExit();
                        return;
                    }

                    if (ftc.Card.technician_card == 'Y')
                    {
                        using (frmLoadBonus flb = new frmLoadBonus(ftc.Card))
                        {
                            flb.ShowDialog();
                        }
                    }
                    else
                    {
                        frmOKMsg.ShowUserMessage("Please tap your staff card");
                    }
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);
            if (tickSecondsRemaining < 10)
            {
                StopKioskTimer();
                KioskStatic.logToFile("Time-out exit");
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        private void btnKioskActivity_Click(object sender, EventArgs e)//25-06-2015:Starts
        {
            log.LogMethodEntry();
            try
            {
                ResetKioskTimer();
                StopKioskTimer();
                KioskStatic.logToFile("KioskActivity pressed");
                using (KioskActivityDetails kioskactivity = new KioskActivityDetails(executionContext, managerCardFlag))
                {
                    kioskactivity.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void btnReboot_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("btnReboot_Click()");
            System.Diagnostics.Process.Start("shutdown.exe", "-r -f -t 00");
            log.LogMethodExit();
        }

        private void btnTrxView_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                ResetKioskTimer();
                KioskStatic.logToFile("Transaction View pressed");
                using (frmKioskTransactionView flb = new frmKioskTransactionView(managerCardFlag))
                {
                    flb.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                KioskStatic.logToFile(ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmAdmin");
            try
            {
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnCancelTextForeColor;//#AdminScreenBtnCancelForeColor
                this.btnExit.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnExitTextForeColor;//#AdminScreenBtnExitForeColor
                this.btnSetup.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnSetupTextForeColor;//#AdminScreenBtnSetupForeColor
                this.btnLoadBonus.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnLoadBonusTextForeColor;//AdminScreenBtnLoadBonusForeColor
                this.btnPrintSummary.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnPrintSummaryTextForeColor;//AdminScreenBtnPrintSummaryForeColor
                this.btnKioskActivity.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnKioskActivityTextForeColor;//AdminScreenBtnKioskActivityForeColor
                this.btnReboot.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnRebootComputerTextForeColor;//AdminScreenBtnRebootComputerForeColor
                this.btnTrxView.ForeColor = KioskStatic.CurrentTheme.AdminScreenBtnTrxViewTextForeColor;//AdminScreenBtnRebootComputerForeColor
                btnCancel.BackgroundImage =
                    btnExit.BackgroundImage =
                    btnSetup.BackgroundImage =
                    btnLoadBonus.BackgroundImage =
                    btnPrintSummary.BackgroundImage =
                    btnKioskActivity.BackgroundImage =
                    btnReboot.BackgroundImage =
                    btnTrxView.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;
                if (ThemeManager.CurrentThemeImages.AdminBackgroundImage != null)
                {
                    this.BackgroundImage = ThemeManager.CurrentThemeImages.AdminBackgroundImage;
                    this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                }
                else
                {
                    this.flowLayoutPanel1.BackColor = System.Drawing.Color.Thistle;
                }
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements  in frmAdmin: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
