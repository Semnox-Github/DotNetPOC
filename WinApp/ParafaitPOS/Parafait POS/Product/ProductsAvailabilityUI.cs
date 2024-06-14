/********************************************************************************************
 * Project Name - Products Availability UI
 * Description  - Form to put products as temporarily unavailable
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.60        02-FEB-2019      Nitin Pai      86-68 Created 
 *2.70.2        18-Dec-2019     Jinto Thomas    Added parameter execution context for userbl declaration with userid 
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
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    public partial class ProductsAvailabilityUI : Form
    {
        private Utilities Utilities;
        public Users Manager;
        private MessageBoxDelegate messageBoxDelegate;

        const string WARNING = "WARNING";
        const string ERROR = "ERROR";
        const string MESSAGE = "MESSAGE";

        // to be replaced with DB messages
        private string ERROR_NO_ROWS_SELECTED = "No products are selected";
        private string AVL_SUCCESSFUL = "All products made available successfully";
        private string UPDATE_SUCCESS = "Products updated successfully";
        private string UPDATE_FAIL = "Product update failed, please try again";
        private string UNAVL_SUCCESSFUL = "All products made unavailable successfully";
        private string ACTION_SUCCESS = "Action completed successfully";

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        List<ProductDisplayGroupFormatDTO> displayFormatGroups;
        List<ProductsDisplayGroupDTO> productsByDisplayGroups;
        List<ProductsDisplayGroupDTO> productsOfExcludedDisplayGroups;

        List<ProductsAvailabilityDTO> availableProducts;
        List<ProductsAvailabilityDTO> unAvailableProducts;
        List<ProductsAvailabilityDTO> cbAvailableProductsDS;
        List<ProductsAvailabilityDTO> cbUnavailableProductsDS;

        ProductsAvailabilityDTO selectedProduct;

        private ProductsAvailabilityUI()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        public ProductsAvailabilityUI(Utilities utilities, int ManagerId, MessageBoxDelegate messageBoxDelegate)
            : this()
        {
            log.LogMethodEntry(utilities, ManagerId);

            this.Utilities = utilities;
            this.messageBoxDelegate = messageBoxDelegate;

            // Fetch and set all data required
            Manager = new Users(Utilities.ExecutionContext, ManagerId);

            ERROR_NO_ROWS_SELECTED = Utilities.MessageUtils.getMessage(2050);
            AVL_SUCCESSFUL = Utilities.MessageUtils.getMessage(2051);
            UPDATE_SUCCESS = Utilities.MessageUtils.getMessage(2052);
            UPDATE_FAIL = Utilities.MessageUtils.getMessage(2053); 
            UNAVL_SUCCESSFUL = Utilities.MessageUtils.getMessage(2054);
            ACTION_SUCCESS = Utilities.MessageUtils.getMessage(2055); 

            log.LogMethodExit();
        }

        private void ProductsAvailabilityUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            
            ProductsAvailabilityBL productsAvailabilityBL = new ProductsAvailabilityBL(Utilities.ExecutionContext);
            productsAvailabilityBL.UpdatedExpiredProductsToAvailable();

            // get all display groups
            GetEligibleDisplayGroupsForUser(Manager.UserDTO.LoginId);

            // fetch all products
            FetchProductsAndSetupScreen();

            Utilities.setLanguage(this);

            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            log.LogMethodExit();
        }

        private void btnMakeUnavailable_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            List<ProductsAvailabilityDTO> selectedProductList = new List<ProductsAvailabilityDTO>();
            DataGridViewRowCollection rows = dgvAvailableProducts.Rows;
            StringBuilder products = new StringBuilder();
            for (int i = 0; i < rows.Count; i++)
            {
                if (dgvAvailableProducts.Rows[i].Cells[0].Value != null &&
                    Convert.ToBoolean(dgvAvailableProducts.Rows[i].Cells[0].Value))
                {
                    selectedProductList.Add(((List<ProductsAvailabilityDTO>)((System.Windows.Forms.BindingSource)dgvAvailableProducts.DataSource).DataSource).ElementAt(i));
                    if (products.Length > 0) { products.Append(" ,"); }
                    products.Append(((ProductsAvailabilityDTO)selectedProductList.Last()).ProductName);
                }
            }

            if (selectedProductList.Count == 0)
            {
                messageBoxDelegate(ERROR_NO_ROWS_SELECTED, "Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            String comments = "";
            DateTime unavailableTill;
            double remainingQuantity = 0;

            using (ProductsAvailabilityAttributes attributesDialog = new ProductsAvailabilityAttributes(Utilities, products.ToString()))
            {
                if (attributesDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    remainingQuantity = Convert.ToDouble(attributesDialog.remainingQuantity);
                    unavailableTill = attributesDialog.unavailableTillDate;
                    comments = attributesDialog.comments;

                    foreach (ProductsAvailabilityDTO selectedProduct in selectedProductList)
                    {
                        selectedProduct.IsAvailable = false;
                        selectedProduct.AvailableQty = Convert.ToDecimal(remainingQuantity.ToString());
                        selectedProduct.UnavailableTill = unavailableTill;
                        selectedProduct.ApprovedBy = Manager.UserDTO.LoginId;
                        selectedProduct.Comments = Utilities.ExecutionContext.GetUserId() + " -" + Utilities.getServerTime() + " -Qty:" 
                                + selectedProduct.AvailableQty +" -Till:" + selectedProduct.UnavailableTill + " - " + comments;
                    }

                    ProductsAvailabilityListBL productsAvailabilityBL = new ProductsAvailabilityListBL(Utilities.ExecutionContext, selectedProductList);
                    List<ValidationError> errorsList = productsAvailabilityBL.Save(this.Manager.UserDTO.LoginId);
                    if (errorsList != null && errorsList.Count > 0)
                    {
                        String message = string.Join("-", errorsList);
                        ShowMessage(ERROR, Utilities.MessageUtils.getMessage(2053, message));
                    }
                    else
                    {
                        ShowMessage(MESSAGE, UNAVL_SUCCESSFUL);
                    }

                    FetchProductsAndSetupScreen();
                }
            }
        }

        private void btnMakeAvailable_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            List<ProductsAvailabilityDTO> selectedProductList = new List<ProductsAvailabilityDTO>();
            DataGridViewRowCollection rows = dgvUnavailableProducts.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                if (dgvUnavailableProducts.Rows[i].Cells[0].Value != null &&
                    Convert.ToBoolean(dgvUnavailableProducts.Rows[i].Cells[0].Value))
                {
                    selectedProductList.Add(((List<ProductsAvailabilityDTO>)((System.Windows.Forms.BindingSource)dgvUnavailableProducts.DataSource).DataSource).ElementAt(i));
                }
            }

            if (selectedProductList.Count == 0)
            {
                messageBoxDelegate(ERROR_NO_ROWS_SELECTED, "Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            foreach (ProductsAvailabilityDTO selectedProduct in selectedProductList)
            {
                selectedProduct.IsAvailable = true;
                selectedProduct.Comments = "System Update " + Utilities.ExecutionContext.GetUserId() + "-" + Utilities.getServerTime() + "-" + "Made available by " + Manager.UserDTO.LoginId;
            }

            ProductsAvailabilityListBL productsAvailabilityBL = new ProductsAvailabilityListBL(Utilities.ExecutionContext, selectedProductList);
            List<ValidationError> errorsList = productsAvailabilityBL.Save(this.Manager.UserDTO.LoginId);
            if (errorsList != null && errorsList.Count > 0)
            {
                String message = string.Join("-", errorsList);
                ShowMessage(ERROR, Utilities.MessageUtils.getMessage(2053, message));
            }
            else
            {
                ShowMessage(MESSAGE, AVL_SUCCESSFUL);
               
            }

            FetchProductsAndSetupScreen();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            FetchProductsAndSetupScreen();
            ShowMessage(MESSAGE, ACTION_SUCCESS);

            log.LogMethodExit();
        }

        private void UpdateUnavailableProduct()
        {
            log.LogMethodEntry();

            if (selectedProduct == null)
            {
                messageBoxDelegate(ERROR_NO_ROWS_SELECTED, "Error", MessageBoxButtons.OK, MessageBoxIcon.None);
                return;
            }

            using (ProductsAvailabilityAttributes attributesDialog = new ProductsAvailabilityAttributes(Utilities,
                                                                    Decimal.ToInt32(selectedProduct.AvailableQty),
                                                                    selectedProduct.UnavailableTill,
                                                                    selectedProduct.Comments,
                                                                    selectedProduct.ProductName))
            {
                if (attributesDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedProduct.AvailableQty = Convert.ToDecimal(attributesDialog.remainingQuantity);
                    selectedProduct.UnavailableTill = attributesDialog.unavailableTillDate;
                    selectedProduct.Comments = this.Manager.UserDTO.LoginId + "-" + Utilities.getServerTime() + "-" +
                                               attributesDialog.comments + "\r\n" + selectedProduct.Comments;
                }
                try
                {
                    ProductsAvailabilityBL updatePABL = new ProductsAvailabilityBL(this.Utilities.ExecutionContext, selectedProduct);
                    lblMessage.Text = updatePABL.Save(this.Manager.UserDTO.LoginId);
                    ShowMessage(MESSAGE, UPDATE_SUCCESS);
                    dgvUnavailableProducts.Refresh();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    ShowMessage(ERROR, Utilities.MessageUtils.getMessage(2053, selectedProduct.ProductName));
                }
                
            }

            log.LogMethodExit();
        }

        private void cbProductDisplayGroup_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            int newPDG = cbProductDisplayGroup.SelectedValue == null ? -1 : Convert.ToInt32(cbProductDisplayGroup.SelectedValue.ToString());
            SetDataToControls(newPDG);
            resizeGridColumns(ref dgvAvailableProducts, false);
            resizeGridColumns(ref dgvUnavailableProducts, true);

            log.LogMethodExit();
        }

        private void GetEligibleDisplayGroupsForUser(string managerId)
        {
            log.LogMethodEntry(managerId);

            // get list of display groups
            ProductDisplayGroupList displayGroupList = new ProductDisplayGroupList(Utilities.ExecutionContext);
            displayFormatGroups = displayGroupList.GetConfiguredDisplayGroupListForLogin(managerId);

            // get products from all display groups
            ProductsDisplayGroupList productsByDisplayGroupList = new ProductsDisplayGroupList(Utilities.ExecutionContext);
            List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> displayGroupSearchParameters
                = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
            productsByDisplayGroups = productsByDisplayGroupList.GetAllProductsDisplayGroup(displayGroupSearchParameters);

            // get list of excluded products
            if (displayFormatGroups != null)
            {
                productsOfExcludedDisplayGroups = productsByDisplayGroups.Where(x => !displayFormatGroups.Any(y => y.Id == x.DisplayGroupId)).ToList();
                List<ProductsDisplayGroupDTO> productsOfincludedDisplayGroups = productsByDisplayGroups.Where(x => displayFormatGroups.Any(y => y.Id == x.DisplayGroupId)).ToList();
                productsOfExcludedDisplayGroups = productsByDisplayGroups.Where(x => !productsOfincludedDisplayGroups.Any(y => y.ProductId == x.ProductId)).ToList();
            }

            //// same product can be part of multiple display groups. Filter once again to remove items which are part of valid groups
            //productsOfExcludedDisplayGroups = productsOfExcludedDisplayGroups.Where(x => !productsByDisplayGroups.Any(y => y.ProductId == x.ProductId)).ToList();

            ProductDisplayGroupFormatDTO allPDG = new ProductDisplayGroupFormatDTO();
            allPDG.Id = -1;
            allPDG.DisplayGroup = "Others";
            allPDG.SortOrder = -1;
            displayFormatGroups.Insert(0,allPDG);

            cbProductDisplayGroup.ValueMember = "Id";
            cbProductDisplayGroup.DisplayMember = "DisplayGroup";
            cbProductDisplayGroup.DataSource = displayFormatGroups;
            cbProductDisplayGroup.SelectedValue = -1;

            log.LogMethodExit();
        }

        private void FetchProductsAndSetupScreen()
        {
            log.LogMethodEntry();

            Cursor.Current = Cursors.WaitCursor;

            if (availableProducts != null)
                availableProducts.Clear();
            
            if (unAvailableProducts != null)
                unAvailableProducts.Clear();
            
            ProductsAvailabilityListBL pAListBL = new ProductsAvailabilityListBL(Utilities.ExecutionContext);
            availableProducts = pAListBL.GetAvailableProductsList(productsOfExcludedDisplayGroups);
            unAvailableProducts = pAListBL.GetUnAvailableProductsList(availableProducts, productsOfExcludedDisplayGroups);

            // filter unavailable products from available products
            availableProducts = availableProducts.Where(x => !unAvailableProducts.Any(y => y.ProductId == x.ProductId)).ToList();

            // all display sources are being bound on change of the combo box item. Force a selection change to get data bindings
            SetDataToControls(cbProductDisplayGroup.SelectedValue == null ? -1 : Convert.ToInt32(cbProductDisplayGroup.SelectedValue.ToString()));

            resizeGridColumns(ref dgvAvailableProducts, false);
            resizeGridColumns(ref dgvUnavailableProducts, true);

            Cursor.Current = Cursors.Default;

            log.LogMethodExit();
        }

        private void SetDataToControls(int selection)
        {
            log.LogMethodEntry(selection);

            cbAutoCompleteAvailableProducts.DataSource = null;
            cbAutoCompleteUnavailableProducts.DataSource = null;

            List<ProductsAvailabilityDTO> selectedAvailableProducts = new List<ProductsAvailabilityDTO>();
            List<ProductsAvailabilityDTO> selectedUnavailableProducts = new List<ProductsAvailabilityDTO>();
            List<ProductsDisplayGroupDTO> displayGroupList;
            // for others get list of all products which are not there in any list
            if (selection == -1)
            {
                if (availableProducts != null)
                {
                    selectedAvailableProducts = availableProducts.Where(x => !productsByDisplayGroups.Any(y => y.ProductId == x.ProductId) && x.ProductId > 0).ToList();
                }
                if (unAvailableProducts != null)
                {
                    selectedUnavailableProducts = unAvailableProducts.Where(x => !productsByDisplayGroups.Any(y => y.ProductId == x.ProductId) && x.ProductId > 0).ToList();
                }
            }
            else
            {
                displayGroupList = productsByDisplayGroups.Where(x => (x.DisplayGroupId == selection)).ToList();
                if (availableProducts != null)
                {
                    selectedAvailableProducts = availableProducts.Where(x => displayGroupList.Any(y => y.ProductId == x.ProductId) && x.ProductId > 0).ToList();
                }
                if (unAvailableProducts != null)
                {
                    selectedUnavailableProducts = unAvailableProducts.Where(x => displayGroupList.Any(y => y.ProductId == x.ProductId) && x.ProductId > 0).ToList();
                }
            }

            ProductsAvailabilityDTO defaultProduct = new ProductsAvailabilityDTO();
            defaultProduct.ProductId = 0;
            defaultProduct.ProductName = " ";

            ProductsAvailabilityDTO defaultProduct1 = new ProductsAvailabilityDTO();
            defaultProduct1.ProductId = 0;
            defaultProduct1.ProductName = " ";

            if (selectedAvailableProducts != null)
            {
                cbAvailableProductsDS = selectedAvailableProducts.ToList();
                cbAvailableProductsDS.Insert(0, defaultProduct);
            }
            if (selectedUnavailableProducts != null)
            {
                cbUnavailableProductsDS = selectedUnavailableProducts.ToList();
                cbUnavailableProductsDS.Insert(0, defaultProduct1);
            }

            cbAutoCompleteAvailableProducts.ValueMember = "ProductId";
            cbAutoCompleteAvailableProducts.DisplayMember = "ProductName";
            cbAutoCompleteAvailableProducts.DataSource = cbAvailableProductsDS;
            cbAutoCompleteAvailableProducts.ResetText();

            cbAutoCompleteUnavailableProducts.ValueMember = "ProductId";
            cbAutoCompleteUnavailableProducts.DisplayMember = "ProductName";
            cbAutoCompleteUnavailableProducts.DataSource = cbUnavailableProductsDS;
            cbAutoCompleteUnavailableProducts.ResetText();

            this.availableProductsDTOBS.DataSource = selectedAvailableProducts;
            this.unavailableProductsDTOBS.DataSource = selectedUnavailableProducts;
            dgvAvailableProducts.DataSource = availableProductsDTOBS;
            dgvUnavailableProducts.DataSource = unavailableProductsDTOBS;

            Utilities.setupDataGridProperties(ref dgvAvailableProducts);
            Utilities.setupDataGridProperties(ref dgvUnavailableProducts);

            resizeGridColumns(ref dgvAvailableProducts, false);
            resizeGridColumns(ref dgvUnavailableProducts, true);

            Utilities.setLanguage(dgvAvailableProducts);
            Utilities.setLanguage(dgvUnavailableProducts);

            log.LogMethodExit();
        }

        private void resizeGridColumns(ref DataGridView dgv, bool showAdditionalColumns)
        {

            log.LogMethodEntry(dgv);

            dgv.SuspendLayout();
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ScrollBars = ScrollBars.None;
            dgv.MultiSelect = false;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;

            if (!showAdditionalColumns)
            {
                dgv.Columns[0].Width = (int)(0.07 * dgv.Width);
                dgv.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[1].Width = (int)(0.93 * dgv.Width);
            }
            else
            {
                dgv.Columns[0].Width = (int)(0.07 * dgv.Width);
                dgv.Columns[1].Width = (int)(0.08 * dgv.Width);
                dgv.Columns[2].Width = (int)(0.45 * dgv.Width);
                dgv.Columns[3].Width = (int)(0.13 * dgv.Width);
                dgv.Columns[3].DefaultCellStyle.Format = Utilities.getNumberFormat();
                dgv.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[4].Width = (int)(0.30 * dgv.Width);
                dgv.Columns[4].DefaultCellStyle.Format = Utilities.getDateTimeFormat();
                dgv.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            
            dgv.ResumeLayout();
            log.LogMethodExit();
        }

        private void ShowMessage(string messageType, string Message)
        {
            switch (messageType)
            {
                case WARNING:
                    lblMessage.BackColor = Color.Yellow;
                    lblMessage.ForeColor = Color.Black;
                    break;
                case ERROR:
                    lblMessage.BackColor = Color.Red;
                    lblMessage.ForeColor = Color.White;
                    break;
                case MESSAGE:
                    lblMessage.BackColor = Color.White;
                    lblMessage.ForeColor = Color.Black;
                    break;
                default:
                    lblMessage.ForeColor = Color.Black;
                    break;
            }
            lblMessage.Text = Message;
        }

        private void cbAutoCompleteAvailableProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(cbAutoCompleteAvailableProducts.DataSource != null && dgvAvailableProducts.DataSource != null)
            {
                int selProdId = cbAutoCompleteAvailableProducts.SelectedValue == null ? -1 : Convert.ToInt32(cbAutoCompleteAvailableProducts.SelectedValue.ToString());
                if (selProdId > 0)
                {
                    log.Debug("selProdId " + selProdId);
                    int currIndex = -1;
                    if (dgvAvailableProducts.CurrentRow != null)
                    {
                        currIndex = dgvAvailableProducts.CurrentRow.Index;
                    }
                    else
                    {
                        log.Debug("Current selected row is null");
                    }
                    List<ProductsAvailabilityDTO> source = ((List<ProductsAvailabilityDTO>)((System.Windows.Forms.BindingSource)dgvAvailableProducts.DataSource).DataSource);
                    if(source != null && source.Count > 0)
                    {
                        log.Debug("dgvAvailableProducts is not null and not empty");
                        var index = source.TakeWhile(t => !(t.ProductId == selProdId)).Count();
                        log.Debug("index " + index);
                        dgvAvailableProducts.Rows[index].Selected = true;
                        dgvAvailableProducts.FirstDisplayedScrollingRowIndex = index;

                        if (index > currIndex)
                            avlProductsVScrollBar.UpdateUpButtonStatus(true);
                        else
                            avlProductsVScrollBar.UpdateDownButtonStatus(true);
                    }
                    else if (source == null)
                    {
                        log.Debug("dgvAvailableProducts is null");
                    }
                    else if (source.Count == 0)
                    {
                        log.Debug("dgvAvailableProducts is empty");
                    }
                }
            }
            log.LogMethodExit();
        }

        private void cbAutoCompleteUnavailableProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (cbAutoCompleteUnavailableProducts.DataSource != null && dgvUnavailableProducts.DataSource != null)
            {
                int selProdId = cbAutoCompleteUnavailableProducts.SelectedValue == null ? -1 : Convert.ToInt32(cbAutoCompleteUnavailableProducts.SelectedValue.ToString());
                if (selProdId > 0)
                {
                    int currIndex = -1;
                    if (dgvUnavailableProducts.CurrentRow != null)
                    {
                        currIndex = dgvUnavailableProducts.CurrentRow.Index;
                    }
                    else
                    {
                        log.Debug("Current selected row is null");
                    }
                    List<ProductsAvailabilityDTO> source = ((List<ProductsAvailabilityDTO>)((System.Windows.Forms.BindingSource)dgvUnavailableProducts.DataSource).DataSource);
                    if (source != null && source.Count > 0)
                    {
                        log.Debug("dgvUnavailableProducts is not null and not empty");
                        var index = source.TakeWhile(t => !(t.ProductId == selProdId)).Count();
                        dgvUnavailableProducts.Rows[index].Selected = true;
                        dgvUnavailableProducts.FirstDisplayedScrollingRowIndex = index;
                        if (index > currIndex)
                            unavlProductsVScrollBar.UpdateUpButtonStatus(true);
                        else
                            unavlProductsVScrollBar.UpdateDownButtonStatus(true);
                    }
                    else if (source == null)
                    {
                        log.Debug("dgvUnavailableProducts is null");
                    }
                    else if (source.Count == 0)
                    {
                        log.Debug("dgvUnavailableProducts is empty");
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvAvailableProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (dgvAvailableProducts.CurrentCell == null)
                return;

            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                if (dgvAvailableProducts.CurrentRow.Cells[0].Value == null ||
                       !Convert.ToBoolean(dgvAvailableProducts.CurrentRow.Cells[0].Value))
                    dgvAvailableProducts.CurrentRow.Cells[0].Value = true;
                else
                    dgvAvailableProducts.CurrentRow.Cells[0].Value = false;
            }

            log.LogMethodExit();
        }

        private void dgvUnavailableProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (dgvUnavailableProducts.CurrentCell == null)
                return;

            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                this.selectedProduct = ((List<ProductsAvailabilityDTO>)((System.Windows.Forms.BindingSource)dgvUnavailableProducts.DataSource).DataSource).ElementAt(e.RowIndex);
                UpdateUnavailableProduct();
            }
            else if (senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                if (dgvUnavailableProducts.CurrentRow.Cells[0].Value == null ||
                       !Convert.ToBoolean(dgvUnavailableProducts.CurrentRow.Cells[0].Value))
                    dgvUnavailableProducts.CurrentRow.Cells[0].Value = true;
                else
                    dgvUnavailableProducts.CurrentRow.Cells[0].Value = false;
            }

            log.LogMethodExit();
        }
    }
}
