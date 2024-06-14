/********************************************************************************************
 * Project Name - DisplayPanelThemeMapDTO
 * Description  - Data object of DisplayPanelThemeMap
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *1.00        01-Mar-2017   Lakshminarayana          Created 
 *2.70.2        30-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the DisplayPanelThemeMap data object class. This acts as data holder for the DisplayPanelThemeMap business object
    /// </summary>
    public class DisplayPanelThemeMapDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  id field
            /// </summary>
            ID,
            
            /// <summary>
            /// Search by PanelId field
            /// </summary>
            PANEL_ID,
            
            /// <summary>
            /// Search by ThemeId field
            /// </summary>
            THEME_ID,
            
            /// <summary>
            /// Search by ScheduleId field
            /// </summary>
            SCHEDULE_ID,
            
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            
            /// <summary>
            /// Search by site id field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int panelId;
        private int themeId;
        private int scheduleId;
        private int id;
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
        public DisplayPanelThemeMapDTO()
        {
            log.LogMethodEntry();
            id = -1;
            panelId = -1;
            themeId = -1;
            scheduleId = -1;
            masterEntityId = -1;
            isActive = true;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public DisplayPanelThemeMapDTO(int id, int panelId, int themeId, int scheduleId, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, panelId, themeId, scheduleId, isActive);
            this.panelId = panelId;
            this.themeId = themeId;
            this.scheduleId = scheduleId;
            this.id = id;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DisplayPanelThemeMapDTO(int id, int panelId, int themeId, int scheduleId, bool isActive, string createdBy,
                                       DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate, int siteId,
                                       int masterEntityId, bool synchStatus, string guid)
            :this(id, panelId, themeId, scheduleId, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdateDate, siteId, masterEntityId,  synchStatus,  guid);
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
        [DisplayName("SI#")]
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
        /// Get/Set method of the PanelId field
        /// </summary>
        [DisplayName("Panel")]
        public int PanelId
        {
            get
            {
                return panelId;
            }

            set
            {
                this.IsChanged = true;
                panelId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ThemeId field
        /// </summary>
        [DisplayName("Theme")]
        public int ThemeId
        {
            get
            {
                return themeId;
            }

            set
            {
                this.IsChanged = true;
                themeId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [Browsable(false)]
        public int ScheduleId
        {
            get
            {
                return scheduleId;
            }

            set
            {
                this.IsChanged = true;
                scheduleId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active")]
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
                    return notifyingObjectIsChanged ||id < 0;
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
        /// Allowes to accept the changes
        /// </summary>
        public void AcceptChanges()
        {
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns a string that represents the current DSignageLookupValuesDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DisplayPanelThemeMapDTO-----------------------------\n");
            returnValue.Append(" Id : " + Id);
            returnValue.Append(" PanelId : " + PanelId);
            returnValue.Append(" ThemeId : " + ThemeId);
            returnValue.Append(" ScheduleId : " + ScheduleId);
            returnValue.Append(" IsActive : " + IsActive);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();
        }
    }
}
