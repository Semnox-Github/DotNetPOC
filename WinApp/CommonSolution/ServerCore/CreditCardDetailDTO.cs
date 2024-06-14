/********************************************************************************************
 * Project Name - ServerCore
 * Description  - Data object of CreditCardDetail
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************
 *2.150.04    02-May-2023   Lakshminarayana Rao     Created 
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.ServerCore
{
    public class CreditCardDetailDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public CreditCardDetailDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        public decimal AmountRequested {get; set;}
        public decimal AmountAuthorized {get; set;}
        public string AuthorizationCode {get; set;}
        public string PartialPan {get; set;}
        public string CardType {get; set;}
        public string TransactionDbId {get; set;}
        public string AuthId {get; set;}
        public string ReceiptId {get; set;}
        public string RRN {get; set;}
        public string Channel {get; set;}
        public string AID {get; set;}
        public string TVR {get; set;}
        public string IAD {get; set;}
        public string TSI {get; set;}
        public string ARC {get; set;}
        public string TransactionTime {get; set;}
        public string Currency {get; set;}
        public string ApplicationLabel {get; set;}
        public string CardTokenA {get; set;}
        public string CardTokenB {get; set;}
        public string TID {get; set;}
        public string CVM {get; set;}
        public bool IsTransactionApproved {get; set;}

        

       
    }
}
