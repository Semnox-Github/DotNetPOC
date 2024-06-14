/********************************************************************************************
 * Project Name - Display Group
 * Description  - UI for Display Group
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 ********************************************************************************************* 
* 2.80        28-Jun-2020   Deeksha        Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Product
{
    public partial class DisplayGroup : Form
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        ExecutionContext machineUserContext;
        public int displayGroupId = -1;
        public string displayGroup = string.Empty;
        private ManagementStudioSwitch managementStudioSwitch;

        public DisplayGroup(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvDisplayGroup);

            machineUserContext = ExecutionContext.GetExecutionContext();
            lnkHQPublish.Visible = false;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                if (utilities.ParafaitEnv.IsMasterSite)
                {
                    lnkHQPublish.Visible = true;
                }
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);

            dgvDisplayGroup.Columns["displayGroupDataGridViewTextBoxColumn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvDisplayGroup.Columns["displayGroupDataGridViewTextBoxColumn"].Width = 150;

            PopulateDisplayGroupGrid();
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }

        public void PopulateDisplayGroupGrid()
        {
            log.LogMethodEntry();
            try
            {
                List<ProductDisplayGroupFormatDTO> ProductDisplayGroupList = new List<ProductDisplayGroupFormatDTO>();
                List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParam = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
                searchParam.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, utilities.ParafaitEnv.IsCorporate ? utilities.ParafaitEnv.SiteId.ToString() : "-1"));
                ProductDisplayGroupList = new ProductDisplayGroupList(machineUserContext).GetAllProductDisplayGroup(searchParam);

                List<ProductDisplayGroupFormatDTO> ProductDisplayGroupSortList = new List<ProductDisplayGroupFormatDTO>(ProductDisplayGroupList);
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = ProductDisplayGroupSortList;
                dgvDisplayGroup.DataSource = bindingSource;
            }
            catch { }

            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();

            try
            {
                BindingSource displayGroupListBS = (BindingSource)dgvDisplayGroup.DataSource;
                var displayGroupListOnDisplay = (List<ProductDisplayGroupFormatDTO>)displayGroupListBS.DataSource;
                ProductDisplayGroupFormat productDisplayGroupFormat;

                if (displayGroupListOnDisplay != null && displayGroupListOnDisplay.Count > 0)
                {
                    foreach (ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO in displayGroupListOnDisplay)
                    {
                        if (string.IsNullOrEmpty(productDisplayGroupFormatDTO.DisplayGroup) || productDisplayGroupFormatDTO.DisplayGroup.Trim() == string.Empty)
                        {
                            continue;
                        }

                        //Check inserting Duplicate displaygroup
                        if (productDisplayGroupFormatDTO.IsChanged && !(productDisplayGroupFormatDTO.Id == -1 && IsExistDisplayGroup(productDisplayGroupFormatDTO.DisplayGroup)))
                        {
                            //Check Updating Duplicate displaygroup
                            if (productDisplayGroupFormatDTO.Id != -1 && !IsValidMadification(productDisplayGroupFormatDTO))
                            {
                                //Invalid Modification Skip this
                                continue;
                            }
                            //Save displaygroup with trim
                            productDisplayGroupFormatDTO.DisplayGroup = productDisplayGroupFormatDTO.DisplayGroup.Trim();
                            productDisplayGroupFormat = new ProductDisplayGroupFormat(machineUserContext, productDisplayGroupFormatDTO);
                            productDisplayGroupFormat.Save();
                        }
                    }
                    PopulateDisplayGroupGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        //Added to Validate duplicate display group name
        bool IsExistDisplayGroup(string displayGroup)
        {
            log.LogMethodEntry(displayGroup);
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(machineUserContext);

            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParam = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            searchParam.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, utilities.ParafaitEnv.IsCorporate ? utilities.ParafaitEnv.SiteId.ToString() : "-1"));
            searchParam.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP, displayGroup.Trim().ToString()));
            List<ProductDisplayGroupFormatDTO> productDisplayGroupListOnDisplay = productDisplayGroupList.GetAllProductDisplayGroup(searchParam);

            if (productDisplayGroupListOnDisplay != null && productDisplayGroupListOnDisplay.Count > 0)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        bool IsValidMadification(ProductDisplayGroupFormatDTO productDisplayGroupFormatDTO)
        {
            log.LogMethodEntry(productDisplayGroupFormatDTO);
            ProductDisplayGroupList productDisplayGroupList = new ProductDisplayGroupList(machineUserContext);

            List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>> searchParam = new List<KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>>();
            searchParam.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.DISPLAY_GROUP, productDisplayGroupFormatDTO.DisplayGroup.ToString()));
            searchParam.Add(new KeyValuePair<ProductDisplayGroupFormatDTO.SearchByDisplayParameters, string>(ProductDisplayGroupFormatDTO.SearchByDisplayParameters.SITE_ID, utilities.ParafaitEnv.IsCorporate ? utilities.ParafaitEnv.SiteId.ToString() : "-1"));
            List<ProductDisplayGroupFormatDTO> productDisplayGroupListOnDisplay = productDisplayGroupList.GetAllProductDisplayGroup(searchParam);

            if (productDisplayGroupListOnDisplay != null && productDisplayGroupListOnDisplay.Count == 1)
            {
                if (productDisplayGroupFormatDTO.Id == productDisplayGroupListOnDisplay[0].Id)
                {
                    log.LogMethodExit(true);
                    return true;
                }
            }
            else if (productDisplayGroupListOnDisplay == null || productDisplayGroupListOnDisplay.Count == 0)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (dgvDisplayGroup.CurrentRow == null || dgvDisplayGroup.CurrentRow.IsNewRow || dgvDisplayGroup.CurrentRow.Cells[0].Value == DBNull.Value)
                    return;
                else
                {
                    DialogResult result1 = MessageBox.Show("Do you want to delete ?", "Delete DisplayGroup", MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        int displayGroupId = Convert.ToInt32(dgvDisplayGroup.CurrentRow.Cells[0].Value);

                        ProductDisplayGroupFormat productDisplayGroupFormat = new ProductDisplayGroupFormat(machineUserContext);
                        int deleteStatus = productDisplayGroupFormat.Delete(displayGroupId);
                        if (deleteStatus > 0)
                        {
                            MessageBox.Show("Display Group Deleted .");
                        }
                        else
                        {
                            MessageBox.Show("Error occurred ! Please Retry again.");
                        }
                        PopulateDisplayGroupGrid();
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                MessageBox.Show(expn.Message.ToString());
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateDisplayGroupGrid();
            log.LogMethodExit();
        }

        private void dgvDisplayGroup_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvDisplayGroup.Columns[e.ColumnIndex].Name == "sortOrderDataGridViewTextBoxColumn")
            {
                char[] chars = e.FormattedValue.ToString().ToCharArray();
                foreach (char c in chars)
                {
                    if (char.IsDigit(c) == false)
                    {
                        MessageBox.Show("You have to enter digits only");
                        e.Cancel = true;
                        break;
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgvDisplayGroup_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MessageBox.Show("Error in DisplayGroup grid data at row " + (e.RowIndex + 1).ToString() + ", Column " + dgvDisplayGroup.Columns[e.ColumnIndex].DataPropertyName +
                   ": " + e.Exception.Message);
                e.Cancel = true;
            }
            catch { }
            log.LogMethodExit();
        }

        private void lnkHQPublish_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            object publishUiInstance = null;
            Type type = Type.GetType("Semnox.Parafait.Publish.PublishUI,Publish");
            try
            {
                if (type != null)
                {
                    if (dgvDisplayGroup.SelectedCells != null && dgvDisplayGroup.SelectedCells.Count > 0)
                    {
                        dgvDisplayGroup.Rows[dgvDisplayGroup.SelectedCells[0].RowIndex].Selected = true;
                    }

                    ConstructorInfo constructor = type.GetConstructor(new[] { typeof(Utilities), typeof(int), typeof(string), typeof(string) });
                    publishUiInstance = constructor.Invoke(new object[] { utilities, Convert.ToInt32(dgvDisplayGroup.SelectedRows[0].Cells["idDataGridViewTextBoxColumn"].Value), "ProductDisplayGroupFormat", dgvDisplayGroup.SelectedRows[0].Cells["displayGroupDataGridViewTextBoxColumn"].Value.ToString() });
                    ((Form)publishUiInstance).ShowDialog();
                }
                else
                {
                    log.LogMethodExit(null, "Cannot create instance of type:PublishUI. Please check whether the dll exist in application folder.");
                    MessageBox.Show("Cannot create instance of type:PublishUI. Please check whether the dll exist in application folder.", "Publish");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show("Cannot create instance of type:PublishUI. Please check whether the dll exist in application folder.", "Publish");
            }

            log.LogMethodExit();
        }
        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvDisplayGroup.AllowUserToAddRows = true;
                dgvDisplayGroup.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                lnkHQPublish.Enabled = true;
            }
            else
            {
                dgvDisplayGroup.AllowUserToAddRows = false;
                dgvDisplayGroup.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                lnkHQPublish.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
