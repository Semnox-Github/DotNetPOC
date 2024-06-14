/********************************************************************************************
 * Project Name - User Job Items DTO
 * Description  - Data object of User job items
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        24-Dec-2015   Raghuveera     Created
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera     Modified 
 *2.70        08-Mar-2019   Guru S A       Renamed MaintenanceJobDTO as UserJobItemsDTO
 *2.70.2        08-Mar-2019   Guru S A       Waiver phase 2 changes
 *2.100.0     24-Sept-2020  Mushahid Faizan     Modified for Service Request enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the User job items data object class. This acts as data holder for the user job items business object
    /// </summary>
    public class UserJobItemsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// SearchByUserJobItemsParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByUserJobItemsParameters
        {
            /// <summary>
            /// Search by MAINT_JOB_ID field
            /// </summary>
            USER_JOB_ID = 0,
            /// <summary>
            /// Search by JOB_NAME field
            /// </summary>
            JOB_NAME = 1,
            /// <summary>
            /// Search by ASSET_ID field
            /// </summary>
            ASSET_ID = 2,
            /// <summary>
            /// Search by ASSET_NAME field
            /// </summary>
            ASSET_NAME = 3,
            /// <summary>
            /// Search by STATUS field
            /// </summary>
            STATUS = 4,
            /// <summary>
            /// Search by TASK_ID field
            /// </summary>
            TASK_ID = 5,
            /// <summary>
            /// Search by TASK_NAME field
            /// </summary>
            TASK_NAME = 6,
            /// <summary>
            /// Search by ASSIGNED_TO field
            /// </summary>
            ASSIGNED_TO = 7,
            /// <summary>
            /// Search by SCHEDULE_FROM_DATE field
            /// </summary>
            SCHEDULE_FROM_DATE = 8,
            /// <summary>
            /// Search by SCHEDULE_TO_DATE field
            /// </summary>
            SCHEDULE_TO_DATE = 9,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            IS_ACTIVE = 10,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            PAST_DUE_DATE = 11,
            /// <summary>
            /// Search by MAINTSCHEDULEID field
            /// </summary>
            JOB_SCHEDULE_ID = 12,
            /// <summary>
            /// Search by LAST_UPDATED_DATE field
            /// </summary>
            LAST_UPDATED_DATE=13,
            /// <summary>
            /// Search by REQUEST_TYPE_ID field
            /// </summary>
            REQUEST_TYPE_ID = 14,
            /// <summary>
            /// Search by PRIORITY field
            /// </summary>
            PRIORITY = 15,
            /// <summary>
            /// Search by REQUEST_FROM_DATE field
            /// </summary>
            REQUEST_FROM_DATE=16,
            /// <summary>
            /// Search by REQUEST_TO_DATE field
            /// </summary>
            REQUEST_TO_DATE=17,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 18,
            /// <summary>
            /// Search by REQUESTED_BY field
            /// </summary>
            REQUESTED_BY = 19,
            /// <summary>
            /// Search by JOB_TYPE field
            /// </summary>
            JOB_TYPE = 20,
            /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 21,//Ends:Modification on 18-Jul-2016 for publish feature
            /// <summary>
            /// Search by JOB_TYPE field
            /// </summary>
            JOB_NUMBER = 22
        }

        private int maintChklstdetId;
        private int jobScheduleId;
        private int jobTaskId;
        private string maintJobName;
        private int maintJobType;
        private string chklstScheduleTime;
        private string assignedTo;
        private int assignedUserId;
        private int departmentId;
        private int status;
        private string checklistCloseDate;
        private string taskName;
        private bool validateTag;
        private string cardNumber;
        private int cardId;
        private string taskCardNumber;
        private bool remarksMandatory;
        private int assetId;
        private string assetName;
        private string assetType;
        private string assetGroupName;
        private bool chklistValue;
        private string chklistRemarks;
        private string sourceSystemId;
        private int durationToComplete;
        private int requestType;
        private string requestDate;
        private int priority;
        private string requestDetail;
        private string imageName;
        private string requestedBy;
        private string contactPhone;
        private string contactEmailId;
        private string resolution;
        private string comments;
        private double repairCost;
        private string docFileName;
        private string attribute1;
        private bool isActive;
        private string createdBy;
        private string creationDate;
        private string lastUpdatedBy;
        private string lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private int bookingId;
        private int bookingCheckListId;
        private int jobScheduleTaskId;
        private string maintJobNumber;
        private string saveType;


        private List<MaintenanceJobStatusDTO> maintenanceJobStatusDTOList;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserJobItemsDTO()
        {
            log.LogMethodEntry();
            maintChklstdetId = -1;
            jobTaskId = -1;
            jobScheduleId = -1;
            maintJobType = -1;
            departmentId = -1;            
            requestType = -1;
            priority = -1;
            status = -1;
            cardId = -1;
            assetId = -1;
            status = -1;
            repairCost = -1;
            assignedUserId = -1;
            isActive = true;
            siteId = -1;
            validateTag = chklistValue = remarksMandatory = false;
            masterEntityId = -1;
            bookingId = -1;
            bookingCheckListId = -1;
            jobScheduleTaskId = -1;
            maintJobNumber = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public UserJobItemsDTO(int maintChklstdetId, int jobScheduleId, int jobTaskId, string maintJobName, int maintJobType, string chklstScheduleTime, string assignedTo,
                                 int assignedUserId, int departmentId, int status, string checklistCloseDate, string taskName, bool validateTag,
                                 string cardNumber, int cardId, string taskCardNumber, bool remarksMandatory, int assetId, string assetName, string assetType, string assetGroupName,
                                 bool chklistValue, string chklistRemarks, string sourceSystemId, int durationToComplete, int requestType, string requestDate, int priority,
                                 string requestDetail, string imageName, string requestedBy, string contactPhone, string contactEmailId, string resolution, string comments,
                                 double repairCost, string docFileName, string attribute1, bool isActive, string createdBy, string creationDate, string lastUpdatedBy,
                                 string lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId, int bookingId, int bookingCheckListId, int jobScheduleTaskId, string maintJobNumber)  
        {
            log.LogMethodEntry(maintChklstdetId, jobScheduleId, jobTaskId, maintJobName, maintJobType, chklstScheduleTime, assignedTo,
                                  assignedUserId, departmentId, status, checklistCloseDate, taskName, validateTag,
                                  cardNumber,  cardId, taskCardNumber, remarksMandatory, assetId, assetName, assetType, assetGroupName,
                                  chklistValue, chklistRemarks, sourceSystemId, durationToComplete, requestType, requestDate, priority,
                                 requestDetail, imageName, requestedBy, contactPhone, contactEmailId, resolution, comments,
                                 repairCost, docFileName, attribute1, isActive, createdBy, creationDate, lastUpdatedBy,
                                 lastUpdatedDate, guid, siteId, synchStatus,  masterEntityId, bookingId, bookingCheckListId, jobScheduleTaskId, maintJobNumber);

            this.maintChklstdetId = maintChklstdetId;
            this.jobScheduleId = jobScheduleId;
            this.jobTaskId = jobTaskId;
            this.maintJobName = maintJobName;
            this.maintJobType = maintJobType;
            this.chklstScheduleTime = chklstScheduleTime;
            this.assignedTo = assignedTo;
            this.assignedUserId = assignedUserId;
            this.departmentId = departmentId;
            this.status = status;
            this.checklistCloseDate = checklistCloseDate;
            this.taskName = taskName;
            this.validateTag = validateTag;
            this.cardNumber = cardNumber;
            this.cardId = cardId;
            this.taskCardNumber = taskCardNumber;
            this.remarksMandatory = remarksMandatory;
            this.assetId = assetId;
            this.assetName = assetName;
            this.assetType = assetType;
            this.assetGroupName = assetGroupName;
            this.chklistValue = chklistValue;
            this.chklistRemarks = chklistRemarks;
            this.sourceSystemId = sourceSystemId;
            this.durationToComplete = durationToComplete;
            this.requestType = requestType;
            this.requestDate = requestDate;
            this.priority = priority;
            this.requestDetail = requestDetail;
            this.imageName = imageName;
            this.requestedBy = requestedBy;
            this.contactPhone = contactPhone;
            this.contactEmailId = contactEmailId;
            this.resolution = resolution;
            this.comments = comments;
            this.repairCost = repairCost;
            this.docFileName = docFileName;
            this.attribute1 = attribute1;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            this.bookingId = bookingId;
            this.bookingCheckListId = bookingCheckListId;
            this.jobScheduleTaskId = jobScheduleTaskId;
            this.maintJobNumber = maintJobNumber;
            log.LogMethodExit();
        }

        public string SaveType { get { return saveType; } set { this.IsChanged = true; saveType = value; } }

        /// <summary>
        /// Get/Set method of the MaintChklstdetId field
        /// </summary>
        [DisplayName("Job Item ID")]
        [ReadOnly(true)]
        public int MaintChklstdetId { get { return maintChklstdetId; } set { maintChklstdetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintScheduleId field
        /// </summary>
        [DisplayName("Job Schedule Id")]
        [Browsable(false)]
        public int JobScheduleId { get { return jobScheduleId; } set { jobScheduleId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintTaskId field
        /// </summary>
        [DisplayName("JOb Task Id")]
        public int JobTaskId { get { return jobTaskId; } set { jobTaskId = value; this.IsChanged = true; } }        
        /// <summary>
        /// Get/Set method of the MaintJobName field
        /// </summary>
        [DisplayName("Job Name")]
        public string MaintJobName { get { return maintJobName; } set { maintJobName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the MaintJobType field
        /// </summary>
        [DisplayName("Job Type")]
        public int MaintJobType { get { return maintJobType; } set { maintJobType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChklstScheduleTime field
        /// </summary>
        [DisplayName("Schedule Date")]
        public string ChklstScheduleTime { get { return chklstScheduleTime; } set { chklstScheduleTime = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssignedTo field
        /// </summary>
        [DisplayName("Assigned To")]
        public string AssignedTo { get { return assignedTo; } set { assignedTo = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssignedUserId field
        /// </summary>
        [DisplayName("Assigned User")]
        public int AssignedUserId { get { return assignedUserId; } set { assignedUserId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DepartmentId field
        /// </summary>
        [DisplayName("Department")]
        public int DepartmentId { get { return departmentId; } set { departmentId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Status field
        /// </summary>
        [DisplayName("Status")]
        public int Status { get { return status; } set { status = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChecklistCloseDate field
        /// </summary>
        [DisplayName("Close Date")]
        public string ChecklistCloseDate { get { return checklistCloseDate; } set { checklistCloseDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaskName field
        /// </summary>
        [DisplayName("Task Name")]
        public string TaskName { get { return taskName; } set { taskName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ValidateTag field
        /// </summary>
        [DisplayName("Validate Tag?")]
        public bool ValidateTag { get { return validateTag; } set { validateTag = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("Card Id")]
        public int CardId { get { return cardId; } set { cardId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaskCardNumber field
        /// </summary>
        [DisplayName("Task Card Number")]
        public string TaskCardNumber { get { return taskCardNumber; } set { taskCardNumber = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RemarksMandatory field
        /// </summary>
        [DisplayName("Remarks Mandatory?")]
        public bool RemarksMandatory { get { return remarksMandatory; } set { remarksMandatory = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetId field
        /// </summary>
        [DisplayName("Asset")]
        public int AssetId { get { return assetId; } set { assetId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetName field
        /// </summary>
        [DisplayName("Asset Name")]
        public string AssetName { get { return assetName; } set { assetName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetType field
        /// </summary>
        [DisplayName("Asset Type")]
        public string AssetType { get { return assetType; } set { assetType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the AssetGroupName field
        /// </summary>
        [DisplayName("Asset Group Name")]
        public string AssetGroupName { get { return assetGroupName; } set { assetGroupName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChklistValue field
        /// </summary>
        [DisplayName("Checked?")]
        public bool ChklistValue { get { return chklistValue; } set { chklistValue = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ChklistRemarks field
        /// </summary>
        [DisplayName("Remarks")]
        public string ChklistRemarks { get { return chklistRemarks; } set { chklistRemarks = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the SourceSystemId field
        /// </summary>
        [DisplayName("Source System Id")]
        public string SourceSystemId { get { return sourceSystemId; } set { sourceSystemId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DurationToComplete field
        /// </summary>
        [DisplayName("Duration To Complete")]
        public int DurationToComplete { get { return durationToComplete; } set { durationToComplete = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequestType field
        /// </summary>
        [DisplayName("Request Type")]
        public int RequestType { get { return requestType; } set { requestType = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequestType field
        /// </summary>
        [DisplayName("Request Date")]
        public string RequestDate { get { return requestDate; } set { requestDate = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Priority field
        /// </summary>
        [DisplayName("Priority")]
        public int Priority { get { return priority; } set { priority = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequestDetail field
        /// </summary>
        [DisplayName("Request Detail")]
        public string RequestDetail { get { return requestDetail; } set { requestDetail = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ImageName field
        /// </summary>
        [DisplayName("Image Name")]
        public string ImageName { get { return imageName; } set { imageName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RequestedBy field
        /// </summary>
        [DisplayName("Requested By")]
        public string RequestedBy { get { return requestedBy; } set { requestedBy = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ContactPhone field
        /// </summary>
        [DisplayName("Contact Phone")]
        public string ContactPhone { get { return contactPhone; } set { contactPhone = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ContactEmailId field
        /// </summary>
        [DisplayName("Contact EmailId")]
        public string ContactEmailId { get { return contactEmailId; } set { contactEmailId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Resolution field
        /// </summary>
        [DisplayName("Resolution")]
        public string Resolution { get { return resolution; } set { resolution = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Comments field
        /// </summary>
        [DisplayName("Comments")]
        public string Comments { get { return comments; } set { comments = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the RepairCost field
        /// </summary>
        [DisplayName("Repair Cost")]
        public double RepairCost { get { return repairCost; } set { repairCost = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the DocFileName field
        /// </summary>
        [DisplayName("Doc File Name")]
        public string DocFileName { get { return docFileName; } set { docFileName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Attribute1 field
        /// </summary>
        [DisplayName("Attribute")]
        public string Attribute1 { get { return attribute1; } set { attribute1 = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }
        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public string CreationDate { get { return creationDate; } set { creationDate = value; } }
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; } }
        /// <summary>
        /// Get/Set method of the Siteid field
        /// </summary>
        [DisplayName("Site id")]
        [Browsable(false)]
        public int Siteid { get { return siteId; } set { siteId = value; } }
        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("Synch Status")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; } }//Ends:Modification on 18-Jul-2016 for publish feature

        /// <summary>
        /// Get/Set method of the BookingId field
        /// </summary>
        [DisplayName("Booking Id")]
        [Browsable(false)]
        public int BookingId { get { return bookingId; } set { bookingId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the bookingCheckListId field
        /// </summary>
        [DisplayName("Booking CheckList Id")]
        [Browsable(false)]
        public int BookingCheckListId { get { return bookingCheckListId; } set { bookingCheckListId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the JobScheduleTaskId field
        /// </summary>
        [DisplayName("Job Schedule Task Id")]
        [Browsable(false)]
        public int JobScheduleTaskId { get { return jobScheduleTaskId; } set { jobScheduleTaskId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the maintJobNumber field
        /// </summary>
        [DisplayName("Request Number")]
        [Browsable(false)]
        public string MaintJobNumber { get { return maintJobNumber; } set { maintJobNumber = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || maintChklstdetId > 0;
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
        /// Get/Set method of the maintenanceJobStatusDTOList field
        /// </summary>
        [Browsable(false)]
        public List<MaintenanceJobStatusDTO> MaintenanceJobStatusDTOList
        {
            get
            {
                return maintenanceJobStatusDTOList;
            }

            set
            {
                maintenanceJobStatusDTOList = value;
            }
        }

        /// <summary>
        /// Returns whether the UserJobItemsDTO changed or any of its maintenanceJobStatusDTO childeren are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (maintenanceJobStatusDTOList != null &&
                   maintenanceJobStatusDTOList.Any(x => x.IsChanged))
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
