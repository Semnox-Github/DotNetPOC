using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Core.Utilities;
namespace Semnox.Core.Utilities
{
    public partial class frmChangePassword : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Semnox.Core.Utilities.Utilities _utilities;
        TextBox CurrentTextBox;
        public frmChangePassword(Semnox.Core.Utilities.Utilities inUtilities, string loginId = null)
        {
            log.LogMethodEntry(inUtilities, loginId);
            _utilities = inUtilities;

            InitializeComponent();
            if (string.IsNullOrEmpty(loginId) == false)
            {
                txtLoginId.Text = loginId;
                txtLoginId.Enabled = false;
            }
            log.LogMethodExit(null);
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Location = new Point(this.Location.X, this.Location.Y);
            SetKeyboardImage();
            _utilities.setLanguage(this);
            log.LogMethodExit(null);
        }

        private void SetKeyboardImage()
        {
            log.LogMethodEntry();
            this.btnShowKeyPad.BackgroundImage = Semnox.Core.Utilities.Properties.Resources.keyboard;
            this.btnShowKeyPad.Size = new Size(Semnox.Core.Utilities.Properties.Resources.keyboard.Width, Semnox.Core.Utilities.Properties.Resources.keyboard.Height);
            log.LogMethodExit();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (string.IsNullOrEmpty(txtLoginId.Text.Trim()))
            {
                MessageBox.Show("Login Id cannot be empty", _utilities.MessageUtils.getMessage("Validation"));
                txtLoginId.Focus();
                log.LogMethodExit(null);
                return;
            }

            string loginId = txtLoginId.Text.Trim();

            byte[] hash = new byte[0];
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                MessageBox.Show(_utilities.MessageUtils.getMessage(273), _utilities.MessageUtils.getMessage("Validation"));
                txtPassword.Focus();
                log.LogMethodExit(null);
                return;
            }

            if (txtReenterPassword.Text.Equals(txtNewPassword.Text) == false)
            {
                MessageBox.Show(_utilities.MessageUtils.getMessage(274), _utilities.MessageUtils.getMessage("Validation"));
                txtReenterPassword.Focus();
                log.LogMethodExit(null);
                return;
            }

            Security sec = new Security(_utilities);

            try
            {
                sec.ChangePassword(loginId, txtPassword.Text, txtNewPassword.Text);
                MessageBox.Show(_utilities.MessageUtils.getMessage(275));
                Close();
            }
            catch (Security.SecurityException se)
            {
                MessageBox.Show(se.Message);
                log.Error("Error occured while changing the password - SecurityException", se);
                log.LogMethodExit(null);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error("Error occured while changing the password - Exception", ex);
                log.LogMethodExit(null);
                return;
            }
            log.LogMethodExit(null);
        }

        private void txtLoginId_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            (sender as TextBox).SelectAll();
            CurrentTextBox = sender as TextBox;
            if (keypad != null)
            {
                keypad.currentTextBox = CurrentTextBox;
            }
            log.LogMethodExit(null);
        }

        AlphaNumericKeyPad keypad;
        private void btnShowKeyPad_Click(object sender, EventArgs e)
        {
            if (keypad == null || keypad.IsDisposed)
            {
                if (CurrentTextBox == null)
                {
                    CurrentTextBox = new TextBox();
                }
                keypad = new AlphaNumericKeyPad(FindParentForm(this), CurrentTextBox, 60);
                keypad.Show();
            }
            else if (keypad.Visible)
            {
                keypad.Hide();
            }
            else
            {
                keypad.Show();
            }
        }
        private Form FindParentForm(Control childControl)
        {
            log.LogMethodEntry(childControl);
            Form parent = null;
            if (childControl is Form)
            {
                parent = childControl as Form;
            }
            else
            {
                parent = FindParentForm(childControl.Parent);
            }
            log.LogMethodExit(parent);
            return parent;
        }

    }
}
