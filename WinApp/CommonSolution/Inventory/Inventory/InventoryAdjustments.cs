/********************************************************************************************
 * Project Name - Inventory Adjustments
 * Description  - Bussiness logic of Adjustments
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By       Remarks          
 *********************************************************************************************
 *1.00        12-Aug-2016    Amaresh          Created 
 *2.70.2      09-Jul-2019    Deeksha          Modifications as per three tier standard
 *2.70.2      29-Dec-2019    Girish Kundar    Modified : added method GetAllInventoryWastageSummaryDTOList()
 *2.110.0    28-Dec-2020     Mushahid Faizan  Modifieid : web Inventory changes
 *2.110.4     01-Oct-2021    Guru S A         Physical count performance fixes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Inventory.Location;
using Semnox.Parafait.Product;
using static Semnox.Parafait.Transaction.Inventory;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory Adjustments will creates and modifies the inventory Adjustments
    /// </summary>
    public class InventoryAdjustmentsBL
    {
        private InventoryAdjustmentsDTO inventoryAdjustmentsDTO;
        private logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext; //= ExecutionContext.GetExecutionContext();
        private Utilities _utilities;

        /// <summary>
        /// Constructor with executionContext as a parameter
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryAdjustmentsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            inventoryAdjustmentsDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="inventoryAdjustmentsDTO">Parameter of the type InventoryAdjustmentsDTO</param>
        /// <param name="executionContext">ExecutionContext</param>
        public InventoryAdjustmentsBL(InventoryAdjustmentsDTO inventoryAdjustmentsDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(inventoryAdjustmentsDTO, executionContext);
            this.inventoryAdjustmentsDTO = inventoryAdjustmentsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the utilities and DTO parameter
        /// </summary>
        /// <param name="utilities">Parameter utilities</param>
        /// <param name="inventoryAdjustmentsDTO">Parameter of the type InventoryAdjustmentsDTO</param>
        public InventoryAdjustmentsBL(Utilities utilities, InventoryAdjustmentsDTO inventoryAdjustmentsDTO)
        {
            log.LogMethodEntry(utilities, inventoryAdjustmentsDTO);
            _utilities = utilities;
            executionContext = utilities.ExecutionContext;
            this.inventoryAdjustmentsDTO = inventoryAdjustmentsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Inventory Adjustments
        /// Inventory Adjustments will be inserted if AdjustmentId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            if (inventoryAdjustmentsDTO.AdjustmentId < 0)
            {
                inventoryAdjustmentsDTO = inventoryAdjustmentsDataHandler.InsertInventoryAdjustments(inventoryAdjustmentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                inventoryAdjustmentsDTO.AcceptChanges();
                InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "Inventory Adjustments Inserted",
                                                            inventoryAdjustmentsDTO.Guid, false, executionContext.GetSiteId(), "InventoryAdjustments", -1,
                                                            inventoryAdjustmentsDTO.AdjustmentId.ToString(), -1, executionContext.GetUserId(),
                                                            ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, inventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            else
            {
                if (inventoryAdjustmentsDTO.IsChanged)
                {
                    inventoryAdjustmentsDTO = inventoryAdjustmentsDataHandler.UpdateInventoryAdjustments(inventoryAdjustmentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    inventoryAdjustmentsDTO.AcceptChanges();
                    InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "Inventory Adjustments Updated",
                                                            inventoryAdjustmentsDTO.Guid, false, executionContext.GetSiteId(), "InventoryAdjustments", -1,
                                                            inventoryAdjustmentsDTO.AdjustmentId.ToString(), -1, executionContext.GetUserId(),
                                                             ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                    InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, inventoryActivityLogDTO);
                    inventoryActivityLogBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        public void SaveInventory(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            InventoryList inventoryList = new InventoryList();
            InventoryDTO inventoryUpdateDTO = new InventoryDTO();
            List<InventoryDTO> inventoryFromDTOList = new List<InventoryDTO>();
            List<InventoryDTO> inventoryToDTOList = new List<InventoryDTO>();
            Inventory inventoryBL = null;
            InventoryLotBL inventoryLotBL = null;
            InventoryLotDTO inventoryLotDTO = null;
            ValidateInventory(sqlTransaction);
            ProductList productList = new ProductList(executionContext);
            ProductDTO productDTO = productList.GetProduct(inventoryAdjustmentsDTO.ProductId);

            if (inventoryAdjustmentsDTO.AdjustmentType == AdjustmentTypes.Transfer.ToString())
            {
                InventoryDTO tempFromDTO = new InventoryDTO();
                InventoryDTO tempToDTO = new InventoryDTO();//inventoryList.GetInventory(inventoryAdjustmentsDTO.ProductId, inventoryAdjustmentsDTO.ToLocationId, inventoryAdjustmentsDTO.LotID, sqlTransaction);
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchToParameters = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                searchToParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchToParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, inventoryAdjustmentsDTO.ProductId.ToString()));
                searchToParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, inventoryAdjustmentsDTO.ToLocationId.ToString()));
                inventoryToDTOList = inventoryList.GetAllInventory(searchToParameters, true, sqlTransaction);

                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchFromParameters = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                searchFromParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                searchFromParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, inventoryAdjustmentsDTO.ProductId.ToString()));
                searchFromParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, inventoryAdjustmentsDTO.FromLocationId.ToString()));
                if (inventoryAdjustmentsDTO.LotID > -1)
                {
                    searchFromParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, inventoryAdjustmentsDTO.LotID.ToString()));
                }
                tempFromDTO = inventoryList.GetAllInventory(searchFromParameters, true, sqlTransaction).FirstOrDefault();
                if (inventoryAdjustmentsDTO.AdjustmentQuantity > tempFromDTO.Quantity)
                {
                    string message = MessageContainerList.GetMessage(executionContext, 2445);//Cannot transfer more than the available quantity
                    throw new ValidationException(message);
                }
                double currentTolocationQty = 0;

                if (inventoryToDTOList != null && inventoryToDTOList.Any())
                {
                    if (productDTO != null && productDTO.LotControlled && productDTO.IssuingApproach == "FIFO" && tempFromDTO.InventoryLotDTO != null)
                    {
                        tempToDTO = inventoryToDTOList.FirstOrDefault();
                        inventoryLotDTO = new InventoryLotDTO(-1, string.Empty, inventoryAdjustmentsDTO.AdjustmentQuantity, inventoryAdjustmentsDTO.AdjustmentQuantity,
                                           tempFromDTO.InventoryLotDTO.ReceivePrice, tempFromDTO.InventoryLotDTO.PurchaseOrderReceiveLineId, tempFromDTO.InventoryLotDTO.Expirydate,
                                           true, string.Empty, tempFromDTO.InventoryLotDTO.UOMId);
                        inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                        inventoryLotBL.Save(sqlTransaction);
                        inventoryUpdateDTO = tempToDTO;
                        inventoryUpdateDTO.InventoryId = -1;
                        inventoryUpdateDTO.LotId = inventoryLotBL.GetInventoryLot.LotId;
                        currentTolocationQty = inventoryAdjustmentsDTO.AdjustmentQuantity;
                        inventoryUpdateDTO.Quantity = currentTolocationQty;
                        //}
                    }
                    else if (productDTO != null && productDTO.LotControlled && productDTO.IssuingApproach == "FEFO" && tempFromDTO.InventoryLotDTO != null)
                    {
                        int count = inventoryToDTOList.Where(y => y.InventoryLotDTO.LotId > -1 && tempFromDTO.InventoryLotDTO.Expirydate == tempFromDTO.InventoryLotDTO.Expirydate).Count();
                        if (count > 0)
                        {
                            tempToDTO = inventoryToDTOList.Where(y => y.InventoryLotDTO.LotId > -1 && tempFromDTO.InventoryLotDTO.Expirydate == tempFromDTO.InventoryLotDTO.Expirydate).LastOrDefault();
                            InventoryLotList inventoryLotList = new InventoryLotList(executionContext);
                            inventoryLotDTO = inventoryLotList.GetInventoryLot(tempToDTO.LotId, sqlTransaction);
                            inventoryLotDTO.BalanceQuantity = inventoryLotDTO.BalanceQuantity + inventoryAdjustmentsDTO.AdjustmentQuantity;
                            inventoryLotDTO.IsChanged = true;
                            inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                            inventoryLotBL.Save(sqlTransaction);
                            inventoryUpdateDTO = tempToDTO;
                            currentTolocationQty = inventoryAdjustmentsDTO.AdjustmentQuantity + inventoryUpdateDTO.Quantity;
                            inventoryUpdateDTO.Quantity = currentTolocationQty;
                        }
                        else
                        {
                            tempToDTO = inventoryToDTOList.FirstOrDefault();
                            inventoryLotDTO = new InventoryLotDTO(-1, string.Empty, inventoryAdjustmentsDTO.AdjustmentQuantity, inventoryAdjustmentsDTO.AdjustmentQuantity,
                                              tempFromDTO.InventoryLotDTO.ReceivePrice, tempFromDTO.InventoryLotDTO.PurchaseOrderReceiveLineId, tempFromDTO.InventoryLotDTO.Expirydate,
                                              true, string.Empty, tempFromDTO.InventoryLotDTO.UOMId);
                            inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                            inventoryLotBL.Save(sqlTransaction);
                            inventoryUpdateDTO = tempToDTO;
                            inventoryUpdateDTO.InventoryId = -1;
                            inventoryUpdateDTO.LotId = inventoryLotBL.GetInventoryLot.LotId;
                            currentTolocationQty = inventoryAdjustmentsDTO.AdjustmentQuantity;
                            inventoryUpdateDTO.Quantity = currentTolocationQty;
                        }
                    }
                    else
                    {
                        tempToDTO = inventoryToDTOList.FirstOrDefault();
                        if (productDTO != null && productDTO.LotControlled && tempFromDTO.InventoryLotDTO != null)
                        {
                            inventoryLotDTO = new InventoryLotDTO(-1, string.Empty, inventoryAdjustmentsDTO.AdjustmentQuantity, inventoryAdjustmentsDTO.AdjustmentQuantity,
                                              tempFromDTO.InventoryLotDTO.ReceivePrice, tempFromDTO.InventoryLotDTO.PurchaseOrderReceiveLineId, tempFromDTO.InventoryLotDTO.Expirydate,
                                              true, string.Empty, tempFromDTO.InventoryLotDTO.UOMId);
                            inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                            inventoryLotBL.Save(sqlTransaction);
                            inventoryUpdateDTO.LotId = inventoryLotBL.GetInventoryLot.LotId;
                            inventoryUpdateDTO = tempToDTO;
                            inventoryUpdateDTO.InventoryId = -1;
                            currentTolocationQty = inventoryAdjustmentsDTO.AdjustmentQuantity;
                            inventoryUpdateDTO.Quantity = currentTolocationQty;
                        }
                        else
                        {
                            inventoryUpdateDTO = tempToDTO;
                            currentTolocationQty = inventoryAdjustmentsDTO.AdjustmentQuantity + inventoryUpdateDTO.Quantity;
                            inventoryUpdateDTO.Quantity = currentTolocationQty;
                        }
                    }
                }
                else
                {
                    if (productDTO != null && productDTO.LotControlled && tempFromDTO.InventoryLotDTO != null)
                    {
                        inventoryLotDTO = new InventoryLotDTO(-1, string.Empty, inventoryAdjustmentsDTO.AdjustmentQuantity, inventoryAdjustmentsDTO.AdjustmentQuantity,
                                            tempFromDTO.InventoryLotDTO.ReceivePrice, tempFromDTO.InventoryLotDTO.PurchaseOrderReceiveLineId, tempFromDTO.InventoryLotDTO.Expirydate,
                                            true, string.Empty, tempFromDTO.InventoryLotDTO.UOMId);
                        inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                        inventoryLotBL.Save(sqlTransaction);
                        inventoryUpdateDTO.LotId = inventoryLotBL.GetInventoryLot.LotId;
                    }
                    inventoryUpdateDTO.ProductId = tempFromDTO.ProductId;
                    inventoryUpdateDTO.ReceivePrice = tempFromDTO.ReceivePrice;
                    inventoryUpdateDTO.UOMId = tempFromDTO.UOMId;
                    inventoryUpdateDTO.InventoryId = -1;
                    currentTolocationQty = inventoryAdjustmentsDTO.AdjustmentQuantity;
                    inventoryUpdateDTO.LocationId = inventoryAdjustmentsDTO.ToLocationId;
                    inventoryUpdateDTO.Quantity = currentTolocationQty;
                }

                inventoryBL = new Inventory(inventoryUpdateDTO, executionContext);
                inventoryBL.Save(sqlTransaction);
                //InventoryDTO tempFromDTO = inventoryList.GetInventory(inventoryAdjustmentsDTO.ProductId, inventoryAdjustmentsDTO.FromLocationId, inventoryAdjustmentsDTO.LotID, sqlTransaction);
                double currentFromlocationQty = 0;

                if (tempFromDTO != null)
                {

                    if (tempFromDTO.LotId > -1)
                    {
                        InventoryLotList inventoryLotList = new InventoryLotList(executionContext);
                        inventoryLotDTO = inventoryLotList.GetInventoryLot(tempFromDTO.LotId, sqlTransaction);
                        inventoryLotDTO.BalanceQuantity = inventoryLotDTO.BalanceQuantity + inventoryAdjustmentsDTO.AdjustmentQuantity * -1;
                        inventoryLotDTO.IsChanged = true;
                        inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                        inventoryLotBL.Save(sqlTransaction);
                    }
                    inventoryUpdateDTO = tempFromDTO;
                    currentFromlocationQty = inventoryUpdateDTO.Quantity + inventoryAdjustmentsDTO.AdjustmentQuantity * -1;
                    inventoryUpdateDTO.Quantity = currentFromlocationQty;
                }
                else
                {
                    string message = MessageContainerList.GetMessage(executionContext, 2695, productDTO.ProductName);
                    throw new ValidationException(message);
                }
                inventoryBL = new Inventory(inventoryUpdateDTO, executionContext);
                inventoryBL.Save(sqlTransaction);
            }
            else if (inventoryAdjustmentsDTO.AdjustmentType == "VendorReturn")
            {
                InventoryDTO tempDTO = new InventoryDTO();
                inventoryAdjustmentsDTO.AdjustmentType = "VendorReturn";
                inventoryAdjustmentsDTO.DocumentTypeID = GetVendorDocumentTypeId();
                ValidateReturnVendor(sqlTransaction);

                try
                {
                    double CurrentQty = 0;
                    double newQuantity = 0;
                    if (productDTO.LotControlled && inventoryAdjustmentsDTO.LotID == -1)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, 2755);
                        throw new ValidationException(message);
                    }
                    int userEnteredUOMId = productDTO.UomId;
                    int inventoryUOMId = inventoryAdjustmentsDTO.UOMId;
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

                    inventoryList = new InventoryList();
                    tempDTO = inventoryList.GetInventory(inventoryAdjustmentsDTO.ProductId, inventoryAdjustmentsDTO.FromLocationId, inventoryAdjustmentsDTO.LotID);
                    double currentqtyinDTO = 0;
                    CurrentQty = tempDTO.Quantity;
                    newQuantity = inventoryAdjustmentsDTO.AdjustmentQuantity * factor;
                    if (tempDTO != null && CurrentQty < newQuantity)
                    {
                        string errorMessage = MessageContainerList.GetMessage(executionContext, 1081, MessageContainerList.GetMessage(executionContext, "return"), MessageContainerList.GetMessage(executionContext, "received quantity"));
                        throw new ValidationException(errorMessage);
                    }

                    if (tempDTO != null)
                    {
                        currentqtyinDTO = tempDTO.Quantity;
                    }
                    //tempDTO.Quantity = AdjQty + currentqtyinDTO;
                    tempDTO.Quantity = currentqtyinDTO - newQuantity;
                    if (tempDTO.Quantity < 0)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, "Adjustment Error");
                        throw new ValidationException(message);
                    }
                    inventoryBL = new Semnox.Parafait.Inventory.Inventory(tempDTO, executionContext); //update DTO with qty

                    inventoryBL.Save(sqlTransaction);
                    inventoryAdjustmentsDTO.AdjustmentQuantity = newQuantity * -1;
                    if (inventoryAdjustmentsDTO.LotID != -1)
                    {
                        double lotQuantity = inventoryBL.GetInventoryDTO.Quantity;
                        Semnox.Parafait.Inventory.InventoryList inventoryList1 = new InventoryList();
                        List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                        searchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, inventoryAdjustmentsDTO.LotID.ToString()));
                        List<InventoryDTO> inventoryLotDTOList1 = inventoryList1.GetAllInventory(searchParams);
                        if (inventoryLotDTOList1 != null)
                        {
                            foreach (InventoryDTO inventoryDTO1 in inventoryLotDTOList1)
                            {
                                if (inventoryDTO1.LocationId != inventoryAdjustmentsDTO.FromLocationId)
                                    lotQuantity += inventoryDTO1.Quantity;
                            }
                        }
                        InventoryLotDTO inventoryLotsDTO = new InventoryLotDTO();
                        InventoryLotList inventoryLotList = new InventoryLotList(executionContext);
                        inventoryLotsDTO = inventoryLotList.GetInventoryLot(inventoryAdjustmentsDTO.LotID);
                        inventoryLotsDTO.BalanceQuantity = lotQuantity;
                        inventoryLotBL = new InventoryLotBL(inventoryLotsDTO, executionContext);
                        inventoryLotBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Vendor Return ended with exception: " + ex.ToString());
                    throw new ValidationException(ex.Message.ToString());
                }
            }
            else
            {
                double currentQuantity = 0;
                InventoryDTO inventoryDTO = inventoryList.GetInventory(inventoryAdjustmentsDTO.ProductId, inventoryAdjustmentsDTO.FromLocationId, inventoryAdjustmentsDTO.LotID, sqlTransaction);

                if (inventoryDTO != null)
                {
                    if (inventoryDTO.LotId > -1)
                    {
                        InventoryLotList inventoryLotList = new InventoryLotList(executionContext);
                        inventoryLotDTO = inventoryLotList.GetInventoryLot(inventoryDTO.LotId, sqlTransaction);
                        inventoryLotDTO.BalanceQuantity = inventoryLotDTO.BalanceQuantity + inventoryAdjustmentsDTO.AdjustmentQuantity;
                        inventoryLotDTO.IsChanged = true;
                        inventoryLotBL = new InventoryLotBL(inventoryLotDTO, executionContext);
                        inventoryLotBL.Save(sqlTransaction);
                    }
                    currentQuantity = inventoryDTO.Quantity;
                    inventoryDTO.Quantity = inventoryAdjustmentsDTO.AdjustmentQuantity + currentQuantity;
                    if (currentQuantity + inventoryAdjustmentsDTO.AdjustmentQuantity < 0)
                    {
                        string message = MessageContainerList.GetMessage(executionContext, "Adjustment Error");
                        throw new ValidationException(message);
                    }
                }
                else
                {
                    inventoryDTO = new InventoryDTO();
                    inventoryDTO.ProductId = inventoryAdjustmentsDTO.ProductId;
                    inventoryDTO.LocationId = inventoryAdjustmentsDTO.FromLocationId;
                    inventoryDTO.Quantity = inventoryAdjustmentsDTO.AdjustmentQuantity + currentQuantity;
                    inventoryDTO.LotId = inventoryAdjustmentsDTO.LotID;
                    inventoryDTO.Timestamp = ServerDateTime.Now;
                }
                inventoryDTO.UOMId = inventoryAdjustmentsDTO.UOMId;
                inventoryBL = new Inventory(inventoryDTO, executionContext);
                inventoryBL.Save(sqlTransaction);
            }
            Save(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// This method gets the document type id for Wastage type
        /// </summary>
        private int GetVendorDocumentTypeId()
        {
            log.LogMethodEntry();
            int vendorDocumentTypeId = -1;
            try
            {
                log.LogMethodEntry();

                InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType("Vendor Return");
                log.LogVariableState("inventoryDocumentTypeDTO :", inventoryDocumentTypeDTO);
                if (inventoryDocumentTypeDTO != null)
                {
                    vendorDocumentTypeId = inventoryDocumentTypeDTO.DocumentTypeId;
                }
                log.Debug("vendorDocumentTypeId : " + vendorDocumentTypeId);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit();
            return vendorDocumentTypeId;
        }

        /// <summary>
        /// Validates the Inventory
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void ValidateInventory(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryAdjustmentsDTO == null)
            {
                log.Error("No products in the selected location");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2703);
                throw new ValidationException(errorMessage);
            }
            if (inventoryAdjustmentsDTO.AdjustmentType == "Adjustment")
            {
                if (inventoryAdjustmentsDTO.AdjustmentQuantity == 0)
                {
                    log.Error("Please enter valid quantity  greater than or less than zero");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 2360);
                    throw new ValidationException(errorMessage);
                }
                if (inventoryAdjustmentsDTO.FromLocationId < 0)
                {
                    log.Error("Please select a target location");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 806);
                    throw new ValidationException(errorMessage);
                }
                if (inventoryAdjustmentsDTO.ProductId < 0)
                {
                    log.Error("Please enter valid value for Product");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Product"));
                    throw new ValidationException(errorMessage);
                }
            }
            if (inventoryAdjustmentsDTO.AdjustmentType == "Transfer")
            {
                if (inventoryAdjustmentsDTO.AdjustmentQuantity == 0)
                {
                    log.Error("Please enter a value to transfer");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 747);
                    throw new ValidationException(errorMessage);
                }
                if (inventoryAdjustmentsDTO.FromLocationId < 0)
                {
                    log.Error("Please select a target location");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 806);
                    throw new ValidationException(errorMessage);
                }
                if (inventoryAdjustmentsDTO.ToLocationId < 0)
                {
                    log.Error("Please select Transfer To Location");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 838);
                    throw new ValidationException(errorMessage);
                }
                if (inventoryAdjustmentsDTO.FromLocationId == inventoryAdjustmentsDTO.ToLocationId)
                {
                    log.Error("From and To locations can not be same.");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1085);
                    throw new ValidationException(errorMessage);
                }
                if (inventoryAdjustmentsDTO.ProductId < 0)
                {
                    log.Error("Please enter valid value for Product");
                    string errorMessage = MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Product"));
                    throw new ValidationException(errorMessage);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the InventoryWastageSummaryDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void ValidatePhysicalCount(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryAdjustmentsDTO == null)
            {
                log.Error("No products in the selected location");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2703);
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrEmpty(inventoryAdjustmentsDTO.Remarks))
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 201);
                throw new ValidationException(errorMessage);
            }
            if (inventoryAdjustmentsDTO.AdjustmentQuantity < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 479);
                throw new ValidationException(errorMessage);
            }
            ProductList ProductList = new ProductList();
            ProductDTO productDTO = ProductList.GetProduct(inventoryAdjustmentsDTO.ProductId);
            if (productDTO.LotControlled && inventoryAdjustmentsDTO.LotID == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2752);
                throw new ValidationException(errorMessage);
            }
            if (inventoryAdjustmentsDTO.AdjustmentQuantity < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2360);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the InventoryWastageSummaryDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        private void ValidateReturnVendor(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryAdjustmentsDTO == null)
            {
                log.Error("No products in the selected location");
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2703);
                throw new ValidationException(errorMessage);
            }
            if (inventoryAdjustmentsDTO.AdjustmentQuantity < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2360);
                throw new ValidationException(errorMessage);
            }
            if (inventoryAdjustmentsDTO.AdjustmentQuantity == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 4643);
                throw new ValidationException(errorMessage);
            }
            InventoryReceiveLinesBL inventoryReceiveLinesBL = new InventoryReceiveLinesBL(executionContext, inventoryAdjustmentsDTO.PurchaseOrderReceiveLineId, sqlTransaction);
            if (inventoryReceiveLinesBL.InventoryReceiveLinesDTO != null && inventoryAdjustmentsDTO.AdjustmentQuantity > inventoryReceiveLinesBL.InventoryReceiveLinesDTO.Quantity)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 1081, MessageContainerList.GetMessage(executionContext, "return"), MessageContainerList.GetMessage(executionContext, "received quantity"));
                throw new ValidationException(errorMessage);
            }
            ProductList ProductList = new ProductList();
            ProductDTO productDTO = ProductList.GetProduct(inventoryAdjustmentsDTO.ProductId);
            if (productDTO.LotControlled && inventoryAdjustmentsDTO.LotID == -1)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 2752);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the InventoryAdjustmentDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
    }

    /// <summary>
    /// Manages the list of Inventory Adjustment List
    /// </summary>
    public class InventoryAdjustmentsList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<InventoryAdjustmentsDTO> inventoryAdjustmentsDTOList = new List<InventoryAdjustmentsDTO>();
        private Dictionary<int, string> keyValuePairs = new Dictionary<int, string>(); // used for InventoryUI to display validation error in the excel

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public InventoryAdjustmentsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="inventoryHistoryDTOList"></param>
        public InventoryAdjustmentsList(ExecutionContext executionContext, List<InventoryAdjustmentsDTO> inventoryAdjustmentDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, inventoryAdjustmentDTOList);
            this.inventoryAdjustmentsDTOList = inventoryAdjustmentDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Inventory Adjustments
        /// </summary>
        /// <param name="adjustmentId">adjustmentId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryAdjustmentsDTO</returns>
        public InventoryAdjustmentsDTO GetInventoryAdjustments(double adjustmentId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(adjustmentId, sqlTransaction);
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            InventoryAdjustmentsDTO inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO();
            inventoryAdjustmentsDTO = inventoryAdjustmentsDataHandler.GetInventoryAdjustmentsDTO(adjustmentId);
            log.LogMethodExit(inventoryAdjustmentsDTO);
            return inventoryAdjustmentsDTO;
        }

        /// <summary>
        /// Returns the Inventory Adjustment List
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryAdjustmentsList</returns>
        public List<InventoryAdjustmentsDTO> GetAllInventoryAdjustments(List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            List<InventoryAdjustmentsDTO> inventoryAdjustmentsList = new List<InventoryAdjustmentsDTO>();
            inventoryAdjustmentsList = inventoryAdjustmentsDataHandler.GetInventoryAdjustmentsList(searchParameters);
            log.LogMethodExit(inventoryAdjustmentsList);
            return inventoryAdjustmentsList;
        }   

        /// <summary>
        /// Returns Adjustment Quantity
        /// Modified on 01-sep-2016
        /// </summary>
        /// <param name="locationId">locationId</param>
        /// <param name="productId">productId</param>
        /// <param name="receiptlineId">receiptlineId</param>
        /// <param name="documentTypeId">documentTypeId</param>
        /// <param name="adjustmentType">adjustmentType</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>quantity</returns>
        public double GetTotalAdjustments(int locationId, int productId, int receiptlineId, int documentTypeId, string adjustmentType, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(locationId, productId, receiptlineId, documentTypeId, adjustmentType, sqlTransaction);
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            double quantity = inventoryAdjustmentsDataHandler.GetAdjustmentQuantity(locationId, productId, receiptlineId, documentTypeId, adjustmentType);
            log.LogMethodExit(quantity);
            return quantity;
        }

        /// <summary>
        /// Returns Inventory Adjustments DTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="advancedSearch">advancedSearch</param>
        /// <param name="pivotColumns">pivotColumns</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryAdjustmentsSummaryDTOs</returns>
        public List<InventoryAdjustmentsSummaryDTO> GetInventoryAdjustmentsSummaryDTO(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters, string advancedSearch, string pivotColumns, SqlTransaction sqlTransaction = null, bool ignoreWastage = true)
        {
            log.LogMethodEntry(searchParameters, advancedSearch, pivotColumns, sqlTransaction, ignoreWastage);
            List<InventoryAdjustmentsSummaryDTO> filteredinventoryAdjustmentsSummaryDTOList = new List<InventoryAdjustmentsSummaryDTO>();
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOs = new List<InventoryAdjustmentsSummaryDTO>();
            inventoryAdjustmentsSummaryDTOs = inventoryAdjustmentsDataHandler.GetInventoryAdjustmentsSummaryDTO(searchParameters, advancedSearch, pivotColumns);
            if(inventoryAdjustmentsSummaryDTOs != null && inventoryAdjustmentsSummaryDTOs.Any() && ignoreWastage)
            {
                filteredinventoryAdjustmentsSummaryDTOList = inventoryAdjustmentsSummaryDTOs.Where(x => x.LocationName != "Wastage").ToList();
            }
            log.LogMethodExit(filteredinventoryAdjustmentsSummaryDTOList);
            return filteredinventoryAdjustmentsSummaryDTOList;
        }

        /// <summary>
        /// Returns Inventory Adjustments DTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="advancedSearch">advancedSearch</param>
        /// <param name="pivotColumns">pivotColumns</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryAdjustmentsSummaryDTOs</returns>
        public List<InventoryAdjustmentsSummaryDTO> GetAllInventoryAdjustmentsSummaryDTO(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters, string advancedSearch, string pivotColumns, SqlTransaction sqlTransaction = null, 
                                                                                         bool ignoreWastage = true, int currentPage = 0, int pageSize = 0)
        {
            log.LogMethodEntry(searchParameters, advancedSearch, pivotColumns, sqlTransaction, ignoreWastage, currentPage, pageSize);
            List<InventoryAdjustmentsSummaryDTO> filteredinventoryAdjustmentsSummaryDTOList = new List<InventoryAdjustmentsSummaryDTO>();
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOs = new List<InventoryAdjustmentsSummaryDTO>();
            inventoryAdjustmentsSummaryDTOs = inventoryAdjustmentsDataHandler.GetInventoryAdjustmentsSummaryList(searchParameters, advancedSearch, pivotColumns, currentPage, pageSize);
            if (inventoryAdjustmentsSummaryDTOs != null && inventoryAdjustmentsSummaryDTOs.Any() && ignoreWastage)
            {
                filteredinventoryAdjustmentsSummaryDTOList = inventoryAdjustmentsSummaryDTOs.Where(x => x.LocationName != "Wastage").ToList();
            }
            log.LogMethodExit(filteredinventoryAdjustmentsSummaryDTOList);
            return filteredinventoryAdjustmentsSummaryDTOList;
        }

        /// <summary>
        /// Returns the no of Inventory Adjustments matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetInventoryAdjustmentsSummaryCount(List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters, string advancedSearch, string pivotColumns, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            int inventoryAdjustmentsCount = inventoryAdjustmentsDataHandler.GetInventoryAdjustmentsSummaryCount(searchParameters, advancedSearch, pivotColumns);
            log.LogMethodExit(inventoryAdjustmentsCount);
            return inventoryAdjustmentsCount;
        }

        /// <summary>
        /// Returns Inventory Adjustments DTO
        /// </summary>
        /// <param name="Barcode">Barcode</param>
        /// <param name="cmbScannedLocation">cmbScannedLocation</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>barcodeScanSummaryDTOs</returns>
        public List<BarcodeScanSummaryDTO> GetBarcodeScanSummaryDTO(string Barcode, string cmbScannedLocation, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(Barcode, cmbScannedLocation, sqlTransaction);
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            ExecutionContext executionContext = ExecutionContext.GetExecutionContext();
            List<BarcodeScanSummaryDTO> barcodeScanSummaryDTOs = new List<BarcodeScanSummaryDTO>();
            barcodeScanSummaryDTOs = inventoryAdjustmentsDataHandler.GetBarcodeScanSummaryDTO(Barcode, cmbScannedLocation, executionContext.GetSiteId());
            log.LogMethodExit(barcodeScanSummaryDTOs);
            return barcodeScanSummaryDTOs;
        }

        /// <summary>
        /// Returns Inventory Adjustments DTO
        /// </summary>
        /// <param name="Barcode">Barcode</param>
        /// <param name="cmbScannedLocation">cmbScannedLocation</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>barcodeScanSummaryDTOs</returns>
        public List<InventoryWastageSummaryDTO> GetAllInventoryWastageSummaryDTOList(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryWastageSummaryDataHandler inventoryWastageSummaryDataHandler = new InventoryWastageSummaryDataHandler(sqlTransaction);
            List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
            inventoryWastageSummaryDTOList = inventoryWastageSummaryDataHandler.GetInventoryWastageSummaryDTOList(searchParameters);
            log.LogMethodExit(inventoryWastageSummaryDTOList);
            return inventoryWastageSummaryDTOList;
        }

        public List<InventoryWastageSummaryDTO> GetInventoryWastageSummaryList(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters,
                                                                               int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, currentPage, pageSize, sqlTransaction);
            InventoryWastageSummaryDataHandler inventoryWastageSummaryDataHandler = new InventoryWastageSummaryDataHandler(sqlTransaction);
            List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
            inventoryWastageSummaryDTOList = inventoryWastageSummaryDataHandler.GetInventoryWastageSummaryList(searchParameters, currentPage, pageSize);
            log.LogMethodExit(inventoryWastageSummaryDTOList);
            return inventoryWastageSummaryDTOList;
        }

        /// <summary>
        /// This method is will return Sheet object for UOM.
        /// <returns></returns>
        public Sheet BuildTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            InventoryWastageSummaryDataHandler inventoryWastageSummaryDataHandler = new InventoryWastageSummaryDataHandler(sqlTransaction);
            List<InventoryWastageSummaryDTO> inventoryWastageSummaryDTOList = new List<InventoryWastageSummaryDTO>();
            List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters = new List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>>();
            searchParameters.Add(new KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>(InventoryWastageSummaryDTO.SearchByInventoryWastageParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            inventoryWastageSummaryDTOList = inventoryWastageSummaryDataHandler.GetInventoryWastageSummaryList(searchParameters);

            InventoryWastageExcelDTODefinition inventoryWastageExcelDTODefinition = new InventoryWastageExcelDTODefinition(executionContext, "");
            ///Building headers from InventoryWastageExcelDTODefinition
            inventoryWastageExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (inventoryWastageSummaryDTOList != null && inventoryWastageSummaryDTOList.Any())
            {
                foreach (InventoryWastageSummaryDTO inventoryWastageSummaryDTO in inventoryWastageSummaryDTOList)
                {
                    inventoryWastageExcelDTODefinition.Configure(inventoryWastageSummaryDTO);

                    Row row = new Row();
                    inventoryWastageExcelDTODefinition.Serialize(row, inventoryWastageSummaryDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }

        /// <summary>
        /// This method is will return Sheet object for Adjustment.
        /// <returns></returns>
        public Sheet BuildAjustmentTemplate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            List<InventoryAdjustmentsDTO> inventoryAdjustmentsList = new List<InventoryAdjustmentsDTO>();

            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>> searchParameters = new List<KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>>();
            searchParameters.Add(new KeyValuePair<InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters, string>(InventoryAdjustmentsDTO.SearchByInventoryAdjustmentsParameters.SITE_ID, executionContext.GetSiteId().ToString()));

            inventoryAdjustmentsList = inventoryAdjustmentsDataHandler.GetInventoryAdjustmentsList(searchParameters);

            InventoryAdjustmentExcelDTODefinition inventoryAdjustmentExcelDTODefinition = new InventoryAdjustmentExcelDTODefinition(executionContext, "");
            ///Building headers from InventoryWastageExcelDTODefinition
            inventoryAdjustmentExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            if (inventoryAdjustmentsList != null && inventoryAdjustmentsList.Any())
            {
                foreach (InventoryAdjustmentsDTO inventoryAdjustmentsDTO in inventoryAdjustmentsList)
                {
                    inventoryAdjustmentExcelDTODefinition.Configure(inventoryAdjustmentsDTO);

                    Row row = new Row();
                    inventoryAdjustmentExcelDTODefinition.Serialize(row, inventoryAdjustmentsDTO);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit();
            return sheet;
        }
        /// <summary>
        /// Validates and saves the InventoryAdjustmentsDTO list to the db
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public List<InventoryAdjustmentsDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryAdjustmentsDTOList == null || inventoryAdjustmentsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return inventoryAdjustmentsDTOList;
            }
            List<ValidationError> validationErrors = new List<ValidationError>();
            for (int i = 0; i < inventoryAdjustmentsDTOList.Count; i++)
            {
                var inventoryAdjustmentsDTO = inventoryAdjustmentsDTOList[i];
                if (inventoryAdjustmentsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    InventoryAdjustmentsBL inventoryAdjustmentsBL = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                    List<ValidationError> validationErrorsIndvidual = inventoryAdjustmentsBL.Validate(sqlTransaction);
                    if (validationErrorsIndvidual != null && validationErrorsIndvidual.Any())
                    {
                        validationErrors.AddRange(validationErrorsIndvidual);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while validating inventoryPhysicalCountDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("inventoryPhysicalCountDTO", inventoryAdjustmentsDTO);
                    throw;
                }
            }
            if (validationErrors.Any())
            {
                log.LogMethodExit(null, "Validation failed. ");
                throw new ValidationException("Validation failed for InventoryPhysicalCount.", validationErrors);
            }
            InventoryAdjustmentsDataHandler inventoryAdjustmentsDataHandler = new InventoryAdjustmentsDataHandler(sqlTransaction);
            inventoryAdjustmentsDTOList = inventoryAdjustmentsDataHandler.Save(inventoryAdjustmentsDTOList, executionContext.GetUserId(), executionContext.GetSiteId());
            log.LogMethodExit();
            return inventoryAdjustmentsDTOList;
        }

        public Dictionary<int, string> BulkUpload(Sheet sheet, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sheet, sqlTransaction);
            InventoryAdjustmentExcelDTODefinition inventoryPhysicalCountExcelDTODefinition = new InventoryAdjustmentExcelDTODefinition(executionContext, "");
            List<InventoryAdjustmentsDTO> rowInventoryPhysicalCountDTOList = new List<InventoryAdjustmentsDTO>();

            for (int i = 1; i < sheet.Rows.Count; i++)
            {
                int index = 0;
                try
                {
                    InventoryAdjustmentsDTO rowInventoryPhysicalCountDTO = (InventoryAdjustmentsDTO)inventoryPhysicalCountExcelDTODefinition.Deserialize(sheet[0], sheet[i], ref index);
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
                        InventoryAdjustmentsList inventoryPhysicalCountsListBL = new InventoryAdjustmentsList(executionContext, rowInventoryPhysicalCountDTOList);
                        inventoryPhysicalCountsListBL.Save(sqlTransaction);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            log.LogMethodExit(keyValuePairs);
            return keyValuePairs;

        }

        /// <summary>
        /// Returns the no of Inventory Wastages matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetInventoryWastagesCount(List<KeyValuePair<InventoryWastageSummaryDTO.SearchByInventoryWastageParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryWastageSummaryDataHandler inventoryWastageSummaryDataHandler = new InventoryWastageSummaryDataHandler(sqlTransaction);
            int inventoryWastagesCount = inventoryWastageSummaryDataHandler.GetInventoryWastagesCount(searchParameters);
            log.LogMethodExit(inventoryWastagesCount);
            return inventoryWastagesCount;
        }

        /// <summary>
        /// Returns the Inventory Total Cost
        /// </summary>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public string GetInventoryTotalCost(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>> searchParameters = new List<KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>>();
            searchParameters.Add(new KeyValuePair<InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters, string>(InventoryAdjustmentsSummaryDTO.SearchByInventoryAdjustmentsSummaryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
            List<InventoryAdjustmentsSummaryDTO> inventoryAdjustmentsSummaryDTOList = inventoryAdjustmentsList.GetAllInventoryAdjustmentsSummaryDTO(searchParameters, null, null, sqlTransaction);
            string totalCost = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CURRENCY_SYMBOL") + inventoryAdjustmentsSummaryDTOList.Sum(x => x.TotalCost).ToString();
            log.LogMethodExit(totalCost);
            return totalCost;
        }

        /// <summary>
        /// This method is will return Sheet object for ProductActivityViewDTO.
        /// <returns></returns>
        public Sheet BuildProductActivityViewTemplate(int productId = -1, int locationId = -1, int lotId = -1, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ProductActivityViewExcel> productActivityViewExcelDTOList = new List<ProductActivityViewExcel>();
            List<ProductActivityViewDTO> productActivityViewDTOList = new List<ProductActivityViewDTO>();
            UOMContainer uOMContainer = new UOMContainer(executionContext);
            if (UOMContainer.uomDTOList == null || UOMContainer.uomDTOList.Count <= 0)
            {
                log.Debug("Container is empty");
                return null;
            }

            List<LocationContainerDTO> locationContainerDTOList = LocationContainerList.GetLocationContainerDTOList(executionContext.GetSiteId());

            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();

            ProductActivityViewExcelDTODefination productActivityViewExcelDTODefination = new ProductActivityViewExcelDTODefination(executionContext, "");
            ///Building headers from recipePlanDetailsExcelDTODefinition
            productActivityViewExcelDTODefination.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);

            ProductActivityViewList productActivityViewList = new ProductActivityViewList(executionContext);

            productActivityViewDTOList = productActivityViewList.GetAllProductActivity(locationId, productId, lotId, 0, 100000, sqlTransaction);

            if (productActivityViewDTOList != null && productActivityViewDTOList.Any())
            {
                foreach (ProductActivityViewDTO productActivityViewDTO in productActivityViewDTOList)
                {
                    string location = string.Empty;
                    string transferLocation = string.Empty;
                    string uom = string.Empty;
                    if (productActivityViewDTO.LocationId > -1)
                    {
                        location = locationContainerDTOList.Where(x => x.LocationId == productActivityViewDTO.LocationId).FirstOrDefault().Name;
                    }
                    if (productActivityViewDTO.TransferLocationId > -1)
                    {
                        transferLocation = locationContainerDTOList.Where(x => x.LocationId == productActivityViewDTO.TransferLocationId).FirstOrDefault().Name;
                    }
                    else
                    {
                        transferLocation = location;
                    }
                    if (productActivityViewDTO.UOMId > -1)
                    {
                        uom = UOMContainer.uomDTOList.Where(x => x.UOMId == productActivityViewDTO.UOMId).FirstOrDefault().UOM;
                    }
                    ProductActivityViewExcel productActivityViewExcel = new ProductActivityViewExcel();
                    productActivityViewExcel.Location = location;
                    productActivityViewExcel.TransferLocation = transferLocation;
                    productActivityViewExcel.TrxType = productActivityViewDTO.Trx_Type;
                    productActivityViewExcel.Trx_Date = productActivityViewDTO.TimeStamp;
                    productActivityViewExcel.Quantity = productActivityViewDTO.Quantity;
                    productActivityViewExcel.UOM = uom;
                    productActivityViewExcel.UserName = productActivityViewDTO.UserName;
                    productActivityViewExcel.LotId = productActivityViewDTO.LotId;
                    productActivityViewExcelDTOList.Add(productActivityViewExcel);
                }
                foreach (ProductActivityViewExcel productActivityViewExcel in productActivityViewExcelDTOList)
                {
                    productActivityViewExcelDTODefination.Configure(productActivityViewExcel);
                    Row row = new Row();
                    productActivityViewExcelDTODefination.Serialize(row, productActivityViewExcel);
                    sheet.AddRow(row);
                }
            }
            log.LogMethodExit(sheet);
            return sheet;
        }
    }
}
