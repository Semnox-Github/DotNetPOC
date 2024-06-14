/********************************************************************************************
 * Project Name - ChangeStaff UI
 * Description  - UI of ChangeStaff
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************
 *2.23.4      12-Sep-2018   Indhu                   Created 
 *2.70.2      18-Dec-2019   Jinto Thomas            Added parameter execution context for userbl & userrolebl declaration with userid 
 *2.130.8     05-Jun-2012   Deeksha Kulal           Modified to Add clocked in users display feature 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semnox.Parafait.User
{
    public partial class ChangeStaffUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public UsersDTO usersDTO;
        string LoginId = string.Empty;
        Utilities Utilities;
        UserContainerDTOCollection userContainerDTOCollection;
        List<UsersDTO> clockedInUsersDTO;

        public ChangeStaffUI(Utilities utilities, string LoginId)
        {
            InitializeComponent();
            ddlUserRoles.DisplayMember = "Role";
            ddlUserRoles.ValueMember = "RoleId";
            ddlUsers.DisplayMember = "LoginId";
            ddlUsers.ValueMember = "UserId";
            this.LoginId = LoginId;
            Utilities = utilities;
            if (!string.IsNullOrEmpty(LoginId))
                this.Text = "Change Staff From : " + LoginId;
            userContainerDTOCollection = UserContainerList.GetUserContainerDTOCollection(Utilities.ExecutionContext.GetSiteId());
            LoadUserRoles();
        }

        private void LoadUserRoles(List<int> roleIdList = null)
        {
            log.LogMethodEntry();
            //UserRolesList userRoleList = new UserRolesList();
            //List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> SearchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
            //SearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ISACTIVE, "1"));
            //SearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
            //List<UserRolesDTO> UserRoleDtoList = userRoleList.GetAllUserRoles(SearchParameters);
            List<UserRoleContainerDTO> userRoleDtoList = UserRoleContainerList.GetUserRoleContainerDTOList(Utilities.ExecutionContext.GetSiteId());
            if (roleIdList != null)
            {
                List<UserRoleContainerDTO> clockedInUserContainerDTO = new List<UserRoleContainerDTO>();
                foreach (int roleId in roleIdList)
                {
                    clockedInUserContainerDTO.AddRange(userRoleDtoList.Where(x => x.RoleId == roleId).ToList());
                }
                userRoleDtoList = clockedInUserContainerDTO;
            }
            if (userRoleDtoList == null)
            {
                userRoleDtoList = new List<UserRoleContainerDTO>();
            }
            ddlUserRoles.DataSource = userRoleDtoList;
            
            if (userRoleDtoList != null)
            {
                ddlUserRoles.SelectedValue = userRoleDtoList[0].RoleId;
                if(roleIdList != null)
                {
                    PopulateUsersForTheSelectedRole();
                }
            }
            log.LogMethodExit(null);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (usersDTO == null)
                MessageBox.Show(Utilities.MessageUtils.getMessage(1725), Utilities.MessageUtils.getMessage("Change Staff"));
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void DdlUserRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateUsersForTheSelectedRole();
            log.LogMethodExit();
        }

        private void PopulateUsersForTheSelectedRole()
        {
            log.LogMethodEntry();
            List<UserContainerDTO> usersDTOList = new List<UserContainerDTO>();
            UserRoleContainerDTO selectedUserRoleDTO = ddlUserRoles.SelectedItem as UserRoleContainerDTO;
            UserRoleContainerDTO userRoleContainerDTO = new UserRoleContainerDTO();
            if (selectedUserRoleDTO != null)
            {
                if (clockedInUsersDTO != null)
                {
                    List<int> clockedInUserIdList = clockedInUsersDTO.Select(x => x.UserId).Distinct().ToList();
                    foreach (int userId in clockedInUserIdList)
                    {
                        usersDTOList.AddRange(userContainerDTOCollection.UserContainerDTOList.FindAll(x => x.UserId == userId).Where(y=>y.RoleId == selectedUserRoleDTO.RoleId).ToList());
                    }
                }
                else
                {
                    userRoleContainerDTO = UserRoleContainerList.GetUserRoleContainerDTO(Utilities.ExecutionContext.GetSiteId(), selectedUserRoleDTO.RoleId);
                    usersDTOList = userContainerDTOCollection.UserContainerDTOList.FindAll(x => x.RoleId == userRoleContainerDTO.RoleId).ToList();
                }
            }
            else
            {
                LoadUserRoles();
            }
            usersDTOList.Insert(0, new UserContainerDTO());
            usersDTOList[0].UserId = -1;
            usersDTOList[0].LoginId = "<SELECT>";
            ddlUsers.DataSource = usersDTOList;
            
            log.LogMethodExit();
        }

        private void DdlUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                int userId = Convert.ToInt32(ddlUsers.SelectedValue);
                if (userId == -1)
                {
                    lblUserName.Text = string.Empty;
                    lblEmployeeNo.Text = string.Empty;
                    log.LogMethodExit(null, "userId == -1");
                    return;
                }
                Users users = new Users(Utilities.ExecutionContext, userId, false, false);
                usersDTO = users.UserDTO;
                lblUserName.Text = Utilities.MessageUtils.getMessage("UserName") + ": " + usersDTO.UserName;
                lblEmployeeNo.Text = Utilities.MessageUtils.getMessage("Employee No") + ": " + usersDTO.EmpNumber;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while assigning username and employee number", ex);
                lblUserName.Text = string.Empty;
                lblEmployeeNo.Text = string.Empty;
            }
            log.LogMethodEntry();
        }

        private void cbClockedInUsers_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            List<UserContainerDTO> usersDTOList = new List<UserContainerDTO>();
            usersDTOList.Insert(0, new UserContainerDTO());
            usersDTOList[0].UserId = -1;
            usersDTOList[0].LoginId = "<SELECT>";
            ddlUsers.DataSource = usersDTOList;

            ddlUserRoles.DataSource = new List<UserRoleContainerDTO>();
            usersDTO = null;
            if (cbClockedInUsers.Checked)
            {
                UsersList usersList = new UsersList(Utilities.ExecutionContext);
                clockedInUsersDTO = usersList.GetCurrentClockedInUsers();
                if (clockedInUsersDTO != null && clockedInUsersDTO.Count > 0)
                {
                    List<UserContainerDTO> clockedInUserContainerDTO = userContainerDTOCollection.UserContainerDTOList.Where(x => x.UserId.Equals(clockedInUsersDTO.Select(y => y.UserId))).ToList();
                    if (clockedInUsersDTO != null && clockedInUsersDTO.Count > 0)
                    {
                        LoadUserRoles(clockedInUsersDTO.Select(x => x.RoleId).Distinct().ToList());
                    }
                }
            }
            else
            {
                clockedInUsersDTO = null;
                LoadUserRoles();
            }
            log.LogMethodExit();
        }
    }
}
