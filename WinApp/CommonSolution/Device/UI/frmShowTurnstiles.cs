/********************************************************************************************
 * Project Name - Device.Turnstile UI
 * Description  - Class for  of frmShowTurnstiles      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Device.Turnstile
{
    public partial class frmShowTurnstiles : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TurnstileClass CurTurnstileDevice;
        private TurnstileUI.TurnstileObjectClass selectedTurnstile;
        private List<TurnstileUI.TurnstileObjectClass> lstTurnstiles;
        private List<TurnstileUI.TurnstileObjectClass> SelectedTurnstiles = new List<TurnstileUI.TurnstileObjectClass>();

        public frmShowTurnstiles(List<TurnstileUI.TurnstileObjectClass> Turnstiles)
        {
            log.LogMethodEntry(Turnstiles);
            InitializeComponent();
            setUp();
            lstTurnstiles = Turnstiles;
            this.Disposed += frmShowTurnstiles_Disposed;

            List<string> gameprofiles = new List<string>();
            gameprofiles.Add(" - All -");
            foreach (TurnstileUI.TurnstileObjectClass item in lstTurnstiles)
            {
                if (!string.IsNullOrEmpty(item.GameProfileName))
                    gameprofiles.Add(item.GameProfileName);

                if (item.Device != null)
                {
                    item.Device.TurnstileStatusEvent += StateEvents; //Events taking place in process communication with the turnstile
                    item.Device.TurnstileDataEvent += TurnstileData; //Recived data from the turnstile
                }
            }
            IEnumerable<string> distinctProfiles = gameprofiles.Distinct();
            cmbProfile.DataSource = distinctProfiles.ToArray();
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            this.StartPosition = FormStartPosition.CenterScreen;
            log.LogMethodExit();
        }

        void setUp()
        {
            log.LogMethodEntry();
            dgvTurnstiles.Columns.Add("ID", "ID");
            dgvTurnstiles.Columns.Add("Name", "Name");
            dgvTurnstiles.Columns.Add("IPAddress", "IP Address");
            dgvTurnstiles.Columns.Add("Type", "Type");
            dgvTurnstiles.Columns.Add("Make", "Make");
            dgvTurnstiles.Columns.Add("Model", "Model");
            dgvTurnstiles.Columns.Add("Status", "Status");

            dgvTurnstiles.AllowUserToAddRows = false;
            dgvTurnstiles.ReadOnly = true;

            dgvTurnstiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvTurnstiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTurnstiles.SelectionChanged += dgvTurnstiles_SelectionChanged;
            dgvTurnstiles.MultiSelect = false;
            dgvTurnstiles.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            log.LogMethodExit();
        }

        void dgvTurnstiles_SelectionChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            SelectedTurnstiles.Clear();
            foreach (DataGridViewRow dr in dgvTurnstiles.SelectedRows)
            {
                if (dr.Tag != null)
                    SelectedTurnstiles.Add(dr.Tag as TurnstileUI.TurnstileObjectClass);
            }

            displayTurnstileDetails();
            log.LogMethodExit();
        }

        void displayTurnstileDetails()
        {
            log.LogMethodEntry();
            txtTurnIp.Clear();
            txtTurnPort.Clear();
            txtTypeMake.Clear();

            selectedTurnstile = null;
            if (CurTurnstileDevice != null)
                CurTurnstileDevice.Disconnect();

            CurTurnstileDevice = null;

            if (SelectedTurnstiles.Count == 1)
            {
                resetButtons();

                selectedTurnstile = SelectedTurnstiles[0];
                TurnstileDTO turnDetail = selectedTurnstile.DTO;
                TurnstileClass device = selectedTurnstile.Device;
                txtName.Text = turnDetail.TurnstileName;
                txtTypeMake.Text = selectedTurnstile.Type + "/" + selectedTurnstile.Make + "/" + selectedTurnstile.Model;
                lblPassageAAlias.Text = turnDetail.PassageAAlias;
                lblPassageBAlias.Text = turnDetail.PassageBAlias;
                chkActive.Checked = turnDetail.Active;

                if (device != null && turnDetail.Active)
                {
                    txtTurnIp.Text = turnDetail.IPAddress;
                    txtTurnPort.Text = turnDetail.PortNumber.ToString();
                    CurTurnstileDevice = device;
                    if (device.Connect())
                    {
                        showStatus(turnDetail.TurnstileName, "Connected");
                        try
                        {
                            device.GetStatus();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occurred at displayTurnstileDetails() method", ex);
                            log.LogMethodExit(null, " Exception : " + ex.Message);
                            showStatus(turnDetail.TurnstileName, ex.Message);
                        }
                    }
                    else
                    {
                        showStatus(turnDetail.TurnstileName, "Unable to connect");
                    }
                }
            }
            else if (dgvTurnstiles.SelectedRows.Count > 1)
            {
                txtName.Text = "Multi-select";
            }
            else
            {
                txtName.Clear();
            }
            log.LogMethodExit();
        }

        private void cmbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvTurnstiles.Rows.Clear();

            string gameProfile = cmbProfile.SelectedItem.ToString();

            foreach (TurnstileUI.TurnstileObjectClass item in lstTurnstiles)
            {
                if (gameProfile == " - All -" || gameProfile.Equals(item.GameProfileName))
                {
                    TurnstileDTO t = item.DTO;
                    dgvTurnstiles.Rows.Add(t.TurnstileId, t.TurnstileName, t.IPAddress, item.Type, item.Make, item.Model);
                    dgvTurnstiles.Rows[dgvTurnstiles.Rows.Count - 1].Tag = item;
                }
            }

            if (dgvTurnstiles.Rows.Count > 0)
            {
                dgvTurnstiles.Rows[0].Selected = false;
                dgvTurnstiles.Rows[0].Selected = true;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This method display message in TextBox
        /// </summary>
        /// <param name="s">text message</param>
        private void showStatus(string s1, string s2)
        {
            log.LogMethodEntry(s1, s2);
            try
            {
                if (textBoxEvents.InvokeRequired)
                {
                    textBoxEvents.BeginInvoke(new Action<string>((ss) => textBoxEvents.AppendText(ss)), s1 + ": " + s2 + "\n");
                    if (dgvTurnstiles.SelectedRows.Count > 0)
                    {
                        dgvTurnstiles.SelectedRows[0].Cells["Status"].Value = s2;
                    }
                }
                else
                {
                    textBoxEvents.AppendText(s1 + ": " + s2 + "\n");
                    if (dgvTurnstiles.SelectedRows.Count > 0)
                    {
                        dgvTurnstiles.SelectedRows[0].Cells["Status"].Value = s2;
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Error occurred at showStatus() method", ex);
                log.LogMethodExit(null, " Exception : " + ex.Message);
            };
        }

        /// <summary>
        /// Receive events taking place in process communication with the turnstile
        /// </summary>
        /// <param name="state"></param>
        private void StateEvents(TurnstileClass.turnstileStatus state)
        {
            log.LogMethodEntry(state);
            showStatus(selectedTurnstile.DTO.TurnstileName, state.Status.ToString());
            if (state.Error)
                showStatus(selectedTurnstile.DTO.TurnstileName, ": Error: " + state.ErrorMessage);
            log.LogMethodExit();
        }

        void resetButtons()
        {
            log.LogMethodEntry();
            System.Drawing.Bitmap inActiveImage = Semnox.Parafait.Device.Properties.Resources.YellowgreenMenuButton;
            button_singleA.BackgroundImage =
            button_singleB.BackgroundImage =
            button_lockA.BackgroundImage =
            button_lockB.BackgroundImage =
            button_FreeA.BackgroundImage =
            button_FreeB.BackgroundImage =
            buttonPanic.BackgroundImage = inActiveImage;
            log.LogMethodExit();
        }

        /// <summary>
        /// Receive data from the turnstile
        /// </summary>
        /// <param name="cmd"></param>
        private void TurnstileData(TurnstileClass.turnstileData data)
        {
            log.LogMethodEntry(data);
            TurnstileClass device = CurTurnstileDevice;

            System.Drawing.Bitmap activeImage = Semnox.Parafait.Device.Properties.Resources.darkGreenMenuButton;
            System.Drawing.Bitmap inActiveImage = Semnox.Parafait.Device.Properties.Resources.YellowgreenMenuButton;

            if (device.TurnstileData.SingleA)
            {
                button_singleA.BackgroundImage = activeImage;
            }
            else
            {
                button_singleA.BackgroundImage = inActiveImage;
            }

            if (device.TurnstileData.SingleB)
            {
                button_singleB.BackgroundImage = activeImage;
            }
            else
            {
                button_singleB.BackgroundImage = inActiveImage;
            }

            if (device.TurnstileData.LockA)
            {
                button_lockA.BackgroundImage = activeImage;
            }
            else
            {
                button_lockA.BackgroundImage = inActiveImage;
            }

            if (device.TurnstileData.LockB)
            {
                button_lockB.BackgroundImage = activeImage;
            }
            else
            {
                button_lockB.BackgroundImage = inActiveImage;
            }

            if (device.TurnstileData.FreeA)
            {
                button_FreeA.BackgroundImage = activeImage;
            }
            else
            {
                button_FreeA.BackgroundImage = inActiveImage;
            }

            if (device.TurnstileData.FreeB)
            {
                button_FreeB.BackgroundImage = activeImage;
            }
            else
            {
                button_FreeB.BackgroundImage = inActiveImage;
            }

            if (device.TurnstileData.Panic)
            {
                buttonPanic.BackgroundImage = activeImage;
            }
            else
            {
                buttonPanic.BackgroundImage = inActiveImage;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activate and deactivate "Panic" state
        /// </summary>
        private void buttonPanic_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (CurTurnstileDevice != null)
            {
                if (CurTurnstileDevice.TurnstileData.Panic)
                    CurTurnstileDevice.PanicOff();
                else
                    CurTurnstileDevice.Panic();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activates the passage A at the time set in the turnstile (5 seconds)
        /// </summary>
        private void button_singleA_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (CurTurnstileDevice != null)
            {
                CurTurnstileDevice.SingleA();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activates the passage A at the time set in the turnstile (5 seconds)
        /// </summary>
        private void button_singleB_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            if (CurTurnstileDevice != null)
            {
                CurTurnstileDevice.SingleB();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activates the passage A on the retention time in the active state 
        /// </summary>
        private void button_FreeA_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (CurTurnstileDevice != null)
            {
                if (CurTurnstileDevice.TurnstileData.FreeA)
                    CurTurnstileDevice.CancelFreeA();
                else
                    CurTurnstileDevice.FreeA();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Command activates the passage B on the retention time in the active state 
        /// </summary>
        private void button_FreeB_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender ,e);
            if (CurTurnstileDevice != null)
            {
                if (CurTurnstileDevice.TurnstileData.FreeB)
                    CurTurnstileDevice.CancelFreeB();
                else
                    CurTurnstileDevice.FreeB();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// 
        /// </summary>        
        private void button_lockA_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender ,e);
            if (CurTurnstileDevice != null)
            {
                if (CurTurnstileDevice.TurnstileData.LockA)
                    CurTurnstileDevice.UnlockA();
                else
                    CurTurnstileDevice.LockA();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>        
        private void button_lockB_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (CurTurnstileDevice != null)
            {
                if (CurTurnstileDevice.TurnstileData.LockB)
                    CurTurnstileDevice.UnlockB();
                else
                    CurTurnstileDevice.LockB();
            }
            log.LogMethodExit();
        }

        private void lnkClearLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            textBoxEvents.Clear();
            log.LogMethodExit();
        }

        void frmShowTurnstiles_Disposed(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e);
            foreach (TurnstileUI.TurnstileObjectClass turnstile in lstTurnstiles)
            {
                if (turnstile.Device != null)
                    turnstile.Device.Dispose();
            }
            log.LogMethodExit();
        }

        private void frmShowTurnstiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            TurnstileUI ui = new TurnstileUI();
            ui.Close();
            log.LogMethodExit();
        }
    }
}
