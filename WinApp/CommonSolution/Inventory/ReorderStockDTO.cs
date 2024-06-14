/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of ReorderStock
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.70.3     28-May-2019   Girish Kundar           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// This is the ReorderStockDTO data object class. This acts as data holder for the ReorderStock business object
    /// </summary>
    public class ReorderStockDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  REORDER STOCK ID field
            /// </summary>
            REORDER_STOCK_ID,
            /// <summary>
            /// Search by  REORDER STOCK ID LIST field
            /// </summary>
            REORDER_STOCK_ID_LIST,
            /// <summary>
            /// Search by  REMARKS field
            /// </summary>
            REMARKS,
            /// <summary>
            /// Search by SITE ID field
            /// </summary>
            SITE_ID,
            /// <summary>
            /// Search by MASTER ENTITY ID field
            /// </summary>
            MASTER_ENTITY_ID
        }
        private int reorderStockId;
        private DateTime timestamp;
        private string remarks;
        private string guid;
        private DateTime lastUpdatedDate;
        private string lastUpdatedBy;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string createdBy;
        private DateTime creationDate;
        private List<ReorderStockLineDTO> reorderStockLineDTOList;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ReorderStockDTO()
        {
            log.LogMethodEntry();
            reorderStockId = -1;
            siteId = -1;
            masterEntityId = -1;
            reorderStockLineDTOList = new List<ReorderStockLineDTO>();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        public ReorderStockDTO(int reorderStockId, DateTime timestamp, string remarks)
            :this()
        {
            log.LogMethodEntry(reorderStockId, timestamp, remarks);
            this.reorderStockId = reorderStockId;
            this.timestamp = timestamp;
            this.remarks = remarks;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with all the fields
        /// </summary>
        public ReorderStockDTO(int reorderStockId,DateTime timestamp, string remarks,string guid,DateTime lastUpdatedDate,
                               string lastUpdatedBy,int siteId, bool synchStatus,int masterEntityId, 
                               string createdBy, DateTime creationDate)
            :this(reorderStockId, timestamp, remarks)
        {
            log.LogMethodEntry(reorderStockId, timestamp, remarks, guid, lastUpdatedDate, lastUpdatedBy, siteId,
                               synchStatus, masterEntityId,createdBy, creationDate);
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

        /// <summary>
        /// Get/Set method of the ReorderStockId  field
        /// </summary>
        public int ReorderStockId
        {
            get { return reorderStockId; }
            set { reorderStockId = value; this.IsChanged = true; }
        }

        /// <summary>
        /// Get/Set method of the Timestamp  field
        /// </summary>
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the Remarks  field
        /// </summary>
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; this.IsChanged = true; }
        }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy
        {
            get { return lastUpdatedBy; }
            set { lastUpdatedBy = value;  }
        }
        /// <summary>
        ///  Get/Set method of the LastUpdateDate field
        /// </summary>
        public DateTime LastUpdatedDate
        {
            get { return lastUpdatedDate; }
            set { lastUpdatedDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId
        {
            get { return siteId; }
            set { siteId = value;  }
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
            set { synchStatus = value;  }
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
            set { createdBy = value;  }
        }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value;  }
        }
        /// <summary>
        /// Get/Set method of the ReorderStockLineDTOList field
        /// </summary>
        public List<ReorderStockLineDTO> ReorderStockLineDTOList
        {
            get { return reorderStockLineDTOList; }
            set { reorderStockLineDTOList = value; }
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
                    return notifyingObjectIsChanged || reorderStockId < 0;
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
        /// Returns whether the ReorderStockDTO changed or any of its children  changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (reorderStockLineDTOList != null &&
                    reorderStockLineDTOList.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
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
