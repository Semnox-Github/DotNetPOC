/********************************************************************************************
 * Project Name - frm Locker Management
 * Description  - User interface for frm Locker Management
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019   Deeksha       Added logger methods.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Lockers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// User interface for frmLockerSetupUI
    /// </summary>
    public partial class frmLockerManagementUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Semnox.Core.Utilities.Utilities utilities;
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private List<LookupValuesDTO> lookupValuesDTOList;
        private List<LockerZonesDTO> lockerZonesDTOList;
        private List<LockerPanelDTO> lockerPanelDTOList;
        private List<LockerDTO> lockerDTOList;

        public frmLockerManagementUI(Semnox.Core.Utilities.Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvLockerZones);
            utilities.setupDataGridProperties(ref dgvPanels);
            machineUserContext = ExecutionContext.GetExecutionContext();
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        private void frmLockerSetupUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadZoneCode();
            LoadLockerMode();
            LoadLockerMake();
            LoadParentZone();
            LoadZones();
            LoadLockerZonesDTOList();
            log.LogMethodExit();
        }

        private void LoadParentZone()
        {
            log.LogMethodEntry();
            LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
            //List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> lockerZoneSearchParams = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
            //lockerZoneSearchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            //lockerZonesList = lockerZonesList.GetParentZonesList(machineUserContext.GetSiteId().ToString());
            List<LockerZonesDTO> LockerParentZonesList = lockerZonesList.GetParentZones(false, false);
            if (LockerParentZonesList == null)
            {
                LockerParentZonesList = new List<LockerZonesDTO>();
            }
            LockerParentZonesList.Insert(0, new LockerZonesDTO());
            LockerParentZonesList[0].ZoneId = -1;
            LockerParentZonesList[0].ZoneName = "<SELECT>";
            BindingSource bs = new BindingSource();
            bs.DataSource = LockerParentZonesList;
            parentZoneIdDataGridViewTextBoxColumn.DataSource = bs;
            parentZoneIdDataGridViewTextBoxColumn.ValueMember = "ZoneId";
            parentZoneIdDataGridViewTextBoxColumn.DisplayMember = "ZoneName";
            log.LogMethodExit();
        }
        private void LoadZones()
        {
            log.LogMethodEntry();
            LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
            List<LockerZonesDTO> LockerGetZonesList = lockerZonesList.GetZones(false, false);
            if (LockerGetZonesList == null)
            {
                LockerGetZonesList = new List<LockerZonesDTO>();
            }
            BindingSource bs = new BindingSource();
            LockerGetZonesList.Insert(0, new LockerZonesDTO());
            LockerGetZonesList[0].ZoneId = -1;
            LockerGetZonesList[0].ZoneName = "<SELECT>";
            bs.DataSource = LockerGetZonesList;
            zoneIdPanelDataGridViewTextBoxColumn.DataSource = bs;
            zoneIdPanelDataGridViewTextBoxColumn.ValueMember = "ZoneId";
            zoneIdPanelDataGridViewTextBoxColumn.DisplayMember = "ZoneName";
            log.LogMethodExit();
        }

        private void LoadLockerZonesDTOList()
        {
            log.LogMethodEntry();
            LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
            List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<LockerZonesDTO.SearchByParameters, string>>();
            //searchParams.Add(new KeyValuePair<LockerZonesDTO.SearchByParameters, string>(LockerZonesDTO.SearchByParameters.ACTIVE_FLAG, "1"));
            lockerZonesDTOList = lockerZonesList.GetLockerZonesList(searchParams, true);
            LoadLockerPanelsDTOList(lockerZonesDTOList);
            BindingSource lockerZonesListBS = new BindingSource();
            if (lockerZonesDTOList != null)
            {
                lockerZonesListBS.DataSource = new SortableBindingList<LockerZonesDTO>(lockerZonesDTOList);
            }
            else
            {
                lockerZonesListBS.DataSource = new SortableBindingList<LockerZonesDTO>();
            }
            bsZones.DataSource = lockerZonesListBS;
            log.LogMethodExit();
        }
        private void LoadLockerPanelsDTOList(List<LockerZonesDTO> lockerZonesDTOListonDisplay)
        {
            log.LogMethodEntry(lockerZonesDTOListonDisplay);
            lockerPanelDTOList = new List<LockerPanelDTO>();
            lockerDTOList = new List<LockerDTO>();
            if (lockerZonesDTOListonDisplay != null)
            {
                foreach (LockerZonesDTO lockerZonesDTO in lockerZonesDTOListonDisplay)
                {
                    if (lockerZonesDTO.LockerPanelDTOList != null)
                    {
                        lockerPanelDTOList.AddRange(lockerZonesDTO.LockerPanelDTOList);
                    }
                }
                LoadLockerDTOList();
            }
            BindingSource lockerPanelsListBS = new BindingSource();
            if (lockerPanelDTOList != null)
            {
                lockerPanelsListBS.DataSource = new SortableBindingList<LockerPanelDTO>(lockerPanelDTOList);
            }
            else
            {
                lockerPanelsListBS.DataSource = new SortableBindingList<LockerPanelDTO>();
            }
            bsPanels.DataSource = lockerPanelsListBS;
            log.LogMethodExit();
        }
        private void LoadLockerDTOList()
        {
            log.LogMethodEntry();
            if (lockerPanelDTOList != null)
            {
                foreach (LockerPanelDTO lockerPanelDTO in lockerPanelDTOList)
                {
                    if (lockerPanelDTO.LockerDTOList != null)
                    {
                        lockerDTOList.AddRange(lockerPanelDTO.LockerDTOList);
                    }
                }
            }
            log.LogMethodExit();
        }
        private void LoadLockerMode()
        {
            log.LogMethodEntry();
            DefaultDataTypeBL defaultDataTypeBL = new DefaultDataTypeBL(machineUserContext);
            Dictionary<string, string> lockerMode = defaultDataTypeBL.GetDefaultCustomDataType("Custom10");
            LockerMode.DataSource = new BindingSource(lockerMode, null);
            LockerMode.DisplayMember = "Value";
            LockerMode.ValueMember = "Key";
            log.LogMethodExit();
        }
        private void LoadLockerMake()
        {
            log.LogMethodEntry();
            DefaultDataTypeBL defaultDataTypeBL = new DefaultDataTypeBL(machineUserContext);
            Dictionary<string, string> lockerMode = defaultDataTypeBL.GetDefaultCustomDataType("Custom11");
            LockerMake.DataSource = new BindingSource(lockerMode, null);
            LockerMake.DisplayMember = "Value";
            LockerMake.ValueMember = "Key";
            log.LogMethodExit();
        }

        /// <summary>
        /// Loads the zone code to the comboboxes
        /// </summary>
        private void LoadZoneCode()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "LOCKER_ZONE_CODE"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDTOList == null)
                {
                    lookupValuesDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = lookupValuesDTOList;
                zoneCodeDataGridViewTextBoxColumn.DataSource = bindingSource;
                zoneCodeDataGridViewTextBoxColumn.ValueMember = "LookupValue";
                zoneCodeDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                log.LogMethodExit();
            }
            catch (Exception e)
            {
                log.Error("Ends-LoadZoneCode() Method with an Exception:", e);
            }
        }
        private void dgvLockerZones_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(585, "Locker Zones", e.RowIndex + 1, dgvLockerZones.Columns[e.ColumnIndex].DataPropertyName) + ": " + e.Exception.Message, utilities.MessageUtils.getMessage("Data Error"));
            log.LogMethodExit();
        }
        private void dgvLockerPanels_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(585, "Locker Panels", e.RowIndex + 1, dgvPanels.Columns[e.ColumnIndex].DataPropertyName) + ": " + e.Exception.Message, utilities.MessageUtils.getMessage("Data Error"));
            log.LogMethodExit();
        }
        private void dgvLockerZones_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            log.LogMethodEntry();
            e.Row.Cells["activeFlagDataGridViewCheckBoxColumn"].Value = true;
            log.LogMethodExit();
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadZones();
            LoadLockerZonesDTOList();
            log.LogMethodExit();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                dgvLockerZones.Rows.Remove(dgvLockerZones.CurrentRow);
            }
            catch (Exception ex)
            {
                log.Error("Error while executing btnDelete_Click()" + ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnCardManagement_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            frmLockerCardUtils frmlock = new frmLockerCardUtils(utilities.ReaderDevice, utilities);
            frmlock.StartPosition = FormStartPosition.CenterScreen;
            frmlock.ShowDialog();
            log.LogMethodExit();
        }

        TreeNode[] getChildren(LockerZonesDTO lZDTO)
        {
            log.LogMethodEntry(lZDTO);
            List<LockerZonesDTO> lockerZonesFilterDTOList;
            lockerZonesFilterDTOList = lockerZonesDTOList.Where(x => (bool)(x.ParentZoneId == lZDTO.ZoneId)).ToList<LockerZonesDTO>();
            if (lockerZonesFilterDTOList == null || (lockerZonesFilterDTOList != null && lockerZonesFilterDTOList.Count == 0))
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                TreeNode[] tnCollection = new TreeNode[lockerZonesFilterDTOList.Count];
                for (int i = 0; i < lockerZonesFilterDTOList.Count; i++)
                {
                    tnCollection[i] = new TreeNode(lockerZonesFilterDTOList[i].ZoneName);
                    tnCollection[i].Tag = lockerZonesFilterDTOList[i];
                }
                log.LogMethodExit(tnCollection);
                return (tnCollection);
            }
        }

        TreeNode getNodes(TreeNode rootNode)
        {
            log.LogMethodEntry(rootNode);
            TreeNode[] tn = getChildren((LockerZonesDTO)rootNode.Tag);
            if (tn == null)
            {
                log.LogMethodExit();
                return null;
            }
            else
            {
                foreach (TreeNode tnode in tn)
                {
                    TreeNode node = getNodes(tnode);
                    if (node == null)
                        rootNode.Nodes.Add(tnode);
                    else
                        rootNode.Nodes.Add(node);
                }
                log.LogMethodExit(rootNode);
                return (rootNode);
            }

        }

        private void dgvLockerZones_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex < 0 || dgvLockerZones["zoneNameDataGridViewTextBoxColumn", e.RowIndex].Value == null)
                return;

            populateTree(lockerZonesDTOList[e.RowIndex], dgvLockerZones["zoneNameDataGridViewTextBoxColumn", e.RowIndex].Value);
            log.LogMethodExit();
        }

        void populateTree(object zone, object name)
        {
            log.LogMethodEntry(zone, name);
            tvCategory.Nodes.Clear();
            TreeNode node = new TreeNode(name.ToString());
            node.Tag = zone;
            tvCategory.Nodes.Add(node);

            try
            {
                // utilities.executeDataTable("select * from getLockerZoneList(@zoneId)", new SqlParameter("@zoneId", zoneId));
                LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
                lockerZonesList.GetLockerZonesList(((LockerZonesDTO)zone).ZoneId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(605) + ": " + ex.Message);
                log.LogMethodExit();
                return;
            }
            getNodes(node);

            if (tvCategory.Nodes.Count > 0)
            {
                tvCategory.Nodes[0].ExpandAll();
                tvCategory.Nodes[0].Text = tvCategory.Nodes[0].Text; // reassign to set proper width for text
            }
            log.LogMethodExit();
        }
        private void tcLockers_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (tcLockers.SelectedTab.Equals(tpLockers) && (cmbPanel.DataSource == null || ((BindingSource)cmbPanel.DataSource).Count != ((BindingSource)dgvPanels.DataSource).Count))
            {
                cmbPanel.SelectedIndexChanged -= cmbPanel_SelectedIndexChanged;
                cmbPanel.DisplayMember = "PanelName";
                cmbPanel.ValueMember = "PanelId";
                cmbPanel.DataSource = dgvPanels.DataSource;
                cmbPanel.SelectedIndex = -1;
                tblLockers.Controls.Clear();
                tblLockers.RowCount = tblLockers.ColumnCount = 0;
                cmbPanel.SelectedIndexChanged += cmbPanel_SelectedIndexChanged;
            }
            log.LogMethodExit();
        }

        private void cmbPanel_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (cmbPanel.SelectedValue != null)
            {
                refreshLockers();
            }
            log.LogMethodExit();
        }
        void refreshLockers()
        {
            log.LogMethodEntry();
            tblLockers.Visible = false;
            tblLockers.Controls.Clear();
            tblLockers.RowCount = tblLockers.ColumnCount = 0;
            List<LockerDTO> lockerDTOFilterList = lockerDTOList.Where(x => (bool)(x.PanelId == Convert.ToInt32(cmbPanel.SelectedValue))).ToList<LockerDTO>();
            if (lockerDTOFilterList != null && lockerDTOFilterList.Count > 0)
            {
                int RowCount = lockerDTOFilterList.Max(x => (int)x.RowIndex);
                if (RowCount > 0)
                {
                    int ColCount = lockerDTOFilterList.Max(x => (int)x.ColumnIndex);
                    for (int i = 0; i < ColCount; i++)
                    {
                        addColumn();
                    }
                    for (int j = 0; j < RowCount; j++)
                    {
                        addRow();
                    }
                    foreach (LockerDTO lockerDTO in lockerDTOFilterList)
                    {
                        int row = lockerDTO.RowIndex;
                        int col = lockerDTO.ColumnIndex;
                        LockerControl pnl = tblLockers.GetControlFromPosition(col - 1, row - 1) as LockerControl;
                        Locker locker = pnl.Tag as Locker;
                        locker.lockerDTO = lockerDTO;
                        locker.chkActive.Checked = lockerDTO.IsActive;
                        locker.chkDisable.Checked = lockerDTO.IsDisabled;
                        locker.txtLockerName.Text = lockerDTO.LockerName;
                        locker.txtIdentifier.Text = lockerDTO.Identifier.ToString();
                        locker.txtPositionX.Text =
                            lockerDTO.PositionX != null ? lockerDTO.PositionX.Value.ToString() : string.Empty;
                        locker.txtPositionY.Text =
                            lockerDTO.PositionY != null ? lockerDTO.PositionY.Value.ToString() : string.Empty;
                        pnl.Refresh();
                    }
                    tblLockers.Visible = true;
                }
            }
            log.LogMethodExit();
        }
        void addRow()
        {
            log.LogMethodEntry();
            tblLockers.RowCount++;
            tblLockers.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));

            for (int i = 0; i < tblLockers.ColumnCount; i++)
            {
                Locker locker = new Locker();
                tblLockers.Controls.Add(locker.panelSample, i, tblLockers.RowCount - 1);
            }
            log.LogMethodExit();
        }
        void addColumn()
        {
            log.LogMethodEntry();
            tblLockers.ColumnCount++;
            tblLockers.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));

            for (int j = 0; j < tblLockers.RowCount; j++)
            {
                Locker locker = new Locker();
                tblLockers.Controls.Add(locker.panelSample, tblLockers.ColumnCount - 1, j);
            }
            log.LogMethodExit();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvLockerZones.EndEdit();
            LockerZones lockerZones;
            BindingSource bindingSource = (BindingSource)bsZones.DataSource;
            SortableBindingList<LockerZonesDTO> lockerZonesSortableList = (SortableBindingList<LockerZonesDTO>)bindingSource.DataSource;
            if (lockerZonesSortableList != null)
            {
                for (int i = 0; i < lockerZonesSortableList.Count; i++)
                {
                    if (lockerZonesSortableList[i].IsChanged)
                    {
                        try
                        {
                            if (lockerZonesSortableList[i].ZoneId == -1)
                            {
                                if (!string.IsNullOrEmpty(lockerZonesSortableList[i].LockerMake))
                                {
                                    utilities.EventLog.logEvent("ZoneLevelLockerMake", 'D', lockerZonesSortableList[i].LockerMake, "Locker make set to " + lockerZonesSortableList[i].LockerMake + " for the zone " + lockerZonesSortableList[i].ZoneName + "(" + lockerZonesSortableList[i].ZoneId + ")", "CONFIGURATION", 3);
                                }
                                if (!string.IsNullOrEmpty(lockerZonesSortableList[i].LockerMode))
                                {
                                    utilities.EventLog.logEvent("ZoneLevelLockerMode", 'D', lockerZonesSortableList[i].LockerMode, "Locker mode set to " + lockerZonesSortableList[i].LockerMode + " for the zone " + lockerZonesSortableList[i].ZoneName + "(" + lockerZonesSortableList[i].ZoneId + ")", "CONFIGURATION", 3);
                                }
                            }
                            else
                            {
                                if (lockerZonesDTOList != null)
                                {
                                    List<LockerZonesDTO> lockerZonesFilterDTOList;
                                    lockerZonesFilterDTOList = lockerZonesDTOList.Where(x => (bool)(x.ZoneId == lockerZonesSortableList[i].ZoneId)).ToList<LockerZonesDTO>();
                                    if (lockerZonesFilterDTOList != null && lockerZonesFilterDTOList.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(lockerZonesSortableList[i].LockerMake))
                                        {
                                            utilities.EventLog.logEvent("ZoneLevelLockerMake", 'D', lockerZonesSortableList[i].LockerMake, "Locker make set to " + lockerZonesSortableList[i].LockerMake + " for the zone " + lockerZonesSortableList[i].ZoneName + "(" + lockerZonesSortableList[i].ZoneId + ")", "CONFIGURATION", 3);
                                        }
                                        if (!string.IsNullOrEmpty(lockerZonesSortableList[i].LockerMode))
                                        {
                                            utilities.EventLog.logEvent("ZoneLevelLockerMode", 'D', lockerZonesSortableList[i].LockerMode, "Locker mode set to " + lockerZonesSortableList[i].LockerMode + " for the zone " + lockerZonesSortableList[i].ZoneName + "(" + lockerZonesSortableList[i].ZoneId + ")", "CONFIGURATION", 3);
                                        }
                                    }
                                }
                            }

                            lockerZones = new LockerZones(machineUserContext, lockerZonesSortableList[i]);
                            if (!lockerZonesSortableList[i].ActiveFlag && lockerZones.ValidateZoneMapping())
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage("Please remove the reference of zone in product before deactivating it."));
                                return;
                            }
                            lockerZones.Save();
                        }
                        catch (Exception)
                        {
                            log.Error("Error while saving event.");
                            dgvLockerZones.Rows[i].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(718));
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            LoadZones();
            LoadLockerZonesDTOList();
            LockerZonesList lockerZonesList = new LockerZonesList(machineUserContext);
            foreach (LockerZonesDTO lockerZonesDTO in lockerZonesSortableList)
            {
                try
                {
                    lockerZonesList.GetLockerZonesList(lockerZonesDTO.ZoneId);
                }
                catch
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(605));
                    int i = 0;
                    while (i < dgvLockerZones.Rows.Count)
                    {
                        if (dgvLockerZones["zoneIdDataGridViewTextBoxColumn", i].Value.Equals(lockerZonesDTO.ZoneId))
                        {
                            dgvLockerZones.CurrentCell = dgvLockerZones["zoneIdDataGridViewTextBoxColumn", i];
                            break;
                        }
                        i++;
                    }
                    return;
                }
            }
            List<LockerZonesDTO> LockerParentZonesList = lockerZonesList.GetParentZones(false, false);
        }
        private void btnSavePanel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LockerPanel lockerPanels;
            BindingSource bindingSource = (BindingSource)bsPanels.DataSource;
            SortableBindingList<LockerPanelDTO> lockerPanelsSortableList = (SortableBindingList<LockerPanelDTO>)bindingSource.DataSource;
            List<LockerPanelDTO> lockerPanelSavedDTOList = new List<LockerPanelDTO>();
            if (lockerPanelsSortableList != null)
            {
                for (int i = 0; i < lockerPanelsSortableList.Count; i++)
                {
                    if (lockerPanelsSortableList[i].IsChanged)
                    {
                        try
                        {
                            lockerPanels = new LockerPanel(machineUserContext, lockerPanelsSortableList[i]);
                            lockerPanels.Save();
                            lockerPanelSavedDTOList.Add(lockerPanelsSortableList[i]);
                            //createLockers(lockerPanelsSortableList[i]);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error while saving event." + ex.ToString());
                            dgvLockerZones.Rows[i].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(718));
                            break;
                        }
                    }
                }
                //createLockers(lockerPanelSavedDTOList);
                LoadLockerZonesDTOList();
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            log.LogMethodExit();
        }
        private void btnSaveLockers_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Semnox.Parafait.Device.Lockers.Locker lockerBL;
            if (cmbPanel.SelectedValue == null)
            {
                return;
            }
            for (int col = 0; col < tblLockers.ColumnCount; col++)
            {
                for (int row = 0; row < tblLockers.RowCount; row++)
                {
                    LockerControl pnl = tblLockers.GetControlFromPosition(col, row) as LockerControl;
                    Locker locker = pnl.Tag as Locker;

                    if (string.IsNullOrEmpty(locker.txtLockerName.Text.Trim()))
                    {
                        MessageBox.Show("Enter Locker Name");
                        this.ActiveControl = locker.txtLockerName;
                        log.LogMethodExit();
                        return;
                    }

                    if (string.IsNullOrEmpty(locker.txtIdentifier.Text))
                    {
                        MessageBox.Show("Enter Locker Identifier");
                        this.ActiveControl = locker.txtIdentifier;
                        log.LogMethodExit();
                        return;
                    }
                    if (locker.lockerDTO.IsActive != locker.chkActive.Checked)
                        locker.lockerDTO.IsActive = locker.chkActive.Checked;
                    if (locker.lockerDTO.IsDisabled != locker.chkDisable.Checked)
                        locker.lockerDTO.IsDisabled = locker.chkDisable.Checked;
                    if (locker.lockerDTO.LockerName != locker.txtLockerName.Text)
                        locker.lockerDTO.LockerName = locker.txtLockerName.Text;
                    if (locker.lockerDTO.Identifier != Convert.ToInt32(locker.txtIdentifier.Text))
                        locker.lockerDTO.Identifier = Convert.ToInt32(locker.txtIdentifier.Text);
                    int? positionX = null;
                    if (string.IsNullOrWhiteSpace(locker.txtPositionX.Text) == false)
                    {
                        int i;
                        if (int.TryParse(locker.txtPositionX.Text, out i))
                        {
                            positionX = i;
                        }
                    }

                    if (locker.lockerDTO.PositionX != positionX)
                    {
                        locker.lockerDTO.PositionX = positionX;
                    }

                    int? positionY = null;
                    if (string.IsNullOrWhiteSpace(locker.txtPositionY.Text) == false)
                    {
                        int i;
                        if (int.TryParse(locker.txtPositionY.Text, out i))
                        {
                            positionY = i;
                        }
                    }

                    if (locker.lockerDTO.PositionY != positionY)
                    {
                        locker.lockerDTO.PositionY = positionY;
                    }
                    lockerBL = new Semnox.Parafait.Device.Lockers.Locker(machineUserContext,locker.lockerDTO);
                    lockerBL.Save();
                }
            }
            log.LogMethodExit();
        }
        void createLockers(List<LockerPanelDTO> changedPanelsList)
        {
            log.LogMethodEntry(changedPanelsList);
            List<LockerDTO> lockerFilterDTO;
            Semnox.Parafait.Device.Lockers.Locker locker = null;
            LockerDTO lockerDTO;
            int identifier;
            if (lockerDTOList.Count > 0)
            {
                identifier = lockerDTOList.Max(x => x.Identifier) + 1;//Convert.ToInt32(utilities.executeScalar("select isnull(max(identifier), 0) from Lockers")) + 1;
            }
            else
            {
                identifier = 1;
            }
            foreach (LockerPanelDTO lockerPanelDTO in changedPanelsList)
            {
                if (lockerPanelDTO.NumRows == -1
                    || lockerPanelDTO.NumCols == -1
                    || lockerPanelDTO.NumRows == 0
                    || lockerPanelDTO.NumCols == 0)
                {
                    if (locker == null)
                    {
                        locker = new Semnox.Parafait.Device.Lockers.Locker(machineUserContext);
                    }
                    locker.RemoveLockers(lockerPanelDTO.PanelId, -1, -1);
                    continue;

                }

                int rows = lockerPanelDTO.NumRows;
                int cols = lockerPanelDTO.NumCols;
                string prefix = lockerPanelDTO.SequencePrefix;
                int index = 1;
                for (int i = 1; i <= rows; i++)
                {
                    for (int j = 1; j <= cols; j++)
                    {
                        lockerFilterDTO = lockerDTOList.Where(x => (bool)(x.PanelId == lockerPanelDTO.PanelId && x.RowIndex == i && x.ColumnIndex == j)).ToList<LockerDTO>();
                        if (lockerFilterDTO == null || (lockerFilterDTO != null && lockerFilterDTO.Count == 0))
                        {
                            lockerDTO = new LockerDTO();
                            lockerDTO.LockerName = prefix + index.ToString();
                            lockerDTO.PanelId = lockerPanelDTO.PanelId;
                            lockerDTO.RowIndex = i;
                            lockerDTO.ColumnIndex = j;
                            lockerDTO.Identifier = identifier;
                            locker = new Semnox.Parafait.Device.Lockers.Locker(machineUserContext, lockerDTO);
                            locker.Save();
                            index++;
                            if (lockerDTO.LockerId > -1)
                                identifier++;
                        }
                        else
                        {
                            if ((string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(new String(lockerFilterDTO[0].LockerName.Where(x => ((bool)Char.IsLetter(x) || Char.IsPunctuation(x) || Char.IsSymbol(x))).ToArray())))
                                || (!string.IsNullOrEmpty(prefix) && new String(prefix.Where(x => ((bool)Char.IsLetter(x) || Char.IsPunctuation(x) || Char.IsSymbol(x))).ToArray()) != new String(lockerFilterDTO[0].LockerName.Where(x => ((bool)Char.IsLetter(x) || Char.IsPunctuation(x) || Char.IsSymbol(x))).ToArray())))
                            {
                                lockerFilterDTO[0].LockerName = prefix + new String(lockerFilterDTO[0].LockerName.Where(Char.IsNumber).ToArray());
                            }
                            locker = new Semnox.Parafait.Device.Lockers.Locker(machineUserContext,lockerFilterDTO[0]);
                            locker.Save();
                            index++;
                        }
                    }
                }
                if (locker == null)
                {
                    locker = new Semnox.Parafait.Device.Lockers.Locker();
                }
                locker.RemoveLockers(lockerPanelDTO.PanelId, lockerPanelDTO.NumRows, -1);
                locker.RemoveLockers(lockerPanelDTO.PanelId, -1, lockerPanelDTO.NumCols);

            }
            log.LogMethodExit();
        }

        private void btnRefreshPanel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            LoadZones();
            LoadLockerZonesDTOList();
            log.LogMethodExit();
        }

        private void btnDeletePanel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            dgvPanels.CurrentRow.Cells["IsActive"].Value = false;
            btnSavePanel.PerformClick();
            log.LogMethodExit();
        }

        private void btnClosePanel_Click(object sender, EventArgs e)
        {
            //if (utilities.ContinueWithoutSaving(lockersDataSet))
            //Close();
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void dgvPanels_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            if (dgvPanels.Columns[e.ColumnIndex].Equals(dcGoToPanelLockers))
            {
                if (dgvPanels["panelIdDataGridViewTextBoxColumn", e.RowIndex].FormattedValue.ToString() == "-1")
                {
                    MessageBox.Show("Save before performing this function");
                    log.LogMethodExit();
                    return;
                }

                tcLockers.SelectedTab = tpLockers;
                cmbPanel.SelectedValue = dgvPanels["panelIdDataGridViewTextBoxColumn", e.RowIndex].Value;
            }
            log.LogMethodExit();
        }

        #region Lockers
        class Locker
        {
            public LockerControl panelSample = new LockerControl();

            public TextBox txtIdentifier
            {
                get
                {
                    return panelSample.txtIdentifier;
                }
            }

            public TextBox txtLockerName
            {
                get
                {
                    return panelSample.txtLockerName;
                }
            }
            public Label lblLockerId
            {
                get
                {
                    return panelSample.lblLockerId;
                }
            }

            public CheckBox chkActive
            {
                get
                {
                    return panelSample.chkActive;
                }
            }

            public CheckBox chkDisable
            {
                get
                {
                    return panelSample.chkDisable;
                }
            }

            public TextBox txtPositionX
            {
                get
                {
                    return panelSample.txtPositionX;
                }
            }

            public TextBox txtPositionY
            {
                get
                {
                    return panelSample.txtPositionY;
                }
            }
            public LockerDTO lockerDTO;
            public Locker()
            {
                log.LogMethodEntry();
                panelSample.Tag = this;
                log.LogMethodExit();
            }
        }
        #endregion

    }
}
