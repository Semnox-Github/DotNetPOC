/********************************************************************************************
 * Project Name - Redemption Kiosk
 * Description  - Redemption kiosk age gate form
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.4.0       10-Sep-2018      Archana            Created 
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
using Semnox.Parafait.Languages;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Customer;

namespace Redemption_Kiosk
{
    public partial class FrmRedemptionKioskAgeGate : Semnox.Parafait.Customer.FrmAgeGate
    {
        static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ScreenModel screenModel;
        ExecutionContext machineUserContext;
        Utilities utilities;
        string cardNumber;
        RedemptionBL redemptionOrder;

        public delegate void AgeGateStartOver();
        public AgeGateStartOver ageGateStartOver;

        public delegate void AgeGateUpdateCustomer(int customerId);
        public AgeGateUpdateCustomer ageGateUpdateCustomer;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="utils">Utilities</param>
        /// <param name="redemptionOrder">RedemptionBL object</param>
        /// <param name="cardNumber">string</param>
        public FrmRedemptionKioskAgeGate(ExecutionContext executionContext, Utilities utils, RedemptionBL redemptionBl, string cardNumber = "") : base(executionContext, utils, cardNumber)
        {
            log.LogMethodEntry(executionContext, utils, redemptionOrder, cardNumber);
            InitializeComponent();
            machineUserContext = executionContext;
            utilities = utils;
            this.cardNumber = cardNumber;
            redemptionOrder = redemptionBl;
            RenderScreen();
            log.LogMethodExit();
        }
        
        protected override void btnNext_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            if (keypad != null && keypad.IsDisposed == false)
                keypad.Hide();

            if (!string.IsNullOrEmpty(txtDate1.Text.Trim())
              && !string.IsNullOrEmpty(txtDate2.Text.Trim())
              && !string.IsNullOrEmpty(txtDate3.Text.Trim()))
            {
                try
                {
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    BirthDate = DateTime.ParseExact(txtDate1.Text + txtDate2.Text + txtDate3.Text, ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "DATE_FORMAT").Replace("/", "").Replace("-", ""), provider);//ParafaitEnv.DATE_FORMAT.Replace("/", "").Replace("-", "")
                }
                catch
                {
                    try
                    {
                        System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.CurrentCulture;
                        BirthDate = Convert.ToDateTime(txtDate1.Text + "-" + txtDate2.Text + "-" + txtDate3.Text, provider);
                    }
                    catch
                    {
                        Common.ShowMessage(MessageContainerList.GetMessage(machineUserContext, 10));
                        return;
                    }
                }

                //try
                //{
                //    string[] dates = BirthDate.ToString(ParafaitDefaultContainer.GetParafaitDefault(machineUserContext, "DATE_FORMAT")).Split('/', '-');
                //    txtDate1.Text = dates[0];
                //    txtDate2.Text = dates[1];
                //    txtDate3.Text = dates[2];
                //}
                //catch { }

                string ageLimit = ParafaitDefaultContainerList.GetParafaitDefault(machineUserContext, "REGISTRATION_AGE_LIMIT").Trim();//Utilities.getParafaitDefaults("REGISTRATION_AGE_LIMIT").Trim();
                if (!string.IsNullOrEmpty(ageLimit))
                {
                    if (ServerDateTime.Now < BirthDate.AddYears(Convert.ToInt32(ageLimit)))
                    {
                        Common.ShowMessage(MessageContainerList.GetMessage(machineUserContext, 805, ageLimit));
                        return;
                    }
                }

                if (keypad != null && keypad.IsDisposed == false)
                    keypad.Close();

                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                Close();
                using (FrmRedemptionKioskCustomer frmRedemptionKioskCustomer = new FrmRedemptionKioskCustomer(machineUserContext, utilities, redemptionOrder, cardNumber, BirthDate))
                {
                    frmRedemptionKioskCustomer.startOver += new FrmRedemptionKioskCustomer.StartOver(StartOver);
                    frmRedemptionKioskCustomer.updateCustomer += new FrmRedemptionKioskCustomer.UpdateCustomer(ageGateUpdateCustomer);
                    frmRedemptionKioskCustomer.BringToFront();
                    frmRedemptionKioskCustomer.ShowDialog();
                }
                
            }
            else
            {
                Common.ShowMessage(MessageContainerList.GetMessage(machineUserContext, 10));
                this.ActiveControl = txtDate1;
            }
            log.LogMethodExit();
        }

        private void StartOver()
        {
            log.LogMethodEntry();
            ageGateStartOver();
            log.LogMethodExit();
        }
        private void RenderScreen()
        {
            log.LogMethodEntry();
            int screenId = Common.GetScreenId("frmRedemptionKioskAgeGate");
            screenModel = new ScreenModel(Convert.ToInt32(screenId));
            Common.RenderPanelContent(screenModel, this, 1);
            Common.RenderPanelContent(screenModel, this.pnlBottom, 2);
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
        

        private void FrmRedemptionKioskAgeGate_Activated(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StartKioskTimer();
            ResetKioskTimer();
            log.LogMethodExit();
        }

        private void FrmRedemptionKioskAgeGate_Deactivate(object sender, FormClosingEventArgs e)
        {
            log.LogMethodEntry(sender, e);
            StopKioskTimer();
            log.LogMethodExit();
        }
    }
}