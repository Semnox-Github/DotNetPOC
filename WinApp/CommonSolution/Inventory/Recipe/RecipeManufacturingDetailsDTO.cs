/********************************************************************************************
 * Project Name - Inventory
 * Description  - Data object of RecipeManufacturingDetails
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       23-Jul-2020   Deeksha             Created for Recipe Management enhancement.
  2.120.00     05-May-2021        Mushahid Faizan      Web Inventory UI Changes
 *********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class RecipeManufacturingDetailsDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// SearchByParameters enum controls the search fields, this can be expanded to include additional fields
        /// </summary>
        /// 
        public enum SearchByParameters
        {
            /// <summary>
            /// Search by  Recipe Manufacturing Detail Id field
            /// </summary>
            RECIPE_MANUFACTURING_DETAIL_ID,
            
            /// <summary>
            /// Search by  Recipe Manufacturing Header Id field
            /// </summary>
            RECIPE_MANUFACTURING_HEADER_ID,
            
            /// <summary>
            /// Search by  MfgLine Id field
            /// </summary>
            MFGLINE_ID,
           
            /// <summary>
            /// Search by  Product Id field
            /// </summary>
            PRODUCT_ID,
            
            /// <summary>
            /// Search by  MfgUOM Id field
            /// </summary>
            MFGUOM_ID,
            
            /// <summary>
            /// Search by  Top MostParent MFGLineId field
            /// </summary>
            TOP_MOST_PARENT_MFG_LINE_ID,
            
            /// <summary>
            /// Search by  Top Parent MFGLineId field
            /// </summary>
            PARENT_MFG_LINE_ID,
            
            /// <summary>
            /// Search by  Top  ActualMfgUOMId field
            /// </summary>
            ACTUALMFG_UOM_ID,
            
            /// <summary>
            /// Search by  Top  RecipePlanDetailId field
            /// </summary>
            RECIPE_PLAN_DETAIL_ID,
            
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

        private int recipeManufacturingDetailId;
        private int recipeManufacturingHeaderId;
        private int mfgLineId;
        private int productId;
        private bool? isParentItem;
        private int parentMFGLineId;
        private int topMostParentMFGLineId;
        private decimal? quantity;
        private int mfgUOMId;
        private decimal? actualMfgQuantity;
        private int actualMfgUOMId;
        private decimal? itemCost;
        private decimal? plannedCost;
        private decimal? actualCost;
        private int recipePlanDetailId;
        private bool isActive;
        private string createdBy;
        private DateTime creationDate;
        private string lastUpdatedBy;
        private DateTime lastUpdateDate;
        private string guid;
        private int siteId;
        private bool synchStatus;
        private int masterEntityId;
        private string itemName;
        private string uom;
        private string itemUOM;
        private DateTime mfgDate;
        private bool isComplete;
        private bool notifyingObjectIsChanged;
        private readonly object notifyingObjectIsChangedSyncRoot = new Object();

        /// <summary>
        /// Default constructor
        /// </summary>
        public RecipeManufacturingDetailsDTO()
        {
            log.LogMethodEntry();
            recipeManufacturingDetailId = -1;
            recipeManufacturingHeaderId = -1;
            mfgLineId = -1;
            productId = -1;
            parentMFGLineId = -1;
            topMostParentMFGLineId = -1;
            mfgUOMId = -1;
            actualMfgUOMId = -1;
            siteId = -1;
            masterEntityId = -1;
            isParentItem = null;
            quantity = null;
            actualMfgQuantity = null;
            itemCost = null;
            actualCost = null;
            isActive = true;
            recipePlanDetailId = -1;
            plannedCost = null;
            isComplete = false;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with required data fields.
        /// </summary>
        public RecipeManufacturingDetailsDTO(int recipeManufacturingDetailId, int recipeManufacturingHeaderId, int mfgLineId, 
                                             int productId, bool? isParentItem, int parentMFGLineId, int topMostParentMFGLineId,
                                             decimal? quantity, int mfgUOMId, decimal? actualMfgQuantity, int actualMfgUOMId, 
                                             decimal? itemCost, decimal? plannedCost, decimal? actualCost, 
                                             int recipePlanDetailId , bool isActive, bool isComplete)
            : this()
        {
            log.LogMethodEntry(recipeManufacturingDetailId, recipeManufacturingHeaderId, mfgLineId, productId, isParentItem,
                                parentMFGLineId, topMostParentMFGLineId, quantity, mfgUOMId, actualMfgQuantity, actualMfgUOMId,
                                itemCost, plannedCost, actualCost, recipePlanDetailId, isActive, isComplete);
            this.recipeManufacturingDetailId = recipeManufacturingDetailId;
            this.recipeManufacturingHeaderId = recipeManufacturingHeaderId;
            this.mfgLineId = mfgLineId;
            this.productId = productId;
            this.isParentItem = isParentItem;
            this.parentMFGLineId = parentMFGLineId;
            this.topMostParentMFGLineId = topMostParentMFGLineId;
            this.quantity = quantity;
            this.mfgUOMId = mfgUOMId;
            this.actualMfgQuantity = actualMfgQuantity;
            this.actualMfgUOMId = actualMfgUOMId;
            this.itemCost = itemCost;
            this.actualCost = actualCost;
            this.plannedCost = plannedCost;
            this.recipePlanDetailId = recipePlanDetailId;
            this.isActive = isActive;
            this.isComplete = isComplete;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor all the required data fields.
        /// </summary>
        public RecipeManufacturingDetailsDTO(int recipeManufacturingDetailId, int recipeManufacturingHeaderId, int mfgLineId, 
                                             int productId, bool? isParentItem, int parentMFGLineId, int topMostParentMFGLineId,
                                             decimal? quantity, int mfgUOMId, decimal? actualMfgQuantity, int actualMfgUOMId,
                                             decimal? itemCost, decimal? plannedCost, decimal? actualCost, int recipePlanDetailId, 
                                             bool isActive, string createdBy, DateTime creationDate, string lastUpdatedBy, 
                                             DateTime lastUpdateDate, string guid, int siteId, bool synchStatus,
                                             int masterEntityId, bool isComplete)
            : this(recipeManufacturingDetailId, recipeManufacturingHeaderId, mfgLineId, productId, isParentItem, parentMFGLineId,
                   topMostParentMFGLineId, quantity, mfgUOMId, actualMfgQuantity, actualMfgUOMId, itemCost, plannedCost, actualCost,
                   recipePlanDetailId, isActive, isComplete)
        {
            log.LogMethodEntry(recipeManufacturingDetailId, recipeManufacturingHeaderId, mfgLineId, productId, isParentItem,
                               parentMFGLineId, topMostParentMFGLineId, quantity, mfgUOMId, actualMfgQuantity, actualMfgUOMId,
                               itemCost, plannedCost, actualCost, recipePlanDetailId, isActive, createdBy, creationDate,
                               lastUpdatedBy, lastUpdateDate, guid, siteId, synchStatus, masterEntityId, isComplete);
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
        /// Get/Set method of the RecipeManufacturingDetailId field
        /// </summary>
        public int RecipeManufacturingDetailId { get { return recipeManufacturingDetailId; } set { this.IsChanged = true; recipeManufacturingDetailId = value; } }

        /// <summary>
        /// Get/Set method of the RecipeManufacturingHeaderId field
        /// </summary>
        public int RecipeManufacturingHeaderId { get { return recipeManufacturingHeaderId; } set { this.IsChanged = true; recipeManufacturingHeaderId = value; } }

        /// <summary>
        /// Get/Set method of the MfgLineId field
        /// </summary>
        public int MfgLineId { get { return mfgLineId; } set { this.IsChanged = true; mfgLineId = value; } }

        /// <summary>
        /// Get/Set method of the ProductId field
        /// </summary>
        public int ProductId { get { return productId; } set { this.IsChanged = true; productId = value; } }

        /// <summary>
        /// Get/Set method of the IsParentItem field
        /// </summary>
        public bool? IsParentItem { get { return isParentItem; } set { this.IsChanged = true; isParentItem = value; } }

        /// <summary>
        /// Get/Set method of the ParentMFGLineId field
        /// </summary>
        public int ParentMFGLineId { get { return parentMFGLineId; } set { this.IsChanged = true; parentMFGLineId = value; } }

        /// <summary>
        /// Get/Set method of the TopMostParentMFGLineId field
        /// </summary>
        public int TopMostParentMFGLineId { get { return topMostParentMFGLineId; } set { this.IsChanged = true; topMostParentMFGLineId = value; } }

        /// <summary>
        /// Get/Set method of the MfgUOMId field
        /// </summary>
        public int MfgUOMId { get { return mfgUOMId; } set { this.IsChanged = true; mfgUOMId = value; } }

        /// <summary>
        /// Get/Set method of the Quantity field
        /// </summary>
        public decimal? Quantity { get { return quantity; } set { this.IsChanged = true; quantity = value; } }

        /// <summary>
        /// Get/Set method of the ActualMfgQuantity field
        /// </summary>
        public decimal? ActualMfgQuantity { get { return actualMfgQuantity; } set { this.IsChanged = true; actualMfgQuantity = value; } }

        /// <summary>
        /// Get/Set method of the ActualMfgUOMId field
        /// </summary>
        public int ActualMfgUOMId { get { return actualMfgUOMId; } set { this.IsChanged = true; actualMfgUOMId = value; } }

        /// <summary>
        /// Get/Set method of the itemCost field
        /// </summary>
        public decimal? ItemCost { get { return itemCost; } set {  itemCost = value; } }

        /// <summary>
        /// Get/Set method of the ActualCost field
        /// </summary>
        public decimal? ActualCost { get { return actualCost; } set { actualCost = value; } }

        /// <summary>
        /// Get/Set method of the ActualCost field
        /// </summary>
        public decimal? PlannedCost { get { return plannedCost; } set { plannedCost = value; } }

        /// <summary>
        /// Get/Set method of the RecipePlanDetailId field
        /// </summary>
        public int RecipePlanDetailId { get { return recipePlanDetailId; } set { this.IsChanged = true; recipePlanDetailId = value; } }


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
        /// Get/Set method of the ItemName field
        /// </summary>
        public string ItemName { get { return itemName; } set { itemName = value; } }

        /// <summary>
        /// Get/Set method of the UOM field
        /// </summary>
        public string UOM { get { return uom; } set {  uom = value; } }

        /// <summary>
        /// Get/Set method of the Item UOM field
        /// </summary>
        public string ItemUOM { get { return itemUOM; } set { itemUOM = value; } }

        /// <summary>
        /// Get/Set method of the MFG date field
        /// </summary>
        public DateTime MFGDate { get { return mfgDate; } set { mfgDate = value; } }

        public bool IsComplete { get { return isComplete; } set { this.IsChanged = true; isComplete = value; } }

        /// <summary>
        /// Get/Set method to track changes to the object
        /// </summary>
        public bool IsChanged
        {
            get
            {
                lock (notifyingObjectIsChangedSyncRoot)
                {
                    return notifyingObjectIsChanged || recipeManufacturingDetailId < 0;
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


    public class RecipeManufacturingDetailsDTODefination : ComplexAttributeDefinition
    {
        public RecipeManufacturingDetailsDTODefination(ExecutionContext executionContext, string fieldName) 
            : base(fieldName, typeof(RecipeManufacturingDetailsDTO))
        {
            attributeDefinitionList.Add(new SimpleAttributeDefinition("MFGDate", "MFG Date", new NullableDateTimeValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ItemName", "Item Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Quantity", "Quantity", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ItemUOM", "Item UOM", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ItemCost", "Item Cost", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("PlannedCost", "Planned Cost", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ActualQuantity", "Actual Quantity", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UOM", "UOM", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ActualCost", "Actual Cost", new NullableDecimalValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ActualUOM", "Actual UOM", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CreatedBy", "Created By", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CreationDate", "Creation Date", new NullableDateTimeValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", "Last Updated By", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdateDate", "Last Update Date", new NullableDateTimeValueConverter()));
        }
    }
}
