/********************************************************************************************
 * Project Name - AttendanceRoles DTO
 * Description  - Data object of AttendanceRoles
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.23.4      03-Sep-2018   Indhu                   Created 
 *2.70.2        15-Jul -2019   Girish Kundar          Modified : Added Parametrized Constructor with required fields
 *            21-Oct-2019    Jagan Mohana           IsActvie parameter added
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.User
{
    public class AttendanceRoleDTO
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
            /// Search by Id
            /// </summary>
            ID,
            /// <summary>
            /// Search by AttendanceRoleId
            /// </summary>
            ATTENDANCE_ROLE_ID,
            /// <summary>
            /// Search by RoleId
            /// </summary>
            ROLE_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by ISACTIVE field
            /// </summary>
            ISACTIVE,
            /// <summary>
            /// Search by MasterEntityId field
            /// </summary>
            MASTER_ENTITY_ID,
        }

       private int id;
       private int roleId;
       private int attendanceRoleId;
       private bool approvalRequired;
       private bool isActive;
       private DateTime creationDate;
       private string createdBy;
       private DateTime lastUpdateDate;
       private string lastUpdatedBy;
       private int site_id;
       private string GUID;
       private bool synchStatus;
       private int masterEntityId;

        ///<summary>
        ///Default Constructor
        ///</summary>
        public AttendanceRoleDTO()
        {
            log.LogMethodEntry();
            id = -1;
            roleId = -1;
            attendanceRoleId = -1;
            approvalRequired = false;
            masterEntityId = -1;
            site_id = -1;
            isActive = true;
            log.LogMethodExit();
        }

        ///<summary>
        ///Parameterized constructor with required parameters
        ///</summary>
        public AttendanceRoleDTO(int Id, int roleId, int attendanceRoleId, bool approvalRequired,  bool isActive)
            :this()
        {
            log.LogMethodEntry(Id,  roleId,  attendanceRoleId,  approvalRequired, isActive);
            this.id = Id;
            this.roleId = roleId;
            this.attendanceRoleId = attendanceRoleId;
            this.approvalRequired = approvalRequired;
            this.isActive = isActive;
            log.LogMethodExit();
        }



        ///<summary>
        ///Parameterized constructor
        ///</summary>
        public AttendanceRoleDTO(int Id, int roleId, int attendanceRoleId, bool approvalRequired,
                                          bool isActive, DateTime creationDate, string createdBy,
                                          DateTime lastUpdateDate, string lastUpdatedBy, int site_id,
                                          string GUID, bool synchStatus, int masterEntityId)
            :this(Id, roleId, attendanceRoleId, approvalRequired, isActive)
        {
            log.LogMethodEntry(Id, roleId, attendanceRoleId, approvalRequired, isActive,
                               creationDate, createdBy, lastUpdateDate, lastUpdatedBy, site_id, GUID, 
                               synchStatus, masterEntityId);
          
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
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        [Browsable(false)]
        public int Id
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
        /// Get/Set method of the RoleId field
        /// </summary>
        [DisplayName("RoleId")]
        public int RoleId
        {
            get
            {
                return roleId;
            }
            set
            {
                roleId = value;
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
        /// Get/Set method of the ApprovalRequired field
        /// </summary>
        [DisplayName("ApprovalRequired")]
        public bool ApprovalRequired
        {
            get
            {
                return approvalRequired;
            }
            set
            {
                approvalRequired = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Is Active")]
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
