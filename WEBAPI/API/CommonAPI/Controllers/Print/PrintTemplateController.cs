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
*2.90         14-07-2020     Girish Kundar        Modified : Moved to Print resource folder and REST API staandard changes
*2.120.0     10-May-2021      Mushahid Faizan     Modified: Added loadActiveChildRecords search param to load active/Inactive child records in WMS.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Print
{
    public class PrintTemplateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Object Receipt and KOT Templates Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Print/PrintTemplates")]
        public HttpResponseMessage Get(string printTemplate = null, string isActive = null, int templateId = -1, string templateName = null , bool buildPreviewImage = false,
                                         bool loadActiveChildRecords = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, templateId, templateName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                string printTemplateBase64String = null;
                List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOList = new List<ReceiptPrintTemplateHeaderDTO>();
                string exportQuery = string.Empty;
                if (string.IsNullOrEmpty(printTemplate) == false && printTemplate.ToUpper().ToString() == "PRINTTEMPLATE")
                {
                    List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>>();
                    searchParameters.Add(new KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>(ReceiptPrintTemplateHeaderDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                    if (string.IsNullOrEmpty(isActive) == false)
                    {
                        if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                        {
                            searchParameters.Add(new KeyValuePair<ReceiptPrintTemplateHeaderDTO.SearchByParameters, string>(ReceiptPrintTemplateHeaderDTO.SearchByParameters.IS_ACTIVE, isActive));
                        }
                    }
                    ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderListBL = new ReceiptPrintTemplateHeaderListBL(executionContext);
                    /// To support Export option. This will give query output in text file
                    if (templateId > 0 && !string.IsNullOrEmpty(templateName))
                    {
                        exportQuery = receiptPrintTemplateHeaderListBL.GetExportQueries(templateId, templateName, false);
                    }
                    receiptPrintTemplateHeaderDTOList = receiptPrintTemplateHeaderListBL.GetReceiptPrintTemplateHeaderDTOList(searchParameters, true,null, loadActiveChildRecords);
                }
                if (buildPreviewImage  && templateId > 0)
                {
                    printTemplateBase64String = PrintTransaction.PrintReceipt(executionContext, templateId);
                    log.LogMethodExit(printTemplateBase64String);
                }
                log.LogMethodExit(receiptPrintTemplateHeaderDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = receiptPrintTemplateHeaderDTOList, ExportQuery = exportQuery , TemplatePreviewImage = printTemplateBase64String });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }
        /// <summary>
        /// Performs a Post operation on PrintTemplates details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Print/PrintTemplates")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateHeaderDTOs)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(receiptPrintTemplateHeaderDTOs);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (receiptPrintTemplateHeaderDTOs != null)
                {
                    ReceiptPrintTemplateHeaderListBL printerListBL = new ReceiptPrintTemplateHeaderListBL(executionContext, receiptPrintTemplateHeaderDTOs);
                    printerListBL.SaveUpdateReceiptPrintTemplateHeaderList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "" });
                }
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

    }
}
