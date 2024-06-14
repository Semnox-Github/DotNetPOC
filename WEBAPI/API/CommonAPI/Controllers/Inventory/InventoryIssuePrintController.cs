/********************************************************************************************
 * Project Name - Inventory
 * Description  - This controller will return Base64String for Inventory Issue .
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.0    15-Dec-2020  Mushahid Faizan         Created.
 *2.110.0    05-Jan-2021  Abhishek                Modified : build report logic
 ***************************************************************************************************/
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Semnox.Core.Utilities;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Inventory;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryIssuePrintController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Generate the base64String for Issue.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Issue/{issueId}/Print")]
        public async Task<HttpResponseMessage> Get([FromUri]int issueId, string timeStamp = null, DateTime? fromDate = null,
                                       DateTime? toDate = null, string outputFormat = "P")
        {
            //try
            //{
            //    log.LogMethodEntry(issueId, timeStamp, fromDate, toDate, outputFormat, ticketCost);
            //    ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(issueId, timeStamp, fromDate, toDate, outputFormat);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                IInventoryIssueHeaderUseCases receiptReportsUseCase = InventoryUseCaseFactory.GetInventoryIssueHeadersUseCases(executionContext);
                var content = await receiptReportsUseCase.PrintIssues(issueId, "InventoryIssueReceipt", timeStamp, fromDate, toDate, outputFormat);

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
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = "InventoryIssueReceipt -" + ServerDateTime.Now.ToString() + "." + extension;
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
