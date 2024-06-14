/********************************************************************************************
 * Project Name - DiscountCouponsUI
 * Description  - Discount Coupons UI 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        19-APR-2019   Raghuveera      added btnImport_Click Event 
 *2.70.2      05-Aug-2019   Girish Kundar   Added LogMethodEntry() and LogMethodExit() methods. 
 *2.80        27-Jun-2020   Deeksha         Modified to Make Product module read only in Windows Management Studio.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// DiscountCouponsUI Class
    /// </summary>
    public partial class DiscountCouponsUI : Form
    {
        private Utilities utilities;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DiscountCouponsHeaderDTO discountCouponsHeaderDTO;
        private ManagementStudioSwitch managementStudioSwitch;
        /// <summary>
        /// Constructor of DiscountCouponsUI class
        /// </summary>
        /// <param name="utilities">Parafait utilities</param>
        /// <param name="discountId">Discount Id</param>
        public DiscountCouponsUI(Utilities utilities, int discountId)
        {
            log.LogMethodEntry(utilities, discountId);
            InitializeComponent();
            this.utilities = utilities;
            utilities.setupDataGridProperties(ref dgvDiscountCouponsDTOList);
            utilities.setLanguage(this);
            DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(utilities.ExecutionContext);
            List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID, discountId.ToString()));
            searchParameters.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
            List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList = discountCouponsHeaderListBL.GetDiscountCouponsHeaderDTOList(searchParameters);
            if(discountCouponsHeaderDTOList != null && discountCouponsHeaderDTOList.Count > 0)
            {
                discountCouponsHeaderDTO = discountCouponsHeaderDTOList[0];
            }
            else
            {
                discountCouponsHeaderDTO = new DiscountCouponsHeaderDTO();
                discountCouponsHeaderDTO.DiscountId = discountId;
                discountCouponsHeaderDTO.EffectiveDate = DateTime.Today;
                discountCouponsHeaderDTO.ExpiryDate = DateTime.Today.AddDays(1);
                discountCouponsHeaderDTO.Count = 1;
                discountCouponsHeaderDTO.Sequential = true;
            }
            expiryDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = "dd-MMM-yyyy";
            startDateDataGridViewTextBoxColumn.DefaultCellStyle.Format = "dd-MMM-yyyy";
            managementStudioSwitch = new ManagementStudioSwitch(utilities.ExecutionContext);
            dgvDiscountCouponsDTOList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            UpdateUIElements();
            log.LogMethodExit();
        }

        private void DiscountCouponsUI_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            nudCouponCount.Maximum = int.MaxValue;
            nudExpiresInDays.Maximum = int.MaxValue;
            dtpEffectiveDate.Value = (DateTime) discountCouponsHeaderDTO.EffectiveDate;
            dtpExpiryDate.Value = (DateTime) discountCouponsHeaderDTO.ExpiryDate;
            dtpExpiryDate.MinDate = ((DateTime) discountCouponsHeaderDTO.EffectiveDate).AddDays(1);
            if(discountCouponsHeaderDTO.ExpiresInDays == null)
            {
                nudExpiresInDays.Text = "";
            }
            else
            {
                nudExpiresInDays.Value = (int) discountCouponsHeaderDTO.ExpiresInDays;
            }
            nudCouponCount.Value = (int) discountCouponsHeaderDTO.Count;
            chbPrintCoupons.Checked = (discountCouponsHeaderDTO.PrintCoupon == "Y");
            chbSequential.Checked = discountCouponsHeaderDTO.Sequential;
            RefreshData();
            log.LogMethodExit();
        }

        private void UpdateDiscountCouponsHeaderDTO()
        {
            log.LogMethodEntry();
            if (discountCouponsHeaderDTO.EffectiveDate != dtpEffectiveDate.Value)
            {
                discountCouponsHeaderDTO.EffectiveDate = dtpEffectiveDate.Value;
            }
            if(discountCouponsHeaderDTO.ExpiryDate != dtpExpiryDate.Value)
            {
                discountCouponsHeaderDTO.ExpiryDate = dtpExpiryDate.Value;
            }
            int? expiresInDays = null;
            if(string.IsNullOrWhiteSpace(nudExpiresInDays.Text) == false)
            {
                expiresInDays = Convert.ToInt32(nudExpiresInDays.Value);
            }
            if(expiresInDays != discountCouponsHeaderDTO.ExpiresInDays)
            {
                discountCouponsHeaderDTO.ExpiresInDays = expiresInDays;
            }
            if(Convert.ToInt32(nudCouponCount.Value) != (int)discountCouponsHeaderDTO.Count)
            {
                discountCouponsHeaderDTO.Count = Convert.ToInt32(nudCouponCount.Value);
            }
            if(chbPrintCoupons.Checked)
            {
                if(discountCouponsHeaderDTO.PrintCoupon == "N")
                {
                    discountCouponsHeaderDTO.PrintCoupon = "Y";
                }
            }
            else
            {
                if (discountCouponsHeaderDTO.PrintCoupon == "Y")
                {
                    discountCouponsHeaderDTO.PrintCoupon = "N";
                }
            }
            if (chbSequential.Checked)
            {
                if (discountCouponsHeaderDTO.Sequential == false)
                {
                    discountCouponsHeaderDTO.Sequential = true;
                }
            }
            else
            {
                if (discountCouponsHeaderDTO.Sequential == true)
                {
                    discountCouponsHeaderDTO.Sequential = false;
                }
            }
            log.LogVariableState("discountCouponsHeaderDTO" , discountCouponsHeaderDTO);
            log.LogMethodExit();
        }

        private void RefreshData()
        {
            log.LogMethodEntry();
            LoadDiscountCouponsDTOList();
            log.LogMethodExit();
        }

        private void LoadDiscountCouponsDTOList()
        {
            log.LogMethodEntry();
            List<DiscountCouponsDTO> discountCouponsDTOList = null;
            if(discountCouponsHeaderDTO.Id != -1)
            {
                DiscountCouponsListBL discountCouponsListBL = new DiscountCouponsListBL(utilities.ExecutionContext);
                List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>>();
                if(chbShowActiveEntries.Checked)
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.IS_ACTIVE,"Y"));
                }
                if(string.IsNullOrWhiteSpace(txtTransactionId.Text) == false)
                {
                    int transactionId;
                    if(int.TryParse(txtTransactionId.Text,out transactionId))
                    {
                        searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.TRANSACTION_ID, txtTransactionId.Text));
                    }
                }
                //searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
                if(discountCouponsHeaderDTO.MasterEntityId != -1 && utilities.ParafaitEnv.IsCorporate)
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.MASTER_HEADER_ID, discountCouponsHeaderDTO.MasterEntityId.ToString()));
                }
                else
                {
                    searchParameters.Add(new KeyValuePair<DiscountCouponsDTO.SearchByParameters, string>(DiscountCouponsDTO.SearchByParameters.DISCOUNT_COUPONS_HEADER_ID, discountCouponsHeaderDTO.Id.ToString()));
                }
                discountCouponsDTOList = discountCouponsListBL.GetDiscountCouponsDTOList(searchParameters);
                log.LogVariableState("discountCouponsDTOList" , discountCouponsDTOList);
            }

            SortableBindingList<DiscountCouponsDTO> discountCouponsDTOSortableList;
            if(discountCouponsDTOList != null)
            {
                discountCouponsDTOSortableList = new SortableBindingList<DiscountCouponsDTO>(
                                                        discountCouponsDTOList.FindAll(
                                                            delegate (DiscountCouponsDTO discountCouponsDTO) {
                                                                bool returnValue = false;
                                                                if(string.IsNullOrWhiteSpace(txtName.Text))
                                                                {
                                                                    returnValue = true;
                                                                }
                                                                else
                                                                {
                                                                    returnValue = (string.IsNullOrEmpty(discountCouponsDTO.FromNumber) == false &&
                                                                                    discountCouponsDTO.FromNumber.IndexOf(txtName.Text, StringComparison.OrdinalIgnoreCase) >= 0) ||
                                                                                    (string.IsNullOrEmpty(discountCouponsDTO.ToNumber) == false &&
                                                                                    discountCouponsDTO.ToNumber.IndexOf(txtName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
                                                                }
                                                                return returnValue;
                                                            }));
            }
            else
            {
                discountCouponsDTOSortableList = new SortableBindingList<DiscountCouponsDTO>();
            }
            log.LogVariableState("discountCouponsDTOSortableList", discountCouponsDTOSortableList);
            discountCouponsDTOListBS.DataSource = discountCouponsDTOSortableList;
            log.LogMethodExit();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            LoadDiscountCouponsDTOList();
            log.LogMethodExit();
        }

        private void dgvDiscountCouponsDTOList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            MessageBox.Show(utilities.MessageUtils.getMessage(1144).Replace("&1", dgvDiscountCouponsDTOList.Columns[e.ColumnIndex].HeaderText));
            e.Cancel = true;
            log.LogMethodExit();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            this.Close();
            log.LogMethodExit();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            chbShowActiveEntries.Checked = true;
            txtName.ResetText();
            txtTransactionId.ResetText();
            RefreshData();
            log.LogMethodExit();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            UpdateDiscountCouponsHeaderDTO();
            if(discountCouponsHeaderDTO.IsChanged)
            {
                try
                {
                    DiscountCouponsHeaderBL discountCouponsHeaderBL = new DiscountCouponsHeaderBL(utilities.ExecutionContext, discountCouponsHeaderDTO);
                    discountCouponsHeaderBL.Save();
                }
                catch(Exception)
                {
                    log.Error("Error while saving discountCouponsHeader.");
                    MessageBox.Show(utilities.MessageUtils.getMessage(718));
                    return;
                }
            }
            dgvDiscountCouponsDTOList.EndEdit();
            SortableBindingList<DiscountCouponsDTO> discountCouponsDTOSortableList = (SortableBindingList<DiscountCouponsDTO>)discountCouponsDTOListBS.DataSource;
            string message;
            DiscountCouponsBL discountCouponsBL;
            bool error = false;            
            if (discountCouponsDTOSortableList != null)
            {
                for(int i = 0; i < discountCouponsDTOSortableList.Count; i++)
                {
                    if(discountCouponsDTOSortableList[i].IsChanged)
                    {
                        message = ValidateDiscountCouponsDTO(discountCouponsDTOSortableList[i]);
                        if(string.IsNullOrEmpty(message))
                        {
                            try
                            {
                                if(discountCouponsDTOSortableList[i].DiscountCouponHeaderId != discountCouponsHeaderDTO.Id)
                                {
                                    discountCouponsDTOSortableList[i].DiscountCouponHeaderId = discountCouponsHeaderDTO.Id;
                                }
                                discountCouponsBL = new DiscountCouponsBL(utilities.ExecutionContext, discountCouponsDTOSortableList[i]);                                
                                discountCouponsBL.Save();                                
                            }
                            catch(ForeignKeyException ex)
                            {
                                log.Error(ex.Message);
                                dgvDiscountCouponsDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                                break;
                            }
                            catch(DuplicateCouponException ex)
                            {
                                log.Error(ex.Message);
                                dgvDiscountCouponsDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(1128));
                                break;
                            }
                            catch(Exception ex)
                            {
                                error = true;
                                log.Error(ex.Message);
                                log.Error("Error while saving discountCoupons.");
                                dgvDiscountCouponsDTOList.Rows[i].Selected = true;
                                MessageBox.Show(utilities.MessageUtils.getMessage(718));
                                break;
                            }
                        }
                        else
                        {
                            error = true;
                            dgvDiscountCouponsDTOList.Rows[i].Selected = true;
                            MessageBox.Show(message);
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(371));
            }
            if(!error)
            {
                btnSearch.PerformClick();
            }
            else
            {
                dgvDiscountCouponsDTOList.Update();
                dgvDiscountCouponsDTOList.Refresh();
            }
            log.LogMethodExit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (dgvDiscountCouponsDTOList.SelectedRows.Count <= 0 && dgvDiscountCouponsDTOList.SelectedCells.Count <= 0)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(959));
                log.Debug("Ends-btnDelete_Click() event by \"No rows selected. Please select the rows you want to delete and press delete..\" message ");
                return;
            }
            bool rowsDeleted = false;
            bool confirmDelete = false;
            bool refreshFromDB = false;
            if(this.dgvDiscountCouponsDTOList.SelectedCells.Count > 0)
            {
                foreach(DataGridViewCell cell in dgvDiscountCouponsDTOList.SelectedCells)
                {
                    dgvDiscountCouponsDTOList.Rows[cell.RowIndex].Selected = true;
                }
            }
            foreach(DataGridViewRow row in dgvDiscountCouponsDTOList.SelectedRows)
            {
                if(Convert.ToInt32(row.Cells[0].Value.ToString()) <= 0)
                {
                    dgvDiscountCouponsDTOList.Rows.RemoveAt(row.Index);
                    rowsDeleted = true;
                }
                else
                {
                    if(confirmDelete || (MessageBox.Show(utilities.MessageUtils.getMessage(958), "Confirm Inactvation.", MessageBoxButtons.YesNo) == DialogResult.Yes))
                    {
                        confirmDelete = true;
                        refreshFromDB = true;
                        SortableBindingList<DiscountCouponsDTO> discountCouponsDTOSortableList = (SortableBindingList<DiscountCouponsDTO>)discountCouponsDTOListBS.DataSource;
                        DiscountCouponsDTO discountCouponsDTO = discountCouponsDTOSortableList[row.Index];
                        discountCouponsDTO.IsActive = false;
                        DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(utilities.ExecutionContext, discountCouponsDTO);
                        try
                        {
                            discountCouponsBL.Save();
                        }
                        catch(ForeignKeyException ex)
                        {
                            log.Error(ex.Message);
                            dgvDiscountCouponsDTOList.Rows[row.Index].Selected = true;
                            MessageBox.Show(utilities.MessageUtils.getMessage(1143));
                            continue;
                        }
                    }
                }
            }
            if(rowsDeleted == true)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(957));
            }
            if(refreshFromDB == true)
            {
                btnSearch.PerformClick();
            }
            log.LogMethodExit();
        }

        private string ValidateDiscountCouponsDTO(DiscountCouponsDTO discountCouponsDTO)
        {
            log.LogMethodEntry(discountCouponsDTO);
            string message = string.Empty;
            try
            {
                DiscountCouponsBL discountCouponsBL = new DiscountCouponsBL(utilities.ExecutionContext, discountCouponsDTO);
                discountCouponsBL.ValidateCouponDefinition();
            }
            catch(DuplicateCouponException)
            {
                message = utilities.MessageUtils.getMessage(1128);
            }
            catch(ForeignKeyException)
            {
                message = utilities.MessageUtils.getMessage(1143);
            }
            catch(Exception ex)
            {
                message = ex.Message;
            }
            if(discountCouponsDTO.Count <= 0)
            {
                message = utilities.MessageUtils.getMessage(1144).Replace("&1", countDataGridViewTextBoxColumn.HeaderText);
            }
            if(discountCouponsDTO.ExpiryDate < discountCouponsDTO.StartDate)
            {
                message = utilities.MessageUtils.getMessage(1144).Replace("&1", expiryDateDataGridViewTextBoxColumn.HeaderText);
            }
            if(string.IsNullOrWhiteSpace(discountCouponsDTO.FromNumber) || discountCouponsDTO.FromNumber.Contains(" "))
            {
                message = utilities.MessageUtils.getMessage(1144).Replace("&1", fromNumberDataGridViewTextBoxColumn.HeaderText);
            }
            if (string.IsNullOrWhiteSpace(discountCouponsDTO.ToNumber) == false && discountCouponsDTO.ToNumber.Contains(" "))
            {
                message = utilities.MessageUtils.getMessage(1144).Replace("&1", toNumberDataGridViewTextBoxColumn.HeaderText);
            }
            if (discountCouponsDTO.ExpiryDate.Value.Date > dtpExpiryDate.Value.Date)
            {
                message = utilities.MessageUtils.getMessage(1144).Replace("&1", expiryDateDataGridViewTextBoxColumn.HeaderText);
            }
            if(discountCouponsDTO.StartDate.Value.Date < dtpEffectiveDate.Value.Date)
            {
                message = utilities.MessageUtils.getMessage(1144).Replace("&1", startDateDataGridViewTextBoxColumn.HeaderText);
            }
            if(discountCouponsDTO.Count <= 0)
            {
                message = utilities.MessageUtils.getMessage(1144).Replace("&1", countDataGridViewTextBoxColumn.HeaderText);
            }
            log.LogMethodExit(message);
            return message;
        }

        private void dtpEffectiveDate_ValueChanged(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender,e );
            if (dtpExpiryDate.Value <= dtpEffectiveDate.Value)
            {
                dtpExpiryDate.Value = dtpEffectiveDate.Value.AddDays(1);
            }
            dtpExpiryDate.MinDate = dtpEffectiveDate.Value.AddDays(1);
            log.LogMethodExit();
        }

        private void discountCouponsDTOListBS_AddingNew(object sender, AddingNewEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            DiscountCouponsDTO discountCouponsDTO = new DiscountCouponsDTO();
            if(dtpEffectiveDate.Value < DateTime.Today)
            {
                discountCouponsDTO.StartDate = DateTime.Today;
            }
            else
            {
                discountCouponsDTO.StartDate = dtpEffectiveDate.Value.Date;
            }
            DateTime expiryDate = dtpExpiryDate.Value.Date;
            if (string.IsNullOrWhiteSpace(nudExpiresInDays.Text) == false)
            {
                DateTime expiresInDaysDate = discountCouponsDTO.StartDate.Value.Date.AddDays(Convert.ToInt32(nudExpiresInDays.Value));
                if(expiresInDaysDate < expiryDate)
                {
                    expiryDate = expiresInDaysDate;
                }
            }
            discountCouponsDTO.ExpiryDate = expiryDate;
            discountCouponsDTO.DiscountId = discountCouponsHeaderDTO.DiscountId;
            e.NewObject = discountCouponsDTO;
            log.LogMethodExit();
        }

        private void btnCouponsUsed_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (discountCouponsHeaderDTO.Id != -1)
            {
                DiscountCouponsUsedDTOListUI discountCouponsUsedDTOListUI = new DiscountCouponsUsedDTOListUI(utilities, discountCouponsHeaderDTO.Id);
                discountCouponsUsedDTOListUI.ShowDialog();
            }
            else
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(989));
            }
            log.LogMethodExit();
        }

        private void dgvDiscountCouponsDTOList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            string message = string.Empty;
            if(e.ColumnIndex == expiryDateDataGridViewTextBoxColumn.Index)
            {
                DateTime expiryDate;
                if(DateTime.TryParse(e.FormattedValue.ToString(), out expiryDate))
                {
                    DateTime effectiveDate = Convert.ToDateTime(dgvDiscountCouponsDTOList[startDateDataGridViewTextBoxColumn.Index, e.RowIndex].FormattedValue.ToString());
                    if(expiryDate < effectiveDate)
                    {
                        message = utilities.MessageUtils.getMessage(1144).Replace("&1", expiryDateDataGridViewTextBoxColumn.HeaderText);
                    }
                    //if(expiryDate > dtpExpiryDate.Value)
                    //{
                    //    message = utilities.MessageUtils.getMessage(1144).Replace("&1", expiryDateDataGridViewTextBoxColumn.HeaderText);
                    //}
                }
                else
                {
                    message = utilities.MessageUtils.getMessage(1144).Replace("&1", expiryDateDataGridViewTextBoxColumn.HeaderText);
                    
                }
            }
            if(e.ColumnIndex == startDateDataGridViewTextBoxColumn.Index)
            {
                DateTime effectiveDate;
                if(DateTime.TryParse(e.FormattedValue.ToString(), out effectiveDate))
                {
                    DateTime expiryDate = Convert.ToDateTime(dgvDiscountCouponsDTOList[expiryDateDataGridViewTextBoxColumn.Index, e.RowIndex].FormattedValue.ToString());
                    if(expiryDate < effectiveDate)
                    {
                        message = utilities.MessageUtils.getMessage(1144).Replace("&1", startDateDataGridViewTextBoxColumn.HeaderText);
                    }
                    //if(effectiveDate < dtpEffectiveDate.Value)
                    //{
                    //    message = utilities.MessageUtils.getMessage(1144).Replace("&1", startDateDataGridViewTextBoxColumn.HeaderText);
                    //}
                }
                else
                {
                    message = utilities.MessageUtils.getMessage(1144).Replace("&1", startDateDataGridViewTextBoxColumn.HeaderText);

                }
            }
            if(string.IsNullOrWhiteSpace(message) == false)
            {
                e.Cancel = true;
                MessageBox.Show(message);
            }
            log.LogMethodExit();
        }

        private void dgvDiscountCouponsDTOList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.Control is TextBox && 
                dgvDiscountCouponsDTOList.CurrentCell != null && 
                (dgvDiscountCouponsDTOList.CurrentCell.ColumnIndex == fromNumberDataGridViewTextBoxColumn.Index ||
                dgvDiscountCouponsDTOList.CurrentCell.ColumnIndex == toNumberDataGridViewTextBoxColumn.Index))
            {
                TextBox textBox = e.Control as TextBox;
                textBox.CharacterCasing = CharacterCasing.Upper;
            }
            log.LogMethodExit();
        }

        private void dgvDiscountCouponsDTOList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (e.ColumnIndex == transactionIdDataGridViewTextBoxColumn.Index)
            {
                if(e.Value != null)
                {
                    int value = Convert.ToInt32(e.Value);
                    if(value  == -1)
                    {
                        e.Value = "";
                        e.FormattingApplied = true;
                    }
                }
            }
            log.LogMethodExit();

        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if(discountCouponsHeaderDTO.Id==-1)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(665));
                return;
            }
            using (ImportDiscountCoupon importDiscountCoupon = new ImportDiscountCoupon(utilities, discountCouponsHeaderDTO.DiscountId, discountCouponsHeaderDTO.Id))
            {
                importDiscountCoupon.ShowDialog();
                btnRefresh.PerformClick();
            }
            log.LogMethodExit();
        }

        private void UpdateUIElements()
        {
            log.LogMethodEntry();
            if (managementStudioSwitch.EnablProductModule)
            {
                dgvDiscountCouponsDTOList.AllowUserToAddRows = true;
                dgvDiscountCouponsDTOList.ReadOnly = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                dgvDiscountCouponsDTOList.AllowUserToAddRows = false;
                dgvDiscountCouponsDTOList.ReadOnly = true;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
            }
            log.LogMethodExit();
        }
    }
}
