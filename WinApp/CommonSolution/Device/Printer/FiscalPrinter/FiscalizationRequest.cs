/********************************************************************************************
 * Project Name - Device                                                                      
 * Description  - 
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.90.0        14-Jul-2020   Gururaja Kanjan     Data structure to hold transaction object.
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
   
    public class FiscalizationRequest
    {
        public int transactionId { get; set; }
        public int shiftId { get; set; }
        public int shiftLogId { get; set; }
        public DateTime receiptMoment { get; set; }
        public TransactionLine[] transactionLines { get; set; }
        public PaymentInfo[] payments { get; set; }
        public string customerEmail { get; set; }
        public bool isReservation { get; set; }
        public bool isReversal { get; set; }
        public bool isClosed { get; set; }
        public string extReference { get; set; }
        public string posId { get; set; }
        public string ftPOSSystemId { get; set; }
        public bool isShiftIn { get; set; }
        public bool isShiftOut { get; set; }
        public bool isPayIn { get; set; }
        public bool isPayOut { get; set; }
        public int originalTransactionId { get; set; }

        public FiscalizationRequest()
        {
            isReservation = false;
            isReversal = false;
            isClosed = false;
        }
    }



    public class TransactionLine
    {
        public long position { get; set; }
        public decimal quantity { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
        public decimal VATRate { get; set; }
        public decimal? VATAmount { get; set; }   
         }

    public class PaymentInfo
    {
        public long position { get; set; }
        public decimal quantity { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
        public DateTime? moment { get; set; }
        public string reference { get; set; }
        public string paymentMode { get; set; }
        public int paymentID { get; set; }
    }


}
