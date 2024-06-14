/********************************************************************************************
 * Project Name - User - LeaveActivity
 * Description  - DataObject
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *2.80        20-May-2020      Vikas Dwivedi       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public class LeaveTypeBalanceDTO
    {
        string leaveType;
        decimal balance;
        public string LeaveType { get { return leaveType; } set { leaveType = value; } }
        public decimal Balance { get { return balance; } set { balance = value; } }
    }
    public class LeaveActivityDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum SearchByLeaveActivityParameter
        {
            /// <summary>
            /// Search by UserId field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,
        }

        private int userId;
        private int site_id;
        private List<UsersDTO> usersDTOList;
        private List<UserRolesDTO> userRolesDTOList;
        private List<DepartmentDTO> departmentDTOList;
        private List<LeaveDTO> leaveDTOList;
        private List<LeaveTypeBalanceDTO> leaveTypeBalanceDTOList;
        
        public LeaveActivityDTO()
        {
            log.LogMethodEntry();
            userId = -1;
            usersDTOList = new List<UsersDTO>();
            userRolesDTOList = new List<UserRolesDTO>();
            departmentDTOList = new List<DepartmentDTO>();
            leaveDTOList = new List<LeaveDTO>();
            leaveTypeBalanceDTOList = new List<LeaveTypeBalanceDTO>();
            log.LogMethodExit(null);
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
        /// Get/Set method of the usersDTOList field
        /// </summary>
        public List<UsersDTO> UsersDTOList
        {
            get { return usersDTOList; }
            set { usersDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the userRolesDTOList field
        /// </summary>
        public List<UserRolesDTO> UserRolesDTOList
        {
            get { return userRolesDTOList; }
            set { userRolesDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the leaveDTOList field
        /// </summary>
        public List<LeaveDTO> LeaveDTOList
        {
            get { return leaveDTOList; }
            set { leaveDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the attendanceLogDTOList field
        /// </summary>
        public List<DepartmentDTO> DepartmentDTOList
        {
            get { return departmentDTOList; }
            set { departmentDTOList = value; }
        }

        /// <summary>
        /// Get/Set method of the attendanceLogDTOList field
        /// </summary>
        public List<LeaveTypeBalanceDTO> LeaveTypeBalanceDTOList
        {
            get { return leaveTypeBalanceDTOList; }
            set { leaveTypeBalanceDTOList = value; }
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
                    return notifyingObjectIsChanged || userId < 0;
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
                if (usersDTOList != null &&
                   usersDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (departmentDTOList != null &&
                   departmentDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (userRolesDTOList != null &&
                   userRolesDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                if (leaveDTOList != null &&
                   leaveDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
