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
 ***************************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.Products
{
    public class ParafaitOptionsController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the ParafaitOptions.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/ParafaitOptions/")]
        public HttpResponseMessage Get(string isActive, int posMachineId = -1, int userId = -1)
        {
            try
            {
                log.LogMethodEntry(isActive);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
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
               
                //if (isActive.ToString() == "1")
                //{
                //    searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                //}
                ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext);
                var content = parafaitOptionValuesListBL.GetParafaitOptionValuesDTOList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Object ParafaitOptions
        /// </summary>
        /// <param name="parafaitOptionValuesListBL">parafaitOptionValuesListBL</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Products/ParafaitOptions/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList)
        {
            try
            {
                log.LogMethodEntry(parafaitOptionValuesDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (parafaitOptionValuesDTOList != null && parafaitOptionValuesDTOList.Count != 0)
                {
                    ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext, parafaitOptionValuesDTOList);
                    parafaitOptionValuesListBL.SaveUpdateParafaitOptionValuesDTOList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
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
        [Route("api/Products/ParafaitOptions/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<ParafaitOptionValuesDTO> parafaitOptionValuesDTOList)
        {
            try
            {
                log.LogMethodEntry(parafaitOptionValuesDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (parafaitOptionValuesDTOList != null || parafaitOptionValuesDTOList.Count != 0)
                {
                    ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext, parafaitOptionValuesDTOList);
                    parafaitOptionValuesListBL.DeleteParafaitOptionValuesDTOList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
                }
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }
    }
}
