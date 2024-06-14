/********************************************************************************************
 * Project Name - POS Login
 * Description  - UI for FingerPrintLogin
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019     Girish Kundar   Modified : Added Logger methods and Removed unused namespace's 
 ********************************************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using GrFingerXLib;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    public partial class FingerPrintLogin : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        char mode = 'L'; // default login mode
        private Util _util;
        public Util Util
        {
            get { return _util; }
            set { _util = value; }
        }

        private AxGrFingerXLib.AxGrFingerXCtrl _fingerLib;

        public FingerPrintLogin(char pMode)
        {
            log.LogMethodEntry(pMode);
            try
            {
                mode = pMode;
                POSStatic.Utilities.setLanguage();
                InitializeComponent();

                this.DialogResult = DialogResult.Cancel;

                pbPrint.Image = Properties.Resources.DefaultImage;

                //setup the util class and the fingerprint library
                _util = new Util(this.pbPrint);
                _fingerLib = new AxGrFingerXLib.AxGrFingerXCtrl();

                ((System.ComponentModel.ISupportInitialize)_fingerLib).BeginInit();
                _fingerLib.Name = "_fingerLib";
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FingerPrintLogin));
                this._fingerLib.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("_fingerLib.OcxState")));
                this.SuspendLayout();
                this.Controls.Add(_fingerLib);
                ((System.ComponentModel.ISupportInitialize)_fingerLib).EndInit();
                this.ResumeLayout(false);
                this.PerformLayout();

                _util.InitializeGrFinger(_fingerLib);
                _fingerLib.ImageAcquired += new AxGrFingerXLib._IGrFingerXCtrlEvents_ImageAcquiredEventHandler(_fingerLib_ImageAcquired);
                _fingerLib.SensorPlug += new AxGrFingerXLib._IGrFingerXCtrlEvents_SensorPlugEventHandler(_fingerLib_SensorPlug);
                _fingerLib.SensorUnplug += new AxGrFingerXLib._IGrFingerXCtrlEvents_SensorUnplugEventHandler(_fingerLib_SensorUnplug);

                POSStatic.Utilities.setLanguage(this);
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        void _fingerLib_SensorUnplug(object sender, AxGrFingerXLib._IGrFingerXCtrlEvents_SensorUnplugEvent e)
        {
            log.LogMethodEntry();
            _fingerLib.CapStopCapture(e.idSensor);
            log.LogMethodExit();
        }

        void _fingerLib_SensorPlug(object sender, AxGrFingerXLib._IGrFingerXCtrlEvents_SensorPlugEvent e)
        {
            log.LogMethodEntry();
            _fingerLib.CapStartCapture(e.idSensor);
            log.LogMethodExit();
        }

        void _fingerLib_ImageAcquired(object sender, AxGrFingerXLib._IGrFingerXCtrlEvents_ImageAcquiredEvent e)
        {
            log.LogMethodEntry();
            log.Debug("Copying acquired image");
            //Copying acquired image
            _util._raw.height = e.height;
            _util._raw.width = e.width;
            _util._raw.Res = e.res;
            _util._raw.img = e.rawImage;

            //display fingerprint image
            log.Debug("display fingerprint image");
            _util.PrintBiometricDisplay(false, GRConstants.GR_DEFAULT_CONTEXT);

            // extract template
            log.Debug("extract template");
            int ret = _util.ExtractTemplate();

            // write template to the screen
            log.Debug("write template to the screen");
            if (ret >= 0)
            {
                // if no error, display minutiae/segments/directions into image
                // _util.PrintBiometricDisplay(true, GRConstants.GR_NO_CONTEXT);
            }
            else
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(29));
                return;
            }

            int score = 0;
            int userid = _util.Identify(ref score);

            if (!btnRegisterFingerPrint.Visible && mode != 'A') // user is trying to register finger print
            {
                if (userid > 0)
                {
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(23));
                    _util._raw.img = null;
                }
                return;
            }

            // login mode
            if (userid <= 0)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(18), "Invalid", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (mode == 'A') // attendance
                {
                    log.Debug("Attendance Mode");
                    string username = "";
                    AttendanceUtils userBL = new AttendanceUtils(POSStatic.Utilities);
                    if (userBL.RecordAttendance(userid, ref username))
                        POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(19, username), "Staff Attendance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                SqlCommand cmd = POSStatic.Utilities.getCommand();

                cmd.CommandText = "select u.user_id, loginid, username, card_number, " +
                                    "r.role, isnull(r.manager_flag, 'N') manager_flag " +
                                    "from users u, user_roles r " +
                                    "where u.role_id = r.role_id " +
                                    "and u.user_id = @user_id";
                cmd.Parameters.AddWithValue("@user_id", userid);

                DataTable DT = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(DT);

                if (DT.Rows.Count == 0)
                {
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(25));
                    return;
                }

                POSStatic.ParafaitEnv.Username = DT.Rows[0]["username"].ToString();
                POSStatic.ParafaitEnv.User_Id = userid;
                POSStatic.ParafaitEnv.LoginID = DT.Rows[0]["loginid"].ToString();
                POSStatic.ParafaitEnv.Role = DT.Rows[0]["role"].ToString();
                POSStatic.ParafaitEnv.UserCardNumber = DT.Rows[0]["card_number"].ToString();
                POSStatic.ParafaitEnv.Manager_Flag = DT.Rows[0]["manager_flag"].ToString().Trim();

                cmd.CommandText = "update users set last_login_time = getdate() where user_id = @user_id";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@user_id", userid);

                cmd.ExecuteNonQuery();

                this.DialogResult = DialogResult.OK;
                this.Close();
                log.LogMethodExit();
            }
        }

        private void btnSaveFingerPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            SqlCommand userCmd = POSStatic.Utilities.getCommand();
            userCmd.CommandText = "select u.user_id, finger_print " +
                                "from users u " +
                                "where u.active_flag = 'Y' " +
                                "and u.loginid = @loginid " +
                                "and u.password = @password ";
            userCmd.Parameters.AddWithValue("@loginid", txtLogingId.Text);
            userCmd.Parameters.AddWithValue("@password", Encryption.Encrypt(txtPassword.Text));

            DataTable DT = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(userCmd);
            da.Fill(DT);

            if (DT.Rows.Count == 0)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(24));
                return;
            }

            if (DT.Rows[0]["finger_print"] != DBNull.Value)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(26));
                return;
            }

            if (_util._raw.img == null)
            {
                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(27));
                return;
            }

            FingerPrint fp = new FingerPrint();

            fp.Image = _util._pbPic.Image;
            fp.User = Convert.ToInt32(DT.Rows[0]["user_id"]);
            fp.Template = (byte[])_util._tpt._tpt;
            fp.Save();

            POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(28));
            changeMode('L');
            log.LogMethodExit();
        }

        private void FingerPrintLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            _util.FinalizeUtil();
            log.LogMethodExit();
        }

        private void btnRegisterFingerPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            changeMode('R');
            log.LogMethodExit();
        }

        private void FingerPrintLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyChar == 27)
            {
                if (mode == 'R') // come out of registration mode
                {
                    log.Debug("come out of registration mode");
                    changeMode('L');
                    e.Handled = true;
                }
                else
                    this.Close();
            }
            log.LogMethodExit();
        }

        void changeMode(char chgMode)
        {
            log.LogMethodEntry(chgMode);
            mode = chgMode;
            if (mode == 'L')
            {                
                btnRegisterFingerPrint.Visible = true;
                lblNotRegistered.Visible = true;
                this.Height -= 90;
                lblSwipeTo.Text = POSStatic.MessageUtils.getMessage(21);
                btnRegisterFingerPrint.Focus();
            }
            else
            {
                lblNotRegistered.Visible = false;
                btnRegisterFingerPrint.Visible = false;
                this.Height += 90;
                lblSwipeTo.Text = POSStatic.MessageUtils.getMessage(27);
                txtLogingId.Focus();
            }
            pbPrint.Image = Properties.Resources.DefaultImage;
            _util._raw.img = null;
            log.LogMethodExit();
        }

        private void FingerPrintLogin_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Show();
            this.Activate();
            log.LogMethodExit();
        }

        private void lnkLoginUsername_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            if (Authenticate.User())
            {
                SqlCommand cmd = POSStatic.Utilities.getCommand();
                cmd.CommandText = "select override_fingerprint from users where loginid = @loginid";
                cmd.Parameters.AddWithValue("@loginid", POSStatic.ParafaitEnv.LoginID);
                if (cmd.ExecuteScalar().ToString() != "Y")
                {
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(20), "Login");
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            log.LogMethodExit();
        }
    }
}