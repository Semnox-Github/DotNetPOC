/********************************************************************************************
 * Project Name - Inventory 
 * Description  - LocalRecipeEstimationUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version      Date               Modified By           Remarks          
 *********************************************************************************************
  2.110.00     16-Nov-2020        Abhishek             Created : POS UI Redesign with REST API
  2.120.00     21-Apr-2021        Deeksha              Modified to add get for Build Forecast data
  2.120.00     05-May-2021        Mushahid Faizan      Web Inventory UI Changes
  2.130.0     15-Jun-2021        Mushahid Faizan      Web Inventory UI Changes
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class LocalRecipeEstimationUseCases : IRecipeEstimationUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalRecipeEstimationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<RecipeEstimationHeaderDTO>> GetRecipeEstimations(List<KeyValuePair<RecipeEstimationHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            return await Task<List<RecipeEstimationHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext);
                int siteId = GetSiteId();
                List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = recipeEstimationHeaderListBL.GetAllRecipeEstimationHeaderDTOList(parameters, loadChildRecords, activeChildRecords);
                log.LogMethodExit(recipeEstimationHeaderDTOList);
                return recipeEstimationHeaderDTOList;
            });
        }

        public async Task<string> SaveRecipeEstimations(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry("recipeEstimationHeaderDTOList");
                string result = string.Empty;
                if (recipeEstimationHeaderDTOList == null)
                {
                    throw new ValidationException("recipeEstimationHeaderDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RecipeEstimationHeaderDTO recipeEstimationHeaderDTO in recipeEstimationHeaderDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                            recipeEstimationHeaderBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> CreatePlan(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry("recipeEstimationHeaderDTOList");
                string result = string.Empty;
                List<RecipePlanHeaderDTO> recipePlanHeaderList = new List<RecipePlanHeaderDTO>();
                RecipePlanHeaderListBL recipePlanHeaderListBL = null;
                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                RecipePlanHeaderDTO recipePlanHeaderDTO = null;

                if (recipeEstimationHeaderDTOList == null)
                {
                    throw new ValidationException("recipeEstimationHeaderDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RecipeEstimationHeaderDTO headerDTO in recipeEstimationHeaderDTOList)
                    {

                        if (headerDTO.RecipeEstimationDetailsDTOList.Count > 0)
                        {
                            recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext);
                            List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> searchPlanParams = new List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>>();
                            searchPlanParams.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.PLAN_FROM_DATE, headerDTO.FromDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            searchPlanParams.Add(new KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>(RecipePlanHeaderDTO.SearchByParameters.PLAN_TO_DATE, headerDTO.ToDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                            List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(searchPlanParams, true);
                            if (recipePlanHeaderDTOList != null && recipePlanHeaderDTOList.Any())
                            {
                                recipePlanHeaderDTO = recipePlanHeaderDTOList[0];
                                recipePlanHeaderDTO.RecipeEstimationHeaderId = headerDTO.RecipeEstimationHeaderId;
                            }
                            else
                            {
                                recipePlanHeaderDTO = new RecipePlanHeaderDTO(-1, headerDTO.FromDate, null, null, null, null, headerDTO.RecipeEstimationHeaderId,
                                                                                null, null, null, null, null, null, null, true);
                            }
                            foreach (RecipeEstimationDetailsDTO estimationDetailsDTO in headerDTO.RecipeEstimationDetailsDTOList)
                            {
                                if (recipePlanHeaderDTO.RecipePlanDetailsDTOList.Exists(x => x.ProductId == estimationDetailsDTO.ProductId))
                                {
                                    recipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.ProductId == estimationDetailsDTO.ProductId).PlannedQty = estimationDetailsDTO.TotalEstimatedQty;
                                    recipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.ProductId == estimationDetailsDTO.ProductId).QtyModifiedDate = serverTimeObject.GetServerDateTime();
                                }
                                else
                                {
                                    RecipePlanDetailsDTO recipePlanDetailsDTO = new RecipePlanDetailsDTO(-1, -1, estimationDetailsDTO.ProductId, estimationDetailsDTO.TotalEstimatedQty, null, estimationDetailsDTO.TotalEstimatedQty,
                                        estimationDetailsDTO.UOMId, estimationDetailsDTO.RecipeEstimationDetailId, null, true);
                                    recipePlanHeaderDTO.RecipePlanDetailsDTOList.Add(recipePlanDetailsDTO);
                                }
                            }
                            recipePlanHeaderList.Add(recipePlanHeaderDTO);
                        }
                    }
                    try
                    {
                        if (recipePlanHeaderList.Count > 0)
                        {
                            parafaitDBTrx.BeginTransaction();
                            recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext, recipePlanHeaderList);
                            recipePlanHeaderListBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                    }
                    catch (ValidationException valEx)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(valEx);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        public async Task<string> DeleteRecipeEstimations(List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry("recipeEstimationHeaderDTOList");
                string result = string.Empty;
                if (recipeEstimationHeaderDTOList == null)
                {
                    throw new ValidationException("recipeEstimationHeaderDTOList is empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RecipeEstimationHeaderDTO recipeEstimationHeaderDTO in recipeEstimationHeaderDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                            recipeEstimationHeaderBL.Delete(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }

        private int GetSiteId()
        {
            log.LogMethodEntry();
            int siteId = -1;
            if (executionContext.GetIsCorporate())
            {
                siteId = executionContext.GetSiteId();
            }
            log.LogMethodExit(siteId);
            return siteId;
        }

        public async Task<List<RecipeEstimationHeaderDTO>> GetRecipeEstimationHeaders(int recipeEstimationHeaderId = -1, DateTime? fromDate = null, DateTime? toDate = null, decimal? aspirationalPerc = null,
                                                    decimal? seasonalPerc = null, bool isEvent = false, int historicalDataInDays = 0, int eventOffset = 0, bool isFinishedItem = false,
                                                    bool isSemiFinishedItem = false, bool isActive = true)
        {
            return await Task<List<RecipeEstimationHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext);
                        int siteId = GetSiteId();
                        DateTime fromDateTime = Convert.ToDateTime(fromDate);
                        DateTime toDateTime = Convert.ToDateTime(toDate);
                        RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = new RecipeEstimationHeaderDTO(recipeEstimationHeaderId, fromDateTime, toDateTime, aspirationalPerc, seasonalPerc,
                                                        isEvent, historicalDataInDays, eventOffset, isFinishedItem, isSemiFinishedItem, isActive);
                        RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                        List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = recipeEstimationHeaderBL.BuildForecastData(parafaitDBTrx.SQLTrx);
                        parafaitDBTrx.EndTransaction();
                        log.LogMethodExit(recipeEstimationHeaderDTOList);
                        return recipeEstimationHeaderDTOList;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            });
        }
        public async Task<List<RecipeEstimationHeaderDTO>> GetRecipeForecastingSummary(DateTime? fromDate = null, DateTime? toDate = null, decimal? aspirationalPerc = null,
                                                    decimal? seasonalPerc = null, bool isEvent = false, int historicalDataInDays = 0, int eventOffset = 0, bool isFinishedItem = false,
                                                    bool isSemiFinishedItem = false, string generateForecastData = null)
        {
            return await Task<List<RecipeEstimationHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry();
                List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        RecipeEstimationHeaderListBL recipeEstimationHeaderListBL = new RecipeEstimationHeaderListBL(executionContext);
                        DateTime fromDateTime = Convert.ToDateTime(fromDate);
                        DateTime toDateTime = Convert.ToDateTime(toDate);
                        RecipeEstimationHeaderDTO recipeEstimationHeaderDTO = new RecipeEstimationHeaderDTO(-1, fromDateTime, toDateTime, aspirationalPerc, seasonalPerc,
                                                        isEvent, historicalDataInDays, eventOffset, isFinishedItem, isSemiFinishedItem, true);
                        RecipeEstimationHeaderBL recipeEstimationHeaderBL = new RecipeEstimationHeaderBL(executionContext, recipeEstimationHeaderDTO);
                        recipeEstimationHeaderDTOList = recipeEstimationHeaderBL.GetData(parafaitDBTrx.SQLTrx, generateForecastData);
                        parafaitDBTrx.EndTransaction();
                        log.LogMethodExit(recipeEstimationHeaderDTOList);
                        return recipeEstimationHeaderDTOList;
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw;
                    }
                }
            });
        }
    }
}
