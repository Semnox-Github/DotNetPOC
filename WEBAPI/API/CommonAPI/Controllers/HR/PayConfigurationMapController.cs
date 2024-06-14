/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the PayConfigurationMap Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.100.0     17-Aug-2020     Vikas Dwivedi       Created
*2.120.0     01-Apr-2021   Prajwal S             Modified.
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class PayConfigurationMapController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/PayConfigurationMaps")]
        public async Task<HttpResponseMessage> Get(int payConfigurationMapId = -1, int userRoleId = -1, int userId = -1, int payConfigurationId = -1, DateTime? effectiveDate = null, DateTime? endDate = null, string isActive = null, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(payConfigurationMapId, userRoleId, userId, isActive, loadActiveChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>> payConfigurationMapSearchParameters = new List<KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>>();
                payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (payConfigurationMapId > -1)
                {
                    payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_ID, payConfigurationMapId.ToString()));
                }
                if (userRoleId > -1)
                {
                    payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.USER_ROLE_ID, userRoleId.ToString()));
                }
                if (userId > -1)
                {
                    payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.USER_ID, userId.ToString()));
                }
                if (payConfigurationId > -1)
                {
                    payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.PAY_CONFIGURATION_ID, payConfigurationId.ToString()));
                }
                if (effectiveDate != null)
                {
                    DateTime userEffectiveDate = Convert.ToDateTime(effectiveDate);
                    payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.EFFECTIVE_DATE_GREATER_THAN, userEffectiveDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (endDate != null)
                {
                    DateTime userEndDate = Convert.ToDateTime(endDate);
                    payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.END_DATE_LESS_THAN_OR_EQUALS, userEndDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        payConfigurationMapSearchParameters.Add(new KeyValuePair<PayConfigurationMapDTO.SearchByParameters, string>(PayConfigurationMapDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IPayConfigurationMapUseCases payConfigurationMapUseCases = UserUseCaseFactory.GetPayConfigurationMapUseCases(executionContext);
                List<PayConfigurationMapDTO> payConfigurationMapDTOList = await payConfigurationMapUseCases.GetPayConfigurationMap(payConfigurationMapSearchParameters);
                log.LogMethodExit(payConfigurationMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = payConfigurationMapDTOList });
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
        /// <param name="payConfigurationMapDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/PayConfigurationMaps")]
        public async Task<HttpResponseMessage> Post([FromBody] List<PayConfigurationMapDTO> payConfigurationMapDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(payConfigurationMapDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (payConfigurationMapDTOList == null || payConfigurationMapDTOList.Any(a => a.PayConfigurationMapId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                if (payConfigurationMapDTOList != null && payConfigurationMapDTOList.Any())
                {
                    IPayConfigurationMapUseCases payConfigurationMapUseCases = UserUseCaseFactory.GetPayConfigurationMapUseCases(executionContext);
                    List<PayConfigurationMapDTO> payConfigurationMapDTOLists = await payConfigurationMapUseCases.SavePayConfigurationMap(payConfigurationMapDTOList);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = payConfigurationMapDTOLists });
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
        /// Post the PayConfigurationMapList collection
        /// <param name="payConfigurationMapDTOList">PayConfigurationMapList</param>
        [HttpPut]
        [Route("api/HR/PayConfigurationMaps")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<PayConfigurationMapDTO> payConfigurationMapDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(payConfigurationMapDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (payConfigurationMapDTOList == null || payConfigurationMapDTOList.Any(a => a.PayConfigurationMapId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IPayConfigurationMapUseCases payConfigurationMapUseCases = UserUseCaseFactory.GetPayConfigurationMapUseCases(executionContext);
                payConfigurationMapDTOList = await payConfigurationMapUseCases.SavePayConfigurationMap(payConfigurationMapDTOList);
                log.LogMethodExit(payConfigurationMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = payConfigurationMapDTOList });
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
