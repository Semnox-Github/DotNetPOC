/********************************************************************************************
 * Project Name - User
 * Description  - Data object of Leave
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
    /// This is the Leave data object class. This acts as data holder for the Leave business object
    /// </summary>
    public class LeaveDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by  LEAVE ID field
            /// </summary>
            LEAVE_ID,
            /// <summary>
            /// Search by  LEAVE CYCLE field
            /// </summary>
            LEAVE_CYCLE_ID,
            /// <summary>
            /// Search by  USER ID field
            /// </summary>
            USER_ID,
            /// <summary>
            /// Search by  LEAVE TYPE ID field
            /// </summary>
            LEAVE_TYPE_ID,
            /// <summary>
            /// Search by   TYPE ID field
            /// </summary>
            TYPE,
            /// <summary>
            /// Search by  LEAVE TEMPLATE ID field
            /// </summary>
            LEAVE_TEMPLATE_ID,
            /// <summary>
            /// Search by  APPROVED BY field
            /// </summary>
            APPROVED_BY,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by IS ACTIVE field
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by START DATE field
            /// </summary>
            START_DATE,
            /// <summary>
            /// Search by END DATE field
            /// </summary>
            END_DATE,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int leaveId;
        private int leaveCycleId;
        private int userId;
        private int leaveTypeId;
        private string type;
        private int leaveTemplateId;
        private DateTime startDate;
        private string starthalf;
        private DateTime? endDate;
        private string endhalf;
        private decimal leaveDays;
        private string leaveStatus;
        private string description;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int approvedBy;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private bool isActive;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LeaveDTO()
        {
            log.LogMethodEntry();
            leaveId = -1;
            leaveCycleId = -1;
            userId = -1;
            leaveTypeId = -1;
            leaveTemplateId = -1;
            approvedBy = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with required fields.
        /// </summary>
       
        public LeaveDTO(int leaveId,int leaveCycleId,int userId,int leaveTypeId,string type,int leaveTemplateId,
                        DateTime startDate,string starthalf,DateTime? endDate,string endhalf,decimal leaveDays, string leaveStatus,
                        string description,int approvedBy) 
            : this()
        {
            log.LogMethodEntry(leaveId, leaveCycleId, userId, leaveTypeId, type, leaveTemplateId,startDate, starthalf,
                            endDate, endhalf, leaveDays, leaveStatus,description, approvedBy, guid,  siteId,synchStatus, masterEntityId);
            this.leaveId = leaveId;
            this.leaveCycleId = leaveCycleId;
            this.userId = userId;
            this.leaveTypeId = leaveTypeId;
            this.type = type;
            this.leaveTemplateId = leaveTemplateId;
            this.startDate = startDate;
            this.starthalf = starthalf;
            this.endDate = endDate;
            this.endhalf = endhalf;
            this.leaveDays = leaveDays;
            this.leaveStatus = leaveStatus;
            this.description = description;
            this.approvedBy = approvedBy;
            log.LogMethodExit();
        }


        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
       
        public LeaveDTO(int leaveId,int leaveCycleId,int userId,int leaveTypeId,string type,int leaveTemplateId,
                        DateTime startDate,string starthalf,DateTime? endDate,string endhalf,decimal leaveDays, string leaveStatus,
                        string description,DateTime lastUpdatedDate,string lastUpdatedBy,int approvedBy,string guid, int siteId,
                        bool synchStatus,int masterEntityId,string createdBy,DateTime creationDate, bool isActive)
            : this(leaveId, leaveCycleId, userId, leaveTypeId, type, leaveTemplateId, startDate, starthalf,
                            endDate, endhalf, leaveDays, leaveStatus, description, approvedBy)
        {
            log.LogMethodEntry(leaveId, leaveCycleId, userId, leaveTypeId, type, leaveTemplateId, startDate, starthalf,
                               endDate, endhalf, leaveDays, leaveStatus, description, lastUpdatedDate, lastUpdatedBy,
                               approvedBy, guid, siteId, synchStatus, masterEntityId, createdBy, creationDate);            
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.lastUpdatedDate = lastUpdatedDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            log.LogMethodExit();
        }



        public LeaveDTO(int Id, int user_Id, string type, int leaveTypeId, string starthalf, DateTime? endDate, string endhalf, decimal leaveDays, string status,
                        string description, DateTime startDate)
            : this()
        {
            log.LogMethodEntry();
            this.leaveId = Id;            
            this.userId = user_Id;
            this.type = type;
            this.leaveTypeId = leaveTypeId;
            this.starthalf = starthalf;
            this.endDate = endDate;
            this.endhalf = endhalf;
            this.leaveDays = leaveDays;
            this.leaveStatus = status;
            this.description = description;
            this.startDate = startDate;
            log.LogMethodExit();
        }

        public LeaveDTO(string lookupValue, decimal leaveDays)
            : this()
        {
            log.LogMethodEntry();            
            this.type = lookupValue;
            this.leaveDays = leaveDays;            
            log.LogMethodExit();
        }

        public LeaveDTO(int leaveId, string leaveType, string type, decimal leaveDays, string leaveStatus , DateTime startDate,
            string starthalf, DateTime? endDate, string endhalf)
            : this()
        {
            log.LogMethodEntry();
            this.leaveId = leaveId;
            this.type = type;
            this.leaveDays = leaveDays;
            this.leaveStatus = leaveStatus;
            this.startDate = startDate;
            this.starthalf = starthalf;
            this.endDate = endDate;
            this.endhalf = endhalf;                                   
            log.LogMethodExit();
        }

        public LeaveDTO(string empName, string leaveType, decimal leaveDays, string leaveStatus, DateTime startDate, string type)
         : this()
        {
            log.LogMethodEntry();
            this.type = type;
            this.leaveDays = leaveDays;
            this.leaveStatus = leaveStatus;
            this.startDate = startDate;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the LeaveId  field
        /// </summary>
        public int LeaveId
        {
            get { return leaveId; }
            set { leaveId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the LeaveCycleId  field
        /// </summary>
        public int LeaveCycleId
        {
            get { return leaveCycleId; }
            set { leaveCycleId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the UserId  field
        /// </summary>
        public int UserId
        {
            get { return userId; }
            set { userId = value; this.IsChanged = true; }
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
        /// Get/Set method of the Type  field
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = value; this.IsChanged = true; }
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
        /// Get/Set method of the StartDate  field
        /// </summary>
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the StartHalf field
        /// </summary>
        public string Starthalf
        {
            get { return starthalf; }
            set { starthalf = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EndDate  field
        /// </summary>
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the EndHalf field
        /// </summary>
        public string Endhalf
        {
            get { return endhalf; }
            set { endhalf = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LeaveDays field
        /// </summary>
        public decimal LeaveDays
        {
            get { return leaveDays; }
            set { leaveDays = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LeaveStatus field
        /// </summary>
        public string LeaveStatus
        {
            get { return leaveStatus; }
            set { leaveStatus = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the ApprovedBy field
        /// </summary>
        public int ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; this.IsChanged = true; }
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
                    return notifyingObjectIsChanged || leaveId < 0;
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
