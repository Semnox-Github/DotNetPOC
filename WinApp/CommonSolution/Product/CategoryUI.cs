using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Semnox.Core.SortableBindingList;
//using Semnox.Parafait.Context;
//using Semnox.Core.Category;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using Semnox.Parafait.SortableBindingList;

namespace Semnox.Parafait.Product
{
    public partial class CategoryUI : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Utilities utilities;
        BindingSource categoryListBS;
        
        public CategoryUI()
        {
            
        }

        public CategoryUI(Utilities _Utilities)
        {
            log.Debug("Starts-CategoryUI(_Utilities) constructor.");
            try
            {
                InitializeComponent();
                utilities = _Utilities;
                utilities.setupDataGridProperties(ref dgvCategory);
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

                dgvCategory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                //Added on 11-6-2016 for adding category details
                dgvCategory.Columns["Assign"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgvCategory.Columns["Assign"].Width = 60;
                log.Debug("Ends-CategoryUI(_Utilities) constructor.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-CategoryUI(_Utilities) constructor.");
                MessageBox.Show(ex.Message);
            }
        }

        private void CategoryUI_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-CategoryUI_Load() method.");
            PopulateCategory();
            log.Debug("Starts-CategoryUI_Load() method.");
        }

        private void PopulateCategory()
        {
            log.Debug("Starts-PopulateCategory() method.");
            try
            {
                CategoryList categoryList = new CategoryList();
                List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categorySearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                categorySearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                List<CategoryDTO> categoryListOnDisplay = categoryList.GetAllCategory(categorySearchParams);
                categoryListBS = new BindingSource();

                if (categoryListOnDisplay != null)
                    categoryListBS.DataSource = new SortableBindingList<CategoryDTO>(categoryListOnDisplay);
                else
                {
                    categoryListBS.DataSource = new SortableBindingList<CategoryDTO>();
                }

                categoryListBS.AddingNew += dgvCategory_BindingSourceAddNew;
                dgvCategory.DataSource = categoryListBS;
                PopulateParentCategory();
                log.Debug("Ends-PopulateCategory() method.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-PopulateCategory() method.");
                MessageBox.Show(ex.Message);
            }
        }

        void PopulateParentCategory()
        {
            log.Debug("Starts-PopulateParentCategory() method.");
            try
            {
                CategoryList categoryList = new CategoryList();
                List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
                List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>> categorySearchParams = new List<KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>>();
                categorySearchParams.Add(new KeyValuePair<CategoryDTO.SearchByCategoryParameters, string>(CategoryDTO.SearchByCategoryParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                categoryDTOList = categoryList.GetAllCategory(categorySearchParams);
                if (categoryDTOList == null)
                {
                    categoryDTOList = new List<CategoryDTO>();
                }
                categoryDTOList.Insert(0, new CategoryDTO());
                categoryDTOList[0].Name = "<Select>";
                categoryDTOList[0].CategoryId = -1;
                BindingSource parentCategoryListBS = new BindingSource();
                parentCategoryListBS.DataSource = categoryDTOList;

                parentCategoryIdDataGridViewTextBoxColumn.DataSource = parentCategoryListBS;
                parentCategoryIdDataGridViewTextBoxColumn.ValueMember = "CategoryId";
                parentCategoryIdDataGridViewTextBoxColumn.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                log.Debug("Ends-PopulateParentCategory() method.");
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvCategory_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.Debug("Starts-dgvCategory_BindingSourceAddNew() Event.");
            try
            {
                if (dgvCategory.Rows.Count == categoryListBS.Count)
                {
                    categoryListBS.RemoveAt(categoryListBS.Count - 1);
                }
                log.Debug("Ends-dgvCategory_BindingSourceAddNew() Event.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-dgvCategory_BindingSourceAddNew() Event.");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSave_Click() Event.");
            try
            {
                BindingSource categoryListBS = (BindingSource)dgvCategory.DataSource;
                var categoryListOnDisplay = (SortableBindingList<CategoryDTO>)categoryListBS.DataSource;
                if (categoryListOnDisplay.Count > 0)
                {
                    foreach (CategoryDTO categoryDTO in categoryListOnDisplay)
                    {
                        if (categoryDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(categoryDTO.Name))
                            {
                                MessageBox.Show("Please enter the name.");
                                return;
                            }
                        }

                        Category category = new Category(categoryDTO);
                        category.Save();
                    }
                    PopulateCategory();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
                log.Debug("Ends-btnSave_Click() Event.");
            }
            catch (Exception ex)
            {
                log.Debug("Ends-btnSave_Click() Event.");
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateCategory();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnDelete_Click() event.");
            try
            {
                if (this.dgvCategory.SelectedRows.Count <= 0 && this.dgvCategory.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvCategory.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvCategory.SelectedCells)
                    {
                        dgvCategory.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow categoryRow in this.dgvCategory.SelectedRows)
                {
                    if (categoryRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(categoryRow.Cells[0].Value) < 0)
                        {
                            dgvCategory.Rows.RemoveAt(categoryRow.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                BindingSource purchaseTaxDTOListDTOBS = (BindingSource)dgvCategory.DataSource;
                                var categoryDTOList = (SortableBindingList<CategoryDTO>)purchaseTaxDTOListDTOBS.DataSource;
                                CategoryDTO categoryDTO = categoryDTOList[categoryRow.Index];
                                categoryDTO.IsActive = "N";
                                Category category = new Category(categoryDTO);
                                category.Save();
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateCategory();
                log.Debug("Ends-btnDelete_Click() event.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnDelete_Click() event with exception: " + ex.ToString());
                MessageBox.Show("Delete failed!!!.\n Error: " + ex.Message);
            }
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.Debug("Starts-dgvCategory_CellContentClick() event.");
            try
            {
                if (dgvCategory.Columns[e.ColumnIndex].Name == "Assign" && e.RowIndex > -1)
                {
                    int categoryId = Convert.ToInt32(dgvCategory.CurrentRow.Cells["categoryIdDataGridViewTextBoxColumn"].Value);
                    if (categoryId > -1)
                    {
                        AccountingCodeCombinationUI f = new AccountingCodeCombinationUI(utilities, categoryId);
                        f.StartPosition = FormStartPosition.CenterScreen;//Added for showing at center on 23-Sep-2016
                        f.ShowDialog();
                    }
                }
                log.Debug("Ends-dgvCategory_CellContentClick() event.");
            }
            catch 
            {
                log.Debug("Ends-dgvCategory_CellContentClick() event.");
            }
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.Debug("Satrts-lnkPublishToSite_LinkClicked() event.");
            PublishUI publishUI;
            if (dgvCategory.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvCategory.SelectedCells)
                {
                    dgvCategory.Rows[cell.RowIndex].Selected = true;
                }
            }
            if (dgvCategory.SelectedRows.Count > 0)
            {
                if (dgvCategory.SelectedRows[0].Cells["nameDataGridViewTextBoxColumn"].Value != null)
                {
                    publishUI = new PublishUI(utilities, Convert.ToInt32(dgvCategory.SelectedRows[0].Cells["categoryIdDataGridViewTextBoxColumn"].Value), "Category", dgvCategory.SelectedRows[0].Cells["nameDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            log.Debug("Ends-lnkPublishToSite_LinkClicked() event.");
        }
    }
}
