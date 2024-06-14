/***********************************************************************************************
 * Project Name - LokerZones DTO
 * Description  - Data object of LockerZones
 *
 **************
 ** Version Log
  **************
  * Version     Date Modified     By           Remarks
 *********************************************************************************************
 *1.00        06-Nov-2017      Archana         Created
 *2.70.2        19-Jul-2019      Dakshakh raj    Modified : Added Parameterized costrustor,
 *                                                        CreatedBy and CreationDate fields
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// This is the Locker Zones data object class. This acts as data holder for the LockerZones Buisness object
    /// </summary>
    public class LockerZonesDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new  Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Id field
            /// </summary>
            ZONE_ID,
           
            /// <summary>
            /// Search by ZoneName field
            /// </summary>
            ZONE_NAME,
           
            /// <summary>
            /// Search by ZoneId field
            /// </summary>
            PARENT_ZONE_ID,
           
            /// <summary>
            /// Search by ZoneCode field
            /// </summary>
            ZONE_CODE,
           
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ACTIVE_FLAG,
           
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            
            /// <summary>
            /// Search by LOCKER MODE field
            /// </summary>
            LOCKER_MODE
        }

        private int zoneId;
        private string zoneName;
        private int parentZoneId;
        private string zoneCode;
        private string lockerMake;
        private bool activeFlag;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string lockerMode;
        private string createdBy;
        private DateTime creationDate;
        IList<LockerPanelDTO> lockerPanelDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public LockerZonesDTO()
        {
            log.LogMethodEntry();
            zoneId = -1;
            parentZoneId = -1;
            activeFlag = true;
            siteId = -1;
            masterEntityId = -1;
            zoneCode = "0";
            lockerMake = "";
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public LockerZonesDTO(int zoneId, string zoneName, int parentZoneId, string zoneCode, bool activeFlag, string lockerMode, string lockerMake)
            :this()
        {
            log.LogMethodEntry(zoneId, zoneName, parentZoneId, zoneCode, activeFlag, lockerMode, lockerMake);
            this.zoneName = zoneName;
            this.parentZoneId = parentZoneId;
            this.zoneCode = zoneCode;
            this.zoneId = zoneId;
            this.activeFlag = activeFlag;
            this.lockerMode = lockerMode;
            this.lockerMake = lockerMake;
            this.lockerPanelDTOList = lockerPanelDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public LockerZonesDTO(int zoneId, string zoneName, int parentZoneId, string zoneCode, bool activeFlag,
                              string lastUpdatedBy, DateTime lastUpdatedDate, int siteId, string guid, bool synchStatus, 
                              int masterEntityId, string lockerMode, string lockerMake, string createdBy, DateTime creationDate)
            :this(zoneId, zoneName, parentZoneId, zoneCode, activeFlag, lockerMode, lockerMake)
        {
            log.LogMethodEntry(lastUpdatedBy, lastUpdatedDate, siteId, guid, synchStatus, masterEntityId, createdBy, creationDate);
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ZoneId field
        /// </summary>
        [DisplayName("Zone Id")]
        [ReadOnly(true)]
        public int ZoneId
        {
            get
            {
                return zoneId;
            }

            set
            {                
                zoneId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ZoneName field
        /// </summary>
        [DisplayName("Zone Name")]
        public string ZoneName
        {
            get
            {
                return zoneName;
            }

            set
            {                
                zoneName = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ParentZoneId field
        /// </summary>
        [DisplayName("Parent Zone")]
        public int ParentZoneId
        {
            get
            {
                return parentZoneId;
            }

            set
            {                
                parentZoneId = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the zoneCode field
        /// </summary>
        [DisplayName("Zone Code")]
        public string ZoneCode
        {
            get
            {
                return zoneCode;
            }

            set
            {                
                zoneCode = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Locker Mode field
        /// </summary>
        [DisplayName("Locker Mode")]
        public string LockerMode
        {
            get
            {
                return lockerMode;
            }

            set
            {
                lockerMode = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("Active Flag")]
        public bool ActiveFlag
        {
            get
            {
                return activeFlag;
            }

            set
            {                
                activeFlag = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
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
                masterEntityId = value;
                this.IsChanged = true;
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
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the LockerMake field
        /// </summary>
        [DisplayName("Locker Make")]
        public string LockerMake
        {
            get
            {
                return lockerMake;
            }

            set
            {
                lockerMake = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// LockerPanelList
        /// </summary>
        [Browsable(false)]
        [XmlIgnore()]
        public IList<LockerPanelDTO> LockerPanelDTOList
        {
            get
            {
                return lockerPanelDTOList;
            }

            set
            {                
                lockerPanelDTOList = value;                
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
                    return notifyingObjectIsChanged || zoneId < 0;
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
        /// IsChangedRecursive for lockerPanelDTOList 
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (lockerPanelDTOList != null &&
                   lockerPanelDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
