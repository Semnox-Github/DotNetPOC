/********************************************************************************************
 * Project Name - POS-Waivers
 * Description  - View Waiver UI form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.80.0      26-Sep-2019   Guru S A                Created for Waiver phase 2 enhancement changes  
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Parafait_POS.Waivers
{
    public partial class frmViewWaiverUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private WaiversDTO waiversDTO;
        private Utilities utilities; 
        private List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList;
        public frmViewWaiverUI(WaiversDTO waiverDTO, Utilities utilities)
        {
            log.LogMethodEntry(waiverDTO);
            InitializeComponent();
            this.utilities = utilities;
            this.waiversDTO = waiverDTO;
            LoadWaiverFile();
            log.LogMethodExit();
        }

        public frmViewWaiverUI(List<CustomerSignedWaiverDTO> customerSignedWaiverDTOList, Utilities utilities)
        {
            log.LogMethodEntry(customerSignedWaiverDTOList);
            InitializeComponent();
            this.utilities = utilities;
            this.customerSignedWaiverDTOList = customerSignedWaiverDTOList;
            LoadCustomerSignedWaiverSetFile();
            log.LogMethodExit();
        }
        private void LoadWaiverFile()
        {
            log.LogMethodEntry();
            if (this.waiversDTO != null && string.IsNullOrEmpty(this.waiversDTO.WaiverFileName) == false)
            {
                lblWaiverName.Text = this.waiversDTO.Name;
                wBrowser.Visible = true;
                GenericUtils genericUtils = new GenericUtils();
                string fileWithPath = genericUtils.DownloadFileFromDB(this.waiversDTO.WaiverFileName, utilities);
                Uri urlLink = new Uri(fileWithPath);
                wBrowser.Navigate(urlLink, false);  
            }
            log.LogMethodExit();
        }

        

        private void LoadCustomerSignedWaiverSetFile()
        {
            log.LogMethodEntry();
            if (this.customerSignedWaiverDTOList != null && this.customerSignedWaiverDTOList.Any())
            {
                wBrowser.Visible = false;
                lblWaiverName.Visible = false;
                pnlWaiverDisplay.Controls.Remove(wBrowser);
                pnlWaiverDisplay.Controls.Remove(lblWaiverName);
                TabControl tabControl = new TabControl();
                tabControl.Font = new Font(lblWaiverName.Font.FontFamily, 9.25f, lblWaiverName.Font.Style);
                tabControl.Size = new System.Drawing.Size(this.pnlWaiverDisplay.Width - 5, this.pnlWaiverDisplay.Height - 5);
                foreach (CustomerSignedWaiverDTO item in this.customerSignedWaiverDTOList)
                {
                    TabPage tabPage = new TabPage();
                    tabPage.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Signed For") + ": " + (string.IsNullOrEmpty(item.SignedForName) ? String.Empty : item.SignedForName);
                    tabPage.Size = new System.Drawing.Size(tabControl.Width - 5, tabControl.Height - 5);
                    CustomerSignedWaiverBL customerSignedWaiverBL = new CustomerSignedWaiverBL(utilities.ExecutionContext, item);
                    string fileWithPath = customerSignedWaiverBL.GetDecryptedWaiverFile(utilities.ParafaitEnv.SiteId);
                    Uri urlLink = new Uri(fileWithPath);
                    WebBrowser webBrowserNew = new WebBrowser();
                    webBrowserNew.Navigate(urlLink, false);
                    webBrowserNew.Dock = DockStyle.Fill;
                    tabPage.Controls.Add(webBrowserNew);
                    tabControl.Controls.Add(tabPage);
                    Thread.Sleep(100);
                }
                this.pnlWaiverDisplay.Controls.Add(tabControl);
            }
            log.LogMethodExit();
        }

        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void frmViewWaiverUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (wBrowser.Visible)
                {
                    try
                    { 
                        FileInfo file = new FileInfo(wBrowser.Url.LocalPath.ToString());
                        if (file.Exists)
                        {
                            wBrowser.Dispose();
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
                                    FileInfo file = new FileInfo(webBrowser.Url.LocalPath.ToString());
                                    if (file.Exists)
                                    {
                                        webBrowser.Dispose();
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
    }
}
