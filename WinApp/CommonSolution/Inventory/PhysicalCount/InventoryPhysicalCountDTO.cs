/********************************************************************************************
* Project Name -inventoryHistory DTO
* Description  -Data object of inventoryPhysicalCount 
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        6-Jan-2017    Amaresh          Created 
*2.80        18-Aug-2019   Deeksha          Modifications as per three tier changes.
*2.100.0     17-Sep-2020   Deeksha          Modified Is changed property to handle unsaved records.
*2.120.0     15-Apr-2021   Mushahid Faizan  Modified: Added ScheduledDate, Frequency column as part of web Inventory changes
********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the InventoryPhysicalCount data object class. This acts as data holder for the InventoryPhysicalCount object
    /// </summary>
    public class InventoryPhysicalCountDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByInventoryPhysicalCountParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryPhysicalCountParameters
        {
            /// <summary>
            /// Search by PHYSICAL COUNT ID field
            /// </summary>
            PHYSICAL_COUNT_ID,

            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME,

            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS,

            /// <summary>
            /// Search by START DATE field
            /// </summary>
            START_DATE,

            /// <summary>
            /// Search by END DATE field
            /// </summary>
            END_DATE,

            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by LOCATION ID field
            /// </summary>
            LOCATIONID,

            SCHEDULED_DATE,
            FREQUENCY
        }

        private int physicalCountID;
        private string name;
        private string status;
        private DateTime startDate;
        private DateTime endDate;
        private string initiatedBy;
        private string closedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private int locationID;
        private string createdBy;
        private DateTime creationDate;
        private DateTime lastUpdateDate;
        private DateTime scheduledDate;
        private string frequency;
        private string lastUpdatedBy;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public InventoryPhysicalCountDTO()
        {
            log.LogMethodEntry();
            physicalCountID = -1;
            masterEntityId = -1;
            locationID = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with required data fields
        /// </summary>
        public InventoryPhysicalCountDTO(int physicalCountID, string name, string status, DateTime startDate, DateTime endDate, string initiatedBy,
                                          string closedBy, int locationID, DateTime scheduledDate, string frequency)
            :this()
        {
            log.LogMethodEntry( physicalCountID,  name,  status,  startDate,  endDate,  initiatedBy, closedBy, locationID, scheduledDate, frequency);
            this.physicalCountID = physicalCountID;
            this.name = name;
            this.status = status;
            this.startDate = startDate;
            this.endDate = endDate;
            this.initiatedBy = initiatedBy;
            this.closedBy = closedBy;
            
            this.locationID = locationID;
            this.scheduledDate = scheduledDate;
            this.frequency = frequency;


            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor with all the data fields
        /// </summary>
        public InventoryPhysicalCountDTO(int physicalCountID, string name, string status, DateTime startDate, DateTime endDate, string initiatedBy,
                                          string closedBy, int siteId, string guid, bool synchStatus, int masterEntityId, int locationID, string createdBy,
                                          DateTime creationDate,string lastUpdatedBy, DateTime lastUpdateDate, DateTime scheduledDate, string frequency)
            :this(physicalCountID, name, status, startDate, endDate, initiatedBy, closedBy, locationID,scheduledDate, frequency)
        {
            log.LogMethodEntry(physicalCountID, name, status, startDate, endDate, initiatedBy,
                              closedBy, siteId, guid, synchStatus, masterEntityId, locationID, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the PhysicalCountID field
        /// </summary>
        [DisplayName("PhysicalCountID")]
        [ReadOnly(true)]
        public int PhysicalCountID { get { return physicalCountID; } set { physicalCountID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the StartDate field
        /// </summary>
        [DisplayName("Start Date")]
        public DateTime StartDate { get { return startDate; } set { startDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the EndDate field
        /// </summary>
        [DisplayName("End Date")]
        public DateTime EndDate { get { return endDate; } set { endDate = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the InitiatedBy field
        /// </summary>
        [DisplayName("Initiated By")]
        public string InitiatedBy { get { return initiatedBy; } set { initiatedBy = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the ClosedBy field
        /// </summary>
        [DisplayName("Closed By")]
        public string ClosedBy { get { return closedBy; } set { closedBy = value; this.IsChanged = true; } }


        /// <summary>
        /// Get method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value;  } }

        /// <summary>
        /// Get method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }

        /// <summary>
        /// Get method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the LocationId field
        /// </summary>
        [DisplayName("Location")]
        public int LocationId { get { return locationID; } set { locationID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get method of the Frequency field
        /// </summary>
        [DisplayName("Interval")]
        public string Frequency { get { return frequency; } set { frequency = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get method of the Frequency field
        /// </summary>
        [DisplayName("Scheduled Date")]
        public DateTime ScheduledDate { get { return scheduledDate; } set { scheduledDate = value; this.IsChanged = true; } }


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
        /// Get/Set method of the LastUpdateDate field
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
        ///  Get/Set method to track changes to the object
        /// </summary>
        [Browsable(false)]
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || physicalCountID < 0;
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
