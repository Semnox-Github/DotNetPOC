/********************************************************************************************
 * Project Name - frmRedemptionCurrencyRule UI
 * Description  - UI of RedemptionCurrencyRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       21-Aug-2019    Dakshakh raj   Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Redemption
{
    public partial class frmRedemptionCurrencyRuleUI : Form
    {
        private Utilities utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleList;
        List<RedemptionCurrencyDTO> redemptionCurrencyListOnDisplay;
        DataGridView dgvfocused = null;

        /// <summary>
        /// frmRedemptionCurrencyRule UI
        /// </summary>
        /// <param name="_Utilities"></param>
        public frmRedemptionCurrencyRuleUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            utilities.setupDataGridProperties(ref dgvRedemptionCurrencyRule);
            utilities.setupDataGridProperties(ref dgvRedemptionCurrencyRuleDetails);
            if (utilities.ParafaitEnv.IsCorporate)
            {
                machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                machineUserContext.SetSiteId(-1);
            }
            machineUserContext.SetIsCorporate(utilities.ExecutionContext.GetIsCorporate());
            machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
            utilities.setLanguage(this);
            log.LogMethodExit();
        }

        /// <summary>
        /// Load all
        /// </summary>
        private void LoadAll()
        {
            log.LogMethodEntry();
            LoadRedemptionCurrencyList();
            LoadRedemptionCurrencyRules();
            log.LogMethodExit();
        }

        /// <summary>
        /// Load RedemptionCurrency List
        /// </summary>
        private void LoadRedemptionCurrencyList()
        {
            log.LogMethodEntry();
            RedemptionCurrencyList redemptionCurrencyList = new RedemptionCurrencyList(machineUserContext);
            List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>> redemptionCurrencySearchParams = new List<KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>>();
            redemptionCurrencySearchParams.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            redemptionCurrencySearchParams.Add(new KeyValuePair<RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters, string>(RedemptionCurrencyDTO.SearchByRedemptionCurrencyParameters.ISACTIVE, "1"));
            redemptionCurrencyListOnDisplay = redemptionCurrencyList.GetAllRedemptionCurrency(redemptionCurrencySearchParams);
            if (redemptionCurrencyListOnDisplay == null)
            {
                redemptionCurrencyListOnDisplay = new List<RedemptionCurrencyDTO>();
            }
            redemptionCurrencyListOnDisplay.Insert(0, new RedemptionCurrencyDTO());

            redemptionCurrencyListOnDisplay[0].CurrencyName = "<SELECT>";
            currencyIdDataGridViewTextBoxColumn.DataSource = redemptionCurrencyListOnDisplay;
            currencyIdDataGridViewTextBoxColumn.ValueMember = "CurrencyId";
            currencyIdDataGridViewTextBoxColumn.DisplayMember = "CurrencyName";
            log.LogMethodExit();
        }

        /// <summary>
        /// Load RedemptionCurrency Rules
        /// </summary>
        private void LoadRedemptionCurrencyRules()
        {
            log.LogMethodEntry();
            RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(machineUserContext);
            List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>> redemptionCurrencyRuleSearchParams = new List<KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>>();
            redemptionCurrencyRuleSearchParams.Add(new KeyValuePair<RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters, string>(RedemptionCurrencyRuleDTO.SearchByRedemptionCurrencyRuleParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            redemptionCurrencyRuleList = redemptionCurrencyRuleListBL.GetAllRedemptionCurrencyRuleList(redemptionCurrencyRuleSearchParams, true, false);
            if (redemptionCurrencyRuleList == null || redemptionCurrencyRuleList.Any() == false)
            {
                redemptionCurrencyRuleList = new List<RedemptionCurrencyRuleDTO>();
            }
            redemptionCurrencyRuleDTOBindingSource.DataSource = new SortableBindingList<RedemptionCurrencyRuleDTO>(redemptionCurrencyRuleList);
            SetDGVStyle();
            log.LogMethodExit();
        }

        private void SetDGVStyle()
        {
            log.LogMethodEntry();
            dgvRedemptionCurrencyRule.Columns["lastUpdateDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvRedemptionCurrencyRule.Columns["creationDateDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvRedemptionCurrencyRule.Columns["amountDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewAmountCellStyle();
            dgvRedemptionCurrencyRuleDetails.Columns["lastUpdateDateDataGridViewTextBoxColumn1"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvRedemptionCurrencyRuleDetails.Columns["creationDateDataGridViewTextBoxColumn1"].DefaultCellStyle = utilities.gridViewDateTimeCellStyle();
            dgvRedemptionCurrencyRuleDetails.Columns["quantityDataGridViewTextBoxColumn"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            log.LogMethodExit();
        }

        /// <summary>
        /// RedemptionCurrencyRules CurrentChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedemptionCurrencyRules_CurrentChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            List<RedemptionCurrencyRuleDetailDTO> redemptionCurrencyRuleDetailDTOList;
            try
            {
                if (redemptionCurrencyRuleDTOBindingSource.Current != null && redemptionCurrencyRuleDTOBindingSource.Current is RedemptionCurrencyRuleDTO)
                {
                    RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO = (redemptionCurrencyRuleDTOBindingSource.Current as RedemptionCurrencyRuleDTO);
                    if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList == null)
                    {
                        redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();
                    }
                    redemptionCurrencyRuleDetailDTOList = redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList;
                }
                else
                {
                    redemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();
                }
                redemptionCurrencyRuleDetailDTOBindingSource.DataSource = redemptionCurrencyRuleDetailDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
                log.LogMethodExit(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvRedemptionCurrencyRuleDetails ComboDataError
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRedemptionCurrencyRuleDetails_ComboDataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == dgvRedemptionCurrencyRuleDetails.Columns["currencyNameDataGridViewTextBoxColumn"].Index)
            {
                if (redemptionCurrencyListOnDisplay != null)
                    dgvRedemptionCurrencyRuleDetails.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = redemptionCurrencyListOnDisplay[0].CurrencyName;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// frmRedemptionCurrencyRuleUI Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmRedemptionCurrencyRuleUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadAll();
            log.LogMethodExit();
        }

        /// <summary>
        /// Has Changes
        /// </summary>
        /// <returns></returns>
        private bool HasChanges()
        {
            log.LogMethodEntry();
            bool result = false;
            List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
            foreach (RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO in redemptionCurrencyRuleDTOBindingSource)
            {
                redemptionCurrencyRuleDTOList.Add(redemptionCurrencyRuleDTO);
            }
            //List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = redemptionCurrencyRuleDTOBindingSource.DataSource as List<RedemptionCurrencyRuleDTO>;
            if (redemptionCurrencyRuleDTOList != null && redemptionCurrencyRuleDTOList.Count > 0)
            {
                foreach (var RedemptionCurrencyRuleDTO in redemptionCurrencyRuleDTOList)
                {
                    if (RedemptionCurrencyRuleDTO.IsChangedRecursive)
                    {
                        result = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// btnClose Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        /// <summary>
        /// btnRefresh Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (HasChanges() &&
                   MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 714), MessageContainerList.GetMessage(utilities.ExecutionContext, "Confirm"), MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    return;
                }
                LoadRedemptionCurrencyRules();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnSave Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (!HasChanges())
                {
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371));
                    log.LogMethodExit("!HasChanges()");
                    return;
                }

                dgvRedemptionCurrencyRuleDetails.EndEdit();
                dgvRedemptionCurrencyRule.EndEdit();

                List<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = new List<RedemptionCurrencyRuleDTO>();
                foreach (RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO in redemptionCurrencyRuleDTOBindingSource)
                {
                    redemptionCurrencyRuleDTOList.Add(redemptionCurrencyRuleDTO);
                }
                if (redemptionCurrencyRuleDTOList != null && redemptionCurrencyRuleDTOList.Count > 0)
                {
                    foreach (RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO in redemptionCurrencyRuleDTOList)
                    {
                        if (!Validate(redemptionCurrencyRuleDTO))
                        {
                            log.LogMethodExit("!Validate(redemptionCurrencyRuleDTO)");
                            return;
                        }
                        else
                        {
                            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                            {
                                try
                                {
                                    parafaitDBTrx.BeginTransaction();
                                    RedemptionCurrencyRuleBL redemptionCurrencyRuleBL = new RedemptionCurrencyRuleBL(machineUserContext, redemptionCurrencyRuleDTO);
                                    redemptionCurrencyRuleBL.Save(parafaitDBTrx.SQLTrx);
                                    parafaitDBTrx.EndTransaction();
                                }
                                catch (Exception ex)
                                {
                                    parafaitDBTrx.RollBack();
                                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Error") + " " + ex.Message, MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Error"));
                                    log.LogMethodExit(ex.Message);
                                    return;
                                }
                            }
                        }
                    }
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 122));
                    LoadRedemptionCurrencyRules();
                }
                else
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 371));
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, "Save Error") + " " + ex.Message, "Save Error");
                log.Error(ex);
            }
        }

        /// <summary>
        /// Validate
        /// </summary>
        /// <param name="redemptionCurrencyRuleDTO"></param>
        ///// <returns>bool</returns>
        private bool Validate(RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO)
        {
            log.LogMethodEntry(redemptionCurrencyRuleDTO);
            try
            {
                RedemptionCurrencyRuleBL redemptionCurrencyRuleBL = new RedemptionCurrencyRuleBL(machineUserContext, redemptionCurrencyRuleDTO);
                redemptionCurrencyRuleBL.Validate();

                if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList == null || redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Count <= 0)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2280));
                }

                if (redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList != null && redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList.Count > 0)
                {
                    foreach (RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)
                    {
                        RedemptionCurrencyRuleDetailBL redemptionCurrencyRuleDetailBL = new RedemptionCurrencyRuleDetailBL(machineUserContext, redemptionCurrencyRuleDetailDTO);
                        redemptionCurrencyRuleDetailBL.Validate();
                    }

                    Double? Amount = 0;
                    foreach (RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO in redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList)

                        if (redemptionCurrencyRuleDTO.IsActive == false
                            || (redemptionCurrencyRuleDetailDTO.IsActive == true && ((redemptionCurrencyListOnDisplay.Exists(Rc => Rc.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId)))
                            || redemptionCurrencyRuleDetailDTO.IsActive == false && (redemptionCurrencyListOnDisplay.Find(Rc => Rc.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId) != null
                            && redemptionCurrencyListOnDisplay.Find(Rc => Rc.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId).IsActive == true))
                            || redemptionCurrencyRuleDetailDTO.IsActive == false)
                        {
                            if (redemptionCurrencyRuleDetailDTO.IsActive == true)
                            {
                                if (redemptionCurrencyListOnDisplay.Exists(sD => sD.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId))
                                {
                                    Double ValueInTickets = redemptionCurrencyListOnDisplay.Find(rD => rD.CurrencyId == redemptionCurrencyRuleDetailDTO.CurrencyId).ValueInTickets;

                                    {
                                        Amount = Amount + (ValueInTickets * redemptionCurrencyRuleDetailDTO.Quantity);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2605));//Currency rule cannot have inactive currency record
                        }
                    if (Amount >= (Double)redemptionCurrencyRuleDTO.Amount && (Double)redemptionCurrencyRuleDTO.Percentage == 0)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2279, redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId.ToString() == "-1" ? String.Empty : redemptionCurrencyRuleDTO.RedemptionCurrencyRuleId.ToString(), redemptionCurrencyRuleDTO.RedemptionCurrencyRuleName));
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
                log.LogMethodExit(false, ex.Message);
                return false;
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// redemptionCurrencyRulesDTOBindingSource AddingNew
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void redemptionCurrencyRulesDTOBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO = new RedemptionCurrencyRuleDTO();
                redemptionCurrencyRuleDTO.RedemptionCurrencyRuleDetailDTOList = new List<RedemptionCurrencyRuleDetailDTO>();
                e.NewObject = redemptionCurrencyRuleDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// btnDelete Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                //if( e.RowIndex > -1)
                if (dgvfocused != null)
                {
                    if (this.dgvfocused.SelectedRows.Count <= 0 && this.dgvfocused.SelectedCells.Count <= 0)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 959));
                        log.Debug("Ends-btnDelete_Click event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
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
                        if (dgvfocused.Name == "dgvRedemptionCurrencyRule")
                        {
                            if (dgvSelectedRow.Cells["redemptionCurrencyRuleIdDataGridViewTextBoxColumn"].Value == null)
                            {
                                return;
                            }
                            if (Convert.ToInt32(dgvSelectedRow.Cells["redemptionCurrencyRuleIdDataGridViewTextBoxColumn"].Value.ToString()) <= 0)
                            {
                                dgvfocused.Rows.RemoveAt(dgvSelectedRow.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    BindingSource focussedGridDataListDTOBS = (BindingSource)dgvfocused.DataSource;
                                    {
                                        var redemptionCurrencyRuleDTOList = (SortableBindingList<RedemptionCurrencyRuleDTO>)focussedGridDataListDTOBS.DataSource;
                                        RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO = redemptionCurrencyRuleDTOList[dgvSelectedRow.Index];
                                        redemptionCurrencyRuleDTO.IsActive = false;
                                        RedemptionCurrencyRuleBL redemptionCurrencyRule = new RedemptionCurrencyRuleBL(machineUserContext, redemptionCurrencyRuleDTO);
                                        redemptionCurrencyRule.Save();
                                        break;

                                    }
                                }
                            }
                        }
                        if (dgvfocused.Name == "dgvRedemptionCurrencyRuleDetails")
                        {
                            if (dgvSelectedRow.Cells["redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn"].Value == null)
                            {
                                return;
                            }
                            if (Convert.ToInt32(dgvSelectedRow.Cells["redemptionCurrencyRuleDetailIdDataGridViewTextBoxColumn"].Value.ToString()) <= 0)
                            {
                                dgvfocused.Rows.RemoveAt(dgvSelectedRow.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    BindingSource focussedGridDataListDTOBS = (BindingSource)dgvfocused.DataSource;
                                    {
                                        var redemptionCurrencyRuleDetailDTOList = (List<RedemptionCurrencyRuleDetailDTO>)focussedGridDataListDTOBS.DataSource;
                                        RedemptionCurrencyRuleDetailDTO redemptionCurrencyRuleDetailDTO = redemptionCurrencyRuleDetailDTOList[dgvSelectedRow.Index];
                                        redemptionCurrencyRuleDetailDTO.IsActive = false;
                                        RedemptionCurrencyRuleDTO parentRuleDTO = GetParentRuleDTO(redemptionCurrencyRuleDetailDTO.RedemptionCurrencyRuleId);
                                        if (parentRuleDTO == null)
                                        {
                                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 2262), MessageContainerList.GetMessage(utilities.ExecutionContext, "ERROR"));
                                        }
                                        else
                                        {
                                            RedemptionCurrencyRuleBL redemptionCurrencyRuleBL = new RedemptionCurrencyRuleBL(machineUserContext, parentRuleDTO);
                                            redemptionCurrencyRuleBL.ValidateRule();
                                        }
                                        RedemptionCurrencyRuleDetailBL redemptionCurrencyRuleDetail = new RedemptionCurrencyRuleDetailBL(machineUserContext, redemptionCurrencyRuleDetailDTO);
                                        redemptionCurrencyRuleDetail.Save();
                                        break;

                                    }
                                }
                            }
                        }

                    }
                    if (rowsDeleted == true)
                        MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 957));
                    LoadAll();
                }
            }
            catch (Exception ex)
            {
                LoadRedemptionCurrencyRules();
                log.Error(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private RedemptionCurrencyRuleDTO GetParentRuleDTO(int redemptionCurrencyRuleId)
        {
            log.LogMethodEntry();
            RedemptionCurrencyRuleDTO redemptionCurrencyRuleDTO = null;

            SortableBindingList<RedemptionCurrencyRuleDTO> redemptionCurrencyRuleDTOList = (SortableBindingList<RedemptionCurrencyRuleDTO>)redemptionCurrencyRuleDTOBindingSource.DataSource;
            redemptionCurrencyRuleDTO = redemptionCurrencyRuleDTOList.Single(rule => rule.RedemptionCurrencyRuleId == redemptionCurrencyRuleId);

            log.LogMethodExit(redemptionCurrencyRuleDTO);
            return redemptionCurrencyRuleDTO;
        }

        /// <summary>
        /// dgvRedemptionCurrencyRule Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRedemptionCurrencyRule_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        }

        /// <summary>
        /// dgvRedemptionCurrencyRuleDetail Enter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRedemptionCurrencyRuleDetail_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            dgvfocused = (DataGridView)sender;
            log.LogMethodExit();
        }

        private void dgvRedemptionCurrencyRule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                try
                {
                    if (dgvRedemptionCurrencyRule.Columns[e.ColumnIndex].Name == "isActiveDataGridViewCheckBoxColumn"
                        || dgvRedemptionCurrencyRule.Columns[e.ColumnIndex].Name == "cumulativeDataGridViewCheckBoxColumn")
                    {
                        string columnName = dgvRedemptionCurrencyRule.Columns[e.ColumnIndex].Name;
                        DataGridViewCheckBoxCell checkBox = (dgvRedemptionCurrencyRule[columnName, e.RowIndex] as DataGridViewCheckBoxCell);
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            checkBox.Value = false;
                        }
                        else
                        {
                            checkBox.Value = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message), MessageContainerList.GetMessage(utilities.ExecutionContext, "ERROR"));

                }
            }
            log.LogMethodExit();
        }

        private void dgvRedemptionCurrencyRuleDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                try
                {
                    if (dgvRedemptionCurrencyRuleDetails.Columns[e.ColumnIndex].Name == "isActiveDataGridViewCheckBoxColumn1")
                    {
                        string columnName = dgvRedemptionCurrencyRuleDetails.Columns[e.ColumnIndex].Name;
                        DataGridViewCheckBoxCell checkBox = (dgvRedemptionCurrencyRuleDetails[columnName, e.RowIndex] as DataGridViewCheckBoxCell);
                        if (Convert.ToBoolean(checkBox.Value))
                        {
                            checkBox.Value = false;
                        }
                        else
                        {
                            checkBox.Value = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 1824, ex.Message), MessageContainerList.GetMessage(utilities.ExecutionContext, "ERROR"));

                }
            }
            log.LogMethodExit();
        }

        private void dgvRedemptionCurrencyRule_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            MessageBox.Show(utilities.MessageUtils.getMessage(1512));
            e.Cancel = true;
            log.LogMethodExit();
        }
    }
}
