/********************************************************************************************
* Project Name - Parafait Queue Management
* Description  - Queue Management Form
* 
**************
**Version Log
**************
*Version     Date             Modified By    Remarks          
*********************************************************************************************
*2.80        11-Sep-2019      Jinto Thomas         Added logger for methods
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace ParafaitQueueManagement
{
    public partial class FormQueueMgmt : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities util = new Utilities();
        QueueManager queueManager;
        Player[] gameQueueComplete;
        DataGridView[] completeDGVOfQueue;
        bool activePlayerGridReady = false;
        int gamePlayId = -1;
        int gameMachineId = -1;
        //AuditManager auditMgr;
        int printCountOfCustomer = 0;
        string[] activePlayersContextMenu = { "Pause", "Restart", "End" };
        int hitCounter = 0;
        string customerQueueCustomerName = string.Empty;
        string customerUniqueId = string.Empty;
        string customerQueueTeamName = string.Empty;
        int customerQueueId;
        string[] contextMenuItems = { "Advance", "Assign Lane", "Move To Priority", "Remove", "Update Team Name", "View Customer Detail", "Print Token", "View H-E Detail", "Inform Customer", "Undo Token" };
        string[] contextMenuItemsGroupList = { "Remove", "Update Team Name", "Print Token", "Undo Token"};
        Point currentCell;
        int currentColumnIndex = 0;
        int currentRowIndex = 0;
        int hitrowindex = 0;
        CustomUIControls.TaskbarNotifier taskBarNotifier;
        TextBox txtMinutes;
        TextBox txtTeamName;
        TextBox txtLaneNo;
        DataGridView dgv;
        DataGridView dgvCustomerQueue;
        System.Windows.Forms.Timer transactioneventtimer = new System.Windows.Forms.Timer();
        Form frmAdvanced;
        Form frmTeamNameUpdate;
        Form frmAssignLane;
        Image image;
        

        int playStartAlertTime = 0;
        int endOfPlayErrorTime = 0;
        int endOfPlayWarningTime = 0;
        int maxQueueEntries = 0;
        int queueSetupTime = 0;
        int queueBufferTime = 0;
        string printerName = string.Empty;
        string dateTimeFormat = string.Empty;
        string queueDebug = string.Empty;
        bool manualOverrideAllowed = false;
        int refreshFrequency;

        DateTime startTime;

        bool digitalTokenEnabled = false;
        int totalActivePlayers = 0;
        bool contextMenuOpen = false;
        KeyManagement keymgmt;

        public FormQueueMgmt(string mode,string loginid,string password)
        {
            log.LogMethodEntry(mode, loginid);
          //  MessageBox.Show(mode);
            InitializeComponent();
            keymgmt = new KeyManagement(util.DBUtilities,util.ParafaitEnv);
            string message = string.Empty;
           // auditMgr=new AuditManager();
            Common.Utilities = new Utilities();
            Common.ParafaitEnv = Common.Utilities.ParafaitEnv;
          //  taskBarNotifier = new TaskbarNotifier();
          //  taskBarNotifier.SetBackgroundBitmap(Image.FromFile(Environment.CurrentDirectory + "\\Resources\\skin.bmp"), Color.FromArgb(255, 0, 255));
         //   taskBarNotifier.SetCloseBitmap(Image.FromFile(Environment.CurrentDirectory + "\\Resources\\close.bmp"), Color.FromArgb(255, 0, 255), new Point(127, 8));
         //   MessageBox.Show(Environment.CurrentDirectory);
          //  taskBarNotifier.SetBackgroundBitmap(Image.FromFile(""), Color.FromArgb(255, 0, 255));
          //  taskBarNotifier.SetCloseBitmap(Image.FromFile(""), Color.FromArgb(255, 0, 255), new Point(127, 8));
            // // initializeTaskBarNotifier();
            //taskBarNotifier.TitleRectangle = new Rectangle(40, 9, 70, 25);
            //taskBarNotifier.ContentRectangle = new Rectangle(8, 41, 133, 68);
            //taskBarNotifier.TitleClick += new EventHandler(TitleClick);
            //taskBarNotifier.ContentClick += new EventHandler(ContentClick);
            //taskBarNotifier.CloseClick += new EventHandler(CloseClick);
            //this.Controls.Add(taskBarNotifier);
            if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1)
            {
                MessageBox.Show("Another instance of Queue Management is already running");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                log.LogMethodExit();
                return;
            }
            if(!keymgmt.validateLicense(ref message))
            {
                MessageBox.Show(message,"Validate License Key");
                log.LogMethodExit();
                Environment.Exit(0);
            }
            else
            {
                if (message != "")
                    MessageBox.Show(message,"Validate License Key");
            }
            if (mode == "U")
            {
                Common.ParafaitEnv.Initialize();
                //User authentication is implemented in this function
                if (!Authenticate.User())
                {
                    log.LogMethodExit();
                    Environment.Exit(0);
                }
            }
            else
            {
               // MessageBox.Show(loginid);
               // MessageBox.Show(password);
                Authenticate.loginInternal(loginid, password);
                
            }
            if (checkAccess().CompareTo("N")==0)
            {
                MessageBox.Show("You do not have access to the queue management module","Access Denied");
                log.LogMethodExit();
                Environment.Exit(0);
            }
            bool initializeStatus = initializeMasterVariables();
            if (initializeStatus == false)
            {
                log.LogMethodExit();
                Environment.Exit(0);
            }
            

        }

        private string checkAccess()
        {
            log.LogMethodEntry();
            // throw new NotImplementedException();
            string  result = string.Empty;
          object o=  Common.Utilities.executeScalar(@"select access_allowed from managementformaccess where main_menu='Semnox' and form_name='Queue Management'
                                             and functiongroup='Apps Access' and role_id=@roleid and ISNULL(IsActive,1) = 1", new SqlParameter("@roleid", Common.ParafaitEnv.RoleId));
          result = Convert.ToString(o);
            log.LogMethodExit(result);
          return result;
        }

        /// <summary>
        /// Fetches all the constants and stores in the local variables. 
        /// This is done to avoid repeated calls to the DB to get the values.
        /// Button is provided in the form also, if the user chooses to refresh the initialization values dynamically.
        /// </summary>
        bool initializeMasterVariables()
        {
            log.LogMethodEntry();
            try
            {
                playStartAlertTime = Convert.ToInt32(Common.Utilities.getParafaitDefaults("PLAY_START_ALERT_TIME"));
                endOfPlayErrorTime = Convert.ToInt32(Common.Utilities.getParafaitDefaults("END_OF_PLAY_ERROR_TIME"));
                endOfPlayWarningTime = Convert.ToInt32(Common.Utilities.getParafaitDefaults("END_OF_PLAY_WARNING_TIME"));
                maxQueueEntries = Convert.ToInt32(Common.Utilities.getParafaitDefaults("QUEUE_MAX_ENTRIES"));
                queueBufferTime = Convert.ToInt32(Common.Utilities.getParafaitDefaults("QUEUE_BUFFER_TIME")) / 60;
                queueSetupTime = Convert.ToInt32(Common.Utilities.getParafaitDefaults("QUEUE_SETUP_TIME")) / 60;
                startTime = Common.Utilities.getServerTime().Date.AddHours(Convert.ToInt32(Common.Utilities.getParafaitDefaults("MAINTENANCE_START_HOUR")));
                
                printerName = Common.Utilities.getParafaitDefaults("PDF_WRITER_PRINTER");
                dateTimeFormat = Common.Utilities.getParafaitDefaults("DATETIME_FORMAT");
                queueDebug = Common.Utilities.getParafaitDefaults("QUEUE_DEBUG_FLAG");
                queueDebug = "N";
                try
                {
                    refreshFrequency = Convert.ToInt32(Common.Utilities.getParafaitDefaults("QUEUE_VIEW_REFRESH"));
                }
                catch
                {
                    refreshFrequency = 5000;
                }
                if (refreshFrequency < 5000)
                    refreshFrequency = 5000;
                if (Common.Utilities.getParafaitDefaults("ALLOW_MANUAL_OVERRIDE").CompareTo("Y") == 0)
                    manualOverrideAllowed = true;
                else
                    manualOverrideAllowed = false;

                if (Common.Utilities.getParafaitDefaults("ENABLE_DIGITAL_TOKEN").CompareTo("Y") == 0)
                {
                    digitalTokenEnabled = true;
                    totalActivePlayers = Convert.ToInt32(Common.Utilities.getParafaitDefaults("DIGITAL_TOKEN_ENABLE_COUNT"));
                }
                else
                    digitalTokenEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failure to initialize. " + ex.Message);
                log.Error(ex.Message);
                log.LogMethodExit(false);
                return false;
            }

            log.LogMethodExit(true);
            return true;
        }
        void initializeTaskBarNotifier()
        {
            log.LogMethodEntry();
            //taskBarNotifier.SetBackgroundBitmap("skin.bmp",
            //                    Color.FromArgb(255, 0, 255));
            //taskBarNotifier.SetCloseBitmap("close.bmp",
            //        Color.FromArgb(255, 0, 255), new Point(127, 8));
            taskBarNotifier.TitleRectangle = new Rectangle(40, 9, 70, 25);
            taskBarNotifier.ContentRectangle = new Rectangle(8, 41, 133, 68);
            taskBarNotifier.TitleClick += new EventHandler(TitleClick);
            //taskBarNotifier.ContentClick += new EventHandler(ContentClick);
            //taskBarNotifier.CloseClick += new EventHandler(CloseClick);
            log.LogMethodExit();
        }
        /// <summary>
        /// Initializes the active player and the queue data. 
        /// Also, the permission is checked, whether the user is allowed to update and if no, the buttons are disabled. 
        /// </summary>
       
  

        private void FormQueueMgmt_Load(object sender, EventArgs e)
        {
            //MessageBox.Show(Utilities.getServerTime().ToString());
            //MessageBox.Show(DateTime.Now.ToString());
            log.LogMethodEntry();
            toolstripLoginID.Text = "Logged in as: " + Common.ParafaitEnv.LoginID;          
            toolStripRole.Text = "Role: " + Common.ParafaitEnv.Role;
            toolStripTime.Text = "Logged in at: " + DateTime.Now.ToString("dd-MMM-yyyy h:mm:ss tt");
            toolstripVersion.Text = "Version: 1.0";
            System.Reflection.Assembly prtImage = System.Reflection.Assembly.GetExecutingAssembly();
            string[] names = prtImage.GetManifestResourceNames();
            Stream stream = prtImage.GetManifestResourceStream("ParafaitQueueManagement.printer.gif");
             image = Image.FromStream(stream);
            
            dgridContextMenu.ShowImageMargin = true;
           

            if (manualOverrideAllowed == false)
            {
                Button initButton = (Button)this.Controls.Find("btnInitialize", true)[0];
                initButton.Visible = false;
                Button reinstateButton = (Button)this.Controls.Find("btnReinstate", true)[0];
                reinstateButton.Visible = false;
                Button setupTeamButton = (Button)this.Controls.Find("btnSetupTeam", true)[0];
                setupTeamButton.Visible = false;
            }
            loadServerInfo();
            queueManager = new QueueManager();
            SetupQueue();
            refreshForm();

            if (startTime <= Common.Utilities.getServerTime())
                startTime = startTime.AddDays(1);
            txtCenterClose.Text = "Center Closing Time: " + startTime.ToString("hh:mm tt");

            for (int queueMenuItems = 0; queueMenuItems < contextMenuItems.Length; queueMenuItems++)
                dgridContextMenu.Items.Add(contextMenuItems[queueMenuItems]);

            for (int activePlayerItem = 0; activePlayerItem < activePlayersContextMenu.Length; activePlayerItem++)
                dgActivePlayersContextMenu.Items.Add(activePlayersContextMenu[activePlayerItem]);

            transactioneventtimer.Interval = refreshFrequency;            
            transactioneventtimer.Tick+=new EventHandler(transactioneventtimer_Tick);
            transactioneventtimer.Start();
            log.LogMethodExit();
        }

        
        /// <summary>
        /// These functions mainly deal with the formatting of the datagridview 
        /// </summary>        
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
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToResizeColumns = true;
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

        public void SetupQueue()
        {
            log.LogMethodEntry();
            string[] listOfGames;
            int x = 0;
            listOfGames = queueManager.GetListOfGame();
            completeDGVOfQueue = new DataGridView[listOfGames.Length];
            //this.Size = new Size((200 * listOfGames.Length + 282 + 30), 614);
            for (int k = 0; k < listOfGames.Length; k++)
            {

                dgvCustomerQueue = new DataGridView();
                dgvCustomerQueue.ScrollBars = ScrollBars.Both;
                dgvCustomerQueue.MouseDown += new MouseEventHandler(dgvcustqueue_MouseDown);
                dgvCustomerQueue.CellContentDoubleClick+=new DataGridViewCellEventHandler(dgvCustomerQueue_CellContentDoubleClick);
                TextBox txtCustWait = new TextBox();
                txtCustWait.Name = listOfGames[k] + "CWTTextBox";
                txtCustWait.ReadOnly = true;
                txtCustWait.Size = new System.Drawing.Size(195, 100);
                
                TextBox txtplayFinishTime = new TextBox();
                txtplayFinishTime.Name = listOfGames[k] + "CPFTTextBox";
                txtplayFinishTime.ReadOnly = true;
                txtplayFinishTime.Size = new System.Drawing.Size(195, 100);

                setupDataGridview(ref dgvCustomerQueue);
                dgvCustomerQueue.GridColor = Color.White;
                dgvCustomerQueue.DefaultCellStyle.BackColor = Color.White;
               // dgvCustomerQueue.Col = DataGridViewHeaderBorderStyle.None;
                
               // dgvCustomerQueue.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                Label lblGameName = new Label();
                lblGameName.Size = new System.Drawing.Size(195, 100);
               // lblGameName.TextAlign = ContentAlignment.MiddleCenter;
                Application.DoEvents();
                dgvCustomerQueue.ColumnCount = 16;
                dgvCustomerQueue.AllowDrop = true;
                dgvCustomerQueue.Name = "dgv" + listOfGames[k];
                dgvCustomerQueue.Location = new Point(282 + x, 100);
                lblGameName.Location = new Point(282 + x, 80);
                txtCustWait.Location = new Point(282 + x, 434);
                
             
                txtplayFinishTime.Location = new Point(282 + x, 464);
                if (listOfGames[k].Length > 20)
                {
                    lblGameName.Text = listOfGames[k].Substring(0, 20);
                }
                else
                {
                    lblGameName.Text = listOfGames[k];
                }
                lblGameName.ForeColor = Color.Red;
                dgvCustomerQueue.Size = new Size(195, 320); //128,320
                
                dgvCustomerQueue.Columns[4].Name = "Team Name";
                dgvCustomerQueue.Columns[0].Name = "Name";
                Font WaitFont = new System.Drawing.Font("Tahoma", 9);
                Size WaitSize = TextRenderer.MeasureText("Wait", WaitFont);
                dgvCustomerQueue.Columns[2].Name = "Wait";               
                dgvCustomerQueue.Columns[2].Width = WaitSize.Width;
                dgvCustomerQueue.Columns[3].Name = "User Name";
                Font LaneNoFont = new System.Drawing.Font("Tahoma", 9);
                Size LaneNoSize = TextRenderer.MeasureText("Lane No", LaneNoFont);
                dgvCustomerQueue.Columns[1].Name = "Lane No";
                dgvCustomerQueue.Columns[1].Width = LaneNoSize.Width;
                dgvCustomerQueue.Columns[5].Name = "Play Time";
                dgvCustomerQueue.Columns[6].Name = "Card ID";
                dgvCustomerQueue.Columns[7].Name = "Customer ID";
                dgvCustomerQueue.Columns[8].Name = "Machine";
                dgvCustomerQueue.Columns[9].Name = "Time";
                dgvCustomerQueue.Columns[10].Name = "Group Number";
                dgvCustomerQueue.Columns[11].Name = "QueueId";
                dgvCustomerQueue.Columns[12].Name = "Print Count";
                dgvCustomerQueue.Columns[15].Name = "Unique ID";
                dgvCustomerQueue.Columns[13].Name = "Queue Entry";
                dgvCustomerQueue.Columns[14].Name = "Queue Entry Calc";
                dgvCustomerQueue.ReadOnly = true;
                dgvCustomerQueue.Columns[5].Visible = false;
                dgvCustomerQueue.Columns[6].Visible = false;
                dgvCustomerQueue.Columns[7].Visible = false;
                dgvCustomerQueue.Columns[8].Visible = false;
                dgvCustomerQueue.Columns[11].Visible = false;
                dgvCustomerQueue.Columns[9].Visible = false;
                dgvCustomerQueue.Columns[14].Visible = false;
                dgvCustomerQueue.Columns["Team Name"].Width = 85;
                dgvCustomerQueue.Columns["Name"].Width = 70;
                dgvCustomerQueue.Columns["Wait"].Width = 55;
                dgvCustomerQueue.Columns["Wait"].SortMode = DataGridViewColumnSortMode.Automatic;

                dgvCustomerQueue.Refresh();

                this.Controls.Add(dgvCustomerQueue);
                this.Controls.Add(lblGameName);
                this.Controls.Add(txtCustWait);
                this.Controls.Add(txtplayFinishTime);

                x = x + 200;
                dgvCustomerQueue.Name = listOfGames[k];
                completeDGVOfQueue[k] = dgvCustomerQueue;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// This function contains the entire logic for the population of the Active Players Datagridview.
        /// An array of the type MachineQueue is bound to the datagridview.
        /// </summary>
        public void ActivePlayersFunctions()
        {
            log.LogMethodEntry();
            activePlayerGridReady = false;          
            // Firstly the players on the machines are updated. This is to ensure that all the changes like game play are 
            // reflected properly in the game machine object.
            queueManager.refreshPlayersOnGameMachine();
            if (hitCounter >= 1)
            {
                currentCell = dgvActivePlayersList.CurrentCellAddress;
                if (dgvActivePlayersList.FirstDisplayedCell != null)
                {
                    currentColumnIndex = dgvActivePlayersList.FirstDisplayedCell.ColumnIndex;
                }
                currentRowIndex = dgvActivePlayersList.FirstDisplayedScrollingRowIndex;
            }
            dgvActivePlayersList.DataSource = "";
           
            //ScrollBars sc = dgvActivePlayersList.ScrollBars;
            dgvActivePlayersList.ScrollBars = ScrollBars.None;
            dgvActivePlayersList.DataSource = queueManager.getMachineDetails();
          
            setupDataGridview(ref dgvActivePlayersList);

            // Based on whether the queue is in debug mode, fields are enabled
            if (queueDebug.CompareTo("N") == 0)
            {
                for (int i = 0; i < dgvActivePlayersList.ColumnCount; i++)
                {
                    dgvActivePlayersList.Columns[i].Visible = false;
                    dgvActivePlayersList.Columns[i].ReadOnly = true;
                }
                dgvActivePlayersList.Columns["LaneName"].Visible = true;
                dgvActivePlayersList.Columns["CustomerName"].Visible = true;
                dgvActivePlayersList.Columns["TimeLeftMins"].Visible = true;
                dgvActivePlayersList.Columns["PauseStatus"].Visible = true;
                dgvActivePlayersList.Columns["PlayDate"].Visible = true;
                dgvActivePlayersList.Columns["UserName"].Visible = true;
                if (dgvActivePlayersList.Columns.Contains("Online"))
                {
                    dgvActivePlayersList.Columns["Online"].Visible = true;
                    dgvActivePlayersList.Columns["Online"].ReadOnly = false;
                }

            }

            dgvActivePlayersList.AllowUserToResizeColumns = true;

            dgvActivePlayersList.Columns["LaneName"].DisplayIndex = 0;
            string Lanetext = "LaneName";
            Font LaneNameFont = new System.Drawing.Font("Tahoma", 9);
            Size LaneNameSize = TextRenderer.MeasureText(Lanetext, LaneNameFont);

            dgvActivePlayersList.Columns["LaneName"].Width = LaneNameSize.Width;
            dgvActivePlayersList.Columns["UserName"].DisplayIndex = 1;
            string UserText = "UserName";
            Font UserNameFont = new System.Drawing.Font("Tahoma", 9);
            Size UserNameSize = TextRenderer.MeasureText(UserText, UserNameFont);

            dgvActivePlayersList.Columns["UserName"].Width = UserNameSize.Width;
            dgvActivePlayersList.Columns["TimeLeftMins"].DisplayIndex = 2;
            dgvActivePlayersList.Columns["TimeLeftMins"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dgvActivePlayersList.Columns["TImeLeftMins"].Width = 50;
            dgvActivePlayersList.Columns["CustomerName"].DisplayIndex = 4;
            dgvActivePlayersList.Columns["GamePlayId"].DisplayIndex = 5;
            dgvActivePlayersList.Columns["PauseStatus"].DisplayIndex = 6;
            dgvActivePlayersList.Columns["UniqueID"].DisplayIndex = 8;
            dgvActivePlayersList.Columns["PlayDate"].DisplayIndex = 10;
            dgvActivePlayersList.Columns["SerialNumber"].DisplayIndex = 9;
            dgvActivePlayersList.Columns["SerialNumber"].Visible = false;
            dgvActivePlayersList.Columns["PauseStatus"].Visible = false;
            dgvActivePlayersList.Columns["CustomerName"].Visible = false;
            dgvActivePlayersList.Columns["CardID"].Visible = false;
            dgvActivePlayersList.Columns["MacPlayer"].Visible = false;
            dgvActivePlayersList.Columns["ActiveFlag"].Visible = false;
            dgvActivePlayersList.Columns["IsActive"].Visible = false;
            dgvActivePlayersList.Columns["GamePlayId"].Visible = false;
            dgvActivePlayersList.Columns["GameMachineId"].Visible = false;
            dgvActivePlayersList.Columns["MachineName"].Visible = false;
            dgvActivePlayersList.Columns["GameNameOfMachine"].Visible = false;
            dgvActivePlayersList.Columns["TimeLeft"].Visible = false;

            if (!dgvActivePlayersList.Columns.Contains("Online"))
            {
                DataGridViewCheckBoxColumn chkcolumn = new DataGridViewCheckBoxColumn();
                chkcolumn.Name = "Online";
                chkcolumn.Width = 40;
                chkcolumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                chkcolumn.HeaderText = "*";
                dgvActivePlayersList.Columns.Add(chkcolumn);
                dgvActivePlayersList.Columns["Online"].DisplayIndex = 3;
                dgvActivePlayersList.Columns["Online"].Visible = true;
                dgvActivePlayersList.Columns["Online"].ReadOnly = false;
                if (manualOverrideAllowed == true)
                    dgvActivePlayersList.Columns["Online"].ReadOnly = false;
                else
                    dgvActivePlayersList.Columns["Online"].ReadOnly = true;
            }
            foreach (DataGridViewRow dgvrow in dgvActivePlayersList.Rows)
            {

                if (Convert.ToInt32(dgvrow.Cells["TimeLeft"].Value) > 0 && Convert.ToInt32(dgvrow.Cells["TimeLeft"].Value) <= 5)
                    dgvrow.DefaultCellStyle.BackColor = Color.Yellow;

                if (dgvrow.Cells["PauseStatus"].Value != DBNull.Value && dgvrow.Cells["PauseStatus"].Value != null)
                {
                    if (dgvrow.Cells["PauseStatus"].Value.ToString() == "Y")
                        dgvrow.DefaultCellStyle.BackColor = Color.LightBlue;
                }
                if (Convert.ToInt32(dgvrow.Cells["TimeLeft"].Value) <= 0 && (Convert.ToInt32(dgvrow.Cells["GamePlayId"].Value) != 0))
                {
                    //dgvrow.Cells["TimeLeft"].Value = 0;
                    if (Convert.ToInt32(dgvrow.Cells["TimeLeft"].Value) <= -60)
                        dgvrow.DefaultCellStyle.BackColor = Color.Red;
                    else
                        dgvrow.DefaultCellStyle.BackColor = Color.PaleVioletRed;
                    if (queueDebug.CompareTo("N") == 0)
                        dgvrow.Cells["TimeLeft"].Value = 0;
                }
                if (dgvrow.Cells["ActiveFlag"].Value.ToString().CompareTo("Y") == 0)
                    dgvrow.Cells["Online"].Value = true;
                else
                {
                    dgvrow.DefaultCellStyle.BackColor = Color.Fuchsia;
                    dgvrow.Cells["Online"].Value = false;
                }
            }
            //dgvActivePlayersList.Sort(dgvActivePlayersList.Columns["SerialNumber"], ListSortDirection.Ascending);
            dgvActivePlayersList.ScrollBars = ScrollBars.Both;
            //dgvActivePlayersList.RefreshEdit();
            //if (currentCell.X > -1)
            //{
            //    //dgvActivePlayersList.CurrentCell = dgvActivePlayersList.Rows[currentCell.Y].Cells[currentCell.X];
            //    dgvActivePlayersList.CurrentCell = dgvActivePlayersList.Rows[2].Cells[0];
            //    dgvActivePlayersList.FirstDisplayedScrollingColumnIndex = currentColumnIndex;
            //    dgvActivePlayersList.FirstDisplayedScrollingColumnIndex = 2;
            //    dgvActivePlayersList.FirstDisplayedScrollingRowIndex = currentRowIndex;
            //}
            activePlayerGridReady = true;
            log.LogMethodExit();
        }

        public void FetchQueueData()
        {
            log.LogMethodEntry();
            string genCustomerQueueResult = string.Empty;

            genCustomerQueueResult = queueManager.generateCustomerQueue();
            if (digitalTokenEnabled == true)
                queueManager.activatePlayers(totalActivePlayers);
            if (genCustomerQueueResult != "")
                MessageBox.Show(genCustomerQueueResult,"ALERT",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            gameQueueComplete = queueManager.getPlayersList();
            log.LogMethodExit();
        }

        
        /// <summary>
        /// This function consists of the logic for the generation of the customer queue.
        /// The datagridviews are created and populated dynamically.
        /// </summary>
        public void CustQueueFunctions()
        {
            log.LogMethodEntry();
            Application.DoEvents();
            

            string[] listOfGames = queueManager.GetListOfGame();
            GameMachineList gameMachinesInSystem = queueManager.GetGameMachineList();
            DataGridView dgvCustomerGameQueue = null;
            log.Debug("listOfGames: " + listOfGames);
            for (int k = 0; k < listOfGames.Length; k++)
            {
                int maxwait = 0;
                int waitVal = 0;
                int currentrow = 0;
              
                TextBox txtCustomerWaitTextBox = (TextBox)this.Controls.Find(listOfGames[k] + "CWTTextBox", true)[0];
                TextBox txtPlayFinishTimeTextBox = (TextBox)this.Controls.Find(listOfGames[k] + "CPFTTextBox", true)[0]; 
                for (int i = 0; i < listOfGames.Length; i++)
                {
                    if (string.Compare(completeDGVOfQueue[i].Name, listOfGames[k]) == 0)
                        dgvCustomerGameQueue = completeDGVOfQueue[i];
                }
                dgvCustomerGameQueue.Rows.Clear();
                for (int queuecount = 0; queuecount < gameQueueComplete.Length; queuecount++)
                {
                    if ((gameQueueComplete[queuecount] != null) && (gameQueueComplete[queuecount].GameMachineAssigned != -1))
                    {
                        GameMachine currentGameMachine = gameMachinesInSystem.IdentifyGameMachine(gameQueueComplete[queuecount].GameMachineAssigned);
                        if ((gameQueueComplete[queuecount].Name != null) && (string.Compare(currentGameMachine.GameNameOfMachine, listOfGames[k]) == 0))
                        {
                            dgvCustomerGameQueue.Rows.Add();
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Team Name"].Value = gameQueueComplete[queuecount].TeamName;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Name"].Value = gameQueueComplete[queuecount].Name;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["User Name"].Value = gameQueueComplete[queuecount].UserName;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Time"].Value = gameQueueComplete[queuecount].DisplayWaitTime;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Play Time"].Value = gameQueueComplete[queuecount].DisplayGamePlayTime;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Card Id"].Value = gameQueueComplete[queuecount].CardId;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Customer Id"].Value = gameQueueComplete[queuecount].CustomerId;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Machine"].Value = gameQueueComplete[queuecount].GameMachineAssigned;
                          //  dgvCustomerGameQueue.Rows[currentrow].Cells["Lane No"].Value = gameQueueComplete[queuecount].MachineName;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Lane No"].Value = gameQueueComplete[queuecount].GameMachineAssignedSerialNumber;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Wait"].Value = gameQueueComplete[queuecount].GroupWaitTime;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Group Number"].Value = gameQueueComplete[queuecount].GroupWaitNumber;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["QueueId"].Value = gameQueueComplete[queuecount].QueueEntryId;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Print Count"].Value = gameQueueComplete[queuecount].PrintCount;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Unique ID"].Value = gameQueueComplete[queuecount].CardNumber;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Queue Entry"].Value = gameQueueComplete[queuecount].QueueEntryTime;
                            dgvCustomerGameQueue.Rows[currentrow].Cells["Queue Entry Calc"].Value = gameQueueComplete[queuecount].CalcQueueEntryTime;

                            waitVal = Convert.ToInt32(dgvCustomerGameQueue.Rows[currentrow].Cells[2].Value);

                            if ((waitVal > endOfPlayWarningTime) && (waitVal <= playStartAlertTime))
                                dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.Brown;

                            if ((waitVal > endOfPlayErrorTime) && (waitVal <= endOfPlayWarningTime))
                                dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.Yellow;

                            if (waitVal <= endOfPlayErrorTime)
                                dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.Red;

                            if ((gameQueueComplete[queuecount].PrintCount > 0) && (digitalTokenEnabled == false))
                                dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.LightGreen;
                            else
                            {
                                if ((gameQueueComplete[queuecount].PrintCount > 0) && (digitalTokenEnabled == true))
                                {
                                    if (gameQueueComplete[queuecount].CustomerIntimated == true)
                                        dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.LightGreen;
                                    else
                                    {
                                        if ((waitVal > endOfPlayWarningTime) && (waitVal <= playStartAlertTime))
                                            dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.Brown;
                                        else
                                            if ((waitVal > endOfPlayErrorTime) && (waitVal <= endOfPlayWarningTime))
                                                dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.Yellow;
                                            else
                                                if (waitVal <= endOfPlayErrorTime)
                                                    dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.Red;
                                                else
                                                    dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.MediumPurple;
                                    }
                                }
                                else
                                {
                                    if ((gameQueueComplete[queuecount].CustomerIntimated == true) && (digitalTokenEnabled == true))
                                        dgvCustomerGameQueue.Rows[currentrow].DefaultCellStyle.BackColor = Color.Orange;
                                }
                            }
                            currentrow = currentrow + 1;
                        }
                    }
                   
                }
                //dgvcustqueue.Sort(dgvcustqueue.Columns["Time"], ListSortDirection.Ascending);
                int queueIdShortest = -1;
                queueManager.shortestWaitMachine(listOfGames[k], ref queueIdShortest, ref maxwait);
                maxwait = maxwait / 60;
                double waitTimeinMin = 0;

                txtCustomerWaitTextBox.Text = "WaitTime: " + Convert.ToString(maxwait + queueSetupTime);
                waitTimeinMin = Convert.ToDouble(Convert.ToString(maxwait + queueBufferTime + queueSetupTime));
                txtPlayFinishTimeTextBox.Text = "Play finish time: " + Common.Utilities.getServerTime().AddMinutes(waitTimeinMin).ToString("hh:mm tt");
                DateTime fintime = Common.Utilities.getServerTime().AddMinutes(waitTimeinMin);
                TimeSpan diff = startTime -fintime;
                if ((Math.Abs(diff.TotalMinutes) <= 15))// && diff.Days==0) || fintime > startTime)
                    txtPlayFinishTimeTextBox.BackColor = Color.Red;
                else
                    txtPlayFinishTimeTextBox.BackColor = Color.Empty;
                
            }

            if(dgvCustomerGameQueue != null)
                dgvCustomerGameQueue.Refresh();
            log.LogMethodExit();
        }
       
        public void transactioneventtimer_Tick(object sender,EventArgs e)
        {
            log.LogMethodEntry();
            refreshForm();
            log.LogMethodExit();
        }
        /// <summary>
        /// This event is used to commit the changes made by the lane allocator in the lanes(online/offline) and implicitly 
        /// call the dgvActivePlayersList_CellValueChanged event.
        /// </summary>
        private void dgvActivePlayersList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (dgvActivePlayersList.IsCurrentCellDirty)
            {
                if (MessageBox.Show("Lane status will change, severely affecting the queue, please confirm", "Confirm Lane Change", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    dgvActivePlayersList.CommitEdit(DataGridViewDataErrorContexts.Commit);
                else
                    dgvActivePlayersList.CancelEdit();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This event is used to handle the updations when the lane allocator take a lane offline or brings it online
        /// </summary>
        
        private void dgvActivePlayersList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            char activeCheck = 'N';
            string laneAllocatorResult = string.Empty;
            int machineIdActedUpon = 0;
            if (dgvActivePlayersList.Columns[e.ColumnIndex].Name == "Online" && activePlayerGridReady == true)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dgvActivePlayersList.Rows[e.RowIndex].Cells["Online"];
                if (checkCell.Value.ToString() == "True")
                    activeCheck = 'Y';
                else
                {
                    if (checkCell.Value.ToString() == "False")
                        activeCheck = 'N';
                }
                log.Debug("activeCheck: " + activeCheck);
                machineIdActedUpon = Convert.ToInt32(dgvActivePlayersList.Rows[e.RowIndex].Cells["GameMachineId"].Value);
                queueManager.updateActiveLanes(machineIdActedUpon, activeCheck);
                refreshForm();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This event is used to capture the X and Y co-ordinates of the selected row in the grid 
        /// when the user right clicks on the context menu.
        /// </summary>
        DataGridView queueAdvanceDataGridView = null;
        private void dgvcustqueue_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            int groupNumber;
            if (manualOverrideAllowed == true)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgv = sender as DataGridView;
                    contextMenuOpen = true;
                    queueAdvanceDataGridView = sender as DataGridView;
                    if (dgv.Rows.Count != 1)
                    {
                        var hti = dgv.HitTest(e.X, e.Y);
                        if (hti.RowIndex != -1)
                        {
                            hitrowindex = dgv.Rows[hti.RowIndex].Index;
                            if (dgv.Rows[hti.RowIndex].Cells["Name"].Value != null)
                            {
                                customerQueueCustomerName = dgv.Rows[hti.RowIndex].Cells["Name"].Value.ToString();
                                customerQueueId = Convert.ToInt32(dgv.Rows[hti.RowIndex].Cells["QueueId"].Value);
                                groupNumber = Convert.ToInt32(dgv.Rows[hti.RowIndex].Cells["Group Number"].Value);
                                customerQueueTeamName = dgv.Rows[hti.RowIndex].Cells["Team Name"].Value.ToString();
                                printCountOfCustomer = Convert.ToInt32(dgv.Rows[hti.RowIndex].Cells["Print Count"].Value);
                                customerUniqueId = dgv.Rows[hti.RowIndex].Cells["Unique ID"].Value.ToString();
                                dgridContextMenu.Items.Clear();
                                if (groupNumber != 0)
                                {
                                    for (int item = 0; item < contextMenuItemsGroupList.Length; item++)
                                        dgridContextMenu.Items.Add(contextMenuItemsGroupList[item]);
                                }
                                else
                                {
                                    int item;
                                    for (item = 0; item < (contextMenuItems.Length - 2); item++)
                                        if (contextMenuItems[item].Contains("Print"))
                                            dgridContextMenu.Items.Add(contextMenuItems[item], image);
                                        else
                                            dgridContextMenu.Items.Add(contextMenuItems[item]);
                                    if (digitalTokenEnabled == true)
                                    {
                                        dgridContextMenu.Items.Add(contextMenuItems[item++]);
                                        dgridContextMenu.Items.Add(contextMenuItems[item]);
                                    }
                                }

                                dgridContextMenu.Show(dgv, new Point(e.X, e.Y));
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This event is used to process the item which is chosen by the user in the context menu
        /// Currently there are 3 options to choose from: Advancing the player by X minutes, removing the player 
        /// from the queue, moving the player to the priority queue.
        /// </summary>
        private void dgridContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry();
            dgridContextMenu.Hide();           
            string option = string.Empty;
            option = e.ClickedItem.ToString();
            log.LogVariableState("option", option);

            switch (option)
            {
                case "Advance":
                    if (MessageBox.Show("Customer being advanced, please confirm?", "Confirm Advance", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        frmAdvanced = new Form();
                        Label lbladvanced = new Label();
                        txtMinutes = new TextBox();
                        Button btnSetAdvanced = new Button();
                        btnSetAdvanced.Click += new EventHandler(btnSetAdvanced_Click);
                        btnSetAdvanced.Location = new Point(140, 20);
                        btnSetAdvanced.Width = 40;
                        btnSetAdvanced.Text = "Set";
                        lbladvanced.Location = new Point(10, 25);
                        txtMinutes.Width = 70;
                        txtMinutes.Location = new Point(70, 20);
                        lbladvanced.Text = "Positions";
                        lbladvanced.Size = new System.Drawing.Size(60, 55);
                        lbladvanced.Visible = true;
                        frmAdvanced.Controls.Add(btnSetAdvanced);
                        frmAdvanced.Controls.Add(lbladvanced);
                        frmAdvanced.Controls.Add(txtMinutes);
                        frmAdvanced.StartPosition = FormStartPosition.CenterScreen;
                        frmAdvanced.Width = 250;
                        frmAdvanced.Height = 100;
                        frmAdvanced.Text = "Advance " + customerQueueCustomerName;
                        frmAdvanced.MaximizeBox = false;
                        frmAdvanced.MinimizeBox = false;
                        frmAdvanced.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        frmAdvanced.Load += new EventHandler(frmAdvanced_Load);
                        frmAdvanced.ShowDialog();
                        refreshForm();
                    }
                    break;
                    
                case "Move To Priority":
                    if (printCountOfCustomer > 0)
                        MessageBox.Show("Customer " +customerQueueCustomerName+"'s token has already been printed. Cannot be moved to any other lane.");
                    else
                    {
                        if (MessageBox.Show("Customer " +customerQueueCustomerName+" being moved to priority queue, please confirm?", "Confirm Priority move", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string moveResult = string.Empty;
                            int returnCode = queueManager.moveToPriority(customerQueueId, Common.Utilities.ParafaitEnv.LoginID, ref moveResult);
                            if (returnCode != 0)
                                MessageBox.Show(moveResult);
                            else
                                refreshForm();
                        }
                    }
                    break;
                case "Remove":
                    DialogResult frmclose = MessageBox.Show("Are you sure you want to delete " + customerQueueCustomerName + " from the queue?", "Remove Customer", MessageBoxButtons.OKCancel);
                    if (frmclose == DialogResult.OK)
                    {
                        DialogResult frmDoubleConfirm = MessageBox.Show("Second confirm - Are you sure you want to delete " + customerQueueCustomerName + " from the queue?", "Remove Customer", MessageBoxButtons.OKCancel);
                        if (frmDoubleConfirm == DialogResult.OK)
                        {
                            try
                            {
                                queueManager.updateCustomerQueuePlayerRemoval(customerQueueId);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Unable to remove" + customerQueueCustomerName + ex.Message.ToString());
                            }
                            refreshForm();
                        }
                    }                  
                    break;
                case "Print Token":
                    queueManager.PrintTokens(customerQueueId);
                    refreshForm();
                    break;
                case "Inform Customer":
                    if (MessageBox.Show("Customer " + customerQueueCustomerName + " is being marked as informed, please confirm?", "Confirm Activation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        queueManager.Activate(customerQueueId);
                        refreshForm();
                    }
                    break;
                case "Undo Token":
                    if (MessageBox.Show("Customer " + customerQueueCustomerName + "'s token is being revoked, please confirm?", "Confirm Revoke", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        queueManager.UndoPrintTokens(customerQueueId, true);
                        refreshForm();
                    }
                    break;
                case "Assign Lane":
                    if (MessageBox.Show("Lane being Assigned, please confirm?", "Confirm Lane Assign", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        frmAssignLane = new Form();
                        Label lblAssignLane = new Label();
                        txtLaneNo = new TextBox();
                        Button btnSetLaneNo = new Button();
                        btnSetLaneNo.Click+=new EventHandler(btnSetLaneNo_Click);
                        btnSetLaneNo.Location = new Point(140, 20);
                        btnSetLaneNo.Width = 50;
                        btnSetLaneNo.Text = "Assign";
                        lblAssignLane.Location = new Point(10, 25);
                        txtLaneNo.Width = 70;
                        txtLaneNo.Location = new Point(70, 20);
                        lblAssignLane.Text = "Lane No: ";
                        lblAssignLane.Size = new System.Drawing.Size(60, 55);
                        lblAssignLane.Visible = true;
                        frmAssignLane.Controls.Add(btnSetLaneNo);
                        frmAssignLane.Controls.Add(lblAssignLane);
                        frmAssignLane.Controls.Add(txtLaneNo);
                        frmAssignLane.StartPosition = FormStartPosition.CenterScreen;
                        frmAssignLane.Width = 250;
                        frmAssignLane.Height = 100;
                        frmAssignLane.Text = "Assign Lane To " + customerQueueCustomerName;
                        frmAssignLane.MaximizeBox = false;
                        frmAssignLane.MinimizeBox = false;
                        frmAssignLane.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        frmAssignLane.Load+=new EventHandler(frmAssignLane_Load);
                        frmAssignLane.ShowDialog();
                        refreshForm();
                    }
                    break;
                case "Update Team Name":
                    frmTeamNameUpdate = new Form();
                    Label lblTeamNameUpdate= new Label();
                    txtTeamName = new TextBox();
                    Button btnTeamNameUpdate = new Button();
                    btnTeamNameUpdate.Click += new EventHandler(btnTeamNameUpdate_Click);
                    btnTeamNameUpdate.Location = new Point(270, 20);
                    btnTeamNameUpdate.Width = 40;
                    btnTeamNameUpdate.Text = "Set";
                    lblTeamNameUpdate.Location = new Point(10, 25);
                    txtTeamName.Width = 140;
                    txtTeamName.Location = new Point(120, 20);
                    lblTeamNameUpdate.Text = "Enter Team Name";
                    lblTeamNameUpdate.Size = new System.Drawing.Size(105, 55);
                    lblTeamNameUpdate.Visible = true;
                    frmTeamNameUpdate.Controls.Add(btnTeamNameUpdate);
                    frmTeamNameUpdate.Controls.Add(lblTeamNameUpdate);
                    frmTeamNameUpdate.Controls.Add(txtTeamName);
                    frmTeamNameUpdate.StartPosition = FormStartPosition.CenterScreen;
                    frmTeamNameUpdate.Width = 400;
                    frmTeamNameUpdate.Height = 100;
                    frmTeamNameUpdate.Text = "Team Name Update: " +dgv.Rows[hitrowindex].Cells["Team Name"].Value.ToString();
                    frmTeamNameUpdate.MaximizeBox = false;
                    frmTeamNameUpdate.MinimizeBox = false;
                    frmTeamNameUpdate.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    frmTeamNameUpdate.Load += new EventHandler(frmTeamNameUpdate_load);
                    frmTeamNameUpdate.ShowDialog();
                    break;
                case "View Customer Detail":
                    frmCustomerDetails frmcust=new frmCustomerDetails();
                    frmcust.Show();
                    string encryptedPassPhrase = Common.Utilities.getParafaitDefaults("CUSTOMER_ENCRYPTION_PASS_PHRASE");
                    string passPhrase = encryptedPassPhrase;

                    frmcust.loadCustomerDetails(customerUniqueId, customerQueueCustomerName, passPhrase);
                    break;
                case "View H-E Detail":
                    if (Common.Utilities.getParafaitDefaults("EXTERNAL_GAME_SYSTEM_WEB_SERVICE_URL") != "")
                    {
                        frmHEDetails frmHEDetails = new frmHEDetails();
                        frmHEDetails.Show();
                        frmHEDetails.LoadHEDetails(customerUniqueId);
                    }
                    else
                    {
                        MessageBox.Show("External URL has not been setup","HE URL");
                    }
                    //frmHEDetails.loadCustomerDetails(customerUniqueId, customerQueueCustomerName);
                    break;
            }
            log.LogMethodExit();
        }
        public void refreshForm()
        {
            log.LogMethodEntry();
            if (Application.OpenForms.Count == 1)
            {
                if (this.Visible && !this.CanFocus)
                {
                }
                else
                {
                    ActivePlayersFunctions();
                    FetchQueueData();
                    queueManager.OnTransactionEvent();
                    CustQueueFunctions();
                    hitCounter++;
                }
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            refreshForm();
            txtCenterClose.Text = "Center Closing Time: " + startTime.ToString("hh:mm tt");
            log.LogMethodExit();
        }
        private void frmAdvanced_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            frmAdvanced.Activate();
            frmAdvanced.Show();
            txtMinutes.Focus();
            log.LogMethodExit();
        }
        /// <summary>
        /// This event is fired when the user sets the players advanced time.
        /// </summary>
        private void btnSetAdvanced_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (txtMinutes.Text != "")
            {
                int movePosition = Convert.ToInt32(txtMinutes.Text);

                if (queueAdvanceDataGridView == null)
                    MessageBox.Show("Failure to advance, system error. Please contact admin", "Advance Customer", MessageBoxButtons.OK);
                else
                {
                    int dirMovement;
                    DateTime advanceCustomerDateTime = Convert.ToDateTime(queueAdvanceDataGridView.Rows[hitrowindex].Cells["Queue Entry Calc"].Value);
                    DateTime prevCustomerDateTime = Convert.ToDateTime(queueAdvanceDataGridView.Rows[hitrowindex].Cells["Queue Entry Calc"].Value);
                    DateTime currentCustomerDateTime = prevCustomerDateTime;
                    int currentGridRowIndex = hitrowindex;
                    if (movePosition < 0)
                            dirMovement = 1;
                    else
                            dirMovement = -1;
                    movePosition -= dirMovement;
                    while (movePosition != 0)
                    {
                        currentGridRowIndex += dirMovement;
                        if ((currentGridRowIndex < 0) || (currentGridRowIndex > (queueAdvanceDataGridView.RowCount - 2)))
                        {
                            movePosition = 0;
                            prevCustomerDateTime = currentCustomerDateTime;
                            currentCustomerDateTime = currentCustomerDateTime.AddMinutes(1 * dirMovement);
                        }
                        else
                        {
                            if (Convert.ToInt32(queueAdvanceDataGridView.Rows[currentGridRowIndex].Cells["Group Number"].Value) == 0)
                            {
                                prevCustomerDateTime = currentCustomerDateTime;
                                currentCustomerDateTime = Convert.ToDateTime(queueAdvanceDataGridView.Rows[currentGridRowIndex].Cells["Queue Entry Calc"].Value);
                                movePosition += dirMovement;
                            }
                        }
                    }
                    
                    double totalTimeToAdvance = ((currentCustomerDateTime - prevCustomerDateTime).TotalSeconds/2);
                    DateTime finalDateTime = prevCustomerDateTime.AddSeconds(totalTimeToAdvance);
                    int finalAdvanceTime = Convert.ToInt32((advanceCustomerDateTime - finalDateTime).TotalSeconds);
                    queueManager.updateCustomerAdvancedTime(finalAdvanceTime, Convert.ToInt32(dgv.Rows[hitrowindex].Cells["QueueId"].Value));
                }
            }
           frmAdvanced.Close();
            log.LogMethodExit();
        }
        
        private void frmTeamNameUpdate_load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            frmTeamNameUpdate.Activate();
            frmTeamNameUpdate.Show();
            txtTeamName.Focus();
            log.LogMethodExit();
        }
        private void btnTeamNameUpdate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (txtTeamName.Text != "")
                queueManager.updateGroupName(txtTeamName.Text, Convert.ToInt32(dgv.Rows[hitrowindex].Cells["QueueId"].Value));
            frmTeamNameUpdate.Close();
            refreshForm();
            log.LogMethodExit();
        }
        private void frmAssignLane_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            frmAssignLane.Activate();
            frmAssignLane.Show();
            txtLaneNo.Focus();
            log.LogMethodExit();
        }
        private void btnSetLaneNo_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int result=0;
            if (txtLaneNo.Text != "") 
            {
                string refMessageOfLaneAssignment = "";
                int serialNumber;
                bool parseSerialNumStatus = int.TryParse(txtLaneNo.Text.Trim(), out serialNumber);
                if (!parseSerialNumStatus)
                {
                    MessageBox.Show("Lane number is not numeric, please enter a number", "Lane number error");
                }
                else
                {
                    result = queueManager.AssignLane(customerQueueId, serialNumber, ref refMessageOfLaneAssignment);
                    if (result < 0)
                        MessageBox.Show(refMessageOfLaneAssignment, "Lane Assignment Error");
                }
            }
            frmAssignLane.Close();
            log.LogMethodExit();
        }

        private void FormQueueMgmt_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            DialogResult result = MessageBox.Show("Do you really want to exit Queue Management System?", "Exit QueueManagement", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {   
                this.Dispose();
            }
            else
            {
                e.Cancel = true;
            }
            log.LogMethodExit();
        }

        private void dgvActivePlayersList_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            DataGridView dgvactplayers;
            if (manualOverrideAllowed == true)
            {
                if (e.Button == MouseButtons.Right)
                {
                    dgvactplayers = sender as DataGridView;
                    if (dgvactplayers.Rows.Count != 1)
                    {
                        var hti = dgvactplayers.HitTest(e.X, e.Y);
                        if (hti.RowIndex != -1)
                        {
                            hitrowindex = dgvactplayers.Rows[hti.RowIndex].Index;
                            if (dgvactplayers.Rows[hti.RowIndex].Cells["GameMachineId"].Value != null)
                            {
                                gamePlayId = Convert.ToInt32(dgvactplayers.Rows[hti.RowIndex].Cells["GamePlayId"].Value);
                                gameMachineId = Convert.ToInt32(dgvactplayers.Rows[hti.RowIndex].Cells["GameMachineId"].Value);
                                dgActivePlayersContextMenu.Show(dgvactplayers, new Point(e.X, e.Y));
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void dgActivePlayersContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry();
            dgActivePlayersContextMenu.Hide();           
            string option = string.Empty;
            string errorMessage = string.Empty; 
            option = e.ClickedItem.ToString();
            log.LogVariableState("option", option);
            switch (option)
            {
                case "Pause":
                    if (gamePlayId != 0)
                    {
                        if (MessageBox.Show("Really pause?", "Confirm Pause", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int statusCheck = queueManager.pause(gameMachineId, ref errorMessage);
                            if (statusCheck != 0)
                                MessageBox.Show(errorMessage, "Pause Action", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            else
                                refreshForm();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid, as no game play active", "Pause Action", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    break;
                case "Restart":
                    if (gamePlayId != 0)
                    {
                        if (MessageBox.Show("Really restart?", "Confirm Restart", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int statusCheck = queueManager.unPause(gameMachineId, ref errorMessage);
                            if (statusCheck != 0)
                                MessageBox.Show(errorMessage, "Restart Action", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            else
                                refreshForm();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid, as no game play active", "Restart Action", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    break;
                case "End":
                    if (gamePlayId != 0)
                    {
                        if (MessageBox.Show("Really end the game?", "Confirm end game", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            int statusCheck = queueManager.GameEnd(gameMachineId, ref errorMessage);
                            if (statusCheck != 0)
                                MessageBox.Show(errorMessage, "Game End Action", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid, as no game play active", "Game End Action", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                    break;
            }
            log.LogMethodExit();
        }

        private void btnInitialize_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            bool initializeVarsStatus = initializeMasterVariables();
            if (initializeVarsStatus == false)
                Environment.Exit(0);
            log.LogMethodExit();
        }

        private void btnReinstate_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            //taskBarNotifier.Show("test", "xyz", 500, 600, 300);
            frmReinstateCustomer frmReinstate = new frmReinstateCustomer();
            frmReinstate.ShowDialog();
            refreshForm();
            log.LogMethodExit();
        }
        void ChildFormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogMethodEntry();
            refreshForm();
            log.LogMethodExit();
        }
        private void btnSetupTeam_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            frmSetupTeam frmTeamSetup = new frmSetupTeam();
            frmTeamSetup.SetPlayersList(gameQueueComplete);
            frmTeamSetup.ShowDialog();
            refreshForm();
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult result = MessageBox.Show("Do you really want to exit Queue Management System?", "Exit QueueManagement", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                //e.Cancel = true;
            }
            log.LogMethodExit();
        }
        private void loadServerInfo()
        {
            log.LogMethodEntry();
            string filename = "ParafaitQueueManagement";
            string serverName = string.Empty;
            ExeConfigurationFileMap filemap = new ExeConfigurationFileMap();
            filemap.ExeConfigFilename = filename + ".exe.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(filemap, ConfigurationUserLevel.None);
            ConnectionStringsSection conSection = config.ConnectionStrings;
            //MessageBox.Show(conSection.ConnectionStrings.ToString());
            ConnectionStringSettingsCollection conStrings = conSection.ConnectionStrings;
            foreach (ConnectionStringSettings conSetting in conStrings)
            {
                SqlConnectionStringBuilder serverinfo = new SqlConnectionStringBuilder(conSetting.ConnectionString);
                toolStripServer.Text = serverinfo.DataSource;
            }
            log.LogMethodExit();
        }

        public void TitleClick(object sender, EventArgs e)
        {
        }
        public void ContentClick(object sender, EventArgs e)
        {
        }
        public void CloseClick(object sender, EventArgs e)
        {
        }

     

        public void dgvCustomerQueue_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           // dgvCustomerQueue = sender as DataGridView;
           // if (dgvCustomerQueue.CurrentRow == null)
           //     return;

           //// MessageBox.Show(dgvCustomerQueue.CurrentRow.Cells["Name"].Value.ToString());
           // frmCustomerDetails frmcust=new frmCustomerDetails();
           // frmcust.Show();
           // frmcust.loadCustomerDetails(dgvCustomerQueue.CurrentRow.Cells["Unique ID"].Value.ToString());

        }

        //string keyPressed = "";
        private void FormQueueMgmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            //if ((int)e.KeyChar == 16 && keyPressed == "")
            if ((int)e.KeyChar == 12)
            {
                GraphFormSelect graphselect = new GraphFormSelect();
                graphselect.ShowDialog();
                // GraphicalView gview = new GraphicalView();
               // gview.ShowDialog();
            }

            log.LogMethodExit();    
            
        }
        //private void dgvActivePlayersList_CurrentCellChanged(object sender, EventArgs e)
        //{
        //    if (dgvActivePlayersList.CurrentCell != null)
        //    {
        //        int currentColumnIndex = dgvActivePlayersList.CurrentCell.ColumnIndex;
        //        Rectangle rec = dgvActivePlayersList.GetColumnDisplayRectangle(currentColumnIndex, false);
        //        Rectangle visiblePart = dgvActivePlayersList.GetColumnDisplayRectangle(currentColumnIndex, false);

        //        if ((visiblePart.Width < rec.Width) || (rec.Width == 0))
        //        {
        //            dgvActivePlayersList.FirstDisplayedCell = dgvActivePlayersList.CurrentCell;
        //            dgvActivePlayersList.FirstDisplayedScrollingColumnIndex = currentColumnIndex;
        //        }
        //    }
        //}                
    }
}
