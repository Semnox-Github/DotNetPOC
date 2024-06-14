/********************************************************************************************
 * Project Name - Products Controller/ProductBOMController
 * Description  - Created to fetch, update and insert in the Product Bill of materials .
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60.3     14-Jun-2019   Nagesh Badiger           Created 
 *2.70       20-Jun-2019   Akshay Gulaganji         modified Get() and added Delete method
 *2.110.0    14-Aug-2020   Girish Kundar           Mdified : 3 Tier changes as part of phase -3 Rest API changes
 *2.120.00   09-Mar-2021   Roshan Devadiga          Modified Get,Post and Added Put method
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class ProductBOMController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the ProductBOM records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Product/ProductBOM")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(int bOMId = -1, int productId = -1, int childProductId = -1, string isActive = null)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(bOMId, productId, childProductId, isActive);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

                List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>> searchParameters = new List<KeyValuePair<BOMDTO.SearchByBOMParameters, string>>();
                searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.SITEID, executionContext.GetSiteId().ToString()));
                if (bOMId > -1)
                {
                    searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.BOMID, bOMId.ToString()));
                }
                if (productId > -1)
                {
                    searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.PRODUCT_ID, productId.ToString()));
                }
                if (childProductId > -1)
                {
                    searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.CHILDPRODUCT_ID, childProductId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<BOMDTO.SearchByBOMParameters, string>(BOMDTO.SearchByBOMParameters.IS_ACTIVE, "1"));
                    }
                }
                IBOMUseCases productBOMUseCases = ProductsUseCaseFactory.GetBOMs(executionContext);
                List<BOMDTO> bOMDTOList = await productBOMUseCases.GetBOMs(searchParameters);
                log.LogMethodExit(bOMDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = bOMDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Posts the Jason Object of BOM List
        /// </summary>
        /// <param name="bOMDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/ProductBOM")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<BOMDTO> bOMDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(bOMDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (bOMDTOList == null)
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IBOMUseCases productBOMUseCases = ProductsUseCaseFactory.GetBOMs(executionContext);
                await productBOMUseCases.SaveBOMs(bOMDTOList);
                log.LogMethodExit(bOMDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = bOMDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the bOMDTOList collection
        /// </summary>
        /// <param name="bOMDTOList"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/Product/ProductBOM")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<BOMDTO> bOMDTOList)
        {
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(bOMDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (bOMDTOList == null || bOMDTOList.Any(a => a.BOMId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IBOMUseCases productBOMUseCases = ProductsUseCaseFactory.GetBOMs(executionContext);
                await productBOMUseCases.SaveBOMs(bOMDTOList);
                log.LogMethodExit(bOMDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = bOMDTOList });
            }

            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Delete the Jason Object of BOM List
        /// </summary>
        /// <param name="bOMDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Products/ProductBOM")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<BOMDTO> bOMDTOList)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(bOMDTOList);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request); ;
                if (bOMDTOList != null && bOMDTOList.Count > 0)
                {
                    BOMList bOMList = new BOMList(executionContext, bOMDTOList);
                    bOMList.SaveUpdateProductBOM();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
