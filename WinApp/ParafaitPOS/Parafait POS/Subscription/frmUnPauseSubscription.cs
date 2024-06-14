/********************************************************************************************
 * Project Name - frmUnPauseSubscription 
 * Description  - form class to Un Pause Subscription
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.120.0     26-Feb-2021    Guru S A            Created for Subscription phase 2 changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Parafait.Transaction; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Parafait_POS.Subscription
{
    public partial class frmUnPauseSubscription : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private SubscriptionHeaderDTO subscriptionHeaderDTO;
        private List<SubscriptionUnPauseDetailsDTO> unPauseDetailsDTOList = new List<SubscriptionUnPauseDetailsDTO>();
        private string DATE_FORMAT;
        private const string MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION = "MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION";
        public frmUnPauseSubscription(Utilities utilities, int subscriptionHeaderId)
        {
            log.LogMethodEntry(subscriptionHeaderId);
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            SubscriptionHeaderBL subscriptionHeaderBL = new SubscriptionHeaderBL(executionContext, subscriptionHeaderId, true);
            this.subscriptionHeaderDTO = subscriptionHeaderBL.SubscriptionHeaderDTO;
            this.DATE_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            InitializeComponent();
            LoadUIDetails();
            log.LogMethodExit();
        }
        private void LoadUIDetails()
        {
            log.LogMethodEntry();
            SetDGVFormat();
            if (subscriptionHeaderDTO != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null
                && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
            {
                LoadLinkedTransactionIds();
                List<SubscriptionBillingScheduleDTO> pausedSubscriptionBillingSchedules = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.Status == SubscriptionStatus.PAUSED).ToList();
                dgvUnPauseBillingSchedules.DataSource = pausedSubscriptionBillingSchedules;
                BuildSubscriptionUnPauseDetailsDTOList(pausedSubscriptionBillingSchedules);
                SetButtonIconsOnTheGrid();
            }
            log.LogMethodExit();
        }
        private void SetDGVFormat()
        {
            log.LogMethodEntry();
            utilities.setupDataGridProperties(ref dgvUnPauseBillingSchedules);
            billFromDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            billToDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            newBillFromDate.DefaultCellStyle = utilities.gridViewDateCellStyle();
            billAmount.DefaultCellStyle = utilities.gridViewAmountCellStyle();
            log.LogMethodExit();
        }
        private void LoadLinkedTransactionIds()
        {
            log.LogMethodEntry();
            this.transactionId = SubscriptionUIHelper.LoadLinkedTransactionId(utilities, subscriptionHeaderDTO.SubscriptionHeaderId, transactionId);
            log.LogMethodExit();
        }
        private void BuildSubscriptionUnPauseDetailsDTOList(List<SubscriptionBillingScheduleDTO> pausedSubscriptionBillingSchedules)
        {
            log.LogMethodEntry(pausedSubscriptionBillingSchedules);
            unPauseDetailsDTOList = new List<SubscriptionUnPauseDetailsDTO>();
            if (pausedSubscriptionBillingSchedules != null && pausedSubscriptionBillingSchedules.Any())
            {
                for (int i = 0; i < pausedSubscriptionBillingSchedules.Count; i++)
                {
                    SubscriptionBillingScheduleDTO billScheduleDTO = pausedSubscriptionBillingSchedules[i];
                    SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = new SubscriptionUnPauseDetailsDTO(executionContext, billScheduleDTO.SubscriptionHeaderId,
                                  billScheduleDTO.SubscriptionBillingScheduleId, billScheduleDTO.BillFromDate, SubscriptionUnPauseOptions.BILL, null, string.Empty);
                    unPauseDetailsDTOList.Add(unPauseDetailsDTO);
                }
            }
            log.LogMethodExit();
        }
        private void SetButtonIconsOnTheGrid()
        {
            log.LogMethodEntry();
            if (dgvUnPauseBillingSchedules != null && dgvUnPauseBillingSchedules.RowCount > 0)
            {
                for (int rowIndex = 0; rowIndex < dgvUnPauseBillingSchedules.RowCount; rowIndex++)
                {
                    DataGridViewImageCell resumeBillingCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["resumeBilling"];
                    resumeBillingCell.Value = Properties.Resources.ResumeBillingActive;
                    //resumeBillingCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    DataGridViewImageCell cancelBillingScheduleCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["cancelBillingSchedule"];
                    cancelBillingScheduleCell.Value = Properties.Resources.CancelSubscriptionCycleGrayed;
                    //cancelBillingScheduleCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    DataGridViewImageCell postponeBillingScheduleCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["postponeBillingSchedule"];
                    postponeBillingScheduleCell.Value = Properties.Resources.PostponeSubscriptionCycleGrayed;
                    //postponeBillingScheduleCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                }
            }
            log.LogMethodExit();
        }
        private void dgvUnPauseBillingSchedules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    if (dgvUnPauseBillingSchedules.Columns[e.ColumnIndex].Name == "resumeBilling")
                    {
                        SetResetBillNow(e.RowIndex);
                        AdjustNewFromDate();
                    }
                    else if (dgvUnPauseBillingSchedules.Columns[e.ColumnIndex].Name == "cancelBillingSchedule")
                    {
                        SetResetCancelBillingSchedule(e.RowIndex);
                        AdjustNewFromDate();
                    }
                    else if (dgvUnPauseBillingSchedules.Columns[e.ColumnIndex].Name == "postponeBillingSchedule")
                    {
                        SetResetPostPoneBillingSchedule(e.RowIndex);
                    }

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void SetResetBillNow(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = GetUnPauseDetailsDTO(rowIndex);
            if (unPauseDetailsDTO != null && unPauseDetailsDTO.UnPauseAction != SubscriptionUnPauseOptions.BILL)
            {
                if (unPauseDetailsDTO.UnPauseAction == SubscriptionUnPauseOptions.POSTPONE
                    && (MiddleOfSchedulesMarkedForPostpone(rowIndex) || TheLastSchedule(rowIndex)))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2977));
                    // "Sorry, schedule is alredy marked for Postpone. Cannot change it to Resume now"
                }
                if (unPauseDetailsDTO.UnPauseAction == SubscriptionUnPauseOptions.CANCEL
                    && MiddleOfSchedulesMarkedForPostpone(rowIndex))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2982));
                    //"Sorry, there are schedules marked for Postpone. Cannot change current schedule to Resume now"
                }
                int index = unPauseDetailsDTOList.IndexOf(unPauseDetailsDTO);
                unPauseDetailsDTOList[index].UnPauseAction = SubscriptionUnPauseOptions.BILL;
                unPauseDetailsDTOList[index].NewBillFromDate = null;
                dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["newBillFromDate"].Value = null;
                DataGridViewImageCell resumeBillingCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["resumeBilling"];
                resumeBillingCell.Value = Properties.Resources.ResumeBillingActive;
                //resumeBillingCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                DataGridViewImageCell cancelBillingScheduleCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["cancelBillingSchedule"];
                cancelBillingScheduleCell.Value = Properties.Resources.CancelSubscriptionCycleGrayed;
                //cancelBillingScheduleCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                DataGridViewImageCell postponeBillingScheduleCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["postponeBillingSchedule"];
                postponeBillingScheduleCell.Value = Properties.Resources.PostponeSubscriptionCycleGrayed;
                //postponeBillingScheduleCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                dgvUnPauseBillingSchedules.EndEdit();
                dgvUnPauseBillingSchedules.RefreshEdit();
            }
            log.LogMethodExit();
        }
        private bool SubsequentSchedulesMarkedForPostpone(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            bool subsequentSchedulesMarkedForPostpone = false;
            SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = GetUnPauseDetailsDTO(rowIndex);
            if (unPauseDetailsDTO != null)
            {
                int index = unPauseDetailsDTOList.IndexOf(unPauseDetailsDTO);
                int startingRecordIndex = index + 1;
                for (int i = startingRecordIndex; i < unPauseDetailsDTOList.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(unPauseDetailsDTOList[i].UnPauseAction) == false
                        && unPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.POSTPONE)
                    {
                        subsequentSchedulesMarkedForPostpone = true;
                        break;
                    }
                }
            }
            log.LogMethodExit(subsequentSchedulesMarkedForPostpone);
            return subsequentSchedulesMarkedForPostpone;
        }
        private bool PreviousSchedulesMarkedForPostpone(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            bool previousSchedulesMarkedForPostpone = false;
            SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = GetUnPauseDetailsDTO(rowIndex);
            if (unPauseDetailsDTO != null)
            {
                int index = unPauseDetailsDTOList.IndexOf(unPauseDetailsDTO);
                int endingRecordIndex = index;
                for (int i = 0; i < endingRecordIndex; i++)
                {
                    if (string.IsNullOrWhiteSpace(unPauseDetailsDTOList[i].UnPauseAction) == false
                        && unPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.POSTPONE)
                    {
                        previousSchedulesMarkedForPostpone = true;
                        break;
                    }
                }
            } 
            log.LogMethodExit(previousSchedulesMarkedForPostpone);
            return previousSchedulesMarkedForPostpone;
        }
        private bool MiddleOfSchedulesMarkedForPostpone(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            bool middleOfSchedulesMarkedForPostpone = false;
            bool subsequentSchedulesMarkedForPostpone = SubsequentSchedulesMarkedForPostpone(rowIndex);
            bool previousSchedulesMarkedForPostpone = PreviousSchedulesMarkedForPostpone(rowIndex);
            middleOfSchedulesMarkedForPostpone = subsequentSchedulesMarkedForPostpone && previousSchedulesMarkedForPostpone;
            log.LogMethodExit(middleOfSchedulesMarkedForPostpone);
            return middleOfSchedulesMarkedForPostpone;
        }
        private bool TheLastSchedule(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            bool theLastSchedulesMarkedForPostpone = false; 
            if (unPauseDetailsDTOList != null && unPauseDetailsDTOList.Count -1 == rowIndex)
            {
                theLastSchedulesMarkedForPostpone = true;
            }
            log.LogMethodExit(theLastSchedulesMarkedForPostpone);
            return theLastSchedulesMarkedForPostpone;
        }
        private void SetResetCancelBillingSchedule(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = GetUnPauseDetailsDTO(rowIndex);
            if (unPauseDetailsDTO != null && unPauseDetailsDTO.UnPauseAction != SubscriptionUnPauseOptions.CANCEL)
            {
                if (dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["transactionLineIdDataGridViewTextBoxColumn"].Value != null 
                    && ((int)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["transactionLineIdDataGridViewTextBoxColumn"].Value) > -1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2989));
                    // "Sorry, this schedule is already billed"
                }
                int index = unPauseDetailsDTOList.IndexOf(unPauseDetailsDTO);
                unPauseDetailsDTOList[index].UnPauseAction = SubscriptionUnPauseOptions.CANCEL;
                unPauseDetailsDTOList[index].NewBillFromDate = null;
                dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["newBillFromDate"].Value = null;
                DataGridViewImageCell resumeBillingCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["resumeBilling"];
                resumeBillingCell.Value = Properties.Resources.ResumeBillingGrayed;
                //resumeBillingCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                DataGridViewImageCell cancelBillingScheduleCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["cancelBillingSchedule"];
                cancelBillingScheduleCell.Value = Properties.Resources.CancelSubscriptionCycleActive;
                //cancelBillingScheduleCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                DataGridViewImageCell postponeBillingScheduleCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["postponeBillingSchedule"];
                postponeBillingScheduleCell.Value = Properties.Resources.PostponeSubscriptionCycleGrayed;
                //postponeBillingScheduleCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                dgvUnPauseBillingSchedules.EndEdit();
                dgvUnPauseBillingSchedules.RefreshEdit();
            }
            log.LogMethodExit();
        }
        private void SetResetPostPoneBillingSchedule(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = GetUnPauseDetailsDTO(rowIndex);
            if (unPauseDetailsDTO != null && unPauseDetailsDTO.UnPauseAction != SubscriptionUnPauseOptions.POSTPONE)
            {
                int index = unPauseDetailsDTOList.IndexOf(unPauseDetailsDTO);

                int postponeCycle = GetPostPoneCycles();
                if (postponeCycle > -1)
                {
                    SetActionAsPostpone(rowIndex, postponeCycle);
                }
            }

            log.LogMethodExit();
        }
        private int GetPostPoneCycles()
        {
            log.LogMethodEntry();
            int postponeByXCyles = -1;
            using (GenericDataEntry gde = new GenericDataEntry(1))
            {
                gde.Text = MessageContainerList.GetMessage(executionContext, "Postpone Billing Schedules By");

                gde.DataEntryObjects[0].mandatory = true;
                gde.DataEntryObjects[0].allowMinusSign = false;
                gde.DataEntryObjects[0].label = MessageContainerList.GetMessage(executionContext, "Subscription Cycles");
                gde.DataEntryObjects[0].dataType = GenericDataEntry.DataTypes.Integer;
                gde.DataEntryObjects[0].width = 50;
                gde.DataEntryObjects[0].maxlength = 2;

                if (gde.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!string.IsNullOrEmpty(gde.DataEntryObjects[0].data))
                    {
                        postponeByXCyles = Convert.ToInt32(gde.DataEntryObjects[0].data);
                    }
                }
            }
            log.LogMethodExit(postponeByXCyles);
            return postponeByXCyles;
        }
        private void SetActionAsPostpone(int rowIndex, int postponeCycle)
        {
            log.LogMethodEntry(rowIndex, postponeCycle);
            bool startPostpone = false;
            int selectedScheduleId = Convert.ToInt32(dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["subscriptionBillingScheduleId"].Value);
            for (int i = rowIndex; i < dgvUnPauseBillingSchedules.RowCount; i++)
            {
                if (startPostpone == false
                    && selectedScheduleId == Convert.ToInt32(dgvUnPauseBillingSchedules.Rows[i].Cells["subscriptionBillingScheduleId"].Value))
                {
                    startPostpone = true;
                }
                if (startPostpone)
                {
                    SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = GetUnPauseDetailsDTO(i);
                    int index = unPauseDetailsDTOList.IndexOf(unPauseDetailsDTO);
                    unPauseDetailsDTOList[index].UnPauseAction = SubscriptionUnPauseOptions.POSTPONE;
                    unPauseDetailsDTOList[index].NewBillFromDate = GetNewBillFromDate(unPauseDetailsDTOList[index], postponeCycle);
                    dgvUnPauseBillingSchedules.Rows[i].Cells["newBillFromDate"].Value = unPauseDetailsDTOList[index].NewBillFromDate;
                    DataGridViewImageCell resumeBillingCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[i].Cells["resumeBilling"];
                    resumeBillingCell.Value = Properties.Resources.ResumeBillingGrayed;
                    //resumeBillingCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    DataGridViewImageCell cancelBillingScheduleCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[i].Cells["cancelBillingSchedule"];
                    cancelBillingScheduleCell.Value = Properties.Resources.CancelSubscriptionCycleGrayed;
                    //cancelBillingScheduleCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    DataGridViewImageCell postponeBillingScheduleCell = (DataGridViewImageCell)dgvUnPauseBillingSchedules.Rows[i].Cells["postponeBillingSchedule"];
                    postponeBillingScheduleCell.Value = Properties.Resources.PostponeSubscriptionCycleActive;
                    //postponeBillingScheduleCell.ImageLayout = DataGridViewImageCellLayout.Stretch;
                    dgvUnPauseBillingSchedules.EndEdit();
                    dgvUnPauseBillingSchedules.RefreshEdit();
                }
            }
            log.LogMethodExit();
        }
        private DateTime GetNewBillFromDate(SubscriptionUnPauseDetailsDTO subscriptionUnPauseDetailsDTO, int postponeCycle)
        {
            log.LogMethodEntry();
            string uomValue = subscriptionHeaderDTO.UnitOfSubscriptionCycle;
            DateTime newDate = (uomValue == UnitOfSubscriptionCycle.DAYS
                                  ? subscriptionUnPauseDetailsDTO.OldBillFromDate.AddDays(postponeCycle)
                                  : uomValue == UnitOfSubscriptionCycle.MONTHS
                                     ? subscriptionUnPauseDetailsDTO.OldBillFromDate.AddMonths(postponeCycle)
                                      : uomValue == UnitOfSubscriptionCycle.YEARS
                                         ? subscriptionUnPauseDetailsDTO.OldBillFromDate.AddYears(postponeCycle)
                                          : subscriptionUnPauseDetailsDTO.OldBillFromDate);
            log.LogMethodExit(newDate);
            return newDate;
        }
        private SubscriptionUnPauseDetailsDTO GetUnPauseDetailsDTO(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            int subscriptionBillingScheduleId = Convert.ToInt32(dgvUnPauseBillingSchedules.Rows[rowIndex].Cells["subscriptionBillingScheduleId"].Value);
            SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = unPauseDetailsDTOList.Find(upd => upd.SubscriptionBillingScheduleId == subscriptionBillingScheduleId);
            log.LogMethodExit(unPauseDetailsDTO);
            return unPauseDetailsDTO;
        }
        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                CanProceedWithUnpause();
                UnPauseSubscrition();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit();
        }
        private void CanProceedWithUnpause()
        {
            log.LogMethodEntry();
            if (unPauseDetailsDTOList != null && unPauseDetailsDTOList.Any())
            {
                List<ValidationError> validationErrorList = new List<ValidationError>();
                for (int i = 0; i < unPauseDetailsDTOList.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(unPauseDetailsDTOList[i].UnPauseAction))
                    {
                        validationErrorList.Add(new ValidationError("UnPause Details", "UnPauseAction",
                                                    MessageContainerList.GetMessage(executionContext, 2979, unPauseDetailsDTOList[i].OldBillFromDate.ToString(DATE_FORMAT))));
                        // "Pleae select action for &1 entry"
                    }
                }
                for (int i = 0; i < unPauseDetailsDTOList.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(unPauseDetailsDTOList[i].UnPauseAction) == false
                        && unPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.POSTPONE)
                    {
                        if (HasInvalidActionAfterPostpone(i))
                        {
                            validationErrorList.Add(new ValidationError("UnPause Details", "POSTPONE",
                                                      MessageContainerList.GetMessage(executionContext,2980, SubscriptionUnPauseOptions.BILL, SubscriptionUnPauseOptions.POSTPONE,
                                                      unPauseDetailsDTOList[i].OldBillFromDate.ToString(DATE_FORMAT))));
                            //"Cannot have &1 action for subsequent schedules once &2 action is selected for &3"
                        }
                    }
                }
                if (validationErrorList.Any())
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Validation Error"), validationErrorList);
                }
            }
            log.LogMethodExit();
        }
        private bool HasInvalidActionAfterPostpone(int rowIndex)
        {
            log.LogMethodEntry(rowIndex);
            bool hasInvalidAction = false;
            int startPoint = rowIndex + 1;
            for (int i = startPoint; i < unPauseDetailsDTOList.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(unPauseDetailsDTOList[i].UnPauseAction) == false
                    && unPauseDetailsDTOList[i].UnPauseAction == SubscriptionUnPauseOptions.BILL)
                {
                    hasInvalidAction = true;
                    break;
                }
            }
            log.LogMethodExit(hasInvalidAction);
            return hasInvalidAction;
        }
        private void UnPauseSubscrition()
        {
            log.LogMethodEntry();
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, MANAGER_APPROVAL_FOR_PAUSE_UNPAUSE_SUBSCRIPTION))
            { 
                subscriptionHeaderDTO.UnPauseApprovedBy = null; //reset any old approval info 
                string mgrLoginId = SubscriptionUIHelper.GetManagerApproval(executionContext);
                subscriptionHeaderDTO.UnPauseApprovedBy = mgrLoginId; 
            }
            SubscriptionHeaderBL subscriptionHeaderBL = new SubscriptionHeaderBL(executionContext, subscriptionHeaderDTO);
            subscriptionHeaderBL.UnPauseSubscription(utilities, this.unPauseDetailsDTOList);
            log.LogMethodExit();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                if (POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1695), MessageContainerList.GetMessage(executionContext, "Clear Actions"), MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SubscriptionHeaderBL subscriptionHeaderBL = new SubscriptionHeaderBL(executionContext, subscriptionHeaderDTO.SubscriptionHeaderId, true);
                    this.subscriptionHeaderDTO = subscriptionHeaderBL.SubscriptionHeaderDTO;
                    if (subscriptionHeaderDTO != null && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList != null
                        && subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Any())
                    {
                        List<SubscriptionBillingScheduleDTO> pausedSubscriptionBillingSchedules = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.Status == SubscriptionStatus.PAUSED).ToList();
                        dgvUnPauseBillingSchedules.DataSource = pausedSubscriptionBillingSchedules;
                        BuildSubscriptionUnPauseDetailsDTOList(pausedSubscriptionBillingSchedules);
                        SetButtonIconsOnTheGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            }
            log.LogMethodExit(); 
        }
        private void AdjustNewFromDate()
        {
            log.LogMethodEntry();
            bool startPostpone = false;
            TimeSpan? postponeCycleDiff = null;
            for (int i = 0; i < dgvUnPauseBillingSchedules.RowCount; i++)
            {
                SubscriptionUnPauseDetailsDTO unPauseDetailsDTO = GetUnPauseDetailsDTO(i);
                int index = unPauseDetailsDTOList.IndexOf(unPauseDetailsDTO);
                if (startPostpone == false && unPauseDetailsDTO.UnPauseAction != SubscriptionUnPauseOptions.POSTPONE && unPauseDetailsDTOList[index].NewBillFromDate != null)
                {
                    unPauseDetailsDTOList[index].NewBillFromDate = null;
                    dgvUnPauseBillingSchedules.Rows[i].Cells["newBillFromDate"].Value = unPauseDetailsDTOList[index].NewBillFromDate;
                }
                if (unPauseDetailsDTO.UnPauseAction == SubscriptionUnPauseOptions.POSTPONE && startPostpone == false)
                {
                    startPostpone = true;
                    postponeCycleDiff = unPauseDetailsDTO.NewBillFromDate - unPauseDetailsDTO.OldBillFromDate;
                    continue;
                }

                if (unPauseDetailsDTO.UnPauseAction != SubscriptionUnPauseOptions.POSTPONE && startPostpone && postponeCycleDiff != null)
                {
                    TimeSpan postponeCycleDiffValue = (TimeSpan)postponeCycleDiff; 
                    unPauseDetailsDTOList[index].NewBillFromDate = unPauseDetailsDTOList[index].OldBillFromDate.Add(postponeCycleDiffValue);
                    dgvUnPauseBillingSchedules.Rows[i].Cells["newBillFromDate"].Value = unPauseDetailsDTOList[index].NewBillFromDate; 
                }
            }
            dgvUnPauseBillingSchedules.EndEdit();
            dgvUnPauseBillingSchedules.RefreshEdit();
            log.LogMethodExit();
        }
    }
}
