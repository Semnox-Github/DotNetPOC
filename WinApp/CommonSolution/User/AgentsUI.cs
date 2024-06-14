/********************************************************************************************
 * Project Name - Agents UI
 * Description  - UI of the Agents 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        15-Jul-2019   Girish Kundar      Modified :Added LogmethodEntry() and LogMethodExit() 
 *2.90.0        15-Jul-2020   Girish Kundar      Modified : Phase -2 changes for REST API 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.User
{
    public partial class AgentsUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Utilities utilities;
        private ExecutionContext machineUserContext;
        private int partnerid = -1;
        private bool initailLoadSetFlag = true;

        public AgentsUI(Utilities _Utilities, int partnerId)
        {
            log.LogMethodEntry(_Utilities , partnerId);
            InitializeComponent();
            utilities = _Utilities;
            this.partnerid = partnerId;
            utilities.setupDataGridProperties(ref dgAgentUser);
            utilities.setupDataGridProperties(ref dgAgentGroup);

            machineUserContext = ExecutionContext.GetExecutionContext();
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

            Bindpartners();
            cmbSelectPartner.SelectedValue = partnerId;
            cmbSelectPartner.SelectedIndex = -1;
            cmbSelectPartner.Text = "";
            PopulateAgentsUserGrid(partnerId);

            BindpartnersAgentGroup();
            PopulateAgentGroupsGrid();
            log.LogMethodExit();
        }

        //Bind partners Combo list  
        public void Bindpartners()
        {
            log.LogMethodEntry();
            try
            {
                PartnersList partnersList = new PartnersList(machineUserContext);
                List<KeyValuePair<PartnersDTO.SearchByParameters, string>> partnerParams = new List<KeyValuePair<PartnersDTO.SearchByParameters, string>>();
                List<PartnersDTO> partnersDTOList = partnersList.GetAllPartnersList(partnerParams);
                if (partnersDTOList != null && partnersDTOList.Any())
                {
                    cmbSelectPartner.DataSource = partnersDTOList;
                    cmbSelectPartner.DisplayMember = "partnerName";
                    cmbSelectPartner.ValueMember = "PartnerId";
                    cmbSelectPartner.SelectedIndex = -1;
                    log.LogMethodExit();
                }
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }

        }

        public int GetpartnerId()
        {
            log.LogMethodEntry();
            try
            {
                if (cmbSelectPartner.Text.ToString() != "")
                {
                    int.TryParse(cmbSelectPartner.SelectedValue.ToString(), out partnerid);
                }
                else
                {
                    partnerid = -1;
                }
                log.LogMethodExit();

            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
            log.LogMethodExit(partnerid);
            return partnerid;
        }

        public void BindpartnersAgentGroup()
        {
            log.LogMethodEntry();
            try
            {
                PartnersList partnersList = new PartnersList(machineUserContext);
                List<KeyValuePair<PartnersDTO.SearchByParameters, string>> partnerParams = new List<KeyValuePair<PartnersDTO.SearchByParameters, string>>();
                List<PartnersDTO> partnersDTOList = partnersList.GetAllPartnersList(partnerParams);
                if (partnersDTOList != null && partnersDTOList.Any())
                {
                    cmbPartnerAgentGroup.DataSource = partnersDTOList;
                    cmbPartnerAgentGroup.DisplayMember = "partnerName";
                    cmbPartnerAgentGroup.ValueMember = "PartnerId";
                    cmbPartnerAgentGroup.SelectedIndex = -1;
                    log.LogMethodExit();
                }
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }

        public void PopulateAgentsUserGridRefresh()
        {
            log.LogMethodEntry();
            try
            {
                int pid = GetpartnerId();
                PopulateAgentsUserGrid(pid);
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }

        public void PopulateAgentGroupsGrid()
        {
            log.LogMethodEntry();
            List<AgentGroupsDTO> populateAgentGroupslist = new List<AgentGroupsDTO>();
            AgentGroupsList agentGroupsList = new AgentGroupsList(machineUserContext);
            List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> searchAgentGroupParameter = new List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>();
            populateAgentGroupslist = agentGroupsList.GetAllAgentGroupsList(searchAgentGroupParameter);
            SortableBindingList<AgentGroupsDTO> agentGroupsSortList = new SortableBindingList<AgentGroupsDTO>(populateAgentGroupslist);
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = agentGroupsSortList;
            dgAgentGroup.DataSource = bindingSource;
            log.LogMethodExit();
        }

        public void BindAgentGroup(int partnerId, int agentGroupId = -1)
        {
            log.LogMethodEntry(partnerid, agentGroupId);
            lstAvailableAgents.Items.Clear();
            lstAssignedAgents.Items.Clear();
            // bool flag = true ;
            //   string loginid = "";
            if (partnerId != -1)
            {
                AgentsList agentsList = new AgentsList(machineUserContext);
                List<KeyValuePair<AgentUserDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<AgentUserDTO.SearchByParameters, string>>();
                searchParameter.Add(new KeyValuePair<AgentUserDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.PARTNER_ID, partnerId.ToString()));
                List<AgentUserDTO> agentUserDTOList = agentsList.GetAllAgentUserList(searchParameter);

                AgentGroupAgentsList agentGroupAgentsList = new AgentGroupAgentsList(machineUserContext);
                List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>> AGAsearchParameter = new List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>>();
                List<AgentGroupAgentsDTO> agentGroupAgentsDTOList = agentGroupAgentsList.GetAllAgentGroupsAgentsList(AGAsearchParameter);

                foreach (AgentUserDTO agentUserDTO in agentUserDTOList)
                {
                    lstAvailableAgents.Items.Add(agentUserDTO);
                }

                if (agentGroupAgentsDTOList.Count > 0)
                {
                    foreach (AgentUserDTO agentUserDTO in agentUserDTOList)
                    {
                        foreach (AgentGroupAgentsDTO agentGroupAgentsDTO in agentGroupAgentsDTOList)
                        {
                            if (agentUserDTO.AgentId == agentGroupAgentsDTO.AgentId)
                            {
                                lstAvailableAgents.Items.Remove(agentUserDTO);
                            }
                        }
                    }
                }
            }

            lstAvailableAgents.ValueMember = "AgentId";
            lstAvailableAgents.DisplayMember = "UserName";

            lstAssignedAgents.ValueMember = "AgentId";
            lstAssignedAgents.DisplayMember = "UserName";
            log.LogMethodExit();
        }

        public void PopulateAgentsUserGrid(int partnerId)
        {
            log.LogMethodEntry(partnerid);
            try
            {
                List<AgentUserDTO> PopulateAgentsUserOnDisplay;
                AgentsList agentsList = new AgentsList(machineUserContext);

                List<KeyValuePair<AgentUserDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<AgentUserDTO.SearchByParameters, string>>();

                if (partnerId > 0)
                {
                    Partners partners = new Partners(machineUserContext ,partnerId);
                    PartnersDTO partnersDTO = partners.GetPartnersDTO;
                    cmbSelectPartner.Text = partnersDTO.PartnerName;

                    searchParameter.Add(new KeyValuePair<AgentUserDTO.SearchByParameters,string>(AgentUserDTO.SearchByParameters.PARTNER_ID,partnerId.ToString()));
                    PopulateAgentsUserOnDisplay = agentsList.GetAllAgentUserList(searchParameter);

                    BindingSource bsAgents = new BindingSource();
                    if (PopulateAgentsUserOnDisplay != null)
                    {
                        SortableBindingList<AgentUserDTO> agentsDTOSortList = new SortableBindingList<AgentUserDTO>(PopulateAgentsUserOnDisplay);
                        dgAgentUser.DataSource = agentsDTOSortList;
                    }
                }
                else
                {
                    PopulateAgentsUserOnDisplay = agentsList.GetAllAgentUserList(searchParameter);
                    SortableBindingList<AgentUserDTO> agentsDTOSortList = new SortableBindingList<AgentUserDTO>(PopulateAgentsUserOnDisplay);
                    dgAgentUser.DataSource = agentsDTOSortList;


                }
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }

        private void btnAgentUserRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                initailLoadSetFlag = true;
                // partnerid = -1;
                //Bindpartners();
                PopulateAgentsUserGrid(partnerid);
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }

        }

        private void btnAgentUserDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                if (dgAgentUser.CurrentRow == null || dgAgentUser.CurrentRow.IsNewRow || dgAgentUser.CurrentRow.Cells[0].Value == DBNull.Value)
                    return;
                else
                {
                    DialogResult result1 = MessageBox.Show("Do You Want Delete ?", "Delete Agent", MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        int agentId = Convert.ToInt32(dgAgentUser.CurrentRow.Cells[0].Value);

                        Agents agents = new Agents(machineUserContext, agentId);
                        int deleteStatus = agents.Delete();
                        if (deleteStatus > 0)
                        {
                            MessageBox.Show("Agent Deleted .");
                        }
                        else
                        {
                            MessageBox.Show("Error Occurred ! Please Retry again.");
                        }
                        int partnerId = GetpartnerId();
                        PopulateAgentsUserGrid(partnerId);
                    }
                }

                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }

        private void btnAgentEdit_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                if (dgAgentUser.CurrentRow == null || dgAgentUser.CurrentRow.IsNewRow || dgAgentUser.CurrentRow.Cells[0].Value == DBNull.Value)
                    return;
                else
                {
                    int agentId;
                    if (int.TryParse(dgAgentUser.CurrentRow.Cells[0].Value.ToString(), out agentId))
                    {
                        int pid = -1;
                        int.TryParse(dgAgentUser.CurrentRow.Cells[4].Value.ToString(), out pid);

                        Agents agents = new Agents(machineUserContext,agentId);
                        AgentsDTO agentsDTO = agents.GetAgentsDTO;
                        AddAgentsUI addAgentsUI = new AddAgentsUI(utilities, agentId, pid);
                        var AddAgentForm = Application.OpenForms.OfType<AddAgentsUI>().FirstOrDefault();
                        if (AddAgentForm != null)
                        {
                            AddAgentForm.Activate();
                        }
                        else
                        {
                            addAgentsUI.ShowDialog();
                        }
                        log.LogMethodExit();
                    }
                }
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }

        }

        private void cmbSelectPartner_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                if (initailLoadSetFlag)
                {
                    initailLoadSetFlag = false;
                }
                else if (cmbSelectPartner.SelectedIndex >= 0)
                {
                    int pId = GetpartnerId();
                    PopulateAgentsUserGrid(pId);
                }
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }

        private void btnAgentNew_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            try
            {
                AddAgentsUI addAgentsUI = new AddAgentsUI(utilities, -1, partnerid);

                var AddAgentForm = Application.OpenForms.OfType<AddAgentsUI>().FirstOrDefault();
                if (AddAgentForm != null)
                {
                    AddAgentForm.Activate();
                }
                else
                {
                    addAgentsUI.ShowDialog();
                }
                PopulateAgentsUserGrid(partnerid);
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }

        private void btnAgentGroupSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int pid = -1;
            BindingSource agentGroupListBS = (BindingSource)dgAgentGroup.DataSource;//new BindingSource();
            var agentGroupListOnDisplay = (SortableBindingList<AgentGroupsDTO>)agentGroupListBS.DataSource;

            if (agentGroupListOnDisplay.Count > 0)
            {

                foreach (AgentGroupsDTO agentGroupsDTO in agentGroupListOnDisplay)
                {
                    {
                        if (string.IsNullOrEmpty(agentGroupsDTO.GroupName))
                        {
                            MessageBox.Show("Please Enter the GroupName");
                            return;
                        }
                        if (cmbPartnerAgentGroup.Text != "")
                        {
                            int.TryParse(cmbPartnerAgentGroup.SelectedValue.ToString(), out pid);
                            if (dgAgentGroup.CurrentRow.Cells[0].Value.ToString() == agentGroupsDTO.AgentGroupId.ToString())
                            {
                                agentGroupsDTO.PartnerId = pid;  // Convert.ToInt32(dgAgentGroup.CurrentRow.Cells[0].Value);
                            }
                        }
                        if (pid == -1)
                        {
                            MessageBox.Show("Please Select Partner Name.");
                            return;
                        }
                        if (dgAgentGroup.CurrentRow.Cells[1].Value.ToString() == "")
                        {
                            MessageBox.Show("Please add group name.");
                            return;
                        }
                        try
                        {
                            AgentGroupsList agentGroupsList = new AgentGroupsList(machineUserContext);
                            List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> agentGrpSearchParam = new List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>();
                            agentGrpSearchParam.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.GROUP_NAME, agentGroupsDTO.GroupName));
                            agentGrpSearchParam.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.AGENT_GROUP_ID, agentGroupsDTO.AgentGroupId.ToString()));
                            List<AgentGroupsDTO> agentGroupList = agentGroupsList.GetAllAgentGroupsList(agentGrpSearchParam);
                            if (agentGroupList.Count == 1)
                            {
                                AgentGroups agentGroups = new AgentGroups(machineUserContext, agentGroupsDTO);
                                agentGroups.Save();
                            }
                            else
                            {
                                List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>> agentGrpNameSearchParam = new List<KeyValuePair<AgentGroupsDTO.SearchByParameters, string>>();
                                agentGrpNameSearchParam.Add(new KeyValuePair<AgentGroupsDTO.SearchByParameters, string>(AgentGroupsDTO.SearchByParameters.GROUP_NAME, agentGroupsDTO.GroupName));
                                List<AgentGroupsDTO> agentGroupList1 = agentGroupsList.GetAllAgentGroupsList(agentGrpNameSearchParam);
                                if (agentGroupList1.Count == 0)
                                {
                                    AgentGroups agentGroups = new AgentGroups(machineUserContext ,agentGroupsDTO);
                                    agentGroups.Save();
                                }
                                else
                                {
                                    MessageBox.Show("Please add unique group name.");
                                    return;
                                }
                            }
                        }
                        catch (Exception expn)
                        {
                            log.LogMethodExit(expn);
                            MessageBox.Show(expn.Message.ToString());
                        }
                    }
                }
                SaveAgentGroups();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            PopulateAgentGroupsGrid();
            log.LogMethodExit();
        }

        private void btnInclude_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            object[] items = new object[2000];
            int selectedI = 0;
            foreach (object machine in lstAvailableAgents.SelectedItems)
            {
                items[selectedI++] = machine;
                lstAssignedAgents.Items.Add(machine);
            }
            for (int i = 0; i < selectedI; i++)
            {
                if (items[i] == null)
                    break;
                lstAvailableAgents.Items.Remove(items[i]);
            }
            if (lstAvailableAgents.Items.Count > 0)
                lstAvailableAgents.SelectedIndex = 0;
            log.LogMethodExit();
        }

        private void btnExclude_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            object[] items = new object[1000];
            int selectedI = 0;
            foreach (object machine in lstAssignedAgents.SelectedItems)
            {
                items[selectedI++] = machine;
                lstAvailableAgents.Items.Add(machine);
            }
            for (int i = 0; i < selectedI; i++)
            {
                if (items[i] == null)
                    break;
                lstAssignedAgents.Items.Remove(items[i]);
            }
            if (lstAssignedAgents.Items.Count > 0)
                lstAssignedAgents.SelectedIndex = 0;

            log.LogMethodExit();
        }
        private void cmbPartnerAgentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            int pid = -1;
            if (cmbPartnerAgentGroup.SelectedIndex >= 0)
            {
                int.TryParse(cmbPartnerAgentGroup.SelectedValue.ToString(), out pid);
            }
            BindAgentGroup(pid);
        }

        private void btnAgentUserClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            this.Close();
            log.LogMethodExit();
        }

        public void SaveAgentGroups()
        {
            log.LogMethodEntry();
            int agentGroupId = -1;
            AgentGroupAgents agentGroupAgents;
            if (int.TryParse(dgAgentGroup.CurrentRow.Cells[0].Value.ToString(), out agentGroupId))
            {

                AgentGroupAgents deleteAgentGroupAgents = new AgentGroupAgents(machineUserContext, agentGroupId);
                int deleteStatus = deleteAgentGroupAgents.DeleteByGroupId(agentGroupId);

                for (int i = 0; i < lstAssignedAgents.Items.Count; i++)
                {
                    AgentGroupAgentsDTO agentGroupAgentsDTO = new AgentGroupAgentsDTO();
                    agentGroupAgentsDTO.AgentGroupId = agentGroupId;
                    object includeObjects = lstAssignedAgents.Items[i];
                    agentGroupAgentsDTO.AgentGroupId = agentGroupId;
                    agentGroupAgentsDTO.AgentId = ((AgentUserDTO)(includeObjects)).AgentId;

                    agentGroupAgents = new AgentGroupAgents(machineUserContext,agentGroupAgentsDTO);
                    agentGroupAgents.Save();
                }

            }
            else
            {
                MessageBox.Show("please select the Agent Group");
            }
            log.LogMethodExit();
        }

        private void dgAgentGroup_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            lstAssignedAgents.Items.Clear();
            int agentGroupId = -1;
            int partnerid = -1;
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (dgAgentGroup["agentGroupIdDataGridViewTextBoxColumn", e.RowIndex].Value != null)
            {
                int.TryParse(dgAgentGroup["agentGroupIdDataGridViewTextBoxColumn", e.RowIndex].Value.ToString(), out agentGroupId);
                int.TryParse(dgAgentGroup["partnerIdDataGridViewTextBoxColumn1", e.RowIndex].Value.ToString(), out partnerid);

                int pid = -1;
                if (cmbPartnerAgentGroup.SelectedIndex >= 0)
                {
                    int.TryParse(cmbPartnerAgentGroup.SelectedValue.ToString(), out pid);
                }
                else
                {
                    pid = partnerid;
                }

                AgentsList agentsList = new AgentsList(machineUserContext);
                List<KeyValuePair<AgentUserDTO.SearchByParameters, string>> searchParameter = new List<KeyValuePair<AgentUserDTO.SearchByParameters, string>>();
                searchParameter.Add(new KeyValuePair<AgentUserDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.PARTNER_ID, partnerid.ToString()));
                List<AgentUserDTO> agentUserDTOList = agentsList.GetAllAgentUserList(searchParameter);

                AgentGroupAgentsList agentGroupAgentsList = new AgentGroupAgentsList(machineUserContext);
                List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>> AGAsearchParameter = new List<KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>>();
                AGAsearchParameter.Add(new KeyValuePair<AgentGroupAgentsDTO.SearchByParameters, string>(AgentGroupAgentsDTO.SearchByParameters.AGENT_GROUP_ID, agentGroupId.ToString()));
                List<AgentGroupAgentsDTO> agentGroupAgentsDTOList = agentGroupAgentsList.GetAllAgentGroupsAgentsList(AGAsearchParameter);


                if (agentGroupAgentsDTOList.Count > 0)
                {
                    foreach (AgentUserDTO agentUserDTO in agentUserDTOList)
                    {
                        foreach (AgentGroupAgentsDTO agentGroupAgentsDTO in agentGroupAgentsDTOList)
                        {
                            if (agentUserDTO.AgentId == agentGroupAgentsDTO.AgentId && partnerid == pid)
                            {
                                lstAssignedAgents.Items.Add(agentUserDTO);

                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnAgentGroupClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnAgentGroupRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            BindpartnersAgentGroup();
            PopulateAgentGroupsGrid();
            log.LogMethodExit();
        }

        private void btnAgentgroupDelete_Click_1(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            if (dgAgentGroup.CurrentRow == null)
            {
                return;
            }
            else
            {
                int agentGroupId = -1;
                int.TryParse(dgAgentGroup.CurrentRow.Cells[0].Value.ToString(), out agentGroupId);
                AgentGroups agentGroups = new AgentGroups(machineUserContext, agentGroupId);
                try
                {
                    DialogResult result1 = MessageBox.Show("Do You Want Delete ?", "Delete Agent", MessageBoxButtons.YesNo);
                    if (result1 == DialogResult.Yes)
                    {
                        int deleteStatus = agentGroups.Delete();
                        if (deleteStatus > 0)
                        {
                            MessageBox.Show("Agent Group Deleted .");
                        }
                        else
                        {
                            MessageBox.Show("Error Occurred ! Please Retry again.");
                        }
                        PopulateAgentGroupsGrid();
                    }
                }
                catch (Exception expn)
                {
                    string message = MessageContainerList.GetMessage(machineUserContext, 546);
                    log.LogMethodExit(null, "Throwing Exception - " + expn.Message);
                    MessageBox.Show(message);
                   
                }
                log.LogMethodExit();
            }
        }

        private void dgAgentGroup_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MessageBox.Show(e.RowIndex + 1 + dgAgentGroup.Columns[e.ColumnIndex].DataPropertyName + ": " + e.Exception.Message, "Data Error");
            }
            catch
            {
            }
            e.Cancel = true;
            log.LogMethodExit();
        }


    }

}
