/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the DB Synch Log 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By            Remarks          
 *********************************************************************************************
 *2.150       28-Sep-2023   Lakshminarayana Rao    Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.DBSynch;

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class CreateMasterDataDBSynchLogController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Post the JSON SystemOption.
        /// </summary>
        /// <param name="systemOptionsDTOList">systemOptionsDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Configuration/CreateMasterDataDBSynchLogs")]
        [Authorize]
        public HttpResponseMessage Post()
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry();
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DBSynchLogListBL dBSynchLogListBL = new DBSynchLogListBL(executionContext);
                dBSynchLogListBL.CreateMasterDataDBSynchLog(securityTokenDTO.SiteId);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
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
