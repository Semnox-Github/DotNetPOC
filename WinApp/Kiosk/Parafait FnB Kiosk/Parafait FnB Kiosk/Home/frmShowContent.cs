/********************************************************************************************
* Project Name - Parafait_Kiosk -frmShowContents.cs
* Description  - frmShowContents 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Parafait_FnB_Kiosk
{
    public partial class frmShowContent : BaseForm
    {
        public frmShowContent()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            Close();
            log.LogMethodExit();
        }

        private void frmRegisterTnC_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                    log.Error("Error occurred while executing frmRegisterTnC_FormClosing()" + ex.Message);
                }
            }
            log.LogMethodExit();
        }

        bool checkRichContent()
        {
            log.LogMethodEntry();
            object contentId = DBNull.Value;

            if (_callingElement.Parameters.Count > 0
            && _callingElement.Parameters[0].DataSource.Rows.Count > 0)
            {
                contentId = _callingElement.Parameters[0].DataSource.Rows[0][0];
            }

            DataTable dt = KioskStatic.Utilities.executeDataTable(@"select rc.data, rc.fileName
                                                                    from richContent rc, applicationContent ac
                                                                    left outer join ApplicationContentTranslated actl
                                                                    on actl.AppContentId = ac.AppContentId
                                                                    and actl.languageId = @langId
                                                                    where rc.id = isnull(actl.contentId, ac.contentId)
                                                                    and ac.AppContentId = @contentId",
                                                                   new SqlParameter("@contentId", contentId),
                                                                   new SqlParameter("@langId", Common.utils.ParafaitEnv.LanguageId));

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
                        panelBrowser.Visible = true;
                        

                        //this.Size = new Size(Screen.PrimaryScreen.WorkingArea.Width - 200, Screen.PrimaryScreen.Bounds.Height - 100);
                        //this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
                        //this.BackColor = Color.FromArgb(117, 47, 138);
                        //this.BackgroundImage = null;

                        //panelBrowser.Width = this.Width - 20;
                        //panelBrowser.Height = this.Height - 185;
                        //panelBrowser.Location = new Point(10, 10);

                        log.LogMethodExit(true);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Common.logToFile(ex.Message);
                    Common.ShowAlert(ex.Message);
                }
            }

            log.LogMethodExit(false);
            return false;
        }
        int xOrdinate = 0;
        int yOrdinate = 0;
        private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                wbTerms.Document.Window.ScrollTo(new Point(xOrdinate, e.NewValue));
                yOrdinate = e.NewValue;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing checkRichContent()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void frmShowContent_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.WindowState = FormWindowState.Normal;
            this.Size = this.BackgroundImage.Size;
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);

            lblScreenTitle.Text = _callingElement.Attribute.ActionScreenTitle1;

            if (!checkRichContent())
                Close();
            log.LogMethodExit();
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                wbTerms.Document.Window.ScrollTo(new Point(e.NewValue, yOrdinate));
                xOrdinate = e.NewValue;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while executing hScrollBar_Scroll()" + ex.Message);
            }
            log.LogMethodExit();
        }
        
    }
}
