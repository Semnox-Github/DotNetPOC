/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Hubs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
*2.110.0      04-Feb-2021   Fiona          Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.CommonAPI.Controllers.Games
{
    public class MachineCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object Hub Details List
        /// </summary>       
        /// <param name="isActive">isActive</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("api/Game/MachineCount")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = "Y", int machineId = -1,
                                       int gameId = -1, int masterId = -1, int referenceMachineId = -1, string machineName = null,
                                       string externalMachineReference = null)
        {
           

            
                log.LogMethodEntry(isActive, machineId, gameId, masterId, referenceMachineId, machineName, externalMachineReference);
                SecurityTokenDTO securityTokenDTO = null;
                ExecutionContext executionContext = null;
                try
                {
                    SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                    securityTokenBL.GenerateJWTToken();
                    securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                    executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                    List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, executionContext.SiteId.ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, isActive));
                    }
                }
                if (machineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_ID, machineId.ToString()));
                }
                if (gameId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.GAME_ID, gameId.ToString()));
                }
                if (masterId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MASTER_ID, masterId.ToString()));
                }
                if (referenceMachineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.REFERENCE_MACHINE_ID, referenceMachineId.ToString()));
                }
                if (string.IsNullOrEmpty(machineName) == false)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.MACHINE_NAME, machineName.ToString()));
                }
                if (string.IsNullOrEmpty(externalMachineReference) == false)
                {
                    searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.EXTERNAL_MACHINE_REFERENCE, externalMachineReference));
                }

                IMachineUseCases machineUseCases = GameUseCaseFactory.GetMachineUseCases(executionContext);
                int totalCount = await machineUseCases.GetMachineCount(searchParameters);

                log.LogMethodExit(totalCount);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = totalCount });
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