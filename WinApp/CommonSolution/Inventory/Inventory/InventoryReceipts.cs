/********************************************************************************************
* Project Name - Inventory Receipts
* Description  - Bussiness logic of inventory receipts
* 
**************
**Version Log
**************
*Version     Date          Modified By    Remarks          
*********************************************************************************************
*1.00        10-08-2016    Raghuveera      Created
*2.70.2      16-Jul-2019   Deeksha         Modifications as per three tier standard
*2.90.0      02-Jul-2020   Deeksha         Inventory process : Weighted Avg Costing changes.
*2.110.0     16-Dec-2020   Abhishek        Modified: added validate(),GetAllInventoryReceipt() for web API
*2.130       04-Jun-2021   Girish Kundar   Modified - POS stock changes
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Inventory Receipts will creates and modifies the inventory receipts
    /// </summary>
    public class InventoryReceiptsBL
    {
        private InventoryReceiptDTO inventoryReceiptDTO;
        private logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Utilities _utilities;
        private ExecutionContext executionContext;//= ExecutionContext.GetExecutionContext();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        private InventoryReceiptsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the InventoryReceipt id as the parameter
        /// Would fetch the Inventory Receipts object from the database based on the id passed. 
        /// </summary>
        /// <param name="receiptId">Requisition id</param>
        /// <param name="sqlTransaction">Sql transaction</param>
        public InventoryReceiptsBL(ExecutionContext executionContext, int receiptId, SqlTransaction sqlTransaction = null, bool loadChildRecords = false, bool activeChildRecords = false)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, receiptId, sqlTransaction);
            InventoryReceiptDataHandler inventoryReceiptDataHandler = new InventoryReceiptDataHandler(sqlTransaction);
            inventoryReceiptDTO = inventoryReceiptDataHandler.GetInventoryReceipt(receiptId);
            if (inventoryReceiptDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, " Inventory Receipt ", receiptId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="inventoryReceiptDTO">Parameter of the type InventoryReceiptDTO</param>
        /// <param name="executionContext">ExcecutionContext</param>
        public InventoryReceiptsBL(InventoryReceiptDTO inventoryReceiptDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(inventoryReceiptDTO, executionContext);
            this.inventoryReceiptDTO = inventoryReceiptDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the utilities and DTO parameter
        /// </summary>
        /// <param name="utilities">Parameter of the type utilities</param>
        /// <param name="inventoryReceiptDTO">Parameter of the type InventoryReceiptDTO</param>
        public InventoryReceiptsBL(Utilities utilities, InventoryReceiptDTO inventoryReceiptDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(utilities, inventoryReceiptDTO);
            _utilities = utilities;
            this.inventoryReceiptDTO = inventoryReceiptDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the InventoryReceiveLineList based on the receipt id.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            InventoryReceiptLineList inventoryReceiptLineList = new InventoryReceiptLineList(executionContext);
            List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> searchParams = new List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>();
            searchParams.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.RECEIPT_ID, inventoryReceiptDTO.ReceiptId.ToString()));
            if (activeChildRecords)
            {
                searchParams.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.ISACTIVE, "1"));
            }
            inventoryReceiptDTO.InventoryReceiveLinesListDTO = inventoryReceiptLineList.GetAllInventoryReceiveLines(searchParams, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Inventory Receipt
        /// Inventory Receipt will be inserted if InventoryReceiptId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (inventoryReceiptDTO.IsChangedRecursive == false
                && inventoryReceiptDTO.ReceiptId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            if (inventoryReceiptDTO.InventoryReceiveLinesListDTO == null)
            {
                inventoryReceiptDTO.InventoryReceiveLinesListDTO = new List<InventoryReceiveLinesDTO>();
            }
            InventoryReceiptDataHandler inventoryReceiptDataHandler = new InventoryReceiptDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (inventoryReceiptDTO.ReceiptId < 0)
            {
                if (inventoryReceiptDTO.GRN == "" || inventoryReceiptDTO.GRN == null)
                {
                    inventoryReceiptDTO.GRN = inventoryReceiptDataHandler.GetNextSeqNo("Receipt");
                }
                inventoryReceiptDTO = inventoryReceiptDataHandler.Insert(inventoryReceiptDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                inventoryReceiptDTO.AcceptChanges();
                InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "Receipt Inserted",
                                                                inventoryReceiptDTO.Guid, false, executionContext.GetSiteId(), "Receipt", -1,
                                                                inventoryReceiptDTO.ReceiptId.ToString(), -1, executionContext.GetUserId(),
                                                                ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, inventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            else
            {
                if (inventoryReceiptDTO.IsChanged)
                {
                    inventoryReceiptDTO = inventoryReceiptDataHandler.Update(inventoryReceiptDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    inventoryReceiptDTO.AcceptChanges();
                    InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "Receipt Updated",
                                                                inventoryReceiptDTO.Guid, false, executionContext.GetSiteId(), "Receipt", -1,
                                                                inventoryReceiptDTO.ReceiptId.ToString(), -1, executionContext.GetUserId(),
                                                                ServerDateTime.Now, executionContext.GetUserId(), ServerDateTime.Now);
                    InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(executionContext, inventoryActivityLogDTO);
                    inventoryActivityLogBL.Save(sqlTransaction);
                }
            }

            if (inventoryReceiptDTO.InventoryReceiveLinesListDTO != null && inventoryReceiptDTO.InventoryReceiveLinesListDTO.Count > 0)
            {

                InventoryReceiveLinesBL inventoryReceiveLinesBL;
                foreach (InventoryReceiveLinesDTO inventoryReceiveLinesDTO in inventoryReceiptDTO.InventoryReceiveLinesListDTO)
                {
                    if (inventoryReceiveLinesDTO.ReceiptId == -1)
                    {
                        inventoryReceiveLinesDTO.ReceiptId = inventoryReceiptDTO.ReceiptId;
                    }
                    if (inventoryReceiveLinesDTO.IsChanged)
                    {
                        inventoryReceiveLinesBL = new InventoryReceiveLinesBL(inventoryReceiveLinesDTO, executionContext);
                        inventoryReceiveLinesBL.Save(sqlTransaction);
                        ProductList productList = new ProductList();
                        ProductDTO productDTO = productList.GetProduct(inventoryReceiveLinesDTO.ProductId, sqlTransaction);
                        if (productDTO.TaxInclusiveCost == "Y")
                        {
                            productDTO.LastPurchasePrice = (inventoryReceiveLinesDTO.Price + (inventoryReceiveLinesDTO.Price * (inventoryReceiveLinesDTO.TaxPercentage / 100.0)));
                        }
                        else
                        {
                            productDTO.LastPurchasePrice = inventoryReceiveLinesDTO.Price;
                        }
                        ProductBL productBL = new ProductBL(executionContext, productDTO);
                        productBL.Save(sqlTransaction);
                        productBL.UpdatePITAndCost(inventoryReceiveLinesDTO.PriceInTickets, inventoryReceiptDTO.MarkupPercent, inventoryReceiptDTO.ReceiptId, sqlTransaction);
                        productDTO = productBL.getProductDTO;
                        if (productDTO != null)
                        {
                            inventoryReceiveLinesDTO.InventoryRequired = true;
                        }
                        if (inventoryReceiveLinesDTO.PriceInTickets != productDTO.PriceInTickets)
                        {
                            inventoryReceiveLinesDTO.PriceInTickets = productDTO.PriceInTickets;
                            inventoryReceiveLinesBL.Save(sqlTransaction);
                        }
                    }
                }
            }

            log.LogMethodExit();
        }


        /// <summary>
        /// Validates the InventoryReceiptDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            //if (string.IsNullOrEmpty(inventoryReceiptDTO.VendorName))
            //{
            //    throw new Exception(MessageContainerList.GetMessage(executionContext, 850));
            //}
            if (inventoryReceiptDTO.InventoryReceiveLinesListDTO == null)//&& !isReceiveAutoPO)
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 852));
            }
            if (string.IsNullOrEmpty(inventoryReceiptDTO.VendorBillNumber))
            {
                throw new Exception(MessageContainerList.GetMessage(executionContext, 851));
            }
            LocationBL locationBL = new LocationBL(executionContext, inventoryReceiptDTO.ReceiveToLocationID, sqlTransaction);
            if (locationBL.GetLocationDTO != null && locationBL.GetLocationDTO.IsActive == false)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, 5069);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public InventoryReceiptDTO InventoryReceiptDTO { get { return inventoryReceiptDTO; } }
    }

    /// <summary>
    /// Manages the list of patch application deployment plan
    /// </summary>
    public class InventoryReceiptList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<InventoryReceiptDTO> inventoryReceiptDTOList = new List<InventoryReceiptDTO>();

        /// <summary>
        /// Default constructor with executionContext
        /// </summary>
        public InventoryReceiptList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parmeterised constructor with 2 params
        /// </summary>
        /// <param name="requisitionDTOList"></param>
        /// <param name="executionContext"></param>
        public InventoryReceiptList(ExecutionContext executionContext, List<InventoryReceiptDTO> requisitionDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, requisitionDTOList);
            this.inventoryReceiptDTOList = requisitionDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the asset list
        /// </summary>
        /// <param name="receiptId">receiptId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryReceiptDTO</returns>
        public InventoryReceiptDTO GetReceipt(int receiptId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(receiptId, sqlTransaction);
            InventoryReceiptDataHandler inventoryReceiptDataHandler = new InventoryReceiptDataHandler(sqlTransaction);
            InventoryReceiptDTO inventoryReceiptDTO = new InventoryReceiptDTO();
            inventoryReceiptDTO = inventoryReceiptDataHandler.GetInventoryReceipt(receiptId);
            log.LogMethodExit(inventoryReceiptDTO);
            return inventoryReceiptDTO;
        }

        /// <summary>
        /// Returns the patch application deployment plan list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryReceiptDTOs</returns>
        public List<InventoryReceiptDTO> GetAllInventoryReceipts(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryReceiptDataHandler inventoryReceiptDataHandler = new InventoryReceiptDataHandler(sqlTransaction);
            List<InventoryReceiptDTO> inventoryReceiptDTOs = new List<InventoryReceiptDTO>();
            inventoryReceiptDTOs = inventoryReceiptDataHandler.GetInventoryReceiptList(searchParameters);
            log.LogMethodExit(inventoryReceiptDTOs);
            return inventoryReceiptDTOs;
        }

        /// <summary>
        /// Returns the complex InventoryReceiptDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="fetchLinkedData">fetchLinkedData</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryReceiptListDTO</returns>
        public List<InventoryReceiptDTO> GetAllInventoryReceipts(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters, Boolean fetchLinkedData, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryReceiptDataHandler inventoryReceiptDataHandler = new InventoryReceiptDataHandler(sqlTransaction);
            List<InventoryReceiptDTO> inventoryReceiptListDTO = inventoryReceiptDataHandler.GetInventoryReceiptList(searchParameters);
            if (fetchLinkedData)
            {
                BuildLinkedInventoryReceiptDTO(inventoryReceiptListDTO, searchParameters, sqlTransaction);
            }
            log.LogMethodExit(inventoryReceiptListDTO);
            return inventoryReceiptListDTO;
        }

        /// <summary>
        /// Builds the complex InventoryReceiptDTO list
        /// </summary>
        private void BuildLinkedInventoryReceiptDTO(List<InventoryReceiptDTO> inventoryReceiptListDTO, List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(inventoryReceiptListDTO, searchParameters);
            string purchaseOrderIds = "";
            Dictionary<int, InventoryReceiptDTO> mapInvReceiptDTO = new Dictionary<int, InventoryReceiptDTO>();
            if (searchParameters != null)
            {
                foreach (KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string> searchParameter in searchParameters)
                {
                    if (searchParameter.Key == InventoryReceiptDTO.SearchByInventoryReceiptParameters.PURCHASE_ORDER_IDS)
                        purchaseOrderIds = searchParameter.Value;
                }
            }
            if (purchaseOrderIds != "" && inventoryReceiptListDTO != null)
            {
                foreach (InventoryReceiptDTO inventoryReceiptDTO in inventoryReceiptListDTO)
                {
                    mapInvReceiptDTO.Add(inventoryReceiptDTO.ReceiptId, inventoryReceiptDTO);
                }
                InventoryReceiptLineList inventoryReceiptLineListBL = new InventoryReceiptLineList(executionContext);
                List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>> invRecieveLineSearchParm;
                invRecieveLineSearchParm = new List<KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>>();
                invRecieveLineSearchParm.Add(new KeyValuePair<InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters, string>(InventoryReceiveLinesDTO.SearchByInventoryReceiveLinesParameters.PURCHASE_ORDER_IDS, purchaseOrderIds));
                List<InventoryReceiveLinesDTO> inventoryReceiveLineListDTO = inventoryReceiptLineListBL.GetAllInventoryReceiveLine(invRecieveLineSearchParm,0,0,true,true, sqlTransaction);
                if (inventoryReceiveLineListDTO != null)
                {
                    foreach (InventoryReceiveLinesDTO inventoryReceiveLinesDTO in inventoryReceiveLineListDTO)
                    {
                        if (mapInvReceiptDTO.ContainsKey(inventoryReceiveLinesDTO.ReceiptId))
                        {
                            mapInvReceiptDTO[inventoryReceiveLinesDTO.ReceiptId].InventoryReceiveLinesListDTO.Add(inventoryReceiveLinesDTO);
                        }
                    }
                }
            }
            log.LogMethodExit();
        }

        public List<InventoryReceiptDTO> GetAllInventoryReceipt(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters,
                                                     bool loadChildRecords = false, bool activeChildRecords = true, bool loadReturnQuantity = false, int currentPage = 0,
                                                     int pageSize = 0, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryReceiptDataHandler inventoryReceiptDataHandler = new InventoryReceiptDataHandler(sqlTransaction);
            this.inventoryReceiptDTOList = inventoryReceiptDataHandler.GetInventoryReceiptsList(searchParameters, currentPage, pageSize);
            if (inventoryReceiptDTOList != null && inventoryReceiptDTOList.Any() && loadChildRecords)
            {
                Build(inventoryReceiptDTOList, activeChildRecords, loadReturnQuantity, sqlTransaction);
            }
            log.LogMethodExit(inventoryReceiptDTOList);
            return inventoryReceiptDTOList;
        }

        private void Build(List<InventoryReceiptDTO> inventoryReceiptDTOList, bool activeChildRecords, bool loadReturnQuantity, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(inventoryReceiptDTOList, activeChildRecords, loadReturnQuantity, sqlTransaction);
            Dictionary<int, InventoryReceiptDTO> inventoryReceiptsDTODictionary = new Dictionary<int, InventoryReceiptDTO>();
            List<int> inventoryReceiptsIdList = new List<int>();
            for (int i = 0; i < inventoryReceiptDTOList.Count; i++)
            {
                if (inventoryReceiptsDTODictionary.ContainsKey(inventoryReceiptDTOList[i].ReceiptId))
                {
                    continue;
                }
                inventoryReceiptsDTODictionary.Add(inventoryReceiptDTOList[i].ReceiptId, inventoryReceiptDTOList[i]);
                inventoryReceiptsIdList.Add(inventoryReceiptDTOList[i].ReceiptId);
            }
            InventoryReceiptLineList inventoryReceiptLineList = new InventoryReceiptLineList(executionContext);
            List<InventoryReceiveLinesDTO> inventoryReceiveLinesDTOList = inventoryReceiptLineList.GetInventoryReceiveLinesDTOList(inventoryReceiptsIdList, true, activeChildRecords, sqlTransaction);

            if (inventoryReceiveLinesDTOList != null && inventoryReceiveLinesDTOList.Any())
            {
                for (int i = 0; i < inventoryReceiveLinesDTOList.Count; i++)
                {
                    if (inventoryReceiptsDTODictionary.ContainsKey(inventoryReceiveLinesDTOList[i].ReceiptId) == false)
                    {
                        continue;
                    }
                    InventoryReceiptDTO inventoryReceiptDTO = inventoryReceiptsDTODictionary[inventoryReceiveLinesDTOList[i].ReceiptId];
                    if (inventoryReceiptDTO.InventoryReceiveLinesListDTO == null)
                    {
                        inventoryReceiptDTO.InventoryReceiveLinesListDTO = new List<InventoryReceiveLinesDTO>();
                    }
                    inventoryReceiptDTO.InventoryReceiveLinesListDTO.Add(inventoryReceiveLinesDTOList[i]);
                    if (loadReturnQuantity)
                    {
                        int vendorDocumentTypeId = -1;
                        InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(executionContext);
                        InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType("Vendor Return");
                        log.LogVariableState("inventoryDocumentTypeDTO :", inventoryDocumentTypeDTO);
                        if (inventoryDocumentTypeDTO != null)
                        {
                            vendorDocumentTypeId = inventoryDocumentTypeDTO.DocumentTypeId;
                        }
                        log.Debug("vendorDocumentTypeId : " + vendorDocumentTypeId);
                        InventoryAdjustmentsList inventoryAdjustmentsList = new InventoryAdjustmentsList(executionContext);
                        inventoryReceiveLinesDTOList[i].VendorReturnedQuantity = inventoryAdjustmentsList.GetTotalAdjustments(inventoryReceiveLinesDTOList[i].LocationId,
                            inventoryReceiveLinesDTOList[i].ProductId, inventoryReceiveLinesDTOList[i].PurchaseOrderReceiveLineId, vendorDocumentTypeId, "VendorReturn");
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of InventoryReceipts matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetInventoryReceiptsCount(List<KeyValuePair<InventoryReceiptDTO.SearchByInventoryReceiptParameters, string>> searchParameters,
                                             SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryReceiptDataHandler inventoryReceiptDataHandler = new InventoryReceiptDataHandler(sqlTransaction);
            int inventoryReceiptsCount = inventoryReceiptDataHandler.GetInventoryReceiptsCount(searchParameters);
            log.LogMethodExit(inventoryReceiptsCount);
            return inventoryReceiptsCount;
        }
    }
}
