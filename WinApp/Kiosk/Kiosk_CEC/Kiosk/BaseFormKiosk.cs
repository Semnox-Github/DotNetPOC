/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - BaseFormKiosk.cs
 * 
 **************
 **Version Log
 ************** 
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/
using System;

namespace Parafait_Kiosk
{
    public partial class BaseFormKiosk : BaseForm
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public BaseFormKiosk()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        public virtual void btnPrev_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.No;
            Close();
            log.LogMethodExit();
        }

        public virtual void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
            log.LogMethodExit();
        }

        public void displaybtnPrev(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnPrev.Visible= switchValue;
            log.LogMethodExit();
        }

        public void displaybtnCancel(bool switchValue)
        {
            log.LogMethodEntry(switchValue);
            this.btnCancel.Visible = switchValue;
            log.LogMethodExit();
        }

    }
}
