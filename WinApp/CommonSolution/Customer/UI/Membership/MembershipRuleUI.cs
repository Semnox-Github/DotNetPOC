/********************************************************************************************
 * Project Name - Customer.Membership
 * Description  - MembershipRuleUI
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019      Girish kundar      Modified :Removed Unused name space's.
 *2.80.0        17-Feb-2019      Deeksha            Modified to Make Cards module as
 *                                                  read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Customer.Membership
{
    public partial class MembershipRuleUI : Form
    {
        Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<KeyValuePair<string, string>> untiWindowValues = new List<KeyValuePair<string, string>>();
        private ManagementStudioSwitch managementStudioSwitch;

        public MembershipRuleUI(Utilities _utilities)
        {
            log.LogMethodEntry(_utilities);
            InitializeComponent();
            utilities = _utilities;
            utilities.setupDataGridProperties(ref dgvMembershipRule);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }    
            machineUserContext.SetIsCorporate(utilities.ExecutionContext.GetIsCorporate());			
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this); 
            untiWindowValues.Add(new KeyValuePair<string, string>("D", "Days"));
            untiWindowValues.Add(new KeyValuePair<string, string>("M", "Months"));
            untiWindowValues.Add(new KeyValuePair<string, string>("Y", "Years"));
            LoadUnitWindow();
            PopulateMembershipRuleGrid();
            managementStudioSwitch = new ManagementStudioSwitch(machineUserContext);
            UpdateUIElements();
            log.LogMethodExit();
        }
        private void LoadUnitWindow()
        {
            log.LogMethodEntry();
            unitOfQualificationWindowDataGridViewTextBoxColumn.DataSource= new BindingSource(untiWindowValues,null);
            unitOfQualificationWindowDataGridViewTextBoxColumn.ValueMember = "Key";
            unitOfQualificationWindowDataGridViewTextBoxColumn.DisplayMember = "Value";
            unitOfRetentionWindowDataGridViewTextBoxColumn.DataSource = new BindingSource(untiWindowValues, null);
            unitOfRetentionWindowDataGridViewTextBoxColumn.ValueMember = "Key";
            unitOfRetentionWindowDataGridViewTextBoxColumn.DisplayMember = "Value";
            log.LogMethodExit();
        }
        private void PopulateMembershipRuleGrid()
        {
            log.LogMethodEntry();
            MembershipRulesList membershipRuleList = new MembershipRulesList(utilities.ExecutionContext);
            List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>> membershipRuleSearchParams = new List<KeyValuePair<MembershipRuleDTO.SearchByParameters, string>>();
            //membershipRuleSearchParams.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.ACTIVE_FLAG, (chbShowActiveEntries.Checked) ? "Y" : ""));
            membershipRuleSearchParams.Add(new KeyValuePair<MembershipRuleDTO.SearchByParameters, string>(MembershipRuleDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));

            List<MembershipRuleDTO> membershipRuleListOnDisplay = membershipRuleList.GetAllMembershipRule(membershipRuleSearchParams);
            BindingSource membershipRuleListBS = new BindingSource();
            if (membershipRuleListOnDisplay != null)
            {
                SortableBindingList<MembershipRuleDTO> membershipRuleDTOSortList = new SortableBindingList<MembershipRuleDTO>(membershipRuleListOnDisplay);
                membershipRuleListBS.DataSource = membershipRuleDTOSortList;
            }
            else
                membershipRuleListBS.DataSource = new SortableBindingList<MembershipRuleDTO>();
            dgvMembershipRule.DataSource = membershipRuleListBS;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender ,e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            BindingSource membershipRuleListBS = (BindingSource)dgvMembershipRule.DataSource;
            var membershipRuleListOnDisplay = (SortableBindingList<MembershipRuleDTO>)membershipRuleListBS.DataSource;
            if (membershipRuleListOnDisplay != null && membershipRuleListOnDisplay.Count > 0)
            {
                foreach (MembershipRuleDTO membershipRuleDTO in membershipRuleListOnDisplay)
                {
                    if (string.IsNullOrEmpty(membershipRuleDTO.RuleName))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1480));
                        this.BringToFront();
                        this.Focus();
                        return;
                    }
                    if (membershipRuleDTO.QualificationWindow > 0 && string.IsNullOrEmpty(membershipRuleDTO.UnitOfQualificationWindow))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1481));
                        this.BringToFront();
                        this.Focus();
                        return;
                    }
                    if (membershipRuleDTO.QualificationWindow == 0 && string.IsNullOrEmpty(membershipRuleDTO.UnitOfQualificationWindow))
                    {
                        membershipRuleDTO.UnitOfQualificationWindow = "D";
                    }
                    if (membershipRuleDTO.RetentionWindow == 0 )
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1482));
                        this.BringToFront();
                        this.Focus();
                        return;
                    }
                    if (string.IsNullOrEmpty(membershipRuleDTO.UnitOfRetentionWindow))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1483));
                        this.BringToFront();
                        this.Focus();
                        return;
                    }
                    //membershipRuleDTO.UnitOfQualificationWindow = membershipRuleDTO.UnitOfQualificationWindow.Substring(0, 1);
                    //membershipRuleDTO.UnitOfRetentionWindow = membershipRuleDTO.UnitOfRetentionWindow.Substring(0, 1);
                    MembershipRuleBL membershipRule = new MembershipRuleBL(machineUserContext, membershipRuleDTO);
                    membershipRule.Save();
                }
                PopulateMembershipRuleGrid();
                this.BringToFront();
                this.Focus();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateMembershipRuleGrid();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (this.dgvMembershipRule.SelectedRows.Count <= 0 && this.dgvMembershipRule.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.LogMethodExit(null, "\"No rows selected. Please select the rows you want to delete and press delete..\" message .");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            if (this.dgvMembershipRule.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in this.dgvMembershipRule.SelectedCells)
                {
                    dgvMembershipRule.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach (DataGridViewRow membershipRuleRow in dgvMembershipRule.SelectedRows)
            {
                if (membershipRuleRow.Cells[0].Value == null)
                {
                    return;
                }
                if (Convert.ToInt32(membershipRuleRow.Cells[0].Value.ToString()) <= 0)
                {
                    dgvMembershipRule.Rows.RemoveAt(membershipRuleRow.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        BindingSource membershipRuleDTOListDTOBS = (BindingSource)dgvMembershipRule.DataSource;
                        var membershipRuleDTOList = (SortableBindingList<MembershipRuleDTO>)membershipRuleDTOListDTOBS.DataSource;
                        MembershipRuleDTO membershipRuleDTO = membershipRuleDTOList[membershipRuleRow.Index];
                        membershipRuleDTO.IsActive = false;
                        MembershipRuleBL membershipRule = new MembershipRuleBL(machineUserContext, membershipRuleDTO);
                        membershipRule.Save();
                    }
                }
            }
            if (rowsDeleted == true)
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            log.LogMethodExit();
        }

        private void dgvMembershipRule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(1512));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnableCardsModule)
            {
                dgvMembershipRule.AllowUserToAddRows = true;
                dgvMembershipRule.ReadOnly = false;;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvMembershipRule.AllowUserToAddRows = false;
                dgvMembershipRule.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
