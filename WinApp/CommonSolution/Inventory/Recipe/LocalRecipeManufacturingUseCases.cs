/********************************************************************************************
 * Project Name - Inventory 
 * Description  - LocalRecipeManufacturingUseCases class to get the data  from local DB 
 * 
 *   
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.00       16-Nov-2020      Abhishek             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Inventory.Recipe
{
    public class LocalRecipeManufacturingUseCases : IRecipeManufacturingUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalRecipeManufacturingUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<RecipeManufacturingHeaderDTO>> GetRecipeManufacturings(List<KeyValuePair<RecipeManufacturingHeaderDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool activeChildRecords = false)
        {          
            return await Task<List<RecipeManufacturingHeaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(parameters);
                RecipeManufacturingHeaderListBL recipeManufacturingHeaderListBL = new RecipeManufacturingHeaderListBL(executionContext);
                int siteId = GetSiteId();
                List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList = recipeManufacturingHeaderListBL.GetAllRecipeManufacturingHeaderDTOList(parameters, loadChildRecords, activeChildRecords);
                log.LogMethodExit(recipeManufacturingHeaderDTOList);
                return recipeManufacturingHeaderDTOList;              
            });
        }

        public async Task<string> SaveRecipeManufacturings(List<RecipeManufacturingHeaderDTO> recipeManufacturingHeaderDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry("recipeManufacturingHeaderDTOList");
                string result = string.Empty;
                if (recipeManufacturingHeaderDTOList == null)
                {
                    throw new ValidationException("recipeManufacturingHeaderDTOList is empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (RecipeManufacturingHeaderDTO recipeManufacturingHeaderDTO in recipeManufacturingHeaderDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            RecipeManufacturingHeaderBL recipeManufacturingHeaderBL = new RecipeManufacturingHeaderBL(executionContext, recipeManufacturingHeaderDTO);
                            recipeManufacturingHeaderBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw ;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ;
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
    }
}
