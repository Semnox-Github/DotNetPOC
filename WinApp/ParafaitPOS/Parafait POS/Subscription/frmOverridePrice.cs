/********************************************************************************************
 * Project Name - frmOverridePrice 
 * Description  - form class for OverridePrice 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.120.0     26-Feb-2021    Guru S A             Created for Subscription phase 2 changes                                                                               
 ********************************************************************************************/
using Semnox.Core.Utilities; 
using Semnox.Parafait.Languages;
using Semnox.Parafait.Transaction; 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Parafait_POS.Subscription
{
    public partial class frmOverridePrice : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private Utilities utilities;
        private SubscriptionBillingScheduleDTO selectedSchedule;
        private string NUMBER_FORMAT;
        private string AMOUNT_FORMAT;
        private string DATETIME_FORMAT;
        private string DATE_FORMAT;
        private const string MANAGER_APPROVAL_TO_OVERRIDE_SUBSCRIPTION_PRICE = "MANAGER_APPROVAL_TO_OVERRIDE_SUBSCRIPTION_PRICE";
        private VirtualKeyboardController virtualKeyboard;
        public frmOverridePrice(Utilities utilities, SubscriptionBillingScheduleDTO selectedSchedule)
        {
            log.LogMethodEntry(selectedSchedule);
            this.utilities = utilities;
            this.executionContext = utilities.ExecutionContext;
            this.selectedSchedule = selectedSchedule;
            InitializeComponent();
            SetFormats();
            LoadUIElements();
            virtualKeyboard = new VirtualKeyboardController();
            virtualKeyboard.Initialize(this, new List<Control>() { btnShowKeyPad}, ParafaitDefaultContainerList.GetParafaitDefault<bool>(utilities.ExecutionContext, "AUTO_POPUP_ONSCREEN_KEYBOARD"), new List<Control>() { txtOverrideAmount });

            log.LogMethodExit();
        }
        private void SetFormats()
        {
            log.LogMethodEntry();
            NUMBER_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NUMBER_FORMAT");
            AMOUNT_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "AMOUNT_FORMAT");
            DATETIME_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
            DATE_FORMAT = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT");
            log.LogMethodExit();
        }
        private void LoadUIElements()
        {
            log.LogMethodEntry();
            txtBillFromDate.Text = selectedSchedule.BillFromDate.ToString(DATE_FORMAT);
            txtBillToDate.Text = selectedSchedule.BillToDate.ToString(DATE_FORMAT);
            txtBillAmount.Text = selectedSchedule.BillAmount.ToString(AMOUNT_FORMAT);
            if (selectedSchedule.OverridedBillAmount != null)
            {
                try
                {
                    txtOverrideAmount.Text = ((decimal)selectedSchedule.OverridedBillAmount).ToString(AMOUNT_FORMAT);
                }
                catch { }
            }
            else
            {
                txtOverrideAmount.Clear();
            }
            log.LogMethodExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
            log.LogMethodExit();
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                ValidateData();
                OverridePrice();
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

        private void ValidateData()
        {
            log.LogMethodEntry();
            decimal outValue;
            if (string.IsNullOrWhiteSpace(txtOverrideAmount.Text) || decimal.TryParse(txtOverrideAmount.Text, out outValue) == false)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Override Bill Amount")));
                //Please enter valid value for &1
            }
            if (outValue < 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Override Bill Amount")));
                //Please enter valid value for &1
            }
            if (string.IsNullOrWhiteSpace(txtOverrideReason.Text))
            { 
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Override Reason")));
                //Please enter &1
            }
            selectedSchedule.OverrideApprovedBy = String.Empty;
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, MANAGER_APPROVAL_TO_OVERRIDE_SUBSCRIPTION_PRICE))
            {
                string managerLoginId = SubscriptionUIHelper.GetManagerApproval(executionContext);
                if (string.IsNullOrWhiteSpace(managerLoginId))
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Manager Approval Required"));
                }
                selectedSchedule.OverrideApprovedBy = managerLoginId;
            }
            log.LogMethodExit();
        }
         
        private void OverridePrice()
        {
            log.LogMethodEntry();
            selectedSchedule.OverridedBillAmount = Convert.ToDecimal(txtOverrideAmount.Text, CultureInfo.InvariantCulture);
            selectedSchedule.OverrideReason = txtOverrideReason.Text;
            SubscriptionBillingScheduleBL subscriptionBillingScheduleBL = new SubscriptionBillingScheduleBL(executionContext, selectedSchedule);
            subscriptionBillingScheduleBL.OverrideBillCyclePrice(null);
            log.LogMethodExit();
        }
        private void blueButton_MouseDown(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.pressed2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }

        private void blueButton_MouseUp(object sender, MouseEventArgs e)
        {
            log.LogMethodEntry();
            try
            {
                Button senderBtn = (Button)sender;
                senderBtn.BackgroundImage = global::Parafait_POS.Properties.Resources.normal2;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
        }
        private void txtOverrideAmount_Enter(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            int? overRideAmount = null;
            if (string.IsNullOrWhiteSpace(txtOverrideAmount.Text) == false)
            {
                int overRideAmountValue = 0;
                if (int.TryParse(txtOverrideAmount.Text, out overRideAmountValue))
                {
                    overRideAmount = overRideAmountValue;
                }
            }
            try
            {
                int updatedQty = (int)NumberPadForm.ShowNumberPadForm(MessageContainerList.GetMessage(executionContext, 2116), txtOverrideAmount.Text, utilities);
                if (updatedQty >= 0 )
                {
                    txtOverrideAmount.Text = (updatedQty == 0? "0": updatedQty.ToString(utilities.ParafaitEnv.NUMBER_FORMAT)); 
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                txtOverrideAmount.Text = (overRideAmount == null? string.Empty
                                           : (overRideAmount == 0? "0" : ((int)overRideAmount).ToString(utilities.ParafaitEnv.NUMBER_FORMAT)));
                POSUtils.ParafaitMessageBox(MessageContainerList.GetMessage(executionContext, 1824, ex.Message));
            } 
            log.LogMethodExit();
        }
    }
}
