/* Project Name - Semnox.Parafait.Booking.MasterScheduleDTO 
* Description  - Data object of the AttractionMasterSchedule
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.70        26-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
*2.80.0      21-02-2020    Girish Kundar       Modified : 3 tier Changes for REST API
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Semnox.Parafait.Product
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
            MASTER_SCHEDULE_ID,
            /// <summary>
            /// Search by  MasterScheduleId field
            /// </summary>
            MASTER_SCHEDULE_ID_LIST,
            /// <summary>
            /// Search by  ActiveFlag field
            /// </summary>
            ACTIVE_FLAG,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
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
        private List<FacilityMapDTO> facilityMapDTOList;
        private List<SchedulesDTO> schedulesDTOList;
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
            siteId = -1;
            activeFlag = true;
            facilityMapDTOList = new List<FacilityMapDTO>();
            schedulesDTOList = new List<SchedulesDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with business fields
        /// </summary>
        public MasterScheduleDTO(int masterScheduleId, string masterScheduleName, bool activeFlag)
            : this()
        {
            log.LogMethodEntry(masterScheduleId, masterScheduleName, activeFlag, schedulesDTOList);
            this.masterScheduleId = masterScheduleId;
            this.masterScheduleName = masterScheduleName;
            this.activeFlag = activeFlag;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MasterScheduleDTO(int masterScheduleId, string masterScheduleName, bool activeFlag,
                                     string guid, int siteId, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdateDate)
            : this(masterScheduleId, masterScheduleName, activeFlag)

        {
            log.LogMethodEntry(masterScheduleId, masterScheduleName, activeFlag,
                                      guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, lastUpdatedBy, lastUpdateDate);
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
        public bool ActiveFlag { get { return activeFlag; } set { activeFlag = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

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
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }

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
            set { createdBy = value; }
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
            set { creationDate = value; }
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
            set { lastUpdatedBy = value; }
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
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// Get/Set method of the FacilityMapDTOList field
        /// </summary>
        [DisplayName("Facility Map DTO List")]
        public List<FacilityMapDTO> FacilityMapDTOList { get { return facilityMapDTOList; } set { facilityMapDTOList = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SchedulesDTOList field
        /// </summary>
        [DisplayName("Schedules DTO List")]
        public List<SchedulesDTO> SchedulesDTOList { get { return schedulesDTOList; } set { schedulesDTOList = value; this.IsChanged = true; } }

        /// <summary>
        /// Returns whether the Master SchedulesDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (schedulesDTOList != null &&
                    schedulesDTOList.Any(x => x.IsChangedRecursive))
                {
                    return true;
                }
                return false;
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
                    return notifyingObjectIsChanged || masterScheduleId < 0;
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
