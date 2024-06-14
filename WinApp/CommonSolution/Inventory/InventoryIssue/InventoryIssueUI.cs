
/********************************************************************************************
 * Project Name - InventoryIssueUI
 * Description  - UI for InventoryIssue
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2       17-Oct-2019    Dakshakh raj      Modified : Issue fix for IN-claus in Sql Injection
 * 2.80        18-May-2020    Laster Menezes    Exception handling while printing InventoryIssueReceipt
 *2.100.0      13-Sep-2020    Deeksha           Modified to add UOM drop down field 
 *2.110.0      22-Oct-2020    Mushahid Faizan   Handled execution Context in the Constructor.
 *2.120        03-May-2021    Laster Menezes    Modified GetInventoryIssueReport method to use receipt framework of reports for InventoryIssueReceipt generation
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.Inventory;
using System.Data.SqlClient;
using System.Net;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Site;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Customer;
using System.Globalization;
using System.Drawing.Printing;
using Semnox.Parafait.Reports;
using System.IO;
using Semnox.Parafait.Product;
using Semnox.Parafait.Vendor;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// User interface for issue item to the department
    /// </summary>
    public partial class InventoryIssueUI : Form
    {
        logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Utilities utilities;
        string issueGuid = "";
        List<InventoryDocumentTypeDTO> inventoryDocumentTypeListOnDisplay;
        List<PurchaseOrderDTO> purchaseOrderListOnDisplay;
        List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList;
        List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList;
        List<ProductDTO> productDTOList;
        //List<LocationDTO> locationDTOList;
        SortableBindingList<ProductDTO> sortProductDTOList;
        SortableBindingList<RequisitionDTO> sortRequisitionDTOList;
        SortableBindingList<PurchaseOrderDTO> sortPurchaseOrderDTOList;
        InventoryDocumentTypeList inventoryDocumentTypeList;
        RequisitionList inventoryRequisitionList;
        PurchaseOrderList purchaseOrderList;
        Product.ProductList productList;
        LocationList locationList;
        List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> inventoryProductSearchParams;
        List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams;
        List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>> purchaseOrderSearchParams;
        List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> inventoryRequisitionSearchParams;
        //List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchByLocationParameters;
        List<RequisitionDTO> inventoryRequisitionListOnDisplay;
        InventoryIssueHeaderDTO inventoryIssueHeaderDTOonsave;
        //int purchaseOrderid = -1;
        int inventoryIssueId = 0; // This is used for issue Report

        /// <summary>
        /// Constructor for item issue screen
        /// </summary>
        public InventoryIssueUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            utilities.setupDataGridProperties(ref dgvIssueLinesGrid);
            utilities.setupDataGridProperties(ref dgvFilter);
            utilities.setupDataGridProperties(ref dgvIssueSearch);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            inventoryDocumentTypeList = new InventoryDocumentTypeList(machineUserContext);
            purchaseOrderList = new PurchaseOrderList(machineUserContext);
            productList = new Product.ProductList(machineUserContext);
            locationList = new LocationList(machineUserContext);

            LoadToSite();
            LoadAll();
            ProductContainer productContainer = new ProductContainer(machineUserContext);
            UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor for item issue screen
        /// </summary>
        public InventoryIssueUI(Utilities _Utilities, string guid)
        {
            log.LogMethodEntry(_Utilities, guid);
            InitializeComponent();
            utilities = _Utilities;
            issueGuid = guid;
            utilities.setupDataGridProperties(ref dgvIssueLinesGrid);
            utilities.setupDataGridProperties(ref dgvFilter);
            utilities.setupDataGridProperties(ref dgvIssueSearch);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            inventoryDocumentTypeList = new InventoryDocumentTypeList(machineUserContext);
            LoadToSite();
            LoadToView();
            log.LogMethodExit();
        }

        private void InventoryIssueUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadUOMComboBox();
            log.LogMethodExit();
        }
        private void LoadToSite()
        {
            log.LogMethodEntry();
            if (utilities.getParafaitDefaults("ENABLE_INTER_STORE_ADJUSTMENT").Equals("Y"))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });//certificate validation procedure for the SSL/TLS secure channel   
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers
                string message = "";
                SiteList siteList = new SiteList(machineUserContext);
                List<SiteDTO> siteDTOList;
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchBySiteParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                searchBySiteParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, utilities.ParafaitEnv.SiteId.ToString()));
                siteDTOList = siteList.GetAllSites(searchBySiteParameters);
                if (siteDTOList != null)
                {
                    try
                    {
                        if (utilities.ParafaitEnv.IsMasterSite && utilities.ParafaitEnv.IsCorporate)
                        {
                            siteDTOList = siteList.GetAllSites(utilities.ParafaitEnv.SiteId, siteDTOList[0].OrgId, siteDTOList[0].CompanyId);
                            if (siteDTOList == null)
                            {
                                siteDTOList = new List<SiteDTO>();
                            }
                            siteDTOList.Insert(0, new SiteDTO());
                            siteDTOList[0].SiteId = -1;
                            siteDTOList[0].SiteName = "<SELECT>";
                            if (siteDTOList != null && siteDTOList.Count > 0)
                            {
                                cmbToSite.DataSource = siteDTOList;
                                cmbToSite.ValueMember = "SiteId";
                                cmbToSite.DisplayMember = "SiteName";
                                cmbToSite.Tag = "T";
                            }
                        }
                        else
                        {
                            ParafaitGateway.SiteDTO[] hqSiteDTOArray;
                            ParafaitGateway.Service parafaitGateway = new ParafaitGateway.Service();
                            parafaitGateway.Url = utilities.getParafaitDefaults("WEBSERVICE_UPLOAD_URL");
                            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };//certificate validation procedure for the SSL/TLS secure channel   
                            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers

                            utilities.ParafaitEnv.getCompanyLoginKey();
                            hqSiteDTOArray = parafaitGateway.GetAllSite(utilities.ParafaitEnv.CompanyLoginKey, utilities.ParafaitEnv.SiteId, siteDTOList[0].OrgId, siteDTOList[0].CompanyId, ref message);
                            if (hqSiteDTOArray.Length == 0)
                            {
                                log.Error("LoadToSite() constructor.Web service not returned the data.");
                                MessageBox.Show(utilities.MessageUtils.getMessage(1539) + message);
                            }
                            else
                            {
                                List<ParafaitGateway.SiteDTO> hqSiteDTOList = hqSiteDTOArray.ToList<ParafaitGateway.SiteDTO>();
                                hqSiteDTOList.Insert(0, new ParafaitGateway.SiteDTO());
                                hqSiteDTOList[0].SiteId = -1;
                                hqSiteDTOList[0].SiteName = "<SELECT>";
                                cmbToSite.DataSource = hqSiteDTOList;
                                cmbToSite.ValueMember = "SiteId";
                                cmbToSite.DisplayMember = "SiteName";
                                cmbToSite.Tag = "T";
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("LoadToSite() constructor.Inter store details load error:" + ex.ToString());
                        MessageBox.Show(utilities.MessageUtils.getMessage(1538));
                        log.LogMethodExit(ex);
                    }
                }
                else
                {
                    log.Error("LoadToSite() constructor.Unable to load current site details");
                    MessageBox.Show(utilities.MessageUtils.getMessage(1539));
                }
            }
            log.LogMethodExit();
        }
        private void LoadToView()
        {
            log.LogMethodEntry();
            clearAll();
            pnlIssueMainArrange(false);
            LoadStatus();
            LoadIssueType();
            LoadIssue(issueGuid);
            log.LogMethodExit();
        }

        private void LoadAll()
        {
            log.LogMethodEntry();
            clearAll();
            pnlIssueMainArrange(false);
            LoadStatus();
            LoadIssueType();
            LoadIssue();
            //LoadLocation();
            log.LogMethodExit();
        }
        private void clearAll()
        {
            log.LogMethodEntry();
            cmbIssueType.Enabled = cmbFilterIssueType.Enabled = true;
            cmbIssueType.SelectedValue = -1;
            cmbIssueType.Tag = null;
            pnlIssue.Tag = null;
            txtDeliveryDate.ResetText();
            txtDeliveryNoteNo.ResetText();
            cmbFilterIssueType.ResetText();
            txtFilterPOReqNo.ResetText();
            txtFilterPOReqNo.Visible = false;
            txtIssueDate.Text = txtFromDate.Text = txtToDate.Text = DateTime.Now.ToString("dd-MMM-yyyy");//utilities.getDateFormat());
            txtPODate.ResetText();
            txtPONumber.ResetText();
            txtPONumber.Tag = null;
            cmbStatus.SelectedValue = "Open";
            cmbToSite.SelectedValue = -1;
            cmbToSite.Tag = null;
            lblFromsite.Text = string.Empty;
            //txtRemarks.ResetText();
            txtReferenceNumber.Text = string.Empty;
            txtIssueNumber.Text = string.Empty;
            txtIssueNumberSearch.Text = string.Empty;
            txtSupInvDate.ResetText();
            txtSupInvoiceNo.ResetText();
            txtVendor.ResetText();
            txtVendorCode.ResetText();
            dgvFilter.DataSource = null;
            dgvIssueLinesGrid.DataSource = null;
            btnSave.Enabled = true;
            txtReferenceNumber.ReadOnly = false;
            txtReferenceNumber.Enabled = true;
            lnkRemarks.Enabled = false;
            lblFromsite.Visible = false;
            lblTosite.Visible = cmbToSite.Visible = false;
            lblMessage.Text = string.Empty;
            lblCodeno.Text = string.Empty;
            EnableDisableControl(true);
            btnPrint.Enabled = false;
            try
            {
                dgvFilter.Columns.Remove("RequisitionTypeName");
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
            }
        }

        private void LoadIssueType()
        {
            log.LogMethodEntry();
            inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY, "Issue"));
            inventoryDocumentTypeListOnDisplay = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
            if (inventoryDocumentTypeListOnDisplay == null)
            {
                inventoryDocumentTypeListOnDisplay = new List<InventoryDocumentTypeDTO>();
            }
            inventoryDocumentTypeListOnDisplay.Insert(0, new InventoryDocumentTypeDTO());
            BindingSource inventoryDocTypeBS = new BindingSource();
            BindingSource inventoryDocTypeForIssueBS = new BindingSource();
            inventoryDocTypeForIssueBS.DataSource = inventoryDocumentTypeListOnDisplay;
            inventoryDocTypeBS.DataSource = inventoryDocumentTypeListOnDisplay;
            cmbIssueType.DataSource = inventoryDocumentTypeListOnDisplay;
            cmbIssueType.ValueMember = "DocumentTypeId";
            cmbIssueType.DisplayMember = "Name";
            cmbFilterIssueType.DataSource = inventoryDocTypeBS;
            cmbFilterIssueType.ValueMember = "DocumentTypeId";
            cmbFilterIssueType.DisplayMember = "Name";

            documentTypeIdDataGridViewTextBoxColumn.DataSource = inventoryDocTypeForIssueBS;
            documentTypeIdDataGridViewTextBoxColumn.ValueMember = "DocumentTypeId";
            documentTypeIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            txtFilterPOReqNo.Visible = false;
            #region MergingDocCode
            //inventoryDocumentTypeListOnDisplay[0].Name = "Select";            
            //var ValueList = new Dictionary<int, string>();
            //foreach (InventoryDocumentTypeDTO inventoryDocumentTypeDTO in inventoryDocumentTypeListOnDisplay)
            //{
            //    ValueList[inventoryDocumentTypeDTO.DocumentTypeId] = inventoryDocumentTypeDTO.Code + ((string.IsNullOrEmpty(inventoryDocumentTypeDTO.Name)) ? "" : " - " + inventoryDocumentTypeDTO.Name);
            //}
            //cmbIssueType.DataSource = new BindingSource(ValueList, null);
            //cmbIssueType.ValueMember = "Key";
            //cmbIssueType.DisplayMember = "Value";
            #endregion
            log.LogMethodExit();
        }
        private void pnlIssueMainArrange(bool pnlPOVisible)
        {
            log.LogMethodEntry(pnlPOVisible);
            //purchaseOrderid = -1;            
            pnlPO.Visible = pnlPOVisible;
            if (pnlPOVisible)
            {
                //pnlIssue.Top = pnlPO.Bottom;
                pnlIssueGrid.Top = pnlPO.Bottom + 5;
                pnlIssueGrid.Height = pnlIssueMainPanel.Bottom - pnlIssue.Bottom - 10;
            }
            else
            {
                //pnlRemarks.Top = pnlPO.Top;
                pnlIssueGrid.Top = pnlIssue.Bottom + 5;
                pnlIssueGrid.Height = pnlIssueMainPanel.Bottom - pnlIssue.Bottom - 10;
            }
            log.LogMethodExit();
        }
        private void cmbFilterIssueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (inventoryDocumentTypeListOnDisplay != null && !cmbFilterIssueType.Text.Equals("Semnox.Parafait.Inventory.InventoryDocumentTypeDTO") && !string.IsNullOrEmpty(cmbFilterIssueType.Text))
            {
                string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((cmbFilterIssueType.SelectedValue == null) ? -1 : (int)cmbFilterIssueType.SelectedValue))).ToList<InventoryDocumentTypeDTO>()[0].Code;
                LoadForm(code, false);
            }
            log.LogMethodExit();
        }
        private void cmbIssueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (inventoryDocumentTypeListOnDisplay != null && !cmbIssueType.Text.Equals("Semnox.Parafait.Inventory.InventoryDocumentTypeDTO") && !string.IsNullOrEmpty(cmbIssueType.Text))
            {
                cmbIssueType.Enabled = cmbFilterIssueType.Enabled = false;
                cmbFilterIssueType.SelectedValue = cmbIssueType.SelectedValue;
                inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
                string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((cmbIssueType.SelectedValue == null) ? -1 : (int)cmbIssueType.SelectedValue))).ToList<InventoryDocumentTypeDTO>()[0].Code;
                LoadForm(code);
            }
            log.LogMethodExit();
        }

        private void LoadForm(string code, bool IsFormArrange = true)
        {
            log.LogMethodEntry(code, IsFormArrange);
            txtFilterPOReqNo.Visible = true;
            fromLocationIDDataGridViewTextBoxColumn.ReadOnly = false;
            toLocationIDDataGridViewTextBoxColumn.ReadOnly = false;
            fromLocationIDDataGridViewTextBoxColumn.Visible = true;
            toLocationIDDataGridViewTextBoxColumn.Visible = true;
            availableQuantityDataGridViewTextBoxColumn.Visible = true;
            lnkRequisition.Enabled = false;
            switch (code)
            {
                case "AJIS":
                    lblCodeno.Text = utilities.MessageUtils.getMessage("Product Code") + ":";
                    if (IsFormArrange)
                    {
                        LoadLocation("Store", true);
                        LoadLocation("Department", false);
                        pnlIssueMainArrange(false);
                        LoadProduct(txtFilterPOReqNo.Text);
                    }
                    break;
                case "DIIS":
                    lblCodeno.Text = utilities.MessageUtils.getMessage("PO No") + ":";
                    if (IsFormArrange)
                    {
                        fromLocationIDDataGridViewTextBoxColumn.ReadOnly = true;
                        toLocationIDDataGridViewTextBoxColumn.ReadOnly = true;
                        fromLocationIDDataGridViewTextBoxColumn.Visible = false;
                        availableQuantityDataGridViewTextBoxColumn.Visible = false;
                        LoadLocation("None", true);
                        LoadLocation("Department", false);
                        pnlIssueMainArrange(true);
                        LoadPurchaseOrder();
                    }
                    break;
                case "REIS":
                    lblCodeno.Text = utilities.MessageUtils.getMessage("Req. No") + ":";
                    if (IsFormArrange)
                    {
                        fromLocationIDDataGridViewTextBoxColumn.ReadOnly = true;
                        toLocationIDDataGridViewTextBoxColumn.ReadOnly = true;
                        LoadLocation("Store", true);
                        LoadLocation("Store,Department", false);
                        pnlIssueMainArrange(false);
                        LoadRequistition();
                    }
                    break;
                case "STIS":
                    lblCodeno.Text = utilities.MessageUtils.getMessage("Product Code") + ":";
                    if (IsFormArrange)
                    {
                        LoadLocation("Store", true);
                        LoadLocation("Store", false);
                        pnlIssueMainArrange(false);
                        LoadProduct(txtFilterPOReqNo.Text);
                    }
                    break;
                case "ITIS":
                    lblCodeno.Text = utilities.MessageUtils.getMessage("Product Code") + ":";
                    if (IsFormArrange)
                    {
                        lnkRequisition.Enabled = true;
                        toLocationIDDataGridViewTextBoxColumn.Visible = false;
                        LoadLocation("Store", true);
                        LoadLocation("In Transit", false);
                        pnlIssueMainArrange(false);
                        lblTosite.Visible = cmbToSite.Visible = true;
                        LoadProduct(txtFilterPOReqNo.Text, true);
                    }
                    break;
            }
            lblCodeno.Refresh();
            log.LogMethodExit();
        }
        private void LoadProduct(string productCode = "", bool isPublished = false)
        {
            log.LogMethodEntry(productCode, isPublished);
            if (productList == null)
            {
                productList = new ProductList(machineUserContext);
            }
            inventoryProductSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
            inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.MARKET_LIST_ITEM, "0"));
            if (isPublished)
            {
                inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_PUBLISHED, "1"));
            }
            if (!string.IsNullOrEmpty(productCode))
            {
                inventoryProductSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.CODE, productCode));
            }
            productDTOList = productList.GetAllProducts(inventoryProductSearchParams);
            if (productDTOList == null)
            {
                sortProductDTOList = new SortableBindingList<ProductDTO>();
            }
            else
            {
                sortProductDTOList = new SortableBindingList<ProductDTO>(productDTOList);
            }
            BindingSource productDTOListBS = new BindingSource();
            productDTOListBS.DataSource = sortProductDTOList;
            dgvFilter.DataSource = productDTOListBS;
            if (dgvFilter.Rows.Count > 0)
            {
                dgvFilter.Rows[0].Selected = true;
            }
            foreach (DataGridViewColumn dgc in dgvFilter.Columns)
            {
                dgc.Visible = false;
            }
            dgvFilter.Columns["Code"].Visible = true;
            dgvFilter.Columns["Description"].Visible = true;
            //dgvFilter.Columns["ProductId"].Visible = true;  

            log.LogMethodExit();

        }
        private void LoadPurchaseOrder(string PONumber = "")
        {
            log.LogMethodEntry(PONumber);
            inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE, "CPPO"));
            inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
            if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
            {
                purchaseOrderSearchParams = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>();
                purchaseOrderSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                purchaseOrderSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_TYPE_ID, inventoryDocumentTypeDTOList[0].DocumentTypeId.ToString()));
                purchaseOrderSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.TO_DATE, DateTime.Today.ToString("yyyy-MM-dd")));
                if (!string.IsNullOrEmpty(PONumber))
                    purchaseOrderSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER, PONumber));
                purchaseOrderListOnDisplay = purchaseOrderList.GetAllPurchaseOrder(purchaseOrderSearchParams, machineUserContext.GetUserId());
                if (purchaseOrderListOnDisplay == null)
                {
                    sortPurchaseOrderDTOList = new SortableBindingList<PurchaseOrderDTO>();
                }
                else
                {
                    sortPurchaseOrderDTOList = new SortableBindingList<PurchaseOrderDTO>(purchaseOrderListOnDisplay);
                }
                BindingSource PurchaseOrderDTOListBS = new BindingSource();
                PurchaseOrderDTOListBS.DataSource = sortPurchaseOrderDTOList;
                dgvFilter.DataSource = PurchaseOrderDTOListBS;
                foreach (DataGridViewColumn dgc in dgvFilter.Columns)
                {
                    dgc.Visible = false;
                }
                dgvFilter.Columns["OrderNumber"].Visible = true;
                dgvFilter.Columns["OrderDate"].Visible = true;
                dgvFilter.Columns["RequestShipDate"].Visible = true;
                dgvFilter.Columns["Fromdate"].Visible = true;
                dgvFilter.Columns["ToDate"].Visible = true;
            }
            log.LogMethodExit();
        }

        private void LoadRequistition(string reqNumber = "")
        {
            log.LogMethodEntry(reqNumber);
            try
            {
                dgvFilter.Columns.Remove("RequisitionTypeName");
            }
            catch { }
            inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE, "ISRQ"));
            inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
            if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
            {
                inventoryRequisitionSearchParams = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
                inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, "1"));
                inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                //inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.EXPECTED_RECEIPT_DATE, DateTime.Today.ToString("yyyy-MM-dd")));
                inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE, inventoryDocumentTypeDTOList[0].DocumentTypeId.ToString()));
                inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.STATUS, "Submitted,InProgress"));
                if (!string.IsNullOrEmpty(reqNumber))
                    inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_NUMBER, reqNumber));
                inventoryRequisitionList = new RequisitionList(machineUserContext);
                inventoryRequisitionListOnDisplay = inventoryRequisitionList.GetAllRequisitions(inventoryRequisitionSearchParams);
                if (inventoryRequisitionListOnDisplay == null)
                {
                    sortRequisitionDTOList = new SortableBindingList<RequisitionDTO>();
                }
                else
                {
                    sortRequisitionDTOList = new SortableBindingList<RequisitionDTO>(inventoryRequisitionListOnDisplay);
                }
                BindingSource RequisitionDTOListBS = new BindingSource();
                RequisitionDTOListBS.DataSource = sortRequisitionDTOList;
                dgvFilter.DataSource = RequisitionDTOListBS;
                foreach (DataGridViewColumn dgc in dgvFilter.Columns)
                {
                    dgc.Visible = false;
                }
                DataGridViewTextBoxColumn dc = new DataGridViewTextBoxColumn();
                dc.Name = "RequisitionTypeName";
                dc.HeaderText = "Type";

                dgvFilter.Columns.Add(dc);
                for (int i = 0; i < dgvFilter.Rows.Count; i++)
                {
                    inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                    inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
                    inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE, "ISRQ"));
                    inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
                    if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
                    {
                        dgvFilter.Rows[i].Cells["RequisitionTypeName"].Value = inventoryDocumentTypeDTOList[0].Name;
                    }
                }
                dgvFilter.Columns["RequisitionNo"].Visible = true;
                //dgvFilter.Columns["RequisitionType"].Visible = true;
                dgvFilter.Columns["RequisitionTypeName"].Visible = true;
                dgvFilter.Columns["RequisitionTypeName"].DisplayIndex = 3;
                dgvFilter.Columns["Status"].Visible = true;
            }
            log.LogMethodExit();
        }

        private void dgvFilter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.RowIndex >= 0)
            {
                try
                {
                    //Semnox.Product product;
                    InventoryIssueLinesDTO inventoryIssueLinesDTO = new InventoryIssueLinesDTO();
                    if (cmbFilterIssueType.SelectedValue != null && (int)cmbFilterIssueType.SelectedValue > 0)
                    {
                        cmbIssueType.SelectedValue = cmbFilterIssueType.SelectedValue;
                    }
                    //if (dgvFilter.CurrentRow != null)
                    //{
                    string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((cmbIssueType.SelectedValue == null) ? -1 : (int)cmbIssueType.SelectedValue))).ToList<InventoryDocumentTypeDTO>()[0].Code;
                    switch (code)
                    {
                        case "AJIS":
                            BindingSource productDTOBS = (BindingSource)dgvFilter.DataSource;
                            var productDTOList = (SortableBindingList<ProductDTO>)productDTOBS.DataSource;
                            ProductDTO productDTO = productDTOList[e.RowIndex];//dgvFilter.CurrentRow.Index];
                            //product = new Semnox.Product(productDTO);
                            inventoryIssueLinesDTO.ProductId = productDTO.ProductId;
                            inventoryIssueLinesDTO.ProductName = productDTO.Description;
                            inventoryIssueLinesDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productDTO.InventoryUOMId).UOM;
                            inventoryIssueLinesDTO.UOMId = productDTO.InventoryUOMId;
                            //inventoryIssueLinesDTO.Price = product.GetProductPrice(1); //productDTO.Cost;
                            inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                            LoadIssueDetails(-1, -1, code, productDTO.ProductId);
                            break;
                        case "DIIS":
                            BindingSource purchaseOrderDTOBS = (BindingSource)dgvFilter.DataSource;
                            var purchaseOrderDTOList = (SortableBindingList<PurchaseOrderDTO>)purchaseOrderDTOBS.DataSource;
                            PurchaseOrderDTO purchaseOrderDTO = purchaseOrderDTOList[e.RowIndex];//dgvFilter.CurrentRow.Index];
                            LoadIssueDetails(-1, -1, code, purchaseOrderDTO.PurchaseOrderId);
                            LoadPODetails(purchaseOrderDTO);
                            txtPONumber.Tag = purchaseOrderDTO.PurchaseOrderId;
                            break;
                        case "REIS":
                            BindingSource requisitionDTOBS = (BindingSource)dgvFilter.DataSource;
                            var requisitionDTOList = (SortableBindingList<RequisitionDTO>)requisitionDTOBS.DataSource;
                            RequisitionDTO requisitionDTO = requisitionDTOList[e.RowIndex];//dgvFilter.CurrentRow.Index];
                            LoadIssueDetails(requisitionDTO.FromDepartment, requisitionDTO.ToDepartment, code, requisitionDTO.RequisitionId);
                            cmbIssueType.Tag = requisitionDTO.RequisitionId;
                            break;
                        case "STIS":
                            BindingSource productsDTOBS = (BindingSource)dgvFilter.DataSource;
                            var productsDTOList = (SortableBindingList<ProductDTO>)productsDTOBS.DataSource;
                            ProductDTO productsDTO = productsDTOList[e.RowIndex];//dgvFilter.CurrentRow.Index];
                            //product = new Semnox.Product(productsDTO);
                            inventoryIssueLinesDTO.ProductId = productsDTO.ProductId;
                            inventoryIssueLinesDTO.ProductName = productsDTO.Description;
                            inventoryIssueLinesDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productsDTO.InventoryUOMId).UOM;
                            inventoryIssueLinesDTO.UOMId = productsDTO.InventoryUOMId;
                            //inventoryIssueLinesDTO.Price = product.GetProductPrice(1); //productsDTO.Cost;
                            inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                            LoadIssueDetails(-1, -1, code, productsDTO.ProductId);
                            break;
                        case "ITIS":
                            BindingSource publishedProductsDTOBS = (BindingSource)dgvFilter.DataSource;
                            var publishedProductsDTOList = (SortableBindingList<ProductDTO>)publishedProductsDTOBS.DataSource;
                            ProductDTO publishedProductsDTO = publishedProductsDTOList[e.RowIndex];//dgvFilter.CurrentRow.Index];
                            //product = new Semnox.Product(publishedProductsDTO);
                            inventoryIssueLinesDTO.ProductId = publishedProductsDTO.ProductId;
                            inventoryIssueLinesDTO.ProductName = publishedProductsDTO.Description;
                            if (publishedProductsDTO.InventoryUOMId != -1)
                            {
                                inventoryIssueLinesDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == publishedProductsDTO.InventoryUOMId).UOM;
                            }
                            else
                            {
                                inventoryIssueLinesDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == publishedProductsDTO.UomId).UOM;
                            }
                            inventoryIssueLinesDTO.UOMId = publishedProductsDTO.InventoryUOMId;
                            //inventoryIssueLinesDTO.Price = product.GetProductPrice(1);//publishedProductsDTO.Cost;
                            inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                            LoadIssueDetails(-1, -1, code, publishedProductsDTO.ProductId);
                            break;
                    }

                    //}
                }
                catch (Exception ex) { log.Fatal("dgvFilter_CellDoubleClick() event. throws exception:" + ex.ToString()); }
            }
            log.LogMethodExit();
        }
        private void LoadPODetails(PurchaseOrderDTO purchaseOrderDTO)
        {
            log.LogMethodEntry(purchaseOrderDTO);
            if (purchaseOrderDTO != null)
            {
                txtPONumber.Text = purchaseOrderDTO.OrderNumber;
                txtPODate.Text = purchaseOrderDTO.OrderDate.ToString("dd-MMM-yyyy");//utilities.getDateFormat());
                LoadVendor(purchaseOrderDTO.VendorId);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Loads the status to the comboboxes
        /// </summary>
        private void LoadStatus()
        {
            log.LogMethodEntry();
            try
            {
                List<LookupValuesDTO> lookupValuesDTOList;
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_ISSUE_STATUS"));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                //lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                bindingSource.DataSource = lookupValuesDTOList;
                cmbStatus.DataSource = bindingSource;
                cmbStatus.ValueMember = "LookupValue";
                cmbStatus.DisplayMember = "Description";
                cmbStatus.SelectedValue = "OPEN";
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadStatus() Method with an Exception:", e);
            }
        }
        private void LoadIssueDetails(int fromLocationId, int toLocationId, string code, int POOrReqid = -1)
        {
            log.LogMethodEntry(fromLocationId, toLocationId, code, POOrReqid);
            if (POOrReqid > -1)
            {
                //Semnox.Product product;
                ProductDTO productDTO;
                InventoryIssueLinesDTO inventoryIssueLinesDTO;
                RequisitionLinesList requisitionLinesList = new RequisitionLinesList(machineUserContext);
                List<RequisitionLinesDTO> requisitionLinesDTOList = new List<RequisitionLinesDTO>();
                List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> inventoryRequisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
                switch (code)
                {
                    case "AJIS":
                        break;
                    case "DIIS":
                        List<PurchaseOrderLineDTO> purchaseOrderLineDTOList = new List<PurchaseOrderLineDTO>();
                        PurchaseOrderLineList purchaseOrderLineList = new PurchaseOrderLineList();
                        List<KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>> inventoryPurchaseOrderLineSearchParams = new List<KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>>();
                        inventoryPurchaseOrderLineSearchParams.Add(new KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.IS_ACTIVE, "Y"));
                        inventoryPurchaseOrderLineSearchParams.Add(new KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_ID, POOrReqid.ToString()));
                        purchaseOrderLineDTOList = purchaseOrderLineList.GetAllPurchaseOrderLine(inventoryPurchaseOrderLineSearchParams);
                        inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
                        if (purchaseOrderLineDTOList != null)
                        {
                            foreach (PurchaseOrderLineDTO purchaseOrderLineDTO in purchaseOrderLineDTOList)
                            {

                                inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
                                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE, "MLRQ"));
                                inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
                                if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
                                {
                                    inventoryRequisitionSearchParams = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
                                    inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, "1"));
                                    inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                    inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_TYPE, inventoryDocumentTypeDTOList[0].DocumentTypeId.ToString()));
                                    inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.STATUS, "Submitted,InProgress"));
                                    inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.EXPECTED_RECEIPT_DATE, DateTime.Today.ToString("yyyy-MM-dd")));

                                    inventoryRequisitionListOnDisplay = inventoryRequisitionList.GetAllRequisitions(inventoryRequisitionSearchParams);
                                    if (inventoryRequisitionListOnDisplay != null)
                                    {
                                        foreach (RequisitionDTO requisitionDTO in inventoryRequisitionListOnDisplay)
                                        {
                                            inventoryRequisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
                                            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
                                            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.PRODUCT_ID, purchaseOrderLineDTO.ProductId.ToString()));
                                            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, requisitionDTO.RequisitionId.ToString()));
                                            requisitionLinesDTOList = requisitionLinesList.GetAllRequisitionLines(inventoryRequisitionLinesSearchParams);

                                            if (requisitionLinesDTOList != null && requisitionLinesDTOList.Count > 0)
                                            {
                                                foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesDTOList)
                                                {
                                                    productDTO = GetProductDTO(purchaseOrderLineDTO.ProductId);
                                                    // product = new Semnox.Product(productDTO);
                                                    inventoryIssueLinesDTO = new InventoryIssueLinesDTO(purchaseOrderLineDTO.ProductId, productDTO.Description, (requisitionLinesDTO.RequestedQuantity - requisitionLinesDTO.ApprovedQuantity), 0, 0, -1, requisitionDTO.RequestingDept, requisitionLinesDTO.RequisitionId, requisitionLinesDTO.RequisitionLineId, requisitionLinesDTO.UOMId);
                                                    inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }
                        break;

                    case "REIS":
                        inventoryRequisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
                        inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
                        inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, POOrReqid.ToString()));
                        requisitionLinesDTOList = requisitionLinesList.GetAllRequisitionLines(inventoryRequisitionLinesSearchParams);
                        inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();

                        if (requisitionLinesDTOList != null && requisitionLinesDTOList.Count > 0)
                        {
                            foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesDTOList)
                            {
                                productDTO = GetProductDTO(requisitionLinesDTO.ProductId);
                                //product = new Semnox.Product(productDTO);
                                inventoryIssueLinesDTO = new InventoryIssueLinesDTO(requisitionLinesDTO.ProductId, productDTO.Description, (requisitionLinesDTO.RequestedQuantity - requisitionLinesDTO.ApprovedQuantity), GetProductQauntity(toLocationId, requisitionLinesDTO.ProductId), 0, toLocationId, fromLocationId, requisitionLinesDTO.RequisitionId, requisitionLinesDTO.RequisitionLineId, requisitionLinesDTO.UOMId);
                                inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                            }
                        }
                        break;
                    case "STIS":
                        break;
                    case "ITIS":
                        //if(inventoryIssueLinesDTOList==null)
                        //inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();

                        //inventoryRequisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
                        //inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
                        //inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, POOrReqid.ToString()));
                        //requisitionLinesDTOList = requisitionLinesList.GetAllRequisitionLines(inventoryRequisitionLinesSearchParams);

                        //if (requisitionLinesDTOList != null && requisitionLinesDTOList.Count > 0)
                        //{
                        //    foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesDTOList)
                        //    {
                        //        inventoryIssueLinesDTO = new InventoryIssueLinesDTO();
                        //        inventoryIssueLinesDTO.ProductId = requisitionLinesDTO.ProductId;
                        //        inventoryIssueLinesDTO.ProductName = GetProductName(requisitionLinesDTO.ProductId);
                        //        inventoryIssueLinesDTO.RequisitionID = requisitionLinesDTO.RequisitionId;
                        //        inventoryIssueLinesDTO.RequisitionLineID = requisitionLinesDTO.RequisitionLineId;
                        //        inventoryIssueLinesDTO.Quantity = requisitionLinesDTO.RequestedQuantity - requisitionLinesDTO.ApprovedQuantity;
                        //        //inventoryIssueLinesDTO.AvailableQuantity = GetProductQauntity(toLocationId, inventoryIssueLinesDTO.ProductId);
                        //        //inventoryIssueLinesDTO.FromLocationID = toLocationId;
                        //        //inventoryIssueLinesDTO.ToLocationID = fromLocationId;
                        //        inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                        //    }
                        //}

                        break;
                }

                SortableBindingList<InventoryIssueLinesDTO> sortInventoryIssueLinesDTOList = new SortableBindingList<InventoryIssueLinesDTO>();
                if (inventoryIssueLinesDTOList == null)
                {
                    sortInventoryIssueLinesDTOList = new SortableBindingList<InventoryIssueLinesDTO>();
                }
                else
                {
                    sortInventoryIssueLinesDTOList = new SortableBindingList<InventoryIssueLinesDTO>(inventoryIssueLinesDTOList);
                }
                BindingSource RequisitionLineDTOListBS = new BindingSource();
                RequisitionLineDTOListBS.DataSource = sortInventoryIssueLinesDTOList;
                dgvIssueLinesGrid.DataSource = RequisitionLineDTOListBS;
                if (sortInventoryIssueLinesDTOList != null)
                {
                    for (int i = 0; i < sortInventoryIssueLinesDTOList.Count; i++)
                    {
                        dgvIssueLinesGrid.Rows[i].Tag = sortInventoryIssueLinesDTOList[i].Quantity;
                        int uomId = sortInventoryIssueLinesDTOList[i].UOMId;
                        if (uomId == -1)
                        {
                            if (ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).InventoryUOMId != -1)
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).InventoryUOMId;
                            }
                            else
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).UomId;
                            }
                        }
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvIssueLinesGrid, i, uomId);
                        dgvIssueLinesGrid["txtUOM", i].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
                    }

                }
            }
            log.LogMethodExit();
        }


        private ProductDTO GetProductDTO(int productId)
        {
            log.LogMethodEntry(productId);

            List<ProductDTO> productDTOList = new List<ProductDTO>();
            Product.ProductList productList = new Product.ProductList();
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productPurchaseOrderLineSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            productPurchaseOrderLineSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, productId.ToString()));

            productDTOList = productList.GetAllProducts(productPurchaseOrderLineSearchParams);
            if (productDTOList != null && productDTOList.Count > 0)
            {
                return productDTOList[0];
            }
            log.LogMethodExit();
            return null;
        }
        private double GetProductQauntity(int LocationId, int productId)
        {
            log.LogMethodEntry(LocationId, productId);
            double quantity = 0.0;
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            InventoryList inventoryList = new InventoryList();
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventoryPurchaseOrderLineSearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
            inventoryPurchaseOrderLineSearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, productId.ToString()));
            inventoryPurchaseOrderLineSearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, LocationId.ToString()));
            inventoryDTOList = inventoryList.GetAllInventory(inventoryPurchaseOrderLineSearchParams);
            if (inventoryDTOList != null && inventoryDTOList.Count > 0)
            {
                foreach (InventoryDTO inventoryDTO in inventoryDTOList)
                    quantity += inventoryDTO.Quantity;
            }
            log.LogMethodExit(quantity);
            return quantity;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int locationId = -1;
            Product.ProductBL product;
            ApprovalRuleDTO approvalRuleDTO = new ApprovalRuleDTO();
            ApprovalRule approvalRule = new ApprovalRule(machineUserContext, approvalRuleDTO);
            List<UserMessagesDTO> userMessagesDTOList;
            UserMessagesList userMessagesList = new UserMessagesList();
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList();
            InventoryIssueLinesDTO inventoryIssueLineSavedDTO;
            UserMessages userMessages;
            lblMessage.Text = "";
            string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((cmbIssueType.SelectedValue == null) ? -1 : (int)cmbIssueType.SelectedValue))).ToList<InventoryDocumentTypeDTO>()[0].Code;
            if (!string.IsNullOrWhiteSpace(code))
            {
                inventoryIssueHeaderDTOonsave = new InventoryIssueHeaderDTO();
                var inventoryIssueLinesListOnDisplay = (SortableBindingList<InventoryIssueLinesDTO>)null;
                if (ValidateForm(code))
                {
                    if (code.Equals("ITIS"))
                    {
                        BindingSource locationList = (BindingSource)toLocationIDDataGridViewTextBoxColumn.DataSource;
                        List<LocationDTO> locationDTOList = (List<LocationDTO>)locationList.DataSource;
                        if (locationDTOList.Count > 1)
                        {
                            locationId = locationDTOList[1].LocationId;
                        }
                        else
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1537));
                            return;
                        }
                    }
                    if (pnlIssue.Tag != null)
                    {
                        inventoryIssueHeaderDTOonsave.InventoryIssueId = Convert.ToInt32(pnlIssue.Tag);
                    }
                    inventoryIssueHeaderDTOonsave.DocumentTypeId = (int)cmbIssueType.SelectedValue;
                    inventoryIssueHeaderDTOonsave.IssueDate = dtpIssueDate.Value;
                    inventoryIssueHeaderDTOonsave.Status = cmbStatus.SelectedValue.ToString();
                    //inventoryIssueHeaderDTO.Remarks = txtRemarks.Text;
                    inventoryIssueHeaderDTOonsave.ReferenceNumber = txtReferenceNumber.Text.ToString();
                    switch (code)
                    {
                        case "AJIS":
                            break;
                        case "DIIS":
                            if (txtPONumber.Tag != null)
                            {
                                inventoryIssueHeaderDTOonsave.PurchaseOrderId = (int)txtPONumber.Tag;
                            }
                            inventoryIssueHeaderDTOonsave.SupplierInvoiceDate = dtpSupInvDate.Value;
                            inventoryIssueHeaderDTOonsave.DeliveryNoteDate = dtpDeliveryDate.Value;
                            inventoryIssueHeaderDTOonsave.DeliveryNoteNumber = txtDeliveryNoteNo.Text;
                            inventoryIssueHeaderDTOonsave.SupplierInvoiceNumber = txtSupInvoiceNo.Text;
                            inventoryIssueHeaderDTOonsave.IsActive = true;
                            break;
                        case "REIS":
                            if (cmbIssueType.Tag != null)
                            {
                                try
                                { inventoryIssueHeaderDTOonsave.RequisitionID = int.Parse(cmbIssueType.Tag.ToString()); }
                                catch (Exception ex)
                                {
                                    log.LogMethodExit(ex);
                                }
                            }

                            break;
                        case "STIS":
                            break;
                        case "ITIS":
                            if (cmbIssueType.Tag != null)
                            {
                                try
                                { inventoryIssueHeaderDTOonsave.RequisitionID = int.Parse(cmbIssueType.Tag.ToString()); }
                                catch (Exception ex)
                                {
                                    log.LogMethodExit(ex);
                                }
                            }
                            if (cmbToSite.Tag != null)
                            {
                                if (cmbToSite.Tag.Equals("T"))
                                {
                                    if (inventoryIssueHeaderDTOonsave.FromSiteID == -1)
                                    {
                                        inventoryIssueHeaderDTOonsave.FromSiteID = utilities.ParafaitEnv.SiteId;
                                    }
                                    inventoryIssueHeaderDTOonsave.ToSiteID = Convert.ToInt32(cmbToSite.SelectedValue);
                                }
                                else if (cmbToSite.Tag.Equals("F"))
                                {
                                    inventoryIssueHeaderDTOonsave.FromSiteID = Convert.ToInt32(cmbToSite.SelectedValue);
                                    inventoryIssueHeaderDTOonsave.ToSiteID = utilities.ParafaitEnv.SiteId;
                                }
                            }
                            else
                            {
                                if (inventoryIssueHeaderDTOonsave.FromSiteID == -1)
                                {
                                    inventoryIssueHeaderDTOonsave.FromSiteID = utilities.ParafaitEnv.SiteId;
                                }
                                inventoryIssueHeaderDTOonsave.ToSiteID = Convert.ToInt32(cmbToSite.SelectedValue);
                            }
                            break;
                    }
                    SqlTransaction SQLTrx = null;
                    SqlConnection TrxCnn = null;

                    if (SQLTrx == null)
                    {
                        TrxCnn = utilities.createConnection();
                        SQLTrx = TrxCnn.BeginTransaction();//IsolationLevel.ReadUncommitted
                    }
                    try
                    {
                        InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTOonsave, machineUserContext);
                        inventoryIssueHeader.Save(SQLTrx);
                        inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTOonsave.InventoryIssueId, machineUserContext, SQLTrx);
                        inventoryIssueHeaderDTOonsave = inventoryIssueHeader.getInventoryIssueHeaderDTO;
                        inventoryIssueId = inventoryIssueHeaderDTOonsave.InventoryIssueId;    // for Report Generation

                        if (inventoryIssueHeaderDTOonsave.InventoryIssueId > -1)
                        {
                            BindingSource inventoryIssueLinesBS = (BindingSource)dgvIssueLinesGrid.DataSource;
                            inventoryIssueLinesListOnDisplay = (SortableBindingList<InventoryIssueLinesDTO>)inventoryIssueLinesBS.DataSource;
                            InventoryIssueLinesDTO inventoryIssueLinesDTO = new InventoryIssueLinesDTO();
                            for (int i = 0; i < inventoryIssueLinesListOnDisplay.Count; i++)
                            {
                                inventoryIssueLinesDTO = inventoryIssueLinesListOnDisplay[i];
                                log.Debug("inventoryIssueLinesDTO.UOMId : " + inventoryIssueLinesDTO.UOMId);

                                if (inventoryIssueLinesDTO.Quantity == 0 && inventoryIssueLinesDTO.IssueLineId == -1)
                                {
                                    continue;
                                }
                                inventoryIssueLinesDTO.IssueId = inventoryIssueHeaderDTOonsave.InventoryIssueId;
                                if (code.Equals("ITIS"))
                                {
                                    if (inventoryIssueHeaderDTOonsave.FromSiteID == utilities.ParafaitEnv.SiteId)
                                    {
                                        inventoryIssueLinesDTO.ToLocationID = locationId;
                                    }
                                }
                                product = new Product.ProductBL(inventoryIssueLinesDTO.ProductId);
                                inventoryIssueLinesDTO.Price = product.GetProductPrice(inventoryIssueLinesDTO.Quantity);
                                inventoryIssueLinesDTO.IssueNumber = inventoryIssueHeaderDTOonsave.IssueNumber;
                                log.Debug("inventoryIssueLinesDTO. UomId : " + inventoryIssueLinesDTO);
                                InventoryIssueLines inventoryIssueLines = new InventoryIssueLines(inventoryIssueLinesDTO, machineUserContext);
                                inventoryIssueLines.Save(SQLTrx);
                                if (inventoryIssueHeaderDTOonsave.Status.Equals("SUBMITTED"))
                                {
                                    inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTOonsave.InventoryIssueId, machineUserContext, SQLTrx);

                                    approvalRuleDTO = approvalRule.GetApprovalRule(utilities.ParafaitEnv.RoleId, inventoryIssueHeader.getInventoryIssueHeaderDTO.DocumentTypeId, machineUserContext.GetSiteId());
                                    if (approvalRuleDTO != null)
                                    {
                                        if (approvalRuleDTO.NumberOfApprovalLevels > 0)
                                        {
                                            List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, code));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                                            userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams, SQLTrx);
                                            if (userMessagesDTOList == null)
                                            {
                                                userMessages = new UserMessages(machineUserContext);
                                                userMessages.CreateUserMessages(approvalRuleDTO, "Inventory", code, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid, utilities.ParafaitEnv.User_Id, "Approval", "Pending for approval", SQLTrx);
                                            }
                                        }
                                    }
                                    userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage("Inventory", code, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid, -1, -1, machineUserContext.GetSiteId(), SQLTrx);
                                    if (inventoryIssueHeaderDTOonsave.ToSiteID == utilities.ParafaitEnv.SiteId && (userMessagesDTOList != null && userMessagesDTOList.Count == 1 && userMessagesDTOList[0].ApprovalRuleID == -1))
                                    {
                                        userMessagesDTOList[0].Status = UserMessagesDTO.UserMessagesStatus.APPROVED.ToString();
                                        userMessagesDTOList[0].ActedByUser = utilities.ParafaitEnv.User_Id;
                                        userMessages = new UserMessages(userMessagesDTOList[0], machineUserContext);
                                        userMessages.Save(SQLTrx);
                                        userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage("Inventory", code, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid, -1, -1, machineUserContext.GetSiteId(), SQLTrx);
                                    }
                                    if (userMessagesDTOList == null || (userMessagesDTOList != null && userMessagesDTOList.Count == 0))
                                    {
                                        if (inventoryIssueLinesDTO.IssueLineId >= 0)
                                        {
                                            inventoryIssueLineSavedDTO = inventoryIssueLinesList.GetIssueLines(inventoryIssueLinesDTO.IssueLineId, SQLTrx);
                                            if (inventoryIssueLineSavedDTO.IsActive)
                                            {
                                                inventoryIssueHeader.SaveIssueDetails(code, (int)cmbIssueType.SelectedValue, inventoryIssueLineSavedDTO, SQLTrx, code.Equals("DIIS") ? (int)txtPONumber.Tag : (code.Equals("ITIS") && cmbIssueType.Tag != null) ? (int)cmbIssueType.Tag : -1, (code.Equals("DIIS") ? txtSupInvoiceNo.Text : ""));

                                            }


                                        }
                                        else
                                        {
                                            throw new Exception(utilities.MessageUtils.getMessage(1074));
                                        }
                                    }
                                }
                            }
                            log.Debug("utilities.ParafaitEnv.IsMasterSite : " + utilities.ParafaitEnv.IsMasterSite);
                            log.Debug("utilities.ParafaitEnv.IsCorporate : " + utilities.ParafaitEnv.IsCorporate);
                            log.Debug("utilities.ParafaitEnv.SiteId : " + utilities.ParafaitEnv.SiteId);
                            log.Debug("utilities.ParafaitEnv.SiteName : " + utilities.ParafaitEnv.SiteName);

                            if (inventoryIssueHeaderDTOonsave != null && inventoryIssueHeaderDTOonsave.Status.Equals("SUBMITTED") && inventoryIssueLinesListOnDisplay.Count > 0)
                            {
                                if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite && inventoryIssueHeaderDTOonsave.FromSiteID == utilities.ParafaitEnv.SiteId && code.Equals("ITIS"))
                                {
                                    log.Debug("Creating PO in ProcessAsAutoHQApproval");
                                    InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((cmbIssueType.SelectedValue == null) ? -1 : (int)cmbIssueType.SelectedValue))).ToList<InventoryDocumentTypeDTO>()[0];
                                    inventoryIssueHeader.ProcessAsAutoHQApproval(inventoryIssueHeaderDTOonsave, inventoryDocumentTypeDTO, utilities, SQLTrx);
                                }

                               

                                if (!code.Equals("ITIS"))
                                {
                                    if (inventoryIssueHeader.getInventoryIssueHeaderDTO != null)
                                    {
                                        inventoryIssueHeader.getInventoryIssueHeaderDTO.Status = "COMPLETE";
                                        inventoryIssueHeader.Save(SQLTrx);
                                    }
                                }
                            }
                            //}
                            if (code.Equals("ITIS") && inventoryIssueLinesListOnDisplay != null && inventoryIssueLinesListOnDisplay.Count > 0 && inventoryIssueHeader.getInventoryIssueHeaderDTO.Status.Equals("SUBMITTED"))
                            {
                                double price = inventoryIssueLinesListOnDisplay.Sum(x => x.Price * x.Quantity);
                                CreateTransaction(price, SQLTrx);
                            }
                            SQLTrx.Commit();

                            btnPrint.Enabled = true;

                            pnlIssue.Tag = inventoryIssueHeaderDTOonsave.InventoryIssueId;
                            lblMessage.Text = utilities.MessageUtils.getMessage(122);
                            if (inventoryIssueHeader.getInventoryIssueHeaderDTO.Status.Equals("SUBMITTED") || inventoryIssueHeader.getInventoryIssueHeaderDTO.Status.Equals("COMPLETE"))
                            {
                                btnSave.Enabled = false;
                                txtReferenceNumber.Enabled = false;
                                fromLocationIDDataGridViewTextBoxColumn.ReadOnly = true;
                                toLocationIDDataGridViewTextBoxColumn.ReadOnly = true;
                            }
                            lnkRemarks.Enabled = true;
                            PopulateLine(inventoryIssueHeader.getInventoryIssueHeaderDTO.InventoryIssueId);
                        }
                        LoadIssue();
                        txtIssueNumber.Text = inventoryIssueHeaderDTOonsave.IssueNumber;
                    }
                    catch (Exception ex)
                    {
                        log.Fatal("Ends-btnSave_Click() event with exception: " + ex.ToString());
                        SQLTrx.Rollback();
                        if (TrxCnn != null)
                            TrxCnn.Close();
                        //inventoryIssueHeaderDTOonsave.InventoryIssueId = -1;
                        //foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesListOnDisplay)
                        //{
                        //    inventoryIssueLinesDTO.IssueLineId = -1;
                        //    inventoryIssueLinesDTO.IssueId = -1;
                        //}
                        MessageBox.Show(utilities.MessageUtils.getMessage("Save Unsuccessfull.") + "\n" + ex.Message);
                    }
                }
            }
            log.LogMethodExit();
        }


        private void CreateTransaction(double trxAmount, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(trxAmount, sqlTrxn);
            string message = string.Empty;
            if (utilities.getParafaitDefaults("CREATE_SALE_TRANSACTION").Equals("Y"))
            {
                Transaction.Transaction transaction = new Transaction.Transaction(utilities);
                CustomerListBL customerListBL = new CustomerListBL(machineUserContext);
                List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.PROFILE_PASSWORD, machineUserContext.GetSiteId().ToString()));
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(searchParameters);
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    transaction.customerDTO = customerDTOList[0];
                }
                Products products = new Products();
                ProductsDTO productsDTO = null;
                List<ProductsDTO> productsDTOList = products.GetProductByTypeList("INVENTORYINTERSTORE", machineUserContext.GetSiteId());
                if (productsDTOList != null)
                {
                    productsDTO = productsDTOList[0];
                }
                if (productsDTO != null)
                {
                    transaction.createTransactionLine(null, productsDTO.ProductId, trxAmount, 1, ref message);
                    if (transaction.SaveOrder(ref message, sqlTrxn) != 0)
                    {
                        MessageBox.Show("Transaction is not created. " + message, "Transaction creation failed.");
                        throw new Exception(message);
                    }
                }
            }
            log.LogMethodExit();
        }


        void PopulateLine(int inventoryIssueId)
        {
            log.LogMethodEntry(inventoryIssueId);
            ProductDTO productDTO;
            //Semnox.Product product;
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList();
            inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
            List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>> inventoryIssueLinesSearchParams = new List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>>();
            inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ACTIVE_FLAG, "1"));
            //inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_ID, inventoryIssueId.ToString()));
            inventoryIssueLinesDTOList = inventoryIssueLinesList.GetAllInventoryIssueLines(inventoryIssueLinesSearchParams);
            SortableBindingList<InventoryIssueLinesDTO> sortInventoryIssueLinesDTOList = new SortableBindingList<InventoryIssueLinesDTO>();
            if ((inventoryIssueLinesDTOList != null) && (inventoryIssueLinesDTOList.Any()))
            {
                foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesDTOList)
                {
                    productDTO = GetProductDTO(inventoryIssueLinesDTO.ProductId);
                    inventoryIssueLinesDTO.ProductName = productDTO.Description;
                    //product = new Semnox.Product(productDTO);
                    //inventoryIssueLinesDTO.Price = product.GetProductPrice(1);
                    if (cmbToSite.Tag != null && cmbToSite.Tag.Equals("F") && !string.IsNullOrEmpty(lblFromsite.Text) && Convert.ToInt32(lblFromsite.Text) == utilities.ParafaitEnv.SiteId)
                    {
                        inventoryIssueLinesDTO.AvailableQuantity = inventoryIssueLinesDTO.Quantity;
                    }
                    else
                    {
                        inventoryIssueLinesDTO.AvailableQuantity = GetProductQauntity(inventoryIssueLinesDTO.FromLocationID, inventoryIssueLinesDTO.ProductId);
                    }
                }
            }
            else
            {
                inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
            }
            sortInventoryIssueLinesDTOList = new SortableBindingList<InventoryIssueLinesDTO>(inventoryIssueLinesDTOList);
            BindingSource issueLineDTOListBS = new BindingSource();
            issueLineDTOListBS.DataSource = sortInventoryIssueLinesDTOList;
            dgvIssueLinesGrid.DataSource = issueLineDTOListBS;
            if (sortInventoryIssueLinesDTOList != null)
            {
                for (int i = 0; i < sortInventoryIssueLinesDTOList.Count; i++)
                {
                    dgvIssueLinesGrid.Rows[i].Tag = sortInventoryIssueLinesDTOList[i].Quantity;
                    int uomId = sortInventoryIssueLinesDTOList[i].UOMId;
                    if (uomId == -1)
                    {
                        if (ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).InventoryUOMId != -1)
                        {
                            uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).InventoryUOMId;
                        }
                        else
                        {
                            uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).UomId;
                        }
                    }
                    CommonFuncs.GetUOMComboboxForSelectedRows(dgvIssueLinesGrid, i, uomId);
                    dgvIssueLinesGrid["txtUOM", i].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
                }
            }
            log.LogMethodExit();
        }
        private bool ValidateForm(string code)
        {
            log.LogMethodEntry(code);
            SortableBindingList<InventoryIssueLinesDTO> inventoryIssueLinesListOnDisplay = new SortableBindingList<InventoryIssueLinesDTO>();
            BindingSource inventoryIssueLinesBS = (BindingSource)dgvIssueLinesGrid.DataSource;
            if (inventoryIssueLinesBS != null)
            {
                inventoryIssueLinesListOnDisplay = (SortableBindingList<InventoryIssueLinesDTO>)inventoryIssueLinesBS.DataSource;
            }

            if (cmbIssueType.SelectedValue == null || (int)cmbIssueType.SelectedValue < 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1075));
                return false;
            }
            if (string.IsNullOrEmpty(txtIssueDate.Text))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1076));
                log.LogMethodExit(false);
                return false;
            }
            else
            {
                try
                {
                    DateTime.ParseExact(txtIssueDate.Text, "dd-MMM-yyyy", CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1077));
                    log.LogMethodExit(false);
                    return false;
                }
            }
            if (cmbStatus.SelectedValue == null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(980));
                log.LogMethodExit(false);
                return false;
            }
            else if (cmbStatus.SelectedValue.ToString().Equals("COMPLETE") || cmbStatus.SelectedValue.ToString().Equals("CANCELLED") || cmbStatus.SelectedValue.ToString().Equals("APPROVED"))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("Invalid status"));
                log.LogMethodExit(false);
                return false;
            }
            for (int i = 0; i < inventoryIssueLinesListOnDisplay.Count; i++)
            {
                if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Any())
                {
                    int userEnteredUOMId = Convert.ToInt32(dgvIssueLinesGrid.Rows[i].Cells["cmbUOM"].Value);
                    int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == inventoryIssueLinesListOnDisplay[i].ProductId).InventoryUOMId;
                    if (userEnteredUOMId != inventoryUOMId)
                    {
                        double factor = UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryUOMId);
                        inventoryIssueLinesListOnDisplay[i].Quantity = inventoryIssueLinesListOnDisplay[i].Quantity * factor;
                    }
                }
            }
            if (code.Equals("DIIS"))
            {
                if (string.IsNullOrEmpty(txtPONumber.Text))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1078));
                    log.LogMethodExit(false);
                    return false;
                }
                if (!string.IsNullOrEmpty(txtSupInvDate.Text))
                {
                    try
                    {
                        DateTime.Parse(txtSupInvDate.Text);
                    }
                    catch
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1079));
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                if (!string.IsNullOrEmpty(txtDeliveryDate.Text))
                {
                    try
                    {
                        DateTime.Parse(txtDeliveryDate.Text);
                    }
                    catch
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1080));
                        log.LogMethodExit(false);
                        return false;
                    }
                }
            }
            if ((inventoryIssueLinesListOnDisplay == null) || (inventoryIssueLinesListOnDisplay != null && inventoryIssueLinesListOnDisplay.Count == 0))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
                log.LogMethodExit(false);
                return false;
            }
            else if (inventoryIssueLinesListOnDisplay != null && inventoryIssueLinesListOnDisplay.Count > 0)
            {
                List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = inventoryIssueLinesListOnDisplay.Where(x => (bool)(x.Quantity == 0 && x.IssueLineId == -1)).ToList<InventoryIssueLinesDTO>();
                if (inventoryIssueLinesDTOList != null && inventoryIssueLinesDTOList.Count == inventoryIssueLinesListOnDisplay.Count)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                    log.LogMethodExit(false);
                    return false;
                }
            }
            if (!code.Equals("DIIS"))
            {

                foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesListOnDisplay)
                {
                    if (inventoryIssueLinesDTO.AvailableQuantity == 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(479));
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (inventoryIssueLinesDTO.AvailableQuantity < inventoryIssueLinesDTO.Quantity)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1084));
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (inventoryIssueLinesDTO.FromLocationID == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1067));
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (inventoryIssueLinesDTO.ToLocationID == -1 && !code.Equals("ITIS"))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1068));
                        log.LogMethodExit(false);
                        return false;
                    }
                    if (inventoryIssueLinesDTO.ToLocationID == inventoryIssueLinesDTO.FromLocationID)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1085));
                        log.LogMethodExit(false);
                        return false;
                    }
                }

            }
            if (code.Equals("DIIS") || code.Equals("REIS"))
            {
                if (inventoryIssueLinesListOnDisplay != null)
                {
                    for (int i = 0; i < inventoryIssueLinesListOnDisplay.Count; i++)
                    {
                        if ((double)dgvIssueLinesGrid.Rows[i].Tag < inventoryIssueLinesListOnDisplay[i].Quantity)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(1081, inventoryIssueLinesListOnDisplay[i].ProductName, dgvIssueLinesGrid.Rows[i].Tag));
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }
            }
            if (inventoryIssueLinesListOnDisplay == null || inventoryIssueLinesListOnDisplay.Count == 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1082));
                log.LogMethodExit(false);
                return false;
            }
            if (code.Equals("ITIS") && (cmbToSite.SelectedValue == null || (cmbToSite.SelectedValue != null && cmbToSite.SelectedValue.Equals(-1))))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1186));
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        private void dtpIssueDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtIssueDate.Text = dtpIssueDate.Value.ToString("yyyy-MM-dd");//utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dtpPODate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtPODate.Text = dtpPODate.Value.ToString("yyyy-MM-dd");//utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dtpSupInvDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtSupInvDate.Text = dtpSupInvDate.Value.ToString("yyyy-MM-dd");//utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dtpDeliveryDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtDeliveryDate.Text = dtpDeliveryDate.Value.ToString("dd-MMM-yyyy");//utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((cmbFilterIssueType.SelectedValue == null) ? -1 : (int)cmbFilterIssueType.SelectedValue))).ToList<InventoryDocumentTypeDTO>()[0].Code;
            if (!string.IsNullOrEmpty(code))
            {
                switch (code)
                {
                    case "AJIS":
                        LoadProduct(txtFilterPOReqNo.Text);
                        break;
                    case "DIIS":
                        LoadPurchaseOrder(txtFilterPOReqNo.Text);
                        break;
                    case "REIS":
                        LoadRequistition(txtFilterPOReqNo.Text);
                        break;
                    case "STIS":
                        LoadProduct(txtFilterPOReqNo.Text);
                        break;
                    case "ITIS":
                        LoadProduct(txtFilterPOReqNo.Text, true);
                        break;
                }
            }
            log.LogMethodExit();
        }

        private void txtPONumber_Leave(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!string.IsNullOrEmpty(txtPONumber.Text))
            {
                inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.CODE, "CPPO"));
                inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);
                if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
                {
                    purchaseOrderSearchParams = new List<KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>>();
                    purchaseOrderSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    purchaseOrderSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.DOCUMENT_TYPE_ID, inventoryDocumentTypeDTOList[0].DocumentTypeId.ToString()));
                    purchaseOrderSearchParams.Add(new KeyValuePair<PurchaseOrderDTO.SearchByPurchaseOrderParameters, string>(PurchaseOrderDTO.SearchByPurchaseOrderParameters.ORDERNUMBER, txtPONumber.Text));
                    purchaseOrderListOnDisplay = purchaseOrderList.GetAllPurchaseOrder(purchaseOrderSearchParams, machineUserContext.GetUserId());
                    if (purchaseOrderListOnDisplay != null && purchaseOrderListOnDisplay.Count > 0)
                    {
                        LoadPODetails(purchaseOrderListOnDisplay[0]);
                        LoadIssueDetails(-1, -1, "DIIS", purchaseOrderListOnDisplay[0].PurchaseOrderId);
                    }
                }
            }
            log.LogMethodExit();
        }

        void LoadVendor(int VendorId)
        {
            log.LogMethodEntry(VendorId);
            List<VendorDTO> vendorDTOList = new List<VendorDTO>();
            Vendor.VendorList vendorList = new VendorList(machineUserContext);
            List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>> searchByVendorParameters = new List<KeyValuePair<VendorDTO.SearchByVendorParameters, string>>();
            searchByVendorParameters.Add(new KeyValuePair<VendorDTO.SearchByVendorParameters, string>(VendorDTO.SearchByVendorParameters.VENDOR_ID, VendorId.ToString()));
            vendorDTOList = vendorList.GetAllVendors(searchByVendorParameters);
            if (vendorDTOList != null && vendorDTOList.Count > 0)
            {
                txtVendor.Text = vendorDTOList[0].Name;
                txtVendorCode.Text = vendorDTOList[0].VendorCode;
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            cmbIssueType.Enabled = cmbFilterIssueType.Enabled = true;
            clearAll();
            LoadIssue();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadAll();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (cmbStatus.SelectedValue == null)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(980));
                    return;
                }
                else if (cmbStatus.SelectedValue.ToString().Equals("OPEN"))
                {
                    if (this.dgvIssueLinesGrid.SelectedRows.Count <= 0 && this.dgvIssueLinesGrid.SelectedCells.Count <= 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(959));
                        log.LogMethodExit("Ends-InventoryIssueDeleteBtn_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                        return;
                    }
                    bool rowsDeleted = false;
                    bool confirmDelete = false;
                    if (this.dgvIssueLinesGrid.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in this.dgvIssueLinesGrid.SelectedCells)
                        {
                            dgvIssueLinesGrid.Rows[cell.RowIndex].Selected = true;
                        }
                    }
                    foreach (DataGridViewRow InventoryIssueRow in this.dgvIssueLinesGrid.SelectedRows)
                    {
                        if (InventoryIssueRow.Cells[0].Value != null)
                        {
                            if (Convert.ToInt32(InventoryIssueRow.Cells[0].Value) <= 0)
                            {
                                dgvIssueLinesGrid.Rows.RemoveAt(InventoryIssueRow.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    BindingSource inventoryIssueLineDTOListDTOBS = (BindingSource)dgvIssueLinesGrid.DataSource;
                                    var inventoryIssueLineDTOList = (SortableBindingList<InventoryIssueLinesDTO>)inventoryIssueLineDTOListDTOBS.DataSource;
                                    InventoryIssueLinesDTO inventoryIssueLineDTO = inventoryIssueLineDTOList[InventoryIssueRow.Index];
                                    inventoryIssueLineDTO.IsActive = false;
                                    InventoryIssueLines inventoryIssueLines = new InventoryIssueLines(inventoryIssueLineDTO, machineUserContext);
                                    inventoryIssueLines.Save(null);//TODO: Based on the location from stock need to be adjusted 
                                }
                            }
                        }
                    }
                    BindingSource inventoryIssueLinesDTOListDTOBS = (BindingSource)dgvIssueLinesGrid.DataSource;
                    var inventoryLinesDTOList = (SortableBindingList<InventoryIssueLinesDTO>)inventoryIssueLinesDTOListDTOBS.DataSource;
                    inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
                    foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryLinesDTOList)
                    {
                        inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                    }
                    if (rowsDeleted == true)
                        MessageBox.Show(utilities.MessageUtils.getMessage(957));
                }
                else if (cmbStatus.SelectedValue.ToString().Equals("SUBMITTED"))
                {
                    try
                    {
                        if (pnlIssue.Tag != null)
                        {
                            string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((cmbIssueType.SelectedValue == null) ? -1 : (int)cmbIssueType.SelectedValue))).ToList<InventoryDocumentTypeDTO>()[0].Code;
                            InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(Convert.ToInt32(pnlIssue.Tag), machineUserContext, null);
                            ApprovalRuleDTO approvalRuleDTO = new ApprovalRuleDTO();
                            ApprovalRule approvalRule = new ApprovalRule(machineUserContext, approvalRuleDTO);
                            approvalRuleDTO = approvalRule.GetApprovalRule(utilities.ParafaitEnv.RoleId, inventoryIssueHeader.getInventoryIssueHeaderDTO.DocumentTypeId, machineUserContext.GetSiteId());
                            if (approvalRuleDTO != null)
                            {
                                UserMessagesList userMessagesList = new UserMessagesList();
                                List<UserMessagesDTO> userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage("Inventory", code, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid, -1, -1, machineUserContext.GetSiteId(), null);
                                if (userMessagesDTOList != null && userMessagesDTOList.Count == approvalRuleDTO.NumberOfApprovalLevels && approvalRuleDTO.NumberOfApprovalLevels != 0)
                                {
                                    using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                                    {
                                        try
                                        {
                                            parafaitDBTransaction.BeginTransaction();
                                            foreach (UserMessagesDTO userMessagesDTO in userMessagesDTOList)
                                            {
                                                userMessagesDTO.IsActive = false;
                                                UserMessages userMessages = new UserMessages(userMessagesDTO, machineUserContext);
                                                userMessages.Save(parafaitDBTransaction.SQLTrx);
                                            }
                                            inventoryIssueHeader.getInventoryIssueHeaderDTO.Status = "CANCELLED";
                                            InventoryIssueHeader inventoryIssueHeadertosave = new InventoryIssueHeader(inventoryIssueHeader.getInventoryIssueHeaderDTO, machineUserContext);
                                            inventoryIssueHeadertosave.Save(parafaitDBTransaction.SQLTrx);
                                            parafaitDBTransaction.EndTransaction();
                                        }
                                        catch (Exception ex1)
                                        {
                                            parafaitDBTransaction.RollBack();
                                            log.Fatal("InventoryIssueDeleteBtn_Click() event.Failed to delete the issue. Exception:" + ex1.ToString());
                                            throw ex1;
                                        }
                                    }

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1540) + " Exception:" + ex.Message);
                        log.Fatal("InventoryIssueDeleteBtn_Click() event.Failed to delete the issue. Exception:" + ex.ToString());
                    }
                }
                else
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1541));
                    log.Fatal("InventoryIssueDeleteBtn_Click() event.Unable to delete this issue.");
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-InventoryIssueDeleteBtn_Click() event with exception: " + ex.ToString());
                MessageBox.Show(utilities.MessageUtils.getMessage(1083) + "\n Error: " + ex.Message);
            }
        }
        private void LoadLocation(string locationType, bool IsFromLocation)
        {
            log.Error("Starts-LoadLocation(" + locationType + "," + IsFromLocation + ") method ");
            LocationList locationList = new LocationList(machineUserContext);
            List<LocationDTO> locationDTOList = new List<LocationDTO>();
            locationDTOList = locationList.GetAllLocations(locationType);
            if (locationDTOList == null)
            {
                locationDTOList = new List<LocationDTO>();
            }
            locationDTOList.Insert(0, new LocationDTO());
            locationDTOList[0].Name = "<Select>";
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = locationDTOList;
            if (IsFromLocation)
            {
                fromLocationIDDataGridViewTextBoxColumn.DataSource = bindingSource;
                fromLocationIDDataGridViewTextBoxColumn.DisplayMember = "Name";
                fromLocationIDDataGridViewTextBoxColumn.ValueMember = "LocationId";
            }
            else
            {
                toLocationIDDataGridViewTextBoxColumn.DataSource = bindingSource;
                toLocationIDDataGridViewTextBoxColumn.DisplayMember = "Name";
                toLocationIDDataGridViewTextBoxColumn.ValueMember = "LocationId";
            }
            log.Error("Ends-LoadLocation(" + locationType + "," + IsFromLocation + ") method ");
        }


        private void dgvIssueLinesGrid_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (inventoryIssueLinesDTOList != null)
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvIssueLinesGrid.Columns[e.ColumnIndex].Name.Equals("fromLocationIDDataGridViewTextBoxColumn"))
                    {
                        dgvIssueLinesGrid.Rows[e.RowIndex].Cells["availableQuantityDataGridViewTextBoxColumn"].Value = GetProductQauntity((int)dgvIssueLinesGrid.Rows[e.RowIndex].Cells["fromLocationIDDataGridViewTextBoxColumn"].Value, inventoryIssueLinesDTOList[e.RowIndex].ProductId);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void lnkRemarks_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            frmInventoryNotes frmInventoryNotes = new frmInventoryNotes(inventoryIssueHeaderDTOonsave.InventoryIssueId, "Inventory Issue", utilities);
            frmInventoryNotes.ShowDialog();
            log.LogMethodExit();
        }

        private void dgvIssueLinesGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //MessageBox.Show(dgvIssueLinesGrid.Columns[e.ColumnIndex].HeaderText + " = " + dgvIssueLinesGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
        }

        private void btnIssueSearch_Click(object sender, EventArgs e)
        {
            LoadIssue();
        }

        private void LoadIssue()
        {
            log.LogMethodEntry();
            InventoryIssueHeaderList inventoryIssueHeaderList = new InventoryIssueHeaderList();
            List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = new List<InventoryIssueHeaderDTO>();
            List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchByInventoryIssueHeaderParameters = new List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>();
            searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG, "1"));
            searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            if (!string.IsNullOrEmpty(txtIssueNumberSearch.Text))
            {
                searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_NUMBER, txtIssueNumberSearch.Text.ToString()));
                txtIssueNumberSearch.Text = "";
            }

            else
            {
                if (!string.IsNullOrEmpty(txtFromDate.Text))
                {
                    try
                    {
                        searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_FROM_DATE, DateTime.Parse(txtFromDate.Text).ToString("yyyy-MM-dd")));
                    }
                    catch
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(15), "Invalid From Date");
                    }
                }
                if (!string.IsNullOrEmpty(txtToDate.Text))
                {
                    try
                    {
                        searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ISSUE_TO_DATE, DateTime.Parse(txtToDate.Text).AddDays(1).ToString("yyyy-MM-dd")));
                    }
                    catch { MessageBox.Show(utilities.MessageUtils.getMessage(15), "Invalid To Date"); }
                }
            }
            inventoryIssueHeaderDTOList = inventoryIssueHeaderList.GetAllInventoryIssueHeaders(searchByInventoryIssueHeaderParameters);
            BindingSource inventoryIssueSearchBS = new BindingSource();
            if (inventoryIssueHeaderDTOList == null)
            {
                inventoryIssueSearchBS.DataSource = new SortableBindingList<InventoryIssueHeaderDTO>();
            }
            else
            {
                inventoryIssueSearchBS.DataSource = new SortableBindingList<InventoryIssueHeaderDTO>(inventoryIssueHeaderDTOList);
            }
            dgvIssueSearch.DataSource = inventoryIssueSearchBS;
            log.LogMethodExit();
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtFromDate.Text = dtpFromDate.Value.ToString("dd-MMM-yyyy");//utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dtpTodate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtToDate.Text = dtpTodate.Value.ToString("dd-MMM-yyyy");//utilities.getDateFormat());
            log.LogMethodExit();
        }

        private void dgvIssueSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadIssue(Convert.ToInt32(dgvIssueSearch.Rows[e.RowIndex].Cells["inventoryIssueIdDataGridViewTextBoxColumn"].Value));
            log.LogMethodExit();
        }
        private void LoadIssue(string guid)
        {
            log.LogMethodEntry(guid);
            InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(issueGuid, machineUserContext);
            LoadIssue(inventoryIssueHeader.getInventoryIssueHeaderDTO, true);
            log.LogMethodExit();
        }
        private void LoadIssue(InventoryIssueHeaderDTO inventoryIssueHeaderDTO, bool isInboxView = false)
        {
            log.LogMethodEntry(inventoryIssueHeaderDTO, isInboxView);
            if (inventoryIssueHeaderDTO != null)
            {
                if (inventoryIssueHeaderDTO.FromSiteID != -1 && inventoryIssueHeaderDTO.ToSiteID != -1 && inventoryIssueHeaderDTO.FromSiteID != utilities.ParafaitEnv.SiteId && inventoryIssueHeaderDTO.ToSiteID != utilities.ParafaitEnv.SiteId)
                {
                    InventoryDocumentTypeList inventoryDocumentType = new InventoryDocumentTypeList(machineUserContext);
                    InventoryDocumentTypeDTO inventoryDocumentTypeDto = inventoryDocumentType.GetInventoryDocumentType(inventoryIssueHeaderDTO.DocumentTypeId);
                    if (inventoryDocumentTypeDto != null)
                    {
                        cmbIssueType.SelectedValue = inventoryDocumentTypeDto.MasterEntityId;
                    }
                }
                else
                {
                    cmbIssueType.SelectedValue = inventoryIssueHeaderDTO.DocumentTypeId;
                }
                cmbIssueType.Tag = inventoryIssueHeaderDTO.RequisitionID;
                cmbStatus.SelectedValue = string.IsNullOrEmpty(inventoryIssueHeaderDTO.Status) ? "OPEN" : inventoryIssueHeaderDTO.Status;
                if (cmbStatus.SelectedValue != null && (cmbStatus.SelectedValue.Equals("OPEN") || cmbStatus.SelectedValue.Equals("CANCELLED")))
                {
                    EnableDisableControl(true);
                    txtReferenceNumber.ReadOnly = false;
                }
                else
                {
                    EnableDisableControl(false);
                    txtReferenceNumber.ReadOnly = true;
                    btnPrint.Enabled = true;               //Fix : For Printing existing Issue Report
                }
                if (inventoryIssueHeaderDTO.ToSiteID == utilities.ParafaitEnv.SiteId)
                {
                    cmbToSite.SelectedValue = inventoryIssueHeaderDTO.FromSiteID;
                    cmbToSite.Tag = "F";
                    lblFromsite.Text = inventoryIssueHeaderDTO.ToSiteID.ToString();
                    LoadLocation("Store", false);
                    LoadLocation("In Transit", true);
                    lblTosite.Text = utilities.MessageUtils.getMessage("From Site") + ":*";
                }
                else
                {
                    lblTosite.Text = utilities.MessageUtils.getMessage("To Site") + ":*";
                    if (inventoryIssueHeaderDTO.ToSiteID != -1)
                    {
                        cmbToSite.SelectedValue = inventoryIssueHeaderDTO.ToSiteID;
                        cmbToSite.Tag = "T";
                        lblFromsite.Text = utilities.MessageUtils.getMessage("From Site") + ": " + inventoryIssueHeaderDTO.FromSiteID.ToString();
                        lblFromsite.Visible = true;
                    }
                }

                if (!inventoryIssueHeaderDTO.DeliveryNoteDate.Equals(DateTime.MinValue))
                {
                    txtDeliveryDate.Text = inventoryIssueHeaderDTO.DeliveryNoteDate.ToString("dd-MMM-yyyy");
                }
                txtDeliveryNoteNo.Text = inventoryIssueHeaderDTO.DeliveryNoteNumber;
                txtIssueDate.Text = inventoryIssueHeaderDTO.IssueDate.ToString("dd-MMM-yyyy");
                txtPONumber.Text = inventoryIssueHeaderDTO.PurchaseOrderId.ToString();
                txtSupInvDate.Text = inventoryIssueHeaderDTO.SupplierInvoiceDate.ToString("dd-MMM-yyyy");
                txtSupInvoiceNo.Text = inventoryIssueHeaderDTO.SupplierInvoiceNumber;
                txtReferenceNumber.Text = inventoryIssueHeaderDTO.ReferenceNumber.ToString();
                txtIssueNumber.Text = inventoryIssueHeaderDTO.IssueNumber.ToString();
                PurchaseOrderDTO purchaseOrderDTO = new PurchaseOrderDTO();
                if (inventoryIssueHeaderDTO.PurchaseOrderId != -1)
                {
                    PurchaseOrderList purchaseOrderList = new PurchaseOrderList(machineUserContext);
                    purchaseOrderDTO = purchaseOrderList.GetPurchaseOrder(inventoryIssueHeaderDTO.PurchaseOrderId, machineUserContext);
                    LoadPODetails(purchaseOrderDTO);
                    txtPONumber.Tag = purchaseOrderDTO.PurchaseOrderId;
                }
                PopulateLine(inventoryIssueHeaderDTO.InventoryIssueId);
                pnlIssue.Tag = inventoryIssueHeaderDTO.InventoryIssueId;

                if (utilities.ParafaitEnv.IsMasterSite && utilities.ParafaitEnv.IsCorporate)
                {
                    if (inventoryIssueHeaderDTO.FromSiteID != utilities.ParafaitEnv.SiteId || (utilities.ParafaitEnv.IsCorporate && inventoryIssueHeaderDTO.FromSiteID == utilities.ParafaitEnv.SiteId && cmbStatus.SelectedValue.Equals("SUBMITTED")))
                    {
                        btnDelete.Enabled = btnNew.Enabled = btnRefresh.Enabled = btnSave.Enabled = btnPrint.Enabled = false;
                        gbIssueSearch.Enabled = pnlIssueMainPanel.Enabled = pnlIssueSearch.Enabled = txtReferenceNumber.Enabled = false;
                    }
                    else if (inventoryIssueHeaderDTO.FromSiteID == utilities.ParafaitEnv.SiteId && (inventoryIssueHeaderDTO.Status.Equals("OPEN") || inventoryIssueHeaderDTO.Status.Equals("CANCELLED")))
                    {
                        btnDelete.Enabled = btnNew.Enabled = btnRefresh.Enabled = btnSave.Enabled = btnPrint.Enabled = true;
                        gbIssueSearch.Enabled = pnlIssueMainPanel.Enabled = pnlIssueSearch.Enabled = txtReferenceNumber.Enabled = true;
                    }

                }
                else if (inventoryIssueHeaderDTO.ToSiteID == utilities.ParafaitEnv.SiteId)
                {
                    if (isInboxView)
                    {
                        btnDelete.Enabled = btnNew.Enabled = btnRefresh.Enabled = false;
                        if ((inventoryIssueHeaderDTO.Status.Equals("OPEN") || inventoryIssueHeaderDTO.Status.Equals("CANCELLED")))
                        {
                            EnableDisableControl(false);
                            cmbStatus.SelectedValue = "SUBMITTED";
                            btnSave.Enabled = true;
                            txtReferenceNumber.Enabled = true;
                        }
                        gbIssueSearch.Enabled = pnlIssueSearch.Enabled = false;
                        pnlIssueMainPanel.Enabled = true;
                        dgvIssueLinesGrid.ReadOnly = false;
                        toLocationIDDataGridViewTextBoxColumn.Visible = true;
                        toLocationIDDataGridViewTextBoxColumn.ReadOnly = false;
                        fromLocationIDDataGridViewTextBoxColumn.Visible = false;
                        //fromLocationIDDataGridViewTextBoxColumn.ReadOnly = true;
                        availableQuantityDataGridViewTextBoxColumn.Visible = false;
                        quantityDataGridViewTextBoxColumn.ReadOnly = true;
                        productNameDataGridViewTextBoxColumn.ReadOnly = true;
                        issueLineIdDataGridViewTextBoxColumn.ReadOnly = true;
                        isActiveDataGridViewCheckBoxColumn.ReadOnly = true;
                    }
                    else
                    {
                        btnSave.Enabled = btnDelete.Enabled = btnNew.Enabled = btnRefresh.Enabled = btnPrint.Enabled = false;
                        gbIssueSearch.Enabled = pnlIssueSearch.Enabled = false;
                        pnlIssueMainPanel.Enabled = false;
                        dgvIssueLinesGrid.ReadOnly = true;
                    }
                }
                else if (cmbStatus.SelectedValue.Equals("SUBMITTED") && (inventoryIssueHeaderDTO.ToSiteID != utilities.ParafaitEnv.SiteId))
                {
                    btnSave.Enabled = false;
                    txtReferenceNumber.Enabled = false;
                    if (isInboxView)
                    {
                        btnRefresh.Enabled = btnDelete.Enabled = btnNew.Enabled = false;
                        gbIssueSearch.Enabled = pnlIssueSearch.Enabled = false;
                        pnlIssueMainPanel.Enabled = false;
                        dgvIssueLinesGrid.ReadOnly = true;
                    }
                    else
                    {
                        btnRefresh.Enabled = btnDelete.Enabled = btnNew.Enabled = true;
                        gbIssueSearch.Enabled = pnlIssueSearch.Enabled = true;
                        pnlIssueMainPanel.Enabled = true;
                        dgvIssueLinesGrid.ReadOnly = true;
                    }
                }
                else
                {
                    if (isInboxView)
                    {
                        btnRefresh.Enabled = btnDelete.Enabled = btnNew.Enabled = btnPrint.Enabled = false;
                        gbIssueSearch.Enabled = pnlIssueSearch.Enabled = false;
                        pnlIssueMainPanel.Enabled = false;
                        dgvIssueLinesGrid.ReadOnly = true;
                        if (!string.IsNullOrEmpty(inventoryIssueHeaderDTO.Status) && !inventoryIssueHeaderDTO.Status.Equals("CANCELLED") && !inventoryIssueHeaderDTO.Status.Equals("COMPLETE"))
                        {
                            btnSave.Enabled = true;
                            txtReferenceNumber.Enabled = true;
                        }
                        else
                        {
                            btnSave.Enabled = false;
                            txtReferenceNumber.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Nothing to load!.");
            }
            log.LogMethodExit();
        }
        private void LoadIssue(int issueId)
        {
            log.LogMethodEntry(issueId);

            inventoryIssueId = issueId;   // This issueId is used for Printing existing Issue when click the dgvIssueSearch grid.

            InventoryIssueHeaderList inventoryIssueHeaderList = new InventoryIssueHeaderList();
            InventoryIssueHeaderDTO inventoryIssueHeaderDTO = new InventoryIssueHeaderDTO();
            inventoryIssueHeaderDTO = inventoryIssueHeaderList.GetIssueHeader(issueId);
            LoadIssue(inventoryIssueHeaderDTO);
            log.LogMethodExit();
        }

        private void EnableDisableControl(bool isEnable)
        {
            cmbStatus.Enabled =
            cmbToSite.Enabled =
            dtpPODate.Enabled =
            dtpSupInvDate.Enabled =
            dtpIssueDate.Enabled =
            btnSave.Enabled = isEnable;
            btnPrint.Enabled = isEnable;
            dgvIssueLinesGrid.ReadOnly =
            txtDeliveryNoteNo.ReadOnly =
            txtIssueDate.ReadOnly =
            txtPONumber.ReadOnly =
            txtSupInvDate.ReadOnly =
            txtSupInvoiceNo.ReadOnly = !isEnable;
            availableQuantityDataGridViewTextBoxColumn.ReadOnly = true;
        }

        private void lnkRequisition_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ProductDTO productDTO;
            frmListRequisitions pr = new frmListRequisitions(utilities, "ISSUE", ((cmbToSite.SelectedValue == null) ? -1 : Convert.ToInt32(cmbToSite.SelectedValue)));
            CommonUIDisplay.setupVisuals(pr);
            pr.StartPosition = FormStartPosition.CenterScreen;
            if (pr.SelectedRequisitionId != -1 || pr.ShowDialog() != DialogResult.Cancel)
            {
                RequisitionDTO requisitionDTO = pr.SelectedRequisitionDTO;
                InventoryIssueLinesDTO inventoryIssueLinesDTO;
                if (requisitionDTO != null)
                {
                    if (!inventoryIssueLinesDTOList.Exists(x => (bool)(x.RequisitionID > -1 && x.IsActive)))
                    {
                        if (cmbToSite.SelectedValue != null && Convert.ToInt32(cmbToSite.SelectedValue) > -1 && Convert.ToInt32(cmbToSite.SelectedValue) != requisitionDTO.FromSiteId && MessageBox.Show("The selected requisition is of different site. \nThe Tosite will be updated with requisition site. \nWould you like to continue?. ", "Different Site Requisition Selected.", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            log.LogMethodExit("Different Site Requisition Selected.");
                            return;
                        }
                        cmbToSite.SelectedValue = requisitionDTO.FromSiteId;
                        cmbToSite.Enabled = false;
                        //LoadIssueDetails(requisitionDTO.FromDepartment, requisitionDTO.ToDepartment, "ITIS", requisitionDTO.RequisitionId);
                        if (inventoryIssueLinesDTOList == null)
                            inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();


                        List<RequisitionLinesDTO> requisitionLinesDTOList = pr.SelectedRequisitionLineDTO;

                        if (requisitionLinesDTOList != null && requisitionLinesDTOList.Count > 0)
                        {
                            foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesDTOList)
                            {
                                log.LogVariableState("requisitionLinesDTO :", requisitionLinesDTO);
                                productDTO = GetProductDTO(requisitionLinesDTO.ProductId);
                                log.Debug("requisitionLinesDTO.UOMId : " + requisitionLinesDTO.UOMId);
                                log.Debug("productDTO.InventoryUOMId : " + productDTO.InventoryUOMId);
                                log.Debug("requisitionDTO.ToSiteId : " + requisitionDTO.ToSiteId);
                                inventoryIssueLinesDTO = new InventoryIssueLinesDTO(requisitionLinesDTO.ProductId, productDTO.Description, (requisitionLinesDTO.RequestedQuantity - requisitionLinesDTO.ApprovedQuantity), 0, ((productDTO.LastPurchasePrice == 0) ? productDTO.Cost : productDTO.LastPurchasePrice), -1, -1, requisitionLinesDTO.RequisitionId, requisitionLinesDTO.RequisitionLineId, requisitionLinesDTO.UOMId);
                                inventoryIssueLinesDTOList.Add(inventoryIssueLinesDTO);
                                log.LogVariableState("After update requisitionLinesDTO :", requisitionLinesDTO);
                            }
                        }
                        cmbIssueType.Tag = requisitionDTO.RequisitionId;
                        SortableBindingList<InventoryIssueLinesDTO> sortInventoryIssueLinesDTOList = new SortableBindingList<InventoryIssueLinesDTO>();
                        if (inventoryIssueLinesDTOList == null)
                        {
                            sortInventoryIssueLinesDTOList = new SortableBindingList<InventoryIssueLinesDTO>();
                        }
                        else
                        {
                            sortInventoryIssueLinesDTOList = new SortableBindingList<InventoryIssueLinesDTO>(inventoryIssueLinesDTOList);
                        }
                        BindingSource RequisitionLineDTOListBS = new BindingSource();
                        RequisitionLineDTOListBS.DataSource = sortInventoryIssueLinesDTOList;
                        dgvIssueLinesGrid.DataSource = RequisitionLineDTOListBS;
                        if (sortInventoryIssueLinesDTOList != null)
                        {
                            for (int i = 0; i < sortInventoryIssueLinesDTOList.Count; i++)
                            {
                                dgvIssueLinesGrid.Rows[i].Tag = sortInventoryIssueLinesDTOList[i].Quantity;

                                int uomId = sortInventoryIssueLinesDTOList[i].UOMId;
                                if (uomId == -1 && ProductContainer.productDTOList != null)
                                {
                                    if (ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).InventoryUOMId != -1)
                                    {
                                        uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).InventoryUOMId;
                                    }
                                    else
                                    {
                                        uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).UomId;
                                    }
                                }
                                CommonFuncs.GetUOMComboboxForSelectedRows(dgvIssueLinesGrid, i, uomId);
                                dgvIssueLinesGrid.Rows[i].Cells["txtUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;

                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Issuing multiple requistion in single issue is not possible.");
                        log.LogMethodExit("Issuing multiple requistion in single issue is not possible.");
                        return;
                    }
                }
            }
            log.LogMethodExit();
        }

        // Print Option been added to InventoryIssueUI as enhancement on 11-04-2019.
        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (inventoryIssueId != -1)
                {
                    string reportFileName = GetInventoryIssueReport();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(utilities.MessageUtils.getMessage(1819, ex.Message), "ERROR");//Error while printing the document. Error message: &1
            }
            log.LogMethodExit();
        }

        private string GetInventoryIssueReport()
        {
            log.LogMethodEntry();
            List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
            reportParam.Add(new clsReportParameters.SelectedParameterValue("IssueId", inventoryIssueId));
            ReceiptReports receiptReports = new ReceiptReports(machineUserContext, "InventoryIssueReceipt", "", null, null, reportParam, "P");
            string reportFileName = receiptReports.GenerateAndPrintReport(false);
            log.LogMethodExit(reportFileName);
            return reportFileName;
        }

        private bool SetupThePrinting(PrintDocument MyPrintDocument)
        {
            log.LogMethodEntry(MyPrintDocument);
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;
            MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = true;
            MyPrintDocument.DocumentName = utilities.MessageUtils.getMessage("InventoryIssueReceipt" + "-" + inventoryIssueId);
            MyPrintDialog.UseEXDialog = true;
            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);
            log.LogMethodExit(true);
            return true;
        }

        private void dgvIssueLinesGrid_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex > -1)
                {
                    BindingSource inventoryIssueLineDTOListDTOBS = (BindingSource)dgvIssueLinesGrid.DataSource;
                    var inventoryIssueLineDTOList = (SortableBindingList<InventoryIssueLinesDTO>)inventoryIssueLineDTOListDTOBS.DataSource;

                    for (int i = 0; i < inventoryIssueLineDTOList.Count; i++)
                    {
                        int uomId = inventoryIssueLineDTOList[i].UOMId;
                        if (uomId == -1 && ProductContainer.productDTOList != null)
                        {
                            if (ProductContainer.productDTOList.Find(x => x.ProductId == inventoryIssueLineDTOList[i].ProductId).InventoryUOMId != -1)
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == inventoryIssueLineDTOList[i].ProductId).InventoryUOMId;
                            }
                            else
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == inventoryIssueLineDTOList[i].ProductId).UomId;
                            }
                        }
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvIssueLinesGrid, i, uomId);
                        dgvIssueLinesGrid.Rows[i].Cells["txtUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void LoadUOMComboBox()
        {
            log.LogMethodEntry();
            try
            {
                if (inventoryIssueLinesDTOList != null)
                {
                    for (int i = 0; i < inventoryIssueLinesDTOList.Count; i++)
                    {
                        int uomId = inventoryIssueLinesDTOList[i].UOMId;
                        if (uomId == -1 && ProductContainer.productDTOList != null)
                        {
                            if (ProductContainer.productDTOList.Find(x => x.ProductId == inventoryIssueLinesDTOList[i].ProductId).InventoryUOMId != -1)
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == inventoryIssueLinesDTOList[i].ProductId).InventoryUOMId;
                            }
                            else
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == inventoryIssueLinesDTOList[i].ProductId).UomId;
                            }
                        }
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvIssueLinesGrid, i, uomId);
                        dgvIssueLinesGrid.Rows[i].Cells["txtUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == uomId).UOM;

                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void InventoryIssueUI_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadUOMComboBox();
            log.LogMethodExit();
        }
    }
}
