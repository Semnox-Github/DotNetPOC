/******************************************************************************************************
 * Project Name - Device
 * Description  - Geidea Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.150.0     26-July-2022    Dakshakh Raj   Geidea Payment gateway integration
 ********************************************************************************************************/
using System;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class GeideaRequestDTO
    {
        public string paymentId { get; set; }
        public Int64? trnxAmount { get; set; }
        public Int64? naqdAmount { get; set; }
        public Int64? refundAmount { get; set; }
        public int isPrintReceiptEnabled { get; set; }
        public string trxnRrn { get; set; }
        public string originalRefundDate { get; set; }
        public string ecrRefNumber { get; set; }
        public string cardNumber { get; set; }
        public string trxnApprovalNumber { get; set; }
        public string TextString { get; set; }
        public bool isUnattended { get; set; }
        //private PaymentGatewayTransactionType trxType;
        public int trnxType { get; set; }
        public string terminalID { get; set; }
        public string cashierNo { get; set; }
        public string repeatCommandRequestBuffer { get; set; }

        public int port { get; set; }
        public int baudRate { get; set; }
        public int parityBit { get; set; }
        public int dataBit { get; set; }
        public int stopBit { get; set; }
        

        public GeideaRequestDTO(Int64? trnxAmount, Int64? naqdAmount, Int64? refundAmount, int isPrintReceiptEnabled, string trxnRrn, string originalRefundDate, string ecrRefNumber, string cardNumber, string trxnApprovalNumber, bool isUnattended, int trnxType, string terminalID, string cashierNo, string repeatCommandRequestBuffer, int port, int baudRate, int parityBit, int dataBit, int stopBit, string paymentId)
        {
            this.trnxAmount = trnxAmount;
            this.naqdAmount = naqdAmount;
            this.refundAmount = refundAmount;
            this.isPrintReceiptEnabled = isPrintReceiptEnabled;
            this.trxnRrn = trxnRrn;
            this.originalRefundDate = originalRefundDate;
            this.ecrRefNumber = ecrRefNumber;
            this.cardNumber = cardNumber;
            this.trxnApprovalNumber = trxnApprovalNumber;
            this.TextString = TextString;
            this.isUnattended = isUnattended;
            this.trnxType = trnxType;
            this.terminalID = terminalID;
            this.cashierNo = cashierNo;
            this.repeatCommandRequestBuffer = repeatCommandRequestBuffer;
            this.port = port;
            this.baudRate = baudRate;
            this.parityBit = parityBit;
            this.dataBit = dataBit;
            this.stopBit = stopBit;
            this.paymentId = paymentId;
        }
    }
}
