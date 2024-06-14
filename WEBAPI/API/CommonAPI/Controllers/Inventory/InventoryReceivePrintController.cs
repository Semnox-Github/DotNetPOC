/********************************************************************************************
 * Project Name - Inventory
 * Description  - This controller will return Base64String for Inventory Receive .
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
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.Utilities;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Reports;

namespace Semnox.CommonAPI.Controllers.Inventory
{
    public class InventoryReceivePrintController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Generate the base64String for Receive.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Inventory/Receive/{receiptId}/Print")]
        public async Task<HttpResponseMessage> Get([FromUri]int receiptId, string timeStamp = null, DateTime? fromDate = null,
                                                   DateTime? toDate = null, string outputFormat = "P")
        {
            //try
            //{
            //    log.LogMethodEntry(receiptId, timeStamp, fromDate,toDate, outputFormat);
            //    ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<clsReportParameters.SelectedParameterValue> backgroundParameters = new List<clsReportParameters.SelectedParameterValue>();
                backgroundParameters.Add(new clsReportParameters.SelectedParameterValue("ReceiptId", receiptId.ToString()));

                IReceiptReportsUseCase receiptReportsUseCase = InventoryUseCaseFactory.PrintReports(executionContext);
                var content = await receiptReportsUseCase.PrintReceives("InventoryReceiveReceipt", timeStamp, fromDate,
                                                                    toDate, backgroundParameters, outputFormat);

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
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = "InventoryReceiveReceipt -" + ServerDateTime.Now.ToString() + "." + extension;
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
