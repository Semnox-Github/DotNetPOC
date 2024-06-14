/********************************************************************************************
 * Project Name - Inventory
 * Description  - This controller will return Base64String for purchase orders .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    14-Dec-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class PurchaseOrderPrintController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Generate the pdf for Purchase Order.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/PurchaseOrder/{orderId}/Print")]
        public async Task<HttpResponseMessage> Get(int orderId, string timeStamp = null, DateTime? fromDate = null,
                                                             DateTime? toDate = null, string outputFormat = "P", double ticketCost = 0)
        {
            //try
            //{
            //log.LogMethodEntry(orderId, timeStamp, fromDate, toDate, outputFormat, ticketCost);
            //ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                IPurchaseOrderUseCases receiptReportsUseCase = InventoryUseCaseFactory.GetPurchaseOrderUseCases(executionContext);
                var content = await receiptReportsUseCase.PrintPurchaseOrders(orderId, "InventoryPurchaseOrderReceipt", timeStamp, fromDate,
                                                                    toDate, outputFormat, ticketCost);

                string extension = "pdf";
                if (outputFormat == "E")
                {
                    extension = "xlsx";
                }
                else if (outputFormat == "V")
                {
                    extension = "csv";
                }
                else if (outputFormat == "H")
                {
                    extension = "html";
                }
                HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
                httpResponseMessage.Content = new StreamContent(content);
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = "InventoryPurchaseOrderReceipt -" + ServerDateTime.Now.ToString() + "." + extension;
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                return httpResponseMessage;

            }
            catch (Exception ex)
            {
                log.Error(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    data = ExceptionSerializer.Serialize(ex)
                });
            }
        }
    }
}
