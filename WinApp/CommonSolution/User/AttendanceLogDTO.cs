/********************************************************************************************
 * Project Name - AttendanceLog DTO
 * Description  - Data object of AttendanceLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018   Indhu                   Created 
 *2.70.2      15-Jul-2019   Girish Kundar           Modified : Added MasterEntityId field to the constructor 
*                                                              and LogMethodEntry() and LogMaethodExit().
*2.80        20-May-2020    Vikas Dwivedi           Modified as per the Standard CheckList
*2.110.0     07-Jan-2021    Deeksha                 Modified to add additional field as part of Attendance & PayRate enhancement.
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the AttendanceLog data object class. This acts as data holder for the AttendanceLog business object
    /// </summary>
    public class AttendanceLogDTO
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
            /// Search by AttendanceLogId
            /// </summary>
            ATTENDANCE_LOG_ID,
            /// <summary>
            /// Search by AttendanceId
            /// </summary>
            ATTENDANCE_ID,
            /// <summary>
            /// Search by AttendanceId List 
            /// </summary>
            ATTENDANCE_ID_LIST,
            /// <summary>
            /// Search by AttendanceRoleApproverId
            /// </summary>
            ATTENDANCE_ROLE_APPROVER_ID,
            /// <summary>
            /// Search by POSMachineId
            /// </summary>
            POS_MACHINE_ID,
            /// <summary>
            /// Search by IsActive field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by CardNumber field
            /// </summary>
            CARD_NUMBER,
            /// <summary>
            /// Search by TimeStamp field
            /// </summary>
            TIMESTAMP,
            /// <summary>
            /// Search by FromDate field
            /// </summary>
            FROM_DATE,
            /// <summary>
            /// Search by ToDate field
            /// </summary>
            TO_DATE,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
            ///<summary>
            ///Search by user_id
            ///</summary>
            USER_ID,
            ///<summary>
            ///Search by Role_ID
            ///</summary>
            ATTENDANCE_ROLE_ID
        }

        private int id;
        private string cardNumber;
        private int readerId;
        private DateTime timestamp;
        private string type;
        private int attendanceId;
        private string mode;
        private int attendanceRoleId;
        private int attendanceRoleApproverId;
        private string status;
        private int machineId;
        private int pOSMachineId;
        private double tipValue;
        private string isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string GUID;
        private bool synchStatus;
        private int masterEntityId;
        private int originalAttendanceLogId;
        private string requestStatus;
        private string approvedBy;
        private DateTime? approvalDate;

        /// <summary>
        /// Default Constructor for AttendanceLogDTO
        /// </summary>
        public AttendanceLogDTO()
        {
            log.LogMethodEntry();
            id = -1;
            attendanceId = -1;
            pOSMachineId = -1;
            machineId = -1;
            masterEntityId = -1;
            site_id = -1;
            readerId = -1;
            attendanceRoleId = -1;
            attendanceRoleApproverId = -1;
            isActive = "Y";
            approvedBy = null;
            originalAttendanceLogId = -1;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized  Constructor with required fields for AttendanceLogDTO 
        /// </summary>

        public AttendanceLogDTO(int id, string cardNumber, int readerId, DateTime timestamp, string type,
                                int attendanceId, string mode, int attendanceRoleId, int attendanceRoleApproverId,
                                string status, int machineId, int pOSMachineId, double tipValue, string isActive,
                                int originalAttendanceLogId, string requestStatus, string approvedBy, DateTime? approvalDate)
            : this()
        {
            log.LogMethodEntry(id, cardNumber, readerId, timestamp, type,
                                 attendanceId, mode, attendanceRoleId, attendanceRoleApproverId,
                                 status, machineId, pOSMachineId, tipValue, isActive,
                                 originalAttendanceLogId, requestStatus, approvedBy, approvalDate);
            this.id = id;
            this.cardNumber = cardNumber;
            this.readerId = readerId;
            this.timestamp = timestamp;
            this.type = type;
            this.attendanceId = attendanceId;
            this.mode = mode;
            this.attendanceRoleId = attendanceRoleId;
            this.attendanceRoleApproverId = attendanceRoleApproverId;
            this.status = status;
            this.machineId = machineId;
            this.pOSMachineId = pOSMachineId;
            this.tipValue = tipValue;
            this.isActive = isActive;
            this.originalAttendanceLogId = originalAttendanceLogId;
            this.requestStatus = requestStatus;
            this.approvedBy = approvedBy;
            this.approvalDate = approvalDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized  Constructor for AttendanceLogDTO 
        /// </summary>

        public AttendanceLogDTO(int id, string cardNumber, int readerId, DateTime timestamp, string type,
                                int attendanceId, string mode, int attendanceRoleId, int attendanceRoleApproverId,
                                string status, int machineId, int pOSMachineId, double tipValue, string isActive,
                                DateTime creationDate, string createdBy, DateTime lastUpdateDate, string lastUpdatedBy,
                                int site_id, string GUID, bool synchStatus, int masterEntityId,
                                int originalAttendanceLogId, string requestStatus, string approvedBy, DateTime approvalDate)
            : this(id, cardNumber, readerId, timestamp, type, attendanceId, mode, attendanceRoleId,
                  attendanceRoleApproverId, status, machineId, pOSMachineId, tipValue, isActive, originalAttendanceLogId,
                   requestStatus, approvedBy, approvalDate)
        {
            log.LogMethodEntry(id, cardNumber, readerId, timestamp, type,
                                 attendanceId, mode, attendanceRoleId, attendanceRoleApproverId,
                                 status, machineId, pOSMachineId, tipValue, isActive,
                                 creationDate, createdBy, lastUpdateDate, lastUpdatedBy,
                                 site_id, GUID, synchStatus, masterEntityId,
                                 originalAttendanceLogId, requestStatus, approvedBy, approvalDate);

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
        /// Get/Set method of the AttendanceLogId field
        /// </summary>
        [DisplayName("AttendanceLogId")]
        [ReadOnly(false)]
        public int AttendanceLogId
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the AttendanceId field
        /// </summary>
        [DisplayName("AttendanceId")]
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
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("CardNumber")]
        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ReaderId field
        /// </summary>
        [DisplayName("ReaderId")]
        public int ReaderId
        {
            get
            {
                return readerId;
            }
            set
            {
                readerId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Timestamp field
        /// </summary>
        [DisplayName("Timestamp")]
        public DateTime Timestamp
        {
            get
            {
                return timestamp;
            }
            set
            {
                timestamp = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of Mode Status field
        /// </summary>
        [DisplayName("Mode")]
        public string Mode
        {
            get
            {
                return mode;
            }
            set
            {
                mode = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the AttendanceRoleId field
        /// </summary>
        [DisplayName("AttendanceRoleId")]
        public int AttendanceRoleId
        {
            get
            {
                return attendanceRoleId;
            }
            set
            {
                attendanceRoleId = value;
                this.IsChanged = true;
            }
        }


        /// <summary>
        /// Get/Set method of the AttendanceRoleApproverId field
        /// </summary>
        [DisplayName("AttendanceRoleApproverId")]
        public int AttendanceRoleApproverId
        {
            get
            {
                return attendanceRoleApproverId;
            }
            set
            {
                attendanceRoleApproverId = value;
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
        /// Get/Set method of the MachineId field
        /// </summary>
        [DisplayName("MachineId")]
        public int MachineId
        {
            get
            {
                return machineId;
            }
            set
            {
                machineId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the POSMachineId field
        /// </summary>
        [DisplayName("POSMachineId")]
        public int POSMachineId
        {
            get
            {
                return pOSMachineId;
            }
            set
            {
                pOSMachineId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the TipValue field
        /// </summary>
        [DisplayName("TipValue")]
        public double TipValue
        {
            get
            {
                return tipValue;
            }
            set
            {
                tipValue = value;
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
                GUID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OriginalAttendanceLogId field
        /// </summary>
        [DisplayName("OriginalAttendanceLogId")]
        public int OriginalAttendanceLogId
        {
            get
            {
                return originalAttendanceLogId;
            }
            set
            {
                originalAttendanceLogId = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the RequestStatus field
        /// </summary>
        [DisplayName("RequestStatus")]
        public string RequestStatus
        {
            get
            {
                return requestStatus;
            }
            set
            {
                requestStatus = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ApprovedBy field
        /// </summary>
        [DisplayName("ApprovedBy")]
        public string ApprovedBy
        {
            get
            {
                return approvedBy;
            }
            set
            {
                approvedBy = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the ApprovalDate field
        /// </summary>
        [DisplayName("ApprovalDate")]
        public DateTime? ApprovalDate
        {
            get
            {
                return approvalDate;
            }
            set
            {
                approvalDate = value;
                this.IsChanged = true;
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
