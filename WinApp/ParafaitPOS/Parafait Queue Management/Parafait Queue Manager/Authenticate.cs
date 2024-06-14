/********************************************************************************************
 * Project Name - Parafait Queue Management
 * Description  - Parafait Queue Manager Authentication
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        10-Sep-2019      Jinto Thomas         Added logger for methods
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Drawing;
using System.Windows.Forms;


namespace ParafaitQueueManagement
{
    static class Authenticate
    {
        static Form formLogin;
        static TextBox txtLogin;
        static TextBox txtPassword;
        static string CardNumber = "";
        static string manager_flag;
        public static string username;
        static string role;
        static int user_id;
        static string loginid;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

            txtLogin.KeyDown += new KeyEventHandler(txtLogin_KeyDown);
            txtPassword.KeyDown += new KeyEventHandler(txtPassword_KeyDown);
            txtLogin.Enter += new EventHandler(txtLogin_Enter);
            txtPassword.Enter += new EventHandler(txtPassword_Enter);
            
            Button OKButton = new Button();
            Button CancelButton = new Button();

            System.Drawing.Font loginFont = new Font(Common.Utilities.getFont().FontFamily, 8f);

            try
            {
                loginLogo.Image = Image.FromFile(Environment.CurrentDirectory + "\\Resources\\LoginLogo.png");
            }
            catch { }
            loginLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            loginLogo.BorderStyle = BorderStyle.None;
            loginLogo.Size = new Size(400, 70);

            lblLogin.Font = loginFont;
            lblPassword.Font = loginFont;

            lblLogin.AutoSize = lblPassword.AutoSize = false;
            lblLogin.Width = lblPassword.Width = 100;
            lblLogin.TextAlign = lblPassword.TextAlign = ContentAlignment.MiddleRight;

            lblLogin.Location = new Point(xpos - 30, ypos);
            lblPassword.Location = new Point(xpos - 30, ypos + 30);

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
            
            formLogin.CancelButton = CancelButton;

            OKButton.Click +=new EventHandler(OKButton_Click);
            CancelButton.Click +=new EventHandler(CancelButton_Click);

            Panel panel = new Panel();
            panel.BackColor = System.Drawing.Color.Turquoise;
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
            formLogin.Text = "Queue Management Login";
            formLogin.Name = "Authenticate";

            Common.Utilities.setLanguage(formLogin);
            try
            {
              //  formLogin.Icon = ParafaitReportsV2.Properties.Resources.parafait_reports;
            }
            catch { }

            formLogin.FormClosing += new FormClosingEventHandler(formLogin_FormClosing);

            
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

        static void txtPassword_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox t = sender as TextBox;
            t.SelectAll();
            log.LogMethodExit();
        }

        static void txtLogin_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox t = sender as TextBox;
            t.SelectAll();
            log.LogMethodExit();
        }

        static void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                OKButton_Click(null, null);
            log.LogMethodExit();
        }

        static void txtLogin_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                formLogin.ActiveControl = txtPassword;
            log.LogMethodExit();
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
           // CardReader.RequiredByOthers = false;
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
            bool success = checkLogin(txtLogin.Text, txtPassword.Text, CardNumber);
            if (success)
            {
                formLogin.DialogResult = DialogResult.OK;
                formLogin.Close();
            }
            else
            {
                txtPassword.Focus();
            }
            log.LogMethodExit();
        }

        public static void cardSwiped()
        {
            log.LogMethodEntry();
           // CardNumber = CardReader.CardNumber;
            bool success = checkLogin(txtLogin.Text, txtPassword.Text, CardNumber);
            if (success)
            {
                formLogin.DialogResult = DialogResult.OK;
                formLogin.Close();
            }
            log.LogMethodExit();
        }

        public static bool checkLogin(string loginId, string passWord, string cardNumber)
        {
            log.LogMethodEntry(loginId, cardNumber);
            Security sec = new Security(Common.Utilities);
            Security.User user = null;
            try
            {
                if (string.IsNullOrEmpty(cardNumber))
                    user = sec.Login(txtLogin.Text, txtPassword.Text);
                else
                    user = sec.Login(cardNumber);
            }
            catch (Security.SecurityException se)
            {
                MessageBox.Show(se.Message);
                if (System.Runtime.InteropServices.Marshal.GetHRForException(se) == Security.SecurityException.ExChangePassword)
                {
                    formLogin.TopMost = false;
                    frmChangePassword fcp = new frmChangePassword(Common.Utilities, string.IsNullOrEmpty(cardNumber) ? txtLogin.Text : null);
                    fcp.ShowDialog();
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex.Message);
                log.LogMethodExit(false);
                return false;
            }
            username = user.UserName;
            loginid = user.LoginId;
            role = user.RoleName;
            int role_id = user.RoleId;
            string userCardNumber = user.CardNumber;
            manager_flag = user.ManagerFlag ? "Y" : "N";
            user_id = user.UserId;

            Common.ParafaitEnv.Username = username;
            Common.ParafaitEnv.User_Id = user_id;
            Common.ParafaitEnv.LoginID = loginid;
            Common.ParafaitEnv.Role = role;
            Common.ParafaitEnv.RoleId = role_id;
            Common.ParafaitEnv.UserCardNumber = userCardNumber;
            Common.ParafaitEnv.Manager_Flag = manager_flag;
            Common.ParafaitEnv.Password = txtPassword.Text;
            log.LogMethodExit(true);
            return true;
        }
            
        static public bool Manager()
        {
            log.LogMethodEntry();
            if (Common.ParafaitEnv.Manager_Flag == "Y")
            return true;

            string tusername = Common.ParafaitEnv.Username;
            int tuser_id = Common.ParafaitEnv.User_Id;
            string tloginid = Common.ParafaitEnv.LoginID;
            string trole = Common.ParafaitEnv.Role;
            int troleId = Common.ParafaitEnv.RoleId;
            string tCardNumber = Common.ParafaitEnv.UserCardNumber;
            string tmanager_flag = Common.ParafaitEnv.Manager_Flag;

            bool success = Login();

            Common.ParafaitEnv.Username = tusername;
            Common.ParafaitEnv.User_Id = tuser_id;
            Common.ParafaitEnv.LoginID = tloginid;
            Common.ParafaitEnv.Role = trole;
            Common.ParafaitEnv.RoleId = troleId;
            Common.ParafaitEnv.UserCardNumber = tCardNumber;
            Common.ParafaitEnv.Manager_Flag = tmanager_flag;

            if (success && manager_flag == "Y")
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

        static public bool User()
        {
            log.LogMethodEntry();
            bool ret = Login();
            log.LogMethodExit(ret);
            return ret;
        }
        static public void loginInternal(string LoginId, string Password)
        {
            log.LogMethodEntry(loginid);
            string decryptPwd = Encryption.Decrypt(Password);
            Security sec = new Security(Common.Utilities);

            try
            {
                Security.User user = sec.Login(LoginId, decryptPwd);

                Common.ParafaitEnv.Username = user.UserName;
                Common.ParafaitEnv.LoginID = user.LoginId;
                Common.ParafaitEnv.Role = user.RoleName;
                Common.ParafaitEnv.UserCardNumber = user.CardNumber;
                Common.ParafaitEnv.Manager_Flag = user.ManagerFlag ? "Y" : "N";
                Common.ParafaitEnv.User_Id = user.UserId;
                Common.ParafaitEnv.Password = Password;
                Common.ParafaitEnv.RoleId = user.RoleId;
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MessageBox.Show(ex.Message, Common.Utilities.MessageUtils.getMessage("Get user details"));
                Environment.Exit(0);
            }
        }
    }
}
