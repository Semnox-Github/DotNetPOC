/********************************************************************************************
* Project Name - Product
* Description  - Product Display Group 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        25-Jun-2020      Deeksha            Modified to Make Product module 
*                                                 read only in Windows Management Studio.
* 2.90        28-Aug-2020      Deeksha            Issue Fix : Display group is inaccessable when loaded from the Inventory
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public partial class frmProductsDisplayGroup : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        Utilities utilities;
        int productId;
        private ManagementStudioSwitch managementStudioSwitch;
        private string applicability = string.Empty;
        private const string INVENTORY_PRODUCTS = "PRODUCT";

        /// <summary>
        /// Default constructor of frmProductsDisplayGroup 
        /// </summary>
        /// <param name="_utilites">Utilities </param>
        /// <param name="productId">Product Id</param>
        public frmProductsDisplayGroup(Utilities _utilites, int productId ,  string applicability)
        {
            log.LogMethodEntry(_utilites, productId, applicability);
            InitializeComponent();
            utilities = _utilites;
            this.applicability = applicability;
            utilities.setupDataGridProperties(ref dgvProductsDisplayGroup);

            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.Username);

            utilities.setLanguage(this);
            this.productId = productId;
            PopulateDisplayGroupCombobox();
            PopulateProductsDisplayGroup();
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }


        void PopulateDisplayGroupCombobox()
        {
            log.LogMethodEntry();

            ProductDisplayGroupList productDisplayList = new ProductDisplayGroupList(machineUserContext);
            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> SearchParams = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            SearchParams.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            List<ProductDisplayGroupFormatDTO> DisplayFormatDtoList = productDisplayList.GetAllProductDisplayGroup(SearchParams);

            if (DisplayFormatDtoList == null)
            {
                DisplayFormatDtoList = new List<ProductDisplayGroupFormatDTO>();
            }

            DisplayFormatDtoList.Insert(0, new ProductDisplayGroupFormatDTO());
            DisplayFormatDtoList[0].DisplayGroup = "None";

            displayGroupIdDataGridViewTextBoxColumn.DataSource = DisplayFormatDtoList;
            displayGroupIdDataGridViewTextBoxColumn.DisplayMember = "DisplayGroup";
            displayGroupIdDataGridViewTextBoxColumn.ValueMember = "Id";

            log.LogMethodExit();
        }

        void PopulateProductsDisplayGroup()
        {
            log.LogMethodEntry();

            ProductsDisplayGroupList productsDisplayGrouplist = new ProductsDisplayGroupList(machineUserContext);
            List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> SearchParameters = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
            SearchParameters.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, productId.ToString()));
            List<ProductsDisplayGroupDTO> productDisplayGroupListOnDisplay = productsDisplayGrouplist.GetAllProductsDisplayGroup(SearchParameters);

            BindingSource productDisplayGroupListBS = new BindingSource();

            if (productDisplayGroupListOnDisplay != null)
            {
                productDisplayGroupListBS.DataSource = productDisplayGroupListOnDisplay;
            }
            else
            {
                productDisplayGroupListBS.DataSource = new List<ProductsDisplayGroupDTO>();
            }

            dgvProductsDisplayGroup.DataSource = productDisplayGroupListBS;

            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateProductsDisplayGroup();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvProductsDisplayGroup.CurrentRow == null || dgvProductsDisplayGroup.CurrentRow.Cells[0].Value == DBNull.Value || dgvProductsDisplayGroup.CurrentRow.Cells[0].Value.ToString() == "-1")
                    return;
                else
                {
                    DialogResult result = MessageBox.Show("Do you want to delete ?", "Delete DisplayGroup", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int id = Convert.ToInt32(dgvProductsDisplayGroup.CurrentRow.Cells[0].Value);

                        ProductsDisplayGroup productsDisplayGroup = new ProductsDisplayGroup(machineUserContext);
                        int deleteStatus = productsDisplayGroup.Delete(id);
                        PopulateProductsDisplayGroup();
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                MessageBox.Show(expn.Message.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            BindingSource productsDisplayGroupListBS = (BindingSource)dgvProductsDisplayGroup.DataSource;
            var productsDisplayGroupListOnDisplay = (List<ProductsDisplayGroupDTO>)productsDisplayGroupListBS.DataSource;

            ProductsDisplayGroup productsDisplayGroup;

            if (productsDisplayGroupListOnDisplay != null && productsDisplayGroupListOnDisplay.Count > 0)
            {
                foreach (ProductsDisplayGroupDTO d in productsDisplayGroupListOnDisplay)
                {
                    if (d.DisplayGroupId != -1 && d.IsChanged && !IsExistDisplayGroup(d.DisplayGroupId))
                    {
                        d.ProductId = productId;
                        productsDisplayGroup = new ProductsDisplayGroup(machineUserContext, d);
                        productsDisplayGroup.Save();
                    }
                }
                PopulateProductsDisplayGroup();
            }
            log.LogMethodExit();
        }

        bool IsExistDisplayGroup(int displayGroupId)
        {
            log.LogMethodEntry(displayGroupId);

            ProductsDisplayGroupList productsDisplayGroupList = new ProductsDisplayGroupList(machineUserContext);

            List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> searchParam = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
            searchParam.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.PRODUCT_ID, productId.ToString()));
            searchParam.Add(new KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>(ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters.DISPLAYGROUP_ID, displayGroupId.ToString()));

            List<ProductsDisplayGroupDTO> productDisplayGroupListOnDisplay = productsDisplayGroupList.GetAllProductsDisplayGroup(searchParam);

            if (productDisplayGroupListOnDisplay != null && productDisplayGroupListOnDisplay.Count > 0)
            {
                MessageBox.Show("Duplicate displaygroup can not be added");
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule || applicability == INVENTORY_PRODUCTS)
                {
                    dgvProductsDisplayGroup.AllowUserToAddRows = true;
                dgvProductsDisplayGroup.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvProductsDisplayGroup.AllowUserToAddRows = false;
                dgvProductsDisplayGroup.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
