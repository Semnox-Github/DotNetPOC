/********************************************************************************************
 * Project Name - China UMS Transaction Request
 * Description  - All the transaction request can be performed using the object of this response class.
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
    /// ChinaUMS Transaction Request Class
    /// </summary>
    public class ChinaUMSTransactionRequest
    {
        /// <summary>
        /// Pos id is the field is the mandatory field. It accepts PosId/Name.
        /// Length of 8 ANS, fills in with spaces, if not enough.
        /// </summary>
        public string TerminalID;
        /// <summary>
        /// Cashier No is the field is the mandatory field. It accepts Cashier Id/Login Id.
        /// Length of 8 ANS, fills in with spaces, if not enough.
        /// </summary>
        public string CashierNo;
        /// <summary>
        /// Based on the requesttype this value will be set.
        /// 00－Sales(consume) 
        /// 01－Cancel
        /// 02－Refund 
        /// 03－Inquiry balance
        /// 04－Re-print consume receipt
        /// </summary>
        internal string TransactionType;
        /// <summary>
        /// The Amount should be assigned without decimal places. i.e., 435.90 is set as 43590.       
        /// </summary>
        public long TransactionAmount;
        /// <summary>
        /// Used in refund transactions. 
        /// </summary>
        public DateTime OldTransactionDate;
        /// <summary>
        /// Used in refund transactions. maximum 12 digits length
        /// </summary>
        public string OriginalReferenceNo;
        /// <summary>
        /// Used in cancel and re-print transactions. When it is re-printing , ‘000000’ is stand for printing last transaction receipt. 
        /// </summary>
        public string OriginalSalesDraft;
        /// <summary>
        /// The payment number of Alipay and Wechat pay
        /// </summary>
        public string PaymentNumber;
        /// <summary>
        /// Radom number for verification
        /// </summary>
        internal string LRC;
        /// <summary>
        /// This just like remarks and of maximum 100 character
        /// </summary>
        public string CustomInformation;
        /// <summary>
        ///ccTransactionsPGW Id 
        /// </summary>
        public int ccResponseId=-1;
        /// <summary>
        /// This is used in receipt printing document number field. 
        /// </summary>
        public int TrxId;
        
    }
}
