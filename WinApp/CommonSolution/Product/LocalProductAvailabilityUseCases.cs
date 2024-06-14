/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductProductAvailabilityUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021      Roshan Devadiga            Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public class LocalProductAvailabilityUseCases : IProductAvailabilityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductAvailabilityUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        public async Task<List<KeyValuePair<string, List<ProductsAvailabilityDTO>>>> GetProductAvailability(string loginId, bool searchUnavailableProduct = false)
        {
            return await Task<List<KeyValuePair<string, List<ProductsAvailabilityDTO>>>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchUnavailableProduct);
                List<ProductsDisplayGroupDTO> productsOfExcludedDisplayGroups = null;
                ProductDisplayGroupList displayGroupList = new ProductDisplayGroupList(executionContext);
                List<ProductDisplayGroupFormatDTO> displayFormatGroups = displayGroupList.GetConfiguredDisplayGroupListForLogin(loginId);

                ProductsDisplayGroupList productsByDisplayGroupList = new ProductsDisplayGroupList(executionContext);
                List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>> displayGroupSearchParameters
                    = new List<KeyValuePair<ProductsDisplayGroupDTO.SearchByProductsDisplayGroupParameters, string>>();
                List<ProductsDisplayGroupDTO> productsByDisplayGroups = productsByDisplayGroupList.GetAllProductsDisplayGroup(displayGroupSearchParameters);

                if (displayFormatGroups != null)
                {
                    productsOfExcludedDisplayGroups = productsByDisplayGroups.Where(x => !displayFormatGroups.Any(y => y.Id == x.DisplayGroupId)).ToList();
                    List<ProductsDisplayGroupDTO> productsOfincludedDisplayGroups = productsByDisplayGroups.Where(x => displayFormatGroups.Any(y => y.Id == x.DisplayGroupId)).ToList();
                    productsOfExcludedDisplayGroups = productsByDisplayGroups.Where(x => !productsOfincludedDisplayGroups.Any(y => y.ProductId == x.ProductId)).ToList();
                }
                ProductsAvailabilityListBL pAListBL = new ProductsAvailabilityListBL(executionContext);
                List<ProductsAvailabilityDTO> productsAvailabilityDTOList = pAListBL.GetAvailableProductsList(productsOfExcludedDisplayGroups);
                List<ProductsAvailabilityDTO> unAvailableProducts = pAListBL.GetUnAvailableProductsList(productsAvailabilityDTOList, productsOfExcludedDisplayGroups);
                productsAvailabilityDTOList = productsAvailabilityDTOList.Where(x => !unAvailableProducts.Any(y => y.ProductId == x.ProductId)).ToList();
                if (searchUnavailableProduct == false)
                {
                    unAvailableProducts = new List<ProductsAvailabilityDTO>();
                }
                List<KeyValuePair<string, List<ProductsAvailabilityDTO>>> result = new List<KeyValuePair<string, List<ProductsAvailabilityDTO>>>();
                result.Add(new KeyValuePair<string, List<ProductsAvailabilityDTO>>("unAvailableProducts", unAvailableProducts));
                result.Add(new KeyValuePair<string, List<ProductsAvailabilityDTO>>("productsAvailabilityDTOList", productsAvailabilityDTOList));

                return result;
            });
        }
        public async Task<List<ValidationError>> SaveAvailableProducts(List<ProductsAvailabilityDTO> productsAvailabilityDTOList, string loginId)
        {
            return await Task<List<ValidationError>>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(productsAvailabilityDTOList);
                if (productsAvailabilityDTOList == null)
                {
                    throw new ValidationException("productsAvailabilityDTOList is Empty");
                }
                List<ValidationError> errorsList = null;
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        ProductsAvailabilityListBL productsAvailabilityBL = new ProductsAvailabilityListBL(executionContext, productsAvailabilityDTOList);
                        errorsList = productsAvailabilityBL.Save(loginId);
                        parafaitDBTrx.EndTransaction();
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw ex;
                    }
                }
                log.LogMethodExit(errorsList);
                return errorsList;
            });
        }
    }
}
