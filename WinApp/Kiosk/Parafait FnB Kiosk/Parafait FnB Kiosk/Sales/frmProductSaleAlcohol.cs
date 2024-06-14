/********************************************************************************************
* Project Name - Parafait_Kiosk -frmProductSaleAlcohol.cs
* Description  - frmProductSaleAlcohol 
* 
**************
**Version Log
**************
*Version     Date               Modified By        Remarks          
*********************************************************************************************
 * 2.80        09-Sep-2019      Deeksha            Added logger methods.
********************************************************************************************/

namespace Parafait_FnB_Kiosk
{
    public partial class frmProductSaleAlcohol : frmProductSaleDrinks
    {
        public frmProductSaleAlcohol()
        {
            log.LogMethodEntry();
            InitializeComponent();
            log.LogMethodExit();
        }

        public override bool ValidateItemAddition()
        {
            log.LogMethodEntry();
            if ((new frmAgeGate()).ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                log.LogMethodExit();
                return true;
            }
            else
            {
                log.LogMethodExit();
                return false;
            }
        }
    }
}
