/********************************************************************************************
 * Project Name - Redemption
 * Description  - Redemption Currency
 * 
 **************
 **Version Log
 **************
 *Version       Date                 Modified By          Remarks          
 *********************************************************************************************
 *2.70.2          12-Aug-2019          Deeksha              Modified logger methods.
 *2.70.2          16-Sep-2019          Dakshakh raj         Redemption currency rule enhancement   
 *2.110.0         02-Nov-2020          Mushahid Faizan      passed execution context & namespace changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.BarcodeUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.Redemption
{
    public partial class RedemptionCurrencyUI : Form
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private Semnox.Core.Utilities.Utilities utilities;
        private BindingSource redemptionCurrencyListBS;
        List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleList;

        public RedemptionCurrencyUI()
        { }

        /// <summary>
        /// RedemptionCurrencyUI
        /// </summary>
        /// <param name="_Utilities">_Utilities</param>
        public RedemptionCurrencyUI(Semnox.Core.Utilities.Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            try
            {
                InitializeComponent();
                utilities = _Utilities;
                utilities.setLanguage(this);
                utilities.setupDataGridProperties(ref dgvRedemptionCurrency);
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
                if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
                {
                    lnkPublishToSite.Visible = true;
                }
                else
                {
                    lnkPublishToSite.Visible = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void RedemptionCurrencyUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateRedemptionCurrency();
            LoadRedemptionCurrencyRuleList();
            log.LogMethodExit();
        }

        private void PopulateRedemptionCurrency()
        {
            log.LogMethodEntry();
            try
            {
                PopulateProducts();
                RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(machineUserContext);
                List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> redemptionCurrencySearchParams = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
                redemptionCurrencySearchParams.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<RedemptionCurrencyDTO> redemptionCurrencyListOnDisplay = redemptionCurrencyList.GetAllRedemptionCurrency(redemptionCurrencySearchParams);
                redemptionCurrencyListBS = new BindingSource();

                if (redemptionCurrencyListOnDisplay != null)
                    redemptionCurrencyListBS.DataSource = new SortableBindingList<RedemptionCurrencyDTO>(redemptionCurrencyListOnDisplay);
                else
                {
                    redemptionCurrencyListBS.DataSource = new SortableBindingList<RedemptionCurrencyDTO>();
                }

                redemptionCurrencyListBS.AddingNew += dgvRedemptionCurrency_BindingSourceAddNew;
                dgvRedemptionCurrency.DataSource = redemptionCurrencyListBS;
                dgvRedemptionCurrency.Columns["lastUpdatedDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();

            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadRedemptionCurrencyRuleList()
        {
            log.LogMethodEntry();
            redemptionCurrencyRuleList = new List<RedemptionCurrencyRuleDTO>();
            RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(machineUserContext);
            List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> redemptionCurrencySearchParams = new List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>>();
            redemptionCurrencySearchParams.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            redemptionCurrencySearchParams.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.IS_ACTIVE, "1"));
            redemptionCurrencyRuleList = redemptionCurrencyRuleListBL.GetAllRedemptionCurrencyRuleList(redemptionCurrencySearchParams, true, true);
            if (redemptionCurrencyRuleList == null)
            {
                redemptionCurrencyRuleList = new List<RedemptionCurrencyRuleDTO>();
            }
        }

        private void PopulateProducts()
        {
            log.LogMethodEntry();
            try
            {
                ProductList productList = new Product.ProductList();
                List<ProductDTO> productDTOList = new List<ProductDTO>();
                List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                productSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                productDTOList = productList.GetAllProducts(productSearchParams);
                if (productDTOList == null)
                {
                    productDTOList = new List<ProductDTO>();
                }
                productDTOList.Insert(0, new ProductDTO());
                productDTOList[0].Code = "<Select>";
                productDTOList[0].ProductId = -1;
                BindingSource productListBS = new BindingSource();
                productListBS.DataSource = productDTOList;

                productIdDataGridViewTextBoxColumn.DataSource = productListBS;
                productIdDataGridViewTextBoxColumn.ValueMember = "ProductId";
                productIdDataGridViewTextBoxColumn.DisplayMember = "Code";
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void dgvRedemptionCurrency_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvRedemptionCurrency.Rows.Count == redemptionCurrencyListBS.Count)
                {
                    redemptionCurrencyListBS.RemoveAt(redemptionCurrencyListBS.Count - 1);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadRedemptionCurrencyRuleList();
            try
            {
                BindingSource redemptionCurrencyListBS = (BindingSource)dgvRedemptionCurrency.DataSource;
                var redemptionCurrencyListOnDisplay = (SortableBindingList<RedemptionCurrencyDTO>)redemptionCurrencyListBS.DataSource;
                List<RedemptionCurrencyDTO> tempList = new List<RedemptionCurrencyDTO>(redemptionCurrencyListOnDisplay);
                if (tempList != null && tempList.Count > 0)
                {
                    var query = tempList.GroupBy(x => new { x.CurrencyName })
                   .Where(g => g.Count() > 1)
                   .Select(y => y.Key)
                   .ToList();
                    if (query.Count > 0)
                    {
                        log.Debug("Duplicate entries detail : " + query[0]);
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2608, "currency"), "Validation Error");
                        return;
                    }
                }
                if (redemptionCurrencyListOnDisplay.Count > 0)
                {
                    foreach (RedemptionCurrencyDTO redemptionCurrencyDTO in redemptionCurrencyListOnDisplay)
                    {
                        if (redemptionCurrencyDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(redemptionCurrencyDTO.CurrencyName.Trim()))
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1586));
                                log.LogMethodExit();
                                return;
                            }
                            if (redemptionCurrencyDTO.IsActive == false && redemptionCurrencyRuleList.Exists(rD => rD.RedemptionCurrencyRuleDetailDTOList.Exists(xD => xD.CurrencyId == Convert.ToInt32(redemptionCurrencyDTO.CurrencyId))))
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(2606));//Sorry, cannot deactivate the currency. It is mapped to active currency rule
                                log.LogMethodExit();
                                return;
                            }

                            if (!String.IsNullOrEmpty(redemptionCurrencyDTO.ShortCutKeys))
                            {
                                if (redemptionCurrencyDTO.ShortCutKeys.Length > 5)
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage(1587, redemptionCurrencyDTO.CurrencyName));
                                    log.LogMethodExit();
                                    return;
                                }
                                else
                                {
                                    char[] shortCutKeyArray = redemptionCurrencyDTO.ShortCutKeys.ToCharArray();
                                    foreach (char charValue in shortCutKeyArray)
                                    {
                                        if (!Char.IsLetterOrDigit(charValue))
                                        {
                                            MessageBox.Show(utilities.MessageUtils.getMessage(1588, redemptionCurrencyDTO.CurrencyName));
                                            log.LogMethodExit();
                                            return;
                                        }
                                    }
                                    List<RedemptionCurrencyDTO> duplicateShortCutKey = redemptionCurrencyListOnDisplay.Where(rcDTO => (rcDTO.ShortCutKeys == redemptionCurrencyDTO.ShortCutKeys && rcDTO.CurrencyId != redemptionCurrencyDTO.CurrencyId && rcDTO.IsActive == true)).ToList();
                                    if (duplicateShortCutKey.Count > 0)
                                    {
                                        MessageBox.Show(utilities.MessageUtils.getMessage(1589, redemptionCurrencyDTO.ShortCutKeys));
                                        log.LogMethodExit();
                                        return;
                                    }
                                }
                            }
                        }

                        RedemptionCurrency redemptionCurrency = new RedemptionCurrency(machineUserContext, redemptionCurrencyDTO);
                        redemptionCurrency.Save();
                    }
                    PopulateRedemptionCurrency();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateRedemptionCurrency();
            LoadRedemptionCurrencyRuleList();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadRedemptionCurrencyRuleList();
            try
            {
                if (this.dgvRedemptionCurrency.SelectedRows.Count <= 0 && this.dgvRedemptionCurrency.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvRedemptionCurrency.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvRedemptionCurrency.SelectedCells)
                    {
                        dgvRedemptionCurrency.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow redemptionCurrencyRow in this.dgvRedemptionCurrency.SelectedRows)
                {
                    if (redemptionCurrencyRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(redemptionCurrencyRow.Cells[0].Value) < 0)
                        {
                            dgvRedemptionCurrency.Rows.RemoveAt(redemptionCurrencyRow.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (redemptionCurrencyRuleList.Exists(rD => rD.RedemptionCurrencyRuleDetailDTOList.Exists(xD => xD.CurrencyId == Convert.ToInt32(redemptionCurrencyRow.Cells[0].Value) && xD.IsActive == true)))
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(2606));//Sorry, cannot deactivate the currency. It is mapped to active currency rule
                            }

                            else
                            {

                                if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    BindingSource redemptionCurrencyDTOListDTOBS = (BindingSource)dgvRedemptionCurrency.DataSource;
                                    var redemptionCurrencyDTOList = (SortableBindingList<RedemptionCurrencyDTO>)redemptionCurrencyDTOListDTOBS.DataSource;
                                    RedemptionCurrencyDTO redemptionCurrencyDTO = redemptionCurrencyDTOList[redemptionCurrencyRow.Index];
                                    redemptionCurrencyDTO.IsActive = false;
                                    RedemptionCurrency redemptionCurrency = new RedemptionCurrency(machineUserContext,redemptionCurrencyDTO);
                                    redemptionCurrency.Save();
                                }
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateRedemptionCurrency();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("Delete failed!!!.\n Error: " + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Dispose();
            log.LogMethodExit();
        }

        private void dgvRedemptionCurrency_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvRedemptionCurrency.Columns[e.ColumnIndex].Name == "GenerateBarcode" && e.RowIndex > -1)
                {
                    int currencyId = Convert.ToInt32(dgvRedemptionCurrency.CurrentRow.Cells["currencyIdDataGridViewTextBoxColumn"].Value);
                    if (currencyId > -1)
                    {
                        int currentIndex = dgvRedemptionCurrency.CurrentRow.Index;
                        frm_barcode f = new frm_barcode(dgvRedemptionCurrency.CurrentRow.Cells["barCodeDataGridViewTextBoxColumn"].Value.ToString() == "" ? dgvRedemptionCurrency.CurrentRow.Cells["currencyNameDataGridViewTextBoxColumn"].Value.ToString() : dgvRedemptionCurrency.CurrentRow.Cells["barcodeDataGridViewTextBoxColumn"].Value.ToString(), utilities);
                        f.StartPosition = FormStartPosition.CenterScreen;//Added for showing at center on 23-Sep-2016
                        if (f.ShowDialog() == DialogResult.OK)
                            dgvRedemptionCurrency.Rows[currentIndex].Cells["barCodeDataGridViewTextBoxColumn"].Value = BarcodeUtilities.BarcodeReader.Barcode;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PublishUI publishUI;

            if (dgvRedemptionCurrency.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvRedemptionCurrency.SelectedCells)
                {
                    dgvRedemptionCurrency.Rows[cell.RowIndex].Selected = true;
                }
            }
            if (dgvRedemptionCurrency.SelectedRows.Count > 0)
            {
                if (dgvRedemptionCurrency.SelectedRows[0].Cells["currencyNameDataGridViewTextBoxColumn"].Value != null)
                {
                    publishUI = new PublishUI(utilities, Convert.ToInt32(dgvRedemptionCurrency.SelectedRows[0].Cells["currencyIdDataGridViewTextBoxColumn"].Value), "RedemptionCurrency", dgvRedemptionCurrency.SelectedRows[0].Cells["currencyNameDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnCurrencyRule_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void btnCurrencyRule_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            frmRedemptionCurrencyRuleUI frmRedemptionCurrencyRuleUI = new frmRedemptionCurrencyRuleUI(utilities);
            foreach (Form child_form in Application.OpenForms)
            {
                if (child_form.Name == "frmRedemptionCurrencyRuleUI")
                {
                    child_form.Close();
                    break;
                }
            }
            CommonUIDisplay.setupVisuals(frmRedemptionCurrencyRuleUI);
            frmRedemptionCurrencyRuleUI.Location = new Point(this.Location.X, this.Location.Y);
            frmRedemptionCurrencyRuleUI.Width = this.Width;
            frmRedemptionCurrencyRuleUI.Height = this.Height;
            frmRedemptionCurrencyRuleUI.MdiParent = this.MdiParent;
            frmRedemptionCurrencyRuleUI.Show();
            log.LogMethodEntry();
        }
    }
}

