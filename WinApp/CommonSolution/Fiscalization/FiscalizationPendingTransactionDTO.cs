/********************************************************************************************
 * Project Name - Fiscalization
 * Description  - Data object of FiscalizationPendingTransactionDTO  
 *  
 **************
 **Version Log
 **************
 *Version          Date          Modified By         Remarks          
 *********************************************************************************************
 *2.155.1.0        12-Aug-2023   Guru S A            Created for chile fiscalization
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// FiscalizationPendingTransactionDTO
    /// </summary>
    public class FiscalizationPendingTransactionDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchParameters
        /// </summary>
        public enum SearchParameters
        {
            /// <summary>
            /// Search by TRX_FROM_DATE field
            /// </summary>
            TRX_FROM_DATE,
            /// <summary>
            /// Search by TRX_TO_DATE field
            /// </summary>
            TRX_TO_DATE,
            /// <summary>
            /// Search by TRX_ID field
            /// </summary>
            TRX_ID,
            /// <summary>
            /// Search by IGNORE_WIP_TRX field
            /// </summary>
            IGNORE_WIP_TRX,
        }
        private string fiscalization;
        private int transactionId;
        private string transactionNumber;
        private DateTime transactionDate;
        private string transactionCustomerName;
        private string transactionPOSMachine;
        private string transactionJsonBuildError;
        private string transactionPostError;
        private DateTime transactionCreationDate;
        private string transactionCreatedBy;
        private DateTime transactionLastUpdateDate;
        private string transactionLastUpdatedBy;
        private int latestRequestId;
        private string latestRequestStatus;
        private string latestRequestPhase;
        private string invoiceOptionName;
        private string taxCode;
        private string uniuqeId;
        private int site_id;

        /// <summary>
        /// FiscalizationPendingTransactionDTO
        /// </summary>
        public FiscalizationPendingTransactionDTO()
        {
            log.LogMethodEntry();
            transactionId = -1;
            latestRequestId = -1;
            log.LogMethodExit();
        }

        /// <summary> 
        /// FiscalizationPendingTransactionDTO
        /// </summary> 
        public FiscalizationPendingTransactionDTO(string fiscalization, int transactionId, string transactionNumber, DateTime transactionDate,
                    string transactionCustomerName, string transactionPOSMachine, string transactionJsonBuildError, string transactionPostError,
                    DateTime transactionCreationDate, string transactionCreatedBy, DateTime transactionLastUpdateDate, string transactionLastUpdatedBy,
                    int latestRequestId, string latestRequestStatus, string latestRequestPhase, string invoiceOptionName, string taxCode, string uniuqeId,
                    int siteId)
        {
            log.LogMethodEntry(fiscalization, transactionId, transactionNumber, transactionDate, transactionCustomerName, transactionPOSMachine, transactionJsonBuildError,
                transactionPostError, transactionCreationDate, transactionCreatedBy, transactionLastUpdateDate, transactionLastUpdatedBy, latestRequestId, latestRequestStatus,
                latestRequestPhase, invoiceOptionName, taxCode, uniuqeId, siteId);
            this.fiscalization = fiscalization;
            this.transactionId = transactionId;
            this.transactionNumber = transactionNumber;
            this.transactionDate = transactionDate;
            this.transactionCustomerName = transactionCustomerName;
            this.transactionPOSMachine = transactionPOSMachine;
            this.transactionJsonBuildError = transactionJsonBuildError;
            this.transactionPostError = transactionPostError;
            this.transactionCreationDate = transactionCreationDate;
            this.transactionCreatedBy = transactionCreatedBy;
            this.transactionLastUpdateDate = transactionLastUpdateDate;
            this.transactionLastUpdatedBy = transactionLastUpdatedBy;
            this.latestRequestId = latestRequestId;
            this.latestRequestStatus = latestRequestStatus;
            this.latestRequestPhase = latestRequestPhase;
            this.invoiceOptionName = invoiceOptionName;
            this.taxCode = taxCode;
            this.uniuqeId = uniuqeId;
            this.site_id = siteId;
            log.LogMethodExit();
        }

        /// <summary> 
        /// FiscalizationPendingTransactionDTO
        /// </summary> 
        public FiscalizationPendingTransactionDTO(FiscalizationPendingTransactionDTO fiscDTO)
        {
            log.LogMethodEntry(fiscDTO);
            this.fiscalization = fiscDTO.Fiscalization;
            this.transactionId = fiscDTO.TransactionId;
            this.transactionNumber = fiscDTO.TransactionNumber;
            this.transactionDate = fiscDTO.TransactionDate;
            this.transactionCustomerName = fiscDTO.TransactionCustomerName;
            this.transactionPOSMachine = fiscDTO.TransactionPOSMachine;
            this.transactionJsonBuildError = fiscDTO.TransactionJsonBuildError;
            this.transactionPostError = fiscDTO.TransactionPostError;
            this.transactionCreationDate = fiscDTO.TransactionCreationDate;
            this.transactionCreatedBy = fiscDTO.TransactionCreatedBy;
            this.transactionLastUpdateDate = fiscDTO.TransactionLastUpdateDate;
            this.transactionLastUpdatedBy = fiscDTO.TransactionLastUpdatedBy;
            this.latestRequestId = fiscDTO.LatestRequestId;
            this.latestRequestStatus = fiscDTO.LatestRequestStatus;
            this.latestRequestPhase = fiscDTO.LatestRequestPhase;
            this.invoiceOptionName = fiscDTO.InvoiceOptionName;
            this.taxCode = fiscDTO.TaxCode;
            this.uniuqeId = fiscDTO.UniuqeId;
            this.site_id = fiscDTO.SiteId;
            log.LogMethodExit();
        }
        /// <summary>
        /// Fiscalization
        /// </summary>
        public string Fiscalization
        {
            set { fiscalization = value; }
            get { return fiscalization; }
        }
        /// <summary>
        /// TransactionId
        /// </summary>
        public int TransactionId
        {
            set { transactionId = value; }
            get { return transactionId; }
        }
        /// <summary>
        /// TransactionNumber
        /// </summary>
        public string TransactionNumber
        {
            set { transactionNumber = value; }
            get { return transactionNumber; }
        }
        /// <summary>
        /// TransactionDate
        /// </summary>
        public DateTime TransactionDate
        {
            set { transactionDate = value; }
            get { return transactionDate; }
        }
        /// <summary>
        /// TransactionCustomerName
        /// </summary>
        public string TransactionCustomerName
        {
            set { transactionCustomerName = value; }
            get { return transactionCustomerName; }
        }
        /// <summary>
        /// TransactionPOSMachine
        /// </summary>
        public string TransactionPOSMachine
        {
            set { transactionPOSMachine = value; }
            get { return transactionPOSMachine; }
        }
        /// <summary>
        /// TransactionJsonBuildError
        /// </summary>
        public string TransactionJsonBuildError
        {
            set { transactionJsonBuildError = value; }
            get { return transactionJsonBuildError; }
        }
        /// <summary>
        /// TransactionPostError
        /// </summary>
        public string TransactionPostError
        {
            set { transactionPostError = value; }
            get { return transactionPostError; }
        }
        /// <summary>
        /// transactionCreationDate
        /// </summary>
        public DateTime TransactionCreationDate
        {
            set { transactionCreationDate = value; }
            get { return transactionCreationDate; }
        }
        /// <summary>
        /// TransactionCreatedBy
        /// </summary>
        public string TransactionCreatedBy
        {
            set { transactionCreatedBy = value; }
            get { return transactionCreatedBy; }
        }
        /// <summary>
        /// TransactionLastUpdateDate
        /// </summary>
        public DateTime TransactionLastUpdateDate
        {
            set { transactionLastUpdateDate = value; }
            get { return transactionLastUpdateDate; }
        }
        /// <summary>
        /// TransactionLastUpdatedBy
        /// </summary>
        public string TransactionLastUpdatedBy
        {
            set { transactionLastUpdatedBy = value; }
            get { return transactionLastUpdatedBy; }
        }
        /// <summary>
        /// LatestRequestId
        /// </summary>
        public int LatestRequestId
        {
            set { latestRequestId = value; }
            get { return latestRequestId; }
        }
        /// <summary>
        /// LatestRequestStatus
        /// </summary>
        public string LatestRequestStatus
        {
            set { latestRequestStatus = value; }
            get { return latestRequestStatus; }
        }
        /// <summary>
        /// LatestRequestPhase
        /// </summary>
        public string LatestRequestPhase
        {
            set { latestRequestPhase = value; }
            get { return latestRequestPhase; }
        }
        /// <summary>
        /// invoiceOptionName
        /// </summary>
        public string InvoiceOptionName
        {
            set { invoiceOptionName = value; }
            get { return invoiceOptionName; }
        }
        /// <summary>
        /// taxCode
        /// </summary>
        public string TaxCode
        {
            set { taxCode = value; }
            get { return taxCode; }
        }
        /// <summary>
        /// UniuqeId
        /// </summary>
        public string UniuqeId
        {
            set { uniuqeId = value; }
            get { return uniuqeId; }
        }
        /// <summary>
        /// SiteId
        /// </summary>
        public int SiteId
        {
            set { site_id = value; }
            get { return site_id; }
        }
    }
}
