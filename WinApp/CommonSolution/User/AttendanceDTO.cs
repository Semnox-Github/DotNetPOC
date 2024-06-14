/********************************************************************************************
 * Project Name - Attendance DTO
 * Description  - Data object of Attendance
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018   Indhu                   Created 
 *2.70.2        15-Jul-2019   Girish Kundar           Modified : Added Parametrized Constructor with required fields
 *                                                             And MasterEntityId field.
  *2.80        20-May-2020   Vikas Dwivedi           Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the Attendance data object class. This acts as data holder for the Attendance business object
    /// </summary>
    public class AttendanceDTO
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
            /// Search by AttendanceId field
            /// </summary>
            ATTENDANCE_ID,
            /// <summary>
            /// Search by Attendance Id List field
            /// </summary>
            ATTENDANCE_ID_LIST,
            /// <summary>
            /// Search by UserId field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by Last X days Login
            /// </summary>
            LAST_X_DAYS_LOGIN
        }

       private int attendanceId;
       private int userId;
       private DateTime startDate;
       private int workShiftScheduleId;
       private DateTime workShiftStartTime;
       private double hours;
       private string status;
       private string isActive;
       private DateTime creationDate;
       private string createdBy;
       private DateTime lastUpdateDate;
       private string lastUpdatedBy;
       private int site_id;
       private string GUID;
       private bool synchStatus;
       private int masterEntityId;
        private List<AttendanceLogDTO> attendanceLogDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AttendanceDTO()
        {
            log.LogMethodEntry();
            attendanceId = -1;
            userId = -1;
            masterEntityId = -1;
            isActive = "Y";
            site_id = -1;
            workShiftScheduleId = -1;
            attendanceLogDTOList = new List<AttendanceLogDTO>();
            log.LogMethodExit();
        }


        /// <summary>
        ///  constructor with Required Parameter
        /// </summary>
        public AttendanceDTO(int attendanceId, int userId, DateTime startDate, int workShiftScheduleId,
                             DateTime workShiftStartTime, double hours, string status, string isActive)
            :this()
        {
            log.LogMethodEntry( attendanceId,  userId,  startDate,  workShiftScheduleId,
                                workShiftStartTime,  hours,  status,  isActive);
            this.attendanceId = attendanceId;
            this.userId = userId;
            this.startDate = startDate;
            this.workShiftScheduleId = workShiftScheduleId;
            this.workShiftStartTime = workShiftStartTime;
            this.hours = hours;
            this.status = status;
            this.isActive = isActive;
            log.LogMethodExit();
        }


        public AttendanceDTO(int attendanceId,int userId,DateTime startDate,int workShiftScheduleId,
                             DateTime workShiftStartTime,double hours,string status,string isActive,
                             DateTime creationDate,string createdBy,DateTime lastUpdateDate,string lastUpdatedBy,
                             int site_id,string GUID,bool synchStatus,int masterEntityId)
            :this(attendanceId, userId, startDate, workShiftScheduleId,
                                workShiftStartTime, hours, status, isActive)
        {
            log.LogMethodEntry( attendanceId, userId, startDate, workShiftScheduleId,
                                workShiftStartTime, hours, status, isActive,
                                creationDate,  createdBy,  lastUpdateDate,  lastUpdatedBy,
                                site_id,  GUID,  synchStatus,  masterEntityId);
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.GUID = GUID;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the AttendanceId field
        /// </summary>
        [DisplayName("AttendanceId")]
        [Browsable(false)]
        public int AttendanceId
        {
            get
            {
                return attendanceId;
            }
            set
            {
                attendanceId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the UserId field
        /// </summary>
        [DisplayName("UserId")]
        public int UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
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
        /// Get/Set method of the StartDate field
        /// </summary>
        [DisplayName("Start Date")]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the workShiftScheduleId field
        /// </summary>
        [DisplayName("WorkShiftScheduleId")]
        public int WorkShiftScheduleId
        {
            get
            {
                return workShiftScheduleId;
            }
            set
            {
                workShiftScheduleId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the WorkShiftStartTime field
        /// </summary>
        [DisplayName("WorkShiftStartTime")]
        public DateTime WorkShiftStartTime
        {
            get
            {
                return workShiftStartTime;
            }
            set
            {
                workShiftStartTime = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Hours field
        /// </summary>
        [DisplayName("Hours")]
        public double Hours
        {
            get
            {
                return hours;
            }
            set
            {
                hours = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
        public string IsActive
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
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
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
        /// Get method of the LastUpdatedUser field
        /// </summary>
        [DisplayName("Last Updated User")]
        public string LastUpdatedUser
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
        [DisplayName("Last Updated Date")]
        public DateTime LastUpdatedDate
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
        /// Get method of the SiteId field
        /// </summary>
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return site_id;
            }
            set
            {
                site_id = value;
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
        /// Get method of the SynchStatus field
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
        /// Get method of the Guid field
        /// </summary>
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return GUID;
            }
            set
            {
                Guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the attendanceLogDTOList field
        /// </summary>
        public List<AttendanceLogDTO> AttendanceLogDTOList
        {
            get { return attendanceLogDTOList; }
            set { attendanceLogDTOList = value; }
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
                    return notifyingObjectIsChanged || attendanceId < 0;
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
        /// Returns true or false whether the AttendanceDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (attendanceLogDTOList != null &&
                   attendanceLogDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
