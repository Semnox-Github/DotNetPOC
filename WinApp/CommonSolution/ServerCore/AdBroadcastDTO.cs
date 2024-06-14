/********************************************************************************************
 * Project Name - AdBroadcast DTO
 * Description  - Data object of AdBroadcast
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************
 *2.60        17-May-2019   Jagan Mohana Rao        Created 
 *2.70.2      26-Jan-2020   Girish Kundar           Modified : Changed to Standard format 
 *2.80       20-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API., Added IsActive Column.
 *********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.ServerCore
{
    public class AdBroadcastDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by ID field
            /// </summary>
            ID,
            /// <summary>
            /// Search by BROADCAST_FILE_NAME field
            /// </summary>
            BROADCAST_FILE_NAME,
            /// <summary>
            /// Search by AD_ID field
            /// </summary>
            AD_ID,
            /// <summary>
            /// Search by AD_ID  LIST field
            /// </summary>
            AD_ID_LIST,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

       private  int id;
       private  string broadcastFileName;
       private  DateTime broadcastBeginTime;
       private  DateTime broadcastEndTime;
       private  int adId;
       private  int machineId;
       private  string guid;
       private  int siteId;
       private  bool synchStatus;
       private  int masterEntityId;
       private  string createdBy;
       private  DateTime creationDate;
       private  string lastUpdatedBy;
       private  DateTime lastUpdatedDate;
       private  bool isActive;
        /// <summary>
        /// Default constructor
        /// </summary>
        public AdBroadcastDTO()
        {
            log.LogMethodEntry();
            id = -1;
            adId = -1;
            siteId = -1;
            masterEntityId = -1;
            machineId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public AdBroadcastDTO(int id, string broadcastFileName, DateTime broadcastBeginTime, DateTime broadcastEndTime, int adId, int machineId, bool isActive)
            :this()
        {
            log.LogMethodEntry(id, broadcastFileName, broadcastBeginTime, broadcastEndTime, adId, machineId, isActive);
            this.id = id;
            this.broadcastFileName = broadcastFileName;
            this.broadcastBeginTime = broadcastBeginTime;
            this.broadcastEndTime = broadcastEndTime;
            this.adId = adId;
            this.machineId = machineId;
            this.isActive = isActive;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public AdBroadcastDTO(int id, string broadcastFileName, DateTime broadcastBeginTime, DateTime broadcastEndTime, int adId, int machineId, string guid, int siteId, bool synchStatus, int masterEntityId,
                           string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, bool isActive)
            :this(id, broadcastFileName, broadcastBeginTime, broadcastEndTime, adId, machineId, isActive)
        {
            log.LogMethodEntry(id, broadcastFileName, broadcastBeginTime, broadcastEndTime, adId,machineId, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, isActive);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        public int Id { get { return id; } set { id = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BroadCastFileName field
        /// </summary>
        public string BroadCastFileName { get { return broadcastFileName; } set { broadcastFileName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BroadcastBeginTime field
        /// </summary>
        public DateTime BroadcastBeginTime { get { return broadcastBeginTime; } set { broadcastBeginTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the BroadcastEndTime field
        /// </summary>
        public DateTime BroadcastEndTime { get { return broadcastEndTime; } set { broadcastEndTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AdId field
        /// </summary>
        public int AdId { get { return adId; } set { adId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MachineId field
        /// </summary>
        public int MachineId { get { return machineId; } set { machineId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
      
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
   
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
       
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
      
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Active field
        /// </summary>
      
        [Browsable(false)]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged || id < 0;
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