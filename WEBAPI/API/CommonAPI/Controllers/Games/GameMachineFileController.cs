/********************************************************************************************
 * Project Name - Common API                                                                    
 * Description  - API to GET Image files for the game machine
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By        Remarks          
 ********************************************************************************************* 
  *2.110.0     01-Feb-2021   Girish Kundar    Created : Virtual Arcade changes
 ********************************************************************************************/


using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;
using Semnox.Parafait.Game.VirtualArcade;
using Semnox.Parafait.Transaction;
namespace Semnox.CommonAPI.Games
{
    /// <summary>
    /// GameMachineImageController
    /// </summary>
    public class GameMachineImageController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Game Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Game/Files")]
        public async Task<HttpResponseMessage> Get(string machineName = null, string fileName = null)
        {
            log.LogMethodEntry(machineName, fileName);

            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (string.IsNullOrWhiteSpace(machineName) || string.IsNullOrWhiteSpace(fileName))
                {
                    string customException = "Please send required parameters";
                    log.Error(customException);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                }

                IVirtualArcadeUseCases virtualArcadeUseCases = GameUseCaseFactory.GetVirtualArcadeUseCases(executionContext);
                string file = await virtualArcadeUseCases.GetGameMachineFile(machineName, fileName);
                if (fileName.Contains(".json"))
                {
                    object json = JsonConvert.DeserializeObject(file);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = json });
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = file });
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