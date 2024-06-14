/********************************************************************************************
 * Project Name - InvoiceSequenceSetupDTO
 * Description  - Data object for Invoice Sequence Setup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.90        11-May-2020   Girish Kundar         Modified : Changes as part of the REST API  
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Transaction 
{
    /// <summary>
    /// This is the InvoiceSequenceSetup data object class. This acts as data holder for the InvoiceSequenceSetup business object
    /// </summary>
    public class InvoiceSequenceSetupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  InvoiceSequenceSetupId field
            /// </summary>
            INVOICE_SEQUENCE_SETUP_ID,
            /// <summary>
            /// Search by InvoiceTypeId field
            /// </summary>
            INVOICE_TYPE_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by ExpiryDate field
            /// </summary>
            EXPIRY_DATE,
            ///<summary>
            ///Search by Prefix field
            ///</summary>
            PREFIX,
            ///<summary>
            ///Search by SeriesStartNumber field
            ///</summary>
            SERIES_START_NUMBER,
            ///<summary>
            ///Search by SeriesEndNumber field
            ///</summary>
            SERIES_END_NUMBER,
            ///<summary>
            ///Search by ResolutionNumber field
            ///</summary>
            RESOLUTION_NUMBER,
            /// <summary>
            /// Search by Approve date field
            /// </summary>
            APPROVE_DATE
        }

        private int invoiceSequenceSetupId;
        private int invoiceTypeId;
        private string prefix;
        private int? currentValue;
        private int? seriesStartNumber;
        private int? seriesEndNumber;
        private DateTime approvedDate;
        private DateTime expiryDate;
        private string resolutionNo;
        private DateTime resolutionDate;
        private bool isActive;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private int invoiceGroupId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvoiceSequenceSetupDTO()
        {
            log.LogMethodEntry();
            invoiceSequenceSetupId = -1;
            invoiceTypeId = -1;
            masterEntityId = -1;
            isActive = true;
            invoiceGroupId = -1;
            seriesStartNumber = null;
            currentValue = null;
            seriesEndNumber = null;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public InvoiceSequenceSetupDTO(int invoiceSequenceSetupId, int invoiceTypeId, string prefix, int? currentValue, int? seriesStartNumber,
                                        int? seriesEndNumber, DateTime approvedDate, DateTime expiryDate, string resolutionNo,
                                        DateTime resolutionDate, int invoiceGroupId, bool isActive)
            :this()
        {
            log.LogMethodEntry(invoiceSequenceSetupId, invoiceTypeId, prefix, currentValue, seriesStartNumber,
                                         seriesEndNumber, approvedDate, expiryDate, resolutionNo,
                                         resolutionDate, invoiceGroupId, isActive);
            this.invoiceSequenceSetupId = invoiceSequenceSetupId;
            this.invoiceTypeId = invoiceTypeId;
            this.prefix = prefix;
            this.currentValue = currentValue;
            this.seriesStartNumber = seriesStartNumber;
            this.seriesEndNumber = seriesEndNumber;
            this.approvedDate = approvedDate;
            this.expiryDate = expiryDate;
            this.resolutionNo = resolutionNo;
            this.resolutionDate = resolutionDate;
            this.invoiceGroupId = invoiceGroupId;
            this.isActive = isActive;
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InvoiceSequenceSetupDTO(int invoiceSequenceSetupId, int invoiceTypeId, string prefix, int? currentValue, int? seriesStartNumber,
                                        int? seriesEndNumber, DateTime approvedDate, DateTime expiryDate, string resolutionNo,
                                        DateTime resolutionDate, int invoiceGroupId, bool isActive, string lastUpdatedBy,
                                        DateTime lastUpdatedDate, int siteId, int masterEntityId, bool synchStatus, string guid,
                                        string createdBy, DateTime creationDate)
            : this(invoiceSequenceSetupId, invoiceTypeId, prefix, currentValue, seriesStartNumber,
                                         seriesEndNumber, approvedDate, expiryDate, resolutionNo,
                                         resolutionDate, invoiceGroupId, isActive)
        {
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the InvoiceSequenceSetupId field
        /// </summary>
        [DisplayName("SI#")]
        [ReadOnly(true)]
        public int InvoiceSequenceSetupId
        {
            get
            {
                return invoiceSequenceSetupId;
            }

            set
            {
                this.IsChanged = true;
                invoiceSequenceSetupId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the InvoiceTypeId field
        /// </summary>
        [DisplayName("Invoice Type")]
        public int InvoiceTypeId
        {
            get
            {
                return invoiceTypeId;
            }

            set
            {
                this.IsChanged = true;
                invoiceTypeId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Prefix field
        /// </summary>
        [DisplayName("Prefix")]
        public string Prefix
        {
            get
            {
                return prefix;
            }

            set
            {
                this.IsChanged = true;
                prefix = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Current Value field
        /// </summary>
        [DisplayName("Current Value")]
        public int? CurrentValue
        {
            get
            {
                return currentValue;
            }

            set
            {
                this.IsChanged = true;
                currentValue = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SeriesStartNumber field
        /// </summary>
        [DisplayName("Series Start Number")]
        public int? SeriesStartNumber
        {
            get
            {
                return seriesStartNumber;
            }

            set
            {
                this.IsChanged = true;
                seriesStartNumber = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SeriesEndNumber field
        /// </summary>
        [DisplayName("Series End Number")]
        public int? SeriesEndNumber
        {
            get
            {
                return seriesEndNumber;
            }

            set
            {
                this.IsChanged = true;
                seriesEndNumber = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ApprovedDate field
        /// </summary>
        [DisplayName("Approved Date")]
        public DateTime ApprovedDate
        {
            get
            {
                return approvedDate;
            }
            set
            {
                this.IsChanged = true;
                approvedDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ExpiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime ExpiryDate
        {
            get
            {
                return expiryDate;
            }
            set
            {
                this.IsChanged = true;
                expiryDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ResolutionNumber field
        /// </summary>
        [DisplayName("Resolution Number")]
        public string ResolutionNumber
        {
            get
            {
                return resolutionNo;
            }

            set
            {
                this.IsChanged = true;
                resolutionNo = value;
            }
        }
        /// <summary>
        /// Get/Set method of the ResolutionDate field
        /// </summary>
        [DisplayName("Resolution Date")]
        public DateTime ResolutionDate
        {
            get
            {
                return resolutionDate;
            }
            set
            {
                this.IsChanged = true;
                resolutionDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the InvoiceGroupId field
        /// </summary>
        [DisplayName("Invoice Group Id")]
        public int InvoiceGroupId
        {
            get
            {
                return invoiceGroupId;
            }

            set
            {
                this.IsChanged = true;
                invoiceGroupId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active")]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set { lastUpdatedDate = value; }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated User")]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set { lastUpdatedBy = value; }
        }


        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }

            set
            {
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set { guid = value; }
        }

        /// <summary>
        /// Created By
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || invoiceSequenceSetupId < 0;
                }
            }

            set
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    if (!Boolean.Equals(notifyingObjectIsChanged, value))
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
            log.LogMethodExit();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
