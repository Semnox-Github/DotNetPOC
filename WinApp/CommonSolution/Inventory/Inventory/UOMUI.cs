/********************************************************************************************
 * Project Name -UOM
 * Description  -UI of  UOM 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.70.2        1-Aug-2019    Deeksha          Modified:Added log() methods.
 *2.70.3        23-Mar-2020   Girish           Modified: UOM name validation issue fix.
 *2.100.0       05-Aug-2020   Deeksha          Modified for Recipe Management enhancement
 ********************************************************************************************/
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using System.ComponentModel;
using Semnox.Parafait.Publish;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    public partial class UOMUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineUserContext = null;
        private Utilities utilities;
        private BindingSource uomListBS;
        List<UOMDTO> uomListOnDisplay;

        public UOMUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            try
            {
                InitializeComponent();
                utilities = _Utilities;
                machineUserContext = utilities.ExecutionContext;
                utilities.setupDataGridProperties(ref dgvUOM);
                if (utilities.ParafaitEnv.IsCorporate)
                {
                    machineUserContext.SetSiteId(utilities.ParafaitEnv.SiteId);
                }
                else
                {
                    machineUserContext.SetSiteId(-1);
                }
                machineUserContext.SetUserId(utilities.ParafaitEnv.LoginID);
                if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
                {
                    lnkPublishToSite.Visible = true;
                }
                else
                {
                    lnkPublishToSite.Visible = false;
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit();
                MessageBox.Show(ex.Message);
            }
        }

        private void UOMUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            utilities.setLanguage(this);
            PopulateUOM();
            dgvUOM.Columns["CreationDate"].DefaultCellStyle.Format = utilities.getDateTimeFormat();
            dgvUOM.Columns["LastUpdateDate"].DefaultCellStyle.Format = utilities.getDateTimeFormat();
            log.LogMethodExit();
        }

        public void PopulateUOM()
        {
            log.LogMethodEntry();
            try
            {
                UOMList uomList = new UOMList(machineUserContext);
                List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomSearchParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
                uomSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, machineUserContext.GetSiteId().ToString()));
                uomListOnDisplay = uomList.GetAllUOMDTOList(uomSearchParams, true, true);
                uomListBS = new BindingSource();
                if (uomListOnDisplay != null)
                    uomListBS.DataSource = new SortableBindingList<UOMDTO>(uomListOnDisplay);
                else
                    uomListBS.DataSource = new SortableBindingList<UOMDTO>();
                uomListBS.AddingNew += dgvUOM_BindingSourceAddNew;
                dgvUOM.DataSource = uomListBS;
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
        }


        void dgvUOM_BindingSourceAddNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (dgvUOM.Rows.Count == uomListBS.Count)
                {
                    uomListBS.RemoveAt(uomListBS.Count - 1);
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                BindingSource uomListBS = (BindingSource)dgvUOM.DataSource;
                var uomListOnDisplay = (SortableBindingList<UOMDTO>)uomListBS.DataSource;
                if (uomListOnDisplay.Count > 0)
                {
                    List<UOMDTO> tempList = new List<UOMDTO>(uomListOnDisplay);
                    var isNull = tempList.Any(item => item.UOM == null);
                    if (isNull)
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2607, "UOM"), "Validation Error");
                        return;
                    }
                    List<string> nameList = tempList.Select(x => x.UOM.Trim().ToLower()).ToList();
                    if (nameList.Count != nameList.Distinct().Count())
                    {
                        MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2608, "UOM"), "Validation Error");
                        return;
                    }

                    foreach (UOMDTO uomDTO in uomListOnDisplay)
                    {
                        if (uomDTO.IsChanged)
                        {
                            if (string.IsNullOrEmpty(uomDTO.UOM.Trim()))
                            {
                                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 2607, "UOM"), "Validation Error");
                                return;
                            }
                        }
                        UOM uom = new UOM(machineUserContext, uomDTO);
                        uom.Save();
                    }
                    PopulateUOM();
                }
                else
                    MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PopulateUOM();
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            try
            {
                if (this.dgvUOM.SelectedRows.Count <= 0 && this.dgvUOM.SelectedCells.Count <= 0)
                {
                    MessageBox.Show(utilities.MessageUtils.getMessage(959));
                    log.LogMethodExit();
                    return;
                }
                bool rowsDeleted = false;
                bool confirmDelete = false;
                if (this.dgvUOM.SelectedCells.Count > 0)
                {
                    foreach (DataGridViewCell cell in this.dgvUOM.SelectedCells)
                    {
                        dgvUOM.Rows[cell.RowIndex].Selected = true;
                    }
                }
                foreach (DataGridViewRow uomRow in this.dgvUOM.SelectedRows)
                {
                    if (uomRow.Cells[0].Value != null)
                    {
                        if (Convert.ToInt32(uomRow.Cells[0].Value) < 0)
                        {
                            dgvUOM.Rows.RemoveAt(uomRow.Index);
                            rowsDeleted = true;
                        }
                        else
                        {
                            if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                            {
                                confirmDelete = true;
                                BindingSource uomDTOListDTOBS = (BindingSource)dgvUOM.DataSource;
                                var uomDTOList = (SortableBindingList<UOMDTO>)uomDTOListDTOBS.DataSource;
                                UOMDTO uomDTO = uomDTOList[uomRow.Index];
                                uomDTO.IsActive = false;
                                UOM uom = new UOM(machineUserContext, uomDTO);
                                uom.Save();
                            }
                        }
                    }
                }
                if (rowsDeleted == true)
                    MessageBox.Show(utilities.MessageUtils.getMessage(957));
                PopulateUOM();
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                MessageBox.Show(MessageContainerList.GetMessage(machineUserContext, 1083) + ex.Message);
                log.LogMethodExit(ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void dgvUOM_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            PublishUI publishUI;
            if (dgvUOM.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvUOM.SelectedCells)
                {
                    dgvUOM.Rows[cell.RowIndex].Selected = true;
                }
            }
            if (dgvUOM.SelectedRows.Count > 0)
            {
                if (dgvUOM.SelectedRows[0].Cells["uOMDataGridViewTextBoxColumn"].Value != null)
                {
                    publishUI = new PublishUI(utilities, Convert.ToInt32(dgvUOM.SelectedRows[0].Cells["uOMIdDataGridViewTextBoxColumn"].Value), "UOM", dgvUOM.SelectedRows[0].Cells["uOMDataGridViewTextBoxColumn"].Value.ToString());
                    publishUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        private void dgvUOM_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if (dgvUOM.Columns[e.ColumnIndex].Name == "UOMConversion")
            {
                try
                {
                    int uomId = Convert.ToInt32(dgvUOM[uOMIdDataGridViewTextBoxColumn.Index, e.RowIndex].Value);
                    bool isActive = Convert.ToBoolean(dgvUOM[IsActive.Index, e.RowIndex].Value);
                    if (uomId > -1 && isActive && uomListOnDisplay != null && uomListOnDisplay.Any())
                    {
                        UOMConversionFactorUI uomConversionFactor = new UOMConversionFactorUI(utilities, uomId);
                        CommonUIDisplay.setupVisuals(uomConversionFactor);
                        uomConversionFactor.Location = new System.Drawing.Point(180, 40);
                        uomConversionFactor.ShowDialog();
                        uomConversionFactor.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }
    }
}
