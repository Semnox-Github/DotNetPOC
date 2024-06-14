/********************************************************************************************
* Project Name - Parafait_Kiosk - BaseFormKiosk
* Description  - BaseFormKiosk 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.4.0       28-Sep-2018      Guru S A           Modified for Online Transaction in Kiosk changes 
*2.80        05-Sep-2019      Deeksha            Added logger methods.
*2.150.0.0   21-Jun-2022      Vignesh Bhat       Back and Cancel button changes
*2.150.1     22-Feb-2023      Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using Semnox.Parafait.KioskCore;
using Semnox.Parafait.Languages;
using System;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class BaseFormKiosk : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseFormKiosk()
        {
            log.LogMethodEntry();
            InitializeComponent();
            this.ShowInTaskbar = false;
            log.LogMethodExit();
        }

        public virtual void btnHome_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Home button clicked");
            try
            {
                PerformHomeButtonAction();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                KioskStatic.logToFile("Error on Home button click: " + ex.Message);
                frmOKMsg.ShowUserMessage(ex.Message);
            }
            log.LogMethodExit();
        }

        protected void PerformHomeButtonAction()
        {
            log.LogMethodEntry();
            DirectCashAbortAction(kioskTransaction, kioskAttractionDTO);
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            int lowerLimit = 0;
            for (int i = Application.OpenForms.Count - 1; i > lowerLimit; i--)
            {
                Application.OpenForms[i].Visible = false;
                Application.OpenForms[i].Close();
            }
            log.LogMethodExit();
        }

        public virtual void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Back button pressed");
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        public virtual void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            KioskStatic.logToFile("Cancel button pressed");
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cursor = Cursors.Default;
            Close();
            log.LogMethodExit();
        }
        public void DisplaybtnHome(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnHome.Visible = switchValue;
            log.LogMethodExit();
        }
        public void DisplaybtnPrev(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnPrev.Visible = switchValue;
            log.LogMethodExit();
        }
        public void DisplaybtnCancel(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnCancel.Visible = switchValue;
            log.LogMethodExit();
        }

        public virtual void btnCart_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            throw new NotImplementedException(MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "btnCart_Click is not implemented"));
            log.LogMethodExit();
        }

        public void DisplaybtnCart(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnCart.Visible = switchValue;
            log.LogMethodExit();
        }

        protected void LaunchCartForm(string NUMBERFORMAT)
        {
            log.LogMethodEntry("kioskTransaction", NUMBERFORMAT);
            if (kioskTransaction != null && kioskTransaction.GetCartItemCount > 0)
            {
                using (frmKioskCart frpm = new frmKioskCart(kioskTransaction))
                {
                    DialogResult dr = frpm.ShowDialog();
                    kioskTransaction = frpm.GetKioskTransaction;
                }
                this.RefreshCartIconText(NUMBERFORMAT);
            }
            else
            {
                string errMsg = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, 
                    "Cart is empty");
                KioskStatic.logToFile("Skipping Cart form launch: "+ errMsg);
                Semnox.Core.Utilities.ValidationException validationException = new Semnox.Core.Utilities.ValidationException(errMsg);
                log.Error(validationException);
                throw validationException; 
            }
            log.LogMethodExit();
        }
        protected void RefreshCartIconText(string numberFormat)
        {
            log.LogMethodEntry("kioskTransaction", numberFormat);
            if (kioskTransaction != null)
            {
                int cartItemCount = kioskTransaction.GetCartItemCount;
                string cartIconText = MessageContainerList.GetMessage(KioskStatic.Utilities.ExecutionContext, "CART");
                this.btnCart.RefreshCartInfo(cartItemCount, cartIconText);                
            }
            log.LogMethodExit();
        }
    }
}
