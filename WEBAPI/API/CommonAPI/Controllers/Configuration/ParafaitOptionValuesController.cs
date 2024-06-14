/********************************************************************************************
 * Project Name - Products Controller/ParafaitOptionsController
 * Description  - Created to fetch, update and insert product details in the Parafait Options 
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60        19-Mar-2019   Muhammed Mehraj          Created to get, insert, update and Delete Methods.
 ***************************************************************************************************
 *2.60        03-Apr-2019   Akshay Gulaganji         added CustomGenericException, log.LogMethodEntry() and log.LogMethodExit()
 *2.110.0     10-Nov-2020   Vikas Dwivedi            Modified as per the REST API Standards.
 ****************************************************************************************************/
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
    public class ParafaitOptionValuesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the Parafait Option Values.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="posmachineId"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/ParafaitOptionValues/")]
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
                searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
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
                ParafaitOptionValuesCustomBL parafaitOptionValuesCustomBL = new ParafaitOptionValuesCustomBL(executionContext);
                List<ParafaitOptionValuesCustomPropertiesDTO> parafaitOptionValuesCustomPropertiesDTOList = parafaitOptionValuesCustomBL.GetParafaitOptionValues(searchParameters);
                if (parafaitOptionValuesCustomPropertiesDTOList != null)
                {
                    parafaitOptionValuesCustomPropertiesDTOList = parafaitOptionValuesCustomPropertiesDTOList.OrderBy(m => m.ScreenGroup).ToList();
                }
                log.LogMethodExit(parafaitOptionValuesCustomPropertiesDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = parafaitOptionValuesCustomPropertiesDTOList });
            }    
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Object Parafait Option Values
        /// </summary>
        /// <param name="parafaitOptionValuesListBL">parafaitOptionValuesListBL</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Configuration/ParafaitOptionValues")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ParafaitOptionValuesCustomPropertiesDTO> parafaitOptionValuesList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(parafaitOptionValuesList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (parafaitOptionValuesList != null && parafaitOptionValuesList.Any())
                {
                    ParafaitOptionValuesCustomBL parafaitOptionValuesListBL = new ParafaitOptionValuesCustomBL(executionContext);
                    parafaitOptionValuesListBL.SaveUpdateParafaitValueList(parafaitOptionValuesList);
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
