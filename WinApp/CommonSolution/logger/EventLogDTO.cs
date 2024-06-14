/********************************************************************************************
 * Project Name - Event Log DTO
 * Description  - Data object of Event log
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.70        16-Jul-2019   Dakshakh raj        Modified :Added WHO fields 
 *2.80.0      20-Mar-2020   Akshay Gulaganji    Added SearchByEventLogParameters
 *2.90        26-May-2020   Vikas Dwivedi       Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// EventLogDTO class to expose EventLog attributes. This is used to insert to EventLog table
    /// </summary>
    public class EventLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByEventLogParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByEventLogParameters
        {
            /// <summary>
            /// Search by EVENT_LOG_ID field
            /// </summary>
            EVENT_LOG_ID,
            /// <summary>
            /// Search by SOURCE field
            /// </summary>
            SOURCE,
            /// <summary>
            /// Search by TYPE field
            /// </summary>
            TYPE,
            /// <summary>
            /// Search by USER_NAME field
            /// </summary>
            USER_NAME,
            /// <summary>
            /// Search by COMPUTER field
            /// </summary>
            COMPUTER,
            /// <summary>
            /// Search by CATEGORY field
            /// </summary>
            CATEGORY,
            /// <summary>
            /// Search by TIMESTAMP field
            /// </summary>
            TIMESTAMP,
            /// <summary>
            /// Search by ORDER BY TIMESTAMP field
            /// </summary>
            ORDER_BY_TIMESTAMP,
            /// <summary>
            /// Search by SITEID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int eventLogId;
        private string source;
        private DateTime timestamp;
        private string type;
        private string username;
        private string computer;
        private string data;
        private string description;
        private string category;
        private int severity;
        private string name;
        private string eventValue;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EventLogDTO()
        {
            log.LogMethodEntry();
            eventLogId = -1;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public EventLogDTO(int eventLogId, string source, DateTime timestamp, string type, string username, string computer,
                           string data, string description, string category, int severity, string name, string eventValue, string guid,
                            int siteId, bool synchStatus)
            : this()
        {
            log.LogMethodEntry( eventLogId,  source,  timestamp,  type,  username,  computer, data,  description,  category,  severity,  name,  eventValue, guid, siteId, synchStatus);
            this.eventLogId = eventLogId;
            this.source = source;
            this.timestamp = timestamp;
            this.type = type;
            this.username = username;
            this.computer = computer;
            this.data = data;
            this.description = description;
            this.category = category;
            this.severity = severity;
            this.name = name;
            this.eventValue = eventValue;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public EventLogDTO(int eventLogId, string source, DateTime timestamp, string type, string username, string computer,
                           string data, string description, string category, int severity, string name, string eventValue,string guid,
                            int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            :this(eventLogId, source, timestamp, type, username, computer, data, description, category, severity, name, eventValue,  guid, siteId, synchStatus)
        {
            log.LogMethodEntry(masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// EventLogId
        /// </summary>
        public int EventLogId { get { return eventLogId; } set { eventLogId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Source
        /// </summary>
        public string Source { get { return source; } set { source= value; this.IsChanged = true; } }
        
        /// <summary>
        /// TimeStamp
        /// </summary>
        public DateTime Timestamp { get { return timestamp; } set { timestamp = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Type
        /// </summary>
        public string Type { get { return type; } set { type = value; this.IsChanged = true; } }

        
        /// <summary>
        /// UserName
        /// </summary>
        public string Username { get { return username; } set { username = value; this.IsChanged = true; } }

        /// <summary>
        /// Computer
        /// </summary>
        public string Computer { get { return computer; } set { computer = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Data
        /// </summary>
        public string Data { get { return data; } set { data = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get { return description; } set { description = value; this.IsChanged = true; } }
       
        /// <summary>
        /// Category
        /// </summary>
        public string Category { get { return category; } set { category = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Severity
        /// </summary>
        public int Severity { get { return severity; } set { severity = value; this.IsChanged = true; } }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get { return name; } set { name = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Event Value
        /// </summary>
        public string EventValue { get { return eventValue; } set { eventValue = value; this.IsChanged = true; } }
        /// <summary>
        /// Guid
        /// </summary>
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Site ID
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// SyncStatus
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the createdBy fields
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreatedDate fields
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the lastUpdatedBy fields
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the lastUpdatedAt fields
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value;} }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value;  } }

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
                    return notifyingObjectIsChanged || eventLogId < 0;
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
