/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of RecipeEstimationHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       17-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RecipeEstimationHeaderDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Recipe Estimation Header Id field
            /// </summary>
            RECIPE_ESTIMATION_HEADER_ID,

            /// <summary>
            /// Search by  From Date field
            /// </summary>
            CURRENT_FROM_DATE,
            /// <summary>
            /// Search by  From Date field
            /// </summary>
            CURRENT_TO_DATE,

            /// <summary>
            /// Search by  From Date field
            /// </summary>
            DATE_NOT_IN_JOB_DATA,

            /// <summary>
            /// Search by  From Date field
            /// </summary>
            FROM_DATE,
            
            /// <summary>
            /// Search by  To Date field
            /// </summary>
            TO_DATE,
            
            /// </summary>
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
            /// Search by Master entity id field
            /// </summary>
            RECIPE_ESTIMATION_HEADER_ID_LIST
        }

        private int recipeEstimationHeaderId;
        private DateTime fromDate;
        private DateTime toDate;
        private decimal? aspirationalPercentage;
        private decimal? seasonalPercentage;
        private bool considerEventPromotions;
        private int historicalDataInDays;
        private int? eventOffsetHrs;
        private bool? includeFinishedItem;
        private bool? includeSemiFinishedItem;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTO;

        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public RecipeEstimationHeaderDTO()
        {
            log.LogMethodEntry();
            recipeEstimationHeaderId = -1;
            isActive = true;
            siteId = -1;
            fromDate = DateTime.Today;
            toDate = DateTime.Today.AddDays(1);
            masterEntityId = -1;
            aspirationalPercentage = null;
            seasonalPercentage = null;
            considerEventPromotions = true;
            eventOffsetHrs = null;
            includeFinishedItem = null;
            includeSemiFinishedItem = null;
            recipeEstimationDetailsDTO = new List<RecipeEstimationDetailsDTO>();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public RecipeEstimationHeaderDTO(int recipeEstimationHeaderId, DateTime fromDate, DateTime toDate,
                                         decimal? aspirationalPercentage, decimal? seasonalPercentage,
                                         bool considerEventPromotions, int historicalDataInDays, 
                                         int? eventOffsetHrs, bool? includeFinishedItem, bool? includeSemiFinishedItem,
                                         bool isActive)
        : this()
        {
            log.LogMethodEntry(recipeEstimationHeaderId, fromDate, toDate, aspirationalPercentage, seasonalPercentage,
                                considerEventPromotions, historicalDataInDays, eventOffsetHrs,
                                includeFinishedItem, includeSemiFinishedItem, isActive);
            this.recipeEstimationHeaderId = recipeEstimationHeaderId;
            this.fromDate = fromDate;
            this.toDate = toDate;
            this.aspirationalPercentage = aspirationalPercentage;
            this.seasonalPercentage = seasonalPercentage;
            this.considerEventPromotions = considerEventPromotions;
            this.historicalDataInDays = historicalDataInDays;
            this.eventOffsetHrs = eventOffsetHrs;
            this.includeFinishedItem = includeFinishedItem;
            this.includeSemiFinishedItem = includeSemiFinishedItem;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with All the data fields.
        /// </summary>
        public RecipeEstimationHeaderDTO(int recipeEstimationHeaderId, DateTime fromDate, DateTime toDate, 
                                         decimal? aspirationalPercentage, decimal? seasonalPercentage,
                                         bool considerEventPromotions, int historicalDataInDays,
                                         int? eventOffsetHrs, bool? includeFinishedItem, bool? includeSemiFinishedItem,
                                         bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy,
                                         DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId)
        : this(recipeEstimationHeaderId, fromDate, toDate, aspirationalPercentage, seasonalPercentage, considerEventPromotions,
               historicalDataInDays,  eventOffsetHrs, includeFinishedItem, includeSemiFinishedItem, isActive)
        {
            log.LogMethodEntry(recipeEstimationHeaderId, fromDate, toDate, aspirationalPercentage, seasonalPercentage, 
                               considerEventPromotions, historicalDataInDays, eventOffsetHrs,
                               includeFinishedItem, includeSemiFinishedItem, isActive, createdBy, creationDate, lastUpdatedBy,
                               lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
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


        /// <summary>
        /// Get/Set method of the RecipeEstimationHeaderId field
        /// </summary>
        public int RecipeEstimationHeaderId { get { return recipeEstimationHeaderId; } set { this.IsChanged = true; recipeEstimationHeaderId = value; } }

        /// <summary>
        /// Get/Set method of the FromDate field
        /// </summary>
        public DateTime FromDate { get { return fromDate; } set { this.IsChanged = true; fromDate = value; } }

        /// <summary>
        /// Get/Set method of the ToDate field
        /// </summary>
        public DateTime ToDate { get { return toDate; } set { this.IsChanged = true; toDate = value; } }

        /// <summary>
        /// Get/Set method of the AspirationalPercentage field
        /// </summary>
        public decimal? AspirationalPercentage { get { return aspirationalPercentage; } set { this.IsChanged = true; aspirationalPercentage = value; } }

        /// <summary>
        /// Get/Set method of the SeasonalPercentage field
        /// </summary>
        public decimal? SeasonalPercentage { get { return seasonalPercentage; } set { this.IsChanged = true; seasonalPercentage = value; } }

        /// <summary>
        /// Get/Set method of the ConsiderEventPromotions field
        /// </summary>
        public bool ConsiderEventPromotions { get { return considerEventPromotions; } set { this.IsChanged = true; considerEventPromotions = value; } }

        /// <summary>
        /// Get/Set method of the HistoricalDataInDays field
        /// </summary>
        public int HistoricalDataInDays { get { return historicalDataInDays; } set { this.IsChanged = true; historicalDataInDays = value; } }

        /// <summary>
        /// Get/Set method of the EventOffsetHrs field
        /// </summary>
        public int? EventOffsetHrs { get { return eventOffsetHrs; } set { this.IsChanged = true; eventOffsetHrs = value; } }

        /// <summary>
        /// Get/Set method of the IncludeFinishedItem field
        /// </summary>
        public bool? IncludeFinishedItem { get { return includeFinishedItem; } set { this.IsChanged = true; includeFinishedItem = value; } }

        /// <summary>
        /// Get/Set method of the IncludeSemiFinishedItem field
        /// </summary>
        public bool? IncludeSemiFinishedItem { get { return includeSemiFinishedItem; } set { this.IsChanged = true; includeSemiFinishedItem = value; } }

        /// <summary>
        /// Get/Set method of the IsActive field
        /// </summary>
        public bool IsActive { get { return isActive; } set { this.IsChanged = true; isActive = value; } }

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
        /// Get/Set method of the RecipeEstimationDetailsDTOList field
        /// </summary>
        public List<RecipeEstimationDetailsDTO> RecipeEstimationDetailsDTOList
        {
            get { return recipeEstimationDetailsDTO; }
            set { recipeEstimationDetailsDTO = value; }
        }

        /// <summary>
        /// Returns true or false whether the RecipeEstimationHeaderDTO changed or any of its children are changed
        /// </summary>
        public bool IsChangedRecursive
        {
            get
            {
                //if (IsChanged)
                //{
                //    return true;
                //}
                if (recipeEstimationDetailsDTO != null &&
                   recipeEstimationDetailsDTO.Any(x => x.IsChanged))
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
                    return notifyingObjectIsChanged || recipeEstimationHeaderId < 0;
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
