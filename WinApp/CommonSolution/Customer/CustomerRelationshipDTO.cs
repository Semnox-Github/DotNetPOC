/********************************************************************************************
 * Project Name - CustomerRelationship DTO
 * Description  - Data object of CustomerRelationship
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Lakshminarayana          Created 
 *2.70.2        19-Jul-2019    Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                         and MasterEntityId field.
 *2.130.7     23-Apr-2022      Nitin Pai           Get linked cards and child's cards for a customer in website                                                                                                                 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the CustomerRelationship data object class. This acts as data holder for the CustomerRelationship business object
    /// </summary>
    public class CustomerRelationshipDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID,
            /// <summary>
            /// Customer Id
            /// </summary>
            CUSTOMER_ID,
            /// <summary>
            /// Related Customer Id
            /// </summary>
            RELATED_CUSTOMER_ID,
            /// <summary>
            /// CustomerRelationship Id
            /// </summary>
            CUSTOMER_RELATIONSHIP_TYPE_ID,
            /// <summary>
            /// Customer Id List
            /// </summary>
            CUSTOMER_ID_LIST,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            ///<summary>
            ///Search by effectiveDate field
            ///</summary>
            EFFECTIVE_DATE,
            ///<summary>
            ///Search by expiryDate field
            ///</summary>
            EXPIRY_DATE,
            ///<summary>
            ///Search by Master Entity Id field
            ///</summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// CustomerRelationship Id LIST
            /// </summary>
            CUSTOMER_RELATIONSHIP_TYPE_ID_LIST,
        }

        private int id;
        private int customerId;
        private int relatedCustomerId;
        private int customerRelationshipTypeId;
        private string customerName;
        private string relatedCustomerName;
        private DateTime? effectiveDate;
        private DateTime? expiryDate;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private CustomerDTO customerDTO;
        private CustomerDTO relatedCustomerDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public CustomerRelationshipDTO()
        {
            log.LogMethodEntry();
            id = -1;
            customerId = -1;
            relatedCustomerId = -1;
            masterEntityId = -1;
            customerRelationshipTypeId = -1;
            isActive = true;
            siteId = -1;
            customerDTO = new CustomerDTO();
            relatedCustomerDTO = new CustomerDTO();
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with REquired data fields
        /// </summary>
        public CustomerRelationshipDTO(int id, int customerId, int relatedCustomerId,
                                       int customerRelationshipTypeId, string customerName,
                                       string relatedCustomerName, DateTime? effectiveDate,
                                       DateTime? expiryDate, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, customerId, relatedCustomerId, customerRelationshipTypeId,
                               customerName, relatedCustomerName, effectiveDate, expiryDate,
                               isActive);
            this.id = id;
            this.customerId = customerId;
            this.relatedCustomerId = relatedCustomerId;
            this.customerRelationshipTypeId = customerRelationshipTypeId;
            this.customerName = customerName;
            this.relatedCustomerName = relatedCustomerName;
            this.effectiveDate = effectiveDate;
            this.expiryDate = expiryDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public CustomerRelationshipDTO(int id, int customerId, int relatedCustomerId,
                                       int customerRelationshipTypeId, string customerName,
                                       string relatedCustomerName, DateTime? effectiveDate,
                                       DateTime? expiryDate, bool isActive, string createdBy,
                                       DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                                       int siteId, int masterEntityId, bool synchStatus, string guid)
            :this(id, customerId, relatedCustomerId, customerRelationshipTypeId,
                               customerName, relatedCustomerName, effectiveDate, expiryDate,
                               isActive)
        {
            log.LogMethodEntry(id, customerId, relatedCustomerId, customerRelationshipTypeId,
                               customerName, relatedCustomerName, effectiveDate, expiryDate,
                               isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
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
        /// Get/Set method of the CustomerId field
        /// </summary>
        [Browsable(false)]
        public int CustomerId
        {
            get
            {
                return customerId;
            }

            set
            {
                this.IsChanged = true;
                customerId = value;
            }
        }

        /// <summary>
        /// Get method of the customerName field
        /// </summary>
        [DisplayName("Customer")]
        public string CustomerName
        {
            get
            {
                return customerName;
            }
        }

        /// <summary>
        /// Get/Set method of the RelatedCustomerId field
        /// </summary>
        [Browsable(false)]
        public int RelatedCustomerId
        {
            get
            {
                return relatedCustomerId;
            }

            set
            {
                this.IsChanged = true;
                relatedCustomerId = value;
            }
        }

        /// <summary>
        /// Get method of the relatedCustomerName field
        /// </summary>
        [DisplayName("Related Customer")]
        public string RelatedCustomerName
        {
            get
            {
                return relatedCustomerName;
            }
        }

        /// <summary>
        /// Get/Set method of the CustomerRelationshipTypeId Text field
        /// </summary>
        [DisplayName("Customer Relationship Type")]
        public int CustomerRelationshipTypeId
        {
            get
            {
                return customerRelationshipTypeId;
            }

            set
            {
                this.IsChanged = true;
                customerRelationshipTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the effectiveDate field
        /// </summary>
        [DisplayName("Effective Date")]
        public DateTime? EffectiveDate
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
        /// Get/Set method of the expiryDate field
        /// </summary>
        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate
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
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
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
        }

        /// <summary>
        /// Get/Set method for CustomerDTO
        /// </summary>
        public CustomerDTO CustomerDTO
        {
            get
            {
                return customerDTO;
            }
            set
            {
                customerDTO = value;
            }
        }

        /// <summary>
        /// Get/Set method for RelatedCustomerDTO
        /// </summary>
        public CustomerDTO RelatedCustomerDTO
        {
            get
            {
                return relatedCustomerDTO;
            }
            set
            {
                relatedCustomerDTO = value;
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
