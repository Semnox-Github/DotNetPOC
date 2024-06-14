/********************************************************************************************
* Project Name - Inventory 
* Description  - Bussiness logic of Inventory Wastage Summary
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*2.110.0     31-Dec-2020   Abhishek       Created
********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory
{
    public class InventoryWastageSummaryBL
    {
        private logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        InventoryWastageSummaryDTO inventoryWastageSummaryDTO = null;
        private ExecutionContext executionContext;
        private int wastageLocationId = -1;
        private int wastageDocumentTypeId = -1;
        private int wastageAdjustmentTypeId = -1;

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="inventoryAdjustmentsDTO">Parameter of the type InventoryAdjustmentsDTO</param>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryWastageSummaryBL(InventoryWastageSummaryDTO inventoryWastageSummaryDTO, ExecutionContext executionContext)
        {
            log.LogMethodEntry(inventoryWastageSummaryDTO, executionContext);
            this.executionContext = executionContext;
            this.inventoryWastageSummaryDTO = inventoryWastageSummaryDTO;
            log.LogMethodExit();
        }

        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            InventoryDTO inventoryDTO = new InventoryDTO();
            List<InventoryDTO> inventoryDTOList = null;
            InventoryList inventoryList = null;
            Validate(sqlTransaction);
            wastageLocationId = GetWastageLocationtId();
            wastageDocumentTypeId = GetWastageDocumentTypeId();
            wastageAdjustmentTypeId = GetWastageAdjustmentTypeID();
            InventoryAdjustmentsDTO inventoryAdjustmentsDTO = null;
            Inventory inventoryBL = new Inventory(executionContext);
            try
            {
                inventoryList = new InventoryList();
                decimal currentQtyInDTO = inventoryWastageSummaryDTO.AvailableQuantity - inventoryWastageSummaryDTO.WastageQuantity;
                if (inventoryWastageSummaryDTO.LotId != -1)
                {
                    List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventorySearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, inventoryWastageSummaryDTO.ProductId.ToString()));
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, inventoryWastageSummaryDTO.LocationId.ToString()));
                    inventorySearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, inventoryWastageSummaryDTO.LotId.ToString()));
                    inventoryDTOList = inventoryList.GetAllInventory(inventorySearchParams, true, sqlTransaction);
                    if (inventoryDTOList != null && inventoryDTOList.Count > 0)
                    {
                        inventoryDTO = inventoryDTOList[0];
                    }
                }
                else
                {
                    inventoryDTO = inventoryList.GetInventory(inventoryWastageSummaryDTO.ProductId, inventoryWastageSummaryDTO.LocationId, inventoryWastageSummaryDTO.LotId);
                }

                if (inventoryWastageSummaryDTO.AdjustmentId == -1)
                {
                    inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO();
                    if (inventoryDTO != null)
                    {
                        inventoryDTO.Quantity = Convert.ToDouble(currentQtyInDTO);
                        inventoryDTO.UOMId = inventoryWastageSummaryDTO.UomId;
                        inventoryBL = new Inventory(inventoryDTO, executionContext);
                        if (inventoryBL.Save(sqlTransaction) <= 0)
                        {
                            log.Error("inventoryBL.Save() : Inventory Quantity update failed");
                            throw new Exception("Inventory Quantity update failed");
                        }
                    }
                    if (inventoryDTO.LotId != -1)
                    {
                        double lotQuantity = inventoryDTO.Quantity;
                        InventoryList inventoryList1 = new InventoryList();
                        List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                        searchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, inventoryDTO.LotId.ToString()));
                        List<InventoryDTO> inventoryLotDTOList = inventoryList1.GetAllInventory(searchParams);
                        if (inventoryLotDTOList != null)
                        {
                            foreach (InventoryDTO tempInventoryDTO in inventoryLotDTOList)
                            {
                                // Skip the quantity from the wastage location while calculating lot quantity from other location
                                if (tempInventoryDTO.LocationId != inventoryDTO.LocationId && tempInventoryDTO.LocationId != wastageLocationId)
                                    lotQuantity += tempInventoryDTO.Quantity;
                            }
                        }
                        inventoryDTO.InventoryLotDTO.BalanceQuantity = lotQuantity;
                        inventoryDTO.InventoryLotDTO.UOMId = inventoryDTO.UOMId;
                        InventoryLotBL inventoryLotBL = new InventoryLotBL(inventoryDTO.InventoryLotDTO, executionContext);
                        inventoryLotBL.Save(sqlTransaction);
                    }

                    // Saving inventory to wastage location : If same product exists then update the wastage quantity eg: previous day added same product to wastage location
                    inventoryDTO = inventoryList.GetInventory(inventoryWastageSummaryDTO.ProductId, wastageLocationId, inventoryWastageSummaryDTO.LotId);
                    if (inventoryDTO != null)
                    {
                        inventoryDTO.Quantity = inventoryDTO.Quantity + Convert.ToDouble(inventoryWastageSummaryDTO.WastageQuantity);
                    }
                    else  // add new record for wastage location
                    {
                        inventoryDTO = new InventoryDTO(inventoryWastageSummaryDTO.ProductId, wastageLocationId, Convert.ToDouble(inventoryWastageSummaryDTO.WastageQuantity), ServerDateTime.Now,
                        0, inventoryWastageSummaryDTO.Remarks, inventoryWastageSummaryDTO.LotId, 0, -1, "", inventoryWastageSummaryDTO.ProductCode,
                        inventoryWastageSummaryDTO.ProductDescription, "Y", null, "", "", "", "", 0, 0, "", inventoryWastageSummaryDTO.LocationName, "", "", "", inventoryWastageSummaryDTO.UomId);
                    }
                    LocationBL locationBL = new LocationBL(executionContext, wastageLocationId, sqlTransaction);
                    inventoryBL = new Inventory(inventoryDTO, executionContext);
                    inventoryBL.Save(sqlTransaction);
                    inventoryAdjustmentsDTO.AdjustmentQuantity = Convert.ToDouble(inventoryWastageSummaryDTO.WastageQuantity) * -1; // Save as negative quantity
                    inventoryAdjustmentsDTO.AdjustmentType = inventoryDTO.LocationName;
                    inventoryAdjustmentsDTO.ProductId = inventoryWastageSummaryDTO.ProductId;
                    inventoryAdjustmentsDTO.Remarks = inventoryWastageSummaryDTO.Remarks;
                    inventoryAdjustmentsDTO.Timestamp = ServerDateTime.Now;
                    inventoryAdjustmentsDTO.FromLocationId = inventoryWastageSummaryDTO.LocationId;
                    inventoryAdjustmentsDTO.UserId = executionContext.GetUserId();
                    inventoryAdjustmentsDTO.UOMId = inventoryWastageSummaryDTO.UomId;
                    inventoryAdjustmentsDTO.DocumentTypeID = wastageDocumentTypeId; // Applicability - WADJ - Wastage Adjustment
                    inventoryAdjustmentsDTO.ToLocationId = wastageLocationId; // Always go to the location wastage 
                    int adjustmentTypeId = inventoryWastageSummaryDTO.AdjustmentTypeId;
                    if (adjustmentTypeId != -1)
                    {
                        inventoryAdjustmentsDTO.AdjustmentTypeId = adjustmentTypeId;
                    }
                    else
                    {
                        inventoryAdjustmentsDTO.AdjustmentTypeId = wastageAdjustmentTypeId;
                    }
                    inventoryAdjustmentsDTO.LotID = inventoryWastageSummaryDTO.LotId;
                }
                InventoryAdjustmentsBL inventoryAdjustmentsBL = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                inventoryAdjustmentsBL.Save(sqlTransaction);
            }

            catch (Exception ex)
            {
                log.Error("Error in save. Exception:" + ex.ToString());
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Save Error" + " " + ex.Message);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the InventoryWastageSummaryDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryWastageSummaryDTO == null && !HasChanges())
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 371);
                throw new ValidationException(errorMessage);
            }
            if (inventoryWastageSummaryDTO.WastageQuantity == 0 && inventoryWastageSummaryDTO.AvailableQuantity == 0 &&
                inventoryWastageSummaryDTO.TodaysWastageQuantity == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2452);
                throw new ValidationException(errorMessage);
            }

            if (inventoryWastageSummaryDTO.WastageQuantity <= 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Please enter valid wastage quantity");
                throw new ValidationException(errorMessage);
            }
            decimal currentQtyInDTO = inventoryWastageSummaryDTO.AvailableQuantity - inventoryWastageSummaryDTO.WastageQuantity;
            if (currentQtyInDTO < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "Wastage quantity should not exceed the available quantity");
                throw new ValidationException(errorMessage);
            }
        }

        private bool HasChanges()
        {
            log.LogMethodEntry();
            bool result = false;
            try
            {
                List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
                log.LogVariableState("inventoryWastageSummaryDTOList", inventoryWastageSummaryDTOList);
                if (inventoryWastageSummaryDTO.IsChanged)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// This method gets the adjustment type Id for the type "Wastage" 
        /// </summary>
        private int GetWastageAdjustmentTypeID()
        {
            try
            {
                log.LogMethodEntry();
                List<LookupValuesDTO> lookupValuesDTOList;
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("INVENTORY_ADJUSTMENT_TYPE", executionContext.GetSiteId());
                log.LogVariableState("lookupValuesDTOList :", lookupValuesDTOList);
                if (lookupValuesDTOList != null)
                {
                    var lookupValuesDTO = lookupValuesDTOList.Where(x => x.LookupValue == "Wastage").FirstOrDefault();
                    wastageAdjustmentTypeId = lookupValuesDTO != null ? wastageAdjustmentTypeId = lookupValuesDTO.LookupValueId : -1;
                }
                log.Debug("wastageAdjustmentTypeId : " + wastageAdjustmentTypeId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(wastageAdjustmentTypeId);
            return wastageAdjustmentTypeId;
        }

        /// <summary>
        /// This method gets the location id for the inventory location wastage of type department 
        /// </summary>
        private int GetWastageLocationtId()
        {
            try
            {
                log.LogMethodEntry();
                LocationList locationlist = new LocationList(executionContext);
                LocationDTO wastageLocationDTO = locationlist.GetWastageLocationDTO();
                if (wastageLocationDTO != null)
                {
                    wastageLocationId = wastageLocationDTO.LocationId;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(wastageLocationId);
            return wastageLocationId;
        }

        /// <summary>
        /// This method gets the document type id for Wastage type
        /// </summary>
        private int GetWastageDocumentTypeId()
        {
            try
            {
                log.LogMethodEntry();
                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType("Wastage Adjustment");
                log.LogVariableState("inventoryDocumentTypeDTO :", inventoryDocumentTypeDTO);
                if (inventoryDocumentTypeDTO != null)
                {
                    wastageDocumentTypeId = inventoryDocumentTypeDTO.DocumentTypeId;
                }
                log.Debug("wastageDocumentTypeId : " + wastageDocumentTypeId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
            return wastageDocumentTypeId;
        }
    }

    /// <summary>
    /// Manages the list of Inventory Wastages
    /// </summary>
    public class InventoryWastageSummaryListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public InventoryWastageSummaryListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="inventoryWastageSummaryDTOList"></param>
        public InventoryWastageSummaryListBL(ExecutionContext executionContext, List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, inventoryWastageSummaryDTOList);
            this.inventoryWastageSummaryDTOList = inventoryWastageSummaryDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method should be called from method Save().
        /// Saves the InventoryWastageSummaryListBL
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            var query = inventoryWastageSummaryDTOList.GroupBy(x => new { x.ProductId, x.LocationId })
                .Where(g => g.Count() > 1)
                .Select(y => y.Key)
                .ToList();
            if (query.Count > 0)
            {
                log.Debug("Duplicate entries detail : " + query[0]);
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2441);
                throw new ValidationException(errorMessage, "Wastage", "Product to Location", errorMessage);
            }
            for (int i = 0; i < inventoryWastageSummaryDTOList.Count; i++)
            {
                var inventoryWastageSummaryDTO = inventoryWastageSummaryDTOList[i];
                if (inventoryWastageSummaryDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    InventoryWastageSummaryBL inventoryWastageSummaryBL = new InventoryWastageSummaryBL(inventoryWastageSummaryDTO, executionContext);
                    inventoryWastageSummaryBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving InventoryWastageSummaryDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("InventoryWastageSummaryDTO", inventoryWastageSummaryDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
