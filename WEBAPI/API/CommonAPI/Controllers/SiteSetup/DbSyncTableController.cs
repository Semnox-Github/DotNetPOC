/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the DB Synch
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************
 *2.60        24-Mar-2019   Jagan Mohana Rao    Created 
 *            09-Apr-2019   Mushahid Faizan     Modified Post & Delete Method
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.DBSynch;

namespace Semnox.CommonAPI.SiteSetup
{
    public class DbSyncTableController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object DBSynch List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/DbSyncTable/")]
        public HttpResponseMessage Get()
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DBSynchList dBSynchList = new DBSynchList(executionContext);
                var content = dBSynchList.GetAllDBSynchList(null);
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
        /// Post the JSON DBSynch.
        /// </summary>
        /// <param name="dBSynchDTOList">dBSynchDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/SiteSetup/DbSyncTable/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<DBSynchTableDTO> dBSynchDTOList)
        {
            try
            {
                log.LogMethodEntry(dBSynchDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (dBSynchDTOList != null)
                {
                    DBSynchList dBSynchList = new DBSynchList(executionContext, dBSynchDTOList);
                    dBSynchList.SaveUpdateDbSynchList();
                    log.LogMethodExit();
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
        /// Delete the JSON DBSynch.
        /// </summary>
        /// <param name="dBSynchDTOList">dBSynchDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/SiteSetup/DbSyncTable/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<DBSynchTableDTO> dBSynchDTOList)
        {
            try
            {
                log.LogMethodEntry(dBSynchDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (dBSynchDTOList != null)
                {
                    DBSynchList dBSynchList = new DBSynchList(executionContext, dBSynchDTOList);
                    dBSynchList.DeleteDbSynchList();
                    log.LogMethodExit();
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