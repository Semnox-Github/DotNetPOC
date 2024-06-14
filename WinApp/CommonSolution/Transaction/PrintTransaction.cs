/********************************************************************************************
 * Project Name - PrintTransaction
 * Description  - Business Logic to prepare transaction for final printing
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        17-Sep-2008      Iqbal Mohammad  Created 
 *2.40.0      19-Sep-2018      Mathew Ninan    Changed methods to use 3 tier objects.  
 *2.40.1      11-Nov-2018      Mathew Ninan    Handling of Printer list being null and Split scenario   
 *2.70.0      02-Jul-2019      Mathew Ninan    Modified Clone trx method to use DTO for check-in/checkindetail  
*2.70         14-Apr-2019      Guru S A        Booking phase 2 enhancement changes
 *2.70.2      24-Sep-2019      Lakshminarayana Removed KDSUtils class. Used 3tier KDSOrder class.
 *            03-Oct-2019      Rakesh Kumar    Added PrintReceipt() and TicketTemplatePreview() method
 *2.70.2      26-Nov-2019      Lakshminarayana Virtual store enhancement
 *2.70.2      04-Feb-2019      Grish Kundar    Ticket printer enhancement
 *2.90.0      14-Jul-2020      Gururaja Knajan  Updated for fiskaltrust integration  
 *2.90.0      18-Aug-2020      Laster menezes  Updated Fiscalization logic for Croatia Fiscalization
 *2.100.0     07-Oct-2020      Mathew Ninan    Skip ticket printing when receiptonly is set 
 *2.110.0     22-Dec-2020      Girish Kundar   Modified :FiscalTrust changes - Shift open/Close/PayIn/PayOut to be fiscalized
 *2.120.0     20-Apr-2021      Guru S A        Wrist band printing process changes
 *2.120       03-May-2021      Laster Menezes  Croatia fiscalization :modified Print method to use 
 *                                             ExternalSourceReference field of trxpayments as fiscal reference
 *2.140.0     27-Jun-2021      Fiona Lishal      Modified for Delivery Order enhancements for F&B and Urban Piper
 *2.140.2     18-Apr-2022      Girish Kundar   Modified:  BOCA changes - Added new column WBModel to printer class
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Security.Principal;
using Semnox.Parafait.Discounts;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.POS;
using Semnox.Parafait.Transaction.KDS;
using System.Configuration;
using Semnox.Parafait.Product;
using Semnox.Parafait.Reports;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Device.Printer.WristBandPrinters;

namespace Semnox.Parafait.Transaction
{
    public class PrintTransaction
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // The class that will do the printing process.
        Printer.ReceiptClass TransactionReceipt;
        List<Printer.clsTicket> TicketList;
        List<POSPrinterDTO> posPrinterDTOList;
        /// <summary>
        /// InvokeHandle
        /// </summary>
        public delegate void InvokeHandle();
        InvokeHandle EndPrintAction;
        /// <summary>
        /// ProgressUpdates
        /// </summary>
        /// <param name="statusMessage"></param>
        public delegate void ProgressUpdates(string statusMessage);
        /// <summary>
        /// PrintProgressUpdates
        /// </summary>
        public ProgressUpdates PrintProgressUpdates;
        /// <summary>
        /// SetCardPrinterError
        /// </summary>
        /// <param name="errorValue"></param>
        public delegate void SetCardPrinterError(bool errorValue);
        /// <summary>
        /// SetCardPrinterErrorValue
        /// </summary>
        public SetCardPrinterError SetCardPrinterErrorValue;

        internal bool RePrint = false;
        /// <summary>
        /// fiscalPrinter
        /// </summary>
        public FiscalPrinter fiscalPrinter;
        /// <summary>
        /// setEndPrintAction
        /// </summary>
        public InvokeHandle setEndPrintAction
        {
            get
            {
                return EndPrintAction;
            }
            set
            {
                EndPrintAction = value;
            }
        }
        /// <summary>
        /// PrintTransaction Constructor
        /// </summary>
        public PrintTransaction()
        {
            log.LogMethodEntry();
            posPrinterDTOList = new List<POSPrinterDTO>();
            log.LogMethodExit(null);
        }
        /// <summary>
        /// PrintTransaction Constructor
        /// </summary>
        /// <param name="posPrinterDTOList"></param>
        public PrintTransaction(List<POSPrinterDTO> posPrinterDTOList) : this()
        {
            log.LogMethodEntry();
            this.posPrinterDTOList = posPrinterDTOList;
            log.LogVariableState("POS Printer DTO List", posPrinterDTOList);
        }

        /// <summary>
        /// SendToKDS - Method to send transaction lines to KDS
        /// </summary>
        /// <param name="Trx"></param>
        /// <param name="message"></param>
        /// <returns>true or false</returns>
        public bool SendToKDS(Transaction Trx, ref string message)
        {
            log.LogMethodEntry(Trx, message);
            if (Trx.TransactionOrderTypes != null && Trx.TransactionOrderTypes.Count > 0
               && Trx.Order != null && Trx.Order.OrderHeaderDTO != null
               && Trx.Order.OrderHeaderDTO.TransactionOrderTypeId == Trx.TransactionOrderTypes["Item Refund"])
            {
                log.Debug("Variable Refund transaction. Tickets shouldn't be printed.");
                return true;
            }
            foreach (Transaction.TransactionLine tl in Trx.TrxLines)
            {
                if (!tl.KDSSent) // already sent to KDS. no need to process
                    tl.SendToKDS = true;
                else
                    tl.SendToKDS = false;
            }

            if (this.posPrinterDTOList == null || this.posPrinterDTOList.Any() == false)
                this.posPrinterDTOList = Trx.POSPrinterDTOList;
            if (posPrinterDTOList == null || posPrinterDTOList.Count == 0)
            {
                POSMachines posMachine = new POSMachines(Trx.Utilities.ExecutionContext, Trx.Utilities.ParafaitEnv.POSMachineId);
                posPrinterDTOList = posMachine.PopulatePrinterDetails();
            }
            Trx.GetPrintableTransactionLines(posPrinterDTOList);
            int noOfProductsPerKDSOrder = ParafaitDefaultContainerList.GetParafaitDefault<int>(Trx.Utilities.ExecutionContext, "MAXIMUM_PRODUCTS_IN_KDS_ORDER", 0);
            foreach (Transaction.EligibleTrxLinesPrinterMapper eligibleTrxLinesPrinterMapper in Trx.EligibleTrxLinesPrinterMapperList)
            {
                if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType != PrinterDTO.PrinterTypes.KDSTerminal)
                {
                    continue;
                }

                List<Transaction.TransactionLine> kdsLines = new List<Transaction.TransactionLine>();

                foreach (Transaction.TransactionLine kdsTrxLine in eligibleTrxLinesPrinterMapper.TrxLines)
                {
                    if (kdsTrxLine.SendToKDS == false)
                        continue;
                    kdsTrxLine.KDSSent = true;
                    kdsLines.Add(kdsTrxLine);
                }

                if (kdsLines.Any() == false)
                {
                    continue;
                }

                if (noOfProductsPerKDSOrder > 0)
                {
                    List<KDSOrderLineGroupBL> kDSOrderLineGroupBLList = GetKDSOrderLineGroupBLList(Trx.Utilities.ExecutionContext, kdsLines.Select(x => x.TransactionLineDTO).ToList());
                    Dictionary<int, Transaction.TransactionLine> lineIdTransactionLineMap = new Dictionary<int, Transaction.TransactionLine>();
                    foreach (var line in kdsLines)
                    {
                        if (lineIdTransactionLineMap.ContainsKey(line.DBLineId))
                        {
                            continue;
                        }
                        lineIdTransactionLineMap.Add(line.DBLineId, line);
                    }
                    int currentDepth = 0;
                    List<Transaction.TransactionLine> subKDSLines = new List<Transaction.TransactionLine>();
                    kDSOrderLineGroupBLList.Sort((a, b) => (a.GetKDSOrderLineGroupDTOCount().CompareTo(b.GetKDSOrderLineGroupDTOCount())));
                    foreach (KDSOrderLineGroupBL kDSOrderLineGroupBL in kDSOrderLineGroupBLList)
                    {
                        if (currentDepth == 0 ||
                            currentDepth + kDSOrderLineGroupBL.GetKDSOrderLineGroupDTOCount() <= noOfProductsPerKDSOrder)
                        {
                            foreach (var lineId in kDSOrderLineGroupBL.GetTransactionLineIdList())
                            {
                                subKDSLines.Add(lineIdTransactionLineMap[lineId]);
                            }
                            currentDepth += kDSOrderLineGroupBL.GetKDSOrderLineGroupDTOCount();
                        }
                        else
                        {
                            CreateKDSOrder(Trx, subKDSLines, eligibleTrxLinesPrinterMapper);
                            currentDepth = kDSOrderLineGroupBL.GetKDSOrderLineGroupDTOCount();
                            subKDSLines.Clear();
                            foreach (var lineId in kDSOrderLineGroupBL.GetTransactionLineIdList())
                            {
                                subKDSLines.Add(lineIdTransactionLineMap[lineId]);
                            }
                        }
                    }

                    if (subKDSLines.Any())
                    {
                        CreateKDSOrder(Trx, subKDSLines, eligibleTrxLinesPrinterMapper);
                    }
                }
                else
                {
                    CreateKDSOrder(Trx, kdsLines, eligibleTrxLinesPrinterMapper);
                }
            }
            log.LogVariableState("message ,", message);
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Returns the group of KDS Order line group dto. grouped by product id
        /// </summary>
        /// <returns></returns>
        public List<KDSOrderLineGroupBL> GetKDSOrderLineGroupBLList(ExecutionContext executionContext, List<TransactionLineDTO> transactionLineDTOList)
        {
            log.LogMethodEntry();
            List<KDSOrderLineGroupDTO> kdsOrderLineGroupDTOList = new List<KDSOrderLineGroupDTO>();
            List<KDSOrderLineGroupBL> parentKDSOrderLineGroupBLList = new List<KDSOrderLineGroupBL>();
            Dictionary<int, KDSOrderLineGroupBL> lineIdKDSOrderLineGroupBLDictionary = new Dictionary<int, KDSOrderLineGroupBL>();
            foreach (TransactionLineDTO transactionLineDTO in transactionLineDTOList)
            {
                //if (kdsOrderLineDTO.LineCancelledTime.HasValue)
                //{
                //    continue;
                //}
                if (transactionLineDTO.ParentLineId >= 0)
                {
                    continue;
                }
                KDSOrderLineGroupBL kdsOrderLineGroupBL = new KDSOrderLineGroupBL(executionContext, transactionLineDTO);
                parentKDSOrderLineGroupBLList.Add(kdsOrderLineGroupBL);
                if (lineIdKDSOrderLineGroupBLDictionary.ContainsKey(transactionLineDTO.LineId))
                {
                    continue;
                }
                lineIdKDSOrderLineGroupBLDictionary.Add(transactionLineDTO.LineId, kdsOrderLineGroupBL);
            }

            foreach (TransactionLineDTO transactionLineDTO in transactionLineDTOList)
            {
                //if (kdsOrderLineDTO.LineCancelledTime.HasValue)
                //{
                //    continue;
                //}
                if (transactionLineDTO.ParentLineId < 0)
                {
                    continue;
                }
                KDSOrderLineGroupBL kdsOrderLineGroupBL = new KDSOrderLineGroupBL(executionContext, transactionLineDTO);
                if (lineIdKDSOrderLineGroupBLDictionary.ContainsKey(transactionLineDTO.LineId))
                {
                    continue;
                }

                if (lineIdKDSOrderLineGroupBLDictionary.ContainsKey(transactionLineDTO.ParentLineId))
                {
                    KDSOrderLineGroupBL parent = lineIdKDSOrderLineGroupBLDictionary[transactionLineDTO.ParentLineId];
                    parent.AddChild(kdsOrderLineGroupBL);
                }
                else
                {
                    parentKDSOrderLineGroupBLList.Add(kdsOrderLineGroupBL);
                }
                lineIdKDSOrderLineGroupBLDictionary.Add(transactionLineDTO.LineId, kdsOrderLineGroupBL);
            }

            List<KDSOrderLineGroupBL> mergedKDSOrderLineGroupBLList = new List<KDSOrderLineGroupBL>();
            Dictionary<string, KDSOrderLineGroupBL> lineHierarchyStringKDSOrderLineGroupBLDictionary = new Dictionary<string, KDSOrderLineGroupBL>();
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in parentKDSOrderLineGroupBLList)
            {
                kdsOrderLineGroupBL.Consolidate();
            }
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in parentKDSOrderLineGroupBLList)
            {
                string lineHierarchyString = kdsOrderLineGroupBL.GetLineHierarchyString();
                if (lineHierarchyStringKDSOrderLineGroupBLDictionary.ContainsKey(lineHierarchyString))
                {
                    lineHierarchyStringKDSOrderLineGroupBLDictionary[lineHierarchyString].Merge(kdsOrderLineGroupBL);
                    continue;
                }
                lineHierarchyStringKDSOrderLineGroupBLDictionary.Add(lineHierarchyString, kdsOrderLineGroupBL);
                mergedKDSOrderLineGroupBLList.Add(kdsOrderLineGroupBL);
            }
            foreach (KDSOrderLineGroupBL kdsOrderLineGroupBL in mergedKDSOrderLineGroupBLList)
            {
                kdsOrderLineGroupBL.SetProductNameOffset(0);
                kdsOrderLineGroupDTOList.AddRange(kdsOrderLineGroupBL.GetKDSOrderLineGroupList());
            }
            log.LogMethodExit(mergedKDSOrderLineGroupBLList);
            return mergedKDSOrderLineGroupBLList;
        }

        private void CreateKDSOrder(Transaction Trx, List<Transaction.TransactionLine> kdsLines, Transaction.EligibleTrxLinesPrinterMapper eligibleTrxLinesPrinterMapper)
        {
            try
            {
                using (ParafaitDBTransaction trx = new ParafaitDBTransaction())
                {
                    trx.BeginTransaction();
                    KDSOrderBL kdsOrderBL = new KDSOrderBL(Trx.Utilities.ExecutionContext, Trx, kdsLines,
                        eligibleTrxLinesPrinterMapper.POSPrinterDTO, trx.SQLTrx);
                    kdsOrderBL.Save(trx.SQLTrx);
                    trx.EndTransaction();
                    foreach (Transaction.TransactionLine line in kdsLines)
                    {
                        Trx.updateTrxLinesKDSSentStatus(line);
                    }
                    LoadKDSKOTEntriesToLines(Trx, kdsLines, kdsOrderBL.KDSOrderDTO);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while sending to KDS", ex);
                foreach (Transaction.TransactionLine kdsLine in kdsLines)
                {
                    Trx.InsertTrxLogs(Trx.Trx_id, kdsLine.DBLineId, Trx.Utilities.ParafaitEnv.LoginID, "PRINT",
                        "KDS Print failed");
                }
            }
        }

        public bool PrintKOT(Transaction Trx, ref string message)
        {
            log.LogMethodEntry(Trx, message, posPrinterDTOList);
            //Trx.GetPrintableTransactionLines(posPrinterDTOList);//Testing. To be removed
            log.LogVariableState("message ,", message);
            log.LogVariableState("POSPrinterDTOList ,", posPrinterDTOList);

            bool returnValueNew = Print(Trx, ref message, true, false);
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        public bool Print(Transaction Trx, ref string message, bool KOTOnly = false, bool ReceiptOnly = false, bool printTrxReceipt = true)
        {
            log.LogMethodEntry(Trx, message, KOTOnly, ReceiptOnly, printTrxReceipt);
            bool returnValueNew = false;
            try
            {
                log.LogVariableState("message ,", message);
                if (this.posPrinterDTOList == null && (Trx.POSPrinterDTOList != null))
                    this.posPrinterDTOList = Trx.POSPrinterDTOList;
                if (posPrinterDTOList == null || posPrinterDTOList.Count == 0)
                {
                    POSMachines posMachine = new POSMachines(Trx.Utilities.ExecutionContext, Trx.Utilities.ParafaitEnv.POSMachineId);
                    posPrinterDTOList = posMachine.PopulatePrinterDetails();
                }
                if (printTrxReceipt == false)
                {
                    //Remomve receipt printer from the selected printer list
                    if (posPrinterDTOList != null && posPrinterDTOList.Any()
                        && posPrinterDTOList.Exists( p=> p.PrinterDTO != null && p.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter))
                    {
                        for (int i = 0; i < posPrinterDTOList.Count; i++)
                        {
                            POSPrinterDTO p = posPrinterDTOList[i];
                            if (p.PrinterDTO != null && p.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter)
                            {
                                posPrinterDTOList.Remove(p);
                                i = (i == 0? 0 : (i - 1));
                            }
                        }
                    }
                }
                Trx.GetPrintableTransactionLines(posPrinterDTOList);
                returnValueNew = Print(Trx, -1, ref message, KOTOnly, ReceiptOnly);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                message = ex.Message;
                return false;
            }
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }

        /// <summary>
        /// Main print method to process lines and prepare transaction for final printing
        /// </summary>
        /// <param name="Trx">Transaction Object</param>
        /// <param name="splitId">Split in case of Split</param>
        /// <param name="message">reference message</param>
        /// <param name="ReceiptOnly">Will be true if its Receipt Only print</param>
        /// <param name="KOTOnly">Will be true if KOT only flag</param>
        public bool Print(Transaction Trx, int splitId, ref string message, bool KOTOnly = false, bool ReceiptOnly = false)
        {
            log.LogMethodEntry(Trx, splitId, message, KOTOnly, ReceiptOnly);

            bool validFound = false;
            List<Transaction.TransactionLine> origTrxLines = new List<Transaction.TransactionLine>();
            origTrxLines.AddRange(Trx.TrxLines);
            foreach (Transaction.TransactionLine tl in Trx.TrxLines)
            {
                if (tl.LineValid)
                {
                    validFound = true;
                    //modified on 17-Nov-2016 for KOT running order print
                    if (!DBNull.Value.Equals(tl.KOTPrintCount))
                    {
                        if (Convert.ToInt32(tl.KOTPrintCount) > 0)
                        {
                            Trx.IsOrderPrinted = true;
                            if (tl.CancelledLine)
                            {
                                Trx.IsOrderPrinted = false;
                            }
                            break;
                        }
                    }
                    //end Modified                 
                }
            }

            Utilities Utilities = Trx.Utilities;
            Transaction printTransaction;
            //call new Printer Line mapping method

            bool allowTrxPrintBeforeSaving = Semnox.Core.Utilities.ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "ALLOW_TRX_PRINT_BEFORE_SAVING") == "Y";
            if (((allowTrxPrintBeforeSaving || Trx.Status == Transaction.TrxStatus.CLOSED) || KOTOnly) && validFound)
            {
                if (splitId > -1)
                {
                    if (this.posPrinterDTOList == null)
                        this.posPrinterDTOList = Trx.POSPrinterDTOList;
                    if (posPrinterDTOList == null || posPrinterDTOList.Count == 0)
                    {
                        POSMachines posMachine = new POSMachines(Trx.Utilities.ExecutionContext, Trx.Utilities.ParafaitEnv.POSMachineId);
                        posPrinterDTOList = posMachine.PopulatePrinterDetails();
                    }
                    Trx.GetPrintableTransactionLines(posPrinterDTOList);
                }
                Trx.TransactionInfo.createTransactionInfo(Trx.Trx_id, splitId);//Call new print generation class to populate printable values
                try
                {
                    if (ReceiptOnly == false)
                        SendToKDS(Trx, ref message);

                    foreach (Transaction.EligibleTrxLinesPrinterMapper eligibleTrxLinesPrinterMapper in Trx.EligibleTrxLinesPrinterMapperList)
                    {
                        if (Trx.TransactionOrderTypes != null && Trx.TransactionOrderTypes.Count > 0
                            && Trx.Order != null && Trx.Order.OrderHeaderDTO != null
                            && Trx.Order.OrderHeaderDTO.TransactionOrderTypeId == Trx.TransactionOrderTypes["Item Refund"]
                            && eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType != PrinterDTO.PrinterTypes.ReceiptPrinter)
                        {
                            log.Debug("Variable Refund transaction. No prints other than receipt printing is allowed");
                            continue;
                        }
                        if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KOTPrinter && ReceiptOnly)
                            continue;
                        if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KOTPrinter && eligibleTrxLinesPrinterMapper.TrxLines.Exists(x => x.ProductTypeCode != "MANUAL" && x.ProductTypeCode != "COMBO"))
                            continue;
                        if ((eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.CardPrinter
                                    || eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter
                                    || eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RFIDWBPrinter
                                ) && ReceiptOnly)
                            continue;

                        if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter &&
                       eligibleTrxLinesPrinterMapper.POSPrinterDTO.OrderTypeGroupId != Trx.OrderTypeGroupId)
                        {
                            if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.OrderTypeGroupId != -1)
                            {
                                continue;
                            }
                            else
                            {
                                List<Transaction.EligibleTrxLinesPrinterMapper> orderCheckEligibelTrxLinesMapperList = Trx.EligibleTrxLinesPrinterMapperList.FindAll(x => x.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter
                                                                                                                                && x.POSPrinterDTO.OrderTypeGroupId == Trx.OrderTypeGroupId);
                                if (orderCheckEligibelTrxLinesMapperList.Count > 0)
                                    continue;
                            }
                        }
                        //KOTOnly flag set and Printer is not KOT or Ticket Printer
                        if ((KOTOnly && eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType != PrinterDTO.PrinterTypes.KOTPrinter
                                     && eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType != PrinterDTO.PrinterTypes.TicketPrinter
                                     && eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType != PrinterDTO.PrinterTypes.RFIDWBPrinter
                                     && eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType != PrinterDTO.PrinterTypes.CardPrinter)
                            || eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KDSTerminal)
                            continue;

                        //KOT Printing
                        if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KOTPrinter)
                        {   //Transaction to be used for further processing
                            printTransaction = Trx;
                            printTransaction.TrxLines.Clear();
                            printTransaction.TrxLines = eligibleTrxLinesPrinterMapper.TrxLines;
                        }
                        else if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.CardPrinter
                                    || eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter
                                    || eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RFIDWBPrinter
                                )
                        {
                            printTransaction = Trx;
                            printTransaction.TrxLines.Clear();
                            printTransaction.TrxLines = eligibleTrxLinesPrinterMapper.TrxLines;
                        }
                        else
                        {//Receipt Printer
                            printTransaction = Trx;
                            printTransaction.TrxLines = new List<Transaction.TransactionLine>();
                            printTransaction.TrxLines.AddRange(origTrxLines);
                        }

                        TransactionUtils transactionUtils = new TransactionUtils(Utilities);
                        FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                        string fiscalPrinterType = ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "FISCAL_PRINTER");
                        if (string.IsNullOrEmpty(fiscalPrinterType) == false)
                        {
                            fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(fiscalPrinterType);
                        }
                        string fiscalizationResponse = string.Empty;
                        if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.CardPrinter
                           || eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter
                           || eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RFIDWBPrinter)
                        {
                            if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.TicketPrinter)
                                printTickets(printTransaction, eligibleTrxLinesPrinterMapper.POSPrinterDTO);
                            else
                            {
                                printCard(printTransaction, eligibleTrxLinesPrinterMapper.POSPrinterDTO);
                                foreach (Transaction.TransactionLine printTransactionLine in printTransaction.TrxLines)
                                {
                                    origTrxLines.First(x => x.DBLineId == printTransactionLine.DBLineId).CardNumber = printTransactionLine.CardNumber;
                                    origTrxLines.First(x => x.DBLineId == printTransactionLine.DBLineId).ReceiptPrinted = printTransactionLine.ReceiptPrinted;
                                }

                                //Trx.TrxLines = origTrxLines;
                            }
                        }
                        else
                        {//PrintReceipt for non-Ticket lines

                            //begin add for fiskaltrust integration 14 July 2020
                            if (fiscalPrinterType.ToUpper().Equals(FiskaltrustPrinter.FISKALTRUST))
                            {
                                string signature = string.Empty;
                                FiscalizationBL fiscalizationHelper = new FiscalizationBL(Utilities.ExecutionContext, printTransaction);
                                bool doFiscalize = fiscalizationHelper.NeedsFiscalization();
                                if (doFiscalize)
                                {
                                    try
                                    {
                                        FiscalizationRequest fiscalizationRequest = fiscalizationHelper.BuildFiscalizationRequest();
                                        bool isSuccess = fiscalPrinter.PrintReceipt(fiscalizationRequest, ref signature);
                                        if (isSuccess)
                                        {
                                            fiscalizationHelper.UpdatePaymentReference(fiscalizationRequest, signature, null);
                                            if (printTransaction.TransactionPaymentsDTOList != null && printTransaction.TransactionPaymentsDTOList.Count > 0)
                                            {
                                                Device.PaymentGateway.TransactionPaymentsListBL transactionPaymentsListBL = new Device.PaymentGateway.TransactionPaymentsListBL(Utilities.ExecutionContext);
                                                List<KeyValuePair<Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>>();
                                                transactionsPaymentsSearchParams.Add(new KeyValuePair<Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>(Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, printTransaction.Trx_id.ToString()));

                                                List<Device.PaymentGateway.TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);

                                                if (transactionPaymentsDTOs != null || transactionPaymentsDTOs.Count > 0)
                                                {
                                                    for (int i = 0; i < transactionPaymentsDTOs.Count(); i++)
                                                    {
                                                        foreach (Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOs)
                                                        {
                                                            if (printTransaction.TransactionPaymentsDTOList[i].PaymentId == transactionPaymentsDTO.PaymentId)
                                                            {
                                                                printTransaction.TransactionPaymentsDTOList[i].ExternalSourceReference = transactionPaymentsDTO.ExternalSourceReference;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (ValidationException ex)
                                    {
                                        log.Error(ex);
                                        printTransaction.InsertTrxLogs(printTransaction.Trx_id, -1, printTransaction.Utilities.ParafaitEnv.LoginID, "FISCALIZATION", ex.Message);
                                    }
                                    catch (Exception ex)
                                    {
                                        log.Error(ex);
                                    }
                                }
                            }
                            //end add for fiskaltrust integration

                            //begin add for Croatia Fiscalization integration 18 Aug 2020
                            else if ((fiscalPrinterType.Equals(FiscalPrinters.CroatiaFiscalization.ToString())))
                            {
                                FiscalPrinterFactory.GetInstance().Initialize(Utilities);
                                bool isFiscalized = false;
                                try
                                {
                                    try
                                    {
                                        isFiscalized = fiscalPrinter.PrintReceipt(printTransaction.Trx_id, ref fiscalizationResponse, null, 0);
                                    }
                                    catch (ValidationException ex)
                                    {
                                        printTransaction.InsertTrxLogs(printTransaction.Trx_id, -1, printTransaction.Utilities.ParafaitEnv.LoginID, "FISCALIZATION", ex.Message);
                                    }

                                    if (printTransaction.TransactionPaymentsDTOList != null && printTransaction.TransactionPaymentsDTOList.Count > 0)
                                    {
                                        Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsListBL transactionPaymentsListBL = new Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsListBL(Utilities.ExecutionContext);
                                        List<KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>> transactionsPaymentsSearchParams = new List<KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>>();
                                        transactionsPaymentsSearchParams.Add(new KeyValuePair<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters, string>(Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO.SearchByParameters.TRANSACTION_ID, printTransaction.Trx_id.ToString()));

                                        List<Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO> transactionPaymentsDTOs = transactionPaymentsListBL.GetTransactionPaymentsDTOList(transactionsPaymentsSearchParams);

                                        if (transactionPaymentsDTOs != null || transactionPaymentsDTOs.Count > 0)
                                        {
                                            for (int i = 0; i < transactionPaymentsDTOs.Count(); i++)
                                            {
                                                foreach (Semnox.Parafait.Device.PaymentGateway.TransactionPaymentsDTO transactionPaymentsDTO in transactionPaymentsDTOs)
                                                {
                                                    if (printTransaction.TransactionPaymentsDTOList[i].PaymentId == transactionPaymentsDTO.PaymentId)
                                                    {
                                                        printTransaction.TransactionPaymentsDTOList[i].ExternalSourceReference = transactionPaymentsDTO.ExternalSourceReference;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex);
                                }
                            }
                            //end add for Croatia Fiscalization integration

                            bool success = printReceipt(printTransaction, eligibleTrxLinesPrinterMapper.POSPrinterDTO, splitId);
                            if (success)
                            {//KOT Count to be incremented only for one KOT print. 
                                if (eligibleTrxLinesPrinterMapper.POSPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.KOTPrinter)
                                {
                                    List<Transaction.TransactionLine> kotPrintedLines = eligibleTrxLinesPrinterMapper.TrxLines.FindAll(x => x.LineValid && x.PrintKOT);
                                    List<Transaction.TransactionLine> kotLinesForOrderEntry = new List<Transaction.TransactionLine>();
                                    foreach (Transaction.TransactionLine originalTrxLine in origTrxLines)
                                    {
                                        if (kotPrintedLines.Exists(y => y.DBLineId == originalTrxLine.DBLineId) && !originalTrxLine.KOTCountIncremented)
                                        {
                                            originalTrxLine.KOTPrintCount = (originalTrxLine.KOTPrintCount == DBNull.Value ? 0 : Convert.ToInt32(originalTrxLine.KOTPrintCount)) + 1;
                                            originalTrxLine.KOTCountIncremented = true;
                                            kotLinesForOrderEntry.Add(originalTrxLine);
                                        }
                                    }
                                    CreateKOTOrderEntry(printTransaction, kotLinesForOrderEntry, eligibleTrxLinesPrinterMapper);
                                }
                            }
                        }
                    }
                    if (EndPrintAction != null)
                    {
                        EndPrintAction.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Failed to print the Transaction! ", ex);
                    message = ex.Message;

                    if (Utilities.ParafaitEnv.IsClientServer)
                        MessageBox.Show(ex.Message);

                    log.LogMethodExit(false);
                    return false;
                }
                Trx.TrxLines = new List<Transaction.TransactionLine>();
                Trx.TrxLines.AddRange(origTrxLines);
                for (int i = 0; i < Trx.TrxLines.Count; i++)
                {
                    Trx.TrxLines[i].ReceiptPrinted = true;
                    if (Trx.TrxLines[i].PrintKOT)
                    {
                        Trx.TrxLines[i].PrintKOT = false;
                    }
                    if (Trx.TrxLines[i].KOTCountIncremented)
                    {
                        Trx.TrxLines[i].KOTCountIncremented = false;
                    }
                }
                DataGridView dataGridViewTransaction = DisplayDatagridView.createRefTrxDatagridview(Trx.Utilities);
                DisplayDatagridView.RefreshTrxDataGrid(Trx, dataGridViewTransaction, Trx.Utilities);
                bool returnValueNew = Trx.updateTrxLinesReceiptPrintedStatus();
                log.LogMethodExit(returnValueNew);
                return returnValueNew;
            }
            else
            {
                message = Utilities.MessageUtils.getMessage(350);
                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Set up Printing by calling PrinterBL Class
        /// </summary>
        /// <param name="MyPrintDocument"></param>
        /// <param name="trx"></param>
        /// <param name="Printer"></param>
        /// <returns></returns>
        private bool SetupThePrinting(PrintDocument MyPrintDocument, Transaction trx, PrinterDTO printerDTO)
        {
            log.LogMethodEntry(MyPrintDocument, trx, printerDTO);

            string documentName = trx.Utilities.MessageUtils.getMessage("Transaction Receipt");
            PrinterBL printerBL = new PrinterBL(trx.Utilities.ExecutionContext, printerDTO);
            if ((!String.IsNullOrEmpty(trx.TransactionInfo.PaymentReference)) && (trx.TransactionInfo.PaymentReference.ToString() == "Refund"))
                documentName = trx.Utilities.MessageUtils.getMessage("Credit Note");

            bool setupPrintStatus = printerBL.SetupThePrinting(MyPrintDocument, trx.Utilities.ParafaitEnv.IsClientServer, documentName);
            log.LogMethodExit(setupPrintStatus);
            return setupPrintStatus;
        }


        /// <summary>
        /// Print Tickets to send ticket specific printing
        /// </summary>
        /// <param name="printTransaction"></param>
        /// <param name="posPrinterDTO"></param>
        /// <returns>true or false</returns>
        bool printTickets(Transaction printTransaction, POSPrinterDTO posPrinterDTO)
        {
            log.LogMethodEntry(printTransaction, posPrinterDTO);

            Utilities Utilities = printTransaction.Utilities;

            if (Utilities.ParafaitEnv.PRINT_TRANSACTION_ITEM_TICKETS != "Y")
            {
                log.LogMethodExit(false);
                return false;
            }
            //Peru Invoice changes
            int TicketsToPrint = 0;
            int printerTemplateId = posPrinterDTO.PrintTemplateId;
            if (printTransaction.TrxPOSPrinterOverrideRulesDTOList != null && printTransaction.TrxPOSPrinterOverrideRulesDTOList.Any())
            {
                TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(Utilities.ExecutionContext);
                int overRidePrinterTemplateId = trxPOSPrinterOverrideRulesListBL.GetTemplateId(printTransaction.TrxPOSPrinterOverrideRulesDTOList, posPrinterDTO);
                if (overRidePrinterTemplateId > -1)
                {
                    printerTemplateId = overRidePrinterTemplateId;
                }
            }
            TicketList = POSPrint.getTickets(printerTemplateId, printTransaction, ref TicketsToPrint, RePrint);
            if (TicketsToPrint == 0)
            {
                log.LogMethodExit(false);
                return false;
            }
            PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext, posPrinterDTO.PrinterDTO);
            PrintDocument PrintTicketDocument = new PrintDocument();
            if (SetupThePrinting(PrintTicketDocument, printTransaction, posPrinterDTO.PrinterDTO))
            {
                PrintTicketDocument.DefaultPageSettings.Margins = TicketList[0].MarginProperty;
                // PrintTicketDocument.DefaultPageSettings.PaperSize = TicketList[0].PaperSize;
                int currentTicket = 0;
                PrintTicketDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                {
                    printerBL.PrintTicketsToPrinter(TicketList, currentTicket, e);
                };
                while (currentTicket < TicketsToPrint)
                {
                    PrintTicketDocument.Print();
                    currentTicket++;
                    if (Utilities.ParafaitEnv.CUT_TICKET_PAPER)
                        printerBL.CutPrinterPaper(PrintTicketDocument.PrinterSettings.PrinterName);
                }
            }

            log.LogMethodExit(true);
            return true;
        }

        bool printReceipt(Transaction printTransaction, POSPrinterDTO posPrinterDTO)
        {
            log.LogMethodEntry(printTransaction, posPrinterDTO);

            bool returnValue = printReceipt(printTransaction, posPrinterDTO, -1);
            log.LogMethodExit(returnValue);
            return returnValue;
        }

        bool printReceipt(Transaction printTransaction, POSPrinterDTO posPrinterDTO, int splitId)
        {
            log.LogMethodEntry(printTransaction, posPrinterDTO, splitId);

            Utilities Utilities = printTransaction.Utilities;

            if (printTransaction.Utilities.ParafaitEnv.PRINT_RECEIPT_ON_BILL_PRINTER != "Y")
            {
                log.LogMethodExit(false);
                return false;
            }

            TransactionReceipt = POSPrint.PrintReceipt(printTransaction, posPrinterDTO, splitId, RePrint);
            if (TransactionReceipt.TotalLines == 0)
            {
                log.LogMethodExit(false);
                return false;
            }

            PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext, posPrinterDTO.PrinterDTO);
            PrintDocument MyPrintDocument = new PrintDocument();
            if (SetupThePrinting(MyPrintDocument, printTransaction, posPrinterDTO.PrinterDTO))
            {
                int receiptLineIndex = 0;
                MyPrintDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                {
                    e.HasMorePages = printerBL.PrintReceiptToPrinter(TransactionReceipt, ref receiptLineIndex, e);
                };

                using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
                {
                    //code to send print document to the printer
                    MyPrintDocument.Print();
                }

                //MyPrintDocument.Print();

                if (printTransaction.Utilities.ParafaitEnv.CUT_RECEIPT_PAPER)
                    printerBL.CutPrinterPaper(MyPrintDocument.PrinterSettings.PrinterName);
            }

            if (RePrint)//Starts: Modified on 17-May-2016 for PosPlus duplicate print count
            {
                printTransaction.updateReprintCount();
            }//Ends: Modified on 17-May-2016 for PosPlus duplicate print count
            else if (posPrinterDTO.PrinterDTO != null &&
                     posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter)
            {
                printTransaction.UpdatePrintCount(); //record print count
            }

            if (posPrinterDTO.SecondaryPrinterId > -1)
            {
                if (posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter
                        && Utilities.getParafaitDefaults("ALLOW_MULTIPLE_TRX_PRINT_COPIES") == "Y"
                        && printTransaction.Status == Transaction.TrxStatus.CLOSED)
                {
                    List<Transaction.TransactionLine> trxLines = new List<Transaction.TransactionLine>();
                    trxLines = printTransaction.TrxLines.FindAll(x => x.LineValid == true && x.TrxProfileId != -1 && x.tax_percentage == 0 && printTransaction.TrxProfileVerificationRequired(x.TrxProfileId));
                    if (trxLines.Count <= 0)
                    {
                        log.LogMethodExit(true);
                        return true;
                    }
                }

                PrinterBL secondayPrinterBL = new PrinterBL(Utilities.ExecutionContext, posPrinterDTO.SecondaryPrinterDTO);
                MyPrintDocument = new PrintDocument();
                if (SetupThePrinting(MyPrintDocument, printTransaction, posPrinterDTO.SecondaryPrinterDTO))
                {
                    int receiptLineIndex = 0;

                    if (posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter
                        && Utilities.getParafaitDefaults("ALLOW_MULTIPLE_TRX_PRINT_COPIES") == "Y"
                        && printTransaction.Status == Transaction.TrxStatus.CLOSED)
                    {
                        for (int i = 0; i < TransactionReceipt.TotalLines; i++)
                        {
                            if (TransactionReceipt.ReceiptLines[i].TemplateSection == "HEADER")
                            {
                                if (TransactionReceipt.ReceiptLines[i].Data[0] == Utilities.MessageUtils.getMessage("Invoice")
                                   || TransactionReceipt.ReceiptLines[i].Data[0] == Utilities.MessageUtils.getMessage("Customer Copy"))
                                {
                                    TransactionReceipt.ReceiptLines[i].Data[0] = Utilities.MessageUtils.getMessage("Accounting Copy");
                                    break;
                                }
                            }
                        }
                    }
                    MyPrintDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                    {
                        e.HasMorePages = printerBL.PrintReceiptToPrinter(TransactionReceipt, ref receiptLineIndex, e);
                    };

                    MyPrintDocument.Print();

                    if (printTransaction.Utilities.ParafaitEnv.CUT_RECEIPT_PAPER)
                        secondayPrinterBL.CutPrinterPaper(MyPrintDocument.PrinterSettings.PrinterName);
                }
            }
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// print card method to prepare card printing 
        /// </summary>
        /// <param name="printTransaction"></param>
        /// <param name="posPrinterDTO"></param>
        /// <returns>true or false</returns>
        bool printCard(Transaction printTransaction, POSPrinterDTO posPrinterDTO)
        {
            log.LogMethodEntry(printTransaction, posPrinterDTO);
            try
            {
                Utilities Utilities = printTransaction.Utilities;
                PrinterBL printerBL = new PrinterBL(printTransaction.Utilities.ExecutionContext, posPrinterDTO.PrinterDTO);
                if (PrintProgressUpdates != null)
                {
                    printerBL.PrintProgressUpdates = new PrinterBL.ProgressUpdates(PrintProgressUpdates);
                }
                if (SetCardPrinterErrorValue != null)
                {
                    printerBL.SetCardPrinterErrorValue = new PrinterBL.SetCardPrinterError(SetCardPrinterErrorValue);
                }
                int cardsToPrint = 0;
                //Peru Invoice changes 
                int printerTemplateId = posPrinterDTO.PrintTemplateId;

                if (printTransaction.TrxPOSPrinterOverrideRulesDTOList != null && printTransaction.TrxPOSPrinterOverrideRulesDTOList.Any())
                {
                    TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(Utilities.ExecutionContext);
                    int overRidePrinterTemplateId = trxPOSPrinterOverrideRulesListBL.GetTemplateId(printTransaction.TrxPOSPrinterOverrideRulesDTOList, posPrinterDTO);
                    if (overRidePrinterTemplateId > -1)
                    {
                        printerTemplateId = overRidePrinterTemplateId;
                    }
                }

                TicketList = POSPrint.getTickets(printerTemplateId, printTransaction, ref cardsToPrint, RePrint);
                List<string> distinctCardNumbers = new List<string>();
                distinctCardNumbers = TicketList.Select(x => x.CardNumber).Distinct().ToList();
                List<clsTicket> clsTicketList = new List<clsTicket>();
                foreach (var cardNumber in distinctCardNumbers)
                {
                    List<clsTicket> clsTicketlist = TicketList.Where(x => x.CardNumber == cardNumber).ToList();
                    if (clsTicketlist.Count > 1)
                    {
                        var clsTicketWithMaxTrxLineId = clsTicketlist.Max(y => y.TrxLineId);
                        clsTicketList.Add(clsTicketlist.Where(x => x.TrxLineId == clsTicketWithMaxTrxLineId).FirstOrDefault());
                    }
                    else if (clsTicketlist.Count == 1)
                    {
                        clsTicketList.AddRange(clsTicketlist);
                    }
                }
                TicketList = clsTicketList;
                cardsToPrint = TicketList.Count;
                if (cardsToPrint == 0)
                {
                    // "No cards to print"
                    string returnMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 3002);
                    SendPrintProgressUpdates(returnMsg);
                    log.LogMethodExit(false);
                    return false;
                }
                // "Number of cards to print: &1"
                string updateMsg = MessageContainerList.GetMessage(Utilities.ExecutionContext, 3003, cardsToPrint.ToString(ParafaitDefaultContainerList.GetParafaitDefault(Utilities.ExecutionContext, "NUMBER_FORMAT")));
                SendPrintProgressUpdates(updateMsg);
                int currentCardIndex = 0;
                WristBandPrinter wristBandPrinter = null;
                try
                {
                    if (printerBL.PrinterDTO != null && printerBL.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.RFIDWBPrinter)
                    {
                        wristBandPrinter = GetWristBandPrinter(Utilities.ExecutionContext, printerBL.PrinterDTO);
                        if (wristBandPrinter != null)
                        {
                            wristBandPrinter.Open();
                        }
                    }
                    while (currentCardIndex < cardsToPrint)
                    {
                        string cardNumber = printerBL.PrintCardToPrinter(TicketList[currentCardIndex], wristBandPrinter);
                        if (!string.IsNullOrEmpty(TicketList[currentCardIndex].CardNumber))
                        {
                            Card toCard = new Card(cardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                            Card fromCard = new Card(TicketList[currentCardIndex].CardNumber, Utilities.ParafaitEnv.LoginID, Utilities);
                            int fromCardId = fromCard.card_id;
                            TaskProcs taskprocs = new TaskProcs(Utilities);
                            string message = "";
                            if (!taskprocs.transferCard(fromCard, toCard, "Card Printed", ref message, null, printTransaction.Trx_id))
                            {
                                log.LogMethodExit(null, "Throwing ApplicationException " + message);
                                SendPrintProgressUpdates(cardNumber + ": " + message);
                                SetCardPrinterErrorValue(true);
                                throw new ApplicationException(message);
                            }
                            SendPrintProgressUpdates(MessageContainerList.GetMessage(Utilities.ExecutionContext, 3004, cardNumber));
                            // "&1 is successfully processed"
                            try
                            {
                                object newTaskIdValue = Utilities.executeScalar(@"select top 1 t.task_id
                                                   from tasks t, task_type tt
                                                    where tt.task_type = 'TRANSFERCARD'
                                                    and tt.task_type_id = t.task_type_id
                                                    and t.card_Id = @toCardId
                                                    and t.transfer_to_card_id = @fromCardId
                                                    and user_id = @userId 
                                                    order by t.task_date desc",
                                                           new SqlParameter[]
                                                         {
                                                     new SqlParameter("@fromCardId", fromCardId),
                                                     new SqlParameter("@toCardId", toCard.card_id),
                                                     new SqlParameter("@userId", Utilities.ExecutionContext.GetUserPKId())
                                                         });

                                if (newTaskIdValue != null)
                                {
                                    Utilities.executeNonQuery(@"update tasks
                                                               set trxId = @trxId, LastUpdateDate = getdate(), LastUpdatedBy=@loginId
                                                             where task_id = @taskId
                                                                ",
                                                       new SqlParameter[]
                                                       {
                                                     new SqlParameter("@trxId", printTransaction.Trx_id),
                                                     new SqlParameter("@taskId",newTaskIdValue),
                                                     new SqlParameter("@loginId",Utilities.ExecutionContext.GetUserId()),
                                                       });
                                }
                                foreach (Transaction.TransactionLine item in printTransaction.TrxLines)
                                {
                                    if (item.CardNumber == fromCard.CardNumber)
                                    {
                                        item.CardNumber = toCard.CardNumber;
                                    }
                                }
                                if (printTransaction.TransactionInfo.TrxProduct != null && printTransaction.TransactionInfo.TrxProduct.Any())
                                {
                                    List<Transaction.clsTransactionInfo.ProductInfo> productInfoList = printTransaction.TransactionInfo.TrxProduct.Where(x => x.cardNumber == fromCard.CardNumber).ToList();
                                    if (productInfoList != null && productInfoList.Any())
                                    {
                                        foreach (Transaction.clsTransactionInfo.ProductInfo prodInfoItem in productInfoList)
                                        {
                                            prodInfoItem.cardNumber = toCard.CardNumber;
                                        }
                                    }
                                    if (printTransaction.TransactionInfo.PrimaryPaymentCardNumber == fromCard.CardNumber)
                                    {
                                        printTransaction.TransactionInfo.PrimaryPaymentCardNumber = toCard.CardNumber;
                                    }
                                }
                                if (printTransaction.PrimaryCard != null
                                    && printTransaction.PrimaryCard.CardNumber == fromCard.CardNumber)
                                {
                                    printTransaction.PrimaryCard.CardNumber = toCard.CardNumber;
                                }
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                            }
                            printTransaction.UpdateTrxLinesReceiptPrintedStatus(TicketList[currentCardIndex].TrxLineId);
                            //Transaction.updateTrxLinesReceiptPrintedStatus(Utilities, TicketList[currentCardIndex].TrxId, TicketList[currentCardIndex].TrxLineId);
                        }
                        currentCardIndex++;
                        if (currentCardIndex < cardsToPrint)
                        {
                            System.Threading.Thread.Sleep(2000); //sleep before next print
                        }
                    }
                }
                finally
                {
                    if (wristBandPrinter != null)
                    {
                        try { wristBandPrinter.Close(); }
                        catch (Exception ex)
                        {
                            log.Error("wristBandPrinter.Close()", ex);
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                SendPrintProgressUpdates(ex.Message);
                SetCardPrinterErrorValue(true);
                log.LogMethodExit(false);
                throw;
            }
            log.LogMethodExit(true);
            return true;
        }

        private WristBandPrinter GetWristBandPrinter(ExecutionContext executionContext, PrinterDTO printer)
        {
            log.LogMethodEntry();
            string writsbandModel = string.Empty;
            WristBandPrinter wristBandPrinter = null;
            LookupsContainerDTO lookupsContainerDTO = LookupsContainerList.GetLookupsContainerDTO(executionContext.GetSiteId(), "RFID_WRISTBAND_MODELS");
            if (lookupsContainerDTO != null &&
                 lookupsContainerDTO.LookupValuesContainerDTOList != null &&
                 lookupsContainerDTO.LookupValuesContainerDTOList.Any()
                 && printer.WBPrinterModel > -1)
            {
                writsbandModel = lookupsContainerDTO.LookupValuesContainerDTOList.Where(x => x.LookupValueId == printer.WBPrinterModel).FirstOrDefault().LookupValue;
            }
            wristBandPrinter = WristbandPrinterFactory.GetInstance(executionContext).GetWristBandPrinter(writsbandModel);
            if (string.IsNullOrWhiteSpace(writsbandModel))
                writsbandModel = "STIMA";
            log.LogVariableState("writsbandModel", writsbandModel);
            switch (writsbandModel)
            {
                case "STIMA":
                    {
                        wristBandPrinter.SetIPAddress(printer.IpAddress);
                    }
                    break;
                case "BOCA":
                    {
                        wristBandPrinter.SetPrinterName(printer.PrinterName);
                    }
                    break;
            }
            log.LogMethodExit(wristBandPrinter);
            return wristBandPrinter;
        }

        public Transaction cloneTrx(Transaction srcTrx)
        {
            log.LogMethodEntry(srcTrx);

            Transaction trgTrx = new Transaction(srcTrx.POSPrinterDTOList, srcTrx.Utilities);
            CommonFuncs cf = new CommonFuncs(srcTrx.Utilities);
            cf.CloneObject(srcTrx, trgTrx);
            foreach (Transaction.TransactionLine tl in srcTrx.TrxLines)
            {
                Transaction.TransactionLine tlNew = new Transaction.TransactionLine();
                cf.CloneObject(tl, tlNew);
                if (tl.LineAtb != null)
                    tlNew.LineAtb = tl.LineAtb;
                if (tl.KOTPrintCount != null)
                    tlNew.KOTPrintCount = tl.KOTPrintCount;
                if (tl.card != null)
                    tlNew.card = tl.card;
                if (tl.LineCheckInDTO != null)
                    tlNew.LineCheckInDTO = tl.LineCheckInDTO;
                if (tl.LineCheckOutDetailDTO != null)
                    tlNew.LineCheckOutDetailDTO = tl.LineCheckOutDetailDTO;
                if (tl.LineCheckInDetailDTO != null)
                    tlNew.LineCheckInDetailDTO = tl.LineCheckInDetailDTO;
                trgTrx.TrxLines.Add(tlNew);
            }

            foreach (Transaction.TransactionLine tl in srcTrx.TrxLines)
            {
                if (tl.ParentLine != null)
                {
                    foreach (Transaction.TransactionLine tlc in trgTrx.TrxLines)
                    {
                        if (tlc.DBLineId == tl.DBLineId)
                        {
                            foreach (Transaction.TransactionLine tlcParent in trgTrx.TrxLines)
                            {
                                if (tl.ParentLine.DBLineId == tlcParent.DBLineId)
                                {
                                    tlc.ParentLine = tlcParent;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (srcTrx.DiscountsSummaryDTOList != null &&
                srcTrx.DiscountsSummaryDTOList.Count > 0)
            {
                trgTrx.DiscountsSummaryDTOList = new List<Semnox.Parafait.Discounts.DiscountsSummaryDTO>();
                trgTrx.DiscountsSummaryDTODictionary = new Dictionary<int, Semnox.Parafait.Discounts.DiscountsSummaryDTO>();
                foreach (var discountsSummaryDTO in srcTrx.DiscountsSummaryDTOList)
                {
                    DiscountsSummaryDTO newDiscountsSummaryDTO = new DiscountsSummaryDTO();
                    newDiscountsSummaryDTO.DiscountAmount = discountsSummaryDTO.DiscountAmount;
                    newDiscountsSummaryDTO.DiscountName = discountsSummaryDTO.DiscountName;
                    newDiscountsSummaryDTO.DiscountPercentage = discountsSummaryDTO.DiscountPercentage;
                    newDiscountsSummaryDTO.DisplayChar = discountsSummaryDTO.DisplayChar;
                    newDiscountsSummaryDTO.DiscountId = discountsSummaryDTO.DiscountId;
                    newDiscountsSummaryDTO.CouponNumbers = new HashSet<string>(discountsSummaryDTO.CouponNumbers);
                    trgTrx.DiscountsSummaryDTOList.Add(newDiscountsSummaryDTO);
                    trgTrx.DiscountsSummaryDTODictionary.Add(newDiscountsSummaryDTO.DiscountId, newDiscountsSummaryDTO);
                }
            }

            if (srcTrx.DiscountApplicationHistoryDTOList != null &&
                srcTrx.DiscountApplicationHistoryDTOList.Count > 0)
            {
                trgTrx.DiscountApplicationHistoryDTOList = new List<DiscountApplicationHistoryDTO>();
                foreach (var discountApplicationHistoryDTO in srcTrx.DiscountApplicationHistoryDTOList)
                {
                    DiscountApplicationHistoryDTO newDiscountApplicationHistoryDTO = new DiscountApplicationHistoryDTO();
                    newDiscountApplicationHistoryDTO.DiscountId = discountApplicationHistoryDTO.DiscountId;
                    newDiscountApplicationHistoryDTO.CouponNumber = discountApplicationHistoryDTO.CouponNumber;
                    newDiscountApplicationHistoryDTO.VariableDiscountAmount = discountApplicationHistoryDTO.VariableDiscountAmount;
                    newDiscountApplicationHistoryDTO.ApprovedBy = discountApplicationHistoryDTO.ApprovedBy;
                    newDiscountApplicationHistoryDTO.Remarks = discountApplicationHistoryDTO.Remarks;
                    if (discountApplicationHistoryDTO.TransactionLineBL != null)
                    {
                        foreach (var newLine in trgTrx.TrxLines)
                        {
                            if (discountApplicationHistoryDTO.TransactionLineBL.TransactionLineDTO != null &&
                                discountApplicationHistoryDTO.TransactionLineBL.TransactionLineDTO.LineId == newLine.DBLineId)
                            {
                                newDiscountApplicationHistoryDTO.TransactionLineBL = newLine;
                                break;
                            }
                        }
                    }
                    newDiscountApplicationHistoryDTO.Remarks = discountApplicationHistoryDTO.Remarks;
                    trgTrx.DiscountApplicationHistoryDTOList.Add(newDiscountApplicationHistoryDTO);
                }
            }


            foreach (Transaction.TransactionLine tls in srcTrx.TrxLines)
            {
                Transaction.TransactionLine tlTarget = null;
                foreach (Transaction.TransactionLine tlt in trgTrx.TrxLines)
                {
                    if (tlt.DBLineId == tls.DBLineId)
                    {
                        tlTarget = tlt;
                        break;
                    }
                }

                if (tlTarget == null)
                    continue;

                if (tls.TransactionDiscountsDTOList != null && tls.TransactionDiscountsDTOList.Count > 0)
                {
                    if (tlTarget.TransactionDiscountsDTOList == null)
                    {
                        tlTarget.TransactionDiscountsDTOList = new List<TransactionDiscountsDTO>();
                    }
                    else
                    {
                        tlTarget.TransactionDiscountsDTOList.Clear();
                    }

                    foreach (var transactionDiscountsDTO in tls.TransactionDiscountsDTOList)
                    {
                        TransactionDiscountsDTO newTransactionDiscountsDTO =
                            new TransactionDiscountsDTO(-1,
                                                        -1,
                                                        -1,
                                                        transactionDiscountsDTO.DiscountId,
                                                        transactionDiscountsDTO.DiscountPercentage,
                                                        transactionDiscountsDTO.DiscountAmount,
                                                        transactionDiscountsDTO.Remarks,
                                                        transactionDiscountsDTO.ApprovedBy,
                                                        transactionDiscountsDTO.Applicability,
                                                        transactionDiscountsDTO.SiteId,
                                                        transactionDiscountsDTO.MasterEntityId,
                                                        transactionDiscountsDTO.SynchStatus,
                                                        transactionDiscountsDTO.Guid,
                                                        transactionDiscountsDTO.CreatedBy,
                                                        transactionDiscountsDTO.CreationDate,
                                                        transactionDiscountsDTO.LastUpdatedBy,
                                                        transactionDiscountsDTO.LastUpdatedDate);
                        if (transactionDiscountsDTO.DiscountCouponsUsedDTO != null)
                        {
                            DiscountCouponsUsedDTO discountCouponsUsedDTO = new DiscountCouponsUsedDTO(-1,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.CouponSetId,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.DiscountId,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.DiscountCouponHeaderId,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.CouponNumber,
                                                                                                      -1,
                                                                                                      -1,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.IsActive,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.SiteId,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.MasterEntityId,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.SynchStatus,
                                                                                                      transactionDiscountsDTO.DiscountCouponsUsedDTO.Guid,
                                                                                                      srcTrx.Utilities.ExecutionContext.GetUserId(),
                                                                                                      srcTrx.Utilities.getServerTime(),
                                                                                                      srcTrx.Utilities.ExecutionContext.GetUserId(),
                                                                                                      srcTrx.Utilities.getServerTime());
                            newTransactionDiscountsDTO.DiscountCouponsUsedDTO = discountCouponsUsedDTO;
                        }
                        tlTarget.TransactionDiscountsDTOList.Add(newTransactionDiscountsDTO);

                    }
                }

                if (tls.IssuedDiscountCouponsDTOList != null && tls.IssuedDiscountCouponsDTOList.Count > 0)
                {
                    if (tlTarget.IssuedDiscountCouponsDTOList == null)
                    {
                        tlTarget.IssuedDiscountCouponsDTOList = new List<DiscountCouponsDTO>();
                    }
                    else
                    {
                        tlTarget.IssuedDiscountCouponsDTOList.Clear();
                    }

                    foreach (var discountCouponsDTO in tls.IssuedDiscountCouponsDTOList)
                    {
                        DiscountCouponsDTO newDiscountCouponsDTO =
                            new DiscountCouponsDTO(discountCouponsDTO.CouponSetId,
                                                    discountCouponsDTO.DiscountCouponHeaderId,
                                                    discountCouponsDTO.DiscountId,
                                                    discountCouponsDTO.TransactionId,
                                                    discountCouponsDTO.LineId,
                                                    discountCouponsDTO.Count,
                                                    discountCouponsDTO.UsedCount,
                                                    discountCouponsDTO.PaymentModeId,
                                                    discountCouponsDTO.FromNumber,
                                                    discountCouponsDTO.ToNumber,
                                                    discountCouponsDTO.StartDate,
                                                    discountCouponsDTO.ExpiryDate,
                                                    discountCouponsDTO.CouponValue,
                                                    discountCouponsDTO.IsActive,
                                                    discountCouponsDTO.LastUpdatedDate,
                                                    discountCouponsDTO.SiteId,
                                                    discountCouponsDTO.MasterEntityId,
                                                    discountCouponsDTO.SynchStatus,
                                                    discountCouponsDTO.Guid,
                                                    discountCouponsDTO.CreatedBy,
                                                    discountCouponsDTO.CreationDate,
                                                    discountCouponsDTO.LastUpdatedBy);
                        tlTarget.IssuedDiscountCouponsDTOList.Add(newDiscountCouponsDTO);
                    }
                }
            }

            trgTrx.TransactionInfo = srcTrx.TransactionInfo;

            log.LogMethodExit(trgTrx);
            return trgTrx;
        }


        //method which is used to get the transaction receipt for handheld devices.
        //it takes transaction object,printer index, page width and height for creting the trx receipt imageand returns base64 string.
        public string printPosReceipt(Transaction printTransaction, POSPrinterDTO posPrinterDTO, int splitId, int width, int height, bool secondaryPrint = false, bool generatePDF = false)
        {
            log.LogMethodEntry(printTransaction, posPrinterDTO, splitId, width, height);

            string base64String = "";
            Utilities Utilities = printTransaction.Utilities;
            if (printTransaction.Utilities.ParafaitEnv.PRINT_RECEIPT_ON_BILL_PRINTER != "Y")
            {
                log.LogMethodExit(null);
                return null;
            }
            //if trx lines has any cardsthen that is to be added to Transaction.TransactionInfo.TrxCards
            List<Transaction.clsTransactionInfo.CardInfo> trxCards = new List<Transaction.clsTransactionInfo.CardInfo>();
            foreach (Transaction.TransactionLine trxLine in printTransaction.TrxLines)
            {
                if ((!string.IsNullOrEmpty(trxLine.CardNumber) && ((trxLine.ProductTypeCode.CompareTo("NEW") == 0) || (trxLine.ProductTypeCode.CompareTo("CARDSALE") == 0))))
                {
                    Transaction.clsTransactionInfo.CardInfo cardInfoItem = new Transaction.clsTransactionInfo.CardInfo();
                    cardInfoItem.CardNumber = trxLine.CardNumber;
                    trxCards.Add(cardInfoItem);
                }
            }

            if (trxCards.Count > 0)
            {
                printTransaction.TransactionInfo.TrxCards = trxCards;
            }

            List<POSPrinterDTO> posPrinterDTOList = new List<POSPrinterDTO>();
            posPrinterDTOList.Add(posPrinterDTO);
            printTransaction.GetPrintableTransactionLines(posPrinterDTOList);
            //checking the displaygroup of selected product under "Print these displaygroup" list and the product details is printable?
            Transaction.EligibleTrxLinesPrinterMapper printTrxLinesMapper = printTransaction.EligibleTrxLinesPrinterMapperList.Find(tl => tl.POSPrinterDTO.Equals(posPrinterDTO));
            printTransaction.TrxLines.Clear();
            printTransaction.TrxLines = printTrxLinesMapper.TrxLines;

            //creating transaction receipt class based on passed transaction object and template configured to passed printer index
            TransactionReceipt = POSPrint.PrintReceipt(printTransaction, posPrinterDTO, splitId, RePrint);
            if (TransactionReceipt.TotalLines == 0)
            {
                log.LogMethodExit(null);
                return null;
            }

            // Print one more copy for BIR
            if (posPrinterDTO.SecondaryPrinterId > -1 && secondaryPrint)
            {
                if (posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter
                        && Utilities.getParafaitDefaults("ALLOW_MULTIPLE_TRX_PRINT_COPIES") == "Y"
                        && printTransaction.Status == Transaction.TrxStatus.CLOSED)
                {
                    List<Transaction.TransactionLine> trxLines = new List<Transaction.TransactionLine>();
                    trxLines = printTransaction.TrxLines.FindAll(x => x.LineValid == true && x.TrxProfileId != -1 && x.tax_percentage == 0 && printTransaction.TrxProfileVerificationRequired(x.TrxProfileId));
                    if (trxLines.Count <= 0)
                    {
                        log.LogMethodExit(true);
                        return "";
                    }
                }

                if (posPrinterDTO.PrinterDTO.PrinterType == PrinterDTO.PrinterTypes.ReceiptPrinter
                    && Utilities.getParafaitDefaults("ALLOW_MULTIPLE_TRX_PRINT_COPIES") == "Y"
                    && printTransaction.Status == Transaction.TrxStatus.CLOSED)
                {
                    for (int i = 0; i < TransactionReceipt.TotalLines; i++)
                    {
                        if (TransactionReceipt.ReceiptLines[i].TemplateSection == "HEADER")
                        {
                            if (TransactionReceipt.ReceiptLines[i].Data[0] == Utilities.MessageUtils.getMessage("Invoice")
                                || TransactionReceipt.ReceiptLines[i].Data[0] == Utilities.MessageUtils.getMessage("Customer Copy"))
                            {
                                TransactionReceipt.ReceiptLines[i].Data[0] = Utilities.MessageUtils.getMessage("Accounting Copy");
                                break;
                            }
                        }
                    }
                }
            }

            PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext, posPrinterDTO.PrinterDTO);
            if (height == -1)
                height = 4000;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            Rectangle margins = new Rectangle();
            margins.Height = bitmap.Height;
            margins.Width = bitmap.Width;
            graphics.FillRectangle(Brushes.White, margins);

            int newHeightOnPage = 0;
            int receiptLineIndex = 0;
            bool status = printerBL.PrintReceiptToPrinter(TransactionReceipt, ref receiptLineIndex, graphics, margins, ref newHeightOnPage);

            //resizing the image, removing the remaining white space
            Rectangle newImageSizeRectngle = new Rectangle(0, 0, width, newHeightOnPage);
            Bitmap finalReceiptImage = new Bitmap(newImageSizeRectngle.Width, newImageSizeRectngle.Height);
            using (Graphics g = Graphics.FromImage(finalReceiptImage))
            {
                g.DrawImage(
                    bitmap,
                    new Rectangle(0, 0, finalReceiptImage.Width, finalReceiptImage.Height),
                    newImageSizeRectngle,
                    GraphicsUnit.Pixel
                    );
            }

            //increasing the image clarity to high while saving. setting the quality parameters 
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
            string lookupKey = "image/png";
            var jpegCodec = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupKey)).FirstOrDefault();
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            //saving and converting the created image into base64string
            using (MemoryStream memory = new MemoryStream())
            {
                finalReceiptImage.Save(memory, jpegCodec, encoderParams);
                byte[] imageBytes = memory.ToArray();
                base64String = Convert.ToBase64String(imageBytes);

            }
            log.LogMethodExit(base64String);

            if (generatePDF)
            {
                String PDFString = "";
                if (!String.IsNullOrEmpty(base64String))
                    PDFString = printTransaction.Utilities.ConvertImageToPDF(new List<string>() { base64String });
                log.LogMethodExit(PDFString);
                return PDFString;
            }
            else
                return base64String;
        }

        //method which is used to get the transaction receipt for handheld devices.
        //it takes transaction object,printer index, page width and height for creting the trx receipt imageand returns base64 string.
        public List<string> printTickets(Transaction printTransaction, POSPrinterDTO posPrinterDTO, int splitId, int width, int height, bool secondaryPrint = false, bool generatePDF = false)
        {
            log.LogMethodEntry(printTransaction, posPrinterDTO, splitId, width, height);

            Utilities Utilities = printTransaction.Utilities;
            if (Utilities.getParafaitDefaults("PRINT_TRANSACTION_ITEM_TICKETS") != "Y")
            {
                log.LogMethodExit(false);
                return null;
            }

            int TicketsToPrint = 0;
            int printerTemplateId = posPrinterDTO.PrintTemplateId;

            if (printTransaction.TrxPOSPrinterOverrideRulesDTOList != null && printTransaction.TrxPOSPrinterOverrideRulesDTOList.Any())
            {
                TrxPOSPrinterOverrideRulesListBL trxPOSPrinterOverrideRulesListBL = new TrxPOSPrinterOverrideRulesListBL(Utilities.ExecutionContext);
                int overridePrinterTemplateId = trxPOSPrinterOverrideRulesListBL.GetTemplateId(printTransaction.TrxPOSPrinterOverrideRulesDTOList, posPrinterDTO);
                if (overridePrinterTemplateId > -1)
                {
                    printerTemplateId = overridePrinterTemplateId;
                }
            }
            TicketList = POSPrint.getTickets(printerTemplateId, printTransaction, ref TicketsToPrint, RePrint);
            if (TicketsToPrint == 0)
            {
                log.LogMethodExit(false);
                log.Info("No tickets to print trx:" + printTransaction.Trx_id);
                return null;
            }
            PrinterBL printerBL = new PrinterBL(Utilities.ExecutionContext, posPrinterDTO.PrinterDTO);

            if (height == -1)
                height = 4000;

            List<string> ticketImages = new List<string>();


            int currentTicket = 0;
            while (currentTicket < TicketsToPrint)
            {
                string base64String = "";

                Bitmap bitmap = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bitmap);
                Rectangle margins = new Rectangle();
                margins.Height = bitmap.Height;
                margins.Width = bitmap.Width;
                graphics.FillRectangle(Brushes.White, margins);

                int newHeightOnPage = 100;

                PrintDocument PrintTicketDocument = new PrintDocument();
                //if (SetupThePrinting(PrintTicketDocument, printTransaction, posPrinterDTO.PrinterDTO))
                {
                    PrintTicketDocument.DefaultPageSettings.Margins = TicketList[0].MarginProperty;
                    newHeightOnPage += TicketList[0].PaperSizeProperty.Height + TicketList[0].MarginProperty.Top + TicketList[0].MarginProperty.Bottom;
                    // PrintTicketDocument.DefaultPageSettings.PaperSize = TicketList[0].PaperSize;
                    printerBL.PrintTicketsToPrinter(TicketList, currentTicket, graphics);
                    currentTicket++;
                }

                //resizing the image, removing the remaining white space
                Rectangle newImageSizeRectngle = new Rectangle(0, 0, width, newHeightOnPage);
                Bitmap finalReceiptImage = new Bitmap(newImageSizeRectngle.Width, newImageSizeRectngle.Height);
                using (Graphics g = Graphics.FromImage(finalReceiptImage))
                {
                    g.DrawImage(
                        bitmap,
                        new Rectangle(0, 0, finalReceiptImage.Width, finalReceiptImage.Height),
                        newImageSizeRectngle,
                        GraphicsUnit.Pixel
                        );
                }

                //increasing the image clarity to high while saving. setting the quality parameters 
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);
                string lookupKey = "image/png";
                var jpegCodec = ImageCodecInfo.GetImageEncoders().Where(i => i.MimeType.Equals(lookupKey)).FirstOrDefault();
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                //saving and converting the created image into base64string
                using (MemoryStream memory = new MemoryStream())
                {
                    finalReceiptImage.Save(memory, jpegCodec, encoderParams);
                    byte[] imageBytes = memory.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);

                }

                ticketImages.Add(base64String);
            }

            if (generatePDF)
            {
                String PDFString = "";
                if (ticketImages != null && ticketImages.Any())
                    PDFString = Utilities.ConvertImageToPDF(ticketImages);
                log.LogMethodExit(PDFString);
                return new List<string>() { PDFString };
            }
            else
                return ticketImages;
        }

        public static string PrintReceipt(ExecutionContext executionContext, int templateId)
        {
            log.LogMethodEntry(executionContext, templateId);
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.Initialize();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.Username = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ParafaitEnv.POSTypeId = Convert.ToInt32(ConfigurationManager.AppSettings["POSTypeId"]);
            string ipAddress = "";
            try
            {
                ipAddress = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList[0].ToString();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            utilities.ParafaitEnv.SetPOSMachine(ipAddress, Environment.MachineName);
            //Transaction trx = new Transaction(utilities);

            //PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1);
            //ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateHeaderBL(executionContext, templateId, true).ReceiptPrintTemplateHeaderDTO;
            //POSPrinterDTO posPrinterDTO = new POSPrinterDTO(-1, -1, -1, -1, -1, -1, templateId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1);

            Transaction trx = new Transaction(utilities);
            Dictionary<string, string> items = new Dictionary<string, string>();
            Semnox.Parafait.Transaction.Card card = new Semnox.Parafait.Transaction.Card("1234567890", executionContext.GetUserId(), utilities);
            trx.Remarks = "Test Print Template";
            string message = "";

            Semnox.Parafait.Product.Products products = new Semnox.Parafait.Product.Products();
            Semnox.Parafait.Product.ProductsDTO productsDTOManual = null;
            List<Semnox.Parafait.Product.ProductsDTO> productsDTOList = products.GetProductByTypeList("MANUAL", executionContext.GetSiteId());
            if (productsDTOList != null && productsDTOList.Count > 0)
            {
                productsDTOManual = productsDTOList[0];
            }
            if (productsDTOManual == null)
            {
                throw new Exception("Manual product not found");
            }
            int productId = productsDTOManual.ProductId;
            try
            {
                trx.createTransactionLine(card, productId, -1, 2, ref message);
            }
            catch { }
            productsDTOList = null;
            productsDTOList = products.GetProductByTypeList("CARDSALE", executionContext.GetSiteId());
            ProductsDTO productsDTOcard = null;
            if (productsDTOList != null && productsDTOList.Count > 0)
            {
                productsDTOcard = productsDTOList[0];
            }
            if (productsDTOcard == null)
            {
                productsDTOList = null;
                productsDTOList = products.GetProductByTypeList("NEW", executionContext.GetSiteId());
                if (productsDTOList != null && productsDTOList.Count > 0)
                {
                    productsDTOcard = productsDTOList[0];
                }
            }
            if (productsDTOcard == null)
            {
                throw new Exception("Card product not found");
            }
            productId = productsDTOcard.ProductId;
            message = "";
            try
            {
                trx.createTransactionLine(card, productId, 1, ref message);
            }
            catch { }

            List<DiscountsDTO> discountsDTOList = null;
            using(UnitOfWork unitOfWork = new UnitOfWork())
            {
                DiscountsListBL discountsListBL = new DiscountsListBL(executionContext, unitOfWork);
                SearchParameterList<DiscountsDTO.SearchByParameters> searchDiscountsParams = new SearchParameterList<DiscountsDTO.SearchByParameters>();
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.DISCOUNT_TYPE, DiscountsBL.DISCOUNT_TYPE_TRANSACTION));
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.ACTIVE_FLAG, "Y"));
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.AUTOMATIC_APPLY, "N"));
                searchDiscountsParams.Add(new KeyValuePair<DiscountsDTO.SearchByParameters, string>(DiscountsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                discountsDTOList = discountsListBL.GetDiscountsDTOList(searchDiscountsParams);
            }
            
            if (discountsDTOList != null && discountsDTOList.Count > 0)
            {
                DiscountsDTO discountsDTO = discountsDTOList[0];
                foreach (var item in discountsDTOList)
                {
                    if (item.VariableDiscounts == "N")
                    {
                        discountsDTO = item;
                        break;
                    }
                }
                string remarks = string.Empty;
                int approvedBy = -1; //
                decimal? variableAmount = null;
                if (discountsDTO.RemarksMandatory == "Y")
                {
                    remarks = "Mandatory Remarks";
                }
                if (discountsDTO.ManagerApprovalRequired == "Y")
                {
                    approvedBy = Common.ParafaitEnv.User_Id;
                }
                if (discountsDTO.VariableDiscounts == "Y")
                {
                    variableAmount = 10;
                }
                try
                {
                    trx.ApplyDiscount(discountsDTO.DiscountId, remarks, approvedBy, variableAmount);
                }
                catch { }
            }
            items.Add("product_type", "product_type");
            items.Add("card_number", "card_number");
            items.Add("product_name", "product_name");
            items.Add("quantity", "quantity");
            items.Add("price", "price");
            items.Add("tax", "tax");
            items.Add("line_amount", "line_amount");
            items.Add("lineId", "lineId");
            items.Add("line_type", "line_type");
            items.Add("Remarks", "Remarks");
            //dgv.AllowUserToAddRows = false;

            PrinterDTO printerDTO = new PrinterDTO(-1, "Default", "Default", 0, true, DateTime.Now, "", DateTime.Now, "", "", "", -1, PrinterDTO.PrinterTypes.ReceiptPrinter, -1, "", false, -1, -1, 0);
            //get default value for Receipt Template ID based on configuration RECEIPT_PRINT_TEMPLATE
            ReceiptPrintTemplateHeaderDTO receiptPrintTemplateDTO = new ReceiptPrintTemplateHeaderBL(executionContext, templateId, true).ReceiptPrintTemplateHeaderDTO;
            POSPrinterDTO posPrinterDTO = new POSPrinterDTO(-1, -1, -1, -1, -1, -1, templateId, printerDTO, null, receiptPrintTemplateDTO, true, DateTime.Now, "", DateTime.Now, "", -1, "", false, -1, -1);

            if (trx.Trx_id == 0)
                trx.Trx_id = 12345;

            trx.TransactionInfo = new Transaction.clsTransactionInfo(utilities, trx);
            int lineNumber = 1;
            foreach (Transaction.TransactionLine trxLine in trx.TrxLines)
            {
                Transaction.clsTransactionInfo.ProductInfo productInfo = new Transaction.clsTransactionInfo.ProductInfo();
                productInfo.DBLineId = lineNumber;
                trxLine.DBLineId = lineNumber;
                productInfo.amount = trxLine.LineAmount;
                productInfo.productName = trxLine.ProductName;
                productInfo.productType = trxLine.ProductTypeCode;
                productInfo.cancelledLine = false;
                productInfo.amountInclTax = trxLine.LineAmount;
                productInfo.cardNumber = trxLine.card != null ? trxLine.card.CardNumber : "";
                productInfo.kotQuantity = trxLine.ProductTypeCode == "MANUAL" ? 1 : 0;
                productInfo.lineType = "P";
                productInfo.price = trxLine.Price;
                productInfo.printEligible = true;
                productInfo.productId = trxLine.ProductID;
                productInfo.quantity = trxLine.quantity;
                productInfo.tax = trxLine.tax_amount;
                productInfo.taxName = trxLine.taxName;
                productInfo.taxPercentage = trxLine.tax_percentage;
                trx.TransactionInfo.TrxProduct.Add(productInfo);
                lineNumber++;
            }
            ReceiptClass returnValue = POSPrint.PrintReceipt(trx, posPrinterDTO, false);
            Image image = new PrinterBuildBL(executionContext).ReceiptImage(returnValue, 0, new Rectangle(0, 0, 280, 960), 0);
            ImageConverter imageConverter = new ImageConverter();
            string imagebase64String = Convert.ToBase64String((byte[])imageConverter.ConvertTo(image, typeof(byte[])));
            log.LogMethodExit(imagebase64String);
            return imagebase64String;
        }

        public static string TicketTemplatePreview(ExecutionContext executionContext, int templateId)
        {
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.Initialize();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            utilities.ParafaitEnv.POSTypeId = Convert.ToInt32(ConfigurationManager.AppSettings["POSTypeId"]);
            Transaction trx = new Transaction(utilities);
            List<clsTicket> TicketList;
            PrinterBuildBL printerBL = new PrinterBuildBL(executionContext);
            Card card = new Card("12345678", executionContext.GetUserId(), utilities);
            trx.Remarks = "Test Print Template";
            string message = "";
            TicketTemplateElementListBL ticketTemplateElementBL = new TicketTemplateElementListBL(executionContext);
            List<KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>> searchParam = new List<KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>>();
            searchParam.Add(new KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>(TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParam.Add(new KeyValuePair<TicketTemplateElementDTO.SearchByTicketTemplateElementParameters, string>(TicketTemplateElementDTO.SearchByTicketTemplateElementParameters.TICKET_TEMPLATE_ID, templateId.ToString()));
            List<TicketTemplateElementDTO> ticketTemplateElementDTOList = ticketTemplateElementBL.GetTicketTemplateElementDTOList(searchParam);
            bool containsCouponRelatedInfo = false;
            if (ticketTemplateElementDTOList != null && ticketTemplateElementDTOList.Count > 0)
            {
                foreach (TicketTemplateElementDTO ticketTemplateElementDTO in ticketTemplateElementDTOList)
                {
                    if (ticketTemplateElementDTO.Name != null && (ticketTemplateElementDTO.Name == "@CouponNumber" || ticketTemplateElementDTO.Name == "@DiscountName" || ticketTemplateElementDTO.Name == "@DiscountPercentage" || ticketTemplateElementDTO.Name == "@DiscountAmount" || ticketTemplateElementDTO.Name == "@CouponEffectiveDate" || ticketTemplateElementDTO.Name == "@CouponExpiryDate" || ticketTemplateElementDTO.Name == "@BarCodeCouponNumber" || ticketTemplateElementDTO.Name == "@QRCodeCouponNumber"))
                    {
                        containsCouponRelatedInfo = true;
                        break;
                    }
                }
            }
            TicketTemplateHeaderBL ticketTemplateHeaderBL = new TicketTemplateHeaderBL(executionContext, templateId, false, false);
            SqlCommand cmd = new DBUtils().getCommand();
            if (containsCouponRelatedInfo)
            {
                cmd.CommandText = "select top 1 product_id from products p, product_type pt where pt.product_type_id = p.product_type_id and pt.product_type = 'VOUCHER'";
            }
            else
            {
                cmd.CommandText = "select top 1 product_id from products p, product_type pt where pt.product_type_id = p.product_type_id and pt.product_type = 'ATTRACTION'";
            }
            int productId = Convert.ToInt32(cmd.ExecuteScalar());
            try
            {
                trx.createTransactionLine(card, productId, -1, 1, ref message);
                if (containsCouponRelatedInfo)
                {
                    DiscountCouponsDTO discountCouponsDTO = new DiscountCouponsDTO();
                    discountCouponsDTO.FromNumber = "AB123456789";
                    discountCouponsDTO.StartDate = DateTime.Today;
                    discountCouponsDTO.ExpiryDate = DateTime.Today.AddDays(10);
                    List<DiscountCouponsDTO> discountCouponsDTOList = new List<DiscountCouponsDTO>();
                    discountCouponsDTOList.Add(discountCouponsDTO);
                    trx.TrxLines[0].IssuedDiscountCouponsDTOList = discountCouponsDTOList;
                }
            }
            catch { }
            int TicketsToPrint = 0;
            TicketList = POSPrint.getTickets(ticketTemplateHeaderBL.TicketTemplateHeaderDTO.TemplateId, trx, ref TicketsToPrint);
            if (TicketList == null)
            {
                log.Error("No active products found.Please add product");
                throw new Exception(utilities.MessageUtils.getMessage(846)); // No active products found. Please add attraction product
            }
            Image image = printerBL.PrintTicketsToPrinter(TicketList, 0, (int)(ticketTemplateHeaderBL.TicketTemplateHeaderDTO.Width * 100), (int)(ticketTemplateHeaderBL.TicketTemplateHeaderDTO.Height * 100));
            ImageConverter imageConverter = new ImageConverter();
            string imagebase64String = Convert.ToBase64String((byte[])imageConverter.ConvertTo(image, typeof(byte[])));
            log.LogMethodExit(imagebase64String);
            return imagebase64String;
        }

        private void SendPrintProgressUpdates(string message)
        {
            log.LogMethodEntry(message);
            if (PrintProgressUpdates != null)
            {
                PrintProgressUpdates(message);
            }
            else
            {
                log.Info("PrintProgressUpdates is not defined. Hence no message sent back");
            }
            log.LogMethodExit();
        }

        private void SendCardPrinterErrorValue(bool errorValue)
        {
            log.LogMethodEntry(errorValue);
            if (SetCardPrinterErrorValue != null)
            {
                SetCardPrinterErrorValue(errorValue);
            }
            else
            {
                log.Info("SetCardPrinterErrorValue is not defined. Hence no error value sent back");
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Generic Receipt Print
        /// </summary>
        /// <param name="printTransaction"></param>
        /// <param name="posPrinterDTO"></param>
        public void GenericReceiptPrint(Transaction printTransaction, POSPrinterDTO posPrinterDTO)
        {
            log.LogMethodEntry(printTransaction, posPrinterDTO);

            Utilities utilities = printTransaction.Utilities;
            printTransaction.TransactionInfo.createTransactionInfo(printTransaction.Trx_id, -1);
            List<POSPrinterDTO> posPrinterDTOList = new List<POSPrinterDTO>();
            posPrinterDTOList.Add(posPrinterDTO);
            printTransaction.GetPrintableTransactionLines(posPrinterDTOList);
            //checking the displaygroup of selected product under "Print these displaygroup" list and the product details is printable?
            Transaction.EligibleTrxLinesPrinterMapper printTrxLinesMapper = printTransaction.EligibleTrxLinesPrinterMapperList.Find(tl => tl.POSPrinterDTO.Equals(posPrinterDTO));
            printTransaction.TrxLines.Clear();
            printTransaction.TrxLines = printTrxLinesMapper.TrxLines;

            TransactionReceipt = POSPrint.PrintReceipt(printTransaction, posPrinterDTO, -1, true);
            if (TransactionReceipt.TotalLines == 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(utilities.ExecutionContext, 2412, printTransaction.Trx_id));
                //Unable to generate receipt for transaction with id &1.
            }

            PrinterBL printerBL = new PrinterBL(utilities.ExecutionContext, posPrinterDTO.PrinterDTO);
            PrintDocument MyPrintDocument = new PrintDocument();
            if (SetupThePrinting(MyPrintDocument, printTransaction, posPrinterDTO.PrinterDTO))
            {
                int receiptLineIndex = 0;
                MyPrintDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
                {
                    e.HasMorePages = printerBL.PrintReceiptToPrinter(TransactionReceipt, ref receiptLineIndex, e);
                };
                using (WindowsImpersonationContext wic = WindowsIdentity.Impersonate(IntPtr.Zero))
                {
                    //code to send print document to the printer
                    MyPrintDocument.Print();
                }
                if (printTransaction.Utilities.ParafaitEnv.CUT_RECEIPT_PAPER)
                    printerBL.CutPrinterPaper(MyPrintDocument.PrinterSettings.PrinterName);
            }
            log.LogMethodExit();
        }
        private void CreateKOTOrderEntry(Transaction trx, List<Transaction.TransactionLine> kotLines, Transaction.EligibleTrxLinesPrinterMapper eligibleTrxLinesPrinterMapper)
        {
            log.LogMethodEntry(kotLines, eligibleTrxLinesPrinterMapper);
            try
            {
                KDSOrderLineListBL kdsOrderLineListBL = new KDSOrderLineListBL(trx.Utilities.ExecutionContext);
                List<KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>(KDSOrderLineDTO.SearchByParameters.TRX_ID, trx.Trx_id.ToString()));
                searchParameters.Add(new KeyValuePair<KDSOrderLineDTO.SearchByParameters, string>(KDSOrderLineDTO.SearchByParameters.KDSKOT_ENTRY_TYPE, KDSOrderLineDTO.KDSKOTEntryType.KOT.ToString()));
                List<KDSOrderLineDTO> kdsOrderLineDTOList = kdsOrderLineListBL.GetKDSOrderLineDTOList(searchParameters);
                List<Transaction.TransactionLine> kotLinesTemp = new List<Transaction.TransactionLine>();
                if (kdsOrderLineDTOList != null && kdsOrderLineDTOList.Any())
                {
                    for (int i = 0; i < kotLines.Count; i++)
                    {
                        //Create entry if it is not already having one.  
                        if (kdsOrderLineDTOList.Exists(kot => kot.LineId == kotLines[i].DBLineId && kot.TrxId == trx.Trx_id) == false)
                        {
                            kotLinesTemp.Add(kotLines[i]);
                        }
                    }
                }
                else
                {
                    kotLinesTemp = kotLines;
                }
                using (ParafaitDBTransaction sqlTrx = new ParafaitDBTransaction())
                {
                    sqlTrx.BeginTransaction();
                    bool isKDSOrder = false;
                    KDSOrderBL kdsOrderBL = new KDSOrderBL(trx.Utilities.ExecutionContext, trx, kotLinesTemp, eligibleTrxLinesPrinterMapper.POSPrinterDTO, sqlTrx.SQLTrx, isKDSOrder);
                    if (kdsOrderBL.KDSOrderDTO != null && kdsOrderBL.KDSOrderDTO.KDSOrderLineDtoList != null && kdsOrderBL.KDSOrderDTO.KDSOrderLineDtoList.Any())
                    {
                        for (int i = 0; i < kdsOrderBL.KDSOrderDTO.KDSOrderLineDtoList.Count; i++)
                        {
                            kdsOrderBL.DeliverOrderLine(kdsOrderBL.KDSOrderDTO.KDSOrderLineDtoList[i].LineId);
                        }
                    }
                    kdsOrderBL.Save(sqlTrx.SQLTrx);
                    LoadKDSKOTEntriesToLines(trx, kotLines, kdsOrderBL.KDSOrderDTO);
                    sqlTrx.EndTransaction();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while creating to KOT Order Entry", ex);
                foreach (Transaction.TransactionLine kdsLine in kotLines)
                {
                    trx.InsertTrxLogs(trx.Trx_id, kdsLine.DBLineId, trx.Utilities.ParafaitEnv.LoginID, "PRINT",
                        "KOT Entry failed");
                }
            }
            log.LogMethodExit();
        }

        private void LoadKDSKOTEntriesToLines(Transaction trx, List<Transaction.TransactionLine> kotLines, KDSOrderDTO kDSOrderDTO)
        {
            log.LogMethodEntry(kotLines, kDSOrderDTO);
            if (trx != null && trx.Trx_id > 0 && kotLines != null && kotLines.Any() && kDSOrderDTO != null && kDSOrderDTO.KDSOrderLineDtoList != null && kDSOrderDTO.KDSOrderLineDtoList.Any())
            {
                for (int i = 0; i < kotLines.Count; i++)
                {
                    List<KDSOrderLineDTO> KDSKOTLineDTOList = kDSOrderDTO.KDSOrderLineDtoList.Where(kot => kot.LineId == kotLines[i].DBLineId && kot.TrxId == trx.Trx_id).ToList();
                    if (KDSKOTLineDTOList != null && KDSKOTLineDTOList.Any())
                    {
                        for (int j = 0; j < KDSKOTLineDTOList.Count; j++)
                        {

                            int trxLineIndex = trx.TrxLines.FindIndex(tl => tl.DBLineId == kotLines[i].DBLineId);
                            if (trxLineIndex > -1)
                            {
                                if (trx.TrxLines[trxLineIndex].KDSOrderLineDTOList != null && trx.TrxLines[trxLineIndex].KDSOrderLineDTOList.Any())
                                {
                                    int entryIndex = trx.TrxLines[trxLineIndex].KDSOrderLineDTOList.FindIndex(kot => KDSKOTLineDTOList[i].Id == kot.Id
                                                                                             && KDSKOTLineDTOList[j].LineId == trx.TrxLines[trxLineIndex].DBLineId
                                                                                             && KDSKOTLineDTOList[j].TrxId == trx.Trx_id);
                                    if (entryIndex > -1)
                                    {
                                        trx.TrxLines[trxLineIndex].KDSOrderLineDTOList[entryIndex] = KDSKOTLineDTOList[j];
                                        if (trx.TrxLines[trxLineIndex].TransactionLineDTO != null)
                                        {
                                            trx.TrxLines[trxLineIndex].TransactionLineDTO.KDSOrderLineDTOList = trx.TrxLines[trxLineIndex].KDSOrderLineDTOList;
                                        }
                                    }
                                }
                                else
                                {
                                    if (trx.TrxLines[trxLineIndex].KDSOrderLineDTOList == null)
                                    { trx.TrxLines[trxLineIndex].KDSOrderLineDTOList = new List<KDSOrderLineDTO>(); }

                                    trx.TrxLines[trxLineIndex].KDSOrderLineDTOList.Add(KDSKOTLineDTOList[j]);
                                    if (trx.TrxLines[trxLineIndex].TransactionLineDTO != null)
                                    {
                                        trx.TrxLines[trxLineIndex].TransactionLineDTO.KDSOrderLineDTOList = trx.TrxLines[trxLineIndex].KDSOrderLineDTOList;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
