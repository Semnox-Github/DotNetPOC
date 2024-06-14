/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of RecipeEstimationHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       22-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 *********************************************************************************************/
using System;
using System.ComponentModel;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RecipePlanDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Recipe Plan Detail Id field
            /// </summary>
            RECIPE_PLAN_DETAIL_ID,

            /// <summary>
            /// Search by  Recipe Plan Header Id field
            /// </summary>
            RECIPE_PLAN_HEADER_ID,

            /// <summary>
            /// Search by  Product Id field
            /// </summary>
            PRODUCT_ID,

            /// <summary>
            /// Search by  UOM Id field
            /// </summary>
            UOM_ID,
            /// <summary>
            /// Search by  Recipe Estimation Detail Id field
            /// </summary>
            RECIPE_ESTIMATION_DETAIL_ID,

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
            MASTER_ENTITY_ID
        }

        private int recipePlanDetailId;
        private int recipePlanHeaderId;
        private int productId;
        private decimal? plannedQty;
        private decimal? incrementalQty;
        private decimal? finalQty;
        private int uOMId;
        private int recipeEstimationDetailId;
        private DateTime? qtyModifiedDate;
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
        private string uom;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default Constructor
        /// </summary>
        public RecipePlanDetailsDTO()
        {
            log.LogMethodEntry();
            recipePlanDetailId = -1;
            recipePlanHeaderId = -1;
            productId = -1;
            plannedQty = null;
            incrementalQty = null;
            finalQty = null;
            uOMId = -1;
            recipeEstimationDetailId = -1;
            qtyModifiedDate = null;
            isActive = true;
            siteId = -1;
            masterEntityId = -1;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public RecipePlanDetailsDTO(int recipePlanDetailId, int recipePlanHeaderId, int productId, decimal? plannedQty,
                                    decimal? incrementalQty, decimal? finalQty, int uOMId, int recipeEstimationDetailId,
                                    DateTime? qtyModifiedDate, bool isActive)
            : this()
        {
            log.LogMethodEntry(recipePlanDetailId, recipePlanHeaderId, productId, plannedQty, incrementalQty, finalQty,
                                uOMId, recipeEstimationDetailId, qtyModifiedDate, isActive);
            this.recipePlanDetailId = recipePlanDetailId;
            this.recipePlanHeaderId = recipePlanHeaderId;
            this.productId = productId;
            this.plannedQty = plannedQty;
            this.incrementalQty = incrementalQty;
            this.finalQty = finalQty;
            this.uOMId = uOMId;
            this.recipeEstimationDetailId = recipeEstimationDetailId;
            this.qtyModifiedDate = qtyModifiedDate;
            this.isActive = isActive;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields.
        /// </summary>
        public RecipePlanDetailsDTO(int recipePlanDetailId, int recipePlanHeaderId, int productId, decimal? plannedQty,
                                    decimal? incrementalQty, decimal? finalQty, int uOMId, int recipeEstimationDetailId,
                                    DateTime? qtyModifiedDate, bool isActive, string createdBy, DateTime creationDate,
                                    string lastUpdatedBy, DateTime lastUpdateDate, string guid, int siteId, bool synchStatus,
                                    int masterEntityId)
            : this(recipePlanDetailId, recipePlanHeaderId, productId, plannedQty, incrementalQty, finalQty, uOMId,
                        recipeEstimationDetailId, qtyModifiedDate, isActive)
        {
            log.LogMethodEntry(recipePlanDetailId, recipePlanHeaderId, productId, plannedQty, incrementalQty, finalQty, uOMId,
                               recipeEstimationDetailId, qtyModifiedDate, isActive, createdBy, creationDate, lastUpdatedBy,
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
        /// Get/Set method of the RecipePlanDetailId field
        /// </summary>
        public int RecipePlanDetailId { get { return recipePlanDetailId; } set { this.IsChanged = true; recipePlanDetailId = value; } }

        /// <summary>
        /// Get/Set method of the RecipePlanHeaderId field
        /// </summary>
        public int RecipePlanHeaderId { get { return recipePlanHeaderId; } set { this.IsChanged = true; recipePlanHeaderId = value; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }

        /// <summary>
        /// Get/Set method of the PlannedQty field
        /// </summary>
        public decimal? PlannedQty { get { return plannedQty; } set { this.IsChanged = true; plannedQty = value; } }

        /// <summary>
        /// Get/Set method of the IncrementalQty field
        /// </summary>
        public decimal? IncrementalQty { get { return incrementalQty; } set { this.IsChanged = true; incrementalQty = value; } }

        /// <summary>
        /// Get/Set method of the FinalQty field
        /// </summary>
        public decimal? FinalQty { get { return finalQty; } set { this.IsChanged = true; finalQty = value; } }

        /// <summary>
        /// Get/Set method of the UOMId field
        /// </summary>
        public int UOMId { get { return uOMId; } set { this.IsChanged = true; uOMId = value; } }

        /// <summary>
        /// Get/Set method of the RecipeEstimationDetailId field
        /// </summary>
        public int RecipeEstimationDetailId { get { return recipeEstimationDetailId; } set { this.IsChanged = true; recipeEstimationDetailId = value; } }

        /// <summary>
        /// Get/Set method of the QtyModifiedDate field
        /// </summary>
        public DateTime? QtyModifiedDate { get { return qtyModifiedDate; } set { this.IsChanged = true; qtyModifiedDate = value; } }


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
        [ReadOnly(false)]
        public string RecipeName { get { return recipeName; } set { recipeName = value; } }

        /// <summary>
        /// Get/Set method of the UOM field
        /// </summary>
        public string UOM { get { return uom; } set { this.IsChanged = true; uom = value; } }

    
        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || recipePlanDetailId < 0;
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

    public class RecipePlanDetailsExcel
    {
        private DateTime? planDate;
        private decimal? planQty;
        private decimal? finalQty;
        private decimal? incrementQty;
        private string recipeName;
        private string uom;

        public DateTime? PlanDate { get { return planDate; } set { planDate = value; } }

        public string RecipeName { get { return recipeName; } set { recipeName = value; } }

        public decimal? PlannedQty { get { return planQty; } set { planQty = value; } }

        public decimal? IncrementedQty { get { return incrementQty; } set { incrementQty = value; } }

        public decimal? FinalQty { get { return finalQty; } set { finalQty = value; } }


        public string UOM { get { return uom; } set { uom = value; } }
    }
}
