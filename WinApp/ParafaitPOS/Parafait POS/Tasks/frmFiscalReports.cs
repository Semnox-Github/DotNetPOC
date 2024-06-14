using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Semnox.Parafait.Device.Printer.FiscalPrint;
//Added on 30-Sep-2015. This form is added to give fiscal printing option for eltrade bulgarian printer.
namespace Parafait_POS
{
    public partial class frmFiscalReports : Form
    {
        FiscalPrinter FiscalPrinter;
        Utilities Utilities;
        MessageUtils MessageUtils;
        DataTable dTable = new DataTable();
        //Begin: Modified Added for logger function on 08-Mar-2016
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //End: Modified Added for logger function on 08-Mar-2016

        public frmFiscalReports(Utilities _utilities, FiscalPrinter FiscalPrintr)
        {
            //Begin: Added to Configure the logger root LogLevel using App.config on 08-March-2016           
            //log = ParafaitUtils.Logger.setLogFileAppenderName(log);
            Logger.setRootLogLevel(log);
            //End: Added to Configure the logger root LogLevel using App.config on 08-March-2016

            log.Debug("Starts-frmFiscalReports(_utilities,FiscalPrintr)");//Added for logger function on 08-Mar-2016
            InitializeComponent();
            Utilities = _utilities;
            FiscalPrinter = FiscalPrintr;
            log.Debug("Ends-frmFiscalReports(_utilities,FiscalPrintr)");//Added for logger function on 08-Mar-2016
        }

        private void frmFiscalReports_Load(object sender, EventArgs e)
        {
            log.Debug("Starts-frmFiscalReports_Load()");//Added for logger function on 08-Mar-2016     
            MessageUtils = Utilities.MessageUtils;
            Button btnReport;
            int formHeight = this.Height;
            int buttonHeight = btnSample.Height;
            int Bottom = btnSample.Top;
            //Fetching the Report values from lookup
            dTable = Utilities.executeDataTable("select LookupValue,Description from LookupValues where LookupId=(" +
                                                           "select top 1 LookupId from Lookups where LookupName='ELTRADE_FISCAL_PRINT')");
            if (dTable != null)
            {
                for (int i = 0; i < dTable.Rows.Count; i++)
                {
                    //Adding the buttons
                    btnReport = new Button();
                    btnReport.Font = btnSample.Font;
                    btnReport.Height = btnSample.Height;
                    btnReport.Width = btnSample.Width;
                    btnReport.BackColor = btnSample.BackColor;
                    btnReport.BackgroundImage = btnSample.BackgroundImage;
                    btnReport.ForeColor = btnSample.ForeColor;
                    btnReport.Click += new EventHandler(btnSample_Click);
                    btnReport.Location = new Point(btnSample.Left, Bottom);
                    btnReport.Text = dTable.Rows[i]["Description"].ToString();
                    btnReport.Name = dTable.Rows[i]["LookupValue"].ToString();
                    formHeight = formHeight + buttonHeight;
                    btnReport.Top = Bottom;                    
                    this.Controls.Add(btnReport);
                    Bottom = btnReport.Bottom + 8;
                }
                this.Height = formHeight;
            }
            else
            {
                POSUtils.ParafaitMessageBox("Please add lookup values for ELTRADE_FISCAL_PRINT.");
                log.Info("frmFiscalReports_Load() - add lookup values for ELTRADE_FISCAL_PRINT");//Added for logger function on 08-Mar-2016     
                Close();
            }
            log.Debug("Ends-frmFiscalReports_Load()");//Added for logger function on 08-Mar-2016     
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
			log.Debug("Starts-btnSample_Click()");//Added for logger function on 08-Mar-2016     
			//on button click the utils PrintReport() calling
             Button b = (Button)sender;
            string Message = "";

            try
            {
                if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y" &&
                        ((Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                        || (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BIXOLON_SRP_812.ToString()))
                        || (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.ELTRADE.ToString())))
                        && b.Name == "RunMonthlyReport")
                {
                    using (frmMonthlyReport frmMonthlyReport = new frmMonthlyReport(Utilities,FiscalPrinter))
                    {
                        frmMonthlyReport.ShowDialog();
                    }
                }
                else if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y" &&
                        ((Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BIXOLON_SRP_812.ToString()))) &&
                        b.Name == "ChangeTax")
                {
                    
                    using (FrmTaxValues frmTaxValues = new FrmTaxValues(Utilities, FiscalPrinter))
                    {
                        DialogResult dr = frmTaxValues.ShowDialog();
                    }
                }
                else
                {
                    FiscalPrinter.PrintReport(b.Name, ref Message);
                    if (!string.IsNullOrEmpty(Message))
                    {
                        log.Error("Print report error : " + Message);
                    }
                }
            }
            catch(Exception ex)
            {
                POSUtils.ParafaitMessageBox(ex.Message);
            }
            
            
            log.Debug("Ends-btnSample_Click()");//Added for logger function on 08-Mar-2016     
        }
    }
}
