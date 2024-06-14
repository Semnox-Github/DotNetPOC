/********************************************************************************************
 * Project Name - Common API                                                                    
 * Description  - API to GET trnslation files for the game
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Game.VirtualArcade;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Transaction.VirtualArcade;

namespace Semnox.CommonAPI.Games
{
    public class GameMachineTranslationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Game Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/GameMachineTranslations")]
        public async Task<HttpResponseMessage> Get(string machineName = null, string fileName = null)
        {
            log.LogMethodEntry(machineName);

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (string.IsNullOrWhiteSpace(machineName))
                {
                    string customException = "Please send required parameters";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }
                IVirtualArcadeUseCases virtualArcadeUseCases = GameUseCaseFactory.GetVirtualArcadeUseCases(executionContext);
                List<string> fileNames = await virtualArcadeUseCases.GetGameMachineTranslations(machineName, fileName);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = fileNames });
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
