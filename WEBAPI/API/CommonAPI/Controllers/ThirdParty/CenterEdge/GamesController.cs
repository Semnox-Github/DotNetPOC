/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - Game  API : Gets the Game details
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.ThirdParty.CenterEdge;

namespace Semnox.CommonAPI.ThirdParty.CenterEdge
{
    public class GamesController : ApiController
    {
        private static readonly Parafait.logging.Logger log = new Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object: This loads the machine list as per parafait 
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [Route("api/ThirdParty/CenterEdge/Games")]
        [Authorize]
        public HttpResponseMessage Get(int skip =0 ,int take =100)
        {
            log.LogMethodEntry(skip,take);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                GamesListBL gamesListBL = new GamesListBL(executionContext);
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, gamesListBL.GetMachines(skip,take));
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { code = ErrorCode.badRequest.ToString(), message = customException });
            }
        }
    }
}
