/********************************************************************************************
 * Project Name - Concurrent Request Details
 * Description  - Data object of the Concurrent Request Details Programs
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By         Remarks          
 *********************************************************************************************
*2.70.2        24-Jul-2019    Dakshakh raj        Modified : Added Parameterized costrustor
 *2.90         26-May-2020    Mushahid Faizan     Modified : 3 tier changes for Rest API., Added IsActive Column.
 *2.140        08-Mar-2022     Fiona Lishal       Initialized string fields with string.Empty in Default Constructor
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.JobUtils
{
    public class ConcurrentRequestDetailsDTO
    {
        /// <summary>
        /// This is the Concurrent Request data object class. This acts as data holder for the  Concurrent Request Details business object
        /// </summary>

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByRequestParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by concurrentRequestDetailsId field
            /// </summary>
            CONCURRENT_REQUEST_DETAILS_ID,
            
            /// <summary>
            /// Search by concurrentProgramId field
            /// </summary>
            CONCURRENT_PROGRAM_ID,
            /// <summary>
            /// Search by concurrentProgramId field
            /// </summary>
            CONCURRENT_PROGRAM_ID_LIST,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by parafaitObjectId field
            /// </summary>
            PARAFAIT_OBJECT_ID,
            
            /// <summary>
            /// Search by parafaitObjectGuid field
            /// </summary>
            PARAFAIT_OBJECT_GUID,
            
            /// <summary>
            /// Search by status field
            /// </summary>
            STATUS,
           
            /// <summary>
            /// Search by siteId field
            /// </summary>
            SITE_ID,
            
            /// <summary>
            /// Search by masterEntityId field
            /// </summary>
            MASTER_ENTITIY_ID,
            
            /// <summary>
            /// Search by concurrentRequestId field
            /// </summary>
            CONCURRENT_REQUEST_ID,
            /// <summary>
            /// Search by EXTERNAL REFERENCE field
            /// </summary>
            EXTERNAL_REFERENCE,
            /// <summary>
            /// Search by TIMESTAMP_GREATER_THAN_EQUAL_TO field
            /// </summary>
            TIMESTAMP_GREATER_THAN_EQUAL_TO,
            /// <summary>
            /// Search by TIMESTAMP_LESS_THAN_EQUAL_TO field
            /// </summary>
            TIMESTAMP_LESS_THAN_EQUAL_TO,
            /// <summary>
            /// Search by STATUS_NOT_IN field
            /// </summary>
            STATUS_NOT_IN
        }

        private int concurrentRequestDetailsId;
        private int concurrentRequestId;
        private DateTime timestamp;
        private int concurrentProgramId;
        private string parafaitObject;
        private int parafaitObjectId;
        private string parafaitObjectGuid;
        private bool isSuccessFul;
        private string status;
        private string externalReference;
        private string data;
        private string remarks;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        private bool isActive;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConcurrentRequestDetailsDTO()
        {
            log.LogMethodEntry();
            concurrentRequestDetailsId = -1;
            concurrentRequestId = -1;
            concurrentProgramId = -1; 
            parafaitObjectId = -1; 
            siteId = -1; 
            masterEntityId = -1;
            isActive = true;
            parafaitObject = string.Empty;
            status = string.Empty;
            externalReference = string.Empty;
            data = string.Empty;
            remarks = string.Empty;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        public ConcurrentRequestDetailsDTO(int concurrentRequestDetailsId, int concurrentRequestId, DateTime timestamp, int concurrentProgramId, string parafaitObject, int parafaitObjectId, string parafaitObjectGuid,
                                          bool isSuccessFul, string status, string externalReference, string data, string remarks, bool isActive)
           :this()
        {
            log.LogMethodEntry(concurrentRequestDetailsId, concurrentRequestId, timestamp, concurrentProgramId, parafaitObject, parafaitObjectId, parafaitObjectGuid, isSuccessFul, status, externalReference, data, remarks, isActive);
            this.concurrentRequestDetailsId = concurrentRequestDetailsId;
            this.concurrentRequestId = concurrentRequestId;
            this.timestamp = timestamp;
            this.concurrentProgramId = concurrentProgramId;
            this.parafaitObject = parafaitObject;
            this.parafaitObjectId = parafaitObjectId;
            this.parafaitObjectGuid = parafaitObjectGuid;
            this.isSuccessFul = isSuccessFul;
            this.status = status;
            this.externalReference = externalReference;
            this.data = data;
            this.remarks = remarks;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ConcurrentRequestDetailsDTO(int concurrentRequestDetailsId, int concurrentRequestId, DateTime timestamp, int concurrentProgramId, string parafaitObject, int parafaitObjectId, string parafaitObjectGuid,
                                           bool isSuccessFul, string status, string externalReference, string data, string remarks, DateTime creationDate, string createdBy, DateTime lastUpdatedDate,
                                           string lastUpdatedBy, int siteId, string guid, bool synchStatus, int masterEntityId, bool isActive)
            :this(concurrentRequestDetailsId, concurrentRequestId, timestamp, concurrentProgramId, parafaitObject, parafaitObjectId, parafaitObjectGuid, isSuccessFul, status, externalReference, data, remarks, isActive)
        {
            log.LogMethodEntry(creationDate, createdBy, lastUpdatedDate, lastUpdatedBy, siteId, guid, synchStatus, masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ConcurrentRequestDetailsId field
        /// </summary>
        [DisplayName("ConcurrentRequestDetailsId")]
        public int ConcurrentRequestDetailsId { get { return concurrentRequestDetailsId; } set { concurrentRequestDetailsId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ConcurrentRequestId field
        /// </summary>
        [DisplayName("ConcurrentRequestId")]
        public int ConcurrentRequestId { get { return concurrentRequestId; } set { concurrentRequestId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        [DisplayName("Timestamp")]
        public DateTime Timestamp { get { return timestamp; } set { timestamp = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ConcurrentProgramId field
        /// </summary>
        [DisplayName("ConcurrentProgramId")]
        public int ConcurrentProgramId { get { return concurrentProgramId; } set { concurrentProgramId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParafaitObject field
        /// </summary>
        [DisplayName("ParafaitObject")]
        public string ParafaitObject { get { return parafaitObject; } set { parafaitObject = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParafaitObjectId field
        /// </summary>
        [DisplayName("ParafaitObjectId")]
        public int ParafaitObjectId { get { return parafaitObjectId; } set { parafaitObjectId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ParafaitObjectGuid field
        /// </summary>
        [DisplayName("ParafaitObjectGuid")]
        public string ParafaitObjectGuid { get { return parafaitObjectGuid; } set { parafaitObjectGuid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the IsSuccessFul field
        /// </summary>
        [DisplayName("IsSuccessFul")]
        public bool IsSuccessFul { get { return isSuccessFul; } set { isSuccessFul = value; this.IsChanged = true; } }


        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status { get { return status; } set { status = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ExternalReference field
        /// </summary>
        [DisplayName("ExternalReference")]
        public string ExternalReference { get { return externalReference; } set { externalReference = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Data field
        /// </summary>
        [DisplayName("Data")]
        public string Data { get { return data; } set { data = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Remarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string Remarks { get { return remarks; } set { remarks = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value;  } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value;  } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the isActive field
        /// </summary>
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
                    return notifyingObjectIsChanged || concurrentRequestDetailsId < 0;
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
