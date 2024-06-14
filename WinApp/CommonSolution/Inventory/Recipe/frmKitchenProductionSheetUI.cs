/********************************************************************************************
 * Project Name - Inventory
 * Description  - UI to create manufacturing data (Kitchen Print Note) 
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       28-Aug-2020   Deeksha             Created as part of Recipe management enhancement
 *********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Inventory.Recipe
{
    public partial class frmKitchenProductionSheetUI : Form
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executioncontext;
        private Utilities utilities;
        private List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = null;
        List<RecipeManufacturingDetailsDTO> manufacturingDetailsDTOListOnDisplay = null;
        private List<ProductDTO> productDTOList = null;
        private bool isUIEnabled = true;

        public frmKitchenProductionSheetUI(Utilities utilities)
        {
            log.LogMethodEntry(utilities);
            this.utilities = utilities;
            executioncontext = utilities.ExecutionContext;
            InitializeComponent();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executioncontext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executioncontext.SetSiteId(-1);
            }
            SetStyleAndLanguage();
            executioncontext.SetUserId(utilities.ParafaitEnv.LoginID);
            GetActualUOM();
            dgvKPN.Refresh();
            ProductContainer productContainer = new ProductContainer(executioncontext);
            UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
            log.LogMethodExit();
        }

        //protected override CreateParams CreateParams
        //{
        //    //this method is used to avoid the table layout flickering.
        //    get
        //    {
        //        CreateParams CP = base.CreateParams;
        //        CP.ExStyle = CP.ExStyle | 0x02000000;
        //        return CP;
        //    }
        //}

        private void frmKitchenPrintNote_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadGrid(utilities.getServerTime());
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to set Grid formatting and language
        /// </summary>
        private void SetStyleAndLanguage()
        {
            log.LogMethodEntry();
            try
            {
                dgvKPN.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                dgvKPN.EnableHeadersVisualStyles = false;

                dgvKPN.BackgroundColor = this.BackColor;
                dgvKPN.EditMode = DataGridViewEditMode.EditOnEnter;
                quantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
                actualMfgQuantityDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewNumericCellStyle();
                actualCostDataGridViewTextBoxColumn.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                quantityDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                actualMfgQuantityDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
                actualCostDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;
                PlannedCost.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;
                PlannedCost.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                ItemCost.DefaultCellStyle = utilities.gridViewAmountCellStyle();
                ItemCost.DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;
                creationDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateTimeFormat();
                lastUpdateDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = utilities.getDateTimeFormat();
                ThemeUtils.SetupVisuals(this);
                utilities.setLanguage(this);
                btnSaveComplete.Text = MessageContainerList.GetMessage(executioncontext, "Save and Complete");
                btnIExport.Text = MessageContainerList.GetMessage(executioncontext, "Export");
                btnExit.Text = MessageContainerList.GetMessage(executioncontext, "Close");
                btnSave.Text = MessageContainerList.GetMessage(executioncontext, "Save");
                btnGO.Text = MessageContainerList.GetMessage(executioncontext, "Go");
                btnRefresh.Text = MessageContainerList.GetMessage(executioncontext, "Refresh");
                lblProdDate.Text = MessageContainerList.GetMessage(executioncontext, "Production Date");
                dtpProdDate.Value = utilities.getServerTime();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Event which fires during Product Search based on product code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvKPN_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                log.LogMethodExit(null, "Invalid cell");
                return;
            }
            ProductContainer productContainer = new ProductContainer(executioncontext);
            if (e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                if (dgvKPN.Columns[e.ColumnIndex].Name == "ItemName")
                {
                    try
                    {
                        lblMessage.Text = string.Empty;
                        string description = dgvKPN[e.ColumnIndex, e.RowIndex].Value.ToString();
                        if (description.Contains("%"))
                        {
                            description = string.Empty;
                        }
                        if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                        {
                            productDTOList = ProductContainer.productDTOList.Where(x => x.Description.IndexOf(description, StringComparison.OrdinalIgnoreCase) != -1 |
                                                                                   x.Code.IndexOf(description, StringComparison.OrdinalIgnoreCase) != -1).ToList();
                        }
                        if (productDTOList == null || productDTOList.Count < 1)
                        {
                            MessageBox.Show(utilities.MessageUtils.getMessage(846), MessageContainerList.GetMessage(executioncontext, "Find Products"));
                            BeginInvoke((MethodInvoker)delegate
                            {
                                RemoveDGVRow(e.RowIndex);
                            });
                        }
                        else if (productDTOList.Count == 1)
                        {
                            ProductDTO productDTO = productDTOList[0];
                            AutoPopulateProductAndBOM(productDTO);
                            dgvKPN.Refresh();
                        }
                        else if (productDTOList.Count > 1)
                        {
                            Panel pnlMultiple_dgv = new Panel();
                            this.Controls.Add(pnlMultiple_dgv);
                            DataGridView multiple_dgv = new DataGridView();
                            pnlMultiple_dgv.Controls.Add(multiple_dgv);
                            multiple_dgv.LostFocus += new EventHandler(multiple_dgv_LostFocus);
                            multiple_dgv.Click += new EventHandler(multiple_dgv_Click);
                            multiple_dgv.Focus();
                            productDTOList = productDTOList.OrderBy(x => x.Description).ToList();
                            multiple_dgv.DataSource = productDTOList;
                            multiple_dgv.Refresh();
                            multiple_dgv_Format(ref pnlMultiple_dgv, ref multiple_dgv);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }
                }
                if (dgvKPN.Columns[e.ColumnIndex].Name == "quantityDataGridViewTextBoxColumn") //Quantity value change event considers yield percentage calculation
                {
                    CalculateProductQuantityWithYieldPerc();
                }
                if (dgvKPN.Columns[e.ColumnIndex].Name == "actualMfgQuantityDataGridViewTextBoxColumn") //Actual Quantity value change ,yield percentage is not considered
                {
                    List<RecipeManufacturingDetailsDTO> parentProductList = manufacturingDetailsDTOListOnDisplay.Where(x => x.IsParentItem == true).ToList();
                    foreach (RecipeManufacturingDetailsDTO parentDTO in parentProductList)
                    {
                        ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == parentDTO.ProductId);
                        if (parentDTO.IsChanged)
                        {
                            int parentLineId = parentDTO.MfgLineId;
                            List<RecipeManufacturingDetailsDTO> childProductList = manufacturingDetailsDTOListOnDisplay.Where(x => x.ParentMFGLineId == parentLineId).ToList();
                            foreach (RecipeManufacturingDetailsDTO childDTO in childProductList)
                            {
                                decimal factor = 1;
                                decimal quantity = productDTO.ProductBOMDTOList.Find(x => x.ChildProductId == childDTO.ProductId).Quantity;
                                int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == childDTO.ProductId).InventoryUOMId;
                                if (childDTO.MfgUOMId != inventoryUOMId)
                                {
                                    factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(childDTO.ActualMfgUOMId, inventoryUOMId));
                                }
                                childDTO.ActualMfgQuantity = (parentDTO.ActualMfgQuantity * quantity);
                                parentDTO.ActualCost = parentDTO.ActualMfgQuantity * parentDTO.ItemCost ;
                                childDTO.ActualCost = childDTO.ActualMfgQuantity * childDTO.ItemCost * factor;
                            }
                        }
                    }
                    List<RecipeManufacturingDetailsDTO> childDTOList = manufacturingDetailsDTOListOnDisplay.Where(x => x.IsParentItem == false).ToList();
                    foreach (RecipeManufacturingDetailsDTO childDTO in childDTOList)
                    {
                        //ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == childDTO.ProductId);
                        if (childDTO.IsChanged)
                        {
                            decimal factor = 1;
                            int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == childDTO.ProductId).InventoryUOMId;
                            if (childDTO.MfgUOMId != inventoryUOMId)
                            {
                                factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(childDTO.ActualMfgUOMId, inventoryUOMId));
                            }
                            childDTO.ActualCost = childDTO.ActualMfgQuantity * childDTO.ItemCost * factor;
                        }
                    }

                }
                dgvKPN.Refresh();
                log.LogMethodExit();
            }
        }

        /// <summary>
        /// Child product quantity calculation based on the parent product quantity & yield percentage
        /// </summary>
        private void CalculateProductQuantityWithYieldPerc()
        {
            log.LogMethodEntry();
            try
            {
                List<RecipeManufacturingDetailsDTO> parentProductList = manufacturingDetailsDTOListOnDisplay.Where(x => x.IsParentItem == true).ToList();
                foreach (RecipeManufacturingDetailsDTO parentDTO in parentProductList)
                {
                    int parentLineId = parentDTO.MfgLineId;
                    ProductDTO parentProductDTO = ProductContainer.productDTOList.Find(x => x.ProductId == parentDTO.ProductId);
                    List<RecipeManufacturingDetailsDTO> childProductList = manufacturingDetailsDTOListOnDisplay.Where(x => x.ParentMFGLineId == parentLineId).ToList();
                    foreach (RecipeManufacturingDetailsDTO childDTO in childProductList)
                    {
                        decimal childQty = parentProductDTO.ProductBOMDTOList.Find(x => x.ChildProductId == childDTO.ProductId).Quantity;
                        if (parentProductDTO.YieldPercentage > 0)
                        {
                            childDTO.Quantity = (parentDTO.Quantity * childQty * 100) / parentProductDTO.YieldPercentage;
                        }
                        else
                        {
                            childDTO.Quantity = (parentDTO.Quantity * childQty);
                        }
                        decimal factor = 1;
                        int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == childDTO.ProductId).InventoryUOMId;
                        if (childDTO.MfgUOMId != inventoryUOMId)
                        {
                            factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(childDTO.ActualMfgUOMId, inventoryUOMId));
                        }
                        childDTO.ActualMfgQuantity = childDTO.Quantity;
                        decimal actualQty = Convert.ToDecimal(childDTO.Quantity) * factor;
                        childDTO.ActualCost = actualQty * childDTO.ItemCost;
                        childDTO.PlannedCost = actualQty * childDTO.ItemCost;

                    }
                    parentDTO.ActualMfgQuantity = parentDTO.Quantity;
                    parentDTO.ActualCost = parentDTO.ActualMfgQuantity * parentDTO.ItemCost;
                    parentDTO.ActualCost = parentDTO.ActualMfgQuantity * parentDTO.ItemCost;
                    parentDTO.PlannedCost = parentDTO.Quantity * parentDTO.ItemCost;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            dgvKPN.Refresh();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to remove Row at specified index
        /// </summary>
        private void RemoveDGVRow(int index)
        {
            log.LogMethodEntry();
            try
            {
                if (index > -1)
                {
                    dgvKPN.Rows.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        void multiple_dgv_LostFocus(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DataGridView dg = (DataGridView)sender;
                if (dg.SelectedRows.Count == 0)
                {
                    dgvKPN.Rows.Remove(dgvKPN.Rows[dgvKPN.CurrentRow.Index]);
                }
                GetActualUOM();
                dg.Visible = false;
                dg.Parent.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }


        void multiple_dgv_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DataGridView dg = (DataGridView)sender;
                ProductDTO productDTO = productDTOList[dg.CurrentRow.Index];
                AutoPopulateProductAndBOM(productDTO);
                dg.Parent.Visible = false;
                dgvKPN.Refresh();
                dgvKPN.RefreshEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to auto populate BOM structure
        /// </summary>
        private void AutoPopulateProductAndBOM(ProductDTO productDTO)
        {
            log.LogMethodEntry(productDTO);
            try
            {
                List<RecipeManufacturingDetailsDTO> tempManufacturingDTOList = new List<RecipeManufacturingDetailsDTO>();
                int rowIndex = manufacturingDetailsDTOListOnDisplay.Count - 1;
                RecipeManufacturingDetailsDTO recipeDTO = manufacturingDetailsDTOListOnDisplay.Find(x => x.ProductId == productDTO.ProductId);
                if (recipeDTO != null && recipeDTO.IsParentItem == true)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executioncontext, 1872);  //"You cannot Insert duplicate records";
                    lblMessage.ForeColor = Color.Red;
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        RemoveDGVRow(rowIndex);
                    });
                    log.LogMethodExit();
                    return;
                }
                if (manufacturingDetailsDTOListOnDisplay != null && manufacturingDetailsDTOListOnDisplay.Any())
                {
                    tempManufacturingDTOList = manufacturingDetailsDTOListOnDisplay;
                    recipeManufacturingDetailsDTOBindingSource.DataSource = null;
                }
                if (tempManufacturingDTOList.Any() && tempManufacturingDTOList.Exists(x => x.RecipeManufacturingDetailId < 0))
                {
                    tempManufacturingDTOList.Remove(tempManufacturingDTOList[tempManufacturingDTOList.Count - 1]);
                }
                tempManufacturingDTOList = BuildProductBOM(tempManufacturingDTOList, productDTO);
                manufacturingDetailsDTOListOnDisplay = tempManufacturingDTOList;
                recipeManufacturingDetailsDTOBindingSource.DataSource = manufacturingDetailsDTOListOnDisplay;
                dgvKPN.RefreshEdit();
                GetActualUOM();
                CalculateProductQuantityWithYieldPerc();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                recipeManufacturingDetailsDTOBindingSource.DataSource = manufacturingDetailsDTOListOnDisplay;
                lblMessage.ForeColor = Color.Red;
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to build Product BOM details.
        /// </summary>
        /// <param name="recipeManufacturingDetailsDTOList"></param>
        /// <returns></returns>
        private List<RecipeManufacturingDetailsDTO> BuildProductBOM(List<RecipeManufacturingDetailsDTO> recipeManufacturingDetailsDTOList, ProductDTO productDTO)
        {
            log.LogMethodEntry(recipeManufacturingDetailsDTOList, productDTO);

            int lineId = 1;
            if (manufacturingDetailsDTOListOnDisplay != null && manufacturingDetailsDTOListOnDisplay.Any())
            {
                lineId = manufacturingDetailsDTOListOnDisplay.Max(x => x.MfgLineId);
                lineId++;
            }
            RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO = new RecipeManufacturingDetailsDTO(-1, -1, lineId, productDTO.ProductId, true, -1, -1, 1,
                                                                productDTO.UomId, 1, productDTO.UomId, Convert.ToDecimal(productDTO.Cost), Convert.ToDecimal(productDTO.Cost), Convert.ToDecimal(productDTO.Cost), -1, true,false);
            recipeManufacturingDetailsDTO.ItemName = productDTO.Description;
            recipeManufacturingDetailsDTO.ItemUOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productDTO.UomId).UOM;
            recipeManufacturingDetailsDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productDTO.InventoryUOMId).UOM;
            recipeManufacturingDetailsDTOList.Add(recipeManufacturingDetailsDTO);
            int count = 0;
            string sOffset = GetOffsetValue();
            int parentLineid = lineId;
            lineId++;
            if (productDTO.ProductBOMDTOList != null && productDTO.ProductBOMDTOList.Any())
            {
                count = productDTO.ProductBOMDTOList.Count;
                while (count > 0)
                {
                    for (int i = 0; i < productDTO.ProductBOMDTOList.Count; i++)
                    {
                        InventoryList inventoryList = new InventoryList(executioncontext);
                        BOMDTO bOMDTO = productDTO.ProductBOMDTOList[i];
                        
                        ProductDTO childproductDTO = ProductContainer.productDTOList.Find(x => x.ProductId == bOMDTO.ChildProductId);
                        if (bOMDTO.UOMId == -1)
                        {
                            bOMDTO.UOMId = childproductDTO.InventoryUOMId;
                        }
                        decimal stock = inventoryList.GetProductStockQuantity(bOMDTO.ChildProductId);
                        if (stock <= 0)
                        {
                            recipeManufacturingDetailsDTOList.RemoveAt(recipeManufacturingDetailsDTOList.Count - 1);
                            throw new ValidationException(MessageContainerList.GetMessage(executioncontext, 2845, productDTO.Description)); //There is no sufficient quantity for &1 to prepare a recipe Please update the stock
                        }
                        recipeManufacturingDetailsDTO = new RecipeManufacturingDetailsDTO(-1, -1, lineId, childproductDTO.ProductId, false, parentLineid, -1, bOMDTO.Quantity,
                                                                        bOMDTO.UOMId, bOMDTO.Quantity, bOMDTO.UOMId, Convert.ToDecimal(childproductDTO.Cost), (bOMDTO.Quantity * Convert.ToDecimal(childproductDTO.Cost)),
                                                                        Convert.ToDecimal(childproductDTO.Cost), -1, true,false);

                        recipeManufacturingDetailsDTO.ItemName = sOffset + childproductDTO.Description;
                        recipeManufacturingDetailsDTO.ItemUOM = UOMContainer.uomDTOList.Find(x => x.UOMId == bOMDTO.UOMId).UOM;
                        recipeManufacturingDetailsDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == childproductDTO.InventoryUOMId).UOM;
                        recipeManufacturingDetailsDTOList.Add(recipeManufacturingDetailsDTO);
                        count--;
                        lineId++;
                    }
                }
            }
            log.LogMethodExit(recipeManufacturingDetailsDTOList);
            return recipeManufacturingDetailsDTOList;
        }

        private string GetOffsetValue()
        {
            log.LogMethodEntry();
            int offset = 1;
            string sOffset = string.Empty;
            byte[] b = new byte[] { 20, 37 };
            sOffset = Encoding.Unicode.GetString(b);
            sOffset = sOffset.PadLeft(offset * 3 + 1, ' ') + " ";
            log.LogMethodExit(sOffset);
            return sOffset;
        }

        void multiple_dgv_Format(ref Panel pnlMultiple_dgv, ref DataGridView multiple_dgv)
        {
            log.LogMethodEntry(pnlMultiple_dgv, multiple_dgv);
            try
            {
                pnlMultiple_dgv.Size = new Size(300, (dgvKPN.Rows[0].Cells[0].Size.Height * 10) - 3);
                pnlMultiple_dgv.AutoScroll = true;
                pnlMultiple_dgv.Location = new Point(150 + dgvKPN.RowHeadersWidth + dgvKPN.CurrentRow.Cells["ItemName"].ContentBounds.Location.X, dgvKPN.Location.Y + dgvKPN.ColumnHeadersHeight);
                pnlMultiple_dgv.BringToFront();
                pnlMultiple_dgv.BorderStyle = BorderStyle.None;
                pnlMultiple_dgv.BackColor = Color.White;
                multiple_dgv.Width = 300;
                multiple_dgv.BorderStyle = BorderStyle.None;
                multiple_dgv.AllowUserToAddRows = false;
                multiple_dgv.BackgroundColor = Color.White;
                multiple_dgv.AllowUserToOrderColumns = true;

                for (int i = 0; i < multiple_dgv.Columns.Count; i++)
                {
                    if (multiple_dgv.Columns[i].HeaderText == "Description"
                       || multiple_dgv.Columns[i].HeaderText == "Code")
                    {
                        continue;
                    }
                    multiple_dgv.Columns[i].Visible = false;
                }
                multiple_dgv.Columns["Code"].DisplayIndex = 0;
                multiple_dgv.Columns["Description"].DisplayIndex = 1;
                multiple_dgv.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                multiple_dgv.Font = new Font("Arial", 8, FontStyle.Regular);
                multiple_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                multiple_dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                multiple_dgv.ReadOnly = true;
                multiple_dgv.BorderStyle = BorderStyle.None;
                multiple_dgv.RowHeadersVisible = false;
                multiple_dgv.ColumnHeadersVisible = false;
                multiple_dgv.AllowUserToResizeColumns = false;
                multiple_dgv.MultiSelect = false;
                multiple_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                multiple_dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.Wheat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to save MFG details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool saveSuccessul = SaveRecipe(false);
                if (saveSuccessul)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executioncontext, 122); //"Save Successful";
                    lblMessage.ForeColor = Color.Blue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Has Changes
        /// </summary>
        /// <returns></returns>
        private bool HasChanges(bool isComplete)
        {
            log.LogMethodEntry();
            if (isComplete)
            {
                log.LogMethodExit(true);
                return true;
            }
            if (recipeManufacturingHeaderDTOList != null && recipeManufacturingHeaderDTOList.Count > 0)
            {
                if (recipeManufacturingHeaderDTOList[0].IsChanged || recipeManufacturingHeaderDTOList[0].IsChangedRecursive)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            if(manufacturingDetailsDTOListOnDisplay != null && manufacturingDetailsDTOListOnDisplay.Count > 0)
            {
                foreach(RecipeManufacturingDetailsDTO recipeDetailsDTO in manufacturingDetailsDTOListOnDisplay)
                {
                    if(recipeDetailsDTO.IsChanged)
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Saves MFG details or Saves and performs Inventory Stock update
        /// </summary>
        /// <param name="isComplete"></param>
        /// <returns></returns>
        private bool SaveRecipe(bool isComplete)
        {
            log.LogMethodEntry(isComplete);
            bool saveSucceful = false;
            lblMessage.Text = string.Empty;
            dgvKPN.RefreshEdit();
            if (manufacturingDetailsDTOListOnDisplay != null && manufacturingDetailsDTOListOnDisplay.Count > 0)
            {
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        manufacturingDetailsDTOListOnDisplay = (List<RecipeManufacturingDetailsDTO>)recipeManufacturingDetailsDTOBindingSource.DataSource;
                        for (int i = 0; i < manufacturingDetailsDTOListOnDisplay.Count; i++)
                        {
                            if (Convert.ToBoolean(dgvKPN.Rows[i].Cells["cbxDel"].Value))
                            {
                                manufacturingDetailsDTOListOnDisplay[i].IsActive = false;
                                int lineID = manufacturingDetailsDTOListOnDisplay[i].MfgLineId;
                                List<RecipeManufacturingDetailsDTO> recipeDTOList = manufacturingDetailsDTOListOnDisplay.Where(x => x.TopMostParentMFGLineId == lineID).ToList();
                                if (recipeDTOList != null)
                                {
                                    foreach (RecipeManufacturingDetailsDTO dto in recipeDTOList)
                                    {
                                        dto.IsActive = false;
                                    }
                                }
                            }
                        }
                        //bool valid = ValidateRecipe(isComplete);
                        //if (valid)
                        {
                            RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO = new RecipeManufacturingHeaderDTO();
                            if (recipeManufacturingHeaderDTOList != null && recipeManufacturingHeaderDTOList.Any())
                            {
                                recipeManufacturingHeaderDTO = recipeManufacturingHeaderDTOList[0];
                            }
                            recipeManufacturingHeaderDTO.MFGDateTime = dtpProdDate.Value;
                            recipeManufacturingHeaderDTO.IsComplete = isComplete;
                            RecipeManufacturingDetailsListBL recipeManufacturingDetailsListBL = new RecipeManufacturingDetailsListBL(executioncontext, manufacturingDetailsDTOListOnDisplay);
                            recipeManufacturingDetailsListBL.SetLineDetails();
                            recipeManufacturingHeaderDTO.RecipeManufacturingDetailsDTOList = manufacturingDetailsDTOListOnDisplay;
                            for (int i = 0; i < manufacturingDetailsDTOListOnDisplay.Count; i++)
                            {
                                if (manufacturingDetailsDTOListOnDisplay[i].ActualMfgUOMId != Convert.ToInt32(dgvKPN.Rows[i].Cells["cmbUOM"].Value))
                                {
                                    manufacturingDetailsDTOListOnDisplay[i].ActualMfgUOMId = Convert.ToInt32(dgvKPN.Rows[i].Cells["cmbUOM"].Value);
                                }
                            }
                            RecipeManufacturingHeaderBL recipeManufacturingHeaderBL = new RecipeManufacturingHeaderBL(executioncontext, recipeManufacturingHeaderDTO);
                            recipeManufacturingHeaderBL.Save(parafaitDBTrx.SQLTrx);
                            saveSucceful = true;
                            parafaitDBTrx.EndTransaction();
                            LoadGrid(recipeManufacturingHeaderDTO.MFGDateTime);
                        }
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        lblMessage.Text = ex.Message;
                        lblMessage.ForeColor = Color.Red;
                        log.Error(ex);
                    }
                }
            }
            log.LogMethodExit();
            return saveSucceful;
        }

        /// <summary>
        /// Method to validate MFG
        /// </summary>
        /// <returns></returns>
        private bool ValidateRecipe(bool isComplete)
        {
            log.LogMethodEntry(isComplete);
            manufacturingDetailsDTOListOnDisplay = (List<RecipeManufacturingDetailsDTO>)recipeManufacturingDetailsDTOBindingSource.DataSource;
            bool hasChanges = HasChanges(isComplete);
            if (!hasChanges)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executioncontext, "Nothing to Save");
                log.LogMethodExit(false);
                return false;
            }
            foreach (RecipeManufacturingDetailsDTO recipeDTO in manufacturingDetailsDTOListOnDisplay)
            {
                if ((recipeDTO.ParentMFGLineId == recipeDTO.TopMostParentMFGLineId) &&
                        (recipeDTO.ActualMfgQuantity == null) || (recipeDTO.ActualMfgQuantity <= 0))
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executioncontext, 2760, recipeDTO.ItemName);//"Please Enter valid Actual Quantity for the Recipe"
                    lblMessage.ForeColor = Color.Red;
                    log.LogMethodExit(false);
                    return false;
                }
            }
            if (isUIEnabled == false)
            {
                lblMessage.Text = MessageContainerList.GetMessage(executioncontext, 2805); //Inventory stock is updated for the displayed products.Cannot edit this
                lblMessage.ForeColor = Color.Red;
                log.LogMethodExit(false);
                return false;
            }
            if (isComplete && MessageBox.Show(MessageContainerList.GetMessage(executioncontext, 2843), "Recipe Manufacturing", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                //This action will finalize the inventory stock and the records will not be editable after performing this action. Do you want to proceed?
                lblMessage.Text = MessageContainerList.GetMessage(executioncontext, 2769);//"Inventory Stock is not updated"; 
                lblMessage.ForeColor = Color.Red;
                log.LogMethodExit(false);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to populate MFG details based on the date selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGO_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                isUIEnabled = true;
                dgvKPN.Enabled = true;
                lblMessage.Text = string.Empty;
                LoadGrid(dtpProdDate.Value);
                dgvKPN.Refresh();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to save MFG details & updates Inventory stock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveComplete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                this.Cursor = Cursors.WaitCursor;
                for (int i = 0; i < manufacturingDetailsDTOListOnDisplay.Count; i++)
                {
                    if (Convert.ToBoolean(dgvKPN.Rows[i].Cells["cbxDel"].Value))
                    {
                        lblMessage.Text = MessageContainerList.GetMessage(executioncontext, 2080); //"Please save product and proceed";
                        lblMessage.ForeColor = Color.Red;
                        return;
                    }
                }
                bool saveSuccessul = SaveRecipe(true);
                if (saveSuccessul)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executioncontext, 2770); //"Inventory Stock updated successfully";
                    lblMessage.ForeColor = Color.Blue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to populate related UOM's in the UOM combo box
        /// </summary>
        private void GetActualUOM()
        {
            log.LogMethodEntry();
            try
            {
                for (int i = 0; i < manufacturingDetailsDTOListOnDisplay.Count; i++)
                {
                    if (dgvKPN.Rows[i].Cells["cmbUOM"].Value == null)
                    {
                        int uomId = manufacturingDetailsDTOListOnDisplay[i].ActualMfgUOMId;
                        if (uomId == -1)
                        {
                            uomId = manufacturingDetailsDTOListOnDisplay[i].MfgUOMId;
                        }
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvKPN, i, uomId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to hide check box for BOM , highlight Quantity field for Unsaved records and ActualQuantity field for Saved records.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvKPN_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                for (int i = 0; i < manufacturingDetailsDTOListOnDisplay.Count; i++)
                {
                    if (manufacturingDetailsDTOListOnDisplay[i].IsParentItem == false)
                    {
                        DataGridViewCheckBoxCell cbcell = dgvKPN.Rows[i].Cells["cbxDel"] as DataGridViewCheckBoxCell;
                        dgvKPN.Rows[i].Cells["cbxDel"].ReadOnly = true;
                    }
                    if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvKPN.Columns[e.ColumnIndex].Name == "cbxDel")
                    {
                        DataGridViewCheckBoxCell cbcell = dgvKPN.Rows[e.RowIndex].Cells["cbxDel"] as DataGridViewCheckBoxCell;
                        {
                            if (cbcell.ReadOnly == true)
                            {
                                e.PaintBackground(e.ClipBounds, true);
                                e.Handled = true;
                            }
                        }
                    }
                    if (manufacturingDetailsDTOListOnDisplay[i].IsComplete)
                    {
                        dgvKPN.Rows[i].ReadOnly = true;
                        dgvKPN.Rows[i].Frozen = true;
                    }
                    else
                    {
                        if (manufacturingDetailsDTOListOnDisplay[i].RecipeManufacturingDetailId > -1)
                        {
                            dgvKPN.Rows[i].Cells["ItemName"].ReadOnly = true;
                            dgvKPN.Rows[i].Cells["quantityDataGridViewTextBoxColumn"].ReadOnly = true;
                            dgvKPN.Rows[i].Cells["actualMfgQuantityDataGridViewTextBoxColumn"].ReadOnly = false;
                            dgvKPN.Rows[i].Cells["actualMfgQuantityDataGridViewTextBoxColumn"].Style.BackColor = Color.LightBlue;
                        }
                        else
                        {
                            dgvKPN.Rows[i].Cells["ItemName"].ReadOnly = false;
                            dgvKPN.Rows[i].Cells["quantityDataGridViewTextBoxColumn"].Style.BackColor = Color.LightBlue;
                            dgvKPN.Rows[i].Cells["actualMfgQuantityDataGridViewTextBoxColumn"].ReadOnly = true;
                            dgvKPN.Rows[i].Cells["quantityDataGridViewTextBoxColumn"].ReadOnly = false;
                        }
                    }
                    GetActualUOM();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to load estimation details to the data grid view
        /// </summary>
        /// <param name="date"></param>
        private void LoadGrid(DateTime fromDate , DateTime? toDate = null, bool isExport = false)
        {
            log.LogMethodEntry(fromDate , toDate);
            try
            {
                lblMessage.Text = string.Empty;
                if(toDate == null)
                {
                    toDate = fromDate;
                }
                UOMContainer uomcontainer = CommonFuncs.GetUOMContainer();
                manufacturingDetailsDTOListOnDisplay = new List<RecipeManufacturingDetailsDTO>();
                RecipeManufacturingHeaderListBL recipeManufacturingHeaderListBL = new RecipeManufacturingHeaderListBL(executioncontext);
                List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_FROM_DATETIME, fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_TO_DATETIME, Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.SITE_ID, executioncontext.GetSiteId().ToString()));
                recipeManufacturingHeaderDTOList = recipeManufacturingHeaderListBL.GetAllRecipeManufacturingHeaderDTOList(searchParameters, true, true);
                if (recipeManufacturingHeaderDTOList != null && recipeManufacturingHeaderDTOList.Any())
                {
                    string sOffset = GetOffsetValue();
                    recipeManufacturingHeaderDTOList = recipeManufacturingHeaderDTOList.OrderBy(x => x.MFGDateTime).ToList();
                    foreach (RecipeManufacturingHeaderDTO headerDTO in recipeManufacturingHeaderDTOList)
                    {
                        List<RecipeManufacturingDetailsDTO> detailsDTOList = new List<RecipeManufacturingDetailsDTO>();
                        detailsDTOList = headerDTO.RecipeManufacturingDetailsDTOList.FindAll(x=>x.IsComplete == false).ToList();
                        if(detailsDTOList != null && detailsDTOList.Count > 0)
                        {
                            detailsDTOList[0].MFGDate = headerDTO.MFGDateTime.Date;
                        }
                        foreach (RecipeManufacturingDetailsDTO recipeDetailsDTO in detailsDTOList)
                        {
                            if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                            {
                                decimal factor = 1;
                                int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == recipeDetailsDTO.ProductId).InventoryUOMId;
                                recipeDetailsDTO.ItemUOM = UOMContainer.uomDTOList.Find(x => x.UOMId == recipeDetailsDTO.MfgUOMId).UOM;
                                if (recipeDetailsDTO.ActualMfgUOMId != inventoryUOMId)
                                {
                                    factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(recipeDetailsDTO.ActualMfgUOMId, inventoryUOMId));
                                }
                                ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipeDetailsDTO.ProductId);
                                if (recipeDetailsDTO.IsParentItem == true)
                                {
                                    recipeDetailsDTO.ItemName = productDTO.Description;
                                }
                                else
                                {
                                    
                                    InventoryList inventoryList = new InventoryList(executioncontext);
                                    decimal stock = inventoryList.GetProductStockQuantity(recipeDetailsDTO.ProductId);
                                    decimal actualQty = Convert.ToDecimal(recipeDetailsDTO.Quantity) * factor;
                                    if (headerDTO.IsComplete == false && (stock <= 0 || actualQty > stock))
                                    {
                                        lblMessage.Text = MessageContainerList.GetMessage(executioncontext, 2762, productDTO.Description);
                                        lblMessage.ForeColor = Color.Red;
                                        //There is no sufficient quantity for &1 to prepare a recipe Please update the stock
                                    }
                                    recipeDetailsDTO.ItemName = sOffset + productDTO.Description;
                                }
                                recipeDetailsDTO.ActualCost = Convert.ToDecimal(productDTO.Cost) * recipeDetailsDTO.ActualMfgQuantity * factor;
                                recipeDetailsDTO.ItemCost = Convert.ToDecimal(productDTO.Cost);
                                recipeDetailsDTO.PlannedCost = Convert.ToDecimal(productDTO.Cost) * recipeDetailsDTO.Quantity * factor;
                                recipeDetailsDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == inventoryUOMId).UOM;
                            }
                        }
                        manufacturingDetailsDTOListOnDisplay.AddRange(detailsDTOList);
                    }
                }
                if (!isExport && manufacturingDetailsDTOListOnDisplay != null && manufacturingDetailsDTOListOnDisplay.Any())
                {
                    recipeManufacturingDetailsDTOBindingSource.DataSource = new List<RecipeManufacturingDetailsDTO>(manufacturingDetailsDTOListOnDisplay);
                    GetActualUOM();
                    //if (recipeManufacturingHeaderDTOList[0].IsComplete)
                    //{
                    //    isUIEnabled = false;
                    //    dgvKPN.Enabled = false;
                    //}
                }
                else if (isExport)
                {
                    tempBindingSource.DataSource = new List<RecipeManufacturingDetailsDTO>(manufacturingDetailsDTOListOnDisplay);
                }
                else
                {
                    recipeManufacturingDetailsDTOBindingSource.DataSource = new List<RecipeManufacturingDetailsDTO>(manufacturingDetailsDTOListOnDisplay);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Export Kitchen print note.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (manufacturingDetailsDTOListOnDisplay != null && manufacturingDetailsDTOListOnDisplay.Count > 0)
            {
                bool exported = KitchenProductionSheet();
                if (exported)
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executioncontext, "Export Successful");
                    lblMessage.ForeColor = Color.Blue;
                }
                else
                {
                    lblMessage.Text = MessageContainerList.GetMessage(executioncontext, "Export Failed");
                    lblMessage.ForeColor = Color.Blue;
                }
            }
            log.LogMethodExit();
        }

        private bool KitchenProductionSheet()
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            bool exportSuccessful = false;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                frmExportDateRangeUI frmExportDateRangeUI = new frmExportDateRangeUI(utilities);
                DialogResult statusResult = new System.Windows.Forms.DialogResult();
                statusResult = frmExportDateRangeUI.ShowDialog();
                if (statusResult == DialogResult.OK)
                {
                    LoadGrid(frmExportDateRangeUI.FromDate, frmExportDateRangeUI.ToDate, true);
                    for (int i = 0; i < manufacturingDetailsDTOListOnDisplay.Count; i++)
                    {
                        dgvTempKPN.Rows[i].Cells["ActualMfgUOM"].Value = UOMContainer.uomDTOList.Find(x => x.UOMId == manufacturingDetailsDTOListOnDisplay[i].ActualMfgUOMId).UOM;
                    }
                    utilities.CreateExcelFromGrid(dgvTempKPN, "Kitchen Production Sheet", "Kitchen Production Sheet", null,
                                                    utilities.getServerTime(), utilities.getServerTime(), false);
                    exportSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            log.LogMethodExit(exportSuccessful);
            return exportSuccessful;
        }

        private void dgvKPN_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            LoadGrid(dtpProdDate.Value);
            log.LogMethodExit();
        }

        private void dgvKPN_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = "Error in KPN grid data at row " + (e.RowIndex + 1).ToString() + ", Column " 
                    + dgvKPN.Columns[e.ColumnIndex].DataPropertyName +
                    ": " + e.Exception.Message;
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
