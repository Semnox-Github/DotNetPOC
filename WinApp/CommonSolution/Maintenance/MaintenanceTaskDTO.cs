/********************************************************************************************
 * Project Name - Maintenance Task DTO
 * Description  - Data object of maintenance task 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        23-Dec-2015   Raghuveera     Created 
 *********************************************************************************************
 *1.00        18-Jul-2016   Raghuveera          Modified 
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
    /// This is the maintenance task data object class. This acts as data holder for the maintenance task business object
    /// </summary>
    public class MaintenanceTaskDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByMaintenanceTaskParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByMaintenanceTaskParameters
        {
            /// <summary>
            /// Search by TASK_NAME field
            /// </summary>
            TASK_NAME = 0,
            /// <summary>
            /// Search by MAINT_TASK_GROUP_ID field
            /// </summary>
            MAINT_TASK_GROUP_ID = 1,
            /// <summary>
            /// Search by ACTIVE_FLAG field
            /// </summary>
            ACTIVE_FLAG = 2,
            /// <summary>//starts:Modification on 18-Jul-2016 for publish feature
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID = 3,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID = 4,
            /// <summary>
            /// Search by MAINT_TASK_ID field
            /// </summary>
            MAINT_TASK_ID = 5//Ends:Modification on 18-Jul-2016 for publish feature
            
        }

        int maintTaskId;
        string taskName;
        int maintTaskGroupId;
        string validateTag;
        string cardNumber;
        int cardId;
        string remarksMandatory;
        string isActive;
        string createdBy;
        DateTime creationDate;
        string lastUpdatedBy;
        DateTime lastUpdatedDate;
        string guid;
        int siteId;
        bool synchStatus;
        int masterEntityId;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MaintenanceTaskDTO()
        {
            log.Debug("Starts-MaintenanceTaskDTO() default constructor.");
            maintTaskId = -1;
            maintTaskGroupId = -1;
            cardId = -1;
            isActive = "Y";
            remarksMandatory = validateTag = "N";
            masterEntityId = -1;//Modification on 18-Jul-2016 for publish feature
            log.Debug("Ends-MaintenanceTaskDTO() default constructor.");
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public MaintenanceTaskDTO(int maintTaskId, string taskName, int maintTaskGroupId, string validateTag, string cardNumber, int cardId, string remarksMandatory, string isActive,
                                   string createdBy, DateTime creationDate, string lastUpdatedBy, DateTime lastUpdatedDate, string guid,
                                   int siteId, bool synchStatus, int masterEntityId)
        {
            log.Debug("Starts-MaintenanceTaskDTO(with all the data fields) Parameterized constructor.");
            this.maintTaskId = maintTaskId;
            this.taskName = taskName;
            this.maintTaskGroupId = maintTaskGroupId;
            this.validateTag = validateTag;
            this.cardNumber = cardNumber;
            this.cardId = cardId;
            this.remarksMandatory=remarksMandatory;
            this.isActive = isActive;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.guid = guid;
            this.siteId = siteId;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.Debug("Ends-MaintenanceTaskDTO(with all the data fields) Parameterized constructor.");
        }

        /// <summary>
        /// Get/Set method of the MaintTaskId field
        /// </summary>
        [DisplayName("Task Id")]
        [ReadOnly(true)]
        public int MaintTaskId { get { return maintTaskId; } set { maintTaskId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the TaskName field
        /// </summary>
        [DisplayName("Task Name")]
        public string TaskName { get { return taskName; } set { taskName = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the Maint Task Group Id field
        /// </summary>
        [DisplayName("Task Group")]
        public int MaintTaskGroupId { get { return maintTaskGroupId; } set { maintTaskGroupId = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the ValidateTag field
        /// </summary>
        [DisplayName("Validate Tag?")]
        public string ValidateTag { get { return validateTag; } set { validateTag = value; this.IsChanged = true; } }
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
        public string RemarksMandatory { get { return remarksMandatory; } set { remarksMandatory = value; this.IsChanged = true; } }
        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public string IsActive { get { return isActive; } set { isActive = value; this.IsChanged = true; } }
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
                    return notifyingObjectIsChanged;
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
            log.Debug("Starts-AcceptChanges() Method.");
            this.IsChanged = false;
            log.Debug("Ends-AcceptChanges() Method.");
        }

    }
}
