/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Redemption kiosk customer screen
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       12-Sep-2018      Archana            Created 
 ********************************************************************************************/
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
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;

namespace Redemption_Kiosk
{
    public partial class FrmRedemptionKioskCustomer : Semnox.Parafait.Customer.FrmKioskCustomer
    {
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void StartOver();
        public StartOver startOver;

        public delegate void UpdateCustomer(int customerId);
        public UpdateCustomer updateCustomer;

        private ScreenModel screenModel;
        RedemptionBL redemptionOrder;
        ExecutionContext machineUserContext;
        

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="utils">utilities</param>
        /// <param name="redempionOrder">Redemption Order</param>
        /// <param name="cardNumber">Card number</param>
        /// <param name="birthDate">Birthdate</param>
        public FrmRedemptionKioskCustomer(ExecutionContext executionContext, Utilities utils, RedemptionBL redempionOrder, string cardNumber = null, object birthDate = null) : base(executionContext, utils, cardNumber, birthDate)
        {
            log.LogMethodEntry(executionContext, utils, redempionOrder, cardNumber, birthDate);
            InitializeComponent();
            machineUserContext = executionContext;
            redemptionOrder = redempionOrder;
            RenderScreen();
            log.LogMethodExit();
        }
        private void CustomUpdateHeader()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        private bool CustomValidateAction(ScreenModel.UIPanelElement element)
        {
            log.LogMethodEntry(element);
            log.LogMethodExit(true);
            return true;
        }
        protected override void btnStartOver_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            if (keypad != null)
                keypad.Hide();
            //this.DialogResult = System.Windows.Forms.DialogResult.No;
            //Close();
            startOver();

            log.LogMethodExit();
        }

        protected override void btnBack_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            ResetKioskTimer();
            if (!String.IsNullOrEmpty(txtFirstName.Text) && !String.IsNullOrEmpty(txtContactPhone1.Text) && !String.IsNullOrEmpty(txtEmail.Text))
            {
                if (Common.ShowDialog(MessageContainerList.GetMessage(machineUserContext, 1661)) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (keypad != null)
                        keypad.Hide();
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                    Close();
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (keypad != null)
                    keypad.Hide();
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                Close();
            }
            log.LogMethodExit();
        }
        protected override void btnSave_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            ResetKioskTimer();
            if (keypad != null)
                keypad.Hide();

            try
            {
                base.btnSave_Click(sender, e);
                if (customerDTO != null && customerDTO.Id > -1)
                {
                    updateCustomer(customerDTO.Id);
                    if (keypad != null)
                        keypad.Hide();
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                    Close();
                }
            }
            catch (Exception ex)
            {
                DisplayMessageLine(MessageContainerList.GetMessage(machineUserContext, 255, ex.Message));
            }

            log.LogMethodExit();
        }
        private void RenderScreen()
        {
            log.LogMethodEntry();
            int screenId = Common.GetScreenId("frmRedemptionKioskCustomer");
            screenModel = new ScreenModel(Convert.ToInt32(screenId));
            Common.RenderPanelContent(screenModel, this, 1);
            Common.RenderPanelContent(screenModel, this.pnlBottom, 2);
            Common.CustomUpdateHeader updateHeader = new Common.CustomUpdateHeader(CustomUpdateHeader);
            Common.CustomValidateAction customValidateAction = new Common.CustomValidateAction(CustomValidateAction);
            log.LogMethodExit();
        }

        protected override void KioskTimer_Tick(object sender, EventArgs e)
        {
            int tickSecondsRemaining = GetKioskTimerSecondsValue();

            tickSecondsRemaining--;

            SetKioskTimerSecondsValue(tickSecondsRemaining);

            if (tickSecondsRemaining == 10)
            {
                if (TimeOut.AbortTimeOut(this))
                    ResetKioskTimer();
                else
                {
                    if (keypad != null)
                        keypad.Hide();
                    this.Close();

                }
            } 
        }

        protected override CreateParams CreateParams
        {
            //this method is used to avoid the table layout flickering.
            get
            {
                CreateParams CP = base.CreateParams;
                CP.ExStyle = CP.ExStyle | 0x02000000;
                return CP;
            }
        }

        private void FrmRedemptionKioskCustomer_FormClosing(object sender, FormClosingEventArgs e)
        {

            log.LogMethodEntry(sender, e);
            if (keypad != null)
            {
                keypad.Hide();
                keypad.Dispose();
            }
            log.LogMethodExit();
        }
    }
}
