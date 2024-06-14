
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// User interface for data access management
    /// </summary>
    public partial class DataAccessControlUI : Form
    {
        Semnox.Core.Utilities.Utilities utilities;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<DataAccessRuleDTO> dataAccessRuleDTOList = null;
        List<LookupValuesDTO> lookupValuesDataAccessEntityDTOList;
        List<LookupValuesDTO> lookupValuesDataAccessLevelDTOList;
        List<LookupValuesDTO> lookupValuesDataExclusionDTOList;
        DataGridView dgvfocused = null;
        //List<DataAccessRuleDTO> dataAccessRuleSortableBindingList;

        class EntityData
        {
            private string entity;
            private string guid;
            private string recordData;

            public string Entity { get { return entity; } set { entity = value; } }
            public string Guid { get { return guid; } set { guid = value; } }
            public string RecordData { get { return recordData; } set { recordData = value; } }
        }
        List<EntityData> entityData = new List<EntityData>();
        /// <summary>
        /// constructor for data access management
        /// </summary>
        public DataAccessControlUI(Semnox.Core.Utilities.Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvDataAccessRule);
            utilities.setupDataGridProperties(ref dgvAccessRuleDetail);
            utilities.setupDataGridProperties(ref dgvExclusionDetails);

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
            LoadAll();
            PopulateDataAccessRule(); 
            log.LogMethodExit();
        }
        
        private void PopulateDataAccessRule()
        {
            log.LogMethodEntry();
            DataAccessRuleList dataAccessRuleList = new DataAccessRuleList(machineUserContext);
            List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>> dataAccessRuleSearchParams = new List<KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>>();
            //dataAccessRuleSearchParams.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.ACTIVE_FLAG, "1"));
            dataAccessRuleSearchParams.Add(new KeyValuePair<DataAccessRuleDTO.SearchByDataAccessRuleParameters, string>(DataAccessRuleDTO.SearchByDataAccessRuleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            dataAccessRuleDTOList = dataAccessRuleList.GetAllDataAccessRule(dataAccessRuleSearchParams, true);
            //dataAccessRuleSortableBindingList = new SortableBindingList<DataAccessRuleDTO>();
            if (dataAccessRuleDTOList != null)
            {
                //dataAccessRuleSortableBindingList = new SortableBindingList<DataAccessRuleDTO>(dataAccessRuleDTOList);
                foreach (var dataAccessRuleDTO in dataAccessRuleDTOList)
                {
                    if (dataAccessRuleDTO.DataAccessDetailDTOList != null)
                    {
                        dataAccessRuleDTO.DataAccessDetailDTOList = new List<DataAccessDetailDTO>(dataAccessRuleDTO.DataAccessDetailDTOList);
                        foreach (var dataAccessDetailDTO in dataAccessRuleDTO.DataAccessDetailDTOList)
                        {
                            if (dataAccessDetailDTO.EntityExclusionDetailDTOList != null)
                            {
                                foreach (EntityExclusionDetailDTO entityExclusionDetailDTO in dataAccessDetailDTO.EntityExclusionDetailDTOList)
                                {
                                    entityExclusionDetailDTO.TableAttributeGuid = entityExclusionDetailDTO.TableAttributeGuid.ToLower();
                                }
                                dataAccessDetailDTO.EntityExclusionDetailDTOList = new List<EntityExclusionDetailDTO>(dataAccessDetailDTO.EntityExclusionDetailDTOList);
                            }
                            else
                            {
                                dataAccessDetailDTO.EntityExclusionDetailDTOList = new List<EntityExclusionDetailDTO>();
                            }
                        }
                    }
                    else
                    {
                        dataAccessRuleDTO.DataAccessDetailDTOList = new List<DataAccessDetailDTO>();
                    }
                }
            }
            dataAccessRuleDTOBindingSource.DataSource = dataAccessRuleDTOList;
            log.LogMethodExit();
        }
        
        private void DataAccessRule_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            List<DataAccessDetailDTO> dataAccessDetailDTOList = null;
            if (dataAccessRuleDTOBindingSource.Current != null && dataAccessRuleDTOBindingSource.Current is DataAccessRuleDTO)
            {
                if ((dataAccessRuleDTOBindingSource.Current as DataAccessRuleDTO).DataAccessDetailDTOList != null)
                {
                    dataAccessDetailDTOList = (dataAccessRuleDTOBindingSource.Current as DataAccessRuleDTO).DataAccessDetailDTOList as List<DataAccessDetailDTO>;
                }
                else
                {
                    dataAccessDetailDTOList = new List<DataAccessDetailDTO>();
                }
            }
            else
            {
                dataAccessDetailDTOList = new List<DataAccessDetailDTO>();
            }
            dataAccessDetailDTOBindingSource.DataSource = dataAccessDetailDTOList;
            log.LogMethodExit();
        }
        private void DataAccessRuleDetailBS_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            List<EntityExclusionDetailDTO> entityExclusionDetailDTOList = null;
            if (dataAccessDetailDTOBindingSource.Current != null && dataAccessDetailDTOBindingSource.Current is DataAccessDetailDTO)
            {
                if ((dataAccessDetailDTOBindingSource.Current as DataAccessDetailDTO).EntityExclusionDetailDTOList != null)
                {
                    entityExclusionDetailDTOList = (dataAccessDetailDTOBindingSource.Current as DataAccessDetailDTO).EntityExclusionDetailDTOList as List<EntityExclusionDetailDTO>;
                }
                else
                {
                    entityExclusionDetailDTOList = new List<EntityExclusionDetailDTO>();
                }
            }
            else
            {
                entityExclusionDetailDTOList = new List<EntityExclusionDetailDTO>();
            }
            entityExclusionDetailDTOBindingSource.DataSource = entityExclusionDetailDTOList;
            LoadExclusionAttributeList();
            log.LogMethodExit();
        }
        private void LoadAll()
        {
            log.LogMethodEntry();
            LoadDetailEntity();
            LoadAccessLevel();
            LoadAccessLimit();
            LoadAccessExclusionEntity();
            log.LogMethodExit();
        }
        private void LoadDetailEntity()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_ACCESS_ENTITY"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDataAccessEntityDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDataAccessEntityDTOList == null)
                {
                    lookupValuesDataAccessEntityDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDataAccessEntityDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDataAccessEntityDTOList[0].LookupValueId = -1;
                lookupValuesDataAccessEntityDTOList[0].Description = utilities.MessageUtils.getMessage("<Select>");
                entityIdDataGridViewTextBoxColumn.DataSource = lookupValuesDataAccessEntityDTOList;
                entityIdDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                entityIdDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
            }
            catch (Exception e)
            {
                log.Error("Exception:" + e.ToString());
            }
            log.LogMethodExit();
        }

        private void LoadAccessLevel()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_ACCESS_LEVEL"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDataAccessLevelDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDataAccessLevelDTOList == null)
                {
                    lookupValuesDataAccessLevelDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDataAccessLevelDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDataAccessLevelDTOList[0].LookupValueId = -1;
                lookupValuesDataAccessLevelDTOList[0].Description = utilities.MessageUtils.getMessage("<Select>");
                accessLevelIdDataGridViewTextBoxColumn.DataSource = lookupValuesDataAccessLevelDTOList;
                accessLevelIdDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                accessLevelIdDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
            }
            catch (Exception e)
            {
                log.Error("Exception:" + e.ToString());
            }
            log.LogMethodExit();
        }

        private void LoadAccessLimit()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_ACCESS_LIMIT"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDataAccessLevelDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDataAccessLevelDTOList == null)
                {
                    lookupValuesDataAccessLevelDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDataAccessLevelDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDataAccessLevelDTOList[0].LookupValueId = -1;
                lookupValuesDataAccessLevelDTOList[0].Description = utilities.MessageUtils.getMessage("<Select>");
                accessLimitIdDataGridViewTextBoxColumn.DataSource = lookupValuesDataAccessLevelDTOList;
                accessLimitIdDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                accessLimitIdDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
            }
            catch (Exception e)
            {
                log.Error("Exception:" + e.ToString());
            }
            log.LogMethodExit();
        }


        private void LoadAccessExclusionEntity()
        {
            log.LogMethodEntry();
            try
            {
                LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_EXCLUSION_ENTITY"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                lookupValuesDataExclusionDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                if (lookupValuesDataExclusionDTOList == null)
                {
                    lookupValuesDataExclusionDTOList = new List<LookupValuesDTO>();
                }
                BindingSource bindingSource = new BindingSource();
                lookupValuesDataExclusionDTOList.Insert(0, new LookupValuesDTO());
                lookupValuesDataExclusionDTOList[0].LookupValueId = -1;
                lookupValuesDataExclusionDTOList[0].Description = utilities.MessageUtils.getMessage("<Select>");
                tableAttributeIdDataGridViewTextBoxColumn.DataSource = lookupValuesDataExclusionDTOList;
                tableAttributeIdDataGridViewTextBoxColumn.ValueMember = "LookupValueId";
                tableAttributeIdDataGridViewTextBoxColumn.DisplayMember = "LookupValue";
                //DataTable dTable;
                //string[] entityName;
                // EntityExclusionDetailList entityExclusionDetailList = new EntityExclusionDetailList();
                //EntityData entityDataObject;
                entityData = new List<EntityData>();
                //foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDataExclusionDTOList)
                //{
                //    entityName = lookupValuesDTO.Description.Split('|');
                //    if (entityName.Length > 1)
                //    {

                //        dTable = entityExclusionDetailList.GetEntityData(entityName[0]);
                //        //DataRow dr = dTable.NewRow();
                //        //dTable.Rows.InsertAt(dr, 0);
                //        //dTable.Rows[0]["AttributeGuid"] = DBNull.Value;
                //        //dTable.Rows[0][entityName[1]] = "<Select>";
                //        foreach (DataRow drow in dTable.Rows)
                //        {
                //            entityDataObject = new EntityData();
                //            entityDataObject.Entity = entityName[0];
                //            entityDataObject.Guid = drow["AttributeGuid"].ToString().ToLower();
                //            entityDataObject.RecordData = drow[entityName[1]].ToString();
                //            entityData.Add(entityDataObject);
                //        }
                //    } 
                //}
                EntityData ent = new EntityData();
                ent.Guid = "-1";
                ent.RecordData = "<Select>";
                entityData.Insert(0, ent);
                BindingSource bindingSource1 = new BindingSource();
                bindingSource1.DataSource = entityData;
                tableAttributeGuidDataGridViewTextBoxColumn.DataSource = bindingSource1;
                tableAttributeGuidDataGridViewTextBoxColumn.ValueMember = "Guid";
                tableAttributeGuidDataGridViewTextBoxColumn.DisplayMember = "RecordData";
            }
            catch (Exception e)
            {
                log.Error("Exception:" + e.ToString());
            }
            log.LogMethodExit();
        }

        private void LoadAccessExclusionAttribute(int dgvExclusionDetailsRowIndex) //System.Windows.Forms.DataGridViewComboBoxCell comboBoxColumnField, object lookupValueId)
        {
            log.LogMethodEntry(dgvExclusionDetailsRowIndex);            
            int lookupValueIdValue = -1;
            try
            {
                try
                {
                    lookupValueIdValue = Convert.ToInt32(dgvExclusionDetails.Rows[dgvExclusionDetailsRowIndex].Cells["tableAttributeIdDataGridViewTextBoxColumn"].Value);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
                if (lookupValueIdValue != -1)
                {
                    LookupValuesList lookupValuesList = new LookupValuesList(machineUserContext);
                    List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "DATA_EXCLUSION_ENTITY"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE_ID, lookupValueIdValue.ToString()));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                    lookupValuesDataExclusionDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                    if (lookupValuesDataExclusionDTOList != null)
                    {
                        //BindingSource bindingSource = new BindingSource();
                        DataTable dTable;
                        string[] entityName;
                        EntityExclusionDetailList entityExclusionDetailList = new EntityExclusionDetailList();
                        EntityData entityDataObject;
                        entityData = new List<EntityData>();
                        foreach (LookupValuesDTO lookupValuesDTO in lookupValuesDataExclusionDTOList)
                        {
                            entityName = lookupValuesDTO.Description.Split('|');
                            if (entityName.Length > 1)
                            {
                                dgvExclusionDetails.Rows[dgvExclusionDetailsRowIndex].Cells["tableNameDataGridViewTextBoxColumn"].Value = entityName[0];
                                dTable = entityExclusionDetailList.GetEntityData(entityName[0], entityName.Length);
                                foreach (DataRow drow in dTable.Rows)
                                {
                                    entityDataObject = new EntityData();
                                    entityDataObject.Entity = entityName[0];
                                    entityDataObject.Guid = drow["AttributeGuid"].ToString().ToLower();
                                    entityDataObject.RecordData = drow[entityName[1]].ToString();
                                    entityData.Add(entityDataObject);
                                }
                            }
                            else if (entityName.Length == 1)
                            {
                                dgvExclusionDetails.Rows[dgvExclusionDetailsRowIndex].Cells["tableNameDataGridViewTextBoxColumn"].Value = lookupValuesDTO.LookupValue;
                                dTable = entityExclusionDetailList.GetEntityData(entityName[0], entityName.Length);
                                foreach (DataRow drow in dTable.Rows)
                                {
                                    entityDataObject = new EntityData();
                                    entityDataObject.Entity = entityName[0];
                                    entityDataObject.Guid = drow["AttributeGuid"].ToString().ToLower();
                                    entityDataObject.RecordData = drow["FieldName"].ToString();
                                    entityData.Add(entityDataObject);
                                }
                            }
                        }
                        EntityData ent = new EntityData();
                        ent.Guid = "-1";
                        ent.RecordData = "<Select>";
                        entityData.Insert(0, ent);
                        BindingSource bindingSource1 = new BindingSource();
                        bindingSource1.DataSource = entityData;
                        DataGridViewComboBoxCell comboBoxColumnField = dgvExclusionDetails.Rows[dgvExclusionDetailsRowIndex].Cells["tableAttributeGuidDataGridViewTextBoxColumn"] as DataGridViewComboBoxCell;
                        comboBoxColumnField.DataSource = bindingSource1;
                        comboBoxColumnField.ValueMember = "Guid";
                        comboBoxColumnField.DisplayMember = "RecordData"; 
                    }
                }
            }
            catch (Exception e)
            {
                log.Error(e);
            }
            log.LogMethodExit();
        }

        private void LoadExclusionAttributeList()
        {
            log.LogMethodEntry();
            if (dgvExclusionDetails != null && dgvExclusionDetails.RowCount > 0)
            {
                foreach (DataGridViewRow item in dgvExclusionDetails.Rows)
                {
                    if (item.Cells["tableAttributeIdDataGridViewTextBoxColumn"].Value != null)
                    {
                        LoadAccessExclusionAttribute(item.Index);
                    }
                }
            }
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvAccessRuleDetail.EndEdit();
            dgvDataAccessRule.EndEdit();
            dgvExclusionDetails.EndEdit();
            if (dataAccessRuleDTOList.Count > 0)
            {
                foreach (DataAccessRuleDTO dataAccessRuleDTO in dataAccessRuleDTOList)
                {
                    if (!Validate(dataAccessRuleDTO))
                    {
                        return;
                    }
                    else
                    {
                        DataAccessRule dataAccessRule = new DataAccessRule(machineUserContext, dataAccessRuleDTO);
                        dataAccessRule.Save(null);
                    }
                }
                MessageBox.Show(utilities.MessageUtils.getMessage(122));
                PopulateDataAccessRule();
            }
            else
                MessageBox.Show(utilities.MessageUtils.getMessage(371));

            log.LogMethodExit();
        }
        private bool Validate(DataAccessRuleDTO dataAccessRuleDTO)
        {
            log.LogMethodEntry(dataAccessRuleDTO);
            if (string.IsNullOrEmpty(dataAccessRuleDTO.Name))
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1654));
                return false;
            }
            if (dataAccessRuleDTO.DataAccessDetailDTOList != null && dataAccessRuleDTO.DataAccessDetailDTOList.Count > 0)
            {
                foreach (DataAccessDetailDTO dataAccessDetailDTO in dataAccessRuleDTO.DataAccessDetailDTOList)
                {
                    if (dataAccessDetailDTO.AccessLevelId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1655));
                        log.LogMethodExit(false, "Please Select Data Access Level.");
                        return false;
                    }
                    if (dataAccessDetailDTO.AccessLimitId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1656));
                        log.LogMethodExit(false, "Please Select Data Access Limit.");
                        return false;
                    }
                    if (dataAccessDetailDTO.EntityId == -1)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(1657));
                        log.LogMethodExit(false, "Please Select Data Access Entity.");
                        return false;
                    }
                    if (dataAccessDetailDTO.EntityExclusionDetailDTOList != null && dataAccessDetailDTO.EntityExclusionDetailDTOList.Count > 0)
                    {
                        int entityExclusionDetailRowCount = 0;
                        foreach (EntityExclusionDetailDTO entityExclusionDetailDTO in dataAccessDetailDTO.EntityExclusionDetailDTOList)
                        {
                            if (entityExclusionDetailDTO.TableAttributeId == -1)
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1658));
                                log.LogMethodExit(false, "Please select Exclusion Entity.");
                                return false;
                            }
                            if ( entityExclusionDetailDTO.TableAttributeGuid == null ||
                                entityExclusionDetailDTO.TableAttributeGuid.Equals("-1"))
                            {
                                MessageBox.Show(utilities.MessageUtils.getMessage(1659));
                                log.LogMethodExit(false, "Please select the attribute.");
                                return false;
                            }
                            
                            entityExclusionDetailRowCount++;
                        }
                    }
                }

            }
            log.LogMethodExit(true);
            return true;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadAll();
            PopulateDataAccessRule();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvfocused != null)
            {
                if (this.dgvfocused.SelectedRows.Count <= 0 && this.dgvfocused.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.Debug("Ends-assetDeleteBtn_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvfocused.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvfocused.SelectedCells)
                    {
                        dgvfocused.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow dgvSelectedRow in this.dgvfocused.SelectedRows)
                {
                    if (dgvSelectedRow.Cells[0].Value == null)
                    {
                        return;
                    }
                    if (Convert.ToInt32(dgvSelectedRow.Cells[0].Value.ToString()) <= 0)
                    {
                        dgvfocused.Rows.RemoveAt(dgvSelectedRow.Index);
                        rowsDeleted = true;
                    }
                    else
                    {
                        if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                        {
                            confirmDelete = true;
                            BindingSource focussedGridDataListDTOBS = (BindingSource)dgvfocused.DataSource;
                            switch (dgvfocused.Name)
                            {
                                case "dgvDataAccessRule":
                                    var dataAccessRuleDTOList = (List<DataAccessRuleDTO>)focussedGridDataListDTOBS.DataSource;
                                    DataAccessRuleDTO dataAccessRuleDTO = dataAccessRuleDTOList[dgvSelectedRow.Index];
                                    dataAccessRuleDTO.IsActive = false;
                                    DataAccessRule dataAccessRule = new DataAccessRule(machineUserContext, dataAccessRuleDTO);
                                    dataAccessRule.Save(null);
                                    break;
                                case "dgvAccessRuleDetail":
                                    var dataAccessDetailDTOList = (List<DataAccessDetailDTO>)focussedGridDataListDTOBS.DataSource;
                                    DataAccessDetailDTO dataAccessDetailDTO = dataAccessDetailDTOList[dgvSelectedRow.Index];
                                    dataAccessDetailDTO.IsActive = false;
                                    DataAccessDetail dataAccessDetail = new DataAccessDetail(machineUserContext, dataAccessDetailDTO);
                                    dataAccessDetail.Save(null);
                                    break;
                                case "dgvExclusionDetails":
                                    var exclusionDTOList = (List<EntityExclusionDetailDTO>)focussedGridDataListDTOBS.DataSource;
                                    EntityExclusionDetailDTO entityExclusionDetailDTO = exclusionDTOList[dgvSelectedRow.Index];
                                    entityExclusionDetailDTO.IsActive = false;
                                    EntityExclusionDetail entityExclusionDetail = new EntityExclusionDetail(machineUserContext, entityExclusionDetailDTO);
                                    entityExclusionDetail.Save(null);
                                    break;
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));

                PopulateDataAccessRule();
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }
        private void dgvDataAccessRule_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        }

        private void dgvAccessRuleDetail_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        }

        private void dgvExclusionDetails_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        } 

        private void dataAccessRuleDTOBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DataAccessRuleDTO dataAccessRuleDTO = new DataAccessRuleDTO();
            dataAccessRuleDTO.DataAccessDetailDTOList = new List<DataAccessDetailDTO>();
            e.NewObject = dataAccessRuleDTO;
            log.LogMethodExit();
        }

        private void dataAccessDetailDTOBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DataAccessDetailDTO dataAccessDetailDTO = new DataAccessDetailDTO();
            dataAccessDetailDTO.EntityExclusionDetailDTOList = new List<EntityExclusionDetailDTO>();
            e.NewObject = dataAccessDetailDTO;
            log.LogMethodExit();
        }

        private void dgvExclusionDetails_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == dgvExclusionDetails.Columns["tableAttributeIdDataGridViewTextBoxColumn"].Index)
            {
                if (lookupValuesDataExclusionDTOList != null)
                    dgvExclusionDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = lookupValuesDataExclusionDTOList[0].LookupValueId;
            }
            else if (e.ColumnIndex == dgvExclusionDetails.Columns["tableAttributeGuidDataGridViewTextBoxColumn"].Index)
            {
                // e.Cancel = true;
                //LookupValuesDTO lookupValuesDTO = lookupValuesDataExclusionDTOList.Where(x => (bool)(x.LookupValueId == Convert.ToInt32(dgvExclusionDetails.Rows[e.RowIndex].Cells["tableAttributeIdDataGridViewTextBoxColumn"].Value))).ToList<LookupValuesDTO>()[0];
                //if (lookupValuesDTO.LookupValueId != -1)
                //{

                //}
            }
            log.LogMethodExit();
        }
 
        private void dgvAccessRuleDetail_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvAccessRuleDetail.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvDataAccessRule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender,e);
            MessageBox.Show(utilities.MessageUtils.getMessage(963) + " " + (e.RowIndex + 1).ToString() + ",   " + utilities.MessageUtils.getMessage("Column") + " " + dgvDataAccessRule.Columns[e.ColumnIndex].DataPropertyName +
               ": " + e.Exception.Message);
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void dgvExclusionDetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvExclusionDetails.Columns[e.ColumnIndex].Name.Equals("tableAttributeIdDataGridViewTextBoxColumn"))
            {
                //LoadAccessExclusionAttribute(dgvExclusionDetails.Rows[e.RowIndex].Cells["tableAttributeGuidDataGridViewTextBoxColumn"] as DataGridViewComboBoxCell, dgvExclusionDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                dgvExclusionDetails.Rows[e.RowIndex].Cells["FieldName"].Value = "";
                dgvExclusionDetails.Rows[e.RowIndex].Cells["tableAttributeGuidDataGridViewTextBoxColumn"].Value = "-1";
                LoadAccessExclusionAttribute(e.RowIndex);
            }

            if (dgvExclusionDetails.Columns[e.ColumnIndex].Name.Equals("tableAttributeGuidDataGridViewTextBoxColumn"))
            {
                DataGridViewComboBoxCell tempCell = (dgvExclusionDetails.Rows[e.RowIndex].Cells["tableAttributeGuidDataGridViewTextBoxColumn"] as DataGridViewComboBoxCell);

                
                if (tempCell.Value.ToString() == "")
                {
                    dgvExclusionDetails.Rows[e.RowIndex].Cells["FieldName"].Value= tempCell.FormattedValue.ToString();
                }
                else
                {
                    dgvExclusionDetails.Rows[e.RowIndex].Cells["FieldName"].Value = "";
                }
            }
            log.LogMethodExit();
        }
 

       
    }
}
