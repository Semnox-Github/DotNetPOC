/********************************************************************************************
 * Project Name - Create Master Data Controller                                                                         
 * Description  - Controller of the SiteSetup class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.70        21-Oct-2019    Rakesh Kumar          Created 
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
    public class HQSiteManagementController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO = null;
        private ExecutionContext executionContext;
        /// <summary>
        /// Post the  HQ site data
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Route("api/SiteSetup/HQSiteManagement/")]
        [Authorize]
        public HttpResponseMessage Get(string activityType, int siteId)
        {
            try
            {
                log.LogMethodEntry();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                if(siteId > 0)
                {
                    executionContext = new ExecutionContext(securityTokenDTO.LoginId, siteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                }
                else
                {
                    executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                }
                
                if (!string.IsNullOrEmpty(activityType) && Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]))
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        try
                        {

                            if (activityType.ToUpper().ToString() == "CREATEMASTERDATA")
                            {
                                DBSynchTableBL dBSynchTableBL = new DBSynchTableBL(executionContext);
                                dBSynchTableBL.CreateMasterData(parafaitDBTrx.SQLTrx);
                            }
                            else if (activityType.ToUpper().ToString() == "DOWNLOADHQSITEDATA")
                            {
                                DBSynchLogBL dBSynchLogBL = new DBSynchLogBL(executionContext);
                                dBSynchLogBL.SynchHQSiteData(parafaitDBTrx.SQLTrx);
                            }
                            else
                            {
                                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "NotFound", token = securityTokenDTO.Token });
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
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
