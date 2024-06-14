/* Project Name - BOMDTO 
* Description  - Data handler object of the BOM
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
********************************************************************************************* 
*2.60.3      14-Jun-2019   Nagesh Badiger          Added who columns property.
*2.100.0     21-Aug-2020   Deeksha                 Enhanced BOM UI to support Multiple UOM selection option,
*                                                  display item type, offset , stock info
*********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System.Collections.Generic;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    public partial class frmBOM : Form
    {
        private int parentProductId;
        private Utilities utilities;
        private ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private BOMDTO bOMDTO = new BOMDTO();
        private List<BOMDTO> bomListOnDisplay = new List<BOMDTO>();
        private List<ProductDTO> productDTOList = null;

        public frmBOM(int productId, Utilities utilities, bool isEditable)
        {
            log.LogMethodEntry(productId, utilities, isEditable);
            InitializeComponent();
            this.utilities = utilities;
            executionContext = utilities.ExecutionContext;
            utilities.setLanguage(this);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            ProductContainer productContainer = new ProductContainer(executionContext);
            UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
            this.parentProductId = productId;
            PopulateProductBOM();
            CalculateTotalRecipeCost();
            if (!isEditable)
            {
                UpdateUIElements();
            }
            log.LogMethodExit();
        }


        private void frmBOM_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                {
                    string productDescription = ProductContainer.productDTOList.Find(x => x.ProductId == parentProductId).Description;
                    lblEditLabel.Text = MessageContainerList.GetMessage(executionContext, "Bill of Material for") + " <" + productDescription + "> ";
                    lblEditLabel.Font = new Font(lblEditLabel.Font, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            utilities.setupDataGridProperties(ref dgvBOM);
            dgvBOM.BackgroundColor = this.BackColor;
            dgvBOM.Columns["stockDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            dgvBOM.Columns["stockDataGridViewTextBoxColumn"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            dgvBOM.Columns["costDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvBOM.Columns["costDataGridViewTextBoxColumn"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;
            dgvBOM.Columns["RecipeCost"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvBOM.Columns["RecipeCost"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_COST_FORMAT;
            dgvBOM.Columns["quantityDataGridViewTextBoxColumn1"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvBOM.Columns["quantityDataGridViewTextBoxColumn1"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;

            dgvBOM.Columns["CreationDate"].DefaultCellStyle.Format = utilities.getDateTimeFormat();
            dgvBOM.Columns["LastUpdateDate"].DefaultCellStyle.Format = utilities.getDateTimeFormat();
            btnSave.Text = MessageContainerList.GetMessage(executionContext, "Save");
            btnRefresh.Text = MessageContainerList.GetMessage(executionContext, "Refresh");
            btnDelete.Text = MessageContainerList.GetMessage(executionContext, "Delete");
            btnExit.Text = MessageContainerList.GetMessage(executionContext, "Close");
            log.LogMethodExit();
        }

        /// <summary>
        /// Product BOM Save /Update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    this.Cursor = Cursors.WaitCursor;
                    dgvBOM.EndEdit();
                    List<BOMDTO> bomList = new List<BOMDTO>();
                    bomList = (List<BOMDTO>)BOMDTOBindingSource.DataSource;
                    BOMList bOMListBL = null;
                    if (bomList != null)
                    {
                        for (int i = 0; i < bomList.Count; i++)
                        {
                            if (parentProductId == bomList[i].ChildProductId)
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 905)); //Child Product cannot be same as its Parent
                                bomList[i] = new BOMDTO();
                                return;
                            }
                            bomList[i].UOMId = Convert.ToInt32(dgvBOM.Rows[i].Cells["cmbUOM"].Value);
                            
                            if (bomList[i].Quantity <= 0)
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 2360)); //Please enter valid Quantity
                                dgvBOM.CurrentCell = dgvBOM.Rows[i].Cells["quantityDataGridViewTextBoxColumn1"];
                                return;
                            }
                            if (bomList[i].BOMId == -1 && bomListOnDisplay.Exists(x => x.ProductId == bomList[i].ProductId & x.ChildProductId == bomList[i].ChildProductId))
                            {
                                decimal qty = bomList.Find(x => x.ProductId == bomListOnDisplay[i].ProductId & x.ChildProductId == bomListOnDisplay[i].ChildProductId).Quantity;
                                bomList[i] = bomListOnDisplay.Find(x => x.ProductId == bomListOnDisplay[i].ProductId & x.ChildProductId == bomListOnDisplay[i].ChildProductId);
                                bomList[i].Quantity = qty;
                                bomList[i].Isactive = true;
                            }
                            
                        }
                        List<BOMDTO> bomDTOList = new List<BOMDTO>(bomList);
                        bOMListBL = new BOMList(executionContext, bomDTOList);
                        bOMListBL.SaveUpdateProductBOM(parafaitDBTrx.SQLTrx);
                        lblMessage.Text = MessageContainerList.GetMessage(executionContext, 122); //"Save Successful"
                        parafaitDBTrx.EndTransaction();
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(371));
                    }
                    dgvBOM.Update();
                    dgvBOM.Refresh();
                    PopulateProductBOM();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 718));
                    parafaitDBTrx.RollBack();
                }
                finally
                { this.Cursor = Cursors.Default; }
            }
            log.LogMethodExit();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        private void dgvBOM_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    if (dgvBOM.Columns[e.ColumnIndex].Name == "itemNameDataGridViewTextBoxColumn")
                    {
                        if (dgvBOM.Columns[e.ColumnIndex].Name == "itemNameDataGridViewTextBoxColumn")
                        {
                            string description = dgvBOM[e.ColumnIndex, e.RowIndex].Value.ToString();
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
                                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 846),
                                    MessageContainerList.GetMessage(executionContext, "Find Products"));
                                BeginInvoke((MethodInvoker)delegate
                                {
                                    RemoveDGVRow(e.RowIndex);
                                });
                            }
                            else if (productDTOList.Count == 1)
                            {
                                BOMDTO bomDTO = BOMDTOBindingSource.Current as BOMDTO;
                                ProductDTO productDTO = productDTOList[0];
                                bomDTO.ChildProductId = productDTO.ProductId;
                                List<BOMDTO> bomDTOListOnDisplay = (List<BOMDTO>)BOMDTOBindingSource.DataSource;
                                var duplicates = bomDTOListOnDisplay.GroupBy(x => x.ChildProductId).Where(x => x.Count() > 1).Select(x => x.Key);
                                if (duplicates.Any())
                                {
                                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1872)); // Cannot add duplicate products.
                                    BeginInvoke((MethodInvoker)delegate
                                    {
                                        RemoveDGVRow(e.RowIndex);
                                    });
                                    return;
                                }
                                int uomId = productDTO.InventoryUOMId;
                                if (uomId != -1)
                                {
                                    CommonFuncs.GetUOMComboboxForSelectedRows(dgvBOM, e.RowIndex, uomId);
                                }
                                bomDTO.Cost = Convert.ToDecimal(productDTO.Cost);
                                int itemType = productDTO.ItemType;
                                bomDTO.ItemType = GetProductItemType(itemType);
                                bomDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productDTO.InventoryUOMId).UOM;
                                bomDTO.UOMId = uomId;
                                bomDTO.ItemName = productDTO.Description;
                                bomDTO.Stock = GetInventoryStock(productDTO.ProductId);
                                bomDTO.ProductId = parentProductId;
                                bomDTO.Quantity = 0;
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
                    }
                    if (dgvBOM.Columns[e.ColumnIndex].Name == "quantityDataGridViewTextBoxColumn1")
                    {
                        CalculateTotalRecipeCost();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void multiple_dgv_Click(object sender, EventArgs e)
        {
            log.LogMethodExit();
            try
            {
                DataGridView dg = (DataGridView)sender;
                ProductDTO productDTO = productDTOList[dg.CurrentRow.Index];
                BOMDTO bomDTO = BOMDTOBindingSource.Current as BOMDTO;
                bomDTO.ChildProductId = productDTO.ProductId;
                List<BOMDTO> bomDTOListOnDisplay = (List<BOMDTO>)BOMDTOBindingSource.DataSource;
                var duplicates = bomDTOListOnDisplay.GroupBy(x => x.ChildProductId).Where(x => x.Count() > 1).Select(x => x.Key);
                if (duplicates.Any())
                {
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1872)); // Cannot add duplicate products.
                    BeginInvoke((MethodInvoker)delegate
                    {
                        RemoveDGVRow(bomDTOListOnDisplay.Count - 1);
                    });
                    return;
                }
                int uomId = Convert.ToInt32(dg.Rows[dg.CurrentRow.Index].Cells["InventoryUOMId"].Value);
                if (uomId != -1)
                {
                    CommonFuncs.GetUOMComboboxForSelectedRows(dgvBOM, dgvBOM.CurrentRow.Index, uomId);
                }
                bomDTO.Cost = Convert.ToDecimal(productDTO.Cost);
                bomDTO.ItemName = productDTO.Description;
                int itemType = productDTO.ItemType;
                bomDTO.ItemType = GetProductItemType(itemType);
                bomDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productDTO.InventoryUOMId).UOM;
                bomDTO.UOMId = uomId;
                bomDTO.ProductId = parentProductId;
                bomDTO.Stock = GetInventoryStock(productDTO.ProductId);
                bomDTO.Quantity = 0;
                bomDTO.RecipeCost = 0;
                dgvBOM.Refresh();
                dg.Parent.Visible = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void multiple_dgv_Format(ref Panel pnlMultiple_dgv, ref DataGridView multiple_dgv)
        {
            log.LogMethodEntry(pnlMultiple_dgv, multiple_dgv);
            try
            {
                pnlMultiple_dgv.Size = new Size(300, (dgvBOM.Rows[0].Cells[0].Size.Height * 10) - 3); //Changed the function so that the grid is in a panel and scroll bar can be added if list is long
                pnlMultiple_dgv.AutoScroll = true;
                pnlMultiple_dgv.Location = new Point(150 + dgvBOM.RowHeadersWidth + dgvBOM.CurrentRow.Cells["itemNameDataGridViewTextBoxColumn"].ContentBounds.Location.X, dgvBOM.Location.Y + dgvBOM.ColumnHeadersHeight);
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

        void multiple_dgv_LostFocus(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                DataGridView dg = (DataGridView)sender;
                if (dg.SelectedRows.Count == 0)
                {
                    dgvBOM.Rows.Remove(dgvBOM.Rows[dgvBOM.CurrentRow.Index]);
                }
                dg.Visible = false;
                dg.Parent.Visible = false;
                this.Controls.Remove(dg.Parent);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to populate the active BOM records
        /// </summary>
        /// <param name="productId"></param>
        private void PopulateProductBOM()
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = string.Empty;
                BOMDTOBindingSource.DataSource = new List<BOMDTO>();
                BOMList bomListBL = new BOMList(executionContext);
                List<BOMDTO> activeBOMRecordsList = new List<BOMDTO>();
                List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> bomSearchParams = new List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>();
                bomSearchParams.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.SITEID, executionContext.GetSiteId().ToString()));
                bomSearchParams.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.PRODUCT_ID, parentProductId.ToString()));
                bomListOnDisplay = bomListBL.GetAllBOMs(bomSearchParams);
                if (bomListOnDisplay != null && bomListOnDisplay.Count > 0)
                {
                    activeBOMRecordsList = bomListOnDisplay.FindAll(x => x.Isactive == true).ToList();
                    if (activeBOMRecordsList != null && activeBOMRecordsList.Any())
                    {
                        for (int i = 0; i < activeBOMRecordsList.Count; i++)
                        {
                            if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                            {
                                ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == activeBOMRecordsList[i].ChildProductId);
                                if (productDTO != null)
                                {
                                    activeBOMRecordsList[i].ItemName = productDTO.Description;
                                    activeBOMRecordsList[i].ItemType = GetProductItemType(productDTO.ItemType);
                                    activeBOMRecordsList[i].UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == productDTO.InventoryUOMId).UOM;
                                    if (activeBOMRecordsList[i].UOMId == -1)
                                    {
                                        activeBOMRecordsList[i].UOMId = productDTO.InventoryUOMId;
                                    }
                                    activeBOMRecordsList[i].Stock = GetInventoryStock(productDTO.ProductId);
                                    activeBOMRecordsList[i].Cost = Convert.ToDecimal(productDTO.Cost);
                                    
                                }
                            }
                        }
                        BOMDTOBindingSource.DataSource = activeBOMRecordsList;
                    }
                }
                dgvBOM.Refresh();
                CalculateTotalRecipeCost();
                PopulateRelatedUOM();
                dgvBOM.RefreshEdit();
                for (int i = 0; i < activeBOMRecordsList.Count; i++)
                {
                    decimal factor = 1;
                    int invUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == activeBOMRecordsList[i].ChildProductId).InventoryUOMId;
                    if (invUOMId != activeBOMRecordsList[i].UOMId)
                    {
                        factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(activeBOMRecordsList[i].UOMId, invUOMId));
                    }
                    decimal qty = activeBOMRecordsList[i].Quantity * factor;
                    activeBOMRecordsList[i].RecipeCost = Math.Round(Convert.ToDecimal(activeBOMRecordsList[i].Cost) * qty, 2);
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to get the Inventory stock for the product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        private decimal GetInventoryStock(int productId)
        {
            log.LogMethodEntry(productId);
            InventoryList inventoryList = new InventoryList(executionContext);
            decimal stock = inventoryList.GetProductStockQuantity(productId);
            log.LogMethodExit(stock);
            return stock;
        }

        /// <summary>
        /// Method to get the Item Type (Finished , Semi Finished , Standard Items) from the Look ups
        /// </summary>
        /// <param name="lookupValueId"></param>
        /// <returns></returns>
        private string GetProductItemType(int lookupValueId)
        {
            log.LogMethodEntry(lookupValueId);
            string itemType = null;
            try
            {
                if (lookupValueId == -1)
                {
                    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("PRODUCT_ITEM_TYPE", executionContext.GetSiteId());
                    LookupValuesDTO lookUpValueDTO = lookupValuesDTOList.Find(x => x.LookupValue == "STANDARD_ITEM");
                    itemType = lookUpValueDTO.Description;
                }
                else
                {
                    LookupValues lookupValuesBL = new LookupValues(executionContext, lookupValueId);
                    itemType = lookupValuesBL.LookupValuesDTO.Description;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(itemType);
            return itemType;
        }

        /// <summary>
        /// Removes DGV row at the specified index
        /// </summary>
        /// <param name="index"></param>
        private void RemoveDGVRow(int index)
        {
            log.LogMethodEntry();
            try
            {
                if (index > -1)
                {
                    dgvBOM.Rows.RemoveAt(index);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Update UI elements
        /// </summary>
        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            dgvBOM.AllowUserToAddRows = false;
            dgvBOM.ReadOnly = true;
            btnSave.Visible = false;
            btnDelete.Visible = false;
            btnRefresh.Visible = false;
            txtTotalRecipeCost.ReadOnly = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Calculates total Recipe cost
        /// </summary>
        private void CalculateTotalRecipeCost()
        {
            log.LogMethodEntry();
            try
            {
                if (bomListOnDisplay != null && bomListOnDisplay.Count > 0)
                {
                    ProductList productList = new ProductList(executionContext);
                    decimal totalRecipeCost = productList.GetBOMProductCost(parentProductId);
                    txtTotalRecipeCost.Text = totalRecipeCost.ToString(utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Displays Stock information on stock link click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBOM_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvBOM.Columns[e.ColumnIndex].Name == "stockDataGridViewTextBoxColumn")
                {
                    using (frmInventoryStockDetails frmStockDetails = new frmInventoryStockDetails(executionContext, Convert.ToInt32(dgvBOM["childProductIdDataGridViewTextBoxColumn1", dgvBOM.CurrentRow.Index].Value)))
                    {
                        CommonUIDisplay.setupVisuals(frmStockDetails);
                        frmStockDetails.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void frmBOM_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                PopulateRelatedUOM();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Populates related UOM in the combo box
        /// </summary>
        private void PopulateRelatedUOM()
        {
            log.LogMethodEntry();
            try
            {
                List<BOMDTO> bomListOnDisplay = new List<BOMDTO>();
                bomListOnDisplay = (List<BOMDTO>)BOMDTOBindingSource.DataSource;
                for (int i = 0; i < bomListOnDisplay.Count; i++)
                {
                    int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == bomListOnDisplay[i].ChildProductId).InventoryUOMId;
                    if (bomListOnDisplay[i].UOMId != -1)
                    {
                        CommonFuncs.GetUOMComboboxForSelectedRows(dgvBOM, i, inventoryUOMId);
                    }
                    dgvBOM.Rows[i].Cells["cmbUOM"].Value = bomListOnDisplay[i].UOMId;
                    if (bomListOnDisplay[i].BOMId > -1)
                    {
                        dgvBOM.Rows[i].Cells["itemNameDataGridViewTextBoxColumn"].ReadOnly = true;
                    }
                    dgvBOM.Rows[i].Cells["quantityDataGridViewTextBoxColumn1"].Style.BackColor = Color.PowderBlue;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Populates UOM on Header Mouse click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBOM_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.ColumnIndex > -1 && e.RowIndex > -1)
                {
                    PopulateRelatedUOM();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Inactive Product BOM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            lblMessage.Text = string.Empty;
            using (ParafaitDBTransaction parafaiTDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaiTDBTrx.BeginTransaction();
                    if (this.dgvBOM.SelectedRows.Count <= 0 && this.dgvBOM.SelectedCells.Count <= 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(959));
                        log.LogMethodExit();
                        return;
                    }
                    bool confirmDelete = false;
                    if (this.dgvBOM.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in this.dgvBOM.SelectedCells)
                        {
                            dgvBOM.Rows[cell.RowIndex].Selected = true;
                        }
                    }
                    foreach (DataGridViewRow bomRow in this.dgvBOM.SelectedRows)
                    {
                        if (bomRow.Cells[0].Value != null)
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                List<BOMDTO> bomListOnDisplay = new List<BOMDTO>();
                                bomListOnDisplay = (List<BOMDTO>)BOMDTOBindingSource.DataSource;
                                BOMDTO bomDTO = bomListOnDisplay[bomRow.Index];
                                bomDTO.Isactive = false;
                                BOMBL bomBL = new BOMBL(executionContext, bomDTO);
                                bomBL.Save(parafaiTDBTrx.SQLTrx);
                                parafaiTDBTrx.EndTransaction();
                                PopulateProductBOM();
                                lblMessage.Text = MessageContainerList.GetMessage(executionContext, 957); //Rows deleted successfully
                            }
                        }
                    }

                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    parafaiTDBTrx.RollBack();
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1083) + ex.Message);
                    log.LogMethodExit(ex);
                }
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                PopulateProductBOM();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void dgvBOM_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                lblMessage.Text = "Error in BOM grid data at row " + (e.RowIndex + 1).ToString() + ", Column "
                    + dgvBOM.Columns[e.ColumnIndex].DataPropertyName +
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
