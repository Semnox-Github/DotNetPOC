/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the print templates Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         12-Mar-2019   Jagan Mohana         Created 
               07-May-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Added isActive Parameter in HttpGet Method.
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using System;
using Semnox.Parafait.Printer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.SiteSetup
{
    public class PrintTemplateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object Receipt and KOT Templates Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/PrintTemplate/")]
        public HttpResponseMessage Get(string activityType = "PRINTTEMPLATE", string isActive = "1", int templateId = -1, string templateName = null)
        {
            try
            {
                log.LogMethodEntry(isActive, templateId, templateName);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOList = new List<ReceiptPrintTemplateHeaderDTO>();
                string exportQuery = string.Empty;
                if (activityType.ToUpper().ToString() == "PRINTTEMPLATE")
                {
                    List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>(ReceiptPrintTemplateHeaderDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                    if (isActive == "1")
                    {
                        searchParameters.Add(new KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>(ReceiptPrintTemplateHeaderDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                    ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderListBL = new ReceiptPrintTemplateHeaderListBL(executionContext);
                    /// To support Export option. This will give query output in text file
                    if (templateId > 0 && !string.IsNullOrEmpty(templateName))
                    {
                        exportQuery = receiptPrintTemplateHeaderListBL.GetExportQueries(templateId, templateName, false);
                    }
                    receiptPrintTemplateHeaderDTOList = receiptPrintTemplateHeaderListBL.GetReceiptPrintTemplateHeaderDTOList(searchParameters, true);
                }
                if (activityType.ToUpper().ToString() == "PRINTTEMPLATEPREVIEW" && templateId > 0)
                {
                    string printTemplateBase64String = PrintTransaction.PrintReceipt(executionContext, templateId);
                    log.LogMethodExit(printTemplateBase64String);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = printTemplateBase64String, token = securityTokenDTO.Token });
                }
                log.LogMethodExit(receiptPrintTemplateHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = receiptPrintTemplateHeaderDTOList, ExportQuery = exportQuery, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on PrintTemplates details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/PrintTemplate/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOs)
        {
            try
            {
                log.LogMethodEntry(receiptPrintTemplateHeaderDTOs);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); 
                if (receiptPrintTemplateHeaderDTOs != null)
                {
                    // if receiptPrintTemplateHeaderDTOs.templateId is less than zero then insert or else update
                    ReceiptPrintTemplateHeaderListBL printerListBL = new ReceiptPrintTemplateHeaderListBL(executionContext, receiptPrintTemplateHeaderDTOs);
                    printerListBL.SaveUpdateReceiptPrintTemplateHeaderList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Delete operation on PrintTemplates details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/PrintTemplate/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOs)
        {
            try
            {
                log.LogMethodEntry(receiptPrintTemplateHeaderDTOs);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (receiptPrintTemplateHeaderDTOs != null)
                {
                    // if receiptPrintTemplateHeaderDTOs.templateId is less than zero then insert or else update
                    ReceiptPrintTemplateHeaderListBL printerListBL = new ReceiptPrintTemplateHeaderListBL(executionContext, receiptPrintTemplateHeaderDTOs);
                    printerListBL.SaveUpdateReceiptPrintTemplateHeaderList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
