/********************************************************************************************
* Project Name - Parafait Report
* Description  - Authenticate 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.80       23-Aug-2019      Jinto Thomas        Added logger into methods
 * 2.80        18-Sep-2019     Dakshakh raj        Modified : Added logs
 * 2.110       02-Feb-2021     Laster Menezes      Commented CardScanCompleteEventHandle for ReaderDevice validation
 * 2.140.8     03-oct-2023     Rakshith Shetty     Uncommented CardScanCompleteEventHandle for ReaderDevice validation  
********************************************************************************************/

using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;
using System;
using System.Drawing;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
//using ReportsLibrary;
using Semnox.Core.Utilities;
using Semnox.Parafait.Reports;
using System.Drawing;
using System.Windows.Forms;

namespace Semnox.Parafait.Report.Reports
{
    static class Authenticate
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static Form formLogin;
        static TextBox txtLogin;
        static TextBox txtPassword;
        //static string CardNumber = "";//line commented on 28-Sep-2016
        static string manager_flag;
        public static string username;

        //  static string role;
        //static int user_id;
        // static string loginid;
        // public static string SecurityPolicy;// Added for fixing Card Reader Login Problem on 28-Sep-2016
        // static int role_id;// Added for fixing Card Reader Login Problem on 28-Sep-2016

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

            OKButton.Click += new EventHandler(OKButton_Click);
            CancelButton.Click += new EventHandler(CancelButton_Click);

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
            formLogin.Text = "Parafait Reports Login";
            formLogin.Name = "Authenticate";

            Common.Utilities.setLanguage(formLogin);
            try
            {
                formLogin.Icon = Semnox.Parafait.Report.Reports.Properties.Resources.parafait_reports;
            }
            catch { }

            formLogin.FormClosing += new FormClosingEventHandler(formLogin_FormClosing);

            if (ReportsCommon.ReaderDevice != null)
                ReportsCommon.ReaderDevice.Register(new EventHandler(CardScanCompleteEventHandle));

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

        /// <summary>
        /// txtPassword_Enter
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static void txtPassword_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox t = sender as TextBox;
            t.SelectAll();
            log.LogMethodExit();
        }


        /// <summary>
        /// txtLogin_Enter
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static void txtLogin_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox t = sender as TextBox;
            t.SelectAll();
            log.LogMethodExit();
        }

        /// <summary>
        /// txtPassword_KeyDown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                OKButton_Click(null, null);
            log.LogMethodExit();
        }

        /// <summary>
        /// txtLogin_KeyDown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static void txtLogin_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                formLogin.ActiveControl = txtPassword;
            log.LogMethodExit();
        }

        /// <summary>
        /// formLogin_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static void formLogin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ((Form)sender).Activate();
            txtLogin.Focus();
            log.LogMethodExit();
        }


        /// <summary>
        /// formLogin_FormClosing
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static void formLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            CardReader.RequiredByOthers = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// CancelButton_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static void CancelButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            formLogin.DialogResult = DialogResult.Cancel;
            formLogin.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// OKButton_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        static void OKButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //bool success = checkLogin(txtLogin.Text, txtPassword.Text, CardNumber);
            bool success = checkLogin();//Modification for fixing Card Reader Login Problem on 28-Sep-2016
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

        /// <summary>
        /// CardScanCompleteEventHandle
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private static void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumberParser tagNumberParser = new TagNumberParser(Common.Utilities.ExecutionContext);
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    log.LogMethodExit("tagNumber == false");
                    return;
                }
                cardSwiped(tagNumber.Value);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// cardSwiped
        /// </summary>
        /// <param name="CardNumber">CardNumber</param>
        public static void cardSwiped(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            bool success = checkLogin(CardNumber);
            if (success)
            {
                formLogin.DialogResult = DialogResult.OK;
                formLogin.Close();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// checkLogin
        /// </summary>
        /// <param name="CardNumber">CardNumber</param>
        /// <returns></returns>
        static bool checkLogin(string CardNumber = null)
        {
            log.LogMethodEntry(CardNumber);
            Security sec = new Security(Common.Utilities);
            Security.User user = null;
            try
            {
                if (string.IsNullOrEmpty(CardNumber))
                    user = sec.Login(txtLogin.Text, txtPassword.Text, Common.ParafaitEnv.IsCorporate ? Common.ParafaitEnv.SiteId : -1);
                else
                    user = sec.Login(txtLogin.Text, Common.ParafaitEnv.IsCorporate ? Common.ParafaitEnv.SiteId : -1, CardNumber);
            }
            catch (Security.SecurityException se)
            {
                MessageBox.Show(se.Message);
                if (System.Runtime.InteropServices.Marshal.GetHRForException(se) == Security.SecurityException.ExChangePassword)
                {
                    formLogin.TopMost = false;
                    frmChangePassword fcp = new frmChangePassword(Common.Utilities, string.IsNullOrEmpty(CardNumber) ? txtLogin.Text : null);
                    fcp.ShowDialog();
                }
                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.LogMethodExit(false, "Caught an exception " + ex.Message);
                return false;
            }
            Common.ParafaitEnv.Username = user.UserName;
            Common.ParafaitEnv.User_Id = user.UserId;
            Common.ParafaitEnv.LoginID = user.LoginId;
            Common.ParafaitEnv.Role = user.RoleName;
            Common.ParafaitEnv.RoleId = user.RoleId;
            Common.ParafaitEnv.UserCardNumber = user.CardNumber;
            Common.ParafaitEnv.Manager_Flag = user.ManagerFlag ? "Y" : "N";
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// User
        /// </summary>
        /// <returns></returns>
        static public bool User()
        {
            log.LogMethodEntry();         
            bool loggedIn = Login();
            log.LogMethodExit(loggedIn);
            return loggedIn;
        }
    }
}
