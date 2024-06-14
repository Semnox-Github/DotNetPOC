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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Printer;

namespace Semnox.CommonAPI.Print
{
    public class PrintSetupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
      
        /// <summary>
        /// Get the JSON Object print setup Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Print/Printers")]
        public HttpResponseMessage Get(string isActive)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

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
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content  });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message  });
            }
        }
        /// <summary>
        /// Performs a Post operation on printerDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Print/Printers")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<PrinterDTO> printerDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(printerDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (printerDTOList != null)
                {
                    PrinterListBL printerListBL = new PrinterListBL(executionContext, printerDTOList);
                    printerListBL.SaveUpdatePrinterList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }
       
    }
}
