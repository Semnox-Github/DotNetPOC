/****************************************************************************************************************
 * Project Name - Transaction Order Type DTO
 * Description  - Transaction Order Type DTO
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *****************************************************************************************************************
 *2.80        26-Jun-2020      Raghuveera     Created 
 *****************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// TransactionOrderTypeDTO data object
    /// </summary>
    public class TransactionOrderTypeDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        private bool notifyingObjectIsChanged;

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {   /// <summary>
            /// Search by Id
            /// </summary>
            ID,
            /// <summary>
            /// Search by Name
            /// </summary>
            Name,            
            /// <summary>
            /// Search by isactive
            /// </summary>
            IS_ACTIVE,
            /// <summary>
            /// Search by MASTER_ENTITY_ID 
            /// </summary>
            MASTER_ENTITY_ID,            
            /// <summary>
            /// Search by site_id
            /// </summary>
            SITE_ID
        }

        private int id;
        private string name;
        private string description;
        private bool isActive;
        private DateTime creationDate;
        private string createdBy;
        private DateTime lastUpdateDate;
        private string lastUpdatedBy;
        private int site_id;
        private string guid;
        private bool synchStatus;
        private int masterEntityId;
        
        /// <summary>
        /// The default constructor
        /// </summary>
        public TransactionOrderTypeDTO()
        {
            log.LogMethodEntry();
            id = -1;
            isActive = true;
            masterEntityId = -1;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        
        public TransactionOrderTypeDTO(int id, string name, string description, bool isActive, DateTime creationDate, string createdBy,
                            DateTime lastUpdateDate, string lastUpdatedBy, int site_id, string guid,
                            bool synchStatus, int masterEntityId)
        {
            log.LogMethodEntry(id, name, description, isActive,
                               creationDate, createdBy, lastUpdateDate,
                               lastUpdatedBy, site_id, guid, synchStatus,
                               masterEntityId);
            this.id = id;
            this.name = name;
            this.description = description;
            this.isActive = isActive;
            this.creationDate = creationDate;
            this.createdBy = createdBy;
            this.lastUpdateDate = lastUpdateDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.site_id = site_id;
            this.guid = guid;
            this.synchStatus = synchStatus;
            this.masterEntityId = masterEntityId;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the orderId field
        /// </summary>
        [Browsable(false)]
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                this.IsChanged = true;
            }
        }
        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                this.IsChanged = true;
            }
        }
        
        /// <summary>
        /// Get/Set method of the isActive field
        /// </summary>
        [Browsable(false)]
        public bool IsActive
        {
            get
            {
                return isActive;
            }

            set
            {
                this.IsChanged = true;
                isActive = value;
            }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [Browsable(false)]
        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
            set
            {
                creationDate = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [Browsable(false)]
        public string CreatedBy
        {
            get
            {
                return createdBy;
            }
            set
            {
                createdBy = value;
                this.IsChanged = true;
            }
        }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [Browsable(false)]
        public DateTime LastUpdatedDate
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
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the site_id field
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
                this.IsChanged = true;
                site_id = value;
            }
        }
        /// <summary>
        /// Get/Set method of the guid field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the synchStatus field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set method of the masterEntityId field
        /// </summary>
        [Browsable(false)]
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
        /// Get/Set Method for select field
        /// Used in orderlistview for row selection 
        /// will not be saved to the db.
        /// </summary>
        public bool Selected
        {
            get; set;
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
                    return notifyingObjectIsChanged || id < 0;
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
