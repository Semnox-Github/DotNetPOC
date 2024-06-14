/********************************************************************************************
 * Project Name - Approval Rule DTO
 * Description  - Data object of ApprovalRule
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *1.00        18-OCT-2017   Raghuveera          Created 
 *2.70.2      13-Aug-2019   Deeksha             modifications as per 3 tier standards
 *2.100.0     17-Sep-2020   Deeksha             Modified Is changed property to handle unsaved records.
 ********************************************************************************************/
using Semnox.Parafait.logging;
using System;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the User Messages data object class. This acts as data holder for the User Messages business object
    /// </summary>
    public class ApprovalRuleDTO
    {
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByApprovalRuleParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByApprovalRuleParameters
        {
            /// <summary>
            /// Search by ACTIVE FLAG field
            /// </summary>
            ACTIVE_FLAG ,            
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID ,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID ,
            /// <summary>
            /// Search by APPROVAL RULE ID field
            /// </summary>
            APPROVAL_RULE_ID ,
            /// <summary>
            /// Search by DOCUMENT TYPE ID field
            /// </summary>
            DOCUMENT_TYPE_ID,
            /// <summary>
            /// Search by ROLE ID field
            /// </summary>
            ROLE_ID ,
            /// <summary>
            /// Search by NUMBER OF APPROVAL LEVELS field
            /// </summary>
            NUMBER_OF_APPROVAL_LEVELS 
        }
        private int approvalRuleID;
        private int documentTypeID;
        private int roleId;
        private int numberOfApprovalLevels;
        private int masterEntityId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdatedDate;
        private string guid;
        private int siteId;
        private bool synchStatus;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ApprovalRuleDTO()
        {
            log.LogMethodEntry();
            approvalRuleID = -1;
            documentTypeID = -1;
            numberOfApprovalLevels = 0;
            roleId = -1;
            masterEntityId = -1;
            isActive = true;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with required data fields
        /// </summary>
        public ApprovalRuleDTO(int approvalRuleID, int documentTypeID, int roleId, int numberOfApprovalLevels, bool isActive)
            :this()
        {
            log.LogMethodEntry( approvalRuleID, documentTypeID, roleId, numberOfApprovalLevels, isActive);
            this.approvalRuleID = approvalRuleID;
            this.documentTypeID = documentTypeID;
            this.roleId = roleId;
            this.numberOfApprovalLevels = numberOfApprovalLevels;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ApprovalRuleDTO(int approvalRuleID, int documentTypeID, int roleId, int numberOfApprovalLevels,
                            int masterEntityId, bool isActive, string createdBy, DateTime creationDate,
                            string lastUpdatedBy, DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus)
            :this(approvalRuleID, documentTypeID, roleId, numberOfApprovalLevels, isActive)
        {
            log.LogMethodEntry(approvalRuleID, documentTypeID, roleId, numberOfApprovalLevels, masterEntityId, isActive,
                               createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, guid, siteId, synchStatus);
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
        /// Get/Set method of the ApprovalRuleID field
        /// </summary>
        [DisplayName("Approval Rule ID")]
        [ReadOnly(true)]
        public int ApprovalRuleID { get { return approvalRuleID; } set { approvalRuleID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the ApprovalRuleID field
        /// </summary>
        [DisplayName("Document Type")]
        [Browsable(false)]
        public int DocumentTypeID { get { return documentTypeID; } set { documentTypeID = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the Role field
        /// </summary>
        [DisplayName("Role")]
        public int RoleId { get { return roleId; } set { roleId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the NumberOfApprovalLevels field
        /// </summary>
        [DisplayName("Number Of Approval Levels")]
        public int NumberOfApprovalLevels { get { return numberOfApprovalLevels; } set { numberOfApprovalLevels = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId { get { return masterEntityId; } set { masterEntityId = value; this.IsChanged = true; } }

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
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate { get { return lastUpdatedDate; } set { lastUpdatedDate = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid { get { return guid; } set { guid = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId { get { return siteId; } set { siteId = value; this.IsChanged = true; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || approvalRuleID < 0;
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
