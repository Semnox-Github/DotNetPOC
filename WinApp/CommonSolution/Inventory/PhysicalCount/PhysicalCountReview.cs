/********************************************************************************************
* Project Name - PhysicalCountReview
* Description  - Bussiness logic of PhysicalCountReview
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        23-Feb-2017   Soumya      Created 
*2.80        19-Aug-2019   Deeksha     Added logger methods.
*2.110.0     04-Jan-2021   Mushahid Faizan  Modified : Web Inventory Changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Inventory.PhysicalCount
{
    /// <summary>
    /// PhysicalCountReview allows to access the PhysicalCountReview details based on the bussiness logic.
    /// </summary>
    public class PhysicalCountReview
    {
        private PhysicalCountReviewDTO physicalCountReviewDTO;
        private ExecutionContext executionContext;
        private InventoryList inventoryList = null;
        private int adjustmentTypeId = -1;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public PhysicalCountReview(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="physicalCountReviewDTO">PhysicalCountReviewDTO</param>
        public PhysicalCountReview(ExecutionContext executionContext, PhysicalCountReviewDTO physicalCountReviewDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, physicalCountReviewDTO);
            this.physicalCountReviewDTO = physicalCountReviewDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Physical Count Reviews
        /// zero else updates the records 
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            InventoryAdjustmentsDTO inventoryAdjustmentsDTO = null;
            InventoryDTO tempDTO = new InventoryDTO();
            string AdjustmentType = "AdjustmentType";
            if (AdjustmentType == "" || AdjustmentType == null)
                AdjustmentType = "Adjustment";
            Validate(sqlTransaction);
            try
            {
                double CurrentQty, AdjQty = 0;
                double newQuantity = 0;
                ProductList ProductList = new ProductList();
                ProductDTO productDTO = ProductList.GetProduct(physicalCountReviewDTO.ProductID);
                int userEnteredUOMId = productDTO.UomId;
                int inventoryUOMId = physicalCountReviewDTO.UOMId;
                if (inventoryUOMId == -1)
                {
                    ProductContainer productContainer = new ProductContainer(executionContext);
                    if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                    {
                        inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == productDTO.ProductId).InventoryUOMId;
                    }
                }
                double factor = 1;
                if (userEnteredUOMId != inventoryUOMId)
                {
                    factor = UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryUOMId);
                }
                CurrentQty = physicalCountReviewDTO.CurrentInventoryQuantity;
                newQuantity = physicalCountReviewDTO.NewQuantity * factor;
                AdjQty = newQuantity - CurrentQty;
                inventoryList = new InventoryList();
                tempDTO = inventoryList.GetInventory(physicalCountReviewDTO.ProductID, physicalCountReviewDTO.LocationID, physicalCountReviewDTO.LotID);
                if (CurrentQty != tempDTO.Quantity)
                {
                    log.Error("Inventory has been updated by other processes after you have refreshed. Please reload before saving changes");
                    string errorMessage = "Inventory has been updated by other processes after you have refreshed. Please reload before saving changes";
                    throw new ValidationException(errorMessage);
                }
                double currentqtyinDTO = 0;

                if (tempDTO != null)
                {
                    currentqtyinDTO = tempDTO.Quantity;
                }
                physicalCountReviewDTO.CurrentInventoryQuantity = AdjQty + currentqtyinDTO;
                tempDTO.Quantity = AdjQty + currentqtyinDTO;

                Semnox.Parafait.Inventory.Inventory inventoryBL = new Semnox.Parafait.Inventory.Inventory(tempDTO, executionContext); //update DTO with qty
                inventoryBL.Save(sqlTransaction);

                if (physicalCountReviewDTO.LotID != -1)
                {
                    double lotQuantity = physicalCountReviewDTO.CurrentInventoryQuantity;
                    Semnox.Parafait.Inventory.InventoryList inventoryList1 = new InventoryList();
                    List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                    searchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, physicalCountReviewDTO.LotID.ToString()));
                    List<InventoryDTO> inventoryLotDTOList1 = inventoryList1.GetAllInventory(searchParams);
                    if (inventoryLotDTOList1 != null)
                    {
                        foreach (InventoryDTO inventoryDTO1 in inventoryLotDTOList1)
                        {
                            if (inventoryDTO1.LocationId != physicalCountReviewDTO.LocationID)
                                lotQuantity += inventoryDTO1.Quantity;
                        }
                    }

                    InventoryLotDTO inventoryLotDTO = new InventoryLotDTO();
                    InventoryLotList inventoryLotList = new InventoryLotList(executionContext);
                    inventoryLotDTO = inventoryLotList.GetInventoryLot(physicalCountReviewDTO.LotID);
                    inventoryLotDTO.BalanceQuantity = lotQuantity;
                    InventoryLotBL inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                    inventoryLotBL.Save(sqlTransaction);
                }

                inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO();
                inventoryAdjustmentsDTO.AdjustmentType = "Adjustment";
                inventoryAdjustmentsDTO.AdjustmentQuantity = AdjQty;
                inventoryAdjustmentsDTO.FromLocationId = physicalCountReviewDTO.LocationID;
                inventoryAdjustmentsDTO.Remarks = "Bulk Adjustment: " + physicalCountReviewDTO.PhysicalCountRemarks;
                inventoryAdjustmentsDTO.ProductId = physicalCountReviewDTO.ProductID;
                inventoryAdjustmentsDTO.LotID = physicalCountReviewDTO.LotID;
                inventoryAdjustmentsDTO.BulkUpdated = true;
                inventoryAdjustmentsDTO.OriginalReferenceId = physicalCountReviewDTO.HistoryId;
                inventoryAdjustmentsDTO.SiteId = executionContext.GetSiteId();
                inventoryAdjustmentsDTO.UOMId = inventoryUOMId;
                if (inventoryAdjustmentsDTO.AdjustmentType == null)
                    inventoryAdjustmentsDTO.AdjustmentTypeId = adjustmentTypeId;
                else
                    inventoryAdjustmentsDTO.AdjustmentTypeId = GetAdjustmentType();
                InventoryAdjustmentsBL inventoryAdjustmentsBL = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                inventoryAdjustmentsBL.Save(sqlTransaction);
            }
            catch (Exception ex)
            {
                log.Error("Adjustment ended with exception: " + ex.ToString());
            }
        }

        public int GetAdjustmentType()
        {
            log.LogMethodEntry();
            List<LookupValuesDTO> lookupValuesDTOList;
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("INVENTORY_ADJUSTMENT_TYPE", executionContext.GetSiteId());
            if (lookupValuesDTOList != null)
            {
                var lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == "Adjustment").FirstOrDefault();
                adjustmentTypeId = lookupValuesDTO != null ? adjustmentTypeId = lookupValuesDTO.LookupValueId : -1;
            }
            if (lookupValuesDTOList == null)
            {
                lookupValuesDTOList = new List<LookupValuesDTO>();
            }
            //To remove adjustment Type 'Wastage' from the list
            if (lookupValuesDTOList.Exists(x => x.LookupValue == "Wastage"))
            {
                int index = lookupValuesDTOList.FindIndex(x => x.LookupValue == "Wastage");
                lookupValuesDTOList.RemoveAt(index);
            }
            log.LogMethodExit();
            return adjustmentTypeId;
        }

        /// <summary>
        /// Validates the InventoryWastageSummaryDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (physicalCountReviewDTO == null)
            {
                log.Error("No products in the selected location");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2703);
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrEmpty(physicalCountReviewDTO.PhysicalCountRemarks) && physicalCountReviewDTO.RemarksMandatory == "Y")
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 201);
                throw new ValidationException(errorMessage);
            }
            if (physicalCountReviewDTO.NewQuantity < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2360);
                throw new ValidationException(errorMessage);
            }
            ProductList ProductList = new ProductList();
            ProductDTO productDTO = ProductList.GetProduct(physicalCountReviewDTO.ProductID);
            if (productDTO.LotControlled && physicalCountReviewDTO.LotID == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2752);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// get PhysicalCountReviewDTO Object
        /// </summary>
        public PhysicalCountReviewDTO GetPhysicalCountReviewDTO
        {
            get { return physicalCountReviewDTO; }
        }
    }

    /// <summary>
    /// Manages the list of PhysicalCountReview
    /// </summary>
    public class PhysicalCountReviewList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private int selectedPhysicalCountID;
        List<PhysicalCountReviewDTO> physicalCountReviewDTOList = new List<PhysicalCountReviewDTO>();
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        public PhysicalCountReviewList(ExecutionContext machineUserContext)
        {
            log.LogMethodEntry(machineUserContext);
            this.executionContext = machineUserContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="physicalCountReviewDTOList"></param>
        public PhysicalCountReviewList(ExecutionContext executionContext, List<PhysicalCountReviewDTO> physicalCountReviewDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, physicalCountReviewDTOList);
            this.physicalCountReviewDTOList = physicalCountReviewDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the PhysicalCountReview list
        /// </summary>
        public List<PhysicalCountReviewDTO> GetAllPhysicalCountReview(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters,
                                                                      int physicalCountId, DateTime startDate, int locationId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, physicalCountId, startDate, locationId);
            PhysicalCountReviewDataHandler physicalCountReviewDataHandler = new PhysicalCountReviewDataHandler(sqlTransaction);
            physicalCountReviewDTOList = physicalCountReviewDataHandler.GetPhysicalCountReviewsList(searchParameters, physicalCountId, startDate, locationId);
            log.LogMethodExit(physicalCountReviewDTOList);
            return physicalCountReviewDTOList;
        }

        /// <summary>
        /// Returns the PhysicalCountReview list
        /// </summary>
        public List<PhysicalCountReviewDTO> GetPhysicalCountReview(string filterText, int PhysicalCountID, DateTime StartDate, int LocationID)
        {
            log.LogMethodEntry(filterText, PhysicalCountID, StartDate, LocationID);
            List<PhysicalCountReviewDTO> physicalCountReviewDTOList = new List<PhysicalCountReviewDTO>();
            try
            {
                PhysicalCountReviewDataHandler physicalCountReviewDataHandler = new PhysicalCountReviewDataHandler();
                physicalCountReviewDTOList = physicalCountReviewDataHandler.GetPhysicalCountReviewList(filterText, PhysicalCountID, StartDate, LocationID, executionContext);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(physicalCountReviewDTOList);
            return physicalCountReviewDTOList;
        }

        /// <summary>
        /// Returns the PhysicalCountReview list
        /// </summary>
        public List<PhysicalCountReviewDTO> GetAllPhysicalCountReviews(string filterText, int physicalCountId, DateTime startDate, int locationId, bool ismodifiedDuringPhysicalCount, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(filterText, physicalCountId, startDate, locationId);
            List<PhysicalCountReviewDTO> physicalCountReviewDTOList = new List<PhysicalCountReviewDTO>();
            List<PhysicalCountReviewDTO> physicalCountReviewDTOListTemp = new List<PhysicalCountReviewDTO>();
            try
            {
                PhysicalCountReviewDataHandler physicalCountReviewDataHandler = new PhysicalCountReviewDataHandler(sqlTransaction);
                physicalCountReviewDTOList = physicalCountReviewDataHandler.GetAllPhysicalCountReviewList(filterText, physicalCountId, startDate, locationId, executionContext, currentPage, pageSize);

                if (physicalCountReviewDTOList != null && physicalCountReviewDTOList.Any())
                {
                    foreach (PhysicalCountReviewDTO physicalCountReviewDTO in physicalCountReviewDTOList)
                    {
                        if (physicalCountReviewDTO.HistoryId == -1)
                        {
                            physicalCountReviewDTO.HistoryId = CreateHistory(physicalCountReviewDTO, physicalCountId, sqlTransaction);
                            physicalCountReviewDTO.StartingQuantity = physicalCountReviewDTO.CurrentInventoryQuantity;
                            physicalCountReviewDTO.AcceptChanges();
                        }
                    }
                    //  populateInventory(physicalCountReviewDTOList);
                }
                if (!ismodifiedDuringPhysicalCount)
                {

                    List<InventoryHistoryDTO> inventoryHistoryList = new List<InventoryHistoryDTO>();
                    InventoryHistoryList inventoryHistorys = new InventoryHistoryList(executionContext);
                    List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParams = new List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>>();
                    searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.MODIFIED_DURING_PHYSICAL_COUNT, "0"));
                    searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.PHYSICAL_COUNT_ID, physicalCountId.ToString()));
                    searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    inventoryHistoryList = inventoryHistorys.GetAllInventoryHistory(searchParams); // Unmodified


                    if (inventoryHistoryList != null && inventoryHistoryList.Any())
                    {

                        physicalCountReviewDTOList = (from w1 in physicalCountReviewDTOList
                                                      where inventoryHistoryList.Any(w2 => w1.HistoryId == w2.Id)
                                                      select w1).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(physicalCountReviewDTOList);
            return physicalCountReviewDTOList;
        }

        /// <summary>
        /// Returns the PhysicalCountReview list
        /// </summary>
        public List<PhysicalCountReviewDTO> GetAllPhysicalCountReviewsDTOList(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters,
                                                                      string advancedSearch, string filterText, int physicalCountId, DateTime startDate, int locationId, bool ismodifiedDuringPhysicalCount, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(filterText, physicalCountId, startDate, locationId);
            List<PhysicalCountReviewDTO> physicalCountReviewDTOList = new List<PhysicalCountReviewDTO>();
            List<PhysicalCountReviewDTO> physicalCountReviewDTOListTemp = new List<PhysicalCountReviewDTO>();
            try
            {
                PhysicalCountReviewDataHandler physicalCountReviewDataHandler = new PhysicalCountReviewDataHandler(sqlTransaction);
                physicalCountReviewDTOList = physicalCountReviewDataHandler.GetAllPhysicalCountReviewDTOList(searchParameters, advancedSearch, filterText, physicalCountId, startDate, locationId, executionContext, currentPage, pageSize);

                if (physicalCountReviewDTOList != null && physicalCountReviewDTOList.Any())
                {
                    foreach (PhysicalCountReviewDTO physicalCountReviewDTO in physicalCountReviewDTOList)
                    {
                        if (physicalCountReviewDTO.HistoryId == -1)
                        {
                            physicalCountReviewDTO.HistoryId = CreateHistory(physicalCountReviewDTO, physicalCountId, sqlTransaction);
                            physicalCountReviewDTO.StartingQuantity = physicalCountReviewDTO.CurrentInventoryQuantity;
                            physicalCountReviewDTO.AcceptChanges();
                        }
                    }
                    //  populateInventory(physicalCountReviewDTOList);
                }
                if (!ismodifiedDuringPhysicalCount)
                {

                    List<InventoryHistoryDTO> inventoryHistoryList = new List<InventoryHistoryDTO>();
                    InventoryHistoryList inventoryHistorys = new InventoryHistoryList(executionContext);
                    List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParams = new List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>>();
                    searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.MODIFIED_DURING_PHYSICAL_COUNT, "0"));
                    searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.PHYSICAL_COUNT_ID, physicalCountId.ToString()));
                    searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    inventoryHistoryList = inventoryHistorys.GetAllInventoryHistory(searchParams); // Unmodified


                    if (inventoryHistoryList != null && inventoryHistoryList.Any())
                    {

                        physicalCountReviewDTOList = (from w1 in physicalCountReviewDTOList
                                                      where inventoryHistoryList.Any(w2 => w1.HistoryId == w2.Id)
                                                      select w1).ToList();
                    }
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
            log.LogMethodExit(physicalCountReviewDTOList);
            return physicalCountReviewDTOList;
        }

        public int CreateHistory(PhysicalCountReviewDTO physicalCountReviewDTO, int physicalCountId, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(physicalCountReviewDTO, SQLTrx);
            int historyId = -1;
            try
            {
                List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
                InventoryDTO inventory = new InventoryDTO();
                InventoryList inventoryList = new InventoryList(executionContext);
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, physicalCountReviewDTO.LocationID.ToString()));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, physicalCountReviewDTO.LotID.ToString()));
                inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, physicalCountReviewDTO.ProductID.ToString()));
                if (physicalCountReviewDTO.LotID == -1)
                {
                    inventory = inventoryList.GetInventory(physicalCountReviewDTO.ProductID, physicalCountReviewDTO.LocationID, -1, SQLTrx);
                }
                else
                {
                    inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams, true, SQLTrx);
                    if (inventoryDTOList != null && inventoryDTOList.Any())
                    {
                        inventory = inventoryDTOList[0];
                    }
                }
                if (inventory != null)
                {
                    InventoryHistoryDTO inventoryHistoryDTO = new InventoryHistoryDTO();
                    inventoryHistoryDTO.ProductId = inventory.ProductId;
                    inventoryHistoryDTO.LocationId = inventory.LocationId;
                    inventoryHistoryDTO.PhysicalCountId = physicalCountId;
                    //inventoryHistoryDTO.PhysicalCountId = SelectedPhysicalCountID;
                    inventoryHistoryDTO.Timestamp = inventory.Timestamp;
                    inventoryHistoryDTO.LastupdatedUserid = inventory.Lastupdated_userid;
                    inventoryHistoryDTO.AllocatedQuantity = inventory.AllocatedQuantity;
                    inventoryHistoryDTO.Quantity = inventory.Quantity;
                    inventoryHistoryDTO.UOMId = inventory.UOMId;
                    inventoryHistoryDTO.SiteId = executionContext.GetSiteId();
                    inventoryHistoryDTO.LotId = inventory.LotId;
                    inventoryHistoryDTO.ReceivePrice = inventory.ReceivePrice;
                    inventoryHistoryDTO.InitialCount = true;
                    InventoryHistory inventoryHistory = new InventoryHistory(executionContext, inventoryHistoryDTO);
                    historyId = inventoryHistory.Save(SQLTrx);
                }

            }
            catch (Exception ex)
            {
                log.Debug(ex);
            }
            log.LogMethodExit(historyId);
            return historyId;
        }


        /// <summary>
        /// Returns the no of Requisition matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>       
        public int GetPhysicalCountReviewsCount(string filterText, int physicalCountId, DateTime startDate, int locationId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(filterText, physicalCountId, startDate, locationId, sqlTransaction);
            PhysicalCountReviewDataHandler physicalCountReviewDataHandler = new PhysicalCountReviewDataHandler(sqlTransaction);
            int requisitionsCount = physicalCountReviewDataHandler.GetPhysicalCountReviewsCount(filterText, physicalCountId, startDate, locationId);
            log.LogMethodExit(requisitionsCount);
            return requisitionsCount;
        }

        /// <summary>
        /// Returns the no of Requisition matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>       
        public int GetPhysicalCountReviewsDTOListCount(List<KeyValuePair<PhysicalCountReviewDTO.SearchByPhysicalCountReviewParameters, string>> searchParameters,
                                                                      string advancedSearch, string filterText, int physicalCountId, DateTime startDate, int locationId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(filterText, physicalCountId, startDate, locationId, sqlTransaction);
            PhysicalCountReviewDataHandler physicalCountReviewDataHandler = new PhysicalCountReviewDataHandler(sqlTransaction);
            int requisitionsCount = physicalCountReviewDataHandler.GetPhysicalCountReviewsDTOListCount(searchParameters, advancedSearch, filterText, physicalCountId,
                                                                                                       startDate, locationId, executionContext);
            log.LogMethodExit(requisitionsCount);
            return requisitionsCount;
        }

        /// <summary>
        /// This method should be called from method Save().
        /// Saves the PhysicalCountReviewList
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(int physicalCountId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            for (int i = 0; i < physicalCountReviewDTOList.Count; i++)
            {
                var physicalCountReviewDTO = physicalCountReviewDTOList[i];
                if (physicalCountReviewDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    PhysicalCountReview physicalCountReviewBL = new PhysicalCountReview(executionContext, physicalCountReviewDTO);

                    InventoryPhysicalCount inventoryPhysicalCount = new InventoryPhysicalCount(executionContext);
                    InventoryPhysicalCountDTO inventoryPhysicalCountDTO = new InventoryPhysicalCountDTO();
                    selectedPhysicalCountID = physicalCountId;
                    inventoryPhysicalCountDTO = inventoryPhysicalCount.GetInventoryPhysicalCountByID(selectedPhysicalCountID, sqlTransaction);
                    int locationID = inventoryPhysicalCountDTO.LocationId;
                    List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
                    InventoryList inventoryList = new InventoryList();
                    List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, locationID.ToString()));
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.MASS_UPDATE_ALLOWED, "Y"));
                    inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams, false, sqlTransaction);

                    if (inventoryDTOList != null)
                    {
                        if (CreateInventoryHistory(inventoryDTOList, sqlTransaction) == false)
                        {
                            return;
                        }
                        ClosePhysicalCount(inventoryPhysicalCountDTO, sqlTransaction);
                    }
                    else
                    {
                        ClosePhysicalCount(inventoryPhysicalCountDTO, sqlTransaction);
                    }

                    physicalCountReviewBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving PhysicalCountReviewDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("PhysicalCountReviewDTO", physicalCountReviewDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        private bool CreateInventoryHistory(List<InventoryDTO> inventoryDTOLists, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inventoryDTOLists, sqlTransaction);
            try
            {
                List<InventoryHistoryDTO> inventoryHistoryList = new List<InventoryHistoryDTO>();
                InventoryHistoryList inventoryHistorys = new InventoryHistoryList(executionContext);
                List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>> searchParams = new List<KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>>();
                searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.MODIFIED_DURING_PHYSICAL_COUNT, "0"));
                searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.PHYSICAL_COUNT_ID, selectedPhysicalCountID.ToString()));
                searchParams.Add(new KeyValuePair<InventoryHistoryDTO.SearchByInventoryHistoryParameters, string>(InventoryHistoryDTO.SearchByInventoryHistoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                inventoryHistoryList = inventoryHistorys.GetAllInventoryHistory(searchParams); // Unmodified

                foreach (InventoryDTO inventoryDTO in inventoryDTOLists)
                {
                    if (inventoryHistoryList == null
                        || (inventoryHistoryList != null
                            && inventoryHistoryList.Exists(x => x.ProductId == inventoryDTO.ProductId
                                                                && x.LotId == inventoryDTO.LotId) == false)
                       )
                    {
                        SaveInventoryHistory(inventoryDTO, sqlTransaction);
                    }

                }
                log.LogMethodExit();
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Ends-CreateInventoryHistory() method with exception: " + ex.ToString());
                return false;
            }
        }

        private void SaveInventoryHistory(InventoryDTO inventoryDTO, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(inventoryDTO);
            InventoryHistoryDTO inventoryHistoryDTO = new InventoryHistoryDTO(-1,
                                                                              inventoryDTO.ProductId,
                                                                              inventoryDTO.LocationId,
                                                                              selectedPhysicalCountID,
                                                                              inventoryDTO.Quantity,
                                                                              inventoryDTO.Timestamp,
                                                                              inventoryDTO.AllocatedQuantity,
                                                                              inventoryDTO.LotId,
                                                                              inventoryDTO.ReceivePrice,
                                                                              false,
                                                                              inventoryDTO.UOMId);
            InventoryHistory inventoryHistory = new InventoryHistory(executionContext, inventoryHistoryDTO);
            inventoryHistory.Save(SQLTrx);
            log.LogMethodExit();
        }

        /// <summary>
        /// Close Physical Counting Process
        /// </summary>
        /// <param name="inventoryPhysicalCountDTO"></param>
        /// <param name="sqlTransaction"></param>
        private void ClosePhysicalCount(InventoryPhysicalCountDTO inventoryPhysicalCountDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inventoryPhysicalCountDTO, sqlTransaction);
            try
            {
                inventoryPhysicalCountDTO.Status = "Closed";
                inventoryPhysicalCountDTO.EndDate = ServerDateTime.Now;
                inventoryPhysicalCountDTO.ClosedBy = executionContext.GetUserId();
                InventoryPhysicalCount inventoryPhysicalCount = new InventoryPhysicalCount(executionContext, inventoryPhysicalCountDTO);
                inventoryPhysicalCount.Save(sqlTransaction);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error("Ends-ClosePhysicalCount() Method with exception: " + ex.ToString());
            }
        }

        /// <summary>
        /// This method is will return Sheet object for iInventoryPhysicalCountDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(int physicalCountId, int locationId, DateTime startDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            PhysicalCountReviewDataHandler physicalCountReviewDataHandler = new PhysicalCountReviewDataHandler(sqlTransaction);
            physicalCountReviewDTOList = physicalCountReviewDataHandler.GetAllPhysicalCountReviewList(string.Empty, physicalCountId, startDate, locationId, executionContext);


            PhysicalCountReviewExcelDTODefinition inventoryPhysicalCountExcelDTODefinition = new PhysicalCountReviewExcelDTODefinition(executionContext, "");
            ///Building headers from PhysicalCountReviewExcelDTODefinition
            inventoryPhysicalCountExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (physicalCountReviewDTOList != null && physicalCountReviewDTOList.Any())
            {
                foreach (PhysicalCountReviewDTO inventoryPhysicalCountDTO in physicalCountReviewDTOList)
                {
                    inventoryPhysicalCountExcelDTODefinition.Configure(inventoryPhysicalCountDTO);

                    Row row = new Row();
                    inventoryPhysicalCountExcelDTODefinition.Serialize(row, inventoryPhysicalCountDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }
        public Sheet BulkUpload(Sheet sheet, int physicalCountId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            PhysicalCountReviewExcelDTODefinition inventoryPhysicalCountExcelDTODefinition = new PhysicalCountReviewExcelDTODefinition(executionContext, "");
            List<PhysicalCountReviewDTO> rowInventoryPhysicalCountDTOList = new List<PhysicalCountReviewDTO>();
            Sheet responseSheet = null;

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    PhysicalCountReviewDTO rowInventoryPhysicalCountDTO = (PhysicalCountReviewDTO)inventoryPhysicalCountExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
                    rowInventoryPhysicalCountDTOList.Add(rowInventoryPhysicalCountDTO);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                try
                {
                    if (rowInventoryPhysicalCountDTOList != null && rowInventoryPhysicalCountDTOList.Any())
                    {

                        PhysicalCountReviewList physicalCountReviewList = new PhysicalCountReviewList(executionContext, rowInventoryPhysicalCountDTOList);//note
                        physicalCountReviewList.Save(physicalCountId, sqlTransaction);
                        rowInventoryPhysicalCountDTOList = new List<PhysicalCountReviewDTO>();
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = string.Empty;
                    string seperator = string.Empty;
                    //In case of exception we add a cell to status object with cell value saved
                    if (responseSheet == null)
                    {
                        responseSheet = new Sheet();
                        responseSheet.AddRow(sheet[0]);
                        responseSheet[0].AddCell(new Cell(MessageContainerList.GetMessage(executionContext, "Status")));
                    }
                    responseSheet.AddRow(sheet[i]);

                    if (ex is ValidationException)
                    {
                        foreach (var validationError in (ex as ValidationException).ValidationErrorList)
                        {
                            errorMessage += seperator;
                            errorMessage += validationError.Message;
                            seperator = ", ";
                        }
                    }
                    else
                    {
                        errorMessage = ex.Message;
                    }
                    responseSheet[responseSheet.Rows.Count - 1].AddCell(new Cell("Failed: " + errorMessage));
                    log.Error(errorMessage);
                    log.LogVariableState("Row", sheet[i]);
                    rowInventoryPhysicalCountDTOList = new List<PhysicalCountReviewDTO>();
                    continue;
                }
            }
            log.LogMethodExit(responseSheet);
            return responseSheet;
        }
    }
}