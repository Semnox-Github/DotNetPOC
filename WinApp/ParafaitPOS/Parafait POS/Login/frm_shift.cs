/********************************************************************************************
* Project Name - Parafait POS
* Description  - Shift UI
*********************************************************************************************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for MultiPoint and Pause allowed changes 
*2.50.0      03-Dec-2018      Mathew Ninan       Deprecated StaticDataExchange class 
*2.60.0      01-Mar-2019      Indhu K            Modified for Remote Shift Open/Close changes
*2.60.2      21-May-2019      Nitin Pai          Fixes for Remote Shift Open/Close changes
*2.70.1      04-Jun-2019      Divya A            Petty Cash Enhancements
*2.70.2      20-Aug-2019      Girish Kundar      Modified : Added Logger methods and Removed unused namespace's 
*2.70.2      06-Jan-2020      Girish Kundar      Modified : Fiscal printer  issue fix for  shift open print
*2.90.0      26-Jul-2020      Girish Kundar      Modified : BowaPegas shift Opem/close and Viber receipt changes
*2.110.0     22-Dec-2020      Girish Kundar      Modified :FiscalTrust changes - Shift open/Close to be fiscalized
*2.120       26-Apr-2021      Laster Menezes     Modified :RunXReport method to use receipt framework of reports to generate POSX report on one click
*2.140.0     14-Aug-2021      Deeksha            Modified : Provisional Shift changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using QRCoder;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Printer.Cashdrawers;
using Semnox.Parafait.Reports;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Parafait_POS
{
    public partial class frm_shift : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataTable DT_shift = new DataTable();
        private string shiftType;
        private string shiftApplication;
        private PrintDocument MyPrintDocument;
        private bool isCloseShiftSuccess = false;
        private Utilities Utilities = POSStatic.Utilities;
        private ParafaitEnv ParafaitEnv = POSStatic.ParafaitEnv;
        private List<UserRolesDTO> managerRole = new List<UserRolesDTO>();
        private List<UsersDTO> usersDTOList = new List<UsersDTO>();
        private List<POSMachineDTO> pOSMachineDTOList = new List<POSMachineDTO>();
        private List<ShiftDTO> OpenShiftDTOList = new List<ShiftDTO>();
        private List<POSMachineDTO> selectedPOSMachines = new List<POSMachineDTO>();
        private List<UsersDTO> selectedUsersDTO = new List<UsersDTO>();
        private bool shiftOpen = true;
        private bool shiftClose = true;
        private string fiscalSignature = string.Empty;
        private ExecutionContext executionContext;
        private bool isCloseBlindShifts = false;
        private ShiftDTO shiftDTO = new ShiftDTO();
        POSMachines pOSMachines;
        bool isEODOperation = false;

        public frm_shift(string shift_type, string shift_application, ParafaitEnv pParafaitEnv)
        {
            log.LogMethodEntry(shift_type, shift_application, pParafaitEnv);
            InitializeComponent();
            executionContext = Utilities.ExecutionContext;
            Utilities.setLanguage(this);
            this.ParafaitEnv = pParafaitEnv;
            POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId);
            shiftType = shift_type;
            shiftApplication = shift_application;
            MyPrintDocument = new PrintDocument();
            MyPrintDocument.PrintPage += new PrintPageEventHandler(MyPrintDocument_PrintPage);
            pOSMachines = new POSMachines(executionContext, ParafaitEnv.POSMachineId);
            log.LogMethodExit();
        }
        private const int CS_NOCLOSE = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CS_NOCLOSE;
                return cp;
            }
        }

        private void frm_shift_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (POSUtils.OpenShiftListDTOList != null &&
                POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).Count >= 1
                && ParafaitEnv.LoginID != POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).FirstOrDefault().ShiftLoginId)
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 508) + ". "
                            + MessageContainerList.GetMessage(executionContext, "Shift belongs to "
                            + POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId).FirstOrDefault().ShiftLoginId),
                            "POS Unlock");
                this.Close();
                return;
            }
            if (ParafaitEnv.AllowShiftOpenClose == false)
            {
                panelPrint.Enabled = cb_open.Enabled = cb_close.Enabled = false;
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4010)); // "User has no shift access"
                this.Close();
                return;
            }
            usersDTOList = GetUsersList();
            pOSMachineDTOList = GetPOSMachineList();
            if (shiftType == ShiftDTO.ShiftActionType.ROpen.ToString() && (usersDTOList == null || usersDTOList.Count == 0 ||
                    pOSMachineDTOList == null || pOSMachineDTOList.Count == 0))
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 2048) + "usersDTOList :" + usersDTOList.Count + "pOSMachineDTOList : " + pOSMachineDTOList.Count);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 2048));
                this.Close();
                return;
            }
            LoadPOSCounter();
            if (ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_BLIND_CLOSE_SHIFT") == "N")
            {
                rbBlindClose.Visible = false;
            }
            if (shiftType == ShiftDTO.ShiftActionType.ROpen.ToString())
            {
                btnRemoteCloseShift.Visible = btnRemoteOpenShift.Visible = true;
                lblUserName.Visible = lblLogOutTime.Visible = lblLoginTime.Visible = txtlb_user.Visible = txtlb_logindate.Visible = txttb_logoutdate.Visible = btnRemoteCloseShift.Enabled = cb_open.Visible = cb_close.Visible == false;
                ClearDataSource();

                SetUpRemoteShiftUI();
                if (ParafaitEnv.Manager_Flag != "Y" && ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ENABLE_BLIND_CLOSE_SHIFT") == "Y")
                {
                    btnRemoteCloseShift.Visible = btnRemoteCloseShift.Enabled = true;
                    rbRemoteClose.Visible = false;
                    isCloseBlindShifts = true;
                    rbBlindClose.Checked = true;
                }
                else if (ParafaitEnv.Manager_Flag == "N")
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4045)); //'Provisional shift close is disabled'
                    this.Close();
                    return;
                }
            }
            else
            {
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_PRINT_IN_SHIFT_OPEN_CLOSE") == "N")
                    lnkPrint.Visible = false;
                cb_open.Visible = cb_close.Visible = true;
                if (!SetupScreen())
                    Environment.Exit(-1);
            }
            cb_EOD.Enabled = false;
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_END_OF_DAY_ON_CLOSE_SHIFT").Equals("Y")
                && shiftType != ShiftDTO.ShiftActionType.Open.ToString())
            {
                if (shiftType != ShiftDTO.ShiftActionType.ROpen.ToString())
                    cb_EOD.Enabled = true;
                else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_REMOTE_EOD").Equals("Y"))
                    cb_EOD.Enabled = true;
            }
            string exeDir = Path.GetDirectoryName(Environment.CommandLine.Replace("\"", ""));
            shiftLogo.Image = (File.Exists(exeDir + "\\Resources\\ClientLogo.png") ? Image.FromFile(exeDir + "\\Resources\\ClientLogo.png") : Properties.Resources.logo);
            log.LogMethodExit();
        }

        private void SetUpMultipleBlindClosedUI()
        {
            log.LogMethodEntry();
            SetUpDGVProperties();
            dgvCloseShift.DataSource = null;
            List<POSMachineDTO> tempPOSMachineDTOList = new List<POSMachineDTO>();
            if (pOSMachineDTOList == null || pOSMachineDTOList.Count == 0)
            {
                log.Error(MessageContainerList.GetMessage(executionContext, 2048) + "pOSMachineDTOList : " + pOSMachineDTOList.Count);
                txtMessage.Text = (MessageContainerList.GetMessage(executionContext, 4236));
            }
            else if (rbBlindClose.Checked)
            {
                int selectedPOSCounter = Convert.ToInt32(cmbPOSCounter.SelectedValue);
                if (selectedPOSCounter != -1)
                    tempPOSMachineDTOList = pOSMachineDTOList.Where(x => x.POSTypeId == selectedPOSCounter).ToList();
                else
                    tempPOSMachineDTOList = pOSMachineDTOList;

                List<RemoteShiftAssignment> blindClosedShiftDTOList = new List<RemoteShiftAssignment>();
                grpBoxOpenShift.Enabled = false;
                int noOfDaysAllowedForBlindClose = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_BLIND_CLOSE_WITHIN_X_DAYS"));
                ShiftListBL shiftListBL = new ShiftListBL(executionContext);
                List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                if (isCloseBlindShifts)
                    searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_USERNAME, ParafaitEnv.Username));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_USERTYPE, "POS"));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "desc"));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.TIMESTAMP, (ServerDateTime.Now.AddDays(-noOfDaysAllowedForBlindClose).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
                searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION, ShiftDTO.ShiftActionType.PClose.ToString()));
                List<ShiftDTO> shiftListDTO = shiftListBL.GetShiftDTOList(searchParams, true,false,null,true);
                if (shiftListDTO != null && shiftListDTO.Any())
                {
                    for (int i = 0; i < shiftListDTO.Count; i++)
                    {
                        if (tempPOSMachineDTOList.Exists(x => x.POSName == shiftListDTO[i].POSMachine))
                        {
                            RemoteShiftAssignment blindClosedShiftDTO = new RemoteShiftAssignment();
                            blindClosedShiftDTO.ShiftId = shiftListDTO[i].ShiftKey;
                            blindClosedShiftDTO.ShiftDate = shiftListDTO[i].ShiftTime;
                            blindClosedShiftDTO.UserId = usersDTOList != null ? usersDTOList.Find(x => x.LoginId == shiftListDTO[i].ShiftLoginId).UserId : -1;
                            blindClosedShiftDTO.MachineId = pOSMachineDTOList != null ? pOSMachineDTOList.Find(x => x.POSName == shiftListDTO[i].POSMachine).POSMachineId : -1;
                            blindClosedShiftDTO.ShiftEndTime = (shiftListDTO[i].ShiftLogDTOList != null && shiftListDTO[i].ShiftLogDTOList.Exists(x => x.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString()))
                                                                    ? shiftListDTO[i].ShiftLogDTOList.Find(x => x.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString()).ShiftTime : ServerDateTime.Now;
                            blindClosedShiftDTOList.Add(blindClosedShiftDTO);
                        }
                    }
                    if (cmbPOSCounter.SelectedValue != null)
                        selectedPOSCounter = Convert.ToInt32(cmbPOSCounter.SelectedValue);
                    if (selectedPOSCounter != -1)
                        cbCloseShiftDGVPOSMachine.DataSource = pOSMachineDTOList.Where(x => x.POSTypeId == selectedPOSCounter).ToList();
                    else
                        cbCloseShiftDGVPOSMachine.DataSource = pOSMachineDTOList;

                    cbCloseShiftDGVPOSMachine.ValueMember = "posMachineId";
                    cbCloseShiftDGVPOSMachine.DisplayMember = "posName";
                    cbCloseShiftDGVUser.DataSource = usersDTOList;
                    cbCloseShiftDGVUser.ValueMember = "UserId";
                    cbCloseShiftDGVUser.DisplayMember = "UserName";
                    dgvCloseShift.DataSource = blindClosedShiftDTOList;
                }
            }
            log.LogMethodExit();
        }

        private void SetUpDGVProperties()
        {
            log.LogMethodEntry();
            Utilities.setupDataGridProperties(ref dgvOpenShift);
            Utilities.setupDataGridProperties(ref dgvCloseShift);
            dgvOpenShift.ColumnHeadersHeight = 32;
            dgvCloseShift.ColumnHeadersHeight = 32;
            dgvOpenShift.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCloseShift.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCloseShift.RowHeadersVisible = false;
            log.LogMethodExit();
        }

        private void SetUpRemoteShiftUI()
        {
            log.LogMethodEntry();
            try
            {
                SetUpDGVProperties();
                if (rbRemoteClose.Checked)
                {
                    POSMachineList pOSMachineList = new POSMachineList(executionContext);
                    OpenShiftDTOList = pOSMachineList.GetOpenShiftDTOList(pOSMachineDTOList);
                    Dictionary<int, string> openShifts = new Dictionary<int, string>();
                    List<UsersDTO> openShiftDGVUsersDTOList = new List<UsersDTO>();
                    List<POSMachineDTO> tempPOSMachineDTOList;
                    List<POSMachineDTO> unavailablePOSMachineDTOList = new List<POSMachineDTO>();
                    List<POSMachineDTO> availablePOSMachineDTOList = new List<POSMachineDTO>();
                    if (usersDTOList != null)
                    {
                        usersDTOList = usersDTOList.Where(x => x.UserId > 0).ToList();
                    }
                    int selectedPOSCounter = Convert.ToInt32(cmbPOSCounter.SelectedValue);
                    if (selectedPOSCounter != -1)
                        tempPOSMachineDTOList = pOSMachineDTOList.Where(x => x.POSTypeId == selectedPOSCounter).ToList();
                    else
                        tempPOSMachineDTOList = pOSMachineDTOList;
                    if (OpenShiftDTOList != null)
                    {
                        foreach (ShiftDTO shiftDTO in OpenShiftDTOList)
                        {
                            if (!openShifts.ContainsKey(shiftDTO.ShiftKey))
                                openShifts.Add(shiftDTO.ShiftKey, shiftDTO.POSMachine);
                        }
                        unavailablePOSMachineDTOList = tempPOSMachineDTOList.Where(x => OpenShiftDTOList.Any(y => y.POSMachine.Equals(x.POSName))).ToList();
                        availablePOSMachineDTOList = tempPOSMachineDTOList.Where(x => !OpenShiftDTOList.Any(y => y.POSMachine.Equals(x.POSName))).ToList();
                    }
                    else
                    {
                        availablePOSMachineDTOList = tempPOSMachineDTOList;
                    }
                    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_CONCURRENT_USER_LOGIN") == "N")
                    {
                        // only unassigned users and managers should be show up in user dropdown
                        List<UsersDTO> tempMgrDTOList = usersDTOList.Where(x => managerRole.Any(y => y.RoleId == x.RoleId)).ToList();
                        List<UsersDTO> assignedUsers = usersDTOList.Where(x => OpenShiftDTOList.Any(y => y.ShiftLoginId.Equals(x.LoginId))).ToList();
                        openShiftDGVUsersDTOList = usersDTOList.Where(x => !assignedUsers.Any(y => y.LoginId.Equals(x.LoginId))).ToList();
                        openShiftDGVUsersDTOList.AddRange(assignedUsers.Where(x => tempMgrDTOList.Any(y => y.RoleId == x.RoleId)));
                    }
                    else
                    {
                        openShiftDGVUsersDTOList = usersDTOList;
                    }
                    openShiftDGVUsersDTOList = openShiftDGVUsersDTOList.OrderBy(x => x.UserName).ToList();
                    UsersDTO defaultUser = new UsersDTO();
                    defaultUser.UserName = "- SELECT -";
                    openShiftDGVUsersDTOList.Insert(0, defaultUser);

                    UsersDTO defaultUser1 = new UsersDTO();
                    defaultUser1.UserName = " ";
                    usersDTOList.Insert(0, defaultUser1);
                    List<RemoteShiftAssignment> remoteOpenShiftDGVDTOList = new List<RemoteShiftAssignment>();
                    List<RemoteShiftAssignment> remoteCloseShifDGVDTOtList = new List<RemoteShiftAssignment>();
                    for (int i = 0; i < availablePOSMachineDTOList.Count; i++)
                    {
                        RemoteShiftAssignment openShiftDTO = new RemoteShiftAssignment();
                        openShiftDTO.MachineId = availablePOSMachineDTOList[i].POSMachineId;
                        remoteOpenShiftDGVDTOList.Add(openShiftDTO);
                    }
                    dgvOpenShift.DataSource = remoteOpenShiftDGVDTOList;
                    cbOpenShiftDGVUser.DisplayMember = "UserName";
                    cbOpenShiftDGVUser.ValueMember = "UserId";
                    cbOpenShiftDGVUser.DataSource = openShiftDGVUsersDTOList;
                    cbOpenShiftDGVPOSMachine.DataSource = availablePOSMachineDTOList;
                    cbOpenShiftDGVPOSMachine.DisplayMember = "posName";
                    cbOpenShiftDGVPOSMachine.ValueMember = "posMachineId";

                    cbCloseShiftDGVPOSMachine.DataSource = pOSMachineDTOList;
                    cbCloseShiftDGVPOSMachine.ValueMember = "posMachineId";
                    cbCloseShiftDGVPOSMachine.DisplayMember = "posName";
                    cbCloseShiftDGVUser.DataSource = usersDTOList;
                    cbCloseShiftDGVUser.ValueMember = "UserId";
                    cbCloseShiftDGVUser.DisplayMember = "UserName";

                    for (int i = 0; i < unavailablePOSMachineDTOList.Count; i++)
                    {
                        //int shiftKey = -1;
                        //openShifts.TryGetValue(out shiftKey, unavailablePOSMachineDTOList[i].POSName );
                        List<ShiftDTO> posSpecificShiftDTOList = OpenShiftDTOList.FindAll(x => x.POSMachine == unavailablePOSMachineDTOList[i].POSName);
                        if (posSpecificShiftDTOList != null && posSpecificShiftDTOList.Any())
                        {
                            foreach (ShiftDTO shiftDTO in posSpecificShiftDTOList)
                            {
                                List<ShiftDTO> shiftDTOs = OpenShiftDTOList.Where(x => x.ShiftKey == shiftDTO.ShiftKey).ToList();
                                List<UsersDTO> usersDTOs = usersDTOList.Where(x => x.LoginId == shiftDTOs[0].ShiftLoginId).ToList();

                                if (shiftDTOs != null && shiftDTOs.Count > 0)
                                {
                                    RemoteShiftAssignment closeShiftDTO = new RemoteShiftAssignment();
                            closeShiftDTO.MachineId = unavailablePOSMachineDTOList[i].POSMachineId;
                            closeShiftDTO.UserId = (usersDTOs != null && usersDTOs.Count > 0) ? usersDTOs[0].UserId : -1;
                            closeShiftDTO.ShiftId = shiftDTOs[0].ShiftKey;
                            closeShiftDTO.ShiftDate = shiftDTOs[0].ShiftTime;
                            closeShiftDTO.ShiftEndTime = ServerDateTime.Now;
                            remoteCloseShifDGVDTOtList.Add(closeShiftDTO);
                        }
                    }
                        }
                    }
                    dgvCloseShift.DataSource = remoteCloseShifDGVDTOtList;
                    cbCloseShiftDGVPOSMachine.DataSource = unavailablePOSMachineDTOList;
                    cbCloseShiftDGVPOSMachine.ValueMember = "posMachineId";
                    cbCloseShiftDGVPOSMachine.DisplayMember = "posName";
                }
            }
            catch (Exception ex)
            {
                log.Error("Error in setup remote UI." + ex.Message);
                POSUtils.ParafaitMessageBox("Error in setup remote UI." + ex.Message);
            }
            log.LogMethodExit();
        }

        private bool SetupScreen()
        {
            log.LogMethodEntry();
            txtUserCreditCardAmount.BackColor = txtUserChequeAmount.BackColor = txtUserCouponAmount.BackColor = txtUserTicketNumber.BackColor
                = txtRemarks.BackColor = System.Drawing.ColorTranslator.FromHtml("#cafcd8");
            //if (POSUtils.GetOpenShiftId(ParafaitEnv.LoginID) != -1 && POSUtils.OpenShiftListDTOList != null) // previous shift was not closed
            //{
            //    shiftDTO = POSUtils.OpenShiftListDTOList.Find(x => x.ShiftKey == POSUtils.GetOpenShiftId(ParafaitEnv.LoginID));
            //    txtlb_user.Text = shiftDTO.ShiftUserName;
            //}
            bool buildSystemNumber = false;
            if (shiftType == ShiftDTO.ShiftActionType.Close.ToString())
            {
                buildSystemNumber = true;
            }
            ShiftListBL shiftListBL = new ShiftListBL(executionContext);
            List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParameters = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
            searchParameters.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            //searchParameters.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.POS_MACHINE, ParafaitEnv.POSMachine));
            searchParameters.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_LOGIN_ID, ParafaitEnv.LoginID));
            searchParameters.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "desc"));
            searchParameters.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.TIMESTAMP, (ServerDateTime.Now.AddDays(-7).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
            List<ShiftDTO> shiftDTOList = shiftListBL.GetShiftDTOList(searchParameters, true, true,null, buildSystemNumber);
            if (shiftDTOList != null && shiftDTOList.Count > 0)
            {
                try
                {
                    shiftDTOList = shiftDTOList.FindAll(x => x.POSMachine == pOSMachines.POSMachineDTO.POSName).OrderByDescending(x => x.ShiftTime).ToList();
                    shiftDTO = shiftDTOList[0];
                    txtlb_user.Text = shiftDTO.ShiftUserName;
                }
                catch(Exception ex)
                {
                    log.Error(ex.Message);
                }
            }

            rb_redemption.Enabled = false;
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_REDEMPTION_IN_POS") == "N")
                txtUserTicketNumber.Enabled = false;
            rb_pos.Enabled = txtUserCashAmount.Enabled = true;
            txtUserCashAmount.Focus();
            this.ActiveControl = txtUserCashAmount;
            if (shiftApplication == "Redemption")
            {
                rb_pos.Enabled = txtUserCashAmount.Enabled = txtUserCardCount.Enabled = false;
                rb_redemption.Enabled = rb_redemption.Checked = txtUserTicketNumber.Enabled = true;
                txtUserTicketNumber.Focus();
                this.ActiveControl = txtUserTicketNumber;
            }
            if (shiftType == ShiftDTO.ShiftActionType.Open.ToString())
            {
                if (shiftDTO != null)
                {
                    if (shiftDTO.ShiftAction == ShiftDTO.ShiftActionType.Open.ToString())
                    {
                        txtlb_logindate.Text = shiftDTO.ShiftTime.ToString();
                        if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 149, ParafaitEnv.Username, shiftDTO.POSMachine), "Shift Open - Simultaneous Session",
                         MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_CONCURRENT_USER_LOGIN") == "N")
                            {
                                if (ParafaitEnv.Manager_Flag != "Y")
                                {
                                    int managerId = -1;
                                    if (!Authenticate.Manager(ref managerId))
                                    {
                                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 150), MessageContainerList.GetMessage(executionContext, 151));
                                        return false;
                                    }
                                }
                            }
                            if (shiftDTO.ShiftUserName == ParafaitEnv.Username)
                            {
                                SetUIElementsOnOpenShift();
                                SetRemotePanelSizeAndLocation();
                                POSUtils.OpenShiftUserName = shiftDTO.ShiftUserName;
                                SetSystemNumbers(false);
                                txtlb_logindate.Text = shiftDTO.ShiftTime.ToString();
                                ShowPOSShiftCollections();
                                if (shiftDTO.POSMachine == ParafaitEnv.POSMachine)
                                {
                                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 152, shiftDTO.ShiftUserName), "Shift Open");
                                }
                                this.Activate();
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        int noOfDaysAllowedForBlindClose = Convert.ToInt32(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_BLIND_CLOSE_WITHIN_X_DAYS", -1));
                        List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>> searchParams = new List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>();
                        if (shiftType == ShiftDTO.ShiftActionType.Open.ToString())
                            searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_USERNAME, ParafaitEnv.Username));
                        searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_USERTYPE, "POS"));
                        searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.ORDER_BY_TIMESTAMP, "desc"));
                        searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.SHIFT_ACTION, ShiftDTO.ShiftActionType.PClose.ToString()));
                        searchParams.Add(new KeyValuePair<ShiftDTO.SearchByShiftParameters, string>(ShiftDTO.SearchByShiftParameters.TIMESTAMP, (ServerDateTime.Now.AddDays(-noOfDaysAllowedForBlindClose).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))));
                        List<ShiftDTO> shiftListDTO = shiftListBL.GetShiftDTOList(searchParams, true,false,null,true);
                        if (shiftListDTO != null && shiftListDTO.Exists(x => x.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString()))
                        {
                            rbRemoteClose.Visible = false;
                            shiftDTO = shiftListDTO.Find(x => x.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString());
                            if (shiftDTO != null)
                            {
                                txttb_logoutdate.Text = shiftDTO.ShiftLogDTOList.Find(x => x.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString()).ShiftTime.ToString();
                                string posMachine = shiftListDTO.FindAll(x => x.ShiftAction == ShiftDTO.ShiftActionType.PClose.ToString()).ToList().FirstOrDefault().POSMachine;
                                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4012, posMachine) //'You have a Blind Closed shift on POSMachine &1.Do you wish to close it?'
                                      , MessageContainerList.GetMessage(executionContext, "Shift Blind Close"),
                                    MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                {
                                    isCloseBlindShifts = cb_close.Visible = btnRemoteCloseShift.Visible = btnRemoteOpenShift.Visible = rbBlindClose.Checked = grpBoxCloseShift.Visible = true;
                                    btnRemoteCloseShift.Visible = false;
                                    SetUpMultipleBlindClosedUI();
                                    return true;
                                }
                                else if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_NEW_SHIFT_OPEN_AFTER_BLIND_CLOSE") == "Y")
                                {
                                    if (shiftDTO.ShiftAction != ShiftDTO.ShiftActionType.Open.ToString() &&
                                        POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4013), "Shift Open", //Do you want to open a new shift?
                                                MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        txtlb_logindate.Text = ServerDateTime.Now.ToString();
                                        txttb_logoutdate.Text = string.Empty;
                                    }
                                    else
                                    {
                                        this.DialogResult = DialogResult.Cancel;
                                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FULL_SCREEN_POS").Equals("N"))
                                        {
                                            Environment.Exit(0);
                                        }
                                        return true;
                                    }
                                }
                                else
                                {
                                    this.DialogResult = DialogResult.Cancel;
                                    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FULL_SCREEN_POS").Equals("N"))
                                    {
                                        Environment.Exit(0);
                                    }
                                    return true;
                                }
                            }
                        }
                    }
                }
                SetUIElementsOnOpenShift();
            }
            else if (shiftType == ShiftDTO.ShiftActionType.Close.ToString())
            {
                SetUIElementsOnCloseShift(ServerDateTime.Now);
            }
            else
            {
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 153), "Error opening Shift window");
                return false;
            }
            SetRemotePanelSizeAndLocation();
            log.LogMethodExit(true);
            return true;
        }

        private void ShowPOSShiftCollections()
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SHOW_POS_SHIFT_COLLECTION") == "N")
            {
                txtSystemTicketNumber.PasswordChar = txtSystemCouponAmount.PasswordChar = txtSystemChequeAmount.PasswordChar = txtSystemGameCardAmount.PasswordChar =
                txtSystemCreditCardAmount.PasswordChar = txtSystemCashAmount.PasswordChar = txtSystemCardCount.PasswordChar = '-';
                lblNetTickets.ForeColor = lblNetTickets.BackColor = lblNetCoupon.ForeColor = lblNetCoupon.BackColor = lblNetCheque.ForeColor = lblNetCheque.BackColor =
                lblNetGameCard.ForeColor = lblNetGameCard.BackColor = lblNetCreditCard.ForeColor = lblNetCreditCard.BackColor = lblNetCash.ForeColor = lblNetCash.BackColor =
                lblNetCards.ForeColor = lblNetCards.BackColor = this.BackColor;
            }
            log.LogMethodExit();
        }

        private void SetRemotePanelSizeAndLocation()
        {
            log.LogMethodEntry();
            panelRemoteAccess.Visible = txtMessage.Visible = false;
            //this.Location = new Point(436, 12);
            //this.Location = new Point(436, 12);
            this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width + 40 - this.Width),
                         (Screen.PrimaryScreen.WorkingArea.Height + 40 - this.Height));
            panelShiftCollection.Location = new Point(12, 12);
            this.Size = new Size(478, 610);
            log.LogMethodExit();
        }

        private void SetUIElementsOnOpenShift()
        {
            log.LogMethodEntry();
            ClearFields();
            gb_header.Text = MessageContainerList.GetMessage(executionContext, "Open Shift");
            cb_open.Visible = cb_open.Enabled = true;
            btnBlindClose.Visible = cb_close.Enabled = cb_EOD.Enabled = false;
            txtlb_logindate.Text = ServerDateTime.Now.ToString();
            txtlb_user.Text = ParafaitEnv.Username;
            btnRemoteOpenShift.Visible = false;
            log.LogMethodExit();
        }

        private void SetUIElementsOnCloseShift(DateTime shiftEndTime)
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_BLIND_CLOSE_SHIFT") == "Y")
                btnBlindClose.Visible = btnBlindClose.Enabled = true;
            cb_close.Enabled = true;
            txttb_logoutdate.Text = ServerDateTime.Now.ToString();
            gb_header.Text = MessageContainerList.GetMessage(executionContext, "Close Shift");
            cb_open.Enabled = false;
            txtUserGameCardAmount.BackColor = txtUserCreditCardAmount.BackColor;
            ShowPOSShiftCollections();
            txtlb_user.Text = ParafaitEnv.Username;
            txtlb_logindate.Text = shiftDTO.ShiftTime.ToString();
            SetSystemNumbers();
            log.LogMethodExit();
        }

        private void SetSystemNumbers(bool isCloseShift = true)
        {
            log.LogMethodEntry(isCloseShift);
            if (shiftDTO == null)
                return;
            txtSystemCashAmount.Text = (Convert.ToDecimal(shiftDTO.ActualAmount) + Convert.ToDecimal(shiftDTO.ShiftAmount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
            txtSystemCreditCardAmount.Text = (Convert.ToDecimal(shiftDTO.ActualCreditCardamount) + Convert.ToDecimal(shiftDTO.CreditCardamount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
            txtSystemGameCardAmount.Text = (Convert.ToDecimal(shiftDTO.ActualGameCardamount) + Convert.ToDecimal(shiftDTO.GameCardamount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
            txtSystemChequeAmount.Text = (Convert.ToDecimal(shiftDTO.ActualChequeAmount) + Convert.ToDecimal(shiftDTO.ChequeAmount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
            txtSystemCouponAmount.Text = (Convert.ToDecimal(shiftDTO.ActualCouponAmount) + Convert.ToDecimal(shiftDTO.CouponAmount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
            txtSystemTicketNumber.Text = Convert.ToInt32(shiftDTO.ShiftTicketNumber).ToString(ParafaitEnv.NUMBER_FORMAT);
            if (isCloseShift)
            {
                log.Debug("shiftType is Close");
                lblNetCash.Text = Convert.ToDecimal(shiftDTO.ActualAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                lblNetCreditCard.Text = Convert.ToDecimal(shiftDTO.ActualCreditCardamount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                lblNetGameCard.Text = Convert.ToDecimal(shiftDTO.ActualGameCardamount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                lblNetCheque.Text = Convert.ToDecimal(shiftDTO.ActualChequeAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                lblNetCoupon.Text = Convert.ToDecimal(shiftDTO.ActualCouponAmount).ToString(ParafaitEnv.AMOUNT_FORMAT);
                lblNetTickets.Text = Convert.ToInt32(shiftDTO.ShiftTicketNumber).ToString(ParafaitEnv.NUMBER_FORMAT);
                lblNetCards.Text = shiftDTO.ActualCards.ToString();
                txtSystemCardCount.Text = (Convert.ToInt32(shiftDTO.CardCount) + Convert.ToInt32(shiftDTO.ActualCards)).ToString(ParafaitEnv.NUMBER_FORMAT);
            }
            else
            {
                txtUserCashAmount.Text = (Convert.ToDecimal(shiftDTO.ShiftAmount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
                txtUserCreditCardAmount.Text = (Convert.ToDecimal(shiftDTO.CreditCardamount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
                txtUserGameCardAmount.Text = (Convert.ToDecimal(shiftDTO.GameCardamount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
                txtUserChequeAmount.Text = (Convert.ToDecimal(shiftDTO.ChequeAmount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
                txtUserCouponAmount.Text = (Convert.ToDecimal(shiftDTO.CouponAmount)).ToString(ParafaitEnv.AMOUNT_FORMAT);
                txtUserTicketNumber.Text = Convert.ToInt32(shiftDTO.ShiftTicketNumber).ToString(ParafaitEnv.NUMBER_FORMAT);
                txtSystemCardCount.Text = (Convert.ToInt32(shiftDTO.CardCount) + Convert.ToInt32(shiftDTO.ActualCards)).ToString(ParafaitEnv.NUMBER_FORMAT);
                txtUserCardCount.Text = shiftDTO.CardCount.ToString();
            }
            log.LogMethodExit();
        }

        private void cb_open_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    dbTrx.BeginTransaction();
                    OpenShift(txtlb_user.Text, ParafaitEnv.LoginID, ParafaitEnv.User_Id, dbTrx.SQLTrx);
                    dbTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    dbTrx.RollBack();
                    DialogResult = DialogResult.Cancel;
                    log.Error(ex.Message);
                    POSUtils.ParafaitMessageBox(ex.Message);
                    return;
                }
            }
            if (shiftOpen && shiftType != ShiftDTO.ShiftActionType.ROpen.ToString())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            log.LogMethodExit();
        }

        private void OpenShift(string userName, string LoginId, int userId, SqlTransaction dbTrx, bool isRemotOpenShift = false)
        {
            log.LogMethodEntry(userName, LoginId);
            decimal openingAmount = 0;
            int cardCount = 0;
            DateTime loginTime;//= !string.IsNullOrEmpty(txtlb_logindate.Text) ? Convert.ToDateTime(txtlb_logindate.Text) : ServerDateTime.Now;
            if (!string.IsNullOrEmpty(txtlb_logindate.Text))
            {
                loginTime = DateTime.Parse(txtlb_logindate.Text.ToString());
            }
            else
            {
                loginTime = ServerDateTime.Now;
            }
            if (shiftApplication == "POS")
            {
                if (string.IsNullOrEmpty(txtUserCashAmount.Text)) { SetUpTextBoxFocusOnError_OpenShift(txtUserCashAmount, 154, false); return; }
                try { openingAmount = Convert.ToDecimal(txtUserCashAmount.Text); }
                catch { SetUpTextBoxFocusOnError_OpenShift(txtUserCashAmount, 155); return; }
                if (string.IsNullOrEmpty(txtUserCardCount.Text)) { SetUpTextBoxFocusOnError_OpenShift(txtUserCardCount, 156, false); return; }
                try { cardCount = Convert.ToInt32(double.Parse(txtUserCardCount.Text)); }
                catch { SetUpTextBoxFocusOnError_OpenShift(txtUserCardCount, 157); return; }
            }
            else
            {
                if (txtUserTicketNumber.Text == string.Empty) { SetUpTextBoxFocusOnError_OpenShift(txtUserTicketNumber, 158, false); return; }
            }
            double shiftTicketNumber = !string.IsNullOrEmpty(txtUserTicketNumber.Text) ? Convert.ToDouble(txtUserTicketNumber.Text) : 0;
            double creditCardAmount = !string.IsNullOrEmpty(txtUserCreditCardAmount.Text) ? Convert.ToDouble(txtUserCreditCardAmount.Text) : 0;
            double ChequeAmount = !string.IsNullOrEmpty(txtUserChequeAmount.Text) ? Convert.ToDouble(txtUserChequeAmount.Text) : 0;
            double CouponAmount = !string.IsNullOrEmpty(txtUserCouponAmount.Text) ? Convert.ToDouble(txtUserCouponAmount.Text) : 0;
            shiftDTO = pOSMachines.CreateNewShift(userName, loginTime, shiftApplication, Convert.ToDouble(openingAmount), cardCount, Convert.ToInt32(shiftTicketNumber),
                                            txtRemarks.Text, LoginId, creditCardAmount, ChequeAmount, CouponAmount, ParafaitEnv.ApprovalTime, userId, isRemotOpenShift, dbTrx);
            shiftOpen = true;
            POSUtils.OpenShiftUserName = shiftDTO.ShiftUserName;
            FisalizationOnOpenShift();
            OpenCashDrawer(shiftDTO.ShiftKey, LoginId, isRemotOpenShift);
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_PRINT_SHIFT_OPEN_RECEIPT") == "Y")
            {
                printShiftReceipt();
            }
            log.LogMethodExit();
        }

        private void FisalizationOnOpenShift()
        {
            log.LogMethodEntry();
            ShiftLogDTO shiftLogDTOFiscal = shiftDTO.ShiftLogDTOList.OrderByDescending(x => x.ShiftLogId).FirstOrDefault();
            FiscalPrinter fiscalPrinter = GetFIscalPrinterInstance();
            FiscalPrinterFactory.GetInstance().Initialize(Utilities);
            string fiscalPrinterType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER");
            if (string.IsNullOrEmpty(fiscalPrinterType) == false)
                fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(fiscalPrinterType);
            if (fiscalPrinterType.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
            {
                string signature = string.Empty;
                FiscalizationBL fiscalizationHelper = new FiscalizationBL(executionContext);
                FiscalizationRequest fiscalizationRequest = fiscalizationHelper.BuildShiftFiscalizationRequest(shiftLogDTOFiscal);
                bool isSuccess = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref signature);
                if (isSuccess)
                {
                    fiscalizationHelper.UpdateShiftPaymentReference(fiscalizationRequest, signature, null);
                    fiscalSignature = signature;
                }
            }
            log.LogMethodExit();
        }

        private void SetUpTextBoxFocusOnError_OpenShift(TextBox textBox, int messageNumber, bool selectAll = true)
        {
            log.LogMethodEntry(textBox, messageNumber);
            textBox.Focus();
            this.ActiveControl = textBox;
            if (selectAll)
                textBox.SelectAll();
            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, messageNumber), MessageContainerList.GetMessage(executionContext, "Open Shift"));
            shiftOpen = false;
            log.LogMethodExit();
        }

        private void SetUpTextBoxFocusOnError_CloseShift(TextBox textBox, int messageNumber, bool selectAll = true)
        {
            log.LogMethodEntry(textBox, messageNumber);
            textBox.Focus();
            this.ActiveControl = textBox;
            if (selectAll)
                textBox.SelectAll();
            POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, messageNumber), MessageContainerList.GetMessage(executionContext, "Close Shift"));
            shiftClose = false;
        }

        private void cb_close_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string loginID = ParafaitEnv.LoginID;
            int userId = ParafaitEnv.User_Id;
            if (isEODOperation && selectedUsersDTO != null && selectedUsersDTO.Any())
            {
                 loginID = selectedUsersDTO[0].LoginId;
                 userId = selectedUsersDTO[0].UserId;
            }
            CloseShift(txtlb_user.Text, loginID, userId);
            if (isCloseShiftSuccess)
            {
                POSUtils.OpenShiftUserName = string.Empty;
                if (isCloseBlindShifts)
                {
                    if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 4013), "Shift Open", //"Do you want to open a new shift?"
                                MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SetUIElementsOnOpenShift();
                        SetRemotePanelSizeAndLocation();
                        txtlb_logindate.Text = ServerDateTime.Now.ToString();
                        return;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.Cancel;
                        if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FULL_SCREEN_POS").Equals("N"))
                        {
                            Environment.Exit(0);
                        }
                        return;
                    }
                }
                if (shiftType != ShiftDTO.ShiftActionType.ROpen.ToString())
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            log.LogMethodExit();
        }

        private void CloseShift(string UserName, string LoginId, int userId, int remoteShiftCloseId = -1)
        {
            log.LogMethodEntry(UserName, LoginId);
            isCloseShiftSuccess = false;
            if (POSUtils.IsPendingPaymentExists(LoginId))
            {
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1127), "Payment Gateway Unsettled Transaction", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    return;
            }
            int cardCount = 0;
            if (shiftApplication == "POS")
            {
                decimal closingAmount;
                if (string.IsNullOrEmpty(txtUserCashAmount.Text)) { SetUpTextBoxFocusOnError_CloseShift(txtUserCashAmount, 159, false); return; }
                try { closingAmount = Convert.ToDecimal(txtUserCashAmount.Text); }
                catch { SetUpTextBoxFocusOnError_CloseShift(txtUserCashAmount, 160); return; }
                if (string.IsNullOrEmpty(txtUserCardCount.Text)) { SetUpTextBoxFocusOnError_CloseShift(txtUserCardCount, 156); return; }
                try { cardCount = Convert.ToInt32(txtUserCardCount.Text); }
                catch { SetUpTextBoxFocusOnError_CloseShift(txtUserCardCount, 157); return; }
            }
            else //"Redemption":
            {
                if (string.IsNullOrEmpty(txtUserTicketNumber.Text)) { SetUpTextBoxFocusOnError_CloseShift(txtUserTicketNumber, 163, false); return; }
            }
            double actualAmount = 0, actualCards = 0, actualTickets = 0, actualcreditCardAmount = 0, actualGameCardAmount = 0, actualChequeAmount = 0, actualCouponAmount = 0;
            double shift_ticketnumber = 0, creditCardAmount = 0, gameCardAmount = 0, chequeAmount = 0, couponAmount = 0, shiftAmount = 0;
            actualAmount = lblNetCash.Text == string.Empty ? 0 : Convert.ToDouble(lblNetCash.Text);
            actualCards = lblNetCards.Text == string.Empty ? 0 : Convert.ToDouble(lblNetCards.Text);
            actualcreditCardAmount = lblNetCreditCard.Text == string.Empty ? 0 : Convert.ToDouble(lblNetCreditCard.Text);
            actualGameCardAmount = lblNetGameCard.Text == string.Empty ? 0 : Convert.ToDouble(lblNetGameCard.Text);
            actualChequeAmount = lblNetCheque.Text == string.Empty ? 0 : Convert.ToDouble(lblNetCheque.Text);
            actualCouponAmount = lblNetCoupon.Text == string.Empty ? 0 : Convert.ToDouble(lblNetCoupon.Text);
            shift_ticketnumber = txtUserTicketNumber.Text == string.Empty ? 0 : Convert.ToDouble(txtUserTicketNumber.Text);
            creditCardAmount = txtUserCreditCardAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtUserCreditCardAmount.Text);
            gameCardAmount = txtUserGameCardAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtUserGameCardAmount.Text);
            chequeAmount = txtUserChequeAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtUserChequeAmount.Text);
            couponAmount = txtUserCouponAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtUserCouponAmount.Text);
            shiftAmount = txtUserCashAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtUserCashAmount.Text);
            int shiftId = remoteShiftCloseId != -1 ? remoteShiftCloseId : shiftDTO.ShiftKey;
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    dbTrx.BeginTransaction();
                    DateTime shiftTime = DateTime.Parse(txtlb_logindate.Text.ToString());
                    pOSMachines.CloseShift(shiftId, userId, shiftTime, LoginId, Convert.ToDouble(actualAmount), cardCount, Convert.ToInt32(actualTickets), txtRemarks.Text,
                                                    Convert.ToDecimal(shiftAmount), shiftApplication, Convert.ToInt32(actualCards), Convert.ToDecimal(couponAmount), Convert.ToDecimal(creditCardAmount), Convert.ToDecimal(chequeAmount),
                                                    Convert.ToDecimal(actualGameCardAmount), Convert.ToDecimal(actualcreditCardAmount), Convert.ToDecimal(actualChequeAmount), Convert.ToDecimal(actualCouponAmount),
                                                    Convert.ToDecimal(gameCardAmount), shift_ticketnumber, dbTrx.SQLTrx);
                    dbTrx.EndTransaction();
                    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_END_OF_DAY_ON_CLOSE_SHIFT").Equals("Y")
                             && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USE_FISCAL_PRINTER") != "Y" &&
                             POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 3025), "POSX Receipt", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        pOSMachines.RunXReport(shiftTime, userId);
                    }

                    FiscalizationOnCloseShift(shiftId);
                    OpenCashDrawer(shiftId, LoginId);
                    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_PRINT_SHIFT_CLOSE_RECEIPT") == "Y")
                    {
                        DialogResult dr = DialogResult.No;
                        while (dr == DialogResult.No)
                        {
                            if (!printShiftReceipt())
                                dr = POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 164), MessageContainerList.GetMessage(executionContext, "Close Shift"), MessageBoxButtons.YesNo);
                            else
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    dbTrx.RollBack();
                }
                isCloseShiftSuccess = true;
            }
            log.LogMethodExit();
        }

        private void FiscalizationOnCloseShift(int shiftId)
        {
            log.LogMethodEntry();
            ShiftBL shiftBL = new ShiftBL(executionContext, shiftId);
            ShiftLogDTO shiftLogDTOFiscal = shiftBL.ShiftDTO.ShiftLogDTOList.OrderByDescending(x => x.ShiftLogId).FirstOrDefault();
            FiscalPrinter fiscalPrinter = GetFIscalPrinterInstance();
            FiscalPrinterFactory.GetInstance().Initialize(Utilities);
            string fiscalPrinterType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER");
            if (string.IsNullOrEmpty(fiscalPrinterType) == false)
            {
                fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(fiscalPrinterType);
            }
            if (fiscalPrinterType.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
            {
                executionContext.SetMachineId(Utilities.ParafaitEnv.POSMachineId);
                FiscalizationBL fiscalizationHelper = new FiscalizationBL(executionContext);
                FiscalizationRequest fiscalizationRequest = fiscalizationHelper.BuildShiftFiscalizationRequest(shiftLogDTOFiscal, null);
                bool isSuccess = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref fiscalSignature);
                if (isSuccess)
                {
                    fiscalizationHelper.UpdateShiftPaymentReference(fiscalizationRequest, fiscalSignature, null);
                }
            }
            log.LogMethodExit();
        }

        private void OpenCashDrawer(int shiftKey, string loginId, bool isRemotOpenShift = false)
        {
            log.LogMethodEntry();
            if (isRemotOpenShift == false)
            {
                string cashdrawerInterfaceMode = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "CASHDRAWER_INTERFACE_MODE");
                log.Debug("cashdrawerInterfaceMode :" + cashdrawerInterfaceMode);
                bool cashdrawerMandatory = ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "CASHDRAWER_ASSIGNMENT_MANDATORY_FOR_TRX");
                log.Debug("cashdrawerMandatory :" + cashdrawerMandatory);
                POSMachineContainerDTO pOSMachineContainerDTO = POSMachineContainerList.GetPOSMachineContainerDTOOrDefault(Utilities.ExecutionContext.SiteId,
                                                                   Utilities.ParafaitEnv.POSMachine, "", -1);

                if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.NONE)
                   || cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.SINGLE))
                {
                    if (pOSMachineContainerDTO != null && pOSMachineContainerDTO.POSCashdrawerContainerDTOList != null &&
                                                pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any())
                    {
                        var posCashdrawerDTO = pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Where(x => x.IsActive == true).FirstOrDefault();
                        if (posCashdrawerDTO != null && posCashdrawerDTO.CashdrawerId > -1)
                        {
                            CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, posCashdrawerDTO.CashdrawerId);
                            cashdrawerBL.OpenCashDrawer();
                        }
                    }
                }
                else if (cashdrawerInterfaceMode == InterfaceModesConverter.ToString(CashdrawerInterfaceModes.MULTIPLE))
                {
                    if (pOSMachineContainerDTO != null && (pOSMachineContainerDTO.POSCashdrawerContainerDTOList == null ||
                                                pOSMachineContainerDTO.POSCashdrawerContainerDTOList.Any() == false))
                    {
                        log.Error("cashdrawer is not mapped to the POS");
                        MessageBox.Show(MessageContainerList.GetMessage(Utilities.ExecutionContext, 4072));
                        return;
                    }
                    POSMachines posMachine = new POSMachines(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
                    List<ShiftDTO> openShifts = posMachine.GetAllOpenShifts();
                    if (openShifts != null && openShifts.Any())
                    {
                        var shiftDTO = openShifts.Where(x => x.ShiftLoginId == loginId && x.POSMachine == Utilities.ParafaitEnv.POSMachine).FirstOrDefault();
                        if (shiftDTO != null && shiftDTO.CashdrawerId > -1)
                        {
                            CashdrawerBL cashdrawerBL = new CashdrawerBL(Utilities.ExecutionContext, shiftDTO.CashdrawerId);
                            cashdrawerBL.OpenCashDrawer();
                        }
                    }
                }

            }
            log.LogMethodExit();
        }

        private void btnCancelShift_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (shiftType == ShiftDTO.ShiftActionType.Close.ToString() && shiftType != ShiftDTO.ShiftActionType.ROpen.ToString())
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
            else if (POSStatic.REQUIRE_LOGIN_FOR_EACH_TRX == false && shiftType != ShiftDTO.ShiftActionType.ROpen.ToString() && !isCloseBlindShifts) // can go ahead without shift open in case login needed for each transaction 
            {
                this.DialogResult = DialogResult.Cancel;
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FULL_SCREEN_POS").Equals("N"))
                {
                    Environment.Exit(0);
                }
            }
            else if (shiftType == ShiftDTO.ShiftActionType.Open.ToString() && shiftType != ShiftDTO.ShiftActionType.ROpen.ToString())//Modification added on 30-Jun-2017 for issue fixing POS opens without enter shift details
            {
                this.DialogResult = DialogResult.Cancel;
                if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FULL_SCREEN_POS").Equals("N"))
                {
                    Environment.Exit(0);
                }
            }
            else
                this.DialogResult = DialogResult.Cancel;
            log.LogMethodExit();
        }

        private void txtCardCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void tb_amt_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                this.ActiveControl = txtUserCardCount;
            log.LogMethodExit();
        }

        private void txtCardCount_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                this.ActiveControl = txtUserCreditCardAmount;
            }
            log.LogMethodExit();
        }

        private void tb_ticket_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                this.ActiveControl = txtRemarks;
            log.LogMethodExit();
        }

        private void rtb_remarks_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                if (cb_open.Enabled)
                    this.ActiveControl = cb_open;
                else
                    this.ActiveControl = cb_close;
            }
            log.LogMethodExit();
        }

        private void txtUserAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry();
            if (!Char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != POSStatic.decimalChar)
            {
                e.Handled = true;
            }
            log.LogMethodExit();
        }

        private void txtUserCreditCardAmount_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
            {
                if (txtUserGameCardAmount.ReadOnly)
                    this.ActiveControl = txtUserChequeAmount;
                else
                    this.ActiveControl = txtUserGameCardAmount;
            }
            log.LogMethodExit();
        }

        private void txtUserGameCardAmount_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                this.ActiveControl = txtUserChequeAmount;
            log.LogMethodExit();
        }

        private void txtUserChequeAmount_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                this.ActiveControl = txtUserCouponAmount;
            log.LogMethodExit();
        }

        private void txtUserCouponAmount_KeyDown(object sender, KeyEventArgs e)
        {
            log.LogMethodEntry();
            if (e.KeyCode == Keys.Enter)
                if (txtUserTicketNumber.Enabled)
                    this.ActiveControl = txtUserTicketNumber;
                else
                    this.ActiveControl = txtRemarks;
            log.LogMethodExit();
        }

        private FiscalPrinter GetFIscalPrinterInstance()
        {
            log.LogMethodEntry();
            FiscalPrinter fiscalPrinter = null;
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USE_FISCAL_PRINTER") == "Y" &&
                        ((ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString())) ||
                        ((ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.FiskalTrust.ToString()))) ||
                        (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.ELTRADE.ToString()))))
            {
                string _FISCAL_PRINTER = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER");
                FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER);
                if (!fiscalPrinter.OpenPort())
                {
                    POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 193, ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER_PORT_NUMBER")), "Fiscal Printer");
                    log.Error("POS_Load() - Unable to initialize Fiscal Printer (Port: " + ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER_PORT_NUMBER") + " )) ");
                    fiscalPrinter.ClosePort();
                    Application.Exit();
                }
            }
            log.LogMethodExit(fiscalPrinter);
            return fiscalPrinter;
        }


        private void lnkPrint_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            FiscalPrinter fiscalPrinter = GetFIscalPrinterInstance();
            if (shiftType == ShiftDTO.ShiftActionType.Open.ToString())
            {
                if (fiscalPrinter != null)
                {
                    if (!fiscalPrinter.DepositeInDrawer(txtUserCashAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtUserCashAmount.Text), txtUserCashAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtUserCashAmount.Text)))
                    {
                        POSUtils.ParafaitMessageBox("Printing of the Shift Open Details failed.", MessageContainerList.GetMessage(executionContext, "Print Error"));
                    }
                    fiscalPrinter.ClosePort();
                    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                    {
                        log.Debug("VKLAD is non fiscalized . printing semnox receipt");
                        printShiftReceipt();
                    }
                }
                else 
                {
                    log.Debug("shiftType is Open and printing Non fiscal");
                    printShiftReceipt();
                }

            }
            else if (shiftType == ShiftDTO.ShiftActionType.Close.ToString())
            {
                log.Debug("shiftType is Close");

                if (fiscalPrinter != null)
                {
                    try
                    {
                        if (!fiscalPrinter.DepositeInDrawer(txtSystemCashAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtSystemCashAmount.Text), txtSystemCashAmount.Text == string.Empty ? 0 : Convert.ToDouble(txtSystemCashAmount.Text), true))
                        {
                            log.Error("DepositeInDrawer method returns false. Widrawer (VIBER) failed");
                            POSUtils.ParafaitMessageBox("Fiscal receipt printing failed.", MessageContainerList.GetMessage(executionContext, "Print Error"));
                        }
                    }
                    catch (ValidationException ex)
                    {
                        log.Error(ex.Message);
                        POSUtils.ParafaitMessageBox(ex.Message);
                    }
                    fiscalPrinter.ClosePort();

                    if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AUTO_PRINT_SHIFT_CLOSE_RECEIPT") != "Y" &&
                           ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                    {
                        printShiftReceipt();
                    }
                }
                else
                {
                    log.Debug("shiftType is Close and printing Non fiscal");
                    printShiftReceipt();
                }
            }
            else
            {
                printShiftReceipt();
            }
            log.LogMethodExit();
        }

        bool printShiftReceipt()
        {
            log.LogMethodEntry();
            if (SetupThePrinting())
            {
                try
                {
                    MyPrintDocument.Print();
                    log.LogMethodExit(true);
                    return true;
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Exception :" + ex.Message);
                    POSUtils.ParafaitMessageBox(ex.Message, MessageContainerList.GetMessage(executionContext, "Print Error"));
                    return false;
                }
            }
            else
            {
                log.LogMethodExit(false);
                return false;
            }
        }

        void MyPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            log.LogMethodEntry();
            int col1x = 0;
            int col2x = 60;
            int col3x = 140;
            int col4x = 220;
            int yLocation = 40;
            int yIncrement = 20;
            bool hideData = false;
            hideData = (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SHOW_POS_SHIFT_COLLECTION") == "N");
            string hideText = "------";
            if (shiftDTO == null)
                return;
            Font defaultFont = new System.Drawing.Font("courier narrow", 7.5f);
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "POS Shift Report") + " - " + gb_header.Text, new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "POS Name") + ": " + shiftDTO.POSMachine, new Font(defaultFont.FontFamily, 9.0F, FontStyle.Bold), Brushes.Black, 10, yLocation);
            yLocation += 30;
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Far;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Cashier") + ": " + shiftDTO.ShiftUserName, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Login Time") + ": " + shiftDTO.ShiftTime.ToString(), defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Logout Time") + ": " + txttb_logoutdate.Text, defaultFont, Brushes.Black, col1x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Cashier"), defaultFont, Brushes.Black, col2x, yLocation);
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "System"), defaultFont, Brushes.Black, col3x, yLocation);
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Net Shift"), defaultFont, Brushes.Black, col4x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Cash") + ":", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(txtUserCashAmount.Text, defaultFont, Brushes.Black, col2x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : txtSystemCashAmount.Text), defaultFont, Brushes.Black, col3x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : lblNetCash.Text), defaultFont, Brushes.Black, col4x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Card Count") + ":", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(txtUserCardCount.Text, defaultFont, Brushes.Black, col2x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : txtSystemCardCount.Text), defaultFont, Brushes.Black, col3x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : lblNetCards.Text), defaultFont, Brushes.Black, col4x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Credit Card") + ":", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(txtUserCreditCardAmount.Text, defaultFont, Brushes.Black, col2x, yLocation);
            yLocation += yIncrement;

            ShiftBL shiftBL = new ShiftBL(executionContext, shiftDTO);
            DateTime shiftEndTime = ServerDateTime.Now;
            if (!string.IsNullOrEmpty(txttb_logoutdate.Text))
                shiftEndTime = DateTime.Parse(txttb_logoutdate.Text.ToString());
            if (shiftDTO.ShiftTime == DateTime.MinValue)
                return;
            ShiftCollections[] paymentModes = shiftBL.GetNetAmountAndPaymentMode(shiftEndTime);
            foreach (ShiftCollections pm in paymentModes)
            {
                if (pm == null)
                    break;
                e.Graphics.DrawString(pm.Mode, defaultFont, Brushes.Black, col1x, yLocation);
                e.Graphics.DrawString(pm.Amount.ToString(ParafaitEnv.AMOUNT_FORMAT), defaultFont, Brushes.Black, col3x, yLocation);
                e.Graphics.DrawString(pm.Amount.ToString(ParafaitEnv.AMOUNT_FORMAT), defaultFont, Brushes.Black, col4x, yLocation);
                yLocation += yIncrement;
            }
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Game Card") + ":", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(txtUserGameCardAmount.Text, defaultFont, Brushes.Black, col2x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : txtSystemGameCardAmount.Text), defaultFont, Brushes.Black, col3x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : lblNetGameCard.Text), defaultFont, Brushes.Black, col4x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Cheques") + ":", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(txtUserChequeAmount.Text, defaultFont, Brushes.Black, col2x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : txtSystemChequeAmount.Text), defaultFont, Brushes.Black, col3x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : lblNetCheque.Text), defaultFont, Brushes.Black, col4x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Coupons") + ":", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(txtUserCouponAmount.Text, defaultFont, Brushes.Black, col2x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : txtSystemCouponAmount.Text), defaultFont, Brushes.Black, col3x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : lblNetCoupon.Text), defaultFont, Brushes.Black, col4x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Tickets") + ":", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(txtUserTicketNumber.Text, defaultFont, Brushes.Black, col2x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : txtSystemTicketNumber.Text), defaultFont, Brushes.Black, col3x, yLocation);
            e.Graphics.DrawString((hideData ? hideText : lblNetTickets.Text), defaultFont, Brushes.Black, col4x, yLocation);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Remarks") + ":", defaultFont, Brushes.Black, col1x, yLocation);
            e.Graphics.DrawString(txtRemarks.Text, defaultFont, Brushes.Black, col2x, yLocation);
            yLocation += yIncrement;

            ShiftCollections[] shiftAmounts = shiftBL.GetShiftAmounts(shiftEndTime);
            double TaxableAmount = 0, DiscountOnTaxableAmount = 0, NonTaxableAmount = 0, DiscountOnNonTaxableAmount = 0, TaxAmount = 0, DiscountOnTaxAmount = 0;
            if (shiftAmounts != null && shiftAmounts.Any())
            {
                TaxableAmount = shiftAmounts[0].TaxableAmount;
                DiscountOnTaxableAmount = shiftAmounts[0].DiscountOnTaxableAmount;
                NonTaxableAmount = shiftAmounts[0].NonTaxableAmount;
                DiscountOnNonTaxableAmount = shiftAmounts[0].DiscountOnNonTaxableAmount;
                TaxAmount = shiftAmounts[0].TaxAmount;
                DiscountOnTaxAmount = shiftAmounts[0].DiscountOnTaxAmount;
            }

            int width1 = (int)e.Graphics.MeasureString("Disc. On Non-TaxableA:", defaultFont).Width;
            int width2 = (int)((e.PageBounds.Width - width1) * .5);
            int width3 = e.PageBounds.Width - width1 - width2 - 10;

            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Amount"), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Tax"), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Taxable Sale") + ":", defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : TaxableAmount.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : TaxAmount.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Non-Taxable Sale") + ":", defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : NonTaxableAmount.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : 0.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Disc. On Taxable") + ":", defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : DiscountOnTaxableAmount.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : DiscountOnTaxAmount.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Disc. On Non-Taxable") + ":", defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : DiscountOnNonTaxableAmount.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : 0.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
            yLocation += yIncrement;
            e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Net Sale") + ":", defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : (TaxableAmount + DiscountOnTaxableAmount + NonTaxableAmount + DiscountOnNonTaxableAmount).ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
            e.Graphics.DrawString((hideData ? hideText : (TaxAmount + DiscountOnTaxAmount).ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
            yLocation += yIncrement;
            ShiftCollections[] shiftdiscountedAmounts = shiftBL.GetShiftDiscountedAmounts(shiftEndTime);
            if (shiftdiscountedAmounts.Any())
            {
                e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Discounts"), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20));
                yLocation += yIncrement;
            }
            foreach (ShiftCollections sd in shiftdiscountedAmounts)
            {
                if (sd == null)
                    break;
                e.Graphics.DrawString(sd.discountName, defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20));
                e.Graphics.DrawString((hideData ? hideText : Convert.ToDouble(sd.discountAmount).ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
                e.Graphics.DrawString((hideData ? hideText : Convert.ToDouble(sd.DiscountedTaxAmount).ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
                yLocation += yIncrement;
            }
            ShiftCollections[] shiftPaymentModes = shiftBL.GetShiftPaymentModes(shiftEndTime);
            if (shiftPaymentModes.Any())
            {
                yLocation += yIncrement;
                e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Payment Mode"), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20));
                e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Amount"), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
                e.Graphics.DrawString("#" + MessageContainerList.GetMessage(executionContext, "Receipts"), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
                yLocation += yIncrement;
            }
            foreach (ShiftCollections pm in shiftPaymentModes)
            {
                if (pm == null)
                    break;
                e.Graphics.DrawString(pm.Mode, defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1, 20));
                e.Graphics.DrawString((hideData ? hideText : pm.Amount.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
                //e.Graphics.DrawString((hideData ? hideText : pm.re.ToString(ParafaitEnv.AMOUNT_FORMAT)), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
                yLocation += yIncrement;
            }
            //Adding the line transactions of Shift cash-card in/out to the shift receipt.
            double totCashAmount = 0.0;
            int totCardCount = 0;
            string posMachine = ParafaitEnv.POSMachine;

            if (shiftDTO != null && shiftDTO.ShiftLogDTOList.Any())
            {
                yLocation += yIncrement;
                e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Shift Cash And Card In/Out"), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1 + width2, 20));
                e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Amount"), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
                e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Cards"), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
                yLocation += yIncrement;
                foreach (ShiftLogDTO shiftLogDTO in shiftDTO.ShiftLogDTOList)
                {
                    if (shiftLogDTO.ShiftAction.Equals("Paid In") || shiftLogDTO.ShiftAction.Equals("Paid Out"))
                    {
                        string sign = shiftLogDTO.ShiftAction.Equals("Paid In") ? "" : "-";
                        e.Graphics.DrawString((hideData ? hideText : sign + shiftLogDTO.ShiftAmount), defaultFont, Brushes.Black, new Rectangle(width1, yLocation, width2, 20), sf);
                        e.Graphics.DrawString((hideData ? hideText : sign + shiftLogDTO.CardCount), defaultFont, Brushes.Black, new Rectangle(width1 + width2, yLocation, width3, 20), sf);
                        e.Graphics.DrawString(shiftLogDTO.ShiftAction, defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1 + width1, 30));
                        yLocation += yIncrement;
                        e.Graphics.DrawString("Reason : " + (hideData ? hideText : shiftLogDTO.ShiftReason), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1 + width1, 30));
                        yLocation += yIncrement;
                        e.Graphics.DrawString("Remarks : " + (hideData ? hideText : shiftLogDTO.ShiftRemarks), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width3, 20));
                        yLocation += yIncrement;
                        if (shiftLogDTO.ShiftAction.Equals("Paid In"))
                        {
                            totCashAmount += Convert.ToDouble(shiftLogDTO.ShiftAmount);
                            totCardCount += Convert.ToInt32(shiftLogDTO.CardCount);
                        }
                        else if (shiftLogDTO.ShiftAction.Equals("Paid Out"))
                        {
                            totCashAmount -= Convert.ToDouble(shiftLogDTO.ShiftAmount);
                            totCardCount -= Convert.ToInt32(shiftLogDTO.CardCount);
                        }
                    }
                }
                yLocation += yIncrement;
                e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Total Petty Cash Amount : ") + (hideData ? hideText : totCashAmount.ToString()), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1 + width3, 20));
                yLocation += yIncrement;
                e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Total Petty Card Count : ") + (hideData ? hideText : totCardCount.ToString()), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1 + width3, 20));
            }
            // Adding signature QR code If the printer is  German ficalization 
            string fiscalPrinterType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FISCAL_PRINTER");
            if (string.IsNullOrEmpty(fiscalPrinterType) == false && fiscalPrinterType.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
            {
                FiskaltrustMapper fiskaltrustMapper = new FiskaltrustMapper(executionContext);
                if (String.IsNullOrEmpty(fiscalSignature) == false)
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData =
                        qrGenerator.CreateQrCode(fiscalSignature, QRCodeGenerator.ECCLevel.M);
                    QRCode qrCode = new QRCode(qrCodeData);
                    if (qrCode != null)
                    {
                        int pixelPerModule = 1;
                        Bitmap BarCode = qrCode.GetGraphic(pixelPerModule);
                        yLocation += yIncrement;
                        e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Fiscalization Reference :"), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1 + width3, 20));
                        yLocation += yIncrement;
                        e.Graphics.DrawImage(BarCode, 100, yLocation);
                    }
                }
                else
                {
                    yLocation += yIncrement;
                    e.Graphics.DrawString(MessageContainerList.GetMessage(executionContext, "Fiscalization Reference : " + fiskaltrustMapper.GetSingatureErrorMessage()), defaultFont, Brushes.Black, new Rectangle(0, yLocation, width1 + width3, 20));
                    yLocation += yIncrement;
                }
            }
            log.LogMethodExit();
        }

        private bool SetupThePrinting()
        {
            log.LogMethodEntry();
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = MyPrintDialog.AllowPrintToFile = MyPrintDialog.AllowSelection = MyPrintDialog.AllowSomePages = MyPrintDialog.PrintToFile = MyPrintDialog.ShowHelp = MyPrintDialog.ShowNetwork = MyPrintDialog.PrinterSettings.DefaultPageSettings.Landscape = false;
            MyPrintDialog.UseEXDialog = true;
            //if (MyPrintDialog.ShowDialog() != DialogResult.OK)
            //    return false;
            bool showPrintDialog = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "SHOW_PRINT_DIALOG_IN_POS").ToLower() == "Y".ToLower();
            if (showPrintDialog && MyPrintDialog.ShowDialog() != DialogResult.OK)
            {
                log.LogMethodExit("User cancelled in print dialog");
                return false;
            }
            MyPrintDocument.DocumentName = MessageContainerList.GetMessage(executionContext, "POS Shift Report");
            MyPrintDocument.PrinterSettings = MyPrintDialog.PrinterSettings;
            MyPrintDocument.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            MyPrintDocument.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);
            log.LogMethodExit(true);
            return true;
        }

        private void btnShowNumPad_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.ActiveControl = CurrentTextBox;
            showNumberPadForm('-');
            log.LogMethodExit();
        }

        TextBox CurrentTextBox;
        private void TextEnterEvent(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            CurrentTextBox = (sender) as TextBox;
            log.LogMethodExit();
        }

        private void showNumberPadForm(char firstKey)
        {
            log.LogMethodEntry(firstKey);
            double varAmount = NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, "Enter Amount"), firstKey, Utilities);
            if (varAmount >= 0)
            {
                TextBox txtBox = null;
                try
                {
                    if (this.ActiveControl.GetType().ToString().ToLower().Contains("textbox"))
                        txtBox = this.ActiveControl as TextBox;
                }
                catch { }
                if (txtBox != null && !txtBox.ReadOnly)
                {
                    txtBox.Text = varAmount.ToString();
                    this.ValidateChildren();
                }
            }
            log.LogMethodExit();
        }

        private void txtRemarks_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            CurrentTextBox = null;
            log.LogMethodExit();
        }

        private void cb_open_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            (sender as Button).BackgroundImage = Properties.Resources.button_pressed;
            log.LogMethodExit();
        }

        private void cb_open_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodExit();
            (sender as Button).BackgroundImage = Properties.Resources.button_normal;
            log.LogMethodExit();
        }

        private void cb_EOD_Click(object sender, EventArgs e)
        {
            log.LogMethodExit();
            int managerId = -1;
            int posmachineId = pOSMachines.POSMachineDTO.POSMachineId;
            if (POSUtils.IsOpenTransactionExists(posmachineId))
            {
                frmParafaitMessageBox messageBox = new frmParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1352), "Open Transaction Exists", MessageBoxButtons.OK);
                messageBox.ShowDialog();
                return;
            }
            bool managerApprovedReqd = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_MANAGER_APPROVAL_FOR_END_OF_DAY").Equals("Y");
            if (!managerApprovedReqd || (managerApprovedReqd && Authenticate.Manager(ref managerId)))
            {
                frmEodUI eodUI = new frmEodUI(Utilities);
                if (eodUI.ShowDialog() != DialogResult.Cancel)
                {
                    isEODOperation = true;
                    cb_close.PerformClick();
                    if (isCloseShiftSuccess)
                        eodUI.PerformEndOfDay(posmachineId);
                }
            }
            log.LogMethodExit();
        }

        private void LoadPOSCounter()
        {
            log.LogMethodEntry();
            POSTypeListBL pOSTypeListBL = new POSTypeListBL(executionContext);
            List<POSTypeDTO> pOSTypeDTOList = pOSTypeListBL.GetPOSCounterByRoleId(ParafaitEnv.RoleId, executionContext);
            if (pOSTypeDTOList != null && pOSTypeDTOList.Count > 0)
            {
                pOSTypeDTOList.Insert(0, new POSTypeDTO());
                pOSTypeDTOList[0].POSTypeName = "All";
                pOSTypeDTOList[0].POSTypeId = -1;
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = pOSTypeDTOList;
                cmbPOSCounter.DataSource = bindingSource;
                cmbPOSCounter.DisplayMember = "POSTypeName";
                cmbPOSCounter.ValueMember = "POSTypeId";
            }
            log.LogMethodExit();
        }
        private List<POSMachineDTO> GetPOSMachineList()
        {
            log.LogMethodEntry();
            Users users = new Users(executionContext, ParafaitEnv.User_Id);
            List<POSMachineDTO> pOSMachineDTOList = new List<POSMachineDTO>();
            POSMachineList pOSMachineList = new POSMachineList(executionContext);
            pOSMachineDTOList = pOSMachineList.GetPOSMachineListByUserId(ParafaitEnv.User_Id, executionContext);
            log.LogMethodExit(pOSMachineDTOList);
            return pOSMachineDTOList;
        }
        private List<UsersDTO> GetUsersList()
        {
            log.LogMethodEntry();
            List<UsersDTO> UsersDTOList = new List<UsersDTO>();
            UserRolesList userRoleList = new UserRolesList();
            List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>> SearchParameters = new List<KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>>();
            SearchParameters.Add(new KeyValuePair<UserRolesDTO.SearchByUserRolesParameters, string>(UserRolesDTO.SearchByUserRolesParameters.ALLOW_SHIFT_OPEN_CLOSE, "1"));
            List<UserRolesDTO> UserRoleDtoList = userRoleList.GetAllUserRoles(SearchParameters);
            if (UserRoleDtoList != null)
            {
                //get list of manager roles
                managerRole = UserRoleDtoList.Where(x => x.ManagerFlag.Equals("Y")).ToList();
                foreach (UserRolesDTO userRoleDTO in UserRoleDtoList)
                {
                    UsersList usersList = new UsersList(executionContext);
                    List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> SearchUserParameters = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                    SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_ID, userRoleDTO.RoleId.ToString()));
                    SearchUserParameters.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ACTIVE_FLAG, "Y"));
                    List<UsersDTO> UsersList = usersList.GetAllUsers(SearchUserParameters);
                    if (UsersList != null)
                    {
                        foreach (UsersDTO usersDTO in UsersList)
                        {
                            UsersDTOList.Add(usersDTO);
                        }
                    }
                }
            }
            log.LogMethodExit(UsersDTOList);
            return UsersDTOList;
        }

        private void ClearFields()
        {
            log.LogMethodEntry();
            lblNetCards.Text = lblNetCash.Text = lblNetCheque.Text = lblNetCoupon.Text = lblNetCreditCard.Text = lblNetGameCard.Text = lblNetTickets.Text = "...";
            txtSystemTicketNumber.Text = txtSystemCouponAmount.Text = txtSystemChequeAmount.Text = txtSystemGameCardAmount.Text = txtSystemCreditCardAmount.Text = txtSystemCashAmount.Text = txtSystemCardCount.Text = string.Empty;
            txtUserCashAmount.Text = txtUserCardCount.Text = txtUserCreditCardAmount.Text = txtUserGameCardAmount.Text = txtUserChequeAmount.Text = txtUserCouponAmount.Text =
            txtUserTicketNumber.Text = txtRemarks.Text = txttb_logoutdate.Text = txtlb_logindate.Text = string.Empty;
            log.LogMethodExit();
        }

        private void BtnRemoteOpenShift_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            shiftType = ShiftDTO.ShiftActionType.ROpen.ToString();
            shiftOpen = true;
            string POSMachines = string.Empty;
            int selectedMachines = 0;
            foreach (DataGridViewRow row in dgvOpenShift.Rows)
            {
                if (row.Cells["chkOpenShiftDGVSelectRow"].Value != null &&
                    Convert.ToBoolean(row.Cells["chkOpenShiftDGVSelectRow"].Value))
                {
                    if (row.Cells["cbOpenShiftDGVUser"].Value == null || Convert.ToInt32(row.Cells["cbOpenShiftDGVUser"].Value) <= 0)
                    {
                        POSUtils.ParafaitMessageBox("Please select a valid user for POS Machine : " + row.Cells["cbOpenShiftDGVPOSMachine"].FormattedValue);
                        break;
                    }
                    selectedMachines++;
                    selectedPOSMachines = pOSMachineDTOList.Where(x => x.POSMachineId == Convert.ToInt32(row.Cells["cbOpenShiftDGVPOSMachine"].Value)).ToList();
                    selectedUsersDTO = usersDTOList.Where(x => x.UserId == Convert.ToInt32(row.Cells["cbOpenShiftDGVUser"].Value)).ToList();

                    if (string.Equals(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_CONCURRENT_USER_LOGIN").ToUpper(), "N"))
                    {
                        List<UsersDTO> assignedUsers = usersDTOList.Where(x => OpenShiftDTOList.Any(y => y.ShiftLoginId.Equals(x.LoginId))).ToList();
                        if (assignedUsers.Contains(selectedUsersDTO[0]))
                        {
                            List<string> machineName = OpenShiftDTOList.Where(x => x.ShiftLoginId == selectedUsersDTO[0].LoginId).Select(x => x.POSMachine).ToList<string>();
                            POSUtils.ParafaitMessageBox(row.Cells["cbOpenShiftDGVUser"].FormattedValue + " is already assigned for the machine: " + machineName[0]);
                            SetUpRemoteShiftUI();
                            ClearFields();
                            txtMessage.Text = string.Empty;
                            return;
                        }
                    }
                    using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            dbTrx.BeginTransaction();
                            pOSMachines = new POSMachines(executionContext, selectedPOSMachines[0].POSMachineId);
                            OpenShift(selectedUsersDTO[0].UserName, selectedUsersDTO[0].LoginId, selectedUsersDTO[0].UserId, dbTrx.SQLTrx, true);
                            dbTrx.EndTransaction();
                        }
                        catch (Exception ex)
                        {
                            dbTrx.RollBack();
                            POSMachines += "\"" + selectedPOSMachines[0].POSName + "\"";
                            log.Error("Error opening the remote shift. " + ex.Message);
                        }
                    }
                }
            }
            if (selectedMachines > 0)
            {
                POSMachineList pOSMachineList = new POSMachineList(executionContext);
                OpenShiftDTOList = pOSMachineList.GetOpenShiftDTOList(pOSMachineDTOList);
                if (shiftOpen)
                {
                    SetUpRemoteShiftUI();
                    ClearFields();
                }
                if (POSMachines.Length > 0)
                {
                    txtMessage.ForeColor = Color.White;
                    txtMessage.BackColor = Color.Red;
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Shift Open failed for ") + POSMachines;
                }
                else if (shiftOpen)
                {
                    txtMessage.ForeColor = Color.Black;
                    txtMessage.BackColor = Color.White;
                    txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Shift Open Successful.");
                }
            }
            log.LogMethodExit();
        }

        private void BtnRemoteCloseShift_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            shiftType = ShiftDTO.ShiftActionType.Close.ToString();
            shiftClose = true;
            txtMessage.Text = string.Empty;
            foreach (DataGridViewRow row in dgvCloseShift.Rows)
            {
                if (row.Cells["chkCloseShiftDGVSelectRow"].Value != null &&
                    (bool)row.Cells["chkCloseShiftDGVSelectRow"].Value)
                {
                    selectedPOSMachines = pOSMachineDTOList.Where(x => x.POSMachineId == Convert.ToInt32(row.Cells["cbCloseShiftDGVPOSMachine"].Value)).ToList();
                    selectedUsersDTO = usersDTOList.Where(x => x.UserId == Convert.ToInt32(row.Cells["cbCloseShiftDGVUser"].Value)).ToList();
                    TransactionListBL transactionList = new TransactionListBL(executionContext);
                    List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, ShiftDTO.ShiftActionType.Open.ToString()));
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.POS_NAME, selectedPOSMachines[0].POSName.ToString()));
                    var list = transactionList.GetTransactionDTOList(searchParameters, Utilities);
                    if ((list == null) || (list != null && POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1353), "Open Transactions Exists", MessageBoxButtons.YesNo) == DialogResult.No))
                    {
                        try
                        {
                            CloseShift(selectedUsersDTO[0].UserName, selectedUsersDTO[0].LoginId, selectedUsersDTO[0].UserId, Convert.ToInt32(row.Cells["txtCloseShiftDGVShiftId"].Value));
                        }
                        catch (Exception ex)
                        {
                            txtMessage.ForeColor = System.Drawing.Color.Red;
                            txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Shift Close Failed");
                            log.Error("Error closing the remote shift. " + ex.Message);
                            return;
                        }
                        if (shiftClose)
                        {
                            txtMessage.ForeColor = Color.Black;
                            txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Shift Close Successful");
                            ClearFields();
                            if (rbBlindClose.Checked)
                            {
                                SetUpMultipleBlindClosedUI();
                                return;
                            }
                            POSMachineList pOSMachineList = new POSMachineList(executionContext);
                            OpenShiftDTOList = pOSMachineList.GetOpenShiftDTOList(pOSMachineDTOList);
                            SetUpRemoteShiftUI();
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private void DgvCloseShift_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                foreach (DataGridViewRow row in dgvOpenShift.Rows)
                {
                    DataGridViewCheckBoxCell checkBox = (row.Cells["chkOpenShiftDGVSelectRow"] as DataGridViewCheckBoxCell);
                    checkBox.Value = false;
                }
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;
                if (e.ColumnIndex == chkCloseShiftDGVSelectRow.Index)
                {
                    selectedUsersDTO = usersDTOList.Where(x => x.UserId == Convert.ToInt32(dgvCloseShift.Rows[e.RowIndex].Cells["cbCloseShiftDGVUser"].Value.ToString())).ToList();

                    gb_header.Text = MessageContainerList.GetMessage(executionContext, "Close Shift");
                    txttb_logoutdate.Visible = txtlb_logindate.Visible = lblLoginTime.Visible = lblLogOutTime.Visible = true;
                    var isChecked = true;
                    if (dgvCloseShift.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null ||
                        !Convert.ToBoolean(dgvCloseShift.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                        isChecked = true;
                    else
                        isChecked = false;
                    if (isChecked)
                    {
                        foreach (DataGridViewRow row in dgvCloseShift.Rows)
                        {
                            if (row.Index != e.RowIndex)
                            {
                                row.Cells[0].Value = !isChecked;
                            }
                        }
                    }
                    cb_EOD.Enabled = false;
                    DateTime businessStartDate = DateTime.Today.AddHours(Convert.ToInt32(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME")));
                    DateTime businessEndDate;
                    if (businessStartDate.CompareTo(ServerDateTime.Now) == 1)
                    {
                        businessStartDate = businessStartDate.AddDays(-1);
                    }
                    businessEndDate = businessStartDate.AddDays(1);
                    DateTime selectedShiftDate = Convert.ToDateTime(dgvCloseShift.Rows[e.RowIndex].Cells["ShiftDate"].Value);
                    if (selectedShiftDate.CompareTo(businessStartDate) >= 0 && selectedShiftDate.CompareTo(businessEndDate) <= 0
                        && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_END_OF_DAY_ON_CLOSE_SHIFT").Equals("Y")
                        && ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ENABLE_REMOTE_EOD").Equals("Y"))
                    {
                        cb_EOD.Enabled = true;
                    }
                    selectedPOSMachines = pOSMachineDTOList.Where(x => x.POSMachineId == Convert.ToInt32(dgvCloseShift.Rows[e.RowIndex].Cells["cbCloseShiftDGVPOSMachine"].Value)).ToList();
                    pOSMachines = new POSMachines(executionContext, Convert.ToInt32(dgvCloseShift.Rows[e.RowIndex].Cells["cbCloseShiftDGVPOSMachine"].Value));
                    dgvCloseShift.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = isChecked;
                    shiftType = ShiftDTO.ShiftActionType.Close.ToString();
                    ShiftBL shiftBL = new ShiftBL(executionContext, Convert.ToInt32(dgvCloseShift.Rows[e.RowIndex].Cells["txtCloseShiftDGVShiftId"].Value), true, true);
                    shiftDTO = shiftBL.ShiftDTO;
                    DateTime shiftEndTime = rbBlindClose.Checked ? Convert.ToDateTime(dgvCloseShift.Rows[e.RowIndex].Cells["ShiftEndDate"].Value) : ServerDateTime.Now;
                    SetUIElementsOnCloseShift(shiftEndTime);
                    btnRemoteOpenShift.Enabled = btnBlindClose.Enabled = false;
                    btnRemoteCloseShift.Enabled = true;
                    txtlb_logindate.Text = shiftDTO.ShiftTime.ToString();
                    txttb_logoutdate.Text = shiftEndTime.ToString();
                    txtlb_user.Text = shiftDTO.ShiftUserName;
                }
            }

            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void DgvOpenShift_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                btnBlindClose.Visible = false;
                cb_open.Visible = true;
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;
                foreach (DataGridViewRow row in dgvCloseShift.Rows)
                {
                    DataGridViewCheckBoxCell checkBox = (row.Cells["chkCloseShiftDGVSelectRow"] as DataGridViewCheckBoxCell);
                    checkBox.Value = false;
                }
                gb_header.Text = MessageContainerList.GetMessage(executionContext, "Open Shift");
                cb_open.Enabled = true;

                if (dgvOpenShift.SelectedCells.Count > 0)
                {
                    btnRemoteOpenShift.Enabled = true;
                    btnRemoteCloseShift.Enabled = false;
                }
                if (e.ColumnIndex == chkOpenShiftDGVSelectRow.Index)
                {
                    DataGridViewCheckBoxCell cell = (dgvOpenShift.Rows[e.RowIndex].Cells[chkOpenShiftDGVSelectRow.Index] as DataGridViewCheckBoxCell);
                    cell.Value = !((bool)cell.Value);
                }
                ClearFields();
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void cmbPOSCounter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                SetUpRemoteShiftUI();
                if (rbBlindClose.Checked)
                {
                    SetUpMultipleBlindClosedUI();
                }
                txtMessage.ForeColor = Color.Black;
                txtMessage.Text = MessageContainerList.GetMessage(executionContext, "Refresh Successful");
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }

        private void btnBlindClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction dbTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (shiftDTO != null && shiftDTO.ShiftKey != -1)
                    {
                        dbTrx.BeginTransaction();
                        pOSMachines.ProvisionalClose(shiftDTO.ShiftKey, ParafaitEnv.User_Id, dbTrx.SQLTrx);
                        POSUtils.OpenShiftUserName = string.Empty;
                        dbTrx.EndTransaction();
                        OpenCashDrawer(shiftDTO.ShiftKey, ParafaitEnv.LoginID);
                        if (shiftType != ShiftDTO.ShiftActionType.ROpen.ToString())
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
                catch (Exception ex) { dbTrx.RollBack(); log.Error(ex); }
            }
            log.LogMethodExit();
        }

        private void rdRemoteClose_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ClearFields();
                ClearDataSource();
                grpBoxOpenShift.Enabled = true;
                btnRemoteOpenShift.Enabled = false;
                btnRemoteCloseShift.Visible = btnRemoteCloseShift.Enabled = true;
                SetUpRemoteShiftUI();
            }
            catch (Exception ex) { log.Error(ex); }
            log.LogMethodExit();
        }
        private void ClearDataSource()
        {
            log.LogMethodEntry();
            dgvOpenShift.DataSource = null;
            dgvCloseShift.DataSource = null;
            cbOpenShiftDGVUser.DataSource = null;
            cbOpenShiftDGVPOSMachine.DataSource = null;
            cbCloseShiftDGVUser.DataSource = null;
            cbCloseShiftDGVPOSMachine.DataSource = null;
            log.LogMethodExit();
        }

        private void dgvOpenShift_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private void frm_shift_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry();
            POSUtils.GetOpenShiftDTOList(ParafaitEnv.POSMachineId);
            log.LogMethodExit();
        }

        private void rbBlindClose_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (rbBlindClose.Checked && rbBlindClose.Visible)
            {
                btnBlindClose.Enabled = false;
                SetUpMultipleBlindClosedUI();
            }
            log.LogMethodExit();
        }
    }
}
