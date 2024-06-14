/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of RecipeManufacturingHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       23-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RecipeManufacturingHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Recipe Manufacturing Header Id field
            /// </summary>
            RECIPE_MANUFACTURING_HEADER_ID,

            /// <summary>
            /// Search by  MFG From DateTime field
            /// </summary>
            MFG_FROM_DATETIME,

            /// <summary>
            /// Search by  MFG To DateTime field
            /// </summary>
            MFG_TO_DATETIME,

            /// <summary>
            /// Search by  Recipe Plan Header Id field
            /// </summary>
            RECIPE_PLAN_HEADER_ID,

            /// <summary>
            /// Search by IsActive field
            /// </summary>
            IS_ACTIVE,

            /// <summary>
            /// Search by site_id field
            /// </summary>
            SITE_ID,

            /// <summary>
            /// Search by Master entity id field
            /// </summary>
            MASTER_ENTITY_ID,

            /// <summary>
            /// Search by Recipe Manufacturing header Id List field
            /// </summary>
            RECIPE_MANUFACTURING_HEADER_ID_LIST
        }

        private int recipeManufacturingHeaderId;
        private string recipeMfgNumber;
        private DateTime mFGDateTime;
        private bool isComplete;
        private int recipePlanHeaderId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<RecipeManufacturingDetailsDTO> recipeManufacturingDetailsDTO;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecipeManufacturingHeaderDTO()
        {
            log.LogMethodEntry();
            recipeManufacturingHeaderId = -1;
            recipePlanHeaderId = -1;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            recipeManufacturingDetailsDTO = new List<RecipeManufacturingDetailsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public RecipeManufacturingHeaderDTO(int recipeManufacturingHeaderId, string recipeMfgNumber ,DateTime mFGDateTime, bool isComplete , int recipePlanHeaderId,
            bool isActive)
            : this()
        {
            log.LogMethodEntry(recipeManufacturingHeaderId, recipeMfgNumber, mFGDateTime, isComplete, recipePlanHeaderId, isActive);
            this.recipeManufacturingHeaderId = recipeManufacturingHeaderId;
            this.mFGDateTime = mFGDateTime;
            this.recipePlanHeaderId = recipePlanHeaderId;
            this.isActive = isActive;
            this.recipeMfgNumber = recipeMfgNumber;
            this.isComplete = isComplete;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields.
        /// </summary>
        public RecipeManufacturingHeaderDTO(int recipeManufacturingHeaderId, string recipeMfgNumber, DateTime mFGDateTime, bool isComplete, int recipePlanHeaderId,
                                            bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                            DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
            : this(recipeManufacturingHeaderId, recipeMfgNumber, mFGDateTime, isComplete, recipePlanHeaderId, isActive)
        {
            log.LogMethodEntry(recipeManufacturingHeaderId, recipeMfgNumber ,mFGDateTime, isComplete ,recipePlanHeaderId, isActive, createdBy, creationDate,
                                lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            log.LogMethodExit();
        }

        // <summary>
        /// Get/Set method of the RecipeManufacturingHeaderId field
        /// </summary>
        public int RecipeManufacturingHeaderId { get { return recipeManufacturingHeaderId; } set { this.IsChanged = true; recipeManufacturingHeaderId = value; } }

        // <summary>
        /// Get/Set method of the RecipeMFGNumber field
        /// </summary>
        public string RecipeMFGNumber { get { return recipeMfgNumber; } set { this.IsChanged = true; recipeMfgNumber = value; } }

        // <summary>
        /// Get/Set method of the MFGDateTime field
        /// </summary>
        public DateTime MFGDateTime { get { return mFGDateTime; } set { this.IsChanged = true; mFGDateTime = value; } }

        /// <summary>
        /// Get/Set method of the RecipePlanHeaderId field
        /// </summary>
        public int RecipePlanHeaderId { get { return recipePlanHeaderId; } set { this.IsChanged = true; recipePlanHeaderId = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

        /// <summary>
        /// Get/Set method of the IsComplete field
        /// </summary>
        public bool IsComplete { get { return isComplete; } set { this.IsChanged = true; isComplete = value; } }

        /// <summary>
        /// Get/Set method of the CreatedBy field
        /// </summary>
        public string CreatedBy { get { return createdBy; } set { createdBy = value; } }

        /// <summary>
        /// Get/Set method of the CreationDate field
        /// </summary>
        public DateTime CreationDate { get { return creationDate; } set { creationDate = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedBy field
        /// </summary>
        public string LastUpdatedBy { get { return lastUpdatedBy; } set { lastUpdatedBy = value; } }

        /// <summary>
        /// Get/Set method of the LastUpdatedDate field
        /// </summary>
        public DateTime LastUpdateDate { get { return lastUpdateDate; } set { lastUpdateDate = value; } }

        /// <summary>
        /// Get/Set method of the SiteId field
        /// </summary>
        public int SiteId { get { return siteId; } set { siteId = value; } }

        /// <summary>
        /// Get/Set method of the MasterEntityId field
        /// </summary>
        public int MasterEntityId { get { return masterEntityId; } set { this.IsChanged = true; masterEntityId = value; } }

        /// <summary>
        /// Get/Set method of the SynchStatus field
        /// </summary>
        public bool SynchStatus { get { return synchStatus; } set { synchStatus = value; } }

        /// <summary>
        /// Get/Set method of the Guid field
        /// </summary>
        public string Guid { get { return guid; } set { this.IsChanged = true; guid = value; } }

        /// <summary>
        /// Get/Set method of the RecipeManufacturingDetailsDTOList field
        /// </summary>
        public List<RecipeManufacturingDetailsDTO> RecipeManufacturingDetailsDTOList
        {
            get { return recipeManufacturingDetailsDTO; }
            set { recipeManufacturingDetailsDTO = value; }
        }

        /// <summary>
        /// Returns true or false whether the RecipeManufacturingHeaderDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                if (IsChanged)
                {
                    return true;
                }
                if (recipeManufacturingDetailsDTO != null &&
                   recipeManufacturingDetailsDTO.Any(x => x.IsChanged))
                {
                    return true;
                }
                return false;
            }
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
                    return notifyingObjectIsChanged || recipeManufacturingHeaderId < 0;
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
