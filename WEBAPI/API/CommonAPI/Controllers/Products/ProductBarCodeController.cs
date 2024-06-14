/********************************************************************************************
 * Project Name - Product BarCode Controller
 * Description  - Created to fetch, update and insert in the Product BarCode entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60.2     13-Jun-2019   Nagesh Badiger           Created 
  *2.110.0    10-Sep-2020   Vikas Dwivedi            Modified as per the REST API Standards.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Controllers.Products
{
    public class ProductBarCodeController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the BarCode .
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductBarCodes")]
        public HttpResponseMessage Get(int barCodeId = -1, int productId = -1, string barCode = null, string isActive = null)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(barCodeId, productId, barCode, isActive);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
                if (barCodeId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.ID, barCodeId.ToString()));
                }
                if (productId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.PRODUCT_ID, productId.ToString()));
                }
                if (!string.IsNullOrEmpty(barCode))
                {
                    searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.BARCODE, barCode));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ProductBarcodeDTO.SearchByParameters, string>(ProductBarcodeDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(executionContext);
                List<ProductBarcodeDTO> productBarcodeDTOList = productBarcodeListBL.GetProductBarcodeDTOList(searchParameters);
                log.LogMethodExit(productBarcodeDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = productBarcodeDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Product BarCode
        /// </summary>
        /// <param name="productBarcodeDTOList">productBarcodeDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/ProductBarCodes")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ProductBarcodeDTO> productBarcodeDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(productBarcodeDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (productBarcodeDTOList != null && productBarcodeDTOList.Any())
                {
                    ProductBarcodeListBL productBarcodeListBL = new ProductBarcodeListBL(executionContext, productBarcodeDTOList);
                    productBarcodeListBL.Save();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = productBarcodeDTOList });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException valEx)
            {
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
