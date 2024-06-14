/********************************************************************************************
 * Project Name - Portait Kiosk
 * Description  - frmSignWaiverFiles UI form
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
*2.150.3.0   28-Apr-2023      Vignesh Bhat        Created: TableTop Kiosk Changes
 ********************************************************************************************/
using iTextSharp.text.pdf;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmSignWaiverFiles : BaseFormKiosk
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities utilities;
        private List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOList;
        private string defaultMsg;
        private Dictionary<int, string> waiverIDLanguageFileName;
        private int fileIndex = 0;
        private List<WaiverSetDTO> waiverSetDTOList = null;
        private System.Windows.Forms.Timer loaderTimer = new System.Windows.Forms.Timer();
        internal class UIElementsDTO
        {
            //internal WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO;
            //internal Panel customerDisplayPanel;
            internal string fileWithPath;
        }
        internal List<UIElementsDTO> uiElementsDTOList;

        public List<WaiverCustomerAndSignatureDTO> GetSignedDTOList { get { return waiverCustomerAndSignatureDTOList; } }
        public frmSignWaiverFiles(List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOList)
        {
            log.LogMethodEntry(waiverCustomerAndSignatureDTOList);
            this.utilities = KioskStatic.Utilities;
            utilities.setLanguage();
            InitializeComponent();
            KioskStatic.setDefaultFont(this);
            this.chkSignConfirm.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, 2433);
            this.waiverCustomerAndSignatureDTOList = waiverCustomerAndSignatureDTOList;
            WaiverSetContainer waiverSetContainer = WaiverSetContainer.GetInstance;
            waiverSetDTOList = waiverSetContainer.GetWaiverSetDTOList(utilities.ExecutionContext.SiteId);
            uiElementsDTOList = new List<UIElementsDTO>();
            KioskStatic.formatMessageLine(txtMessage, 26, ThemeManager.CurrentThemeImages.BottomMessageLineImage);
            SetCustomizedFontColors();
            DisplaybtnCancel(true);
            utilities.setLanguage(this);
            KioskStatic.logToFile("Loading Sign waiver file form");
            log.LogMethodExit();
        }

        private void frmSelectWaiverOption_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            loaderTimer.Interval = 10;
            loaderTimer.Enabled = true;
            loaderTimer.Tick += LoaderTimer_Tick;
            log.LogMethodExit();
        }


        private void LoaderTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                loaderTimer.Enabled = false;
                loaderTimer.Stop();
                Application.DoEvents();
                FormLoadAction();
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile(ex.Message);
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message, true))
                {
                    frmOK.ShowDialog();
                }
            }
            loaderTimer.Enabled = false;
            loaderTimer.Stop();
            log.LogMethodExit();
        }
        private void FormLoadAction()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2439);
            ShowHideSignConfirmCheckBox();
            LoadIdAndFileNameList();
            RefreshWaiverIdAndFileName(utilities.ParafaitEnv.LanguageId);
            BuildWaiverFileInfo(fileIndex);
            //Thread.Sleep(60);
            //Application.DoEvents();
            //Please read carefully and sign
            DisplayMessageLine(defaultMsg);
            //Thread.Sleep(60);
            // Application.DoEvents();
            ResetKioskTimer();
            log.LogMethodExit();
        }
        private void LoadIdAndFileNameList()
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            waiverIDLanguageFileName = new Dictionary<int, string>();
            foreach (WaiverCustomerAndSignatureDTO item in waiverCustomerAndSignatureDTOList)
            {
                // waiverIDLanguageFileName.Add(new KeyValuePair<int, string>(item.WaiversDTO.WaiverSetDetailId, item.WaiversDTO.WaiverFileName)); 
                waiverIDLanguageFileName.Add(item.WaiversDTO.WaiverSetDetailId, item.WaiversDTO.WaiverFileName);
            }
            log.LogMethodExit();
        }

        private void RefreshWaiverIdAndFileName(int languageId)
        {
            log.LogMethodEntry(languageId);
            bool foundTranslatedFile;
            log.LogVariableState("waiverIDLanguageFileName -before refresh", waiverIDLanguageFileName);
            foreach (WaiverCustomerAndSignatureDTO item in waiverCustomerAndSignatureDTOList)
            {
                foundTranslatedFile = false;
                if (item.WaiversDTO.ObjectTranslationsDTOList != null && item.WaiversDTO.ObjectTranslationsDTOList.Any())
                {
                    ObjectTranslationsDTO objectTranslationsDTO = item.WaiversDTO.ObjectTranslationsDTOList.Find(otl => otl.LanguageId == languageId && otl.ElementGuid == item.WaiversDTO.Guid);
                    if (objectTranslationsDTO != null)
                    {
                        waiverIDLanguageFileName[item.WaiversDTO.WaiverSetDetailId] = objectTranslationsDTO.Translation;
                        foundTranslatedFile = true;
                    }
                }
                if (foundTranslatedFile == false)
                {
                    waiverIDLanguageFileName[item.WaiversDTO.WaiverSetDetailId] = item.WaiversDTO.WaiverFileName;
                }
            }
            log.LogVariableState("waiverIDLanguageFileName -after refresh", waiverIDLanguageFileName);
            log.LogMethodExit();
        }


        void DisplayMessageLine(string message)
        {
            log.LogMethodEntry(message);
            txtMessage.Text = message;
            ResetKioskTimer();
            log.LogMethodExit();
        }


        private void BuildWaiverFileInfo(int fileIndex)
        {
            log.LogMethodEntry(fileIndex);
            try
            {
                this.btnOkay.Enabled = false;
                ResetKioskTimer();
                if (waiverCustomerAndSignatureDTOList != null && waiverCustomerAndSignatureDTOList.Any())
                {
                    for (int i = 0; i < waiverCustomerAndSignatureDTOList.Count; i++)
                    {
                        if (fileIndex == i)
                        {
                            CreateWaiverPage(waiverCustomerAndSignatureDTOList[i]);
                            break;
                        }
                    }
                }
                Application.DoEvents();
            }
            finally
            {
                this.btnOkay.Enabled = true;
                this.Refresh();
            }
            log.LogMethodExit();
        }

        private void CreateWaiverPage(WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            this.chkSignConfirm.Checked = false;
            string WaiverSetName = string.Empty;
            if (waiverSetDTOList != null)
            {
                WaiverSetDTO waiverSetDTO = waiverSetDTOList.Find(ws => ws.WaiverSetId == waiverCustomerAndSignatureDTO.WaiversDTO.WaiverSetId);
                if (waiverSetDTO != null)
                {
                    WaiverSetName = waiverSetDTO.Description;
                }
            }
            this.lblSignWaiverFile.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Signing waiver") + " - "
                                                             + (fileIndex + 1).ToString(utilities.ParafaitEnv.NUMBER_FORMAT) + " "
                                                             + MessageContainerList.GetMessage(utilities.ExecutionContext, "of") + " "
                                                             + (waiverCustomerAndSignatureDTOList.Count).ToString(utilities.ParafaitEnv.NUMBER_FORMAT) + " "
                                                             + MessageContainerList.GetMessage(utilities.ExecutionContext, "for") + " " + MessageContainerList.GetMessage(utilities.ExecutionContext, "waiver set")
                                                             + " - " + WaiverSetName;

            //this.pnlWaiver.SuspendLayout();
            this.pnlWaiverDisplay.SuspendLayout();
            //this.pnlWaiver.Controls.Clear();
            if (webBrowser != null)
            {
                try
                {
                    string pdfFileName = webBrowser.Url.LocalPath.ToString();
                    webBrowser.Dispose();
                    webBrowser = null;
                    FileInfo file = new FileInfo(pdfFileName);
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
            this.pnlWaiverDisplay.Controls.Clear();
            UIElementsDTO uIElementsDTOForWaiver = new UIElementsDTO();
            //uIElementsDTOForWaiver.waiverCustomerAndSignatureDTO = waiverCustomerAndSignatureDTO;

            string fileName = waiverIDLanguageFileName[waiverCustomerAndSignatureDTO.WaiversDTO.WaiverSetDetailId];
            string fileWithPath = GetFileNameWithPath(fileName);

            webBrowser = new WebBrowser();
            Uri urlLink = new Uri(fileWithPath + KioskStatic.PdfViewSettings);
            webBrowser.Navigate(urlLink, false);
            webBrowser.Name = waiverCustomerAndSignatureDTO.WaiversDTO.WaiverFileName.Substring(0, waiverCustomerAndSignatureDTO.WaiversDTO.WaiverFileName.LastIndexOf('.'));
            webBrowser.ScrollBarsEnabled = true;
            webBrowser.Size = new Size(pnlWaiverDisplay.Width - 5, (pnlWaiverDisplay.Height - 5));
            webBrowser.IsWebBrowserContextMenuEnabled = false;
            webBrowser.WebBrowserShortcutsEnabled = false;
            webBrowser.AllowNavigation = false;
            webBrowser.AllowWebBrowserDrop = false;
            uIElementsDTOForWaiver.fileWithPath = fileWithPath;
            this.pnlWaiverDisplay.Controls.Add(webBrowser);
            //Thread.Sleep(60);
            //Application.DoEvents();
            this.pnlWaiverDisplay.ResumeLayout(false);
            //this.chkSignConfirm.Checked = true;
            //Thread.Sleep(60);
            //Application.DoEvents();

            //Thread.Sleep(60);
            //Application.DoEvents();

            Panel pnlWaiverCustomer = new Panel();
            pnlWaiverCustomer.Size = new Size(pnlMaster.Width - 50, pnlMaster.Height - 50);
            pnlWaiverCustomer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            pnlWaiverCustomer.AutoSize = false;
            pnlWaiverCustomer.Size = new Size(1100, 170);
            FlowLayoutPanel fpnlCustomerInfo = new FlowLayoutPanel();
            fpnlCustomerInfo.AutoSize = true;
            fpnlCustomerInfo.Location = new Point(pnlWaiverCustomer.Location.X + 5, pnlWaiverCustomer.Location.Y + 5);
            fpnlCustomerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left
           | System.Windows.Forms.AnchorStyles.Right)));
            int flowPanelHeight = 0;
            foreach (CustomerContentForWaiverDTO customerInfo in waiverCustomerAndSignatureDTO.CustomerContentDTOList)
            {
                Panel pnlCustInfo = new Panel();
                pnlCustInfo.AutoSize = true;
                DisplayCustomerInfo(pnlCustInfo, customerInfo);
                DisplayAttributes(pnlCustInfo, customerInfo);
                fpnlCustomerInfo.Controls.Add(pnlCustInfo);
                flowPanelHeight = flowPanelHeight + pnlCustInfo.Height;
            }
            fpnlCustomerInfo.Width = this.Width - 50;
            fpnlCustomerInfo.Height = flowPanelHeight + 2;
            pnlWaiverCustomer.Controls.Add(fpnlCustomerInfo);
            this.pnlWaiver.Controls.Add(fpnlCustomerInfo);
            //uIElementsDTOForWaiver.customerDisplayPanel = pnlWaiverCustomer;
            //foreach (Control item in pnlWaiverCustomer.Controls)
            //{
            //    this.pnlWaiver.Controls.Add(item);
            //}
            pnlWaiver.AutoSize = true;
            pnlWaiver.AutoSize = false;
            pnlWaiver.AutoScroll = true;
            //pnlWaiver.Size = new System.Drawing.Size(874, 338);
            //pnlWaiver.ResumeLayout(false);
            AdjustCustomerInfoPanelHeight(waiverCustomerAndSignatureDTO);
            bigVerticalScrollWaiver.UpdateButtonStatus();
            uiElementsDTOList.Add(uIElementsDTOForWaiver);
            Application.DoEvents();
            ResetKioskTimer();
            log.LogMethodExit(uIElementsDTOForWaiver);
        }

        private void AdjustCustomerInfoPanelHeight(WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO)
        {
            log.LogMethodEntry(waiverCustomerAndSignatureDTO);
            if (waiverCustomerAndSignatureDTO.CustomerContentDTOList.Count() == 1)
            {
                pnlWaiver.Size = new Size(1174, 130);
                pnlMaster.Size = new Size(1300, 130);
                bigVerticalScrollWaiver.Visible = false;
                pnlWaiverDisplay.Location = new Point(310, 242);
                pnlWaiverDisplay.Size = new Size(1300, 510);
                pnlWaiverDisplay.Controls[0].Size = new Size(pnlWaiverDisplay.Width - 5, (pnlWaiverDisplay.Height - 5)); ;
            }
            log.LogMethodExit();
        }

        private void DisplayCustomerInfo(Panel pnlCustInfo, CustomerContentForWaiverDTO customerInfo)
        {
            log.LogMethodEntry(customerInfo);
            int rowPositionX = 5;
            int labelPostionPadding = 10;

            Label lblCustomerName = new Label();
            lblCustomerName.Text = customerInfo.CustomerName;
            lblCustomerName.TextAlign = ContentAlignment.MiddleLeft;
            lblCustomerName.MinimumSize = new Size(550, 50);
            lblCustomerName.Width = 550;
            lblCustomerName.Height = 50;
            lblCustomerName.Location = new Point(rowPositionX, labelPostionPadding + 2);
            lblCustomerName.BorderStyle = BorderStyle.FixedSingle;

            Label lblCustomerEmail = new Label();
            lblCustomerEmail.Text = customerInfo.EmailId;
            lblCustomerEmail.MinimumSize = new Size(550, 50);
            lblCustomerEmail.Width = 550;
            lblCustomerEmail.Height = 50;
            lblCustomerEmail.TextAlign = ContentAlignment.MiddleLeft;
            lblCustomerEmail.Location = new Point(lblCustomerName.Location.X + lblCustomerName.Width + labelPostionPadding, lblCustomerName.Location.Y);
            lblCustomerEmail.BorderStyle = BorderStyle.FixedSingle;
            lblCustomerName.Font = lblCustomerEmail.Font = lblSample.Font;

            pnlCustInfo.Controls.Add(lblCustomerName);
            pnlCustInfo.Controls.Add(lblCustomerEmail);
            log.LogMethodExit();
        }


        private void DisplayAttributes(Panel pnlCustInfo, CustomerContentForWaiverDTO customerInfo)
        {
            log.LogMethodEntry(customerInfo);
            int padding = 15;
            int height = 50;
            int panelheight = 0;
            panelheight = 50;
            if (customerInfo.WaiverCustomAttributeList != null && customerInfo.WaiverCustomAttributeList.Any())
            {
                Panel pnlWaiverAttributes = new Panel();
                pnlWaiverAttributes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                pnlWaiverAttributes.Location = new Point(pnlCustInfo.Location.X + 10, padding + panelheight);
                pnlWaiverAttributes.AutoSize = true;
                Label lblcustAttribute1 = new Label();
                lblcustAttribute1.Text = customerInfo.Attribute1Name;
                lblcustAttribute1.Width = 400;
                lblcustAttribute1.Height = 50;
                lblcustAttribute1.TextAlign = ContentAlignment.MiddleLeft;
                lblcustAttribute1.Location = new Point(pnlWaiverAttributes.Location.X + 5, 2);


                Label lblcustAttribute2 = new Label();
                lblcustAttribute2.Text = customerInfo.Attribute2Name;
                lblcustAttribute2.Width = 380;
                lblcustAttribute2.Height = 50;
                lblcustAttribute2.TextAlign = ContentAlignment.MiddleLeft;
                lblcustAttribute2.Location = new Point(lblcustAttribute1.Location.X + lblcustAttribute1.Width + padding, lblcustAttribute1.Location.Y);

                lblcustAttribute1.Font = lblcustAttribute2.Font = new Font(lblCustomerContact.Font.FontFamily, 18F);
                lblcustAttribute1.ForeColor = lblcustAttribute2.ForeColor = this.lblCustomerContact.ForeColor;

                pnlWaiverAttributes.Controls.Add(lblcustAttribute1);
                pnlWaiverAttributes.Controls.Add(lblcustAttribute2);
                panelheight = 0;
                foreach (KeyValuePair<string, string> item in customerInfo.WaiverCustomAttributeList)
                {
                    panelheight = panelheight + height;
                    Label lblcustAttribute1Value = new Label();
                    lblcustAttribute1Value.Text = item.Key;
                    lblcustAttribute1Value.Width = 400;
                    lblcustAttribute1Value.Height = 40;
                    lblcustAttribute1Value.TextAlign = ContentAlignment.MiddleLeft;
                    lblcustAttribute1Value.Location = new Point(lblcustAttribute1.Location.X, panelheight);

                    Label lblcustAttribute2Value = new Label();
                    lblcustAttribute2Value.Text = item.Value;
                    lblcustAttribute2Value.Width = 380;
                    lblcustAttribute2Value.Height = 40;
                    lblcustAttribute2Value.TextAlign = ContentAlignment.MiddleLeft;
                    lblcustAttribute2Value.Location = new Point(lblcustAttribute2.Location.X, panelheight);

                    lblcustAttribute1Value.Font = lblcustAttribute2Value.Font = lblSample.Font;

                    pnlWaiverAttributes.Controls.Add(lblcustAttribute1Value);
                    pnlWaiverAttributes.Controls.Add(lblcustAttribute2Value);
                }
                pnlCustInfo.Controls.Add(pnlWaiverAttributes);
                pnlWaiverAttributes.Width = pnlCustInfo.Width;

            }
            log.LogMethodExit();
        }

        private string GetFileNameWithPath(string fileName)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            string fileNameWithPath = string.Empty;
            GenericUtils genericUtils = new GenericUtils();
            fileNameWithPath = genericUtils.DownloadFileFromDB(fileName, utilities);
            log.LogMethodExit(fileNameWithPath);
            return fileNameWithPath;
        }
        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            KioskStatic.logToFile("user clicked proceed button");
            ResetKioskTimer();
            try
            {
                if (chkSignConfirm.Visible && chkSignConfirm.Checked == false)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1206));
                    //Please accept our Terms and Conditions to proceed
                }
                else
                {
                    CaptureSignatureImage(fileIndex);
                    if (fileIndex == waiverCustomerAndSignatureDTOList.Count - 1)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        fileIndex += 1;
                        if (fileIndex < waiverCustomerAndSignatureDTOList.Count)
                        {
                            BuildWaiverFileInfo(fileIndex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile(ex.Message);
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message, true))
                {
                    frmOK.ShowDialog();
                }
            }
            ResetKioskTimer();
            log.LogMethodExit();

        }

        private void CaptureSignatureImage(int fileIndex)
        {
            log.LogMethodEntry(fileIndex);
            ResetKioskTimer();
            if (waiverCustomerAndSignatureDTOList != null && waiverCustomerAndSignatureDTOList.Any())
            {
                for (int i = 0; i < waiverCustomerAndSignatureDTOList.Count; i++)
                {
                    if (fileIndex == i)
                    {
                        if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "WHO_CAN_SIGN_FOR_MINOR")
                                == WaiverCustomerAndSignatureDTO.WhoCanSignForMinor.ADULT_AND_MINOR.ToString())
                        {
                            waiverCustomerAndSignatureDTOList[i] = GetSignatureFromAdultAndMinor(waiverCustomerAndSignatureDTOList[i]);
                        }
                        else
                        {
                            Image generatedImage = GenerateSignatureImage();
                            if (generatedImage != null)
                            {
                                string name = (string.IsNullOrWhiteSpace(waiverCustomerAndSignatureDTOList[i].SignatoryCustomerDTO.FirstName) ? "" : waiverCustomerAndSignatureDTOList[i].SignatoryCustomerDTO.FirstName)
                                                + " "+ (string.IsNullOrWhiteSpace(waiverCustomerAndSignatureDTOList[i].SignatoryCustomerDTO.LastName) ? "" : waiverCustomerAndSignatureDTOList[i].SignatoryCustomerDTO.LastName);
                                WaiveSignatureImageWithCustomerDetailsDTO custIdSignatureImage = new WaiveSignatureImageWithCustomerDetailsDTO(waiverCustomerAndSignatureDTOList[i].SignatoryCustomerDTO.Id, name, generatedImage);
                                waiverCustomerAndSignatureDTOList[i].CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                                waiverCustomerAndSignatureDTOList[i].CustIdNameSignatureImageList.Add(custIdSignatureImage);
                            }
                            else
                            {
                                throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2851));
                                // "Unexpected error while creating signature image"
                            }
                        }

                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        private WaiverCustomerAndSignatureDTO GetSignatureFromAdultAndMinor(WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO)
        {
            log.LogMethodEntry(waiverCustomerAndSignatureDTO);
            try
            {
                waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                CustomerBL customerBL = new CustomerBL(utilities.ExecutionContext, waiverCustomerAndSignatureDTO.SignatoryCustomerDTO);
                bool customerIsAdult = customerBL.IsAdult();
                string customerName = (string.IsNullOrEmpty(waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.FirstName) ? "" : waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.FirstName)
                                  + " " + (string.IsNullOrEmpty(waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.LastName) ? "" : waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.LastName);
                Image generatedImage = CollectCustomerSignature(customerName, customerIsAdult);

                string name = (string.IsNullOrWhiteSpace(waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.FirstName) ? "" : waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.FirstName)
                                               + " " + (string.IsNullOrWhiteSpace(waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.LastName) ? "" : waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.LastName);

                WaiveSignatureImageWithCustomerDetailsDTO custIdSignatureImageKey = new WaiveSignatureImageWithCustomerDetailsDTO(waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.Id, name, generatedImage);
                waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList.Add(custIdSignatureImageKey);
                for (int i = 0; i < waiverCustomerAndSignatureDTO.CustomerContentDTOList.Count; i++)
                {
                    if (waiverCustomerAndSignatureDTO.CustomerContentDTOList[i].CustomerId == waiverCustomerAndSignatureDTO.SignatoryCustomerDTO.Id)
                    {
                        continue;//Already received signatory customer signature
                    }
                    generatedImage = CollectCustomerSignature(waiverCustomerAndSignatureDTO.CustomerContentDTOList[i].CustomerName, false);
                    WaiveSignatureImageWithCustomerDetailsDTO tempCustIdSignatureImageKey = new WaiveSignatureImageWithCustomerDetailsDTO(waiverCustomerAndSignatureDTO.CustomerContentDTOList[i].CustomerId, waiverCustomerAndSignatureDTO.CustomerContentDTOList[i].CustomerName, generatedImage);
                    waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList.Add(tempCustIdSignatureImageKey);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                waiverCustomerAndSignatureDTO.CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                throw;
            }

            log.LogMethodExit();
            return waiverCustomerAndSignatureDTO;
        }


        private Image GenerateSignatureImage()
        {
            log.LogMethodEntry();
            Image generatedImage = null;
            using (Image signatureImage = Properties.Resources.IAgreeToTheTerms)
            {
                // Create graphics from image 
                using (Graphics graphics = Graphics.FromImage(signatureImage))
                {
                    //graphics.SmoothingMode = SmoothingMode.AntiAlias; 
                    // Create text position
                    PointF textStartPoint = new PointF(105, 25);
                    // Create font
                    Font textFont = new Font("Microsoft Sans Serif", 12.0f, FontStyle.Regular);
                    BaseFont baseFont = GetFont();
                    if (baseFont != null)
                    {
                        iTextSharp.text.Font finalFont = new iTextSharp.text.Font(baseFont);
                        textFont = new Font(finalFont.Familyname, 12.0f, FontStyle.Regular);
                    }
                    // Draw text
                    graphics.DrawString(MessageContainerList.GetMessage(utilities.ExecutionContext, 2433), textFont, Brushes.Black, textStartPoint);

                    generatedImage = (Image)signatureImage.Clone();
                }
            }
            log.LogMethodExit();
            return generatedImage;
        }

        private Image CollectCustomerSignature(string customerName, bool guardian)
        {
            log.LogMethodEntry(customerName, guardian);
            Image customerSignatureConfirmation = null;
            using (Waiver.frmCustomerSignatureConfirmation signatureConfirmation = new Waiver.frmCustomerSignatureConfirmation(utilities.ExecutionContext, customerName, guardian))
            {
                if (signatureConfirmation.ShowDialog()== DialogResult.OK)
                {
                    customerSignatureConfirmation = GenerateSignatureImage();
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 1206));
                    //Please accept our Terms and Conditions to proceed
                }
            } 
            log.LogMethodExit();
            return customerSignatureConfirmation;
        }
        private BaseFont GetFont()
        {
            log.LogMethodEntry();
            string fontFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            BaseFont baseFont = baseFont = BaseFont.CreateFont(fontFolderPath + "\\micross.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            try
            {
                Languages languages = new Languages(utilities.ExecutionContext, utilities.ParafaitEnv.LanguageId);
                if (languages.GetLanguagesDTO != null)
                {
                    if (languages.GetLanguagesDTO.LanguageName.Equals("Chinese (PRC)"))
                    {
                        baseFont = BaseFont.CreateFont(fontFolderPath + "\\simhei.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                DisplayMessageLine(ex.Message);
                KioskStatic.logToFile(ex.Message);
                using (frmOKMsg frmOK = new frmOKMsg(ex.Message, true))
                {
                    frmOK.ShowDialog();
                }
            }
            log.LogMethodExit(baseFont);
            return baseFont;
        } 
        private void DownButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void UpButtonClick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            log.LogMethodExit();
        } 

        private void frmSelectWaiverOption_Closing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateSignedFileName();
            DeleteFilesFromTabPages();
            KioskStatic.logToFile("Closing select waiver Files form");
            log.LogMethodExit();
        }

        private void UpdateSignedFileName()
        {
            log.LogMethodEntry();
            foreach (WaiverCustomerAndSignatureDTO item in waiverCustomerAndSignatureDTOList)
            {
                if (item.WaiversDTO.WaiverFileName != waiverIDLanguageFileName[item.WaiversDTO.WaiverSetDetailId])
                {
                    item.WaiversDTO.WaiverFileName = waiverIDLanguageFileName[item.WaiversDTO.WaiverSetDetailId];
                }
            }
            log.LogMethodExit();
        }

        private void DeleteFilesFromTabPages()
        {
            log.LogMethodEntry();
            try
            {
                webBrowser.Dispose();
                webBrowser = null;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            if (uiElementsDTOList != null)
            {
                foreach (UIElementsDTO pageItem in uiElementsDTOList)
                {
                    if (string.IsNullOrEmpty(pageItem.fileWithPath) == false)
                    {
                        try
                        {
                            FileInfo file = new FileInfo(pageItem.fileWithPath);
                            if (file.Exists)
                            {
                                //pageItem.waiverBrowser.Dispose();
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
            log.LogMethodExit();
        }
        private void chkSignConfirm_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (chkSignConfirm.Checked)
            {
                pbCheckBox.Image = Properties.Resources.tick_box_checked;
            }
            else
            {
                pbCheckBox.Image = Properties.Resources.tick_box_unchecked;
            }
            log.LogMethodExit();
        }
        private void pbCheckBox_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            chkSignConfirm.Checked = !chkSignConfirm.Checked;
            log.LogMethodExit();
        }
        public override void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (waiverCustomerAndSignatureDTOList != null)
            {
                //This action will clear current customer session. Do you want to proceed?
                using (frmYesNo frmyn = new frmYesNo(MessageContainerList.GetMessage(utilities.ExecutionContext, 2459)))//"This action will clear current customer session. Do you want to proceed?")))
                {
                    if (frmyn.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {
                        base.btnHome_Click(sender, e);
                    }
                }
            }
            else
            {
                base.btnHome_Click(sender, e);
            }
            log.LogMethodExit();
        }
        private void ShowHideSignConfirmCheckBox()
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "WHO_CAN_SIGN_FOR_MINOR")
                                == WaiverCustomerAndSignatureDTO.WhoCanSignForMinor.ADULT_AND_MINOR.ToString())
            {
                chkSignConfirm.Visible = false;
                pbCheckBox.Visible = false;
                defaultMsg = MessageContainerList.GetMessage(utilities.ExecutionContext, 2852);
                // "Please read the document. Click OK to confirm"
                DisplayMessageLine(defaultMsg);
            }
            log.LogMethodExit();
        }
        private void SetCustomizedFontColors()
        {
            log.LogMethodEntry();
            //KioskStatic.logToFile("Setting customized font colors for the UI elements in frmSignWaiverFiles");
            try
            {
                this.btnHome.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesBtnHomeTextForeColor;
                this.lblSignWaiverFile.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesLblSignWaiverFileTextForeColor;
                this.lblCustomerName.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesLblCustomerNameTextForeColor;
                this.lblCustomerContact.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesLblCustomerContactTextForeColor;
                this.pnlWaiver.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesPnlWaiverTextForeColor;
                this.pnlWaiverDisplay.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesPnlWaiverDisplayTextForeColor;
                this.chkSignConfirm.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesChkSignConfirmTextForeColor;
                this.btnOkay.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesBtnOkayTextForeColor;
                this.btnCancel.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesBtnCancelTextForeColor;
                this.txtMessage.ForeColor = KioskStatic.CurrentTheme.FrmSignWaiverFilesTxtMessageTextForeColor;
                this.bigVerticalScrollWaiver.InitializeScrollBar(ThemeManager.CurrentThemeImages.ScrollDownEnabled, ThemeManager.CurrentThemeImages.ScrollDownDisabled, ThemeManager.CurrentThemeImages.ScrollUpEnabled, ThemeManager.CurrentThemeImages.ScrollUpDisabled);
                this.BackgroundImage = ThemeManager.GetBackgroundImage(ThemeManager.CurrentThemeImages.SignWaiverFilesBackgroundImage);
                btnHome.BackgroundImage = ThemeManager.CurrentThemeImages.HomeButton;
                btnOkay.BackgroundImage =
                    btnCancel.BackgroundImage = ThemeManager.CurrentThemeImages.BackButtonImage;
                pnlMaster.BackgroundImage = ThemeManager.CurrentThemeImages.KioskActivityTableImage;
            }
            catch (Exception ex)
            {
                log.Error("Errow while Setting Customized Fore Colors", ex);
                KioskStatic.logToFile("Error while setting customized font colors for the UI elements in frmSignWaiverFiles: " + ex.Message);
            }
            log.LogMethodExit();
        }

    }
}

