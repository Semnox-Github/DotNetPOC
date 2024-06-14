/********************************************************************************************
* Project Name - Parafait_Kiosk - frmRegisterTnC
* Description  - frmRegisterTnC.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        05-Sep-2019      Deeksha            Added logger methods.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.140.0     18-Oct-2021      Sathyavathi        Modified for Check-In Check-Out feature in Kiosk
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.0.0   23-Sep-2022      Sathyavathi        Check-In feature Phase-2
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Semnox.Parafait.KioskCore;
using Semnox.Core.GenericUtilities;
using System.Collections.Generic;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Parafait_Kiosk
{
    public partial class frmTermsAndConditions : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private KioskStatic.ApplicationContentModule applicationContentModule;

        public frmTermsAndConditions(KioskStatic.ApplicationContentModule contentModule)
        {
            log.LogMethodEntry();
            try
            {
                StopKioskTimer();
                Audio.PlayAudio(Audio.RegisterTermsAndConditions);

                InitializeComponent();
                KioskStatic.setDefaultFont(this);
                this.applicationContentModule = contentModule;
                RichContentDTO richContentDTO = null;
                switch (applicationContentModule)
                {
                    case KioskStatic.ApplicationContentModule.REGISTRATION:
                        richContentDTO = KioskStatic.RegistrationRichContentDTO;
                        break;
                    case KioskStatic.ApplicationContentModule.CHECKIN:
                        richContentDTO = KioskStatic.CheckInRichContentDTO;
                        break;
                    default:
                        break;
                }

                LoadRichContent(richContentDTO, applicationContentModule);
                panelButtons.Location = new Point(((this.Width - panelButtons.Width) / 2 ) + 26, panelButtons.Location.Y);
                this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.Bounds.Height);
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
                this.BackColor = Color.FromArgb(117, 47, 138);
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.TermsAndConditionsBackgroundImage);
                this.btnCancel.BackgroundImage = btnYes.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                SetCustomizedFontColors();
                DisplaybtnCancel(true);
                DisplaybtnPrev(false);
                DisplaybtnHome(false);
                KioskStatic.Utilities.setLanguage(this);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while loading Terms and Conditions: " + ex.Message);
            }
            //timer1.Start();
            try
            {
                SetKioskTimerTickValue(60);
                ResetKioskTimer();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error while loading Terms and Conditions: " + ex.Message);
            }
            finally
            {
                StartKioskTimer();
            }
            log.LogMethodExit();
        } 

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
            log.LogMethodExit();
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                ResetKioskTimer();
                if (applicationContentModule == KioskStatic.ApplicationContentModule.CHECKIN)
                {
                    //"This will abondon the transaction and take you to the home screen. Are you sure you want to exit?""
                    string screen = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "home");
                    bool isShowCartInKiosk = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<bool>(KioskStatic.Utilities.ExecutionContext, "SHOW_CART_IN_KIOSK", false);
                    string msg = (isShowCartInKiosk == false) ? MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4287, screen) //"This will abondon the transaction and take you back to the quantity screen"
                                : MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4850, screen) //"This action will remove the product from the Cart and take you back to the quantity screen"
                                + Environment.NewLine 
                                + MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 4291);

                    int screenTimeout = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault<int>(KioskStatic.Utilities.ExecutionContext, "BALANCE_SCREEN_TIMEOUT", 30000); //30 seconds
                    using (frmYesNo yesNo = new frmYesNo(msg, string.Empty, screenTimeout))
                    {
                        DialogResult dr = yesNo.ShowDialog();
                        if (dr == DialogResult.No)
                        {
                            this.DialogResult = DialogResult.None;
                            yesNo.Close();
                            log.LogMethodExit();
                            return;
                        }
                        else
                        {
                            this.DialogResult = DialogResult.No;
                            this.Close();
                        }
                    }
                }
                else
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                KioskStatic.logToFile("Error in btnCancel_Click() of Terms and Conditions file: " + ex.Message);
            }
            log.LogMethodExit();
            return;
        }

        private void frmTermsAndConditions_FormClosing(object sender, FormClosingEventArgs e)
        {
            // timer1.Stop();
            log.LogMethodEntry();
            Audio.Stop();
            wbTerms.Dispose();
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetTempPath());
            foreach (FileInfo fi in di.GetFiles("ParafaitKioskFAQ*"))
            {
                try
                {
                    fi.Delete();
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        public void LoadRichContent(RichContentDTO richContentDTO, KioskStatic.ApplicationContentModule contentModule)
        {
            log.LogMethodEntry(richContentDTO, contentModule);
            try
            {
                if (richContentDTO != null)
                {
                    byte[] bytes = richContentDTO.Data;
                    if (bytes != null)
                    {
                        string extension = richContentDTO.FileName;
                        try
                        {
                            extension = (new FileInfo(extension)).Extension;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex.Message);
                            KioskStatic.logToFile("Error getting Terms and Conditions Extention: " + ex.Message);
                        }
                        string tempFile = System.IO.Path.GetTempPath() + "ParafaitKioskFAQ" + Guid.NewGuid().ToString() + extension;
                        using (FileStream file = new System.IO.FileStream(tempFile, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            file.Write(bytes, 0, bytes.Length);
                        }

                        wbTerms.Url = new Uri(tempFile);
                        this.Tag = richContentDTO;

                        panelBrowser.Visible = true;

                        panelBrowser.Height = this.Height - 322;
                        panelBrowser.Width = this.Width;
                        panelBrowser.Location = new Point(10, 10);
                        panelButtons.Location = new Point(panelButtons.Location.X, panelButtons.Location.Y);
                    }
                    else
                    {
                        KioskStatic.logToFile("Content data is not available");
                        throw new Exception("Content data is not available");
                    }
                }
                else
                {
                    string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 5170); //"Setup Issue: Terms and Conditions content data is not set"
                    KioskStatic.logToFile(errMsg);
                    throw new Exception(errMsg);
                }
            }
            catch (Exception ex)
            {
                btnYes.Enabled = false;
                log.Error(ex);
                using (frmOKMsg frm = new frmOKMsg(ex.Message))
                {
                    frm.ShowDialog();
                }
                KioskStatic.logToFile(ex.Message + ":" + ex.StackTrace);
            }
            log.LogMethodExit();
        }
        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                wbTerms.Document.Window.ScrollTo(new Point(0, e.NewValue));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in Terms & Condition form");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.RegisterTnCHeaderTextForeColor;
                this.btnYes.ForeColor = KioskStatic.CurrentTheme.RegisterTnCBtnYesTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.RegisterTnCBtnCancelTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Error while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in Terms & Condition form: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
