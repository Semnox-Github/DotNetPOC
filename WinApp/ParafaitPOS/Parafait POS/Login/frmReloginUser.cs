/********************************************************************************************
 * Project Name - frmReloginUser
 * Description  - Basic relogin Authentication form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80        16-Apr-2020      Guru S A       Created for Redemption UI enhancements
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Parafait_POS.Login
{
    public partial class frmReloginUser : Form
    {
        [DllImport("kernel32.dll")]
        private static extern bool Wow64EnableWow64FsRedirection(bool set);

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string cardNumber = "";
        private bool lockOutMode;
        private Security.User loginUser = null;
        private Security.User currentUser = null;
        private Utilities utilities;
        private System.Diagnostics.Process keyBoardProcess;
        //private int locationX = 0;
        //private int locationY = 0;
        private DeviceClass cardReader;
        internal delegate void GetFormUpdatesDeletegate(bool isVerifiedUser);
        internal GetFormUpdatesDeletegate GetFormUpdates;
        internal delegate void UserEventDelegate(Security.User User);
        internal event UserEventDelegate UserEventHandler; 
        public string GetCurrentLoggedUser { get { return (currentUser != null ? currentUser.LoginId : string.Empty); } }
        public frmReloginUser(Utilities utilities, Security.User currentLoggedUser, DeviceClass cardReaderInput, bool lockOutModeInput = true)
        {
            log.LogMethodEntry(currentLoggedUser, lockOutModeInput);
            InitializeComponent();
            this.utilities = utilities;
            this.lockOutMode = lockOutModeInput;
            this.currentUser = currentLoggedUser;
            this.cardReader = cardReaderInput;
            try
            {
                this.Icon = Properties.Resources.Parafait_icon;
            }
            catch { }
            //lblVersion.Text = "POS - V" + utilities.executeScalar("select version from site").ToString();

            //lblPOSCounter.Text = "POS - " + utilities.executeScalar(@"SELECT ISNULL((select top 1 POSTypeName from POSTypes pt, posmachines pm
            //                                                                    where pt.POSTypeId = pm.posTypeId
            //                                                                        and pm.computer_name = @POSMachine),'')",
            //                                                                        new SqlParameter("@POSMachine", Environment.MachineName)).ToString();
            //lblPOSCounter.Text = utilities.getParafaitDefaults("ENABLE_BIR_REGULATION_PROCESS").Equals("Y") ? lblPOSCounter.Text : "";

            txtPassword.BackColor = txtLogin.BackColor = System.Drawing.ColorTranslator.FromHtml("#037ede");
            lblHeader.ForeColor = System.Drawing.ColorTranslator.FromHtml("#393939");
            //lblVersion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#393939");
            //lblPOSCounter.ForeColor = System.Drawing.ColorTranslator.FromHtml("#393939");
            txtPassword.ForeColor = txtLogin.ForeColor = System.Drawing.ColorTranslator.FromHtml("#cafcd8");
            if (cardReader != null)
            {
                log.Debug("Card Readers: " + cardReader);
                cardReader.Register(new EventHandler(CardScanCompleteEventHandle));
            }

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            //this.Location = new Point(this.locationX - this.Width / 2, this.locationY - this.Height / 2);

            btnOK.DialogResult = DialogResult.None;
            this.TopLevel = false;
            //if (lockOutMode == false && Parent != null)
            //{
            //    this.Left = Parent.Left + (Parent.Width - this.Width) / 2;
            //    this.Top = Parent.Top + (Parent.Height - this.Height) / 2;
            //    this.TopLevel = this.TopMost = true;
            //}
        }

        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                log.Debug("Scanned Number: " + checkScannedEvent.Message);
                TagNumberParser tagNumberParser = new TagNumberParser(utilities.ExecutionContext);
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    string message = tagNumberParser.Validate(checkScannedEvent.Message);
                    POSUtils.ParafaitMessageBox(message);
                    log.LogMethodExit(null, "Invalid Tag Number.");
                    return;
                }

                try
                {
                    CardSwiped(tagNumber.Value);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ClearKeyBoardProcess();
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = "osk.exe";

            try
            {
                Wow64EnableWow64FsRedirection(false);
                keyBoardProcess = System.Diagnostics.Process.Start(psi);
                Wow64EnableWow64FsRedirection(true);
            }
            catch
            {
                try
                {
                    System.Diagnostics.Process.Start(psi);
                }
                catch { }
            }
            log.LogMethodExit();
        }

        private void ClearKeyBoardProcess()
        {
            log.LogMethodEntry();
            try
            {
                if (keyBoardProcess != null)
                {
                    keyBoardProcess.Kill();
                    keyBoardProcess.Close();
                    keyBoardProcess.Dispose();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void OKButton_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.login_button_pressed;
            log.LogMethodExit();
        }

        private void OKButton_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.login_button_normal;
            log.LogMethodExit();
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox t = sender as TextBox;
            t.SelectAll();
            log.LogMethodExit();
        }

        private void txtLogin_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            TextBox t = sender as TextBox;
            t.SelectAll();
            log.LogMethodExit();
        }


        private void OkButtonEvent()
        {
            log.LogMethodEntry();
            bool success = CheckLogin();
            if (success)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                if (lockOutMode)
                {
                    GetFormUpdates(true);
                }
                else
                {
                    UserEventHandler(loginUser);
                }
            }
            else
            {
                txtPassword.SelectAll();
                this.ActiveControl = txtPassword;
            }
            log.LogMethodEntry();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            OkButtonEvent();
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            CancelButtonEvent();
            log.LogMethodExit();
        }

        private void CancelButtonEvent()
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            if (lockOutMode)
            {
                GetFormUpdates(false);
            }
            else
            {
                UserEventHandler(null);
            }
            log.LogMethodExit();
        }

        private void CardSwiped(string inCardNumber)
        {
            log.LogMethodEntry(inCardNumber);
            cardNumber = inCardNumber;
            bool success = CheckLogin();
            cardNumber = "";
            if (success)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
                if (lockOutMode)
                {
                    GetFormUpdates(true);
                }
                else
                {
                    UserEventHandler(loginUser);
                }
            }
            //else
            //{
            //    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(utilities.ExecutionContext, 25));
            //}
            log.LogMethodExit();
        }

        private bool CheckLogin()
        {
            log.LogMethodEntry();
            bool returnValue = false;
            Security sec = new Security(utilities);
            try
            {
                if (string.IsNullOrEmpty(cardNumber))
                {
                    loginUser = sec.Login(txtLogin.Text, txtPassword.Text);
                    
                }
                else
                {
                    loginUser = sec.Login(txtLogin.Text, -1, cardNumber);
                     
                }
                if (lockOutMode)
                {
                    if (loginUser != null && loginUser.LoginId == currentUser.LoginId)
                    {
                        returnValue = true;
                    }
                    else
                    {
                        //"Wrong user login. This screen belows to " + currentUser.LoginId
                        throw new Exception(MessageContainerList.GetMessage(utilities.ExecutionContext, 2680, currentUser.LoginId));
                    }
                }
                else
                {
                    returnValue = true;
                }
            }
            catch (Security.SecurityException se)
            {
                POSUtils.ParafaitMessageBox(se.Message);
                if (System.Runtime.InteropServices.Marshal.GetHRForException(se) == Security.SecurityException.ExChangePassword)
                {
                    frmChangePassword fcp = new frmChangePassword(utilities, cardNumber == null ? txtLogin.Text : null);
                    fcp.ShowDialog();
                }
                returnValue = false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
                returnValue = false;
            }
            log.LogMethodExit(returnValue);
            return returnValue;
        }
        private void frmReloginUser_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            txtLogin.Focus();
            this.Activate();
            this.BringToFront();
            log.LogMethodExit();
        }

        private void frmReloginUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            if (cardReader != null)
            {
                cardReader.UnRegister();
            }
            ClearKeyBoardProcess();
            log.LogMethodExit();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            log.LogMethodEntry(msg, keyData);
            if (keyData.Equals(Keys.Enter))
            {
                if (this.ActiveControl == txtLogin)
                {
                    this.ActiveControl = txtPassword;
                }
                else if (this.ActiveControl == txtPassword)
                {
                    OkButtonEvent();
                }
                return true;

            }
            bool retValue = base.ProcessCmdKey(ref msg, keyData);
            log.LogMethodExit(retValue);
            return retValue;
        }


    }
}
