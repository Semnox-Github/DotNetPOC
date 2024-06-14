/******************************************************************************************************
 * Project Name - Device
 * Description  - PineLabs Payment gateway
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By    Remarks          
 ******************************************************************************************************
 *2.140.5     11-Jan-2023    Prasad & Fiona   PineLabs Payment gateway integration
 ********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class PineLabsPlutusA920ResponseDTO
    {
        public string InternalResponseCode { get; set; } // 0 - tx failed & 1 - tx successful

        public string InternalResponseMessage { get; set; } // 0 - Error/Exception & 1 - Success

        public string InvoiceNo { get; set; } // ccRequestId or Billing reference number

        public string AcctNo { get; set; } // Masked Card Number

        public string AuthCode { get; set; } // AuthCode/ Approval Code

        public string CardType { get; set; } // CardTypeName e.g VISA

        public string CaptureStatus { get; set; } // How the card was entered: Tap, Swipe etc

        public string RefNo { get; set; } // TXN ID

        public string RecordNo { get; set; } // Invoice No, Sequence No etc

        //Response string if a response for transaction was received from bank switch. Otherwise, if any error occurs before response is received, this is an empty string
        public string TextResponse { get; set; } // Response Message / HotResponse

        public string TranCode { get; set; } // TranCode / Transaction Type

        public string Authorize { get; set; } // Amount Authorised by PG / BASE AMT

        // Description of error, if an error occurs. Otherwise, empty string. An empty string in this field DOES NOT imply successful transaction authorization
        public string DSIXReturnCode { get; set; } // Response Message / Remark
        public string MerchantId { get; set; }
        public string BankCode { get; set; }
        public string RRN { get; set; }//Retrieval Reference Number. EFT RRN, if transaction authorized.Otherwise, empty string
        public string TxnAcquirerName { get; set; }//Name of host to which transaction was routed
        public string UpiQrProgramType { get; set; }//UPI/Bharat QR program type for which transaction was processed. HDFC BHARAT QR-1, HDFC UPI -2, AXIS UPI-3
        public string BatchNo { get; set; }

        public int reqSent { get; set; }
        public bool overAllTimeout { get; set; }
    }


    public class GetPaymentResponseDto
    {
        public Cardtxndata CardTxnData { get; set; }
        public object RewardTxnData { get; set; }
        public object PaybackTxnData { get; set; }
        public object Pine360TxnData { get; set; }
        public object UPITxnData { get; set; }
        public string ResponseMessage { get; set; }
        public object BillingRefNumber { get; set; }
        public int Ptrn { get; set; }
        public object HostTransactionNumber { get; set; }
    }

    public class Cardtxndata
    {
        public string BillReferenceNumber { get; set; }
        public string ResponseMessage { get; set; }
        public string TxnStatus { get; set; }
        public string TxnType { get; set; }
        public string TxnDateTime { get; set; }
        public string RRN { get; set; }
        public string AuthCode { get; set; }
        public string AcquirerName { get; set; }
        public string TxnAmtPaise { get; set; }
        public string MaskedPanNum { get; set; }
        public string CardHolderName { get; set; }
        public string BankMid { get; set; }
        public string BankTid { get; set; }
        public string BatchNum { get; set; }
        public string InvoiceNum { get; set; }
        public string CardType { get; set; }
        public string CardEntryMode { get; set; }
        public string AcquirerCode { get; set; }
        public string MerchantName { get; set; }
        public string MerchantAddress { get; set; }
        public string MerchantCity { get; set; }
        public string BatchStatus { get; set; }
        public List<object> AdditionalInfo { get; set; }
    }

}
