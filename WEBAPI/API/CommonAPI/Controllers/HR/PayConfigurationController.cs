/********************************************************************************************
* Project Name - CommnonAPI - HR Module 
* Description  - API for the PayConfiguration Controller.
* 
**************
**Version Log
**************
*Version     Date            Modified By         Remarks          
*********************************************************************************************
*2.100.0     17-Aug-2020     Vikas Dwivedi       Created
*2.120.0     01-Apr-2021   Prajwal S             Modified.
********************************************************************************************/
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Semnox.CommonAPI.HR
{
    public class PayConfigurationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Performs a Get operation 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/HR/PayConfigurations")]
        public async Task<HttpResponseMessage> Get(int payConfigurationId = -1, string payConfigurationName = null, int payTypeId = -1, string isActive = null, bool buildChildRecords = false, bool loadActiveChild = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(payConfigurationId, payConfigurationName, payTypeId, isActive, buildChildRecords, loadActiveChild);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>> payConfigurationsSearchParameters = new List<KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>>();
                payConfigurationsSearchParameters.Add(new KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>(PayConfigurationsDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (payConfigurationId > -1)
                {
                    payConfigurationsSearchParameters.Add(new KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>(PayConfigurationsDTO.SearchByParameters.PAY_CONFIGURATION_ID, payConfigurationId.ToString()));
                }
                if (!string.IsNullOrEmpty(payConfigurationName))
                {
                    payConfigurationsSearchParameters.Add(new KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>(PayConfigurationsDTO.SearchByParameters.PAY_CONFIGURATION_NAME, payConfigurationName.ToString()));
                }
                if (payTypeId > -1)
                {
                    payConfigurationsSearchParameters.Add(new KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>(PayConfigurationsDTO.SearchByParameters.PAY_TYPE_ID, payTypeId.ToString()));
                }
                if (!string.IsNullOrEmpty(isActive))
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        payConfigurationsSearchParameters.Add(new KeyValuePair<PayConfigurationsDTO.SearchByParameters, string>(PayConfigurationsDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                IPayConfigurationsUseCases payConfigurationsUseCases = UserUseCaseFactory.GetPayConfigurationsUseCases(executionContext);
                List<PayConfigurationsDTO> payConfigurationsDTOLists = await payConfigurationsUseCases.GetPayConfigurations(payConfigurationsSearchParameters, buildChildRecords, loadActiveChild);
                log.LogMethodExit(payConfigurationsDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = payConfigurationsDTOLists,
                });
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
        /// <param name="payConfigurationsDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("api/HR/PayConfigurations")]
        public async Task<HttpResponseMessage> Post([FromBody] List<PayConfigurationsDTO> payConfigurationsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(payConfigurationsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (payConfigurationsDTOList == null || payConfigurationsDTOList.Any(a => a.PayConfigurationId > -1))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IPayConfigurationsUseCases payConfigurationsUseCases = UserUseCaseFactory.GetPayConfigurationsUseCases(executionContext);
                List<PayConfigurationsDTO> payConfigurationsDTOLists = await payConfigurationsUseCases.SavePayConfigurations(payConfigurationsDTOList);
                log.LogMethodExit(payConfigurationsDTOLists);
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    data = payConfigurationsDTOLists,
                });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Post the PayConfigurationsList collection
        /// <param name="payConfigurationsDTOList">PayConfigurationsList</param>
        [HttpPut]
        [Route("api/HR/PayConfigurations")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody]List<PayConfigurationsDTO> payConfigurationsDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(payConfigurationsDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (payConfigurationsDTOList == null || payConfigurationsDTOList.Any(a => a.PayConfigurationId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IPayConfigurationsUseCases payConfigurationsUseCases = UserUseCaseFactory.GetPayConfigurationsUseCases(executionContext);
                payConfigurationsDTOList = await payConfigurationsUseCases.SavePayConfigurations(payConfigurationsDTOList);
                log.LogMethodExit(payConfigurationsDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = payConfigurationsDTOList });
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
