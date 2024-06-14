/********************************************************************************************
 * Project Name - CCRequestPGW DTO
 * Description  - Data object of CCRequestPGW
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        19-Jun-2017   Lakshminarayana          Created 
 *2.70.2        10-Jul-2019   Girish kundar            Modified : Added constructor for required fields .
 *                                                              Added Missed Who columns. 
 *2.150.2     08-Mar-2023   Muaaz Musthafa           Modified: Added new search param to filter based TRANSACTION_TYPE
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{

    /// <summary>
    /// This is the CCRequestPGW data object class. This acts as data holder for the CCRequestPGW business object
    /// </summary>
    public class CCRequestPGWDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  RequestID field
            /// </summary>
            REQUEST_ID,
            /// <summary>
            /// Search by InvoiceNo field
            /// </summary>
            INVOICE_NUMBER,
            /// <summary>
            /// Search by SiteId field
            /// </summary> 
            SITE_ID,
            /// <summary>
            /// Search by MerchantId field
            /// </summary>
            MERCHANT_ID,
            /// <summary>
            /// Search by Master Entity Id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by payment process status
            /// </summary>
            PAYMENT_PROCESS_STATUS,
            /// <summary>
            /// Search by TRANSACTION_TYPE
            /// </summary>
            TRANSACTION_TYPE

        }
        private int requestID;
        private string invoiceNo;
        private DateTime requestDatetime;
        private string pOSAmount;
        private string transactionType;
        private string referenceNo;
        private int statusID;
        private string cardLastFourDigits;
        private string eDSettlement;
        private string merchantID;
        private string guid;
        private bool synchStatus;
        private int siteId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private string paymentProcessStatus;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CCRequestPGWDTO()
        {
            log.LogMethodEntry();
            requestID = -1;
            statusID = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public CCRequestPGWDTO(int requestID, string invoiceNo, DateTime requestDatetime, string pOSAmount,
                                                  string transactionType, string referenceNo, int statusID,
                                                  string cardLastFourDigits, string eDSettlement, string merchantID)
            :this()
        {
            log.LogMethodEntry(requestID, invoiceNo, requestDatetime, pOSAmount, transactionType, referenceNo,
                               statusID, cardLastFourDigits, eDSettlement, merchantID);
            this.requestID = requestID;
            this.invoiceNo = invoiceNo;
            this.requestDatetime = requestDatetime;
            this.pOSAmount = pOSAmount;
            this.transactionType = transactionType;
            this.referenceNo = referenceNo;
            this.statusID = statusID;
            this.cardLastFourDigits = cardLastFourDigits;
            this.eDSettlement = eDSettlement;
            this.merchantID = merchantID;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CCRequestPGWDTO(int requestID, string invoiceNo, DateTime requestDatetime, string pOSAmount,
                               string transactionType, string referenceNo, int statusID,
                               string cardLastFourDigits, string eDSettlement, string merchantID,
                               string guid, bool synchStatus, int siteId, int masterEntityId,
                               string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, string paymentProcessStatus)

            : this(requestID, invoiceNo, requestDatetime, pOSAmount, transactionType, referenceNo,
                   statusID, cardLastFourDigits, eDSettlement, merchantID)
        {
               log.LogMethodEntry(requestID, invoiceNo, requestDatetime, pOSAmount, transactionType, referenceNo,
                statusID, cardLastFourDigits, eDSettlement, merchantID, guid, synchStatus, siteId, masterEntityId,
                createdBy, creationDate, lastUpdatedBy, lastUpdateDate, paymentProcessStatus);

            this.guid = guid;
            this.synchStatus = synchStatus;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.lastUpdatedBy = lastUpdatedBy;
            this.creationDate = creationDate;
            this.lastUpdateDate = lastUpdateDate;
            this.paymentProcessStatus = paymentProcessStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the RequestID field
        /// </summary>
        public int RequestID
        {
            get
            {
                return requestID;
            }

            set
            {
                IsChanged = true;
                requestID = value;
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
        /// Get/Set method of the RequestDatetime field
        /// </summary>
        public DateTime RequestDatetime
        {
            get
            {
                return requestDatetime;
            }

            set
            {
                IsChanged = true;
                requestDatetime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the POSAmount field
        /// </summary>
        public string POSAmount
        {
            get
            {
                return pOSAmount;
            }

            set
            {
                IsChanged = true;
                pOSAmount = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TransactionType field
        /// </summary>
        public string TransactionType
        {
            get
            {
                return transactionType;
            }

            set
            {
                IsChanged = true;
                transactionType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ReferenceNo field
        /// </summary>
        public string ReferenceNo
        {
            get
            {
                return referenceNo;
            }

            set
            {
                IsChanged = true;
                referenceNo = value;
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
        /// Get/Set method of the CardLastFourDigits field
        /// </summary>
        public string CardLastFourDigits
        {
            get
            {
                return cardLastFourDigits;
            }

            set
            {
                IsChanged = true;
                cardLastFourDigits = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EDSettlement field
        /// </summary>
        public string EDSettlement
        {
            get
            {
                return eDSettlement;
            }

            set
            {
                IsChanged = true;
                eDSettlement = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MerchantID field
        /// </summary>
        public string MerchantID
        {
            get
            {
                return merchantID;
            }

            set
            {
                IsChanged = true;
                merchantID = value;
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
            set { createdBy = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value;  }
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
            set { lastUpdateDate = value; }
        }

        public string PaymentProcessStatus
        {
            get { return paymentProcessStatus; }
            set
            {
                IsChanged = true;
                paymentProcessStatus = value;
            }
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
                    return notifyingObjectIsChanged || requestID < 0;
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
        /// Returns a string that represents the current CCRequestPGWDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------CCRequestPGWDTO-----------------------------\n");
            returnValue.Append(" RequestID : " + RequestID);
            returnValue.Append(" InvoiceNo : " + InvoiceNo);
            returnValue.Append(" RequestDatetime : " + RequestDatetime);
            returnValue.Append(" POSAmount : " + POSAmount);
            returnValue.Append(" TransactionType : " + TransactionType);
            returnValue.Append(" ReferenceNo : " + ReferenceNo);
            returnValue.Append(" StatusID : " + StatusID);
            returnValue.Append(" CardLastFourDigits : " + CardLastFourDigits);
            returnValue.Append(" EDSettlement : " + EDSettlement);
            returnValue.Append(" MerchantID : " + MerchantID);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();

        }
    }
}
