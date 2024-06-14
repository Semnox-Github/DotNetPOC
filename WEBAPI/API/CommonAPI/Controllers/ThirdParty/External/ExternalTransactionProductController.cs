/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalProductController  API -  add and delete product data in Parafait
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *2.130.7    07-Apr-2022            Ashish Bhat           Created - External  REST API
 ********************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalTransactionProductController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Post the JSON Add Products
        /// </summary>       
        /// <param name="externalAddProductDTO">externalAddProductDTO</param>
        /// <returns>HttpResponseMessage</returns>              
        [HttpPost]
        [Route("api/External/Transaction/{transactionId}/Product")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromUri] int transactionId, [FromBody]ExternalAddProductDTO externalAddProductDTO)
        {
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(transactionId, externalAddProductDTO);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                if (externalAddProductDTO == null)
                {
                    string customException = "Products data cannot be null.Please enter the Product Details";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                ExternalTransactionListBL externalTransactionBL = new ExternalTransactionListBL(executionContext);
                ExternalTransactionDTO externalTransactionDTO = externalTransactionBL.AddProducts(transactionId, externalAddProductDTO);
                //string message = "Product Added Successfully";
                log.LogMethodExit(externalTransactionDTO);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = externalTransactionDTO });
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
