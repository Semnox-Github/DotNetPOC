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
 *2.90.0      14-Jun-2020   Girish Kundar       Modified : REST API phase 2 changes/standard             
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

namespace Semnox.CommonAPI.Configuration
{
    public class DbSyncTableController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object DBSynch List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/DBSyncTables")]
        public HttpResponseMessage Get()
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

                DBSynchList dBSynchList = new DBSynchList(executionContext);
                List<KeyValuePair<DBSynchTableDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<DBSynchTableDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<DBSynchTableDTO.SearchByParameters, string>(DBSynchTableDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                var content = dBSynchList.GetAllDBSynchList(searchParameters);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Post the JSON DBSynch.
        /// </summary>
        /// <param name="dBSynchDTOList">dBSynchDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Configuration/DBSyncTables")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<DBSynchTableDTO> dBSynchDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(dBSynchDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (dBSynchDTOList != null && dBSynchDTOList.Any())
                {
                    DBSynchList dBSynchList = new DBSynchList(executionContext, dBSynchDTOList);
                    dBSynchList.SaveUpdateDbSynchList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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

        /// <summary>
        /// Delete the JSON DBSynch.
        /// </summary>
        /// <param name="dBSynchDTOList">dBSynchDTOList</param>
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Configuration/DBSyncTables")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<DBSynchTableDTO> dBSynchDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(dBSynchDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (dBSynchDTOList != null && dBSynchDTOList.Any())
                {
                    DBSynchList dBSynchList = new DBSynchList(executionContext, dBSynchDTOList);
                    dBSynchList.DeleteDbSynchList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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