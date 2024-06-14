/********************************************************************************************
 * Project Name - PurchaseOrderTaxLineBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By       Remarks          
 **********************************************************************************************
 *2.60        11-Apr-2019       Girish Kundar     Created
 *2.70.2      16-Jul-2019       Deeksha           Modified save method
 *2.110.0     11-Jan-2021      Abhishek           Modified : modified for API changes
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory
{
    public class PurchaseOrderTaxLineBL
    {
        private PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO;
        // PurchaseOrderTaxLineDataHandler purchaseOrderTaxLineDataHandler;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = ExecutionContext.GetExecutionContext();

        /// <summary>
        /// Default constructor
        /// </summary>
        public PurchaseOrderTaxLineBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            purchaseOrderTaxLineDTO = new PurchaseOrderTaxLineDTO();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="purchaseOrderTaxLineDTO">Parameter of the type InventoryIssueHeaderDTO</param>
        /// <param name="machineContext">Execution context</param>
        public PurchaseOrderTaxLineBL(PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO, ExecutionContext executionContext)
            : this(executionContext)
        {
            log.LogMethodEntry(purchaseOrderTaxLineDTO, executionContext);
            this.purchaseOrderTaxLineDTO = purchaseOrderTaxLineDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="PurchaseOrderTaxLineId">Parameter of the type Integer</param>
        /// <param name="machineContext">Execution Context</param>
        /// <param name="sqltrxn">Parameter of the type SqlTransaction</param>
        public PurchaseOrderTaxLineBL(int PurchaseOrderTaxLineId, ExecutionContext executionContext, SqlTransaction sqltrxn)
            : this(executionContext)
        {
            log.LogMethodEntry(PurchaseOrderTaxLineId, executionContext, sqltrxn);
            PurchaseOrderTaxLineDataHandler purchaseOrderTaxLineDataHandler = new PurchaseOrderTaxLineDataHandler(sqltrxn);
            //this.purchaseOrderTaxLineDTO = purchaseOrderTaxLineDataHandler.GetPurchaseOrderTaxLineHeader(PurchaseOrderTaxLineId, sqltrxn);
            log.LogMethodExit();
        }

        /// <summary>
        /// purchaseOrderTaxLine will be inserted if purchaseOrderLineId is less than or equal to
        /// zero else Marked for the deletion
        /// </summary>
        /// <param name="SQLTrx"></param>
        public void Save(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);
            PurchaseOrderTaxLineDataHandler purchaseOrderTaxLineDataHandler = new PurchaseOrderTaxLineDataHandler(SQLTrx);
            if (purchaseOrderTaxLineDTO.PurchaseOrderTaxLineId < 0)
            {
                purchaseOrderTaxLineDTO= purchaseOrderTaxLineDataHandler.Insert(purchaseOrderTaxLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                purchaseOrderTaxLineDTO.AcceptChanges();
            }
            else if (purchaseOrderTaxLineDTO.IsActive)
            {
                purchaseOrderTaxLineDTO=purchaseOrderTaxLineDataHandler.Update(purchaseOrderTaxLineDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                purchaseOrderTaxLineDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the PurchaseOrderTaxLineDTO
        /// </summary>
        public PurchaseOrderTaxLineDTO PurchaseOrderTaxLineDTO { get { return purchaseOrderTaxLineDTO; } }

        public List<PurchaseOrderTaxLineDTO>  BuildTaxLines (PurchaseOrderLineDTO purchaseOrderLineDTO, SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(purchaseOrderLineDTO, SQLTrx);
            List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList;
            PurchaseOrderTaxLineListBL purchaseOrderTaxLineListBL = new PurchaseOrderTaxLineListBL(executionContext);
            List<KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>> searchParameters = new List<KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>>();
            searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_LINE_ID, purchaseOrderLineDTO.PurchaseOrderLineId.ToString()));
            searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PRODUCT_ID, purchaseOrderLineDTO.ProductId.ToString()));
            searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_ID, purchaseOrderLineDTO.PurchaseOrderId.ToString()));
            searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.ISACTIVE_FLAG, "1"));
            purchaseOrderTaxLineDTOList = purchaseOrderTaxLineListBL.GetPurchaseOrderTaxLines(searchParameters, SQLTrx);
            if (purchaseOrderTaxLineDTOList == null)
            {
                purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
            }
            else
            {
                foreach (PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO in purchaseOrderTaxLineDTOList)
                {
                    purchaseOrderTaxLineDTO.IsActive = false;
                }
            }
            // Step 2
            // if the new tax id is 0 or -1, only the amount is being updated
            // in this case, the Update of tax line dto should be called
            if (purchaseOrderLineDTO.PurchaseTaxId <= 0)
            {
                PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO = new PurchaseOrderTaxLineDTO(-1, purchaseOrderLineDTO.PurchaseOrderId, purchaseOrderLineDTO.PurchaseOrderLineId, -1, string.Empty,
                                                                  -1, string.Empty, purchaseOrderLineDTO.ProductId, 0, Convert.ToDecimal(purchaseOrderLineDTO.TaxAmount), true, string.Empty,
                                                                  DateTime.MinValue, string.Empty, DateTime.MinValue, string.Empty,
                                                                  false, -1, -1);
                
                purchaseOrderTaxLineDTO.IsChanged = true;
                log.LogMethodExit("No Tax -  : One line with Purchase tax ID =Null to  PurchaseOrderTaxLine  ");
                log.LogVariableState("PurchaseOrderTaxLineDTO", purchaseOrderTaxLineDTO);
                purchaseOrderTaxLineDTOList.Add(purchaseOrderTaxLineDTO);
                log.LogVariableState("PurchaseOrderTaxLineDTOList", purchaseOrderTaxLineDTOList);
            }

            // Step 3
            // if the tax id is greated than 0, a tax has been chosen
            // get details of the tax and corresponding tax structure
            // mark all old taxes for deletion
            else // (purchaseTaxId > 0)
            {
                List<PurchaseOrderTaxLineDTO> purchaseTaxLineDTOList = CalculateTaxes(purchaseOrderLineDTO.PurchaseTaxId, purchaseOrderLineDTO.PurchaseOrderId, purchaseOrderLineDTO.PurchaseOrderLineId, Convert.ToDecimal(purchaseOrderLineDTO.TaxAmount), purchaseOrderLineDTO.ProductId);
                log.LogVariableState("PurchaseTaxLineDTOList", purchaseTaxLineDTOList);
                purchaseOrderTaxLineDTOList.AddRange(purchaseTaxLineDTOList);
            }
            log.LogVariableState("PurchaseOrderTaxLineDTOList", purchaseOrderTaxLineDTOList);
            log.LogMethodExit(purchaseOrderTaxLineDTOList);
            return purchaseOrderTaxLineDTOList;
           
        }
        /// <summary>
        /// Creates multiple Tax Lines based on the Purchase tax Id 
        /// </summary>
        /// <param name="purchaseTaxId"></param>
        /// <param name="poNumber"></param>
        /// <param name="poLineNumber"></param>
        /// <param name="taxAmount"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        private List<PurchaseOrderTaxLineDTO> CalculateTaxes(int purchaseTaxId, int poId, int poLineId, decimal taxAmount, int productId)
        {
            log.LogMethodEntry(purchaseTaxId, poId, poLineId, taxAmount, productId);
            List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
            TaxList taxList = new TaxList(executionContext);
            List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>> SearchByTaxParameters = new List<KeyValuePair<TaxDTO.SearchByTaxParameters, string>>();
            SearchByTaxParameters.Add(new KeyValuePair<TaxDTO.SearchByTaxParameters, string>(TaxDTO.SearchByTaxParameters.TAX_ID, purchaseTaxId.ToString()));
            List<TaxDTO> taxDTOList = taxList.GetAllTaxes(SearchByTaxParameters);
            log.LogMethodExit("Tax List for the current Purchase Tax Id");
            log.LogVariableState("TaxDTOList", taxDTOList);
            if (taxDTOList != null && taxDTOList.Count > 0)
            {
                TaxDTO selectedTax = taxDTOList[0];
                //Tax Structure List Details
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
                            PurchaseOrderTaxLineDTO parentTaxLine = new PurchaseOrderTaxLineDTO();

                            if (currentTax.ParentStructureId > 0)
                            {
                                List<PurchaseOrderTaxLineDTO> parentTax = purchaseOrderTaxLineDTOList.Where(x => x.TaxStructureId == currentTax.ParentStructureId).ToList();
                                if (parentTax != null && parentTax.Count > 0)
                                {
                                    parentTaxLine = parentTax[0];
                                }
                            }
                            Decimal parentTaxAmount = parentTaxLine.TaxAmount > 0 ? parentTaxLine.TaxAmount : taxAmount;
                            decimal poTaxAmount = ((parentTaxAmount) * (Convert.ToDecimal(currentTax.Percentage.ToString())) / Convert.ToDecimal(selectedTax.TaxPercentage.ToString()));
                            PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO = new PurchaseOrderTaxLineDTO(-1, poId, poLineId, purchaseTaxId, selectedTax.TaxName,
                                                                              currentTax.TaxStructureId, currentTax.StructureName, productId, Convert.ToDecimal(currentTax.Percentage.ToString()),
                                                                              poTaxAmount, true, string.Empty, DateTime.MinValue, string.Empty, DateTime.MinValue, string.Empty, false, -1, -1);
                            purchaseOrderTaxLineDTO.IsChanged = true;
                            log.LogVariableState("PurchaseOrderTaxLineDTO", purchaseOrderTaxLineDTO);
                            log.LogMethodExit("Tax with multiple tax Structure : Multiple Tax lines to  PurchaseOrderTaxLine  ");
                            purchaseOrderTaxLineDTOList.Add(purchaseOrderTaxLineDTO);
                            selectedTaxStructure.RemoveAt(0);
                            processedTax.Add(currentTax.TaxStructureId, 0);
                            log.LogVariableState("PurchaseOrderTaxLineDTOList", purchaseOrderTaxLineDTOList);
                        }
                    }
                }
                else
                {
                    // Single tax
                    PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO = new PurchaseOrderTaxLineDTO(-1, poId, poLineId, selectedTax.TaxId, selectedTax.TaxName, -1, string.Empty, productId,
                                                                      Convert.ToDecimal(selectedTax.TaxPercentage.ToString()), taxAmount, true, string.Empty, DateTime.MinValue, string.Empty,
                                                                      DateTime.MinValue, string.Empty, false, -1, -1);
                    purchaseOrderTaxLineDTO.IsChanged = true;
                    log.LogMethodExit("Single Tax - Tax with no Structure : One Tax line to  PurchaseOrderTaxLine  ");
                    log.LogVariableState("PurchaseOrderTaxLineDTO", purchaseOrderTaxLineDTO);
                    purchaseOrderTaxLineDTOList.Add(purchaseOrderTaxLineDTO);
                }
            }
            log.LogMethodExit(purchaseOrderTaxLineDTOList);
            return purchaseOrderTaxLineDTOList;
        }
        /// <summary>
        /// This function is called from frmOrder for Tax PopUp display.
        /// </summary>
        /// <param name="purchaseTaxId"></param>
        /// <param name="poId"></param>
        /// <param name="poLineId"></param>
        /// <param name="taxAmount"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<PurchaseOrderTaxLineDTO> GetTaxLines(int purchaseTaxId, int poId, int poLineId, decimal taxAmount, int productId)
        {
            log.LogMethodEntry(purchaseTaxId, poId, poLineId, taxAmount, productId);
            List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
            if (purchaseTaxId <= 0)
            {
                PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO = new PurchaseOrderTaxLineDTO(-1, poId, poLineId, -1, string.Empty,
                                                                  -1, string.Empty, productId, 0, taxAmount, true, string.Empty,
                                                                  DateTime.MinValue, string.Empty, DateTime.MinValue, string.Empty,
                                                                  false, -1, -1);

                purchaseOrderTaxLineDTOList.Add(purchaseOrderTaxLineDTO);
                
            }
            else
            {
                purchaseOrderTaxLineDTOList = CalculateTaxes(purchaseTaxId, poId, poLineId, taxAmount, productId);
                if (purchaseOrderTaxLineDTOList == null)
                {
                    purchaseOrderTaxLineDTOList = new List<PurchaseOrderTaxLineDTO>();
                }
            }
            log.LogMethodExit(purchaseOrderTaxLineDTOList);
            return purchaseOrderTaxLineDTOList;
        }

    }

    /// <summary>
    /// Manages the list of Purchase Order Tax Line 
    /// </summary>
    public class PurchaseOrderTaxLineListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList;
        /// <summary>
        /// Parameterized  constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public PurchaseOrderTaxLineListBL(ExecutionContext executionContext)
        {
            this.executionContext = executionContext;
        }

        public PurchaseOrderTaxLineListBL(ExecutionContext executionContext, List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList)
            : this(executionContext)
        {
            this.purchaseOrderTaxLineDTOList = purchaseOrderTaxLineDTOList;
        }
        public List<PurchaseOrderTaxLineDTO> GetPurchaseOrderTaxLines(List<KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>> searchParameters, SqlTransaction SQLTrx = null)
        {
            log.LogMethodEntry(searchParameters, SQLTrx);
            PurchaseOrderTaxLineDataHandler purchaseOrderTaxLineDataHandler = new PurchaseOrderTaxLineDataHandler(SQLTrx);
            this.purchaseOrderTaxLineDTOList = purchaseOrderTaxLineDataHandler.GetPurchaseOrderTaxLines(searchParameters);
            log.LogMethodExit(purchaseOrderTaxLineDTOList);
            return purchaseOrderTaxLineDTOList;
        }

        /// <summary>
        /// Calls the save method for every PurchaseOrderTaxLine
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction SQLTrx)
        {
            log.LogMethodEntry(SQLTrx);
            if (purchaseOrderTaxLineDTOList != null && purchaseOrderTaxLineDTOList.Count > 0)
            {
                foreach (PurchaseOrderTaxLineDTO purchaseOrderTaxLineDTO in purchaseOrderTaxLineDTOList)
                {
                    PurchaseOrderTaxLineBL purchaseOrderTaxLineBL = new PurchaseOrderTaxLineBL(purchaseOrderTaxLineDTO, this.executionContext);
                    purchaseOrderTaxLineBL.Save(SQLTrx);
                }
            }
            log.LogMethodExit();
        }

        internal List<PurchaseOrderTaxLineDTO> GetTotalPOTaxView(int purchaseOrderId, int purchaseOrderLineId, bool totalTaxView = false)
        {
            log.LogMethodEntry(purchaseOrderId);

            PurchaseOrderTaxLineBL purchaseOrderTaxLineBL = new PurchaseOrderTaxLineBL(executionContext);
            PurchaseOrder purchaseOrder = new PurchaseOrder(purchaseOrderId, executionContext, null, true, true );
            List<PurchaseOrderTaxLineDTO> purchaseOrderTaxLineDTOList = null;
            if (totalTaxView == false && purchaseOrderLineId < 0)
            {
                string errorMessage = MessageContainerList.GetMessage(executionContext, "purchaseOrderLineId is mandatory for single tax view in PO");
                throw new ValidationException(errorMessage);
            }
            PurchaseOrderTaxLineListBL PurchaseOrderTaxLineListBL = new PurchaseOrderTaxLineListBL(executionContext);
            if (totalTaxView)
            {
                List<KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>> searchParameters = new List<KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>>();
                searchParameters.Add(new KeyValuePair<PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters, string>(PurchaseOrderTaxLineDTO.SearchByPurchaseOrderTaxLineParameters.PO_ID, purchaseOrder.getPurchaseOrderDTO.PurchaseOrderId.ToString()));
                purchaseOrderTaxLineDTOList = PurchaseOrderTaxLineListBL.GetPurchaseOrderTaxLines(searchParameters, null);
            }
            else
            {
                PurchaseOrderLineDTO purchaseOrderLineDTO = purchaseOrder.getPurchaseOrderDTO.PurchaseOrderLineListDTO.Where(x => x.PurchaseOrderLineId == purchaseOrderLineId && x.PurchaseOrderId == purchaseOrderId).FirstOrDefault();
                purchaseOrderTaxLineDTOList = purchaseOrderTaxLineBL.GetTaxLines(purchaseOrderLineDTO.PurchaseTaxId, purchaseOrderId, purchaseOrderLineId, Convert.ToDecimal(purchaseOrderLineDTO.TaxAmount), purchaseOrderLineDTO.ProductId);
            }
            log.LogMethodExit(purchaseOrderTaxLineDTOList);
            return purchaseOrderTaxLineDTOList;
        }
    }
}

