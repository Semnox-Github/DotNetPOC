/********************************************************************************************
 * Project Name - Parafait_POS
 * Description  - WaiverCashierWindow form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.80        11-Oct-2019      Guru S A       Waiver phase 2 enhancement
 *2.100       19-Oct-2020      Guru S A       Enabling minor signature option for waiver
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmWaiverCashierWindow : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities = POSStatic.Utilities;
        private const string WARNING = "WARNING";
        private const string ERROR = "ERROR";
        private const string MESSAGE = "MESSAGE"; 

        //Dictionary<int, Image> imageFileList = new Dictionary<int, Image>();
        //List<WaiversDTO> DeviceWaiverSetDetailDTOList = new List<WaiversDTO>();
        //List<WaiversDTO> ManualWaiverSetDetailDTOList = new List<WaiversDTO>();

        //int tabCount = 0;
        //TabPage tabPage;
        //List<TabPage> tabPageList = new List<TabPage>();
        //List<AxAcroPDF> axAcroPDFList = new List<AxAcroPDF>();
        private string folderPath = "";
        private List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOList;
        private bool userAction = false;


        public frmWaiverCashierWindow(List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOList)
        {
            log.LogMethodEntry(waiverCustomerAndSignatureDTOList);
            InitializeComponent(); 
            userAction = false;
            //if (imageFileList != null && imageFileList.Count > 0)
            //{
            //    this.imageFileList = imageFileList;
            //}
            this.waiverCustomerAndSignatureDTOList = waiverCustomerAndSignatureDTOList;
            //this.pbSignature.SizeMode = PictureBoxSizeMode.StretchImage;
            //this.DeviceWaiverSetDetailDTOList = DeviceWaiverSetDetailDTOList;
            //this.ManualWaiverSetDetailDTOList = ManualWaiverSetDetailDTOList;
            LoadWaiverTabPages();
            log.LogMethodExit();
        }

        void LoadWaiverTabPages()
        {
            log.LogMethodEntry();
            userAction = false;
            try
            { 
                folderPath = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "IMAGE_DIRECTORY");
                string readText = string.Empty;
                tbCtrlWaiverVerification.ItemSize = new Size(90, 50);
                int height = 0;
                if (waiverCustomerAndSignatureDTOList != null && waiverCustomerAndSignatureDTOList.Any())
                {
                    foreach (WaiverCustomerAndSignatureDTO item in waiverCustomerAndSignatureDTOList)
                    {
                        TabPage newTabPage = CreateTabPage(item);
                        if (height < newTabPage.Height)
                        {
                            height = newTabPage.Height;
                        }
                        tbCtrlWaiverVerification.Controls.Add(newTabPage);
                    }
                }
                tbCtrlWaiverVerification.Controls.Remove(tabPage1);
                if (tbCtrlWaiverVerification.Height < height + 5)
                {
                    tbCtrlWaiverVerification.Size = new Size(tbCtrlWaiverVerification.Width, height + 5);
                }
                 
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            userAction = true;
            if (tbCtrlWaiverVerification != null)
            {
                tbCtrlWaiverVerification.TabIndex = 0;
                RefresherWaiverDetails();
            }
            log.LogMethodExit();
        }

        private TabPage CreateTabPage(WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO)
        {
            log.LogMethodEntry();
            int heightAdjusterValue = ((waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList == null
                                        || waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList.Any() == false) ? fPnlSignatures.Height * -1 : fPnlSignatures.Height);

            TabPage newTabPage = new TabPage();
            newTabPage.Size = new Size(tbCtrlWaiverVerification.Width - 2, (tbCtrlWaiverVerification.Height - 5- (heightAdjusterValue < 0? heightAdjusterValue : 0)));
            newTabPage.Tag = waiverCustomerAndSignatureDTO;
            newTabPage.Text = waiverCustomerAndSignatureDTO.WaiversDTO.Name;
            
            Panel pnlWaiver = new Panel();
            pnlWaiver.Location = new Point(newTabPage.Bounds.X + 5, newTabPage.Bounds.Y + 5);
            pnlWaiver.Size = new Size(newTabPage.Width - 5, newTabPage.Height - 5);
            pnlWaiver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            string fileName = waiverCustomerAndSignatureDTO.WaiversDTO.WaiverFileName; //waiverIDLanguageFileName[waiverCustomerAndSignatureDTO.WaiversDTO.WaiverSetDetailId];
            string fileWithPath = GetFileNameWithPath(fileName);
            WebBrowser webBrowser = new WebBrowser();
            Uri urlLink = new Uri(fileWithPath);
            webBrowser.Navigate(urlLink, false);
            webBrowser.Location = new Point(pnlWaiver.Location.X, 5);
            webBrowser.Name = waiverCustomerAndSignatureDTO.WaiversDTO.WaiverFileName.Substring(0, waiverCustomerAndSignatureDTO.WaiversDTO.WaiverFileName.LastIndexOf('.'));
            webBrowser.Size = new Size(pnlWaiver.Width - 20, this.Height - btnOK.Height - (heightAdjusterValue > 0? fPnlSignatures.Height : 0) - 5);
            pnlWaiver.Controls.Add(webBrowser);
            newTabPage.Controls.Add(pnlWaiver);
            log.LogMethodExit(newTabPage);
            return newTabPage;
        }

        private string GetFileNameWithPath(string fileName)
        {
            log.LogMethodEntry();
            string fileNameWithPath = string.Empty;
            GenericUtils genericUtils = new GenericUtils();
            fileNameWithPath = genericUtils.DownloadFileFromDB(fileName, utilities);
            log.LogMethodExit(fileNameWithPath);
            return fileNameWithPath;
        }
         
        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }
         
        private void tbCtrlWaiverVerification_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (userAction)
            {
                RefresherWaiverDetails();
            }
            log.LogMethodExit(); 
        }

        private void RefresherWaiverDetails()
        {
            log.LogMethodEntry(tbCtrlWaiverVerification.SelectedTab);
            if (tbCtrlWaiverVerification.SelectedTab != null &&
                   tbCtrlWaiverVerification.SelectedTab.Tag != null &&
                   tbCtrlWaiverVerification.SelectedTab.Tag is WaiverCustomerAndSignatureDTO)
            {
                WaiverCustomerAndSignatureDTO selectTabDTO = tbCtrlWaiverVerification.SelectedTab.Tag as WaiverCustomerAndSignatureDTO;
                if (selectTabDTO.CustIdNameSignatureImageList != null && selectTabDTO.CustIdNameSignatureImageList.Any())
                { 
                    fPnlSignatures.Controls.Clear();
                    flowLayoutPanel1.SuspendLayout();
                    fPnlSignatures.SuspendLayout(); 
                    Semnox.Parafait.Customer.CustomerBL signatoryCustomerBL = new Semnox.Parafait.Customer.CustomerBL(utilities.ExecutionContext, selectTabDTO.SignatoryCustomerDTO);
                    bool adultCustomer = signatoryCustomerBL.IsAdult();
                    for (int i = 0; i < selectTabDTO.CustIdNameSignatureImageList.Count; i++)
                    {
                        Panel pnlCustSignature = new Panel(); 
                        PictureBox pnSignature = BuildSignaturePictureBox(selectTabDTO.CustIdNameSignatureImageList[i].CustomerId,
                                                                          selectTabDTO.CustIdNameSignatureImageList[i].SignatureImage);
                        bool isGuardian = adultCustomer && selectTabDTO.SignatoryCustomerDTO.Id == selectTabDTO.CustIdNameSignatureImageList[i].CustomerId;
                        Label lblName = BuildNameTag(isGuardian);
                         pnlCustSignature.Size = new Size(pnSignature.Width + 2, pnSignature.Height + lblName.Height + 5); 
                        pnlCustSignature.Controls.Add(pnSignature);
                        lblName.Location = new Point(pnSignature.Location.X, pnSignature.Location.Y + pnSignature.Height + 1);
                        pnlCustSignature.Controls.Add(lblName);
                        fPnlSignatures.Controls.Add(pnlCustSignature); 
                    }
                    fPnlSignatures.ResumeLayout(true); 
                    fPnlSignatures.Refresh();
                    flowLayoutPanel1.PerformLayout();
                    flowLayoutPanel1.Refresh();
                }
                else
                {
                    fPnlSignatures.Size = new Size(fPnlSignatures.MinimumSize.Width, fPnlSignatures.MinimumSize.Height);
                }
            }
            log.LogMethodExit();
        }
        
        private static PictureBox BuildSignaturePictureBox(int custId, Image signatureImage)
        {
            log.LogMethodEntry(custId);
            PictureBox pnSignature = new PictureBox(); 
            pnSignature.Size = new System.Drawing.Size(265, 101);
            pnSignature.MinimumSize = new System.Drawing.Size(265, 101);
            pnSignature.SizeMode = PictureBoxSizeMode.StretchImage;
            pnSignature.Tag = custId;
            pnSignature.Image = signatureImage;
            pnSignature.BorderStyle = BorderStyle.FixedSingle;
            log.LogMethodExit();
            return pnSignature;
        }

        private Label BuildNameTag(bool guardian)
        {
            log.LogMethodEntry(guardian);
            Label lblName = new Label();
            lblName.Size = new Size(265, 20);
            lblName.AutoEllipsis = true;
            lblName.Margin = new Padding(1);
            lblName.BackColor = Color.Transparent;
            lblName.TextAlign = ContentAlignment.MiddleCenter; 
            lblName.Text = (guardian ? MessageContainerList.GetMessage(utilities.ExecutionContext, "Guardian")
                                     : MessageContainerList.GetMessage(utilities.ExecutionContext, "Participant"));
            log.LogMethodExit();
            return lblName;
        }

        private void frmWaiverCashierWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                //axAcroPDF.Dispose(); 
                DeleteFilesFromTabPages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        private void DeleteFilesFromTabPages()
        {
            log.LogMethodEntry();

            foreach (TabPage tapPageItem in tbCtrlWaiverVerification.TabPages)
            {
                foreach (Control pageElement in tapPageItem.Controls)
                {
                    if (pageElement is Panel)
                    {
                        foreach (Control panelElement in pageElement.Controls)
                        {
                            if (panelElement is WebBrowser)
                            {
                                try
                                {
                                    WebBrowser webBrowser = (WebBrowser)panelElement;
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
