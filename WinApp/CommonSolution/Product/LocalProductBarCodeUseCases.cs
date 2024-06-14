/********************************************************************************************
 * Project Name - Product
 * Description  - LocalProductBarcodeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
*2.140.00   14-Sep-2021      Roshan Devadiga            Created :POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    // <summary>
    /// Implementation of productBarcode use-cases
    /// </summary>
    public class LocalProductBarcodeUseCases:IProductBarcodeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalProductBarcodeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProductBarcodeDTO>> GetProductBarcodes(List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<ProductBarcodeDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(executionContext);
                List<ProductBarcodeDTO> productBarcodeDTOList = productBarcodeListBL.GetProductBarcodeDTOList(searchParameters);

                log.LogMethodExit(productBarcodeDTOList);
                return productBarcodeDTOList;
            });
        }
        public async Task<string> SaveProductBarcodes(List<ProductBarcodeDTO> productBarcodeDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(productBarcodeDTOList);
                if (productBarcodeDTOList == null)
                {
                    throw new ValidationException("productBarcodeDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (ProductBarcodeDTO productBarcodeDTO in productBarcodeDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ProductBarcodeBL productBarcodeBL = new ProductBarcodeBL(executionContext, productBarcodeDTO);
                            productBarcodeBL.Save(parafaitDBTrx.SQLTrx);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
