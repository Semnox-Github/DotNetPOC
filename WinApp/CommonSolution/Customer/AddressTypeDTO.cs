/********************************************************************************************
 * Project Name - AddressType DTO
 * Description  - Data object of AddressType
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
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Address Type enum
    /// </summary>
    public enum AddressType
    {
        /// <summary>
        /// None
        /// </summary>
        NONE,
        /// <summary>
        /// Work
        /// </summary>
        WORK,
        /// <summary>
        /// Home
        /// </summary>
        HOME,
       
    }
    /// <summary>
    /// This is the AddressType data object class. This acts as data holder for the AddressType business object
    /// </summary>
    public class AddressTypeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Id field
            /// </summary>
            ID,
            /// <summary>
            /// Search by  Name field
            /// </summary>
            NAME,
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
            MASTER_ENTITY_ID,
        }

        private int id;
        private string name;
        private string description;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public AddressTypeDTO()
        {
            log.LogMethodEntry();
            id = -1;
            masterEntityId = -1;
            isActive = true;
            name = "NONE";
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public AddressTypeDTO(int id, string name, string description, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, name, description, isActive);
            this.id = id;
            this.name = name;
            this.description = description;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AddressTypeDTO(int id, string name, string description, bool isActive, string createdBy,
                            DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId,
                            int masterEntityId, bool synchStatus, string guid)
            :this(id, name, description, isActive)
        {
            log.LogMethodEntry(id, name, description, isActive, createdBy, creationDate, lastUpdatedBy,
                               lastUpdateDate,siteId, masterEntityId, synchStatus, guid);
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
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                this.IsChanged = true;
                name = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Address Type field
        /// </summary>
        [DisplayName("Address Type")]
        public AddressType AddressType
        {
            get
            {
                AddressType addressType = AddressType.NONE;
                try
                {
                    addressType = (AddressType)Enum.Parse(typeof(AddressType), name, true);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while parsing the address type", ex);
                }
                return addressType;
            }
        }

        /// <summary>
        /// Get/Set method of the Description Text field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                this.IsChanged = true;
                description = value;
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
            set
            {
                createdBy = value;
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
            set
            {
                creationDate = value;
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
            set
            {
                lastUpdatedBy = value;
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
            set
            {
                lastUpdateDate = value;
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
            set
            {
                guid = value;
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
