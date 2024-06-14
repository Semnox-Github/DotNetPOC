/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Hubs
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
*2.110.0      05-Feb-2021   Fiona          Created.
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
    public class MachineGroupsCountController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Get the JSON Object Hub Details List
        /// </summary>       
        /// <param name="isActive">isActive</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("api/Game/MachineGroupsCount")]
        [Authorize]
        public async Task<HttpResponseMessage> Get(string isActive = null, int machineGroupId = -1, string groupName = null)
        {


            log.LogMethodEntry(isActive, machineGroupId, groupName);
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MachineGroupsDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.ISACTIVE, isActive.ToString()));
                    }
                }
                if (machineGroupId > 0)
                {
                    searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.MACHINE_GROUP_ID, machineGroupId.ToString()));
                }
                if (!string.IsNullOrEmpty(groupName))
                {
                    searchParameters.Add(new KeyValuePair<MachineGroupsDTO.SearchByParameters, string>(MachineGroupsDTO.SearchByParameters.GROUP_NAME, groupName.ToString()));
                }

                IMachineGroupsUseCases machineGroupsUseCases = GameUseCaseFactory.GetMachineGroupsUseCases(executionContext);
                int totalCount = await machineGroupsUseCases.GetMachineGroupsCount(searchParameters);
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