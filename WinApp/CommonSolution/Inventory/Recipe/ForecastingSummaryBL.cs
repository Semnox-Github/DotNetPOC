/********************************************************************************************
 * Project Name - AccountSummaryBL
 * Description  - BL for Manage Account Summary Details
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By        Remarks          
 *********************************************************************************************
 *2.100.0    15-Sep-2020        Deeksha            Created for Recipe management enhancement.
 ********************************************************************************************/
using System;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;
using System.Collections.Generic;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class ForecastingSummaryListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList = new List<RecipeEstimationHeaderDTO>();


        public ForecastingSummaryListBL(ExecutionContext executionContext, List<RecipeEstimationHeaderDTO> recipeEstimationHeaderDTOList)
        {
            log.LogMethodEntry(executionContext, recipeEstimationHeaderDTOList);
            this.executionContext = executionContext;
            this.recipeEstimationHeaderDTOList = recipeEstimationHeaderDTOList;
            log.LogMethodExit();
        }

        public List<ForecastingSummaryDTO> GetForecastingSummary(DateTime fromDate , DateTime toDate)
        {
            log.LogMethodEntry(fromDate , toDate);
            UOMContainer uOMContainer = CommonFuncs.GetUOMContainer();
            List<ForecastingSummaryDTO> forecastingSummaryDTOList = new List<ForecastingSummaryDTO>();
            List<RecipeEstimationDetailsDTO> recipeEstimationDetailsDTOList = new List<RecipeEstimationDetailsDTO>();
            if(ProductContainer.productDTOList == null || ProductContainer.productDTOList.Count <= 0)
            {
                log.LogMethodExit();
                return null;
            }
            List<ProductDTO> productDTOList = ProductContainer.productDTOList.Where(x => x.IsActive == true & x.IncludeInPlan == true).ToList();

            foreach (ProductDTO productDTO in productDTOList)
            {
                if (recipeEstimationHeaderDTOList.Exists(x => x.RecipeEstimationDetailsDTOList.Exists(y => y.ProductId == productDTO.ProductId)))
                {
                    InventoryList inventoryList = new InventoryList();
                    decimal productStock = inventoryList.GetProductStockQuantity(productDTO.ProductId);
                    ForecastingSummaryDTO forecastingSummaryDTO = new ForecastingSummaryDTO();
                    forecastingSummaryDTO.PlannedQuantity = Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.PlannedQty));
                    forecastingSummaryDTO.RecipeName = productDTO.Description;
                    if ((toDate - fromDate).TotalDays >= 28)
                    {
                        forecastingSummaryDTO.EstimatedMonthlyQuantity = Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty));
                        forecastingSummaryDTO.EstimatedWeeklyQuantity = Math.Round(Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty)) / 4);
                    }
                    else if ((toDate - fromDate).TotalDays >= 21)
                    {
                        forecastingSummaryDTO.EstimatedMonthlyQuantity = Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty));
                        forecastingSummaryDTO.EstimatedWeeklyQuantity = Math.Round(Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty)) / 4);
                    }
                    else if ((toDate - fromDate).TotalDays >= 14)
                    {
                        forecastingSummaryDTO.EstimatedMonthlyQuantity = Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty));
                        forecastingSummaryDTO.EstimatedWeeklyQuantity = Math.Round(Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty)) / 3);
                    }
                    else if ((toDate - fromDate).TotalDays >= 7)
                    {
                        forecastingSummaryDTO.EstimatedWeeklyQuantity = Math.Round(Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty)) / 2);
                        forecastingSummaryDTO.EstimatedMonthlyQuantity = Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty));
                    }
                    else
                    {
                        forecastingSummaryDTO.EstimatedWeeklyQuantity = Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty));
                        forecastingSummaryDTO.EstimatedMonthlyQuantity = Convert.ToDecimal(recipeEstimationHeaderDTOList.SelectMany(x => x.RecipeEstimationDetailsDTOList.FindAll(y => y.ProductId == productDTO.ProductId)).Sum(x => x.TotalEstimatedQty));
                    }
                    forecastingSummaryDTO.Stock = productStock;
                    forecastingSummaryDTO.ProductId = productDTO.ProductId;
                    forecastingSummaryDTO.UOM = UOMContainer.uomDTOList.Find(x=>x.UOMId == productDTO.InventoryUOMId).UOM;
                    forecastingSummaryDTO.EstimationDetails = "Daily Estimate";
                    forecastingSummaryDTOList.Add(forecastingSummaryDTO);
                }
            }
            log.LogMethodExit(forecastingSummaryDTOList);
            return forecastingSummaryDTOList;
        }
    }
}
