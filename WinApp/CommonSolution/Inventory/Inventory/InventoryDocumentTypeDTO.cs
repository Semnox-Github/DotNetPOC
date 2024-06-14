/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of InventoryDocumentType
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
*2.70.2        07-JUl-2019    Deeksha                Modifications as per three tier standard
 ********************************************************************************************/

using System;
using Semnox.Parafait.logging;
using System.ComponentModel;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the  Inventory Document Type data object class. This acts as data holder for the Inventory Document Type business object
    /// </summary>
    public class InventoryDocumentTypeDTO
    {
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private static readonly Logger log = new Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        

        /// <summary>
        /// SearchByTaxParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByInventoryDocumentTypeParameters
        {
            /// <summary>
            /// Search by DOCUMENT TYPE ID field
            /// </summary>
            DOCUMENT_TYPE_ID ,
            /// <summary>
            /// Search by NAME field
            /// </summary>
            NAME ,
            /// <summary>
            /// Search by APPLICABILITY field
            /// </summary>
            APPLICABILITY ,
            /// <summary>
            /// Search by CODE field
            /// </summary>
            CODE ,
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
            SITE_ID
        }

        private int documentTypeId;
        private string name;
        private string description;
        private string applicability;
        private string code;
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
        public InventoryDocumentTypeDTO()
        {
            log.LogMethodEntry();
            documentTypeId = -1;
            isActive = true;
            masterEntityId = -1;
            siteId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with Required  data fields
        /// </summary>
        public InventoryDocumentTypeDTO(int documentTypeId, string name, string description, string applicability, string code,bool isActive)
            :this()
        {
            log.LogMethodEntry( documentTypeId,  name,  description,  applicability,  code, isActive);
            this.documentTypeId = documentTypeId;
            this.name = name;
            this.description = description;
            this.applicability = applicability;
            this.code = code;
            this.isActive = isActive;         
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public InventoryDocumentTypeDTO(int documentTypeId, string name, string description, string applicability, string code,
                                        bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                        DateTime lastUpdatedDate, string guid, int siteId, bool synchStatus, int masterEntityId)
            :this(documentTypeId, name, description, applicability, code, isActive)
        {
            log.LogMethodEntry(documentTypeId, name, description, applicability, code, isActive, createdBy,  creationDate,  lastUpdatedBy,
                                         lastUpdatedDate,  guid,  siteId,  synchStatus,  masterEntityId);
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
        /// Get/Set method of the DocumentTypeId field
        /// </summary>
        [DisplayName("Document Type Id")]
        [ReadOnly(true)]
        public int DocumentTypeId
        {
            get { return documentTypeId; }
            set { documentTypeId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the name field
        /// </summary>        
        [DisplayName("Name")]
        public string Name
        {
            get { return name; }
            set { name = value; this.IsChanged = true; }
        }       
                
        /// <summary>
        /// Get/Set method of the Applicability field
        /// </summary>        
        [DisplayName("Applicability")]
        public string Applicability
        {
            get { return applicability; }
            set { applicability = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the code field
        /// </summary>        
        [DisplayName("Code")]
        public string Code
        {
            get { return code; }
            set { code = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>        
        [DisplayName("Description")]
        public string Description
        {
            get { return description; } 
            set { description = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        [DisplayName("Active?")]
        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get method of the CreatedBy field
        /// </summary>
        [DisplayName("Created By")]
        [Browsable(false)]
        public string CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        /// <summary>
        /// Get method of the CreationDate field
        /// </summary>
        [DisplayName("Creation Date")]
        [Browsable(false)]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        /// <summary>
        /// Get method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("Updated By")]
        [Browsable(false)]
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value; }
        }

        /// <summary>
        /// Get method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("Updated Date")]
        [Browsable(false)]
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value; }
        } 

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        [Browsable(false)]
        public string Guid
        {
            get { return guid; }
            set { guid = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        [DisplayName("SiteId")]
        [Browsable(false)]
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        [Browsable(false)]
        public bool SynchStatus
        {
            get { return synchStatus; }
            set { synchStatus = value;}
        }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>        
        [DisplayName("MasterEntityId")]
        [Browsable(false)]
        public int MasterEntityId
        {
            get { return masterEntityId; }
            set { masterEntityId = value; this.IsChanged = true; } }

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
                    return notifyingObjectIsChanged || documentTypeId < 0;
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
