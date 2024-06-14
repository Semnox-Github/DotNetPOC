/********************************************************************************************
 * Project Name - Screen Setup DTO
 * Description  - Data object of Theme
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
    /// This is the ScreenSetup data object class. This acts as data holder for the ScreenSetup business object
    /// </summary>
    public class ScreenSetupDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
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

        private int screenId;
        private string name;
        private int alignment;
        private decimal scrDivHorizontal;
        private decimal scrDivVertical;
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
        private List<ScreenZoneDefSetupDTO> screenZoneDefSetupDTOList;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScreenSetupDTO()
        {
            log.LogMethodEntry();
            screenId = -1;
            masterEntityId = -1;
            alignment = -1;
            isActive = true;
            siteId = -1;
            screenZoneDefSetupDTOList = new List<ScreenZoneDefSetupDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScreenSetupDTO(int screenId, string name, int alignment, decimal scrDivHorizontal, decimal scrDivVertical,
                              string description, bool isActive)
            :this()
        {
            log.LogMethodEntry(screenId, name, alignment, scrDivHorizontal, scrDivVertical,
                              description, isActive);
            this.name = name;
            this.alignment = alignment;
            this.scrDivHorizontal = scrDivHorizontal;
            this.scrDivVertical = scrDivVertical;
            this.description = description;
            this.screenId = screenId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScreenSetupDTO(int screenId, string name, int alignment, decimal scrDivHorizontal, decimal scrDivVertical,
                              string description, bool isActive, string createdBy, DateTime creationDate,
                              string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId,
                              bool synchStatus, string guid)
            :this(screenId, name, alignment, scrDivHorizontal, scrDivVertical, description, isActive)
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
        /// Get/Set method of the ScreenID field
        /// </summary>
        [DisplayName("ID")]
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
        [DisplayName("Screen Name")]
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
        /// Get/Set method of the Alignment field
        /// </summary>
        [DisplayName("Alignment")]
        public int Alignment
        {
            get
            {
                return alignment;
            }

            set
            {
                this.IsChanged = true;
                alignment = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ScrDivHorizontal field
        /// </summary>
        [DisplayName("Rows")]
        public decimal ScrDivHorizontal
        {
            get
            {
                return scrDivHorizontal;
            }

            set
            {
                this.IsChanged = true;
                scrDivHorizontal = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ScrDivVertical field
        /// </summary>
        [DisplayName("Columns")]
        public decimal ScrDivVertical
        {
            get
            {
                return scrDivVertical;
            }

            set
            {
                this.IsChanged = true;
                scrDivVertical = value;
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
                    return notifyingObjectIsChanged || screenId < 0;
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
        /// Get/Set method of the ScreenZoneDefSetupDTOList field
        /// </summary>
        [Browsable(false)]
        public List<ScreenZoneDefSetupDTO> ScreenZoneDefSetupDTOList
        {
            get
            {
                return screenZoneDefSetupDTOList;
            }

            set
            {
                screenZoneDefSetupDTOList = value;
            }
        }

        /// <summary>
        /// Returns whether the screenZoneDefSetupDTO changed or any of its screenZoneDefSetupDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (screenZoneDefSetupDTOList != null &&
                   screenZoneDefSetupDTOList.Any(x => x.IsChanged))
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
