/********************************************************************************************
 * Project Name - Inventory
 * Description  - ApprovalRule UI
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
  *2.70.2        13-Aug-2019   Deeksha       Added logger methods.
  *2.100.0      14-Oct-2020    Mushahid Faizan   passed machineUserContext .
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Semnox.Parafait.Inventory
{
    public partial class ApprovalRuleUI : Form
    {
        private Utilities utilities;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        public ApprovalRuleUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvApprovalRule);
            utilities.setupDataGridProperties(ref dgvDocumentType);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            LoadUserRoles();
            PopulateDocumentTypeGrid();
            //if (dgvDocumentType.Rows.Count>0)
            //{
            //    PopulateApprovalRuleGrid(Convert.ToInt32(dgvDocumentType.Rows[0].Cells["documentTypeIdDataGridViewTextBoxColumn"].Value));
            //}
            log.LogMethodExit();
        }
        private void PopulateDocumentTypeGrid()
        {
            log.LogMethodEntry();
            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(machineUserContext);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypesByApplicability(new string[] { "PO", "ISSUE" }, machineUserContext.GetSiteId());
            if (inventoryDocumentTypeDTOList!=null)
            {
                dgvDocumentType.DataSource = inventoryDocumentTypeDTOList;
            }
            log.LogMethodExit();
        }
        private void LoadUserRoles()
        {
            log.LogMethodEntry();
            UserRolesList userRolesList = new UserRolesList();
            List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> userRolesSearchParams = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
            userRolesSearchParams.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            List<UserRolesDTO> userRolesDTOList = userRolesList.GetAllUserRoles(userRolesSearchParams);
            if (userRolesDTOList==null)
            {
                userRolesDTOList = new List<UserRolesDTO>();
            }
            userRolesDTOList.Insert(0,new UserRolesDTO());
            userRolesDTOList[0].Role = utilities.MessageUtils.getMessage("<SELECT>");
            roleIdDataGridViewTextBoxColumn.DataSource = userRolesDTOList;
            roleIdDataGridViewTextBoxColumn.ValueMember = "RoleId";
            roleIdDataGridViewTextBoxColumn.DisplayMember = "Role";
            log.LogMethodExit();
        }
        
        private void PopulateApprovalRuleGrid(int documentTypeId)
        {
            log.LogMethodEntry(documentTypeId);
            ApprovalRulesList approvalRuleList = new ApprovalRulesList(machineUserContext);
            List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>> approvalRuleSearchParams = new List<KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>>();
            approvalRuleSearchParams.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            approvalRuleSearchParams.Add(new KeyValuePair<ApprovalRuleDTO.SearchByApprovalRuleParameters, string>(ApprovalRuleDTO.SearchByApprovalRuleParameters.DOCUMENT_TYPE_ID, documentTypeId.ToString()));
            List<ApprovalRuleDTO> approvalRuleDTOList = approvalRuleList.GetAllApprovalRule(approvalRuleSearchParams);
            SortableBindingList<ApprovalRuleDTO> approvalRuleDTOBindingList;
            BindingSource bindingSource = new BindingSource();
            if (approvalRuleDTOList != null)
            {                
                approvalRuleDTOBindingList = new SortableBindingList<ApprovalRuleDTO>(approvalRuleDTOList);                
            }
            else
            {
                approvalRuleDTOBindingList = new SortableBindingList<ApprovalRuleDTO>();
            }
            bindingSource.DataSource = approvalRuleDTOBindingList;
            dgvApprovalRule.DataSource = bindingSource;
            log.LogMethodExit();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int documentTypeId = -1;
            BindingSource approvalRuleListBS = (BindingSource)dgvApprovalRule.DataSource;
            var approvalRuleListOnDisplay = (SortableBindingList<ApprovalRuleDTO>)approvalRuleListBS.DataSource;
            if (approvalRuleListOnDisplay.Count > 0)
            {
                if (dgvDocumentType.SelectedRows.Count > 0)
                {
                    documentTypeId = Convert.ToInt32(dgvDocumentType.SelectedRows[0].Cells["documentTypeIdDataGridViewTextBoxColumn"].Value);
                }
                else
                {
                    MessageBox.Show("Please select document type");
                    log.LogMethodExit();
                    return;
                }
                foreach (ApprovalRuleDTO approvalRuleDTO in approvalRuleListOnDisplay)
                {
                    if (approvalRuleDTO.RoleId==-1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage("Please select user role"));
                        log.LogMethodExit();
                        return;
                    }
                    if(approvalRuleDTO.IsChanged)
                    {
                        if(approvalRuleDTO.DocumentTypeID==-1)
                        {
                            approvalRuleDTO.DocumentTypeID = documentTypeId;
                        }
                    }
                    ApprovalRule approvalRule = new ApprovalRule(machineUserContext,approvalRuleDTO);
                    approvalRule.Save();
                }
                if(dgvDocumentType.SelectedRows.Count>0)
                {                    
                    PopulateApprovalRuleGrid(Convert.ToInt32(dgvDocumentType.SelectedRows[0].Cells["documentTypeIdDataGridViewTextBoxColumn"].Value));
                    dgvDocumentType.CurrentCell = dgvDocumentType.SelectedRows[0].Cells["nameDataGridViewTextBoxColumn"];
                }
                else
                {
                    btnRefresh.PerformClick();
                }
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvDocumentType.Rows.Count>0)
            {
                PopulateApprovalRuleGrid(Convert.ToInt32(dgvDocumentType.Rows[0].Cells["documentTypeIdDataGridViewTextBoxColumn"].Value));
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void dgvDocumentType_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);         
            if(e.RowIndex>=0)
            {
                PopulateApprovalRuleGrid(Convert.ToInt32(dgvDocumentType.Rows[e.RowIndex].Cells["documentTypeIdDataGridViewTextBoxColumn"].Value));
            }
            log.LogMethodExit();
        }
    }
}
