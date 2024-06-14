/********************************************************************************************
 * Project Name - POS Redesign
 * Description  - HRApproval logs dto
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 ********************************************************************************************* 
 *2.140.0     23-June-2021   Prashanth            Created for POS UI Redesign 
 ********************************************************************************************/
using System;
using System.ComponentModel;


namespace Semnox.Parafait.User
{
    /// <summary>
    /// DTO class for HRApprovalLogs
    /// </summary>
    public class HRApprovalLogsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        public enum EntityType
        {
            USERIDENTIFICATIONTAGS
        }

        public enum ActionType
        {
            ADD,
            DEACTIVATE
        }

        public enum SearchByParameters
        {
            /// <summary>
            /// Search by APPROVAL_LOG_ID field
            /// </summary>
            APPROVAL_LOG_ID,

            /// <summary>
            /// Search by ENTITY field
            /// </summary>
            ENTITY,

            /// <summary>
            /// Search by ENTITY_GUID field
            /// </summary>
            ENTITY_GUID,

            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by POS_MACHINE_ID field
            /// </summary>
            POS_MACHINE_ID,

            /// <summary>
            /// Search by ACTION field
            /// </summary>
            ACTION,

            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID
        }

        private int approvalLogId;
        private string entity;
        private string entityGuid;
        private string action;
        private string approverId;
        private DateTime? approvalTime;
        private string approvalLevel;
        private string remarks;
        private string createdBy;
        private DateTime? creationDate;
        private string lastUpdatedBy;
        private DateTime? lastUpdatedDate;
        private int posMachineId;
        private int siteId;
        private string guid;
        private bool syncStatus;
        private int masterEntityId;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public HRApprovalLogsDTO()
        {
            log.LogMethodEntry();
            approvalLogId = -1;
            approverId = "-1";
            approvalTime = null;
            posMachineId = -1;
            siteId = -1;
            syncStatus = false;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        ///<summary>
        /// Parameterized Constructor
        ///</summary>
        public HRApprovalLogsDTO(HRApprovalLogsDTO hrApprovalLogsDTO)
        {
            this.approvalLogId = hrApprovalLogsDTO.ApprovalLogId;
            this.entity = hrApprovalLogsDTO.Entity;
            this.entityGuid = hrApprovalLogsDTO.EntityGuid;
            this.action = hrApprovalLogsDTO.Action;
            this.approverId = hrApprovalLogsDTO.ApproverId;
            this.approvalTime = hrApprovalLogsDTO.ApprovalTime;
            this.approvalLevel = hrApprovalLogsDTO.ApprovalLevel;
            this.remarks = hrApprovalLogsDTO.Remarks;
            this.createdBy = hrApprovalLogsDTO.CreatedBy;
            this.creationDate = hrApprovalLogsDTO.CreationDate;
            this.lastUpdatedBy = hrApprovalLogsDTO.LastUpdatedBy;
            this.lastUpdatedDate = hrApprovalLogsDTO.LastUpdatedDate;
            this.posMachineId = hrApprovalLogsDTO.POSMachineId;
            this.siteId = hrApprovalLogsDTO.SiteId;
            this.guid = hrApprovalLogsDTO.Guid;
            this.syncStatus = hrApprovalLogsDTO.SyncStatus;
            this.masterEntityId = hrApprovalLogsDTO.MasterEntityId;
    }

        ///<summary>
        /// Parameterized Constructor with all fields
        ///</summary>
        public HRApprovalLogsDTO(int approvalLogId, string entity, string entityGuid, string action, string approverId,
            DateTime? approvalTime, string approvalLevel, string remarks, string createdBy, DateTime? creationDate,
            string lastUpdatedBy, DateTime? lastUpdatedDate, int posMachineId, int siteId, string guid, bool syncStatus, int masterEntityId) : this()
        {
            log.LogMethodEntry(approvalLogId, entity, entityGuid, action, approverId, approvalTime, approvalLevel, remarks,
                createdBy, creationDate, lastUpdatedBy, lastUpdatedDate, posMachineId, siteId, guid, syncStatus, masterEntityId);
            this.approvalLogId = approvalLogId;
            this.entity = entity;
            this.entityGuid = entityGuid;
            this.action = action;
            this.approverId = approverId;
            this.approvalTime = approvalTime;
            this.approvalLevel = approvalLevel;
            this.remarks = remarks;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            this.posMachineId = posMachineId;
            this.siteId = siteId;
            this.guid = guid;
            this.syncStatus = syncStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }


        ///<summary>
        /// Get/Set method for approvalLogId field
        ///</summary>
        [DisplayName("ApprovalLogId")]
        [ReadOnly(true)]
        [Browsable(false)]
        public int ApprovalLogId
        {
            get
            {
                return approvalLogId;
            }
            set
            {
                IsChanged = true;
                approvalLogId = value;
            }
        }

        ///<summary>
        /// Get/Set method for entity field
        ///</summary>
        [DisplayName("Entity")]
        public string Entity
        {
            get
            {
                return entity;
            }
            set
            {
                IsChanged = true;
                entity = value;
            }
        }

        ///<summary>
        /// Get/Set method for entityGuid field
        ///</summary>
        [DisplayName("EntityGuid")]
        [Browsable(false)]
        public string EntityGuid
        {
            get
            {
                return entityGuid;
            }
            set
            {
                IsChanged = true;
                entityGuid = value;
            }
        }

        ///<summary>
        /// Get/Set method for action field
        ///</summary>
        [DisplayName("Action")]
        public string Action
        {
            get
            {
                return action;
            }
            set
            {
                IsChanged = true;
                action = value;
            }
        }

        ///<summary>
        /// Get/Set method for approverId field
        ///</summary>
        [DisplayName("ApproverId")]
        public string ApproverId
        {
            get
            {
                return approverId;
            }
            set
            {
                IsChanged = true;
                approverId = value;
            }
        }

        ///<summary>
        /// Get/Set method for approvalTime field
        ///</summary>
        [DisplayName("ApprovalTime")]
        public DateTime? ApprovalTime
        {
            get
            {
                return approvalTime;
            }
            set
            {
                IsChanged = true;
                approvalTime = value;
            }
        }

        ///<summary>
        /// Get/Set method for approverLevel field
        ///</summary>
        [DisplayName("ApprovalLevel")]
        public string ApprovalLevel
        {
            get
            {
                return approvalLevel;
            }
            set
            {
                IsChanged = true;
                approvalLevel = value;
            }
        }

        ///<summary>
        /// Get/Set method for remarks field
        ///</summary>
        [DisplayName("Remarks")]
        public string Remarks
        {
            get
            {
                return remarks;
            }
            set
            {
                IsChanged = true;
                remarks = value;
            }
        }

        ///<summary>
        /// Get/Set method for createdBy field
        ///</summary>
        [DisplayName("CreatedBy")]
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                IsChanged = true;
                createdBy = value;
            }
        }

        ///<summary>
        /// Get/Set method for creationDate field
        ///</summary>
        [DisplayName("CreatedDate")]
        [Browsable(false)]
        public DateTime? CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                IsChanged = true;
                creationDate = value;
            }
        }

        ///<summary>
        /// Get/Set method for lastUpdatedBy field
        ///</summary>
        [DisplayName("LastUpdatedBy")]
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                IsChanged = true;
                lastUpdatedBy = value;
            }
        }

        ///<summary>
        /// Get/Set method for lastUpdatedDate field
        ///</summary>
        [DisplayName("LastUpdatedDate")]
        [Browsable(false)]
        public DateTime? LastUpdatedDate
        {
            get
            {
                return lastUpdatedDate;
            }
            set
            {
                IsChanged = true;
                lastUpdatedDate = value;
            }
        }

        ///<summary>
        /// Get/Set method for posMachineId
        ///</summary>
        [DisplayName("POSMachineId")]
        [Browsable(false)]
        public int POSMachineId
        {
            get
            {
                return posMachineId;
            }
            set
            {
                IsChanged = true;
                posMachineId = value;
            }
        }

        ///<summary>
        /// Get/Set method for siteId field
        ///</summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId
        {
            get
            {
                return siteId;
            }
            set
            {
                IsChanged = true;
                siteId = value;
            }
        }

        ///<summary>
        /// Get/Set method for guid field
        ///</summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                IsChanged = true;
                guid = value;
            }
        }

        ///<summary>
        /// Get/Set method for syncStatus field
        ///</summary>
        [DisplayName("SyncStatus")]
        [Browsable(false)]
        public bool SyncStatus
        {
            get
            {
                return syncStatus;
            }
            set
            {
                IsChanged = true;
                syncStatus = value;
            }
        }

        ///<summary>
        /// Get/Set method for masterEntityId field
        ///</summary>
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId
        {
            get
            {
                return masterEntityId;
            }
            set
            {
                IsChanged = true;
                masterEntityId = value;
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
                    return notifyingObjectIsChanged || ApprovalLogId < 0;
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
