/********************************************************************************************
* Project Name - FreedomPayDTO
* Description  - DTO class for for Freedompay response and request content
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.70.2.0       20-Sep-2019      Archana            Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class FreedomPayDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public FreedomPayRequestDTO freedomPayRequestDTO = null;
        public FreedomPayResponseDTO freedomPayResponseDTO = null;

        public FreedomPayDTO()
        {
            log.LogMethodEntry();
            freedomPayRequestDTO = new FreedomPayRequestDTO();
            freedomPayResponseDTO = new FreedomPayResponseDTO();
            log.LogMethodExit();
        }
    }

    public class FreedomPayRequestDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string requestType;
        private string requestGuid;
        private string storeId;
        private string terminalId;
        private decimal chargeAmount;
        private string cardNumber;
        private string workStationId;
        private string clientEnvironment;
        private string merchantReferenceCode;
        private string invoiceNumber;
        private string allowPartial;
        private string cardType;
        private int expiryDate;
        private int floorLimit;
        private string laneId;
        private string requestId;
        private decimal taxAmount;
        private decimal tipAmount;
        private string tokenType;
        private string useDCC;
        private string billToFirstName;
        private string billToLastName;
        private string billToStreet1;
        private string billToStreet2;
        private string billToCity;
        private string billToState;
        private string billToPostalCode;
        private string cardDataBlockString;
        private string cardIssuer;
        private string cardPassword;
        private string clerkId;
        private string currency;
        private string customerCode;
        private string customerId;
        private string customerPODate;
        private string customerPONumber;
        private string cvNumber;
        private string enableAVS;
        private string eodGroupCode;
        private string eodLevel;
        private string fallBackReason;
        private decimal gstAmount;


        public FreedomPayRequestDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set property for RequestType field
        /// </summary>
        public string RequestType
        {
            get { return requestType; }
            set { requestType = value; }
        }

        /// <summary>
        /// Get/Set property for RequestGuid field
        /// </summary>
        public string RequestGuid
        {
            get { return requestGuid; }
            set { requestGuid = value; }
        }

        /// <summary>
        /// Get/Set property for StoreId field
        /// </summary>
        public string StoreId
        {
            get { return storeId; }
            set { storeId = value; }
        }

        /// <summary>
        /// Get/Set property for TerminalId field
        /// </summary>
        public string TerminalId
        {
            get { return terminalId; }
            set { terminalId = value; }
        }

        /// <summary>
        /// Get/Set property for Charge Amount field
        /// </summary>
        public decimal ChargeAmount
        {
            get { return chargeAmount; }
            set { chargeAmount = value; }
        }

        /// <summary>
        /// Get/Set property for CardNumber field
        /// </summary>
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        /// <summary>
        /// Get/Set property for WorkStationID field
        /// </summary>
        public string WorkStationId
        {
            get { return workStationId; }
            set { workStationId = value; }
        }

        /// <summary>
        /// Get/Set property for ClientEnvironment field
        /// </summary>
        public string ClientEnvironment
        {
            get { return clientEnvironment; }
            set { clientEnvironment = value; }
        }

        /// <summary>
        /// Get/Set property for MerchantReferenceCode field
        /// </summary>
        public string MerchantReferenceCode
        {
            get { return merchantReferenceCode; }
            set { merchantReferenceCode = value; }
        }

        /// <summary>
        /// Get/Set property for InvoiceNumber field
        /// </summary>
        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { invoiceNumber = value; }
        }

        /// <summary>
        /// Get/Set property for AllowPartial field
        /// </summary>
        public string AllowPartial
        {
            get { return allowPartial; }
            set { allowPartial = value; }
        }

        /// <summary>
        /// Get/Set property for CardType field
        /// </summary>
        public string CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        /// <summary>
        /// Get/Set property for ExpiryDate field
        /// </summary>
        public int ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }

        /// <summary>
        /// Get/Set property for FloorLimit field
        /// </summary>
        public int FloorLimit
        {
            get { return floorLimit; }
            set { floorLimit = value; }
        }

        /// <summary>
        /// Get/Set property for LaneId field
        /// </summary>
        public string LaneId
        {
            get { return laneId; }
            set { laneId = value; }
        }

        /// <summary>
        /// Get/Set property for RequestId field
        /// </summary>
        public string RequestId
        {
            get { return requestId; }
            set { requestId = value; }
        }

        /// <summary>
        /// Get/Set property for TaxAmount field
        /// </summary>
        public decimal TaxAmount
        {
            get { return taxAmount; }
            set { taxAmount = value; }
        }

        /// <summary>
        /// Get/Set property for TipAmount field
        /// </summary>
        public decimal TipAmount
        {
            get { return tipAmount; }
            set { tipAmount = value; }
        }

        /// <summary>
        /// Get/Set property for TokenType field
        /// </summary>
        public string TokenType
        {
            get { return tokenType; }
            set { tokenType = value; }
        }

        /// <summary>
        /// Get/Set property for UseDCC field
        /// </summary>
        public string UseDCC
        {
            get { return useDCC; }
            set { useDCC = value; }
        }

        /// <summary>
        /// Get/Set property for BillTo_City field
        /// </summary>
        public string BillTo_City
        {
            get { return billToCity; }
            set { billToCity = value; }
        }

        /// <summary>
        /// Get/Set property for LaneId field
        /// </summary>
        public string BillTo_FirstName
        {
            get { return billToFirstName; }
            set { billToFirstName = value; }
        }

        /// <summary>
        /// Get/Set property for BillTo_LastName field
        /// </summary>
        public string BillTo_LastName
        {
            get { return billToLastName; }
            set { billToLastName = value; }
        }

        /// <summary>
        /// Get/Set property for BillTo_Street1 field
        /// </summary>
        public string BillTo_Street1
        {
            get { return billToStreet1; }
            set { billToStreet1 = value; }
        }

        /// <summary>
        /// Get/Set property for BillTo_Street2 field
        /// </summary>
        public string BillTo_Street2
        {
            get { return billToStreet2; }
            set { billToStreet2 = value; }
        }

        /// <summary>
        /// Get/Set property for BillTo_State field
        /// </summary>
        public string BillTo_State
        {
            get { return billToState; }
            set { billToState = value; }
        }

        /// <summary>
        /// Get/Set property for BillTo_PostalCode field
        /// </summary>
        public string BillTo_PostalCode
        {
            get { return billToPostalCode; }
            set { billToPostalCode = value; }
        }

        /// <summary>
        /// Get/Set property for CardDataBlockString field
        /// </summary>
        public string CardDataBlockString
        {
            get { return cardDataBlockString; }
            set { cardDataBlockString = value; }
        }

        /// <summary>
        /// Get/Set property for CardIssuer field
        /// </summary>
        public string CardIssuer
        {
            get { return cardIssuer; }
            set { cardIssuer = value; }
        }

        /// <summary>
        /// Get/Set property for CardPassword field
        /// </summary>
        public string CardPassword
        {
            get { return cardPassword; }
            set { cardPassword = value; }
        }

        /// <summary>
        /// Get/Set property for ClerkId field
        /// </summary>
        public string ClerkId
        {
            get { return clerkId; }
            set { clerkId = value; }
        }

        /// <summary>
        /// Get/Set property for Currency field
        /// </summary>
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        /// <summary>
        /// Get/Set property for CustomerCode field
        /// </summary>
        public string CustomerCode
        {
            get { return customerCode; }
            set { customerCode = value; }
        }

        /// <summary>
        /// Get/Set property for CustomerId field
        /// </summary>
        public string CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }

        /// <summary>
        /// Get/Set property for CustomerPODate field
        /// </summary>
        public string CustomerPODate
        {
            get { return customerPODate; }
            set { customerPODate = value; }
        }

        /// <summary>
        /// Get/Set property for CustomerPONumber field
        /// </summary>
        public string CustomerPONumber
        {
            get { return customerPONumber; }
            set { customerPONumber = value; }
        }

        /// <summary>
        /// Get/Set property for CvNumber field
        /// </summary>
        public string CvNumber
        {
            get { return cvNumber; }
            set { cvNumber = value; }
        }

        /// <summary>
        /// Get/Set property for EnableAVS field
        /// </summary>
        public string EnableAVS
        {
            get { return enableAVS; }
            set { enableAVS = value; }
        }

        /// <summary>
        /// Get/Set property for Currency field
        /// </summary>
        public string EodGroupCode
        {
            get { return eodGroupCode; }
            set { eodGroupCode = value; }
        }

        /// <summary>
        /// Get/Set property for EodLevel field
        /// </summary>
        public string EodLevel
        {
            get { return eodLevel; }
            set { eodLevel = value; }
        }
        /// <summary>
        /// Get/Set property for FallBackReason field
        /// </summary>
        public string FallBackReason
        {
            get { return fallBackReason; }
            set { fallBackReason = value; }
        }
        /// <summary>
        /// Get/Set property for GstAmount field
        /// </summary>
        public decimal GstAmount
        {
            get { return gstAmount; }
            set { gstAmount = value; }
        }
    }

    public class  FreedomPayResponseDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string requestGuid;
        private string transactionId;
        private string requestId;
        private decimal approvedAmount;
        private string token;
        private string maskedCardNumber;
        private string cardType;
        private string merchantReferenceCode;
        private string dccAcceped;
        private string errorMode;
        private string decision;
        private string errorCode;
        private string receiptText;
        private string message;

        public FreedomPayResponseDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set property for RequestId field
        /// </summary>
        public string RequestId
        {
            get { return requestId; }
            set { requestId = value;  }
        }
        /// <summary>
        /// Get/Set property for TransactionId field
        /// </summary>
        public string TransactionId
        {
            get { return transactionId; }
            set { transactionId = value; }
        }
        /// <summary>
        /// Get/Set property for RequestGuid field
        /// </summary>
        public string RequestGuid
        {
            get { return requestGuid; }
            set { requestGuid = value; }
        }
        /// <summary>
        /// Get/Set property for ApprovedAmount
        /// </summary>
        public decimal ApprovedAmount
        {
            get { return approvedAmount; }
            set { approvedAmount = value; }
        }

        /// <summary>
        /// Get/Set property for Token
        /// </summary>
        public string Token
        {
            get { return token; }
            set { token = value; }
        }

        /// <summary>
        /// Get/Set property for MaskedCardNumber
        /// </summary>
        public string MaskedCardNumber
        {
            get { return maskedCardNumber; }
            set { maskedCardNumber = value; }
        }

        /// <summary>
        /// Get/Set property for CardType
        /// </summary>
        public string CardType
        {
            get { return cardType; }
            set { cardType = value; }
        }

        /// <summary>
        /// Get/Set property for MerchantReferenceCode
        /// </summary>
        public string MerchantReferenceCode
        {
            get { return merchantReferenceCode; }
            set { merchantReferenceCode = value; }
        }

        /// <summary>
        /// Get/Set property for DccAcceped
        /// </summary>
        public string DccAcceped
        {
            get { return dccAcceped; }
            set { dccAcceped = value; }
        }

        /// <summary>
        /// Get/Set property for ErrorMode
        /// </summary>
        public string ErrorMode
        {
            get { return errorMode; }
            set { errorMode = value; }
        }

        /// <summary>
        /// Get/Set property for Decision
        /// </summary>
        public string Decision
        {
            get { return decision; }
            set { decision = value; }
        }

        /// <summary>
        /// Get/Set property for ErrorCode
        /// </summary>
        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        /// <summary>
        /// Get/Set property for ReceiptText
        /// </summary>
        public string ReceiptText
        {
            get { return receiptText; }
            set { receiptText = value; }
        }
        /// <summary>
        /// Get/Set property for Message
        /// </summary>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
}
