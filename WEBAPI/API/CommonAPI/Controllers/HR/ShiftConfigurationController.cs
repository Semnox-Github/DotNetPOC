/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the ShiftConfiguration Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.100.0     17-Aug-2020     Vikas Dwivedi       Created
*2.120.0     01-Apr-2021     Prajwal S           Modified.
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
using Semnox.Parafait.User;

namespace Semnox.CommonAPI.HR
{
    public class ShiftConfigurationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/ShiftConfigurations")]
        public async Task<HttpResponseMessage> Get(int shiftConfigurationId = -1, string shiftConfigurationName = null, bool shiftTrackAllowed = false, bool overtimeAllowed = false, string isActive = null, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(shiftConfigurationId, shiftConfigurationName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>> shiftConfigurationsSearchParameters = new List<KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>>();
                shiftConfigurationsSearchParameters.Add(new KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>(ShiftConfigurationsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (shiftConfigurationId > -1)
                {
                    shiftConfigurationsSearchParameters.Add(new KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>(ShiftConfigurationsDTO.SearchByParameters.SHIFT_CONFIGURATION_ID, shiftConfigurationId.ToString()));
                }
                if (!string.IsNullOrEmpty(shiftConfigurationName))
                {
                    shiftConfigurationsSearchParameters.Add(new KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>(ShiftConfigurationsDTO.SearchByParameters.SHIFT_CONFIGURATION_NAME, shiftConfigurationName.ToString()));
                }
                if (shiftTrackAllowed)
                {
                    shiftConfigurationsSearchParameters.Add(new KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>(ShiftConfigurationsDTO.SearchByParameters.SHIFT_TRACK_ALLOWED, shiftTrackAllowed.ToString()));
                }
                if (overtimeAllowed)
                {
                    shiftConfigurationsSearchParameters.Add(new KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>(ShiftConfigurationsDTO.SearchByParameters.OVERTIME_ALLOWED, overtimeAllowed.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        shiftConfigurationsSearchParameters.Add(new KeyValuePair<ShiftConfigurationsDTO.SearchByParameters, string>(ShiftConfigurationsDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IShiftConfigurationsUseCases shiftConfigurationsUseCases = UserUseCaseFactory.GetShiftConfigurationUseCases(executionContext);
                List<ShiftConfigurationsDTO> shiftConfigurationsDTOList = await shiftConfigurationsUseCases.GetShiftConfigurations(shiftConfigurationsSearchParameters);
                log.LogMethodExit(shiftConfigurationsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = shiftConfigurationsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Performs a Post operation 
        /// </summary>
        /// <param name="shiftConfigurationsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/ShiftConfigurations")]
        public async Task<HttpResponseMessage> Post([FromBody] List<ShiftConfigurationsDTO> shiftConfigurationsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(shiftConfigurationsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (shiftConfigurationsDTOList == null || shiftConfigurationsDTOList.Any(a => a.ShiftConfigurationId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (shiftConfigurationsDTOList != null && shiftConfigurationsDTOList.Any())
                {
                    IShiftConfigurationsUseCases shiftConfigurationsUseCases = UserUseCaseFactory.GetShiftConfigurationUseCases(executionContext);
                    List<ShiftConfigurationsDTO> shiftConfigurationsDTOLists = await shiftConfigurationsUseCases.SaveShiftConfigurations(shiftConfigurationsDTOList);
                    log.LogMethodExit(shiftConfigurationsDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = shiftConfigurationsDTOLists });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the ShiftConfigurationsList collection
        /// <param name="shiftConfigurationsDTOList">ShiftConfigurationsList</param>
        [HttpPut]
        [Route("api/HR/ShiftConfigurations")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<ShiftConfigurationsDTO> shiftConfigurationsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(shiftConfigurationsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (shiftConfigurationsDTOList == null || shiftConfigurationsDTOList.Any(a => a.ShiftConfigurationId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IShiftConfigurationsUseCases shiftConfigurationsUseCases = UserUseCaseFactory.GetShiftConfigurationUseCases(executionContext);
                shiftConfigurationsDTOList = await shiftConfigurationsUseCases.SaveShiftConfigurations(shiftConfigurationsDTOList);
                log.LogMethodExit(shiftConfigurationsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = shiftConfigurationsDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
    }
}
