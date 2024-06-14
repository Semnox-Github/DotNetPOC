/********************************************************************************************
 * Project Name - Products Controller/AttractionPlaysController
 * Description  - Created to fetch, update and insert in the Attraction Plays.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Created By               Remarks          
 ***************************************************************************************************
 *2.60        04-Feb-2019   Nagesh Badiger          Created to get, insert, update and Delete Methods. 
 *2.70        27-Jun-2019  Akshay Gulaganji         modified Delete method.
 *2.80        21-Apr-2020  Girish Kundar            modified as per REST API standard.
  *2.100.0     10-Sep-2020   Vikas Dwivedi           Modified as per the REST API Standards.
  * ***************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class AttractionPlaysController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the AttractionPlays.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/AttractionPlays")]
        public HttpResponseMessage Get(string isActive = null, int attractionPlayId = -1, string attractionName = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, attractionPlayId, attractionName);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>> searchParameters = new List<KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>>();
                searchParameters.Add(new KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>(AttractionPlaysDTO.SearchByAttractionPlaysParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>(AttractionPlaysDTO.SearchByAttractionPlaysParameters.IS_ACTIVE, isActive.ToString()));
                    }
                }
                if (attractionPlayId > -1)
                {
                    searchParameters.Add(new KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>(AttractionPlaysDTO.SearchByAttractionPlaysParameters.ID, attractionPlayId.ToString()));
                }
                if (string.IsNullOrEmpty(attractionName) == false)
                {
                    searchParameters.Add(new KeyValuePair<AttractionPlaysDTO.SearchByAttractionPlaysParameters, string>(AttractionPlaysDTO.SearchByAttractionPlaysParameters.PLAYNAME, attractionName));
                }
                AttractionPlaysBLList attractionPlaysBLList = new AttractionPlaysBLList(executionContext);
                List<AttractionPlaysDTO> attractionPlaysDTOList = attractionPlaysBLList.GetAttractionPlaysDTOList(searchParameters);
                log.LogMethodExit(attractionPlaysDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = attractionPlaysDTOList });
            }
            catch (Exception ex) 
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }

        /// <summary>
        /// Post the JSON Object AttractionPlays
        /// </summary>
        /// <param name="attractionPlaysList">AttractionPlaysList</param>
        /// <returns>HttpMessgae</returns>
        [HttpPost]
        [Route("api/Product/AttractionPlays")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<AttractionPlaysDTO> attractionPlayDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(attractionPlayDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (attractionPlayDTOList != null && attractionPlayDTOList.Any())
                {
                    AttractionPlaysBLList attractionPlaysBL = new AttractionPlaysBLList(executionContext, attractionPlayDTOList);
                    attractionPlaysBL.SaveUpdateAttractionPlaysList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }

        /// <summary>
        /// Delete the Attraction Plays
        /// </summary>        
        /// <returns>HttpMessgae</returns>
        [HttpDelete]
        [Route("api/Product/AttractionPlays")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<AttractionPlaysDTO> attractionPlayDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(attractionPlayDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (attractionPlayDTOList != null && attractionPlayDTOList.Any())
                {
                    AttractionPlaysBLList attractionPlayList = new AttractionPlaysBLList(executionContext, attractionPlayDTOList);
                    attractionPlayList.DeleteAttractionPlaysList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
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
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }
    }
}
