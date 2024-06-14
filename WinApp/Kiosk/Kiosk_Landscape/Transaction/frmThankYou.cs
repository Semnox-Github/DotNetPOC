/********************************************************************************************
* Project Name - Parafait_Kiosk 
* Description  - frmThankYou 
* 
**************
**Version Log
**************
*Version     Date             Modified By        Remarks          
*********************************************************************************************
* 2.80        5-Sep-2019       Deeksha            Added logger methods.
*2.150.1     22-Feb-2023       Guru S A           Kiosk Cart Enhancements
 ********************************************************************************************/
using System;
using Semnox.Parafait.KioskCore;
using System.Windows.Forms;

namespace Parafait_Kiosk
{
    public partial class frmThankYou : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmThankYou(bool receiptPrinted)
        {
            log.LogMethodEntry(receiptPrinted);
            InitializeComponent();
            this.Size = this.BackgroundImage.Size;
            button1.Text = KioskStatic.Utilities.MessageUtils.getMessage(499);

            if (receiptPrinted)
                lblMessage.Text = KioskStatic.Utilities.MessageUtils.getMessage(498);
            else
                lblMessage.Text = "";

            KioskStatic.setDefaultFont(this);//Starts:Modification on 17-Dec-2015 for introducing new theme
            this.BackgroundImage = ThemeManager.CurrentThemeImages.TapCardBox;
            btnPrev.BackgroundImage = ThemeManager.CurrentThemeImages.CloseButton;//Ends:Modification on 17-Dec-2015 for introducing new theme
            KioskTimerInterval(5000);
            log.LogMethodExit();
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            log.LogMethodExit();
        }

     
        public override void KioskTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            Close();
            Dispose();
            log.LogMethodExit();
        }

        private void btnPrev_MouseUp(object sender, MouseEventArgs e)
        {
            // btnPrev.BackgroundImage = Properties.Resources.cancel_btn;//Modification on 17-Dec-2015 for introducing new theme
        }

        private void btnPrev_MouseDown(object sender, MouseEventArgs e)
        {
            //btnPrev.BackgroundImage = Properties.Resources.cancel_btn_pressed;//Modification on 17-Dec-2015 for introducing new theme
        }
    }
}
