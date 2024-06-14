using Semnox.Core.Utilities;
using Semnox.Parafait.logging;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
//using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parafait_POS
{
    public partial class frmFindStaff : Form
    {
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        Utilities Utilities;
        public UsersDTO UserObj { get; set; }
        public int siteid { get; set; }
        /// <summary>
        /// Construction to define FindStaff()
        /// </summary>
        public frmFindStaff(Utilities _utilities)
        {
            log.Debug("Starts-frmFindStaff()");
            InitializeComponent();
            Utilities = _utilities;
            Utilities.setLanguage(this);

            if (Utilities.ParafaitEnv.IsCorporate)//Starts:Modification on 02-jan-2017 fo customer feedback
            {
                siteid = Utilities.ParafaitEnv.SiteId;
            }
            else
            {
                siteid = -1;
            }
            LoadUserRolesDTO();
            //LoadUsers("", "");//gets fired during default user roles load
            log.Debug("Ends-frmFindStaff()");
        }

        /// <summary>
        /// Get all user roles
        /// </summary>
        void LoadUserRolesDTO()
        {
            log.Debug("Starts-LoadUserRolesDTO()");
            try
            {
                UserRolesList userRoleList = new UserRolesList();
                List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> SearchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
                SearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ISACTIVE, "1"));
                SearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ROLE_NAME_EXCLUSION_LIST, "Semnox Admin"));
                List<UserRolesDTO> UserRoleDtoList = userRoleList.GetAllUserRoles(SearchParameters);

                if (UserRoleDtoList == null)
                {
                    UserRoleDtoList = new List<UserRolesDTO>();
                }

                UserRoleDtoList.Insert(0, new UserRolesDTO());
                UserRoleDtoList[0].Role = "- Select -";
                UserRoleDtoList[0].RoleId = -1;

                cmbStaffRoles.DataSource = UserRoleDtoList;
                cmbStaffRoles.DisplayMember = "Role";
                cmbStaffRoles.ValueMember = "RoleId";
            }
            catch (Exception ex)
            {
                log.Error("Ends with error -LoadUserRolesDTO() :" + ex.Message);
            }
            log.Debug("Ends-LoadUserRolesDTO()");
        }

        /// <summary>
        /// To get users list matching with search criteria
        /// </summary>
        /// <param name="empNumber">get user matching with empNumber</param>
        /// <param name="roleId">get list of user matching with roleId</param>
        void LoadUsers(string empNumber, string roleId)
        {
            log.Debug("Starts-LoadUsers(" + empNumber + "," + roleId + ") method.");
            List<UsersDTO> usersDTOList = new List<UsersDTO>();
            UsersList usersList = new UsersList(Utilities.ExecutionContext);
            List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> usersDTOSearchParams = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();

            try
            {
                if (!string.IsNullOrEmpty(empNumber))
                    usersDTOSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.EMP_NUMBER, string.IsNullOrEmpty(empNumber) ? "" : empNumber));

                if (!string.IsNullOrEmpty(roleId))
                    usersDTOSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_ID, string.IsNullOrEmpty(Convert.ToString(roleId)) ? "" : Convert.ToString(roleId)));

                usersDTOSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_NOT_IN, "Semnox Admin"));
                usersDTOSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                usersDTOSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.USER_STATUS, "ACTIVE"));
                usersDTOSearchParams.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, siteid.ToString()));
                usersDTOList = usersList.GetAllUsers(usersDTOSearchParams);

            }
            catch (Exception ex)
            {
                log.Error("End- error in LoadUsers(" + empNumber + "," + roleId + ") method. " + ex.Message);
            }
            if (usersDTOList == null)
            {
                usersDTOList = new List<UsersDTO>();
            }
            usersDTOList.Insert(0, new UsersDTO());
            usersDTOList[0].UserName = "- Select -";
            usersDTOList[0].UserId = -1;

            cmbStaffName.DataSource = usersDTOList;
            cmbStaffName.DisplayMember = "UserName";
            cmbStaffName.ValueMember = "UserId";
            log.Debug("Ends-LoadUsers(" + empNumber + "," + roleId + ") method.");
        }

        /// <summary>
        /// Event called when Cmbobox staff roles list selected index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStaffRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-cmbStaffRoles_SelectedIndexChanged() ");
            try
            {
                if (cmbStaffRoles.SelectedIndex != -1 && cmbStaffRoles.SelectedIndex != 0)
                {
                    UserRolesDTO dto = (UserRolesDTO)cmbStaffRoles.SelectedItem;
                    string role = dto.RoleId.ToString();
                    LoadUsers("", Convert.ToString(role));
                }
                else
                {
                    LoadUsers("", "");
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnSearch_Click() Error :" + ex.Message);
            }
            log.Debug("Ends-cmbStaffRoles_SelectedIndexChanged() ");
        }

        /// <summary>
        /// Event called when Cmbobox staff name list selected index changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbStaffName_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.Debug("Starts-cmbStaffName_SelectedIndexChanged() method.");
            try
            {
                if (cmbStaffName.SelectedIndex != -1 && cmbStaffName.SelectedIndex != 0)
                {
                    UsersDTO dto = (UsersDTO)cmbStaffName.SelectedItem;
                    int userId = dto.UserId;

                    if (!string.IsNullOrEmpty(Convert.ToString(userId)) && userId != -1)
                    {
                        UsersDTO userdto;
                        try
                        {
                            Users users = new Users(Utilities.ExecutionContext, userId);
                            userdto = users.UserDTO;
                        }
                        catch (Exception ex)
                        {
                            log.Error(ex);
                            userdto = null;
                        }
                        
                        if (userdto != null)
                        {
                            cmbStaffRoles.SelectedValue = userdto.RoleId;
                            cmbStaffName.SelectedValue = userdto.UserId;
                        }
                        else
                            txtMessage.Text = Utilities.MessageUtils.getMessage(730, "WARNING");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends- Error in cmbStaffName_SelectedIndexChanged() method." + ex.Message);
            }
            log.Debug("Ends-cmbStaffName_SelectedIndexChanged() method.");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.Debug("Starts-btnSearch_Click() method.");
            try
            {
                if (cmbStaffName.SelectedIndex != 0 && cmbStaffName.Text != "- Select -")
                {
                    UserObj = (UsersDTO)cmbStaffName.SelectedItem;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    txtMessage.Text = Utilities.MessageUtils.getMessage(1162);
                    UserObj = null;
                }
            }
            catch (Exception ex)
            {
                log.Error("Ends-btnSearch_Click() Error :" + ex.Message);
            }
            log.Debug("Ends-btnSearch_Click() method.");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbStaffName_Format(object sender, ListControlConvertEventArgs e)
        {
            log.Debug("Starts-cmbStaffName_Format() method.");
            try
            {
                UsersDTO userObj = (UsersDTO)e.ListItem;
                if (userObj != null && userObj.UserName != "- Select -")
                {
                    string displayName = string.Empty;
                    displayName = ((UsersDTO)e.ListItem).UserName;

                    if (!string.IsNullOrEmpty(userObj.EmpLastName))
                    {
                        displayName = displayName + " " + userObj.EmpLastName;
                    }

                    displayName = displayName + " - " + userObj.LoginId;
                    e.Value = displayName;
                }
                log.Debug("Ends-cmbStaffName_Format() method.");
            }
            catch (Exception ex)
            {
                log.Error("Ends-error in cmbStaffName_Format() method."+ ex.Message);
            }
        }
    }
}
