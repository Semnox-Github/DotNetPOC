/********************************************************************************************
 * Project Name - Parafait_Kiosk
 * Description  - frmViewWaiverUI UI form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmViewWaiverUI : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private WaiversDTO waiversDTO;
        private Utilities utilities;
        private List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList;
        private System.Windows.Forms.Timer loaderTimer = new System.Windows.Forms.Timer();
        private Dictionary<int, string> waiverIDLanguageFileName;
        public frmViewWaiverUI(WaiversDTO waiverDTO)
        {
            log.LogMethodEntry(waiverDTO);
            this.utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            this.waiversDTO = waiverDTO;
            waiverIDLanguageFileName = new Dictionary<int, string>();
            if (waiversDTO != null && waiversDTO.WaiverSetDetailId > -1)
            {
                waiverIDLanguageFileName.Add(waiversDTO.WaiverSetDetailId, waiversDTO.WaiverFileName);
            }
            RefreshWaiverIdAndFileName(utilities.ParafaitEnv.LanguageId);
            KioskStatic.logToFile("Loading View Waiver form with waiver file");
            LoadWaiverFile();
            SetCustomizedFontColors();
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        public frmViewWaiverUI(List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList)
        {
            log.LogMethodEntry(customerSignedWaiverDTOList);
            this.utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            utilities.setLanguage(this);
            KioskStatic.setDefaultFont(this);
            this.customerSignedWaiverDTOList = customerSignedWaiverDTOList;
            KioskStatic.logToFile("Loading View Waiver form with customer signed waiver file list");
            LoadCustomerSignedWaiverSetFile();
            SetCustomizedFontColors();
            log.LogMethodExit();
        }

        private void frmViewWaiverUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            StartKioskTimer();
            ResetKioskTimer();
            this.Cursor = Cursors.WaitCursor;
            this.Cursor = Cursors.Default;
            try
            {
                this.pnlWaiverDisplay.SuspendLayout();
                this.SuspendLayout();
            }
            finally
            {
                this.pnlWaiverDisplay.ResumeLayout(true);
                this.ResumeLayout(true);
            }
            this.btnClose.Focus();
            log.LogMethodExit();
        }

        private void LoadWaiverFile()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Loading Waiver file");
            try
            {
                this.pnlWaiverDisplay.SuspendLayout();
                this.SuspendLayout();
                log.LogVariableState("this.waiversDTO.WaiverFileName", this.waiversDTO.WaiverFileName);
                if (this.waiversDTO != null && string.IsNullOrEmpty(this.waiversDTO.WaiverFileName) == false)
                {
                    if (tabControl != null)
                    {
                        tabControl.Visible = false;
                        this.pnlWaiverDisplay.Controls.Remove(tabControl);
                    }
                    //this.pnlWaiverDisplay.SuspendLayout();
                    lblWaiverName.Text = waiverIDLanguageFileName[waiversDTO.WaiverSetDetailId]; //waiversDTO.Name;
                    GenericUtils genericUtils = new GenericUtils();
                    string fileWithPath = genericUtils.DownloadFileFromDB(waiverIDLanguageFileName[waiversDTO.WaiverSetDetailId], utilities);
                    this.webBrowser.ClientSize = new System.Drawing.Size(this.pnlWaiverDisplay.Width - 5, this.pnlWaiverDisplay.Height - 5);
                    Uri urlLink = new Uri(fileWithPath + KioskStatic.PdfViewSettings);
                    webBrowser.Navigate(urlLink, false);
                    webBrowser.IsWebBrowserContextMenuEnabled = false;
                    webBrowser.WebBrowserShortcutsEnabled = false;
                    webBrowser.AllowNavigation = false;
                    webBrowser.AllowWebBrowserDrop = false;
                    //this.pnlWaiverDisplay.ResumeLayout();
                }
            }
            finally
            {
                this.pnlWaiverDisplay.ResumeLayout(true);
                this.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
        private void LoadCustomerSignedWaiverSetFile()
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Loading Customer Signed Waiver files");
            try
            {
                this.pnlWaiverDisplay.SuspendLayout();
                this.SuspendLayout();
                webBrowser.Visible = false;
                lblWaiverName.Visible = false;
                this.pnlWaiverDisplay.Controls.Remove(webBrowser);
                this.pnlWaiverDisplay.Height = this.pnlWaiverDisplay.Height + lblWaiverName.Height;
                this.pnlWaiverDisplay.Location = new System.Drawing.Point(this.pnlWaiverDisplay.Location.X, pnlWaiverDisplay.Location.Y - 90);
                if (this.customerSignedWaiverDTOList != null && this.customerSignedWaiverDTOList.Any())
                {
                    //tabControl = new TabControl();
                    tabControl.Font = new Font(lblWaiverName.Font.FontFamily, 18.0f, lblWaiverName.Font.Style);
                    tabControl.Size = new System.Drawing.Size(this.pnlWaiverDisplay.Width - 5, this.pnlWaiverDisplay.Height - 5);
                    foreach (CustomerSignedWaiverDTO item in this.customerSignedWaiverDTOList)
                    {
                        TabPage tabPage = new TabPage();
                        tabPage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Signed For") + ": " + (string.IsNullOrEmpty(item.SignedForName) ? String.Empty : item.SignedForName);
                        tabPage.Size = new System.Drawing.Size(tabControl.Width - 5, tabControl.Height - 5);
                        tabPage.Enter += new EventHandler(tabPage_Enter);

                        CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(utilities.ExecutionContext, item);
                        string fileWithPath = customerSignedWaiverBL.GetDecryptedWaiverFile(utilities.ParafaitEnv.SiteId);
                        Uri urlLink = new Uri(fileWithPath + KioskStatic.PdfViewSettings);
                        WebBrowser webBrowserNew = new WebBrowser();
                        webBrowserNew.Navigate(urlLink, false);
                        webBrowserNew.ClientSize = new System.Drawing.Size(tabPage.Width - 5, tabPage.Height - 5);
                        webBrowserNew.IsWebBrowserContextMenuEnabled = false;
                        webBrowserNew.WebBrowserShortcutsEnabled = false;
                        webBrowserNew.AllowNavigation = false;
                        webBrowserNew.AllowWebBrowserDrop = false;
                        tabPage.Controls.Add(webBrowserNew);
                        tabControl.Controls.Add(tabPage);
                    }
                    //this.pnlWaiverDisplay.Controls.Add(tabControl);
                }
            }
            finally
            {
                this.pnlWaiverDisplay.ResumeLayout(true);
                this.ResumeLayout(true);
            }
            log.LogMethodExit();
        }
        private void tabPage_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            KioskStatic.logToFile("Cancel button is clicked");
            log.LogMethodExit();
        }
        private void frmViewWaiverUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (webBrowser.Visible)
                {
                    try
                    {
                        string fileName = webBrowser.Url.LocalPath.ToString();
                        webBrowser.Dispose();
                        FileInfo file = new FileInfo(fileName);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                else
                {
                    DeleteFilesFromTabPages();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void DeleteFilesFromTabPages()
        {
            log.LogMethodEntry();
            List<TabControl> tabControlElementList = pnlWaiverDisplay.Controls.OfType<TabControl>().ToList();
            if (tabControlElementList != null)
            {
                for (int i = 0; i < tabControlElementList.Count; i++)
                {
                    foreach (TabPage tapPageItem in tabControlElementList[i].TabPages)
                    {
                        foreach (Control pageElement in tapPageItem.Controls)
                        {
                            if (pageElement is WebBrowser)
                            {
                                try
                                {
                                    WebBrowser webBrowser = (WebBrowser)pageElement;
                                    string fileName = webBrowser.Url.LocalPath.ToString();
                                    webBrowser.Dispose();
                                    webBrowser = null;
                                    FileInfo file = new FileInfo(fileName);
                                    if (file.Exists)
                                    {
                                        file.Delete();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();  //This action will clear current customer session. Do you want to proceed?
            using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2459)))//"This action will clear current customer session. Do you want to proceed?")))
            {
                if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    base.btnHome_Click(sender, e);
                }
            }
            log.LogMethodExit();
        }
        private void RefreshWaiverIdAndFileName(int languageId)
        {
            log.LogMethodEntry(languageId);
            bool foundTranslatedFile;
            log.LogVariableState("waiverIDLanguageFileName -before refresh", waiverIDLanguageFileName);
            if (waiversDTO != null && waiversDTO.WaiverSetDetailId > -1)
            {
                foundTranslatedFile = false;
                if (waiversDTO.ObjectTranslationsDTOList != null && waiversDTO.ObjectTranslationsDTOList.Any())
                {
                    ObjectTranslationsDTO objectTranslationsDTO = waiversDTO.ObjectTranslationsDTOList.Find(otl => otl.LanguageId == languageId && otl.ElementGuid == waiversDTO.Guid);
                    if (objectTranslationsDTO != null)
                    {
                        waiverIDLanguageFileName[waiversDTO.WaiverSetDetailId] = objectTranslationsDTO.Translation;
                        foundTranslatedFile = true;
                    }
                }
                if (foundTranslatedFile == false)
                {
                    waiverIDLanguageFileName[waiversDTO.WaiverSetDetailId] = waiversDTO.WaiverFileName;
                }
            }
            log.LogVariableState("waiverIDLanguageFileName -after refresh", waiverIDLanguageFileName);
            log.LogMethodExit();
        }
        private void SetTextBoxFontColors()
        {
            log.LogMethodEntry();
            if (KioskStatic.CurrentTheme == null ||
               (KioskStatic.CurrentTheme != null && KioskStatic.CurrentTheme.TextForeColor == Color.White))
            {
                webBrowser.ForeColor = Color.Black;
            }
            else
            {
                webBrowser.ForeColor = KioskStatic.CurrentTheme.TextForeColor;
            }
            log.LogMethodExit();
        }

        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmViewWaiverUI");
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmViewWaiverUIBtnHomeTextForeColor;
                this.lblWaiverName.ForeColor = KioskStatic.CurrentTheme.FrmViewWaiverUILblWaiverNameTextForeColor;
                this.webBrowser.ForeColor = KioskStatic.CurrentTheme.FrmViewWaiverUIWebBrowserTextForeColor;
                this.btnClose.ForeColor = KioskStatic.CurrentTheme.FrmViewWaiverUIBtnCloseTextForeColor;
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.ViewWaiverUIBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnClose.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmViewWaiverUI: " + ex.Message);
            }
            log.LogMethodExit();
        }
        public override void Form_Deactivate(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }
}
