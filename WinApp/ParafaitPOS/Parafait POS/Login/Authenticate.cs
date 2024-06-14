/********************************************************************************************
 * Project Name - Authenticate
 * Description  - Authenticate class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        14-Dec-2018      Raghuveera     Modified for getting encrypted key value 
 *2.60.0      23-Feb-2019      Mathew         Modified to register all card readers instead of
 *                                            primary reader
 *2.70        1-Jul-2019       Lakshminarayana     Modified to add support for ULC cards 
 *2.80        20-Aug-2019      Girish Kundar  Modified : Added Logger methods and Removed unused namespace's 
 *2.80        26-Feb-2020      Indrajeet K    Added LoginFingerPrint Method and Modified CheckLogin to Support Login FingerPrint
 *2.110       11-Jan-2021      Deeksha        Modified as part of attendance & PayRate Enhancement to set Role Id based on selected Attendance Role
 *2.110       11-Feb-2021      Girish Kundar  Modified : As part of tokenization changes -  adding session Id to the table
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    static class Authenticate
    {
        static Form formLogin;
        static TextBox txtLogin;
        static TextBox txtPassword;
        static string CardNumber = "";
        public static bool LoginChanged = true;
        static Security.User _User = null;
        static System.Diagnostics.Process keyBoardProcess = null;
        //Added on 16-May-2016 to handle OSK issue with 64 bit processor
        [DllImport("kernel32.dll")]
        private static extern bool Wow64EnableWow64FsRedirection(bool set);

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static private bool Login(ref Security.User user, bool topMostWindow = true, bool ManagerApproval = false)
        {
            log.LogMethodEntry(user, topMostWindow, ManagerApproval);
            formLogin = new Form();

            System.Windows.Forms.Panel panelLogin;
            System.Windows.Forms.Panel panelLoginId;
            System.Windows.Forms.PictureBox pictureBoxUser;
            System.Windows.Forms.Panel panelPassword;
            System.Windows.Forms.PictureBox pictureBoxPassword;
            System.Windows.Forms.Button OKButton;
            System.Windows.Forms.Label lblVersion;
            System.Windows.Forms.Label lblPOSCounter;
            System.Windows.Forms.Label lblHeader;
            System.Windows.Forms.PictureBox pictureBoxLogo;
            System.Windows.Forms.Button CancelButton;
            System.Windows.Forms.Button btnShowNumPad;
            System.Windows.Forms.LinkLabel lnkFingerPrintLogin;

            panelLogin = new System.Windows.Forms.Panel();
            CancelButton = new System.Windows.Forms.Button();
            OKButton = new System.Windows.Forms.Button();
            lblVersion = new System.Windows.Forms.Label();
            lblPOSCounter = new System.Windows.Forms.Label();
            lblHeader = new System.Windows.Forms.Label();
            panelPassword = new System.Windows.Forms.Panel();
            pictureBoxPassword = new System.Windows.Forms.PictureBox();
            txtPassword = new System.Windows.Forms.TextBox();
            panelLoginId = new System.Windows.Forms.Panel();
            pictureBoxUser = new System.Windows.Forms.PictureBox();
            txtLogin = new System.Windows.Forms.TextBox();
            pictureBoxLogo = new System.Windows.Forms.PictureBox();
            btnShowNumPad = new Button();
            lnkFingerPrintLogin = new System.Windows.Forms.LinkLabel();

            panelLogin.SuspendLayout();
            panelPassword.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxPassword)).BeginInit();
            panelLoginId.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxLogo)).BeginInit();
            formLogin.SuspendLayout();
            // 
            // panelLogin
            // 
            panelLogin.BackgroundImage = Properties.Resources.login_bg;
            panelLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            panelLogin.Controls.Add(CancelButton);
            panelLogin.Controls.Add(OKButton);
            panelLogin.Controls.Add(lblVersion);
            panelLogin.Controls.Add(lblPOSCounter);
            panelLogin.Controls.Add(lblHeader);
            panelLogin.Controls.Add(panelPassword);
            panelLogin.Controls.Add(panelLoginId);
            panelLogin.Controls.Add(btnShowNumPad);
            panelLogin.Controls.Add(lnkFingerPrintLogin);
            panelLogin.Location = new System.Drawing.Point(90, 124);
            panelLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panelLogin.Name = "panelLogin";
            panelLogin.Size = new System.Drawing.Size(393, 302);
            panelLogin.TabIndex = 0;
            // 
            // CancelButton
            // 
            CancelButton.BackColor = System.Drawing.Color.Transparent;
            CancelButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            CancelButton.FlatAppearance.BorderSize = 0;
            CancelButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            CancelButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            CancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CancelButton.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CancelButton.ForeColor = System.Drawing.Color.Black;
            CancelButton.Location = new System.Drawing.Point(344, 12);
            CancelButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new System.Drawing.Size(28, 37);
            CancelButton.TabIndex = 4;
            CancelButton.Text = "X";
            CancelButton.UseVisualStyleBackColor = false;
            CancelButton.Click += new System.EventHandler(CancelButton_Click);
            // 
            // OKButton
            // 
            OKButton.BackColor = System.Drawing.Color.Transparent;
            OKButton.BackgroundImage = Properties.Resources.login_button_normal;
            OKButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            OKButton.FlatAppearance.BorderSize = 0;
            OKButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            OKButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            OKButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            OKButton.ForeColor = System.Drawing.Color.White;
            OKButton.Location = new System.Drawing.Point(192, 237);
            OKButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            OKButton.Name = "OKButton";
            OKButton.Size = new System.Drawing.Size(193, 51);
            OKButton.TabIndex = 3;
            OKButton.Text = "     Login";
            OKButton.UseVisualStyleBackColor = false;
            OKButton.MouseDown += new System.Windows.Forms.MouseEventHandler(OKButton_MouseDown);
            OKButton.MouseUp += new System.Windows.Forms.MouseEventHandler(OKButton_MouseUp);
            OKButton.Click += OKButton_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = false;
            lblVersion.BackColor = System.Drawing.Color.Transparent;
            lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblVersion.ForeColor = System.Drawing.Color.DimGray;
            lblVersion.Location = new System.Drawing.Point(0, 0);
            lblVersion.TextAlign = ContentAlignment.MiddleLeft;
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new System.Drawing.Size(400, 20);
            lblVersion.TabIndex = 2;
            lblVersion.Text = "POS - V" + POSStatic.Utilities.executeScalar("select version from site").ToString();

            // 
            // lblPOSCounter
            // 
            lblPOSCounter.AutoSize = false;
            lblPOSCounter.BackColor = System.Drawing.Color.Transparent;
            lblPOSCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblPOSCounter.ForeColor = System.Drawing.Color.DimGray;
            lblPOSCounter.Location = new System.Drawing.Point(0, 5);
            lblPOSCounter.TextAlign = ContentAlignment.MiddleLeft;
            lblPOSCounter.Name = "lblPOSCounter";
            lblPOSCounter.Size = new System.Drawing.Size(400, 40);
            lblPOSCounter.TabIndex = 3;
            lblPOSCounter.Text = "POS - " + POSStatic.Utilities.executeScalar(@"SELECT ISNULL((select top 1 POSTypeName from POSTypes pt, posmachines pm
                                                                                where pt.POSTypeId = pm.posTypeId
                                                                                    and pm.computer_name = @POSMachine),'')",
                                                                                    new SqlParameter("@POSMachine", Environment.MachineName)).ToString();
            lblPOSCounter.Text = POSStatic.Utilities.getParafaitDefaults("ENABLE_BIR_REGULATION_PROCESS").Equals("Y") ? lblPOSCounter.Text : "";
            // 
            // lblHeader
            // 
            lblHeader.AutoSize = false;
            lblHeader.BackColor = System.Drawing.Color.Transparent;
            lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblHeader.ForeColor = System.Drawing.Color.DimGray;
            lblHeader.Location = new System.Drawing.Point(1, 45);
            lblHeader.TextAlign = ContentAlignment.MiddleCenter;
            lblHeader.Name = "lblHeader";
            lblHeader.Size = new System.Drawing.Size(400, 20);
            lblHeader.TabIndex = 2;
            lblHeader.Text = "User Login";
            if (ManagerApproval)
                lblHeader.Text += " - Manager Approval";
            // 
            // panelPassword
            // 
            panelPassword.BackColor = System.Drawing.Color.Transparent;
            panelPassword.BackgroundImage = Properties.Resources.password_button;
            panelPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            panelPassword.Controls.Add(pictureBoxPassword);
            panelPassword.Controls.Add(txtPassword);
            panelPassword.Location = new System.Drawing.Point(24, 160);
            panelPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panelPassword.Name = "panelPassword";
            panelPassword.Size = new System.Drawing.Size(351, 55);
            panelPassword.TabIndex = 1;
            // 
            // pictureBoxPassword
            // 
            pictureBoxPassword.BackgroundImage = Properties.Resources.password_icon;
            pictureBoxPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBoxPassword.Location = new System.Drawing.Point(6, 0);
            pictureBoxPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pictureBoxPassword.Name = "pictureBoxPassword";
            pictureBoxPassword.Size = new System.Drawing.Size(32, 55);
            pictureBoxPassword.TabIndex = 1;
            pictureBoxPassword.TabStop = false;
            // 
            // txtPassword
            // 
            txtPassword.BackColor = System.Drawing.Color.RoyalBlue;
            txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtPassword.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            txtPassword.ForeColor = System.Drawing.Color.White;
            txtPassword.Location = new System.Drawing.Point(42, 16);
            txtPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new System.Drawing.Size(290, 25);
            txtPassword.TabIndex = 0;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // panelLoginId
            // 
            panelLoginId.BackColor = System.Drawing.Color.Transparent;
            panelLoginId.BackgroundImage = Properties.Resources.password_button;
            panelLoginId.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            panelLoginId.Controls.Add(pictureBoxUser);
            panelLoginId.Controls.Add(txtLogin);
            panelLoginId.Location = new System.Drawing.Point(24, 78);
            panelLoginId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            panelLoginId.Name = "panelLoginId";
            panelLoginId.Size = new System.Drawing.Size(351, 55);
            panelLoginId.TabIndex = 0;
            // 
            // pictureBoxUser
            // 
            pictureBoxUser.BackgroundImage = Properties.Resources.username_icon;
            pictureBoxUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            pictureBoxUser.Location = new System.Drawing.Point(6, 0);
            pictureBoxUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            pictureBoxUser.Name = "pictureBoxUser";
            pictureBoxUser.Size = new System.Drawing.Size(32, 55);
            pictureBoxUser.TabIndex = 1;
            pictureBoxUser.TabStop = false;
            // 
            // txtLogin
            // 
            txtLogin.BackColor = System.Drawing.Color.SaddleBrown;
            txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtLogin.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            txtLogin.ForeColor = System.Drawing.Color.White;
            txtLogin.Location = new System.Drawing.Point(42, 16);
            txtLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new System.Drawing.Size(290, 25);
            txtLogin.TabIndex = 0;
            // 
            // pictureBoxLogo
            // 
            pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            string exeDir = Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""));
            if (File.Exists(exeDir + "\\Resources\\ParafaitLogoLogin.png"))
            {
                pictureBoxLogo.BackgroundImage = Image.FromFile(exeDir + "\\Resources\\ParafaitLogoLogin.png");
            }
            else
            {
                pictureBoxLogo.BackgroundImage = Properties.Resources.ParafaitLogoLogin;
            }
            pictureBoxLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            pictureBoxLogo.Location = new System.Drawing.Point(23, 34);
            pictureBoxLogo.Name = "pictureBoxLogo";
            pictureBoxLogo.Size = new System.Drawing.Size(286, 124);
            pictureBoxLogo.TabIndex = 1;
            pictureBoxLogo.TabStop = false;
            // 
            // lnkFingerPrintLogin
            //
            lnkFingerPrintLogin.AutoSize = true;
            lnkFingerPrintLogin.BackColor = System.Drawing.Color.Transparent;
            lnkFingerPrintLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lnkFingerPrintLogin.Location = new System.Drawing.Point(67, 272);
            lnkFingerPrintLogin.Name = "lnkFingerPrintLogin";
            lnkFingerPrintLogin.Size = new System.Drawing.Size(129, 13);
            lnkFingerPrintLogin.TabStop = true;
            lnkFingerPrintLogin.Text = "Use FingerPrint Login";
            lnkFingerPrintLogin.Visible = POSStatic.Utilities.getParafaitDefaults("POS_FINGER_PRINT_AUTHENTICATION") == "Y" ? true : false;
            lnkFingerPrintLogin.LinkClicked += lnkFingerPrintLogin_LinkClicked;
            // 
            // btnShowNumPad
            // 
            btnShowNumPad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            btnShowNumPad.BackColor = System.Drawing.Color.Transparent;
            btnShowNumPad.BackgroundImage = global::Parafait_POS.Properties.Resources.keyboard;
            btnShowNumPad.CausesValidation = false;
            btnShowNumPad.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            btnShowNumPad.FlatAppearance.BorderSize = 0;
            btnShowNumPad.FlatAppearance.MouseOverBackColor = btnShowNumPad.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            btnShowNumPad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnShowNumPad.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            btnShowNumPad.ForeColor = System.Drawing.Color.Black;
            btnShowNumPad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            btnShowNumPad.Location = new System.Drawing.Point(24, 255);
            btnShowNumPad.Name = "btnShowNumPad";
            btnShowNumPad.Size = new System.Drawing.Size(36, 36);
            btnShowNumPad.TabIndex = 21;
            btnShowNumPad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            btnShowNumPad.UseVisualStyleBackColor = false;
            btnShowNumPad.Click += new System.EventHandler(btnShowNumPad_Click);

            txtPassword.BackColor =
            txtLogin.BackColor = System.Drawing.ColorTranslator.FromHtml("#037ede");

            lblHeader.ForeColor = System.Drawing.ColorTranslator.FromHtml("#393939");
            lblVersion.ForeColor = System.Drawing.ColorTranslator.FromHtml("#393939");
            lblPOSCounter.ForeColor = System.Drawing.ColorTranslator.FromHtml("#393939");
            txtPassword.ForeColor = txtLogin.ForeColor = System.Drawing.ColorTranslator.FromHtml("#cafcd8");

            txtLogin.KeyDown += new KeyEventHandler(txtLogin_KeyDown);
            txtPassword.KeyDown += txtPassword_KeyDown;
            txtLogin.Enter += new EventHandler(txtLogin_Enter);
            txtPassword.Enter += new EventHandler(txtPassword_Enter);
            // 
            // formLogin
            // 
            formLogin.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            formLogin.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            formLogin.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            formLogin.BackColor = System.Drawing.Color.Aquamarine;
            formLogin.ClientSize = new System.Drawing.Size(533, 488);
            formLogin.Controls.Add(panelLogin);
            formLogin.Controls.Add(pictureBoxLogo);
            formLogin.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            formLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            formLogin.Name = "formLogin";
            formLogin.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            formLogin.Text = "Login";
            formLogin.TransparencyKey = System.Drawing.Color.Aquamarine;
            formLogin.CancelButton = CancelButton;
            panelLogin.ResumeLayout(false);
            panelLogin.PerformLayout();
            panelPassword.ResumeLayout(false);
            panelPassword.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxPassword)).EndInit();
            panelLoginId.ResumeLayout(false);
            panelLoginId.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(pictureBoxLogo)).EndInit();
            formLogin.ResumeLayout(false);

            try
            {
                formLogin.Icon = Properties.Resources.Parafait_icon;
            }
            catch { }

            formLogin.FormClosing += new FormClosingEventHandler(formLogin_FormClosing);

            // Common.Devices.RegisterPrimaryCardReader(new EventHandler(CardScanCompleteEventHandle));
            log.Debug("Card Readers: " + Common.Devices.CardReaders);
            Common.Devices.RegisterCardReaders(new EventHandler(CardScanCompleteEventHandle));

            formLogin.TopMost = topMostWindow;
            formLogin.Load += new EventHandler(formLogin_Load);
            OKButton.DialogResult = DialogResult.None;
            POSStatic.Utilities.setLanguage(formLogin);
            DialogResult DR = DialogResult.None;
            if (ShowScreenSaver)
            {
                frmScreenSaver screenSaver = new frmScreenSaver();
                screenSaver.Load += delegate
                {
                    DR = formLogin.ShowDialog();
                    screenSaver.Close();
                };
                screenSaver.ShowDialog();
            }
            else
            {
                DR = formLogin.ShowDialog();
            }

            user = _User;
            log.LogMethodExit(DR == DialogResult.OK);
            if (DR == DialogResult.OK)
                return true;
            else
                return false;
        }

        private static void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is DeviceScannedEventArgs)
            {
                DeviceScannedEventArgs checkScannedEvent = e as DeviceScannedEventArgs;
                log.Debug("Scanned Number: " + checkScannedEvent.Message);
                TagNumberParser tagNumberParser = new TagNumberParser(POSStatic.Utilities.ExecutionContext);
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
                    cardSwiped(tagNumber.Value);
                }
                catch (Exception ex)
                {
                    POSUtils.ParafaitMessageBox(ex.Message);
                }
            }
            log.LogMethodExit();
        }

        //Modified to handle osk issue with 64 bit processor 16-May-2016
        static void btnShowNumPad_Click(object sender, EventArgs e)
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

        static void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                OKButton_Click(null, null);
            }
            log.LogMethodExit();
        }

        static void OKButton_Click(object sender, EventArgs e)

        {
            log.LogMethodEntry();
            bool success = checkLogin(ref _User);
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

        static void lnkFingerPrintLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            formLogin.Dispose();
            frmPOSFingerPrintLogin frmPOSFingerPrintLogin = new frmPOSFingerPrintLogin();
            frmPOSFingerPrintLogin.Closed += (s, args) => formLogin.Close();
            formLogin.DialogResult = frmPOSFingerPrintLogin.ShowDialog();
            log.LogMethodExit();
        }

        static void OKButton_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.login_button_pressed;
            log.LogMethodExit();
        }

        static void OKButton_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.login_button_normal;
            log.LogMethodExit();
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
            ((Form)sender).Show();
            ((Form)sender).Activate();
            txtLogin.Focus();
            ((Form)sender).Activate();
            log.LogMethodExit();
        }

        static void formLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            //Common.Devices.UnregisterPrimaryCardReader();
            Common.Devices.UnregisterCardReaders();
            ClearKeyBoardProcess();
            log.LogMethodExit();
        }

        static void CancelButton_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            formLogin.DialogResult = DialogResult.Cancel;
            formLogin.Close();
            log.LogMethodExit();
        }

        public static void cardSwiped(string inCardNumber)
        {
            log.LogMethodEntry(inCardNumber);
            CardNumber = inCardNumber;
            bool success = checkLogin(ref _User);
            CardNumber = "";
            if (success)
            {
                formLogin.DialogResult = DialogResult.OK;
                formLogin.Close();
            }
            else
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(25));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// FingerSwiped Method Authenticate the Finger Print
        /// </summary>
        /// <param name="usersDTO"></param>
        public static void FingerSwiped(UsersDTO usersDTO)
        {
            log.LogMethodEntry(usersDTO);
            try
            {
                bool success = checkLogin(ref _User, usersDTO);
                log.Debug("Success Value : " + success);
                //if (success)
                //{
                //    formLogin.DialogResult = DialogResult.OK;
                //    formLogin.Close();
                //}
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
            log.LogMethodExit();
        }

        static bool checkLogin(ref Security.User user, UsersDTO usersDTO = null)
        {
            log.LogMethodEntry();
            Security sec = new Security(POSStatic.Utilities);
            try
            {
                if (string.IsNullOrEmpty(CardNumber) && usersDTO == null)
                    user = sec.Login(txtLogin.Text, txtPassword.Text);
                else if (usersDTO != null
                         && usersDTO.UserIdentificationTagsDTOList != null
                         && usersDTO.UserIdentificationTagsDTOList.Any(x => x.FingerNumber > -1 && !string.IsNullOrEmpty(x.FPSalt)))
                    user = LoginFingerPrint(usersDTO); //Modified to Support FingerPrint Login
                else
                    //Modified to accept Login ID and Card Number tap. Site id is passed as -1 since HQ won't have POS login. 29-Sep-2015
                    //user = sec.Login(CardNumber);
                    user = sec.Login(txtLogin.Text, -1, CardNumber);

                //Modified to consider attendance Role Id during user Login and on manager approval 
                int attendanceRoleId = GetAttendanceRoleId(user.UserId);
                if (attendanceRoleId != -1)
                {
                    UserRoles usersRolesBL = new UserRoles(POSStatic.Utilities.ExecutionContext, attendanceRoleId);
                    if (usersRolesBL.getUserRolesDTO != null)
                    {
                        user.RoleId = attendanceRoleId;
                        user.RoleName = usersRolesBL.getUserRolesDTO.Role;
                        user.ManagerFlag = usersRolesBL.getUserRolesDTO.ManagerFlag == "Y" ? true : false;
                        user.EnablePOSClockIn = usersRolesBL.getUserRolesDTO.EnablePOSClockIn;
                        user.AllowShiftOpenClose = usersRolesBL.getUserRolesDTO.AllowShiftOpenClose;
                        user.AllowPOSAccess = usersRolesBL.getUserRolesDTO.AllowPosAccess;
                    }
                }
                //End Modification- Accept Login ID and Card Number tap. 29-Sep-2015                
            }
            catch (Security.SecurityException se)
            {
                POSUtils.ParafaitMessageBox(se.Message);
                if (System.Runtime.InteropServices.Marshal.GetHRForException(se) == Security.SecurityException.ExChangePassword)
                {
                    formLogin.TopMost = false;
                    frmChangePassword fcp = new frmChangePassword(POSStatic.Utilities, CardNumber == null ? txtLogin.Text : null);
                    fcp.ShowDialog();
                }
                return false;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// LoginFingerPrint Create the Security.User based on the UsersDTO parameter.
        /// </summary>
        /// <param name="usersDTO"></param>
        /// <returns></returns>
        public static Security.User LoginFingerPrint(UsersDTO usersDTO)
        {
            log.LogMethodEntry(usersDTO);
            log.Debug("UserDTO Value In Login : " + usersDTO);
            Security.User user = new Security.User();
            UserRolesDTO userRolesDTO = GetUserRoleDTO(usersDTO.RoleId);

            if (usersDTO != null)
            {
                user.UserId = usersDTO.UserId;
                user.RoleId = usersDTO.RoleId;
                user.SiteId = usersDTO.SiteId;
                user.LoginId = usersDTO.LoginId;
                user.UserName = usersDTO.UserName;
                user.EmpNumber = usersDTO.EmpNumber;
                user.LastName = usersDTO.EmpLastName;
                user.CardNumber = string.Empty;
                user.PasswordSalt = usersDTO.PasswordSalt;
                user.GUID = usersDTO.Guid;
                user.RoleName = userRolesDTO.Role;
                user.AllowPOSAccess = userRolesDTO.AllowPosAccess;
                user.ManagerFlag = userRolesDTO.ManagerFlag.ToString().Equals("Y");
                user.EnablePOSClockIn = userRolesDTO.EnablePOSClockIn;
                user.AllowShiftOpenClose = userRolesDTO.AllowShiftOpenClose;
                loginUser(user);
                log.LogMethodExit();
            }
            log.LogMethodEntry();
            log.Debug("User Value In Login : " + user);
            return user;
        }

        public static void loginUser(Security.User user, ParafaitEnv parafaitEnv = null)
        {
            log.LogMethodEntry();
            if (parafaitEnv == null)
                parafaitEnv = POSStatic.ParafaitEnv;

            if (user.UserId.Equals(POSStatic.ParafaitEnv.User_Id))
                LoginChanged = false;
            else
                LoginChanged = true;

            parafaitEnv.Username = user.UserName;
            parafaitEnv.User_Id = user.UserId;
            parafaitEnv.LoginID = user.LoginId;
            parafaitEnv.Role = user.RoleName;
            parafaitEnv.RoleId = user.RoleId;
            parafaitEnv.UserCardNumber = user.CardNumber;
            parafaitEnv.Manager_Flag = user.ManagerFlag ? "Y" : "N";
            if (txtPassword != null)
                parafaitEnv.Password = txtPassword.Text;
            parafaitEnv.EnablePOSClockIn = user.EnablePOSClockIn;
            parafaitEnv.AllowShiftOpenClose = user.AllowShiftOpenClose;
            parafaitEnv.AllowPOSAccess = user.AllowPOSAccess;

            //Modified to consider attendance Role Id during user Login and on manager approval 
            int attendanceRoleId = GetAttendanceRoleId(user.UserId);
            if (attendanceRoleId != -1)
            {
                UserRoles usersRolesBL = new UserRoles(POSStatic.Utilities.ExecutionContext, attendanceRoleId);
                if (usersRolesBL.getUserRolesDTO != null)
                {
                    user.RoleId = attendanceRoleId;
                    user.RoleName = usersRolesBL.getUserRolesDTO.Role;
                    user.ManagerFlag = usersRolesBL.getUserRolesDTO.ManagerFlag == "Y" ? true : false;
                    user.EnablePOSClockIn = usersRolesBL.getUserRolesDTO.EnablePOSClockIn;
                    user.AllowShiftOpenClose = usersRolesBL.getUserRolesDTO.AllowShiftOpenClose;
                    user.AllowPOSAccess = usersRolesBL.getUserRolesDTO.AllowPosAccess;

                    parafaitEnv.RoleId = attendanceRoleId;
                    parafaitEnv.Role = usersRolesBL.getUserRolesDTO.Role;
                    parafaitEnv.Manager_Flag = usersRolesBL.getUserRolesDTO.ManagerFlag;
                    parafaitEnv.EnablePOSClockIn = usersRolesBL.getUserRolesDTO.EnablePOSClockIn;
                    parafaitEnv.AllowShiftOpenClose = usersRolesBL.getUserRolesDTO.AllowShiftOpenClose;
                    parafaitEnv.AllowPOSAccess = usersRolesBL.getUserRolesDTO.AllowPosAccess;
                }
            }

            try
            {
                Users userBL = new Users(parafaitEnv.ExecutionContext, user.UserId, true, true);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                int tokenLifeTime = ParafaitDefaultContainerList.GetParafaitDefault(parafaitEnv.ExecutionContext, "JWT_TOKEN_LIFE_TIME", 0);
                securityTokenBL.GenerateNewJWTToken(user.LoginId, userBL.UserDTO.Guid, parafaitEnv.IsCorporate ? parafaitEnv.SiteId.ToString() : "-1", parafaitEnv.ExecutionContext.LanguageId.ToString(), user.RoleId.ToString(), "User", parafaitEnv.POSMachineId.ToString(), Guid.NewGuid().ToString(), tokenLifeTime);
                var securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                parafaitEnv.ExecutionContext.WebApiToken = securityTokenDTO.Token;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while generating the web-api token", ex);
            }

            log.LogMethodExit();
        }


        private static int GetAttendanceRoleId(int userId)
        {
            log.LogMethodEntry(userId);
            int attendanceRoleId = -1;
            if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "ENABLE_POS_ATTENDANCE").Equals("Y")
                && POSStatic.ParafaitEnv.EnablePOSClockIn == true)
            {
                Users users = new Users(POSStatic.Utilities.ExecutionContext, userId);
                AttendanceDTO attendanceDTO = users.GetAttendanceForDay();
                if (attendanceDTO != null)
                {
                    List<AttendanceLogDTO> attendanceLogDTOList = attendanceDTO.AttendanceLogDTOList.OrderByDescending(x => x.Timestamp).ToList();
                    if (attendanceLogDTOList[0].Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.ON_BREAK)
                        && attendanceLogDTOList[0].Status != AttendanceLogDTO.AttendanceLogStatusToString(AttendanceLogDTO.AttendanceLogStatus.CLOCK_OUT))
                    {
                        attendanceRoleId = attendanceLogDTOList[0].AttendanceRoleId;
                    }
                }
            }
            log.LogMethodExit(attendanceRoleId);
            return attendanceRoleId;
        }

        /**
        /// Modified on:30-Dec-2016
        /// Reason : For manger approval in based on hierarchy 
        **/
        static public bool Manager(ref int ManagerId, ref Security.User User)
        {
            log.LogMethodEntry();
            string msg = string.Empty;
            List<int> managerRoleIds = GetAssignedManagerRoleId(POSStatic.ParafaitEnv.RoleId, ref msg);

            if (msg != string.Empty)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1125));
                return false;
            }

            //If Login user has manager flag and none of the manager role id are assigned
            if ((managerRoleIds.Count == 0 && IsRoleExist(POSStatic.ParafaitEnv.RoleId)) || (POSStatic.ParafaitEnv.Manager_Flag == "Y" && managerRoleIds.Count == 0))
            {
                User = new Security.User();
                ManagerId = User.UserId = POSStatic.ParafaitEnv.User_Id;
                User.LoginId = POSStatic.ParafaitEnv.LoginID;
                return true;
            }

            ShowScreenSaver = false;
            bool success = Login(ref User, true, true);

            if (success)
            {
                if ((managerRoleIds.Count > 0 && managerRoleIds.Contains(User.RoleId)) ||
                    (managerRoleIds.Count == 0 && IsRoleExist(User.RoleId)) ||
                     (managerRoleIds.Count == 0 && User.ManagerFlag))
                {
                    ManagerId = User.UserId;
                    return true;
                }
            }

            ManagerId = -1;
            log.LogMethodExit(false);
            return false;
        }
        /**
        /// Modified on:30-Dec-2016
        /// Reason : For Getting assigned manager role id
        **/
        static public List<int> GetAssignedManagerRoleId(int userRoleId, ref string message)
        {
            log.LogMethodEntry(userRoleId);
            List<int> roleIds = new List<int>();
            DataTable assignedManagerRoleId;
            try
            {
                assignedManagerRoleId = POSStatic.Utilities.executeDataTable(@"with n(role_id, role, AssignedManagerRoleId, Level) as 
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
        //end 

        static bool ShowScreenSaver = true;
        /**
       /// Modified on:30-Dec-2016
       /// Reason : For manger approval in based on hierarchy 
       **/
        static public bool Manager(ref int ManagerId)
        {
            log.LogMethodEntry();
            string msg = string.Empty;
            List<int> managerRoleIds = GetAssignedManagerRoleId(POSStatic.ParafaitEnv.RoleId, ref msg);

            if (msg != string.Empty)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(1125));
                ManagerId = -1;
                return false;
            }

            //If Login user has manager flag and none of the manager role id are assigned
            if ((managerRoleIds.Count == 0 && IsRoleExist(POSStatic.ParafaitEnv.RoleId)) || (POSStatic.ParafaitEnv.Manager_Flag == "Y" && managerRoleIds.Count == 0))
            {
                ManagerId = POSStatic.ParafaitEnv.User_Id;
                return true;
            }

            Security.User User = null;
            ShowScreenSaver = false;
            bool success = Login(ref User, true, true);

            if (success)
            {
                if ((managerRoleIds.Count > 0 && managerRoleIds.Contains(User.RoleId)) ||
                    (managerRoleIds.Count == 0 && IsRoleExist(User.RoleId)) ||
                     (managerRoleIds.Count == 0 && User.ManagerFlag))
                {
                    ManagerId = User.UserId;
                    log.LogMethodExit(true);
                    return true;
                }
            }

            ManagerId = -1;
            log.LogMethodExit(false);
            return false;
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
                DataTable dt = POSStatic.Utilities.executeDataTable(@"SELECT distinct AssignedManagerRoleId FROM user_roles WHERE AssignedManagerRoleId is not null");

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
            catch { }
            log.LogMethodExit(false);
            return false;
        }

        static public bool User()
        {
            log.LogMethodEntry();
            Security.User user = null;
            ShowScreenSaver = true;
            bool success = Login(ref user);
            if (success)
            {
                loginUser(user);
                POSStatic.LoggedInUser = user;
            }
            log.LogMethodExit(success);
            return success;
        }
        static public bool BasicCheck(ref Security.User user, bool showScreenSaver = true)
        {
            log.LogMethodEntry(showScreenSaver);
            ShowScreenSaver = showScreenSaver;
            log.LogMethodExit();
            return Login(ref user, false);
        }

        public static UserRolesDTO GetUserRoleDTO(int userRoleId)
        {
            log.LogMethodEntry(userRoleId);
            UserRoles userRole = new UserRoles(POSStatic.Utilities.ExecutionContext, userRoleId);
            log.LogMethodExit(userRole.getUserRolesDTO);
            return userRole.getUserRolesDTO;
        }

        static internal void ClearKeyBoardProcess()
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
    }
}
