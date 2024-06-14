/********************************************************************************************
 * Project Name -Inventory
 * Description  -UI of  UOM Conversion Factor.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.100.0      04-Aug-2020   Deeksha           Created for Recipe Management Enhancement.
 ********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Publish;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Conversion Factor UI
    /// </summary>
    public partial class UOMConversionFactorUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private int uomId = -1;

        /// <summary>
        /// Constructor which accepts utilities as a parameter
        /// </summary>
        /// <param name="_Utilities"></param>
        public UOMConversionFactorUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            this.utilities = _Utilities;
            executionContext = _Utilities.ExecutionContext;
            if (utilities.ParafaitEnv.IsCorporate)
            {
                executionContext.SetSiteId(utilities.ParafaitEnv.SiteId);
            }
            else
            {
                executionContext.SetSiteId(-1);
            }
            executionContext.SetUserId(utilities.ParafaitEnv.LoginID);
            if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite)
            {
                lnkPublishToSite.Visible = true;
            }
            else
            {
                lnkPublishToSite.Visible = false;
            }
            LoadUOMCombobox();
            PopulateUOMConversionFactor();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor which accepts utilities and uomId as a parameter
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="uomId"></param>
        public UOMConversionFactorUI(Utilities utilities, int uomId)
            : this(utilities)
        {
            log.LogMethodEntry(utilities, uomId);
            this.uomId = uomId;
            LoadUOMCombobox();
            PopulateUOMConversionFactor();
            log.LogMethodExit();
        }
        private void UOMConversionFactorUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            utilities.setLanguage(this);
            utilities.setupDataGridProperties(ref dgvUOMConversionFactor);
            dgvUOMConversionFactor.Columns["conversionFactorDataGridViewTextBox"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgvUOMConversionFactor.Columns["conversionFactorDataGridViewTextBox"].DefaultCellStyle = utilities.gridViewNumericCellStyle();
            dgvUOMConversionFactor.Columns["conversionFactorDataGridViewTextBox"].DefaultCellStyle.Format = utilities.ParafaitEnv.INVENTORY_QUANTITY_FORMAT;
            dgvUOMConversionFactor.Columns["conversionFactorDataGridViewTextBox"].Width = 100;
            dgvUOMConversionFactor.Columns["CreationDate"].DefaultCellStyle.Format = utilities.getDateTimeFormat();
            dgvUOMConversionFactor.Columns["LastUpdateDate"].DefaultCellStyle.Format = utilities.getDateTimeFormat();
            log.LogMethodExit();
        }

        private void LoadUOMCombobox()
        {
            log.LogMethodEntry();
            List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> uomListSearchParams = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
            uomListSearchParams.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, executionContext.GetSiteId().ToString()));
            UOMDataHandler uomDataHandler = new UOMDataHandler();
            List<UOMDTO> uomListOnDisplay = uomDataHandler.GetUOMList(uomListSearchParams);

            if (uomListOnDisplay == null)
                uomListOnDisplay = new List<UOMDTO>();
            uomListOnDisplay.Insert(0, new UOMDTO());
            uomListOnDisplay[0].UOM = "None";
            uomListOnDisplay[0].UOMId = -1;
            cmbBaseUOM.DataSource = uomListOnDisplay;
            cmbBaseUOM.DisplayMember = "UOM";
            cmbBaseUOM.ValueMember = "UOMId";
            cmbUOM.DataSource = uomListOnDisplay;
            cmbUOM.DisplayMember = "UOM";
            cmbUOM.ValueMember = "UOMId";
            log.LogMethodExit();
        }

        private void PopulateUOMConversionFactor()
        {
            log.LogMethodEntry(uomId);
            try
            {
                BindingSource uomConversionFactorListBS = new BindingSource();
                List<UOMConversionFactorDTO> uomConversionListOnDisplay = new List<UOMConversionFactorDTO>();
                UOMConversionFactorListBL uomConversionFactorListBL = new UOMConversionFactorListBL(executionContext);
                List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>> uomSearchParams = new List<KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>>();
                uomSearchParams.Add(new KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>(UOMConversionFactorDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (uomId > -1)
                {
                    uomSearchParams.Add(new KeyValuePair<UOMConversionFactorDTO.SearchByParameters, string>(UOMConversionFactorDTO.SearchByParameters.BASE_UOM_ID, uomId.ToString()));
                }
                uomConversionListOnDisplay = uomConversionFactorListBL.GetUOMConversionFactorDTOList(uomSearchParams);
                if (uomConversionListOnDisplay != null)
                    uomConversionFactorListBS.DataSource = new SortableBindingList<UOMConversionFactorDTO>(uomConversionListOnDisplay);
                else
                    uomConversionFactorListBS.DataSource = new SortableBindingList<UOMConversionFactorDTO>();
                dgvUOMConversionFactor.DataSource = uomConversionFactorListBS;
            }
            catch (Exception ex)
            {
                log.LogMethodExit(ex);
                MessageBox.Show(ex.Message);
            }
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.Close();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    BindingSource productBS = (BindingSource)dgvUOMConversionFactor.DataSource;
                    var uomConversionListOnDisplay = (SortableBindingList<UOMConversionFactorDTO>)productBS.DataSource;
                    List<UOMConversionFactorDTO> uomConversionList = new List<UOMConversionFactorDTO>(uomConversionListOnDisplay);
                    bool valid =  Validate(uomConversionList);
                    if (valid)
                    {
                        parafaitDBTrx.BeginTransaction();
                        UOMConversionFactorListBL uomConversionFactorListBL = new UOMConversionFactorListBL(executionContext, uomConversionList);
                        uomConversionFactorListBL.Save(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                        PopulateUOMConversionFactor();
                    }
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                }
            }
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            PopulateUOMConversionFactor();
            log.LogMethodExit();
        }

        private bool Validate(List<UOMConversionFactorDTO> uomConversionFactorDTOList)
        {
            log.LogMethodEntry();
            foreach(UOMConversionFactorDTO uomConversion in uomConversionFactorDTOList)
            {
                if(uomConversion.UOMId > -1 && uomConversion.BaseUOMId > -1 
                   && uomConversion.BaseUOMId == uomConversion.UOMId)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext , 2773)); //Base UOM and Conversion UOM cannot be same
                    log.LogMethodExit();
                    return false;
                }
                if(uomId > -1 && uomConversion.BaseUOMId != uomId)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 2774)); //Please choose a valid parent UOM
                    log.LogMethodExit();
                    return false;
                }
                if(uomConversion.UOMId <= -1 || uomConversion.BaseUOMId <= -1)
                {
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1837)); //Please select a valid entry from the dropdown list
                    log.LogMethodExit();
                    return false;
                }
                if (uomConversion.UOMConversionFactorId <= -1 && (uomConversionFactorDTOList.Exists(x => x.BaseUOMId == uomConversion.BaseUOMId 
                                        & x.UOMConversionFactorId > -1 & x.UOMId == uomConversion.UOMId)))
                {
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1872)); //You cannot insert the duplicate record
                    log.LogMethodExit();
                    return false;
                }
            }
            log.LogMethodExit();
            return true;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            using (ParafaitDBTransaction parafatDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafatDBTrx.BeginTransaction();
                    if (this.dgvUOMConversionFactor.SelectedRows.Count <= 0 && this.dgvUOMConversionFactor.SelectedCells.Count <= 0)
                    {
                        MessageBox.Show(utilities.MessageUtils.getMessage(959));
                        log.LogMethodExit();
                        return;
                    }
                    bool rowsDeleted = false;
                    bool confirmDelete = false;
                    if (this.dgvUOMConversionFactor.SelectedCells.Count > 0)
                    {
                        foreach (DataGridViewCell cell in this.dgvUOMConversionFactor.SelectedCells)
                        {
                            dgvUOMConversionFactor.Rows[cell.RowIndex].Selected = true;
                        }
                    }
                    foreach (DataGridViewRow uomRow in this.dgvUOMConversionFactor.SelectedRows)
                    {
                        if (uomRow.Cells[0].Value != null)
                        {
                            if (Convert.ToInt32(uomRow.Cells[0].Value) < 0)
                            {
                                dgvUOMConversionFactor.Rows.RemoveAt(uomRow.Index);
                                rowsDeleted = true;
                            }
                            else
                            {
                                if (confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactivation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                                {
                                    confirmDelete = true;
                                    BindingSource uomDTOListDTOBS = (BindingSource)dgvUOMConversionFactor.DataSource;
                                    SortableBindingList<UOMConversionFactorDTO> uomDTOList = (SortableBindingList<UOMConversionFactorDTO>)uomDTOListDTOBS.DataSource;
                                    List<UOMConversionFactorDTO> uomConversionFactorDTOList = new List<UOMConversionFactorDTO>(uomDTOList);
                                    UOMConversionFactorDTO uomConversionDTO = uomConversionFactorDTOList[uomRow.Index];
                                    uomConversionDTO.IsActive = false;
                                    UOMConversionFactorBL uomConversionFactor = new UOMConversionFactorBL(executionContext, uomConversionDTO);
                                    uomConversionFactor.Save(parafatDBTrx.SQLTrx);
                                    parafatDBTrx.EndTransaction();
                                }
                            }
                        }
                    }
                    if (rowsDeleted == true)
                        MessageBox.Show(utilities.MessageUtils.getMessage(957));
                    PopulateUOMConversionFactor();
                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    parafatDBTrx.RollBack(); 
                    MessageBox.Show(MessageContainerList.GetMessage(executionContext ,1083) + ex.Message);
                    log.LogMethodExit(ex);
                }
            }
        }

        private void dgvUOMConversionFactor_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            if(e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                if(uomId > -1)
                {
                    dgvUOMConversionFactor.Rows[e.RowIndex].Cells["cmbBaseUOM"].Value = uomId;
                    dgvUOMConversionFactor.Rows[e.RowIndex].Cells["cmbBaseUOM"].ReadOnly = true;
                }
            }
            log.LogMethodExit();
        }

        private void lnkPublishToSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            log.LogMethodEntry();
            UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
            PublishUI publishUI;
            if (dgvUOMConversionFactor.SelectedCells.Count > 0)
            {
                foreach (DataGridViewCell cell in dgvUOMConversionFactor.SelectedCells)
                {
                    dgvUOMConversionFactor.Rows[cell.RowIndex].Selected = true;
                }
            }
            if (dgvUOMConversionFactor.SelectedRows.Count > 0)
            {
                if (dgvUOMConversionFactor.SelectedRows[0].Cells["UOMConversionFactorId"].Value != null)
                {
                    string baseUOM = UOMContainer.uomDTOList.Find(x => x.UOMId == Convert.ToUInt32(dgvUOMConversionFactor.SelectedRows[0].Cells["cmbBaseUOM"].Value)).UOM;
                    string conversionUOM = UOMContainer.uomDTOList.Find(x => x.UOMId == Convert.ToUInt32(dgvUOMConversionFactor.SelectedRows[0].Cells["cmbUOM"].Value)).UOM;
                    publishUI = new PublishUI(utilities, Convert.ToInt32(dgvUOMConversionFactor.SelectedRows[0].Cells["UOMConversionFactorId"].Value), "UOMConversionFactor",
                        baseUOM + " to " + conversionUOM);
                    publishUI.ShowDialog();
                }
            }
            log.LogMethodExit();
        }

        private void dgvUOMConversionFactor_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                MessageBox.Show(MessageContainerList.GetMessage(executionContext, 1144, 
                    MessageContainerList.GetMessage(executionContext , "Conversion Factor"))); // Please enter valid value for &1
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
    }
}
