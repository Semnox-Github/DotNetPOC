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
 ****************************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Semnox.CommonAPI.Games.Controllers.Products
{
    public class ParafaitOptionValuesController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private SecurityTokenDTO securityTokenDTO;
        private ExecutionContext executionContext;

        /// <summary>
        /// Get the Parafait Option Values.
        /// </summary>
        /// <param name="isActive"></param>
        /// <param name="posmachineId"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Products/ParafaitOptionValues/")]
        public HttpResponseMessage Get(string isActive, int posMachineId = -1, int userId = -1)
        {
            try
            {
                log.LogMethodEntry(isActive, posMachineId, userId);
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
                if (isActive == "1")
                {                
                    searchParameters.Add(new KeyValuePair<ParafaitOptionValuesDTO.SearchByParameters, string>(ParafaitOptionValuesDTO.SearchByParameters.IS_ACTIVE, isActive.ToString()));
                }
                ParafaitOptionValuesCustomBL parafaitOptionValuesCustomBL = new ParafaitOptionValuesCustomBL(executionContext);
                List<ParafaitOptionValuesCustomPropertiesDTO> content = parafaitOptionValuesCustomBL.GetParafaitOptionValues(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content.OrderBy(m=>m.ScreenGroup), token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Object Parafait Option Values
        /// </summary>
        /// <param name="parafaitOptionValuesListBL">parafaitOptionValuesListBL</param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Products/ParafaitOptionValues/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<ParafaitOptionValuesCustomPropertiesDTO> parafaitOptionValuesList)
        {
            try
            {
                log.LogMethodEntry(parafaitOptionValuesList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (parafaitOptionValuesList != null && parafaitOptionValuesList.Count != 0)
                {
                    ParafaitOptionValuesCustomBL parafaitOptionValuesListBL = new ParafaitOptionValuesCustomBL(executionContext);
                    parafaitOptionValuesListBL.SaveUpdateParafaitValueList(parafaitOptionValuesList);
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

        ///// <summary>
        ///// Delete theParafaitOptionValues
        ///// </summary>        
        ///// <returns>HttpMessgae</returns>
        //[HttpDelete]
        //[Route("api/Products/ParafaitOptionValues/")]
        //[Authorize]
        //public HttpResponseMessage Delete([FromBody]List<ParafaitOptionValuesDTO> attractionPlayDTOList)
        //{
        //    SecurityTokenDTO securityTokenDTO = null;
        //    try
        //    {
        //        log.Debug("ParafaitOptionsController-Delete() Method.");
        //        SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        //        securityTokenBL.GenerateJWTToken();
        //        securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
        //        ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

        //        if (attractionPlayDTOList.Count != 0)
        //        {
        //            ParafaitOptionValuesListBL parafaitOptionValuesListBL = new ParafaitOptionValuesListBL(executionContext);
        //            parafaitOptionValuesListBL.SaveUpdateParafaitOptionValuesDTOList();
        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
        //        }
        //        else
        //        {
        //            log.Debug("ParafaitOptionsController-Delete() Method.");
        //            return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "NotFound", token = securityTokenDTO.Token });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Debug("ParafaitOptionsController Delete() - Error: " + ex.Message);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
        //    }
        //}
    }
}
