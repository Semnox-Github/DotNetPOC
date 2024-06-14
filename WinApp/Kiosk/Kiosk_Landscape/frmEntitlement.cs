/********************************************************************************************
* Project Name - Parafait_Kiosk
* Description  - frmEntitlement.cs 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
*2.80        4-Sep-2019       Deeksha             Added logger methods.
*2.150.1     22-Feb-2023      Guru S A            Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.KioskCore;

namespace Parafait_Kiosk
{
         
    public partial class frmEntitlement : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string selectedEntitlement = "Time";
        MessageUtils MessageUtils = KioskStatic.Utilities.MessageUtils;
        public frmEntitlement( string message)
        {
            log.LogMethodEntry(message);
            InitializeComponent();

            lblmsg.Text = message;
            btnTime.Text = MessageUtils.getMessage("Play Time");
            btnPoints.Text = MessageUtils.getMessage("Play Points");
            log.LogMethodExit();
        }

        private void btnTime_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            selectedEntitlement = "Time";
            KioskStatic.logToFile("ENTITLEMENT_TYPE is Time");
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        private void btnPoints_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            selectedEntitlement = KioskTransaction.CREDITS_ENTITLEMENT;
            KioskStatic.logToFile("ENTITLEMENT_TYPE is Credits");
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }
    }
}
