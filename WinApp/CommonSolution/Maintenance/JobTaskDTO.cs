/********************************************************************************************
 * Project Name - Job Task DTO
 * Description  - Data object of job task 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera     Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera     Modified 
 *2.70        08-Mar-2019   Guru S A       Renamed MaintenanceTaskDTO as JobTaskDTO
 *2.70        07-Jul-2019   Dakshakh raj        Modified (Added Parameterized costrustor)
 ********************************************************************************************/

using System;
using System.ComponentModel;

namespace Semnox.Parafait.Maintenance
{
    /// <summary>
    /// This is the job task data object class. This acts as data holder for the job task business object
    /// </summary>
    public class JobTaskDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// JobTaskDTO enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByJobTaskParameters
        {
            /// <summary>
            /// Search by TASK NAME field
            /// </summary>
            TASK_NAME,
            
            /// <summary>
            /// Search by MAINT TASK GROUP ID field
            /// </summary>
            JOB_TASK_GROUP_ID,
            
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            IS_ACTIVE,
           
            /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID,
           
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
           
            /// <summary>
            /// Search by MAINT TASK ID field
            /// </summary>
            JOB_TASK_ID//Ends:Modification on 18-Jul-2016 for publish feature
            
        }

        private int jobTaskId;
        private string taskName;
        private int jobTaskGroupId;
        private bool validateTag;
        private string cardNumber;
        private int cardId;
        private bool remarksMandatory;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public JobTaskDTO()
        {
            log.LogMethodEntry();
            jobTaskId = -1;
            jobTaskGroupId = -1;
            cardId = -1;
            isActive = true;
            remarksMandatory = validateTag = false;
            masterEntityId = -1;//Modification on 18-Jul-2016 for publish feature
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor required data fields
        /// </summary>
        public JobTaskDTO(int jobTaskId, string taskName, int jobTaskGroupId, bool validateTag, string cardNumber, int cardId, bool remarksMandatory, bool isActive)
            :this()
        {
            log.LogMethodEntry(jobTaskId, taskName, jobTaskGroupId, validateTag, cardNumber, cardId, remarksMandatory, isActive);
            this.jobTaskId = jobTaskId;
            this.taskName = taskName;
            this.jobTaskGroupId = jobTaskGroupId;
            this.validateTag = validateTag;
            this.cardNumber = cardNumber;
            this.cardId = cardId;
            this.remarksMandatory = remarksMandatory;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public JobTaskDTO(int jobTaskId, string taskName, int jobTaskGroupId, bool validateTag, string cardNumber, int cardId, bool remarksMandatory, bool isActive,
                                   string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                   int siteId, bool synchStatus, int masterEntityId)
            :this(jobTaskId, taskName, jobTaskGroupId, validateTag, cardNumber, cardId, remarksMandatory, isActive)
        {
            log.LogMethodEntry(createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the MaintTaskId field
        /// </summary>
        [DisplayName("Task Id")]
        [ReadOnly(true)]
        public int JobTaskId { get { return jobTaskId; } set { jobTaskId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the TaskName field
        /// </summary>
        [DisplayName("Task Name")]
        public string TaskName { get { return taskName; } set { taskName = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the Maint Task Group Id field
        /// </summary>
        [DisplayName("Task Group")]
        public int JobTaskGroupId { get { return jobTaskGroupId; } set { jobTaskGroupId = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the ValidateTag field
        /// </summary>
        [DisplayName("Validate Tag?")]
        public bool ValidateTag { get { return validateTag; } set { validateTag = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the CardNumber field
        /// </summary>
        [DisplayName("Card Number")]
        public string CardNumber { get { return cardNumber; } set { cardNumber = value; this.IsChanged = true; } }
        
        /// <summary>
        /// Get/Set method of the CardId field
        /// </summary>
        [DisplayName("Card Id")]
        [Browsable(false)]
        public int CardId { get { return cardId; } set { cardId = value; } }
       
        /// <summary>
        /// Get/Set method of the RemarksMandatory field
        /// </summary>
        [DisplayName("Remarks Mandatory?")]
        public bool RemarksMandatory { get { return remarksMandatory; } set { remarksMandatory = value; this.IsChanged = true; } }
        
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
        [DisplayName("Created Date")]
        [Browsable(false)]
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }
        
        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }
        
        /// <summary>
        /// Get method of the LastupdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastupdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }
        
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }
       
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
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true;} }//Ends:Modification on 18-Jul-2016 for publish feature

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
                    return notifyingObjectIsChanged || jobTaskId < 0;
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
