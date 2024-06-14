using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class CheckInPauseLogDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByCheckInPauseLogParameters
        {
            /// <summary>
            /// Search by  CHECK_IN_PAUSE_LOG_ID field
            /// </summary>
            CHECK_IN_PAUSE_LOG_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by CheckInDetailId field
            /// </summary>
            CHECK_IN_DETAIL_ID,
            /// <summary>
            /// Search by CheckInDetailIdList field
            /// </summary>
            CHECK_IN_DETAIL_ID_LIST,
            /// <summary>
            /// Search by PauseEndTimeIsNull field
            /// </summary>
            PAUSE_END_TIME_IS_NULL,
        }
        
        int checkInPauseLogId;
        int checkInDetailId;
        DateTime pauseStartTime;
        DateTime? pauseEndTime;
        int totalPauseTime;
        string pOSMachine;
        string pausedBy;
        string unPausedBy;
        bool isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastupdatedDate;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();


        public CheckInPauseLogDTO()
        {
            checkInPauseLogId = -1;
            checkInDetailId = -1;
            isActive = true;
            masterEntityId = -1;
        }

        public CheckInPauseLogDTO(int checkInPauseLogId, int checkInDetailId, DateTime pauseStartTime,
                                    DateTime? pauseEndTime, int totalPauseTime, string pOSMachine,
                                    string pausedBy,string unPausedBy,bool isActive, string createdBy,
                                    DateTime creationDate, string lastUpdatedBy,
                                    DateTime lastupdatedDate, string guid, int siteId, bool synchStatus,
                                    int masterEntityId)
        {
            this.checkInPauseLogId = checkInPauseLogId;
            this.checkInDetailId = checkInDetailId;
            this.pauseStartTime = pauseStartTime;
            this.pauseEndTime = pauseEndTime;
            this.totalPauseTime = totalPauseTime;
            this.pOSMachine = pOSMachine;
            this.pausedBy = pausedBy;
            this.unPausedBy = unPausedBy;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastupdatedDate = lastupdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
        }


        /// <summary>
        /// Get/Set method of the CheckInPauseLogId field
        /// </summary>
        public int CheckInPauseLogId
        {
            get
            {
                return checkInPauseLogId;
            }

            set
            {
                this.IsChanged = true;
                checkInPauseLogId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CheckInDetailId field
        /// </summary>
        public int CheckInDetailId
        {
            get
            {
                return checkInDetailId;
            }

            set
            {
                this.IsChanged = true;
                checkInDetailId = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PauseStartTime field
        /// </summary>
        public DateTime PauseStartTime
        {
            get
            {
                return pauseStartTime;
            }

            set
            {
                this.IsChanged = true;
                pauseStartTime = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PauseEndTime field
        /// </summary>
        public DateTime? PauseEndTime
        {
            get
            {
                return pauseEndTime;
            }

            set
            {
                this.IsChanged = true;
                pauseEndTime = value;
            }
        }
        /// <summary>
        /// Get/Set method of the totalPauseTime field
        /// </summary>
        public int TotalPauseTime
        {
            get
            {
                return totalPauseTime;
            }

            set
            {
                this.IsChanged = true;
                totalPauseTime = value;
            }
        }
        
        /// <summary>
        /// Get/Set method of the POSMachine field
        /// </summary>
        public string POSMachine
        {
            get
            {
                return pOSMachine;
            }

            set
            {
                this.IsChanged = true;
                pOSMachine = value;
            }
        }

        /// <summary>
        /// Get/Set method of the PausedBy field
        /// </summary>
        public string PausedBy
        {
            get
            {
                return pausedBy;
            }

            set
            {
                this.IsChanged = true;
                pausedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UnPausedBy field
        /// </summary>
        public string UnPausedBy
        {
            get
            {
                return unPausedBy;
            }

            set
            {
                this.IsChanged = true;
                unPausedBy = value;
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
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }

            set
            {
                this.IsChanged = true;
                createdBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }

            set
            {
                this.IsChanged = true;
                creationDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate
        {
            get
            {
                return lastupdatedDate;
            }
            set
            {
                this.IsChanged = true;
                lastupdatedDate = value;
            }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get
            {
                return siteId;
            }

            set
            {
                this.IsChanged = true;
                siteId = value;
            }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
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
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }

            set
            {
                this.IsChanged = true;
                synchStatus = value;
            }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
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
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || checkInPauseLogId < 0;
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
            log.LogMethodExit(null);
        }
    }
}
