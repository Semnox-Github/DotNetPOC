/********************************************************************************************
 * Project Name - AddAgentsUI
 * Description  - Add Agents UI 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        05-Aug-2019   Girish Kundar   Added LogMethodEntry() and LogMethodExit() methods. 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public partial class AddAgentsUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities _Utilities;
        private int agentid;
        private int partnerid = -1;
        private ExecutionContext machineUserContext;

        public AddAgentsUI(Utilities _inUtilities, int agentId, int partnerId)
        {
            log.LogMethodEntry(_inUtilities , agentId , partnerId);
            InitializeComponent();
            machineUserContext = ExecutionContext.GetExecutionContext();

            try
            {
                this.partnerid = partnerId;
                this._Utilities = _inUtilities;

                Bindpartners();
                BindUsers();

                if (partnerId > 0)
                {
                    drpPartnerSelect.SelectedValue = partnerId;
                }

                chkActive.Checked = true;

                if (agentId > 0) //EDIT OR NEW Form
                {
                    AgentsDTO agentDTO = new Agents(machineUserContext, agentId).GetAgentsDTO;

                    drpUserSelect.SelectedValue = agentDTO.User_Id;
                    txtMobileNo.Text = agentDTO.MobileNo;
                    txtCommission.Text = agentDTO.Commission.ToString();
                    chkActive.Checked = agentDTO.Active;
                }

                this.agentid = agentId;
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }

        //Binding partners Combo list  
        public void Bindpartners()
        {
            try
            {
                log.LogMethodEntry();
                PartnersList partnersList = new PartnersList(machineUserContext);
                List<KeyValuePair<PartnersDTO.SearchByParameters, string>> partnerParams = new List<KeyValuePair<PartnersDTO.SearchByParameters, string>>();
                List<PartnersDTO> partnersDTOList = partnersList.GetAllPartnersList(partnerParams);

                drpPartnerSelect.DataSource = partnersDTOList;
                drpPartnerSelect.DisplayMember = "partnerName";
                drpPartnerSelect.ValueMember = "PartnerId";
                drpPartnerSelect.SelectedIndex = -1;
                drpPartnerSelect.Text = "";
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }

        //Bind Users Combo list  
        public void BindUsers()
        {
            log.LogMethodEntry();
            try
            {
                UsersList users = new UsersList(machineUserContext);
                List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                List<UsersDTO> UsersDTOList = users.GetAllUsers(searchParameters);

                //Bind Users Combo list
                drpUserSelect.DataSource = UsersDTOList;
                drpUserSelect.DisplayMember = "loginid";
                drpUserSelect.ValueMember = "userid";
                drpUserSelect.SelectedIndex = -1;
                drpUserSelect.Text = "";
                log.LogMethodExit();
            }
            catch (Exception expn)
            {
                log.LogMethodExit(expn);
                MessageBox.Show(expn.Message.ToString());
            }
        }
        private void AllowNumbersKeyPressed(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (Char.IsDigit(e.KeyChar)) return;
            if (Char.IsControl(e.KeyChar)) return;
            e.Handled = true;
            log.LogMethodExit();
        }

        private void CommissionKeyPressed(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (Char.IsDigit(e.KeyChar)) return;
            if (Char.IsControl(e.KeyChar) || e.KeyChar == '.') return;
            e.Handled = true;
            log.LogMethodExit();
        }

        private void btnAgentUserSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender ,e );
            txtCommission.Text = "0";
            
            bool flag = false;
            double doubleOutput;
            try
            {
                ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
                if (drpPartnerSelect.Text.ToString() == "")
                {
                    MessageBox.Show("Please select the Partner.");
                }
                else if (drpUserSelect.Text.ToString() == "")
                {
                    MessageBox.Show("Please select the Login Id .");
                }

                else if (txtMobileNo.Text == "")
                {
                    MessageBox.Show("Please Enter the mobile no.");
                }
                else if (!(txtMobileNo.Text == ""))
                {

                    if (!double.TryParse(txtMobileNo.Text, out doubleOutput))
                    {
                        MessageBox.Show("Please Enter the Numeric mobile no.");
                    }
                    else if (txtMobileNo.Text.Length < 10 || txtMobileNo.Text.Length > 12)
                    {
                        MessageBox.Show("Please Enter 10 digit mobile No");
                    }
                    else if (txtCommission.Text == "")
                    {
                        MessageBox.Show("Please Enter the commisssion.");
                    }
                    else if (!(txtCommission.Text == ""))
                    {
                        if (!double.TryParse(txtCommission.Text, out doubleOutput))
                        {
                            MessageBox.Show("Please Enter valid numeric commisssion.");
                        }
                        else if (double.TryParse(txtCommission.Text, out doubleOutput))
                        {
                            if (doubleOutput > 999.999)
                            {
                                MessageBox.Show("Commission Should be less than 999.999 ");
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }
                }

                AgentsDTO agentsDTO;
                Agents agents;
                if (flag)
                {
                    if (agentid < 0)   //Insert New 
                    {
                        agentsDTO = new AgentsDTO();
                        agentsDTO.User_Id = Convert.ToInt32(drpUserSelect.SelectedValue);

                        if (UserExist(agentsDTO.User_Id))
                        {
                            MessageBox.Show("User already assigned.");
                            return;
                        }

                    }
                    else  /// Update Existing 
                    {
                        agents = new Agents(machineUserContext, agentid);
                        agentsDTO = agents.GetAgentsDTO;

                        if (agents.UserTransactionExist(agentsDTO.User_Id) > 0)
                        {
                            if (drpUserSelect.SelectedValue.ToString() != agentsDTO.User_Id.ToString())
                            {
                                MessageBox.Show("User has active Transcations! Cannot Assign user.");
                                return;
                            }
                        }
                    }


                    agents = new Agents(machineUserContext, agentsDTO);
                    agentsDTO.PartnerId = Convert.ToInt32(drpPartnerSelect.SelectedValue);
                    agentsDTO.User_Id = Convert.ToInt32(drpUserSelect.SelectedValue);
                    agentsDTO.MobileNo = txtMobileNo.Text;

                    agentsDTO.Commission = Convert.ToDouble(txtCommission.Text);
                    agentsDTO.Active = chkActive.Checked;

                    agents.Save();
                    this.Close();

                    var agentForm = Application.OpenForms.OfType<AgentsUI>().FirstOrDefault();
                    if (agentForm != null)
                    {
                        agentForm.Activate();
                        agentForm.PopulateAgentsUserGridRefresh();
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

        public bool UserExist(int user_id)
        {
            log.LogMethodEntry(user_id);
            AgentsList agentsList = new AgentsList(machineUserContext);

            List<KeyValuePair<AgentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<AgentsDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<AgentsDTO.SearchByParameters, string>(AgentsDTO.SearchByParameters.USER_ID, user_id.ToString()));
            List<AgentsDTO> agents = agentsList.GetAllAgentsList(searchParameters);
            if (agents.Count > 0)
            {
                log.LogMethodExit(true);
                return true;
            }
            log.LogMethodExit(false);
            return false;
        }

    }
}
