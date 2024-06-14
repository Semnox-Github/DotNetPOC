/********************************************************************************************
 * Project Name - China UMS Transaction Response
 * Description  - This is the response class of china UMS.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Apr-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// ChinaUMS Transaction Response Class
    /// </summary>
    public class ChinaUMSTransactionResponse
    {
        /// <summary>
        /// The code returned from bankall() method call of ChinaUMS
        /// </summary>
        public long ReturnCode;
        /// <summary>
        /// 00 stand for success , other for fail.
        /// </summary>
        public string ResponseCode;
        /// <summary>
        /// Card issuer’s NO.
        /// </summary>
        public string BankId;
        /// <summary>
        /// Card no of the transaction
        /// </summary>
        public string CardNo;
        /// <summary>
        /// Draft no of the transaction
        /// </summary>
        public string DraftNo;
        /// <summary>
        /// Amount,is of 12 digit number, no decimal point. correct to penny. Fill in with zero to the left. 
        /// Example: 435.90 yuan ： 000000043590
        /// </summary>
        public long TransactionAmount;
        /// <summary>
        /// Mistakes explanation is remark field.
        /// </summary>
        public string MistakesExplanation;
        /// <summary>
        /// Merchant Id
        /// </summary>
        public string MerchantId;
        /// <summary>
        /// Pos terminal Id
        /// </summary>
        public string TerminalID;
        /// <summary>
        /// Batch No
        /// 6 digit numeric
        /// </summary>
        public string BatchNo;
        /// <summary>
        /// Transaction Date
        /// 4 digit numeric
        /// </summary>
        public string TransactionDate;
        /// <summary>
        /// Transaction Time
        /// 6 digit numeric
        /// </summary>
        public string TransactionTime;
        /// <summary>
        /// Transaction Reference No
        /// 12 digit numeric
        /// </summary>
        public string ReferenceNo;
        /// <summary>
        /// Authorization No
        /// 6 digit numeric
        /// </summary>
        public string AuthorizationNo;
        /// <summary>
        /// Settlement Date.
        /// 4 digit numeric
        /// </summary>
        public string SettlementDate;
        /// <summary>
        /// Card Type
        /// China Union Pay - CUP
        /// VISA - VIS
        /// Master Card - MCC
        /// Maestro Card - MAE
        /// JCB - JCB
        /// Dinner Club - DCC
        /// American Express - AEX
        /// Prepaid Card - YFK
        /// </summary>
        public string CardType;
        /// <summary>
        /// LRC is used for data verification
        /// </summary>
        internal string LRC;
        /// <summary>
        ///ccTransactionsPGW Id 
        /// </summary>
        public int ccResponseId;
        /// <summary>
        ///Receipt Text
        /// </summary>
        public string ReceiptText;
    }
}
