/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of RecipeEstimationDetails
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0        20-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RecipeEstimationDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Recipe Estimation Detail Id field
            /// </summary>
            RECIPE_ESTIMATION_DETAIL_ID,
            
            /// <summary>
            /// Search by  Recipe Estimation Header Id field
            /// </summary>
            RECIPE_ESTIMATION_HEADER_ID,
            
            /// <summary>
            /// Search by  Product Id field
            /// </summary>
            PRODUCT_ID,
            
            /// <summary>
            /// Search by  UOM Id field
            /// </summary>
            UOM_ID,
            
            /// <summary>
            /// Search by  Accounting Calendar MasterId Id field
            /// </summary>
            ACCOUNTING_CALENDAR_MASTER_ID,
            
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
            MASTER_ENTITY_ID
        }

        private int recipeEstimationDetailId;
        private int recipeEstimationHeaderId;
        private int productId;
        private decimal? estimatedQty;
        private decimal? eventQty;
        private decimal? totalEstimatedQty;
        private decimal? plannedQty;
        private decimal? stockQty;
        private int uomId;
        private int accountingCalendarMasterId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string recipeName;
        private DateTime eventDate;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// default constructor
        /// </summary>
        public RecipeEstimationDetailsDTO()
        {
            log.LogMethodEntry();
            recipeEstimationDetailId = -1;
            recipeEstimationHeaderId = -1;
            productId = -1;
            uomId = -1;
            accountingCalendarMasterId = -1;
            siteId = -1;
            masterEntityId = -1;
            isActive = true;
            estimatedQty = null;
            eventQty = null;
            totalEstimatedQty = null;
            plannedQty = null;
            stockQty = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public RecipeEstimationDetailsDTO(int recipeEstimationDetailId, int recipeEstimationHeaderId, int productId, 
                                          decimal? estimatedQty, decimal? eventQty, decimal? totalEstimatedQty,
                                          decimal? plannedQty,  decimal? stockQty, int uomId, int accountingCalendarMasterId,
                                          bool isActive)
        : this()
        {
            log.LogMethodEntry(recipeEstimationDetailId, recipeEstimationHeaderId, productId, estimatedQty, eventQty, 
                                totalEstimatedQty, plannedQty, stockQty, uomId, accountingCalendarMasterId, isActive);
            this.recipeEstimationDetailId = recipeEstimationDetailId;
            this.recipeEstimationHeaderId = recipeEstimationHeaderId;
            this.productId = productId;
            this.estimatedQty = estimatedQty;
            this.eventQty = eventQty;
            this.totalEstimatedQty = totalEstimatedQty;
            this.plannedQty = plannedQty;
            this.stockQty = stockQty;
            this.uomId = uomId;
            this.accountingCalendarMasterId = accountingCalendarMasterId;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with All data fields.
        /// </summary>
        public RecipeEstimationDetailsDTO(int recipeEstimationDetailId, int recipeEstimationHeaderId, int productId, 
                                          decimal? estimatedQty, decimal? eventQty, decimal? totalEstimatedQty,
                                          decimal? plannedQty, decimal? stockQty, int uomId, int accountingCalendarMasterId, 
                                          bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, 
                                          DateTime lastUpdateDate, string guid, int siteId, bool synchStatus, int masterEntityId, DateTime eventDate)
        : this(recipeEstimationDetailId, recipeEstimationHeaderId, productId, estimatedQty, eventQty, totalEstimatedQty, 
                plannedQty, stockQty, uomId, accountingCalendarMasterId, isActive)
        {
            log.LogMethodEntry(recipeEstimationDetailId, recipeEstimationHeaderId, productId, estimatedQty, eventQty,
                                totalEstimatedQty, plannedQty, stockQty, uomId, accountingCalendarMasterId, isActive, createdBy,
                                creationDate, lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId);
            this.createdBy = createdBy;
            this.creationDate = creationDate;
            this.lastUpdatedBy = lastUpdatedBy;
            this.lastUpdateDate = lastUpdateDate;
            this.siteId = siteId;
            this.masterEntityId = masterEntityId;
            this.synchStatus = synchStatus;
            this.guid = guid;
            this.eventDate = eventDate;
            log.LogMethodExit();
        }


        /// <summary>
        /// Get/Set method of the RecipeEstimationDetailId field
        /// </summary>
        public int RecipeEstimationDetailId { get { return recipeEstimationDetailId; } set { this.IsChanged = true; recipeEstimationDetailId = value; } }

        /// <summary>
        /// Get/Set method of the RecipeEstimationHeaderId field
        /// </summary>
        public int RecipeEstimationHeaderId { get { return recipeEstimationHeaderId; } set { this.IsChanged = true; recipeEstimationHeaderId = value; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }

        /// <summary>
        /// Get/Set method of the EstimatedQty field
        /// </summary>
        public decimal? EstimatedQty { get { return estimatedQty; } set { this.IsChanged = true; estimatedQty = value; } }

        /// <summary>
        /// Get/Set method of the EventQty field
        /// </summary>
        public decimal? EventQty { get { return eventQty; } set { this.IsChanged = true; eventQty = value; } }


        /// <summary>
        /// Get/Set method of the TotalEstimatedQty field
        /// </summary>
        public decimal? TotalEstimatedQty { get { return totalEstimatedQty; } set { this.IsChanged = true; totalEstimatedQty = value; } }

        /// <summary>
        /// Get/Set method of the PlannedQty field
        /// </summary>
        public decimal? PlannedQty { get { return plannedQty; } set { this.IsChanged = true; plannedQty = value; } }

        /// <summary>
        /// Get/Set method of the StockQty field
        /// </summary>
        public decimal? StockQty { get { return stockQty; } set { this.IsChanged = true; stockQty = value; } }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        public int UOMId { get { return uomId; } set { this.IsChanged = true; uomId = value; } }

        /// <summary>
        /// Get/Set method of the AccountingCalendarMasterId field
        /// </summary>
        public int AccountingCalendarMasterId { get { return accountingCalendarMasterId; } set { this.IsChanged = true; accountingCalendarMasterId = value; } }

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
        /// Get/Set method of the RecipeName field
        /// </summary>
        public string RecipeName { get { return recipeName; } set { this.IsChanged = true; recipeName = value; } }

        /// <summary>
        /// Get/Set method of the EventDate field
        /// </summary>
        public DateTime EventDate { get { return eventDate; } set { this.IsChanged = true; eventDate = value; } }


        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || recipeEstimationDetailId < 0;
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
