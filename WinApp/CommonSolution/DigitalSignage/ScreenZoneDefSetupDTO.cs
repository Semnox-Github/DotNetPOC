/********************************************************************************************
 * Project Name - ScreenZoneDefSetup DTO
 * Description  - Data object of ScreenZoneDefSetup
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        03-Mar-2017   Lakshminarayana          Created 
 *2.70.2        31-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the ScreenZoneDefSetup data object class. This acts as data holder for the ScreenZoneDefSetup business object
    /// </summary>
    public class ScreenZoneDefSetupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ZoneID field
            /// </summary>
            ZONE_ID,
           
            /// <summary>
            /// Search by  ScreenID field
            /// </summary>
            SCREEN_ID,
            
            /// <summary>
            /// Search by Name field
            /// </summary>
            NAME,
            
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int zoneId;
        private int screenId;
        private string name;
        private decimal topLeft;
        private decimal bottomRight;
        private string description;
        private int displayOrder;
        private int topOffsetY;
        private int bottomOffsetY;
        private int leftOffsetX;
        private int rightOffsetX;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private int siteId;
        private int masterEntityId;
        private bool synchStatus;
        private string guid;
        private List<ScreenZoneContentMapDTO> screenZoneContentMapDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScreenZoneDefSetupDTO()
        {
            log.LogMethodEntry();
            zoneId = -1;
            masterEntityId = -1;
            screenId = -1;
            topLeft = 0;
            bottomRight = 0;
            topOffsetY = 0;
            leftOffsetX = 0;
            rightOffsetX = 0;
            bottomOffsetY = 0;
            isActive = true;
            siteId = -1;
            screenZoneContentMapDTOList = new List<ScreenZoneContentMapDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScreenZoneDefSetupDTO(int zoneId, int screenId, string name, decimal topLeft, decimal bottomRight,
                                     string description, int displayOrder, int topOffsetY, int bottomOffsetY,
                                     int leftOffsetX, int rightOffsetX, bool isActive)
            :this()
        {
            log.LogMethodEntry(zoneId, screenId, name, topLeft, bottomRight,
                                      description, displayOrder, topOffsetY, bottomOffsetY,
                                      leftOffsetX, rightOffsetX, isActive);
            this.zoneId = zoneId;
            this.screenId = screenId;
            this.name = name;
            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
            this.description = description;
            this.displayOrder = displayOrder;
            this.topOffsetY = topOffsetY;
            this.bottomOffsetY = bottomOffsetY;
            this.leftOffsetX = leftOffsetX;
            this.rightOffsetX = rightOffsetX;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScreenZoneDefSetupDTO(int zoneId, int screenId, string name, decimal topLeft, decimal bottomRight, 
                                     string description, int displayOrder, int topOffsetY, int bottomOffsetY, 
                                     int leftOffsetX, int rightOffsetX, bool isActive, string createdBy,
                                     DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId,
                                     int masterEntityId, bool synchStatus, string guid)
            :this(zoneId, screenId, name, topLeft, bottomRight, description, displayOrder, topOffsetY, bottomOffsetY,
                                     leftOffsetX, rightOffsetX, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId, synchStatus, guid);
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
        /// Get/Set method of the ZoneId field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
        public int ZoneId
        {
            get
            {
                return zoneId;
            }

            set
            {
                this.IsChanged = true;
                zoneId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ScreenID field
        /// </summary>
        [DisplayName("Screen ID")]
        [ReadOnly(true)]
        public int ScreenId
        {
            get
            {
                return screenId;
            }

            set
            {
                this.IsChanged = true;
                screenId = value;
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
        /// Get/Set method of the DisplayOrder field
        /// </summary>
        [DisplayName("Display Order")]
        public int DisplayOrder
        {
            get
            {
                return displayOrder;
            }

            set
            {
                this.IsChanged = true;
                displayOrder = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Description field
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
        /// Get/Set method of the TopLeft field
        /// </summary>
        [DisplayName("Top Left")]
        public decimal TopLeft
        {
            get
            {
                return topLeft;
            }

            set
            {
                this.IsChanged = true;
                topLeft = value;
            }
        }

        /// <summary>
        /// Get/Set method of the BottomRight field
        /// </summary>
        [DisplayName("Bottom Right")]
        public decimal BottomRight
        {
            get
            {
                return bottomRight;
            }

            set
            {
                this.IsChanged = true;
                bottomRight = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TopOffsetY field
        /// </summary>
        [DisplayName("Top Offset")]
        public int TopOffsetY
        {
            get
            {
                return topOffsetY;
            }

            set
            {
                this.IsChanged = true;
                topOffsetY = value;
            }
        }

        /// <summary>
        /// Get/Set method of the BottomOffsetY field
        /// </summary>
        [DisplayName("Bottom Offset")]
        public int BottomOffsetY
        {
            get
            {
                return bottomOffsetY;
            }

            set
            {
                this.IsChanged = true;
                bottomOffsetY = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LeftOffsetX field
        /// </summary>
        [DisplayName("Left Offset")]
        public int LeftOffsetX
        {
            get
            {
                return leftOffsetX;
            }

            set
            {
                this.IsChanged = true;
                leftOffsetX = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RightOffsetX field
        /// </summary>
        [DisplayName("Right Offset")]

        public int RightOffsetX
        {
            get
            {
                return rightOffsetX;
            }

            set
            {
                this.IsChanged = true;
                rightOffsetX = value;
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
        /// Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || zoneId < 0;
                }
            }

            set
            {
                lock(notifyingObjectIsChangedSyncRoot)
                {
                    if(!Boolean.Equals(notifyingObjectIsChanged, value))
                    {
                        notifyingObjectIsChanged = value;
                    }
                }
            }
        }

        /// <summary>
        /// Get/Set method of the ScreenZoneContentMapDTO field
        /// </summary>
        [Browsable(false)]
        public List<ScreenZoneContentMapDTO> ScreenZoneContentMapDTOList
        {
            get
            {
                return screenZoneContentMapDTOList;
            }

            set
            {
                screenZoneContentMapDTOList = value;
            }
        }

        /// <summary>
        /// Returns whether the screenZoneContentMapDTO changed or any of its screenZoneContentMapDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (screenZoneContentMapDTOList != null &&
                   screenZoneContentMapDTOList.Any(x => x.IsChanged))
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
