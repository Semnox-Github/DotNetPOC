/********************************************************************************************
 * Project Name - CCTransactionPGW DTO
 * Description  - Data object of CCTransactionPGW
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        19-Jun-2017   Lakshminarayana          Created 
 *2.70.2      10-Jul-2019   Girish kundar            Modified : Added constructor for required fields .
 *                                                              Added Missed Who columns.      
 *2.110.0     08-Dec-2020   Guru S A                 Subscription changes    
 *2.150.1     31-Jan-2023   Nitin Pai                Modified - Added a new method to check the response from payment gateway and tell if the payment is successful or not.
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// This is the CCTransactionsPGW data object class. This acts as data holder for the CCTransactionsPGW business object
    /// </summary>
    public class CCTransactionsPGWDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ResponseID field
            /// </summary>
            RESPONSE_ID,
            /// <summary>
            /// Search by INVOICE_NUMBER field
            /// </summary>
            INVOICE_NUMBER,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by TranCode field
            /// </summary>
            TRAN_CODE,
            /// <summary>
            /// Search by SplitId field
            /// </summary>
            SPLIT_ID,
            /// <summary>
            /// Search by TransactionId field
            /// </summary>
            TRANSACTION_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Authcode field
            /// </summary>
            AUTH_CODE,
            /// <summary>
            /// Search by RefNo field
            /// </summary>
            REF_NO,
            /// <summary>
            /// Search by RESPONSE_ORIGIN field
            /// </summary>
            RESPONSE_ORIGIN
        }

        private int responseID;
        private string invoiceNo;
        private string tokenID;
        private string recordNo;
        private string dSIXReturnCode;
        private int statusID;
        private string textResponse;
        private string acctNo;
        private string cardType;
        private string tranCode;
        private string refNo;
        private string purchase;
        private string authorize;
        private DateTime transactionDatetime;
        private string authCode;
        private string processData;
        private string responseOrigin;
        private string userTraceData;
        private string captureStatus;
        private string acqRefData;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private string tipAmount;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string customerCopy;
        private string merchantCopy;
        private string customerCardProfileId;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CCTransactionsPGWDTO()
        {
            log.LogMethodEntry();
            responseID = -1;
            statusID = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public CCTransactionsPGWDTO(int responseID, string invoiceNo, string tokenID, string recordNo, string dSIXReturnCode, int statusID,
                                    string textResponse, string acctNo, string cardType, string tranCode, string refNo, string purchase,
                                    string authorize, DateTime transactionDatetime, string authCode, string processData, string responseOrigin,
                                    string userTraceData, string captureStatus, string acqRefData,string tipAmount, string customerCopy, string merchantCopy, string customerCardProfileId)
            :this()
        {
            log.LogMethodEntry(responseID, invoiceNo, tokenID, recordNo, dSIXReturnCode, statusID,
                                textResponse, "acctNo", cardType, tranCode, refNo, purchase,
                                authorize, transactionDatetime, authCode, processData, responseOrigin, userTraceData, captureStatus, "acqRefData",
                                tipAmount, customerCopy, merchantCopy, customerCardProfileId);

            this.responseID = responseID;
            this.invoiceNo = invoiceNo;
            this.tokenID = tokenID;
            this.recordNo = recordNo;
            this.dSIXReturnCode = dSIXReturnCode;
            this.statusID = statusID;
            this.textResponse = textResponse;
            this.acctNo = acctNo;
            this.cardType = cardType;
            this.tranCode = tranCode;
            this.refNo = refNo;
            this.purchase = purchase;
            this.authorize = authorize;
            this.transactionDatetime = transactionDatetime;
            this.authCode = authCode;
            this.processData = processData;
            this.responseOrigin = responseOrigin;
            this.userTraceData = userTraceData;
            this.captureStatus = captureStatus;
            this.acqRefData = acqRefData;
            this.tipAmount = tipAmount;
            this.customerCopy = customerCopy;
            this.merchantCopy = merchantCopy;
            this.customerCardProfileId = customerCardProfileId;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CCTransactionsPGWDTO(int responseID, string invoiceNo, string tokenID, string recordNo, string dSIXReturnCode, int statusID,
                                    string textResponse, string acctNo, string cardType, string tranCode, string refNo, string purchase,
                                    string authorize, DateTime transactionDatetime, string authCode, string processData, string responseOrigin,
                                    string userTraceData, string captureStatus, string acqRefData, string guid, bool synchStatus, int siteId,
                                    string tipAmount, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string customerCopy, string merchantCopy, string customerCardProfileId)
            :this(responseID, invoiceNo, tokenID, recordNo, dSIXReturnCode, statusID,
                                textResponse, acctNo, cardType, tranCode, refNo, purchase,
                                authorize, transactionDatetime, authCode, processData, responseOrigin, userTraceData, captureStatus, acqRefData,
                                tipAmount, customerCopy, merchantCopy, customerCardProfileId)
        {
            log.LogMethodEntry(responseID, invoiceNo, tokenID, recordNo, dSIXReturnCode, statusID,
                                textResponse, "acctNo", cardType, tranCode, refNo, purchase,
                                authorize, transactionDatetime, authCode, processData, responseOrigin, userTraceData, captureStatus, "acqRefData",
                                guid, synchStatus, siteId, tipAmount, masterEntityId,createdBy, creationDate, lastUpdatedBy, lastUpdateDate, customerCopy, merchantCopy, customerCardProfileId);
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Get/Set method of the ResponseID field
        /// </summary>
        public int ResponseID
        {
            get
            {
                return responseID;
            }

            set
            {
                IsChanged = true;
                responseID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the InvoiceNo field
        /// </summary>
        public string InvoiceNo
        {
            get
            {
                return invoiceNo;
            }

            set
            {
                IsChanged = true;
                invoiceNo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TokenID field
        /// </summary>
        public string TokenID
        {
            get
            {
                return tokenID;
            }

            set
            {
                IsChanged = true;
                tokenID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RecordNo field
        /// </summary>
        public string RecordNo
        {
            get
            {
                return recordNo;
            }

            set
            {
                IsChanged = true;
                recordNo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DSIXReturnCode field
        /// </summary>
        public string DSIXReturnCode
        {
            get
            {
                return dSIXReturnCode;
            }

            set
            {
                IsChanged = true;
                dSIXReturnCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the StatusID field
        /// </summary>
        public int StatusID
        {
            get
            {
                return statusID;
            }

            set
            {
                IsChanged = true;
                statusID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TextResponse field
        /// </summary>
        public string TextResponse
        {
            get
            {
                return textResponse;
            }

            set
            {
                IsChanged = true;
                textResponse = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TextResponse field
        /// </summary>
        public bool IsPaymentSuccessful
        {
            get
            {
                bool successStatus = false;
                if((textResponse.ToUpper().Contains("SUCCESS") ||
                    textResponse.ToUpper().Contains("ACCEPT") ||
                    textResponse.ToUpper().Contains("APPROVE")
                    ) && (
                    !textResponse.ToUpper().Contains("DECLINE") ||
                    !textResponse.ToUpper().Contains("FAIL") ||
                    !textResponse.ToUpper().Contains("CANCEL")))
                {
                    successStatus = true;
                }
                return successStatus;
            }
        }

        /// <summary>
        /// Get/Set method of the AcctNo field
        /// </summary>
        public string AcctNo
        {
            get
            {
                return acctNo;
            }

            set
            {
                IsChanged = true;
                acctNo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CardType field
        /// </summary>
        public string CardType
        {
            get
            {
                return cardType;
            }

            set
            {
                IsChanged = true;
                cardType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TranCode field
        /// </summary>
        public string TranCode
        {
            get
            {
                return tranCode;
            }

            set
            {
                IsChanged = true;
                tranCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RefNo field
        /// </summary>
        public string RefNo
        {
            get
            {
                return refNo;
            }

            set
            {
                IsChanged = true;
                refNo = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Purchase field
        /// </summary>
        public string Purchase
        {
            get
            {
                return purchase;
            }

            set
            {
                IsChanged = true;
                purchase = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Authorize field
        /// </summary>
        public string Authorize
        {
            get
            {
                return authorize;
            }

            set
            {
                IsChanged = true;
                authorize = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionDatetime field
        /// </summary>
        public DateTime TransactionDatetime
        {
            get
            {
                return transactionDatetime;
            }

            set
            {
                IsChanged = true;
                transactionDatetime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AuthCode field
        /// </summary>
        public string AuthCode
        {
            get
            {
                return authCode;
            }

            set
            {
                IsChanged = true;
                authCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ProcessData field
        /// </summary>
        public string ProcessData
        {
            get
            {
                return processData;
            }

            set
            {
                IsChanged = true;
                processData = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ResponseOrigin field
        /// </summary>
        public string ResponseOrigin
        {
            get
            {
                return responseOrigin;
            }

            set
            {
                IsChanged = true;
                responseOrigin = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UserTraceData field
        /// </summary>
        public string UserTraceData
        {
            get
            {
                return userTraceData;
            }

            set
            {
                IsChanged = true;
                userTraceData = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CaptureStatus field
        /// </summary>
        public string CaptureStatus
        {
            get
            {
                return captureStatus;
            }

            set
            {
                IsChanged = true;
                captureStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AcqRefData field
        /// </summary>
        public string AcqRefData
        {
            get
            {
                return acqRefData;
            }

            set
            {
                IsChanged = true;
                acqRefData = value;
            }
        }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                guid = value;
            }

        }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }

        }

        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                siteId = value;
            }

        }

        /// <summary>
        /// Get method of the TipAmount field
        /// </summary>
        public string TipAmount
        {
            get
            {
                return tipAmount;
            }

            set
            {
                IsChanged = true;
                tipAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                masterEntityId = value;
                IsChanged = true;
            }

        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value;  }
        }
        /// <summary>
        ///  Get/Set method of the CustomerCopy field
        /// </summary>
        public string CustomerCopy
        {
            get { return customerCopy; }
            set { customerCopy = value; }
        }
        /// <summary>
        ///  Get/Set method of the MerchantCopy field
        /// </summary>
        public string MerchantCopy
        {
            get { return merchantCopy; }
            set { merchantCopy = value; }
        }
        /// <summary>
        ///  Get/Set method of the CustomerCardProfileId field
        /// </summary>
        public string CustomerCardProfileId
        {
            get { return customerCardProfileId; }
            set { customerCardProfileId = value; IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || responseID < 0;
                }
            }

            set
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    if(!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Allows to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current CCTransactionsPGW
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------CCTransactionsPGWDTO-----------------------------\n");
            returnValue.Append(" ResponseID : " + ResponseID);
            returnValue.Append(" InvoiceNo : " + InvoiceNo);
            returnValue.Append(" TokenID : " + TokenID);
            returnValue.Append(" RecordNo : " + RecordNo);
            returnValue.Append(" StatusID : " + StatusID);
            returnValue.Append(" TextResponse : " + TextResponse);
            returnValue.Append(" AcctNo : " + "AcctNo");
            returnValue.Append(" CardType : " + CardType);
            returnValue.Append(" TranCode : " + TranCode);
            returnValue.Append(" RefNo : " + RefNo);
            returnValue.Append(" Purchase : " + Purchase);
            returnValue.Append(" Authorize : " + Authorize);
            returnValue.Append(" TransactionDatetime : " + TransactionDatetime);
            returnValue.Append(" AuthCode : " + AuthCode);
            returnValue.Append(" ProcessData : " + ProcessData);
            returnValue.Append(" ResponseOrigin : " + ResponseOrigin);
            returnValue.Append(" UserTraceData : " + UserTraceData);
            returnValue.Append(" CaptureStatus : " + CaptureStatus);
            returnValue.Append(" AcqRefData : " + "AcqRefData");
            returnValue.Append(" TipAmount : " + TipAmount);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();

        }
    }
}
