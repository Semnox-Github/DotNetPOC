/********************************************************************************************
 * Project Name - Contact DTO
 * Description  - Data object of Contact
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Lakshminarayana          Created 
 *2.60        08-May-2019   Nitin Pai                Added UUID parameter for Guest App
 *2.70.2        19-Jul-2019   Girish Kundar            Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Contact data object class. This acts as data holder for the Contact business object
    /// </summary>

    public class ContactDTO
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
            /// Search by  ProfileId field
            /// </summary>
            PROFILE_ID,
            /// <summary>
            /// Search by  ProfileIdList field
            /// </summary>
            PROFILE_ID_LIST,
            /// <summary>
            /// Search by  ContactTypeId field
            /// </summary>
            CONTACT_TYPE_ID,
            /// <summary>
            /// Search by  ContactType field
            /// </summary>
            CONTACT_TYPE,
            /// <summary>
            /// Search by  Attribute1 field
            /// </summary>
            ATTRIBUTE1,
            /// <summary>
            /// Search by  Attribute2 field
            /// </summary>
            ATTRIBUTE2,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by UUID field
            /// </summary>
            UUID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by WhatsApp entity id field
            /// </summary>
            WHATSAPP_ENABLED,
            /// <summary>
            /// Search by CUSTOMER_ID id field
            /// </summary>
            CUSTOMER_ID
        }

        //Member Fields
        private int id;
        private int profileId;
        private int addressId;
        private int countryId;
        private int contactTypeId;
        private ContactType contactType;
        private string attribute1;
        private string attribute2;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string uuid;
        private bool notifyingObjectIsChanged;
        private bool whatsAppEnabled;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ContactDTO()
        {
            log.LogMethodEntry();
            id = -1;
            profileId = -1;
            contactTypeId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            contactType = ContactType.NONE;
            whatsAppEnabled = true;
            addressId = -1;
            countryId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ContactDTO(int id, int profileId, int contactTypeId, ContactType contactType, string attribute1, string attribute2,
                          bool isActive, string uuid, bool whatsAppEnabled, int addressId)
            : this()
        {
            log.LogMethodEntry(id, profileId, contactTypeId, contactType, "attribute1", "attribute2",
                               isActive, uuid, whatsAppEnabled, addressId);
            this.id = id;
            this.profileId = profileId;
            this.contactTypeId = contactTypeId;
            this.contactType = contactType;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.isActive = isActive;
            this.uuid = uuid;
            this.addressId = addressId;
            this.whatsAppEnabled = whatsAppEnabled;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ContactDTO(int id, int profileId, int contactTypeId, ContactType contactType, string attribute1, string attribute2,
                          bool isActive, string createdBy, DateTime creationDate,
                          string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId,
                          bool synchStatus, string guid, string uuid, bool whatsAppEnabled, int addressId, int countryId)
            : this(id, profileId, contactTypeId, contactType, attribute1, attribute2,
                               isActive, uuid, whatsAppEnabled, addressId)
        {
            log.LogMethodEntry(id, profileId, contactTypeId, contactType, "attribute1", "attribute2",
                               isActive, createdBy, creationDate, lastUpdatedBy, lastUpdateDate,
                               siteId, masterEntityId, synchStatus, guid, uuid, addressId);

            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.countryId = countryId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
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
        /// Get/Set method of the ProfileId field
        /// </summary>
        [DisplayName("ProfileId")]
        public int ProfileId
        {
            get
            {
                return profileId;
            }

            set
            {
                this.IsChanged = true;
                profileId = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the ProfileId field
        /// </summary>
        [DisplayName("AddressId")]
        public int AddressId
        {
            get
            {
                return addressId;
            }

            set
            {
                this.IsChanged = true;
                addressId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CountryId field
        /// </summary>
        [DisplayName("CountryId")]
        public int CountryId
        {
            get
            {
                return countryId;
            }

            set
            {
                this.IsChanged = true;
                countryId = value;
            }
        }

        /// <summary>
        /// Get method of the ContactTypeId field
        /// </summary>
        public int ContactTypeId
        {
            get
            {
                return contactTypeId;
            }
        }

        /// <summary>
        /// Get/Set method of the ContactType field
        /// </summary>
        [DisplayName("Contact Type")]
        public ContactType ContactType
        {
            get
            {
                return contactType;
            }

            set
            {
                this.IsChanged = true;
                contactType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Attribute1 field
        /// </summary>
        [DisplayName("Attribute1")]
        public string Attribute1
        {
            get
            {
                return attribute1;
            }

            set
            {
                this.IsChanged = true;
                attribute1 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Attribute2 field
        /// </summary>
        [DisplayName("Attribute2")]
        public string Attribute2
        {
            get
            {
                return attribute2;
            }

            set
            {
                this.IsChanged = true;
                attribute2 = value;
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
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                this.IsChanged = true;
                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                this.IsChanged = true;
                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
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
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {

                lastUpdateDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
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
                this.IsChanged = true;
                masterEntityId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
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
        public string Guid
        {
            get
            {
                return guid;
            }

            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UUID field
        /// </summary>
        public string UUID
        {
            get
            {
                return uuid;
            }

            set
            {
                this.IsChanged = true;
                uuid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("WhatsAppEnabled?")]
        public bool WhatsAppEnabled
        {
            get
            {
                return whatsAppEnabled;
            }

            set
            {
                this.IsChanged = true;
                whatsAppEnabled = value;
            }
        }
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || id == -1;
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
