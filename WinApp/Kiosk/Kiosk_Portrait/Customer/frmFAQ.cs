/********************************************************************************************
* Project Name - Parafait_Kiosk - frmFAQ
* Description  - frmFAQ.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        05-Sep-2019      Deeksha            Added logger methods.
*2.130.0     09-Jul-2021      Dakshak            Theme changes to support customized Font ForeColor
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
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
    public partial class frmFAQ : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmFAQ()
        {
            log.LogMethodEntry();
            SetStyle(ControlStyles.DoubleBuffer, true); 

            InitializeComponent();
            KioskStatic.setDefaultFont(this);//Modification on 17-Dec-2015 for introducing new theme
            btnPrev.BackgroundImage = btnSampleTopic.BackgroundImage = ThemeManager.CurrentThemeImages.TermsButton;
            this.StartPosition = FormStartPosition.Manual;
            this.WindowState = FormWindowState.Normal;
            this.BackgroundImage = ThemeManager.GetBackgroundImageTwo(ThemeManager.CurrentThemeImages.FAQBackgroundImage);//Starts:Modification on 17-Dec-2015 for introducing new theme
            btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.TermsButton;
            btnSampleTopic.BackgroundImage = ThemeManager.CurrentThemeImages.TermsButton;//Ends:Modification on 17-Dec-2015 for introducing new theme
            lblHeading.Text = KioskStatic.Utilities.MessageUtils.getMessage(801);
            SetCustomizedFontColors();
            DisplaybtnPrev(true);
            DisplaybtnHome(false);
            log.LogMethodExit();
        }


        private void frmFAQ_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - 50, Screen.PrimaryScreen.Bounds.Height - 400);
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);

            DataTable dt = KioskStatic.Utilities.executeDataTable(@"select ac.AppContentId, isnull(actl.chapter, ac.chapter) chapter
                                                                from applicationcontent ac
                                                                left outer join ApplicationContentTranslated actl
                                                                on actl.AppContentId = ac.AppContentId
                                                                and actl.languageId = @langId
                                                                where ac.Application = 'KIOSK'
                                                                and ac.Module = 'FAQ'",
                                                                new SqlParameter("@langId", KioskStatic.Utilities.ParafaitEnv.LanguageId));

            flpTopics.Controls.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                Button btnTopic = new Button();
                btnTopic.BackColor = btnSampleTopic.BackColor;
                btnTopic.ForeColor = btnSampleTopic.ForeColor;
                btnTopic.BackgroundImage = btnSampleTopic.BackgroundImage;
                btnTopic.BackgroundImageLayout = btnSampleTopic.BackgroundImageLayout;
                btnTopic.FlatStyle = btnSampleTopic.FlatStyle;
                btnTopic.FlatAppearance.BorderSize = 0;
                btnTopic.FlatAppearance.CheckedBackColor =
                btnTopic.FlatAppearance.MouseOverBackColor =
                btnTopic.FlatAppearance.MouseDownBackColor = Color.Transparent;
                btnTopic.Font = btnSampleTopic.Font;
                btnTopic.Size = btnSampleTopic.Size;
                btnTopic.Margin = btnSampleTopic.Margin;

                btnTopic.Text = btnTopic.Name = dr["chapter"].ToString();
                btnTopic.Tag = dr["AppContentId"];

                btnTopic.Click += btnTopic_Click;

                flpTopics.Controls.Add(btnTopic);
            }

            KioskStatic.Utilities.setLanguage(this);

            if (flpTopics.Controls.Count > 0)
                btnTopic_Click((flpTopics.Controls[0] as Button), null);
            log.LogMethodExit();
        }

        void btnTopic_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            Button b = sender as Button;

            DataTable dt = KioskStatic.Utilities.executeDataTable(@"select rc.data, rc.fileName
                                                                    from richContent rc, applicationContent ac
                                                                    left outer join ApplicationContentTranslated actl
                                                                    on actl.AppContentId = ac.AppContentId
                                                                    and actl.languageId = @langId
                                                                    where rc.id = isnull(actl.contentId, ac.contentId)
                                                                    and ac.AppContentId = @appContentId",
                                                                    new SqlParameter("@langId", KioskStatic.Utilities.ParafaitEnv.LanguageId),
                                                                    new SqlParameter("@appContentId", b.Tag));

            if (dt.Rows.Count == 0 || dt.Rows[0]["data"] == DBNull.Value)
                wbFAQ.Url = null;
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
                            log.Error(ex.Message);
                        }
                        string tempFile = System.IO.Path.GetTempPath() + "ParafaitKioskFAQ" + Guid.NewGuid().ToString() + extension;
                        using (FileStream file = new System.IO.FileStream(tempFile, FileMode.Create, System.IO.FileAccess.Write))
                        {
                            file.Write(bytes, 0, bytes.Length);
                        }

                        wbFAQ.Url = new Uri(tempFile);
                    }
                    else
                    {
                        wbFAQ.Url = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void frmFAQ_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                wbFAQ.Url = null;
                wbFAQ.Dispose();
                DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetTempPath());
                foreach (FileInfo fi in di.GetFiles("ParafaitKioskFAQ*"))
                {
                    try
                    {
                        fi.Delete();
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        //MessageBox.Show(ex.Message);
                    }
                }
            }catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            try
            {
                wbFAQ.Document.Window.ScrollTo(new Point(0, e.NewValue));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmFAQ");
            try
            {
                this.lblHeading.ForeColor = KioskStatic.CurrentTheme.FAQScreenHeaderTextForeColor;//Heading
                this.btnSampleTopic.ForeColor = KioskStatic.CurrentTheme.FAQScreenBtnTermsTextForeColor;//Buy new card/ terms
                this.btnPrev.ForeColor = KioskStatic.CurrentTheme.FAQScreenBtnBackTextForeColor;//Back button
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements  in frmFAQ: " + ex.Message);
            }
            log.LogMethodExit();
        }
    }
}
