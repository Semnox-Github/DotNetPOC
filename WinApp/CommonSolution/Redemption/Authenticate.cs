/********************************************************************************************
 * Class Name - RedemptionUtils                                                                         
 * Description - Authenticate
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Redemption
{
    public class Authenticate
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static Form formLogin;
        static TextBox txtLogin;
        static TextBox txtPassword;
        static string manager_flag;
        public static string username;
        static string role;
        static int role_id;
        static int user_id;
        static string loginid;

        static private bool Login()
        {
            log.LogMethodEntry();
            int xpos = 50;
            int ypos = 100;
            formLogin = new Form();

            PictureBox loginLogo = new PictureBox();

            Label lblLogin = new Label();
            Label lblPassword = new Label();

            txtLogin = new TextBox();
            txtPassword = new TextBox();

            Button OKButton = new Button();
            Button CancelButton = new Button();

            System.Drawing.Font loginFont = new Font(CommonFuncs.Utilities.getFont().FontFamily, 8f);

            try
            {
                loginLogo.Image = Image.FromFile(Environment.CurrentDirectory + "\\Resources\\LoginLogo.png");
            }
            catch (Exception ex)
            {
                log.Error("Error while executing Login()" + ex.Message);
            }
            loginLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            loginLogo.BorderStyle = BorderStyle.None;
            loginLogo.Size = new Size(400, 70);

            lblLogin.Font = loginFont;
            lblPassword.Font = loginFont;
            lblLogin.AutoSize = lblPassword.AutoSize = false;
            lblLogin.Width = 70;
            lblPassword.Width = 80;
            lblLogin.TextAlign = lblPassword.TextAlign = ContentAlignment.MiddleRight;

            lblLogin.Location = new Point(xpos + 10, ypos + 3);
            lblPassword.Location = new Point(xpos - 1, ypos + 33);

            txtLogin.Font = loginFont;
            txtPassword.Font = loginFont;

            txtLogin.Location = new Point(xpos + 80, ypos);
            txtPassword.Location = new Point(xpos + 80, ypos + 30);

            txtLogin.Width = txtPassword.Width = 170;

            OKButton.Font = loginFont;
            CancelButton.Font = loginFont;

            OKButton.Location = new Point(xpos + 80, ypos + 60);
            CancelButton.Location = new Point(xpos + 175, ypos + 60);

            OKButton.BackColor = Color.White;
            CancelButton.BackColor = Color.White;

            OKButton.Height = CancelButton.Height = 25;
            txtPassword.PasswordChar = '*';

            lblLogin.Text = "Login ID:";
            lblPassword.Text = "Password:";

            OKButton.Text = "OK";
            CancelButton.Text = "Cancel";

            formLogin.AcceptButton = OKButton;
            formLogin.CancelButton = CancelButton;

            OKButton.Click += new EventHandler(OKButton_Click);
            CancelButton.Click += new EventHandler(CancelButton_Click);

            Panel panel = new Panel();
            panel.BackColor = System.Drawing.Color.Goldenrod;
            panel.Location = new System.Drawing.Point(4, 4);
            panel.Name = "panel";
            panel.Size = new System.Drawing.Size(392, 232);

            panel.Controls.Add(loginLogo);
            panel.Controls.Add(lblLogin);
            panel.Controls.Add(lblPassword);
            panel.Controls.Add(txtLogin);
            panel.Controls.Add(txtPassword);
            panel.Controls.Add(OKButton);
            panel.Controls.Add(CancelButton);

            formLogin.Controls.Add(panel);

            formLogin.StartPosition = FormStartPosition.CenterScreen;
            formLogin.FormBorderStyle = FormBorderStyle.None;
            formLogin.BackColor = Color.White;
            formLogin.Width = 400;
            formLogin.Height = 240;
            formLogin.Text = "Parafait eZeeInventory Login";
            formLogin.Name = "Authenticate";

            try
            {
                formLogin.Icon = Properties.Resources.redemption;
            }
            catch (Exception ex)
            {
                log.Error("Error while executing Login()" + ex.Message);
            }

            //Added 22-Jul-2015:: Language was not getting set. Explicit call to setLanguage required
            CommonFuncs.Utilities.setLanguage(formLogin);
            //End 22-Jul-2015:: End Modification
            formLogin.FormClosing += new FormClosingEventHandler(formLogin_FormClosing);

            CardReader.setReceiveAction = cardSwiped;

            formLogin.TopMost = true;
            formLogin.Load += new EventHandler(formLogin_Load);
            DialogResult DR = formLogin.ShowDialog();

            if (DR == DialogResult.OK)
            {
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        static void formLogin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ((Form)sender).Activate();
            txtLogin.Focus();
            log.LogMethodExit();
        }

        static void formLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            CardReader.setReceiveAction = null;
            log.LogMethodExit();
        }

        static void CancelButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            formLogin.DialogResult = DialogResult.Cancel;
            formLogin.Close();
            log.LogMethodExit();
        }

        static void OKButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool success = checkLogin();
            if (success)
            {
                formLogin.DialogResult = DialogResult.OK;
                formLogin.Close();
            }
            else
            {
                txtPassword.SelectAll();
                formLogin.ActiveControl = txtPassword;
            }
            log.LogMethodExit();
        }

        public static void cardSwiped()
        {
            log.LogMethodEntry();
            bool success = checkLogin(CardReader.CardNumber);
            if (success)
            {
                formLogin.DialogResult = DialogResult.OK;
                formClose();
            }
            log.LogMethodExit();
        }

        private delegate void UIThreaddelegate();

        static private void formClose()
        {
            log.LogMethodEntry();
            if (txtLogin.InvokeRequired)
            {
                UIThreaddelegate target = formClose;
                txtLogin.Invoke(target);
            }
            else
            {
                formLogin.Close();
            }
            log.LogMethodExit();
        }

        static bool checkLogin(string CardNumber = null)
        {
            log.LogMethodEntry(CardNumber);
            Security sec = new Security(CommonFuncs.Utilities);
            Security.User user = null;
            try
            {
                if (string.IsNullOrEmpty(CardNumber))
                    user = sec.Login(txtLogin.Text, txtPassword.Text);
                else
                    user = sec.Login(CardNumber);
            }
            catch (Security.SecurityException se)
            {
                MessageBox.Show(se.Message);
                if (System.Runtime.InteropServices.Marshal.GetHRForException(se) == Security.SecurityException.ExChangePassword)
                {
                    formLogin.TopMost = false;
                    frmChangePassword fcp = new frmChangePassword(CommonFuncs.Utilities, string.IsNullOrEmpty(CardNumber) ? txtLogin.Text : null);
                    fcp.ShowDialog();
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.LogMethodExit(false);
                return false;
            }
            username = user.UserName;
            loginid = user.LoginId;
            role = user.RoleName;
            role_id = user.RoleId;
            string userCardNumber = user.CardNumber;
            manager_flag = user.ManagerFlag ? "Y" : "N";
            user_id = user.UserId;

            CommonFuncs.ParafaitEnv.Username = username;
            CommonFuncs.ParafaitEnv.User_Id = user_id;
            CommonFuncs.ParafaitEnv.LoginID = loginid;
            CommonFuncs.ParafaitEnv.Role = role;
            CommonFuncs.ParafaitEnv.RoleId = role_id;
            CommonFuncs.ParafaitEnv.UserCardNumber = userCardNumber;
            CommonFuncs.ParafaitEnv.Manager_Flag = manager_flag;
            CommonFuncs.ParafaitEnv.Password = txtPassword.Text;

            log.LogMethodExit(true);
            return true;
        }
        /**
       /// Modified on:30-Dec-2016
       /// Reason : For manger approval in based on hierarchy 
       **/
        static public bool Manager()
        {
            log.LogMethodEntry();
            string msg = string.Empty;
            List<int> managerRoleIds = GetAssignedManagerRoleId(CommonFuncs.ParafaitEnv.RoleId, ref msg);

            if (msg != string.Empty)
            {
                System.Windows.Forms.MessageBox.Show(CommonFuncs.Utilities.MessageUtils.getMessage(1125));
                log.LogMethodExit(false);
                return false;
            }

            //If Login user has manager flag and none of the manager role id are assigned
            if ((managerRoleIds.Count == 0 && IsRoleExist(CommonFuncs.ParafaitEnv.RoleId)) || (CommonFuncs.ParafaitEnv.Manager_Flag == "Y" && managerRoleIds.Count == 0))
            {
                log.LogMethodExit(true);
                return true;
            }
            string tusername = CommonFuncs.ParafaitEnv.Username;
            int tuser_id = CommonFuncs.ParafaitEnv.User_Id;
            string tloginid = CommonFuncs.ParafaitEnv.LoginID;
            string trole = CommonFuncs.ParafaitEnv.Role;
            int troleId = CommonFuncs.ParafaitEnv.RoleId;
            string tCardNumber = CommonFuncs.ParafaitEnv.UserCardNumber;
            string tmanager_flag = CommonFuncs.ParafaitEnv.Manager_Flag;

            bool success = Login();

            CommonFuncs.ParafaitEnv.Username = tusername;
            CommonFuncs.ParafaitEnv.User_Id = tuser_id;
            CommonFuncs.ParafaitEnv.LoginID = tloginid;
            CommonFuncs.ParafaitEnv.Role = trole;
            CommonFuncs.ParafaitEnv.RoleId = troleId;
            CommonFuncs.ParafaitEnv.UserCardNumber = tCardNumber;
            CommonFuncs.ParafaitEnv.Manager_Flag = tmanager_flag;

            if (success)
            {
                log.LogMethodExit(true);
                return true;
            }

            log.LogMethodExit(false);
            return false;
        }//end
       
        /**
        /// Modified on:30-Dec-2016
        /// Reason : For Getting assigned manager role id
        **/
        static public List<int> GetAssignedManagerRoleId(int userRoleId, ref string message)
        {
            log.LogMethodEntry(userRoleId, message);
            List<int> roleIds = new List<int>();
            DataTable assignedManagerRoleId;
            try
            {
                assignedManagerRoleId = CommonFuncs.Utilities.executeDataTable(@"with n(role_id, role, AssignedManagerRoleId, Level) as 
	                                                                                    (select role_id, role, AssignedManagerRoleId, 1 as Level
	                                                                                    from user_roles
	                                                                                    where role_id= @roleid
	                                                                                    union all
	                                                                                    select u.role_id, u.role,u.AssignedManagerRoleId, n.Level + 1
	                                                                                    from user_roles u, n 
	                                                                                    where u.role_id = n.AssignedManagerRoleId)
	                                                                                    select role_id, role, AssignedManagerRoleId, Level from n"
                                                                                       , new SqlParameter("@roleid", userRoleId));
            }
            catch (SqlException SQlex)
            {
                for (int i = 0; i < SQlex.Errors.Count; i++)
                {
                    message = "Index #" + i + "\n" +
                             "Message: " + SQlex.Errors[i].Message + "\n" +
                             "LineNumber: " + SQlex.Errors[i].LineNumber + "\n" +
                             "Source: " + SQlex.Errors[i].Source + "\n";
                }
                log.LogMethodExit();
                return new List<int>();
            }

            if (assignedManagerRoleId != null)
            {
                foreach (DataRow row in assignedManagerRoleId.Rows)
                {
                    if (!DBNull.Value.Equals(row["AssignedManagerRoleId"]))
                    {
                        roleIds.Add(Convert.ToInt32(row["AssignedManagerRoleId"]));
                    }
                }
            }
            log.LogMethodExit(roleIds);
            return roleIds;
        }//end
        /**
        /// Modified on:30-Dec-2016
        /// Reason : For checking is role manager for any other user 
       **/
        public static bool IsRoleExist(int roleId)
        {
            log.LogMethodEntry(roleId);
            try
            {
                DataTable dt = CommonFuncs.Utilities.executeDataTable(@"SELECT distinct AssignedManagerRoleId FROM user_roles WHERE AssignedManagerRoleId is not null");

                if (dt != null && dt.Rows.Count > 0)
                {
                    bool roleExist = dt.AsEnumerable().Where(c => c.Field<int>("AssignedManagerRoleId").Equals(roleId)).Count() > 0;
                    if (roleExist)
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while executing IsRoleExist()" + ex.Message);
            }
            log.LogMethodExit(false);
            return false;
        }

        static public bool User()
        {
            log.LogMethodEntry();
            bool returnValue = Login();
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
