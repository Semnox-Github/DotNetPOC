/********************************************************************************************
 * Project Name - Inventory 
 * Description  - LocalRecipePlanUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00       16-Nov-2020      Abhishek             Created : POS UI Redesign with REST API
 2.130.0     13-Jun-2021         Mushahid Faizan       Modified : Web Inventory UI Changes.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class LocalRecipePlanUseCases : IRecipePlanUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalRecipePlanUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<RecipePlanHeaderDTO>> GetRecipePlans(List<KeyValuePair<RecipePlanHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false)
        {
            return await Task<List<RecipePlanHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RecipePlanHeaderListBL recipePlanHeaderListBL = new RecipePlanHeaderListBL(executionContext);
                int siteId = GetSiteId();
                List<RecipePlanHeaderDTO> recipePlanHeaderDTOList = recipePlanHeaderListBL.GetAllRecipePlanHeaderDTOList(parameters, loadChildRecords, activeChildRecords);
                log.LogMethodExit(recipePlanHeaderDTOList);
                return recipePlanHeaderDTOList;
            });
        }

        public async Task<string> SaveRecipePlans(List<RecipePlanHeaderDTO> recipePlanHeaderDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry("recipePlanHeaderDTOList");
                string result = string.Empty;
                if (recipePlanHeaderDTOList == null)
                {
                    throw new ValidationException("recipePlanHeaderDTOList is empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RecipePlanHeaderDTO recipePlanHeaderDTO in recipePlanHeaderDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            RecipePlanHeaderBL recipePlanHeaderBL = new RecipePlanHeaderBL(executionContext, recipePlanHeaderDTO);
                            recipePlanHeaderBL.Save(parafaitDBTrx.SQLTrx);
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

        public async Task<string> CreateKPN(List<RecipePlanDetailsDTO> recipePlanDetailsDTOList, int planHeaderId)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry("recipePlanHeaderDTOList");
                string result = string.Empty;
                if (recipePlanDetailsDTOList == null)
                {
                    throw new ValidationException("recipePlanHeaderDTOList is empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        ProductContainer productContainer = new ProductContainer(executionContext);
                        RecipePlanHeaderBL recipePlanHeaderBL = new RecipePlanHeaderBL(executionContext, planHeaderId, true, true);
                        RecipePlanHeaderDTO recipePlanHeaderDTO = recipePlanHeaderBL.RecipePlanHeaderDTO;
                        recipePlanHeaderDTO.RecipePlanDetailsDTOList = new List<RecipePlanDetailsDTO>(recipePlanDetailsDTOList);

                        RecipeManufacturingHeaderDTO manufacturingHeaderDTO = new RecipeManufacturingHeaderDTO();

                        RecipeManufacturingHeaderListBL recipeManufacturingHeaderListBL = new RecipeManufacturingHeaderListBL(executionContext);
                        List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_FROM_DATETIME, recipePlanHeaderDTO.PlanDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.MFG_TO_DATETIME, recipePlanHeaderDTO.PlanDateTime.AddDays(1).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
                        searchParameters.Add(new KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>(RecipeManufacturingHeaderDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = recipeManufacturingHeaderListBL.GetAllRecipeManufacturingHeaderDTOList(searchParameters, true, true);
                        if (recipeManufacturingHeaderDTOList.Any())
                        {
                            manufacturingHeaderDTO = recipeManufacturingHeaderDTOList[0];
                            //if (manufacturingHeaderDTO.IsComplete)
                            //{
                            //    string message = MessageContainerList.GetMessage(executionContext, 2844);
                            //    log.Debug("Cannot create Manufacturing details. Stock is finalized for the selected date");
                            //    throw new ValidationException(message);
                            //}
                        }
                        else
                        {
                            manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList = new List<RecipeManufacturingDetailsDTO>();
                            manufacturingHeaderDTO.MFGDateTime = recipePlanHeaderDTO.PlanDateTime;
                        }
                        manufacturingHeaderDTO.RecipePlanHeaderId = recipePlanHeaderDTO.RecipePlanHeaderId;

                        foreach (RecipePlanDetailsDTO recipePlanDetailsDTO in recipePlanHeaderDTO.RecipePlanDetailsDTOList)
                        {
                            RecipeManufacturingDetailsDTO dto = manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Find(x => x.RecipePlanDetailId == recipePlanDetailsDTO.RecipePlanDetailId
                                            & x.ProductId == recipePlanDetailsDTO.ProductId);
                            if (dto == null && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                            {
                                ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == recipePlanDetailsDTO.ProductId);
                                dto = new RecipeManufacturingDetailsDTO(-1, -1, -1, productDTO.ProductId, true, -1, -1, 1,
                                                                        productDTO.UomId, 1, productDTO.UomId, Convert.ToDecimal(productDTO.Cost), Convert.ToDecimal(productDTO.Cost), 
                                                                        Convert.ToDecimal(productDTO.Cost), recipePlanDetailsDTO.RecipePlanDetailId, true, false);

                                manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Add(dto);
                                if (productDTO.ProductBOMDTOList != null && productDTO.ProductBOMDTOList.Any())
                                {
                                    int count = productDTO.ProductBOMDTOList.Count;
                                    while (count > 0)
                                    {
                                        for (int j = 0; j < productDTO.ProductBOMDTOList.Count; j++)
                                        {
                                            BOMDTO bOMDTO = productDTO.ProductBOMDTOList[j];
                                            ProductDTO childproductDTO = ProductContainer.productDTOList.Find(x => x.ProductId == bOMDTO.ChildProductId);
                                            if (bOMDTO.UOMId == -1)
                                            {
                                                bOMDTO.UOMId = childproductDTO.InventoryUOMId;
                                            }
                                            dto = new RecipeManufacturingDetailsDTO(-1, -1, -1, childproductDTO.ProductId, false, -1, -1, bOMDTO.Quantity,
                                                                                            bOMDTO.UOMId, bOMDTO.Quantity, bOMDTO.UOMId, Convert.ToDecimal(childproductDTO.Cost), (bOMDTO.Quantity * Convert.ToDecimal(childproductDTO.Cost)),
                                                                                            Convert.ToDecimal(childproductDTO.Cost), recipePlanDetailsDTO.RecipePlanDetailId, true, false);

                                            manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Add(dto);
                                            count--;
                                        }
                                    }
                                }
                                RecipeManufacturingDetailsListBL recipeManufacturingDetailsListBL = new RecipeManufacturingDetailsListBL(executionContext, manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList);
                                recipeManufacturingDetailsListBL.SetLineDetails();
                            }
                        }
                        List<RecipeManufacturingDetailsDTO> parentProductList = manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Where(x => x.IsParentItem == true).ToList();
                        foreach (RecipeManufacturingDetailsDTO parentDTO in parentProductList)
                        {
                            if (parentDTO.RecipePlanDetailId > -1 && ProductContainer.productDTOList != null && ProductContainer.productDTOList.Count > 0)
                            {
                                decimal? finalQuantity = 0;
                                ProductDTO productDTO = ProductContainer.productDTOList.Find(x => x.ProductId == parentDTO.ProductId);
                                RecipePlanDetailsDTO recipePlanDetailsDTO = recipePlanHeaderDTO.RecipePlanDetailsDTOList.Find(x => x.RecipePlanDetailId == parentDTO.RecipePlanDetailId);

                                if (recipePlanDetailsDTO != null)
                                {
                                    finalQuantity = recipePlanDetailsDTO.FinalQty;
                                    int parentLineId = parentDTO.MfgLineId;
                                    List<RecipeManufacturingDetailsDTO> childProductList = manufacturingHeaderDTO.RecipeManufacturingDetailsDTOList.Where(x => x.ParentMFGLineId == parentLineId).ToList();
                                    foreach (RecipeManufacturingDetailsDTO childDTO in childProductList)
                                    {
                                        decimal quantity = productDTO.ProductBOMDTOList.Find(x => x.ChildProductId == childDTO.ProductId).Quantity;
                                        if (productDTO.YieldPercentage > 0)
                                        {
                                            childDTO.Quantity = (finalQuantity * quantity * 100) / productDTO.YieldPercentage;
                                        }
                                        else
                                        {
                                            childDTO.Quantity = (finalQuantity * quantity);
                                        }

                                        childDTO.ActualMfgQuantity = childDTO.Quantity;
                                        childDTO.ActualCost = childDTO.ActualMfgQuantity * childDTO.ItemCost;
                                    }
                                    parentDTO.Quantity = finalQuantity;
                                    parentDTO.ActualMfgQuantity = finalQuantity;
                                    parentDTO.ActualCost = parentDTO.ActualMfgQuantity * parentDTO.ItemCost;
                                }
                            }
                        }
                        parafaitDBTrx.BeginTransaction();
                        RecipeManufacturingHeaderBL recipeManufacturingHeaderBL = new RecipeManufacturingHeaderBL(executionContext, manufacturingHeaderDTO);
                        recipeManufacturingHeaderBL.Save(parafaitDBTrx.SQLTrx);
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
    }
}
