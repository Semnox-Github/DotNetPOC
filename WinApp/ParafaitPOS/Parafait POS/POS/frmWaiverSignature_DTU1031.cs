/********************************************************************************************
 * Project Name - Parafait_POS
 * Description  - WaiverSignature_DTH1031 form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 ********************************************************************************************* 
 *2.70.2      11-Oct-2019      Guru S A       Waiver phase 2 enhancement
 *2.100       19-Oct-2020      Guru S A       Enabling minor signature option for waiver
 ********************************************************************************************/
using Florentis;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Waivers;
using Semnox.Parafait.Languages;
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
    public partial class frmWaiverSignature_DTU1031 : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public Image ImageFile
        //{
        //    get
        //    {
        //        return sigImage;
        //    }
        //}

        public bool isSigWindowOpen;
        public bool IsSigWindowOpen
        {
            get
            {
                return isSigWindowOpen;
            }
        }

        // public Dictionary<int, Image> imageFileList = new Dictionary<int, Image>();

        public EventHandler raiseEvent;
        private Utilities utilities;
        private List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOList;

        // List<WaiversDTO> DeviceWaiverSetDetailDTOList;
        bool initialLoad = false;
        private int signWindowCount = 1;
        private int defaultLanguage = -1;
        //private TabPage tabPage;
        //private List<TabPage> tabPageList = new List<TabPage>();
        //List<AxAcroPDF> axAcroPDFList = new List<AxAcroPDF>();
        private string folderPath = "";
        //CustomerDTO customerDTO;
        private List<LanguagesDTO> languagesDTOList = new List<LanguagesDTO>();
        //bool gridVisible = false; 
        public EventArgs raisevent = new EventArgs();
        // DataGridView grid;
        private List<LookupValuesDTO> custAttributesInWaiverLookUpValueDTOList;
        //private Image sigImage;
        private Dictionary<int, string> waiverIDLanguageFileName;
        private string wacomLicenseKey;

        public List<WaiverCustomerAndSignatureDTO> WaiverCustomerAndSignatureDTOList { get { return waiverCustomerAndSignatureDTOList; } }
        public frmWaiverSignature_DTU1031(List<WaiverCustomerAndSignatureDTO> waiverCustomerAndSignatureDTOList, Utilities utilities)
        {
            log.LogMethodEntry(waiverCustomerAndSignatureDTOList);
            try
            {
                InitializeComponent();
                this.utilities = utilities;
                this.waiverCustomerAndSignatureDTOList = waiverCustomerAndSignatureDTOList;
                isSigWindowOpen = false;
                initialLoad = true;
                //cntrl.Size = panel2.Size;
                //modification added: on 30-Sep-2017 to change btn text
                //this.DeviceWaiverSetDetailDTOList = DeviceWaiverSetDetailDTOList;
                //this.customerDTO = customerDTO;
                //end modification added: on 30-Sep-2017 to change btn text
                //grid = (DataGridView)cntrl;

                //grid.Columns["Product_Type"].Visible = false;

                //grid.BackgroundColor = Color.WhiteSmoke;
                //grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
                //grid.ScrollBars = ScrollBars.Vertical;

                //if (grid != null && grid.Rows.Count > 0)
                //{
                //    grid.Rows[0].Visible = false;
                //    grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                //    grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                //    grid.Rows[grid.Rows.Count - 1].Visible = false;
                //}

                //foreach (DataGridViewRow c in grid.Rows)
                //{
                //    c.Height = 50;
                //    foreach (DataGridViewCell cell in c.Cells)
                //    {
                //        cell.Style.Font = new Font(grid.DefaultCellStyle.Font.FontFamily, 10);
                //    }
                //}
                //foreach (DataGridViewColumn column in grid.Columns)
                //{
                //    column.HeaderCell.Style.Font = new Font(grid.Font.FontFamily, 10);
                //}
                //grid.ColumnHeadersHeight = 40;
                //grid.Width = panel2.Width - verticalScrollBarView2.Width - 10;

                //panel2.Controls.Add(grid);

                //grid.Columns["Line_Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //grid.Columns["Tax"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //grid.Columns["Price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //verticalScrollBarView2.DataGridView = grid;
                //panel2.Controls.Add(verticalScrollBarView2);

                //if (customerDTO != null)
                //{
                //    txtCusName.Text = customerDTO.FirstName;
                //}

                //if (customerDTO != null)
                //{
                //    txtPhone.Text = customerDTO.PhoneNumber.ToString();
                //}

                this.utilities.setLanguage(this);
                LoadIdAndFileNameList();
                LoadLanguages();

                btnOK.Text = this.utilities.MessageUtils.getMessage(1247);
                GetCustomAttributesInWaiverLookupValues();
                wacomLicenseKey = Encryption.GetParafaitKeys("WacomKey");
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new Exception(ex.Message);
            }
            finally
            {
                initialLoad = false;
            }
            log.LogMethodExit();
        }

        private void GetCustomAttributesInWaiverLookupValues()
        {
            log.LogMethodEntry();
            custAttributesInWaiverLookUpValueDTOList = WaiverCustomerUtils.GetWaiverCustomerAttributeLookup(utilities.ExecutionContext);
            log.LogMethodExit(custAttributesInWaiverLookUpValueDTOList);
        }

        private void LoadLanguages()
        {
            log.LogMethodEntry();
            initialLoad = true;
            defaultLanguage = ParafaitDefaultContainerList.GetParafaitDefault<int>(utilities.ExecutionContext, "DEFAULT_LANGUAGE", -1);
            GetLanguageDTOList();


            BindingSource bs = new BindingSource();
            bs.DataSource = languagesDTOList;
            ddlLanguages.DataSource = bs;
            ddlLanguages.ValueMember = "LanguageId";
            ddlLanguages.DisplayMember = "LanguageName";

            if (defaultLanguage != -1)
            {
                ddlLanguages.SelectedValue = defaultLanguage;
            }
            initialLoad = false;
            log.LogMethodExit();
        }

        private void GetLanguageDTOList()
        {
            log.LogMethodEntry();
            Languages languages = new Languages(utilities.ExecutionContext);
            List<KeyValuePair<LanguagesDTO.SearchByParameters, string>> languageSerachParam = new List<KeyValuePair<LanguagesDTO.SearchByParameters, string>>();
            languageSerachParam.Add(new KeyValuePair<LanguagesDTO.SearchByParameters, string>(LanguagesDTO.SearchByParameters.IS_ACTIVE, "1"));
            languagesDTOList = languages.GetAllLanguagesList(languageSerachParam);
            if (languagesDTOList == null)
            {
                languagesDTOList = new List<LanguagesDTO>();
            }
            languagesDTOList.Insert(0, new LanguagesDTO());
            languagesDTOList[0].LanguageId = -1;
            languagesDTOList[0].LanguageName = "DEFAULT";
            log.LogMethodExit();
        }

        private void LoadIdAndFileNameList()
        {
            log.LogMethodEntry();
            waiverIDLanguageFileName = new Dictionary<int, string>();
            foreach (WaiverCustomerAndSignatureDTO item in waiverCustomerAndSignatureDTOList)
            {
                // waiverIDLanguageFileName.Add(new KeyValuePair<int, string>(item.WaiversDTO.WaiverSetDetailId, item.WaiversDTO.WaiverFileName)); 
                waiverIDLanguageFileName.Add(item.WaiversDTO.WaiverSetDetailId, item.WaiversDTO.WaiverFileName);
            }
            log.LogMethodExit();
        }


        void LoadControlsText()
        {
            log.LogMethodEntry();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            lblDate.Text = utilities.MessageUtils.getMessage("Date") + " : " + DateTime.Now.ToString();
            folderPath = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "IMAGE_DIRECTORY");
            SetLogoImage();
            LoadWaiverTabPages();
            log.LogMethodExit();
        }

        private void LoadWaiverTabPages()
        {
            log.LogMethodEntry();
            tabPage1.Hide();
            if (waiverCustomerAndSignatureDTOList != null && waiverCustomerAndSignatureDTOList.Any())
            {
                foreach (WaiverCustomerAndSignatureDTO item in waiverCustomerAndSignatureDTOList)
                {
                    TabPage newTabPage = CreateTabPage(item);
                    tbCtrlWaiver.Controls.Add(newTabPage);
                }
            }
            tbCtrlWaiver.Controls.Remove(tabPage1);
            log.LogMethodExit();
        }

        private TabPage CreateTabPage(WaiverCustomerAndSignatureDTO waiverCustomerAndSignatureDTO)
        {
            log.LogMethodEntry();
            TabPage newTabPage = new TabPage();
            newTabPage.Size = new Size(tbCtrlWaiver.Width - 2, tbCtrlWaiver.Height - 5);
            newTabPage.Tag = waiverCustomerAndSignatureDTO;
            newTabPage.Text = waiverCustomerAndSignatureDTO.WaiversDTO.Name;
            //waiverCustomerAndSignatureDTO = WaiverCustomerUtils.CreateCustomerContentForWaiver(utilities.ExecutionContext, waiverCustomerAndSignatureDTO);
            Panel pnlWaiver = new Panel();
            pnlWaiver.Location = new Point(newTabPage.Bounds.X + 5, newTabPage.Bounds.Y + 5);
            pnlWaiver.Size = new Size(newTabPage.Width - 5, newTabPage.Height - 5);
            pnlWaiver.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            DisplayLogo(pnlWaiver);
            int coordinateY = 5;
            FlowLayoutPanel fpnlCustomerInfo = new FlowLayoutPanel();
            fpnlCustomerInfo.AutoSize = true;
            fpnlCustomerInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            fpnlCustomerInfo.Location = new Point(newTabPage.Bounds.X + 5, newTabPage.Bounds.Y + 5);
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
            pnlWaiver.Controls.Add(fpnlCustomerInfo);
            if (fpnlCustomerInfo.Height > 201)
            {
                fpnlCustomerInfo.AutoSize = false;
                fpnlCustomerInfo.Height = 200;
                fpnlCustomerInfo.AutoScroll = true;
                //VerticalScrollBarView vScrollBarCustInfo = new VerticalScrollBarView();
                //vScrollBarCustInfo.ScrollableControl = pnlCustomerInfo;
                //vScrollBarCustInfo.Location = new Point(pnlCustomerInfo.Location.X + pnlCustomerInfo.Width - 5, pnlCustomerInfo.Location.Y);
                //vScrollBarCustInfo.Height = pnlCustomerInfo.Height;
                //pnlWaiver.Controls.Add(vScrollBarCustInfo);
            }
            // Panel pnlWaiverDoc = new Panel();
            //pnlWaiverDoc.Location = new Point(newTabPage.Bounds.X + 5, pnlCustomerInfo.Height+ 5);
            // pnlWaiverDoc.Size = new Size(newTabPage.Width - 30, newTabPage.Height - pnlCustomerInfo.Height- 5);
            // pnlWaiverDoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            string fileName = waiverIDLanguageFileName[waiverCustomerAndSignatureDTO.WaiversDTO.WaiverSetDetailId];
            string fileWithPath = GetFileNameWithPath(fileName);
            WebBrowser webBrowser = new WebBrowser();
            Uri urlLink = new Uri(fileWithPath);
            webBrowser.Navigate(urlLink, false);
            // webBrowser.Location = new Point(pnlWaiverDoc.Location.X, 5); 
            webBrowser.Location = new Point(fpnlCustomerInfo.Location.X, fpnlCustomerInfo.Height + 2);
            webBrowser.Name = waiverCustomerAndSignatureDTO.WaiversDTO.WaiverFileName.Substring(0, waiverCustomerAndSignatureDTO.WaiversDTO.WaiverFileName.LastIndexOf('.'));
            webBrowser.ScrollBarsEnabled = true;
            //webBrowser.ScrollBarsEnabled = false; 
            //webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            webBrowser.Size = new Size(pnlWaiver.Width - 20, (pnlWaiver.Height - fpnlCustomerInfo.Height));
            //pnlWaiverDoc.Controls.Add(webBrowser);
            //pnlWaiverDoc.AutoScroll = true;
            //VerticalScrollBarView vScrollBarWaiverDoc = new VerticalScrollBarView();
            //vScrollBarWaiverDoc.ScrollableControl = pnlWaiverDoc; 
            //vScrollBarWaiverDoc.Location = new Point(pnlWaiverDoc.Width - 30, pnlWaiverDoc.Location.Y);
            //vScrollBarWaiverDoc.Height = tbCtrlWaiver.Height - pnlCustomerInfo.Height-10;
            //pnlWaiver.Controls.Add(vScrollBarWaiverDoc);
            //pnlWaiver.Controls.Add(pnlWaiverDoc);
            pnlWaiver.Controls.Add(webBrowser);
            newTabPage.Controls.Add(pnlWaiver);
            log.LogMethodExit(newTabPage);
            return newTabPage;
        }



        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            log.LogMethodEntry();
            //WebBrowser sendingBrowser = (WebBrowser)sender;
            //sendingBrowser.Size = sendingBrowser.Document.Body.ScrollRectangle.Size;
            log.LogMethodExit();
        }
        private void DisplayLogo(Panel pnlWaiver)
        {
            log.LogMethodEntry();
            //PictureBox pbxLogoImage = new PictureBox();
            //pbxLogoImage.Image = pbLogo.Image;
            //pbxLogoImage.Location = new Point(pbLogo.Location.X, pbLogo.Location.Y);
            //pbxLogoImage.Height = pbLogo.Height;
            //pbxLogoImage.Width = pbLogo.Width;
            //pnlWaiver.Controls.Add(pbxLogoImage);
            log.LogMethodExit();
        }

        private void DisplayCustomerInfo(Panel pnlWaiver, CustomerContentForWaiverDTO customerInfo)
        {
            log.LogMethodEntry(customerInfo);
            int rowPositionX = pbLogo.Location.X;
            int labelPostionPadding = 10;
            int txtBoxPositionPadding = 5;

            Label lblCustomerName = new Label();
            lblCustomerName.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Customer Name") + ": ";
            lblCustomerName.TextAlign = ContentAlignment.MiddleRight;
            lblCustomerName.Width = 220;
            lblCustomerName.Location = new Point(rowPositionX, labelPostionPadding + 2);

            TextBox txtCustomerName = new TextBox();
            txtCustomerName.Text = customerInfo.CustomerName;
            txtCustomerName.ReadOnly = true;
            txtCustomerName.Width = 200;
            txtCustomerName.Location = new Point(lblCustomerName.Location.X + lblCustomerName.Width + txtBoxPositionPadding, lblCustomerName.Location.Y);

            Label lblCustomerPh = new Label();
            lblCustomerPh.Text = MessageContainerList.GetMessage(utilities.ExecutionContext, "Phone Number") + ": ";
            lblCustomerPh.Width = 200;
            lblCustomerPh.TextAlign = ContentAlignment.MiddleRight;
            lblCustomerPh.Location = new Point(txtCustomerName.Location.X + txtCustomerName.Width + labelPostionPadding, lblCustomerName.Location.Y);

            TextBox txtCustomerPh = new TextBox();
            txtCustomerPh.Text = customerInfo.PhoneNumber;
            txtCustomerPh.ReadOnly = true;
            txtCustomerPh.Width = 220;
            txtCustomerPh.Location = new Point(lblCustomerPh.Location.X + lblCustomerPh.Width + txtBoxPositionPadding, lblCustomerName.Location.Y);

            lblCustomerName.Font = txtCustomerName.Font = lblCustomerPh.Font = txtCustomerPh.Font = lblSample.Font;

            pnlWaiver.Controls.Add(lblCustomerName);
            pnlWaiver.Controls.Add(txtCustomerName);
            pnlWaiver.Controls.Add(lblCustomerPh);
            pnlWaiver.Controls.Add(txtCustomerPh);
            log.LogMethodExit();
        }

        private void DisplayAttributes(Panel pnlWaiver, CustomerContentForWaiverDTO customerInfo)
        {
            log.LogMethodEntry(customerInfo);
            int padding = 8;
            int height = 40;
            int panelheight = 0;
            // int txtBoxPositionPadding = 5;
            panelheight = height;
            if (customerInfo.WaiverCustomAttributeList != null && customerInfo.WaiverCustomAttributeList.Any())
            {
                Panel pnlWaiverAttributes = new Panel();
                pnlWaiverAttributes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                pnlWaiverAttributes.Location = new Point(pnlWaiver.Location.X + 2, padding + panelheight);
                pnlWaiverAttributes.AutoSize = true;
                Label lblcustAttribute1 = new Label();
                lblcustAttribute1.Text = customerInfo.Attribute1Name;
                lblcustAttribute1.Width = 300;
                lblcustAttribute1.Height = 35;
                lblcustAttribute1.TextAlign = ContentAlignment.MiddleLeft;
                lblcustAttribute1.Location = new Point(pnlWaiverAttributes.Location.X + 5, 2);

                Label lblcustAttribute2 = new Label();
                lblcustAttribute2.Text = customerInfo.Attribute2Name;
                lblcustAttribute2.Width = 300;
                lblcustAttribute2.Height = 35;
                lblcustAttribute2.TextAlign = ContentAlignment.MiddleLeft;
                lblcustAttribute2.Location = new Point(lblcustAttribute1.Location.X + lblcustAttribute1.Width + padding, lblcustAttribute1.Location.Y);

                lblcustAttribute1.Font = lblcustAttribute2.Font = lblSample.Font;

                pnlWaiverAttributes.Controls.Add(lblcustAttribute1);
                pnlWaiverAttributes.Controls.Add(lblcustAttribute2);
                panelheight = 0;
                foreach (KeyValuePair<string, string> item in customerInfo.WaiverCustomAttributeList)
                {
                    panelheight = panelheight + height;
                    Label lblcustAttribute1Value = new Label();
                    lblcustAttribute1Value.Text = item.Key;
                    lblcustAttribute1Value.Width = 300;
                    lblcustAttribute1Value.Height = 35;
                    lblcustAttribute1Value.TextAlign = ContentAlignment.MiddleLeft;
                    lblcustAttribute1Value.Location = new Point(lblcustAttribute1.Location.X, panelheight);

                    Label lblcustAttribute2Value = new Label();
                    lblcustAttribute2Value.Text = item.Value;
                    lblcustAttribute2Value.Width = 300;
                    lblcustAttribute2Value.Height = 35;
                    lblcustAttribute2Value.TextAlign = ContentAlignment.MiddleLeft;
                    lblcustAttribute2Value.Location = new Point(lblcustAttribute2.Location.X, panelheight);

                    pnlWaiverAttributes.Controls.Add(lblcustAttribute1Value);
                    pnlWaiverAttributes.Controls.Add(lblcustAttribute2Value);
                }
                pnlWaiver.Controls.Add(pnlWaiverAttributes);
                pnlWaiverAttributes.Width = this.Width - 85;

            }
            log.LogMethodExit();
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

        private void SetLogoImage()
        {
            log.LogMethodEntry();
            bool found = false;
            Image image = null;
            GenericUtils genericUtils = new GenericUtils();
            try
            {
                byte[] bytes = genericUtils.GetFileFromServer(utilities, ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "IMAGE_DIRECTORY") + "\\DisplayDeviceLogo.png");
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = System.Drawing.Image.FromStream(ms, false, true);
                    if (image != null)
                        found = true;
                }

                if (!found)
                {
                    image = Properties.Resources.DisplayDeviceLogo;
                }
            }
            catch
            {
                image = Properties.Resources.DisplayDeviceLogo;
            }

            pbLogo.Image = image;
            // pbLogo.Visible = false;
            log.LogMethodExit();
        }

        private void frmWaiverSignature_DTU1031_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LanguageDisplaySettings();
            RefreshWaiverIdAndFileName(defaultLanguage);
            LoadControlsText();
            log.LogMethodExit();
        }

        private void LanguageDisplaySettings()
        {
            log.LogMethodEntry();
            if (languagesDTOList != null && languagesDTOList.Count == 2)
            {
                btnLanguage.Visible = true;
                if (Convert.ToInt32(defaultLanguage) != -1)
                {
                    btnLanguage.Tag = Convert.ToInt32(defaultLanguage);
                    btnLanguage.Text = "English";
                }
                else
                {
                    btnLanguage.Tag = languagesDTOList[1].LanguageId;
                    btnLanguage.Text = languagesDTOList[1].LanguageName;
                }
            }
            else
            {
                ddlLanguages.Visible = true;
                lblLanguage.Visible = true;
            }
            log.LogMethodExit();
        }

        private void pbSignature_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                log.LogVariableState("signWindowCount", signWindowCount);
                log.LogVariableState("waiverCustomerAndSignatureDTOList.Count", waiverCustomerAndSignatureDTOList.Count);
                if (signWindowCount <= waiverCustomerAndSignatureDTOList.Count)
                {
                    isSigWindowOpen = true;
                    if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "WHO_CAN_SIGN_FOR_MINOR")
                                     == WaiverCustomerAndSignatureDTO.WhoCanSignForMinor.ADULT_AND_MINOR.ToString())
                    {
                        GetSignatoryAndMinorCustomerSignature();
                    }
                    else
                    {
                        GetSignatoryCustomerSignature();
                    }
                }
            }
            catch (Exception ex)
            {
                // sigImage = null;
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                log.Error(ex);
                throw;
            }
            isSigWindowOpen = false;
            log.LogMethodExit();
        }

        private void GetSignatoryAndMinorCustomerSignature()
        {
            log.LogMethodEntry();
            try
            {
                CustomerDTO sigCustomerDTO = waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].SignatoryCustomerDTO;
                string custName = (string.IsNullOrEmpty(sigCustomerDTO.FirstName) ? "" : sigCustomerDTO.FirstName)
                                  + " " + (string.IsNullOrEmpty(sigCustomerDTO.LastName) ? "" : sigCustomerDTO.LastName);


                Image sigImage = GetSignatureImage(custName);
                WaiveSignatureImageWithCustomerDetailsDTO custIdNameSign = GetCustIdNameSignature(sigCustomerDTO, sigImage);
                waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].CustIdNameSignatureImageList.Add(custIdNameSign);

                for (int i = 0; i < waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].SignForCustomerDTOList.Count; i++)
                {
                    CustomerDTO customerDTO = waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].SignForCustomerDTOList[i];
                    if (customerDTO.Id == sigCustomerDTO.Id)
                    {
                        //Already received signatory customer signature
                        continue;
                    }
                    else
                    {
                        custName = (string.IsNullOrEmpty(customerDTO.FirstName) ? "" : customerDTO.FirstName)
                                  + " " + (string.IsNullOrEmpty(customerDTO.LastName) ? "" : customerDTO.LastName);
                        sigImage = GetSignatureImage(custName);
                        custIdNameSign = GetCustIdNameSignature(customerDTO, sigImage);
                        waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].CustIdNameSignatureImageList.Add(custIdNameSign);
                    }
                }

                if (signWindowCount == waiverCustomerAndSignatureDTOList.Count)
                {
                    log.Info("signWindowCount == waiverCustomerAndSignatureDTOList.Count, close the form");
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                tbCtrlWaiver.SelectedIndex = signWindowCount++;
                log.LogVariableState("signWindowCount++", signWindowCount);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                Close();
            }
            log.LogMethodExit();
        }


        private void GetSignatoryCustomerSignature()
        {
            log.LogMethodEntry();
            try
            {
                CustomerDTO customerDTO = waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].SignatoryCustomerDTO;
                string custName = (string.IsNullOrEmpty(customerDTO.FirstName) ? "" : customerDTO.FirstName)
                                  + " " + (string.IsNullOrEmpty(customerDTO.LastName) ? "" : customerDTO.LastName);

                Image sigImage = GetSignatureImage(custName);
                WaiveSignatureImageWithCustomerDetailsDTO custIdNameSignature = GetCustIdNameSignature(customerDTO, sigImage);
                waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].CustIdNameSignatureImageList.Add(custIdNameSignature);

                if (signWindowCount == waiverCustomerAndSignatureDTOList.Count)
                {
                    log.Info("signWindowCount == waiverCustomerAndSignatureDTOList.Count, close the form");
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                tbCtrlWaiver.SelectedIndex = signWindowCount++;
                log.LogVariableState("signWindowCount++", signWindowCount);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                waiverCustomerAndSignatureDTOList[tbCtrlWaiver.SelectedIndex].CustIdNameSignatureImageList = new List<WaiveSignatureImageWithCustomerDetailsDTO>();
                Close();
            }
            log.LogMethodExit();
        }



        private Image GetSignatureImage(string custName)
        {
            log.LogMethodEntry(custName);
            Image signatureImage = null;
            SigCtl sigCtl = new SigCtl();
            sigCtl.Licence = wacomLicenseKey;
            DynamicCapture dc = new DynamicCapture();
            try
            {
                sigCtl.InputWho = "  ";
                sigCtl.InputWhy = "  ";
                sigCtl.InkWidth = 2;
                sigCtl.Caption = utilities.MessageUtils.getMessage("Waiver Signature");
                sigCtl.InputWho = (string.IsNullOrWhiteSpace(custName) == false ? custName: MessageContainerList.GetMessage(utilities.ExecutionContext, "Guest"));
                var res = dc.Capture(sigCtl, null, null, null, null);
                log.LogVariableState("dc.Capture res", res);
                if (res == 0)
                {
                    if (res == DynamicCaptureResult.DynCaptOK)
                    {
                        SigObj sigObj = (SigObj)sigCtl.Signature;

                        log.LogVariableState("sigObj", sigObj);
                        log.LogVariableState("sigObj.IsCaptured", sigObj.IsCaptured);
                        if (sigObj.IsCaptured)
                        {
                            try
                            {
                                object sigdata = sigObj.SigData;
                                object imgedata = sigObj.RenderBitmap("", 200, 150, "image/png", 0.5f, 0xff0000, 0xffffff, 10.0f, 10.0f, RBFlags.RenderOutputBase64 | RBFlags.RenderColor32BPP | RBFlags.RenderEncodeData);
                                log.LogVariableState("imgedata", imgedata);
                                if (imgedata != null)
                                {
                                    signatureImage = ConvertBase64ToImage(imgedata.ToString());
                                    log.LogVariableState("signatureImage", signatureImage);
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                                throw;
                            }
                        }
                    }
                    else if (res == DynamicCaptureResult.DynCaptCancel)
                    {
                        log.Info("res == DynamicCaptureResult.DynCaptCancel, close the form");
                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "User cancelled signature process"));
                    }
                    else if (res == DynamicCaptureResult.DynCaptError)
                    {
                        log.Info("res == DynamicCaptureResult.DynCaptError, close the form");
                        this.DialogResult = System.Windows.Forms.DialogResult.None;
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "Unexpected error while capturing signature"));
                    }
                }
                else //signature not added by customer
                {
                    log.Info("else signature not added by customer, close the form");
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, "User cancelled signature process"));
                }
            }
            finally
            {
                sigCtl = null;
                dc = null;
            }
            log.LogMethodExit(signatureImage);
            return signatureImage;
        }

        private WaiveSignatureImageWithCustomerDetailsDTO GetCustIdNameSignature(CustomerDTO custDTO, Image sigImage)
        {
            log.LogMethodEntry(custDTO);
            string name = (string.IsNullOrEmpty(custDTO.FirstName) ? "" : custDTO.FirstName) + " " + (string.IsNullOrEmpty(custDTO.LastName) ? "" : custDTO.LastName);
            WaiveSignatureImageWithCustomerDetailsDTO custIdNameSignature = new WaiveSignatureImageWithCustomerDetailsDTO(custDTO.Id, name, sigImage);
            log.LogMethodExit(custIdNameSignature);
            return custIdNameSignature;

        }
        //To convert base64 to Image
        public Image ConvertBase64ToImage(string base64)
        {
            log.Debug("Start-ConvertBase64ToImage()");
            Image image;
            byte[] bytes = Convert.FromBase64String(base64);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = System.Drawing.Image.FromStream(ms, false, true);
            }
            log.Debug("End-ConvertBase64ToImage()");
            return image;
        }


        private void ddlLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            List<WaiversDTO> LangDeviceWaiverSetDetailDTOList = new List<WaiversDTO>();
            if (!initialLoad)
            {
                if (ddlLanguages.SelectedIndex > -1)
                {
                    LoadWaiverDocument(Convert.ToInt32(ddlLanguages.SelectedValue));
                }
            }
            log.LogMethodExit(null);
        }

        private void LoadWaiverDocument(int languageId)
        {
            log.LogMethodEntry(languageId);
            RefreshWaiverIdAndFileName(languageId);
            RefreshTabDocument();
            log.LogMethodExit();
        }

        private void RefreshTabDocument()
        {
            log.LogMethodEntry();
            foreach (TabPage tb in tbCtrlWaiver.TabPages)
            {
                WaiverCustomerAndSignatureDTO tagDTO = (WaiverCustomerAndSignatureDTO)tb.Tag;
                string fileName = waiverIDLanguageFileName[tagDTO.WaiversDTO.WaiverSetDetailId];
                string fileWithPath = GetFileNameWithPath(fileName);
                Uri urlLink = new Uri(fileWithPath);
                foreach (Control item in tb.Controls)
                {
                    foreach (Control tabItem in item.Controls)
                    {
                        if (tabItem.Name == tagDTO.WaiversDTO.WaiverFileName.Substring(0, tagDTO.WaiversDTO.WaiverFileName.LastIndexOf('.')))
                        {
                            WebBrowser tabWebBrowser = (WebBrowser)tabItem;
                            tabWebBrowser.Navigate(urlLink, false);
                            break;
                        }
                    }
                }

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

        private void tbCtrlWaiver_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            //if (tbCtrlWaiver.SelectedTab != null &&
            //       tbCtrlWaiver.SelectedTab.Tag != null &&
            //       tbCtrlWaiver.SelectedTab.Tag is WaiversDTO)
            //{
            //    WaiversDTO temp = tbCtrlWaiver.SelectedTab.Tag as WaiversDTO;
            //    foreach (AxAcroPDF axAcroPDF in axAcroPDFList)
            //    {
            //        if (axAcroPDF.Name == temp.Guid)
            //        {
            //            string fileName = GetFileName(temp.WaiverFileName);
            //            axAcroPDF.src = fileName;
            //        }
            //    }
            //}
            log.LogMethodExit(null);
        }

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            LoadWaiverDocument(Convert.ToInt32(btnLanguage.Tag));

            GetLanguageDTOList();
            if (languagesDTOList != null)
            {
                foreach (LanguagesDTO languageDTO in languagesDTOList)
                {
                    if (languageDTO.LanguageId != Convert.ToInt32(btnLanguage.Tag))
                    {
                        btnLanguage.Tag = languageDTO.LanguageId;
                        btnLanguage.Text = languageDTO.LanguageName;
                    }
                    else
                    {
                        btnLanguage.Tag = -1;
                        btnLanguage.Text = "English";
                    }
                }
            }
        }

        private void frmWaiverSignature_DTU1031_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                //axFirstAcroPDF.Dispose();
                UpdateSignedFileName();
                DeleteFilesFromTabPages();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
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
            if (tbCtrlWaiver != null)
            {

                foreach (TabPage tapPageItem in tbCtrlWaiver.TabPages)
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
            }
            log.LogMethodExit();
        }

    }

    public class eventArgs : EventArgs
    {
        public string value1
        {
            get;
            private set;
        }
        public bool value2
        {
            get;
            private set;
        }

        public eventArgs(string value1Passed, bool value2Passed)
        {
            value1 = value1Passed;
            value2 = value2Passed;
        }
    }
}
