/********************************************************************************************
 * Project Name - PurchaseOrderReceiveTaxLine BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version      Date             Modified By       Remarks          
 ************ **********************************************************************************
 *2.60        11-Apr-2019       Girish Kundar     Created
 *2.70.2        16-Jul-2019       Deeksha           Modified save method
  *2.110.0     29-Dec-2020       Prajwal S         Added GetAllPurchaseOrderReceiveTaxLineDTOList to get 
 *                                                PurchaseOrderReceiveTaxLineDTOList using parent Id list
  ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;


namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderReceiveTaxLineBL
    {
        public PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO;
        private PurchaseOrderReceiveTaxLineDataHandler purchaseOrderReceiveTaxLineDataHandler;
        private ExecutionContext executionContext;
        private List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineDTOList;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// default Constructor
        /// </summary>
        public PurchaseOrderReceiveTaxLineBL(ExecutionContext excecutionContext)
        {
            log.LogMethodEntry(excecutionContext);
            purchaseOrderReceiveTaxLineDTO = new PurchaseOrderReceiveTaxLineDTO();
            this.executionContext = excecutionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="purchaseOrderTaxLineDTO">Parameter of the type InventoryIssueHeaderDTO</param>
        /// <param name="executionContext">Execution context</param>
        public PurchaseOrderReceiveTaxLineBL(PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(purchaseOrderReceiveTaxLineDTO, executionContext);
            this.purchaseOrderReceiveTaxLineDTO = purchaseOrderReceiveTaxLineDTO;
            log.LogMethodExit();
        }

        public PurchaseOrderReceiveTaxLineBL(ExecutionContext executionContext, List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineDTOList)
           : this(executionContext)
        {
            this.purchaseOrderReceiveTaxLineDTOList = purchaseOrderReceiveTaxLineDTOList;
        }
        /// <summary>
        /// Save  purchaseOrderReceiveTaxLineDTO 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            purchaseOrderReceiveTaxLineDataHandler = new PurchaseOrderReceiveTaxLineDataHandler(sqlTransaction);
            if (purchaseOrderReceiveTaxLineDTO.PurchaseOrderReceiveTaxLineId <= 0)
            {
                purchaseOrderReceiveTaxLineDTO = purchaseOrderReceiveTaxLineDataHandler.Insert(purchaseOrderReceiveTaxLineDTO,
                                                                               executionContext.GetUserId(), executionContext.GetSiteId());
                purchaseOrderReceiveTaxLineDTO.AcceptChanges();
            }
            else if (purchaseOrderReceiveTaxLineDTO.IsChanged)
            {
                purchaseOrderReceiveTaxLineDTO=purchaseOrderReceiveTaxLineDataHandler.Update(purchaseOrderReceiveTaxLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                purchaseOrderReceiveTaxLineDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates and saves the PurchaseOrderReceiveTaxLine  records 
        /// </summary>
        public List<PurchaseOrderReceiveTaxLineDTO> BuildTaxLines(InventoryReceiveLinesDTO inventoryReceiveLinesDTO, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(inventoryReceiveLinesDTO, sqlTransaction);
            List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderTaxReceiveLineDTOList;
            PurchaseOrderReceiveTaxLineListBL purchaseOrderTaxLineListBL = new PurchaseOrderReceiveTaxLineListBL(executionContext);
            List<KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>> searchParameters = new List<KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>>();
            searchParameters.Add(new KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>(PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PO_RECEIVE_LINE_ID, inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId.ToString()));
            searchParameters.Add(new KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>(PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.PRODUCT_ID, inventoryReceiveLinesDTO.ProductId.ToString()));
            searchParameters.Add(new KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>(PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.RECEIPT_ID, inventoryReceiveLinesDTO.ReceiptId.ToString()));
            searchParameters.Add(new KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>(PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters.ISACTIVE_FLAG, "1"));
            purchaseOrderTaxReceiveLineDTOList = purchaseOrderTaxLineListBL.GetPurchaseOrderReceiveTaxLines(searchParameters, sqlTransaction);
            log.LogMethodExit("Existing PurchaseOrderTaxReceiveLines");
            log.LogVariableState("PurchaseOrderTaxReceiveLineDTOList", purchaseOrderTaxReceiveLineDTOList);

            if (purchaseOrderTaxReceiveLineDTOList == null)
            {
                purchaseOrderTaxReceiveLineDTOList = new List<PurchaseOrderReceiveTaxLineDTO>();
            }
            else
            {
                foreach (PurchaseOrderReceiveTaxLineDTO purchaseOrderTaxLineDTO in purchaseOrderTaxReceiveLineDTOList)
                {
                    purchaseOrderTaxLineDTO.IsActive = false;
                }
            }
            // Step 2
            // if the new tax id is 0 or -1, only the amount is being updated
            // in this case, the Update of tax line dto should be called
            if (inventoryReceiveLinesDTO.PurchaseTaxId <= 0)
            {
                PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO = new PurchaseOrderReceiveTaxLineDTO(
                                                 -1, inventoryReceiveLinesDTO.ReceiptId, inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId,
                                                 -1, string.Empty, -1, string.Empty, inventoryReceiveLinesDTO.ProductId, 0, 
                                                  inventoryReceiveLinesDTO.TaxAmount, true,string.Empty, DateTime.MinValue, string.Empty,
                                                  DateTime.MinValue, string.Empty, false, -1, -1);
                purchaseOrderReceiveTaxLineDTO.IsChanged = true;
                purchaseOrderTaxReceiveLineDTOList.Add(purchaseOrderReceiveTaxLineDTO);
                log.LogMethodExit("No Tax selected - One record to   PurchaseOrderTaxReceiveLine");
                log.LogVariableState("PurchaseOrderTaxReceiveLineDTOList", purchaseOrderTaxReceiveLineDTOList);
            }
            // Step 3
            // if the tax id is greated than 0, a tax has been chosen
            // get details of the tax and corresponding tax structure
            // mark all old taxes for deletion
            else // (purchaseTaxId > 0)
            {
                //Tax List details
                TaxList taxList = new TaxList(executionContext);
                List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> SearchByTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
                SearchByTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_ID, inventoryReceiveLinesDTO.PurchaseTaxId.ToString()));
                List<TaxDTO> taxDTOList = taxList.GetAllTaxes(SearchByTaxParameters);

                if (taxDTOList != null && taxDTOList.Count > 0)
                {
                    TaxDTO selectedTax = taxDTOList[0];
                    TaxStructureList taxStructureList = new TaxStructureList(executionContext);
                    List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>> SearchByTaxStructureParameters = new List<KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>>();
                    SearchByTaxStructureParameters.Add(new KeyValuePair<TaxStructureDTO.SearchByTaxStructureParameters, string>(TaxStructureDTO.SearchByTaxStructureParameters.TAX_ID, selectedTax.TaxId.ToString()));
                    List<TaxStructureDTO> selectedTaxStructure = taxStructureList.GetTaxStructureList(SearchByTaxStructureParameters);
                    log.LogMethodExit("Tax structure List for the current Purchase Tax Id");
                    log.LogVariableState("SelectedTaxStructure", selectedTaxStructure);
                    // Step 4
                    // add new tax lines dto (single if there is no structure) 
                    if (selectedTaxStructure != null && selectedTaxStructure.Count > 0)
                    {
                        List<TaxStructureDTO> templList = new List<TaxStructureDTO>();
                        Dictionary<int, int> processedTax = new Dictionary<int, int>();
                        selectedTaxStructure = selectedTaxStructure.OrderBy(x => x.ParentStructureId).ToList();
                        templList.Add(selectedTaxStructure[0]);
                        while (selectedTaxStructure.Count > 0)
                        {
                            TaxStructureDTO currentTax = selectedTaxStructure[0];
                            if (currentTax.ParentStructureId > 0 && !processedTax.ContainsKey(currentTax.ParentStructureId))
                            {
                                selectedTaxStructure.RemoveAt(0);
                                selectedTaxStructure.Add(currentTax);
                            }
                            else
                            {
                                // create tax line
                                PurchaseOrderReceiveTaxLineDTO parentTaxLine = new PurchaseOrderReceiveTaxLineDTO();

                                if (currentTax.ParentStructureId > 0)
                                {
                                    List<PurchaseOrderReceiveTaxLineDTO> parentTax = purchaseOrderTaxReceiveLineDTOList.Where(x => x.TaxStructureId == currentTax.ParentStructureId).ToList();
                                    if (parentTax != null && parentTax.Count > 0)
                                    {
                                        parentTaxLine = parentTax[0];
                                    }
                                }
                                Decimal parentTaxAmount = parentTaxLine.TaxAmount > 0 ? parentTaxLine.TaxAmount : inventoryReceiveLinesDTO.TaxAmount;
                                decimal poTaxAmount = ((parentTaxAmount) * (Convert.ToDecimal(currentTax.Percentage.ToString())) / Convert.ToDecimal(selectedTax.TaxPercentage.ToString()));

                                PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO = new PurchaseOrderReceiveTaxLineDTO(-1, inventoryReceiveLinesDTO.ReceiptId, 
                                                                                                inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId, inventoryReceiveLinesDTO.PurchaseTaxId, 
                                                                                                selectedTax.TaxName, currentTax.TaxStructureId, currentTax.StructureName,
                                                                                                inventoryReceiveLinesDTO.ProductId,Convert.ToDecimal(currentTax.Percentage.ToString()),
                                                                                                poTaxAmount, true, string.Empty, DateTime.MinValue, string.Empty, DateTime.MinValue, string.Empty, false, -1, -1);
                               
                                purchaseOrderReceiveTaxLineDTO.IsChanged = true;
                                purchaseOrderTaxReceiveLineDTOList.Add(purchaseOrderReceiveTaxLineDTO);
                                selectedTaxStructure.RemoveAt(0);
                                processedTax.Add(currentTax.TaxStructureId, 0);
                                log.LogMethodExit("Tax  with strucuture selected - Multiple records to   PurchaseOrderTaxReceiveLine");
                                log.LogVariableState("PurchaseOrderTaxReceiveLineDTOList", purchaseOrderTaxReceiveLineDTOList);
                            }
                        }
                    }
                    else //add new tax lines dto (Multiple if there is  structure) 
                    {
                        // Single tax
                        PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO = new PurchaseOrderReceiveTaxLineDTO(
                                                    -1, inventoryReceiveLinesDTO.ReceiptId, inventoryReceiveLinesDTO.PurchaseOrderReceiveLineId,
                                                    selectedTax.TaxId, selectedTax.TaxName, -1, string.Empty, inventoryReceiveLinesDTO.ProductId,
                                                    Convert.ToDecimal(selectedTax.TaxPercentage.ToString()), inventoryReceiveLinesDTO.TaxAmount, true,
                                                    string.Empty, DateTime.MinValue, string.Empty, DateTime.MinValue, string.Empty, false, -1, -1);
                       
                        purchaseOrderReceiveTaxLineDTO.IsChanged = true;
                        purchaseOrderTaxReceiveLineDTOList.Add(purchaseOrderReceiveTaxLineDTO);
                        log.LogMethodExit("Tax with no structure selected - One record to   PurchaseOrderTaxReceiveLine");
                        log.LogVariableState("PurchaseOrderTaxReceiveLineDTOList", purchaseOrderTaxReceiveLineDTOList);
                    }
                }
            }
            log.LogVariableState("PurchaseOrderTaxReceiveLineDTOList", purchaseOrderTaxReceiveLineDTOList);
            log.LogMethodExit(purchaseOrderTaxReceiveLineDTOList);
            return purchaseOrderTaxReceiveLineDTOList;
       
        }

    }

    public class PurchaseOrderReceiveTaxLineListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineList = new List<PurchaseOrderReceiveTaxLineDTO>();
        public PurchaseOrderReceiveTaxLineListBL(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="purchaseOrderReceiveTaxLineList"></param>
        public PurchaseOrderReceiveTaxLineListBL(ExecutionContext executionContext, List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineList)
            : this(executionContext)
        {
            this.purchaseOrderReceiveTaxLineList = purchaseOrderReceiveTaxLineList;
        }
        public List<PurchaseOrderReceiveTaxLineDTO> GetPurchaseOrderReceiveTaxLines(List<KeyValuePair<PurchaseOrderReceiveTaxLineDTO.SearchByPurchaseOrderReceiveTaxLineParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            PurchaseOrderReceiveTaxLineDataHandler purchaseOrderReceiveTaxLineDataHandler = new PurchaseOrderReceiveTaxLineDataHandler(sqlTransaction);
            List<PurchaseOrderReceiveTaxLineDTO> purchaseOrderReceiveTaxLineDTOList = purchaseOrderReceiveTaxLineDataHandler.GetPurchaseOrderReceiveTaxLines(searchParameters);
            log.LogMethodExit(purchaseOrderReceiveTaxLineDTOList);
            return purchaseOrderReceiveTaxLineDTOList;
        }

        /// <summary>
        /// Saves the PurchaseOrderReceiveTaxLineList
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            if (purchaseOrderReceiveTaxLineList != null && purchaseOrderReceiveTaxLineList.Count > 0)
            {
                foreach (PurchaseOrderReceiveTaxLineDTO purchaseOrderReceiveTaxLineDTO in purchaseOrderReceiveTaxLineList)
                {
                    PurchaseOrderReceiveTaxLineBL purchaseOrderReceiveTaxLineBL = new PurchaseOrderReceiveTaxLineBL(purchaseOrderReceiveTaxLineDTO, this.executionContext);
                    purchaseOrderReceiveTaxLineBL.Save(sqlTransaction);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PurchaseOrderReceiveTaxLineDTO List for screenIdList
        /// </summary>
        /// <param name="purchaseOrderReceiveLineIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of PurchaseOrderReceiveTaxLineDTO</returns>
        public List<PurchaseOrderReceiveTaxLineDTO> GetAllPurchaseOrderReceiveTaxLineDTOList(List<int> purchaseOrderReceiveLineIdList, bool activeRecords, SqlTransaction sqlTransaction = null) //added
        {
            log.LogMethodEntry(purchaseOrderReceiveLineIdList, activeRecords, sqlTransaction);
            PurchaseOrderReceiveTaxLineDataHandler purchaseOrderReceiveTaxLineDataHandler = new PurchaseOrderReceiveTaxLineDataHandler(sqlTransaction);
            this.purchaseOrderReceiveTaxLineList = purchaseOrderReceiveTaxLineDataHandler.GetPurchaseOrderReceiveTaxLineDTOList(purchaseOrderReceiveLineIdList, activeRecords);
            log.LogMethodExit(purchaseOrderReceiveTaxLineList);
            return purchaseOrderReceiveTaxLineList;
        }
    }
}
