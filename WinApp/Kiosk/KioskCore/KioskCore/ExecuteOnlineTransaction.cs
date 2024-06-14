/********************************************************************************************
* Project Name - Semnox.Parafait.KioskCore
* Description  - ExecuteOnlineTransaction 
* 
**************
**Version Log
**************
*Version         Date             Modified By           Remarks          
*********************************************************************************************
*2.150.5.0       28-Sep-2023      Guru S A              Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Languages;
using Semnox.Parafait.POS;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.KioskCore
{
    public class ExecuteOnlineTransaction : KioskTransaction
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private TransactionDTO transactionDTO;
        private bool isvirtualStore;
        public override int GetTransactionId { get { return GetTrxId(); } }
        public bool IsvirtualStore { get { return isvirtualStore; } }
        public override Transaction.Transaction KioskTransactionObject { get { return (isvirtualStore ? null : this.kioskTrx); } }
        public ExecuteOnlineTransaction(Utilities utils, TransactionDTO transactionDTO, bool isvirtualStore)
            : base(utils, isvirtualStore ? -1 : transactionDTO.TransactionId)
        {
            log.LogMethodEntry("utils", transactionDTO, isvirtualStore);
            this.isvirtualStore = isvirtualStore;
            this.transactionDTO = transactionDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Transaction Customer
        /// </summary> 
        public override List<string> GetTransactionCustomerIdentifierList()
        {
            log.LogMethodEntry();
            List<string> idList = new List<string>();
            if (isvirtualStore)
            {
                idList.Add(this.transactionDTO.CustomerIdentifier);
            }
            else
            {
                idList = base.GetTransactionCustomerIdentifierList();
            }
            log.LogMethodExit(idList);
            return idList;
        }
        private int GetTrxId()
        {
            log.LogMethodEntry();
            int trxId = transactionDTO.TransactionId;
            log.LogMethodExit(trxId);
            return trxId;
        }
        /// <summary>
        /// Get Transaction Customer
        /// </summary> 
        public override CustomerDTO GetTransactionCustomer()
        {
            log.LogMethodEntry();
            CustomerDTO customerDTO = null;
            if (isvirtualStore)
            {
                //if (this.transactionDTO.CustomerId > -1)
                //customerDTO = this.transactionDTO.CustomerId
            }
            else
            {
                customerDTO = base.GetTransactionCustomer();
            }
            log.LogMethodExit(customerDTO);
            return customerDTO;
        }
        /// <summary>
        /// SetGuestEmailId
        /// </summary>
        /// <param name="emailId"></param>
        public override void SetGuestEmailId(string emailId)
        {
            log.LogMethodEntry(emailId);
            if (isvirtualStore == false)
            {
                base.SetGuestEmailId(emailId);
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// GetExecuteOnlineReceipt
        /// </summary> 
        /// <returns></returns>
        public override Printer.ReceiptClass GetExecuteOnlineReceipt(POSPrinterDTO posPrinterDTO, int cardCount)
        {
            log.LogMethodEntry(posPrinterDTO, cardCount);
            Printer.ReceiptClass transactionReceipt = null; 
            TransactionBL transactionBL = new TransactionBL(executionContext, this.transactionDTO);
            transactionDTO = transactionBL.PrintExecuteOnlineReceipt(kioskTrx, posPrinterDTO, cardCount);
            transactionReceipt = transactionBL.GetReceiptClassFromReceiptDTO(transactionDTO.ReceiptDTO);
            if (transactionReceipt.TotalLines == 0)
            {
                log.LogMethodExit(null);
                return null;
            }
            log.LogMethodExit(transactionReceipt);
            return transactionReceipt;
        }

        /// <summary>
        /// GetExecuteOnlineErrorReceipt
        /// </summary> 
        /// <returns></returns>
        public override Printer.ReceiptClass GetExecuteOnlineErrorReceipt(POSPrinterDTO posPrinterDTO, int cardCount)
        {
            log.LogMethodEntry(posPrinterDTO, cardCount);
            Printer.ReceiptClass transactionReceipt = null; 
            TransactionBL transactionBL = new TransactionBL(executionContext, this.transactionDTO);
            transactionDTO = transactionBL.PrintExecuteOnlineErrorReceipt(kioskTrx, posPrinterDTO, cardCount);
            transactionReceipt = transactionBL.GetReceiptClassFromReceiptDTO(transactionDTO.ReceiptDTO);
            if (transactionReceipt.TotalLines == 0)
            {
                log.LogMethodExit(null);
                return null;
            }
            log.LogMethodExit(transactionReceipt);
            return transactionReceipt;
        }
    }
}
