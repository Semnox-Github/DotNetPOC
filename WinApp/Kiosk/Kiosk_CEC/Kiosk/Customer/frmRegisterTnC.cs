/********************************************************************************************
 * Project Name - Customer
 * Description  - frmRegisterTnC.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80         3-Sep-2019      Deeksha            Added logger methods.
 *2.80.1     02-Feb-2021      Deeksha              Theme changes to support customized Images/Font
 *2.130.0      30-Jun-2021     Dakshakh          Theme changes to support customized Font ForeColor
 *********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmRegisterTnC : BaseFormKiosk
    {
        //int ticks = 0;
        public frmRegisterTnC()
        {
            log.LogMethodEntry();
            Audio.PlayAudio(Audio.RegisterTermsAndConditions);
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            //this.Size = this.BackgroundImage.Size;
            this.BackgroundImage = Semnox.Parafait.KioskCore.KioskStatic.CurrentTheme.TapCardBackgroundImage;

            if (!checkRichContent())
                lblmsg.Text = KioskStatic.Utilities.MessageUtils.getMessage(760);
            else
                lblmsg.Visible = false;

            panelButtons.Location = new Point((this.Width - panelButtons.Width) / 2, panelButtons.Location.Y);
            this.BringToFront();
            //timer1.Start();
            SetKioskTimerTickValue(60);
            ResetKioskTimer();
            displaybtnPrev(false);
            displaybtnCancel(true);
            KioskStatic.Utilities.setLanguage(this);
            SetCustomizedFontColors();

            log.LogMethodExit();
        }

        //Begin Timer Cleanup
        //protected override CreateParams CreateParams//12-06-2015:starts
        //{
        //    //this method is used to avoid the able layout flickering.
        //    get
        //    {
        //        CreateParams CP = base.CreateParams;
        //        CP.ExStyle = CP.ExStyle | 0x02000000;
        //        return CP;
        //    }
        //}
        //End Timer Cleanup

        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    ticks++;
        //    if (ticks == 50)
        //    {
        //        if (TimeOut.AbortTimeOut(this))
        //        {
        //            ticks = 0;
        //        }
        //    }

        //    if (ticks > 60)
        //    {
        //        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        //        Close();
        //    }
        //}

        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int tickSecondsRemaining = GetKioskTimerSecondsValue();
            tickSecondsRemaining--;
            setKioskTimerSecondsValue(tickSecondsRemaining);

            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                {
                    ResetKioskTimer();
                }
                else
                    tickSecondsRemaining = 0;
            }

            if (tickSecondsRemaining <= 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            log.LogMethodExit();
        }

        //private void btnNo_Click(object sender, EventArgs e)
        //{
        //    this.DialogResult = System.Windows.Forms.DialogResult.No;
        //    Close();
        //}
        public override void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
            log.LogMethodExit();
        }

        private void frmRegisterTnC_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer1.Stop();
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
                catch { }
            }
            log.LogMethodExit();
        }

        bool checkRichContent()
        {
            log.LogMethodEntry();
            DataTable dt = KioskStatic.Utilities.executeDataTable(@"select rc.data, rc.fileName
                                                                    from richContent rc, applicationContent ac
                                                                    left outer join ApplicationContentTranslated actl
                                                                    on actl.AppContentId = ac.AppContentId
                                                                    and actl.languageId = @langId
                                                                    where rc.id = isnull(actl.contentId, ac.contentId)
                                                                    and ac.Application = 'KIOSK'
                                                                    and ac.Module = 'REGISTRATION'",
                                                                   new SqlParameter("@langId", KioskStatic.Utilities.ParafaitEnv.LanguageId));

            if (dt.Rows.Count == 0 || dt.Rows[0]["data"] == DBNull.Value)
            {
                wbTerms.Url = null;
            }
            else
            {
                try
                {
                    byte[] bytes = dt.Rows[0]["data"] as byte[];
                    if (bytes != null)
                    {
                        string extension = dt.Rows[0]["fileName"].ToString();
                        try
                        {
                            extension = (new FileInfo(extension)).Extension;
                        }
                        catch { }
                        string tempFile = System.IO.Path.GetTempPath() + "ParafaitKioskFAQ" + Guid.NewGuid().ToString() + extension;
                        using (FileStream file = new System.IO.FileStream(tempFile, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            file.Write(bytes, 0, bytes.Length);
                        }

                        wbTerms.Url = new Uri(tempFile);
                        panelBrowser.Visible = true;

                        this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - 200, Screen.PrimaryScreen.Bounds.Height - 100);
                        this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
                        this.BackColor = Color.FromArgb(117, 47, 138);
                        this.BackgroundImage = null;

                        panelBrowser.Width = this.Width - 20;
                        panelBrowser.Height = this.Height - 185;
                        panelBrowser.Location = new Point(10, 10);
                        panelButtons.Location = new Point(panelButtons.Location.X, panelButtons.Location.Y);

                        log.LogMethodExit(true);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            log.LogMethodExit(false);
            return false;
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                wbTerms.Document.Window.ScrollTo(new Point(0, e.NewValue));
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while executing vScrollBar_Scroll()" + ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Setting customized font colors for the UI elements");
            try
            {
                this.lblmsg.ForeColor = KioskStatic.CurrentTheme.RegisterTnCHeaderTextForeColor;
                this.btnYes.ForeColor = KioskStatic.CurrentTheme.RegisterTnCBtnYesTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.RegisterTnCBtnCancelTextForeColor;
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.RegisterTnCBtnPrevTextForeColor;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
