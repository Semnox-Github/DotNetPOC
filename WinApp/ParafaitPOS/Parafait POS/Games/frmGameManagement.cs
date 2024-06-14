/********************************************************************************************
 * Project Name - Parafait_POS.Games
 * Description  -  A UI for frmGameManagement
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
*2.70.2       20-Aug-2019      Girish Kundar  Modified : Added Logger methods and Removed unused namespace's 
*2.70.3       03-Dec-2019      Archana        Modified to add Edit Machine Limited and Edit Machine config tabs
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.DigitalSignage;
using Semnox.Parafait.Game;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.User;

namespace Parafait_POS.Games
{
    public partial class frmGameManagement : Form
    {
        //Begin: Modified Added for logger function on 08-Mar-2016
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016
        bool showMachineNameWithGameName = false;
        List<GameDTO> gameDTOList = null;
        List<LookupValuesDTO> gameStatusLookupValueDTOList = null;
        List<MachineDTO> machineDTOList = null;
        List<ThemeDTO> themeListDTO = null;
        List<HubDTO> hubListDTO = null;
        bool showTabAddMachine = false;
        bool showTabEditMachine = false;
        bool showTabOutOfService = false;
        bool showTabEditMachineLimited = false;
        bool showTabEditMachineConfig = false;

        public frmGameManagement()
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.LogMethodEntry();
            InitializeComponent();

            showTabAddMachine = ShowTabPagesBasedOnUserRole(tpAddMachine, "Add Machine");
            showTabEditMachine = ShowTabPagesBasedOnUserRole(tpEditMachine, "Edit Machine");
            showTabOutOfService = ShowTabPagesBasedOnUserRole(tpOutOfService, "Set Out Of Service");
            showTabEditMachineLimited = ShowTabPagesBasedOnUserRole(tpEditMachineLimited, "Edit Machine Limited");
            showTabEditMachineConfig = ShowTabPagesBasedOnUserRole(tpEditMachineConfig, "Edit Machine Config");
            if (ParafaitDefaultContainerList.GetParafaitDefault(POSStatic.Utilities.ExecutionContext, "SHOW_GAME_NAME_IN_GAME_MANAGEMENT").ToString() == "Y")
            {
                showMachineNameWithGameName = true;
            }
            
            log.LogMethodExit();
        }

        private bool ShowTabPagesBasedOnUserRole(TabPage tabPage, string formName)
        {
            log.LogMethodEntry(tabPage, formName);
            try
            {
                ManagementFormAccessListBL managementFormAccessListBL = new ManagementFormAccessListBL(POSStatic.Utilities.ExecutionContext);
                List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                searchParams = new List<KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>>();
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.MAIN_MENU, "Parafait POS"));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ACCESS_ALLOWED, "1"));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.FORM_NAME, formName));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ROLE_ID, POSStatic.Utilities.ParafaitEnv.RoleId.ToString()));
                searchParams.Add(new KeyValuePair<ManagementFormAccessDTO.SearchByParameters, string>(ManagementFormAccessDTO.SearchByParameters.ISACTIVE, "1"));
                List<ManagementFormAccessDTO> managementFormAccessDTOList = managementFormAccessListBL.GetManagementFormAccessDTOList(searchParams);
                if (managementFormAccessDTOList == null)
                {
                    tcGameManagement.TabPages.Remove(tabPage);
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch(Exception ex)
            {
                POSUtils.ParafaitMessageBox("Unexpected error occurred during the load of TabPages", "Game Management");
                log.Error("Error in loading tabpage : " + formName + ex.Message);
            }

            log.LogMethodExit(true);
            return true;
        }
        
        private void frmGameManagement_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Cursor = Cursors.WaitCursor;
                lstOutOfService.Items.Clear();
                GetGameList();
                GetMachineList();
                GetGameStatusLookupValues();
                (Application.OpenForms["POS"] as Parafait_POS.POS).lastTrxActivityTime = DateTime.Now;
                GetHubListDTO();
                LoadMachineComboBoxForOutofService();
                GetThemesList();

                refreshOutOfServiceList();
                if (!showTabOutOfService)
                {
                    if (showTabAddMachine)
                    {
                        tcGameManagement.SelectedTab = tpAddMachine;
                        tcGameManagement_SelectedIndexChanged(this, eventArgs.Empty);
                    }
                    else if (showTabEditMachine)
                    {
                        tcGameManagement.SelectedTab = tpEditMachine;
                        tcGameManagement_SelectedIndexChanged(this, eventArgs.Empty);
                    }
                    else if (showTabEditMachineLimited)
                    {
                        tcGameManagement.SelectedTab = tpEditMachineLimited;
                        tcGameManagement_SelectedIndexChanged(this, eventArgs.Empty);
                    }
                    else if (showTabEditMachineConfig)
                    {
                        tcGameManagement.SelectedTab = tpEditMachineConfig;
                        tcGameManagement_SelectedIndexChanged(this, eventArgs.Empty);
                    }
                }
                log.LogMethodExit();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }            
        }

        private void LoadMachineComboBoxForOutofService()
        {
            log.LogMethodEntry();
            Cursor = Cursors.WaitCursor;
            cmbMachines.DataSource = null;
            List<MachineDTO> machineListForOutofService = new List<MachineDTO>();
            if (machineDTOList != null && machineDTOList.Count > 0)
            {
                foreach (MachineDTO machineDTO in machineDTOList)
                {
                    if (machineDTO.MachineId != -1 && 
                        machineDTO.GameMachineAttributes.FindAll(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE).Exists(y=>y.AttributeValue == "1") == false) 
                    {
                        machineListForOutofService.Add(machineDTO);
                    }
                }
            }
            if (machineListForOutofService.Any())
            {            
                cmbMachines.DataSource = machineListForOutofService;
                cmbMachines.ValueMember = "MachineId";
                cmbMachines.DisplayMember = showMachineNameWithGameName == false ? "MachineNameHubName" : "MachineNameGameNameHubName";
            }
            Cursor = Cursors.Default;
            log.LogMethodExit();
        }

        private void GetGameList()
        {
            log.LogMethodEntry();
            GameList gameList = new GameList();
            List<KeyValuePair<GameDTO.SearchByGameParameters, string>> searchParameters = new List<KeyValuePair<GameDTO.SearchByGameParameters, string>>();
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.IS_ACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<GameDTO.SearchByGameParameters, string>(GameDTO.SearchByGameParameters.SITE_ID, POSStatic.Utilities.ExecutionContext.GetSiteId().ToString()));
            gameDTOList = gameList.GetGameList(searchParameters);
            if(gameDTOList!= null && gameDTOList.Count>0)
            {
                gameDTOList = gameDTOList.OrderBy(x => x.GameName).ToList();
            }
            log.LogMethodExit();
        }
        
        private List<MachineDTO> GetMachineList()
        {
            log.LogMethodEntry();
            machineDTOList = new List<MachineDTO>();
            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));
            searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, POSStatic.Utilities.ExecutionContext.GetSiteId().ToString()));
            MachineList machineList = new MachineList(POSStatic.Utilities.ExecutionContext);
            machineDTOList = machineList.GetMachineList(searchParameters,true);
            if (machineDTOList != null && machineDTOList.Count > 0)
            {
                machineDTOList.Insert(0, new MachineDTO());
                machineDTOList[0].MachineId = -1;
                machineDTOList[0].MachineName = "";
                if (showMachineNameWithGameName)
                {
                    machineDTOList = machineDTOList.OrderBy(x => x.MachineNameGameName).ToList();
                }
                else
                {
                    machineDTOList = machineDTOList.OrderBy(x => x.MachineName).ToList();
                }
            }
            log.LogMethodExit(machineDTOList);
            return machineDTOList;
        }

        void refreshOutOfServiceList()
        {
            log.LogMethodEntry();
            lstOutOfService.DataSource = null;
            lstOutOfService.Items.Clear();
            MachineList machineListBL = new MachineList();
            List<MachineDTO> outOfServiceMachineList = machineListBL.GetOutOfServiceMachines();
            if (outOfServiceMachineList != null && outOfServiceMachineList.Count > 0)
            {
                outOfServiceMachineList = outOfServiceMachineList.OrderBy(x => x.MachineName).ToList();
            }
            if (outOfServiceMachineList != null)
            {
                lstOutOfService.DataSource = outOfServiceMachineList;
                lstOutOfService.ValueMember = "MachineID";
                lstOutOfService.DisplayMember = "MachineNameHubName";
            }
            log.LogMethodExit();
        }

        private void btnMakeOutOfService_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (cmbMachines.SelectedIndex >= 0 && Convert.ToInt32(cmbMachines.SelectedValue) != -1)
                {
                    if (POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(567), "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {                        
                        bool IsExists = false;
                        MachineDTO machineDTO = machineDTOList.Find(x => x.MachineId == Convert.ToInt32(cmbMachines.SelectedValue));
                        if (machineDTO != null)
                        {                        
                            foreach (MachineAttributeDTO machineAttributeDTO in machineDTO.GameMachineAttributes)
                            {
                                if (machineAttributeDTO.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE 
                                     && (machineAttributeDTO.AttributeValue == "1"))
                                {
                                    IsExists = true;
                                    break;
                                }
                            }
                            if (!IsExists)
                            {
                                ReversalRemarks rm = new ReversalRemarks(POSStatic.Utilities, "MACHINE_OOS_REASONS");
                                rm.Text = "Out of Service Reason";
                                while (1 == 1)
                                {
                                    if (rm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                    {
                                        Cursor = Cursors.WaitCursor;
                                        if (string.IsNullOrEmpty(rm.Remarks) || string.IsNullOrEmpty(rm.reason)) //16-May-2016::Both reason and remarks should be mandatory
                                        {
                                            POSUtils.ParafaitMessageBox(POSStatic.Utilities.MessageUtils.getMessage(949), "Out of service Remarks", MessageBoxButtons.OK);
                                            continue;
                                        }                                        
                                        if (machineDTO.GameMachineAttributes.Any(x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE))
                                        {
                                            machineDTO.GameMachineAttributes.Find((x => x.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && x.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)).AttributeValue = "1";
                                        }
                                        else
                                        {
                                            MachineAttributeDTO updatedAttribute = new MachineAttributeDTO(-1, MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE, "1", "Y", "N", MachineAttributeDTO.AttributeContext.MACHINE, "", false, POSStatic.Utilities.ExecutionContext.GetSiteId(), POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now, -1, POSStatic.Utilities.ExecutionContext.GetUserId(), DateTime.Now);
                                            machineDTO.GameMachineAttributes.Add(updatedAttribute);
                                            machineDTO.IsChanged = true;
                                        }
                                        machineDTO.MachineCommunicationLogDTO = null;
                                        Machine machine = new Machine(machineDTO, POSStatic.Utilities.ExecutionContext);
                                        machine.Save();
                                        machineDTOList[machineDTOList.FindIndex(x => x.MachineId == machine.GetMachineDTO.MachineId)] = machine.GetMachineDTO;
                                        POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(cmbMachines.SelectedValue), cmbMachines.Text + " set to out of service from POS", rm.reason, rm.Remarks, MachineAttributeLogDTO.UpdateTypes.OUT_OF_SERVICE.ToString(), 1);
                                        POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(622), "Out of service"); //Modified on 05-Mar-2016
                                        break;
                                    }
                                    else
                                        return;
                                }
                            }
                            else
                            {
                                POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(948));
                            }
                        }
                    }
                    Cursor = Cursors.WaitCursor;
                    refreshOutOfServiceList();
                    LoadMachineComboBoxForOutofService();                    
                }
            }
            catch(Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnRemoveOutOfService_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Cursor = Cursors.WaitCursor;
                if (lstOutOfService.SelectedItems.Count > 0)
                {
                    foreach (MachineDTO selectedMachineDTO in lstOutOfService.SelectedItems)
                    {
                        MachineDTO machineDTO = machineDTOList.Find(x => x.MachineId == Convert.ToInt32(selectedMachineDTO.MachineId));
                        if (machineDTO != null)
                        {
                            foreach (MachineAttributeDTO machineAttributeDTO in machineDTO.GameMachineAttributes)
                            {
                                if (machineAttributeDTO.AttributeName == MachineAttributeDTO.MachineAttribute.OUT_OF_SERVICE && machineAttributeDTO.ContextOfAttribute == MachineAttributeDTO.AttributeContext.MACHINE)
                                {
                                    machineAttributeDTO.AttributeValue = "0";                                    
                                    break;
                                }
                            }
                        }
                        machineDTO.MachineCommunicationLogDTO = null;
                        Machine machine = new Machine(machineDTO);
                        machine.Save();
                        machineDTOList[machineDTOList.FindIndex(x => x.MachineId == machine.GetMachineDTO.MachineId)] = machine.GetMachineDTO;
                        POSStatic.Utilities.MachineLog.logMachineUpdate(Convert.ToInt32(selectedMachineDTO.MachineId), selectedMachineDTO.MachineName.ToString() + " set to in service from POS", "", "", MachineAttributeLogDTO.UpdateTypes.IN_TO_SERVICE.ToString(), 0);
                    }
                    POSUtils.ParafaitMessageBox(POSStatic.MessageUtils.getMessage(2598), "Out of service");
                    refreshOutOfServiceList();
                    LoadMachineComboBoxForOutofService();
                }

            }
            catch(Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

        ContextMenuStrip ctAPs = null;
        private void btnRestartAP_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (ctAPs == null)
            {
                ctAPs = new ContextMenuStrip();
                ctAPs.Font = cmbAps.Font;

                //DataTable dtAps = POSStatic.Utilities.executeDataTable(@"select master_id, master_name
                //                                                        from masters ma where active_flag = 'Y' order by 2");


                //foreach (DataRow dr in dtAps.Rows) 
                if (hubListDTO != null && hubListDTO.Any())
                {
                    foreach(HubDTO hub in hubListDTO)
                    {
                        ToolStripMenuItem ti = new ToolStripMenuItem();
                        ti.Text = hub.MasterName;
                        ti.Tag = hub.MasterId;
                        ctAPs.Items.Add(ti);
                    }
                }

                ctAPs.ItemClicked += ctAPs_ItemClicked;
            }
            Application.DoEvents();
            ctAPs.Show(btnRestartAP, new Point(0, Math.Max(-btnRestartAP.PointToScreen(Point.Empty).Y, -btnRestartAP.Height - 20)));
            Application.DoEvents();
            log.LogMethodExit();
        }

        void ctAPs_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            log.LogMethodEntry();
            ctAPs.Hide();
            //POSStatic.Utilities.executeNonQuery("update masters set RestartAP = 1 where master_id = @masterId",
            //                                     new SqlParameter("@masterId", e.ClickedItem.Tag));
            Hub hub = new Hub(Convert.ToInt32(e.ClickedItem.Tag));
            HubDTO hubDTO = hub.GetHubDTO;
            hubDTO.RestartAP = true;
            hub.Save();
            POSStatic.Utilities.EventLog.logEvent("ParafaitPOS", 'D', "AP ID: " + e.ClickedItem.Tag.ToString(), "AP Restarted from POS", "PARAFAITSERVER", 0);

            POSUtils.ParafaitMessageBox(e.ClickedItem.Text + " is set to restart immediately", "Out of service"); //Modified on 05-Mar-2016
            log.LogMethodExit();
        }

        private void tcGameManagement_SelectedIndexChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            if (tcGameManagement.SelectedTab != null)
            {
                if (tcGameManagement.SelectedTab.Equals(tpAddMachine))
                {
                    LoadAddMachine();
                }
                else if (tcGameManagement.SelectedTab.Equals(tpEditMachine))
                {
                    editMachineLoad();
                }
                if (tcGameManagement.SelectedTab.Equals(tpOutOfService))
                {
                    LoadMachineComboBoxForOutofService();
                }
                else if (tcGameManagement.SelectedTab.Equals(tpEditMachineLimited))
                {
                    LoadEditMachineLimited();
                }
                else if (tcGameManagement.SelectedTab.Equals(tpEditMachineConfig))
                {
                    LoadEditMachineConfig();
                }
            }
            log.LogMethodExit();
        }

        private List<ThemeDTO> GetThemesList()
        {
            log.LogMethodEntry();
            themeListDTO = new List<ThemeDTO>();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(POSStatic.Utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>
                {
                    new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "THEME_TYPE"),
                    new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, POSStatic.Utilities.ExecutionContext.GetSiteId().ToString())
                };
                List<LookupValuesDTO> lookupValuesDTOList = lookupValuesList.GetAllLookupValues(searchParams);

                List<int> lookupValueIdList = new List<int>();
                if (lookupValuesDTOList != null && lookupValuesDTOList.Any())
                {
                    foreach (LookupValuesDTO lookupValue in lookupValuesDTOList)
                    {
                        if(lookupValue.LookupValue == "Audio" || lookupValue.LookupValue == "Display" || lookupValue.LookupValue == "Visualization")
                            lookupValueIdList.Add(lookupValue.LookupValueId);
                    }
                }
                string themeTypeId = string.Empty;
                if (lookupValueIdList != null && lookupValueIdList.Any())
                {
                    foreach(int i in lookupValueIdList)
                    {
                        themeTypeId = themeTypeId + i + ",";
                    }
                    if (themeTypeId != string.Empty)
                    {
                        themeTypeId = themeTypeId.Substring(0, themeTypeId.Length - 1);
                    }
                }
                List<KeyValuePair<ThemeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ThemeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.SITE_ID, Convert.ToString(POSStatic.Utilities.ExecutionContext.GetSiteId())));
                searchParameters.Add(new KeyValuePair<ThemeDTO.SearchByParameters, string>(ThemeDTO.SearchByParameters.TYPE_ID_LIST, themeTypeId));

                ThemeListBL themeList = new ThemeListBL(POSStatic.Utilities.ExecutionContext);
                themeListDTO = themeList.GetThemeDTOList(searchParameters);
                if (themeListDTO != null)
                {
                    themeListDTO.Insert(0, new ThemeDTO());
                    themeListDTO[0].Name = " ";
                    themeListDTO[0].Id = -1;
                    themeListDTO = themeListDTO.OrderBy(x => x.ThemeNameWithThemeNumber).ToList();
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            log.LogMethodExit();
            return themeListDTO;            
        }

        private void GetHubListDTO()
        {
            log.LogMethodEntry();
            hubListDTO = new List<HubDTO>();
            HubList hubList = new HubList(POSStatic.Utilities.ExecutionContext);
            List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters = new List<KeyValuePair<HubDTO.SearchByHubParameters, string>>
            {
                new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.IS_ACTIVE, "Y"),
                new KeyValuePair<HubDTO.SearchByHubParameters, string>(HubDTO.SearchByHubParameters.SITE_ID, POSStatic.Utilities.ExecutionContext.GetSiteId().ToString())
            };
            hubListDTO = hubList.GetHubSearchList(searchParameters,null,true);
            if (hubListDTO != null && hubListDTO.Any())
            {
                hubListDTO = hubListDTO.OrderBy(x => x.HubNameWithMachineCount).ToList();
                hubListDTO.Insert(0, new HubDTO());
                hubListDTO[0].MasterId = -1;
                hubListDTO[0].MasterName = " ";
            }
            log.LogMethodExit();
        }

        private void txtBoxConfig_KeyPress(object sender, KeyPressEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }   
            log.LogMethodExit();
        }

        public void GetGameStatusLookupValues()
        {
            log.LogMethodEntry();
            gameStatusLookupValueDTOList = new List<LookupValuesDTO>();
            List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> searchlookupParameters = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            searchlookupParameters.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "GAME_MACHINE_STATUS"));
            gameStatusLookupValueDTOList = new LookupValuesList(POSStatic.Utilities.ExecutionContext).GetAllLookupValues(searchlookupParameters);
            log.LogMethodExit();
        }

       
    }
}
