/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the printer setup printers Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60.2         12-Mar-2019   Jagan Mohana         Created 
 *               16-May-2019   Mushahid Faizan      Modified Get and Post method.
 *                                                  Added Delete method.
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

namespace Semnox.CommonAPI.SiteSetup
{
    public class PrintSetupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object print setup Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/PrintSetup/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<PrinterDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<PrinterDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<PrinterDTO.SearchByParameters, string>(PrinterDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                bool loadActiveChildRecords = false;
                if (isActive == "1")
                {
                    loadActiveChildRecords = true;
                    searchParameters.Add(new KeyValuePair<PrinterDTO.SearchByParameters, string>(PrinterDTO.SearchByParameters.ISACTIVE, isActive));
                }                
                PrinterListBL printerBl = new PrinterListBL(executionContext);
                var content = printerBl.GetPrinterDTOList(searchParameters, true, loadActiveChildRecords);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on printerDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/PrintSetup/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<PrinterDTO> printerDTOList)
        {
            try
            {
                log.LogMethodEntry(printerDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (printerDTOList != null)
                {
                    // if printerDTOs.printerId is less than zero then insert or else update
                    PrinterListBL printerListBL = new PrinterListBL(executionContext, printerDTOList);
                    printerListBL.SaveUpdatePrinterList();
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
        /// Performs a Delete operation on printerDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/PrintSetup/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<PrinterDTO> printerDTOList)
        {
            try
            {
                log.LogMethodEntry(printerDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (printerDTOList != null)
                {
                    PrinterListBL printerListBL = new PrinterListBL(executionContext, printerDTOList);
                    printerListBL.SaveUpdatePrinterList();
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
