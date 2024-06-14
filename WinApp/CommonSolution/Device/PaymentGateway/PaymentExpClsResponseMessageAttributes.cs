using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class PaymentExpClsResponseMessageAttributes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string ResponseCode { get; set; }
        public string TrxStatus { get; set; }
        public string CardSuffix { get; set; }
        public string CardID { get; set; }
        public string TxnState { get; set; }
        public string Cardnumber2 { get; set; }
        public string DPSBillingId { get; set; }
        public string AuthCode { get; set; }
        public string CardHolderName { get; set; }
        public string DpsTxnRef { get; set; }
        public string InvoiceNo { get; set; }
        public string AmountRequested { get; set; }
        public string AmountAuthorized { get; set; }
        public string ErrorMessage { get; set; }
        public string ReceiptText { get; set; }
    }

    public class ClsStatusMessageAttributes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string ResponseCode { get; set; }
        public string MessageCount { get; set; }
        public bool CardPresent { get; set; }
        public string Status { get; set; }
        public string TxnState { get; set; }
        public string FirmwarePending { get; set; }
    }

    public class ClsBasicMessageAttributes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string Object { get; set; }
        public string Action { get; set; }
        public string CmdSeq { get; set; }
        public string ResponseCode { get; set; }
    }

    public class PaymentExpClsRequestMessageAttributes
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string Object { get; set; }
        public string Action { get; set; }
        public string CmdSeq { get; set; }
        public string Amount { get; set; }
        public string InvoiceNo { get; set; }
    } 
}
