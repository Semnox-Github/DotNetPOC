/********************************************************************************************
 * Project Name - User
 * Description  - Data object of LeaveTemplate
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70        28-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;

namespace Semnox.Parafait.User
{
    /// <summary>
    /// This is the LeaveTemplate data object class. This acts as data holder for the LeaveTemplate business object
    /// </summary>
    public class LeaveTemplateDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by  LEAVE TEMPLATE ID field
            /// </summary>
            LEAVE_TEMPLATE_ID,
            /// <summary>
            /// Search by  DEPARTMENT field
            /// </summary>
            DEPARTMENT_ID,
            /// <summary>
            /// Search by  ROLE ID field
            /// </summary>
            ROLE_ID,
            /// <summary>
            /// Search by  LEAVE TYPE ID field
            /// </summary>
            LEAVE_TYPE_ID,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID,
            /// <summary>
            /// Search by IS_ACTIVEfield
            /// </summary>
            IS_ACTIVE
        }

        private int leaveTemplateId;
        private int departmentId;
        private int roleId;
        private string frequency;
        private int leaveDays;
        private int leaveTypeId;
        private DateTime effectiveDate;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool notifyingObjectIsChanged;
        private bool isActive;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LeaveTemplateDTO()
        {
            log.LogMethodEntry();
            leaveTemplateId = -1;
            departmentId = -1;
            roleId = -1;
            leaveTypeId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields
        /// </summary>
        public LeaveTemplateDTO(int leaveTemplateId,int departmentId,int roleId,string frequency, int leaveDays,int leaveTypeId,
                                DateTime effectiveDate,bool isActive)
            : this()
        {
            log.LogMethodEntry(leaveTemplateId, departmentId, roleId, frequency, leaveDays, leaveTypeId, effectiveDate, isActive);
            this.leaveTemplateId = leaveTemplateId;
            this.departmentId = departmentId;
            this.roleId = roleId;
            this.frequency = frequency;
            this.leaveDays = leaveDays;
            this.leaveTypeId = leaveTypeId;
            this.effectiveDate = effectiveDate;
            this.isActive = isActive; 
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public LeaveTemplateDTO(int leaveTemplateId,int departmentId,int roleId,string frequency, int leaveDays,int leaveTypeId,
                                DateTime effectiveDate,DateTime lastUpdatedDate, string lastUpdatedBy,string guid, int siteId,bool synchStatus,
                                int masterEntityId, string createdBy, DateTime creationDate, bool isActive)
            : this(leaveTemplateId, departmentId, roleId, frequency, leaveDays, leaveTypeId, effectiveDate,isActive)
        {
            log.LogMethodEntry(leaveTemplateId, departmentId, roleId, frequency, leaveDays, leaveTypeId,effectiveDate, 
                               lastUpdatedDate, lastUpdatedBy, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate, isActive);
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the LeaveTemplateId  field
        /// </summary>
        public int LeaveTemplateId
        {
            get { return leaveTemplateId; }
            set { leaveTemplateId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the DepartmentId  field
        /// </summary>
        public int DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the RoleId  field
        /// </summary>
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Frequency  field
        /// </summary>
        public string Frequency
        {
            get { return frequency; }
            set { frequency = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LeaveDays  field
        /// </summary>
        public int LeaveDays
        {
            get { return leaveDays; }
            set { leaveDays = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LeaveTypeId  field
        /// </summary>
        public int LeaveTypeId
        {
            get { return leaveTypeId; }
            set { leaveTypeId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the EffectiveDate  field
        /// </summary>
        public DateTime EffectiveDate
        {
            get { return effectiveDate; }
            set { effectiveDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value; }
        }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || leaveTemplateId < 0;
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
            IsChanged = false;
            log.LogMethodExit();
        }
    }
}
