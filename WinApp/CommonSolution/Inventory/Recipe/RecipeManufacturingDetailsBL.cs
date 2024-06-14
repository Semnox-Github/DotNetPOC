/********************************************************************************************
 * Project Name - Inventory
 * Description  - BL logic of RecipeManufacturingDetails
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0        24-Jul-2020   Deeksha             Created for Recipe Management enhancement.
  2.120.00     05-May-2021     Mushahid Faizan      Web Inventory UI Changes
  2.130.0      13-Jun-2021         Mushahid Faizan       Modified : Web Inventory UI Changes.
 *********************************************************************************************/
using System;
using System.Linq;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using Semnox.Core.GenericUtilities.Excel;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// Recipe Manufacturing Details BL
    /// </summary>
    public class RecipeManufacturingDetailsBL
    {
        private RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of RecipeManufacturingDetailsBL class
        /// </summary>
        private RecipeManufacturingDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates RecipeManufacturingDetailsBL object using the RecipeManufacturingDetailsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="recipeManufacturingDetailsDTO">RecipeManufacturingDetailsDTO DTO object</param>
        public RecipeManufacturingDetailsBL(ExecutionContext executionContext,
                                             RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeManufacturingDetailsDTO);
            this.recipeManufacturingDetailsDTO = recipeManufacturingDetailsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RecipeManufacturingDetails  id as the parameter
        /// Would fetch the RecipeManufacturingDetails object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="recipeEstimationDetailId">id -PromotionRule </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public RecipeManufacturingDetailsBL(ExecutionContext executionContext, int recipeEstimationDetailId,
                                            SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeEstimationDetailId, sqlTransaction);
            RecipeManufacturingDetailsDataHandler recipeManufacturingDetailsDataHandler = new RecipeManufacturingDetailsDataHandler(sqlTransaction);
            recipeManufacturingDetailsDTO = recipeManufacturingDetailsDataHandler.GetRecipeManufacturingDetailsId(recipeEstimationDetailId);
            if (recipeManufacturingDetailsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "RecipeManufacturingDetailsDTO", recipeEstimationDetailId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RecipeManufacturingDetails DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeManufacturingDetailsDTO.IsChanged == false
                && recipeManufacturingDetailsDTO.RecipeManufacturingDetailId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            RecipeManufacturingDetailsDataHandler RecipeManufacturingDetailsDataHandler = new RecipeManufacturingDetailsDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (recipeManufacturingDetailsDTO.RecipeManufacturingDetailId < 0)
            {
                recipeManufacturingDetailsDTO = RecipeManufacturingDetailsDataHandler.Insert(recipeManufacturingDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipeManufacturingDetailsDTO.AcceptChanges();
            }
            else if (recipeManufacturingDetailsDTO.IsChanged)
            {
                recipeManufacturingDetailsDTO = RecipeManufacturingDetailsDataHandler.Update(recipeManufacturingDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipeManufacturingDetailsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the RecipeManufacturingDetailsDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (recipeManufacturingDetailsDTO == null)
            {
                errorMessage = MessageContainerList.GetMessage(executionContext, 2248, MessageContainerList.GetMessage(executionContext, "recipeManufacturingDetailsDTO"));//"Recipe Manufacturing Details are missing"
                validationErrorList.Add(new ValidationError(MessageContainerList.GetMessage(executionContext, "Recipe Manufacturing Details"), MessageContainerList.GetMessage(executionContext, "DTO"), errorMessage));
            }

            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RecipeManufacturingDetailsDTO RecipeManufacturingDetailsDTO
        {
            get
            {
                return recipeManufacturingDetailsDTO;
            }
        }
    }

    public class RecipeManufacturingDetailsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RecipeManufacturingDetailsDTO> recipeManufacturingDetailsDTOList = new List<RecipeManufacturingDetailsDTO>();
        private List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = new List<RecipeManufacturingHeaderDTO>();
        /// <summary>
        ///  Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public RecipeManufacturingDetailsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="promotionRuleDTOList">RecipeManufacturingDetails DTO List as parameter </param>
        public RecipeManufacturingDetailsListBL(ExecutionContext executionContext,
                                                    List<RecipeManufacturingDetailsDTO> recipeManufacturingDetailsDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeManufacturingDetailsDTOList);
            this.recipeManufacturingDetailsDTOList = recipeManufacturingDetailsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the RecipeManufacturingDetails DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of PromotionRuleDTO </returns>
        public List<RecipeManufacturingDetailsDTO> GetRecipeManufacturingDetailsDTOList(List<KeyValuePair<RecipeManufacturingDetailsDTO.SearchByParameters, string>> searchParameters,
                                                              SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RecipeManufacturingDetailsDataHandler RecipeManufacturingDetailsDataHandler = new RecipeManufacturingDetailsDataHandler(sqlTransaction);
            List<RecipeManufacturingDetailsDTO> RecipeManufacturingDetailsDTOList = RecipeManufacturingDetailsDataHandler.GetRecipeManufacturingDetailsDTOList(searchParameters);
            log.LogMethodExit(RecipeManufacturingDetailsDTOList);
            return RecipeManufacturingDetailsDTOList;
        }

        /// <summary>
        /// Gets the RecipeManufacturingDetailsDTO List for recipeManufacturingHeaderIdList
        /// </summary>
        /// <param name="recipeManufacturingHeaderIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of RecipeManufacturingDetailsDTO</returns>
        public List<RecipeManufacturingDetailsDTO> GetRecipeManufacturingHeaderDTOListOfRecipe(List<int> recipeManufacturingHeaderIdList,
                                                         bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(recipeManufacturingHeaderIdList, activeRecords, sqlTransaction);
            RecipeManufacturingDetailsDataHandler recipeManufacturingDetailsDataHandler = new RecipeManufacturingDetailsDataHandler(sqlTransaction);
            List<RecipeManufacturingDetailsDTO> recipeManufacturingDetailsDTOList = recipeManufacturingDetailsDataHandler.GetRecipeManufacturingDetailsDTOListOfRecipe(recipeManufacturingHeaderIdList, activeRecords);
            log.LogMethodExit(recipeManufacturingDetailsDTOList);
            return recipeManufacturingDetailsDTOList;
        }

        /// <summary>
        /// Saves the  list of RecipeManufacturingDetailsDTOList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeManufacturingDetailsDTOList == null ||
                recipeManufacturingDetailsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < recipeManufacturingDetailsDTOList.Count; i++)
            {
                RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO = recipeManufacturingDetailsDTOList[i];
                if (recipeManufacturingDetailsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RecipeManufacturingDetailsBL recipeManufacturingDetailsBL = new RecipeManufacturingDetailsBL(executionContext, recipeManufacturingDetailsDTO);
                    recipeManufacturingDetailsBL.Save(sqlTransaction);
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving RecipeManufacturingDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("RecipeManufacturingDetailsDTO", recipeManufacturingDetailsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to update Inventory Stock
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void UpdateInventoryStock(DateTime mfgDate, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<RecipeManufacturingDetailsDTO> isCompletedRecipeManufacturingDetailsDTOList = new List<RecipeManufacturingDetailsDTO>();
            isCompletedRecipeManufacturingDetailsDTOList = recipeManufacturingDetailsDTOList.FindAll(x => x.IsComplete == false).ToList();
            if (isCompletedRecipeManufacturingDetailsDTOList != null && isCompletedRecipeManufacturingDetailsDTOList.Any())
            {
                List<RecipeManufacturingDetailsDTO> parentDTOList = isCompletedRecipeManufacturingDetailsDTOList.Where(x => x.IsParentItem == true).ToList();
                List<RecipeManufacturingDetailsDTO> childDTOList = isCompletedRecipeManufacturingDetailsDTOList.Where(x => x.IsParentItem == false).ToList();
                List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("INVENTORY_ADJUSTMENT_TYPE", executionContext.GetSiteId());
                LookupValuesDTO lookupValuesDTO = lookupValuesDTOList.Find(x => x.LookupValue == "Recipe Production");

                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime currentDate = serverTimeObject.GetServerDateTime();

                foreach (RecipeManufacturingDetailsDTO recipeManufacturingDTO in isCompletedRecipeManufacturingDetailsDTOList)
                {
                    if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                    {
                        ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipeManufacturingDTO.ProductId);
                        InventoryDTO inventoryDTO = new InventoryDTO();
                        List<InventoryDTO> inventoryDTOList = null;
                        Inventory inventory = null;
                        InventoryList inventoryList = new InventoryList();
                        inventoryDTOList = inventoryList.GetProductStockDetails(recipeManufacturingDTO.ProductId);
                        double adjQty = 0;
                        double updatedInventoryQty = 0;
                        double actualRecipeQty = 0;
                        int lotId = -1;
                        if (inventoryDTOList == null)
                        {
                            inventoryDTOList = new List<InventoryDTO>();
                            inventoryDTOList.Add(inventoryDTO);
                        }
                        int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == recipeManufacturingDTO.ProductId).InventoryUOMId;
                        actualRecipeQty = Convert.ToDouble(recipeManufacturingDTO.ActualMfgQuantity);
                        if (inventoryUOMId != recipeManufacturingDTO.ActualMfgUOMId)
                        {
                            double factor = UOMContainer.GetConversionFactor(recipeManufacturingDTO.ActualMfgUOMId, inventoryUOMId);
                            actualRecipeQty = Convert.ToDouble(recipeManufacturingDTO.ActualMfgQuantity) * factor;
                        }

                        if (productDTO.LotControlled && parentDTOList.Contains(recipeManufacturingDTO))
                        {
                            DateTime expiryDate = mfgDate;
                            if (productDTO.IssuingApproach == "FEFO" || productDTO.IssuingApproach == "FIFO")
                            {
                                if (productDTO.ExpiryType == "E" || productDTO.ExpiryType == "N")
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2869));
                                    //Lot expiry type cannot be set as expiryDate for manufacturing products. Please update the product setup
                                }
                                else if (productDTO.ExpiryType == "D")
                                {
                                    expiryDate = expiryDate.AddDays(productDTO.ExpiryDays);
                                }
                            }
                            InventoryLotDTO inventoryLotDTO = new InventoryLotDTO(-1, null, actualRecipeQty, actualRecipeQty, 0, -1, expiryDate, true, null,
                                                                    inventoryUOMId);
                            InventoryLotBL inventoryLot = new InventoryLotBL(inventoryLotDTO, executionContext);
                            inventoryLot.Save(sqlTransaction);
                            inventoryDTO.LotId = inventoryLot.GetInventoryLot.LotId;
                            updatedInventoryQty = actualRecipeQty;
                            adjQty = actualRecipeQty;
                        }
                        else if (parentDTOList.Contains(recipeManufacturingDTO))
                        {
                            inventoryDTO = inventoryDTOList[0];
                            updatedInventoryQty = inventoryDTO.Quantity + actualRecipeQty;
                            adjQty = actualRecipeQty;
                        }
                        else if (!productDTO.LotControlled)
                        {
                            inventoryDTO = inventoryDTOList[0];

                            updatedInventoryQty = inventoryDTO.Quantity - actualRecipeQty;
                            if (updatedInventoryQty < 0)
                            {
                                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2762, productDTO.Description)); //There is no sufficient quantity for &1 to prepare a recipe Please update the stock
                            }
                            adjQty = -actualRecipeQty;
                        }

                        if (childDTOList.Contains(recipeManufacturingDTO) && productDTO.LotControlled)
                        {
                            adjQty = DeductInventoryQtyWithLotInfo(recipeManufacturingDTO, inventoryDTOList, actualRecipeQty, sqlTransaction);
                        }
                        else
                        {
                            inventoryDTO.ProductId = recipeManufacturingDTO.ProductId;
                            inventoryDTO.LocationId = productDTO.OutboundLocationId;
                            inventoryDTO.Quantity = updatedInventoryQty;
                            inventoryDTO.UOMId = inventoryUOMId;
                            inventory = new Inventory(inventoryDTO, executionContext);
                            inventory.Save(sqlTransaction);
                            lotId = inventory.GetInventoryDTO.LotId;
                        }
                        InventoryAdjustmentsDTO inventoryAdjustmentsDTO = new InventoryAdjustmentsDTO(-1, lookupValuesDTO.LookupValue, adjQty, inventoryDTO.LocationId, -1,
                        "Manufactured Item", recipeManufacturingDTO.ProductId, currentDate, executionContext.GetUserId(), null, lookupValuesDTO.LookupValueId,
                        lotId, 0, -1, false, -1, inventoryUOMId, -1);
                        InventoryAdjustmentsBL inventoryAdjustmentsBL = new InventoryAdjustmentsBL(inventoryAdjustmentsDTO, executionContext);
                        inventoryAdjustmentsBL.Save(sqlTransaction);
                    }
                    recipeManufacturingDTO.IsComplete = true;
                }
                RecipeManufacturingDetailsListBL recipeManufacturingDetailsListBL = new RecipeManufacturingDetailsListBL(executionContext , isCompletedRecipeManufacturingDetailsDTOList);
                recipeManufacturingDetailsListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to deduct BOM from the Lot
        /// </summary>
        /// <param name="recipeDTO"></param>
        /// <param name="inventoryDTOList"></param>
        /// <param name="actualRecipeQty"></param>
        /// <param name="sqlTransaction"></param>
        private double DeductInventoryQtyWithLotInfo(RecipeManufacturingDetailsDTO recipeDTO, List<InventoryDTO> inventoryDTOList,
                                                        double actualRecipeQty, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(recipeDTO, inventoryDTOList, sqlTransaction);
            ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipeDTO.ProductId);
            Inventory inventory = new Inventory(executionContext);
            List<InventoryLotDTO> inventoryLotDTOList = new List<InventoryLotDTO>();
            InventoryLotList inventoryLot = new InventoryLotList(executionContext);
            string lotIdList = string.Join(",", inventoryDTOList.Select(x => x.LotId));
            List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>> searchParams = new List<KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>>();
            searchParams.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.LOT_ID_LIST, lotIdList));
            searchParams.Add(new KeyValuePair<InventoryLotDTO.SearchByInventoryLotParameters, string>(InventoryLotDTO.SearchByInventoryLotParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            inventoryLotDTOList = inventoryLot.GetAllInventoryLot(searchParams);
            if (productDTO.IssuingApproach == "FIFO")
            {
                inventoryLotDTOList = inventoryLotDTOList.OrderBy(x => x.LotId).ToList();
            }
            else if (productDTO.IssuingApproach == "FEFO")
            {
                inventoryLotDTOList = inventoryLotDTOList.OrderBy(x => x.Expirydate).ToList();
            }
            double recipeQty = 0;
            double remainingQty = 0;
            foreach (InventoryLotDTO lotDTO in inventoryLotDTOList)
            {
                double currentLotQty = lotDTO.BalanceQuantity;
                if (currentLotQty == 0)
                {
                    if (recipeQty == 0)
                    {
                        remainingQty = actualRecipeQty;
                    }
                    else
                    {
                        remainingQty = recipeQty;
                    }
                    continue;
                }
                if (recipeQty == 0)
                {
                    recipeQty = actualRecipeQty;
                }
                if ((productDTO.IssuingApproach == "FEFO" &&
                    lotDTO.Expirydate != null && lotDTO.Expirydate < CommonFuncs.Utilities.getServerTime())
                    || lotDTO.LotId == -1)
                {
                    continue;
                }
                if (currentLotQty < recipeQty)
                {
                    lotDTO.BalanceQuantity = 0;
                    remainingQty = recipeQty - currentLotQty;
                    recipeQty = remainingQty;
                }
                else if (currentLotQty >= recipeQty)
                {
                    lotDTO.BalanceQuantity = currentLotQty - recipeQty;
                    remainingQty = recipeQty - currentLotQty;
                }

                InventoryLotBL inventoryLotBL = new InventoryLotBL(lotDTO, executionContext);
                inventoryLotBL.Save(sqlTransaction);
                InventoryList inventoryBL = new InventoryList(executionContext);
                List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>> searchparams = new List<KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>>();
                searchparams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.LOT_ID, lotDTO.LotId.ToString()));
                searchparams.Add(new KeyValuePair<InventoryDTO.SearchByInventoryParameters, string>(InventoryDTO.SearchByInventoryParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<InventoryDTO> inventoryDTOListofLots = inventoryBL.GetAllInventory(searchparams);
                if (inventoryDTOListofLots != null && inventoryDTOListofLots.Count > 0)
                {
                    inventoryDTOListofLots[0].Quantity = lotDTO.BalanceQuantity;
                }
                inventory = new Inventory(inventoryDTOListofLots[0], executionContext);
                inventory.Save(sqlTransaction);
                if (remainingQty <= 0)
                {
                    break;
                }
            }
            if (remainingQty > 0)
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2762, productDTO.Description)); //There is no sufficient quantity for &1 to prepare a recipe Please update the stock
            }
            //InventoryLotList inventoryLotList = new InventoryLotList(executionContext, inventoryLotDTOList);
            //inventoryLotList.Save(sqlTransaction);
            log.LogMethodExit(-actualRecipeQty);
            return -actualRecipeQty;
        }


        /// <summary>
        /// Method to Create MFG Line details
        /// </summary>
        /// <returns></returns>
        public List<RecipeManufacturingDetailsDTO> SetLineDetails()
        {
            log.LogMethodEntry();
            int mfgLineId = 1;
            if (recipeManufacturingDetailsDTOList != null && recipeManufacturingDetailsDTOList.Any())
            {
                mfgLineId = recipeManufacturingDetailsDTOList.Max(x => x.MfgLineId);
                if (mfgLineId == -1)
                {
                    mfgLineId = 1;
                }
                else
                {
                    mfgLineId++;
                }
            }
            int parentLineId = -1;
            for (int i = 0; i < recipeManufacturingDetailsDTOList.Count; i++)
            {
                if (recipeManufacturingDetailsDTOList[i].RecipeManufacturingDetailId == -1)
                {
                    if (recipeManufacturingDetailsDTOList[i].IsParentItem == true)
                    {
                        recipeManufacturingDetailsDTOList[i].MfgLineId = mfgLineId;
                        recipeManufacturingDetailsDTOList[i].TopMostParentMFGLineId = -1;
                        recipeManufacturingDetailsDTOList[i].ParentMFGLineId = -1;
                        parentLineId = mfgLineId;
                    }
                    else
                    {
                        recipeManufacturingDetailsDTOList[i].MfgLineId = mfgLineId;
                        recipeManufacturingDetailsDTOList[i].TopMostParentMFGLineId = parentLineId;
                        recipeManufacturingDetailsDTOList[i].ParentMFGLineId = parentLineId;
                    }
                    mfgLineId++;
                }
            }
            log.LogMethodExit(recipeManufacturingDetailsDTOList);
            return recipeManufacturingDetailsDTOList;
        }

        private string GetOffsetValue()
        {
            log.LogMethodEntry();
            int offset = 1;
            string sOffset = string.Empty;
            byte[] b = new byte[] { 20, 37 };
            sOffset = Encoding.Unicode.GetString(b);
            sOffset = sOffset.PadLeft(offset * 3 + 1, ' ') + " ";
            log.LogMethodExit(sOffset);
            return sOffset;
        }

        /// <summary>
        /// This method is will return Sheet object for RecipeManufacturingDetailsDTO.
        /// <returns></returns>
        public Sheet BuildTemplate(DateTime? fromDate = null, DateTime? toDate = null, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            Sheet sheet = new Sheet();
            ///All column Headings are in a headerRow object
            Row headerRow = new Row();
            ProductContainer productContainer = new ProductContainer(executionContext);
            UOMContainer uOMContainer = new UOMContainer(executionContext);
            if (ProductContainer.productDTOList == null || ProductContainer.productDTOList.Count <= 0 ||
                UOMContainer.uomDTOList == null || UOMContainer.uomDTOList.Count <= 0)
            {
                log.Debug("Container is empty");
                return null;
            }

            RecipeManufacturingDetailsDTODefination recipeManufacturingDetailsExcelDTODefinition = new RecipeManufacturingDetailsDTODefination(executionContext, "");
            ///Building headers from RecipeManufacturingDetailsDTODefination
            recipeManufacturingDetailsExcelDTODefinition.BuildHeaderRow(headerRow);
            sheet.AddRow(headerRow);



            //RecipeManufacturingDetailsDataHandler recipeManufacturingDetailsDataHandler = new RecipeManufacturingDetailsDataHandler(sqlTransaction);
            //List<KeyValuePair<RecipeManufacturingDetailsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipeManufacturingDetailsDTO.SearchByParameters, string>>();
            //searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_FROM_DATETIME, Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            //searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_TO_DATETIME, Convert.ToDateTime(toDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            //searchParameters.Add(new KeyValuePair<RecipeManufacturingDetailsDTO.SearchByParameters, string>(RecipeManufacturingDetailsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            //recipeManufacturingDetailsDTOList = recipeManufacturingDetailsDataHandler.GetRecipeManufacturingDetailsDTOList(searchParameters);

            RecipeManufacturingHeaderListBL recipeManufacturingHeaderListBL = new RecipeManufacturingHeaderListBL(executionContext);
            List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>>();
            if (fromDate != null && toDate != null)
            {
                searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_FROM_DATETIME, Convert.ToDateTime(fromDate).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_TO_DATETIME, Convert.ToDateTime(toDate).AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            }
            searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            recipeManufacturingHeaderDTOList = recipeManufacturingHeaderListBL.GetAllRecipeManufacturingHeaderDTOList(searchParameters, true, true);

            if (recipeManufacturingHeaderDTOList != null && recipeManufacturingHeaderDTOList.Any())
            {
                string sOffset = GetOffsetValue();
                recipeManufacturingHeaderDTOList = recipeManufacturingHeaderDTOList.OrderBy(x => x.MFGDateTime).ToList();
                foreach (RecipeManufacturingHeaderDTO headerDTO in recipeManufacturingHeaderDTOList)
                {
                    List<RecipeManufacturingDetailsDTO> detailsDTOList = new List<RecipeManufacturingDetailsDTO>();
                    detailsDTOList = headerDTO.RecipeManufacturingDetailsDTOList;
                    if (detailsDTOList != null && detailsDTOList.Count > 0)
                    {
                        for (int i = 0; i < detailsDTOList.Count; i++)
                        {
                            detailsDTOList[i].MFGDate = headerDTO.MFGDateTime.Date;
                        }
                
                        //detailsDTOList[0].MFGDate = headerDTO.MFGDateTime.Date;
                    }
                    foreach (RecipeManufacturingDetailsDTO recipeDetailsDTO in detailsDTOList)
                    {
                        if (ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                        {
                            decimal factor = 1;
                            int inventoryUOMId = ProductContainer.productDTOList.Find(x => x.ProductId == recipeDetailsDTO.ProductId).InventoryUOMId;
                            recipeDetailsDTO.ItemUOM = UOMContainer.uomDTOList.Find(x => x.UOMId == recipeDetailsDTO.MfgUOMId).UOM;
                            if (recipeDetailsDTO.ActualMfgUOMId != inventoryUOMId)
                            {
                                factor = Convert.ToDecimal(UOMContainer.GetConversionFactor(recipeDetailsDTO.ActualMfgUOMId, inventoryUOMId));
                            }
                            ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipeDetailsDTO.ProductId);
                            if (recipeDetailsDTO.IsParentItem == true)
                            {
                                recipeDetailsDTO.ItemName = productDTO.Description;
                            }
                            else
                            {

                                InventoryList inventoryList = new InventoryList(executionContext);
                                decimal stock = inventoryList.GetProductStockQuantity(recipeDetailsDTO.ProductId);
                                decimal actualQty = Convert.ToDecimal(recipeDetailsDTO.Quantity) * factor;
                                recipeDetailsDTO.ItemName = sOffset + productDTO.Description;
                            }
                            recipeDetailsDTO.ActualCost = Convert.ToDecimal(productDTO.Cost) * recipeDetailsDTO.ActualMfgQuantity * factor;
                            recipeDetailsDTO.ItemCost = Convert.ToDecimal(productDTO.Cost);
                            recipeDetailsDTO.PlannedCost = Convert.ToDecimal(productDTO.Cost) * recipeDetailsDTO.Quantity * factor;
                            recipeDetailsDTO.UOM = UOMContainer.uomDTOList.Find(x => x.UOMId == inventoryUOMId).UOM;
                        }
                    }
                    recipeManufacturingDetailsDTOList.AddRange(detailsDTOList);
                }
            }
            if (recipeManufacturingHeaderDTOList != null && recipeManufacturingHeaderDTOList.Any())
            {
                if (recipeManufacturingDetailsDTOList != null && recipeManufacturingDetailsDTOList.Any())
                {
                    foreach (RecipeManufacturingDetailsDTO recipeManufacturingDetailsDTO in recipeManufacturingDetailsDTOList)
                    {
                        recipeManufacturingDetailsExcelDTODefinition.Configure(recipeManufacturingDetailsDTO);

                        Row row = new Row();
                        recipeManufacturingDetailsExcelDTODefinition.Serialize(row, recipeManufacturingDetailsDTO);
                        sheet.AddRow(row);
                    }
                }
            }
            log.LogMethodExit();
            return sheet;
        }

    }
}
