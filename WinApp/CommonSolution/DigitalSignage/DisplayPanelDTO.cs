/********************************************************************************************
 * Project Name - DisplayPanel DTO
 * Description  - Data object of DisplayPanel
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *1.00        03-Mar-2017   Lakshminarayana          Created
 *2.70.2        30-Jul-2019   Dakshakh raj             Modified : Added Parameterized costrustor
 ********************************************************************************************/
using System;
using System.ComponentModel;
using System.Text;

namespace Semnox.Parafait.DigitalSignage
{
    /// <summary>
    /// This is the DisplayPanel data object class. This acts as data holder for the DisplayPanel business object
    /// </summary>
    public class DisplayPanelDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  PanelID field
            /// </summary>
            PANEL_ID,
            
            /// <summary>
            /// Search by PanelName field
            /// </summary>
            PANEL_NAME,
            
            /// <summary>
            /// Search by DisplayGroup field
            /// </summary>
            DISPLAY_GROUP,
            
            /// <summary>
            /// Search by PCName field
            /// </summary>
            PC_NAME,

            /// <summary>
            /// Search by PCName field
            /// </summary>
            PC_NAME_EXACT,
            
            /// <summary>
            /// Search by MACAddress field
            /// </summary>
            MAC_ADDRESS,
            
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
        private string panelName;
        private string pCName;
        private string displayGroup;
        private string location;
        private string mACAddress;
        private string description;
        private decimal startTime;
        private decimal endTime;
        private int resolutionX;
        private int resolutionY;
        private string localFolder;
        private string restartFlag;
        private DateTime? lastRestartTime;
        private int? shutdownSec;

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
        public DisplayPanelDTO()
        {
            log.LogMethodEntry();
            panelId = -1;
            masterEntityId = -1;
            isActive = true;
            restartFlag = "N";
            resolutionX = -1;
            resolutionY = -1;
            startTime = new decimal(12.0);
            endTime = new decimal(12.0);
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public DisplayPanelDTO(int panelId, string panelName, string pCName, string displayGroup, string location,
                               string mACAddress, string description, decimal startTime, decimal endTime, int resolutionX,
                               int resolutionY, string localFolder, string restartFlag, DateTime? lastRestartTime,
                               int? shutdownSec, bool isActive)
            :this()
        {
            log.LogMethodEntry(panelId, panelName, pCName, displayGroup, location, mACAddress, description, startTime, endTime,
                               resolutionX, resolutionY, localFolder, restartFlag, lastRestartTime, shutdownSec, isActive);
            this.panelId = panelId;
            this.panelName = panelName;
            this.pCName = pCName;
            this.displayGroup = displayGroup;
            this.location = location;
            this.mACAddress = mACAddress;
            this.description = description;
            this.startTime = startTime;
            this.endTime = endTime;
            this.resolutionX = resolutionX;
            this.resolutionY = resolutionY;
            this.localFolder = localFolder;
            this.restartFlag = restartFlag;
            this.lastRestartTime = lastRestartTime;
            this.shutdownSec = shutdownSec;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DisplayPanelDTO(int panelId, string panelName, string pCName, string displayGroup, string location,
                               string mACAddress, string description, decimal startTime, decimal endTime, int resolutionX,
                               int resolutionY, string localFolder, string restartFlag, DateTime? lastRestartTime,
                               int? shutdownSec, bool isActive, string createdBy, DateTime creationDate,
                               string lastUpdatedBy, DateTime lastUpdateDate, int siteId, int masterEntityId,
                               bool synchStatus, string guid)
            :this(panelId, panelName, pCName, displayGroup, location, mACAddress, description, startTime, endTime,
                  resolutionX, resolutionY, localFolder, restartFlag, lastRestartTime, shutdownSec, isActive)
        {
            log.LogMethodEntry(createdBy,  creationDate, lastUpdatedBy,  lastUpdateDate,  siteId,  masterEntityId, synchStatus,  guid);
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
        /// Get/Set method of the PanelID field
        /// </summary>
        [DisplayName("ID")]
        [ReadOnly(true)]
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
        /// Get/Set method of the PanelName field
        /// </summary>
        [DisplayName("Panel Name")]
        public string PanelName
        {
            get
            {
                return panelName;
            }

            set
            {
                this.IsChanged = true;
                panelName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Location field
        /// </summary>
        [DisplayName("Location")]
        public string Location
        {
            get
            {
                return location;
            }

            set
            {
                this.IsChanged = true;
                location = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MACAddress field
        /// </summary>
        [DisplayName("MAC Address")]
        public string MACAddress
        {
            get
            {
                return mACAddress;
            }

            set
            {
                this.IsChanged = true;
                mACAddress = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PCName field
        /// </summary>
        [DisplayName("PC Name")]
        public string PCName
        {
            get
            {
                return pCName;
            }

            set
            {
                this.IsChanged = true;
                pCName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ShutdownSec field
        /// </summary>
        [DisplayName("Shutdown Sec")]
        public int? ShutdownSec
        {
            get
            {
                return shutdownSec;
            }

            set
            {
                this.IsChanged = true;
                shutdownSec = value;
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
        /// Get/Set method of the ResolutionX field
        /// </summary>
        [DisplayName("Horizontal (Pixels)")]
        public int ResolutionX
        {
            get
            {
                return resolutionX;
            }

            set
            {
                this.IsChanged = true;
                resolutionX = value;
            }
        }

        /// <summary>
        /// Get/Set method of the ResolutionY field
        /// </summary>
        [DisplayName("Vertical (Pixels)")]
        public int ResolutionY
        {
            get
            {
                return resolutionY;
            }

            set
            {
                this.IsChanged = true;
                resolutionY = value;
            }
        }

        /// <summary>
        /// Get/Set method of the DisplayGroup field
        /// </summary>
        [DisplayName("Display Group")]
        public string DisplayGroup
        {
            get
            {
                return displayGroup;
            }

            set
            {
                this.IsChanged = true;
                displayGroup = value;
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
        /// Get/Set method of the StartTime field
        /// </summary>
        [DisplayName("Start Time")]
        public decimal StartTime
        {
            get
            {
                return startTime;
            }

            set
            {
                this.IsChanged = true;
                startTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the EndTime field
        /// </summary>
        [DisplayName("End Time")]
        public decimal EndTime
        {
            get
            {
                return endTime;
            }

            set
            {
                this.IsChanged = true;
                endTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LocalFolder field
        /// </summary>
        [DisplayName("Restart Flag")]
        public string RestartFlag
        {
            get
            {
                return restartFlag;
            }

            set
            {
                this.IsChanged = true;
                restartFlag = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LocalFolder field
        /// </summary>
        [DisplayName("Local Folder")]
        public string LocalFolder
        {
            get
            {
                return localFolder;
            }

            set
            {
                this.IsChanged = true;
                localFolder = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastRestartTime field
        /// </summary>
        [Browsable(false)]
        public DateTime? LastRestartTime
        {
            get
            {
                return lastRestartTime;
            }

            set
            {
                this.IsChanged = true;
                lastRestartTime = value;
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
                    return notifyingObjectIsChanged || panelId < 0;
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
        /// Returns a string that represents the current DisplayPanelDTO
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            log.LogMethodEntry();
            StringBuilder returnValue = new StringBuilder("\n-----------------------DisplayPanelDTO-----------------------------\n");
            returnValue.Append(" PanelID : " + PanelId);
            returnValue.Append(" PanelName : " + PanelName);
            returnValue.Append(" PCName : " + PCName);
            returnValue.Append(" Display_Group : " + DisplayGroup);
            returnValue.Append(" Location : " + Location);
            returnValue.Append(" MACAddress : " + MACAddress);
            returnValue.Append(" Active : " + IsActive);
            returnValue.Append(" Description : " + Description);
            returnValue.Append(" StartTime : " + StartTime);
            returnValue.Append(" EndTime : " + EndTime);
            returnValue.Append(" ResolutionX : " + ResolutionX);
            returnValue.Append(" ResolutionY : " + ResolutionY);
            returnValue.Append(" LocalFolder : " + LocalFolder);
            returnValue.Append(" RestartFlag : " + RestartFlag);
            returnValue.Append(" LastRestartTime : " + LastRestartTime);
            returnValue.Append("\n-------------------------------------------------------------\n");
            log.LogMethodExit(returnValue.ToString());
            return returnValue.ToString();

        }
    }
}
