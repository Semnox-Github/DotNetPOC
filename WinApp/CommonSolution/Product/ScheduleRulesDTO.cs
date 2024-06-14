/* Project Name - Semnox.Parafait.Booking.ScheduleRulesDTO 
* Description  - Data object of the AttractionScheduleRules
* 
**************
**Version Log
**************
*Version     Date           Modified By             Remarks          
********************************************************************************************* 
*2.50        26-Nov-2018    Guru S A             Created for Booking enhancement changes 
*2.60        18-Feb-2019    Nagesh Badiger       Added is Active property 
*2.70        14-Mar-2019    Guru S A             Booking phase 2 enhancement changes 
*2.80.0      21-02-2020     Girish Kundar        Modified : 3 tier Changes for REST API
********************************************************************************************/

using System;
using System.ComponentModel;


namespace Semnox.Parafait.Product
{

    /// <summary>
    /// ScheduleRulesDTO Class
    /// </summary>
    public class ScheduleRulesDTO
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
            SCHEDULE_RULE_ID,
            /// <summary>
            /// Search by  ScheduleId field
            /// </summary>
            SCHEDULE_ID,
            /// <summary>
            /// Search by  ScheduleId field
            /// </summary>
            SCHEDULE_ID_LIST,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
            ///// <summary>
            ///// Search by FACILITY_ID field
            ///// </summary>
            //FACILITY_ID,
            /// <summary>
            /// Search by FACILITY_MAP_ID field
            /// </summary>
            FACILITY_MAP_ID,
            /// <summary>
            /// Search by MASTER_SCHEDULE_ID field
            /// </summary>
            MASTER_SCHEDULE_ID,
            /// <summary>
            /// Search by IS_ACTIVE field
            /// </summary>
            IS_ACTIVE
        }

        private int scheduleRulesId;
        private int scheduleId;
        private decimal? day;
        private DateTime? fromDate;
        private DateTime? toDate;
        private int? units;
        //private decimal? price;
        private int siteId;
        private string guid;
        private bool synchStatus;
        //private int productId;
        //private int facilityId;
        private int facilityMapId;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private bool isActive;


        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScheduleRulesDTO()
        {
            log.LogMethodEntry();
            scheduleRulesId = -1;
            scheduleId = -1;
            siteId = -1;
            //facilityId = -1;
            facilityMapId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ScheduleRulesDTO(int scheduleRulesId, int scheduleId, decimal? day, DateTime? fromDate, DateTime? toDate,
                                int? units, bool isActive, int facilityMapId = -1)
            : this()
        {
            log.LogMethodEntry(scheduleRulesId, scheduleId, day, fromDate, toDate, units, isActive, facilityMapId);
            this.scheduleRulesId = scheduleRulesId;
            this.scheduleId = scheduleId;
            this.day = day;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.units = units;
            this.isActive = isActive;
            this.facilityMapId = facilityMapId;
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ScheduleRulesDTO(int scheduleRulesId, int scheduleId, decimal? day, DateTime? fromDate, DateTime? toDate, int? units, int siteId,
                                string guid, bool synchStatus, int masterEntityId, string createdBy, DateTime creationDate,
                                string lastUpdatedBy, DateTime lastUpdateDate, int facilityMapId, bool isActive)
            : this(scheduleRulesId, scheduleId, day, fromDate, toDate, units, isActive, facilityMapId)
        {
            log.LogMethodEntry(scheduleRulesId, scheduleId, day, fromDate, toDate, units, siteId, guid, synchStatus, masterEntityId, createdBy,
                               creationDate, lastUpdatedBy, lastUpdateDate, facilityMapId, isActive);
            this.siteId = siteId;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the ScheduleRulesId field
        /// </summary>
        [DisplayName("Schedule RulesId")]
        public int ScheduleRulesId { get { return scheduleRulesId; } set { scheduleRulesId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ScheduleId field
        /// </summary>
        [DisplayName("ScheduleId")]
        public int ScheduleId { get { return scheduleId; } set { scheduleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the day field
        /// </summary>
        public decimal? Day
        {
            get { return day; }
            set
            {
                if (value <= 999999999999999999 || value == null) //for 18 
                {
                    day = value;
                    this.IsChanged = true;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        /// Get method of the FromDate field
        /// </summary>
        [DisplayName("From Date")]
        public DateTime? FromDate
        {
            get
            {
                return fromDate;
            }
            set { fromDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the ToDate field
        /// </summary>
        [DisplayName("To Date")]
        public DateTime? ToDate
        {
            get
            {
                return toDate;
            }
            set { toDate = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Units field
        /// </summary>
        [DisplayName("Units")]
        public int? Units { get { return units; } set { units = value; this.IsChanged = true; } }

        ///// <summary>
        ///// Get/Set method of the facilityId field
        ///// </summary>
        //[DisplayName("facility Id")]
        //public int FacilityId { get { return facilityId; } set { facilityId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the FacilityMapId field
        /// </summary>
        [DisplayName("Facility Map Id")]
        public int FacilityMapId { get { return facilityMapId; } set { facilityMapId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("Site Id")]
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
        [DisplayName("Master Entity Id")]
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
        /// Get method of the IsActive field
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
                    return notifyingObjectIsChanged || scheduleRulesId < 0;
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
