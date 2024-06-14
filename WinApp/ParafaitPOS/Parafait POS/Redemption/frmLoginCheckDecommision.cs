/********************************************************************************************
* Project Name - Parafait POS
* Description  - frmLoginCheck 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Device;
using Semnox.Core.Utilities;

namespace Parafait_POS.Redemption
{
    public partial class frmLoginCheckDecommission : Form
    {
        internal Security.User _User = null;
        DeviceClass _cardReader;
        private readonly TagNumberParser tagNumberParser;

        public frmLoginCheckDecommission(Form Parent, DeviceClass cardReader)
        {
            InitializeComponent();
            POSStatic.Utilities.setLanguage(this);//added on 26-Jul-2017
            tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
            this.FormClosing += frmLoginCheck_FormClosing;

            _cardReader = cardReader;

            if (cardReader != null)
                cardReader.Register(new EventHandler(CardScanCompleteEventHandle));

            this.Left = Parent.Left + (Parent.Width - this.Width) / 2;
            this.Top = Parent.Top + (Parent.Height - this.Height) / 2;

            this.TopLevel = this.TopMost = true;
        }

        void frmLoginCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cardReader != null)
                _cardReader.UnRegister();
        }

        public delegate void UserEventDelegate(Security.User User);
        public event UserEventDelegate UserEventHandler;

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    POSUtils.ParafaitMessageBox(message);
                    return;
                }

                string CardNumber = tagNumber.Value;
                try
                {
                    CheckCardLogin(CardNumber);
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }
        }

        void OKButton_MouseDown(object sender, MouseEventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.login_button_pressed;
        }

        void OKButton_MouseUp(object sender, MouseEventArgs e)
        {
            (sender as Button).BackgroundImage = Properties.Resources.login_button_normal;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            UserEventHandler(null);
            Close();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            bool success = checkLogin(null, ref _User);
            if (success)
            {
                UserEventHandler(_User);
                Close();
            }
            else
            {
                txtPassword.SelectAll();
                ActiveControl = txtPassword;
            }
        }

        void CheckCardLogin(string CardNumber)
        {
            bool success = checkLogin(CardNumber, ref _User);
            if (success)
            {
                UserEventHandler(_User);
                Close();
            }
            else
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(25));
            }
        }

        bool checkLogin(string CardNumber, ref Security.User user)
        {
            Security sec = new Security(POSStatic.Utilities);
            try
            {
                if (string.IsNullOrEmpty(CardNumber))
                    user = sec.Login(txtLogin.Text, txtPassword.Text);
                else
                    user = sec.Login(txtLogin.Text, -1, CardNumber);
            }
            catch (Security.SecurityException se)
            {
                POSUtils.ParafaitMessageBox(se.Message);
                if (System.Runtime.InteropServices.Marshal.GetHRForException(se) == Security.SecurityException.ExChangePassword)
                {
                    frmChangePassword fcp = new frmChangePassword(POSStatic.Utilities, CardNumber == null ? txtLogin.Text : null);
                    fcp.ShowDialog();
                }
                return false;
            }
            catch (Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
                return false;
            }

            return true;
        }

        private void txtLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ActiveControl = txtPassword;
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OKButton_Click(null, null);
            }
        }

        private void frmLoginCheck_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtLogin;
            txtLogin.Focus();
        }
    }
}
