/********************************************************************************************
 * Project Name - Parafait POS
 * Description  - Prints Mothly report between the intervals given
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ********************************************************************************************
*2.6.3       18-Jun-2019      Indhu K        Ability to print monthly report from the printer
*2.80.0      22-Oct-2019      Girish kundar  Modified : Fiscal printer enhancement code merge to 2.80
********************************************************************************************/
using System;
using System.Windows.Forms;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer.FiscalPrint;

namespace Parafait_POS
{
    public partial class frmMonthlyReport : Form
    {
        internal DateTime fromDate = DateTime.Now;
        internal DateTime toDate = DateTime.Now;
        FiscalPrinter FiscalPrinter;
        Utilities Utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public frmMonthlyReport(Utilities Utilities, FiscalPrinter FiscalPrinter)
        {
            log.LogMethodEntry(Utilities);
            InitializeComponent();
            this.FiscalPrinter = FiscalPrinter;
            this.Utilities = Utilities;
            if (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
            {
                radioButtonReceipt.Visible = radioButtonZReport.Visible = true;
            }
            log.LogMethodExit();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            string message = string.Empty;
            fromDate = dtpFrom.Value;
            toDate = dtpTo.Value;
            FiscalPrinter.PrintMonthlyReport(fromDate, toDate, radioButtonZReport.Checked ? 'Z' : 'U', ref message);
            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }
    }
}
