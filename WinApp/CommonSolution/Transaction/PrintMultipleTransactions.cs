/********************************************************************************************
 * Project Name - PrintMultipleTransactions
 * Description  - Business Logic to prepare transaction for final printing
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *2.70.2      14-Nov-2019      Girish Kundar  Modified: Error handling in the print method
 *2.90.0      30-Jul-2020      Girish Kundar  Modified: BowaPegas shift open/close and Viber changes

 ********************************************************************************************/

using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer.FiscalPrint;
using Semnox.Parafait.POS;

namespace Semnox.Parafait.Transaction
{
    public class PrintMultipleTransactions
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Utilities Utilities;
        public PrintMultipleTransactions(Utilities pUtilities)
        {
            log.LogMethodEntry(pUtilities);

            Utilities = pUtilities;

            log.LogMethodExit(null);
        }

        public bool Print(List<int> TrxIdList, bool rePrint, ref string message)
        {
            log.LogMethodEntry(TrxIdList, rePrint, message);

            if (TrxIdList.Count == 0)
            {
                message = "No Transactions to Print";

                log.LogVariableState("message ", message);
                log.LogMethodExit(false);
                return false;
            }

            try
            {
                POSMachines posMachine = new POSMachines(Utilities.ExecutionContext, Utilities.ParafaitEnv.POSMachineId);
                List<POSPrinterDTO> posPrintersDTOList = posMachine.PopulatePrinterDetails();
                PrintTransaction prt = new PrintTransaction(posPrintersDTOList);
                prt.RePrint = rePrint;
                bool returnValue;
                TransactionUtils TransactionUtils = new TransactionUtils(Utilities);
                foreach (int trxId in TrxIdList)
                {
                    Transaction trx = TransactionUtils.CreateTransactionFromDB(trxId, Utilities);
                    if (Utilities.getParafaitDefaults("USE_FISCAL_PRINTER") != "Y" &&
                        !(Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas)))
                    {
                        returnValue = prt.Print(trx, ref message, false, true);
                        if (returnValue == false)
                        {
                            log.Error("Error Printing : " + message);
                            log.LogMethodExit(returnValue);
                            return false;
                        }
                    }
                    else
                    {
                        string _FISCAL_PRINTER = Utilities.getParafaitDefaults("FISCAL_PRINTER");
                        FiscalPrinter fiscalPrinter = FiscalPrinterFactory.GetInstance().GetFiscalPrinter(_FISCAL_PRINTER);

                        if (!fiscalPrinter.PrintReceipt(trxId, ref message, null, 0, true, rePrint))
                        {
                            if (Utilities.getParafaitDefaults("FISCAL_PRINTER").Equals(FiscalPrinters.BowaPegas.ToString()))
                            {
                                // Non fiscal type for type 'D' taxed products
                                returnValue = prt.Print(trx, ref message, false, true);
                                if (returnValue == false)
                                {
                                    log.Error("Error Printing : " + message);
                                    log.LogMethodExit(returnValue);
                                    return false;
                                }
                            }
                            else
                            {
                                log.Error("Error Printing : " + message);
                                return false;
                            }
                        }
                    }
                }

                log.LogVariableState("message ", message);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Failed to  Print Multiple Transactions! ", ex);
                message = ex.Message;
                log.LogMethodExit(false);
                return false;
            }
        }
    }
}
