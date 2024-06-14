/********************************************************************************************
 * Project Name - Inventory
 * Description  - BL  object of RecipeEstimationHeader
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.100.0       20-Jul-2020   Deeksha             Created for Recipe Management enhancement.
 2.120.00     05-May-2021        Mushahid Faizan      Web Inventory UI Changes
 2.130.0     15-Jun-2021        Mushahid Faizan      Web Inventory UI Changes
 *********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory.Recipe
{
    /// <summary>
    /// RecipeEstimationHeaderBL
    /// </summary>
    public class RecipeEstimationHeaderBL
    {
        private RecipeEstimationHeaderDTO recipeEstimationHeaderDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of RecipeEstimationHeaderBL class
        /// </summary>
        private RecipeEstimationHeaderBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// / Creates RecipeEstimationHeaderBL object using the recipeEstimationHeaderDTO
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="recipeEstimationHeaderDTO"></param>
        public RecipeEstimationHeaderBL(ExecutionContext executionContext, RecipeEstimationHeaderDTO recipeEstimationHeaderDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeEstimationHeaderDTO);
            this.recipeEstimationHeaderDTO = recipeEstimationHeaderDTO;
            log.LogMethodExit();
        }

        // <summary>
        /// Constructor with th RecipeEstimationHeader id as the parameter
        /// Would fetch the RecipeEstimationHeader object from the database based on the id passed.
        /// </summary>
        /// <param name = "RecipeEstimationHeaderId" > RecipeEstimationDetails id</param>
        public RecipeEstimationHeaderBL(ExecutionContext executionContext, int recipeEstimationHeaderId,
                                         bool loadChildRecords = false, bool activeChildRecords = false,
                                         SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, recipeEstimationHeaderId, loadChildRecords, activeChildRecords, sqlTransaction);
            RecipeEstimationHeaderDataHandler recipeEstimationHeaderDataHandler = new RecipeEstimationHeaderDataHandler(sqlTransaction);
            this.recipeEstimationHeaderDTO = recipeEstimationHeaderDataHandler.GetRecipeEstimationHeaderDTO(recipeEstimationHeaderId);
            if (loadChildRecords == false ||
                recipeEstimationHeaderDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            RecipeEstimationDetailsListBL recipeEstimationDetailsListBL = new RecipeEstimationDetailsListBL(executionContext);
            recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList = recipeEstimationDetailsListBL.GetRecipeEstimationHeaderDTOListOfRecipe(new List<int> { recipeEstimationHeaderId }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the RecipeEstimationHeader DTO
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            RecipeEstimationHeaderDataHandler recipeEstimationHeaderDataHandler = new RecipeEstimationHeaderDataHandler(sqlTransaction);
            if (recipeEstimationHeaderDTO.IsChanged == false
                 && recipeEstimationHeaderDTO.RecipeEstimationHeaderId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            if (recipeEstimationHeaderDTO.RecipeEstimationHeaderId < 0)
            {
                recipeEstimationHeaderDTO = recipeEstimationHeaderDataHandler.Insert(recipeEstimationHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                recipeEstimationHeaderDTO.AcceptChanges();
            }
            else
            {
                if (recipeEstimationHeaderDTO.IsChanged)
                {
                    recipeEstimationHeaderDTO = recipeEstimationHeaderDataHandler.Update(recipeEstimationHeaderDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    recipeEstimationHeaderDTO.AcceptChanges();
                }
            }
            if (recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList != null &&
                  recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList.Count != 0)
            {
                foreach (RecipeEstimationDetailsDTO estimationDetailsDTO in recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList)
                {
                    estimationDetailsDTO.RecipeEstimationHeaderId = recipeEstimationHeaderDTO.RecipeEstimationHeaderId;
                }
                RecipeEstimationDetailsListBL recipeEstimationDetailsBL = new RecipeEstimationDetailsListBL(executionContext, recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList);
                recipeEstimationDetailsBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Deletes the RecipeEstimationHeader DTO
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            RecipeEstimationHeaderDataHandler recipeEstimationHeaderDataHandler = new RecipeEstimationHeaderDataHandler(sqlTransaction);

            if (recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList != null &&
                  recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList.Count != 0)
            {
                RecipeEstimationDetailsListBL recipeEstimationDetailsBL = new RecipeEstimationDetailsListBL(executionContext, recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList);
                recipeEstimationDetailsBL.Delete(sqlTransaction);
            }
            if (recipeEstimationHeaderDTO.RecipeEstimationHeaderId > 0)
            {
                recipeEstimationHeaderDataHandler.Delete(recipeEstimationHeaderDTO.RecipeEstimationHeaderId);
                recipeEstimationHeaderDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Method to Purge Fore -casted Data
        /// </summary>
        /// <param name="purgeDataInDays"></param>
        /// <param name="sqlTransaction"></param>
        public void PurgeOldData(int purgeDataInDays, SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(purgeDataInDays, sqlTransaction);
            RecipeEstimationDetailsListBL recipeEstimationDetailsBL = new RecipeEstimationDetailsListBL(executionContext, recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList);
            recipeEstimationDetailsBL.PurgeOldData(purgeDataInDays, sqlTransaction);
            RecipeEstimationHeaderDataHandler recipeEstimationHeaderDataHandler = new RecipeEstimationHeaderDataHandler(sqlTransaction);
            recipeEstimationHeaderDataHandler.PurgeOldData(purgeDataInDays);
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the DTO
        /// </summary>
        public RecipeEstimationHeaderDTO RecipeEstimationHeaderDTO
        {
            get
            {
                return recipeEstimationHeaderDTO;
            }
        }

        /// <summary>
        /// Method to build the forecasting data based on the from and To date
        /// </summary>
        /// <param name="loadInventory"></param>
        /// <param name="sqlTransaction"></param>
        public List<RecipeEstimationHeaderDTO> BuildForecastData(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<RecipeEstimationHeaderDTO> recipeEstHeaderDTOList = null;
            ProductContainer productContainer = new ProductContainer(executionContext);
            if (ProductContainer.productDTOList == null && ProductContainer.productDTOList.Count <= 0)
            {
                log.LogMethodExit();
                return null;
            }
            List<ProductDTO> productDTOList = ProductContainer.productDTOList.Where(x => x.IsActive == true & x.IncludeInPlan == true).ToList();
            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            string currentDatetime = serverTimeObject.GetServerDateTime().ToString("yyyy-MM-dd");
            TimeSpan ts = new TimeSpan(6, 00, 0);
            currentDatetime = currentDatetime + " " + ts + " " + "AM";
            DateTime date = Convert.ToDateTime(currentDatetime);
            ForecastingTypePointListBL forecastingTypePointBL = new ForecastingTypePointListBL(executionContext);
            int calendarMasterId = -1;
            if (ForecastingTypePointListBL.accountingCalendarMasterDTOList != null)
            {
                AccountingCalendarMasterDTO calendarMasterDTO = ForecastingTypePointListBL.accountingCalendarMasterDTOList.Find(x => x.AccountingCalenderDate.Date == date.Date);
                if (calendarMasterDTO != null)
                {
                    calendarMasterId = calendarMasterDTO.AccountingCalendarMasterId;
                }
            }
            try
            {
                if (recipeEstimationHeaderDTO.FromDate == date)
                {
                    int loopCount = 1;
                    RecipeEstimationHeaderListBL recipeEstimationList = new RecipeEstimationHeaderListBL(executionContext);
                    List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchParams = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
                    searchParams.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = recipeEstimationList.GetAllRecipeEstimationHeaderDTOList(searchParams, false);
                    if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count == 0)
                    {
                        loopCount = 30;
                    }
                    DateTime recipeFromDate = recipeEstimationHeaderDTO.FromDate;
                    for (int j = 0; j < loopCount; j++)
                    {
                        DateTime fromDate = recipeFromDate.AddDays(-j);
                        RecipeEstimationHeaderDTO estimationHeaderDTO = new RecipeEstimationHeaderDTO(-1, fromDate, fromDate, null, null, false, 365, null, null, null, true);
                        List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = new List<RecipeEstimationDetailsDTO>();
                        List<ForecastingPeriodRatioDTO> forecastingTypePointsList = forecastingTypePointBL.GetPeriodDataPointsRatioAndTrx(recipeEstimationHeaderDTO.HistoricalDataInDays, fromDate);
                        for (int i = 0; i < productDTOList.Count; i++)
                        {
                            decimal totalEstimatedQty = 0;
                            ProductDTO productDTO = productDTOList[i];
                            totalEstimatedQty = GetForecastFor365Days(fromDate, forecastingTypePointsList, productDTO.ProductId);
                            log.LogVariableState("Total Estimated Qty for Product" + productDTO.Description + "=", totalEstimatedQty);
                            if (totalEstimatedQty <= 0)
                            {
                                continue;
                            }
                            RecipeEstimationDetailsDTO recipeEstimationDetailsDTO = new RecipeEstimationDetailsDTO(-1, -1, productDTO.ProductId, totalEstimatedQty,
                                null, totalEstimatedQty, null, null, productDTO.InventoryUOMId, calendarMasterId, true);
                            recipeEstimationDetailsDTOList.Add(recipeEstimationDetailsDTO);
                        }
                        estimationHeaderDTO.RecipeEstimationDetailsDTOList = recipeEstimationDetailsDTOList;
                        if (estimationHeaderDTO.RecipeEstimationDetailsDTOList.Count > 0)
                        {
                            recipeEstimationHeaderDTO = estimationHeaderDTO;
                            Save(sqlTransaction);
                        }
                    }
                }
                else
                {
                    List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
                    LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                    lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("PRODUCT_ITEM_TYPE", executionContext.GetSiteId());
                    int finishedTypeId = -1;
                    int semiFinishedTypeId = -1;
                    if (recipeEstimationHeaderDTO.IncludeFinishedItem == true)
                    {
                        finishedTypeId = lookupValuesDTOList.Find(x => x.LookupValue == "FINISHED_ITEM").LookupValueId;
                    }
                    if (recipeEstimationHeaderDTO.IncludeSemiFinishedItem == true)
                    {
                        semiFinishedTypeId = lookupValuesDTOList.Find(x => x.LookupValue == "SEMI_FINISHED_ITEM").LookupValueId;
                    }
                    productDTOList = productDTOList.FindAll(x => x.IsActive == true & x.IncludeInPlan == true & (x.ItemType == finishedTypeId) | (x.ItemType == semiFinishedTypeId));

                    int datediff = Convert.ToInt32(Math.Round((recipeEstimationHeaderDTO.ToDate - recipeEstimationHeaderDTO.FromDate).TotalDays));
                    if (datediff == 0)
                    {
                        datediff = 1;
                    }
                    datediff++;
                    List<RecipeEstimationDetailsDTO> recipeEventList = null;
                    RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext);
                    List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchEstimationParameter = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_FROM_DATE, recipeEstimationHeaderDTO.FromDate.AddDays(-recipeEstimationHeaderDTO.HistoricalDataInDays).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_TO_DATE, recipeEstimationHeaderDTO.FromDate.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(searchEstimationParameter, true, true);
                    recipeEstimationHeaderDTOList = recipeEstimationHeaderDTOList.Where(x => x.FromDate.Hour.ToString() == "6" & x.FromDate.Minute.ToString() == "0").ToList();
                    if (recipeEstimationHeaderDTOList != null && recipeEstimationHeaderDTOList.Count > 0)
                    {
                        recipeEstHeaderDTOList = new List<RecipeEstimationHeaderDTO>();
                        if (recipeEstimationHeaderDTO.ConsiderEventPromotions == true)
                        {
                            recipeEventList = GetEventProducts(sqlTransaction, productDTOList);

                        }
                        List<KeyValuePair<int, decimal>> productIdStock = new List<KeyValuePair<int, decimal>>();
                        InventoryList inventoryList = new InventoryList(executionContext);
                        List<int> productIdList = productDTOList.Select(x => x.ProductId).ToList();
                        productIdStock = inventoryList.GetProductStockQuantity(productIdList);

                        DateTime fromDate = recipeEstimationHeaderDTO.FromDate;
                        for (int i = 0; i < datediff; i++)
                        {
                            RecipeEstimationHeaderDTO headerDTO = null;

                            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = GetRecipePlanDetails(fromDate.AddDays(i));

                            calendarMasterId = ForecastingTypePointListBL.accountingCalendarMasterDTOList.Find(x => x.AccountingCalenderDate.ToString("yyyy-MM-dd") == fromDate.AddDays(i).ToString("yyyy-MM-dd")).AccountingCalendarMasterId;
                            List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchHeaderParameter = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
                            searchHeaderParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.FROM_DATE, fromDate.AddDays(i).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            searchHeaderParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.TO_DATE, fromDate.AddDays(i).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                            searchHeaderParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            List<RecipeEstimationHeaderDTO> recipeHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(searchHeaderParameter, true, true);
                            if (recipeHeaderDTOList == null || recipeHeaderDTOList.Count > 0 == false)
                            {
                                headerDTO = new RecipeEstimationHeaderDTO(-1, fromDate.AddDays(i), fromDate.AddDays(i), recipeEstimationHeaderDTO.AspirationalPercentage, recipeEstimationHeaderDTO.SeasonalPercentage,
                                                                         recipeEstimationHeaderDTO.ConsiderEventPromotions, recipeEstimationHeaderDTO.HistoricalDataInDays,
                                                                         recipeEstimationHeaderDTO.EventOffsetHrs, recipeEstimationHeaderDTO.IncludeFinishedItem, recipeEstimationHeaderDTO.IncludeSemiFinishedItem, recipeEstimationHeaderDTO.IsActive);
                            }
                            else
                            {
                                headerDTO = recipeHeaderDTOList[0];
                            }
                            for (int j = 0; j < productDTOList.Count; j++)
                            {
                                decimal estimatedQty = 0;
                                decimal? totalEstimatedQty = 0;
                                decimal? eventQty = 0;
                                decimal? planneddQty = 0;
                                ProductDTO productDTO = productDTOList[j];
                                estimatedQty = GetHistoricalEstimate(recipeEstimationHeaderDTOList, fromDate.AddDays(i), productDTO);
                                if (recipeEventList != null && recipeEventList.Count > 0 &&
                                    recipeEventList.Exists(x => x.ProductId == productDTO.ProductId & x.EventDate.Date == fromDate.AddDays(i).Date))
                                {
                                    eventQty = recipeEventList.Find(x => x.ProductId == productDTO.ProductId & x.EventDate.Date == fromDate.AddDays(i).Date).EventQty;
                                }
                                totalEstimatedQty = estimatedQty + eventQty;

                                if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Count > 0 &&
                                    recipePlanHeaderDTOList[0].RecipePlanDetailsDTOList.Exists(x => x.ProductId == productDTO.ProductId))
                                {
                                    planneddQty = recipePlanHeaderDTOList[0].RecipePlanDetailsDTOList.Find(x => x.ProductId == productDTO.ProductId).PlannedQty;
                                }
                                totalEstimatedQty = totalEstimatedQty * (1 + (recipeEstimationHeaderDTO.AspirationalPercentage / 100)) * (1 + (recipeEstimationHeaderDTO.SeasonalPercentage / 100));
                                totalEstimatedQty = Math.Round(Convert.ToDecimal(totalEstimatedQty));
                                if (totalEstimatedQty == 0)
                                {
                                    continue;
                                }
                                decimal stockqty = productIdStock.Find(x => x.Key == productDTO.ProductId).Value;
                                RecipeEstimationDetailsDTO recipeEstimationDetailsDTO = null;
                                recipeEstimationDetailsDTO = headerDTO.RecipeEstimationDetailsDTOList.Find(x => x.ProductId == productDTO.ProductId);
                                if (recipeEstimationDetailsDTO != null)
                                {
                                    recipeEstimationDetailsDTO.EstimatedQty = estimatedQty;
                                    recipeEstimationDetailsDTO.TotalEstimatedQty = totalEstimatedQty;
                                    recipeEstimationDetailsDTO.EventQty = eventQty;
                                    recipeEstimationDetailsDTO.StockQty = stockqty;
                                    recipeEstimationDetailsDTO.PlannedQty = planneddQty;
                                }
                                else
                                {
                                    recipeEstimationDetailsDTO = new RecipeEstimationDetailsDTO(-1, -1, productDTO.ProductId, estimatedQty,
                                                                                        eventQty, totalEstimatedQty, planneddQty, stockqty, productDTO.InventoryUOMId, calendarMasterId, true);
                                    headerDTO.RecipeEstimationDetailsDTOList.Add(recipeEstimationDetailsDTO);
                                }
                            }
                            recipeEstHeaderDTOList.Add(headerDTO);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(recipeEstHeaderDTOList);
            return recipeEstHeaderDTOList;
        }

        private decimal GetForecastFor365Days(DateTime fromDate, List<ForecastingPeriodRatioDTO> forecastingTypePointsList, int productId)
        {
            log.LogMethodEntry(fromDate, forecastingTypePointsList, productId);
            int trxCount = 0;
            decimal totalEstimatedQty = 0;
            List<LookupValuesDTO> lookupValuesDTOList = new List<LookupValuesDTO>();
            LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
            lookupValuesDTOList = lookupValuesList.GetInventoryLookupValuesByValueName("INVENTORY_TRANSACTION_TYPE", executionContext.GetSiteId());
            LookupValuesDTO lookupValuesDTO = lookupValuesDTOList.Find(x => x.LookupValue == "Sales");
            int lookupValueId = lookupValuesDTO.LookupValueId;

            string foreCastingType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FORECASTING_PERIOD_TYPE");
            InventoryTransactionList inventoryList = new InventoryTransactionList(executionContext);
            List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>> searchParameters = new List<KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>>();
            searchParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.TRANSACTION_FROM_DATE, fromDate.AddDays(-365).ToString("yyyy-MM-dd")));
            searchParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.TRANSACTION_TO_DATE, fromDate.ToString("yyyy-MM-dd")));
            searchParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.PRODUCT_ID, productId.ToString()));
            searchParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<InventoryTransactionDTO.SearchByInventoryTransactionParameters, string>(InventoryTransactionDTO.SearchByInventoryTransactionParameters.INVENTORY_TRANSACTION_TYPE_ID, lookupValueId.ToString()));
            List<InventoryTransactionDTO> inventoryTransactionDTOList = inventoryList.GetInventoryTransactionList(searchParameters);

            if (inventoryTransactionDTOList != null && inventoryTransactionDTOList.Count > 0)
            {
                foreach (ForecastingPeriodRatioDTO forecastingDTO in forecastingTypePointsList)
                {
                    List<InventoryTransactionDTO> trxList = inventoryTransactionDTOList.FindAll(x => x.TransactionDate.Date == forecastingDTO.AccountingCalendarMasterDTO.AccountingCalenderDate.Date);
                    if (trxList != null && trxList.Count > 0)
                    {
                        InventoryTransactionList inventoryTransactionList = new InventoryTransactionList(executionContext, trxList);
                        if (foreCastingType == "MULTIPERIODWEIGHTEDAVERAGE")
                        {
                            totalEstimatedQty += inventoryTransactionList.GetEstimatedQuantity() * Convert.ToDecimal(forecastingDTO.Ratio);
                        }
                        else
                        {
                            totalEstimatedQty += inventoryTransactionList.GetEstimatedQuantity();
                        }
                        trxCount++;
                    }
                }
                if (trxCount == 0)
                {
                    trxCount = 1;
                }
                totalEstimatedQty = Math.Ceiling(totalEstimatedQty / trxCount);
                totalEstimatedQty = Math.Round(totalEstimatedQty);
                log.LogVariableState("Total Estimated Qty for productId :", productId);
            }
            else
            {
                log.LogVariableState("Sale not found for productId :", productId);
            }
            log.LogMethodExit(totalEstimatedQty);
            return totalEstimatedQty;
        }


        /// <summary>
        /// Method to get the Event transactions
        /// </summary>
        /// <param name="sqlTransaction"></param>
        /// <param name="productDTOList"></param>
        /// <returns></returns>
        private List<RecipeEstimationDetailsDTO> GetEventProducts(SqlTransaction sqlTransaction, List<ProductDTO> productDTOList)
        {
            log.LogMethodEntry(sqlTransaction, productDTOList);
            RecipeEstimationDetailsListBL recipeEstimationDetailsListBL = new RecipeEstimationDetailsListBL(executionContext);
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = recipeEstimationDetailsListBL.GetEventProducts(recipeEstimationHeaderDTO.FromDate, recipeEstimationHeaderDTO.ToDate, sqlTransaction);
            if (recipeEstimationDetailsDTOList != null && recipeEstimationDetailsDTOList.Count > 0)
            {
                foreach (RecipeEstimationDetailsDTO dto in recipeEstimationDetailsDTOList)
                {
                    if (productDTOList.Exists(x => x.ProductId == dto.ProductId & x.IncludeInPlan == true))
                    {
                        dto.EventDate = dto.EventDate.AddHours(-Convert.ToDouble(recipeEstimationHeaderDTO.EventOffsetHrs));
                    }
                }
            }
            log.LogMethodExit(recipeEstimationDetailsDTOList);
            return recipeEstimationDetailsDTOList;
        }

        /// <summary>
        /// Method to get the estimated data for past 7,30 and 365 days
        /// </summary>
        /// <param name="recipeEstimationHeaderDTOList"></param>
        /// <param name="fromDate"></param>
        /// <param name="productDTO"></param>
        /// <returns></returns>
        private decimal GetHistoricalEstimate(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList, DateTime fromDate, ProductDTO productDTO)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTOList, fromDate, productDTO);
            decimal totalEstimatedQty = 0;
            try
            {
                string foreCastingType = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FORECASTING_PERIOD_TYPE");
                if (recipeEstimationHeaderDTOList.Exists(x => x.RecipeEstimationDetailsDTOList.Exists(y => y.ProductId == productDTO.ProductId)))
                {
                    int trxCount = 0;
                    List<AccountingCalendarMasterDTO> accountingcalendarList = ForecastingTypePointListBL.accountingCalendarMasterDTOList.FindAll(
                                                            x => x.AccountingCalenderDate > fromDate.AddDays(-recipeEstimationHeaderDTO.HistoricalDataInDays)
                                                            & x.AccountingCalenderDate <= fromDate).ToList();
                    if (accountingcalendarList != null && accountingcalendarList.Any())
                    {
                        AccountingCalendarMasterDTO dto = accountingcalendarList.Find(x => x.AccountingCalenderDate.Date == fromDate.Date);
                        if (dto != null)
                        {
                            dto = accountingcalendarList.Find(x => x.AccountingCalenderDate == dto.AccountingCalenderDate.AddDays(-7));
                            if (dto != null)
                            {
                                RecipeEstimationHeaderDTO dto7 = recipeEstimationHeaderDTOList.Find(x => x.FromDate.Date == dto.AccountingCalenderDate.Date);
                                if (dto7 != null)
                                {
                                    if (foreCastingType == "MULTIPERIODWEIGHTEDAVERAGE")
                                    {
                                        totalEstimatedQty = dto7.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productDTO.ProductId).Sum(x => x.TotalEstimatedQty).Value * Convert.ToDecimal(0.7);
                                        trxCount++;
                                    }
                                    else
                                    {
                                        totalEstimatedQty = dto7.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productDTO.ProductId).Sum(x => x.TotalEstimatedQty).Value;
                                        trxCount++;
                                    }
                                }
                            }
                        }
                        dto = accountingcalendarList.Find(x => x.AccountingCalenderDate.Date == fromDate.Date);
                        dto = accountingcalendarList.Where(x => x.DayWeek == dto.DayWeek
                                                            & x.AccountingCalenderDate.Date >= dto.AccountingCalenderDate.Date.AddDays(-32)
                                                            & x.AccountingCalenderDate.Date <= dto.AccountingCalenderDate.Date.AddDays(-27)
                                                            & ((x.Month == ((dto.Month - 1) <= 0 ? (12 - (1 - dto.Month)) : (dto.Month - 1)))
                                                            | (x.Month == (dto.Month))))
                                                            .OrderByDescending(x => x.AccountingCalenderDate.Date).ToList().FirstOrDefault();
                        if (dto != null)
                        {
                            RecipeEstimationHeaderDTO dto30 = recipeEstimationHeaderDTOList.Find(x => x.FromDate.Date == dto.AccountingCalenderDate.Date);
                            if (dto30 != null)
                            {
                                if (foreCastingType == "MULTIPERIODWEIGHTEDAVERAGE")
                                {
                                    totalEstimatedQty += dto30.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productDTO.ProductId).Sum(x => x.TotalEstimatedQty).Value * Convert.ToDecimal(0.2);
                                    trxCount++;
                                }
                                else
                                {
                                    totalEstimatedQty += dto30.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productDTO.ProductId).Sum(x => x.TotalEstimatedQty).Value;
                                    trxCount++;
                                }
                            }
                        }
                        dto = accountingcalendarList.Find(x => x.AccountingCalenderDate.Date == fromDate.Date);
                        dto = accountingcalendarList.Where(x => x.DayWeek == dto.DayWeek & x.Year != dto.Year
                                                            & x.Month == ((dto.Month - 12) <= 0 ? (12 - (12 - dto.Month)) : (dto.Month - 12))).OrderByDescending(x => x.AccountingCalenderDate).ToList().LastOrDefault();
                        if (dto != null)
                        {
                            RecipeEstimationHeaderDTO dto365 = recipeEstimationHeaderDTOList.Find(x => x.FromDate.Date == dto.AccountingCalenderDate.Date);
                            if (dto365 != null)
                            {
                                if (foreCastingType == "MULTIPERIODWEIGHTEDAVERAGE")
                                {
                                    totalEstimatedQty += dto365.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productDTO.ProductId).Sum(x => x.TotalEstimatedQty).Value * Convert.ToDecimal(0.1);
                                    trxCount++;
                                }
                                else
                                {
                                    totalEstimatedQty += dto365.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productDTO.ProductId).Sum(x => x.TotalEstimatedQty).Value;
                                    trxCount++;
                                }
                            }
                        }
                        if (trxCount == 0 && recipeEstimationHeaderDTO.HistoricalDataInDays > 6)
                        {
                            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                            DateTime currentDatetime = serverTimeObject.GetServerDateTime();
                            int? fromDateWeekDay = accountingcalendarList.Find(x => x.AccountingCalenderDate.Date == fromDate.Date).DayWeek;
                            List<AccountingCalendarMasterDTO> dtoList = accountingcalendarList.Where(x => x.AccountingCalenderDate.Date <= currentDatetime.Date
                                                                                                    & x.AccountingCalenderDate.Date >= currentDatetime.Date.AddDays(-7)).OrderByDescending(x => x.AccountingCalenderDate).ToList();
                            dto = dtoList.Find(x => x.DayWeek == fromDateWeekDay);
                            RecipeEstimationHeaderDTO dtoCurrentweekday = recipeEstimationHeaderDTOList.Find(x => x.FromDate.Date == dto.AccountingCalenderDate.Date);
                            if (dtoCurrentweekday != null)
                            {
                                if (foreCastingType == "MULTIPERIODWEIGHTEDAVERAGE")
                                {
                                    totalEstimatedQty = dtoCurrentweekday.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productDTO.ProductId).Sum(x => x.TotalEstimatedQty).Value * Convert.ToDecimal(0.7);
                                    trxCount++;
                                }
                                else
                                {
                                    totalEstimatedQty = dtoCurrentweekday.RecipeEstimationDetailsDTOList.FindAll(x => x.ProductId == productDTO.ProductId).Sum(x => x.TotalEstimatedQty).Value;
                                    trxCount++;
                                }
                            }
                            if (trxCount == 0)
                            {
                                trxCount = 1;
                            }
                        }
                        totalEstimatedQty = Math.Ceiling(totalEstimatedQty / trxCount);
                        totalEstimatedQty = Math.Round(totalEstimatedQty);
                    }
                }
                else
                {
                    ForecastingTypePointListBL forecastingTypePointBL = new ForecastingTypePointListBL(executionContext);
                    List<ForecastingPeriodRatioDTO> forecastingTypePointsList = forecastingTypePointBL.GetPeriodDataPointsRatioAndTrx(recipeEstimationHeaderDTO.HistoricalDataInDays, fromDate);
                    totalEstimatedQty = GetForecastFor365Days(fromDate, forecastingTypePointsList, productDTO.ProductId);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.LogMethodExit(totalEstimatedQty);
            return totalEstimatedQty;
        }

        /// <summary>
        /// Method to get the Recipe plan details
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        private List<RecipePlanHeaderDTO> GetRecipePlanDetails(DateTime fromDate)
        {
            log.LogMethodEntry(fromDate);
            RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext);
            List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchPlanParams = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
            searchPlanParams.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.FROM_DATE, fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            searchPlanParams.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.TO_DATE, fromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(searchPlanParams, true);
            log.LogMethodExit(recipePlanHeaderDTOList);
            return recipePlanHeaderDTOList;
        }


        public List<RecipeEstimationHeaderDTO> GetData(SqlTransaction sqlTransaction, string generateForecastData)
        {
            log.LogMethodEntry();
            List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();

            List<ValidationError> validationErrors = ValidateForecast(sqlTransaction);
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation Failed", validationErrors);
            }

            LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
            string currentDatetime = serverTimeObject.GetServerDateTime().ToString("yyyy-MM-dd");
            TimeSpan ts = new TimeSpan(6, 00, 0);
            currentDatetime = currentDatetime + " " + ts + " " + "AM";
            DateTime date = Convert.ToDateTime(currentDatetime);
            RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext);
            List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchEstimationParameter = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
            searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_FROM_DATE, date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.CURRENT_TO_DATE, date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
            searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            recipeEstimationHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(searchEstimationParameter, true, true);
            if (recipeEstimationHeaderDTOList == null || recipeEstimationHeaderDTOList.Any() == false)
            {
                // UI have to make two call -- if recipeEstimationHeaderDTOList is null then it will throw an exception
                // Display Alert message with the exception. If yes,
                // then UI have to make another call with generateForecastData= Y to build the historicalData.
                // else, UI have to make another call with generateForecastData= N to Get Forecast For Given Date Range.
                if (!string.IsNullOrEmpty(generateForecastData) && generateForecastData.ToUpper() == "Y")
                {
                    recipeEstimationHeaderDTOList = BuildHistoricalData(date);
                }
                else if (!string.IsNullOrEmpty(generateForecastData) && generateForecastData.ToUpper() == "N")
                {
                    recipeEstimationHeaderDTOList = GetForecastForGivenDateRange(date);
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2806));
                }
            }
            else
            {
                recipeEstimationHeaderDTOList = GetForecastForGivenDateRange(date);
            }

            log.LogMethodExit(recipeEstimationHeaderDTOList);
            return recipeEstimationHeaderDTOList;
        }

        private List<ValidationError> ValidateForecast(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            if (recipeEstimationHeaderDTO.ToDate.Date < recipeEstimationHeaderDTO.FromDate.Date)
            {
                log.Debug("To Date should be greater than From Date");
                validationErrorList.Add(new ValidationError("RecipeEstimationHeader", "Date", MessageContainerList.GetMessage(executionContext, 724, MessageContainerList.GetMessage(executionContext, "Date"))));
            }
            if ((recipeEstimationHeaderDTO.ToDate.Date - recipeEstimationHeaderDTO.FromDate.Date).TotalDays > 30)
            {
                log.Debug("Date range should not be more than 30 Days");
                recipeEstimationHeaderDTO.ToDate = recipeEstimationHeaderDTO.FromDate.Date.AddDays(30);
                validationErrorList.Add(new ValidationError("RecipeEstimationHeader", "Date", MessageContainerList.GetMessage(executionContext, 2789, MessageContainerList.GetMessage(executionContext, "Date"))));
            }
            if (recipeEstimationHeaderDTO.HistoricalDataInDays < 0 || recipeEstimationHeaderDTO.HistoricalDataInDays > 365)
            {
                log.Debug("Historical Days cannot be less than a year.");
                validationErrorList.Add(new ValidationError("RecipeEstimationHeader", "HistoricalDataInDays", MessageContainerList.GetMessage(executionContext, 2871, MessageContainerList.GetMessage(executionContext, "HistoricalDataInDays"))));
            }
            if (recipeEstimationHeaderDTO.AspirationalPercentage < -100 || recipeEstimationHeaderDTO.SeasonalPercentage > 100
                || recipeEstimationHeaderDTO.SeasonalPercentage < -100 || recipeEstimationHeaderDTO.AspirationalPercentage > 100)
            {
                log.Debug("Percentage Range should be between 100%. Please enter valid percentage");
                validationErrorList.Add(new ValidationError("RecipeEstimationHeader", "AspirationalPercentage", MessageContainerList.GetMessage(executionContext, 2855, MessageContainerList.GetMessage(executionContext, "AspirationalPercentage"))));
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        private List<RecipeEstimationHeaderDTO> BuildHistoricalData(DateTime date)
        {
            log.LogMethodEntry(date);
            List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();
                    RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = new RecipeEstimationHeaderDTO(-1, date, date,
                                                                                        0, 0, false, 365,
                                                                                       -1, null, null, true);
                    RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                    recipeEstimationHeaderDTOList = recipeEstimationHeaderBL.BuildForecastData(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();
                    recipeEstimationHeaderDTOList = GetForecastForGivenDateRange(date);
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                }
            }
            log.LogMethodExit(recipeEstimationHeaderDTOList);
            return recipeEstimationHeaderDTOList;
        }
        private List<RecipeEstimationHeaderDTO> GetForecastForGivenDateRange(DateTime date)
        {
            log.LogMethodEntry(date);
            List<ForecastingSummaryDTO> forecastingSummaryDTOList = new List<ForecastingSummaryDTO>();
            List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();

            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    parafaitDBTrx.BeginTransaction();

                    RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext);
                    DateTime fromdate = recipeEstimationHeaderDTO.FromDate.Date;
                    DateTime todate = recipeEstimationHeaderDTO.ToDate.Date;
                    List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchEstimationParameter = new List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>>();
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.FROM_DATE, fromdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.TO_DATE, todate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.DATE_NOT_IN_JOB_DATA, date.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                    searchEstimationParameter.Add(new KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>(RecipeEstimationHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    recipeEstimationHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(searchEstimationParameter, true, true);
                    if (recipeEstimationHeaderDTOList.Exists(x => x.FromDate.Hour == 6 & x.FromDate.Minute == 0))
                    {
                        recipeEstimationHeaderDTOList = recipeEstimationHeaderDTOList.FindAll(x => x.FromDate.Hour != 6).ToList();
                    }
                    else if (recipeEstimationHeaderDTOList == null || recipeEstimationHeaderDTOList.Count == 0)
                    {
                        RecipeEstimationHeaderDTO recipeEstimationHeader = new RecipeEstimationHeaderDTO(-1, fromdate, todate,
                                                                                                             recipeEstimationHeaderDTO.AspirationalPercentage,
                                                                                                             recipeEstimationHeaderDTO.SeasonalPercentage,
                                                                                                             recipeEstimationHeaderDTO.ConsiderEventPromotions, recipeEstimationHeaderDTO.HistoricalDataInDays,
                                                                                                            recipeEstimationHeaderDTO.EventOffsetHrs, recipeEstimationHeaderDTO.IncludeFinishedItem,
                                                                                                             recipeEstimationHeaderDTO.IncludeSemiFinishedItem, true);
                        RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeader);
                        recipeEstimationHeaderDTOList = recipeEstimationHeaderBL.BuildForecastData(parafaitDBTrx.SQLTrx);
                    }
                    parafaitDBTrx.EndTransaction();
                }
                catch (Exception ex)
                {
                    parafaitDBTrx.RollBack();
                    log.Error(ex);
                }
                log.LogMethodExit(recipeEstimationHeaderDTOList);
                return recipeEstimationHeaderDTOList;

            }
        }

    }


    public class RecipeEstimationHeaderListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();

        public RecipeEstimationHeaderListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="recipeEstimationHeaderDTOList">recipeEstimationHeaderDTOList</param>
        public RecipeEstimationHeaderListBL(ExecutionContext executionContext,
                                             List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.recipeEstimationHeaderDTOList = recipeEstimationHeaderDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the recipeEstimationHeaderDTO list
        /// </summary>
        public List<RecipeEstimationHeaderDTO> GetAllRecipeEstimationHeaderDTOList(List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchParameters,
                                       bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            RecipeEstimationHeaderDataHandler recipeEstimationHeaderDataHandler = new RecipeEstimationHeaderDataHandler(sqlTransaction);
            List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = recipeEstimationHeaderDataHandler.GetRecipeEstimationHeaderDTOList(searchParameters);
            if (loadChildRecords == false ||
                recipeEstimationHeaderDTOList == null ||
                recipeEstimationHeaderDTOList.Count > 0 == false)
            {
                log.LogMethodExit(recipeEstimationHeaderDTOList, "Child records are not loaded.");
                return recipeEstimationHeaderDTOList;
            }
            BuildRecipeEstimationHeaderDTOListt(recipeEstimationHeaderDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(recipeEstimationHeaderDTOList);
            return recipeEstimationHeaderDTOList;
        }

        private void BuildRecipeEstimationHeaderDTOListt(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList, bool activeChildRecords,
                                                    SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(recipeEstimationHeaderDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOIdMap = new Dictionary<int, RecipeEstimationHeaderDTO>();
            List<int> recipeEstimationHeaderIdList = new List<int>();
            for (int i = 0; i < recipeEstimationHeaderDTOList.Count; i++)
            {
                if (recipeEstimationHeaderDTOIdMap.ContainsKey(recipeEstimationHeaderDTOList[i].RecipeEstimationHeaderId))
                {
                    continue;
                }
                recipeEstimationHeaderDTOIdMap.Add(recipeEstimationHeaderDTOList[i].RecipeEstimationHeaderId, recipeEstimationHeaderDTOList[i]);
                recipeEstimationHeaderIdList.Add(recipeEstimationHeaderDTOList[i].RecipeEstimationHeaderId);
            }
            RecipeEstimationDetailsListBL recipeEstimationDetailsListBL = new RecipeEstimationDetailsListBL(executionContext);
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = recipeEstimationDetailsListBL.GetRecipeEstimationHeaderDTOListOfRecipe(recipeEstimationHeaderIdList, activeChildRecords, sqlTransaction);
            if (recipeEstimationDetailsDTOList != null && recipeEstimationDetailsDTOList.Count > 0)
            {
                for (int i = 0; i < recipeEstimationDetailsDTOList.Count; i++)
                {
                    if (recipeEstimationHeaderDTOIdMap.ContainsKey(recipeEstimationDetailsDTOList[i].RecipeEstimationHeaderId) == false)
                    {
                        continue;
                    }
                    RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = recipeEstimationHeaderDTOIdMap[recipeEstimationDetailsDTOList[i].RecipeEstimationHeaderId];
                    if (recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList == null)
                    {
                        recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList = new List<RecipeEstimationDetailsDTO>();
                    }
                    recipeEstimationHeaderDTO.RecipeEstimationDetailsDTOList.Add(recipeEstimationDetailsDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the RecipeEstimationHeaderDTO list
        /// </summary>
        public List<RecipeEstimationHeaderDTO> GetRecipeEstimationHeaderDTOList(List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> searchParameters,
                                                                       SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            RecipeEstimationHeaderDataHandler recipeEstimationHeaderDataHandler = new RecipeEstimationHeaderDataHandler(sqlTransaction);
            List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = recipeEstimationHeaderDataHandler.GetRecipeEstimationHeaderDTOList(searchParameters);
            log.LogMethodExit(recipeEstimationHeaderDTOList);
            return recipeEstimationHeaderDTOList;
        }

        /// <summary>
        /// Saves the  list of RecipeEstimationHeaderDTOList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeEstimationHeaderDTOList == null ||
                recipeEstimationHeaderDTOList.Count > 0 == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < recipeEstimationHeaderDTOList.Count; i++)
            {
                RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = recipeEstimationHeaderDTOList[i];
                if (recipeEstimationHeaderDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                    recipeEstimationHeaderBL.Save(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes the  list of RecipeEstimationHeaderDTOList DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (recipeEstimationHeaderDTOList == null ||
                recipeEstimationHeaderDTOList.Count > 0 == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < recipeEstimationHeaderDTOList.Count; i++)
            {
                RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = recipeEstimationHeaderDTOList[i];
                try
                {
                    RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                    recipeEstimationHeaderBL.Delete(sqlTransaction);
                }
                catch (SqlException ex)
                {
                    log.Error(ex);
                    if (ex.Number == 2601)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                    }
                    else if (ex.Number == 547)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                    }
                    else
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                    throw;
                }
            }
        }
    }
}
