/********************************************************************************************
 * Project Name - Products Controller/ParafaitOptionsController
 * Description  - Created to fetch, update and insert product details in the Parafait Options 
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60        02-Apr-2019   Nagesh Badiger          Created 
 ****************************************************************************************************
 *2.60        03-Apr-2019   Akshay Gulaganji        added posMachineId parameter to Get() method
 *2.60        08-May-2019   Mushahid Faizan         Added userId parameter in Get() method.
 *2.70        29-Jun-2019   Akshay Gulaganji         modified Delete() method.
 *2.110.0     10-Nov-2020   Vikas Dwivedi            Modified as per the REST API Standards.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;

namespace Semnox.CommonAPI.Configuration
{
    public class ParafaitOptionsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the ParafaitOptions.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/ParafaitOptions")]
        public HttpResponseMessage Get(int posMachineId = -1, int userId = -1, int optionId = -1, int optionValueId = -1, string optionValue = null, string isActive = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(posMachineId, userId, optionId, optionValueId, optionValue, isActive);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (posMachineId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.POSMACHINEID, posMachineId.ToString()));
                }
                if (userId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.USER_ID, userId.ToString()));
                }
                if (optionId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.OPTION_ID, optionId.ToString()));
                }
                if (optionValueId > 0)
                {
                    searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.OPTION_VALUE_ID, optionValueId.ToString()));
                }
                if (!string.IsNullOrEmpty(optionValue))
                {
                    searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.USER_ID, optionValue));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.IS_ACTIVE, isActive));
                    }
                }
                ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext);
                var content = parafaitOptionValuesListBL.GetParafaitOptionValuesDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object ParafaitOptions
        /// </summary>
        /// <param name="parafaitOptionValuesListBL">parafaitOptionValuesListBL</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Configuration/ParafaitOptions")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(parafaitOptionValuesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (parafaitOptionValuesDTOList != null && parafaitOptionValuesDTOList.Any())
                {
                    ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext, parafaitOptionValuesDTOList);
                    parafaitOptionValuesListBL.SaveUpdateParafaitOptionValuesDTOList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Delete the ParafaitOptions
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Configuration/ParafaitOptions")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(parafaitOptionValuesDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (parafaitOptionValuesDTOList != null && parafaitOptionValuesDTOList.Any())
                {
                    ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext, parafaitOptionValuesDTOList);
                    parafaitOptionValuesListBL.DeleteParafaitOptionValuesDTOList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException valEx)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
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
