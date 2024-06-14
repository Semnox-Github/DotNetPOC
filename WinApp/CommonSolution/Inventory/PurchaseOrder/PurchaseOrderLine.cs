/********************************************************************************************
*Project Name -                                                                           
*Description  -
*************
**Version Log
*************
*Version     Date                   Modified By                 Remarks          
*********************************************************************************************
 *2.60      13-04-2019            Girish                       Modified
 *2.70.2      16-Jul-2019           Deeksha                      Modified save method
 **********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// Bussiness logic for purchase order lines
    /// </summary>
    public class PurchaseOrderLine
    {
        private PurchaseOrderLineDTO purchaseOrderLineDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor
        /// </summary>
        public PurchaseOrderLine()
        {
            log.LogMethodEntry();
            purchaseOrderLineDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the PurchaseOrderLine DTO parameter
        /// </summary>
        /// <param name="purchaseOrderLineDTO">Parameter of the type PurchaseOrderLineDTO</param>
        /// <param name="executionUserContext">Execution context</param>
        public PurchaseOrderLine(PurchaseOrderLineDTO purchaseOrderLineDTO, ExecutionContext executionContext)
        {
            log.LogMethodEntry(purchaseOrderLineDTO, executionContext);
            this.purchaseOrderLineDTO = purchaseOrderLineDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        
        /// <summary>
        /// Constructor with the PurchaseOrderLine DTO parameter
        /// </summary>
        /// <param name="purchaseOrderLineDTO">Parameter of the type PurchaseOrderLineDTO</param>
        /// <param name="executionUserContext">Execution context</param>
        public PurchaseOrderLine(ExecutionContext executionContext,PurchaseOrderLineDTO purchaseOrderLineDTO)
        {
            log.LogMethodEntry(purchaseOrderLineDTO, executionContext);
            this.purchaseOrderLineDTO = purchaseOrderLineDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the PurchaseOrderLine id as the parameter
        /// Would fetch the PurchaseOrderLine object from the database based on the id passed.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="id"></param>
        /// <param name="loadChildRecords"></param>
        /// <param name="activeChildRecords"></param>
        /// <param name="sqlTransaction"></param>
        public PurchaseOrderLine(ExecutionContext executionContext, int id, bool loadChildRecords = true,
           bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            PurchaseOrderLineDataHandler purchaseOrderLineDataHandler = new PurchaseOrderLineDataHandler(sqlTransaction);
            purchaseOrderLineDTO = purchaseOrderLineDataHandler.GetPurchaseOrderLine(id);
            if (purchaseOrderLineDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 14911, "PurchaseOrderLine", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords, sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);
            PurchaseOrderTaxLineListBL purchaseOrderTaxLineListBL = new PurchaseOrderTaxLineListBL(executionContext);
            List<KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>> searchParameters = new List<KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>>();
            searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_LINE_ID, purchaseOrderLineDTO.PurchaseOrderLineId.ToString()));
            searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PRODUCT_ID, purchaseOrderLineDTO.ProductId.ToString()));
            searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_ID, purchaseOrderLineDTO.PurchaseOrderId.ToString()));
            if (activeChildRecords)
            {
                searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.ISACTIVE_FLAG, "1"));
            }
            purchaseOrderLineDTO.PurchaseOrderTaxLineDTOList = purchaseOrderTaxLineListBL.GetPurchaseOrderTaxLines(searchParameters, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the purchaseOrderLine
        /// purchaseOrderLine will be inserted if purchaseOrderLineId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);
            PurchaseOrderLineDataHandler purchaseOrderLineDataHandler = new PurchaseOrderLineDataHandler(SQLTrx);
            if (purchaseOrderLineDTO.PurchaseOrderLineId < 0)
            {
                purchaseOrderLineDTO = purchaseOrderLineDataHandler.Insert(purchaseOrderLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                PurchaseOrderTaxLineBL purchaseOrderTaxLineBL = new PurchaseOrderTaxLineBL(executionContext);
                purchaseOrderLineDTO.PurchaseOrderTaxLineDTOList = purchaseOrderTaxLineBL.BuildTaxLines(purchaseOrderLineDTO, SQLTrx);
                purchaseOrderLineDTO.AcceptChanges();
            }
            else
            {
                if (purchaseOrderLineDTO.IsChanged)
                {
                    purchaseOrderLineDTO=purchaseOrderLineDataHandler.Update(purchaseOrderLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    PurchaseOrderTaxLineBL purchaseOrderTaxLineBL = new PurchaseOrderTaxLineBL(executionContext);
                    purchaseOrderLineDTO.PurchaseOrderTaxLineDTOList = purchaseOrderTaxLineBL.BuildTaxLines(purchaseOrderLineDTO, SQLTrx);
                    purchaseOrderLineDTO.AcceptChanges();
                }
            }

            if (purchaseOrderLineDTO.PurchaseOrderTaxLineDTOList != null &&
               purchaseOrderLineDTO.PurchaseOrderTaxLineDTOList.Any())
            {
                List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
                foreach (var purchaseOrderTaxLineDTO in purchaseOrderLineDTO.PurchaseOrderTaxLineDTOList)
                {
                    
                    if (purchaseOrderTaxLineDTO.IsChanged)
                    {
                        purchaseOrderTaxLineDTOList.Add(purchaseOrderTaxLineDTO);
                    }
                }
                if (purchaseOrderTaxLineDTOList.Any())
                {
                    PurchaseOrderTaxLineListBL purchaseOrderTaxLineListBL = new PurchaseOrderTaxLineListBL(executionContext, purchaseOrderLineDTO.PurchaseOrderTaxLineDTOList);
                    purchaseOrderTaxLineListBL.Save(SQLTrx);
                }
            }

            log.LogMethodExit();
        }

        public PurchaseOrderLineDTO PurchaseOrderLineDTO { get { return purchaseOrderLineDTO; } }
    }

    /// <summary>
    /// Manages the list of Inventory lot List
    /// </summary>
    public class PurchaseOrderLineList
        {
            private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            /// <summary>
            /// 
            /// Returns the purchaseOrderLine
            /// </summary>
            public PurchaseOrderLineDTO GetPurchaseOrderLine(int purchaseOrderLineId,SqlTransaction sqlTransaction=null)
            {
                log.LogMethodEntry(purchaseOrderLineId);
                PurchaseOrderLineDataHandler purchaseOrderLineDataHandler = new PurchaseOrderLineDataHandler(sqlTransaction);
                PurchaseOrderLineDTO purchaseOrderLineDTO = purchaseOrderLineDataHandler.GetPurchaseOrderLine(purchaseOrderLineId);
                log.LogMethodExit(purchaseOrderLineDTO);
                return purchaseOrderLineDTO;
            }
            /// <summary>
            /// Returns the PurchaseOrderLineDTO List
            /// </summary>
            public List<PurchaseOrderLineDTO> GetAllPurchaseOrderLine(List<KeyValuePair<PurchaseOrderLineDTO.SearchByPurchaseOrderLineParameters, string>> searchParameters, SqlTransaction sqltrxn = null)
            {
                log.LogMethodEntry(searchParameters, sqltrxn);
                PurchaseOrderLineDataHandler purchaseOrderLineDataHandler = new PurchaseOrderLineDataHandler(sqltrxn);
                List<PurchaseOrderLineDTO> purchaseOrderLineDTOList = purchaseOrderLineDataHandler.GetPurchaseOrderLineList(searchParameters);
                log.LogMethodExit(purchaseOrderLineDTOList);
                return purchaseOrderLineDTOList;
            }

            ///// <summary>
            ///// Returns the POLine Open Qty
            ///// </summary>
            //public double GetPOLineOpenQty(PurchaseOrderLineDTO purchaseOrderLineDTO, PurchaseOrderDTO purchaseOrderDTO)
            //{
            //    log.LogMethodEntry("Starts-GetPOLineOpenQty.");
            //    double lineQty = 0;
            //    double receivedQty = 0;
            //    lineQty = purchaseOrderLineDTO.Quantity;
            //    if (purchaseOrderDTO != null && purchaseOrderLineDTO != null)
            //    {
            //        if (purchaseOrderDTO.InventoryReceiptListDTO != null && purchaseOrderDTO.PurchaseOrderId == purchaseOrderLineDTO.PurchaseOrderId)
            //        {
            //            foreach (InventoryReceiptDTO inventoryReceiptDTO in purchaseOrderDTO.InventoryReceiptListDTO)
            //            {
            //                if (inventoryReceiptDTO.InventoryReceiveLinesListDTO != null)
            //                {
            //                    List<InventoryReceiveLinesDTO> inventoryReceiveLinesListDTO = new List<InventoryReceiveLinesDTO>();
            //                    inventoryReceiveLinesListDTO = inventoryReceiptDTO.InventoryReceiveLinesListDTO.Where(receiveLine => receiveLine.PurchaseOrderLineId == purchaseOrderLineDTO.PurchaseOrderLineId).ToList();
            //                    if (inventoryReceiveLinesListDTO != null)
            //                    {
            //                        foreach (InventoryReceiveLinesDTO receiveLine in inventoryReceiveLinesListDTO)
            //                        {
            //                            receivedQty = receivedQty + receiveLine.Quantity;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }   
            //    log.LogMethodEntry("Ends-GetPOLineOpenQty.");
            //    return (lineQty - receivedQty);
            //}
        }
 }


