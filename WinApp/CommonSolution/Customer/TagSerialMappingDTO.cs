/********************************************************************************************
 * Project Name - TagSerialMapping Data Object
 * Description  - Data object of asset TagSerialMapping.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        22-Mar-2019   Jagan Mohana Rao    added status and messgae properties
 ********************************************************************************************** 
 *2.60        22-Mar-2019   Akshay Gulaganji    added author description and log.MethodEntry() and log.MethodExit()
 *2.70.2        19-Jul-2019    Girish Kundar       Modified : Added Constructor with required Parameter
 *                                                         and MasterEntityId field.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// This is the TagSerialMapping data object class. This acts as data holder for the TagSerialMapping business object
    /// </summary>
    public class TagSerialMappingDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by AccountGameId field
            /// </summary>
            TAG_SERIAL_MAPPING_ID,
            /// <summary>
            /// Search by AccountGameId field
            /// </summary>
            SERIAL_NUMBER,
            /// <summary>
            /// Search by AccountId field
            /// </summary>
            TAG_NUMBER,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            ///<summary>
            ///Search by Master Entity Id field
            ///</summary>
            MASTER_ENTITY_ID,
            ///<summary>
            ///Search by ALREADY_ISSUED field
            ///</summary>
            ALREADY_ISSUED,
            /// <summary>
            /// Search by serial number field
            /// </summary>
            SERIAL_NUMBER_FROM,
            /// <summary>
            /// Search by serial number field
            /// </summary>
            SERIAL_NUMBER_TO
        }

        private int tagSerialMappingId;
        private string serialNumber;
        private string tagNumber;
        private DateTime? creationDate;
        private string createdBy;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private string status;
        private string message;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public TagSerialMappingDTO()
        {
            log.LogMethodEntry();
            tagSerialMappingId = -1;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with Required data fields
        /// </summary>
        public TagSerialMappingDTO(int tagSerialMappingId, string serialNumber, string tagNumber)
            :this()
        {
            log.LogMethodEntry(tagSerialMappingId, serialNumber, tagNumber);
            this.tagSerialMappingId = tagSerialMappingId;
            this.serialNumber = serialNumber;
            this.tagNumber = tagNumber;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public TagSerialMappingDTO(int tagSerialMappingId, string serialNumber, string tagNumber, DateTime? creationDate,
                         string createdBy, int siteId, int masterEntityId, bool synchStatus, string guid,string lastUpdatedBy, DateTime lastUpdateDate)
            :this(tagSerialMappingId, serialNumber, tagNumber)
        {
            log.LogMethodEntry(tagSerialMappingId, serialNumber, tagNumber, creationDate, createdBy,
                               siteId, masterEntityId, synchStatus, guid, lastUpdatedBy, lastUpdateDate);
           
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the tagSerialMappingId field
        /// </summary>
        [Browsable(false)]
        public int TagSerialMappingId
        {
            get
            {
                return tagSerialMappingId;
            }

            set
            {
                this.IsChanged = true;
                tagSerialMappingId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the serialNumber field
        /// </summary>

        [DisplayName("Serial Number")]
        public string SerialNumber
        {
            get
            {
                return serialNumber;
            }

            set
            {
                this.IsChanged = true;
                serialNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the tagNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string TagNumber
        {
            get
            {
                return tagNumber;
            }

            set
            {
                this.IsChanged = true;
                tagNumber = value;
            }
        }

        /// <summary>
        /// Get/Set method of the creationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime? CreationDate
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
        /// Get/Set method of the createdBy field
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
                this.IsChanged = true;
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
                this.IsChanged = true;
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
        /// Get/Set method of the status field
        /// </summary>
        [DisplayName("Status")]
        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                this.IsChanged = true;
                status = value;
            }
        }


        /// <summary>
        /// Get/Set method of the Message field
        /// </summary>
        [DisplayName("Message")]
        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                this.IsChanged = true;
                message = value;
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || tagSerialMappingId < 0;
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
