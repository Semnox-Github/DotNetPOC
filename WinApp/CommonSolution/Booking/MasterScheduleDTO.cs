/* Project Name - Semnox.Parafait.Booking.MasterScheduleDTO 
* Description  - Data object of the AttractionMasterSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
********************************************************************************************/

using System;
using System.ComponentModel;


namespace Semnox.Parafait.Booking
{

    /// <summary>
    /// MasterScheduleDTO Class
    /// </summary>
    public class MasterScheduleDTO
    {

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>

        public enum SearchByParameters
        { 
            /// <summary>
            /// Search by  MasterScheduleId field
            /// </summary>
            MASTER_SCHEDULE_ID = 0,
            /// <summary>
            /// Search by  ActiveFlag field
            /// </summary>
            ACTIVE_FLAG = 1,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 2,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 3
        }
         
        private int masterScheduleId;
        private string masterScheduleName; 
        private bool activeFlag;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate; 

        private int siteId;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public MasterScheduleDTO()
        {
            log.LogMethodEntry(); 
            masterEntityId = -1;
            masterScheduleId = -1;
            activeFlag = true;
            log.LogMethodExit();
        }



        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MasterScheduleDTO(int masterScheduleId, string masterScheduleName, bool activeFlag,
                                     string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)

        {

            log.LogMethodEntry(masterScheduleId, masterScheduleName, activeFlag,
                                      guid,  siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
            this.masterScheduleId = masterScheduleId;
            this.masterScheduleName = masterScheduleName;
            this.activeFlag = activeFlag;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate; 

            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MasterScheduleId field
        /// </summary>
        [DisplayName("MasterScheduleId")] 
        public int MasterScheduleId { get { return masterScheduleId; } set { masterScheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterScheduleName field
        /// </summary>
        [DisplayName("MasterScheduleName")]
        public string MasterScheduleName { get { return masterScheduleName; } set { masterScheduleName = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        public bool ActiveFlag { get { return activeFlag; } set {activeFlag = value; this.IsChanged = true; } }         

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
        }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Last Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Last Update Date")]
        [Browsable(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
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
                    return notifyingObjectIsChanged;
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
