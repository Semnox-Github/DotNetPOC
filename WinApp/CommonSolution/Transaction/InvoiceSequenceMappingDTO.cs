/********************************************************************************************
 * Project Name - Invoice Sequence MappingDTO
 * Description  - Data object for Invoice Sequence Mapping
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
    /// This is the InvoiceSequenceMapping data object class. This acts as data holder for the InvoiceSequenceMapping business object
    /// </summary>
    public class InvoiceSequenceMappingDTO
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Id field
            /// </summary>
            ID,
            ///<summary>
            ///Search by InvoiceSetupId field
            ///</summary>
            INVOICE_SETUP_ID,
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
            ///<summary>
            ///Search by EFFECTIVE_DATE
            ///</summary>
            EFFECTIVE_DATE,
            ///<summary>
            ///Search by INVOICE_TYPE_ID
            ///</summary>
            INVOICE_TYPE_ID,
            ///<summary>
            ///Search by  EFFECTIVE_DATE_LESSER_THAN 
            ///</summary>
            EFFECTIVE_DATE_LESSER_THAN,
            ///<summary>
            ///Search by InvoiceType field
            ///</summary>
            INVOICE_TYPE,
            ///<summary>
            ///Search by SequenceId field
            ///</summary>
            SEQUENCE_ID
        }

        private int id;
        private int sequenceId;
        private int invoiceSequenceSetupId;
        private DateTime effectiveDate;
        private bool isActive;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate; 
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public InvoiceSequenceMappingDTO()
        {
            log.LogMethodEntry();
            id = -1;
            invoiceSequenceSetupId = -1;
            sequenceId = -1;
            masterEntityId = -1;
            isActive = false;
            siteId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with  required data fields
        /// </summary>
        public InvoiceSequenceMappingDTO(int id, int sequenceId, int invoiceSequenceSetupId, DateTime effectiveDate, bool isActive)
            : this()
        {
            log.LogMethodEntry(id, sequenceId, invoiceSequenceSetupId, effectiveDate, isActive);
            this.id = id;
            this.sequenceId = sequenceId;
            this.invoiceSequenceSetupId = invoiceSequenceSetupId;
            this.effectiveDate = effectiveDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InvoiceSequenceMappingDTO(int id, int sequenceId, int invoiceSequenceSetupId, DateTime effectiveDate, bool isActive,
                                          string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, int masterEntityId, bool synchStatus, string guid,
                                            string createdBy, DateTime creationDate)
            : this(id, sequenceId, invoiceSequenceSetupId, effectiveDate, isActive)
        {
            log.LogMethodEntry(id, sequenceId, invoiceSequenceSetupId, effectiveDate, isActive, lastUpdatedBy, lastUpdatedDate, siteId,
                                masterEntityId, synchStatus, guid, createdBy, creationDate);

            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = masterEntityId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                this.IsChanged = true;
                id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SequenceId field
        /// </summary>
        [DisplayName("Sequence Id")]
        public int SequenceId
        {
            get
            {
                return sequenceId;
            }

            set
            {
                this.IsChanged = true;
                sequenceId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the InvoiceSequenceSetupId field
        /// </summary>
        [DisplayName("Invoice Sequence")]

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
        /// Get/Set method of the EffectiveDate field
        /// </summary>
        [DisplayName("Effective Date")]
        public DateTime EffectiveDate
        {
            get
            {
                return effectiveDate;
            }
            set
            {
                this.IsChanged = true;
                effectiveDate = value;
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
            set
            {
                lastUpdatedDate = value;
            }
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
            set
            {
                lastUpdatedBy = value;
            }
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
            set
            {
                siteId = value;
            }
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
            set
            {
                synchStatus = value;
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
            set
            {
                guid = value;
            }
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
                    return notifyingObjectIsChanged || id < 0;
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
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
