/********************************************************************************************
 * Project Name - POSTypeDTO
 * Description  - Data object of the POSType
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.00        04-Mar-2019   Indhu          Modified for Remote Shift Open/Close changes
 *2.70        09-Jul-2019   Deeksha        Modified:Added new Constructor with required fields
 *2.130.0     21-May-2021   Girish Kundar   Modified: Added Attribue1  to 5 columns to the table as part of Xero integration
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.POS
{
    /// <summary>
    /// This is the POSType data object class. This acts as data holder for the POSType business object
    /// </summary>
    public class POSTypeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by POSTypeId field
            /// </summary>
            POS_TYPE_ID,
            /// <summary>
            /// Search by POSTypeName field
            /// </summary>
            POS_TYPE_NAME,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            ///<summary>
            ///Search by POSTypeName List
            ///</summary>
            POS_TYPE_NAME_LIST,
            ///<summary>
            ///Search by MasterEntityId List
            ///</summary>
            MASTER_ENTITY_ID,
            ///<summary>
            ///Search by IS_ACTIVE List
            ///</summary>
            IS_ACTIVE
        }

        private int pOSTypeId;
        private string pOSTypeName;
        private string description;
        private bool isActive;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private string attribute1;
        private string attribute2;
        private string attribute3;
        private string attribute4;
        private string attribute5;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public POSTypeDTO()
        {
            log.LogMethodEntry();
            pOSTypeId = -1;
            masterEntityId = -1;
            siteId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public POSTypeDTO(int pOSTypeId, string pOSTypeName, string description, string attribute1, string attribute2, string attribute3, string attribute4, string attribute5)
            :this()
        {
            log.LogMethodEntry(pOSTypeId, pOSTypeName, description, attribute1, attribute2, attribute3, attribute4, attribute5);
            this.pOSTypeId = pOSTypeId;
            this.pOSTypeName = pOSTypeName;
            this.description = description;
            this.attribute1 = attribute1;
            this.attribute2 = attribute2;
            this.attribute3 = attribute3;
            this.attribute4 = attribute4;
            this.attribute5 = attribute5;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the fields
        /// </summary>
        public POSTypeDTO(int pOSTypeId, string pOSTypeName, string description, string guid, int siteId, bool synchStatus,
                          int masterEntityId,bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate,
                           string attribute1, string attribute2, string attribute3, string attribute4, string attribute5)
            :this(pOSTypeId, pOSTypeName, description, attribute1,  attribute2,  attribute3,  attribute4,  attribute5)
        {
            log.LogMethodEntry(pOSTypeId, pOSTypeName, description, isActive, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate,
                                lastUpdatedBy, lastUpdateDate);
                               

            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate; 
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the pOSTypeId field
        /// </summary>
        [DisplayName("Id")]
        public int POSTypeId
        {
            get
            {
                return pOSTypeId;
            }

            set
            {
                this.IsChanged = true;
                pOSTypeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the pOSTypeName field
        /// </summary>
        [DisplayName("Name")]
        public string POSTypeName
        {
            get
            {
                return pOSTypeName;
            }

            set
            {
                this.IsChanged = true;
                pOSTypeName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the description field
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
        /// Get/Set method of the createdBy field
        /// </summary>
        [DisplayName("CreatedBy")]
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
        /// Get/Set method of the createdDate field
        /// </summary>
        [DisplayName("CreatedDate")]
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
        /// Get/Set method of the lastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
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
        /// Get/Set method of the lastUpdateDate field
        /// </summary>
        [DisplayName("lastUpdateDate")]
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
        /// Get/Set method of the isActive field
        /// </summary>
        [Browsable(false)]
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
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Attribute1
        /// </summary>
        public string Attribute1
        {
            get { return attribute1; }
            set { this.IsChanged = true; attribute1 = value; }
        }

        /// <summary>
        /// Attribute2
        /// </summary>
        public string Attribute2
        {
            get { return attribute2; }
            set { this.IsChanged = true; attribute2 = value; }
        }

        /// <summary>
        /// Attribute3
        /// </summary>
        public string Attribute3
        {
            get { return attribute3; }
            set { this.IsChanged = true; attribute3 = value; }
        }

        /// <summary>
        /// Attribute4
        /// </summary>
        public string Attribute4
        {
            get { return attribute4; }
            set { this.IsChanged = true; attribute4 = value; }
        }

        /// <summary>
        /// Attribute5
        /// </summary>
        public string Attribute5
        {
            get { return attribute5; }
            set { this.IsChanged = true; attribute5 = value; }
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
                    return notifyingObjectIsChanged || pOSTypeId < 0;
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
