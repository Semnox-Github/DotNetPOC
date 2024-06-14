/********************************************************************************************
 * Class Name - POS                                                                         
 * Description - EOD UI
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By        Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019          Deeksha          Added logger methods.
 *2.120         26-Apr-2021          Laster Menezes   Modified PerformEndOfDay method to use receipt framework of reports to generate POSZ report on one click
 *2.130         14-Jul-2021          Laster Menezes   Modified PerformEndOfDay method to support customized POSZ TRDX template along with default template
 *2.140.5       28-Apr-2023          Rakshith Shetty  Added method call to PrintReceiptReport which is used to print the report source instead of generating the pdf and then printing.   
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Reports;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Semnox.Parafait.POS
{
    public partial class frmEodUI : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Utilities utilities;
        public frmEodUI(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            log.LogMethodExit();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry();
            this.DialogResult = DialogResult.OK;
            this.Close();
            log.LogMethodExit();
        }

        public void PerformEndOfDay(int posMachineId)
        {
            log.LogMethodEntry();
            try
            {
                ReportsList reportsList = new ReportsList(utilities.ExecutionContext);
                List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>> reportsSearchParams = new List<KeyValuePair<ReportsDTO.SearchByReportsParameters, string>>();
                POSMachineDTO posMachineDTO = new POSMachineDTO();
                POSMachineList posMachineList = new POSMachineList(utilities.ExecutionContext);
                LookupValuesList lookupValuesList = new LookupValuesList(utilities.ExecutionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                string urlData = "", displayData = "";
                string[] data;
                DateTime eodTime = utilities.getServerTime();
                DateTime nextBusinessDay = DateTime.Today.AddHours(Convert.ToInt32(utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME")));
                if (nextBusinessDay.CompareTo(eodTime) <= 0)
                {
                    nextBusinessDay = nextBusinessDay.AddDays(1);
                }
                if (MessageBox.Show(utilities.MessageUtils.getMessage(1351) + nextBusinessDay.ToString(utilities.ParafaitEnv.DATETIME_FORMAT), "End of Day Process", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    // This code is added to print the Z- report if Fiscal printer is connected and End of theDay flag is enabled
                    if (utilities.getParafaitDefaults("USE_FISCAL_PRINTER") == "Y" &&
                       ((utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString())) ||
                       (utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.ELTRADE.ToString()))))
                    {
                        string _FISCAL_PRINTER = utilities.getParafaitDefaults("FISCAL_PRINTER");
                        FiscalPrinterFactory.GetInstance().Initialize(utilities);
                        FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER);
                        if (!fiscalPrinter.OpenPort())
                        {
                            MessageBox.Show(MessageContainerList.GetMessage(utilities.ExecutionContext, 193, utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER")), "Fiscal Printer");
                            log.Error("Unable to initialize Fiscal Printer (Port: " + utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER") + " )) ");
                            fiscalPrinter.ClosePort();
                        }
                        string message = string.Empty;
                        fiscalPrinter.PrintReport("RunZReport", ref message);
                        if (!string.IsNullOrEmpty(message))
                        {
                            log.Error("Print report error : " + message);
                        }
                        posMachineDTO = posMachineList.GetPOSMachine(posMachineId);
                        if (posMachineDTO != null)
                        {
                            posMachineDTO.DayEndTime = eodTime;
                            POSMachines posMachine = new POSMachines(utilities.ExecutionContext, posMachineDTO);
                            posMachine.Save();
                        }
                    }
                    else
                    {
                        posMachineDTO = posMachineList.GetPOSMachine(posMachineId);

                        if (posMachineDTO != null)
                        {
                            posMachineDTO.DayEndTime = eodTime;
                            try
                            {
                                //Get the receipt report POSZReceipt or POSZReceiptCustom                           
                                string poszReportKey = "POSZReceipt";
                                List<ReportsDTO> reportsDTOList = reportsList.GetReportDTOWithCustomKey(poszReportKey);
                                if (reportsDTOList != null && reportsDTOList.Count > 0)
                                {
                                    try
                                    {
                                        List<clsReportParameters.SelectedParameterValue> reportParam = new List<clsReportParameters.SelectedParameterValue>();
                                        reportParam.Add(new clsReportParameters.SelectedParameterValue("posname", posMachineDTO.POSName));
                                        reportParam.Add(new clsReportParameters.SelectedParameterValue("userid", utilities.ParafaitEnv.User_Id));
                                        ReceiptReports receiptReports = new ReceiptReports(utilities.ExecutionContext, "POSZReceipt", "", posMachineDTO.DayBeginTime, posMachineDTO.DayEndTime, reportParam, "P");
                                        receiptReports.PrintReceiptReport();
                                        SqlCommand cmd = utilities.getCommand();
                                        cmd.CommandText = @"insert into PosMachinereportLog(POSMachineName,reportid,startTime,endTime,site_id, IsActive, CreatedBy, CreationDate, LastUpdatedBy, LastupdatedDate)
                                                Values(@posMachineName, @reportid, @startTime, @endTime, @siteid, 1,'" + utilities.ParafaitEnv.LoginID + "', getdate(),'" + utilities.ParafaitEnv.LoginID + "',getdate()) ";
                                        cmd.Parameters.AddWithValue("@reportid", reportsDTOList[0].ReportId);
                                        cmd.Parameters.AddWithValue("@startTime", posMachineDTO.DayBeginTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        cmd.Parameters.AddWithValue("@endTime", posMachineDTO.DayEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                                        if (utilities.ParafaitEnv.IsCorporate)
                                        {
                                            cmd.Parameters.AddWithValue("@siteid", utilities.ParafaitEnv.SiteId);
                                        }
                                        else
                                        {
                                            cmd.Parameters.AddWithValue("@siteid", DBNull.Value);
                                        }
                                        cmd.Parameters.AddWithValue("@posMachineName", posMachineDTO.POSName);
                                        cmd.ExecuteScalar();

                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(utilities.MessageUtils.getMessage("Report does not exists in customer !!!."));
                                    log.LogMethodExit();
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }
                        }
                    }
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(utilities.MessageUtils.getMessage(1350) + ex.Message, "Error", MessageBoxButtons.OK);
                log.Fatal("Ends- btnPrint_Click() Event " + ex.ToString());
            }
        }
    }
}
