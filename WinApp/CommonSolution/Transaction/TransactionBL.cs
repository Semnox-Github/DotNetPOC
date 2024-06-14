/*************************************************************************************************************************
 * Project Name - Transaction                                                                     
 * Description  - Business logic class of transaction
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 ***************************************************************************************************************************
 *2.70.2         26-Nov-2019   Nitin Pai            Modified for Virtual store enhancement
 *2.70.2         26-Nov-2019   Lakshminarayana            Modified for Virtual store enhancement
 *2.70.2         06-Feb-2019   Nitin Pai            Fun City Changes to override site in app 
 *2.70.3         19-Mar-2020   Nitin Pai            Changing to user TrxHeaderDTO
 *2.70.3         06-May-2020   Guru S A             Changed method name for GetTransactionHeaderDTOList
 *2.80.0         04-Jun-2020   Nitin                Removed TrxHeaderDTO and TrxLineDTO. Using TransactionDTO and TransactionLineDTO impact in Execute(), CreateTransactionLine() and SaveTransaction()
 *                                                  TransactionListBL - GetTransactionList() methods
 *2.80.0         04-Jun-2020   Nitin                Website Enhancement - Continue as Guest - Saving CustomerIdentifier field in trx_header table, 
 *                                                  Send Receipt to customer view Email and SMS after transaction is closed successfully
 *2.80.0         04-Jun-2020   Nitin                Website Enhancement - Combo Sales, Attraction Sales in CreateTransactionLine and SaveTransaction, 
 *                                                  Pending code review comments, moved generic receipt printing method to Utilities
 *2.90.0         08-08-2020    Girish Kundar        Modified : Added method  to get refreshTransactionLines(trxId) for Report API
 *2.90.0         12-Jul-2020   Nitin                Website Fixes - receipt printing and consider user overriding price for manual products
 *2.90.0         19-Aug-2020   Nitin                Website Fixes - Misc fixes in online purchase, attraction purchase and payment using debit card
 *2.90.0         19-Aug-2020   Girish Kundar        Modified : Added BuildTemplate method to export excel fucntionality for Transaction view report WMS/Excel build issue fix
 *2.100.0        12-Oct-2020   Guru S A             Changes for print feature in Execute online transaction 
 *2.110          25-Nov-2020   Girish Kundar        Modified:  Paymemnt link enhancement/CenterEdge changes
 *2.110.0        08-Dec-2020   Guru S A             Subscription changes
 *2.110.0        29-Mar-2021   Nitin Pai            Fixes: Load to single card was not working for non attractions. Virtual card was failing on execute
 *2.120.0        29-Apr-2021   Girish Kundar        Modified : Guest login fixes
 *2.130.1        01-Dec-2021   Nitin Pai            Fixes: Combo level price is not considered, discounts are not calculated for non attraction products
 *2.140.0        01-Jun-2021   Fiona Lishal         Modified for Delivery Order enhancements for F&B
 *2.140.1        29-Dec-2021   Girish Kundar        Fixes: Guest Login Issue Fix for website
 *2.140.2        14-APR-2022    Girish Kundar       Modified : Aloha BSP changes
 *2.130.7        04-May-2022   Nitin Pai            Modified : After transaction save, add roaming data of only the CCP, CG and CD lines which were created by this transaction
 *2.140.2        17-May-2022    Girish Kundar       Modified : TET changes
 *2.130.7        23-May-2022   Nitin Pai            Fixes: Customer Identifier is returning data for like matches also
 *2.130.9        10-Jun-2022   Nitin Pai            Fixes: For debit card payment, validate the card balance and shift between credits and credit plus        
 *2.130.9        16-Jun-2022   Guru S A             Execute online transaction changes in Kiosk
 *2.130.9.2      12-Sep-2022   Mathew Ninan         Modification in BuildTransactionWithPrintDetails
 *                                                  to support multiple ticket printer
 ************************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using Utilities = Semnox.Core.Utilities.Utilities;
using Newtonsoft.Json;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.DBSynch;
using Semnox.Parafait.Device.PaymentGateway;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.JobUtils;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using Semnox.Parafait.Transaction.KDS;
using Semnox.Parafait.GenericUtilities;

namespace Semnox.Parafait.Transaction
{
    public class TransactionVisitDate
    {
        public DateTime VisitDate;
    }

    /// <summary>
    /// Business logic class of transaction
    /// </summary>
    public class TransactionBL
    {
        private TransactionDTO transactionDTO;
        private readonly ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Transaction newTrx;
        private const string TRXTOKENOBJECTCODE = "TRX_HEADER";
        private IUpdateDetails updateDetails;
        /// <summary>
        /// Default constructor of Transaction class
        /// </summary>
        private TransactionBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            transactionDTO = null;
            newTrx = null;
            log.LogMethodExit();
        }

        ///  <summary>
        /// Constructor will fetch the Transaction DTO based on the transaction id passed 
        ///  </summary>
        ///  <param name="executionContext">execution context</param>
        ///  <param name="transactionId">Transaction id</param>
        ///  <param name="sqlTransaction">sql transaction</param>
        public TransactionBL(ExecutionContext executionContext, int transactionId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, transactionId, sqlTransaction);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);

            Utilities Utilities = new Utilities();
            Utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            Utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            Utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            Utilities.ExecutionContext.SetMachineId(executionContext.MachineId);
            Utilities.ExecutionContext.SetPosMachineGuid(executionContext.PosMachineGuid);
            Utilities.ParafaitEnv.Initialize();

            if (transactionId > 0)
            {
                TransactionUtils trxUtils = new TransactionUtils(Utilities);
                newTrx = trxUtils.CreateTransactionFromDB(transactionId, Utilities, sqlTrx: sqlTransaction);
                if (newTrx == null)
                {
                    throw new ValidationException("Transaction Not Found");
                }
            }

            transactionDTO = newTrx.TransactionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates transaction object using the Transaction
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="transaction">Transaction object</param>
        /// <param name="sqlTransaction"></param>
        public TransactionBL(ExecutionContext executionContext, TransactionDTO transaction, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.transactionDTO = transaction;
            log.LogMethodExit();
        }

        public int GetProductId(int trxLineId, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(trxLineId);
            int productId = -1;
            if (transactionDTO != null)
            {
                List<TransactionLineDTO> transactionLineDTOList = transactionDTO.TransactionLinesDTOList.Where(trxLine => trxLine.LineId == trxLineId).ToList();
                if (transactionLineDTOList != null && transactionLineDTOList.Any())
                {
                    productId = transactionLineDTOList[0].ProductId;
                }
            }
            log.LogMethodExit(productId);
            return productId;
        }

        /// <summary>
        /// Saves the transaction record
        /// Checks if the TransactionId is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (transactionDTO.IsChanged)
            {
                List<ValidationError> validationErrorList = Validate(sqlTransaction);
                if (validationErrorList.Count > 0)
                {
                    throw new ValidationException("Validation Failed", validationErrorList);
                }
                TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
                if (transactionDTO.TransactionId < 0)
                {
                    throw new ValidationException("Validation Error - Creating a new transaction from 3-tier is not allowed");
                }
                else
                {
                    if (transactionDTO.ReverseTransaction)
                    {
                        ReverseTransaction();
                    }
                    else
                    {
                        if (transactionDTO.IsChanged)
                        {
                            transactionDTO = transactionDataHandler.UpdateTransaction(transactionDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                            transactionDTO.AcceptChanges();
                        }

                        if (transactionDTO.IsChangedRecursive)
                        {
                            foreach (TransactionLineDTO transactionLineDTO in transactionDTO.TransactionLinesDTOList)
                            {
                                TransactionLineBL transactionLineBL =
                                    new TransactionLineBL(executionContext, transactionLineDTO);
                                transactionLineBL.Save(sqlTransaction);
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        private List<ValidationError> Validate(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        public void UpdateTransactionRemarksAndExtReference(string transactionGuid, string externalSystemRef,
                                 string remarks, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(transactionGuid, externalSystemRef, remarks, sqlTransaction);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            transactionDataHandler.UpdateTransactionRemarksAndExtReference(transactionGuid,
                                                                     externalSystemRef, remarks);

            log.LogMethodExit();
        }

        /// <summary>
        /// Whether transaction contains printable online transaction lines.
        /// </summary>
        /// <returns></returns>
        public bool IsPrintableOnlineTransactionLinesExists()
        {
            log.LogMethodEntry();
            bool result = false;
            foreach (TransactionLineDTO transactionLineDTO in transactionDTO.TransactionLinesDTOList)
            {
                if (transactionLineDTO.ReceiptPrinted)
                {
                    continue;
                }
                //Manual lines
                if (string.IsNullOrWhiteSpace(transactionLineDTO.CardNumber))
                {
                    result = true;
                    break;
                }
                //Card lines
                if (string.IsNullOrWhiteSpace(transactionLineDTO.CardNumber) == false &&
                    transactionLineDTO.CardNumber.StartsWith("T") == false)
                {
                    result = true;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        internal List<KeyValuePair<string, List<TransactionLineDTO>>> GetPrintableTransactionLines(string printerTypeList, bool forVirtualStore)
        {
            log.LogMethodEntry(printerTypeList, forVirtualStore);
            List<KeyValuePair<string, List<TransactionLineDTO>>> printableLinesList = new List<KeyValuePair<string, List<TransactionLineDTO>>>();
            if (string.IsNullOrWhiteSpace(printerTypeList))
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 4562));
                // "Printer type list is not provided"
            }
            string[] printerTypes = printerTypeList.Split('|');
            List<PrinterDTO.PrinterTypes> printerTypeEnumList = new List<PrinterDTO.PrinterTypes>();
            for (int i = 0; i < printerTypes.Count(); i++)
            {
                PrinterDTO.PrinterTypes printerType = PrinterDTO.GetPrinterTypesEnumValue(printerTypes[i]);
                printerTypeEnumList.Add(printerType);
            }
            //int posMachineId = executionContext.GetMachineId();
            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
            if (forVirtualStore == false)
            {
                searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_MACHINE_ID, executionContext.GetMachineId().ToString()));
            }
            POSMachineList posMachineList = new POSMachineList(executionContext);
            List<POSMachineDTO> posMachines = posMachineList.GetAllPOSMachines(searchParameters);
            POSMachines posMachine = new POSMachines(executionContext, posMachines[0].POSMachineId);
            List<POSPrinterDTO> posPrinterDTOList = posMachine.PopulatePrinterDetails();
            if (posPrinterDTOList != null && posPrinterDTOList.Any())
            {
                for (int i = 0; i < printerTypeEnumList.Count; i++)
                {
                    for (int j = 0; j < posPrinterDTOList.Count; j++)
                    {
                        POSPrinterDTO posPrinterDTO = posPrinterDTOList[j];
                        if (posPrinterDTO.IsActive && posPrinterDTO.PrinterDTO != null && posPrinterDTO.PrinterDTO.PrinterType == printerTypeEnumList[i])
                        {
                            List<TransactionLineDTO> printableLines = new List<TransactionLineDTO>();
                            for (int k = 0; k < TransactionDTO.TransactionLinesDTOList.Count; k++)
                            {
                                TransactionLineDTO lineDTO = TransactionDTO.TransactionLinesDTOList[k];
                                if (lineDTO.CancelledTime == null && IsPrintableProduct(lineDTO.ProductId, posPrinterDTO))
                                {
                                    printableLines.Add(lineDTO);
                                }
                            }
                            if (printableLines.Any())
                            {
                                KeyValuePair<string, List<TransactionLineDTO>> valuePair = new KeyValuePair<string, List<TransactionLineDTO>>(printerTypeEnumList[i].ToString(), printableLines);
                                printableLinesList.Add(valuePair);
                            }
                        }
                    }
                }
            }
            else
            {
                string msg1 = MessageContainerList.GetMessage(executionContext, "Printer");
                string msg2 = MessageContainerList.GetMessage(executionContext, "POS Machine");
                string msg = MessageContainerList.GetMessage(executionContext, 2932, msg1, msg2) + " " + MessageContainerList.GetMessage(executionContext, "For") + " " + posMachine.POSMachineDTO.POSName;
                log.Info(msg);
                throw new Exception(msg);
                //&1 details are missing in &2 setup.
            }
            log.LogMethodExit(printableLinesList);
            return printableLinesList;
        }
        private static bool IsPrintableProduct(int productId, POSPrinterDTO posPrinterDTO)
        {
            log.LogMethodEntry(productId, posPrinterDTO);
            bool isPrintable = false;
            if (posPrinterDTO != null && posPrinterDTO.PrinterDTO != null && posPrinterDTO.PrinterDTO.PrintableProductIds != null
                && posPrinterDTO.PrinterDTO.PrintableProductIds.Any())
            {
                isPrintable = posPrinterDTO.PrinterDTO.PrintableProductIds.Exists(ppId => ppId == productId);
            }
            log.LogMethodExit(isPrintable);
            return isPrintable;
        }
        /// <summary>
        /// Prints the online transaction
        /// </summary>
        /// <param name="utilities">utilities</param>
        /// <param name="fiscalPrinter">fiscal printer</param>
        /// <param name="posPrintersDTOList">pos printer list</param>
        /// <returns></returns>
        public bool PrintOnlineTransaction(Utilities utilities, FiscalPrinter fiscalPrinter, List<POSPrinterDTO> posPrintersDTOList, List<TransactionLineDTO> selectedLines = null, bool printTrxReceipt = true)
        {
            log.LogMethodEntry(utilities, fiscalPrinter, posPrintersDTOList, selectedLines, printTrxReceipt);
            bool result;
            if (IsVirtualStore())
            {
                result = PrintVirtualStoreTransaction(utilities, posPrintersDTOList, selectedLines, printTrxReceipt);
            }
            else
            {
                result = PrintLocalStoreTransaction(utilities, fiscalPrinter, posPrintersDTOList, selectedLines, printTrxReceipt);
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool PrintVirtualStoreTransaction(Utilities utilities, List<POSPrinterDTO> posPrintersDTOList, List<TransactionLineDTO> selectedLines = null, bool printTrxReceipt = true)
        {
            log.LogMethodEntry(utilities, posPrintersDTOList, selectedLines, printTrxReceipt);
            TransactionWebDataHandler transactionWebDataHandler = new TransactionWebDataHandler(executionContext, GetVirtualSiteId());
            TransactionDTO printTransactionDTO = transactionWebDataHandler.DoOnlineTransactionPrint(transactionDTO, selectedLines);
            if (printTransactionDTO == null)
            {
                log.LogMethodExit(false, "Unable to print the online transaction through web-api.");
                return false;
            }
            bool result = true;
            if (printTrxReceipt)
            {
                ReceiptClass receipt = GetReceiptClassFromReceiptDTO(printTransactionDTO.ReceiptDTO);
                result = PrintVirtualStoreTransactionReceipt(utilities, receipt, posPrintersDTOList);
            }
            if (printTransactionDTO.TicketPrinterMapDTOList != null)
            {
                foreach (TicketPrinterMapDTO ticketPrinterMapDTO in printTransactionDTO.TicketPrinterMapDTOList)
                {
                    log.LogVariableState("TicketDTO from Virtual Store", ticketPrinterMapDTO.TicketDTOList);
                    List<clsTicket> tickets = GetClsTicketsFromTicketDTOList(ticketPrinterMapDTO.TicketDTOList);
                    log.LogVariableState("Printer DTO from Virtual Store", ticketPrinterMapDTO.POSPrinterDTO);
                    result &= PrintVirtualStoreTransactionTickets(utilities, tickets, posPrintersDTOList);
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool PrintVirtualStoreTransactionTickets(Utilities utilities, List<clsTicket> tickets, List<POSPrinterDTO> posPrintersDTOList)
        {
            log.LogMethodEntry(utilities, tickets, posPrintersDTOList);
            if (utilities.ParafaitEnv.PRINT_TRANSACTION_ITEM_TICKETS != "Y")
            {
                log.LogMethodExit(false, "PRINT_TRANSACTION_ITEM_TICKETS == 'N'");
                return true;
            }
            if (tickets == null || tickets.Any() == false)
            {
                log.LogMethodExit(true, "tickets are empty.");
                return true;
            }

            POSPrinterDTO ticketPOSPrinterDTO = GetDefaultTicketPOSPrinterDTO(posPrintersDTOList);
            if (ticketPOSPrinterDTO == null)
            {
                log.LogMethodExit(true, "No ticket printer configured.");
                return false;
            }
            PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext, ticketPOSPrinterDTO.PrinterDTO);
            System.Drawing.Printing.PrintDocument printTicketDocument = new System.Drawing.Printing.PrintDocument();
            string documentName = utilities.MessageUtils.getMessage("Transaction Receipt");

            bool setupPrintStatus = printerBL.SetupThePrinting(printTicketDocument, utilities.ParafaitEnv.IsClientServer, documentName);
            if (setupPrintStatus)
            {
                printTicketDocument.DefaultPageSettings.Margins = tickets[0].MarginProperty;
                int currentTicket = 0;
                printTicketDocument.PrintPage += (sender, e) =>
                {
                    printerBL.PrintTicketsToPrinter(tickets, currentTicket, e);
                };
                foreach (clsTicket clsTicket in tickets)
                {
                    printTicketDocument.Print();
                    currentTicket++;
                    if (utilities.ParafaitEnv.CUT_TICKET_PAPER)
                        printerBL.CutPrinterPaper(printTicketDocument.PrinterSettings.PrinterName);
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        private POSPrinterDTO GetDefaultTicketPOSPrinterDTO(List<POSPrinterDTO> posPrintersDTOList)
        {
            log.LogMethodEntry(posPrintersDTOList);
            POSPrinterDTO result = null;
            foreach (POSPrinterDTO posPrinterDTO in posPrintersDTOList)
            {
                if (posPrinterDTO.PrinterDTO != null &&
                    posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter)
                {
                    result = posPrinterDTO;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<clsTicket> GetClsTicketsFromTicketDTOList(List<TicketDTO> ticketDTOList)
        {
            log.LogMethodEntry(ticketDTOList);
            if (ticketDTOList == null || ticketDTOList.Any() == false)
            {
                log.LogMethodExit(null, "ticketDTOList is empty");
                return null;
            }
            List<clsTicket> clsTicketList = new List<clsTicket>();
            foreach (TicketDTO ticketDTO in ticketDTOList)
            {
                clsTicket clsTicket = GetClsTicketFromTicketDTO(ticketDTO);
                clsTicket.Backside = GetClsTicketFromTicketDTO(ticketDTO.Backside);
                clsTicketList.Add(clsTicket);
            }
            return clsTicketList;
        }

        private clsTicket GetClsTicketFromTicketDTO(TicketDTO ticketDTO)
        {
            log.LogMethodEntry(ticketDTO);
            if (ticketDTO == null)
            {
                log.LogMethodExit(null, "ticketDTO is null");
                return null;
            }
            clsTicket clsTicket = new clsTicket
            {
                BackgroundImage = ConvertBase64StringToImage(ticketDTO.BackgroundImageBase64),
                MarginProperty = ticketDTO.MarginProperty,
                PaperSizeProperty = ticketDTO.PaperSizeProperty,
                TicketBorderProperty = ticketDTO.TicketBorderProperty,
                TrxId = ticketDTO.TrxId,
                TrxLineId = ticketDTO.TrxLineId,
                CardNumber = ticketDTO.CardNumber,
                PrintObjectList = new List<clsTicket.PrintObject>()
            };
            if (ticketDTO.PrintObjectList != null && ticketDTO.PrintObjectList.Any())
            {
                foreach (TicketDTO.PrintObject printObject in ticketDTO.PrintObjectList)
                {
                    clsTicket.PrintObject newPrintObject =
                        new clsTicket.PrintObject
                        {
                            AlignmentProperty = printObject.AlignmentProperty,
                            BarCodeEncodeTypeProperty = printObject.BarCodeEncodeTypeProperty,
                            BarCodeHeightProperty = printObject.BarCodeHeightProperty,
                            BarCodeProperty = ConvertBase64StringToImage(printObject.BarCodeBase64Property),
                            ColorProperty = printObject.ColorProperty,
                            FontProperty = printObject.FontProperty,
                            ImageProperty = ConvertBase64StringToImage(printObject.ImageBase64Property),
                            LocationProperty = printObject.LocationProperty,
                            RotateProperty = printObject.RotateProperty,
                            TextProperty = printObject.TextProperty,
                            WidthProperty = printObject.WidthProperty
                        };
                    if (newPrintObject.BarCodeProperty != null)
                    {
                        newPrintObject.BarCodeProperty.Tag = printObject.BarCodeTagProperty;
                    }
                    clsTicket.PrintObjectList.Add(newPrintObject);
                }
            }
            log.LogMethodExit(clsTicket);
            return clsTicket;
        }

        private bool PrintLocalStoreTransaction(Utilities utilities, FiscalPrinter fiscalPrinter, List<POSPrinterDTO> posPrintersDTOList, List<TransactionLineDTO> selectedLines = null, bool printTrxReceipt = true)
        {
            log.LogMethodEntry(posPrintersDTOList, selectedLines, printTrxReceipt);
            bool result = false;
            string message = null;
            bool kotOnly = false;
            bool receiptOnly = false;

            TransactionUtils transactionUtils = new TransactionUtils(utilities);
            Transaction trx = transactionUtils.CreateTransactionFromDB(transactionDTO.TransactionId, utilities);
            DateTime printStartTime = utilities.getServerTime();
            if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(executionContext, "USE_FISCAL_PRINTER"))
            {
                object o = utilities.executeScalar(@"select PrintStartTime from trx_header WHERE TrxId = @TrxId  ", new SqlParameter("@TrxId", transactionDTO.TransactionId));
                if (o == null || o == DBNull.Value)
                {
                    result = fiscalPrinter.PrintReceipt(transactionDTO.TransactionId, ref message, null, 0, false);
                    if (result == false && utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                    {
                        PrintTransaction printTransactions = new PrintTransaction(posPrintersDTOList);
                        result = printTransactions.Print(trx, ref message, kotOnly, receiptOnly, printTrxReceipt);
                    }
                    log.LogMethodExit(result, "Transaction printed through fiscal printer.");
                    trx.UpdateTrxHeaderSavePrintTime(trx.Trx_id, null, null, printStartTime, utilities.getServerTime());
                    return result;
                }
                else
                {
                    result = false;
                    log.LogMethodExit(result, "Transaction already printed. Skipping print..");
                    return result;
                }
            }
            foreach (Transaction.TransactionLine tl in trx.TrxLines)
            {
                if (string.IsNullOrEmpty(tl.CardNumber))
                {
                    tl.LineValid = !tl.ReceiptPrinted;
                }
                else
                {
                    tl.LineValid = (!tl.CardNumber.StartsWith("T") && !tl.ReceiptPrinted);
                }
                //eligible, but not selected
                if (tl.LineValid && selectedLines != null && selectedLines.Exists(stl => stl.Guid == tl.guid) == false)
                {
                    tl.LineValid = false;
                }
            }

            PrintTransaction printTransaction = new PrintTransaction(posPrintersDTOList);
            result = printTransaction.Print(trx, ref message, kotOnly, receiptOnly, printTrxReceipt);
            trx.UpdateTrxHeaderSavePrintTime(trx.Trx_id, null, null, printStartTime, utilities.getServerTime());
            return result;
        }

        private bool PrintVirtualStoreTransactionReceipt(Utilities utilities, ReceiptClass receipt, List<POSPrinterDTO> posPrintersDTOList)
        {
            POSPrinterDTO defaultReceiptPOSPrinterDTO = GetDefaultReceiptPOSPrinterDTO(posPrintersDTOList);
            if (defaultReceiptPOSPrinterDTO == null)
            {
                log.LogMethodExit(false, "Unable to find the default receipt printer");
                return false;
            }

            if (receipt == null || receipt.TotalLines <= 0)
            {
                log.LogMethodExit(false, "Empty Receipt");
                return false;
            }
            PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext, defaultReceiptPOSPrinterDTO.PrinterDTO);
            System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();
            string documentName = utilities.MessageUtils.getMessage("Transaction Receipt");

            bool setupPrintStatus =
                printerBL.SetupThePrinting(printDocument, utilities.ParafaitEnv.IsClientServer, documentName);
            if (setupPrintStatus)
            {
                int receiptLineIndex = 0;
                printDocument.PrintPage += (sender, e) =>
                {
                    e.HasMorePages = printerBL.PrintReceiptToPrinter(receipt, ref receiptLineIndex, e);
                };

                using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
                {
                    //code to send print document to the printer
                    printDocument.Print();
                }

                //MyPrintDocument.Print();

                if (utilities.ParafaitEnv.CUT_RECEIPT_PAPER)
                    printerBL.CutPrinterPaper(printDocument.PrinterSettings.PrinterName);
            }

            if (defaultReceiptPOSPrinterDTO.SecondaryPrinterId > -1)
            {
                PrinterBL secondaryPrinterBL =
                    new PrinterBL(utilities.ExecutionContext, defaultReceiptPOSPrinterDTO.SecondaryPrinterDTO);
                printDocument = new System.Drawing.Printing.PrintDocument();
                documentName = utilities.MessageUtils.getMessage("Transaction Receipt");

                setupPrintStatus =
                    printerBL.SetupThePrinting(printDocument, utilities.ParafaitEnv.IsClientServer, documentName);
                if (setupPrintStatus)
                {
                    int receiptLineIndex = 0;

                    if (defaultReceiptPOSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter
                        && utilities.getParafaitDefaults("ALLOW_MULTIPLE_TRX_PRINT_COPIES") == "Y"
                        && transactionDTO.Status == Transaction.TrxStatus.CLOSED.ToString())
                    {
                        for (int i = 0; i < receipt.TotalLines; i++)
                        {
                            if (receipt.ReceiptLines[i].TemplateSection == "HEADER")
                            {
                                if (receipt.ReceiptLines[i].Data[0] == utilities.MessageUtils.getMessage("Invoice")
                                    || receipt.ReceiptLines[i].Data[0] == utilities.MessageUtils.getMessage("Customer Copy"))
                                {
                                    receipt.ReceiptLines[i].Data[0] = utilities.MessageUtils.getMessage("Accounting Copy");
                                    break;
                                }
                            }
                        }
                    }

                    printDocument.PrintPage += (sender, e) =>
                    {
                        e.HasMorePages = printerBL.PrintReceiptToPrinter(receipt, ref receiptLineIndex, e);
                    };

                    printDocument.Print();

                    if (utilities.ParafaitEnv.CUT_RECEIPT_PAPER)
                        secondaryPrinterBL.CutPrinterPaper(printDocument.PrinterSettings.PrinterName);
                }
            }
            log.LogMethodExit(true);
            return true;
        }


        private POSPrinterDTO GetDefaultReceiptPOSPrinterDTO(List<POSPrinterDTO> posPrintersDTOList)
        {
            log.LogMethodEntry(posPrintersDTOList);
            POSPrinterDTO result = null;
            foreach (POSPrinterDTO posPrinterDTO in posPrintersDTOList)
            {
                if (posPrinterDTO.PrinterDTO != null &&
                    posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter)
                {
                    result = posPrinterDTO;
                    break;
                }
            }
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// GetReceiptClassFromReceiptDTO
        /// </summary> 
        /// <returns></returns>
        public ReceiptClass GetReceiptClassFromReceiptDTO(ReceiptDTO receiptDTO)
        {
            log.LogMethodEntry(receiptDTO);
            if (receiptDTO == null)
            {
                log.LogMethodExit(null, "Empty receiptDTO");
                return null;
            }
            int count = receiptDTO.TotalLines;
            ReceiptClass receiptClass = new ReceiptClass(count);

            for (int i = 0; i < count; i++)
            {

                ReceiptDTO.line line = receiptDTO.ReceiptLines[i];
                ReceiptClass.line newLine = receiptClass.ReceiptLines[i];
                newLine.Alignment = line.Alignment;
                newLine.BarCode = ConvertBase64StringToImage(line.BarCodeBase64);
                if (newLine.BarCode != null)
                {
                    newLine.BarCode.Tag = line.BarCodeTag;
                }
                newLine.CancelledLine = line.CancelledLine;
                newLine.colCount = line.colCount;
                newLine.Data = line.Data;
                newLine.LineFont = line.LineFont;
                newLine.LineHeight = line.LineHeight;
                newLine.LineId = line.LineId;
                newLine.TemplateSection = line.TemplateSection;
            }
            receiptClass.TotalLines = count;
            log.LogMethodExit(receiptClass);
            return receiptClass;
        }

        private bool IsVirtualStore()
        {
            string virtualStoreSiteId =
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "VIRTUAL_STORE_SITE_ID");
            return string.IsNullOrWhiteSpace(virtualStoreSiteId) == false;
        }

        private int GetVirtualSiteId()
        {
            return ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "VIRTUAL_STORE_SITE_ID");
        }

        /// <summary>
        /// Executes the online transactions. Issues cards
        /// </summary>
        /// <param name="utilities">utilities</param>
        /// <param name="cardRoamingRemotingClient">remoting client</param>
        /// <param name="tempCardLines">lines to be executed</param>
        /// <param name="tagNumbers">physical tag number</param>
        /// <exception cref="Exception"></exception>
        public void ExecuteOnlineTransaction(Utilities utilities, RemotingClient cardRoamingRemotingClient, List<TransactionLineDTO> tempCardLines, List<string> tagNumbers)
        {
            log.LogMethodEntry(utilities, tempCardLines, tagNumbers);
            if (tempCardLines == null || tagNumbers == null || tempCardLines.Count == 0 ||
                                tempCardLines.Count != tagNumbers.Count)
            {
                log.LogMethodExit(null, "Lines to be executed is empty.");
                return;
            }
            if (IsVirtualStore())
            {
                DownloadTempCards(cardRoamingRemotingClient, utilities.ParafaitEnv.SiteId, tempCardLines);
            }
            CheckQueueProducts(tempCardLines, tagNumbers);

            using (ParafaitDBTransaction parafaitDbTransaction = new ParafaitDBTransaction())
            {
                parafaitDbTransaction.BeginTransaction();
                TaskProcs tp = new TaskProcs(utilities);
                CardUtils cardUtils = new CardUtils(utilities);
                List<string> cardNumberList = tempCardLines.Where(tl => string.IsNullOrWhiteSpace(tl.CardNumber) == false).Select(tl => tl.CardNumber).ToList();
                cardNumberList.AddRange(tagNumbers);
                List<Card> cardObjectList = cardUtils.GetCardList(cardNumberList, "", parafaitDbTransaction.SQLTrx);

                for (int i = 0; i < tempCardLines.Count; i++)
                {
                    TransactionLineDTO tempCardLine = tempCardLines[i];
                    string tagNumber = tagNumbers[i];
                    if (tempCardLine.CardNumber == tagNumber)
                    {
                        continue;
                    }
                    Card tempCard = cardObjectList.Find(cardObject => cardObject.CardNumber == tempCardLine.CardNumber); // new Card(tempCardLine.CardNumber, "", utilities);
                    if (tempCard.CardStatus.Equals("NEW"))
                    {
                        string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 1684, tempCardLine.CardNumber);
                        log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                        throw new Exception(errorMessage);
                    }
                    Card card = cardObjectList.Find(cardObject => cardObject.CardNumber == tagNumber); // new Card(tagNumber, "", utilities);
                    if (card.CardStatus == "ISSUED")
                    {
                        List<Card> cards = new List<Card> { tempCard, card };
                        string message = string.Empty;
                        if (!tp.Consolidate(cards, 2, "Execute Transaction", ref message, parafaitDbTransaction.SQLTrx, true, true))
                        {
                            log.LogMethodExit(false, "Throwing Exception - " + message);
                            throw new Exception(message);
                        }
                    }
                    else
                    {
                        string message = string.Empty;
                        if (!tp.transferCard(tempCard, card, "Execute Transaction", ref message, parafaitDbTransaction.SQLTrx, (IsVirtualStore() ? -1 : this.transactionDTO.TransactionId)))
                        {
                            log.LogMethodExit(false, "Throwing Exception - " + message);
                            throw new Exception(message);
                        }
                    }
                }
                if (IsVirtualStore())
                {
                    //Execute the virtual store transaction
                    UpdateVirtualStoreTransaction(tempCardLines, tagNumbers);
                }
                parafaitDbTransaction.EndTransaction();
            }
            log.LogMethodExit();
        }

        private void UpdateVirtualStoreTransaction(List<TransactionLineDTO> tempCardLines, List<string> tagNumbers)
        {
            log.LogMethodEntry(tempCardLines, tagNumbers);
            TransactionDTO copyTransactionDTO = new TransactionDTO(transactionDTO);
            for (int i = 0; i < tempCardLines.Count; i++)
            {
                TransactionLineDTO tempCardLine = tempCardLines[i];
                string tagNumber = tagNumbers[i];
                List<TransactionLineDTO> copyTransactionLineDTOList =
                    copyTransactionDTO.TransactionLinesDTOList.Where(x => x.CardNumber == tempCardLine.CardNumber).ToList();
                if (copyTransactionLineDTOList.Any() == false)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2416,
                        tempCardLine.CardNumber); //"Unable to find a transaction line with card number (&1) ")
                    log.LogMethodExit("Throwing exception - " + errorMessage);
                    throw new Exception(errorMessage);
                }

                copyTransactionDTO.IsChanged = true;
                AccountListBL accountListBL = new AccountListBL(executionContext);
                List<KeyValuePair<AccountDTO.SearchByParameters, string>> searchParameters =
                    new List<KeyValuePair<AccountDTO.SearchByParameters, string>>();
                searchParameters.Add(
                    new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.TAG_NUMBER,
                        tagNumber));
                searchParameters.Add(
                    new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.VALID_FLAG, "Y"));
                searchParameters.Add(
                    new KeyValuePair<AccountDTO.SearchByParameters, string>(AccountDTO.SearchByParameters.REFUND_FLAG, "N"));
                List<AccountDTO> accountDTOList = accountListBL.GetAccountDTOList(searchParameters, null);
                foreach (TransactionLineDTO copyTransactionLineDTO in copyTransactionLineDTOList)
                {
                    copyTransactionLineDTO.CardNumber = tagNumber;
                    if (accountDTOList != null && accountDTOList.Count > 0)
                    {
                        copyTransactionLineDTO.CardGuid = accountDTOList[0].Guid;
                    }
                }
                log.LogMethodExit();
            }
            copyTransactionDTO.SaveTransaction = true;
            TransactionWebDataHandler transactionWebDataHandler =
                new TransactionWebDataHandler(executionContext, GetVirtualSiteId());
            transactionWebDataHandler.UpdateTransaction(copyTransactionDTO);
        }

        private void CheckQueueProducts(List<TransactionLineDTO> tempCardLines, List<string> tagNumbers)
        {
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler();
            for (int i = 0; i < tempCardLines.Count; i++)
            {
                if (transactionDataHandler.IsQueueProductExists(tempCardLines[i].CardNumber, tagNumbers[i]))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 495) + " [" + tagNumbers[i] + "]";
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new Exception(errorMessage);
                }
            }
        }

        private void DownloadTempCards(RemotingClient cardRoamingRemotingClient, int siteId, List<TransactionLineDTO> tempCardLines)
        {
            log.LogMethodEntry(cardRoamingRemotingClient, tempCardLines);
            if (cardRoamingRemotingClient == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2417);
                log.LogMethodExit("Throwing exception - " + errorMessage);
                throw new Exception(errorMessage);
            }
            foreach (TransactionLineDTO transactionLineDTO in tempCardLines)
            {
                string message = string.Empty;
                string cardNumber;
                try
                {
                    cardNumber = cardRoamingRemotingClient.GetServerCard(transactionLineDTO.CardNumber, siteId, ref message);
                }
                catch (Exception ex)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2415);//"Error occured while downloading temp card(s) from server."
                    log.Error(errorMessage, ex);
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new Exception(errorMessage, ex);
                }

                if (cardNumber != transactionLineDTO.CardNumber)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2414, transactionLineDTO.CardNumber);//"Unable to download temp card from server. Temp Card Number &1"
                    log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                    throw new Exception(errorMessage);
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Returns Purchased Transaction lines from the given time range
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="selectedLineDTO"></param>
        /// <returns></returns>
        public void BuildTransactionWithPrintDetails(Utilities utilities, List<TransactionLineDTO> selectedLineDTO = null)
        {
            log.LogMethodEntry(utilities, selectedLineDTO);
            try
            {
                TransactionUtils transactionUtils = new TransactionUtils(utilities);
                Transaction newTrx = transactionUtils.CreateTransactionFromDB(this.transactionDTO.TransactionId, utilities);

                if (newTrx != null && newTrx.TrxLines != null && newTrx.TrxLines.Any())
                {
                    foreach (Transaction.TransactionLine tl in newTrx.TrxLines)
                    {
                        if (selectedLineDTO != null && selectedLineDTO.Exists(line => line.Guid == tl.Guid) == false)
                        {
                            tl.LineValid = false;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(tl.CardNumber))
                            {
                                tl.LineValid = !tl.ReceiptPrinted;
                            }
                            else
                            {
                                tl.LineValid = (!tl.CardNumber.StartsWith("T") && !tl.ReceiptPrinted);
                            }
                        }
                    }

                    if (newTrx.TrxLines.Any(x => x.LineValid) == false)
                    {
                        log.LogMethodExit(null, "No cards to print");
                        return;
                    }
                    newTrx.TransactionInfo.createTransactionInfo(newTrx.Trx_id, -1);
                    List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                    POSMachineList posMachineList = new POSMachineList(executionContext);
                    List<POSMachineDTO> posMachines = posMachineList.GetAllPOSMachines(searchParameters);
                    POSMachines posMachine = new POSMachines(executionContext, posMachines[0].POSMachineId);
                    List<POSPrinterDTO> posPrintersDTOList = posMachine.PopulatePrinterDetails();
                    log.Debug("POS Machine: " + posMachine.POSMachineDTO.POSName);
                    log.Debug("posPrintersDTOList : " + posPrintersDTOList);
                    if (posPrintersDTOList == null || !posPrintersDTOList.Any())
                    {
                        log.Debug("posPrintersDTOList == null");
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2413, posMachines[0].POSName); //"Unable to find a receipt printer associated with &1 POS";
                        throw new Exception(errorMessage);
                    }
                    POSPrinterDTO receiptPrinterDTO = posPrintersDTOList.FirstOrDefault(x => x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter);
                    POSPrinterDTO ticketPrinterDTO = posPrintersDTOList.FirstOrDefault(x => x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter);

                    if (receiptPrinterDTO == null)
                    {
                        log.Debug("receiptPrinterDTO == null");
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2413, posMachines[0].POSName); //"Unable to find a receipt printer associated with &1 POS";
                        throw new Exception(errorMessage);
                    }

                    ReceiptClass content = POSPrint.PrintReceipt(newTrx, receiptPrinterDTO);
                    if (content != null)
                    {
                        transactionDTO.ReceiptDTO = CreateReceiptDTOFromReceiptClass(content);
                    }

                    else
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2412, transactionDTO.TransactionId);//"Unable to generate receipt for transaction with id &1.";
                        throw new Exception(errorMessage);
                    }

                    if (ticketPrinterDTO != null) //Existence of at least one ticket printer
                    {
                        List<Transaction.TransactionLine> origTrxLines = new List<Transaction.TransactionLine>();
                        origTrxLines.AddRange(newTrx.TrxLines);
                        newTrx.GetPrintableTransactionLines(posPrintersDTOList);
                        Transaction printTransaction;
                        List<TicketPrinterMapDTO> ticketPrintMapDTOList = new List<TicketPrinterMapDTO>();
                        foreach (Transaction.EligibleTrxLinesPrinterMapper eligibleTrxLinesPrinterMapper in newTrx.EligibleTrxLinesPrinterMapperList)
                        {
                            if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter)
                            {
                                int ticketsToPrint = 0;
                                int templateId = eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrintTemplateId;
                                if (newTrx.TrxPOSPrinterOverrideRulesDTOList != null && newTrx.TrxPOSPrinterOverrideRulesDTOList.Any())
                                {
                                    TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(executionContext);
                                    int overrideTemplateId = trxPOSPrinterOverrideRulesListBL.GetTemplateId(newTrx.TrxPOSPrinterOverrideRulesDTOList, ticketPrinterDTO);
                                    if (overrideTemplateId > -1)
                                    {
                                        templateId = overrideTemplateId;
                                    }
                                }
                                printTransaction = newTrx;
                                printTransaction.TrxLines.Clear();
                                printTransaction.TrxLines.AddRange(eligibleTrxLinesPrinterMapper.TrxLines);
                                log.Debug("Printer is " + eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterName + ". Count of Trx Lines: " + eligibleTrxLinesPrinterMapper.TrxLines.Count);
                                List<clsTicket> ticketList = POSPrint.getTickets(templateId, printTransaction, ref ticketsToPrint);
                                if (ticketList != null)
                                {
                                    List<TicketDTO> ticketDTOList = CreateTicketDTOFromClsTicket(ticketList);
                                    //this.transactionDTO.TicketDTOList = CreateTicketDTOFromClsTicket(ticketList);
                                    ticketPrintMapDTOList.Add(new TicketPrinterMapDTO(ticketDTOList, eligibleTrxLinesPrinterMapper.POSPrinterDTO));
                                }
                            }
                        }
                        log.Debug("Ticket Print Map List: " + ticketPrintMapDTOList.Count);
                        this.transactionDTO.TicketPrinterMapDTOList = new List<TicketPrinterMapDTO>();
                        this.transactionDTO.TicketPrinterMapDTOList.AddRange(ticketPrintMapDTOList);
                        newTrx.TrxLines.Clear();
                        newTrx.TrxLines.AddRange(origTrxLines);
                    }

                    foreach (Transaction.TransactionLine transactionLine in newTrx.TrxLines)
                    {
                        if (transactionLine.LineValid)
                        {
                            transactionLine.ReceiptPrinted = true;
                        }
                    }
                    newTrx.updateTrxLinesReceiptPrintedStatus();
                }
                else
                {
                    string errorMessage =
                        MessageContainerList.GetMessage(executionContext, 2411, transactionDTO.TransactionId);// "Unable to find a transaction with transaction id &1.";
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while building transaction with print details", ex);
                log.LogMethodExit(null, "Throwing Exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// BuildTransactionReceiptAndTickets
        /// </summary>
        /// <param name="buildReceipt"></param>
        /// <param name="buildTickets"></param>
        /// <returns></returns>
        public void BuildTransactionReceiptAndTickets(bool buildReceipt, bool buildTickets)
        {
            log.LogMethodEntry(buildReceipt, buildTickets);
            try
            {
                if (newTrx != null && newTrx.TrxLines != null && newTrx.TrxLines.Any())
                {
                    if (newTrx.TrxLines.Any(x => x.LineValid) == false)
                    {
                        log.LogMethodExit(null, "No cards to print");
                        return;
                    }
                    newTrx.TransactionInfo.createTransactionInfo(newTrx.Trx_id, -1);
                    List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, String.IsNullOrEmpty(this.transactionDTO.PosMachine) ? Environment.MachineName : this.transactionDTO.PosMachine));

                    POSMachineList posMachineList = new POSMachineList(executionContext);
                    List<POSMachineDTO> posMachines = posMachineList.GetAllPOSMachines(searchParameters);
                    if (posMachines == null || !posMachines.Any())
                    {
                        String error = MessageContainerList.GetMessage(executionContext, 1743, newTrx.POSMachine);
                        log.Error(error);
                        throw new Exception(error);
                    }

                    POSMachines posMachine = new POSMachines(executionContext, posMachines[0].POSMachineId);
                    List<POSPrinterDTO> posPrintersDTOList = posMachine.PopulatePrinterDetails();

                    if (posPrintersDTOList == null || !posPrintersDTOList.Any())
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2413, posMachines[0].POSName); //"Unable to find a receipt printer associated with &1 POS";
                        log.Error(errorMessage);
                        throw new Exception(errorMessage);
                    }

                    POSPrinterDTO receiptPrinterDTO = posPrintersDTOList.FirstOrDefault(x => x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter);
                    POSPrinterDTO ticketPrinterDTO = posPrintersDTOList.FirstOrDefault(x => x.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter);
                    PrintTransaction printTransaction = new PrintTransaction(posPrintersDTOList);

                    if (receiptPrinterDTO == null && buildReceipt)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 2413, posMachines[0].POSName); //"Unable to find a receipt printer associated with &1 POS";
                        throw new Exception(errorMessage);
                    }
                    else if (buildReceipt)
                    {
                        newTrx.TransactionDTO.Receipt = transactionDTO.Receipt = printTransaction.printPosReceipt(newTrx, receiptPrinterDTO, -1, 303, -1, false, true);
                        newTrx.TransactionDTO.ReceiptHTML = transactionDTO.ReceiptHTML = GenerateReceiptEMailFromTemplate(newTrx.Utilities.getParafaitDefaults("ONLINE_RECEIPT_EMAIL_TEMPLATE"));

                    }

                    if (ticketPrinterDTO != null && buildTickets)
                    {
                        List<string> tickets = printTransaction.printTickets(newTrx, ticketPrinterDTO, -1, 303, -1, false, true);
                        if (tickets != null && tickets.Any())
                        {
                            newTrx.TransactionDTO.Tickets = transactionDTO.Tickets = tickets[0];
                            newTrx.TransactionDTO.TicketsHTML = transactionDTO.TicketsHTML = GenerateTicketEMailFromTemplate(newTrx.Utilities.getParafaitDefaults("ONLINE_TICKETS_B2C_EMAIL_TEMPLATE"));
                        }
                    }
                }
                else
                {
                    string errorMessage =
                        MessageContainerList.GetMessage(executionContext, 2411, transactionDTO.TransactionId);// "Unable to find a transaction with transaction id &1.";
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while building transaction with print details", ex);
                log.LogMethodExit(null, "Throwing Exception - " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        private ReceiptDTO CreateReceiptDTOFromReceiptClass(ReceiptClass content)
        {
            log.LogMethodEntry(content);
            int count = content.TotalLines;
            ReceiptDTO receiptDTO = new ReceiptDTO(count);

            for (int i = 0; i < count; i++)
            {
                ReceiptClass.line line = content.ReceiptLines[i];
                ReceiptDTO.line newLine = receiptDTO.ReceiptLines[i];
                newLine.Alignment = line.Alignment;
                newLine.BarCodeBase64 = ConvertImageToBase64String(line.BarCode);
                newLine.BarCodeTag = (line.BarCode != null && line.BarCode.Tag != null)
                    ? line.BarCode.Tag.ToString()
                    : string.Empty;
                newLine.CancelledLine = line.CancelledLine;
                newLine.colCount = line.colCount;
                newLine.Data = line.Data;
                newLine.LineFont = line.LineFont;
                newLine.LineHeight = line.LineHeight;
                newLine.LineId = line.LineId;
                newLine.TemplateSection = line.TemplateSection;
            }
            receiptDTO.TotalLines = count;
            log.LogMethodExit(receiptDTO);
            return receiptDTO;
        }

        private string ConvertImageToBase64String(System.Drawing.Image image)
        {
            if (image == null)
            {
                return string.Empty;
            }

            string result = string.Empty;
            try
            {
                using (var stream = new MemoryStream())
                {
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    result = Convert.ToBase64String(stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while converting image to base 64 string", ex);
            }

            return result;
        }

        private System.Drawing.Image ConvertBase64StringToImage(string base64String)
        {
            log.LogMethodEntry();
            System.Drawing.Image result = GenericUtils.ConvertBase64StringToImage(base64String);
            log.LogMethodExit();
            return result;
        }

        private List<TicketDTO> CreateTicketDTOFromClsTicket(List<clsTicket> contentList)
        {
            log.LogMethodEntry(contentList);
            List<TicketDTO> ticketDTOList = new List<TicketDTO>();
            foreach (clsTicket content in contentList)
            {
                TicketDTO ticketDTO = GetTicketDTOFromClsTicket(content);
                ticketDTO.Backside = GetTicketDTOFromClsTicket(content.Backside);
                ticketDTOList.Add(ticketDTO);
            }
            log.LogMethodExit(ticketDTOList);
            return ticketDTOList;
        }

        private TicketDTO GetTicketDTOFromClsTicket(clsTicket clsTicket)
        {
            log.LogMethodEntry(clsTicket);
            if (clsTicket == null)
            {
                log.LogMethodExit(null, "clsTicket is empty");
                return null;
            }
            TicketDTO ticketDTO = new TicketDTO
            {
                MarginProperty = clsTicket.MarginProperty,
                PaperSizeProperty = clsTicket.PaperSizeProperty,
                TicketBorderProperty = clsTicket.TicketBorderProperty,
                TrxId = clsTicket.TrxId,
                TrxLineId = clsTicket.TrxLineId,
                BackgroundImageBase64 = ConvertImageToBase64String(clsTicket.BackgroundImage),
                CardNumber = clsTicket.CardNumber,
                PrintObjectList = new List<TicketDTO.PrintObject>()
            };
            foreach (clsTicket.PrintObject printObject in clsTicket.PrintObjectList)
            {
                TicketDTO.PrintObject newPrintObject = new TicketDTO.PrintObject
                {
                    AlignmentProperty = printObject.AlignmentProperty,
                    BarCodeEncodeTypeProperty = printObject.BarCodeEncodeTypeProperty,
                    BarCodeHeightProperty = printObject.BarCodeHeightProperty,
                    BarCodeBase64Property = ConvertImageToBase64String(printObject.BarCodeProperty),
                    BarCodeTagProperty = printObject.BarCodeProperty != null && printObject.BarCodeProperty.Tag != null ? printObject.BarCodeProperty.Tag.ToString() : string.Empty,
                    ColorProperty = printObject.ColorProperty,
                    FontProperty = printObject.FontProperty,
                    ImageBase64Property = ConvertImageToBase64String(printObject.ImageProperty),
                    LocationProperty = printObject.LocationProperty,
                    RotateProperty = printObject.RotateProperty,
                    TextProperty = printObject.TextProperty,
                    WidthProperty = printObject.WidthProperty
                };
                ticketDTO.PrintObjectList.Add(newPrintObject);
            }

            return ticketDTO;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public TransactionDTO TransactionDTO
        {
            get
            {
                return transactionDTO;
            }
        }

        /// <summary>
        /// Gets the Transaction
        /// </summary>
        public Transaction Transaction
        {
            get
            {
                return this.newTrx;
            }
        }

        private void CreateRefreshTransactionHeader(Utilities Utilities)
        {
            log.LogMethodEntry();
            CustomerDTO customerDTO = null;
            if (this.transactionDTO.CustomerId != -1)
                customerDTO = (new CustomerBL(Utilities.ExecutionContext, this.transactionDTO.CustomerId)).CustomerDTO;


            if (newTrx != null && newTrx.Status == Transaction.TrxStatus.PENDING)
            {
                log.Error("Transaction is in PENDING status. Cannot edit.");
                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1413));
            }
            else if (newTrx == null)
            {
                newTrx = new Transaction(null, Utilities);
                log.Debug("New trx date is " + newTrx.TrxDate + "transaction date is " + transactionDTO.TransactionDate);
                log.Debug("transactionDTO.ApplyOffset is " + transactionDTO.ApplyOffset + "site info " + Utilities.ExecutionContext.GetSiteId());
                DateTime transactionDate = Utilities.getServerTime();
                if (!string.IsNullOrEmpty(transactionDTO.VisitDate) && transactionDTO.ApplyVisitDate)
                {
                    String datejson = "{ \"VisitDate\":\" " + transactionDTO.VisitDate + " \"}";
                    try
                    {
                        TransactionVisitDate visitDate = JsonConvert.DeserializeObject<TransactionVisitDate>(datejson);
                        transactionDate = visitDate.VisitDate;
                    }
                    catch (Exception ex)
                    {
                        log.Error("Could not convert json to date" + datejson + ex.Message);
                        DateTime visitDateParse = DateTime.MinValue;
                        if (DateTime.TryParse(transactionDTO.VisitDate, out visitDateParse))
                        {
                            log.Debug("Parsed transaction date " + visitDateParse.ToString());
                            transactionDate = visitDateParse;
                        }
                        else
                        {
                            transactionDate = Utilities.getServerTime();
                        }
                    }

                    log.Error("Transaction date " + transactionDate.ToString());
                    transactionDate = transactionDate.Date.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 6));

                    // Adding 5 minutes to handle boundry condition
                    transactionDate = transactionDate.AddMinutes(5);
                    log.Error("Transaction date " + transactionDate.ToString());

                    TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                    int offSetDuration = timeZoneUtil.GetOffSetDuration(Utilities.ExecutionContext.GetSiteId(), Utilities.getServerTime().Date);
                    log.Debug("Applying offset to transaction:" + offSetDuration);
                    transactionDate = transactionDate.AddSeconds(offSetDuration);
                }
                else if (transactionDTO.TransactionDate != DateTime.MinValue && transactionDTO.ApplyOffset)
                {
                    transactionDate = transactionDTO.TransactionDate.Date;
                    transactionDate = transactionDate.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 6));

                    // Adding 5 minutes to handle boundry condition
                    transactionDate = transactionDate.AddMinutes(5);
                    log.Error("Transaction date " + transactionDate.ToString());

                    TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                    int offSetDuration = timeZoneUtil.GetOffSetDuration(Utilities.ExecutionContext.GetSiteId(), Utilities.getServerTime().Date);
                    log.Debug("Applying offset to transaction:" + offSetDuration);
                    transactionDate = transactionDate.AddSeconds(offSetDuration);
                }
                log.Debug("transactionDate " + transactionDate);
                newTrx.TrxDate = newTrx.EntitlementReferenceDate = transactionDate;
            }

            log.Debug("Final trx date is " + newTrx.TrxDate);

            if (!String.IsNullOrEmpty(transactionDTO.PrimaryCard) && newTrx.PrimaryCard == null)
                newTrx.PrimaryCard = new Card(transactionDTO.PrimaryCard, newTrx.Utilities.ExecutionContext.GetUserId(), newTrx.Utilities);

            if (transactionDTO.PrimaryCardId != -1 && newTrx.PrimaryCard == null)
                newTrx.PrimaryCard = new Card(transactionDTO.PrimaryCardId, newTrx.Utilities.ExecutionContext.GetUserId(), newTrx.Utilities);

            //Primary card should be an existing card.Set it to null if it does not exist
            if (newTrx.PrimaryCard != null && newTrx.PrimaryCard.card_id == -1)
                newTrx.PrimaryCard = null;

            if (customerDTO != null)
                newTrx.customerDTO = customerDTO;

            if (string.IsNullOrWhiteSpace(transactionDTO.ExternalSystemReference) == false)
            {
                newTrx.externalSystemReference = transactionDTO.ExternalSystemReference;
            }
            log.LogMethodExit();
        }

        private string CreateTransactionLine(TransactionLineDTO transactionLineDTO, Utilities Utilities, TransactionLineDTO parentTrxLineDTO = null)
        {
            log.LogMethodEntry(transactionLineDTO, Utilities, parentTrxLineDTO);
            string message = "";
            bool AutoGenCard = false;
            String newCardNumber = "";
            Card CurrentCard = null;
            TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
            CommonFuncs CommonFuncs = new CommonFuncs(Utilities);
            int product_id = transactionLineDTO.ProductId;
            decimal ProductQuantity = transactionLineDTO.Quantity == null ? 0 : Convert.ToDecimal(transactionLineDTO.Quantity.ToString());
            TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(Utilities.ExecutionContext);

            try
            {
                Semnox.Parafait.Product.Products productBL = new Semnox.Parafait.Product.Products(product_id);
                if (productBL.GetProductsDTO == null || productBL.GetProductsDTO.ProductId == -1)
                {
                    log.Info("Product not found");
                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 111));
                }

                //ProductsDTO parentProduct = null;
                ProductsContainerDTO parentProductContainerDTO = null;
                int parentProductIdForValidation = -1;
                if (parentTrxLineDTO != null)
                {
                    //Semnox.Parafait.Product.Products parentProductBL = new Semnox.Parafait.Product.Products(parentTrxLineDTO.ProductId);
                    //parentProduct = parentProductBL.GetProductsDTO;
                    parentProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(Utilities.ExecutionContext, parentTrxLineDTO.ProductId);
                    parentProductIdForValidation = parentProductContainerDTO.ProductId;
                }

                ProductsDTO Product = productBL.GetProductsDTO;
                if (!Product.ProductType.Equals("DEPOSIT", StringComparison.InvariantCultureIgnoreCase) &&
                    !IsValidProduct(Utilities.ExecutionContext.SiteId, product_id, parentProductIdForValidation, Utilities.ExecutionContext.POSMachineName))
                {
                    log.Info("Product not found");
                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 111));
                }

                string productType = Product.ProductType;
                double productLinePrice = -1;
                bool overridePrice = Product.AllowPriceOverride.Equals("Y");

                // If override flag is enabled, accept the price sent by client
                if (overridePrice)
                {
                    productLinePrice = (transactionLineDTO.Price != null ? (double)transactionLineDTO.Price : -1)
                                        + (transactionLineDTO.TaxAmount != null ? (double)transactionLineDTO.TaxAmount : 0);
                }

                // special handling for combo
                if (productType == "COMBO")
                {
                    // For combo, if the product price is set as -1, then the price is calculated at child levels. So the parent line price is 0
                    if (Product.Price == -1)
                    {
                        productLinePrice = 0;
                    }
                }

                // special handling for combo child
                if (parentTrxLineDTO != null && parentProductContainerDTO != null && parentProductContainerDTO.ProductType == "COMBO")
                {
                    // If the parent line has price, set price of the current line to 0
                    if (parentTrxLineDTO.Price > 0)//check this
                    {
                        productLinePrice = 0;
                    }
                    else if (parentProductContainerDTO.ComboProductContainerDTOList != null &&
                        parentProductContainerDTO.ComboProductContainerDTOList.FirstOrDefault(x => x.ChildProductId == product_id) != null)
                    {
                        double? comboLinePrice = parentProductContainerDTO.ComboProductContainerDTOList.FirstOrDefault(x => x.ChildProductId == product_id).Price;
                        productLinePrice = comboLinePrice != null ? Convert.ToDouble(comboLinePrice) : -1;
                    }
                }

                if (!String.IsNullOrEmpty(transactionLineDTO.CardNumber))
                {
                    CurrentCard = new Card(transactionLineDTO.CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                }

                if ((productType == "RECHARGE" || productType == "VARIABLECARD") && (CurrentCard == null))
                {
                    log.Info("Ends-CreateProduct() as CurrentCard == null , need to Tap Card");
                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 257));
                }

                if (Utilities.ParafaitEnv.CARD_MANDATORY_FOR_TRANSACTION == "Y"
                    && (productType == "MANUAL" || productType == "COMBO" || productType == "ATTRACTION")
                    && (CurrentCard == null || CurrentCard.CardStatus == "NEW"))
                {
                    log.Info("Ends-CreateProduct() as Valid Card Mandatory for Transaction");
                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 222));
                }

                if ((productType == "NEW" || productType == "CARDSALE" || productType == "GAMETIME" || productType == "CHECK-IN" || productType == "ATTRACTION")
                        && CurrentCard == null)
                {
                    RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                    string cardNum = "";
                    if (parentProductContainerDTO != null && parentProductContainerDTO.LoadToSingleCard)
                    {
                        log.Debug("Parent card load to single is true");
                        cardNum = newTrx.GetConsolidateCardFromTransaction(Product, parentProductContainerDTO.ProductId, parentProductContainerDTO.LoadToSingleCard);
                        log.Debug("Parent card load to single is true " + cardNum);
                    }
                    else if (Product.LoadToSingleCard == true)
                    {
                        log.Debug("Product card load to single is true");
                        if (parentProductContainerDTO != null)
                            cardNum = newTrx.GetConsolidateCardFromTransaction(Product, parentProductContainerDTO.ProductId, parentProductContainerDTO.LoadToSingleCard);
                        else
                            cardNum = newTrx.GetConsolidateCardFromTransaction(Product);
                        log.Debug("Product card load to single is true " + cardNum);
                    }

                    if (String.IsNullOrEmpty(cardNum))
                    {
                        cardNum = Product.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1);
                    }

                    CurrentCard = new Card(cardNum, Utilities.ParafaitEnv.LoginID, Utilities);
                    AutoGenCard = true;
                    newCardNumber = cardNum;
                }

                if (Product.OnlyForVIP.ToString() == "Y")
                {
                    if (CurrentCard == null || CurrentCard.vip_customer == 'N')
                    {
                        log.Info("Ends-CreateProduct() as Product available only for VIP");
                        throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 223));
                    }
                }

                if (!CommonFuncs.checkForExceptionProduct((CurrentCard == null ? "" : CurrentCard.CardNumber), product_id))
                {
                    //Added additional check to see if Primary card is a member card. Only if this fails then message is shown - 16-Sep-2015
                    if (!CommonFuncs.checkForExceptionProduct((newTrx.PrimaryCard == null ? "" : newTrx.PrimaryCard.CardNumber), product_id))
                    {
                        log.Info("Ends-CreateProduct() as Selected Product is not allowed for this Card");
                        throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 224));
                    }
                    //End additional check to see if Primary card is a member card - 16-Sep-2015
                }

                Transaction.TransactionLine parentTransactionLine = null;
                if (parentTrxLineDTO != null)
                {
                    parentTransactionLine = newTrx.TrxLines.FirstOrDefault(x => x.guid == parentTrxLineDTO.ClientGuid);
                }

                if (productType == "VARIABLECARD")
                {
                    log.Info("CreateProduct() - VARIABLECARD");
                    var varAmount = transactionLineDTO.Amount == null ? 0 : Convert.ToDouble(transactionLineDTO.Amount.ToString());
                    if (varAmount > 0)
                    {
                        if (ParafaitDefaultContainerList.GetParafaitDefault<bool>(Utilities.ExecutionContext, "ALLOW_DECIMALS_IN_VARIABLE_RECHARGE") == false)
                        {
                            if (varAmount != Math.Round(varAmount, 0))
                            {
                                log.Info(MessageContainerList.GetMessage(Utilities.ExecutionContext, 932) + " Decimals not allowed for variable recharge.");
                                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 932));
                            }
                        }
                        double maxAmount = 100;
                        try
                        {
                            String maxAmountValue = Utilities.getParafaitDefaults("MAX_VARIABLE_RECHARGE_AMOUNT");
                            maxAmount = Convert.ToDouble(string.IsNullOrEmpty(maxAmountValue) ? "0" : maxAmountValue);
                        }
                        catch { }
                        if (varAmount > maxAmount)
                        {
                            log.Info("Ends-CreateProduct() - VARIABLECARD as Maximum allowed amount is " + maxAmount);
                            throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 930));
                        }

                        Transaction.TransactionLine outTransactionLine = new Transaction.TransactionLine();
                        outTransactionLine.guid = transactionLineDTO.ClientGuid;
                        if (newTrx.createTransactionLine(CurrentCard, product_id, varAmount, 1, parentTransactionLine, ref message, outTransactionLine) != 0)
                        {
                            log.Info("Failed: " + message);
                            throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, message));
                        }

                    }
                }
                else if (productType == "VOUCHER")
                {
                    log.Info("CreateProduct() - VOUCHER ");
                    ProductDiscountsListBL productDiscountsListBL = new ProductDiscountsListBL(Utilities.ExecutionContext);
                    List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>> searchProductDiscountsParams = new List<KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>>();
                    searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.PRODUCT_ID, product_id.ToString()));
                    searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.SITE_ID, Utilities.ExecutionContext.GetSiteId().ToString()));
                    searchProductDiscountsParams.Add(new KeyValuePair<ProductDiscountsDTO.SearchByParameters, string>(ProductDiscountsDTO.SearchByParameters.IS_ACTIVE, "Y"));
                    List<ProductDiscountsDTO> productDiscountsDTOList = productDiscountsListBL.GetProductDiscountsDTOList(searchProductDiscountsParams);

                    if (productDiscountsDTOList != null && productDiscountsDTOList.Count == 1)
                    {
                        ProductDiscountsDTO productDiscountsDTO = productDiscountsDTOList[0];
                        if (productDiscountsDTO.DiscountId != -1)
                        {
                            DiscountCouponsHeaderListBL discountCouponsHeaderListBL = new DiscountCouponsHeaderListBL(executionContext);
                            List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>> searchDiscountCouponsHeaderParams = new List<KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>>();
                            searchDiscountCouponsHeaderParams.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.DISCOUNT_ID, productDiscountsDTO.DiscountId.ToString()));
                            searchDiscountCouponsHeaderParams.Add(new KeyValuePair<DiscountCouponsHeaderDTO.SearchByParameters, string>(DiscountCouponsHeaderDTO.SearchByParameters.SITE_ID, (Utilities.ParafaitEnv.IsCorporate ? Utilities.ParafaitEnv.SiteId : -1).ToString()));
                            List<DiscountCouponsHeaderDTO> discountCouponsHeaderDTOList = discountCouponsHeaderListBL.GetDiscountCouponsHeaderDTOList(searchDiscountCouponsHeaderParams);
                            if (discountCouponsHeaderDTOList != null && discountCouponsHeaderDTOList.Count > 0)
                            {
                                DateTime startDate;
                                if (newTrx.EntitlementReferenceDate != DateTime.MinValue)
                                {
                                    startDate = newTrx.EntitlementReferenceDate;
                                }
                                else
                                {
                                    startDate = Utilities.getServerTime();
                                }
                                int businessStartTime;
                                if (int.TryParse(Utilities.getParafaitDefaults("BUSINESS_DAY_START_TIME"), out businessStartTime) == false)
                                {
                                    businessStartTime = 6;
                                }
                                if (startDate.Hour < businessStartTime)
                                {
                                    startDate = startDate.AddDays(-1).Date;
                                }
                                if (discountCouponsHeaderDTOList[0].EffectiveDate != null &&
                                    startDate.Date < discountCouponsHeaderDTOList[0].EffectiveDate.Value.Date)
                                {
                                    startDate = discountCouponsHeaderDTOList[0].EffectiveDate.Value.Date;
                                }
                                ProductDiscountsBL productDiscountsBL = new ProductDiscountsBL(executionContext, productDiscountsDTO);
                                DateTime expiryDate = GetExpiryDateForCoupons(startDate, productDiscountsBL.ProductDiscountsDTO, discountCouponsHeaderDTOList[0]);
                                if (expiryDate < Utilities.getServerTime().Date)
                                {
                                    log.Info("Ends-CreateProduct() - VOUCHER as Coupons can''t be issued through this product as the discount coupon header is expired.");
                                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1225));
                                }
                                List<DiscountCouponsDTO> discountCouponsDTOList = null;
                                //DiscountCouponIssueUI discountCouponIssueUI = new DiscountCouponIssueUI(Utilities, discountCouponsHeaderDTOList[0], startDate, expiryDate);
                                //if (discountCouponIssueUI.ShowDialog() == DialogResult.OK)
                                //{
                                //    discountCouponsDTOList = discountCouponIssueUI.DiscountCouponsDTOList;
                                //}
                                if (discountCouponsDTOList == null ||
                                    discountCouponsDTOList.Count == 0 ||
                                    discountCouponsDTOList.Count < discountCouponsHeaderDTOList[0].Count)
                                {
                                    message = "VOUCHER as User didn't generate the coupons.";
                                    log.Info("Ends-CreateProduct() - VOUCHER as User didn't generate the coupons.");
                                    throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, message));
                                }
                                Transaction.TransactionLine line = new Transaction.TransactionLine();
                                if (0 == newTrx.createTransactionLine(null, product_id, -1, 1, ref message, line))
                                {
                                    line.IssuedDiscountCouponsDTOList = discountCouponsDTOList;
                                }
                                else
                                {
                                    ProductQuantity = 0;
                                    //return null;
                                }
                            }
                            else
                            {
                                log.Info("Ends-CreateProduct() - VOUCHER as Discount associated with product doesn't support coupons. Please check the setup.");
                                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1220));
                            }
                        }
                        else
                        {
                            log.Info("Ends-CreateProduct() - VOUCHER as A valid Discount is not associated with voucher product. Please check the setup.");
                            throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1221));
                        }
                    }
                    else
                    {
                        log.Info("Invalid Discount Setup. Please check the setup.");
                        throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, 1222));
                    }
                }
                else if (productType == "MANUAL")
                {
                    log.Info("CreateProduct() - MANUAL ");

                    Transaction.TransactionLine outTransactionLine = new Transaction.TransactionLine();
                    outTransactionLine.guid = transactionLineDTO.ClientGuid;

                    if (0 != newTrx.createTransactionLine(null, product_id, productLinePrice, ProductQuantity, parentTransactionLine, ref message, outTransactionLine))
                        throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, message));

                    List<TransactionLineDTO> childProducts = transactionDTO.TransactionLinesDTOList.Where
                        (x => !String.IsNullOrEmpty(x.ParentLineGuid) && x.ParentLineGuid.Equals(transactionLineDTO.Guid)).ToList();
                    if (childProducts != null && childProducts.Any())
                    {
                        foreach (TransactionLineDTO childLine in childProducts)
                        {
                            message += CreateTransactionLine(childLine, newTrx.Utilities, transactionLineDTO);
                        }
                    }
                }
                else if (productType == "COMBO")
                {
                    log.Info("CreateProduct() - COMBO ");
                    Transaction.TransactionLine outTransactionLine = new Transaction.TransactionLine();
                    outTransactionLine.guid = transactionLineDTO.ClientGuid;
                    if (0 != newTrx.createTransactionLine(null, product_id, productLinePrice, ProductQuantity, parentTransactionLine, ref message, outTransactionLine))
                        throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, message));

                    // reset the price
                    transactionLineDTO.Price = Convert.ToDecimal(outTransactionLine.Price.ToString());

                    List<TransactionLineDTO> childProducts = transactionDTO.TransactionLinesDTOList.Where
                        (x => !String.IsNullOrEmpty(x.ParentLineGuid) && x.ParentLineGuid.Equals(transactionLineDTO.ClientGuid)).ToList();
                    if (childProducts != null && childProducts.Any())
                    {
                        foreach (TransactionLineDTO childLine in childProducts)
                        {
                            //if (outTransactionLine.Price > 0)//check this
                            //{
                            //    childLine.Price = 0;
                            //    if (childLine.AttractionBookingDTO != null)
                            //        childLine.AttractionBookingDTO.Price = 0;
                            //}
                            message += CreateTransactionLine(childLine, newTrx.Utilities, transactionLineDTO);
                        }
                    }
                }
                else if (productType == "ATTRACTION")
                {
                    log.Info("CreateProduct() - ATTRACTION ");
                    if (transactionLineDTO.AttractionBookingDTO == null)
                    {
                        log.Info("Failed to create transaction as attraction booking object is not found" + newTrx);
                        throw new ValidationException(MessageContainerList.GetMessage(newTrx.Utilities.ExecutionContext, 2097));
                    }

                    log.Debug("ATB Price " + transactionLineDTO.AttractionBookingDTO.Price);
                    double atbPrice = transactionLineDTO.AttractionBookingDTO.Price;
                    AttractionBooking atb = new AttractionBooking(newTrx.Utilities.ExecutionContext, transactionLineDTO.AttractionBookingDTO.BookingId);
                    if (atb != null && atb.AttractionBookingDTO != null)
                    {
                        if (atb.AttractionBookingDTO.ExpiryDate < atb.AttractionBookingDTO.ScheduleFromDate)
                            atb.AttractionBookingDTO.ExpiryDate = atb.AttractionBookingDTO.ScheduleFromDate.AddMinutes(atb.GetBookingCompletionTimeLimit());

                        // Reset price of ATB as data handler, sets this to 0
                        atb.AttractionBookingDTO.Price = atbPrice;

                        Card attractionCard = null;
                        bool createNewCard = true;
                        if (Product.LoadToSingleCard)
                        {
                            String existingCard = newTrx.GetConsolidateCardFromTransaction(Product, parentTransactionLine != null ? parentTransactionLine.ProductID : -1, parentTransactionLine != null ? parentProductContainerDTO.LoadToSingleCard : false);
                            if (!String.IsNullOrEmpty(existingCard))
                            {
                                attractionCard = new Card(existingCard, newTrx.Utilities.ExecutionContext.GetUserId(), newTrx.Utilities);
                                createNewCard = false;
                            }
                        }

                        if (createNewCard)
                        {
                            if (CurrentCard != null)
                                attractionCard = CurrentCard;
                            else
                            {
                                RandomTagNumber randomTagNumber = new RandomTagNumber(Utilities.ExecutionContext, tagNumberLengthList);
                                attractionCard = new Card(Product.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1),
                                 newTrx.Utilities.ExecutionContext.GetUserId(), newTrx.Utilities);
                            }
                        }

                        List<Card> cardList = new List<Card>();
                        cardList.Add(attractionCard);
                        atb.cardList = cardList;

                        if (Product.CardSale.Equals("Y") == false)
                        {
                            if (atb.cardList != null)
                                atb.cardList = null;
                        }

                        try
                        {
                            double businessDayStartTime = !String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BUSINESS_DAY_START_TIME")) ? ParafaitDefaultContainerList.GetParafaitDefault<double>(executionContext, "BUSINESS_DAY_START_TIME") : 6;
                            TimeZoneUtil timeZoneUtil = new TimeZoneUtil();

                            DateTime scheduleDate = atb.AttractionBookingDTO.ScheduleFromDate;
                            int offSetDuration = 0;
                            scheduleDate = scheduleDate.Date.AddHours(businessDayStartTime);
                            offSetDuration = timeZoneUtil.GetOffSetDuration(atb.AttractionBookingDTO.SiteId, scheduleDate);

                            if (atb.AttractionBookingDTO.ExpiryDate < newTrx.Utilities.getServerTime())
                            {
                                log.Debug("atb.AttractionBookingDTO.ExpiryDate : " + atb.AttractionBookingDTO.ExpiryDate);
                                log.Debug(" newTrx.Utilities.getServerTime()e : " + newTrx.Utilities.getServerTime());
                                message = "Attraction Booking has expired";
                                log.Info(message);
                                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, message));
                            }

                            atb.AttractionBookingDTO.AvailableUnits = atb.AttractionBookingDTO.AvailableUnits == null ? 0 : atb.AttractionBookingDTO.AvailableUnits;

                            int parentineId = -1;
                            if (parentTrxLineDTO != null)
                            {
                                for (int i = 0; i < newTrx.TrxLines.Count; i++)
                                {
                                    if (newTrx.TrxLines[i].Guid == parentTransactionLine.Guid)
                                    {
                                        parentineId = i;
                                        break;
                                    }
                                }
                            }

                            double price = productLinePrice;
                            //price = transactionLineDTO.Price == null ? -1 : Convert.ToDouble(transactionLineDTO.Price);
                            //if (Product.AllowPriceOverride.Equals("Y"))
                            //{
                            //    // In combo scenarios, the child product maynot have price overridden flag but will should be overridden if combo price is set
                            //    double linePrice = -1;
                            //    if (parentTransactionLine != null && parentTransactionLine.Price > 0)
                            //    {
                            //        linePrice = 0;
                            //    }

                            //    log.Debug("taking poverridden price");
                            //    price = transactionLineDTO.Price == null ? linePrice : Convert.ToDouble(transactionLineDTO.Price);
                            //}
                            //else
                            //{
                            //    // In combo scenarios, the child product maynot have price overridden flag but will should be overridden if combo price is set
                            //    if (parentTransactionLine != null && parentTransactionLine.Price > 0)
                            //    {
                            //        price = 0;
                            //    }

                            //    log.Debug("Price override not allowed, reset to 1");
                            //    transactionLineDTO.Price = (decimal)price;
                            //}
                            // reset the atb price. Setting the slot price is no longer available. Setup has to be done as a promo 
                            atb.AttractionBookingDTO.Price = -1;
                            log.Debug("Attraction Price is " + transactionLineDTO.Price + ":" + atbPrice + ":" + price + ":" + transactionLineDTO.AttractionBookingDTO.Price);

                            if (newTrx.CreateAttractionProduct(Product.ProductId, productLinePrice, 1,
                                                                parentineId, atb, atb.cardList, ref message) != 0)
                            {
                                log.Error("Failed to create attraction " + message);
                                log.Info("Failed to create transaction " + newTrx);
                                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, message));
                            }

                            atb = newTrx.TrxLines[newTrx.TrxLines.Count - 1].LineAtb;
                            log.Debug("Before offset & offset is " + offSetDuration);
                            log.Debug(atb.AttractionBookingDTO.BookingId + ":" + atb.AttractionBookingDTO.ScheduleFromDate + ":" + atb.AttractionBookingDTO.ScheduleToDate);

                            // The ATB is built from DB - So it contains HQ time - already adjusted for offset, remove that from the time.
                            atb.AttractionBookingDTO.ScheduleFromDate = atb.AttractionBookingDTO.ScheduleFromDate.AddSeconds(-1 * offSetDuration);
                            atb.AttractionBookingDTO.ScheduleToDate = atb.AttractionBookingDTO.ScheduleToDate.AddSeconds(-1 * offSetDuration);
                            atb.AttractionBookingDTO.ExpiryDate = atb.AttractionBookingDTO.ExpiryDate.AddSeconds(-1 * offSetDuration);

                            log.Debug("After offset" + atb.AttractionBookingDTO.BookingId + ":" + atb.AttractionBookingDTO.ScheduleFromDate + ":" + atb.AttractionBookingDTO.ScheduleToDate);
                        }
                        catch (Exception ex)
                        {
                            log.Info("Failed to create transaction " + newTrx);
                            throw;
                        }
                    }
                    else
                    {
                        log.Info("Failed to create transaction as attraction booking object is not found" + newTrx);
                        throw new ValidationException(MessageContainerList.GetMessage(newTrx.Utilities.ExecutionContext, 2097));
                    }
                }
                else if (productType == "CARDDEPOSIT")
                {
                    // do nothing
                }
                else
                {
                    int cardCount = Product.CardCount;
                    decimal quantity = transactionLineDTO.Quantity != null ? Convert.ToDecimal(transactionLineDTO.Quantity.ToString()) : 0;
                    while (quantity > 0)
                    {
                        Transaction.TransactionLine outTransactionLine = new Transaction.TransactionLine();
                        outTransactionLine.guid = transactionLineDTO.ClientGuid;
                        if ((productType == "NEW" || productType == "CARDSALE" || productType == "GAMETIME"))
                        {
                            if (CurrentCard != null && newTrx.customerDTO != null)
                                CurrentCard.customerDTO = newTrx.customerDTO;
                            if (newTrx.createTransactionLine(CurrentCard, product_id, productLinePrice,
                                1, parentTransactionLine, ref message, outTransactionLine, true, -1, transactionLineDTO.SubscriptionHeaderDTO) != 0)
                            {
                                log.Info("Failed to create transaction " + newTrx);
                                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, message));
                            }
                        }
                        else
                        {
                            if (CurrentCard != null && newTrx.customerDTO != null)
                                CurrentCard.customerDTO = newTrx.customerDTO;
                            if (newTrx.createTransactionLine(CurrentCard, product_id, productLinePrice,
                                1, parentTransactionLine, ref message, outTransactionLine, true, -1, transactionLineDTO.SubscriptionHeaderDTO) != 0)
                            {
                                log.Info("Failed to create transaction " + newTrx);
                                throw new ValidationException(MessageContainerList.GetMessage(Utilities.ExecutionContext, message));
                            }
                        }
                        quantity = quantity - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                log.Fatal("Ends-CreateProduct() due to exception " + ex.Message);
                throw;
            }
            finally
            {
                if (AutoGenCard)
                    CurrentCard = null;
                log.Debug("Ends-CreateProduct() ");
            }

            return message;
        }

        /// <summary>
        /// Returns the coupon expiry date
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="discountCouponsHeaderDTO"></param>
        /// <returns></returns>
        public DateTime GetExpiryDateForCoupons(DateTime startDate, ProductDiscountsDTO productDiscountsDTO, DiscountCouponsHeaderDTO discountCouponsHeaderDTO)
        {
            log.LogMethodEntry(startDate, discountCouponsHeaderDTO);
            DateTime expiryDate = startDate;
            if (productDiscountsDTO.ValidFor != null)
            {
                if (productDiscountsDTO.ValidForDaysMonths == "D")
                {
                    expiryDate = startDate.Date.AddDays(Convert.ToInt32(productDiscountsDTO.ValidFor));
                }
                else
                {
                    expiryDate = startDate.Date.AddMonths(Convert.ToInt32(productDiscountsDTO.ValidFor));
                }
                if (productDiscountsDTO.ExpiryDate != null)
                {
                    if (expiryDate.Date > productDiscountsDTO.ExpiryDate.Value.Date)
                    {
                        expiryDate = productDiscountsDTO.ExpiryDate.Value.Date;
                    }
                }
            }
            else if (productDiscountsDTO.ExpiryDate != null)
            {
                expiryDate = productDiscountsDTO.ExpiryDate.Value.Date;
            }
            else if (discountCouponsHeaderDTO.ExpiresInDays != null)
            {
                expiryDate = startDate.Date.AddDays(Convert.ToInt32(discountCouponsHeaderDTO.ExpiresInDays));
                if (discountCouponsHeaderDTO.ExpiryDate != null)
                {
                    if (expiryDate.Date > discountCouponsHeaderDTO.ExpiryDate.Value.Date)
                    {
                        expiryDate = discountCouponsHeaderDTO.ExpiryDate.Value.Date;
                    }
                }
            }
            else if (discountCouponsHeaderDTO.ExpiryDate != null)
            {
                expiryDate = discountCouponsHeaderDTO.ExpiryDate.Value.Date;
            }
            log.LogMethodExit(expiryDate);
            return expiryDate;
        }

        /// <summary>
        /// Execute - Create a transaction by applying the given payment details
        /// </summary>
        public TransactionDTO Execute()
        {
            String message = "";
            Utilities Utilities = GetUtilities();

            if (Utilities.ParafaitEnv.IsCorporate)
            {
                string getDepositProductQuery = @"select product_id productId
                                                from products x, product_type y
                                                where x.product_type_id = y.product_type_id
                                                    and y.product_type = 'CARDDEPOSIT'
                                                    and x.site_Id = @siteId";

                List<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@siteId", Utilities.ParafaitEnv.SiteId.ToString()));
                DataTable dtDepositProduct = (new DataAccessHandler().executeSelectQuery(getDepositProductQuery, sqlParams.ToArray()));

                int depositProductId;
                try
                {
                    depositProductId = Convert.ToInt32(dtDepositProduct.Rows[0]["productId"]);
                    Utilities.ParafaitEnv.CardDepositProductId = depositProductId;
                }
                catch (Exception ex)
                {
                    log.Error("Deposit product setup is incorrect. Please set it up properly. " + ex.Message);
                }
            }

            if (transactionDTO.TransactionId > -1 && transactionDTO.SaveTransaction && (transactionDTO.CommitTransaction || transactionDTO.CloseTransaction))
                throw new ValidationException("Invalid control flags in transaction object");

            if (transactionDTO.TransactionId > -1 && (transactionDTO.SaveTransaction || transactionDTO.ReverseTransaction))
            {
                try
                {
                    // For reverse transaction fix - Userd was -1 in the utilities.
                    if (newTrx != null)
                        newTrx.Utilities = Utilities;
                    if (newTrx == null)
                    {
                        TransactionUtils trxUtils = new TransactionUtils(Utilities);
                        newTrx = trxUtils.CreateTransactionFromDB(transactionDTO.TransactionId, Utilities);
                    }
                    if (newTrx == null)
                    {
                        throw new ValidationException("Transaction Not Found");
                    }
                    newTrx.Utilities = Utilities;
                    Save();
                    return transactionDTO;
                }
                catch (Exception ex)
                {
                    log.Error("Transaction Save failed" + ex.Message);
                    throw;
                }
            }
            else if (transactionDTO.CommitTransaction || transactionDTO.CloseTransaction)
            {
                try
                {
                    if (transactionDTO.TransactionId > 0)
                    {
                        TransactionUtils trxUtils = new TransactionUtils(Utilities);
                        newTrx = trxUtils.CreateTransactionFromDB(transactionDTO.TransactionId, Utilities);
                        if (newTrx == null)
                        {
                            throw new ValidationException("Transaction Not Found");
                        }
                        newTrx.Utilities = Utilities;
                    }

                    // refresh the transaction header as the header information might be updated in successive calls
                    CreateRefreshTransactionHeader(Utilities);

                    if (transactionDTO.TransactionLinesDTOList != null)
                    {
                        foreach (TransactionLineDTO transactionLine in transactionDTO.TransactionLinesDTOList)
                        {
                            if (transactionLine.LineId == -1 && String.IsNullOrEmpty(transactionLine.ParentLineGuid))
                            {
                                message += CreateTransactionLine(transactionLine, Utilities);
                            }
                        }
                    }

                    ApplyTransactionDiscount();

                    message += SaveTransaction(Utilities);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    throw;
                }
            }
            else
            {
                try
                {
                    CreateRefreshTransactionHeader(Utilities);

                    foreach (TransactionLineDTO transactionLine in transactionDTO.TransactionLinesDTOList)
                    {
                        if (transactionLine.LineId == -1 && String.IsNullOrEmpty(transactionLine.ParentLineGuid))
                        {
                            message += CreateTransactionLine(transactionLine, Utilities);
                        }
                    }

                    ApplyTransactionDiscount();
                }
                catch (Exception ex)
                {
                    log.Info(ex.Message);
                    throw;
                }
            }
            if (newTrx != null)
            {
                log.LogMethodExit("Success");
                TransactionDTO createdTransactionDTO = newTrx.TransactionDTO;
                // Add the cancelled discounts also, else the cancelled discounts which have auto apply flag as true will be re-applied when the transactionn is sent back for processing
                if (transactionDTO.DiscountApplicationHistoryDTOList != null && transactionDTO.DiscountApplicationHistoryDTOList.Any())
                {
                    if (createdTransactionDTO.DiscountApplicationHistoryDTOList == null)
                        createdTransactionDTO.DiscountApplicationHistoryDTOList = new List<DiscountApplicationHistoryDTO>();
                    createdTransactionDTO.DiscountApplicationHistoryDTOList.AddRange(transactionDTO.DiscountApplicationHistoryDTOList.Where(x => x.IsCancelled).ToList());
                }
                log.LogMethodExit(createdTransactionDTO);
                return createdTransactionDTO;
            }
            else
            {
                throw new Exception(message);
            }
        }

        private Utilities GetUtilities()
        {
            log.LogMethodEntry();
            string errorMessage = string.Empty;
            bool isCorporate = this.executionContext.GetIsCorporate();

            Users user = null;
            if (executionContext.GetSiteId() == (isCorporate ? transactionDTO.SiteId : -1))
            {
                log.Debug("Site of execution context and site is same, so build user from that");
                user = new Users(executionContext, executionContext.GetUserId(), executionContext.GetSiteId());
            }
            else
            {
                log.Debug("Site of execution context and site is not same, so build user from trx site");
                user = new Users(executionContext, executionContext.GetUserId(), (isCorporate ? transactionDTO.SiteId : -1));
            }

            if (String.IsNullOrEmpty(user.UserDTO.LoginId))
            {
                errorMessage = "Please setup the user " + this.executionContext.GetUserId() + ":" + this.executionContext.GetSiteId();
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }

            List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, Convert.ToString(isCorporate ? transactionDTO.SiteId : -1)));
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, transactionDTO.PosMachine));
            searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
            POSMachineList pOSMachineList = new POSMachineList(this.executionContext);
            List<POSMachineDTO> content = pOSMachineList.GetAllPOSMachines(searchParameters, false, false);
            if (content == null || !content.Any())
            {
                errorMessage = "POS Machine " + transactionDTO.PosMachine + " is not set up.";
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }

            Utilities Utilities = new Utilities();
            Utilities.ParafaitEnv.SiteId = transactionDTO.SiteId;
            Utilities.ParafaitEnv.LoginID = user.UserDTO.LoginId;
            Utilities.ParafaitEnv.User_Id = user.UserDTO.UserId;
            Utilities.ParafaitEnv.RoleId = user.UserDTO.RoleId;
            Utilities.ParafaitEnv.POSMachineId = content[0].POSMachineId;
            Utilities.ParafaitEnv.POSMachine = content[0].POSName;
            Utilities.ParafaitEnv.POSMachineGuid = content[0].Guid;
            Utilities.ParafaitEnv.IsCorporate = isCorporate;
            Utilities.ExecutionContext.SetIsCorporate(isCorporate);
            Utilities.ExecutionContext.SetMachineId(content[0].POSMachineId);
            Utilities.ParafaitEnv.Initialize();
            log.LogMethodExit();
            return Utilities;
        }

        private string SaveTransaction(Utilities Utilities, SqlTransaction SQLTrx = null)
        {

            log.LogMethodEntry(newTrx, SQLTrx);
            DateTime now = DateTime.Now;
            string message = String.Empty;

            MessageUtils MessageUtils = Utilities.MessageUtils;
            TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
            CommonFuncs CommonFuncs = new CommonFuncs(Utilities);
            TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(Utilities.ExecutionContext);

            int offsetTimeSecs = 0;
            if (executionContext.GetIsCorporate())
            {
                TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                offsetTimeSecs = timeZoneUtil.GetOffSetDuration(executionContext.GetSiteId(), newTrx.TrxDate.Date);
            }

            if (newTrx == null)
            {
                log.Info("Ends-saveTrx() as NewTrx == null");
                message = "Transaction object is null";
                throw new ValidationException("Transaction object is null");
            }

            bool onDemandRoamingEnabled = false;
            if (newTrx.Utilities.getParafaitDefaults("ENABLE_ON_DEMAND_ROAMING").Equals("Y")
                        && newTrx.Utilities.getParafaitDefaults("AUTOMATIC_ON_DEMAND_ROAMING").Equals("Y")
                        && newTrx.Utilities.getParafaitDefaults("ALLOW_ROAMING_CARDS").Equals("Y")
                        )
            {
                onDemandRoamingEnabled = true;
            }

            if (newTrx != null && newTrx.Trx_id > 0)
                newTrx.InsertTrxLogs(newTrx.Trx_id, -1, newTrx.Utilities.ParafaitEnv.LoginID, "SAVE Begin", "POS Application SAVE Process started", SQLTrx);

            if (newTrx.customerDTO == null && this.transactionDTO.CustomerId != -1)
                newTrx.customerDTO = (new CustomerBL(Utilities.ExecutionContext, this.transactionDTO.CustomerId)).CustomerDTO;

            if (transactionDTO.CustomerIdentifier != null)
            {
                newTrx.customerIdentifier = transactionDTO.CustomerIdentifier;

                if (newTrx.Trx_id <= 0)
                {
                    log.Debug("Customer Identifier " + transactionDTO.CustomerIdentifier);
                    String decryptedCustomerIdentifier = newTrx.customerIdentifier;
                    if (newTrx.customerIdentifier.IndexOf("|") == -1)
                    {
                        try
                        {
                            decryptedCustomerIdentifier = Encryption.Decrypt(newTrx.customerIdentifier);
                            log.Debug("Customer Identifier after decrypt " + newTrx.customerIdentifier);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Failed to decrypt " + newTrx.customerIdentifier);
                            log.Error(ex.Message);
                        }
                    }
                    string[] customerIdentifierStringArray = decryptedCustomerIdentifier.Split(new[] { '|' });
                    newTrx.customerIdentifiersList = new List<string>();
                    foreach (string identifier in customerIdentifierStringArray)
                    {
                        newTrx.customerIdentifiersList.Add(identifier);
                    }
                    log.Debug("Customer Identifier list " + String.Join(":", newTrx.customerIdentifiersList));
                }

            }

            DateTime transactionDate = Utilities.getServerTime();
            if (!string.IsNullOrEmpty(transactionDTO.VisitDate) && transactionDTO.ApplyVisitDate)
            {
                String datejson = "{ \"VisitDate\":\" " + transactionDTO.VisitDate + " \"}";
                try
                {
                    TransactionVisitDate visitDate = JsonConvert.DeserializeObject<TransactionVisitDate>(datejson);
                    transactionDate = visitDate.VisitDate;
                }
                catch (Exception ex)
                {
                    log.Error("Could not convert json to date" + datejson + ex.Message);
                    DateTime visitDateParse = DateTime.MinValue;
                    if (DateTime.TryParse(transactionDTO.VisitDate, out visitDateParse))
                    {
                        log.Debug("Parsed transaction date " + visitDateParse.ToString());
                        transactionDate = visitDateParse;
                    }
                    else
                    {
                        transactionDate = Utilities.getServerTime();
                    }
                }

                log.Error("Transaction date " + transactionDate.ToString());
                transactionDate = transactionDate.Date.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 6));

                // Adding 5 minutes to handle boundry condition
                transactionDate = transactionDate.AddMinutes(5);
                log.Error("Transaction date " + transactionDate.ToString());

                log.Debug("Applying offset to transaction:" + offsetTimeSecs);
                transactionDate = transactionDate.AddSeconds(offsetTimeSecs);
                newTrx.TrxDate = newTrx.EntitlementReferenceDate = transactionDate;
            }
            else if (newTrx.TrxDate == DateTime.MinValue && this.transactionDTO.ApplyOffset)
            {
                if (transactionDTO.TransactionDate != DateTime.MinValue)
                {
                    transactionDate = transactionDTO.TransactionDate.Date;
                    transactionDate = transactionDate.AddHours(ParafaitDefaultContainerList.GetParafaitDefault<int>(Utilities.ExecutionContext, "BUSINESS_DAY_START_TIME", 6));
                }
                log.Debug("Applying offset to transaction:" + offsetTimeSecs);
                transactionDate = transactionDate.AddSeconds(offsetTimeSecs);
                newTrx.TrxDate = newTrx.EntitlementReferenceDate = transactionDate;
            }

            PaymentModeDTO paymentModeDTO = null;
            PaymentModeList paymentModesListBL = new PaymentModeList(newTrx.Utilities.ExecutionContext);
            List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(false);
            if (paymentModesDTOList != null)
                paymentModesDTOList = paymentModesDTOList.Where(x => x.PaymentModeId == transactionDTO.PaymentMode).ToList();
            if (paymentModesDTOList != null && paymentModesDTOList.Any())
            {
                paymentModeDTO = paymentModesDTOList[0];
                log.Debug("Payment mode gateway " + paymentModeDTO.Gateway);
                log.Debug("Payment mode gateway lookup " + paymentModeDTO.GatewayLookUp);
                log.Debug("Payment mode payment gateway " + paymentModeDTO.PaymentGateway);
            }
            else
            {
                //check if payment details already entered.clear out if not or if it is different than current trx
                if (newTrx.Net_Transaction_Amount != newTrx.TotalPaidAmount && transactionDTO.TrxPaymentsDTOList != null)
                {
                    PaymentGatewayFactory.GetInstance().Initialize(newTrx.Utilities, true, null);
                    foreach (TransactionPaymentsDTO trxPaymentDTO in transactionDTO.TrxPaymentsDTOList)
                    {
                        PaymentMode paymentMode = null;

                        if (trxPaymentDTO.PaymentModeId != -1)
                        {
                            paymentMode = new PaymentMode(newTrx.Utilities.ExecutionContext, trxPaymentDTO.PaymentModeId);
                            trxPaymentDTO.paymentModeDTO = paymentMode.GetPaymentModeDTO;
                        }
                        else
                        {
                            log.Error("Invalid Payment Mode");
                            throw new ValidationException("Invalid payment mode");
                        }

                        if (transactionDTO.PaymentProcessingCompleted)
                        {
                            trxPaymentDTO.GatewayPaymentProcessed = true;
                            trxPaymentDTO.paymentModeDTO.GatewayLookUp = PaymentGateways.None;
                            trxPaymentDTO.paymentModeDTO.AcceptChanges();
                        }
                        else
                        {
                            trxPaymentDTO.GatewayPaymentProcessed = false;
                            if (trxPaymentDTO.paymentModeDTO != null)
                            {
                                string gateway = (new PaymentMode(this.executionContext, trxPaymentDTO.paymentModeDTO)).Gateway;
                                if (!string.IsNullOrEmpty(gateway) && Enum.IsDefined(typeof(PaymentGateways), gateway))
                                    trxPaymentDTO.paymentModeDTO.GatewayLookUp = (PaymentGateways)Enum.Parse(typeof(PaymentGateways), gateway);
                            }
                        }

                        if (trxPaymentDTO.paymentModeDTO.IsDebitCard == true) //Debit card payment Ver 1.01
                        {
                            AccountDTO payCard = null;
                            AccountBL payCardBL = null;
                            if (trxPaymentDTO.CardId > -1)
                                payCardBL = new AccountBL(Utilities.ExecutionContext, trxPaymentDTO.CardId, true, true);
                            else if (!String.IsNullOrWhiteSpace(trxPaymentDTO.PaymentCardNumber))
                                payCardBL = new AccountBL(Utilities.ExecutionContext, trxPaymentDTO.PaymentCardNumber, true, true);
                            else
                            {
                                log.Error("Invalid Payment card");
                                throw new ValidationException("Invalid payment card");
                            }

                            if (payCardBL.AccountDTO == null || !payCardBL.AccountDTO.ValidFlag)
                            {
                                log.Error("Invalid Payment card");
                                throw new ValidationException("Invalid payment card");
                            }

                            payCard = payCardBL.AccountDTO;
                            double paymentUsedCredits = 0;
                            double paymentUsedCreditPlus = 0;

                            double creditPlusAmount = (payCard.AccountSummaryDTO.CreditPlusCardBalance != null ? Convert.ToDouble(payCard.AccountSummaryDTO.CreditPlusCardBalance) : 0)
                                + (payCard.AccountSummaryDTO.CreditPlusItemPurchase != null ? Convert.ToDouble(payCard.AccountSummaryDTO.CreditPlusItemPurchase) : 0);

                            double credits = payCard.Credits != null ? Convert.ToDouble(payCard.Credits) : 0;

                            double paymentRequired = trxPaymentDTO.Amount + trxPaymentDTO.PaymentUsedCreditPlus;

                            if ((credits + creditPlusAmount) < paymentRequired) //Trx Amount is more than game card balance
                            {
                                log.Error("Insufficient balance on card " + payCard.TagNumber);
                                throw new Exception("Insufficient balance on card " + payCard.TagNumber);
                            }

                            if (creditPlusAmount >= paymentRequired) //Credit Plus is more than trx amount. So credit plus is deducted
                            {
                                paymentUsedCreditPlus = paymentRequired;
                                paymentUsedCredits = 0;
                            }
                            else //use credit plus and balance to be taken from credits
                            {
                                trxPaymentDTO.CardEntitlementType = "C";
                                paymentUsedCreditPlus = creditPlusAmount;
                                paymentUsedCredits = (paymentRequired - creditPlusAmount);
                            }

                            trxPaymentDTO.Amount = paymentUsedCredits;
                            trxPaymentDTO.PaymentUsedCreditPlus = paymentUsedCreditPlus;
                        }

                        if (newTrx.TransactionPaymentsDTOList == null)
                            newTrx.TransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();

                        newTrx.TransactionPaymentsDTOList.Add(trxPaymentDTO);
                        newTrx.CreateRoundOffPayment();
                    }
                }
            }

            // Attraction bookings have been recast for offset, reverse this before saving
            for (int i = 0; i < newTrx.TrxLines.Count; i++)
            {
                if (newTrx.TrxLines[i].LineAtb != null)
                {
                    log.Debug("Rebuilding atb from db");
                    AttractionBooking atb = new AttractionBooking(Utilities.ExecutionContext, newTrx.TrxLines[i].LineAtb.AttractionBookingDTO.BookingId, true, SQLTrx);
                    log.Debug(atb.AttractionBookingDTO.BookingId + ":" + atb.AttractionBookingDTO.ScheduleFromDate + ":" + atb.AttractionBookingDTO.ScheduleToDate);
                    newTrx.TrxLines[i].LineAtb = atb;
                }
            }

            //If customer is not present in site, put a record to send customer data to site
            if (onDemandRoamingEnabled && newTrx.customerDTO != null)
            {
                if (newTrx.customerDTO.SiteId != newTrx.Utilities.ExecutionContext.SiteId)
                {
                    try
                    {
                        log.Error("Make roaming entry for customer " + newTrx.customerDTO != null ? newTrx.customerDTO.Id + ":" + newTrx.customerDTO.SiteId + ":" + newTrx.Utilities.ExecutionContext.SiteId : "no customer found");
                        log.Error("making roaming entry for customer ");
                    }
                    catch (Exception ex)
                    {

                    }
                    DBSynchLogDTO dbSynchLogDTOProfile = new DBSynchLogDTO("I", newTrx.customerDTO.ProfileDTO.Guid, "Profile", newTrx.Utilities.getServerTime(), newTrx.Utilities.ExecutionContext.SiteId);
                    DBSynchLogBL dbSynchLogBLProfile = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTOProfile);
                    dbSynchLogBLProfile.Save();

                    log.Error("making roaming entry for customer ");
                    DBSynchLogDTO dbSynchLogDTOCustomer = new DBSynchLogDTO("I", newTrx.customerDTO.Guid, "Customers", newTrx.Utilities.getServerTime(), newTrx.Utilities.ExecutionContext.SiteId);
                    DBSynchLogBL dbSynchLogBLCustomer = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTOCustomer);
                    dbSynchLogBLCustomer.Save();
                }
                log.Error("Made customer entry");
            }

            int retcode = 0;
            if (transactionDTO.CloseTransaction)
            {
                retcode = newTrx.SaveTransacation(SQLTrx, ref message);
                if (retcode == 0)
                    newTrx.Status = Transaction.TrxStatus.CLOSED;
            }
            else
            {
                retcode = newTrx.SaveOrder(ref message, SQLTrx);
            }

            if (retcode != 0)
            {
                message = MessageUtils.getMessage(240, message);
                log.Info("saveTrx() in SaveTransacation as retcode != 0 error: " + message + newTrx);
                throw new ValidationException(MessageContainerList.GetMessage(newTrx.Utilities.ExecutionContext, message));
            }
            else
            {
                #region Merkle Integration Code block - Update API call for Coupons used
                if (newTrx.customerDTO != null)
                {
                    if (Utilities.getParafaitDefaults("ENABLE_MERKLE_INTEGRATION").Equals("Y") && newTrx.customerDTO != null)
                    {
                        List<string> couponsList = newTrx.GetCouponsList(newTrx.Trx_id);
                        if (couponsList != null)
                        {
                            try
                            {
                                bool status = UpdateCouponStatus("used", couponsList);
                            }
                            catch (Exception ex)
                            {
                                message = ex.Message;
                                throw;
                            }
                        }
                    }
                }
                #endregion

                try
                {
                    foreach (Transaction.TransactionLine currTrxLine in newTrx.TrxLines)
                    {
                        if (currTrxLine.card != null)
                        {
                            AccountDTO updatedAccountDTO = new AccountBL(newTrx.Utilities.ExecutionContext, currTrxLine.card.card_id, true, true).AccountDTO;
                            DBSynchLogService dBSynchLogService = new DBSynchLogService(newTrx.Utilities.ExecutionContext, "Cards", updatedAccountDTO.Guid, updatedAccountDTO.SiteId);
                            dBSynchLogService.CreateRoamingData();

                            if (onDemandRoamingEnabled)
                            {
                                //account site id does not match with transaction site id. This can happen in virtual store scenario
                                if (updatedAccountDTO.SiteId != newTrx.Utilities.ExecutionContext.GetSiteId())
                                {
                                    DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "Cards", newTrx.Utilities.getServerTime(), newTrx.Utilities.ExecutionContext.GetSiteId());
                                    DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTO);
                                    dbSynchLogBL.Save();
                                }
                            }

                            if (updatedAccountDTO.AccountCreditPlusDTOList != null && updatedAccountDTO.AccountCreditPlusDTOList.Any())
                            {
                                updatedAccountDTO.AccountCreditPlusDTOList = updatedAccountDTO.AccountCreditPlusDTOList.Where(x => x.TransactionId == newTrx.Trx_id).ToList();

                                if (updatedAccountDTO.AccountCreditPlusDTOList != null && updatedAccountDTO.AccountCreditPlusDTOList.Any())
                                {
                                    foreach (AccountCreditPlusDTO updatedAccountCreditPlus in updatedAccountDTO.AccountCreditPlusDTOList)
                                    {
                                        DBSynchLogService dBSynchLogServiceCP = new DBSynchLogService(newTrx.Utilities.ExecutionContext, "CardCreditPlus", updatedAccountCreditPlus.Guid, updatedAccountCreditPlus.SiteId);
                                        dBSynchLogServiceCP.CreateRoamingData();
                                        if (onDemandRoamingEnabled)
                                        {
                                            DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", updatedAccountCreditPlus.Guid, "CardCreditPlus", newTrx.Utilities.getServerTime(), updatedAccountDTO.SiteId);
                                            DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTO);
                                            dbSynchLogBL.Save();
                                        }

                                        if (updatedAccountCreditPlus.AccountCreditPlusConsumptionDTOList != null)
                                        {
                                            foreach (AccountCreditPlusConsumptionDTO updatedAccountCPConsumpDTO in updatedAccountCreditPlus.AccountCreditPlusConsumptionDTOList)
                                            {
                                                DBSynchLogService dBSynchLogServiceCPConsp = new DBSynchLogService(newTrx.Utilities.ExecutionContext, "CardCreditPlusConsumption", updatedAccountCPConsumpDTO.Guid, updatedAccountCPConsumpDTO.SiteId);
                                                dBSynchLogServiceCPConsp.CreateRoamingData();
                                                if (onDemandRoamingEnabled)
                                                {
                                                    DBSynchLogDTO dbSynchLogDTO = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardCreditPlusConsumption", newTrx.Utilities.getServerTime(), updatedAccountDTO.SiteId);
                                                    DBSynchLogBL dbSynchLogBL = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTO);
                                                    dbSynchLogBL.Save();
                                                }
                                            }
                                        }
                                        if (updatedAccountCreditPlus.AccountCreditPlusPurchaseCriteriaDTOList != null)
                                        {
                                            foreach (AccountCreditPlusPurchaseCriteriaDTO updatedAccountCPPCDTO in updatedAccountCreditPlus.AccountCreditPlusPurchaseCriteriaDTOList)
                                            {
                                                DBSynchLogService dBSynchLogServiceCPPC = new DBSynchLogService(newTrx.Utilities.ExecutionContext, "CardCreditPlusPurchaseCriteria", updatedAccountCPPCDTO.Guid, updatedAccountCPPCDTO.SiteId);
                                                dBSynchLogServiceCPPC.CreateRoamingData();
                                                if (onDemandRoamingEnabled)
                                                {
                                                    DBSynchLogDTO dbSynchLogDTOVs = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardCreditPlusPurchaseCriteria", newTrx.Utilities.getServerTime(), updatedAccountDTO.SiteId);
                                                    DBSynchLogBL dbSynchLogBLVs = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTOVs);
                                                    dbSynchLogBLVs.Save();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (updatedAccountDTO.AccountGameDTOList != null && updatedAccountDTO.AccountGameDTOList.Any())
                            {
                                updatedAccountDTO.AccountGameDTOList = updatedAccountDTO.AccountGameDTOList.Where(x => x.TransactionId == newTrx.Trx_id).ToList();

                                if (updatedAccountDTO.AccountGameDTOList != null && updatedAccountDTO.AccountGameDTOList.Any())
                                {
                                    foreach (AccountGameDTO updatedAccountGame in updatedAccountDTO.AccountGameDTOList)
                                    {
                                        DBSynchLogService dBSynchLogServiceGame = new DBSynchLogService(newTrx.Utilities.ExecutionContext, "CardGames", updatedAccountGame.Guid, updatedAccountGame.SiteId);
                                        dBSynchLogServiceGame.CreateRoamingData();
                                        if (onDemandRoamingEnabled)
                                        {
                                            DBSynchLogDTO dbSynchLogDTOVs = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardGames", newTrx.Utilities.getServerTime(), updatedAccountDTO.SiteId);
                                            DBSynchLogBL dbSynchLogBLVs = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTOVs);
                                            dbSynchLogBLVs.Save();
                                        }

                                        if (updatedAccountGame.AccountGameExtendedDTOList != null)
                                        {
                                            foreach (AccountGameExtendedDTO updatedAccountGameExtDTO in updatedAccountGame.AccountGameExtendedDTOList)
                                            {
                                                DBSynchLogService dBSynchLogServiceGameExt = new DBSynchLogService(newTrx.Utilities.ExecutionContext, "CardGameExtended", updatedAccountGameExtDTO.Guid, updatedAccountGameExtDTO.SiteId);
                                                dBSynchLogServiceGameExt.CreateRoamingData();

                                                if (onDemandRoamingEnabled)
                                                {
                                                    DBSynchLogDTO dbSynchLogDTOVs = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardGameExtended", newTrx.Utilities.getServerTime(), updatedAccountDTO.SiteId);
                                                    DBSynchLogBL dbSynchLogBLVs = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTOVs);
                                                    dbSynchLogBLVs.Save();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (updatedAccountDTO.AccountDiscountDTOList != null && updatedAccountDTO.AccountDiscountDTOList.Any())
                            {
                                updatedAccountDTO.AccountDiscountDTOList = updatedAccountDTO.AccountDiscountDTOList.Where(x => x.TransactionId == newTrx.Trx_id).ToList();

                                if (updatedAccountDTO.AccountDiscountDTOList != null && updatedAccountDTO.AccountDiscountDTOList.Any())
                                {
                                    foreach (AccountDiscountDTO updatedAccountDiscount in updatedAccountDTO.AccountDiscountDTOList)
                                    {
                                        DBSynchLogService dBSynchLogServiceDiscount = new DBSynchLogService(newTrx.Utilities.ExecutionContext, "CardDiscounts", updatedAccountDiscount.Guid, updatedAccountDiscount.SiteId);
                                        dBSynchLogServiceDiscount.CreateRoamingData();

                                        if (onDemandRoamingEnabled)
                                        {
                                            DBSynchLogDTO dbSynchLogDTOVs = new DBSynchLogDTO("I", updatedAccountDTO.Guid, "CardDiscounts", newTrx.Utilities.getServerTime(), updatedAccountDTO.SiteId);
                                            DBSynchLogBL dbSynchLogBLVs = new DBSynchLogBL(newTrx.Utilities.ExecutionContext, dbSynchLogDTOVs);
                                            dbSynchLogBLVs.Save();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Create Roaming data failed for Transaction: " + newTrx.Trx_id.ToString(), ex);
                }

                if (newTrx != null)
                    newTrx.InsertTrxLogs(newTrx.Trx_id, -1, newTrx.Utilities.ParafaitEnv.LoginID, "SAVE END", "POS Application SAVE Process ended", SQLTrx);
            }

            try
            {
                String transactionReceipt = "";
                string printReceipt = ParafaitDefaultContainerList.GetParafaitDefault(newTrx.Utilities.ExecutionContext, "ALLOW_TRX_PRINT_BEFORE_SAVING");
                if (printReceipt == "Y" || newTrx.Status == Transaction.TrxStatus.CLOSED)
                {
                    BuildTransactionReceiptAndTickets(true, true);
                }
                //if (newTrx.Status == Transaction.TrxStatus.CLOSED)
                //{
                //    BuildTransactionReceiptAndTickets(true, true);
                //}


                if (printReceipt == "Y" ||
                          (newTrx.Status == Transaction.TrxStatus.CLOSED) && (newTrx.customerDTO != null ||
                          (!string.IsNullOrEmpty(newTrx.customerIdentifier))))
                {
                    if (!string.IsNullOrEmpty(newTrx.customerIdentifier))
                    {
                        log.Debug(newTrx.customerIdentifier + " - " + string.Join(":", newTrx.customerIdentifiersList));
                    }
                    else
                    {
                        log.Debug("Customer Identifier is empty");
                    }

                    try
                    {
                        // newTrx.SendSubscriptionPurchaseMessage(MessagingClientDTO.MessagingChanelType.NONE, SQLTrx);
                        newTrx.SendTransactionPurchaseMessage(MessagingClientDTO.MessagingChanelType.NONE, SQLTrx);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                    }

                    log.LogMethodExit();
                }
            }
            catch (Exception ex)
            {
                log.LogMethodExit("Send Receipt failed " + ex.Message);
            }

            log.LogMethodExit();
            return message;
        }

        public bool UpdateCouponStatus(string couponStatus, List<string> couponsToUpdate)
        {
            log.LogMethodEntry(couponStatus, couponsToUpdate);

            bool retStatus = false;
            if (couponsToUpdate != null)
            {
                for (int d = 0; d < couponsToUpdate.Count; d++)
                {
                    if (couponsToUpdate[d] != null)
                    {
                        retStatus = updateDetails.Update(couponsToUpdate[d].ToString(), couponStatus);

                        if (!retStatus)
                            newTrx.Utilities.EventLog.logEvent("ParafaitDataTransfer", 'E', "Merkle Failed Updating Coupon status -" + couponStatus + " Coupon Number: " + couponsToUpdate[d].ToString(), "", "MerkleAPIIntegration", 1, "", "", newTrx.Utilities.ParafaitEnv.LoginID, newTrx.Utilities.ParafaitEnv.POSMachine, null);
                    }
                }
            }
            log.LogMethodExit(retStatus);
            return retStatus;
        }
        private void ReverseTransaction()
        {
            log.LogMethodEntry();
            String message = "";
            TransactionUtils trxUtils = new TransactionUtils(newTrx.Utilities);
            if (!trxUtils.reverseTransaction(newTrx.Trx_id, -1, true, (newTrx.Utilities.ParafaitEnv.POSMachine == null ? Environment.MachineName : newTrx.Utilities.ParafaitEnv.POSMachine),
                newTrx.Utilities.ParafaitEnv.LoginID, newTrx.Utilities.ParafaitEnv.User_Id, newTrx.Utilities.ParafaitEnv.LoginID, "Reverse LoadTickets: ", ref message))
            {
                log.Error(message);
                throw new Exception(message);
            }
            log.LogMethodExit();
        }



        private void ApplyTransactionDiscount()
        {
            if (transactionDTO.DiscountApplicationHistoryDTOList != null && transactionDTO.DiscountApplicationHistoryDTOList.Any())
            {
                // Applying the active discount
                foreach (DiscountApplicationHistoryDTO discountApplicationHistoryDTO in transactionDTO.DiscountApplicationHistoryDTOList)
                {
                    if (!discountApplicationHistoryDTO.IsCancelled)
                    {
                        Transaction.TransactionLine trxLine = null;
                        if (!String.IsNullOrEmpty(discountApplicationHistoryDTO.TransactionLineGuid))
                        {
                            // do something to get trxLIne
                            List<Transaction.TransactionLine> tempList = newTrx.TrxLines.Where(x => (x.Guid == discountApplicationHistoryDTO.TransactionLineGuid)).ToList();
                            if (tempList != null && tempList.Any())
                            {
                                trxLine = tempList[0];
                            }
                        }

                        if (!IsDiscountApplied(discountApplicationHistoryDTO)) // Checking if discount is not already applied
                        {
                            if (!(String.IsNullOrEmpty(discountApplicationHistoryDTO.CouponNumber)))
                            {
                                DiscountCouponsBL discountCouponBL = new DiscountCouponsBL(newTrx.Utilities.ExecutionContext, discountApplicationHistoryDTO.CouponNumber);
                                discountCouponBL.ValidateCouponApplication();// Validating coupon
                                newTrx.ApplyCoupon(discountApplicationHistoryDTO.CouponNumber, discountApplicationHistoryDTO.Remarks, discountApplicationHistoryDTO.ApprovedBy, discountApplicationHistoryDTO.VariableDiscountAmount);
                            }
                            else
                            {
                                DiscountContainerDTO discountContainerDTO = DiscountContainerList.GetDiscountContainerDTOOrDefault(executionContext, discountApplicationHistoryDTO.DiscountId);
                                if (discountContainerDTO != null && discountContainerDTO.AutomaticApply.CompareTo("N") == 0) // Discount to be applied is not auto apply discount
                                    newTrx.ApplyDiscount(discountApplicationHistoryDTO.DiscountId, discountApplicationHistoryDTO.Remarks, discountApplicationHistoryDTO.ApprovedBy, discountApplicationHistoryDTO.VariableDiscountAmount, trxLine, false);
                            }
                        }
                        else
                        {
                            log.Debug("Discount is already applied, ignore" + discountApplicationHistoryDTO.CouponNumber);
                        }
                    }
                    else if (discountApplicationHistoryDTO.DiscountId > -1)
                    {
                        log.Debug("Cancelling discount" + discountApplicationHistoryDTO.DiscountId + discountApplicationHistoryDTO.CouponNumber);
                        newTrx.cancelDiscountLine(discountApplicationHistoryDTO.DiscountId);
                    }
                    else
                    {
                        log.Debug("New discount is applied in cancelled state, ignore" + discountApplicationHistoryDTO.CouponNumber);
                    }

                }
            }
        }

        private bool IsDiscountApplied(DiscountApplicationHistoryDTO discountApplicationHistoryDTO)
        {
            bool isDiscountApplied = false;

            if (newTrx.DiscountApplicationHistoryDTOList == null)
                return isDiscountApplied;

            if (!String.IsNullOrEmpty(discountApplicationHistoryDTO.CouponNumber))
            {
                if (newTrx.DiscountsSummaryDTODictionary.ContainsKey(discountApplicationHistoryDTO.DiscountId) &&
                    newTrx.DiscountsSummaryDTODictionary[discountApplicationHistoryDTO.DiscountId].CouponNumbers != null &&
                    newTrx.DiscountsSummaryDTODictionary[discountApplicationHistoryDTO.DiscountId].CouponNumbers.Contains(discountApplicationHistoryDTO.CouponNumber))
                {
                    isDiscountApplied = true;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(discountApplicationHistoryDTO.TransactionLineGuid))
                {
                    List<DiscountApplicationHistoryDTO> tempList = newTrx.DiscountApplicationHistoryDTOList.Where(x => (x.TransactionLineBL != null && ((((Transaction.TransactionLine)x.TransactionLineBL).Guid == discountApplicationHistoryDTO.TransactionLineGuid)))).ToList();
                    if (tempList != null && tempList.Any())
                    {
                        isDiscountApplied = true;
                    }
                }
                else
                {
                    List<DiscountApplicationHistoryDTO> tempList = newTrx.DiscountApplicationHistoryDTOList.Where(x => x.DiscountId == discountApplicationHistoryDTO.DiscountId).ToList();
                    if (tempList != null && tempList.Any())
                    {
                        isDiscountApplied = true;
                    }
                }
            }

            return isDiscountApplied;
        }

        /// <summary>
        /// Get Transaction Object.
        /// </summary>
        public string GenerateReceiptEMailFromTemplate(String templateName)
        {
            log.LogMethodEntry(templateName);
            string emailContent = "";
            try
            {
                if (newTrx.Trx_id > 0)
                {
                    string contentID = "";
                    string attachFile = null;

                    int templateId = -1;
                    if (string.IsNullOrEmpty(templateName) == false)
                    {
                        EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(newTrx.Utilities.ExecutionContext);
                        List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, templateName));
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, newTrx.Utilities.ExecutionContext.GetSiteId().ToString()));
                        List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                        if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                        {
                            templateId = emailTemplateDTOList[0].EmailTemplateId;
                        }
                    }

                    if (templateId < 0)
                    {
                        // throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Email template for Online Tickets B2C is not set"));
                        throw new Exception(MessageContainerList.GetMessage(newTrx.Utilities.ExecutionContext, "Email template for ONLINE_RECEIPT_EMAIL_TEMPLATE is not set : ") + templateName);
                    }
                    TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(newTrx.Utilities.ExecutionContext, newTrx.Utilities, templateId, newTrx, null);
                    emailContent = transactionEmailTemplatePrint.GenerateEmailTemplateContent();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
            return emailContent;
        }

        /// <summary>
        /// Get Transaction Object.
        /// </summary>
        public string GenerateTicketEMailFromTemplate(String templateName)
        {
            log.LogMethodEntry(templateName);
            string emailContent = "";
            try
            {
                if (newTrx.Trx_id > 0)
                {
                    string contentID = "";
                    string attachFile = null;
                    int templateId = -1;
                    if (string.IsNullOrEmpty(templateName) == false)
                    {
                        EmailTemplateListBL emailTemplateListBL = new EmailTemplateListBL(newTrx.Utilities.ExecutionContext);
                        List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<EmailTemplateDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.NAME, templateName));
                        searchParameters.Add(new KeyValuePair<EmailTemplateDTO.SearchByParameters, string>(EmailTemplateDTO.SearchByParameters.SITE_ID, newTrx.Utilities.ExecutionContext.GetSiteId().ToString()));
                        List<EmailTemplateDTO> emailTemplateDTOList = emailTemplateListBL.GetEmailTemplateDTOList(searchParameters);
                        if (emailTemplateDTOList != null && emailTemplateDTOList.Any())
                        {
                            templateId = emailTemplateDTOList[0].EmailTemplateId;
                        }
                    }

                    if (templateId < 0)
                    {
                        // throw new Exception(MessageContainerList.GetMessage(Utilities.ExecutionContext, "Email template for Online Tickets B2C is not set"));
                        throw new Exception(MessageContainerList.GetMessage(newTrx.Utilities.ExecutionContext, 1962) + templateName);
                    }
                    TransactionEmailTemplatePrint transactionEmailTemplatePrint = new TransactionEmailTemplatePrint(newTrx.Utilities.ExecutionContext, newTrx.Utilities, templateId, newTrx, null);
                    emailContent = transactionEmailTemplatePrint.GenerateTicketEmailTemplateContent();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit();
            return emailContent;
        }

        /// <summary>
        /// GenerateTransactionBasedToken
        /// </summary>
        /// <returns></returns>
        public string GenerateTransactionBasedToken()
        {
            log.LogMethodEntry();
            string securityToken = string.Empty;
            if (this.transactionDTO != null && this.transactionDTO.TransactionId > -1)
            {
                SecurityTokenListBL securityTokenListBL = new SecurityTokenListBL();
                List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>> searchParm = new List<KeyValuePair<SecurityTokenDTO.SearchByParameters, string>>();
                searchParm.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT_GUID, this.transactionDTO.Guid));
                searchParm.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.OBJECT, TRXTOKENOBJECTCODE));
                searchParm.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                searchParm.Add(new KeyValuePair<SecurityTokenDTO.SearchByParameters, string>(SecurityTokenDTO.SearchByParameters.IS_EXPIRED, "N"));
                List<SecurityTokenDTO> securityTokenDTOList = securityTokenListBL.GetSecurityTokenDTOList(searchParm);
                if (securityTokenDTOList != null && securityTokenDTOList.Any())
                {
                    securityToken = securityTokenDTOList[0].Token;
                }
                else
                {
                    try
                    {
                        SecurityTokenBL securityTokenBL = new SecurityTokenBL(executionContext);
                        securityTokenBL.GenerateToken(this.transactionDTO.Guid, TRXTOKENOBJECTCODE);
                        //securityTokenBL.GenerateToken("127CC2F5-8D34-4510-B3D1-B5CC4DA3029D", "TRX_HEADER");
                        //securityTokenBL.GenerateNewJWTToken(utilities.ExecutionContext.GetUserId(),this.transaction.TrxGuid, utilities.ExecutionContext.GetSiteId().ToString(), utilities.ExecutionContext.GetLanguageId().ToString(), utilities.ParafaitEnv.RoleId.ToString(), "TRANSACTION", utilities.ExecutionContext.GetMachineId().ToString()); 
                        securityToken = securityTokenBL.GetSecurityTokenDTO.Token;
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        string msg = MessageContainerList.GetMessage(executionContext, "Unexpected error while generating security token");
                        throw new Exception(msg);
                    }
                }
            }
            log.LogMethodExit(securityToken);
            return securityToken;
        }

        /// <summary>
        /// GetInvoiceType
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public string GetInvoiceType(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            string invoiceType = string.Empty;
            if (newTrx != null)
            {
                invoiceType = newTrx.GetInvoiceType(sqlTransaction);
            }
            log.LogMethodExit(invoiceType);
            return invoiceType;

        }

        /// <summary>
        /// ApplyDiscounts
        /// </summary>
        /// <returns>TransactionDTO</returns>
        public TransactionDTO ApplyDiscounts()
        {
            log.LogMethodEntry();

            string message = string.Empty;
            TransactionDTO createdTransactionDTO = null;
            Utilities utilities = GetUtilities();
            try
            {
                if (transactionDTO.CommitTransaction || transactionDTO.CloseTransaction)
                {
                    try
                    {
                        if (transactionDTO.TransactionId > 0)
                        {
                            TransactionUtils trxUtils = new TransactionUtils(utilities);
                            newTrx = trxUtils.CreateTransactionFromDB(transactionDTO.TransactionId, utilities);
                            if (newTrx == null)
                            {
                                throw new ValidationException("Transaction Not Found");
                            }
                            newTrx.Utilities = utilities;
                        }

                        // refresh the transaction header as the header information might be updated in successive calls
                        CreateRefreshTransactionHeader(utilities);
                        if (transactionDTO.TransactionLinesDTOList != null && transactionDTO.TransactionLinesDTOList.Any())
                        {
                            foreach (TransactionLineDTO transactionLine in transactionDTO.TransactionLinesDTOList)
                            {
                                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(utilities.ExecutionContext, transactionLine.ProductId);
                                if (productsContainerDTO != null && productsContainerDTO.ProductType == "COMBO")
                                {
                                    if (transactionLine.LineId == -1)
                                    {
                                        log.Info("CreateProduct() - COMBO ");
                                        Transaction.TransactionLine outTransactionLine = new Transaction.TransactionLine();
                                        Transaction.TransactionLine parentTransactionLine = null;
                                        outTransactionLine.guid = transactionLine.ClientGuid;
                                        if (0 != newTrx.createTransactionLine(null, transactionLine.ProductId, -1, Convert.ToDecimal(transactionLine.Quantity),
                                                                                     parentTransactionLine, ref message, outTransactionLine))
                                        {
                                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, message));
                                        }

                                        {
                                            parentTransactionLine = newTrx.TrxLines.FirstOrDefault(x => x.guid == transactionLine.ClientGuid);
                                        }
                                        List<TransactionLineDTO> childProducts = transactionDTO.TransactionLinesDTOList.Where(x => !String.IsNullOrEmpty(x.ParentLineGuid) && x.ParentLineGuid.Equals(transactionLine.ClientGuid)).ToList();
                                        if (childProducts != null && childProducts.Any())
                                        {
                                            foreach (TransactionLineDTO childLine in childProducts)
                                            {
                                                if (parentTransactionLine.Price > 0)//check this
                                                {
                                                    childLine.Price = 0;
                                                    if (childLine.AttractionBookingDTO != null)
                                                        childLine.AttractionBookingDTO.Price = 0;
                                                }
                                                if (childLine.AttractionBookingDTO != null)
                                                {
                                                    Transaction.TransactionLine trxLine = GetTransactionLine(childLine, parentTransactionLine);
                                                    newTrx.TrxLines.Add(trxLine);
                                                }
                                                else
                                                {
                                                    message += CreateTransactionLine(childLine, utilities);
                                                }
                                            }
                                        }

                                    }
                                }
                                // If product is attraction product not part of any combo products
                                else if (string.IsNullOrWhiteSpace(transactionLine.ParentLineGuid) && productsContainerDTO != null && productsContainerDTO.ProductType == "ATTRACTION")
                                {
                                    Transaction.TransactionLine trxLine = GetTransactionLine(transactionLine, null);
                                    newTrx.TrxLines.Add(trxLine);
                                }
                                else if (string.IsNullOrWhiteSpace(transactionLine.ParentLineGuid) == false
                                        && transactionDTO.TransactionLinesDTOList.Exists(x => x.ClientGuid == transactionLine.ParentLineGuid))
                                {
                                    int parentProductId = transactionDTO.TransactionLinesDTOList.FirstOrDefault(x => x.ClientGuid == transactionLine.ParentLineGuid).ProductId;
                                    ProductsContainerDTO parentProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(utilities.ExecutionContext, parentProductId);
                                    if (parentProductContainerDTO == null || (parentProductContainerDTO != null && parentProductContainerDTO.ProductType == "COMBO"))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        message += CreateTransactionLine(transactionLine, utilities);
                                    }
                                }
                                else
                                {
                                    message += CreateTransactionLine(transactionLine, utilities);
                                }
                            }
                            ApplyTransactionDiscount();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message);
                        throw;
                    }
                }
                else
                {
                    try
                    {
                        CreateRefreshTransactionHeader(utilities);
                        if (transactionDTO.TransactionLinesDTOList != null && transactionDTO.TransactionLinesDTOList.Any())
                        {
                            foreach (TransactionLineDTO transactionLine in transactionDTO.TransactionLinesDTOList)
                            {
                                ProductsContainerDTO productsContainerDTO = ProductsContainerList.GetProductsContainerDTO(utilities.ExecutionContext, transactionLine.ProductId);
                                if (productsContainerDTO != null && productsContainerDTO.ProductType == "COMBO")
                                {
                                    if (transactionLine.LineId == -1)
                                    {
                                        log.Info("CreateProduct() - COMBO ");
                                        Transaction.TransactionLine outTransactionLine = new Transaction.TransactionLine();
                                        Transaction.TransactionLine parentTransactionLine = null;
                                        outTransactionLine.guid = transactionLine.ClientGuid;
                                        if (0 != newTrx.createTransactionLine(null, transactionLine.ProductId, -1, Convert.ToDecimal(transactionLine.Quantity),
                                                                                     parentTransactionLine, ref message, outTransactionLine))
                                        {
                                            throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, message));
                                        }

                                        {
                                            parentTransactionLine = newTrx.TrxLines.FirstOrDefault(x => x.guid == transactionLine.ClientGuid);
                                        }
                                        List<TransactionLineDTO> childProducts = transactionDTO.TransactionLinesDTOList.Where(x => !String.IsNullOrEmpty(x.ParentLineGuid) && x.ParentLineGuid.Equals(transactionLine.ClientGuid)).ToList();
                                        if (childProducts != null && childProducts.Any())
                                        {
                                            foreach (TransactionLineDTO childLine in childProducts)
                                            {
                                                if (parentTransactionLine.Price > 0)//check this
                                                {
                                                    childLine.Price = 0;
                                                    if (childLine.AttractionBookingDTO != null)
                                                        childLine.AttractionBookingDTO.Price = 0;
                                                }
                                                if (childLine.AttractionBookingDTO != null)
                                                {
                                                    Transaction.TransactionLine trxLine = GetTransactionLine(childLine, parentTransactionLine);
                                                    newTrx.TrxLines.Add(trxLine);
                                                }
                                                else
                                                {
                                                    message += CreateTransactionLine(childLine, utilities);
                                                }
                                            }
                                        }

                                    }
                                }
                                // If product is attraction product not part of any combo products
                                else if (string.IsNullOrWhiteSpace(transactionLine.ParentLineGuid) && productsContainerDTO != null && productsContainerDTO.ProductType == "ATTRACTION")
                                {
                                    Transaction.TransactionLine trxLine = GetTransactionLine(transactionLine, null);
                                    newTrx.TrxLines.Add(trxLine);
                                }
                                else if (string.IsNullOrWhiteSpace(transactionLine.ParentLineGuid) == false
                                          && transactionDTO.TransactionLinesDTOList.Exists(x => x.ClientGuid == transactionLine.ParentLineGuid))
                                {
                                    int parentProductId = transactionDTO.TransactionLinesDTOList.FirstOrDefault(x => x.ClientGuid == transactionLine.ParentLineGuid).ProductId;
                                    ProductsContainerDTO parentProductContainerDTO = ProductsContainerList.GetProductsContainerDTO(utilities.ExecutionContext, parentProductId);
                                    if (parentProductContainerDTO == null || (parentProductContainerDTO != null && parentProductContainerDTO.ProductType == "COMBO"))
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        message += CreateTransactionLine(transactionLine, utilities);
                                    }
                                }
                                else
                                {
                                    message += CreateTransactionLine(transactionLine, utilities);
                                }
                            }
                            ApplyTransactionDiscount();
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Info(ex.Message);
                        throw;
                    }
                }
                if (newTrx != null)
                {
                    log.LogMethodExit("Success");
                    createdTransactionDTO = newTrx.TransactionDTO;
                    // Add the cancelled discounts also, else the cancelled discounts which have auto apply flag as true will be re-applied when the transactionn is sent back for processing
                    if (transactionDTO.DiscountApplicationHistoryDTOList != null && transactionDTO.DiscountApplicationHistoryDTOList.Any())
                    {
                        if (createdTransactionDTO.DiscountApplicationHistoryDTOList == null)
                            createdTransactionDTO.DiscountApplicationHistoryDTOList = new List<DiscountApplicationHistoryDTO>();
                        createdTransactionDTO.DiscountApplicationHistoryDTOList.AddRange(transactionDTO.DiscountApplicationHistoryDTOList.Where(x => x.IsCancelled).ToList());
                    }
                }
                else
                {
                    throw new Exception(message);
                }
                log.LogMethodExit(createdTransactionDTO);
                return createdTransactionDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        private Transaction.TransactionLine GetTransactionLine(TransactionLineDTO transactionLineDTO, Transaction.TransactionLine parentLine)
        {
            log.LogMethodEntry(transactionLineDTO, parentLine);
            Utilities utilities = GetUtilities();
            Transaction.TransactionLine TrxLine = new Transaction.TransactionLine();
            string message = "";
            Card CurrentCard = null;
            TransactionUtils TransactionUtils = new TransactionUtils(utilities);
            CommonFuncs CommonFuncs = new CommonFuncs(utilities);
            int product_id = transactionLineDTO.ProductId;
            decimal ProductQuantity = transactionLineDTO.Quantity == null ? 0 : Convert.ToDecimal(transactionLineDTO.Quantity.ToString());
            TagNumberLengthList tagNumberLengthList = new TagNumberLengthList(utilities.ExecutionContext);

            try
            {
                Semnox.Parafait.Product.Products productBL = new Semnox.Parafait.Product.Products(product_id);
                if (productBL.GetProductsDTO == null || productBL.GetProductsDTO.ProductId == -1)
                {
                    log.Info("Product not found");
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 111));
                }
                ProductsDTO Product = productBL.GetProductsDTO;
                string productType = Product.ProductType;

                if (!String.IsNullOrEmpty(transactionLineDTO.CardNumber))
                {
                    CurrentCard = new Card(transactionLineDTO.CardNumber, utilities.ParafaitEnv.LoginID, utilities);
                }
                if (transactionLineDTO.AttractionBookingDTO == null)
                {
                    log.Info("Failed to create transaction as attraction booking object is not found" + newTrx);
                    throw new ValidationException(MessageContainerList.GetMessage(newTrx.Utilities.ExecutionContext, 2097));
                }
                if (utilities.ParafaitEnv.CARD_MANDATORY_FOR_TRANSACTION == "Y" && (CurrentCard == null || CurrentCard.CardStatus == "NEW"))
                {
                    log.Info("Ends-CreateProduct() as Valid Card Mandatory for Transaction");
                    throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 222));
                }
                double atbPrice = transactionLineDTO.AttractionBookingDTO.Price;
                AttractionBooking atb = new AttractionBooking(newTrx.Utilities.ExecutionContext, transactionLineDTO.AttractionBookingDTO.BookingId);
                if (atb != null && atb.AttractionBookingDTO != null)
                {
                    if (atb.AttractionBookingDTO.ExpiryDate < atb.AttractionBookingDTO.ScheduleFromDate)
                        atb.AttractionBookingDTO.ExpiryDate = atb.AttractionBookingDTO.ScheduleFromDate.AddMinutes(atb.GetBookingCompletionTimeLimit());
                    Card attractionCard = null;
                    bool createNewCard = true;
                    if (Product.LoadToSingleCard)
                    {
                        String existingCard = newTrx.GetConsolidateCardFromTransaction(Product, -1, false);
                        if (!String.IsNullOrEmpty(existingCard))
                        {
                            attractionCard = new Card(existingCard, newTrx.Utilities.ExecutionContext.GetUserId(), newTrx.Utilities);
                            createNewCard = false;
                        }
                    }

                    if (createNewCard)
                    {
                        if (CurrentCard != null)
                            attractionCard = CurrentCard;
                        else
                        {
                            RandomTagNumber randomTagNumber = new RandomTagNumber(utilities.ExecutionContext, tagNumberLengthList);
                            attractionCard = new Card(Product.AutoGenerateCardNumber == "Y" ? randomTagNumber.Value : "T" + randomTagNumber.Value.Substring(1),
                             newTrx.Utilities.ExecutionContext.GetUserId(), newTrx.Utilities);
                        }
                    }

                    List<Card> cardList = new List<Card>();
                    cardList.Add(attractionCard);
                    atb.cardList = cardList;

                    if (Product.CardSale.Equals("Y") == false)
                    {
                        if (atb.cardList != null)
                            atb.cardList = null;
                    }
                    double businessDayStartTime = !String.IsNullOrEmpty(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BUSINESS_DAY_START_TIME")) ? ParafaitDefaultContainerList.GetParafaitDefault<double>(executionContext, "BUSINESS_DAY_START_TIME") : 6;
                    TimeZoneUtil timeZoneUtil = new TimeZoneUtil();

                    DateTime scheduleDate = atb.AttractionBookingDTO.ScheduleFromDate;
                    int offSetDuration = 0;
                    scheduleDate = scheduleDate.Date.AddHours(businessDayStartTime);
                    offSetDuration = timeZoneUtil.GetOffSetDuration(atb.AttractionBookingDTO.SiteId, scheduleDate);

                    if (atb.AttractionBookingDTO.ExpiryDate < newTrx.Utilities.getServerTime())
                    {
                        log.Debug("atb.AttractionBookingDTO.ExpiryDate : " + atb.AttractionBookingDTO.ExpiryDate);
                        log.Debug(" newTrx.Utilities.getServerTime()e : " + newTrx.Utilities.getServerTime());
                        message = "Attraction Booking has expired";
                        log.Info(message);
                        throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, message));
                    }

                    atb.AttractionBookingDTO.AvailableUnits = atb.AttractionBookingDTO.AvailableUnits == null ? 0 : atb.AttractionBookingDTO.AvailableUnits;
                    log.Debug("Before offset & offset is " + offSetDuration);
                    log.Debug(atb.AttractionBookingDTO.BookingId + ":" + atb.AttractionBookingDTO.ScheduleFromDate + ":" + atb.AttractionBookingDTO.ScheduleToDate);

                    // The ATB is built from DB - So it contains HQ time - already adjusted for offset, remove that from the time.
                    atb.AttractionBookingDTO.ScheduleFromDate = atb.AttractionBookingDTO.ScheduleFromDate.AddSeconds(-1 * offSetDuration);
                    atb.AttractionBookingDTO.ScheduleToDate = atb.AttractionBookingDTO.ScheduleToDate.AddSeconds(-1 * offSetDuration);
                    atb.AttractionBookingDTO.ExpiryDate = atb.AttractionBookingDTO.ExpiryDate.AddSeconds(-1 * offSetDuration);

                    log.Debug("After offset" + atb.AttractionBookingDTO.BookingId + ":" + atb.AttractionBookingDTO.ScheduleFromDate + ":" + atb.AttractionBookingDTO.ScheduleToDate);
                    TrxLine.AttractionDetails = transactionLineDTO.ProductDetail;
                    TrxLine.card = CurrentCard;
                    if (CurrentCard != null)
                    {
                        TrxLine.CardNumber = CurrentCard.CardNumber;
                    }
                    TrxLine.Credits = transactionLineDTO.Credits.HasValue ? Convert.ToDouble(transactionLineDTO.Credits) : 0;
                    TrxLine.Bonus = transactionLineDTO.Bonus.HasValue ? Convert.ToDouble(transactionLineDTO.Bonus) : 0;
                    TrxLine.Courtesy = transactionLineDTO.Courtesy.HasValue ? Convert.ToDouble(transactionLineDTO.Courtesy) : 0;
                    TrxLine.DBLineId = transactionLineDTO.LineId;
                    TrxLine.IsWaiverRequired = transactionLineDTO.IsWaiverSignRequired;
                    TrxLine.LineAmount = transactionLineDTO.Amount.HasValue ? Convert.ToDouble(transactionLineDTO.Amount) : 0;
                    TrxLine.LineValid = true;
                    TrxLine.MembershipId = transactionLineDTO.MembershipId;
                    TrxLine.OriginalLineID = transactionLineDTO.OriginalLineId;
                    TrxLine.Price = transactionLineDTO.Price.HasValue ? Convert.ToDouble(transactionLineDTO.Price) : 0;
                    TrxLine.ParentLine = parentLine;
                    TrxLine.ProductID = product_id;
                    TrxLine.ProductName = transactionLineDTO.ProductName;
                    TrxLine.LineAtb = atb;
                    TrxLine.ProductTypeCode = transactionLineDTO.ProductTypeCode;
                    TrxLine.PromotionId = transactionLineDTO.PromotionId;
                    TrxLine.quantity = transactionLineDTO.Quantity.HasValue ? Convert.ToDecimal(transactionLineDTO.Quantity) : 0;
                    TrxLine.ReceiptPrinted = transactionLineDTO.ReceiptPrinted;
                    TrxLine.Remarks = transactionLineDTO.Remarks;
                    TrxLine.Time = transactionLineDTO.Time.HasValue ? Convert.ToDouble(transactionLineDTO.Time) : 0;
                    TrxLine.LoyaltyPoints = transactionLineDTO.LoyaltyPoints.HasValue ? Convert.ToDouble(transactionLineDTO.LoyaltyPoints) : 0;
                    TrxLine.TransactionDiscountsDTOList = transactionLineDTO.TransactionDiscountsDTOList;
                    TrxLine.UserPrice = transactionLineDTO.UserPrice;
                    if (CurrentCard != null)
                    {
                        TrxLine.vip_card = CurrentCard.vip_customer.ToString();
                    }
                    TrxLine.WaiverSignedDTOList = transactionLineDTO.WaiverSignedDTOList;
                    TrxLine.Tickets = transactionLineDTO.Tickets.HasValue ? Convert.ToInt32(transactionLineDTO.Tickets) : 0;
                    TrxLine.SubscriptionHeaderDTO = transactionLineDTO.SubscriptionHeaderDTO;
                    TrxLine.LineProcessed = false;
                    TrxLine.KDSSent = transactionLineDTO.KDSSent;
                    TrxLine.KOTPrintCount = transactionLineDTO.KOTPrintCount;
                    TrxLine.tax_id = transactionLineDTO.TaxId;
                    TrxLine.tax_percentage = transactionLineDTO.TaxPercentage.HasValue ? Convert.ToDouble(transactionLineDTO.TaxPercentage) : 0;

                    // if the transactionLineDTO.TaxAmount is not null, it indicates that the input already had tax calculated. Do not deduct tax twice
                    if (Product.TaxInclusivePrice.ToString() == "Y" && !transactionLineDTO.TaxAmount.HasValue)
                    {
                        TrxLine.Price = TrxLine.Price / (1.0 + TrxLine.tax_percentage / 100.0);
                    }
                    TrxLine.tax_amount = transactionLineDTO.TaxAmount.HasValue ? Convert.ToDouble(transactionLineDTO.TaxAmount) : 0;
                }
                else
                {
                    log.Info("Failed to create transaction as attraction booking object is not found" + newTrx);
                    throw new ValidationException(MessageContainerList.GetMessage(newTrx.Utilities.ExecutionContext, 2097));
                }
                log.LogMethodExit(TrxLine);
                return TrxLine;
            }
            catch (Exception ex)
            {
                log.Info("Failed to create transaction " + newTrx);
                throw;
            }
        }

        /// <summary>
        /// SettleTransactionPayment
        /// </summary>
        public string SettleTransactionPayment(TransactionPaymentsDTO inputTrxPaymentDTO)
        {
            log.LogMethodEntry(inputTrxPaymentDTO);
            string message = null;
            if (transactionDTO != null)
            {
                if (newTrx == null)
                {
                    throw new ValidationException("Transaction object is null");
                }
                if (newTrx.Trx_id <= 0)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 665) + ". " + MessageContainerList.GetMessage(executionContext, "Settle Transaction Payment"));
                    //Please save changes before this operation
                }
                if (inputTrxPaymentDTO == null)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1831) + " - " + MessageContainerList.GetMessage(executionContext, "TransactionPaymentsDTO"));
                    //The Parameter should not be empty
                }
                TransactionPaymentsBL transactionPaymentsBL = new TransactionPaymentsBL(executionContext, inputTrxPaymentDTO.PaymentId);
                if (transactionPaymentsBL.TransactionPaymentsDTO != null && transactionPaymentsBL.TransactionPaymentsDTO.TransactionId != transactionDTO.TransactionId)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4097, transactionDTO.TransactionId));
                    //'Sorry, payment record does not belong to this transaction & 1' 
                }
                transactionPaymentsBL.SettleTransactionPayment(inputTrxPaymentDTO);

                bool allPaymentsAreSettled = true;
                message = MessageContainerList.GetMessage(executionContext, 2206);
                // string errorMessage = "Net Amount does not match total Trx Payments amount. Net Amount: " + newTrx.Net_Transaction_Amount + ". , Paid Amount: " + newTrx.TotalPaidAmount + ".";
                if (newTrx.Net_Transaction_Amount == newTrx.TotalPaidAmount && transactionDTO.TrxPaymentsDTOList != null)
                {
                    PaymentGatewayFactory.GetInstance().Initialize(newTrx.Utilities, true, null);
                    for (int i = 0; i < transactionDTO.TrxPaymentsDTOList.Count; i++)
                    {
                        if (transactionDTO.TrxPaymentsDTOList[i].PaymentModeDTO.GatewayLookUp != PaymentGateways.None)
                        {
                            PaymentGateway paymentGateway = PaymentGatewayFactory.GetInstance().GetPaymentGateway(transactionDTO.TrxPaymentsDTOList[i].PaymentModeDTO.GatewayLookUp.ToString());
                            if (paymentGateway.IsSettlementPending(transactionDTO.TrxPaymentsDTOList[i]) == true)
                            {
                                allPaymentsAreSettled = false;
                                break;
                            }
                        }
                    }
                    if (allPaymentsAreSettled)
                    {
                        transactionDTO.CloseTransaction = true;
                        string str = SaveTransaction(newTrx.Utilities);
                        message += MessageContainerList.GetMessage(executionContext, "Transaction has been successfully completed");
                    }
                }
                else
                {
                    message += MessageContainerList.GetMessage(executionContext, 4998, newTrx.Net_Transaction_Amount, newTrx.TotalPaidAmount);
                }
            }
            log.LogMethodExit(message);
            return message;
        }

        /// <summary>
        /// AssignRider
        /// </summary>
        /// <param name="transactionDeliveryDetailsDTO"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public TransactionOrderDispensingDTO AssignRider(TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionDeliveryDetailsDTO, sqlTransaction);
            CanAssignRider();
            TransactionOrderDispensingBL transctionOrderDispensingBL = new TransactionOrderDispensingBL(executionContext, transactionDTO.TransctionOrderDispensingDTO, true, false, sqlTransaction);
            transctionOrderDispensingBL.AssignRider(transactionDeliveryDetailsDTO, sqlTransaction);
            //TransactionOrderDispensingDTO result = transctionOrderDispensingBL.TransctionOrderDispensingDTO;
            TransactionOrderDispensingDTO result = new TransactionOrderDispensingBL(executionContext, transactionDTO.TransctionOrderDispensingDTO.TransactionOrderDispensingId, true, false, sqlTransaction).TransctionOrderDispensingDTO;
            log.LogMethodExit(result);
            return result;
        }
        private void CanAssignRider()
        {
            log.LogMethodEntry();
            if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                || transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3044);//Transaction Is cancelled. Cannot Assign Rider
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (TransactionDTO.TransctionOrderDispensingDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Cannot assign rider as order dispensing details are missing for the transaction");//Transaction Is cancelled. Cannot Assign Rider
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// UnAssignRider
        /// </summary>
        /// <param name="transactionDeliveryDetailsDTO"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public TransactionOrderDispensingDTO UnAssignRider(TransactionDeliveryDetailsDTO transactionDeliveryDetailsDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(transactionDeliveryDetailsDTO, sqlTransaction);
            CanUnAssignRider();
            TransactionOrderDispensingBL transctionOrderDispensingBL = new TransactionOrderDispensingBL(executionContext, TransactionDTO.TransctionOrderDispensingDTO, true, false, sqlTransaction);
            transctionOrderDispensingBL.UnAssignRider(transactionDeliveryDetailsDTO, sqlTransaction);
            //TransactionOrderDispensingDTO result = transctionOrderDispensingBL.TransctionOrderDispensingDTO;
            TransactionOrderDispensingDTO result = new TransactionOrderDispensingBL(executionContext, transactionDTO.TransctionOrderDispensingDTO.TransactionOrderDispensingId, true, false, sqlTransaction).TransctionOrderDispensingDTO;
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Executes the online transactions in Kiosk to Issue cards
        /// </summary>
        /// <param name="utilities">utilities</param>
        /// <param name="cardRoamingRemotingClient">remoting client</param>
        /// <param name="tempCardLines">lines to be executed</param>
        /// <param name="tagNumbers">physical tag number</param>
        /// <exception cref="Exception"></exception>
        public void ExecuteOnlineTransactionInKiosk(Utilities utilities, RemotingClient cardRoamingRemotingClient, List<TransactionLineDTO> tempCardLines, List<string> tagNumbers)
        {
            log.LogMethodEntry(utilities, tempCardLines, tagNumbers);
            if (tempCardLines == null || tagNumbers == null || tempCardLines.Count == 0 ||
                                tempCardLines.Count != tagNumbers.Count)
            {
                log.LogMethodExit(null, "Lines to be executed is empty.");
                return;
            }
            if (IsVirtualStore())
            {
                DownloadTempCards(cardRoamingRemotingClient, utilities.ParafaitEnv.SiteId, tempCardLines);
            }
            CheckQueueProducts(tempCardLines, tagNumbers);

            using (ParafaitDBTransaction parafaitDbTransaction = new ParafaitDBTransaction())
            {
                parafaitDbTransaction.BeginTransaction();
                TaskProcs tp = new TaskProcs(utilities);
                CardUtils cardUtils = new CardUtils(utilities);
                List<string> cardNumberList = tempCardLines.Where(tl => string.IsNullOrWhiteSpace(tl.CardNumber) == false).Select(tl => tl.CardNumber).ToList();
                cardNumberList.AddRange(tagNumbers);
                List<Card> cardObjectList = cardUtils.GetCardList(cardNumberList, "", parafaitDbTransaction.SQLTrx);
                for (int i = 0; i < tempCardLines.Count; i++)
                {
                    TransactionLineDTO tempCardLine = tempCardLines[i];
                    string tagNumber = tagNumbers[i];
                    if (tempCardLine.CardNumber == tagNumber)
                    {
                        continue;
                    }
                    Card tempCard = cardObjectList.Find(cardObject => cardObject.CardNumber == tempCardLine.CardNumber); // new Card(tempCardLine.CardNumber, "", utilities); 
                    if (tempCard.CardStatus.Equals("NEW"))
                    {
                        string errorMessage = MessageContainerList.GetMessage(utilities.ExecutionContext, 1684, tempCardLine.CardNumber);
                        log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                        throw new Exception(errorMessage);
                    }
                    Card card = cardObjectList.Find(cardObject => cardObject.CardNumber == tagNumber); // new Card(tagNumber, "", utilities); 
                    if (card.CardStatus == "ISSUED")
                    {
                        if (card.technician_card.Equals('N'))
                        {
                            string refundMsg = "";
                            if (!tp.RefundCard(card, 0, 0, 0, "Deactivated By Kiosk", ref refundMsg, true))
                            {
                                log.Info("Unable to Deactivate card [" + card.CardNumber + " ]" + " error: " + refundMsg);
                                throw new Exception(refundMsg);
                            }
                            else
                            {
                                card.invalidateCard(null);
                                card = new Card(card.CardNumber, "", utilities);
                            }
                        }
                        else if (card.technician_card.Equals('Y'))
                        {
                            card = null;
                            string msg = MessageContainerList.GetMessage(utilities.ExecutionContext, 4540, card.CardNumber);
                            //'Technician card cannot be dispensed. Card number: [ &1 ]'
                            log.Info(msg);
                            throw new Exception(msg);
                        }
                    }
                    else
                    {
                        string message = string.Empty;
                        if (!tp.transferCard(tempCard, card, "Execute Transaction", ref message, parafaitDbTransaction.SQLTrx, (IsVirtualStore() ? -1 : this.transactionDTO.TransactionId)))
                        {
                            log.LogMethodExit(false, "Throwing Exception - " + message);
                            throw new Exception(message);
                        }
                    }
                }
                if (IsVirtualStore())
                {
                    //Execute the virtual store transaction
                    UpdateVirtualStoreTransaction(tempCardLines, tagNumbers);
                }
                parafaitDbTransaction.EndTransaction();
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Get Printable Lines For Online Transaction
        /// </summary> 
        /// <returns></returns>
        public List<KeyValuePair<string, List<TransactionLineDTO>>> GetPrintableLinesForOnlineTransaction(string printerTypeList)
        {
            log.LogMethodEntry(printerTypeList);
            List<KeyValuePair<string, List<TransactionLineDTO>>> result;
            if (IsVirtualStore())
            {
                result = GetPrintableLinesForVirtualStoreTransaction(printerTypeList);
            }
            else
            {
                bool forVirtualStore = false;
                result = GetPrintableTransactionLines(printerTypeList, forVirtualStore);
            }
            log.LogMethodExit(result);
            return result;
        }

        private List<KeyValuePair<string, List<TransactionLineDTO>>> GetPrintableLinesForVirtualStoreTransaction(string printerTypeList)
        {
            log.LogMethodEntry(printerTypeList);
            TransactionWebDataHandler transactionWebDataHandler = new TransactionWebDataHandler(executionContext, GetVirtualSiteId());
            List<KeyValuePair<string, List<TransactionLineDTO>>> printableLineList = transactionWebDataHandler.GetPrintableTransactionLines(transactionDTO.TransactionId, printerTypeList, true);
            if (printableLineList == null)
            {
                log.LogMethodExit(false, "Unable to retrive printable lines via web-api.");
                return printableLineList;
            }
            log.LogMethodExit(printableLineList);
            return printableLineList;
        }

        private void CanUnAssignRider()
        {
            log.LogMethodEntry();
            if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                || transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3045);//"Transaction is cancelled. Cannot Unassign Rider"
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (transactionDTO.TransctionOrderDispensingDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4019, MessageContainerList.GetMessage(executionContext, "Rider Assignment"));
                //Cannot perform &1 as Order dispensing details are missing for the transaction
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        /// <param name="sqlTransaction"></param>
        public TransactionOrderDispensingDTO SaveRiderDeliveryStatus(List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            CanSaveRiderDeliveryStatus();
            TransactionOrderDispensingBL transctionOrderDispensingBL = new TransactionOrderDispensingBL(executionContext, TransactionDTO.TransctionOrderDispensingDTO, true, false, sqlTransaction);
            transctionOrderDispensingBL.SaveRiderDeliveryStatus(transactionDeliveryDetailsDTOList, sqlTransaction);
            TransactionOrderDispensingDTO resultDTO = new TransactionOrderDispensingBL(executionContext, transactionDTO.TransctionOrderDispensingDTO.TransactionOrderDispensingId, true, false, sqlTransaction).TransctionOrderDispensingDTO;
            log.LogMethodExit(resultDTO);
            return resultDTO;
        }
        private void CanSaveRiderDeliveryStatus()
        {
            log.LogMethodEntry();
            if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString() ||
                TransactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3046);//"Cannot Save Delivery status. Transaction is cancelled"
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (transactionDTO.TransctionOrderDispensingDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4019, MessageContainerList.GetMessage(executionContext, "Rider status Update"));
                //'Cannot perform &1 as Order dispensing details are missing for the transaction'
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionDeliveryDetailsDTOList"></param>
        /// <param name="sqlTransaction"></param>
        public TransactionOrderDispensingDTO SaveRiderAssignmentRemarks(List<TransactionDeliveryDetailsDTO> transactionDeliveryDetailsDTOList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            CanSaveRiderAssignmentRemarks();
            TransactionOrderDispensingBL transctionOrderDispensingBL = new TransactionOrderDispensingBL(executionContext, TransactionDTO.TransctionOrderDispensingDTO.TransactionOrderDispensingId, true, false, sqlTransaction);
            transctionOrderDispensingBL.SaveRiderAssignmentRemarks(transactionDeliveryDetailsDTOList, sqlTransaction);
            TransactionOrderDispensingDTO resultDTO = new TransactionOrderDispensingBL(executionContext, transactionDTO.TransctionOrderDispensingDTO.TransactionOrderDispensingId, true, false, sqlTransaction).TransctionOrderDispensingDTO;
            log.LogMethodExit(resultDTO);
            return resultDTO;
        }

        private void CanSaveRiderAssignmentRemarks()
        {
            log.LogMethodEntry();
            if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                || TransactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3047);//Transaction is Cancelled. Cannot save Rider Assignment Remarks
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            if (transactionDTO.TransctionOrderDispensingDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4019, MessageContainerList.GetMessage(executionContext, "Rider remarks Update"));//Transaction Is cancelled. Cannot Assign Rider
                log.Error(errorMessage);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// SetAsCustomerReconfirmedOrder
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void SetAsCustomerReconfirmedOrder(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CanMarkAsReconfirmedOrder();
            TransactionOrderDispensingBL transctionOrderDispensingBL = new TransactionOrderDispensingBL(executionContext, TransactionDTO.TransctionOrderDispensingDTO, true, false, sqlTransaction);
            transctionOrderDispensingBL.SetAsCustomerReconfirmedOrder(sqlTransaction);
            log.LogMethodExit();
        }
        private void CanMarkAsReconfirmedOrder()
        {
            log.LogMethodEntry();
            if (transactionDTO.Status == Transaction.TrxStatus.PREPARED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3048);
                //"Transaction Status is Prepared. Cannot mark as Reconfirmed Order"
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            if (transactionDTO.Status == Transaction.TrxStatus.CLOSED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3049);//Transaction is Completed. Cannot mark as Reconfirmed Order
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }

            if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                || transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3050);//Transaction is Cancelled. Cannot mark as Reconfirmed Order
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            if (transactionDTO.TransctionOrderDispensingDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4019, MessageContainerList.GetMessage(executionContext, "Order Reconfirmation"));
                //Cannot perform &1 as Order dispensing details are missing for the transaction
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }

            log.LogMethodExit();
        }
        /// <summary>
        /// SetAsPreparationReconfirmedOrder
        /// </summary>
        public void SetAsPreparationReconfirmedOrder(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CanMarkAsPreparationReconfirmedOrder();
            TransactionOrderDispensingBL transctionOrderDispensingBL = new TransactionOrderDispensingBL(executionContext, TransactionDTO.TransctionOrderDispensingDTO, true, false, sqlTransaction);
            transctionOrderDispensingBL.SetAsPreparationReconfirmedOrder(sqlTransaction);
            log.LogMethodExit();
        }
        private void CanMarkAsPreparationReconfirmedOrder()
        {
            log.LogMethodEntry();
            if (transactionDTO.Status == Transaction.TrxStatus.PREPARED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3051);//"Transaction Status is Prepared. Cannot mark as Preparation ReconfirmedOrder Order"
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            if (transactionDTO.Status == Transaction.TrxStatus.CLOSED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3052);
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }

            if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                || transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3053);//Transaction is Cancelled. Cannot mark as Preparation ReconfirmedOrder Order
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            if (transactionDTO.TransctionOrderDispensingDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4019, MessageContainerList.GetMessage(executionContext, "Preparation Reconfirmation"));
                //Cannot perform &1 as Order dispensing details are missing for the transaction
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newStatus"></param>
        /// <param name="sqlTransaction"></param>
        public void UpdateTransactionStatus(string newStatus, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(newStatus);
            if (newStatus == Transaction.TrxStatus.BOOKING.ToString())
            {
                if (transactionDTO.Status == Transaction.TrxStatus.BOOKING.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.CLOSED.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.OPEN.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.PENDING.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.RESERVED.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.PREPARED.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.ORDERED.ToString())
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 3054);//Cannot Update Transaction status to Booked
                    log.Error(errorMessage);
                    throw new Exception(errorMessage);
                }
            }
            if (newStatus == Transaction.TrxStatus.PREPARED.ToString())
            {
                if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.CLOSED.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.PREPARED.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.PENDING.ToString()
                    || transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 3055);//Cannot Update Transaction status to PREPARED
                    log.Error(errorMessage);
                    throw new Exception(errorMessage);
                }
            }
            if (newStatus == Transaction.TrxStatus.ORDERED.ToString())
            {
                if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                     || transactionDTO.Status == Transaction.TrxStatus.CLOSED.ToString()
                     || transactionDTO.Status == Transaction.TrxStatus.PREPARED.ToString()
                     || transactionDTO.Status == Transaction.TrxStatus.ORDERED.ToString()
                     || transactionDTO.Status == Transaction.TrxStatus.PENDING.ToString()
                     || transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 3056, transactionDTO.Status);
                    //Cannot Update Transaction status to Ordered because the transaction is in &1 status
                    log.Error(errorMessage);
                    throw new Exception(errorMessage);
                }
                if (transactionDTO.TransctionOrderDispensingDTO.ReconfirmationOrder == TransactionOrderDispensingDTO.ReConformationStatus.YES)
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 3057);//"Order not yet Reconfimed. Cannot Update transaction status to Ordered"
                    log.Error(errorMessage);
                    throw new Exception(errorMessage);

                }
            }
            ParafaitMessageQueueListBL parafaitMessageQueueListBL = new ParafaitMessageQueueListBL(executionContext);
            List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.IS_ACTIVE, "1"));
            searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID, transactionDTO.Guid));
            searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.ATTEMPTS_LESS_THAN, "3"));
            List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = parafaitMessageQueueListBL.GetParafaitMessageQueues(searchParameters);
            if (parafaitMessageQueueDTOList != null && parafaitMessageQueueDTOList.Any())
            {
                if (parafaitMessageQueueDTOList.Exists(x => x.Status == MessageQueueStatus.UnRead))
                {
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 4229);
                    //Cannot Proceed. The transaction is still processing
                    log.Error(errorMessage);
                    throw new Exception(errorMessage);
                }
            }
            if (newStatus != transactionDTO.Status)
            {
                transactionDTO.Status = newStatus;
                Save(sqlTransaction);
            }
            AddtoParafaitMessageQueue(sqlTransaction);
            log.LogMethodExit();
        }

        private void AddtoParafaitMessageQueue(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            ParafaitMessageQueueListBL parafaitMessageQueueListBL = new ParafaitMessageQueueListBL(executionContext);
            List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ParafaitMessageQueueDTO.SearchByParameters, string>(ParafaitMessageQueueDTO.SearchByParameters.ENTITY_GUID, transactionDTO.Guid));
            List<ParafaitMessageQueueDTO> parafaitMessageQueueDTOList = parafaitMessageQueueListBL.GetParafaitMessageQueues(searchParameters);
            string remarks = string.Empty;
            String actionType = "Transaction" + transactionDTO.Status;
            if (transactionDTO.Status == Transaction.TrxStatus.ORDERED.ToString())
            {
                remarks = MessageContainerList.GetMessage(executionContext, 4234);
                //Transaction status is successfully updated to ORDERED status
                ParafaitMessageQueueDTO parafaitMessageQueueDTO = new ParafaitMessageQueueDTO(-1, transactionDTO.Guid, ParafaitMessageQueueDTO.EntityNames.Transaction.ToString(), transactionDTO.Status, MessageQueueStatus.Read, true, actionType, remarks, 0);
                ParafaitMessageQueueBL parafaitMessageQueueBL = new ParafaitMessageQueueBL(executionContext, parafaitMessageQueueDTO, sqlTransaction);
                parafaitMessageQueueBL.Save(sqlTransaction);
            }
            else
            {
                if (transactionDTO.Status == Transaction.TrxStatus.PREPARED.ToString())
                {
                    remarks = MessageContainerList.GetMessage(executionContext, 4233);
                    //"Transaction has been updated to PREPARED. Request will be raised with Urban Piper for Marking Order as Prepared"
                }
                ParafaitMessageQueueDTO parafaitMessageQueueDTO = new ParafaitMessageQueueDTO(-1, transactionDTO.Guid, ParafaitMessageQueueDTO.EntityNames.Transaction.ToString(), transactionDTO.Status, MessageQueueStatus.UnRead, true, actionType, remarks, 0);
                ParafaitMessageQueueBL parafaitMessageQueueBL = new ParafaitMessageQueueBL(executionContext, parafaitMessageQueueDTO, sqlTransaction);
                parafaitMessageQueueBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// AmendKOTScheduleTime
        /// </summary>
        /// <param name="timetoAmend"></param>
        /// <param name="sqlTransaction"></param>
        public void AmendKOTScheduleTime(double timetoAmend, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(timetoAmend, sqlTransaction);
            CanAmendKOTSchedule(timetoAmend);
            //foreach (KeyValuePair<int, List<KeyValuePair<int, double>>> keyValuePair in dictionary)
            for (int i = 0; i < transactionDTO.TransactionLinesDTOList.Count; i++)
            {
                if (transactionDTO.TransactionLinesDTOList[i].KDSOrderLineDTOList != null && transactionDTO.TransactionLinesDTOList[i].KDSOrderLineDTOList.Any())
                {
                    TransactionLineBL transactionLineBL = new TransactionLineBL(executionContext, transactionDTO.TransactionLinesDTOList[i]);
                    transactionLineBL.AmendKOTScheduleTime(timetoAmend);
                }
            }
            TransactionOrderDispensingDTO transactionOrderDispensingDTO = transactionDTO.TransctionOrderDispensingDTO;
            if (timetoAmend != 0)
            {
                transactionOrderDispensingDTO.ScheduledDispensingTime = transactionOrderDispensingDTO.ScheduledDispensingTime.Value.AddMinutes(timetoAmend);
            }
            else
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime serverDateTime = lookupValuesList.GetServerDateTime();
                transactionOrderDispensingDTO.ScheduledDispensingTime = serverDateTime;
            }
            TransactionOrderDispensingBL transactionOrderDispensingBL = new TransactionOrderDispensingBL(executionContext, transactionOrderDispensingDTO, false, false, sqlTransaction);
            transactionOrderDispensingBL.Save(sqlTransaction);
            log.LogMethodExit();
        }
        private void CanAmendKOTSchedule(double timeToAmend)
        {
            log.LogMethodEntry();
            if (transactionDTO.Status == Transaction.TrxStatus.CANCELLED.ToString()
                 || transactionDTO.Status == Transaction.TrxStatus.SYSTEMABANDONED.ToString())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 3042);//Transaction has been cancelled. Cannot Ammend KOT schedule
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            if (transactionDTO.TransctionOrderDispensingDTO == null)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4019);
                //Cannot assign rider as Order dispensing details are missing for the transaction
                log.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            DateTime serverDateTime = lookupValuesList.GetServerDateTime();

            if (transactionDTO.TransactionLinesDTOList != null
               && transactionDTO.TransactionLinesDTOList.Exists(tl => tl.CancelledTime == null && tl.KDSOrderLineDTOList != null
                                                                      && tl.KDSOrderLineDTOList.Exists(k => k.ScheduleTime == null)))
            {
                string message = MessageContainerList.GetMessage(executionContext, 4020);
                //Cannot Amend Time for Immediate Orders
                log.Error(message);
                throw new ValidationException(message);
            }
            else if (transactionDTO.TransactionLinesDTOList != null
               && transactionDTO.TransactionLinesDTOList.Exists(tl => tl.CancelledTime == null && tl.KDSOrderLineDTOList != null
                                                                      && tl.KDSOrderLineDTOList.Exists(k => (k.DeliveredTime != null || k.PreparedTime != null))))
            {
                string message = MessageContainerList.GetMessage(executionContext, 4021);
                //Cannot Amend Time. Order already prepared
                log.Error(message);
                throw new ValidationException(message);
            }
            else if (transactionDTO.TransactionLinesDTOList != null
               && transactionDTO.TransactionLinesDTOList.Exists(tl => tl.CancelledTime == null && tl.KDSOrderLineDTOList != null
                                                                      && tl.KDSOrderLineDTOList.Exists(k => (k.PrepareStartTime != null))))
            {
                string message = MessageContainerList.GetMessage(executionContext, 4022);
                //Cannot Amend Time.Preparation has started
                log.Error(message);
                throw new ValidationException(message);
            }
            else if (transactionDTO.TransactionLinesDTOList != null
              && transactionDTO.TransactionLinesDTOList.Exists(tl => tl.CancelledTime == null && tl.KDSOrderLineDTOList != null
                                                                  && tl.KDSOrderLineDTOList.Exists(k => k.ScheduleTime != null
                                                                                     && k.ScheduleTime.Value.AddMinutes(timeToAmend) < serverDateTime)))
            {
                string message = MessageContainerList.GetMessage(executionContext, 4023);
                //Scheduled Time is less than current Time
                log.Error(message);
                throw new ValidationException(message);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// UpdateTransactionPayments
        /// </summary>
        /// <param name="transactionPaymentsDTOList"></param>
        public void UpdateTransactionPayments(List<TransactionPaymentsDTO> transactionPaymentsDTOList)
        {
            log.LogMethodEntry(transactionPaymentsDTOList);
            CanUpdateTransactionPayments();
            TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL(executionContext, transactionPaymentsDTOList);
            transactionPaymentsListBL.UpdateTransactionPaymentModeDetails();
            log.LogMethodExit();
        }
        private void CanUpdateTransactionPayments()
        {
            log.LogMethodEntry();
            if (transactionDTO.Status != Transaction.TrxStatus.CLOSED.ToString())
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4116));
                //Cannot update payment details. Transaction is not in Closed status
            }
            log.LogMethodExit();
        }

        private bool IsValidProduct(int siteId, int productid, int parentProductId, string posMachine)
        {
            log.LogMethodEntry(siteId, productid, parentProductId, posMachine);
            bool validProduct = false;

            if (parentProductId > -1)
            {
                log.Debug("Checking parent validity");
                string parentproductValidationQuery = @"select 1 from ComboProduct
                                                        where isnull(IsActive, 1) = 1 
                                                        and product_id = @parentProductId 
                                                        and ChildProductId in (@ProductIdList)
                                                        union
                                                        select 1 from productmodifiers p, ModifierSetDetails d 
                                                        where p.ModifierSetId = d.ModifierSetId 
                                                        and isnull(p.IsActive, 'Y') = 'Y' 
                                                        and isnull(d.IsActive, 'Y') = 'Y'
                                                        and d.ModifierProductId in (@ProductIdList) 
                                                        and p.ProductId = @parentProductId";

                List<SqlParameter> sqlParamsParent = new List<SqlParameter>();
                sqlParamsParent.Add(new SqlParameter("@ProductIdList", productid.ToString()));
                sqlParamsParent.Add(new SqlParameter("@parentProductId", parentProductId.ToString()));
                DataTable dtProductParent = (new DataAccessHandler().executeSelectQuery(parentproductValidationQuery, sqlParamsParent.ToArray()));

                try
                {
                    if (dtProductParent.Rows != null && dtProductParent.Rows.Count > 0)
                    {
                        //validProduct = true;
                        log.Debug("Valid parent product " + productid + ":" + parentProductId);
                    }
                    else
                    {
                        log.Error("Invalid parent product is being added to transaction siteId:" + siteId + " parent:" + parentProductId + " prd:" + productid + " pos:" + posMachine);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Parent validity check failed " + ex.Message);
                }
            }

            string productValidationQuery = @"SELECT p.product_id
                                                FROM POSMachines POSM,
                                                PRODUCTS p,
                                                ProductsDisplayGroup pd,
                                                ProductDisplayGroupFormat pdf 
                                                WHERE
                                                (POSM.IPAddress = @posMachineName
                                                    OR POSM.Computer_Name = @posMachineName
                                                    OR POSM.PosName = @posMachineName)
                                                AND(p.site_id = @siteId or  @siteId = -1)
                                                AND p.active_flag = 'Y'
                                                AND(p.POSTypeId = POSM.POSTypeId or p.POSTypeId is null)
                                                AND pd.ProductId = p.product_id
                                                AND pdf.Id = pd.DisplayGroupId
                                                AND NOT EXISTS(SELECT POSPE.ProductDisplayGroupFormatId
                                                                FROM POSProductExclusions POSPE
                                                                WHERE POSPE.POSMachineId = POSM.POSMachineId
                                                                AND POSPE.ProductDisplayGroupFormatId = Pdf.Id)
                                                AND(POSM.site_id = @siteId or  @siteId = -1)
                                                AND isnull(p.DisplayInPOS, 'N') = 'Y'
                                                AND p.Product_Id in (@ProductIdList)";

            String productIdToValidate = parentProductId > -1 ? parentProductId.ToString() : productid.ToString();
            log.Debug("Check availability for " + productIdToValidate);
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@siteId", siteId.ToString()));
            sqlParams.Add(new SqlParameter("@ProductIdList", productIdToValidate));
            sqlParams.Add(new SqlParameter("@posMachineName", posMachine.ToString()));
            DataTable dtProduct = (new DataAccessHandler().executeSelectQuery(productValidationQuery, sqlParams.ToArray()));

            try
            {
                if (dtProduct.Rows != null && dtProduct.Rows.Count > 0)
                {
                    int productId = Convert.ToInt32(dtProduct.Rows[0]["product_id"]);
                    validProduct = true;
                    log.Debug("Valid product " + productId);
                }
                else
                {
                    log.Error("Invalid product is being added to transaction siteId:" + siteId + " prd:" + productid + " pos:" + posMachine);
                    validProduct = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Product validity check failed " + ex.Message);
            }

            log.LogMethodExit(validProduct);
            return validProduct;
        }
        /// <summary>
        /// PrintExecuteOnlineReceipt
        /// </summary> 
        /// <returns></returns>
        public TransactionDTO PrintExecuteOnlineReceipt(Transaction transaction, POSPrinterDTO posPrinterDTO, int cardCount)
        {
            log.LogMethodEntry("transaction", posPrinterDTO, cardCount); 
            if (IsVirtualStore() == false || GetVirtualSiteId() == executionContext.SiteId)
            {
                transactionDTO = PrintLocalStoreExecuteOnlineReceipt(transaction, posPrinterDTO, cardCount);
            }
            else
            {
                transactionDTO = PrintVirtualStoreExecuteOnlineReceipt(cardCount);
            } 
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }
        /// <summary>
        /// PrintExecuteOnlineReceipt
        /// </summary> 
        /// <returns></returns>
        public TransactionDTO PrintExecuteOnlineErrorReceipt(Transaction transaction, POSPrinterDTO posPrinterDTO, int cardCount)
        {
            log.LogMethodEntry("transaction", posPrinterDTO, cardCount); 
            if (IsVirtualStore() == false || GetVirtualSiteId() == executionContext.SiteId)
            {
                transactionDTO = PrintLocalStoreExecuteOnlineErrorReceipt(transaction, posPrinterDTO, cardCount);
            }
            else
            {
                transactionDTO = PrintVirtualStoreExecuteOnlineErrorReceipt(cardCount);
            }
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }
        private TransactionDTO PrintVirtualStoreExecuteOnlineReceipt(int cardCount)
        {
            log.LogMethodEntry(cardCount);
            TransactionWebDataHandler transactionWebDataHandler = new TransactionWebDataHandler(executionContext, GetVirtualSiteId());
            TransactionDTO printTransactionDTO = transactionWebDataHandler.DoExecuteOnlineReceipt(transactionDTO, cardCount, true);
            if (printTransactionDTO == null)
            {
                log.LogMethodExit(false, "Unable to print the online transaction through web-api.");
                return null;
            }
            ReceiptClass receipt = GetReceiptClassFromReceiptDTO(printTransactionDTO.ReceiptDTO);
            receipt = ReplaceIssuedCardsTag(cardCount, transactionDTO, receipt);
            if (receipt != null)
            {
                printTransactionDTO.ReceiptDTO = CreateReceiptDTOFromReceiptClass(receipt);
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2412, printTransactionDTO.TransactionId);
                //"Unable to generate receipt for transaction with id &1.";
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(printTransactionDTO);
            return printTransactionDTO;
        } 
        private TransactionDTO PrintLocalStoreExecuteOnlineReceipt(Transaction transaction, POSPrinterDTO posPrinterDTO, int cardCount)
        {
            log.LogMethodEntry("transaction", posPrinterDTO, cardCount);
            POSPrinterDTO tempPOSPrinterDTO = new POSPrinterDTO(posPrinterDTO);
            int executeOnlineReceiptId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "EXECUTE_ONLINE_TRX_RECEIPT", -1);
            if (executeOnlineReceiptId == -1)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4901) + MessageContainerList.GetMessage(executionContext, "for") + ": EXECUTE_ONLINE_TRX_RECEIPT");
                //No Template Found
            }
            else
            {
                tempPOSPrinterDTO.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(executionContext, executeOnlineReceiptId, true)).ReceiptPrintTemplateHeaderDTO;
            }
            ReceiptClass receipt = POSPrint.PrintReceipt(transaction, tempPOSPrinterDTO, -1, false);
            receipt = ReplaceIssuedCardsTag(cardCount, transaction.TransactionDTO, receipt);
            if (receipt != null)
            {
                transactionDTO.ReceiptDTO = CreateReceiptDTOFromReceiptClass(receipt);
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2412, transactionDTO.TransactionId);
                //"Unable to generate receipt for transaction with id &1.";
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }
        private TransactionDTO PrintVirtualStoreExecuteOnlineErrorReceipt(int cardCount)
        {
            log.LogMethodEntry(cardCount);
            TransactionWebDataHandler transactionWebDataHandler = new TransactionWebDataHandler(executionContext, GetVirtualSiteId());
            TransactionDTO printTransactionDTO = transactionWebDataHandler.DoExecuteOnlineReceipt(transactionDTO, cardCount, false);
            if (printTransactionDTO == null)
            {
                log.LogMethodExit(false, "Unable to print the online transaction through web-api.");
                return null;
            }
            //ReceiptClass receipt = GetReceiptClassFromReceiptDTO(printTransactionDTO.ReceiptDTO);
            //receipt = ReplaceIssuedCardTag(cardCount, receipt);
            //if (receipt != null)
            //{
            //    printTransactionDTO.ReceiptDTO = CreateReceiptDTOFromReceiptClass(receipt);
            //}
            //else
            //{
            //    string errorMessage = MessageContainerList.GetMessage(executionContext, 2412, printTransactionDTO.TransactionId);
            //    //"Unable to generate receipt for transaction with id &1.";
            //    throw new Exception(errorMessage);
            //}
            log.LogMethodExit(printTransactionDTO);
            return printTransactionDTO;
        }
        private TransactionDTO PrintLocalStoreExecuteOnlineErrorReceipt(Transaction transaction, POSPrinterDTO posPrinterDTO, int cardCount)
        {
            log.LogMethodEntry("transaction", posPrinterDTO, cardCount);
            POSPrinterDTO tempPOSPrinterDTO = new POSPrinterDTO(posPrinterDTO);
            int executeOnlineErrorReceiptId = ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "EXECUTE_ONLINE_TRX_ERROR_RECEIPT", -1);
            if (executeOnlineErrorReceiptId == -1)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 4901) + MessageContainerList.GetMessage(executionContext, "for") + ": EXECUTE_ONLINE_TRX_ERROR_RECEIPT");
                //No Template Found
            }
            else
            {
                tempPOSPrinterDTO.ReceiptPrintTemplateHeaderDTO = (new ReceiptPrintTemplateHeaderBL(executionContext, executeOnlineErrorReceiptId, true)).ReceiptPrintTemplateHeaderDTO;
            }
            ReceiptClass receipt = POSPrint.PrintReceipt(transaction, tempPOSPrinterDTO, -1, false);
            receipt = ReplaceIssuedCardsTag(cardCount, transaction.TransactionDTO, receipt);
            if (receipt != null)
            {
                transactionDTO.ReceiptDTO = CreateReceiptDTOFromReceiptClass(receipt);
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2412, transactionDTO.TransactionId);
                //"Unable to generate receipt for transaction with id &1.";
                throw new Exception(errorMessage);
            }
            log.LogMethodExit(transactionDTO);
            return transactionDTO;
        }
        private static ReceiptClass ReplaceIssuedCardsTag(int cardCount, TransactionDTO transactionDTO, ReceiptClass receipt)
        {
            log.LogMethodEntry(cardCount, transactionDTO, receipt);
            //receipt.ReceiptLines[rLines].Data[receiptColumnData.Sequence - 1]
            for (int i = 0; i < receipt.TotalLines; i++)
            {
                for (int j = 0; j < receipt.ReceiptLines[i].Data.Length; j++)
                {
                    if (receipt.ReceiptLines[i].Data[j] != null)
                    {
                        if (receipt.ReceiptLines[i].Data[j].Contains("@TodaysIssuedCards"))
                        {
                            receipt.ReceiptLines[i].Data[j] = receipt.ReceiptLines[i].Data[j].Replace("@TodaysIssuedCards", cardCount.ToString());
                        }
                    }
                }
            }
            log.LogMethodExit(receipt);
            return receipt;
        }
    }

    /// <summary>
    /// Manages the list of transaction
    /// </summary>
    public class TransactionListBL
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public TransactionListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of Transaction matching the search criteria
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransNaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetTransactionCount(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            int transactionCount = transactionDataHandler.GetTransactionCount(searchParameters);
            log.LogMethodExit(transactionCount);
            return transactionCount;
        }

        public DataTable GetRefreshHeaderRecords(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters, string trxNoHeading,
                                               bool showAmountFieldsTransaction = false, int userId = -1, bool transactionDetailsEntitlement = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            DataTable transactionDTOList;
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            if (transactionDetailsEntitlement)
            {
                transactionDTOList = transactionDataHandler.GetTransactionDetailsWithEntitlements(searchParameters, trxNoHeading, showAmountFieldsTransaction, userId);
            }
            else
            {
                transactionDTOList = transactionDataHandler.GetTrxHeaderDetails(searchParameters, trxNoHeading, showAmountFieldsTransaction, userId);
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        /// <summary>
        /// This method returns the lines details for WMS view transaction Records
        /// </summary>
        /// <param name="trxId"></param>
        /// <param name="showAmountFieldsTransaction"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public DataTable GetRefreshedTransactionLines(int trxId, bool showAmountFieldsTransaction = false, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(trxId, showAmountFieldsTransaction);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            DataTable refreshedLines = transactionDataHandler.GetRefreshLines(trxId, showAmountFieldsTransaction);
            log.LogMethodExit(refreshedLines);
            return refreshedLines;
        }

        /// <summary>
        /// Returns the transaction list
        /// </summary>
        public List<TransactionDTO> GetTransactionDTOList(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters, Utilities utilities, SqlTransaction sqlTransaction = null,
            int pageNumber = 0, int pageSize = 10, bool buildChildRecords = false, bool buildTickets = false, bool buildReceipt = false)
        {
            //***Important note*** Any parameter changes to this method would need testing of CustomerSignedWaiverBL.CanDeactivate method
            log.LogMethodEntry(searchParameters, sqlTransaction, pageNumber, pageSize, buildChildRecords, buildTickets, buildReceipt);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            List<TransactionDTO> transactionDTOList = transactionDataHandler.GetTransactionDTOList(searchParameters, pageNumber, pageSize);

            if (transactionDTOList != null && searchParameters != null)
            {
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> customerIdentifierList = searchParameters.Where(x => x.Key == TransactionDTO.SearchByParameters.CUSTOMER_IDENTIFIER).ToList();
                if (customerIdentifierList != null && customerIdentifierList.Any())
                {
                    KeyValuePair<TransactionDTO.SearchByParameters, string> searchParam = customerIdentifierList[0];
                    List<TransactionDTO> tempTransactionDTOList = new List<TransactionDTO>();
                    foreach (TransactionDTO TransactionDTO in transactionDTOList)
                    {
                        if (TransactionDTO.Status == "CLOSED" && TransactionDTO.CustomerIdentifier != null)
                        {
                            bool compareResult = false;
                            //string decryptedCustomerReference = Encryption.Decrypt(TransactionDTO.CustomerIdentifier);
                            string[] transactionCustomerIdentifierArray = Encryption.Decrypt(TransactionDTO.CustomerIdentifier).Split(new[] { '|' });
                            string[] inputCustomerIdentifierArray = searchParam.Value.Split(new[] { '|' });
                            //int emailResult = 1, phoneResult = 1;
                            bool emailResult = false, phoneResult = false;
                            if (transactionCustomerIdentifierArray.Length > 0 && inputCustomerIdentifierArray.Length > 0)
                            {
                                if (string.IsNullOrEmpty(transactionCustomerIdentifierArray[0].Trim()) == false
                                    && string.IsNullOrEmpty(inputCustomerIdentifierArray[0].Trim()) == false)
                                {
                                    //emailResult = string.Compare(transactionCustomerIdentifierArray[0], inputCustomerIdentifierArray[0]);
                                    emailResult = transactionCustomerIdentifierArray[0].ToUpperInvariant().Equals(inputCustomerIdentifierArray[0].ToUpperInvariant());
                                }
                                if (transactionCustomerIdentifierArray.Length > 1 && inputCustomerIdentifierArray.Length > 1
                                    && string.IsNullOrEmpty(transactionCustomerIdentifierArray[1].Trim()) == false
                                    && string.IsNullOrEmpty(inputCustomerIdentifierArray[1].Trim()) == false)
                                {
                                    // phoneResult = string.Compare(transactionCustomerIdentifierArray[1], inputCustomerIdentifierArray[1]);
                                    phoneResult = transactionCustomerIdentifierArray[1].ToUpperInvariant().Equals(inputCustomerIdentifierArray[1].ToUpperInvariant());
                                }
                                if (emailResult || phoneResult)
                                {
                                    compareResult = true;
                                }
                            }

                            if (compareResult)
                            {
                                tempTransactionDTOList.Add(TransactionDTO);
                            }
                        }
                    }
                    transactionDTOList = tempTransactionDTOList;
                }
            }
             
            transactionDTOList = BuildChildRecords(utilities != null ? utilities.ExecutionContext : executionContext, transactionDTOList, buildChildRecords, buildTickets, buildReceipt, sqlTransaction);
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        private List<TransactionDTO> BuildChildRecords(ExecutionContext executionContextInput, List<TransactionDTO> transactionDTOList, bool buildChildRecords, bool buildTickets, bool buildReceipt,
            SqlTransaction sqlTrx)
        {
            log.LogMethodEntry(executionContextInput, buildChildRecords, buildTickets, buildReceipt, sqlTrx);
            if (transactionDTOList != null && transactionDTOList.Any() && buildChildRecords)
            {
                foreach (TransactionDTO currTransactionDTO in transactionDTOList)
                {
                    ExecutionContext executionContext = new ExecutionContext(executionContextInput.GetUserId(),
                        executionContextInput.GetSiteId(), executionContextInput.GetMachineId(),
                        executionContextInput.GetUserPKId(), executionContextInput.GetIsCorporate(),
                        executionContextInput.GetLanguageId());
                    executionContext.SetIsCorporate(executionContextInput.GetIsCorporate());
                    if (executionContextInput.GetIsCorporate())
                    {
                        executionContext.SetSiteId(currTransactionDTO.SiteId);
                    }
                    else
                    {
                        executionContext.SetSiteId(-1);
                    }
                    executionContext.SetUserId(executionContextInput.GetUserId());

                    TransactionBL transactionBL = new TransactionBL(executionContext, currTransactionDTO.TransactionId, sqlTrx);
                    Transaction currTransaction = transactionBL.Transaction;
                    currTransactionDTO.TransctionOrderDispensingDTO = currTransaction.TransctionOrderDispensingDTO;
                    if (currTransaction != null && currTransaction.TrxLines != null && currTransaction.TrxLines.Any())
                    {
                        currTransactionDTO.TransactionLinesDTOList = new List<TransactionLineDTO>();
                        foreach (Transaction.TransactionLine trxLine in currTransaction.TrxLines)
                        {
                            trxLine.TransactionLineDTO.TransactionId = currTransaction.Trx_id;
                            if (trxLine.ProductTypeCode.Equals("ATTRACTION"))
                            {
                                trxLine.TransactionLineDTO.ProductDetail = trxLine.AttractionDetails;
                            }
                            else if (trxLine.ProductTypeCode.Equals("LOCKER"))
                            {
                                string dateFormat = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT");
                                trxLine.TransactionLineDTO.ProductDetail = trxLine.LockerName + " : " + trxLine.LockerNumber + " : " + (trxLine.lockerAllocationDTO != null ? trxLine.lockerAllocationDTO.ValidToTime.ToString(dateFormat) : "");
                            }
                            currTransactionDTO.TransactionLinesDTOList.Add(trxLine.TransactionLineDTO);
                        }

                        //String message = "";
                        currTransaction.TransactionInfo.createTransactionInfo(currTransaction.Trx_id);

                        if (executionContext.GetIsCorporate())
                        {
                            TimeZoneUtil timeZoneUtil = new TimeZoneUtil();
                            int offsetTimeSecs = timeZoneUtil.GetOffSetDuration(executionContext.GetSiteId(), currTransaction.TrxDate.Date);
                            currTransaction.TrxDate = currTransaction.TrxDate.AddSeconds(offsetTimeSecs);
                            log.Debug("currTransaction.TrxDate" + currTransaction.TrxDate);
                            // Attraction bookings need to be offset before printing
                            for (int i = 0; i < currTransaction.TrxLines.Count; i++)
                            {
                                if (currTransaction.TrxLines[i].LineAtb != null)
                                {
                                    AttractionBooking atb = currTransaction.TrxLines[i].LineAtb;
                                    atb.AttractionBookingDTO.ScheduleFromDate = atb.AttractionBookingDTO.ScheduleFromDate.AddSeconds(-1 * offsetTimeSecs);
                                    atb.AttractionBookingDTO.ScheduleToDate = atb.AttractionBookingDTO.ScheduleToDate.AddSeconds(-1 * offsetTimeSecs);
                                    currTransaction.TrxLines[i].AttractionDetails = atb.AttractionBookingDTO.AttractionScheduleName + ":" + atb.AttractionBookingDTO.ScheduleFromDate.ToString("d-MMM-yyyy h:mm tt");
                                    log.Debug(atb.AttractionBookingDTO.BookingId + ":" + atb.AttractionBookingDTO.ScheduleFromDate + ":" + atb.AttractionBookingDTO.ScheduleToDate + ":" + currTransaction.TrxLines[i].AttractionDetails);
                                }
                            }
                        }

                        currTransactionDTO.TransactionNetAmount = Convert.ToDecimal(currTransaction.Net_Transaction_Amount.ToString(CultureInfo.InvariantCulture));
                        currTransactionDTO.TransactionAmount = Convert.ToDecimal(currTransaction.Transaction_Amount.ToString(CultureInfo.InvariantCulture));
                        currTransactionDTO.TaxAmount = Convert.ToDecimal(currTransaction.Tax_Amount);

                        log.Debug("checking customer identifier " + currTransaction.customerIdentifier);
                        if (!string.IsNullOrEmpty(currTransaction.customerIdentifier) && currTransaction.customerIdentifier.IndexOf("|") == -1)
                        {
                            try
                            {
                                log.Debug(currTransaction.customerIdentifier);
                                String decryptedCustomerIdentifier = Encryption.Decrypt(currTransaction.customerIdentifier);
                                currTransactionDTO.CustomerIdentifier = currTransaction.customerIdentifier;
                                log.Debug(currTransaction.customerIdentifier);
                                string[] customerIdentifierStringArray = decryptedCustomerIdentifier.Split(new[] { '|' });
                                currTransaction.customerIdentifiersList = new List<string>();
                                foreach (string identifier in customerIdentifierStringArray)
                                {
                                    currTransaction.customerIdentifiersList.Add(identifier);
                                }
                                log.Debug("Customer Identifier List " + string.Join(":", currTransaction.customerIdentifiersList));
                            }
                            catch (Exception ex)
                            {
                                log.Debug("Exception while converting customer identifier");
                            }
                        }

                        string printReceipt = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ALLOW_TRX_PRINT_BEFORE_SAVING");
                        log.Debug("ALLOW_TRX_PRINT_BEFORE_SAVING for site " + executionContext.SiteId + ":" + printReceipt);
                        log.Debug(" Trx Status " + transactionBL.TransactionDTO.Status);
                        if (transactionBL.TransactionDTO.Status == Transaction.TrxStatus.CLOSED.ToString())
                        {

                             transactionBL.BuildTransactionReceiptAndTickets(buildReceipt, buildTickets);
                            if (buildTickets)
                            {
                                currTransactionDTO.Tickets = transactionBL.TransactionDTO.Tickets;
                                currTransactionDTO.TicketsHTML = transactionBL.TransactionDTO.TicketsHTML;
                            }

                            if (buildReceipt)
                            {
                                currTransactionDTO.Receipt = transactionBL.TransactionDTO.Receipt;
                                currTransactionDTO.ReceiptHTML = transactionBL.TransactionDTO.ReceiptHTML;
                            }
                        }
                        else
                        {
                            if (buildTickets)
                            {
                                currTransactionDTO.Tickets = "";
                                currTransactionDTO.TicketsHTML = "";
                            }

                            if (buildReceipt)
                            {
                                currTransactionDTO.Receipt = "";
                                currTransactionDTO.ReceiptHTML = "";
                            }
                        }

                        // Load payments ALOHA BSP changes
                        currTransactionDTO.TrxPaymentsDTOList = currTransaction.TransactionPaymentsDTOList;
                    }
                }
            }
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        /// <summary>
        /// Returns Purchased Transaction lines from the given time range
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<PurchasedTransactionLineStruct> GetPurchasedTransactionLineDTOList(DateTime fromDate, DateTime toDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(fromDate, toDate);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            List<PurchasedTransactionLineStruct> purchasedTransactionLineStruct = transactionDataHandler.GetPurchasedTransactionLineDTOList(fromDate, toDate);
            log.LogMethodExit(purchasedTransactionLineStruct);
            return purchasedTransactionLineStruct;
        }



        /// <summary>
        /// Returns the online transactions matching the criteria
        /// </summary>
        /// <param name="originalSystemReference">Original transaction Id</param>
        /// <param name="transactionOtp">transaction OTP</param>
        /// <param name="utilities">utilities</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns></returns>
        public List<TransactionDTO> GetOnlineTransactionDTOList(string originalSystemReference, string transactionOtp, Utilities utilities, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(originalSystemReference, transactionOtp, utilities, sqlTransaction);
            List<TransactionDTO> result;
            if (string.IsNullOrWhiteSpace(originalSystemReference) &&
                string.IsNullOrWhiteSpace(transactionOtp))
            {
                result = new List<TransactionDTO>();
                log.LogMethodExit(result, "Both originalSystemReference and transactionOTP is empty.");
                return result;
            }

            List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
            if (string.IsNullOrWhiteSpace(originalSystemReference) == false)
            {
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ORIGINAL_SYSTEM_REFERENCE, originalSystemReference));
            }
            if (string.IsNullOrWhiteSpace(transactionOtp) == false)
            {
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_OTP, transactionOtp));
            }

            if (string.IsNullOrWhiteSpace(originalSystemReference) &&
                string.IsNullOrWhiteSpace(transactionOtp) == false)
            {
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                DateTime serverDateTime = lookupValuesList.GetServerDateTime();
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_FROM_DATE, serverDateTime.AddDays(-300).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                if (IsVirtualStore())
                {
                    searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.TRANSACTION_TO_DATE, serverDateTime.AddDays(300).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.STATUS, "BOOKING,RESERVED,OPEN,INITIATED,ORDERED,PREPARED,CLOSED,PENDING"));
            }

            if (IsVirtualStore())
            {
                TransactionWebDataHandler transactionWebDataHandler = new TransactionWebDataHandler(executionContext, GetVirtualSiteId());
                result = transactionWebDataHandler.GetTransactionDTOList(searchParameters, 0, 100, true);
            }
            else
            {
                result = GetTransactionDTOList(searchParameters, utilities, sqlTransaction, 0, 100, true);
            }

            if (result == null)
            {
                result = new List<TransactionDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }



        /// <summary>
        /// Returns the online transactions matching the criteria
        /// </summary>
        /// <param name="customerDTO"></param>
        /// <param name="utilities">utilities</param>
        /// <param name="sqlTransaction">sql transaction</param>
        /// <returns></returns>
        public List<TransactionDTO> GetOnlineTransactionDTOList(CustomerDTO customerDTO, Utilities utilities, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry("customerDTO", utilities, sqlTransaction);
            List<TransactionDTO> result;
            if (customerDTO == null || customerDTO.Id < 0)
            {
                result = new List<TransactionDTO>();
                log.LogMethodExit(result, "No valid customer selected.");
                return result;
            }

            if (IsVirtualStore())
            {
                TransactionWebDataHandler transactionWebDataHandler = new TransactionWebDataHandler(executionContext, GetVirtualSiteId());
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_GUID_ID, customerDTO.Guid));
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ONLINE_ONLY, true.ToString()));
                result = transactionWebDataHandler.GetTransactionDTOList(searchParameters, 0, 100, true);
            }
            else
            {
                List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.CUSTOMER_ID, customerDTO.Id.ToString()));
                searchParameters.Add(new KeyValuePair<TransactionDTO.SearchByParameters, string>(TransactionDTO.SearchByParameters.ONLINE_ONLY, true.ToString()));
                result = GetTransactionDTOList(searchParameters, utilities, sqlTransaction, 0, 100, true);
            }

            if (result == null)
            {
                result = new List<TransactionDTO>();
            }
            log.LogMethodExit(result);
            return result;
        }

        private bool IsVirtualStore()
        {
            string virtualStoreSiteId =
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "VIRTUAL_STORE_SITE_ID");
            return string.IsNullOrWhiteSpace(virtualStoreSiteId) == false;
        }

        private int GetVirtualSiteId()
        {
            return ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext, "VIRTUAL_STORE_SITE_ID");
        }


        /// <summary>
        /// Build the sheet object templete ViewTransaction for Webmanagement
        /// </summary>
        /// <returns></returns>
        public List<Sheet> BuildTemplete(bool loadDetailsOnly, DataTable headerDetails, DataTable linedetails, DateTime? fromDate, DateTime? toDate, string siteName, int trxId)
        {
            try
            {
                log.LogMethodEntry(loadDetailsOnly, headerDetails, linedetails, fromDate, toDate, siteName, trxId);
                List<Sheet> sheets = new List<Sheet>();
                Sheet trxDetailsSheet = new Sheet();
                Sheet trxHeadersSheet = new Sheet();
                Sheet trxRefreshLinesSheet = new Sheet();
                ///All column Headings are in a headerRow object
                Row headerRow = new Row();
                Row refreshHeaderRow = new Row();
                if (loadDetailsOnly)
                {
                    ///All defaultvalues for attributes are in defaultValueRow object
                    Row defaultFileNameValueRow = new Row();
                    defaultFileNameValueRow.AddCell(new Cell());
                    defaultFileNameValueRow.AddCell(new Cell(""));
                    defaultFileNameValueRow.AddCell(new Cell("Transaction Details"));
                    trxDetailsSheet.AddRow(defaultFileNameValueRow);

                    ///Adding DefaultValue - Site:
                    Row defaultSiteValueRow = new Row();
                    defaultSiteValueRow.AddCell(new Cell());
                    defaultSiteValueRow.AddCell(new Cell("Site:"));
                    defaultSiteValueRow.AddCell(new Cell(siteName));
                    trxDetailsSheet.AddRow(defaultSiteValueRow);

                    ///Adding DefaultValue - From:
                    Row defaultReqFromDateValueRow = new Row();
                    defaultReqFromDateValueRow.AddCell(new Cell());
                    defaultReqFromDateValueRow.AddCell(new Cell("From:"));
                    defaultReqFromDateValueRow.AddCell(new Cell(fromDate.ToString()));
                    trxDetailsSheet.AddRow(defaultReqFromDateValueRow);

                    ///Adding DefaultValue - To:
                    Row defaultReqToDateValueRow = new Row();
                    defaultReqToDateValueRow.AddCell(new Cell());
                    defaultReqToDateValueRow.AddCell(new Cell("To:"));
                    defaultReqToDateValueRow.AddCell(new Cell(toDate.ToString()));
                    trxDetailsSheet.AddRow(defaultReqToDateValueRow);

                    ///Adding DefaultValue - Run At:
                    Row defaultRunAtValueRow = new Row();
                    defaultRunAtValueRow.AddCell(new Cell());
                    defaultRunAtValueRow.AddCell(new Cell("Run At:"));
                    defaultRunAtValueRow.AddCell(new Cell(DateTime.Now.ToString()));
                    trxDetailsSheet.AddRow(defaultRunAtValueRow);

                    ///Adding DefaultValue - Run At:
                    Row defaultRunByValueRow = new Row();
                    defaultRunByValueRow.AddCell(new Cell());
                    defaultRunByValueRow.AddCell(new Cell("Run By:"));
                    defaultRunByValueRow.AddCell(new Cell(executionContext.GetUserId()));
                    trxDetailsSheet.AddRow(defaultRunByValueRow);

                    Row defaultEmptyValueRow = new Row();
                    trxDetailsSheet.AddRow(defaultEmptyValueRow);

                    ///Mapping the DataTable values to Excel Sheet
                    TransactionDetailsExcelDTODefinition transactionDetailsExcelDTODefinition = new TransactionDetailsExcelDTODefinition(executionContext, "");
                    List<TransactionDetailsDTODefinition> transactionDetailsDTOList = new List<TransactionDetailsDTODefinition>();
                    for (int i = 0; i < headerDetails.Rows.Count; i++)
                    {
                        TransactionDetailsDTODefinition transactionDetailsDefinitionObject = new TransactionDetailsDTODefinition();
                        transactionDetailsDefinitionObject.TransactionId = headerDetails.Rows[i]["ID"] == DBNull.Value ? -1 : Convert.ToInt32(headerDetails.Rows[i]["ID"]);
                        transactionDetailsDefinitionObject.TransactionNumber = headerDetails.Rows[i]["Trx No"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["Trx No"]);
                        transactionDetailsDefinitionObject.TransactionDate = headerDetails.Rows[i]["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(headerDetails.Rows[i]["Date"]);
                        transactionDetailsDefinitionObject.Amount = headerDetails.Rows[i]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["Amount"]);
                        transactionDetailsDefinitionObject.TransactionNetAmount = headerDetails.Rows[i]["Net_amount"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["Net_amount"]);
                        transactionDetailsDefinitionObject.PaymentMode = headerDetails.Rows[i]["pay_mode"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["pay_mode"]);
                        transactionDetailsDefinitionObject.PosMachine = headerDetails.Rows[i]["POS"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["POS"]);
                        transactionDetailsDefinitionObject.UserName = headerDetails.Rows[i]["Cashier"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["Cashier"]);
                        transactionDetailsDefinitionObject.ProductName = headerDetails.Rows[i]["Product"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["Product"]);
                        transactionDetailsDefinitionObject.Price = headerDetails.Rows[i]["Price/Disc%"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["Price/Disc%"]);
                        transactionDetailsDefinitionObject.Amount = headerDetails.Rows[i]["Line_amount"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["Line_amount"]);
                        transactionDetailsDefinitionObject.WaiverCustomer = headerDetails.Rows[i]["Waiver_Customer"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["Waiver_Customer"]);
                        transactionDetailsDefinitionObject.CardNumber = headerDetails.Rows[i]["card_number"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["card_number"]);
                        transactionDetailsDefinitionObject.Credits = headerDetails.Rows[i]["credits"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["credits"]);
                        transactionDetailsDefinitionObject.Courtesy = headerDetails.Rows[i]["courtesy"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["courtesy"]);
                        transactionDetailsDefinitionObject.Bonus = headerDetails.Rows[i]["bonus"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["bonus"]);
                        transactionDetailsDefinitionObject.Time = headerDetails.Rows[i]["time"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["time"]);
                        transactionDetailsDefinitionObject.Tickets = headerDetails.Rows[i]["tickets"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["tickets"]);
                        transactionDetailsDefinitionObject.TaxName = headerDetails.Rows[i]["tax_name"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["tax_name"]);
                        transactionDetailsDefinitionObject.TaxPercentage = headerDetails.Rows[i]["Tax %"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["Tax %"]);
                        transactionDetailsDefinitionObject.Quantity = headerDetails.Rows[i]["quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["quantity"]);
                        transactionDetailsDefinitionObject.LoyaltyPoints = headerDetails.Rows[i]["loyalty_points"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[i]["loyalty_points"]);
                        transactionDetailsDefinitionObject.LineId = headerDetails.Rows[i]["Line"] == DBNull.Value ? -1 : Convert.ToInt32(headerDetails.Rows[i]["Line"]);
                        transactionDetailsDefinitionObject.Status = headerDetails.Rows[i]["Status"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[i]["Status"]);
                        transactionDetailsExcelDTODefinition.Configure(transactionDetailsDefinitionObject);
                        transactionDetailsDTOList.Add(transactionDetailsDefinitionObject);
                    }
                    transactionDetailsExcelDTODefinition.BuildHeaderRow(headerRow);
                    trxDetailsSheet.AddRow(headerRow);
                    foreach (TransactionDetailsDTODefinition data in transactionDetailsDTOList)
                    {
                        Row row = new Row();
                        transactionDetailsExcelDTODefinition.Serialize(row, data);
                        trxDetailsSheet.AddRow(row);
                    }
                    sheets.Add(trxDetailsSheet);
                    log.LogMethodExit(sheets);
                    return sheets;
                }
                else
                {
                    if (headerDetails != null)
                    {
                        ///HeaderExcel
                        ///All defaultvalues for attributes are in defaultValueRow object
                        Row defaultFileNameValueRow = new Row();
                        defaultFileNameValueRow.AddCell(new Cell());
                        defaultFileNameValueRow.AddCell(new Cell(""));
                        defaultFileNameValueRow.AddCell(new Cell("Transactions"));
                        trxHeadersSheet.AddRow(defaultFileNameValueRow);

                        ///Adding DefaultValue - Site:
                        Row defaultSiteValueRow = new Row();
                        defaultSiteValueRow.AddCell(new Cell());
                        defaultSiteValueRow.AddCell(new Cell("Site:"));
                        defaultSiteValueRow.AddCell(new Cell(siteName));
                        trxHeadersSheet.AddRow(defaultSiteValueRow);

                        ///Adding DefaultValue - From:
                        Row defaultReqFromDateValueRow = new Row();
                        defaultReqFromDateValueRow.AddCell(new Cell());
                        defaultReqFromDateValueRow.AddCell(new Cell("From:"));
                        defaultReqFromDateValueRow.AddCell(new Cell(fromDate.ToString()));
                        trxHeadersSheet.AddRow(defaultReqFromDateValueRow);

                        ///Adding DefaultValue - To:
                        Row defaultReqToDateValueRow = new Row();
                        defaultReqToDateValueRow.AddCell(new Cell());
                        defaultReqToDateValueRow.AddCell(new Cell("To:"));
                        defaultReqToDateValueRow.AddCell(new Cell(toDate.ToString()));
                        trxHeadersSheet.AddRow(defaultReqToDateValueRow);

                        ///Adding DefaultValue - Run At:
                        Row defaultRunAtValueRow = new Row();
                        defaultRunAtValueRow.AddCell(new Cell());
                        defaultRunAtValueRow.AddCell(new Cell("Run At:"));
                        defaultRunAtValueRow.AddCell(new Cell(DateTime.Now.ToString()));
                        trxHeadersSheet.AddRow(defaultRunAtValueRow);

                        ///Adding DefaultValue - Run At:
                        Row defaultRunByValueRow = new Row();
                        defaultRunByValueRow.AddCell(new Cell());
                        defaultRunByValueRow.AddCell(new Cell("Run By:"));
                        defaultRunByValueRow.AddCell(new Cell(executionContext.GetUserId()));
                        trxHeadersSheet.AddRow(defaultRunByValueRow);

                        Row defaultEmptyValueRow = new Row();
                        trxHeadersSheet.AddRow(defaultEmptyValueRow);

                        ///Mapping the DataTable values to Excel Sheet - ForTrxHeaders
                        TransactionHeadersExcelDTODefinition transactionHeadersExcelDTODefinition = new TransactionHeadersExcelDTODefinition(executionContext, "");
                        List<TransactionDTO> transactionDTOList = new List<TransactionDTO>();

                        for (int k = 0; k < headerDetails.Rows.Count; k++)
                        {
                            TransactionDTO transactionDTOObject = new TransactionDTO();
                            transactionDTOObject.TransactionId = headerDetails.Rows[k]["ID"] == DBNull.Value ? -1 : Convert.ToInt32(headerDetails.Rows[k]["ID"]);
                            transactionDTOObject.TransactionNumber = headerDetails.Rows[k]["Trx No"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["Trx No"]);
                            transactionDTOObject.TableNumber = headerDetails.Rows[k]["Table#"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["Table#"]);
                            transactionDTOObject.TransactionDate = headerDetails.Rows[k]["Date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(headerDetails.Rows[k]["Date"]);
                            transactionDTOObject.TransactionAmount = headerDetails.Rows[k]["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["Amount"]);
                            transactionDTOObject.TaxAmount = headerDetails.Rows[k]["Tax"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["Tax"]);
                            transactionDTOObject.TransactionNetAmount = headerDetails.Rows[k]["Net_amount"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["Net_amount"]);
                            transactionDTOObject.Paid = headerDetails.Rows[k]["Paid"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["Paid"]);
                            transactionDTOObject.TransactionDiscountPercentage = headerDetails.Rows[k]["avg_disc_%"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["avg_disc_%"]);
                            transactionDTOObject.PaymentModeName = headerDetails.Rows[k]["pay_mode"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["pay_mode"]);
                            transactionDTOObject.PosMachine = headerDetails.Rows[k]["POS"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["POS"]);
                            transactionDTOObject.UserName = headerDetails.Rows[k]["Cashier"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["Cashier"]);
                            transactionDTOObject.CustomerName = headerDetails.Rows[k]["Customer_Name"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["Customer_Name"]);
                            transactionDTOObject.CashAmount = headerDetails.Rows[k]["Cash"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["Cash"]);
                            transactionDTOObject.CreditCardAmount = headerDetails.Rows[k]["C_C_Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["C_C_Amount"]);
                            transactionDTOObject.GameCardAmount = headerDetails.Rows[k]["Game_card_amt"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["Game_card_amt"]);
                            transactionDTOObject.OtherPaymentModeAmount = headerDetails.Rows[k]["Other_Mode_Amt"] == DBNull.Value ? 0 : Convert.ToDecimal(headerDetails.Rows[k]["Other_Mode_Amt"]);
                            transactionDTOObject.PaymentReference = headerDetails.Rows[k]["Ref"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["Ref"]);
                            transactionDTOObject.Status = headerDetails.Rows[k]["Status"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["Status"]);
                            transactionDTOObject.Remarks = headerDetails.Rows[k]["Remarks"] == DBNull.Value ? "" : Convert.ToString(headerDetails.Rows[k]["Remarks"]);
                            transactionHeadersExcelDTODefinition.Configure(transactionDTOObject);
                            transactionDTOList.Add(transactionDTOObject);
                        }

                        transactionHeadersExcelDTODefinition.BuildHeaderRow(headerRow);
                        trxHeadersSheet.AddRow(headerRow);
                        foreach (TransactionDTO data in transactionDTOList)
                        {
                            Row trxHeaderRow = new Row();
                            transactionHeadersExcelDTODefinition.Serialize(trxHeaderRow, data);
                            trxHeadersSheet.AddRow(trxHeaderRow);
                        }
                        sheets.Add(trxHeadersSheet);
                        log.LogMethodExit(sheets);
                    }
                    if (linedetails != null)
                    {
                        ///RefershLine Excel
                        ///All defaultvalues for attributes are in defaultValueRow object
                        Row defaultFileNameRow = new Row();
                        defaultFileNameRow.AddCell(new Cell());
                        defaultFileNameRow.AddCell(new Cell(""));
                        defaultFileNameRow.AddCell(new Cell("Transaction Details" + "-" + "TrxID" + "-" + trxId));
                        trxRefreshLinesSheet.AddRow(defaultFileNameRow);

                        ///Adding DefaultValue - Site:
                        Row defaultSiteRow = new Row();
                        defaultSiteRow.AddCell(new Cell());
                        defaultSiteRow.AddCell(new Cell("Site:"));
                        defaultSiteRow.AddCell(new Cell(siteName));
                        trxRefreshLinesSheet.AddRow(defaultSiteRow);

                        ///Adding DefaultValue - From:
                        Row defaultReqFromDateRow = new Row();
                        defaultReqFromDateRow.AddCell(new Cell());
                        defaultReqFromDateRow.AddCell(new Cell("From:"));
                        defaultReqFromDateRow.AddCell(new Cell(fromDate.ToString()));
                        trxRefreshLinesSheet.AddRow(defaultReqFromDateRow);

                        ///Adding DefaultValue - To:
                        Row defaultReqToDateRow = new Row();
                        defaultReqToDateRow.AddCell(new Cell());
                        defaultReqToDateRow.AddCell(new Cell("To:"));
                        defaultReqToDateRow.AddCell(new Cell(toDate.ToString()));
                        trxRefreshLinesSheet.AddRow(defaultReqToDateRow);

                        ///Adding DefaultValue - Run At:
                        Row defaultRunAtRow = new Row();
                        defaultRunAtRow.AddCell(new Cell());
                        defaultRunAtRow.AddCell(new Cell("Run At:"));
                        defaultRunAtRow.AddCell(new Cell(DateTime.Now.ToString()));
                        trxRefreshLinesSheet.AddRow(defaultRunAtRow);

                        ///Adding DefaultValue - Run At:
                        Row defaultRunByRow = new Row();
                        defaultRunByRow.AddCell(new Cell());
                        defaultRunByRow.AddCell(new Cell("Run By:"));
                        defaultRunByRow.AddCell(new Cell(executionContext.GetUserId()));
                        trxRefreshLinesSheet.AddRow(defaultRunByRow);

                        Row defaultEmptyRow = new Row();
                        trxRefreshLinesSheet.AddRow(defaultEmptyRow);

                        ///Mapping the DataTable values to Excel Sheet - For TrxRefreshLines
                        TransactionRefreshLinesExcelDTODefinition transactionRefreshLinesExcelDTODefinition = new TransactionRefreshLinesExcelDTODefinition(executionContext, "");
                        List<TransactionRefreshLineDTODefinition> transactionRefreshLineDTOList = new List<TransactionRefreshLineDTODefinition>();
                        for (int j = 0; j < linedetails.Rows.Count; j++)
                        {
                            TransactionRefreshLineDTODefinition transactionRefreshLineDefinitionObject = new TransactionRefreshLineDTODefinition();
                            transactionRefreshLineDefinitionObject.ProductName = linedetails.Rows[j]["Product"] == DBNull.Value ? "" : Convert.ToString(linedetails.Rows[j]["Product"]);
                            transactionRefreshLineDefinitionObject.Quantity = linedetails.Rows[j]["quantity"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["quantity"]);
                            transactionRefreshLineDefinitionObject.Price = linedetails.Rows[j]["Price"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["Price"]);
                            transactionRefreshLineDefinitionObject.Amount = linedetails.Rows[j]["amount"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["amount"]);
                            transactionRefreshLineDefinitionObject.WaiverCustomer = linedetails.Rows[j]["Waiver_Customer"] == DBNull.Value ? "" : Convert.ToString(linedetails.Rows[j]["Waiver_Customer"]);
                            transactionRefreshLineDefinitionObject.CardNumber = linedetails.Rows[j]["card_number"] == DBNull.Value ? "" : Convert.ToString(linedetails.Rows[j]["card_number"]);
                            transactionRefreshLineDefinitionObject.Credits = linedetails.Rows[j]["credits"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["credits"]);
                            transactionRefreshLineDefinitionObject.Courtesy = linedetails.Rows[j]["courtesy"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["courtesy"]);
                            transactionRefreshLineDefinitionObject.Bonus = linedetails.Rows[j]["bonus"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["bonus"]);
                            transactionRefreshLineDefinitionObject.Time = linedetails.Rows[j]["time"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["time"]);
                            transactionRefreshLineDefinitionObject.Tickets = linedetails.Rows[j]["tickets"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["tickets"]);
                            transactionRefreshLineDefinitionObject.TaxName = linedetails.Rows[j]["tax_name"] == DBNull.Value ? "" : Convert.ToString(linedetails.Rows[j]["tax_name"]);
                            transactionRefreshLineDefinitionObject.TaxPercentage = linedetails.Rows[j]["Tax %"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["Tax %"]);
                            transactionRefreshLineDefinitionObject.LoyaltyPoints = linedetails.Rows[j]["loyalty_points"] == DBNull.Value ? 0 : Convert.ToDecimal(linedetails.Rows[j]["loyalty_points"]);
                            transactionRefreshLineDefinitionObject.UserPrice = linedetails.Rows[j]["UserPrice"] == DBNull.Value ? false : Convert.ToBoolean(linedetails.Rows[j]["UserPrice"]);
                            transactionRefreshLineDefinitionObject.LineId = linedetails.Rows[j]["Line"] == DBNull.Value ? -1 : Convert.ToInt32(linedetails.Rows[j]["Line"]);
                            transactionRefreshLineDefinitionObject.WaiversSignedCount = linedetails.Rows[j]["Waivers_Signed"] == DBNull.Value ? -1 : Convert.ToInt32(linedetails.Rows[j]["Waivers_Signed"]);
                            transactionRefreshLineDefinitionObject.Remarks = linedetails.Rows[j]["Remarks"] == DBNull.Value ? "" : Convert.ToString(linedetails.Rows[j]["Remarks"]);
                            transactionRefreshLinesExcelDTODefinition.Configure(transactionRefreshLineDefinitionObject);
                            transactionRefreshLineDTOList.Add(transactionRefreshLineDefinitionObject);
                        }
                        transactionRefreshLinesExcelDTODefinition.BuildHeaderRow(refreshHeaderRow);
                        trxRefreshLinesSheet.AddRow(refreshHeaderRow);
                        foreach (TransactionRefreshLineDTODefinition data in transactionRefreshLineDTOList)
                        {
                            Row trxRefreshLinesRow = new Row();
                            transactionRefreshLinesExcelDTODefinition.Serialize(trxRefreshLinesRow, data);
                            trxRefreshLinesSheet.AddRow(trxRefreshLinesRow);
                        }
                        sheets.Add(trxRefreshLinesSheet);
                        log.LogMethodExit(sheets);
                    }
                    return sheets;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }


        /// <summary>
        /// This method returns the list of parafait transaction Id which are mapped to the CenterEdge transactions
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="transactionId"></param>
        /// <param name="posMachineId"></param>
        /// <param name="siteId"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<int> GetTransactionIdList(int accountId, int transactionId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(accountId, transactionId, siteId, sqlTransaction);
            List<int> trxIdList = new List<int>();
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            trxIdList = transactionDataHandler.GetAllTransactions(accountId, transactionId, siteId);
            log.LogMethodExit(trxIdList);
            return trxIdList;
        }
        /// <summary>
        /// GetTransactionDTOList using TransactionSearchCriteria parameters
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="sqlTransaction"></param>
        /// <param name="buildChildRecords"></param>
        /// <param name="buildTickets"></param>
        /// <param name="buildReceipt"></param>
        /// <returns></returns>
        public List<TransactionDTO> GetTransactionDTOList(TransactionSearchCriteria searchCriteria, SqlTransaction sqlTransaction = null, bool buildChildRecords = false, bool buildTickets = false,
            bool buildReceipt = false)
        {
            log.LogMethodEntry(searchCriteria, sqlTransaction, buildChildRecords, buildTickets, buildReceipt);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            List<TransactionDTO> transactionDTOList = transactionDataHandler.GetTransactionDTOList(searchCriteria);
            transactionDTOList = BuildChildRecords(executionContext, transactionDTOList, buildChildRecords, buildTickets, buildReceipt, sqlTransaction);
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }
        /// <summary>
        /// GetUnsettledTransactionPayyments
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="paymentModeId"></param>
        /// <param name="deliveryChannelId"></param>
        /// <param name="trxFromDate"></param>
        /// <param name="trxToDate"></param>
        /// <param name="sQLTrx"></param>
        /// <returns></returns>
        public List<TransactionPaymentsDTO> GetUnsettledTransactionPayyments(int transactionId = -1, int paymentModeId = -1, int deliveryChannelId = -1, DateTime? trxFromDate = null, DateTime? trxToDate = null, SqlTransaction sQLTrx = null)
        {
            log.LogMethodEntry(transactionId, paymentModeId, deliveryChannelId, trxFromDate, trxToDate, sQLTrx);
            List<TransactionPaymentsDTO> unsettledTransactionPaymentsDTOList = new List<TransactionPaymentsDTO>();
            TransactionPaymentsListBL transactionPaymentsListBL = new TransactionPaymentsListBL();
            Dictionary<int, PaymentGateway> paymentgateways = InitializePaymentGateways();
            foreach (var paymentGateway in paymentgateways)
            {
                if (paymentModeId == -1 || (paymentModeId > -1 && paymentGateway.Key == paymentModeId))
                {
                    List<CCTransactionsPGWDTO> pendingCCTransactionsPGWDTOList = paymentGateway.Value.GetAllUnsettledCreditCardTransactions();
                    if (pendingCCTransactionsPGWDTOList != null && pendingCCTransactionsPGWDTOList.Count > 0)
                    {
                        List<int> responseIdList = new List<int>();
                        foreach (var cCTransactionsPGWDTO in pendingCCTransactionsPGWDTOList)
                        {
                            responseIdList.Add(cCTransactionsPGWDTO.ResponseID);
                        }
                        List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.PAYMENT_MODE_ID, paymentGateway.Key.ToString()));
                        if (transactionId > -1)
                        {
                            searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, transactionId.ToString()));
                        }
                        if (deliveryChannelId > -1)
                        {
                            searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.DELIVERY_CHANNEL_ID, deliveryChannelId.ToString()));
                        }
                        if (trxFromDate != null)
                        {
                            searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_FROM_DATE, ((DateTime)trxFromDate).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        if (trxToDate != null)
                        {
                            searchParameters.Add(new KeyValuePair<TransactionPaymentsDTO.SearchByParameters, string>(TransactionPaymentsDTO.SearchByParameters.TRANSACTION_TO_DATE, ((DateTime)trxToDate).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        List<TransactionPaymentsDTO> transactionPaymentsDTOList = transactionPaymentsListBL.GetNonReversedTransactionPaymentsDTOList(searchParameters, responseIdList);
                        if (transactionPaymentsDTOList != null)
                        {
                            unsettledTransactionPaymentsDTOList.AddRange(transactionPaymentsDTOList);
                        }
                    }
                    if (paymentModeId > -1)
                    {
                        break;
                    }
                }
            }
            log.LogMethodExit(unsettledTransactionPaymentsDTOList);
            return unsettledTransactionPaymentsDTOList;
        }

        private Dictionary<int, PaymentGateway> InitializePaymentGateways()
        {
            log.LogMethodEntry();
            Dictionary<int, PaymentGateway> paymentgateways = new Dictionary<int, PaymentGateway>();
            try
            {
                PaymentModeList paymentModesListBL = new PaymentModeList(executionContext);
                List<PaymentModeDTO> paymentModesDTOList = paymentModesListBL.GetPaymentModesWithPaymentGateway(true);
                if (paymentModesDTOList != null)
                {
                    PaymentGatewayFactory.GetInstance().Initialize(new Utilities(), true);
                    foreach (var paymentModesDTO in paymentModesDTOList)
                    {
                        try
                        {
                            PaymentMode paymentModesBL = new PaymentMode(executionContext, paymentModesDTO);
                            paymentgateways.Add(paymentModesDTO.PaymentModeId, PaymentGatewayFactory.GetInstance().GetPaymentGateway(paymentModesBL.Gateway));
                        }
                        catch (Exception ex1)
                        {
                            log.Error("Error occurred while initializing Payment Gateway", ex1);
                            log.Error("*" + ex1.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while initializing Payment Gateway", ex);
                paymentgateways = new Dictionary<int, PaymentGateway>();
            }
            log.LogMethodExit();
            return paymentgateways;
        }
        public List<TransactionDTO> GetTransactionsForAlohaSynch(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters,
                                                                int sqlCommandTimeOut = 600, SqlTransaction sqlTransaction = null)
        {
            //***Important note*** Used specifically for ALohaBSP integration ****//
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            List<TransactionDTO> transactionDTOList = transactionDataHandler.GetTransactionsForAlohaSynch(searchParameters, sqlCommandTimeOut);
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        public List<TransactionDTO> GetTETTransactions(SqlTransaction sqlTransaction = null)
        {
            //***Important note*** Used specifically for TET integration ****//
            log.LogMethodEntry(sqlTransaction);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            List<TransactionDTO> transactionDTOList = transactionDataHandler.GetTETTransactions();
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

        /// <summary>
        /// GetOrdersToBePostedInAloha
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlConnectionTimeOut"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<TransactionDTO> GetOrdersToBePostedInAloha(List<KeyValuePair<TransactionDTO.SearchByParameters, string>> searchParameters,
                                                        int sqlConnectionTimeOut = 600, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlConnectionTimeOut, sqlTransaction);
            TransactionDataHandler transactionDataHandler = new TransactionDataHandler(sqlTransaction);
            List<TransactionDTO> transactionDTOList = transactionDataHandler.GetOrdersToBePostedInAloha(searchParameters, sqlConnectionTimeOut);
            log.LogMethodExit(transactionDTOList);
            return transactionDTOList;
        }

    }
}


