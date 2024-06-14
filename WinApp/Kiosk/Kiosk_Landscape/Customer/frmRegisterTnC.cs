/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmRegisterTnC.cs
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.80         4-Sep-2019       Deeksha        Added logger methods.
 *2.150.1      22-Feb-2023       Guru S A       Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
    public partial class frmRegisterTnC : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        int ticks = 0;
        public frmRegisterTnC()
        {
            log.LogMethodEntry();
            Audio.PlayAudio(Audio.RegisterTermsAndConditions);
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            //this.Size = this.BackgroundImage.Size;
            this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnNo.BackgroundImage = btnYes.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;//Ends:Modification on 17-Dec-2015 for introducing new theme
            if (!checkRichContent())
                lblmsg.Text = KioskStatic.Utilities.MessageUtils.getMessage(760);
            else
                lblmsg.Visible = false;

            panelButtons.Location = new Point((this.Width - panelButtons.Width) / 2, panelButtons.Location.Y);

            //timer1.Start();
            SetKioskTimerTickValue(60);
            ResetKioskTimer();
            log.LogMethodExit();
        }

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

        private void btnNo_Click(object sender, EventArgs e)
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
                catch(Exception ex)
                {
                    log.Error("Error occurred while executing frmRegisterTnC_FormClosing()" + ex.Message);
                }
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
                        catch (Exception ex)
                        {
                            log.Error("Error occurred while executing checkRichContent()" + ex.Message);
                        }
                        string tempFile = System.IO.Path.GetTempPath() + "ParafaitKioskFAQ" + Guid.NewGuid().ToString() + extension;
                        using (FileStream file = new System.IO.FileStream(tempFile, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            file.Write(bytes, 0, bytes.Length);
                        }

                        wbTerms.Url = new Uri(tempFile);
                        panelBrowser.Location = new Point(15, 15);
                        panelBrowser.Visible = true;

                        this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - 200, Screen.PrimaryScreen.Bounds.Height - 100);
                        this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);

                        //panelBrowser.Size = this.Size;
                        panelBrowser.Height = this.Height - 185;
                        panelBrowser.Width = this.Width - 30;
                        panelButtons.Location = new Point(panelButtons.Location.X, panelButtons.Location.Y - 20);

                        log.LogMethodExit(true);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
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
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
