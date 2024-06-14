/********************************************************************************************
 * Project Name - Address DTO
 * Description  - Data object of Address
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        02-Feb-2017   Lakshminarayana          Created 
 *2.70.2       19-Jul-2019    Girish Kundar            Modified : Added Constructor with required Parameter
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the Address data object class. This acts as data holder for the Address business object
    /// </summary>
   
    public class AddressDTO
    {
        private static readonly  Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            /// Search by  AddressTypeId field
            /// </summary>
            ADDRESS_TYPE_ID,
            /// <summary>
            /// Search by  AddressType field
            /// </summary>
            ADDRESS_TYPE,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int id;
        private int profileId;
        private int addressTypeId;
        private AddressType addressType;
        private string line1;
        private string line2;
        private string line3;
        private string city;
        private int stateId;
        private string postalCode;
        private int countryId;
        private string stateCode;
        private string stateName;
        private string countryName;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private bool isDefault;
        List<ContactDTO> contactDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AddressDTO()
        {
            log.LogMethodEntry();
            id = -1;
            addressTypeId = -1;
            profileId = -1;
            masterEntityId = -1;
            countryId = -1;
            stateId = -1;
            isActive = true;
            siteId = -1;
            addressType = AddressType.NONE;
            isDefault = false;
            contactDTOList = new List<ContactDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public AddressDTO(int id, int profileId, int addressTypeId, AddressType addressType, string line1, string line2,
                          string line3, string city, int stateId, string postalCode, int countryId,
                          string stateCode, string stateName, string country, bool isDefault, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, profileId, addressTypeId, addressType, line1, line2, line3,
                               city, stateId, postalCode, countryId, stateCode, stateName, country, isDefault, isActive);
            this.id = id;
            this.profileId = profileId;
            this.addressTypeId = addressTypeId;
            this.addressType = addressType;
            this.line1 = line1;
            this.line2 = line2;
            this.line3 = line3;
            this.city = city;
            this.stateId = stateId;
            this.postalCode = postalCode;
            this.countryId = countryId;
            this.stateCode = stateCode;
            this.stateName = stateName;
            this.countryName = country;
            this.isDefault = isDefault;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AddressDTO(int id, int profileId, int addressTypeId, AddressType addressType, string line1, string line2,
                          string line3, string city, int stateId, string postalCode, int countryId,
                          string stateCode, string stateName, string country, bool isDefault, bool isActive,
                          string createdBy, DateTime creationDate, string lastUpdatedBy,
                          DateTime lastUpdateDate, int siteId, int masterEntityId, bool synchStatus,  string guid)
            :this(id, profileId, addressTypeId, addressType, line1, line2, line3,
                  city, stateId, postalCode, countryId, stateCode, stateName, country, isDefault, isActive)
        {
            log.LogMethodEntry(id, profileId, addressTypeId, addressType, line1, line2, line3,
                               city, stateId, postalCode, countryId, isActive, createdBy, creationDate,
                               lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);

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
        /// Get method of the AddressTypeId field
        /// </summary>
        [DisplayName("Address Type")]
        public int AddressTypeId
        {
            get
            {
                return addressTypeId;
            }
            set
            {
                this.IsChanged = true;
                addressTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the AddressType field
        /// </summary>
        [DisplayName("Address Type")]
        public AddressType AddressType
        {
            get
            {
                return addressType;
            }

            set
            {
                this.IsChanged = true;
                addressType = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Line1 field
        /// </summary>
        [DisplayName("Line1")]
        public string Line1
        {
            get
            {
                return line1;
            }

            set
            {
                this.IsChanged = true;
                line1 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Line2 field
        /// </summary>
        [DisplayName("Line2")]
        public string Line2
        {
            get
            {
                return line2;
            }

            set
            {
                this.IsChanged = true;
                line2 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Line3 field
        /// </summary>
        [DisplayName("Line3")]
        public string Line3
        {
            get
            {
                return line3;
            }

            set
            {
                this.IsChanged = true;
                line3 = value;
            }
        }

        /// <summary>
        /// Get/Set method of the City field
        /// </summary>
        [DisplayName("City")]
        public string City
        {
            get
            {
                return city;
            }

            set
            {
                this.IsChanged = true;
                city = value;
            }
        }

        /// <summary>
        /// Get/Set method of the StateId field
        /// </summary>
        [DisplayName("State")]
        public int StateId
        {
            get
            {
                return stateId;
            }

            set
            {
                this.IsChanged = true;
                stateId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CountryId field
        /// </summary>
        [DisplayName("Country")]
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
        /// Get/Set method of the PostalCode field
        /// </summary>
        [DisplayName("PostalCode")]
        public string PostalCode
        {
            get
            {
                return postalCode;
            }

            set
            {
                this.IsChanged = true;
                postalCode = value;
            }
        }

        /// <summary>
        /// Get/Set method of the StateCode field
        /// </summary>
        [DisplayName("State Code")]
        public string StateCode
        {
            get
            {
                return stateCode;
            }

            set
            {
            }
        }

        /// <summary>
        /// Get/Set method of the StateName field
        /// </summary>
        [DisplayName("State Name")]
        public string StateName
        {
            get
            {
                return stateName;
            }

            set
            {
                stateName = value; this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the CountryName field
        /// </summary>
        [DisplayName("Country Name")]
        public string CountryName
        {
            get
            {
                return countryName;
            }

            set
            {
                countryName = value; this.IsChanged = true;
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
        /// Get/Set method of the IsDefault field
        /// </summary>
        public bool IsDefault
        {
            get
            {
                return isDefault;
            }

            set
            {
                this.IsChanged = true;
                isDefault = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the IsDefault field
        /// </summary>
        public List<ContactDTO> ContactDTOList
        {
            get
            {
                return contactDTOList;
            }

            set
            {
                contactDTOList = value;
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
        /// Returns whether the PosMachineDTO changed or any of its posProductDisplayList  are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (contactDTOList != null &&
                    contactDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
