using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    /// <summary>
    /// Incotex Fiscal printer functions are handled in this class.
    /// </summary>
    public class Incotex : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        new Utilities utilities;
        string outputPath = "";
        string inputPath = "";
        string errorPath = "";
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// contructor
        /// </summary>
        /// <param name="_Utilities"> _Utilities object with environment</param>
        public Incotex(Utilities _Utilities) : base(_Utilities)
        {
            log.LogMethodEntry(_Utilities);

            try
            {
                log.Debug("Starts- Incotex(_Utilities,Message) constructor");
                utilities = _Utilities;
                inputPath = utilities.getParafaitDefaults("INCOTEX_INPUT_FOLDER");
                outputPath = utilities.getParafaitDefaults("INCOTEX_OUTPUT_FOLDER");
                errorPath = utilities.getParafaitDefaults("INCOTEX_ERROR_FOLDER");
                log.Debug("Ends- Incotex(_Utilities,Message) constructor");
            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing initializations in Incortex", ex);
                log.Fatal("Ends- Incotex(_Utilities,Message) constructor. With exception: " + ex.Message);
            }

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Prints the receipt
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="Message"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="tenderedCash"></param>
        /// <param name="isFiscal"></param>
        /// <param name="trxReprint"></param>
        /// <returns></returns>
        public override bool PrintReceipt(int TrxId, ref string Message, SqlTransaction SQLTrx = null, decimal tenderedCash = 0, bool isFiscal = true, bool trxReprint = false)
        {
            log.LogMethodEntry(TrxId, SQLTrx, trxReprint, Message);

            try
            {
                log.Debug("Starts- PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method");
                string[] Command;
                DataTable dTable = new DataTable();
                SqlCommand cmdgetTrxDetails = utilities.getCommand(SQLTrx);
                cmdgetTrxDetails.CommandText = "select t.product_id,convert(Numeric(12,2),t.price) as price, t.tax_Id, t.quantity,t.LineId, p.product_name,isnull(convert(numeric(12,2),td.DiscountAmount)*-1,0) as discount from trx_lines t left join TrxDiscounts td on t.TrxId=td.TrxId and t.LineId=td.LineId inner join products p on t.product_id=p.product_id where t.trxid=@trxid order by LineId";
                cmdgetTrxDetails.Parameters.AddWithValue("@trxid", TrxId);
                SqlDataAdapter da = new SqlDataAdapter(cmdgetTrxDetails);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    log.Debug("PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method. No of lines found:" + dt.Rows.Count);
                    Command = new string[dt.Rows.Count];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Command[i] = "R_TRP " + ((dt.Rows[i]["product_name"] == DBNull.Value) ? "" : "\"" + dt.Rows[i]["product_name"].ToString() + "\"") + dt.Rows[i]["quantity"].ToString() + "*" + dt.Rows[i]["price"].ToString() + ((dt.Rows[i]["tax_Id"]==DBNull.Value)?"V1":"V2");
                        log.Debug("PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method. Generated command:" + Command[i]);
                    }
                    if (Command.Length > 0)
                    {
                        System.IO.File.WriteAllLines(inputPath + "\\Parafait" + TrxId + ".txt", Command);
                        log.Info("PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method.File:" + inputPath + "\\Parafait" + TrxId + ".txt is created.");
                    }
                    else
                    {
                        log.Debug("PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method. No rows printed.");
                        Message = "No rows printed";

                        log.LogMethodExit(false);
                        return false;
                    }
                }
                else
                {
                    log.Debug("PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method. No rows to print.");
                    Message = "No rows to print";

                    log.LogMethodExit(false);
                    return false;
                }
                foreach (string filename in System.IO.Directory.GetFiles(outputPath))
                {
                    if (filename.Contains("Parafait" + TrxId))
                    {
                        foreach (string errFilename in System.IO.Directory.GetFiles(errorPath))
                        {
                            if (filename.Contains("Parafait" + TrxId))
                            {
                                Message = "Operation errored out.";
                                log.Debug("PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method. Operation errored out.");

                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                        Message = "Print Succussful.";
                        log.Debug("PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method. Print Succussful.");

                        log.LogMethodExit(true);
                        return true;
                    }
                }

                Message = "No response from the printer.";
                log.Debug("PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method. No response from the printer.");

                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing", ex);
                Message = ex.Message;
                log.Fatal("Ends- PrintReceipt(" + TrxId + ",SQLTrx," + Message + ") method. With exception: " + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// Reprints the last transaction
        /// </summary>
        /// <param name="Message"> reference Message</param>
        /// <returns> status of the command</returns>
        public bool ReprintLastTransaction(ref string Message)
        {
            log.LogMethodEntry(Message);

            try
            {
                log.Debug("Starts- Reprint(" + Message + ") method");
                string[] Command = new string[1];
                string file = "\\ParafaitReprint" + DateTime.Today.ToString("HHmmss");
                Command[0] = "C_CPY";
                if (Command.Length > 0)
                {
                    System.IO.File.WriteAllLines(inputPath + file + ".txt", Command);
                    log.Info("Reprint(" + Message + ") method.File:" + inputPath + file + ".txt is created.");
                }
                else
                {
                    log.Debug("Reprint(" + Message + ") method. No rows printed.");
                    Message = "No rows printed";

                    log.LogMethodExit(false);
                    return false;
                }

                foreach (string filename in System.IO.Directory.GetFiles(outputPath))
                {
                    if (filename.Contains(file))
                    {
                        foreach (string errFilename in System.IO.Directory.GetFiles(errorPath))
                        {
                            if (filename.Contains(file))
                            {
                                Message = "Operation errored out.";
                                log.Debug("Reprint(" + Message + ") method. Operation errored out.");

                                log.LogMethodExit(false);
                                return false;
                            }
                        }
                        Message = "Reprint Succussful.";
                        log.Debug("Reprint(" + Message + ") method. Reprint Succussful.");

                        log.LogMethodExit(true);
                        return true;
                    }
                }
                Message = "No response from the printer.";
                log.Debug("Reprint(" + Message + ") method. No response from the printer.");

                log.LogMethodExit(false);
                return false;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing", ex);
                Message = "Printing process is incomplete due to the error: " + ex.Message;
                log.Fatal("Ends- Reprint(" + Message + ")  method with exception: " + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }
        /// <summary>
        /// Generates the Report
        /// </summary>
        /// <param name="Report">should be X_RPT(RunXReport)->X report,Z_RPT(RunZReport)->Z report and P_RPT(RunPLUReport)-> P report </param>
        /// <param name="Message">  ref Message</param>
        /// <returns>status of the command</returns>
        public override void PrintReport(string Report, ref string Message)
        {
            log.LogMethodEntry(Report,Message);

            try
            {
                log.Debug("Starts- GetReport(" + Message + ") method");
                string[] Command = new string[1];
                string file = "";
                switch (Report)
                {
                    case "RunXReport": Command[0] = "C_DYX";
                        file = "\\ParafaitXRPT" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                        break;
                    case "RunZReport": Command[0] = "C_DYZ";
                        file = "\\ParafaitZRPT" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                        break;
                    case "RunPLUReport": Command[0] = "C_PTRP";
                        file = "\\ParafaitPRPT" + DateTime.Now.ToString("ddMMyyyyHHmmss");
                        break;
                    default: Message = "Invalid report";
                        break;
                }

                if (Command.Length > 0)
                {
                    System.IO.File.WriteAllLines(inputPath + file + ".txt", Command);
                    log.Info("GetReport(" + Message + ") method.File:" + inputPath + file + ".txt is created.");
                }
                else
                {
                    log.Debug("GetReport(" + Message + ") method. No rows printed.");
                    Message = "No rows printed";

                    log.LogMethodExit(null);
                }

                foreach (string filename in System.IO.Directory.GetFiles(outputPath))
                {
                    if (filename.Contains(file))
                    {
                        foreach (string errFilename in System.IO.Directory.GetFiles(errorPath))
                        {
                            if (filename.Contains(file))
                            {
                                Message = "Operation errored out.";
                                log.Debug("GetReport(" + Message + ") method. Operation errored out.");
                                log.LogMethodExit(null);
                            }
                        }

                        Message = "Report generated succussfully.";
                        log.Debug("GetReport(" + Message + ") method. Report generated succussfully.");
                        log.LogMethodExit(null);
                    }
                }
                Message = "No response from the printer.";
                log.Debug("Reprint(" + Message + ") method. No response from the printer.");
                log.LogMethodExit(null);
            }
            catch (Exception ex)
            {
                log.Error("Error occured while printing", ex);
                Message = "Printing process is incomplete due to the error: " + ex.Message;
                log.Fatal("Ends- Reprint(" + Message + ")  method with exception: " + ex.ToString());
                log.LogMethodExit(null);
            }
        }
    }
}
