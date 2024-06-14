/********************************************************************************************
 * Project Name - frm Locker SetUp
 * Description  - User interface for frm Locker SetUp
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Deeksha       Added logger methods.
 *2.130.00    29-Jun-2021   Dakshakh raj   Modified as part of Metra locker integration 
 *2.150.5      18-Oct-2023     Abhishek    Modified: Show error message if externalIdentifier
 *                                            for locker not setup.(Hecere Lockers)  
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Lockers;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    public partial class frmLockerSetup : Form
    {
        bool _forAllocation;
        Semnox.Core.Utilities.Utilities utilities;//Starts:Online Locker 10-08-2017
        string onlineServiceUrl;
        LockerPanelDTO selectedLockerPanelDTO;
        int zoneId = -1;
        string mode;
        public string Mode { set { mode = value; } }
        string zoneCode;
        string lockerZoneMode;
        bool isReassign = false;
        List<LockerZonesDTO> lockerZonesDTOList;
        private MessageBoxDelegate messageBoxDelegate;
        AuthenticateManagerDelegate authenticateManagerDelegate;
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();//Ends:Online Locker 10-08-2017
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DeviceClass primaryCardReader;
        string lockerMake = "";
        public string LockerMake { get { return lockerMake; } }
        Card tappedcard;
        public Card Currentcard { set { tappedcard = value; } }
        bool isLockerReturn;
        public frmLockerSetup(Semnox.Core.Utilities.Utilities _Utilities, int zoneId, DeviceClass primaryCardReader, MessageBoxDelegate messageBoxDelegate, AuthenticateManagerDelegate authenticateManagerDelegate, bool forAllocation = true, bool isLockerReturn = false)//Online Locker 10-08-2017
        {
            Semnox.Core.Utilities.Logger.setRootLogLevel(log);
            log.LogMethodEntry(_Utilities, zoneId, primaryCardReader, messageBoxDelegate, authenticateManagerDelegate, forAllocation, isLockerReturn);
            InitializeComponent();
            _forAllocation = forAllocation;
            this.isLockerReturn = isLockerReturn;
            this.zoneId = zoneId;
            utilities = _Utilities;
            this.messageBoxDelegate = messageBoxDelegate;
            this.authenticateManagerDelegate = authenticateManagerDelegate;
            this.primaryCardReader = primaryCardReader;
            mode = "ALL";
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }

            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            pnlOnlineOptions.Top = flpLockerPanels.Top;
            btnReassign.Enabled = btnRelease.Enabled = false;
            gpLockerOption.Visible = false;
            tbcLocks.TabPages.Remove(tpLockerOptions);
            tbcLocks.TabPages.Remove(tpOtherOptions);
            tbcLocks.TabPages.Remove(tpApStatus);
            tbcLocks.Visible = false;
            gpApStatus.Visible = false;
            utilities.setupDataGridProperties(ref dgvZone);
            lockerMake = utilities.getParafaitDefaults("LOCKER_LOCK_MAKE");
            if (utilities.getParafaitDefaults("IS_ONLINE_OPTION_ENABLED").Equals("Y"))//Online Locker
            {
                btnBlockCard.Visible = btnBlockLock.Visible = btnOpenLocker.Visible = btnOpenAll.Visible = true;
                tbcLocks.TabPages.Add(tpOtherOptions);
                LoadAPstatus();
                tbcLocks.Visible = true;
                onlineServiceUrl = utilities.getParafaitDefaults("ONLINE_LOCKER_SERVICE_URL");
            }
            else
            {
                flpLockerZones.Size = new Size(gpApStatus.Right, flpLockerZones.Height);
                btnBlockCard.Visible = btnBlockLock.Visible = btnOpenLocker.Visible = btnOpenAll.Visible = false;
                btnBlockCard.Enabled = btnBlockLock.Enabled = btnOpenLocker.Enabled = btnOpenAll.Enabled = false;
            }

            LoadStatus();
            //LoadMetraStatus();
            log.LogMethodExit();
        }

        private void LoadLockerZones()
        {
            log.LogMethodEntry();
            lockerZonesDTOList = new List<LockerZonesDTO>();
            LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
            List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> lockerZoneSearchParams = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
            if (zoneId != -1)
            {
                lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ZONE_ID, zoneId.ToString()));
            }
            if (mode.Equals(ParafaitLockCardHandlerDTO.LockerSelectionMode.FIXED.ToString()))
            {
                lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.LOCKER_MODE, mode));
            }
            lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            lockerZonesDTOList = lockerZonesList.GetLockerZonesList(lockerZoneSearchParams, false);
            if (!string.IsNullOrEmpty(onlineServiceUrl))
            {
                BindingSource bindingSourceZones = new BindingSource();
                bindingSourceZones.DataSource = lockerZonesDTOList;
                dgvZone.DataSource = bindingSourceZones;
                if (dgvZone.Columns.Count > 1)
                {
                    dgvZone.Columns[1].Visible = false;
                    foreach (DataGridViewColumn dgvc in dgvZone.Columns)
                    {
                        dgvc.ReadOnly = true;
                        dgvc.Visible = false;
                    }
                    foreach (DataGridViewRow dr in dgvZone.Rows)
                    {
                        if (dr.Cells["LockerMake"].Value == null || (dr.Cells["LockerMake"].Value != null && string.IsNullOrEmpty(dr.Cells["LockerMake"].Value.ToString())))
                        {
                            dr.Cells["LockerMake"].Value = ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOCKER_LOCK_MAKE");
                        }
                    }
                    dgvZone.Columns[0].ReadOnly = false;
                    dgvZone.Columns[0].Visible = true;
                    dgvZone.Columns["ZoneName"].HeaderText = utilities.MessageUtils.getMessage("Zone Name");
                    dgvZone.Columns["ZoneName"].Visible = true;
                    dgvZone.Columns["ZoneCode"].HeaderText = utilities.MessageUtils.getMessage("Zone Code");
                    dgvZone.Columns["ZoneCode"].Visible = true;
                    if (dgvZone.Rows.Count > 0)
                    {
                        ckbSelectAll.Visible = true;
                    }
                    else
                    {
                        ckbSelectAll.Visible = false;
                    }
                }
                else
                {
                    ckbSelectAll.Visible = false;
                }
            }
            else
            {
                ckbSelectAll.Visible = false;
            }
            if (lockerZonesDTOList != null && lockerZonesDTOList.Count == 1)
            {
                zoneCode = lockerZonesDTOList[0].ZoneCode;
                lockerZoneMode = lockerZonesDTOList[0].LockerMode;
                flpLockerZones.Visible = false;
                flpLockerPanels.Location = flpLockerZones.Location;
                panelLockers.Top = flpLockerPanels.Bottom + 3;
                LoadPanel(lockerZonesDTOList[0]);
                lockerMake = string.IsNullOrEmpty(lockerZonesDTOList[0].LockerMake) ? ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOCKER_LOCK_MAKE") : lockerZonesDTOList[0].LockerMake;
            }
            else if (lockerZonesDTOList != null)
            {
                foreach (LockerZonesDTO lockerZonesDTO in lockerZonesDTOList)
                {
                    Button btnZone = new Button();
                    btnZone.FlatAppearance.BorderSize = btnSampleZone.FlatAppearance.BorderSize;
                    btnZone.FlatAppearance.CheckedBackColor = btnSampleZone.FlatAppearance.CheckedBackColor;
                    btnZone.FlatAppearance.MouseDownBackColor = btnSampleZone.FlatAppearance.MouseDownBackColor;
                    btnZone.FlatAppearance.MouseOverBackColor = btnSampleZone.FlatAppearance.MouseOverBackColor;
                    btnZone.FlatStyle = FlatStyle.Flat;
                    btnZone.Size = btnSampleZone.Size;
                    btnZone.BackgroundImage = btnSampleZone.BackgroundImage;
                    btnZone.BackgroundImageLayout = btnSampleZone.BackgroundImageLayout;
                    btnZone.Text = lockerZonesDTO.ZoneName.ToString();
                    btnZone.Tag = lockerZonesDTO;
                    btnZone.Click += btnZone_Click;

                    flpLockerZones.Controls.Add(btnZone);
                }

                flpLockerZones.Controls.Remove(btnSampleZone);
                if (flpLockerZones.Controls.Count > 0) //added on 20-July-2017, for fixing nullreference issue when no lockers exist
                {
                    (flpLockerZones.Controls[0] as Button).PerformClick();
                }
            }
            log.LogMethodExit();
        }
        private void LoadPanel(LockerZonesDTO lockerZoneDTO)
        {
            log.Debug("Starts-LoadPanel(" + lockerZoneDTO + ")");//Added for logger function on 08-Mar-2016

            flpLockerPanels.Controls.Clear();
            if (lockerZoneDTO.LockerPanelDTOList == null)
            {
                return;
            }
            if (lockerZoneDTO.LockerPanelDTOList.Count > 0)
            {
                foreach (LockerPanelDTO lockerPanelDTO in lockerZoneDTO.LockerPanelDTOList)
                {
                    Button btnPanel = new Button();
                    btnPanel.FlatAppearance.BorderSize = btnSamplePanel.FlatAppearance.BorderSize;
                    btnPanel.FlatAppearance.CheckedBackColor = btnSamplePanel.FlatAppearance.CheckedBackColor;
                    btnPanel.FlatAppearance.MouseDownBackColor = btnSamplePanel.FlatAppearance.MouseDownBackColor;
                    btnPanel.FlatAppearance.MouseOverBackColor = btnSamplePanel.FlatAppearance.MouseOverBackColor;
                    btnPanel.FlatStyle = FlatStyle.Flat;
                    btnPanel.Size = btnSamplePanel.Size;
                    btnPanel.BackgroundImage = btnSamplePanel.BackgroundImage;
                    btnPanel.BackgroundImageLayout = btnSamplePanel.BackgroundImageLayout;
                    btnPanel.Text = lockerPanelDTO.PanelName;
                    btnPanel.Tag = lockerPanelDTO;
                    btnPanel.Click += btnPanel_Click;
                    flpLockerPanels.Controls.Add(btnPanel);
                }
                LoadLocker(lockerZoneDTO.LockerPanelDTOList[0], "", -1);
            }
            log.Debug("Ends-LoadPanel(" + lockerZoneDTO + ")");//Added for logger function on 08-Mar-2016
        }

        private void LoadLocker(LockerPanelDTO lockerPanelDTO, string lockerStatus, int batteryStatus = -1)
        {
            log.LogMethodEntry(lockerPanelDTO, lockerStatus, batteryStatus);
            List<LockerDTO> lockerDTOList;
            if (lockerPanelDTO != null)
            {
                LockerPanel lockerPanel = new LockerPanel(machineUserContext, lockerPanelDTO.PanelId);
                lockerPanelDTO = lockerPanel.getLockerPanelDTO;
                this.selectedLockerPanelDTO = lockerPanelDTO;
                tblLockers.Controls.Clear();
                tblLockers.RowCount = tblLockers.ColumnCount = 0;

                int RowCount = lockerPanelDTO.LockerDTOList.Max(x => x.RowIndex);
                if (RowCount > 0)
                {
                    int ColCount = lockerPanelDTO.LockerDTOList.Max(x => x.ColumnIndex);
                    if (string.IsNullOrEmpty(lockerStatus) && batteryStatus == -1)//Starts:Online Locker 10-08-2017
                    {
                        addRow(RowCount);
                        addColumn(ColCount);
                    }
                    lockerDTOList = lockerPanelDTO.LockerDTOList.Where(x => (bool)((string.IsNullOrEmpty(lockerStatus) || x.LockerStatus.Equals(lockerStatus)) && (batteryStatus == -1 || x.BatteryStatus == batteryStatus))).ToList<LockerDTO>();
                    if (lockerDTOList == null)
                    {
                        log.LogMethodExit();
                        return;
                    }
                    int row = 1;
                    int col = 1;
                    if (!string.IsNullOrEmpty(lockerStatus) || batteryStatus != -1)//Online Locker
                    {
                        if (lockerDTOList != null && lockerDTOList.Count > 0)//Online Locker
                        {
                            addRow((lockerDTOList.Count / 10) + 1);
                            addColumn((lockerDTOList.Count > 10) ? 10 : lockerDTOList.Count);
                        }
                    }
                    foreach (LockerDTO lockerDTO in lockerDTOList)//Starts:Online Locker 10-08-2017
                    {
                        if (string.IsNullOrEmpty(lockerStatus) && batteryStatus == -1)//Online Locker
                        {
                            row = lockerDTO.RowIndex;
                            col = lockerDTO.ColumnIndex;
                        }
                        Button buttonLocker = tblLockers.GetControlFromPosition(col - 1, row - 1) as Button;
                        LockerDetails lockerDetail = buttonLocker.Tag as LockerDetails;
                        buttonLocker.Enabled = lockerDTO.IsActive;
                        if (lockerDTO.IsActive)
                        {
                            buttonLocker.BackgroundImage = Properties.Resources.green_76x76;
                        }
                        if (lockerDTO.IsDisabled)
                        {
                            buttonLocker.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.yellow_76x76;
                            buttonLocker.Enabled = false;
                        }

                        if (lockerDTO.LockerAllocated != null && lockerDTO.LockerAllocated.Id != -1)
                        {
                            buttonLocker.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.red_76x76;
                            if (_forAllocation)
                                buttonLocker.Enabled = false;
                        }
                        lockerDetail.lockerDTO = lockerDTO;
                        List<LockerZonesDTO> loopLockerZonesDTOList = lockerZonesDTOList.Where(x => (bool)(x.ZoneId == lockerPanelDTO.ZoneId)).ToList<LockerZonesDTO>();
                        if (loopLockerZonesDTOList != null && loopLockerZonesDTOList.Count > 0)
                        {
                            lockerDetail.ZoneCode = (string.IsNullOrEmpty(loopLockerZonesDTOList[0].ZoneCode)) ? "0" : loopLockerZonesDTOList[0].ZoneCode;
                            lockerDetail.LockerMode = (string.IsNullOrEmpty(loopLockerZonesDTOList[0].LockerMode)) ? utilities.getParafaitDefaults("LOCKER_SELECTION_MODE") : loopLockerZonesDTOList[0].LockerMode;
                        }
                        buttonLocker.Text = lockerDetail.lockerDTO.LockerName + Environment.NewLine + "[" + lockerDetail.lockerDTO.Identifier.ToString() + "]";

                        if (lockerDetail.lockerDTO.LockerStatus.Equals("C") && !string.IsNullOrEmpty(lockerDetail.LockerMode) && lockerDetail.LockerMode.Equals("FREE"))
                        {
                            buttonLocker.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.red_76x76;
                        }

                        if (lockerDetail.lockerDTO.LockerStatus.Equals("B"))
                        {
                            buttonLocker.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.Black_76x76;
                            buttonLocker.ForeColor = Color.White;
                        }

                        if (lockerDetail.lockerDTO.BatteryStatus == 1)
                        {
                            buttonLocker.BackgroundImage = Semnox.Parafait.Transaction.Properties.Resources.orangeBatteryLow_76x76;
                        }
                        if (!string.IsNullOrEmpty(lockerStatus) || batteryStatus != -1)//Online Locker
                        {
                            if (col == 10)
                            {
                                col = 1;
                                row++;
                            }
                            else col++;
                        }
                    }
                    //int cellcount = tblLockers.RowCount * tblLockers.ColumnCount;
                    //col = tblLockers.ColumnCount;
                    //row = tblLockers.RowCount;
                    //for (int i = cellcount; i > lockerPanelDTO.LockerDTOList.Count; i--)
                    //{
                    //    Button buttonLocker = tblLockers.GetControlFromPosition(col - 1, row - 1) as Button;
                    //    if (buttonLocker != null)
                    //    {
                    //        buttonLocker.BackgroundImage = null;
                    //        buttonLocker.Enabled = false;
                    //    }
                    //    col--;
                    //}
                }
            }
            log.LogMethodExit();
        }
        private void LoadAPstatus()
        {
            log.LogMethodEntry();
            try
            {
                List<LockerAccessPointDTO> activeAccessPointDTOList;
                LockerAccessPointList lockerAccessPointList = new LockerAccessPointList(machineUserContext);
                List<KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>> lockerAccessPointSearchParams = new List<KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>>();
                lockerAccessPointSearchParams.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.ACTIVE_FLAG, "1"));
                lockerAccessPointSearchParams.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lockerAccessPointSearchParams.Add(new KeyValuePair<LockerAccessPointDTO.SearchByLockerAccessPointParameters, string>(LockerAccessPointDTO.SearchByLockerAccessPointParameters.LOAD_CHILD_RECORDS,"0"));
                List<LockerAccessPointDTO> accessPointDTOList = lockerAccessPointList.GetAllLockerAccessPoint(lockerAccessPointSearchParams);
                if (accessPointDTOList == null || (accessPointDTOList != null && accessPointDTOList.Count == 0))
                {
                    btnApStatus.Text = utilities.MessageUtils.getMessage("No Active AP");
                    btnApStatus.BackColor = Color.Red;
                }
                else
                {
                    activeAccessPointDTOList = accessPointDTOList.Where(x => (bool)(x.IsAlive)).ToList<LockerAccessPointDTO>();
                    if (activeAccessPointDTOList.Count == accessPointDTOList.Count)
                    {
                        btnApStatus.Text = utilities.MessageUtils.getMessage("All APs are Online");
                        btnApStatus.BackColor = Color.Green;
                    }
                    else if (activeAccessPointDTOList.Count == 0)
                    {
                        btnApStatus.Text = utilities.MessageUtils.getMessage("All APs are Offline");
                        btnApStatus.BackColor = Color.Red;
                    }
                    else if (activeAccessPointDTOList.Count != accessPointDTOList.Count)
                    {
                        btnApStatus.Text = utilities.MessageUtils.getMessage("Few APs are Offline");
                        btnApStatus.BackColor = Color.Orange;
                    }
                    Label lbl = new Label();
                    flpAplist.Controls.Clear();
                    foreach (LockerAccessPointDTO lockerAccessPointDTO in accessPointDTOList)
                    {
                        lbl = new Label();
                        lbl.AutoSize = false;
                        lbl.Size = new Size(flpAplist.Width, btnApStatus.Height);
                        lbl.Font = btnApStatus.Font;
                        lbl.ForeColor = Color.White;
                        lbl.Margin = new Padding(0, 5, 0, 5);
                        if (lockerAccessPointDTO.IsAlive)
                        {
                            lbl.BackColor = Color.Green;
                        }
                        else
                        {
                            lbl.BackColor = Color.Red;
                        }
                        lbl.Text = utilities.MessageUtils.getMessage("Name") + ": " + lockerAccessPointDTO.Name + ", " + utilities.MessageUtils.getMessage("Group") + ": " + lockerAccessPointDTO.GroupCode + ", " + utilities.MessageUtils.getMessage("Locker Range") + ": " + lockerAccessPointDTO.LockerIDFrom + " - " + lockerAccessPointDTO.LockerIDTo;
                        flpAplist.Controls.Add(lbl);
                    }
                }

                if (!tbcLocks.TabPages.Contains(tpLockerOptions))
                {
                    tbcLocks.TabPages.Add(tpApStatus);
                }
                gpApStatus.Visible = true;
            }
            catch (Exception ex)
            {
                if (tbcLocks.TabPages.Contains(tpLockerOptions))
                {
                    tbcLocks.TabPages.Remove(tpApStatus);
                }
                gpApStatus.Visible = false;
                log.Error("Error occured while executing LoadAPstatus()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void LoadStatus()
        {
            log.LogMethodEntry();
            try
            {
                List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "PASSTECH_LOCKER_FILTER_OPTIONS"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDTOList[0].LookupValueId = -1;
                lookupValuesDTOList[0].LookupValue = "";
                lookupValuesDTOList[0].Description = "<SELECT>";
                bindingSource.DataSource = lookupValuesDTOList;
                cmbStatus.DataSource = bindingSource;
                cmbStatus.ValueMember = "LookupValue";
                cmbStatus.DisplayMember = "Description";

                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadStatus() Method with an Exception:", e);
            }
        }
        private void frmLockerSetup_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            btnClose.Location = new Point((this.Width / 2 - btnClose.Width / 2) - 20, btnClose.Top);
            LoadLockerZones();
            log.LogMethodExit();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }
        void btnZone_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (Control c in flpLockerZones.Controls)
                c.BackgroundImage = btnSampleZone.BackgroundImage;

            (sender as Control).BackgroundImage = Properties.Resources.NewCard;

            LoadPanel((sender as Button).Tag as LockerZonesDTO);
            lockerMake = string.IsNullOrEmpty(((sender as Button).Tag as LockerZonesDTO).LockerMake) ? ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOCKER_LOCK_MAKE") : ((sender as Button).Tag as LockerZonesDTO).LockerMake;
            lblLockerType.Text = lockerMake == "NONE" ? "MANUAL" : lockerMake;
            zoneCode = ((sender as Button).Tag as LockerZonesDTO).ZoneCode;
            lockerZoneMode = ((sender as Button).Tag as LockerZonesDTO).LockerMode;
            log.LogMethodExit();
        }
        void btnPanel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            cmbStatus.SelectedValue = "";
            foreach (Control c in flpLockerPanels.Controls)
                c.BackgroundImage = btnSamplePanel.BackgroundImage;

            (sender as Control).BackgroundImage = Properties.Resources.NewCard;
            LoadLocker((sender as Button).Tag as LockerPanelDTO, "");
            log.LogMethodExit();
        }
        class LockerDetails
        {
            public LockerDTO lockerDTO;
            public string ZoneCode = "ALL";
            public string LockerMode = "";
        }

        void buttonSample_Click(object sender, EventArgs e)//Starts:Online Locker 10-08-2017
        {
            log.LogMethodEntry();
            Control c = sender as Control;
            LockerDetails lockerdetail = c.Tag as LockerDetails;
            txtCardNumber.Visible = false;
            cmbStatus.SelectedValue = "";
            if (lockerdetail.lockerDTO == null)
            {
                messageBoxDelegate(utilities.MessageUtils.getMessage(1814), "Locker", MessageBoxButtons.OK);
                log.LogMethodExit();
                return;
            }
            if (_forAllocation)
            {
                Form f = c.FindForm();
                if (isReassign)
                {
                    f.Tag = lockerdetail;
                }
                else
                {
                    f.Tag = lockerdetail.lockerDTO;
                }

                f.DialogResult = DialogResult.OK;
                f.Close();
            }
            else
            {
                if (isLockerReturn && lockerMake.Equals("NONE"))
                {
                    this.Tag = lockerdetail.lockerDTO;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    log.LogMethodExit();
                    return;
                }
                tbcLocks.Visible = true;
                if (!tbcLocks.TabPages.Contains(tpLockerOptions))
                {
                    tbcLocks.TabPages.Insert(0, tpLockerOptions);
                    tbcLocks.Refresh();
                }
                tbcLocks.SelectedTab = tpLockerOptions;
                if (lockerdetail.lockerDTO.LockerStatus.Equals("B"))
                {
                    btnBlockLock.Text = utilities.MessageUtils.getMessage("Unblock Lock");
                    btnBlockLock.Tag = "U";
                }
                else
                {
                    btnBlockLock.Text = utilities.MessageUtils.getMessage("Block Lock");
                    btnBlockLock.Tag = "B";
                }
                GetLockerDetail(lockerdetail);
            }
            log.LogMethodExit();
        }

        LockerDetails selectedLockerDetail;
        private void GetLockerDetail(LockerDetails lockerDetail)
        {
            log.LogMethodEntry(lockerDetail);
            DateTime serverTime = utilities.getServerTime();
            LockerAllocationDTO lockerAllocationDTO = null;
            if (lockerDetail == null)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage("Invalid locker details."));
                log.Info("Ends-viewLockers() as Invalid locker details. ");
                log.LogMethodExit();
                return;
            }
            if (lockerDetail.lockerDTO != null && lockerDetail.lockerDTO.LockerId >= 0)
            {
                lblLockerName.Text = lockerDetail.lockerDTO.LockerName;
                lblLockerNumber.Tag = lockerDetail.lockerDTO.LockerId;
                ParafaitLockCardHandler locker;
                if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()) || lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                    locker = new MetraLockCardHandler(null, utilities.ExecutionContext);
                else
                    locker = new ParafaitLockCardHandler(null, utilities.ExecutionContext);
                string zoneCode = (string.IsNullOrEmpty(lockerDetail.ZoneCode)) ? "0" : lockerDetail.ZoneCode;
                lblLockerNumber.Text = locker.GetLockerNumber(zoneCode, lockerDetail.lockerDTO.Identifier.ToString());
                if (lockerDetail.lockerDTO.LockerAllocated != null && lockerDetail.lockerDTO.LockerAllocated.Id > -1)
                {
                    lockerAllocationDTO = lockerDetail.lockerDTO.LockerAllocated;
                }
                else
                {
                    LockerAllocationList lockerAllocationList = new LockerAllocationList();
                    List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>> lockerAllocationSearchParams = new List<KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>>();
                    lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.LOCKER_ID, lockerDetail.lockerDTO.LockerId.ToString()));
                    lockerAllocationSearchParams.Add(new KeyValuePair<LockerAllocationDTO.SearchByLockerAllocationParameters, string>(LockerAllocationDTO.SearchByLockerAllocationParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    List<LockerAllocationDTO> lockerAllocationDTOList = lockerAllocationList.GetAllLockerAllocations(lockerAllocationSearchParams);
                    if (lockerAllocationDTOList != null && lockerAllocationDTOList.Count > 0)
                    {
                        lockerAllocationDTO = lockerAllocationDTOList[0];
                    }
                }
                if (lockerAllocationDTO != null)
                {
                    lblCardNumber.Text = lockerAllocationDTO.CardNumber;
                    if (IsOnlineLockerCardBlocked(lblCardNumber.Text))
                    {
                        btnBlockCard.Text = "Unblock Card";
                        btnBlockCard.Tag = "U";
                    }
                    else
                    {
                        btnBlockCard.Text = "Block Card";
                        btnBlockCard.Tag = "B";
                    }
                    lblIssuedBy.Text = lockerAllocationDTO.IssuedBy;
                    lblIssuedTime.Text = lockerAllocationDTO.IssuedTime.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    lblValidFrom.Text = lockerAllocationDTO.ValidFromTime.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    lblValidTo.Text = lockerAllocationDTO.ValidToTime.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                    lblBatteryStatus.Text = ((lockerDetail.lockerDTO.BatteryStatus == 0) ? "Normal" : (lockerDetail.lockerDTO.BatteryStatus == 1) ? "Low" : "Normal");
                    if (lockerAllocationDTO.Refunded)
                    {
                        lblReturned.Text = "Yes";
                    }
                    else
                        lblReturned.Text = "No";
                    selectedLockerDetail = lockerDetail;

                    if (!gpLockerOption.Visible)
                    {
                        gpLockerOption.Visible = true;
                    }
                    if (serverTime.CompareTo(lockerAllocationDTO.ValidFromTime) > 0
                        && serverTime.CompareTo(lockerAllocationDTO.ValidToTime) < 0
                        && !lockerAllocationDTO.Refunded)
                    {
                        btnRelease.Enabled = true;
                        btnReassign.Enabled = true;
                    }
                    else
                    {
                        btnRelease.Enabled = false;
                        btnReassign.Enabled = false;
                    }
                    if (utilities.getParafaitDefaults("IS_ONLINE_OPTION_ENABLED").Equals("Y") && !string.IsNullOrEmpty(onlineServiceUrl))
                    {
                        if (!string.IsNullOrEmpty(lblLockerNumber.Text))
                        {
                            btnBlockLock.Enabled = btnOpenLocker.Enabled = true;
                        }
                        else
                        {
                            btnBlockLock.Enabled = btnOpenLocker.Enabled = false;
                        }
                        if (!string.IsNullOrEmpty(lblCardNumber.Text))
                        {
                            btnBlockCard.Enabled = true;
                        }
                        else
                        {
                            btnBlockCard.Enabled = false;
                        }
                    }
                    else
                    {
                        btnBlockCard.Enabled = btnBlockLock.Enabled = btnOpenLocker.Enabled = false;
                    }
                }
                else
                {
                    gpLockerOption.Visible = false;
                    MessageBox.Show(utilities.MessageUtils.getMessage(1241));
                    log.Info("Ends-viewLockers() as No allocation data found for this Locker ");//Added for logger function on 08-Mar-2016
                    log.LogMethodExit();
                    return;
                }
                utilities.setLanguage(this);
            }
            else
            {
                gpLockerOption.Visible = false;
                MessageBox.Show(utilities.MessageUtils.getMessage("Locker data not found."));
                log.Info("Ends-viewLockers() as Locker data not found. ");
                log.LogMethodExit();
                return;
            }
            log.LogMethodExit();
        }

        void addRow(int rowCount)
        {
            log.LogMethodEntry();//Added for logger function on 08-Mar-2016
            for (int j = 0; j < rowCount; j++)
            {
                tblLockers.RowCount++;
                tblLockers.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));
            }
            log.Debug("Ends-addRow(" + rowCount + ")");//Added for logger function on 08-Mar-2016            
        }

        void addColumn(int colCount)
        {
            log.Debug("Starts-addColumn(" + colCount + ")");//Added for logger function on 08-Mar-2016    
            for (int j = 0; j < colCount; j++)
            {
                tblLockers.ColumnCount++;
                tblLockers.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            }

            for (int i = 0; i < tblLockers.ColumnCount; i++)
            {
                for (int j = 0; j < tblLockers.RowCount; j++)
                {
                    LockerDetails locker = new LockerDetails();
                    Button buttonSample = new Button();
                    //buttonSample.BackgroundImage = Properties.Resources.green_76x76;
                    buttonSample.FlatAppearance.BorderSize = 0;
                    buttonSample.FlatAppearance.CheckedBackColor = Color.Transparent;
                    buttonSample.FlatAppearance.MouseDownBackColor = Color.Transparent;
                    buttonSample.FlatAppearance.MouseOverBackColor = Color.Transparent;
                    buttonSample.FlatStyle = FlatStyle.Flat;
                    buttonSample.Size = new Size(76, 76);
                    buttonSample.BackgroundImageLayout = ImageLayout.Stretch;
                    buttonSample.Click += buttonSample_Click;
                    buttonSample.Tag = locker;
                    tblLockers.Controls.Add(buttonSample, i, j);
                }
            }
            log.Debug("Ends-addColumn(" + colCount + ")");//Added for logger function on 08-Mar-2016
        }

        private void btnOpenLocker_Click(object sender, EventArgs e)//Starts:Online Locker 10-08-2017
        {
            log.LogMethodEntry();
            int mgrId = -1;
            ParafaitLockCardHandler locker;
            if (authenticateManagerDelegate(ref mgrId))
            {
                string lockerid = "";
                if (string.IsNullOrEmpty(onlineServiceUrl))
                {
                    ShowMessageBox(utilities.MessageUtils.getMessage(1813), "Open Locker", MessageBoxButtons.OK);
                    log.LogMethodExit();
                    return;
                }
                if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                    locker = new PassTechLockCardHandler(null, utilities.ExecutionContext);
                else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()) || lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                    locker = new MetraLockCardHandler(null, utilities.ExecutionContext);
                else
                {
                    ShowMessageBox(utilities.MessageUtils.getMessage(1815, LockerMake) + utilities.MessageUtils.getMessage("open locker feature."), "Open Locker", MessageBoxButtons.OK);
                    log.LogMethodExit();
                    return;
                }
                try
                {
                    if (string.IsNullOrEmpty(lblLockerNumber.Text))
                    {
                        ShowMessageBox(utilities.MessageUtils.getMessage(1235), "Open Locker", MessageBoxButtons.OK);
                        log.LogMethodExit();
                        return;
                    }
                    else
                    {
                        lockerid = lblLockerNumber.Text.PadLeft(5, '0');
                    }
                    List<String> lockerList = new List<string>();
                    lockerList.Add(lockerid);
                    if (locker.SendOnlineCommand(onlineServiceUrl, RequestType.OPEN_LOCK, lockerList, null, zoneCode, lockerMake))
                    {
                        messageBoxDelegate(utilities.MessageUtils.getMessage(1236), "Open Locker", MessageBoxButtons.OK);
                        LockerLog.POSLockerLogMessage(Convert.ToInt32(lblLockerNumber.Tag), "OPEN LOCK", "To Locker " + lblLockerNumber.Text + " sent an open request.", "SUCCESS");
                        //MessageBox.Show(utilities.MessageUtils.getMessage(1236));
                        log.Debug("OpenLocker success");
                    }
                    else
                    {
                        messageBoxDelegate(utilities.MessageUtils.getMessage(1237), "Open Locker", MessageBoxButtons.OK);
                        //MessageBox.Show(utilities.MessageUtils.getMessage(1237));
                        log.Debug("OpenLocker failed");
                    }
                }
                catch (Exception ex)
                {
                    messageBoxDelegate(utilities.MessageUtils.getMessage(1234) + ex.Message, "Open Locker", MessageBoxButtons.OK);
                    //MessageBox.Show(utilities.MessageUtils.getMessage(1234) + ex.Message);
                    log.Debug("Ends-btnOpenLocker_Click");
                }
            }
            log.LogMethodExit();
        }//Ends:Online Locker 10-08-2017

        private void btnBlockLock_Click(object sender, EventArgs e)//Starts:Online Locker 10-08-2017
        {
            log.LogMethodEntry();
            int mgrId = -1;
            ParafaitLockCardHandler locker;
            string requestType = (btnBlockLock.Tag.ToString().Equals("B")) ? "Block" : "Unbock";
            if (authenticateManagerDelegate(ref mgrId))
            {
                if (string.IsNullOrEmpty(onlineServiceUrl))
                {
                    ShowMessageBox(utilities.MessageUtils.getMessage(1813), "Block Lock", MessageBoxButtons.OK);
                    log.LogMethodExit();
                    return;
                }
                if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                {
                    locker = new PassTechLockCardHandler(null, utilities.ExecutionContext);
                }
                else
                {
                    ShowMessageBox(utilities.MessageUtils.getMessage(1815, LockerMake) + utilities.MessageUtils.getMessage("block lock feature."), "Block Locker", MessageBoxButtons.OK);
                    log.LogMethodExit();
                    return;
                }
                try
                {
                    string lockerid = "";
                    if (string.IsNullOrEmpty(lblLockerNumber.Text))
                    {
                        ShowMessageBox(utilities.MessageUtils.getMessage(1231), "Block Lock", MessageBoxButtons.OK);
                        log.LogMethodExit();
                        return;
                    }
                    else
                    {
                        lockerid = lblLockerNumber.Text.PadLeft(5, '0');
                    }
                    List<String> lockerList = new List<string>();//Starts: 2017-sep-09 Online locker changes
                    lockerList.Add(lockerid);
                    if (locker.SendOnlineCommand(onlineServiceUrl, (btnBlockLock.Tag.ToString().Equals("B")) ? RequestType.BLOCK_LOCK : RequestType.UNBLOCK_LOCK, lockerList, null, null, lockerMake))//Ends:2017-sep-09 Online locker changes
                    {
                        messageBoxDelegate(utilities.MessageUtils.getMessage(1232, requestType), "Block Locker", MessageBoxButtons.OK);
                        //MessageBox.Show(utilities.MessageUtils.getMessage(1232, requestType));
                        log.Debug(requestType + " Locker success");
                        if (btnBlockLock.Tag.ToString().Equals("B"))
                        {
                            btnBlockLock.Text = utilities.MessageUtils.getMessage("Unblock Locker");
                            btnBlockLock.Tag = "U";
                            LockerLog.POSLockerLogMessage(Convert.ToInt32(lblLockerNumber.Tag), "BlockLock", "Locker " + lblLockerNumber.Text + " has blocked", "SUCCESS");
                        }
                        else
                        {
                            btnBlockLock.Text = utilities.MessageUtils.getMessage("Block Locker");
                            btnBlockLock.Tag = "B";
                            LockerLog.POSLockerLogMessage(Convert.ToInt32(lblLockerNumber.Tag), "BlockLock", "Locker " + lblLockerNumber.Text + " has unblocked", "SUCCESS");
                        }
                        btnBlockLock.Refresh();
                    }
                    else
                    {
                        messageBoxDelegate(utilities.MessageUtils.getMessage(1232, requestType), "Block Locker", MessageBoxButtons.OK);
                        //MessageBox.Show(utilities.MessageUtils.getMessage(1233, requestType));
                        log.Debug(requestType + " Locker failed");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1234) + ex.Message);
                    log.Debug("Ends-btnBlockLock_Click exception occured :" + ex.ToString());
                }
            }
            log.LogMethodExit();
        }//Ends:Online Locker 10-08-2017

        private void btnReassign_Click(object sender, EventArgs e)//Starts:Online Locker 10-08-2017
        {
            log.LogMethodEntry();//Added for logger function on 08-Mar-2016
            if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()))
            {
                ShowMessageBox(utilities.MessageUtils.getMessage(1815, LockerMake) + utilities.MessageUtils.getMessage("Reassign feature."), "Reassign Locker", MessageBoxButtons.OK);
                log.LogMethodExit();
                return;
            }
            if (messageBoxDelegate(utilities.MessageUtils.getMessage(1242), "Reassign Locker?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)//(POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(1242), "Reassign Locker?") == System.Windows.Forms.DialogResult.Yes)
            {
                if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()) && string.IsNullOrEmpty(selectedLockerDetail.lockerDTO.ExternalIdentifier))
                {
                    ShowMessageBox(utilities.MessageUtils.getMessage(5253, selectedLockerDetail.lockerDTO.LockerName), "Reassign Locker", MessageBoxButtons.OK);
                    log.LogMethodExit();
                    return;
                }
                if (tappedcard == null && !lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.NONE.ToString()))
                {
                    messageBoxDelegate(utilities.MessageUtils.getMessage(1731), "Locker reassign", MessageBoxButtons.OK);
                    return;
                }
                else if (tappedcard != null)
                {
                    LockerAllocation lockerAllocation = new LockerAllocation();
                    lockerAllocation.LoadAllocationByCardId(tappedcard.card_id);
                    if (lockerAllocation.GetLockerAllocationDTO == null || (lockerAllocation.GetLockerAllocationDTO != null && lockerAllocation.GetLockerAllocationDTO.LockerId != selectedLockerDetail.lockerDTO.LockerId))
                    {
                        messageBoxDelegate(utilities.MessageUtils.getMessage(1732), "Locker reassign", MessageBoxButtons.OK);
                        return;
                    }
                }
                int mgrId = -1;
                if (authenticateManagerDelegate(ref mgrId))
                {
                    LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Reassign", "Manager id:" + mgrId + " has approved for reassignment", "SUCCESS");
                    try
                    {
                        frmLockerSetup frml = new frmLockerSetup(utilities, zoneId, primaryCardReader, messageBoxDelegate, authenticateManagerDelegate, true);
                        frml.WindowState = FormWindowState.Maximized;
                        frml.isReassign = true;
                        frml.TopMost = true;
                        frml.mode = selectedLockerDetail.LockerMode;
                        if (frml.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            frml.isReassign = false;
                            //int LockerNumber = Convert.ToInt32(frml.Tag);
                            LockerDetails lkr = frml.Tag as LockerDetails;
                            if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()) && string.IsNullOrEmpty(lkr.lockerDTO.ExternalIdentifier))
                            {
                                ShowMessageBox(utilities.MessageUtils.getMessage(5253, lkr.lockerDTO.LockerName), "Reassign Locker", MessageBoxButtons.OK);
                                log.LogMethodExit();
                                return;
                            }
                            using (SqlConnection cnn = utilities.createConnection())
                            {
                                SqlTransaction SQLTrx = cnn.BeginTransaction();
                                try
                                {
                                    //POSCore.Lockers.ParafaitLockerLock.ReturnLockerWithoutCard(Convert.ToInt32(_dt.Rows[0]["LockerAllocationId"]), POSStatic.Utilities, SQLTrx);
                                    ParafaitLockCardHandler.ReturnLockerWithoutCard(selectedLockerDetail.lockerDTO.LockerAllocated, SQLTrx);
                                    ParafaitLockCardHandler locker;

                                    //POSCore.Card card = new POSCore.Card(Common.Devices.PrimaryCardReader, _dt.Rows[0]["CardNumber"].ToString(), POSStatic.Utilities.ParafaitEnv.LoginID, POSStatic.Utilities);
                                    Card card = new Card(primaryCardReader, selectedLockerDetail.lockerDTO.LockerAllocated.CardNumber, utilities.ParafaitEnv.LoginID, utilities);

                                    if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.COCY.ToString()))
                                        locker = new CocyLockCardHandler(card.ReaderDevice, utilities.ExecutionContext, Convert.ToByte(utilities.ParafaitEnv.MifareCustomerKey));
                                    else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.INNOVATE.ToString()))
                                        locker = new InnovateLockCardHandler(card.ReaderDevice, utilities.ExecutionContext, Convert.ToByte(utilities.ParafaitEnv.MifareCustomerKey), card.CardNumber);
                                    else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                                        locker = new PassTechLockCardHandler(card.ReaderDevice, utilities.ExecutionContext);
                                    else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()) || lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                                        locker = new MetraLockCardHandler(card.ReaderDevice, utilities.ExecutionContext, card.CardNumber, selectedLockerDetail.lockerDTO.Identifier.ToString(), zoneCode, lockerMake, selectedLockerDetail.LockerMode);
                                    else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()))
                                        locker = new HecereLockCardHandler(card.ReaderDevice, utilities.ExecutionContext, card.CardNumber);
                                    else
                                        locker = new ParafaitLockCardHandler(card.ReaderDevice, utilities.ExecutionContext);
                                    lkr.lockerDTO.LockerAllocated = selectedLockerDetail.lockerDTO.LockerAllocated;
                                    lkr.lockerDTO.LockerAllocated.LockerId = lkr.lockerDTO.LockerId;
                                    lkr.lockerDTO.LockerAllocated.ZoneCode = lkr.ZoneCode;
                                    if (lkr.lockerDTO.LockerAllocated.ValidToTime.Equals(DateTime.MinValue))
                                    {
                                        lkr.lockerDTO.LockerAllocated.ValidToTime = lkr.lockerDTO.LockerAllocated.IssuedTime.AddHours(24);
                                    }
                                    lkr.lockerDTO.LockerAllocated.Refunded = false;
                                    locker.CreateLockerCard(lkr.lockerDTO.LockerAllocated, lkr.LockerMode, SQLTrx, zoneCode, lockerMake);
                                    SQLTrx.Commit();
                                    LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Reassign", "Locker " + selectedLockerDetail.lockerDTO.Identifier + "(ID:" + selectedLockerDetail.lockerDTO.LockerId + ") is reassigned to locker " + lkr.lockerDTO.Identifier + " (ID:" + lkr.lockerDTO.LockerId + ".)", "SUCCESS");
                                    utilities.EventLog.logEvent("Locker", 'D', utilities.ParafaitEnv.LoginID, "Reassign Locker; Allocation Id: " + lkr.lockerDTO.LockerAllocated.Id.ToString() + "; From Locker Number " + selectedLockerDetail.lockerDTO.Identifier + " To " + lkr.lockerDTO.Identifier, "", 0, "Manager Id", mgrId.ToString(), null);
                                    this.WindowState = FormWindowState.Normal;
                                    this.TopMost = false;
                                    Print(lkr.lockerDTO.LockerAllocated.TrxId, card);

                                    Close();
                                }
                                catch (Exception ex)
                                {
                                    SQLTrx.Rollback();
                                    messageBoxDelegate(ex.Message, "Reasign Locker");
                                    LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Reassign", "Error:" + ex.Message, "ERROR");
                                    //POSUtils.ParafaitMessageBox(ex.Message);
                                    log.Fatal("Ends-btnReassign_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        messageBoxDelegate(ex.Message, "Reasign Locker");
                        LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Reassign", "Error:" + ex.Message, "ERROR");
                        //POSUtils.ParafaitMessageBox(ex.Message);
                        log.Fatal("Ends-btnReassign_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    }
                }
                else
                {
                    messageBoxDelegate(utilities.MessageUtils.getMessage(268), "Reasign Locker");
                    LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Reassign", "Manager approval failed.", "ERROR");
                    //POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(268));
                    log.Warn("btnReassign_Click() - Manager Approval Required for this Task");//Added for logger function on 08-Mar-2016
                }
            }
            log.LogMethodExit();//Added for logger function on 08-Mar-2016
        }//Ends:Online Locker 10-08-2017
        private void Print(int transactionId, Card card)
        {
            log.LogMethodEntry(transactionId, card);
            try
            {
                int lockerReassignTemplateId = Convert.ToInt32(string.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOCKER_REASSIGN_TEMPLATE")) ? "-1" : ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "LOCKER_REASSIGN_TEMPLATE"));
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                if (lockerReassignTemplateId != -1)
                {
                    if (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TRX_AUTO_PRINT_AFTER_SAVE").Equals("Y")
                        || (ParafaitDefaultContainerList.GetParafaitDefault(utilities.ExecutionContext, "TRX_AUTO_PRINT_AFTER_SAVE").Equals("A") && messageBoxDelegate(utilities.MessageUtils.getMessage(2728, utilities.MessageUtils.getMessage("Locker Reassign Receipt")), "Locker Reassign Receipt", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        //Transaction transaction = transactionUtils.CreateTransactionFromDB(transactionId, utilities);
                        System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
                        POSMachines posMachine = new POSMachines(utilities.ExecutionContext, utilities.ParafaitEnv.POSMachineId);
                        List<POSPrinterDTO> POSPrintersDTOList = posMachine.PopulatePrinterDetails();
                        POSPrinterDTO posPrinterDTO = POSPrintersDTOList.Find(x => x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter);
                        printDocument.PrintPage += (object printSender, PrintPageEventArgs printEvent) =>
                        {
                            POSPrint.PrintCardBalanceReceipt(card, lockerReassignTemplateId, posPrinterDTO, utilities, printEvent);
                        };
                        printDocument.Print();
                    }
                }
                else
                {
                    log.Info("LOCKER_REASSIGN_TEMPLATE configuration is not set");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error while printing reassing receipt.", ex);
            }
            log.LogMethodExit();
        }
        private void btnRelease_Click(object sender, EventArgs e)//Starts:Online Locker 10-08-2017
        {
            log.LogMethodEntry();//Added for logger function on 08-Mar-2016

            if (messageBoxDelegate(utilities.MessageUtils.getMessage(1243), "Release Locker?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)//(POSUtils.ParafaitMessageBox(utilities.MessageUtils.getMessage(1243), "Release Locker?") == System.Windows.Forms.DialogResult.Yes)
            {
                if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()) && string.IsNullOrEmpty(selectedLockerDetail.lockerDTO.ExternalIdentifier))
                {
                    ShowMessageBox(utilities.MessageUtils.getMessage(5253, selectedLockerDetail.lockerDTO.LockerName), "Release Locker", MessageBoxButtons.OK);
                    log.LogMethodExit();
                    return;
                }
                int mgrId = -1;
                if (authenticateManagerDelegate(ref mgrId))
                {
                    LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Release", "Manager id:" + mgrId + " has approved for release", "SUCCESS");
                    try
                    {
                        if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.HECERE.ToString()))
                        {

                            Card card = new Card(primaryCardReader, selectedLockerDetail.lockerDTO.LockerAllocated.CardNumber, utilities.ParafaitEnv.LoginID, utilities);
                            ParafaitLockCardHandler locker;
                            locker = new HecereLockCardHandler(card.ReaderDevice, utilities.ExecutionContext, card.CardNumber);
                            locker.SetAllocation(selectedLockerDetail.lockerDTO.LockerAllocated);
                            locker.ReturnLocker(null);
                        }
                        else
                        {
                            ParafaitLockCardHandler.ReturnLockerWithoutCard(selectedLockerDetail.lockerDTO.LockerAllocated);
                        }
                        LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Release", "Locker is released.", "SUCCESS");
                        utilities.EventLog.logEvent("Locker", 'D', utilities.ParafaitEnv.LoginID, "Locker released; Allocation Id: " + selectedLockerDetail.lockerDTO.LockerAllocated.Id, "", 0, "Manager Id", mgrId.ToString(), null);//Added to log the user on Dec-9-2015//
                        Close();
                    }
                    catch (Exception ex)
                    {
                        messageBoxDelegate(ex.Message, "Release Locker");
                        LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Release", "Error:" + ex.Message, "ERROR");
                        //POSUtils.ParafaitMessageBox(ex.Message);
                        log.Fatal("Ends-btnRelease_Click() due to exception " + ex.Message);//Added for logger function on 08-Mar-2016
                    }
                }
                else
                {
                    messageBoxDelegate(utilities.MessageUtils.getMessage(268), "Release Locker");
                    LockerLog.POSLockerLogMessage(selectedLockerDetail.lockerDTO.LockerId, "Release", "Manager approval failed.", "ERROR");
                    //POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(268));
                    log.Info("btnRelease_Click() - Manager Approval Required for this Task");//Added for logger function on 08-Mar-2016   
                }
            }
            log.LogMethodExit();//Added for logger function on 08-Mar-2016
        }//Ends:Online Locker 10-08-2017

        private void btnBlockCard_Click(object sender, EventArgs e)//Starts:Online Locker 10-08-2017
        {
            log.LogMethodEntry();
            int mgrId = -1;
            string requestType = (btnBlockCard.Tag.ToString().Equals("B")) ? "Block" : "Unbock";
            LockerBlockedCardsDTO lockerBlockedCardsDTO;
            LockerBlockedCards lockerBlockedCards;
            ParafaitLockCardHandler locker;
            if (authenticateManagerDelegate(ref mgrId))
            {
                if (string.IsNullOrEmpty(onlineServiceUrl))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1813));
                    log.LogMethodExit();
                    return;
                }
                if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                    locker = new PassTechLockCardHandler(null, utilities.ExecutionContext);
                else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()) && lockerZoneMode.Equals("FIXED"))
                    locker = new MetraLockCardHandler(null, utilities.ExecutionContext, null, zoneCode, null, lockerMake, lockerZoneMode);
                else
                {
                    ShowMessageBox(utilities.MessageUtils.getMessage(1815, LockerMake) + utilities.MessageUtils.getMessage("block card feature."), "Block Card", MessageBoxButtons.OK);
                    log.LogMethodExit();
                    return;
                }
                try
                {
                    string cardId = "";
                    if (string.IsNullOrEmpty(lblCardNumber.Text))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1238));
                    }
                    else
                    {
                        cardId = lblCardNumber.Text.PadRight(10, '0');//Starts:2017-Sep-07 Online locker changes
                    }
                    List<String> cardList = new List<string>();
                    cardList.Add(cardId);
                    if (locker.SendOnlineCommand(onlineServiceUrl, (btnBlockCard.Tag.ToString().Equals("B")) ? RequestType.BLOCK_CARD : RequestType.UNBLOCK_CARD, null, cardList, null, lockerMake))
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1239, requestType));
                        log.Debug("Block Locker success");
                        if (btnBlockCard.Tag.ToString().Equals("B"))
                        {
                            lockerBlockedCardsDTO = new LockerBlockedCardsDTO();
                            lockerBlockedCardsDTO.CardNumber = lblCardNumber.Text;
                            lockerBlockedCards = new LockerBlockedCards(lockerBlockedCardsDTO);
                            lockerBlockedCards.Save();
                            btnBlockCard.Text = utilities.MessageUtils.getMessage("Unblock Card");
                            btnBlockCard.Tag = "U";
                            LockerLog.POSLockerLogMessage(-1, "CardBlock", "Card " + lockerBlockedCardsDTO.CardNumber + " has blocked", "SUCCESS");

                        }
                        else
                        {
                            lockerBlockedCards = new LockerBlockedCards();
                            lockerBlockedCardsDTO = lockerBlockedCards.GetBlockedCard(lblCardNumber.Text);
                            lockerBlockedCardsDTO.IsActive = false;
                            lockerBlockedCards = new LockerBlockedCards(lockerBlockedCardsDTO);
                            lockerBlockedCards.Save();
                            //lockerBlockedCards.RemoveBlockedCard(lblCardNumber.Text);
                            btnBlockCard.Text = utilities.MessageUtils.getMessage("Block Card");
                            btnBlockCard.Tag = "B";
                            LockerLog.POSLockerLogMessage(-1, "CardBlock", "Card " + lockerBlockedCardsDTO.CardNumber + " has unblocked", "SUCCESS");
                        }
                        btnBlockCard.Refresh();//Ends: Online Locker 2017-Aug-17
                    }
                    else
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1240, requestType));
                        log.Debug("Block Locker failed");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1234) + ex.Message);
                    log.Debug("Ends-btnBlockLock_Click exception occured :" + ex.ToString());
                }
            }
            log.LogMethodExit();
        }//Ends:Online Locker 10-08-2017

        private void btnSearch_Click(object sender, EventArgs e)//Starts:Online Locker 10-08-2017
        {
            log.LogMethodEntry();
            gpLockerOption.Visible = false; //Online Locker 2017-Aug-17
            if (cmbStatus.SelectedValue != null && !string.IsNullOrEmpty(cmbStatus.SelectedValue.ToString()))
            {

                switch (cmbStatus.SelectedValue.ToString())
                {
                    case "O":
                        LoadLocker(selectedLockerPanelDTO, "O", -1);
                        break;
                    case "C":
                        LoadLocker(selectedLockerPanelDTO, "C", -1);
                        break;
                    case "B":
                        LoadLocker(selectedLockerPanelDTO, "B", -1);
                        break;
                    case "L":
                        LoadLocker(selectedLockerPanelDTO, "", 1);
                        break;
                    case "N":
                        LoadLocker(selectedLockerPanelDTO, "", 0);
                        break;
                    case "CN":
                        if (string.IsNullOrEmpty(txtCardNumber.Text))
                        {
                            messageBoxDelegate(utilities.MessageUtils.getMessage(172), "Invalid card", MessageBoxButtons.OK);
                            return;
                        }
                        else
                        {
                            Card card = new Card(txtCardNumber.Text, "", utilities);
                            LockerAllocation lockerAllocation = new LockerAllocation();
                            lockerAllocation.LoadAllocationByCardId(card.card_id);
                            if (lockerAllocation.GetLockerAllocationDTO != null && lockerAllocation.GetLockerAllocationDTO.Id > -1)
                            {
                                btnReassign.Enabled = btnRelease.Enabled = false;
                                if (lockerAllocation.GetLockerAllocationDTO.LockerId > -1)
                                {
                                    Locker locker = new Locker(lockerAllocation.GetLockerAllocationDTO.LockerId);
                                    LockerPanel panel = new LockerPanel(machineUserContext, locker.getLockerDTO.PanelId);
                                    LockerZones zones = new LockerZones(machineUserContext, panel.getLockerPanelDTO.ZoneId);
                                    lblLockerName.Text = locker.getLockerDTO.LockerName;
                                    ParafaitLockCardHandler lockerCardHandler;
                                    if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()) || lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                                        lockerCardHandler = new MetraLockCardHandler(null, utilities.ExecutionContext);
                                    else
                                        lockerCardHandler = new ParafaitLockCardHandler(null, utilities.ExecutionContext);
                                    string zoneCode = (string.IsNullOrEmpty(zones.GetLockerZonesDTO.ZoneCode)) ? "0" : zones.GetLockerZonesDTO.ZoneCode;
                                    lblLockerNumber.Text = lockerCardHandler.GetLockerNumber(zoneCode, locker.getLockerDTO.Identifier.ToString());
                                    lblCardNumber.Text = lockerAllocation.GetLockerAllocationDTO.CardNumber;
                                    if (IsOnlineLockerCardBlocked(lblCardNumber.Text))
                                    {
                                        btnBlockCard.Text = "Unblock Card";
                                        btnBlockCard.Tag = "U";
                                    }
                                    else
                                    {
                                        btnBlockCard.Text = "Block Card";
                                        btnBlockCard.Tag = "B";
                                    }//Ends: Online Locker 2017-Aug-17
                                    if (locker.getLockerDTO.LockerStatus.Equals("B"))
                                    {
                                        btnBlockLock.Text = utilities.MessageUtils.getMessage("Unblock Lock");
                                        btnBlockLock.Tag = "U";
                                    }
                                    else
                                    {
                                        btnBlockLock.Text = utilities.MessageUtils.getMessage("Block Lock");
                                        btnBlockLock.Tag = "B";
                                    }
                                    lblIssuedBy.Text = lockerAllocation.GetLockerAllocationDTO.IssuedBy;
                                    lblIssuedTime.Text = lockerAllocation.GetLockerAllocationDTO.IssuedTime.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                                    lblValidFrom.Text = lockerAllocation.GetLockerAllocationDTO.ValidFromTime.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                                    lblValidTo.Text = lockerAllocation.GetLockerAllocationDTO.ValidToTime.ToString(utilities.ParafaitEnv.DATETIME_FORMAT);
                                    lblBatteryStatus.Text = (locker.getLockerDTO.BatteryStatus == 1) ? "Low" : "Normal";
                                    if (lockerAllocation.GetLockerAllocationDTO.Refunded == true)
                                    {
                                        lblReturned.Text = "Yes";
                                    }
                                    else
                                        lblReturned.Text = "No";
                                    if (!gpLockerOption.Visible)
                                    {
                                        gpLockerOption.Visible = true;
                                    }
                                    DateTime dtServer = utilities.getServerTime();
                                    if (dtServer > lockerAllocation.GetLockerAllocationDTO.ValidFromTime
                                        && dtServer < lockerAllocation.GetLockerAllocationDTO.ValidToTime
                                        && lockerAllocation.GetLockerAllocationDTO.Refunded == false)
                                    {
                                        btnBlockCard.Enabled = btnBlockLock.Enabled = btnOpenLocker.Enabled = true;
                                    }
                                    else
                                    {
                                        if ((string.IsNullOrEmpty(zones.GetLockerZonesDTO.LockerMode) ? utilities.getParafaitDefaults("LOCKER_SELECTION_MODE") : zones.GetLockerZonesDTO.LockerMode).Equals("FREE"))
                                            btnBlockCard.Enabled = btnBlockLock.Enabled = btnOpenLocker.Enabled = true;
                                        else
                                            btnBlockCard.Enabled = btnBlockLock.Enabled = btnOpenLocker.Enabled = false;
                                    }
                                    utilities.setLanguage(this);
                                }
                                tbcLocks.Visible = true;
                                if (!tbcLocks.TabPages.Contains(tpLockerOptions))
                                {
                                    tbcLocks.TabPages.Insert(0, tpLockerOptions);
                                    tbcLocks.Refresh();
                                }
                                tbcLocks.SelectedTab = tpLockerOptions;
                            }
                            else
                            {
                                if (tbcLocks.TabPages.Contains(tpLockerOptions))
                                {
                                    tbcLocks.TabPages.Remove(tpApStatus);
                                }
                                gpLockerOption.Visible = false;
                                messageBoxDelegate(utilities.MessageUtils.getMessage(1241), "Search Options");
                                log.Info("Ends-viewLockers() as No allocation data found for this Locker ");//Added for logger function on 08-Mar-2016
                                return;
                            }
                        }
                        break;
                    default:
                        LoadLocker(selectedLockerPanelDTO, "", -1);
                        break;
                }
            }
            else
            {
                LoadLocker(selectedLockerPanelDTO, "", -1);
            }
            log.LogMethodExit();
        }//Ends:Online Locker 10-08-2017

        private bool IsOnlineLockerCardBlocked(string cardNumber)//Starts: Online Locker 2017-Aug-17
        {
            log.LogMethodEntry(cardNumber);
            try
            {
                LockerBlockedCards lockerBlockedCards = new LockerBlockedCards();
                LockerBlockedCardsDTO lockerBlockedCardsDTO = new LockerBlockedCardsDTO();
                lockerBlockedCardsDTO = lockerBlockedCards.GetBlockedCard(cardNumber);
                if (lockerBlockedCardsDTO != null)
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
            catch (Exception ex)
            {
                log.Error("Ends IsOnlineLockerCardBlocked() method Exception:" + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }//Ends: Online Locker 2017-Aug-17

        private void btnOpenAll_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            int mgrId = -1;
            string message = "";
            int zoneCodeValue = -1;
            ParafaitLockCardHandler locker;
            if (authenticateManagerDelegate(ref mgrId))
            {
                if (string.IsNullOrEmpty(onlineServiceUrl))
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(1813));
                    return;
                }
                if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                    locker = new PassTechLockCardHandler(null, utilities.ExecutionContext);
                else if (lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()) || lockerMake.Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                    locker = new MetraLockCardHandler(null, utilities.ExecutionContext);
                else
                    locker = new ParafaitLockCardHandler(null, utilities.ExecutionContext);
                try
                {
                    List<String> lockerList = new List<string>();//Starts:2017-Aug-07 Online Locker changes
                    foreach (DataGridViewRow dgvr in dgvZone.Rows)
                    {
                        if (Convert.ToBoolean(dgvr.Cells[0].Value).Equals(true))
                        {
                            if (dgvr.Cells["LockerMake"].Value.ToString().Equals(ParafaitLockCardHandlerDTO.LockerMake.PASSTECH.ToString()))
                            {
                                lockerList.Add(dgvr.Cells[4].Value.ToString().PadRight(5, '0'));
                            }
                            if (dgvr.Cells["LockerMake"].Value.ToString().Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS.ToString()) || dgvr.Cells["LockerMake"].Value.ToString().Equals(ParafaitLockCardHandlerDTO.LockerMake.METRA_ELS_NET.ToString()))
                            {
                                lockerZonesDTOList = new List<LockerZonesDTO>();
                                LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
                                List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> lockerZoneSearchParams = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
                                lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ZONE_NAME, dgvr.Cells["ZoneName"].Value.ToString()));
                                lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ZONE_CODE, dgvr.Cells["ZoneCode"].Value.ToString()));
                                lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ACTIVE_FLAG, "1"));
                                lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                                lockerZonesDTOList = lockerZonesList.GetLockerZonesList(lockerZoneSearchParams, false);
                                if (lockerZonesDTOList != null && lockerZonesDTOList.Any())
                                {
                                    IList<LockerPanelDTO> loopLockerPanelDTOList = this.lockerZonesDTOList.FirstOrDefault(x => x.ZoneCode == zoneCode).LockerPanelDTOList;
                                    if (loopLockerPanelDTOList != null && loopLockerPanelDTOList.Any())
                                    {
                                        List<LockerDTO> lockerDTOList = loopLockerPanelDTOList.SelectMany(x => x.LockerDTOList).ToList();
                                        List<int> lockerIdList = lockerDTOList.Select(x => x.Identifier).ToList();
                                        foreach (int id in lockerIdList)
                                        {
                                            lockerList.Add(id.ToString());
                                        }

                                    }
                                }
                            }
                            else
                            {
                                message += dgvr.Cells["ZoneName"].Value + ",";
                            }
                        }

                    }
                    if (message.Length > 2)
                    {
                        messageBoxDelegate(message + utilities.MessageUtils.getMessage(" zones will be ignored."), "Open All Lockers", MessageBoxButtons.OK);
                    }
                    if (lockerList.Count == 0)
                    {
                        messageBoxDelegate(utilities.MessageUtils.getMessage("Please select the zone."), "Open All Lockers", MessageBoxButtons.OK);
                        return;
                    }

                    if (messageBoxDelegate(utilities.MessageUtils.getMessage(1433), "Open All Lockers", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (locker.SendOnlineCommand(onlineServiceUrl, RequestType.OPEN_ALL_LOCK, lockerList, null, zoneCode, lockerMake))
                        {
                            messageBoxDelegate(utilities.MessageUtils.getMessage(1236), "Open All Lockers");
                            log.Debug("OpenALLLocker success");
                        }
                        else
                        {
                            messageBoxDelegate(utilities.MessageUtils.getMessage(1237), "Open All Lockers");
                            log.Debug("OpenAllLocker failed");
                        }
                    }
                }
                catch (Exception ex)
                {
                    messageBoxDelegate(utilities.MessageUtils.getMessage(1234) + ex.Message, "Open All Lockers");
                    log.Debug("Ends-btnOpenAll_Click");
                }
            }
            log.LogMethodExit();
        }

        private void ckbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            foreach (DataGridViewRow dgvr in dgvZone.Rows)
            {
                dgvr.Cells[0].Value = ckbSelectAll.Checked;
            }
            dgvZone.RefreshEdit();
            log.LogMethodExit();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbStatus.SelectedValue.Equals("CN"))
            {
                txtCardNumber.Visible = true;
            }
            else
            {
                txtCardNumber.Text = "";
                txtCardNumber.Visible = false;
            }
            log.LogMethodExit();
        }
        private void ShowMessageBox(string message, string caption, MessageBoxButtons messageBoxButtons)
        {
            log.LogMethodEntry(message, caption, messageBoxButtons);
            if (messageBoxDelegate == null)
            {
                MessageBox.Show(message, caption, messageBoxButtons);
            }
            else
            {
                messageBoxDelegate(message, caption, messageBoxButtons);
            }
            log.LogMethodExit();
        }
    }
}
