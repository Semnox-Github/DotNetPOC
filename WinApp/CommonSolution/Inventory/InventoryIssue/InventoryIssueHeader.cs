
/********************************************************************************************
 * Project Name - Inventory Issue Header
 * Description  - Bussiness logic of inventory issue header
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 * 1.00        11-08-2016   Raghuveera     Created 
 * 2.60        22-03-2019   Girish K       Adding Issue Number
 ********************************************************************************************
 * 2.60        08-04-2019   Girish K        Replaced PurchaseTax 3 tier with Tax 3 Tier
                                            GetProductTax() Method.
* 2.60.2       06-Jun-2019  Akshay G        Code merge from Development to WebManagementStudio
                                            
 *2.70.2       14-Jul-2019  Deeksha         Modified :save method returns DTO instaed of id.
 *2.70.2       18-Dec-2019  Jinto Thomas    Added parameter execution context for userrolrbl declaration with userid 
 *2.100.0      24-Jul-2020  Deeksha         Modified to handle UOMId field in the constructor.
 *2.110.0      22-Oct-2020  Mushahid Faizan  Handled execution Context in the Constructor.
 *2.110.0      28-Dec-2020  Prajwal S       Added GetAllInventoryIssueHeader method to get Issue header list and also child list.
 *                                          Added GetInventoryIssueHeaderCount to get count. Added Default contstructor and contructor with execution
 *                                          context as parameter. Modified Save to Save Child. Added Build to get Child Records.
 *                                          Added Validation.
 *2.110.0      18-Jan-2020  Mushahid Faizan Modified : Web Inventory Changes
 * ******************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;

using Semnox.Parafait.Product;
using Semnox.Parafait.User;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer;
using System.Collections.Concurrent;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Business logic for Issue header
    /// </summary>
    public class InventoryIssueHeader
    {
        private InventoryIssueHeaderDTO inventoryIssueHeaderDTO;

        private InventoryAdjustmentsDTO inventoryAdjustmentsDTO;
        private InventoryLotDTO inventoryLotDTO;
        private InventoryReceiptDTO inventoryReceiptDTO;
        private InventoryReceiveLinesDTO inventoryReceiveLinesDTO;
        private List<PurchaseOrderLineDTO> purchaseOrderLineDTOList;
        private List<Core.Utilities.LookupValuesDTO> lookupValuesDTOList;
        private InventoryLotBL inventoryLot;
        //InventoryAdjustments.InventoryAdjustmentsBL inventoryAdjustments;
        private InventoryReceiveLinesBL inventoryReceiveLines;
        private InventoryReceiptsBL inventoryReceipts;
        private int count = 0;
        private LookupValuesList lookupValuesList;// = new Core.Utilities.LookupValuesList(ExecutionContext.GetExecutionContext());


        private PurchaseOrderLineList purchaseOrderLineList = new PurchaseOrderLineList();
        private List<KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams;
        private List<KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>> searchByPurchaseOrderLineParameters;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext machineContext;//= ExecutionContext.GetExecutionContext();

        private InventoryIssueHeaderDTO inventoryIssueHeaderDTOonsave;
        private Product.ProductBL product;
        private List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
        private static readonly ConcurrentDictionary<int, UOMContainer> uomContainerDictionary = new ConcurrentDictionary<int, UOMContainer>();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="machineContext">ExecutionContext</param>
        public InventoryIssueHeader(ExecutionContext machineContext)
        {
            log.LogMethodEntry(machineContext);
            inventoryIssueHeaderDTO = null;
            this.machineContext = machineContext;
            lookupValuesList = new LookupValuesList(machineContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="inventoryIssueHeaderDTO">Parameter of the type InventoryIssueHeaderDTO</param>
        /// <param name="machineContext">Execution context</param>
        public InventoryIssueHeader(InventoryIssueHeaderDTO inventoryIssueHeaderDTO, ExecutionContext machineContext)
        {
            log.LogMethodEntry(inventoryIssueHeaderDTO, machineContext);
            this.inventoryIssueHeaderDTO = inventoryIssueHeaderDTO;
            this.machineContext = machineContext;
            lookupValuesList = new LookupValuesList(machineContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="issueId">Parameter of the type Integer</param>
        /// <param name="machineContext">Execution Context</param>
        /// <param name="sqlTransaction">Parameter of the type SqlTransaction</param>
        public InventoryIssueHeader(int issueId, ExecutionContext machineContext, SqlTransaction sqlTransaction, bool loadChildRecords = false, bool activeChildRecords = false)
            : this(machineContext)
        {
            log.LogMethodEntry(issueId, machineContext, sqlTransaction);
            InventoryIssueHeaderDataHandler inventoryIssueHeaderDataHandler = new InventoryIssueHeaderDataHandler(sqlTransaction);
            this.inventoryIssueHeaderDTO = inventoryIssueHeaderDataHandler.GetInventoryIssueHeader(issueId);
            if (inventoryIssueHeaderDTO == null)
            {
                string message = MessageContainerList.GetMessage(machineContext, 2196, " InventoryIssueHeader ", issueId);
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
        /// <param name="guid">Parameter of the type guid of the record</param>
        /// <param name="machineContext">Execution Context</param>
        /// <param name="sqlTransaction">Sql transaction object</param>
        public InventoryIssueHeader(string guid, ExecutionContext machineContext, SqlTransaction sqlTransaction = null)
             : this(machineContext)
        {
            log.LogMethodEntry(guid, machineContext, sqlTransaction);
            List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchByInventoryIssueHeaderParameters = new List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>();
            searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG, "1"));
            searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.GUID, guid));
            InventoryIssueHeaderList inventoryIssueHeaderList = new InventoryIssueHeaderList();
            List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = inventoryIssueHeaderList.GetAllInventoryIssueHeaders(searchByInventoryIssueHeaderParameters, sqlTransaction);
            if (inventoryIssueHeaderDTOList == null || (inventoryIssueHeaderDTOList != null && inventoryIssueHeaderDTOList.Count == 0))
            {
                inventoryIssueHeaderDTO = new InventoryIssueHeaderDTO();
            }
            else
            {
                inventoryIssueHeaderDTO = inventoryIssueHeaderDTOList[0];
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Gets the inventoryIssueLinesList based on the issue id.
        /// </summary>
        /// <param name="activeChildRecords">activeChildRecords</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);


            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList(machineContext);
            List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>> searchParams = new List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>>();
            searchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_ID, inventoryIssueHeaderDTO.InventoryIssueId.ToString()));
            if (activeChildRecords)
            {
                searchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ACTIVE_FLAG, "1"));
            }
            inventoryIssueHeaderDTO.InventoryIssueLinesListDTO = inventoryIssueLinesList.GetAllInventoryIssueLines(searchParams, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Inventory Issue Header
        /// Inventory Issue Header will be inserted if InventoryIssueId is less than or equal to
        /// zero else updates the records based on primary key
        /// <param name="sqlTransaction">Sql transaction object</param>
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            InventoryIssueHeaderDataHandler inventoryIssueHeaderDataHandler = new InventoryIssueHeaderDataHandler(sqlTransaction);
            if (inventoryIssueHeaderDTO.InventoryIssueId <= 0)
            {
                string IssueNumber = inventoryIssueHeaderDataHandler.GetNextSeqNo("InventoryIssues");
                if (String.IsNullOrEmpty(IssueNumber))
                {
                    throw new Exception("Issue Number is not generated");
                }
                inventoryIssueHeaderDTO.IssueNumber = IssueNumber;
                inventoryIssueHeaderDTO = inventoryIssueHeaderDataHandler.InsertInventoryIssueHeader(inventoryIssueHeaderDTO, machineContext.GetUserId(), machineContext.GetSiteId());
                inventoryIssueHeaderDTO.AcceptChanges();
                InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "InventoyIssueHeader Inserted",
                                                             inventoryIssueHeaderDTO.Guid, false, machineContext.GetSiteId(), "InventoryIssueHeader", -1,
                                                             inventoryIssueHeaderDTO.InventoryIssueId.ToString(), -1, machineContext.GetUserId(),
                                                             lookupValuesList.GetServerDateTime(), machineContext.GetUserId(), ServerDateTime.Now);
                InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(machineContext, inventoryActivityLogDTO);
                inventoryActivityLogBL.Save(sqlTransaction);
            }
            else
            {
                if (inventoryIssueHeaderDTO.IsChanged)
                {
                    inventoryIssueHeaderDTO = inventoryIssueHeaderDataHandler.UpdateInventoryIssueHeader(inventoryIssueHeaderDTO, machineContext.GetUserId(), machineContext.GetSiteId());
                    inventoryIssueHeaderDTO.AcceptChanges();
                    InventoryActivityLogDTO inventoryActivityLogDTO = new InventoryActivityLogDTO(ServerDateTime.Now, "InventoyIssueHeader Updated",
                                                         inventoryIssueHeaderDTO.Guid, false, machineContext.GetSiteId(), "InventoryIssueHeader", -1,
                                                         inventoryIssueHeaderDTO.InventoryIssueId.ToString(), -1, machineContext.GetUserId(),
                                                         lookupValuesList.GetServerDateTime(), machineContext.GetUserId(), ServerDateTime.Now);
                    InventoryActivityLogBL inventoryActivityLogBL = new InventoryActivityLogBL(machineContext, inventoryActivityLogDTO);
                    inventoryActivityLogBL.Save(sqlTransaction);
                }
            }
            if (inventoryIssueHeaderDTO.InventoryIssueLinesListDTO != null && inventoryIssueHeaderDTO.InventoryIssueLinesListDTO.Any())
            {

                foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueHeaderDTO.InventoryIssueLinesListDTO)
                {
                    inventoryIssueLinesDTO.IssueId = inventoryIssueHeaderDTO.InventoryIssueId;
                    InventoryIssueLines inventoryIssueLines = new InventoryIssueLines(inventoryIssueLinesDTO, machineContext);
                    inventoryIssueLines.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Save the data to the Inventory,InventoryAdjustments,Receipt, PurchaseOrderRecieve_line, Inventorylot,InventoryTransaction
        /// </summary>
        /// <param name="code">Document Type code</param>
        /// <param name="DocumentTypeId">DocumentTypeId</param>
        /// <param name="inventoryIssueLinesDTO">inventoryIssueLinesDTO</param>
        /// <param name="SQLTrx">SQLTrx</param>
        /// <param name="POId">POId</param>
        /// <param name="InvoiceNo">InvoiceNo</param>        
        public void SaveIssueDetails(string code, int DocumentTypeId, InventoryIssueLinesDTO inventoryIssueLinesDTO, SqlTransaction SQLTrx, int POId = -1, string InvoiceNo = "")
        {
            log.LogMethodEntry(code, DocumentTypeId, inventoryIssueLinesDTO, SQLTrx, POId, InvoiceNo);
            int taxId = -1;
            double taxPercentage = 0, taxAmount = 0;
            string taxInclusive = "N";
            inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO();
            inventoryAdjustmentsDTO.DocumentTypeID = DocumentTypeId;
            inventoryLot = new InventoryLotBL(machineContext);
            switch (code)
            {
                case "AJIS":
                    if (inventoryIssueLinesDTO.ToLocationID < 0)
                    {
                        int adjustmentTypeId = -1;
                        lookupValuesSearchParams = new List<KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineContext.GetSiteId().ToString()));
                        lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_ADJUSTMENT_TYPE"));
                        lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Adjustment"));
                        lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                        {
                            adjustmentTypeId = lookupValuesDTOList[0].LookupValueId;
                        }
                        inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO(-1, "Adjustment", inventoryIssueLinesDTO.Quantity, inventoryIssueLinesDTO.FromLocationID, inventoryIssueLinesDTO.ToLocationID,
                            "Adjustment Issue", inventoryIssueLinesDTO.ProductId, DateTime.Now, machineContext.GetUserId(), string.Empty, adjustmentTypeId, -1, 0, DocumentTypeId, false, -1, inventoryIssueLinesDTO.UOMId, -1);
                        InventoryAdjustmentsBL inventoryAdjustmentsBL = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, machineContext);
                        inventoryAdjustmentsBL.SaveInventory(SQLTrx);
                    }
                    else
                    {
                        inventoryLot.ExecuteInventoryLotIssue(inventoryIssueLinesDTO.ProductId, inventoryIssueLinesDTO.Quantity, inventoryIssueLinesDTO.FromLocationID, inventoryIssueLinesDTO.ToLocationID, machineContext.GetSiteId(), machineContext.GetUserId(), "ISSUELINE", inventoryIssueLinesDTO.Guid, DocumentTypeId, SQLTrx);
                    }
                    break;
                case "DIIS":
                    inventoryReceiptDTO = new InventoryReceiptDTO();
                    inventoryReceiptDTO.DocumentTypeID = DocumentTypeId;
                    inventoryReceiptDTO.PurchaseOrderId = POId;
                    inventoryReceiptDTO.ReceiveDate = ServerDateTime.Now;
                    inventoryReceiptDTO.ReceivedBy = machineContext.GetUserId();
                    inventoryReceiptDTO.ReceiveToLocationID = inventoryIssueLinesDTO.ToLocationID;
                    inventoryReceiptDTO.Remarks = "Direct Issue";
                    inventoryReceiptDTO.VendorBillNumber = InvoiceNo;
                    inventoryReceipts = new InventoryReceiptsBL(inventoryReceiptDTO, machineContext);
                    inventoryReceipts.Save(SQLTrx);
                    if (inventoryReceiptDTO.ReceiptId > 0)
                    {
                        searchByPurchaseOrderLineParameters = new List<KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>>();
                        searchByPurchaseOrderLineParameters.Add(new KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.IS_ACTIVE, "Y"));
                        searchByPurchaseOrderLineParameters.Add(new KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PURCHASE_ORDER_ID, POId.ToString()));
                        searchByPurchaseOrderLineParameters.Add(new KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>(PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters.PRODUCT_ID, inventoryIssueLinesDTO.ProductId.ToString()));
                        purchaseOrderLineDTOList = purchaseOrderLineList.GetAllPurchaseOrderLine(searchByPurchaseOrderLineParameters);
                        if (purchaseOrderLineDTOList != null && purchaseOrderLineDTOList.Count > 0)
                        {
                            inventoryReceiveLinesDTO = new InventoryReceiveLinesDTO();
                            inventoryReceiveLinesDTO.ProductId = inventoryIssueLinesDTO.ProductId;
                            inventoryReceiveLinesDTO.IsReceived = "Y";
                            List<LocationDTO> locationDTOList = new List<LocationDTO>();
                            LocationList locationList = new LocationList(machineContext);
                            locationDTOList = locationList.GetAllLocations("Receive");
                            if (locationDTOList == null || locationDTOList.Count == 0)
                            {
                                throw new Exception(" There is no recieve location defined.");
                            }
                            inventoryReceiveLinesDTO.LocationId = locationDTOList[0].LocationId;//inventoryIssueLinesDTO.ToLocationID;
                            inventoryReceiveLinesDTO.PurchaseOrderId = POId;
                            inventoryReceiveLinesDTO.PurchaseOrderLineId = purchaseOrderLineDTOList[0].PurchaseOrderLineId;
                            inventoryReceiveLinesDTO.Quantity = inventoryIssueLinesDTO.Quantity;
                            inventoryReceiveLinesDTO.VendorBillNumber = InvoiceNo;
                            inventoryReceiveLinesDTO.Price = inventoryIssueLinesDTO.Price;
                            GetProductTax(inventoryReceiveLinesDTO.ProductId, ref taxId, ref taxPercentage, ref taxInclusive);
                            if (taxInclusive == "Y")
                            {
                                inventoryReceiveLinesDTO.Price = inventoryReceiveLinesDTO.Price / (1 + taxPercentage / 100);
                                taxAmount = inventoryReceiveLinesDTO.Price * taxPercentage / 100;
                            }
                            else
                            {
                                taxAmount = inventoryReceiveLinesDTO.Price * taxPercentage / 100;
                            }
                            inventoryReceiveLinesDTO.TaxPercentage = taxPercentage;
                            //inventoryReceiveLinesDTO.TaxId = taxId;
                            inventoryReceiveLinesDTO.PurchaseTaxId = taxId;
                            inventoryReceiveLinesDTO.Amount = inventoryReceiveLinesDTO.Quantity * (inventoryReceiveLinesDTO.Price + taxAmount);
                            inventoryReceiveLinesDTO.TaxInclusive = taxInclusive;
                            inventoryReceiveLinesDTO.Description = purchaseOrderLineDTOList[0].Description;
                            inventoryReceiveLinesDTO.ReceiptId = inventoryReceiptDTO.ReceiptId;
                            inventoryReceiveLines = new InventoryReceiveLinesBL(inventoryReceiveLinesDTO, machineContext);
                            inventoryReceiveLines.Save(SQLTrx);
                            if (inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId == -1)
                            {
                                throw new Exception("Receive line is not saved for the product " + inventoryIssueLinesDTO.ProductName);
                            }
                        }
                        else
                        {
                            throw new Exception("There is no purchase order line for the product " + inventoryIssueLinesDTO.ProductName);
                        }
                        inventoryLotDTO = new InventoryLotDTO();
                        inventoryLotDTO.BalanceQuantity = 0;
                        inventoryLotDTO.ReceivePrice = inventoryIssueLinesDTO.Price;//Added on 25-Oct-2016
                        inventoryLotDTO.OriginalQuantity = inventoryIssueLinesDTO.Quantity;
                        //inventoryLotDTO.LotNumber = "RT-001";//Todo:call sequence method.
                        inventoryLotDTO.IsActive = true;
                        inventoryLotDTO.PurchaseOrderReceiveLineId = inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId;
                        inventoryLot = new InventoryLotBL(inventoryLotDTO, machineContext);
                        inventoryLot.Save(SQLTrx);
                        if (inventoryLotDTO.LotId > 0)
                        {
                            //SetAdjustment(inventoryIssueLinesDTO);
                            //inventoryAdjustmentsDTO.Remarks = "Direct Issue";
                            //inventoryAdjustmentsDTO.LotID = inventoryLotDTO.LotId;
                            //inventoryAdjustments = new InventoryAdjustments.InventoryAdjustmentsBL(inventoryAdjustmentsDTO);
                            //inventoryAdjustments.Save(SQLTrx);
                        }
                        else
                        {
                            throw new Exception("Lot details are not save for the product " + inventoryIssueLinesDTO.ProductName);
                        }
                    }
                    else
                    {
                        throw new Exception("Receipt is not saved for the product " + inventoryIssueLinesDTO.ProductName);
                    }
                    try
                    {

                        //DTO inventoryDTO = new DTO();
                        //inventoryDTO.LocationId = inventoryReceiveLinesDTO.LocationId;
                        //inventoryDTO.LotId = inventoryLotDTO.LotId;
                        //inventoryDTO.ProductId = inventoryReceiveLinesDTO.ProductId;
                        //inventoryDTO.Quantity = 0;
                        //inventoryDTO.ReceivePrice = inventoryReceiveLinesDTO.Price;
                        //inventoryDTO.Remarks = "Direct Isuue";                        
                        // inventory = new (inventoryDTO);
                        //inventory.Save(SQLTrx);
                        lookupValuesSearchParams = new List<KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                        lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, inventoryIssueHeaderDTO.SiteId.ToString()));
                        lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_TRANSACTION_TYPE"));
                        lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "DirectIssue"));
                        lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                        if (lookupValuesDTOList == null || (lookupValuesDTOList != null && lookupValuesDTOList.Count == 0))
                        {
                            throw new Exception("The Lookups INVENTORY_TRANSACTION_TYPE not found the value 'DirectIssue'");
                        }
                        InventoryTransactionDTO inventoryTransactionDTO = new InventoryTransactionDTO();
                        inventoryTransactionDTO.Applicability = "ISSUELINE";
                        inventoryTransactionDTO.OriginalReferenceGUID = inventoryIssueLinesDTO.Guid;
                        inventoryTransactionDTO.InventoryTransactionTypeId = lookupValuesDTOList[0].LookupValueId;
                        inventoryTransactionDTO.LineId = inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId;
                        inventoryTransactionDTO.LocationId = inventoryIssueLinesDTO.ToLocationID;
                        inventoryTransactionDTO.ProductId = inventoryIssueLinesDTO.ProductId;
                        inventoryTransactionDTO.Quantity = inventoryIssueLinesDTO.Quantity;
                        inventoryTransactionDTO.SalePrice = inventoryIssueLinesDTO.Price;
                        inventoryTransactionDTO.TaxInclusivePrice = "";
                        inventoryTransactionDTO.TaxPercentage = 0.0;
                        inventoryTransactionDTO.TransactionDate = ServerDateTime.Now;
                        InventoryTransactionBL inventoryTransactionBL = new InventoryTransactionBL(inventoryTransactionDTO, machineContext);
                        inventoryTransactionBL.Save(SQLTrx);
                        //InventoryIssueHeaderDataHandler inventoryIssueHeaderDataHandler = new InventoryIssueHeaderDataHandler();
                        //inventoryIssueHeaderDataHandler.insertInventoryTransaction(inventoryIssueLinesDTO.ProductId, inventoryIssueLinesDTO.ToLocationID, inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId, inventoryIssueLinesDTO.Quantity, inventoryIssueLinesDTO.Price, 0.0, machineContext.GetUserId(), machineContext.GetSiteId(), SQLTrx);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        log.LogMethodExit(null, "throwing exception");
                        throw;
                    }
                    UpdateInventoryRequisition(inventoryIssueLinesDTO, SQLTrx);
                    break;
                case "REIS":
                    inventoryLot.ExecuteInventoryLotIssue(inventoryIssueLinesDTO.ProductId, inventoryIssueLinesDTO.Quantity, inventoryIssueLinesDTO.FromLocationID, inventoryIssueLinesDTO.ToLocationID, machineContext.GetSiteId(), machineContext.GetUserId(), "ISSUELINE", inventoryIssueLinesDTO.Guid, DocumentTypeId, SQLTrx);
                    UpdateInventoryRequisition(inventoryIssueLinesDTO, SQLTrx);
                    break;
                case "STIS":
                    inventoryLot.ExecuteInventoryLotIssue(inventoryIssueLinesDTO.ProductId, inventoryIssueLinesDTO.Quantity, inventoryIssueLinesDTO.FromLocationID, inventoryIssueLinesDTO.ToLocationID, machineContext.GetSiteId(), machineContext.GetUserId(), "ISSUELINE", inventoryIssueLinesDTO.Guid, DocumentTypeId, SQLTrx);
                    break;
                case "ITIS":
                    inventoryLot.ExecuteInventoryLotIssue(inventoryIssueLinesDTO.ProductId, inventoryIssueLinesDTO.Quantity, inventoryIssueLinesDTO.FromLocationID, inventoryIssueLinesDTO.ToLocationID, machineContext.GetSiteId(), machineContext.GetUserId(), "ISSUELINE", inventoryIssueLinesDTO.Guid, DocumentTypeId, SQLTrx);
                    //if (inventoryIssueLinesDTO.RequisitionID > -1 && inventoryIssueLinesDTO.RequisitionLineID > -1)
                    //{
                    //    UpdateInventoryRequisition(inventoryIssueLinesDTO, SQLTrx);
                    //}
                    break;
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// This method processes the approval or reject requests
        /// </summary>
        /// <param name="userMessagesDTO">UserMessagesDTO object</param>
        /// <param name="userMessagesStatus">UserMessagesDTO.UserMessagesStatus type data</param>
        /// <param name="utilities">The utilities object</param>
        /// <param name="sqlTransaction">SqlTransaction object.</param>
        public string ProcessRequests(UserMessagesDTO userMessagesDTO, UserMessagesDTO.UserMessagesStatus userMessagesStatus, Utilities utilities, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(userMessagesDTO, userMessagesStatus, sqlTransaction);
            string message = string.Empty;
            bool isMasterSite = (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite);
            InventoryIssueHeaderList inventoryIssueHeaderList = new InventoryIssueHeaderList();
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList();
            UserMessages userMessages = new UserMessages(machineContext);
            List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchByInventoryIssueHeaderParameters = new List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>();
            List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>> searchByInventoryIssueLinesParameters = new List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>>();
            UserMessagesList userMessagesList = new UserMessagesList(machineContext);
            List<UserMessagesDTO> userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage(userMessagesDTO.ModuleType, userMessagesDTO.ObjectType, userMessagesDTO.ObjectGUID, -1, -1, machineContext.GetSiteId(), sqlTransaction);

            if (userMessagesDTOList != null && userMessagesDTOList.Count > 0)
            {
                UserMessagesDTO approvedUserMessagesDTO = userMessages.GetHighestApprovedUserMessage(userMessagesDTO.ApprovalRuleID, -1, -1, userMessagesDTO.ModuleType, userMessagesDTO.ObjectType, userMessagesDTO.ObjectGUID, machineContext.GetSiteId(), sqlTransaction);
                foreach (UserMessagesDTO higherUserMessagesDTO in userMessagesDTOList)
                {
                    if (approvedUserMessagesDTO != null)
                    {
                        if (approvedUserMessagesDTO.Level < higherUserMessagesDTO.Level)
                        {
                            log.LogMethodExit("Ends-ProcessRequests(userMessagesDTO,userMessagesStatus,siteId,isMasterSite,sqlTransaction) method.");
                            return message;
                        }
                    }
                }
            }

            searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG, "1"));
            searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.GUID, userMessagesDTO.ObjectGUID));

            List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = inventoryIssueHeaderList.GetAllInventoryIssueHeaders(searchByInventoryIssueHeaderParameters, null);
            if (inventoryIssueHeaderDTOList == null || (inventoryIssueHeaderDTOList != null && inventoryIssueHeaderDTOList.Count == 0))
            {
                throw new Exception("There is no issue record found.");
            }

            searchByInventoryIssueLinesParameters.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ACTIVE_FLAG, "1"));
            searchByInventoryIssueLinesParameters.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_ID, inventoryIssueHeaderDTOList[0].InventoryIssueId.ToString()));

            List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = inventoryIssueLinesList.GetAllInventoryIssueLines(searchByInventoryIssueLinesParameters);
            if (inventoryIssueLinesDTOList == null || (inventoryIssueLinesDTOList != null && inventoryIssueLinesDTOList.Count == 0))
            {
                throw new Exception("There is no issue line records found.");
            }
            if ((userMessagesDTO.ObjectType.Equals("ITIS") && utilities.ParafaitEnv.SiteId == inventoryIssueHeaderDTOList[0].FromSiteID) || (!userMessagesDTO.ObjectType.Equals("ITIS")))//Transfered from master site or other site adding stock to inventory 
            {
                if (userMessagesStatus.Equals(UserMessagesDTO.UserMessagesStatus.APPROVED))
                {
                    foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesDTOList)
                    {
                        //SaveIssueDetails(userMessagesDTO.ObjectType, inventoryIssueHeaderDTOList[0].DocumentTypeId, inventoryIssueLinesDTO, sqlTransaction, ((inventoryIssueHeaderDTOList[0].PurchaseOrderId != -1) ? inventoryIssueHeaderDTOList[0].PurchaseOrderId : (inventoryIssueHeaderDTOList[0].RequisitionID != -1) ? inventoryIssueHeaderDTOList[0].RequisitionID : -1), inventoryIssueHeaderDTOList[0].SupplierInvoiceNumber);
                        if (inventoryIssueLinesDTO.IssueLineId != -1
                                                   && inventoryIssueLinesDTO.IssueLineId == inventoryIssueLinesDTOList[inventoryIssueLinesDTOList.Count - 1].IssueLineId)
                        {
                            if (!userMessagesDTO.ObjectType.Equals("ITIS"))
                            {
                                if (inventoryIssueHeaderDTOList[0] != null)
                                {
                                    inventoryIssueHeaderDTOList[0].Status = "COMPLETE";
                                }
                            }
                        }
                    }
                    //if (isMasterSite && userMessagesDTO.ObjectType.Equals("ITIS"))
                    if (userMessagesDTO.ObjectType.Equals("ITIS"))
                    {
                        InventoryDocumentTypeList inventoryDocumentType = new InventoryDocumentTypeList(machineContext);
                        InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentType.GetInventoryDocumentType(inventoryIssueHeaderDTOList[0].DocumentTypeId);
                        ProcessAsAutoHQApproval(inventoryIssueHeaderDTOList[0], inventoryDocumentTypeDTO, utilities, sqlTransaction);
                    }
                    inventoryIssueHeaderDTOList[0].IsActive = true;
                    this.inventoryIssueHeaderDTO = inventoryIssueHeaderDTOList[0];
                    Save(sqlTransaction);
                    message = MessageContainerList.GetMessage(machineContext, 5057);
                }
                else if (userMessagesStatus.Equals(UserMessagesDTO.UserMessagesStatus.REJECTED))
                {
                    InventoryLotDTO inventoryLotDTO = null;
                    InventoryLotList inventoryLotList = new InventoryLotList(machineContext);
                    List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>> searchByInventoryTransactionParameters;
                    InventoryList inventoryList = new InventoryList();
                    InventoryTransactionList inventoryTransactionList = new InventoryTransactionList(machineContext);
                    List<InventoryTransactionDTO> inventoryTransactionDTOList = null;
                    InventoryDTO inventoryDTO;
                    Inventory inventory;
                    InventoryLotBL inventoryLotBL;
                    machineContext.SetSiteId(inventoryIssueHeaderDTOList[0].SiteId);
                    foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesDTOList)// Incase of HQ rejection putting reverse entry of stock
                    {
                        //try
                        //{
                        //    locationId = inventoryIssueLinesDTO.ToLocationID;
                        //    inventoryIssueLinesDTO.ToLocationID = inventoryIssueLinesDTO.FromLocationID;
                        //    inventoryIssueLinesDTO.FromLocationID = locationId;
                        //    SaveIssueDetails(userMessagesDTO.ObjectType, inventoryIssueHeaderDTOList[0].DocumentTypeId, inventoryIssueLinesDTO, sqlTransaction, ((inventoryIssueHeaderDTOList[0].PurchaseOrderId != -1) ? inventoryIssueHeaderDTOList[0].PurchaseOrderId : (inventoryIssueHeaderDTOList[0].RequisitionID != -1) ? inventoryIssueHeaderDTOList[0].RequisitionID : -1), inventoryIssueHeaderDTOList[0].SupplierInvoiceNumber);
                        //}
                        //catch (Exception ex)
                        //{

                        searchByInventoryTransactionParameters = new List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>>();
                        searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.SITE_ID, machineContext.GetSiteId().ToString()));
                        searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.PRODUCT_ID, inventoryIssueLinesDTO.ProductId.ToString()));
                        searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.ORIGINAL_REFERENCE_GUID, inventoryIssueLinesDTO.Guid.ToString()));
                        searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.APPLICABILITY, "ISSUELINE"));
                        inventoryTransactionDTOList = inventoryTransactionList.GetInventoryTransactionList(searchByInventoryTransactionParameters, sqlTransaction);
                        if (inventoryTransactionDTOList != null && inventoryTransactionDTOList.Count > 0)
                        {
                            inventoryDTO = inventoryList.GetInventory(inventoryTransactionDTOList[0].ProductId, inventoryIssueLinesDTO.FromLocationID, inventoryTransactionDTOList[0].LotId, sqlTransaction);
                            if (inventoryDTO != null && inventoryDTO.ProductId > -1)
                            {
                                inventoryDTO.Quantity += inventoryIssueLinesDTO.Quantity;
                                inventory = new Inventory(inventoryDTO, machineContext);
                                inventory.Save(sqlTransaction);
                                if (inventoryDTO.LotId > -1)
                                {
                                    inventoryLotDTO = inventoryLotList.GetInventoryLot(inventoryDTO.LotId);
                                    if (inventoryLotDTO != null && inventoryLotDTO.LotId > -1)
                                    {
                                        inventoryLotDTO.Quantity += inventoryIssueLinesDTO.Quantity;
                                        inventoryLotBL = new InventoryLotBL(inventoryLotDTO, machineContext);
                                        inventoryLotBL.Save(sqlTransaction);
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("Failed to fetch the original inventory record.");
                            }

                        }
                        //else
                        //{
                        //    throw new Exception("Failed to fetch the original transaction.");
                        //}
                        //}
                    }
                    inventoryIssueHeaderDTOList[0].Status = "CANCELLED";
                    this.inventoryIssueHeaderDTO = inventoryIssueHeaderDTOList[0];
                    Save(sqlTransaction);
                    message = MessageContainerList.GetMessage(machineContext, 5058);
                }
            }
            //else if (userMessagesDTO.ObjectType.Equals("ITIS") && isMasterSite)//Not from master site and to master site or other site tarnsfer
            else if (userMessagesDTO.ObjectType.Equals("ITIS"))//Not from master site and to master site or other site tarnsfer
            {
                if (userMessagesStatus.Equals(UserMessagesDTO.UserMessagesStatus.APPROVED))
                {
                    InventoryDocumentTypeList inventoryDocumentType = new InventoryDocumentTypeList(machineContext);
                    InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentType.GetInventoryDocumentType(inventoryIssueHeaderDTOList[0].DocumentTypeId);
                    ProcessAsAutoHQApproval(inventoryIssueHeaderDTOList[0], inventoryDocumentTypeDTO, utilities, sqlTransaction);
                    machineContext.SetSiteId(inventoryIssueHeaderDTOList[0].SiteId);
                    inventoryIssueHeaderDTOList[0].IsChanged = true;
                    inventoryIssueHeaderDTOList[0].Status = "COMPLETE";
                    this.inventoryIssueHeaderDTO = inventoryIssueHeaderDTOList[0];
                    Save(sqlTransaction);
                    machineContext.SetSiteId((utilities.ParafaitEnv.IsCorporate) ? utilities.ParafaitEnv.SiteId : -1);
                    message = MessageContainerList.GetMessage(machineContext, 5057);
                }
                else if (userMessagesStatus.Equals(UserMessagesDTO.UserMessagesStatus.REJECTED))
                {
                    //int locationId = -1;
                    InventoryLotDTO inventoryLotDTO = null;
                    InventoryLotList inventoryLotList = new InventoryLotList(machineContext);
                    List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>> searchByInventoryTransactionParameters;
                    InventoryList inventoryList = new InventoryList();
                    InventoryTransactionList inventoryTransactionList = new InventoryTransactionList(machineContext);
                    List<InventoryTransactionDTO> inventoryTransactionDTOList = null;
                    InventoryDTO inventoryDTO;
                    Inventory inventory;
                    InventoryLotBL inventoryLotBL;
                    machineContext.SetSiteId(inventoryIssueHeaderDTOList[0].SiteId);
                    foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesDTOList)// Incase of HQ rejection putting reverse entry of stock
                    {
                        //try
                        //{
                        //    locationId = inventoryIssueLinesDTO.ToLocationID;
                        //    inventoryIssueLinesDTO.ToLocationID = inventoryIssueLinesDTO.FromLocationID;
                        //    inventoryIssueLinesDTO.FromLocationID = locationId;
                        //    SaveIssueDetails(userMessagesDTO.ObjectType, inventoryIssueHeaderDTOList[0].DocumentTypeId, inventoryIssueLinesDTO, sqlTransaction, ((inventoryIssueHeaderDTOList[0].PurchaseOrderId != -1) ? inventoryIssueHeaderDTOList[0].PurchaseOrderId : (inventoryIssueHeaderDTOList[0].RequisitionID != -1) ? inventoryIssueHeaderDTOList[0].RequisitionID : -1), inventoryIssueHeaderDTOList[0].SupplierInvoiceNumber);
                        //}
                        //catch (Exception ex)
                        //{

                        searchByInventoryTransactionParameters = new List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>>();
                        searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.SITE_ID, machineContext.GetSiteId().ToString()));
                        searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.PRODUCT_ID, inventoryIssueLinesDTO.ProductId.ToString()));
                        searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.ORIGINAL_REFERENCE_GUID, inventoryIssueLinesDTO.Guid.ToString()));
                        searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.APPLICABILITY, "ISSUELINE"));
                        inventoryTransactionDTOList = inventoryTransactionList.GetInventoryTransactionList(searchByInventoryTransactionParameters, sqlTransaction);
                        if (inventoryTransactionDTOList != null && inventoryTransactionDTOList.Count > 0)
                        {
                            inventoryDTO = inventoryList.GetInventory(inventoryTransactionDTOList[0].ProductId, inventoryIssueLinesDTO.FromLocationID, inventoryTransactionDTOList[0].LotId, sqlTransaction);
                            if (inventoryDTO != null && inventoryDTO.ProductId > -1)
                            {
                                inventoryDTO.Quantity += inventoryIssueLinesDTO.Quantity;
                                inventory = new Inventory(inventoryDTO, machineContext);
                                inventory.Save(sqlTransaction);
                                if (inventoryDTO.LotId > -1)
                                {
                                    inventoryLotDTO = inventoryLotList.GetInventoryLot(inventoryDTO.LotId);
                                    if (inventoryLotDTO != null && inventoryLotDTO.LotId > -1)
                                    {
                                        inventoryLotDTO.Quantity += inventoryIssueLinesDTO.Quantity;
                                        inventoryLotBL = new InventoryLotBL(inventoryLotDTO, machineContext);
                                        inventoryLotBL.Save(sqlTransaction);
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("Failed to fetch the original inventory record.");
                            }

                        }
                        //else
                        //{
                        //    throw new Exception("Failed to fetch the original transaction.");
                        //}
                        //}
                    }
                    inventoryIssueHeaderDTOList[0].Status = "CANCELLED";
                    this.inventoryIssueHeaderDTO = inventoryIssueHeaderDTOList[0];
                    Save(sqlTransaction);
                    machineContext.SetSiteId((utilities.ParafaitEnv.IsCorporate) ? utilities.ParafaitEnv.SiteId : -1);
                    message = MessageContainerList.GetMessage(machineContext, 5057);
                }
            }
            else
            {
                throw new Exception("Invalid request to process.");
            }
            log.LogMethodExit(message);
            return message;
        }

        private void GetProductTax(int productId, ref int taxId, ref double taxPercentage, ref string taxInc)
        {
            log.LogMethodEntry(productId, taxId, taxPercentage, taxInc);
            ProductList productList = new ProductList();
            ProductDTO productDTO = new ProductDTO();
            TaxDTO purchaseTaxDTO = new TaxDTO();
            Tax purchaseTax = new Tax(machineContext, purchaseTaxDTO);

            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> searchByPurchaseTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();

            productDTO = productList.GetProduct(productId);
            if (productDTO != null && productDTO.ProductId != -1)
            {
                taxInc = productDTO.TaxInclusiveCost;
                if (productDTO.PurchaseTaxId != -1)
                {
                    purchaseTax = new Tax(machineContext, productDTO.PurchaseTaxId);
                    purchaseTaxDTO = purchaseTax.GetTaxDTO();
                    if (purchaseTaxDTO != null && purchaseTaxDTO.TaxId != -1)
                    {
                        taxId = purchaseTaxDTO.TaxId;
                        taxPercentage = purchaseTaxDTO.TaxPercentage;
                    }
                    else
                    {
                        throw new Exception("No tax found!!...");
                    }
                }
            }
            else
            {
                throw new Exception("No product found!!...");
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Reject the issues based on the parameter passed
        /// </summary>
        public void AutoRejectIssues(int numberOfDaysValidity, int siteId, Utilities utilities)
        {
            log.LogMethodEntry(numberOfDaysValidity, siteId, utilities);
            ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
            try
            {
                machineUserContext.SetUserId("External POS");
                if (numberOfDaysValidity > 0)
                {
                    log.LogMethodEntry("AutoRejectIssues( numberOfDaysValidity, siteId) method. SHOW_MESSAGES_FOR_DAYS_IN_INBOX value is set to:" + numberOfDaysValidity + " and valid.");
                    InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(machineUserContext);
                    List<InventoryDocumentTypeDTO> inventoryDocumentTypeDTOList = new List<InventoryDocumentTypeDTO>();
                    List<InventoryDocumentTypeDTO> inventoryDocumentTypeFilterDTOList = new List<InventoryDocumentTypeDTO>();
                    UserMessagesList userMessagesList = new UserMessagesList();
                    List<UserMessagesDTO> userMessagesDTOList;
                    List<UserMessagesDTO> userMessagesDTOofIssueList;
                    List<UserMessagesDTO> userMessagesDTORemoveList = new List<UserMessagesDTO>();
                    List<UserMessagesDTO> userMessagesDTOofDiffStatusList;
                    string[] applicability = new string[] { "ISSUE" };
                    inventoryDocumentTypeDTOList = inventoryDocumentTypeList.GetAllInventoryDocumentTypesByApplicability(applicability, siteId);
                    List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID, siteId.ToString()));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.STATUS, UserMessagesDTO.UserMessagesStatus.PENDING.ToString()));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.LAST_UPDATED_DATE_TILL, DateTime.Today.AddDays(-1 * numberOfDaysValidity).ToString("yyyy-MM-dd HH:mm:ss")));
                    userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);
                    if (inventoryDocumentTypeDTOList != null && inventoryDocumentTypeDTOList.Count > 0)
                    {
                        if (userMessagesDTOList != null && userMessagesDTOList.Count > 0)
                        {
                            foreach (UserMessagesDTO userMessagesDTO in userMessagesDTOList)
                            {
                                List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchByInventoryIssueHeaderParameters = new List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>();
                                searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG, "1"));
                                searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.GUID, userMessagesDTO.ObjectGUID));
                                InventoryIssueHeaderList inventoryIssueHeaderList = new InventoryIssueHeaderList();
                                List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = inventoryIssueHeaderList.GetAllInventoryIssueHeaders(searchByInventoryIssueHeaderParameters, null);
                                if (inventoryIssueHeaderDTOList != null && inventoryIssueHeaderDTOList.Count > 0
                                   && inventoryIssueHeaderDTOList[0].FromSiteID != -1 && inventoryIssueHeaderDTOList[0].FromSiteID != siteId
                                       && !(utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite))

                                {
                                    if (!userMessagesDTORemoveList.Contains(userMessagesDTO))
                                        userMessagesDTORemoveList.Add(userMessagesDTO);
                                    continue;
                                }
                                inventoryDocumentTypeFilterDTOList = inventoryDocumentTypeDTOList.Where(x => (bool)(x.Code == userMessagesDTO.ObjectType)).ToList<InventoryDocumentTypeDTO>();
                                if (inventoryDocumentTypeFilterDTOList != null && inventoryDocumentTypeFilterDTOList.Count > 0)
                                {
                                    userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID, siteId.ToString()));
                                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, userMessagesDTO.ObjectType));
                                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, userMessagesDTO.ObjectGUID));
                                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                                    userMessagesDTOofIssueList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);
                                    if (userMessagesDTOofIssueList != null && userMessagesDTOofIssueList.Count > 0)
                                    {
                                        userMessagesDTOofDiffStatusList = userMessagesDTOofIssueList.Where(x => (bool)(x.Status != UserMessagesDTO.UserMessagesStatus.PENDING.ToString())).ToList<UserMessagesDTO>();
                                        if (userMessagesDTOofDiffStatusList != null && userMessagesDTOofDiffStatusList.Count > 0)
                                        {
                                            foreach (UserMessagesDTO userMessagesDTOActed in userMessagesDTOofIssueList)
                                            {
                                                if (!userMessagesDTORemoveList.Contains(userMessagesDTOActed))
                                                    userMessagesDTORemoveList.Add(userMessagesDTOActed);
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    if (!userMessagesDTORemoveList.Contains(userMessagesDTO))
                                        userMessagesDTORemoveList.Add(userMessagesDTO);
                                }

                            }
                            foreach (UserMessagesDTO userMessagesDTO in userMessagesDTORemoveList)
                            {
                                if (userMessagesDTOList.Exists(x => (bool)(x.MessageId == userMessagesDTO.MessageId)))
                                    userMessagesDTOList.Remove(userMessagesDTOList.Where(x => (bool)(x.MessageId == userMessagesDTO.MessageId)).ToList<UserMessagesDTO>()[0]);
                                if (userMessagesDTOList.Count(x => (bool)(x.ObjectGUID == userMessagesDTO.ObjectGUID)) > 1)
                                    userMessagesDTOList.Remove(userMessagesDTOList.Where(x => (bool)(x.ObjectGUID == userMessagesDTO.ObjectGUID)).ToList<UserMessagesDTO>()[0]);
                            }
                            using (ParafaitDBTransaction parafaitDBTrxn = new ParafaitDBTransaction())
                            {
                                try
                                {
                                    UserMessages userMessages = new UserMessages(machineContext);
                                    parafaitDBTrxn.BeginTransaction();
                                    foreach (UserMessagesDTO userMessagesDTO in userMessagesDTOList)
                                    {
                                        inventoryDocumentTypeFilterDTOList = inventoryDocumentTypeDTOList.Where(x => (bool)(x.Code == userMessagesDTO.ObjectType)).ToList<InventoryDocumentTypeDTO>();
                                        if (inventoryDocumentTypeFilterDTOList != null && inventoryDocumentTypeFilterDTOList.Count > 0)
                                        {
                                            Users users = new Users(utilities.ExecutionContext, "External POS");
                                            userMessages.UpdateStatus(userMessagesDTO, UserMessagesDTO.UserMessagesStatus.REJECTED, inventoryDocumentTypeFilterDTOList[0].DocumentTypeId, users.UserDTO.UserId, parafaitDBTrxn.SQLTrx);
                                            ProcessRequests(userMessagesDTO, UserMessagesDTO.UserMessagesStatus.REJECTED, utilities, parafaitDBTrxn.SQLTrx);
                                            userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID, siteId.ToString()));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, userMessagesDTO.ObjectType));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, userMessagesDTO.ObjectGUID));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                                            userMessagesDTOofIssueList = userMessagesList.GetAllUserMessages(userMessagesSearchParams);
                                            if (userMessagesDTOofIssueList != null && userMessagesDTOofIssueList.Count > 0)
                                            {
                                                foreach (UserMessagesDTO userMessagesDTOSameIssue in userMessagesDTOofIssueList)
                                                {
                                                    if (userMessagesDTOList.Contains(userMessagesDTOSameIssue))
                                                        userMessagesDTOList.Remove(userMessagesDTOSameIssue);
                                                }
                                            }
                                        }
                                    }
                                    parafaitDBTrxn.EndTransaction();
                                }
                                catch (Exception e)
                                {
                                    parafaitDBTrxn.RollBack();
                                    log.LogMethodExit(e);
                                    throw;
                                }
                            }
                        }
                    }
                    else
                    {
                        log.LogMethodExit();
                    }
                }
                else
                {
                    log.LogMethodExit(numberOfDaysValidity);
                }

            }
            catch (Exception ex)
            {
                log.Error("AutoRejectIssues( numberOfDaysValidity, siteId) method.Exception:" + ex.ToString());
            }
            finally
            {
                machineUserContext.SetUserId("External POS");
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Updates the requisition status
        /// </summary>
        /// <param name="inventoryIssueLinesDTO">InventoryIssueLinesDTO</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void UpdateInventoryRequisition(InventoryIssueLinesDTO inventoryIssueLinesDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(inventoryIssueLinesDTO, sqlTransaction);
            bool isClosed = true;
            List<RequisitionDTO> requisitionDTOList = new List<RequisitionDTO>();
            RequisitionList requisitionList = new RequisitionList(machineContext);
            RequisitionBL requisition;
            List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>> inventoryRequisitionSearchParams = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
            List<RequisitionLinesDTO> requisitionLinesDTOList = new List<RequisitionLinesDTO>();
            RequisitionLinesList requisitionLinesList = new RequisitionLinesList(machineContext);
            RequisitionLinesBL requisitionLines;
            List<ProductDTO> productDTOList = new List<ProductDTO>();
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchByProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            ProductList productList = new ProductList();
            if (this.inventoryIssueHeaderDTO.ToSiteID != -1)
            {
                searchByProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID, inventoryIssueLinesDTO.ProductId.ToString()));
                searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, this.inventoryIssueHeaderDTO.ToSiteID.ToString()));
                productDTOList = productList.GetAllProducts(searchByProductParameters);
            }
            List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>> inventoryRequisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
            if (this.inventoryIssueHeaderDTO.ToSiteID != -1)
            {
                if (productDTOList != null && productDTOList.Count > 0)
                {
                    inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.PRODUCT_ID, productDTOList[0].ProductId.ToString()));
                }
                else
                {
                    throw new Exception("Product not found in the destination site.");
                }

            }
            else
                inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.PRODUCT_ID, inventoryIssueLinesDTO.ProductId.ToString()));
            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, inventoryIssueLinesDTO.RequisitionID.ToString()));
            requisitionLinesDTOList = requisitionLinesList.GetAllRequisitionLines(inventoryRequisitionLinesSearchParams, sqlTransaction);
            if (requisitionLinesDTOList != null && requisitionLinesDTOList.Count > 0)
            {
                foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesDTOList)
                {
                    if (requisitionLinesDTO.ApprovedQuantity == -1)
                    {
                        requisitionLinesDTO.ApprovedQuantity = inventoryIssueLinesDTO.Quantity;
                    }
                    else
                    {
                        requisitionLinesDTO.ApprovedQuantity += inventoryIssueLinesDTO.Quantity;
                    }
                    requisitionLines = new RequisitionLinesBL(machineContext, requisitionLinesDTO);
                    requisitionLines.Save(sqlTransaction);
                }
            }
            //else
            //{
            //    throw new Exception("Requisition line not found.");
            //}
            inventoryRequisitionLinesSearchParams = new List<KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>>();
            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.ACTIVE_FLAG, "1"));
            inventoryRequisitionLinesSearchParams.Add(new KeyValuePair<RequisitionLinesDTO.SearchByRequisitionLinesParameters, string>(RequisitionLinesDTO.SearchByRequisitionLinesParameters.REQUISITION_ID, inventoryIssueLinesDTO.RequisitionID.ToString()));
            requisitionLinesDTOList = requisitionLinesList.GetAllRequisitionLines(inventoryRequisitionLinesSearchParams, sqlTransaction);
            if (requisitionLinesDTOList != null && requisitionLinesDTOList.Count > 0)
            {
                foreach (RequisitionLinesDTO requisitionLinesDTO in requisitionLinesDTOList)
                {
                    if (requisitionLinesDTO.ApprovedQuantity != requisitionLinesDTO.RequestedQuantity)
                    {
                        isClosed = false;
                    }
                }
            }
            inventoryRequisitionSearchParams = new List<KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>>();
            inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.ACTIVE_FLAG, "1"));
            inventoryRequisitionSearchParams.Add(new KeyValuePair<RequisitionDTO.SearchByRequisitionParameters, string>(RequisitionDTO.SearchByRequisitionParameters.REQUISITION_ID, inventoryIssueLinesDTO.RequisitionID.ToString()));
            requisitionDTOList = requisitionList.GetAllRequisitions(inventoryRequisitionSearchParams);
            if (requisitionDTOList != null && requisitionDTOList.Count > 0)
            {
                foreach (RequisitionDTO requisitionDTO in requisitionDTOList)
                {
                    if (isClosed)
                    {
                        requisitionDTO.Status = "Closed";
                    }
                    else
                    {
                        requisitionDTO.Status = "InProgress";
                    }
                    requisition = new RequisitionBL(machineContext, requisitionDTO);
                    requisition.Save(sqlTransaction);
                }
            }
            //else
            //{
            //    throw new Exception("Requisition is not found.");
            //}
            log.LogMethodExit();
        }
        private void AcceptStock(InventoryIssueLinesDTO inventoryIssueLinesDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(inventoryIssueLinesDTO, sqlTransaction);
            lookupValuesDTOList = new List<Core.Utilities.LookupValuesDTO>();
            InventoryTransactionList inventoryTransactionList = new InventoryTransactionList(machineContext);
            List<InventoryTransactionDTO> inventoryTransactionDTOList = new List<InventoryTransactionDTO>();
            InventoryAdjustmentsBL inventoryAdjustmentsBL;
            List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>> searchByInventoryTransactionParameters = new List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>>();
            InventoryDTO inventoryDTO;
            Inventory inventory;
            InventoryList inventoryList = new InventoryList();
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchByInventoryParameters = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();

            lookupValuesSearchParams = new List<KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>>();
            lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, machineContext.GetSiteId().ToString()));
            lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "INVENTORY_ADJUSTMENT_TYPE"));
            lookupValuesSearchParams.Add(new KeyValuePair<Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters, string>(Core.Utilities.LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "Adjustment"));
            lookupValuesDTOList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);

            searchByInventoryTransactionParameters = new List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>>();
            searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.SITE_ID, machineContext.GetSiteId().ToString()));
            searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.PRODUCT_ID, inventoryIssueLinesDTO.ProductId.ToString()));
            searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.ORIGINAL_REFERENCE_GUID, inventoryIssueLinesDTO.Guid.ToString()));
            searchByInventoryTransactionParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.APPLICABILITY, "ISSUELINE"));
            inventoryTransactionDTOList = inventoryTransactionList.GetInventoryTransactionList(searchByInventoryTransactionParameters, sqlTransaction);
            if (inventoryTransactionDTOList != null && inventoryTransactionDTOList.Count > 0)
            {
                foreach (InventoryTransactionDTO inventoryTransactionDTO in inventoryTransactionDTOList)
                {
                    if (lookupValuesDTOList != null && lookupValuesDTOList.Count > 0)
                    {
                        inventoryAdjustmentsDTO.AdjustmentTypeId = lookupValuesDTOList[0].LookupValueId;
                    }
                    inventoryAdjustmentsDTO.AdjustmentQuantity = inventoryTransactionDTO.Quantity;
                    inventoryAdjustmentsDTO.AdjustmentType = "Adjustment";
                    inventoryAdjustmentsDTO.Remarks = "Inter Store Adjustment Issue";
                    inventoryAdjustmentsDTO.Price = inventoryTransactionDTO.SalePrice;
                    inventoryAdjustmentsDTO.FromLocationId = inventoryIssueLinesDTO.FromLocationID;
                    inventoryAdjustmentsDTO.ToLocationId = inventoryIssueLinesDTO.ToLocationID;
                    inventoryAdjustmentsDTO.LotID = inventoryTransactionDTO.LotId;
                    inventoryAdjustmentsDTO.ProductId = inventoryIssueLinesDTO.ProductId;
                    inventoryAdjustmentsBL = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, machineContext);
                    inventoryAdjustmentsBL.Save(sqlTransaction);
                    searchByInventoryParameters = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                    searchByInventoryParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, machineContext.GetSiteId().ToString()));
                    searchByInventoryParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, inventoryIssueLinesDTO.ProductId.ToString()));
                    if (inventoryTransactionDTO.LotId != -1)
                    {
                        searchByInventoryParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, inventoryTransactionDTO.LotId.ToString()));
                    }
                    searchByInventoryParameters.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, inventoryIssueLinesDTO.ToLocationID.ToString()));
                    inventoryDTOList = inventoryList.GetAllInventory(searchByInventoryParameters, false, sqlTransaction);
                    if (inventoryDTOList == null || (inventoryDTOList != null && inventoryDTOList.Count == 0))
                    {
                        inventoryDTO = new InventoryDTO();
                    }
                    else
                    {
                        inventoryDTO = inventoryDTOList[0];
                    }
                    inventoryDTO.ProductId = inventoryIssueLinesDTO.ProductId;
                    inventoryDTO.LotId = inventoryTransactionDTO.LotId;
                    inventoryDTO.LocationId = inventoryIssueLinesDTO.ToLocationID;
                    inventoryDTO.Quantity += inventoryTransactionDTO.Quantity;
                    inventoryDTO.Remarks = "Received from inter store.";
                    inventory = new Inventory(inventoryDTO, machineContext);
                    inventory.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Process the Inter store transfer request either auto approved or HQ approved
        /// </summary>
        /// <param name="issueHeaderRow"> The data row of issue header data</param>
        /// <param name="inventoryDocumentTypeDTO">ITIS document type of the source site </param>
        /// <param name="utilities">utilities </param>
        public void ProcessAsAutoHQApproval(DataRow issueHeaderRow, InventoryDocumentTypeDTO inventoryDocumentTypeDTO, Utilities utilities, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(issueHeaderRow, inventoryDocumentTypeDTO, utilities);
            InventoryIssueHeaderDataHandler inventoryIssueHeaderDataHandler = new InventoryIssueHeaderDataHandler(sqlTransaction);
            inventoryIssueHeaderDTO = new InventoryIssueHeaderDTO();
            inventoryIssueHeaderDTO.CreationDate = issueHeaderRow["CreationDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(issueHeaderRow["CreationDate"]);
            inventoryIssueHeaderDTO.CreatedBy = issueHeaderRow["CreatedBy"].ToString();
            inventoryIssueHeaderDTO.DeliveryNoteDate = issueHeaderRow["DeliveryNoteDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(issueHeaderRow["DeliveryNoteDate"]);
            inventoryIssueHeaderDTO.DeliveryNoteNumber = issueHeaderRow["DeliveryNoteNumber"].ToString();
            inventoryIssueHeaderDTO.DocumentTypeId = inventoryDocumentTypeDTO.DocumentTypeId;
            inventoryIssueHeaderDTO.Guid = issueHeaderRow["Guid"].ToString();
            inventoryIssueHeaderDTO.IssueDate = issueHeaderRow["IssueDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(issueHeaderRow["IssueDate"]);
            inventoryIssueHeaderDTO.IsActive = issueHeaderRow["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(issueHeaderRow["IsActive"]);
            inventoryIssueHeaderDTO.LastUpdatedBy = issueHeaderRow["LastUpdatedBy"].ToString();
            inventoryIssueHeaderDTO.LastUpdatedDate = issueHeaderRow["LastupdatedDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(issueHeaderRow["LastupdatedDate"]);
            inventoryIssueHeaderDTO.MasterEntityId = issueHeaderRow["MasterEntityId"] == DBNull.Value ? -1 : Convert.ToInt32(issueHeaderRow["MasterEntityId"]);
            inventoryIssueHeaderDTO.OriginalReferenceGUID = issueHeaderRow["OriginalReferenceGUID"].ToString();
            inventoryIssueHeaderDTO.Remarks = issueHeaderRow["Remarks"].ToString();
            inventoryIssueHeaderDTO.Status = issueHeaderRow["Status"].ToString();
            inventoryIssueHeaderDTO.SupplierInvoiceDate = issueHeaderRow["SupplierInvoiceDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(issueHeaderRow["SupplierInvoiceDate"]);
            inventoryIssueHeaderDTO.SupplierInvoiceNumber = issueHeaderRow["SupplierInvoiceNumber"].ToString();
            inventoryIssueHeaderDTO.FromSiteID = issueHeaderRow["FromSiteID"] == DBNull.Value ? -1 : Convert.ToInt32(issueHeaderRow["FromSiteID"]);
            inventoryIssueHeaderDTO.ToSiteID = issueHeaderRow["ToSiteID"] == DBNull.Value ? -1 : Convert.ToInt32(issueHeaderRow["ToSiteID"]);
            inventoryIssueHeaderDTO.SiteId = issueHeaderRow["site_id"] == DBNull.Value ? -1 : Convert.ToInt32(issueHeaderRow["site_id"]);

            if (issueHeaderRow["Guid"] != DBNull.Value)
            {
                List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchByInventoryIssueHeaderParameters = new List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>>();
                searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.ACTIVE_FLAG, "1"));
                searchByInventoryIssueHeaderParameters.Add(new KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>(InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters.GUID, issueHeaderRow["Guid"].ToString()));
                InventoryIssueHeaderList inventoryIssueHeaderList = new InventoryIssueHeaderList();
                List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = inventoryIssueHeaderList.GetAllInventoryIssueHeaders(searchByInventoryIssueHeaderParameters, null);
                if (inventoryIssueHeaderDTOList == null || (inventoryIssueHeaderDTOList != null && inventoryIssueHeaderDTOList.Count == 0))
                {
                    log.LogMethodExit();
                    throw new Exception("Problem in fetching inventory issue.");
                }
                else
                {
                    inventoryIssueHeaderDTO.InventoryIssueId = inventoryIssueHeaderDTOList[0].InventoryIssueId;
                }

            }

            if (issueHeaderRow["RequisitionID"] != DBNull.Value)
            {
                RequisitionBL requisitionBL = new RequisitionBL(machineContext, issueHeaderRow["RequisitionID"].ToString());
                inventoryIssueHeaderDTO.RequisitionID = requisitionBL.GetRequisitionsDTO.RequisitionId;
            }
            if (issueHeaderRow["PurchaseOrderId"] != DBNull.Value)
            {
                PurchaseOrder purchaseOrder = new PurchaseOrder(issueHeaderRow["PurchaseOrderId"].ToString(), machineContext);
                inventoryIssueHeaderDTO.PurchaseOrderId = purchaseOrder.getPurchaseOrderDTO.PurchaseOrderId;
            }
            ProcessAsAutoHQApproval(inventoryIssueHeaderDTO, inventoryDocumentTypeDTO, utilities);
            log.LogMethodExit();
        }

        /// <summary>
        /// Process the Inter store transfer request either auto approved or HQ approved
        /// </summary>
        /// <param name="inventoryIssueHeaderDTO"> Inventory issue header DTO is the record which is transfered</param>
        /// <param name="inventoryDocumentTypeDTO">ITIS document type of the source site</param>
        /// <param name="utilities">utilities</param>
        /// <param name="sqlTransaction">SqlTransaction object</param>
        public void ProcessAsAutoHQApproval(InventoryIssueHeaderDTO inventoryIssueHeaderDTO, InventoryDocumentTypeDTO inventoryDocumentTypeDTO, Utilities utilities, SqlTransaction sqlTransaction = null)
        {
            log.Debug("ProcessAsAutoHQApproval started :Id " + inventoryIssueHeaderDTO.InventoryIssueId);
            log.LogMethodEntry(inventoryIssueHeaderDTO, inventoryDocumentTypeDTO, utilities);
            int envContextSiteId = machineContext.GetSiteId();
            string loginId = machineContext.GetUserId();
            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(machineContext);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeListOnDisplay = new List<InventoryDocumentTypeDTO>();
            List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            Publish.Publish publish = new Publish.Publish();
            LocationDTO locationDTO = new LocationDTO();
            LocationBL location = new LocationBL(machineContext, locationDTO);
            LocationList locationList = new LocationList(machineContext);
            List<LocationDTO> locationDTOList = new List<LocationDTO>();
            List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>> searchByLocationParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList();
            ProductList productList = new ProductList();
            List<ProductDTO> productDTOList = new List<ProductDTO>();
            ProductDTO productDTO;
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchByProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            bool isFirstLine = false;
            PurchaseOrderDTO purchaseOrderDTO;
            PurchaseOrder purchaseOrder;
            PurchaseOrderLineDTO purchaseOrderLineDTO;
            List<PurchaseOrderLineDTO> purchaseOrderLineDTOList = new List<PurchaseOrderLineDTO>();
            InventoryDocumentTypeDTO siteRegularPODocumentTypeDTO = new InventoryDocumentTypeDTO();
            double price = 0;
            int vendorId = -1;
            DateTime serverTime = utilities.getServerTime();
            ParafaitDBTransaction parafaitDBTrx = null;
            ProductBL product = new ProductBL();
            Tax purchaseTax;
            if (sqlTransaction == null)
            {
                parafaitDBTrx = new ParafaitDBTransaction();
            }

            try
            {
                if (parafaitDBTrx != null)
                {
                    parafaitDBTrx.BeginTransaction();
                }
                siteRegularPODocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType("RGPO", inventoryIssueHeaderDTO.ToSiteID, null);
                if (siteRegularPODocumentTypeDTO == null || (siteRegularPODocumentTypeDTO != null && siteRegularPODocumentTypeDTO.DocumentTypeId == -1))
                {
                    publish = new Publish.Publish("InventoryDocumentType", utilities);
                    publish.PublishEntity(inventoryDocumentTypeDTO.MasterEntityId, machineContext.GetSiteId(), inventoryIssueHeaderDTO.ToSiteID);
                    siteRegularPODocumentTypeDTO = inventoryDocumentTypeList.GetInventoryDocumentType("RGPO", inventoryIssueHeaderDTO.ToSiteID, null);
                    if (siteRegularPODocumentTypeDTO == null || (siteRegularPODocumentTypeDTO != null && siteRegularPODocumentTypeDTO.DocumentTypeId == -1))
                    {
                        log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) The document type Regular Purchase Order not published to the destination site.");
                        throw new Exception("The document type Regular Purchase Order is not published to the destination site.");
                    }
                }
                log.LogMethodEntry("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) The document type is loaded.");
                List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
                List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>> inventoryIssueLinesSearchParams = new List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>>();
                inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ACTIVE_FLAG, "1"));
                inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.SITE_ID, inventoryIssueHeaderDTO.FromSiteID.ToString()));
                inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_ID, inventoryIssueHeaderDTO.InventoryIssueId.ToString()));
                inventoryIssueLinesDTOList = inventoryIssueLinesList.GetAllInventoryIssueLines(inventoryIssueLinesSearchParams, sqlTransaction);
                if (inventoryIssueLinesDTOList == null || (inventoryIssueLinesDTOList != null && inventoryIssueLinesDTOList.Count == 0))
                {
                    log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) Problem in fetching the issue lines.");
                    throw new Exception("Problem in fetching the issue lines.");
                }
                foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesDTOList)
                {
                    log.LogVariableState("inventoryIssueLinesDTO", inventoryIssueLinesDTO);
                    log.Debug("inventoryIssueLinesDTO UOMId: " + inventoryIssueLinesDTO.UOMId);
                    purchaseOrderLineDTO = new PurchaseOrderLineDTO();
                    location = new LocationBL(utilities.ExecutionContext, inventoryIssueLinesDTO.ToLocationID);
                    if (location.GetLocationDTO.MasterEntityId == -1)
                    {
                        log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) The In transit Location is not published. Please map to HQ in transit location.");
                        throw new Exception("The In transit Location is not published. Please map to HQ in transit location.");
                    }
                    searchByLocationParameters = new List<KeyValuePair<LocationDTO.SearchByLocationParameters, string>>();
                    searchByLocationParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.IS_ACTIVE, "Y"));
                    searchByLocationParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.SITE_ID, inventoryIssueHeaderDTO.ToSiteID.ToString()));
                    if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite && location.GetLocationDTO.SiteId == utilities.ParafaitEnv.SiteId)
                    {
                        searchByLocationParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.MASTER_ENTITY_ID, location.GetLocationDTO.LocationId.ToString()));
                    }
                    else
                    {
                        searchByLocationParameters.Add(new KeyValuePair<LocationDTO.SearchByLocationParameters, string>(LocationDTO.SearchByLocationParameters.MASTER_ENTITY_ID, location.GetLocationDTO.MasterEntityId.ToString()));
                    }
                    locationDTOList = locationList.GetAllLocations(searchByLocationParameters);

                    if (locationDTOList == null || (locationDTOList != null && locationDTOList.Count == 0))
                    {
                        publish = new Publish.Publish("Location", utilities);
                        publish.PublishEntity(location.GetLocationDTO.MasterEntityId, machineContext.GetSiteId(), inventoryIssueHeaderDTO.ToSiteID);
                        locationDTOList = locationList.GetAllLocations(searchByLocationParameters);
                        if (locationDTOList == null || (locationDTOList != null && locationDTOList.Count == 0))
                        {
                            log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) The In transit location is not published to the destination site.");
                            throw new Exception("The In transit location is not published to the destination site.");
                        }
                    }

                    productList = new ProductList();
                    productDTO = productList.GetProduct(inventoryIssueLinesDTO.ProductId);
                    if (productDTO.MasterEntityId == -1)
                    {
                        log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) The product " + productDTO.ProductName + ":" + productDTO.ProductId + " is not published. Please map right product in HQ to this product.");
                        throw new Exception("The product " + productDTO.ProductName + ":" + productDTO.ProductId + " is not published. Please map right product in HQ to this product.");
                    }
                    searchByProductParameters = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
                    searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.IS_ACTIVE, "Y"));
                    searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, inventoryIssueHeaderDTO.ToSiteID.ToString()));
                    log.LogMethodEntry("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) Product fetching to publish Corporate:" + utilities.ParafaitEnv.IsCorporate.ToString() + " IsmasterSite:" + utilities.ParafaitEnv.IsMasterSite.ToString() + " product SiteId:" + productDTO.SiteId + " utilities.ParafaitEnv.SiteId:" + utilities.ParafaitEnv.SiteId + " productDTO.MasterEntityId:" + productDTO.MasterEntityId);
                    if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite && productDTO.SiteId == utilities.ParafaitEnv.SiteId)
                    {
                        searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID, productDTO.ProductId.ToString()));
                    }
                    else
                    {
                        searchByProductParameters.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.MASTER_ENTITY_ID, productDTO.MasterEntityId.ToString()));
                    }
                    productDTOList = productList.GetAllProducts(searchByProductParameters);

                    if (productDTOList == null || (productDTOList != null && productDTOList.Count == 0))
                    {
                        publish = new Publish.Publish("product", utilities);
                        publish.PublishEntity(productDTO.MasterEntityId, machineContext.GetSiteId(), inventoryIssueHeaderDTO.ToSiteID);
                        productDTOList = productList.GetAllProducts(searchByProductParameters);

                        if (productDTOList == null || (productDTOList != null && productDTOList.Count == 0))
                        {
                            log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) The product " + productDTO.ProductName + ":" + productDTO.ProductId + " is not published to the destination site.");
                            throw new Exception("The product " + productDTO.ProductName + ":" + productDTO.ProductId + " is not published to the destination site.");
                        }
                    }
                    if (!isFirstLine || vendorId == -1)
                    {
                        vendorId = productDTOList[0].DefaultVendorId;
                        isFirstLine = true;
                    }
                    purchaseTax = new Tax(machineContext, productDTOList[0].PurchaseTaxId);
                    product = new ProductBL(productDTOList[0].ProductId);
                    price = product.GetProductPrice(1);
                    log.Debug("Inv Issue Line UOMID" + inventoryIssueLinesDTO.UOMId);
                    log.Debug("Product UOM Id UOMID" + productDTOList[0].InventoryUOMId);


                    UOM uOM = new UOM(machineContext, inventoryIssueLinesDTO.UOMId, false, false);
                    if (uOM.getUOMDTO != null && uOM.getUOMDTO.MasterEntityId == -1)
                    {
                        log.Error("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) The product " + uOM.getUOMDTO.UOM + ":" + uOM.getUOMDTO.UOMId + " is not published. Please map right UOM in HQ to this UOM.");
                        throw new Exception("The UOM " + uOM.getUOMDTO.UOM + ":" + uOM.getUOMDTO.UOMId + " is not published. Please map right UOm in HQ to this UOM.");
                    }

                    List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>> searchByUOMParameters = new List<KeyValuePair<UOMDTO.SearchByUOMParameters, string>>();
                    searchByUOMParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.IS_ACTIVE, "Y"));
                    searchByUOMParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.SITEID, inventoryIssueHeaderDTO.ToSiteID.ToString()));
                    if (utilities.ParafaitEnv.IsCorporate && utilities.ParafaitEnv.IsMasterSite && uOM.getUOMDTO.SiteId == utilities.ParafaitEnv.SiteId)
                    {
                        searchByUOMParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.MASTER_ENTITY_ID, uOM.getUOMDTO.UOMId.ToString()));
                    }
                    else
                    {
                        searchByUOMParameters.Add(new KeyValuePair<UOMDTO.SearchByUOMParameters, string>(UOMDTO.SearchByUOMParameters.MASTER_ENTITY_ID, uOM.getUOMDTO.MasterEntityId.ToString()));
                    }
                    UOMList uOMList = new UOMList(machineContext);
                    List<UOMDTO> uomDTOList = uOMList.GetAllUOMDTOList(searchByUOMParameters, false, false, (parafaitDBTrx != null) ? parafaitDBTrx.SQLTrx : sqlTransaction);
                    if (uomDTOList == null || (uomDTOList != null && uomDTOList.Count == 0))
                    {
                        log.Debug(" UOM Ipublishing for To site : ");
                        publish = new Publish.Publish("UOM", utilities);
                        publish.PublishEntity(uOM.getUOMDTO.MasterEntityId, machineContext.GetSiteId(), inventoryIssueHeaderDTO.ToSiteID);
                        uomDTOList = uOMList.GetAllUOMDTOList(searchByUOMParameters, false, false, (parafaitDBTrx != null) ? parafaitDBTrx.SQLTrx : sqlTransaction);

                        if (uomDTOList == null || (uomDTOList != null && uomDTOList.Count == 0))
                        {
                            log.Error("ProcessAsAutoHQApproval:  The UOM " + uOM.getUOMDTO.UOM + ":" + uOM.getUOMDTO.UOMId + " is not published. Please map right UOM in HQ to this UOM.");
                            throw new Exception("The UOM " + uOM.getUOMDTO.UOM + ":" + uOM.getUOMDTO.UOMId + " is not published. Please map right UOm in HQ to this UOM.");
                        }
                    }
                    UOMDTO uOMDTO = uomDTOList.FirstOrDefault();
                    log.Debug("Product UOM Id UOMID for To site : " + uOMDTO.UOMId);

                    purchaseOrderLineDTO = new PurchaseOrderLineDTO(productDTOList[0].Code, productDTOList[0].Description, inventoryIssueLinesDTO.Quantity, (((productDTOList[0].LastPurchasePrice == 0) ? productDTOList[0].Cost : productDTOList[0].LastPurchasePrice) > price) ? price : ((productDTOList[0].LastPurchasePrice == 0) ? productDTOList[0].Cost : productDTOList[0].LastPurchasePrice),
                                                                    product.GetProductPrice(inventoryIssueLinesDTO.Quantity), purchaseTax.GetTaxAmount(((productDTOList[0].LastPurchasePrice == 0) ? productDTOList[0].Cost : productDTOList[0].LastPurchasePrice), productDTOList[0].TaxInclusiveCost.Equals("Y")), 0, serverTime.AddMonths(3), productDTOList[0].ProductId, DateTime.MinValue,
                                                                    inventoryIssueLinesDTO.RequisitionID, inventoryIssueLinesDTO.RequisitionLineID, 0, productDTOList[0].PriceInTickets, "Y", inventoryIssueLinesDTO.Guid, string.Empty, productDTOList[0].PurchaseTaxId, uOMDTO.UOMId);
                    purchaseOrderLineDTOList.Add(purchaseOrderLineDTO);
                    log.LogVariableState("purchaseOrderLineDTOList", purchaseOrderLineDTOList);
                }
                if (vendorId == -1)
                {
                    log.Fatal("ProcessAsAutoHQApproval : Failed to create Purchase order in site:" + inventoryIssueHeaderDTO.ToSiteID + ". The default vendor should be present for the product.");
                    throw new Exception("Failed to create Purchase order in site:" + inventoryIssueHeaderDTO.ToSiteID + " The default vendor should be present for the product.");
                }
                double total = purchaseOrderLineDTOList.Sum(x => (x.SubTotal));
                machineContext.SetSiteId(inventoryIssueHeaderDTO.ToSiteID);
                purchaseOrderDTO = new PurchaseOrderDTO("Open", inventoryIssueHeaderDTO.IssueDate, vendorId, "Requistion:" + inventoryIssueHeaderDTO.RequisitionID.ToString(), DateTime.MinValue, total,
                                                                       "", DateTime.MinValue, "", DateTime.MinValue, "Y", siteRegularPODocumentTypeDTO.DocumentTypeId, serverTime, serverTime.AddMonths(3), "Issued from " + inventoryIssueHeaderDTO.FromSiteID + " to " + inventoryIssueHeaderDTO.ToSiteID + "Ref Issueid:" + inventoryIssueHeaderDTO.InventoryIssueId + " dated:" + inventoryIssueHeaderDTO.IssueDate.ToString("yyyy-MMM-dd HH:mm:ss") + ".",string.Empty, 0,
                                                                       0, "F", inventoryIssueHeaderDTO.FromSiteID, inventoryIssueHeaderDTO.ToSiteID, inventoryIssueHeaderDTO.Guid, purchaseOrderLineDTOList, inventoryIssueHeaderDTO.IsActive, false);

                utilities.ParafaitEnv.IsCorporate = true;
                utilities.ParafaitEnv.SiteId = inventoryIssueHeaderDTO.ToSiteID;
                string roleGuid = utilities.getParafaitDefaults("APPROVER_ROLE_FOR_INTER_STORE_ADJUSTMENT_RECEIVE");
                log.LogVariableState("roleGuid", roleGuid);
                UsersDTO usersDTO = null;
                if (string.IsNullOrEmpty(roleGuid) || (!string.IsNullOrEmpty(roleGuid) && roleGuid.Equals("00000000-0000-0000-0000-000000000000")))
                {
                    Users users = new Users(utilities.ExecutionContext, "WebInventory", inventoryIssueHeaderDTO.ToSiteID);
                    if (users.UserDTO.UserId == -1)
                    {
                        log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) There is no External POS user in site:" + inventoryIssueHeaderDTO.ToSiteID);
                        throw new Exception("There is no External POS user in site:" + inventoryIssueHeaderDTO.ToSiteID);
                    }
                    else
                    {
                        usersDTO = users.UserDTO;
                        UserRoles userRoles = new UserRoles(machineContext, usersDTO.RoleId);
                        if (userRoles.getUserRolesDTO == null || (userRoles.getUserRolesDTO != null && userRoles.getUserRolesDTO.RoleId == -1))
                        {
                            log.Fatal("ProcessDropShipRequest(sqlTransaction) Please select the APPROVER_ROLE_FOR_INTER_STORE_ADJUSTMENT_RECEIVE configuration in site:" + purchaseOrderDTO.ToSiteId);
                            throw new Exception("Please select the APPROVER_ROLE_FOR_INTER_STORE_ADJUSTMENT_RECEIVE configuration in site:" + purchaseOrderDTO.ToSiteId);
                        }
                    }
                }
                else
                {
                    UserRolesList userRolesList = new UserRolesList();
                    UserRolesDTO userRolesDTO = userRolesList.GetUserRole(roleGuid);
                    if (userRolesDTO == null || (userRolesDTO != null && userRolesDTO.RoleId == -1))
                    {
                        log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) Please select the APPROVER_ROLE_FOR_INTER_STORE_ADJUSTMENT_RECEIVE configuration in site:" + inventoryIssueHeaderDTO.ToSiteID);
                        throw new Exception("Please select the APPROVER_ROLE_FOR_INTER_STORE_ADJUSTMENT_RECEIVE configuration in site:" + inventoryIssueHeaderDTO.ToSiteID);
                    }
                    UsersList usersList = new UsersList(machineContext);
                    List<KeyValuePair<UsersDTO.SearchByUserParameters, string>> searchParameter = new List<KeyValuePair<UsersDTO.SearchByUserParameters, string>>();
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.ROLE_ID, userRolesDTO.RoleId.ToString()));
                    searchParameter.Add(new KeyValuePair<UsersDTO.SearchByUserParameters, string>(UsersDTO.SearchByUserParameters.SITE_ID, inventoryIssueHeaderDTO.ToSiteID.ToString()));
                    List<UsersDTO> userDTOList = usersList.GetAllUsers(searchParameter, false, true, (parafaitDBTrx != null) ? sqlTransaction : sqlTransaction);
                    if (userDTOList == null || (userDTOList != null && userDTOList.Count == 0))
                    {
                        log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) For the role " + userRolesDTO.Role + " user does not exists in site:" + inventoryIssueHeaderDTO.ToSiteID + ". If the inter store transfer happens with this users are unable to see the records");
                        throw new Exception("The user role " + userRolesDTO.Role + " user does not exists in site:" + inventoryIssueHeaderDTO.ToSiteID + ". If the inter store transfer happens with this users are unable to see the records");
                    }
                    usersDTO = userDTOList.Where(x => x.RoleId == userRolesDTO.RoleId && x.UserStatus == "ACTIVE").FirstOrDefault();
                    if(usersDTO == null)
                    {
                        usersDTO = userDTOList.FirstOrDefault();
                    }
                }
                if (usersDTO != null)
                {
                    machineContext.SetUserId(usersDTO.LoginId);
                }
                purchaseOrder = new PurchaseOrder(purchaseOrderDTO, machineContext);
                if (!purchaseOrder.IsPurchaseOrderAlreadyTransfferedToSite((parafaitDBTrx != null) ? sqlTransaction : sqlTransaction))
                {
                    purchaseOrder.Save((parafaitDBTrx != null) ? sqlTransaction : sqlTransaction);

                    if (purchaseOrderDTO.PurchaseOrderId == -1)
                    {
                        log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) Failed to create Purchase order in site:" + inventoryIssueHeaderDTO.ToSiteID);
                        throw new Exception("Failed to create Purchase order in site:" + inventoryIssueHeaderDTO.ToSiteID);
                    }
                    else
                    {
                        log.LogMethodEntry("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) Created purchase order in site:" + inventoryIssueHeaderDTO.ToSiteID);
                    }
                    PurchaseOrder purchaseOrderSaved = new PurchaseOrder(purchaseOrderDTO.PurchaseOrderId, machineContext, (parafaitDBTrx != null) ? sqlTransaction : sqlTransaction);
                    if (purchaseOrderSaved.getPurchaseOrderDTO == null)
                    {
                        log.Fatal("ProcessAsAutoHQApproval(inventoryIssueHeaderDTO,inventoryDocumentTypeDTO,utilities) Failed to load PO of id:" + purchaseOrderDTO.PurchaseOrderId + " created in sqltransaction, with site id:" + machineContext.GetSiteId() + " and for the login Id:" + machineContext.GetUserId());
                        throw new Exception("Failed to load Purchase order in site:" + machineContext.GetSiteId() + " and for the user:" + machineContext.GetUserId() + ". Please check the data access permission at site " + machineContext.GetSiteId() + " for the same user role.");
                    }
                    UserMessagesList userMessagesList = new UserMessagesList();
                    List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.SITE_ID, inventoryIssueHeaderDTO.ToSiteID.ToString()));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, purchaseOrderSaved.getPurchaseOrderDTO.Guid));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, "RGPO"));
                    userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                    List<UserMessagesDTO> userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams, (parafaitDBTrx != null) ? sqlTransaction : sqlTransaction);
                    if (userMessagesDTOList == null)
                    {
                        UserMessagesDTO userMessagesDTO = new UserMessagesDTO();
                        userMessagesDTO.Level = 0;
                        userMessagesDTO.Message = "Please receive the inter store purchase order.";
                        userMessagesDTO.MessageType = "Information";
                        userMessagesDTO.ModuleType = "Inventory";
                        userMessagesDTO.ObjectGUID = purchaseOrderSaved.getPurchaseOrderDTO.Guid;
                        userMessagesDTO.ObjectType = "RGPO";
                        userMessagesDTO.RoleId = usersDTO.RoleId;
                        userMessagesDTO.UserId = usersDTO.UserId;
                        machineContext.SetSiteId(inventoryIssueHeaderDTO.ToSiteID);
                        UserMessages userMessages = new UserMessages(userMessagesDTO, machineContext);
                        userMessages.Save((parafaitDBTrx != null) ? sqlTransaction : sqlTransaction);
                    }
                }
                else
                {
                    log.Fatal("ProcessDropShipRequest(sqlTransaction) Purchase order " + purchaseOrderDTO.OriginalReferenceGUID + " is already exists in " + purchaseOrderDTO.ToSiteId + " site.");
                }
                log.LogMethodExit();
                if (parafaitDBTrx != null)
                {
                    parafaitDBTrx.EndTransaction();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                if (parafaitDBTrx != null)
                {
                    parafaitDBTrx.RollBack();
                }
                log.LogMethodExit(ex);
                throw;
            }
            finally
            {
                utilities.ParafaitEnv.SiteId = envContextSiteId;
                machineContext.SetSiteId(envContextSiteId);
                machineContext.SetUserId(loginId);
            }

            log.LogMethodExit();
        }

        public void UpdateIssueForInbox(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<InventoryDocumentTypeDTO> inventoryDocumentTypeListOnDisplay = new List<InventoryDocumentTypeDTO>();

            InventoryIssueLinesDTO inventoryIssueLineSavedDTO;
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList();

            if (inventoryIssueHeaderDTO.Status == null)
            {
                log.Error("Please select status.");
                throw new Exception(MessageContainerList.GetMessage(machineContext, 980));
            }
            else if (inventoryIssueHeaderDTO.Status.Equals("OPEN"))
            {
                if (this.inventoryIssueHeaderDTO.InventoryIssueLinesListDTO.Count <= 0 && this.inventoryIssueHeaderDTO.InventoryIssueLinesListDTO != null)
                {
                    log.Error("No rows selected. Please select the rows before clicking delete.");
                    throw new Exception(MessageContainerList.GetMessage(machineContext, 959)); //No rows selected. Please select the rows before clicking delete.
                }
                foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueHeaderDTO.InventoryIssueLinesListDTO)
                {
                    if (inventoryIssueLinesDTO.IssueId > 0)
                    {
                        inventoryIssueLineSavedDTO = inventoryIssueLinesList.GetIssueLines(inventoryIssueLinesDTO.IssueLineId, sqlTransaction);
                        inventoryIssueLineSavedDTO.IsActive = false;

                        InventoryIssueLines inventoryIssueLines = new InventoryIssueLines(inventoryIssueLineSavedDTO, machineContext);
                        inventoryIssueLines.Save(null);
                    }
                }
            }
            else if (inventoryIssueHeaderDTO.Status.Equals("SUBMITTED"))
            {
                try
                {
                    if (inventoryIssueHeaderDTO.InventoryIssueId > 0)
                    {
                        string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((inventoryIssueHeaderDTO.DocumentTypeId == -1) ? -1 : (int)inventoryIssueHeaderDTO.DocumentTypeId))).ToList<InventoryDocumentTypeDTO>()[0].Code;

                        InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTO.InventoryIssueId, machineContext, sqlTransaction);

                        ApprovalRuleDTO approvalRuleDTO = new ApprovalRuleDTO();
                        ApprovalRule approvalRule = new ApprovalRule(machineContext, approvalRuleDTO);

                        UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(machineContext.GetUserId(), "", machineContext.GetSiteId());
                        int roleId = user.RoleId;
                        approvalRuleDTO = approvalRule.GetApprovalRule(roleId, inventoryIssueHeader.getInventoryIssueHeaderDTO.DocumentTypeId, machineContext.GetSiteId());

                        if (approvalRuleDTO != null)
                        {
                            UserMessagesList userMessagesList = new UserMessagesList();
                            List<UserMessagesDTO> userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage("Inventory", code, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid, -1, -1, machineContext.GetSiteId(), null);
                            if (userMessagesDTOList != null && userMessagesDTOList.Count == approvalRuleDTO.NumberOfApprovalLevels && approvalRuleDTO.NumberOfApprovalLevels != 0)
                            {
                                using (ParafaitDBTransaction parafaitDBTransaction = new ParafaitDBTransaction())
                                {
                                    try
                                    {
                                        parafaitDBTransaction.BeginTransaction();
                                        foreach (UserMessagesDTO userMessagesDTO in userMessagesDTOList)
                                        {
                                            userMessagesDTO.IsActive = false;
                                            UserMessages userMessages = new UserMessages(userMessagesDTO, machineContext);
                                            userMessages.Save(parafaitDBTransaction.SQLTrx);
                                        }
                                        inventoryIssueHeader.getInventoryIssueHeaderDTO.Status = "CANCELLED";
                                        InventoryIssueHeader inventoryIssueHeadertosave = new InventoryIssueHeader(inventoryIssueHeader.getInventoryIssueHeaderDTO, machineContext);
                                        inventoryIssueHeadertosave.Save(parafaitDBTransaction.SQLTrx);
                                        parafaitDBTransaction.EndTransaction();
                                    }
                                    catch (Exception ex1)
                                    {
                                        parafaitDBTransaction.RollBack();
                                        log.Error("Failed to delete the issue. Exception:" + ex1.ToString());
                                        throw ex1;
                                    }
                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = MessageContainerList.GetMessage(machineContext, 1540);
                    throw new Exception((errorMessage));
                }
            }
            else
            {
                string errorMessage = MessageContainerList.GetMessage(machineContext, 1541);
                throw new Exception((errorMessage));
            }
            log.LogMethodExit();
        }
        public void SaveIssueForInbox(SqlTransaction sqlTransaction = null)
        {
            ApprovalRuleDTO approvalRuleDTO = new ApprovalRuleDTO();
            ApprovalRule approvalRule = new ApprovalRule(machineContext, approvalRuleDTO);
            List<UserMessagesDTO> userMessagesDTOList = new List<UserMessagesDTO>();
            UserMessages userMessages;
            UserMessagesList userMessagesList = new UserMessagesList(machineContext);
            InventoryIssueLinesDTO inventoryIssueLineSavedDTO;
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList();
            int locationId = -1;

            List<InventoryDocumentTypeDTO> inventoryDocumentTypeListOnDisplay = new List<InventoryDocumentTypeDTO>();

            InventoryDocumentTypeList inventoryDocumentTypeList = new InventoryDocumentTypeList(machineContext);

            List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>> inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams = new List<KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>>();
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.ACTIVE_FLAG, "1"));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.SITE_ID, machineContext.GetSiteId().ToString()));
            inventoryDocumentTypeSearchParams.Add(new KeyValuePair<InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters, string>(InventoryDocumentTypeDTO.SearchByInventoryDocumentTypeParameters.APPLICABILITY, "Issue"));
            inventoryDocumentTypeListOnDisplay = inventoryDocumentTypeList.GetAllInventoryDocumentTypes(inventoryDocumentTypeSearchParams);

            string code = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == ((inventoryIssueHeaderDTO.DocumentTypeId == -1) ? -1 : (int)inventoryIssueHeaderDTO.DocumentTypeId))).ToList<InventoryDocumentTypeDTO>()[0].Code;
            if (!string.IsNullOrWhiteSpace(code))
            {
                inventoryIssueHeaderDTOonsave = new InventoryIssueHeaderDTO();
                List<InventoryIssueLinesDTO> inventoryIssueLinesListOnDisplay = new List<InventoryIssueLinesDTO>();
                if (ValidateForm(code))
                {
                    if (code.Equals("ITIS"))
                    {
                        //LocationBL locationBL = new LocationBL(machineContext, inventoryIssueHeaderDTO.InventoryIssueLinesListDTO[0].FromLocationID);
                        LocationList locationList = new LocationList(machineContext);
                        List<LocationDTO> toLocationList = locationList.GetLocationsOnLocationType("'In Transit'", machineContext.GetSiteId(), true, sqlTransaction);

                        if (toLocationList != null && toLocationList.Count > 0)
                        {
                            locationId = toLocationList.FirstOrDefault().LocationId;
                        }
                        else
                        {
                            log.Debug("Please publish the Location of In Transit type from HQ.");
                            string errorMessage = MessageContainerList.GetMessage(machineContext, 1537);
                            throw new ValidationException(errorMessage);
                        }
                    }
                    if (inventoryIssueHeaderDTO.InventoryIssueId > 0)
                    {
                        inventoryIssueHeaderDTOonsave.InventoryIssueId = Convert.ToInt32(inventoryIssueHeaderDTO.InventoryIssueId);
                    }
                    inventoryIssueHeaderDTOonsave.DocumentTypeId = (int)inventoryIssueHeaderDTO.DocumentTypeId;
                    inventoryIssueHeaderDTOonsave.IssueDate = inventoryIssueHeaderDTO.IssueDate;
                    inventoryIssueHeaderDTOonsave.Status = inventoryIssueHeaderDTO.Status;
                    inventoryIssueHeaderDTOonsave.ReferenceNumber = inventoryIssueHeaderDTO.ReferenceNumber;
                    switch (code)
                    {
                        case "AJIS":
                            break;
                        case "DIIS":
                            if (inventoryIssueHeaderDTO.PurchaseOrderId > 0)
                            {
                                inventoryIssueHeaderDTOonsave.PurchaseOrderId = inventoryIssueHeaderDTO.PurchaseOrderId;
                            }
                            inventoryIssueHeaderDTOonsave.SupplierInvoiceDate = inventoryIssueHeaderDTO.SupplierInvoiceDate;
                            inventoryIssueHeaderDTOonsave.DeliveryNoteDate = inventoryIssueHeaderDTO.DeliveryNoteDate;
                            inventoryIssueHeaderDTOonsave.DeliveryNoteNumber = inventoryIssueHeaderDTO.DeliveryNoteNumber;
                            inventoryIssueHeaderDTOonsave.SupplierInvoiceNumber = inventoryIssueHeaderDTO.SupplierInvoiceNumber;
                            inventoryIssueHeaderDTOonsave.IsActive = true;
                            break;
                        case "REIS":
                            if (inventoryIssueHeaderDTOonsave.DocumentTypeId > 0)
                            {
                                try
                                { inventoryIssueHeaderDTOonsave.RequisitionID = int.Parse(inventoryIssueHeaderDTO.RequisitionID.ToString()); }
                                catch (Exception ex)
                                {
                                    log.LogMethodExit(ex);
                                }
                            }

                            break;
                        case "STIS":
                            break;
                        case "ITIS":
                            if (inventoryIssueHeaderDTOonsave.DocumentTypeId > 0)
                            {
                                try
                                { inventoryIssueHeaderDTOonsave.RequisitionID = int.Parse(inventoryIssueHeaderDTO.RequisitionID.ToString()); }
                                catch (Exception ex)
                                {
                                    log.LogMethodExit(ex);
                                }
                            }
                            if (inventoryIssueHeaderDTOonsave.ToSiteID > 0)
                            {
                                // The below validation can be handled in UI.

                                //if (inventoryIssueHeaderDTO.ToSiteID.Equals("T"))   // need to check - Faizan
                                //{
                                //    if (inventoryIssueHeaderDTOonsave.FromSiteID == -1)
                                //    {
                                //        inventoryIssueHeaderDTOonsave.FromSiteID = machineContext.GetSiteId();
                                //    }
                                //    inventoryIssueHeaderDTOonsave.ToSiteID = Convert.ToInt32(inventoryIssueHeaderDTO.ToSiteID);
                                //}
                                //else if (inventoryIssueHeaderDTO.ToSiteID.Equals("F")) // need to check - Faizan
                                //{
                                //    inventoryIssueHeaderDTOonsave.FromSiteID = Convert.ToInt32(inventoryIssueHeaderDTO.FromSiteID);
                                //    inventoryIssueHeaderDTOonsave.ToSiteID = machineContext.GetSiteId();
                                //}
                            }
                            else
                            {
                                if (inventoryIssueHeaderDTOonsave.FromSiteID == -1)
                                {
                                    inventoryIssueHeaderDTOonsave.FromSiteID = machineContext.GetSiteId();
                                }
                                inventoryIssueHeaderDTOonsave.ToSiteID = Convert.ToInt32(inventoryIssueHeaderDTO.ToSiteID);
                                if (inventoryIssueHeaderDTOonsave.ToSiteID == inventoryIssueHeaderDTOonsave.FromSiteID)
                                {
                                    string errorMessage = MessageContainerList.GetMessage(machineContext, 5440);//From and To sites can not be same.
                                    throw new ValidationException(errorMessage);
                                }
                            }
                            break;
                    }
                    try
                    {
                        InventoryIssueHeader inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTOonsave, machineContext);
                        inventoryIssueHeader.Save(sqlTransaction);
                        inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTOonsave.InventoryIssueId, machineContext, sqlTransaction);
                        inventoryIssueHeaderDTOonsave = inventoryIssueHeader.getInventoryIssueHeaderDTO;

                        if (inventoryIssueHeaderDTOonsave.InventoryIssueId > -1)
                        {
                            inventoryIssueLinesListOnDisplay = inventoryIssueHeaderDTO.InventoryIssueLinesListDTO;

                            InventoryIssueLinesDTO inventoryIssueLinesDTO = new InventoryIssueLinesDTO();
                            for (int i = 0; i < inventoryIssueLinesListOnDisplay.Count; i++)
                            {
                                inventoryIssueLinesDTO = inventoryIssueLinesListOnDisplay[i];
                                if (inventoryIssueLinesDTO.Quantity == 0 && inventoryIssueLinesDTO.IssueLineId == -1)
                                {
                                    continue;
                                }
                                inventoryIssueLinesDTO.IssueId = inventoryIssueHeaderDTOonsave.InventoryIssueId;
                                if (code.Equals("ITIS"))
                                {
                                    if (inventoryIssueHeaderDTOonsave.FromSiteID == machineContext.GetSiteId())
                                    {
                                        inventoryIssueLinesDTO.ToLocationID = locationId;
                                    }
                                }
                                product = new Product.ProductBL(inventoryIssueLinesDTO.ProductId);
                                inventoryIssueLinesDTO.Price = product.GetProductPrice(inventoryIssueLinesDTO.Quantity);
                                inventoryIssueLinesDTO.IssueNumber = inventoryIssueHeaderDTOonsave.IssueNumber;
                                InventoryIssueLines inventoryIssueLines = new InventoryIssueLines(inventoryIssueLinesDTO, machineContext);
                                inventoryIssueLines.Save(sqlTransaction);
                                if (inventoryIssueHeaderDTOonsave.Status.Equals("SUBMITTED"))
                                {
                                    inventoryIssueHeader = new InventoryIssueHeader(inventoryIssueHeaderDTOonsave.InventoryIssueId, machineContext, sqlTransaction);

                                    UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(machineContext.GetUserId(), "", machineContext.GetSiteId());
                                    int roleId = user.RoleId;
                                    approvalRuleDTO = approvalRule.GetApprovalRule(roleId, inventoryIssueHeader.getInventoryIssueHeaderDTO.DocumentTypeId, machineContext.GetSiteId());
                                    if (approvalRuleDTO != null)
                                    {
                                        if (approvalRuleDTO.NumberOfApprovalLevels > 0)
                                        {
                                            List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>> userMessagesSearchParams = new List<KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>>();
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.ACTIVE_FLAG, "1"));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_GUID, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.OBJECT_TYPE, code));
                                            userMessagesSearchParams.Add(new KeyValuePair<UserMessagesDTO.SearchByUserMessagesParameters, string>(UserMessagesDTO.SearchByUserMessagesParameters.MODULE_TYPE, "Inventory"));
                                            userMessagesDTOList = userMessagesList.GetAllUserMessages(userMessagesSearchParams, sqlTransaction);
                                            if (userMessagesDTOList == null)
                                            {
                                                userMessages = new UserMessages(machineContext);
                                                userMessages.CreateUserMessages(approvalRuleDTO, "Inventory", code, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid, user.UserId, "Approval", "Pending for approval", sqlTransaction);
                                                if (inventoryIssueLinesDTO.IssueLineId >= 0)
                                                {
                                                    inventoryIssueLineSavedDTO = inventoryIssueLinesList.GetIssueLines(inventoryIssueLinesDTO.IssueLineId, sqlTransaction);
                                                    if (inventoryIssueLineSavedDTO.IsActive)
                                                    {
                                                        inventoryIssueHeader.SaveIssueDetails(code, inventoryIssueHeaderDTO.DocumentTypeId, inventoryIssueLineSavedDTO, sqlTransaction, code.Equals("DIIS") ? inventoryIssueHeaderDTO.PurchaseOrderId : (code.Equals("ITIS") && inventoryIssueHeaderDTOonsave.DocumentTypeId > 0) ? (int)inventoryIssueHeaderDTOonsave.DocumentTypeId : -1, (code.Equals("DIIS") ? inventoryIssueHeaderDTO.SupplierInvoiceNumber : ""));

                                                    }
                                                }
                                                else
                                                {
                                                    throw new Exception(MessageContainerList.GetMessage(machineContext, 1074));
                                                }
                                            }
                                        }
                                    }
                                    userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage("Inventory", code, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid, -1, -1, machineContext.GetSiteId(), sqlTransaction);
                                    if (inventoryIssueHeaderDTOonsave.ToSiteID == machineContext.GetSiteId() && (userMessagesDTOList != null && userMessagesDTOList.Count == 1 && userMessagesDTOList[0].ApprovalRuleID == -1))
                                    {
                                        userMessagesDTOList[0].Status = UserMessagesDTO.UserMessagesStatus.APPROVED.ToString();
                                        userMessagesDTOList[0].ActedByUser = Convert.ToInt32(machineContext.GetUserId());
                                        userMessages = new UserMessages(userMessagesDTOList[0], machineContext);
                                        userMessages.Save(sqlTransaction);
                                        userMessagesDTOList = userMessagesList.GetPendingApprovalUserMessage("Inventory", code, inventoryIssueHeader.getInventoryIssueHeaderDTO.Guid, -1, -1, machineContext.GetSiteId(), sqlTransaction);
                                    }
                                    if (userMessagesDTOList == null || (userMessagesDTOList != null && userMessagesDTOList.Count == 0))
                                    {
                                        if (inventoryIssueLinesDTO.IssueLineId >= 0)
                                        {
                                            inventoryIssueLineSavedDTO = inventoryIssueLinesList.GetIssueLines(inventoryIssueLinesDTO.IssueLineId, sqlTransaction);
                                            if (inventoryIssueLineSavedDTO.IsActive)
                                            {
                                                inventoryIssueHeader.SaveIssueDetails(code, inventoryIssueHeaderDTO.DocumentTypeId, inventoryIssueLineSavedDTO, sqlTransaction, code.Equals("DIIS") ? inventoryIssueHeaderDTO.PurchaseOrderId : (code.Equals("ITIS") && inventoryIssueHeaderDTOonsave.DocumentTypeId > 0) ? (int)inventoryIssueHeaderDTOonsave.DocumentTypeId : -1, (code.Equals("DIIS") ? inventoryIssueHeaderDTO.SupplierInvoiceNumber : ""));

                                            }
                                        }
                                        else
                                        {
                                            throw new Exception(MessageContainerList.GetMessage(machineContext, 1074));
                                        }
                                    }
                                }
                            }
                            if (inventoryIssueHeaderDTOonsave != null && inventoryIssueHeaderDTOonsave.Status.Equals("SUBMITTED") && inventoryIssueLinesListOnDisplay.Count > 0)
                            {
                                if (userMessagesDTOList == null || (userMessagesDTOList != null && userMessagesDTOList.Count == 0))
                                {
                                    InventoryDocumentTypeDTO inventoryDocumentTypeDTO = inventoryDocumentTypeListOnDisplay.Where(x => (bool)(x.DocumentTypeId == (inventoryIssueHeaderDTO.DocumentTypeId < 0 ? -1 : (int)inventoryIssueHeaderDTO.DocumentTypeId))).ToList<InventoryDocumentTypeDTO>()[0];
                                    if (code.Equals("ITIS") && inventoryIssueHeaderDTOonsave.FromSiteID == machineContext.GetSiteId())
                                    {
                                        inventoryIssueHeader.ProcessAsAutoHQApproval(inventoryIssueHeaderDTOonsave, inventoryDocumentTypeDTO, GetUtility(), sqlTransaction);
                                    }
                                    if (inventoryIssueHeader.getInventoryIssueHeaderDTO != null)
                                    {
                                        inventoryIssueHeader.getInventoryIssueHeaderDTO.Status = "COMPLETE";
                                        inventoryIssueHeader.Save(sqlTransaction);
                                    }
                                }

                                //if (!code.Equals("ITIS"))
                                //{
                                //    if (inventoryIssueHeader.getInventoryIssueHeaderDTO != null)
                                //    {
                                //        inventoryIssueHeader.getInventoryIssueHeaderDTO.Status = "COMPLETE";
                                //        inventoryIssueHeader.Save(sqlTransaction);
                                //    }
                                //}
                            }
                            //if (code.Equals("ITIS") && inventoryIssueLinesListOnDisplay != null && inventoryIssueLinesListOnDisplay.Count > 0 && inventoryIssueHeader.getInventoryIssueHeaderDTO.Status.Equals("SUBMITTED"))
                            if (code.Equals("ITIS") && inventoryIssueLinesListOnDisplay != null && inventoryIssueLinesListOnDisplay.Count > 0 && inventoryIssueHeaderDTOonsave.Status.Equals("SUBMITTED"))
                            {
                                double price = inventoryIssueLinesListOnDisplay.Sum(x => x.Price * x.Quantity);
                                CreateTransaction(price, sqlTransaction);
                            }
                            PopulateLine(inventoryIssueHeader.getInventoryIssueHeaderDTO.InventoryIssueId);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        throw ex;
                    }
                }
            }
        }

        private void CreateTransaction(double trxAmount, SqlTransaction sqlTrxn)
        {
            log.LogMethodEntry(trxAmount, sqlTrxn);
            string message = string.Empty;

            if (ParafaitDefaultContainerList.GetParafaitDefault(machineContext, "CREATE_SALE_TRANSACTION").Equals("Y"))
            {
                Transaction.Transaction transaction = new Transaction.Transaction(GetUtility());
                CustomerListBL customerListBL = new CustomerListBL(machineContext);
                List<KeyValuePair<CustomerSearchByParameters, string>> searchParameters = new List<KeyValuePair<CustomerSearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<CustomerSearchByParameters, string>(CustomerSearchByParameters.PROFILE_PASSWORD, machineContext.GetSiteId().ToString()));
                List<CustomerDTO> customerDTOList = customerListBL.GetCustomerDTOList(searchParameters);
                if (customerDTOList != null && customerDTOList.Count > 0)
                {
                    transaction.customerDTO = customerDTOList[0];
                }
                Products products = new Products();
                ProductsDTO productsDTO = null;
                List<ProductsDTO> productsDTOList = products.GetProductByTypeList("INVENTORYINTERSTORE", machineContext.GetSiteId());
                if (productsDTOList != null && productsDTOList.Count != 0)
                {
                    productsDTO = productsDTOList[0];
                }
                if (productsDTO != null)
                {
                    transaction.createTransactionLine(null, productsDTO.ProductId, trxAmount, 1, ref message);
                    if (transaction.SaveOrder(ref message, sqlTrxn) != 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, "Transaction is not created. " + message, "Transaction creation failed.");
                        throw new Exception((errorMessage));
                    }
                }
            }
            log.LogMethodExit();
        }


        void PopulateLine(int inventoryIssueId)
        {
            log.LogMethodEntry(inventoryIssueId);
            ProductDTO productDTO;
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList();
            inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
            List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>> inventoryIssueLinesSearchParams = new List<KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>>();
            inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ACTIVE_FLAG, "1"));
            //inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.SITE_ID, machineUserContext.GetSiteId().ToString()));
            inventoryIssueLinesSearchParams.Add(new KeyValuePair<InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters, string>(InventoryIssueLinesDTO.SearchByInventoryIssueLinesParameters.ISSUE_ID, inventoryIssueId.ToString()));
            inventoryIssueLinesDTOList = inventoryIssueLinesList.GetAllInventoryIssueLines(inventoryIssueLinesSearchParams);
            List<InventoryIssueLinesDTO> sortInventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
            if ((inventoryIssueLinesDTOList != null) && (inventoryIssueLinesDTOList.Any()))
            {
                foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesDTOList)
                {
                    productDTO = GetProductDTO(inventoryIssueLinesDTO.ProductId);
                    inventoryIssueLinesDTO.ProductName = productDTO.Description;
                    if (inventoryIssueHeaderDTOonsave.ToSiteID > 0 && inventoryIssueHeaderDTOonsave.FromSiteID > 0 && inventoryIssueHeaderDTOonsave.FromSiteID == machineContext.GetSiteId())
                    {
                        inventoryIssueLinesDTO.AvailableQuantity = inventoryIssueLinesDTO.Quantity;
                    }
                    else
                    {
                        inventoryIssueLinesDTO.AvailableQuantity = GetProductQuantity(inventoryIssueLinesDTO.FromLocationID, inventoryIssueLinesDTO.ProductId);
                    }
                }
            }
            else
            {
                inventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>();
            }
            sortInventoryIssueLinesDTOList = new List<InventoryIssueLinesDTO>(inventoryIssueLinesDTOList);

            if (sortInventoryIssueLinesDTOList != null)
            {
                for (int i = 0; i < sortInventoryIssueLinesDTOList.Count; i++)
                {
                    int uomId = sortInventoryIssueLinesDTOList[i].UOMId;
                    if (uomId == -1)
                    {
                        if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                        {
                            if (ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).InventoryUOMId != -1)
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).InventoryUOMId;
                            }

                            else
                            {
                                uomId = ProductContainer.productDTOList.Find(x => x.ProductId == sortInventoryIssueLinesDTOList[i].ProductId).UomId;
                            }
                        }
                    }
                    UOMContainer uomcontainer = GetUOMContainer(machineContext);
                    List<UOMDTO> uomDTOList = uomcontainer.relatedUOMDTOList.FirstOrDefault(x => x.Key == uomId).Value;
                }
            }
            log.LogMethodExit();
        }



        private double GetProductQuantity(int LocationId, int productId)
        {
            log.LogMethodEntry(LocationId, productId);
            double quantity = 0.0;
            List<InventoryDTO> inventoryDTOList = new List<InventoryDTO>();
            InventoryList inventoryList = new InventoryList();
            List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> inventoryPurchaseOrderLineSearchParams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
            inventoryPurchaseOrderLineSearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.PRODUCT_ID, productId.ToString()));
            inventoryPurchaseOrderLineSearchParams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOCATION_ID, LocationId.ToString()));
            inventoryDTOList = inventoryList.GetAllInventory(inventoryPurchaseOrderLineSearchParams);
            if (inventoryDTOList != null && inventoryDTOList.Count > 0)
            {
                foreach (InventoryDTO inventoryDTO in inventoryDTOList)
                    quantity += inventoryDTO.Quantity;
            }
            log.LogMethodExit(quantity);
            return quantity;
        }

        private ProductDTO GetProductDTO(int productId)
        {
            log.LogMethodEntry(productId);

            List<ProductDTO> productDTOList = new List<ProductDTO>();
            Product.ProductList productList = new Product.ProductList();
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> productPurchaseOrderLineSearchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            productPurchaseOrderLineSearchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.PRODUCT_ID, productId.ToString()));

            productDTOList = productList.GetAllProducts(productPurchaseOrderLineSearchParams);
            if (productDTOList != null && productDTOList.Count > 0)
            {
                return productDTOList[0];
            }
            log.LogMethodExit();
            return null;
        }

        private Utilities GetUtility()
        {
            Utilities Utilities = new Utilities();
            Utilities.ParafaitEnv.IsCorporate = machineContext.GetIsCorporate();
            Utilities.ParafaitEnv.User_Id = machineContext.GetUserPKId();
            Utilities.ParafaitEnv.LoginID = machineContext.GetUserId();
            Utilities.ParafaitEnv.SetPOSMachine("", Environment.MachineName);
            Utilities.ParafaitEnv.IsCorporate = machineContext.GetIsCorporate();
            Utilities.ParafaitEnv.SiteId = machineContext.GetSiteId();
            Utilities.ExecutionContext.SetIsCorporate(machineContext.GetIsCorporate());
            Utilities.ExecutionContext.SetSiteId(machineContext.GetSiteId());
            Utilities.ExecutionContext.SetUserId(machineContext.GetUserId());


            Utilities.ParafaitEnv.Initialize();
            return Utilities;
        }
        /// <summary>
        /// Method to get the UOM Container
        /// </summary>
        /// <returns></returns>
        public static UOMContainer GetUOMContainer(ExecutionContext machineContext)
        {
            log.LogMethodEntry();
            if (uomContainerDictionary.ContainsKey(machineContext.GetSiteId()) == false)
            {
                uomContainerDictionary[machineContext.GetSiteId()] = new UOMContainer(machineContext);
            }
            UOMContainer result = uomContainerDictionary[machineContext.GetSiteId()];
            log.LogMethodExit(result);
            return result;
        }


        private bool ValidateForm(string code)
        {
            log.LogMethodEntry(code);
            List<InventoryIssueLinesDTO> inventoryIssueLinesListOnDisplay = new List<InventoryIssueLinesDTO>();

            if (inventoryIssueHeaderDTO.InventoryIssueLinesListDTO != null)
            {
                inventoryIssueLinesListOnDisplay = inventoryIssueHeaderDTO.InventoryIssueLinesListDTO;
            }
            if (inventoryIssueHeaderDTO.DocumentTypeId < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(machineContext, 1075);
                throw new ValidationException(errorMessage);
            }
            if (string.IsNullOrEmpty(inventoryIssueHeaderDTO.IssueDate.ToString()))
            {
                string errorMessage = MessageContainerList.GetMessage(machineContext, 1076);
                throw new ValidationException(errorMessage);
            }
            if (inventoryIssueHeaderDTO.Status == null)
            {
                string errorMessage = MessageContainerList.GetMessage(machineContext, 980);
                throw new ValidationException(errorMessage);
            }
            else if (inventoryIssueHeaderDTO.Status.Equals("COMPLETE") || inventoryIssueHeaderDTO.Status.Equals("CANCELLED") || inventoryIssueHeaderDTO.Status.Equals("APPROVED"))
            {
                string errorMessage = MessageContainerList.GetMessage(machineContext, "Invalid status");
                throw new ValidationException(errorMessage);
            }
            for (int i = 0; i < inventoryIssueLinesListOnDisplay.Count; i++)
            {
                ProductsContainerDTOCollection productsContainerDTOCollection = ProductsContainerList.GetProductsContainerDTOCollection(machineContext.GetSiteId(), "INVENTORY");
                if (productsContainerDTOCollection.ProductContainerDTOList != null && productsContainerDTOCollection.ProductContainerDTOList.Any())
                {
                    int userEnteredUOMId = inventoryIssueLinesListOnDisplay[i].UOMId;
                    ProductsContainerDTO productsContainerDTO = productsContainerDTOCollection.ProductContainerDTOList.Find(x => x.InventoryItemContainerDTO.ProductId == inventoryIssueLinesListOnDisplay[i].ProductId);
                    int inventoryUOMId = productsContainerDTO.InventoryItemContainerDTO.InventoryUomId;
                    if (userEnteredUOMId != inventoryUOMId)
                    {
                        double factor = UOMContainer.GetConversionFactor(userEnteredUOMId, inventoryUOMId);
                        inventoryIssueLinesListOnDisplay[i].Quantity = inventoryIssueLinesListOnDisplay[i].Quantity * factor;
                    }
                }
            }
            if (code.Equals("DIIS"))
            {
                if (this.inventoryIssueHeaderDTO.PurchaseOrderId < 0)
                {
                    string errorMessage = MessageContainerList.GetMessage(machineContext, 1078);
                    throw new ValidationException(errorMessage);

                }
                if (inventoryIssueHeaderDTO.SupplierInvoiceDate != null)
                {
                    try
                    {
                        Convert.ToDateTime(inventoryIssueHeaderDTO.SupplierInvoiceDate);
                    }
                    catch
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, 1079);
                        throw new ValidationException(errorMessage);
                    }
                }
                if (inventoryIssueHeaderDTO.DeliveryNoteDate != null)
                {
                    try
                    {
                        Convert.ToDateTime(inventoryIssueHeaderDTO.DeliveryNoteDate);
                    }
                    catch
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, 1080);
                        throw new ValidationException(errorMessage);
                    }
                }
            }
            if ((inventoryIssueLinesListOnDisplay == null) || (inventoryIssueLinesListOnDisplay != null && inventoryIssueLinesListOnDisplay.Count == 0))
            {
                string errorMessage = MessageContainerList.GetMessage(machineContext, 371);
                throw new ValidationException(errorMessage);

            }
            else if (inventoryIssueLinesListOnDisplay != null && inventoryIssueLinesListOnDisplay.Count > 0)
            {
                List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = inventoryIssueLinesListOnDisplay.Where(x => (bool)(x.Quantity == 0 && x.IssueLineId == -1)).ToList<InventoryIssueLinesDTO>();
                if (inventoryIssueLinesDTOList != null && inventoryIssueLinesDTOList.Count == inventoryIssueLinesListOnDisplay.Count)
                {
                    string errorMessage = MessageContainerList.GetMessage(machineContext, 371);
                    throw new ValidationException(errorMessage);
                }
            }
            if (!code.Equals("DIIS"))
            {

                foreach (InventoryIssueLinesDTO inventoryIssueLinesDTO in inventoryIssueLinesListOnDisplay)
                {
                    if (inventoryIssueLinesDTO.Quantity == 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, 479);
                        throw new ValidationException(errorMessage);
                    }
                    if (inventoryIssueLinesDTO.AvailableQuantity <= 0 && inventoryIssueLinesDTO.Quantity < 0)
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, 4963);
                        throw new ValidationException(errorMessage);
                    }
                    if (!code.Equals("AJIS") && inventoryIssueLinesDTO.AvailableQuantity < inventoryIssueLinesDTO.Quantity)
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, 1084);
                        throw new ValidationException(errorMessage);
                    }
                    if (inventoryIssueLinesDTO.FromLocationID == -1)
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, 1067);
                        throw new ValidationException(errorMessage);
                    }
                    if (inventoryIssueLinesDTO.ToLocationID == -1 && !code.Equals("ITIS") && !code.Equals("AJIS"))
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, 1068);
                        throw new ValidationException(errorMessage);
                    }
                    if (inventoryIssueLinesDTO.ToLocationID == inventoryIssueLinesDTO.FromLocationID)
                    {
                        string errorMessage = MessageContainerList.GetMessage(machineContext, 1085);
                        throw new ValidationException(errorMessage);
                    }
                }

            }
            if (code.Equals("DIIS") || code.Equals("REIS"))
            {
                // Faizan : This Validation need to verify.

                if (inventoryIssueLinesListOnDisplay != null)
                {
                    for (int i = 0; i < inventoryIssueLinesListOnDisplay.Count; i++)
                    {
                        if ((double)inventoryIssueLinesListOnDisplay[i].Quantity < inventoryIssueLinesListOnDisplay[i].Quantity)
                        {
                            string errorMessage = MessageContainerList.GetMessage(machineContext, 1081, inventoryIssueLinesListOnDisplay[i].ProductName);
                            throw new ValidationException(errorMessage);
                        }
                    }
                }
            }
            if (inventoryIssueLinesListOnDisplay == null || inventoryIssueLinesListOnDisplay.Count == 0)
            {
                string errorMessage = MessageContainerList.GetMessage(machineContext, 1082);
                throw new ValidationException(errorMessage);
            }
            if (code.Equals("ITIS") && inventoryIssueHeaderDTO.ToSiteID.Equals(-1))
            {
                string errorMessage = MessageContainerList.GetMessage(machineContext, 1186);
                throw new ValidationException(errorMessage);
            }
            log.LogMethodExit(true);
            return true;
        }
        /// <summary>
        /// Gets the InventoryIssueHeaderDTO
        /// </summary>
        public InventoryIssueHeaderDTO getInventoryIssueHeaderDTO { get { return inventoryIssueHeaderDTO; } }
    }
    /// <summary>
    /// Manages the list of inventory issue header
    /// </summary>
    public class InventoryIssueHeaderList
    {
        private ExecutionContext machineContext;
        List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = new List<InventoryIssueHeaderDTO>();
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor with no parameter.
        /// </summary>
        public InventoryIssueHeaderList() //added
        {
            log.LogMethodEntry();
            this.inventoryIssueHeaderDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Default constructor with machineContext
        /// </summary>
        public InventoryIssueHeaderList(ExecutionContext machineContext)
            : this()
        {
            log.LogMethodEntry(machineContext);
            this.machineContext = machineContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the asset list
        /// </summary>
        /// <param name="issueId">issueId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryIssueHeaderDTO</returns>
        public InventoryIssueHeaderDTO GetIssueHeader(int issueId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(issueId, sqlTransaction);
            InventoryIssueHeaderDataHandler inventoryIssueHeaderDataHandler = new InventoryIssueHeaderDataHandler(sqlTransaction);
            InventoryIssueHeaderDTO inventoryIssueHeaderDTO = inventoryIssueHeaderDataHandler.GetInventoryIssueHeader(issueId);
            log.LogMethodExit(inventoryIssueHeaderDTO);
            return inventoryIssueHeaderDTO;
        }

        /// <summary>
        /// Returns the Inventory Issue Header list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>inventoryIssueHeaderDTOList</returns>
        public List<InventoryIssueHeaderDTO> GetAllInventoryIssueHeaders(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            InventoryIssueHeaderDataHandler inventoryIssueHeaderDataHandler = new InventoryIssueHeaderDataHandler(sqlTransaction);
            List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = inventoryIssueHeaderDataHandler.GetInventoryIssueHeaderList(searchParameters);
            log.LogMethodExit(inventoryIssueHeaderDTOList);
            return inventoryIssueHeaderDTOList;
        }
        /// <summary>
        /// Returns the Inventory Receive Lines list along with Child
        /// </summary>
        public List<InventoryIssueHeaderDTO> GetAllInventoryIssueHeader(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, bool buildChildRecords = false, bool loadActiveChild = false, SqlTransaction sqlTransaction = null) //added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryIssueHeaderDataHandler InventoryIssueHeaderDataHandler = new InventoryIssueHeaderDataHandler(sqlTransaction);
            List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList = InventoryIssueHeaderDataHandler.GetInventoryIssueHeaderLists(searchParameters, currentPage, pageSize);
            if (inventoryIssueHeaderDTOList != null && inventoryIssueHeaderDTOList.Count > 0 && buildChildRecords)
            {
                Build(inventoryIssueHeaderDTOList, loadActiveChild, sqlTransaction);
            }
            log.LogMethodExit(inventoryIssueHeaderDTOList);
            return inventoryIssueHeaderDTOList;
        }

        private void Build(List<InventoryIssueHeaderDTO> inventoryIssueHeaderDTOList, bool activeChildRecords, SqlTransaction sqlTransaction) //added
        {
            Dictionary<int, InventoryIssueHeaderDTO> inventoryIssueHeaderDTODictionary = new Dictionary<int, InventoryIssueHeaderDTO>();
            List<int> inventoryIssueHeaderIdList = new List<int>();
            for (int i = 0; i < inventoryIssueHeaderDTOList.Count; i++)
            {
                if (inventoryIssueHeaderDTODictionary.ContainsKey(inventoryIssueHeaderDTOList[i].InventoryIssueId))
                {
                    continue;
                }
                inventoryIssueHeaderDTODictionary.Add(inventoryIssueHeaderDTOList[i].InventoryIssueId, inventoryIssueHeaderDTOList[i]);
                inventoryIssueHeaderIdList.Add(inventoryIssueHeaderDTOList[i].InventoryIssueId);
            }
            InventoryIssueLinesList inventoryIssueLinesList = new InventoryIssueLinesList(machineContext);
            List<InventoryIssueLinesDTO> inventoryIssueLinesDTOList = inventoryIssueLinesList.GetAllInventoryIssueLinesDTOList(inventoryIssueHeaderIdList, activeChildRecords, sqlTransaction);
            if (inventoryIssueLinesDTOList != null && inventoryIssueLinesDTOList.Any())
            {
                for (int i = 0; i < inventoryIssueLinesDTOList.Count; i++)
                {
                    if (inventoryIssueHeaderDTODictionary.ContainsKey(inventoryIssueLinesDTOList[i].IssueId) == false)
                    {
                        continue;
                    }
                    InventoryIssueHeaderDTO inventoryIssueHeaderDTO = inventoryIssueHeaderDTODictionary[inventoryIssueLinesDTOList[i].IssueId];
                    if (inventoryIssueHeaderDTO.InventoryIssueLinesListDTO == null)
                    {
                        inventoryIssueHeaderDTO.InventoryIssueLinesListDTO = new List<InventoryIssueLinesDTO>();
                    }
                    inventoryIssueHeaderDTO.InventoryIssueLinesListDTO.Add(inventoryIssueLinesDTOList[i]);
                }
            }
        }
        /// <summary>
        /// Returns the no of InventoryIssueHeader matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetInventoryIssueHeaderCount(List<KeyValuePair<InventoryIssueHeaderDTO.SearchByInventoryIssueHeaderParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            InventoryIssueHeaderDataHandler inventoryIssueHeaderDataHandler = new InventoryIssueHeaderDataHandler(sqlTransaction);
            int inventoryIssueHeadersCount = inventoryIssueHeaderDataHandler.GetInventoryIssueHeadersCount(searchParameters);
            log.LogMethodExit(inventoryIssueHeadersCount);
            return inventoryIssueHeadersCount;
        }
    }
}
