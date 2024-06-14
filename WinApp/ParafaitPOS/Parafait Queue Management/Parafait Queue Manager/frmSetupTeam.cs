/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - frmSetupTeam 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
 * 2.70        1-Jul-2019      Lakshminarayana     Modified to add support for ULC cards 
 * 2.80        11-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.POS;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ParafaitQueueManagement
{
    public partial class frmSetupTeam : Form
    {
        USBListener.CardListener simpleCardListener;
        string cardNumber;
        bool isCaptainSetup = false;
        Player[] playerList;
        Player captain;
        List<Player> playerOfTeam;
        int MAX_PLAYERS_IN_TEAM = 5;
        int leftTrimCard = 0;
        int rightTrimCard = 0;
        private readonly TagNumberParser tagNumberParser;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Utilities parafaitUtility = new Utilities();
        private bool registerUSBDevice()
        {
            log.LogMethodEntry();
            string USBReaderVID = Common.Utilities.getParafaitDefaults("USB_READER_VID");
            string USBReaderPID = Common.Utilities.getParafaitDefaults("USB_READER_PID");
            string USBReaderOptionalString = Common.Utilities.getParafaitDefaults("USB_READER_OPT_STRING");
            

            EventHandler currEventHandler = new EventHandler(CardScanCompleteEventHandle);
            if (simpleCardListener != null)
                simpleCardListener.Dispose();

            if (IntPtr.Size == 8)
                simpleCardListener = new USBListener.CardListener64();
            else
                simpleCardListener = new USBListener.CardListener32();

            bool flag = simpleCardListener.InitializeUSBCardReader(this, currEventHandler, USBReaderVID, USBReaderPID, USBReaderOptionalString);
            if (simpleCardListener.isOpen)
            {
                //displayMessageLine(simpleCardListener.dInfo.deviceName, MESSAGE);
                //MessageBox.Show("Connected USB Card Reader");
                log.LogMethodExit(true);
                return true;
            }
            else
            {
                MessageBox.Show("Unable to find USB card reader");
                log.LogMethodExit(false);
                return false;
            }
        }
        private void CardScanCompleteEventHandle(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (e is CardReaderScannedEventArgs)
            {
                CardReaderScannedEventArgs checkScannedEvent = e as CardReaderScannedEventArgs;
                TagNumber tagNumber;
                if (tagNumberParser.TryParse(checkScannedEvent.Message, out tagNumber) == false)
                {
                    log.LogMethodExit();
                    return;
                }

                cardNumber = tagNumber.Value;

                HandleCard(cardNumber);                
            }
            log.LogMethodExit();
        }
        private Player GetPlayerDetails(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            for (int i = 0; i < playerList.Length; i++)
            {
                if (string.Compare(cardNumber, playerList[i].CardNumber) == 0)
                {
                    log.LogMethodExit(playerList[i]);
                    return playerList[i];
                }
            }
            log.LogMethodExit(null);
            return null;
        }
        private void SetupGrid(DataGridView dgPassed)
        {
            log.LogMethodEntry(dgPassed);
            for (int i = 0; i < dgPassed.Columns.Count; i++)
            {
                dgPassed.Columns[i].Visible = false;
            }
            dgPassed.Columns["Name"].Visible = true;
            dgPassed.Columns["TeamName"].Visible = true;
            dgPassed.Columns["CardNumber"].Visible = true;
            dgPassed.Columns["QueueEntryTime"].Visible = true;
            dgPassed.Columns["GameName"].Visible = true;
            log.LogMethodExit();
            
        }
        private void ShowPlayerDetails(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            teamMemberCardNumber.Text = cardNumber;
            Player teamMember = GetPlayerDetails(cardNumber);

            if (playerOfTeam.Count >= (MAX_PLAYERS_IN_TEAM-1))
            {
                MessageBox.Show("Max players in team is " + MAX_PLAYERS_IN_TEAM + " (including captain). You are exceeding..");
                log.LogMethodExit();
                return;
            }
            if (teamMember == null)
            {
                MessageBox.Show(cardNumber + " is not setup as a player yet. Please verify card..");
                log.LogMethodExit();
                return;
            }
            if (string.Compare(teamMember.GameName, captain.GameName) != 0)
            {
                MessageBox.Show(teamMember.Name + " is not playing the same game as captain. Please verify..");
                log.LogMethodExit();
                return;
            }
            if (teamMember.QueueEntryId == captain.QueueEntryId)
            {
                MessageBox.Show(teamMember.Name + " is also entered as captain. Please verify..");
                log.LogMethodExit();
                return;
            }
            
            bool isPlayerAlreadyPresent = false;
            for (int i = 0; (i < playerOfTeam.Count) && (isPlayerAlreadyPresent == false); i++)
            {
                if (teamMember.QueueEntryId == playerOfTeam[i].QueueEntryId)
                    isPlayerAlreadyPresent = true;
            }
            if (isPlayerAlreadyPresent == true)
            {
                MessageBox.Show(teamMember.Name + " is already present. Please verify..");
                log.LogMethodExit();
                return;
            }
            playerOfTeam.Add(teamMember);
            if (dgTeamMemberDetails.Rows.Count == 0)
                setupDataGridview(ref dgTeamMemberDetails);
            dgTeamMemberDetails.DataSource = "";
            dgTeamMemberDetails.DataSource = playerOfTeam;
            SetupGrid(dgTeamMemberDetails);
            dgTeamMemberDetails.EndEdit();
            dgTeamMemberDetails.Refresh();
            log.LogMethodExit();
        }
        private void ShowCaptainDetails(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            captainCardNumber.Text = cardNumber;
            Player[] captainOfTeam = new Player[1]; 
            captain = GetPlayerDetails(cardNumber);
            captainOfTeam[0] = captain;
            if (captainOfTeam[0] == null)
                MessageBox.Show("Captain is not setup as a player yet. Verify card - " + cardNumber);
            else
            {
                isCaptainSetup = true;
                dgCaptainDetails.DataSource = captainOfTeam;
                SetupGrid(dgCaptainDetails);
                setupDataGridview(ref dgCaptainDetails);
                dgCaptainDetails.Refresh();

            }
            log.LogMethodExit();
        }
        private void HandleCard(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            if (isCaptainSetup == false)
            {
                ShowCaptainDetails(cardNumber);
            }
            else
            {
                ShowPlayerDetails(cardNumber);
            }
            log.LogMethodExit();
        }
        public frmSetupTeam()
        {
            log.LogMethodEntry();
            InitializeComponent();
            playerOfTeam = new List<Player>();
            bool cardReaderExists = registerUSBDevice();
            leftTrimCard = Convert.ToInt32(Common.Utilities.getParafaitDefaults("LEFT_TRIM_CARD_NUMBER"));
            rightTrimCard = Convert.ToInt32(Common.Utilities.getParafaitDefaults("RIGHT_TRIM_CARD_NUMBER"));
            tagNumberParser = new TagNumberParser(Common.Utilities.ExecutionContext);
            log.LogMethodExit();
        }
        public void SetPlayersList(Player[] playerListPassed)
        {
            log.LogMethodEntry(playerListPassed);
            playerList = playerListPassed;
            log.LogMethodExit();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }
        public void setupDataGridview(ref DataGridView dgv)
        {
            log.LogMethodEntry(dgv);
            dgv.BackgroundColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.GridColor = Color.LightSteelBlue;
            dgv.AlternatingRowsDefaultCellStyle = gridViewAlternateRowStyle();
            dgv.RowHeadersDefaultCellStyle = gridViewRowHeaderStyle();
            dgv.RowsDefaultCellStyle.SelectionBackColor = Color.LightSteelBlue;
            dgv.ColumnHeadersDefaultCellStyle = gridViewColumnHeaderStyle();
            dgv.RowHeadersVisible =
            dgv.AllowUserToResizeRows = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeColumns = true;
            dgv.ScrollBars = ScrollBars.Both;
            log.LogMethodExit();
        }
        public static DataGridViewCellStyle gridViewAlternateRowStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.Azure;
            log.LogMethodExit(style);
            return style;
        }
        public static DataGridViewCellStyle gridViewRowHeaderStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.White;
            log.LogMethodExit(style);
            return style;
        }
        public static DataGridViewCellStyle gridViewColumnHeaderStyle()
        {
            log.LogMethodEntry();
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = System.Drawing.Color.Azure;
            log.LogMethodExit(style);
            return style;
        }

        private void bntGetTeamDetails_Click(object sender, EventArgs e)
        {

        }
        private int ValidateTeamName(string teamName)
        {
            log.LogMethodEntry(teamName);
            try
            {
                SqlCommand cmdCheckGroupname = Common.Utilities.getCommand();
                cmdCheckGroupname.CommandText = @"select * 
                                                    from customerqueue 
                                                   where customer_group_name = @teamName
                                                     and play_request > dateadd(mi,@minutesAddParam,getdate())";
                cmdCheckGroupname.Parameters.AddWithValue("@teamName", teamName);
                cmdCheckGroupname.Parameters.AddWithValue("@minutesAddParam", -180);
                SqlDataAdapter daTeamNameDetails = new SqlDataAdapter(cmdCheckGroupname);
                DataTable dtTeamNameDetails = new DataTable();
                daTeamNameDetails.Fill(dtTeamNameDetails);
                if (dtTeamNameDetails.Rows.Count > 0)
                {
                    log.LogMethodExit(-1);
                    return -1;
                }
                else
                {
                    log.LogMethodExit(0);
                    return 0;
                }
            }
            catch (Exception Ex)
            {
                log.Error(Ex.Message);
                throw Ex;
            }
            

        }
        private void btnValidate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string teamName = txtTeamName.Text;
            int teamNameValidation = ValidateTeamName(teamName);
            if (teamNameValidation == -1)
            {
                MessageBox.Show("Team name already in use, please choose another");
                txtTeamName.Focus();
            }
            else
                MessageBox.Show("Team name validated, the team is fine");
            log.LogMethodExit();
        }

        private void frmSetupTeam_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            setupDataGridview(ref dgCaptainDetails);
            setupDataGridview(ref dgTeamMemberDetails);
            log.LogMethodExit();
        }

        private void btnGetTeamMemberDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string cardNumberEntered = teamMemberCardNumber.Text;
            if ((string.Compare(cardNumberEntered, "") == 0) || (cardNumberEntered.Length != Convert.ToInt32(Common.Utilities.getParafaitDefaults("CARD_NUMBER_LENGTH"))))
            {
                MessageBox.Show("Card number is not entered properly, please verify", "Card Number Validation");
                return;
            }
            ShowPlayerDetails(cardNumberEntered);
            log.LogMethodExit();
        }

        private void lblTeamMember_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void dgTeamMemberDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        private void ClearPlayerGrids()
        {
            log.LogMethodEntry();
            dgCaptainDetails.DataSource = "";
            dgTeamMemberDetails.DataSource = "";
            isCaptainSetup = false;
            captain = null;
            playerOfTeam.Clear();
            dgCaptainDetails.Refresh();
            dgTeamMemberDetails.Refresh();
            log.LogMethodExit();

        }
        private void btnDeleteCaptain_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgCaptainDetails.RowCount == 0)
            {
                MessageBox.Show("Captain not entered, cannot delete", "Captain Delete");
            }
            else
            {
                if (MessageBox.Show("This will clear captain and all players. Are you ok?", "Confirm captain delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ClearPlayerGrids();
                }
            }
            log.LogMethodExit();
        }

        private void btnDeleteMember_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgTeamMemberDetails.RowCount == 0)
            {
                MessageBox.Show("Players not entered, cannot delete. Please check..", "Player Delete");
            }
            else
            {
                if (MessageBox.Show("This will clear all players. Are you ok?", "Confirm team delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dgTeamMemberDetails.DataSource = "";
                    playerOfTeam.Clear();
                    dgTeamMemberDetails.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private void btnDeleteSingleMember_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgTeamMemberDetails.RowCount == 0)
            {
                MessageBox.Show("Players not entered, cannot delete. Please check..", "Player Delete");
            }
            else
            {
                if (MessageBox.Show("Selected players will be deleted. Are you ok?", "Confirm team member delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DataGridViewSelectedRowCollection dgvSelectedPlayers = dgTeamMemberDetails.SelectedRows;
                    for (int i = 0; i < dgvSelectedPlayers.Count; i++)
                    {
                        DataGridViewRow playerRow = (DataGridViewRow)dgvSelectedPlayers[i];
                        string queueIdOfSelectedPlayer = playerRow.Cells["QueueEntryId"].Value.ToString();
                        for (int j = 0; j < playerOfTeam.Count; j++)
                        {

                            if (playerOfTeam[j].QueueEntryId == Convert.ToInt32(queueIdOfSelectedPlayer))
                            {
                                playerOfTeam.Remove(playerOfTeam[j]);
                            }
                        }
                    }
                    dgTeamMemberDetails.DataSource = "";
                    dgTeamMemberDetails.DataSource = playerOfTeam;
                    for (int i = 0; i < dgTeamMemberDetails.Columns.Count; i++)
                    {
                        dgTeamMemberDetails.Columns[i].Visible = false;
                    }
                    dgTeamMemberDetails.Columns["Name"].Visible = true;
                    dgTeamMemberDetails.Columns["TeamName"].Visible = true;
                    dgTeamMemberDetails.Columns["CardNumber"].Visible = true;
                    dgTeamMemberDetails.Columns["QueueEntryTime"].Visible = true;
                    dgTeamMemberDetails.Columns["GameName"].Visible = true;
                    dgTeamMemberDetails.EndEdit();
                    dgTeamMemberDetails.Refresh();
                }
            }
            log.LogMethodExit();
        }

        private void bntGetCaptainDetails_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (isCaptainSetup == true)
            {
                MessageBox.Show("Captain is already setup. Please clear captain first.", "Captain Get Details");
                log.Debug("Captain is already setup. Please clear captain first.");
                log.LogMethodExit();
                return;
            }
            string cardNumberEntered = captainCardNumber.Text;
            if ((string.Compare(cardNumberEntered, "") == 0) || (cardNumberEntered.Length != Convert.ToInt32(Common.Utilities.getParafaitDefaults("CARD_NUMBER_LENGTH"))))
            {
                MessageBox.Show("Card number is not entered properly, please verify", "Card Number Validation");
                log.Debug("Card number is not entered properly, please verify");
                log.LogMethodExit();
                return;
            }
            ShowCaptainDetails(cardNumberEntered);
            log.LogMethodExit();
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ClearPlayerGrids();
            captainCardNumber.Text = "";
            teamMemberCardNumber.Text = "";
            txtTeamName.Text = "";
            log.LogMethodExit();
        }

        private void btnSaveTeamName_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string teamName = txtTeamName.Text;
            
            if (teamName.Trim().CompareTo("") == 0)
            {
                MessageBox.Show("Team name not entered, please enter", "Team Validation");
                txtTeamName.Focus();
                log.Debug("Team name not entered, please enter");
                log.LogMethodExit();
                return;

            }
            int teamNameValidation = ValidateTeamName(teamName);
            if (teamNameValidation == -1)
            {
                MessageBox.Show("Team name already in use, please choose another");
                txtTeamName.Focus();
                log.Debug("Team name already in use, please choose another");
                log.LogMethodExit();
                return;
            }
            if (isCaptainSetup == false)
            {
                MessageBox.Show("Captain not setup, no team to save", "Captain Validation");
                log.Debug("Captain not setup, no team to save");
                log.LogMethodExit();
                return;
            }
            captain.UpdateTeamName(teamName);
            for (int i = 0; i < playerOfTeam.Count; i++)
                playerOfTeam[i].UpdateTeamName(teamName);
            Player[] captainOfTeam = new Player[1];
            captainOfTeam[0] = captain;
            dgCaptainDetails.DataSource = "";
            dgCaptainDetails.DataSource = captainOfTeam;
            SetupGrid(dgCaptainDetails);
            dgTeamMemberDetails.DataSource = "";
            dgTeamMemberDetails.DataSource = playerOfTeam;
            SetupGrid(dgTeamMemberDetails);
            log.LogMethodExit();
        }
    }
}