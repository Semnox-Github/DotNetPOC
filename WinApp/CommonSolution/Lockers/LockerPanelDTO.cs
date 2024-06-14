/********************************************************************************************
 * Project Name - Locker Panels DTO
 * Description  - Data object of locker panels DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 *********************************************************************************************
 *1.00        17-Apr-2017   Raghuveera          Created 
 *2.70        19-Jul-2019   Dakshakh raj        Modified : Added Parameterized costrustor,
 *                                                         CreatedBy and CreationDate fields
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace Semnox.Parafait.Device.Lockers
{
    /// <summary>
    /// This is the locker panels data object class. This acts as data holder for the locker panels business object
    /// </summary>
    public class LockerPanelDTO
    {
        /// <summary>
        /// SearchByLockerPanelsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByLockerPanelsParameters
        {
            /// <summary>
            /// Search by PANEL ID field
            /// </summary>
            PANEL_ID,
            
            /// <summary>
            /// Search by PANEL NAME field
            /// </summary>
            PANEL_NAME,
           
            /// <summary>
            /// Search by ZONE ID field
            /// </summary>
            ZONE_ID,
           
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
           
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
           
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by Zone ID LIST field
            /// </summary>
            Zone_ID_LIST
        }

        private int panelId;
        private string panelName;
        private int zoneId;
        private string sequencePrefix;
        private int numRows;
        private int numCols;
        private bool isActive;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;

        private IList<LockerDTO> lockerDTOList;

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Contructor
        /// </summary>
        public LockerPanelDTO()
        {
            log.LogMethodEntry();
            panelId = -1;
            zoneId = -1;
            numRows = -1;
            numCols = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required fields
        /// </summary>        
        public LockerPanelDTO(int panelId, string panelName, int zoneId, string sequencePrefix, int numRows, int numCols, bool isActive)
            :this()
        {
            log.LogMethodEntry(panelId, panelName, zoneId, sequencePrefix, numRows, numCols, isActive);
            this.panelId = panelId;
            this.panelName = panelName;
            this.zoneId = zoneId;
            this.sequencePrefix = sequencePrefix;
            this.numRows = numRows;
            this.numCols = numCols;
            this.isActive = isActive;
            this.lockerDTOList = lockerDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>        
        public LockerPanelDTO(int panelId, string panelName, int zoneId, string sequencePrefix, int numRows, int numCols,
                          bool isActive, string lastUpdatedBy, DateTime lastUpdatedDate, int siteId,
                          string guid, bool synchStatus, int masterEntityId,  string createdBy ,DateTime creationDate)
 
            : this(panelId, panelName, zoneId, sequencePrefix, numRows, numCols, isActive)
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
        /// Get/Set method of the PanelId field
        /// </summary>
        [DisplayName("PanelId")]
        [ReadOnly(true)]
        public int PanelId { get { return panelId; } set { panelId = value; } }

        /// <summary>
        /// Get/Set method of the PanelName field
        /// </summary>
        [DisplayName("PanelName")]
        public string PanelName { get { return panelName; } set { panelName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ZoneId field
        /// </summary>
        [DisplayName("ZoneId")]
        public int ZoneId { get { return zoneId; } set { zoneId = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Get/Set method of the SequencePrefix field
        /// </summary>
        [DisplayName("SequencePrefix")]
        public string SequencePrefix { get { return sequencePrefix; } set { sequencePrefix = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the NumRows field
        /// </summary>
        [DisplayName("NumRows")]
        public int NumRows { get { return numRows; } set { numRows = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the NumCols field
        /// </summary>
        [DisplayName("NumCols")]
        public int NumCols { get { return numCols; } set { numCols = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("IsActive")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedUserId field
        /// </summary>
        [DisplayName("LastUpdatedUserId")]
        [Browsable(false)]
        public string LastUpdatedUserId { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value;} }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
        /// Get/Set method of the Locker DTO List field
        /// </summary>
        [XmlIgnore()]
        [Browsable(false)]
        public IList<LockerDTO> LockerDTOList { get { return lockerDTOList; } set { lockerDTOList = value;} }
       
        /// <summary>
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || panelId < 0;
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
        /// IsChangedRecursive for lockerDTOList
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (lockerDTOList != null &&
                   lockerDTOList.Any(x => x.IsChanged))
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
