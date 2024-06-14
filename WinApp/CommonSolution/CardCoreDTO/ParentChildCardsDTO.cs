/********************************************************************************************************
 * Project Name - ParentChildCardsDTO
 * Description  - Data Transfer Object for ParentChildCards Entity
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************************
*2.100.0      10-Oct-2020     Mathew Ninan      Modified: Support for Daily Limit Percentage for child cards
**********************************************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.CardCore
{
    public class ParentChildCardsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  ID field
            /// </summary>
            ID =1,
            /// <summary>
            /// Search by  PARENT_CARD_ID field
            /// </summary>
            PARENT_CARD_ID =2,
            /// <summary>
            /// CHILD_CARD_ID
            /// </summary>
            CHILD_CARD_ID =3,
            /// <summary>
            /// ACTIVE_FLAG
            /// </summary>
            ACTIVE_FLAG =4,
            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID =5,
            /// <summary>
            /// Search by MASTER_ENTITY_ID field
            /// </summary>
            MASTER_ENTITY_ID =6
        }

        int id;
        int parentCardId;
        int childCardId;
        string guid;
        int site_id;
        bool synchStatus;
        string createdBy;
        DateTime? creationDate;
        string lastUpdatedBy;
        DateTime? lastUpdatedDate;
        bool activeFlag;
        int masterEntityId;
        int? dailyLimitPercentage;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ParentChildCardsDTO()
        {
            log.LogMethodEntry();
            Id = -1;
            parentCardId = -1;
            childCardId = -1;
            site_id = -1;
            activeFlag = true;
            masterEntityId = -1;
            synchStatus = false;
            dailyLimitPercentage = null;
            log.LogMethodExit();
        }

        public ParentChildCardsDTO(int id, int parentCardId, int childCardId, bool activeFlag, int masterEntityId, int? dailyLimitPercentage) : this()
        {
            log.LogMethodEntry(id, parentCardId, childCardId, activeFlag, masterEntityId, dailyLimitPercentage);
            this.id = id;
            this.parentCardId = parentCardId;
            this.childCardId = childCardId;
            this.activeFlag = activeFlag;
            this.masterEntityId = masterEntityId;
            this.dailyLimitPercentage = dailyLimitPercentage;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ParentChildCardsDTO(int id, int parentCardId, int childCardId, int site_id, string guid, bool synchStatus, DateTime? creationDate, string createdBy, DateTime? lastUpdatedDate, string lastUpdatedBy, bool activeFlag, int masterEntityId, int? dailyLimitPercentage)
            : this(id, parentCardId, childCardId, activeFlag, masterEntityId, dailyLimitPercentage)
        {
            log.LogMethodEntry(id, parentCardId, childCardId, site_id, guid, synchStatus, creationDate, createdBy, lastUpdatedDate, lastUpdatedBy,  activeFlag, masterEntityId);
            this.guid = guid;
            this.site_id = site_id;
            this.synchStatus = synchStatus;
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdatedDate = lastUpdatedDate;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int Id
        { get { return id; } set { this.IsChanged = true; id = value; } }
        /// <summary>
        /// Get/Set method of the parentCardId field
        /// </summary>
        [DisplayName("ParentCardId")]
        public int ParentCardId
        { get { return parentCardId; } set { this.IsChanged = true; parentCardId = value; } }
        /// <summary>
        /// Get/Set method of the childCardId field
        /// </summary>
        [DisplayName("ChildCardId")]
        public int ChildCardId
        { get { return childCardId; } set { this.IsChanged = true; childCardId = value; } }
        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        [DisplayName("Guid")]
        public string Guid
        { get { return guid; } set { this.IsChanged = true; guid = value; } }
        /// <summary>
        /// Get/Set method of the Site_id field
        /// </summary>
        [DisplayName("Site_id")]
        public int Site_id
        { get { return site_id; } set { this.IsChanged = true; site_id = value; } }
        /// <summary>
        /// Get/Set method of the synchStatus field
        /// </summary>
        [DisplayName("SynchStatus")]
        public bool SynchStatus
        { get { return synchStatus; } set { this.IsChanged = true; synchStatus = value; } }
        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        [DisplayName("CreatedBy")]
        public string CreatedBy
        { get { return createdBy; } set { this.IsChanged = true; createdBy = value; } }
        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        [DisplayName("CreationDate")]
        public DateTime? CreationDate
        { get { return creationDate; } set { this.IsChanged = true; creationDate = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        [DisplayName("LastUpdatedBy")]
        public string LastUpdatedBy
        { get { return lastUpdatedBy; } set { this.IsChanged = true; lastUpdatedBy = value; } }
        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        [DisplayName("LastUpdatedDate")]
        public DateTime? LastUpdatedDate
        { get { return lastUpdatedDate; } set { this.IsChanged = true; lastUpdatedDate = value; } }
        /// <summary>
        /// Get/Set method of the ActiveFlag field
        /// </summary>
        [DisplayName("ActiveFlag")]
        public bool ActiveFlag
        { get { return activeFlag; } set { this.IsChanged = true; activeFlag = value; } }
        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        [DisplayName("MasterEntityId")]
        public int MasterEntityId
        { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        [DisplayName("DailyLimitPercentage")]
        public int? DailyLimitPercentage
        {
            get { return dailyLimitPercentage; }
            set { this.IsChanged = true; dailyLimitPercentage = value; }
        }
        //

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
            log.LogMethodExit(null);
        }

    }
}
