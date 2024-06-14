
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.BarcodeUtilities;
using Semnox.Parafait.logging;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Add Barcode UI
    /// </summary>
    public partial class frmAddBarcode : Form
    {

        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        string scannedBarcode = "";
        int productID;
        string pCode;
        string pDescription;
        double pPrice;
        Utilities utilities;
        public ProductBarcodeDTO productBarcodeDTO = new ProductBarcodeDTO();
        /// <summary>
        /// Add Barcode Constructor
        /// </summary>
        public frmAddBarcode(int _ProductID, string code, string description)
        {
            InitializeComponent();
            productID = _ProductID;
            pCode = code;
            pDescription = description;
            productBarcode_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Add Barcode Constructor
        /// </summary>
        public frmAddBarcode(int _ProductID, string code, string description, double price, Utilities _Utilities)
        {
            InitializeComponent();
            productID = _ProductID;
            pCode = code;
            pDescription = description;
            pPrice = price;
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref productBarcode_dgv);
            utilities.setLanguage(this);
            productBarcode_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
        }

        private void PopulateAddBarcodeGrid()
        {
            log.Debug("Starts-PopulateProductBarcodeGrid() method.");
            ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(machineUserContext);
            List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> SearchParameter = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
            SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.PRODUCT_ID, productID.ToString()));
            List<ProductBarcodeDTO> productBarcodeListOnDisplay = productBarcodeListBL.GetProductBarcodeDTOList(SearchParameter);

            if (productBarcodeListOnDisplay != null)
            {
                SortableBindingList<ProductBarcodeDTO> productBarcodeDTOSortList = new SortableBindingList<ProductBarcodeDTO>(productBarcodeListOnDisplay);
                productBarcodeDTOListBS.DataSource = productBarcodeDTOSortList;
            }
            else
                productBarcodeDTOListBS.DataSource = new SortableBindingList<ProductBarcodeListBL>();


            productBarcode_dgv.DataSource = productBarcodeDTOListBS;
            lblHeading.Text = "Barcode for " + pCode + " - " + pDescription;
            log.Debug("Ends-PopulateProductBarcodeGrid() method.");
        }

        private void frm_addBarcode_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frm_addBarcode_Load() Event.");
            PopulateAddBarcodeGrid();
            log.Debug("Ends-frm_addBarcode_Load() Event.");
        }

        private bool txtBarcode_Validating()
        {
            log.Debug("Starts-txtBarcode_Validating() method");
            if (!string.IsNullOrEmpty(txtbarcode.Text))
            {
                //Check for duplicate barcode
                ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(machineUserContext);
                List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> SearchParameter = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
                SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.BARCODE, txtbarcode.Text));
                SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.IS_ACTIVE, "Y"));
                List<ProductBarcodeDTO> productBarcodeListOnDisplay = productBarcodeListBL.GetProductBarcodeDTOList(SearchParameter);

                if (productBarcodeListOnDisplay != null)
                {
                    if (lblBarcodeid.Text != productBarcodeListOnDisplay[0].Id.ToString())
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(904), utilities.MessageUtils.getMessage("Duplicate Bar Code"));
                        return false;
                    }
                }
                log.Debug("Ends-txtBarcode_Validating() method");
                return true;
            }
            else
            {
                log.Error("Ends - txtBarcode_Validating() method with Barcode cannot be null or empty");
                MessageBox.Show("Barcode cannot be null or empty", utilities.MessageUtils.getMessage("Empty Bar Code"));
                return false;
            }
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btn_new_Click() event");
            lblBarcodeid.Text = "";
            txtbarcode.Text = "";
            ddlactive.Text = "Y";
            txtbarcode.Focus();
            log.Debug("Ends-btn_new_Click() event");
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btn_save_Click() event.");
            if (txtbarcode.Text == "")
            {
                MessageBox.Show("Barcode cannot be empty.", utilities.MessageUtils.getMessage("Save Barcode"));
                txtbarcode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(ddlactive.Text))
            {
                MessageBox.Show("Please select the active status.", utilities.MessageUtils.getMessage("Save Barcode"));
                ddlactive.Focus();
                return;
            }

            if (lblBarcodeid.Text == "")
            {
                try
                {
                    //ProductBarcodeDTO productBarcodeDTO = new ProductBarcodeDTO();
                    ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(machineUserContext);
                    List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> SearchParameter = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
                    SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.BARCODE, txtbarcode.Text));
                    SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.IS_ACTIVE, ddlactive.Text));
                    List<ProductBarcodeDTO> productBarcodeListOnDisplay = productBarcodeListBL.GetProductBarcodeDTOList(SearchParameter);

                    if (productBarcodeListOnDisplay == null)
                    {
                        ProductBarcodeDTO productBarcodeDTO = new ProductBarcodeDTO();
                        productBarcodeDTO.BarCode = txtbarcode.Text;
                        productBarcodeDTO.IsActive = ddlactive.Text == "Y" ? true : false;
                        productBarcodeDTO.Product_Id = productID;
                        ProductBarcodeBL productcode = new ProductBarcodeBL(machineUserContext, productBarcodeDTO);
                        productcode.Save(null);
                        PopulateAddBarcodeGrid();
                    }
                    else
                    {
                        MessageBox.Show("Barcode already exists", utilities.MessageUtils.getMessage("Add Barcode"));
                        txtbarcode.Focus();
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(895) + exc.Message, utilities.MessageUtils.getMessage("Add Barcode"));
                }
            }
            else
            {
                try
                {
                    ////Verify if row present.                    
                    ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(machineUserContext);
                    List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> SearchParameter = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
                    SearchParameter.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.ID, lblBarcodeid.Text));
                    List<ProductBarcodeDTO> productBarcodeListOnDisplay = productBarcodeListBL.GetProductBarcodeDTOList(SearchParameter);

                    if (productBarcodeListOnDisplay.Count > 0)
                    {
                        if (!txtBarcode_Validating())
                        {
                            return;
                        }

                        productBarcodeDTO.BarCode = txtbarcode.Text;
                        productBarcodeDTO.IsActive = ddlactive.Text == "Y" ? true : false;
                        ProductBarcodeBL productcode = new ProductBarcodeBL(machineUserContext, productBarcodeDTO);
                        productcode.Save(null);
                        MessageBox.Show(utilities.MessageUtils.getMessage(1016), utilities.MessageUtils.getMessage("Update Product Barcode"));
                        PopulateAddBarcodeGrid();
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Barcode does not exist."), utilities.MessageUtils.getMessage("Update Product Barcode")); //Updated message 27-Apr-2016
                        txtbarcode.Focus();
                    }

                }
                catch (Exception exc)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(895) + exc.Message, utilities.MessageUtils.getMessage("Update Category"));
                }
            }
            productBarcode_dgv.Refresh();
            log.Debug("Ends-btn_save_Click() event.");
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-frm_addBarcodeCloseBtn_Click() event.");
            this.Close();
            log.Debug("Ends-frm_addBarcodeCloseBtn_Click() event.");
        }

        private void frm_addBarcode_Activated(object sender, EventArgs e)
        {
            log.Debug("Starts-frm_addBarcode_Activated() event");
            BarcodeReader.setReceiveAction = serialbarcodeDataReceived;
            txtbarcode.Focus();
            log.Debug("Ends-frm_addBarcode_Activated() event");
        }

        private void serialbarcodeDataReceived()
        {
            log.Debug("Starts-serialbarcodeDataReceived() method");
            scannedBarcode = BarcodeReader.Barcode;
            txtbarcode.Text = scannedBarcode;
            log.Debug("Ends-serialbarcodeDataReceived() method");
        }

        private void productBarcodeDTOListBS_CurrentChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-productBarcodeDTOListBS_CurrentChanged() event");
            if (productBarcodeDTOListBS.Current != null &&
                productBarcodeDTOListBS.Current is ProductBarcodeDTO)
            {
                productBarcodeDTO = productBarcodeDTOListBS.Current as ProductBarcodeDTO;
                lblBarcodeid.Text = productBarcodeDTO.Id.ToString();
                txtbarcode.Text = productBarcodeDTO.BarCode;
                ddlactive.Text = productBarcodeDTO.IsActive ? "Y" : "N";
            }
            log.Debug("Ends-productBarcodeDTOListBS_CurrentChanged() event");
        }

        private void btn_generateBarcode_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btn_generateBarcode_Click() event");
            if (pCode != "" && txtbarcode.Text.Trim() == "")
                txtbarcode.Text = pCode;

            // frm_barcode f = new frm_barcode(txtBarcode.Text == "" ? pCode : txtBarcode.Text);
            //Changed method to call frm_ProductBarcode instead of frm_barcode 
            frm_ProductBarcode f = new frm_ProductBarcode(txtbarcode.Text == "" ? pCode : txtbarcode.Text, pDescription, pPrice, utilities);
            Semnox.Parafait.Inventory.CommonUIDisplay.setupVisuals(f);//Added for style GUI

            f.StartPosition = FormStartPosition.CenterScreen;//Added for showing in center 
            if (f.ShowDialog() == DialogResult.OK)
                txtbarcode.Text = BarcodeReader.Barcode;
            log.Debug("Ends-btn_generateBarcode_Click() event");
        }
    }
}
