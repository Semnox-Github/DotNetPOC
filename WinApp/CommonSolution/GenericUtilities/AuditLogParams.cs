/********************************************************************************************
 * Project Name - Audit Params DTO Object
 * Description  - Data object of Generic Audit Params Object for Games, Machines, Masters, Products
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        31-Jan-2019   Akshay Gulaganji          Created 
 ********************************************************************************************/
using System;
using System.ComponentModel;

namespace Semnox.Parafait.GenericUtilities
{
    /// <summary>
    /// This is the Audit Params DTO Object class. This acts as data holder for the Audit Params business object
    /// </summary>
    public class AuditLogParams
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        ///// <summary>
        ///// SearchByProductGamesParameters enum controls the search fields, this can be expanded to include additional fields
        ///// </summary>
        public enum SearchByAuditParameters
        {
            /// <summary>
            /// Search by AUDIT_ID field
            /// </summary>
            AUDIT_ID,
            /// <summary>
            /// Search by TABLE_NAME field
            /// </summary>
            TABLE_NAME,
            /// <summary>
            /// Search by RECORD_ID field
            /// </summary>
            RECORD_ID,
            /// <summary>
            /// Search by SITE_ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by FIELD_NAME field
            /// </summary>
            FIELD_NAME
        }

        private int auditId;

        private DateTime dateOfLog;
        private string tableName;
        private string fieldName;
        private string recordID;
        private string type;
        private string oldValue;
        private string newValue;
        private string userName;
        private string guid;
        private bool synchStatus;
        private int site_id;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public AuditLogParams()
        {
            log.LogMethodEntry();
            auditId = -1;
            site_id = -1;
            masterEntityId = -1;
            tableName = "";
            recordID = "";
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor to initialize the fields
        /// </summary>
        /// <param name="auditId"></param>
        /// <param name="tableName"></param>
        /// <param name="recordId"></param>
        /// <param name="userName"></param>
        public AuditLogParams(int auditId, string tableName, string recordId, string userName)
        {
            log.LogMethodEntry(auditId, tableName, recordId, userName);
            this.auditId = auditId;
            this.tableName = tableName;
            this.recordID = recordId;
            this.userName = userName;
            this.site_id = -1;
            this.masterEntityId = -1;
            log.LogMethodExit();
        }



        /// <summary>
        /// Get/Set method of the DateOfLog field
        /// </summary>
        [DisplayName("DateOfLog")]
        [ReadOnly(true)]
        public DateTime DateOfLog
        {
            get
            {
                return dateOfLog;
            }
            set
            {
                this.IsChanged = true;
                this.dateOfLog = value;
            }
        }

        /// <summary>
        /// Get/Set method of the TableName field
        /// </summary>
        [DisplayName("TableName")]
        [ReadOnly(false)]
        public string TableName
        {
            get
            {
                return tableName;
            }
            set
            {
                this.IsChanged = true;
                this.tableName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the FieldName field
        /// </summary>
        [DisplayName("FieldName")]
        [ReadOnly(true)]
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                this.IsChanged = true;
                fieldName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the RecordID field
        /// </summary>
        [DisplayName("RecordID")]
        [ReadOnly(true)]
        public string RecordID
        {
            get
            {
                return recordID;
            }
            set
            {
                this.IsChanged = true;
                this.recordID = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Type field
        /// </summary>
        [DisplayName("Type")]
        [ReadOnly(true)]
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                this.IsChanged = true;
                type = value;
            }
        }

        /// <summary>
        /// Get/Set method of the OldValue field
        /// </summary>
        [DisplayName("OldValue")]
        [ReadOnly(false)]
        public string OldValue
        {
            get
            {
                return oldValue;
            }
            set
            {
                this.IsChanged = true;
                oldValue = value;
            }
        }

        /// <summary>
        /// Get/Set method of the NewValue field
        /// </summary>
        [DisplayName("NewValue")]
        [ReadOnly(true)]
        public string NewValue
        {
            get
            {
                return newValue;
            }
            set
            {
                this.IsChanged = true;
                newValue = value;
            }
        }

        /// <summary>
        /// Get/Set method of the UserName field
        /// </summary>
        [DisplayName("UserName")]
        [ReadOnly(true)]
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                this.IsChanged = true;
                userName = value;
            }
        }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [ReadOnly(false)]
        public string Guid
        {
            get
            {
                return guid;
            }
            set
            {
                this.IsChanged = true;
                guid = value;
            }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [ReadOnly(false)]
        public bool SynchStatus
        {
            get
            {
                return synchStatus;
            }
            set
            {
                this.IsChanged = true;
                synchStatus = value;
            }
        }

        /// <summary>
        /// Get/Set method of the site_id field
        /// </summary>
        [DisplayName("site_id")]
        [ReadOnly(false)]
        public int Site_id
        {
            get
            {
                return site_id;
            }
            set
            {
                this.IsChanged = true;
                site_id = value;
            }
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        [ReadOnly(false)]
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
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        [ReadOnly(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                this.IsChanged = true;
                createdBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        [ReadOnly(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                this.IsChanged = true;
                creationDate = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        [ReadOnly(false)]
        public string LastUpdatedBy
        {
            get
            {
                return lastUpdatedBy;
            }
            set
            {
                this.IsChanged = true;
                lastUpdatedBy = value;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdateDate field
        /// </summary>
        [DisplayName("LastUpdateDate")]
        [ReadOnly(false)]
        public DateTime LastUpdateDate
        {
            get
            {
                return lastUpdateDate;
            }
            set
            {
                this.IsChanged = true;
                lastUpdateDate = value;
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
            log.LogMethodEntry();
            this.IsChanged = false;
            log.LogMethodExit();
        }
    }
}
