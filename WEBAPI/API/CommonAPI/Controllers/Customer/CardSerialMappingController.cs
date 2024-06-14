/********************************************************************************************
 * Project Name - CommnonAPI
 * Description  - API for the Cards "Bulk Upload Cards" entity. 
 * 
 **************
 **Version Log
 **************
 *Version     Date            Modified By         Remarks          
 *********************************************************************************************
 *2.60        4-Mar-2019     Nagesh Badiger      Created
 *2.70.0      17-Sept-2019   Jagan Mohana        Renamed BulkUploadCardsController to TagInventoryController
 *2.80         05-Apr-2020   Girish Kundar        Modified: API path changes
 *2.150.6     14-Dec-2023    Abhishek            Modified: API to get the card serial mapping details.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Customer;

namespace Semnox.CommonAPI.Customer
{
    public class CardSerialMappingController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object BulkUploadCards Collections.
        /// </summary>
        /// <param name="tagSerialMappingDTOList"></param>
        [HttpGet]
        [Route("api/Customer/CardSerialMappings")]
        [Authorize]
        public HttpResponseMessage Get(string serialNumber = null, string cardNumber = null, string serialNumberFrom = null, string serialNumberTo = null)
        {
            try
            {
                log.LogMethodEntry(serialNumber, cardNumber, serialNumberFrom, serialNumberTo);
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));

                if (!string.IsNullOrEmpty(serialNumber))
                {
                    searchParameters.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER, serialNumber));
                }
                if (!string.IsNullOrEmpty(cardNumber))
                {
                    searchParameters.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.TAG_NUMBER, cardNumber));
                }
                if (!string.IsNullOrEmpty(serialNumberFrom))
                {
                    searchParameters.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER_FROM, serialNumberFrom));
                }
                if (!string.IsNullOrEmpty(serialNumberTo))
                {
                    searchParameters.Add(new KeyValuePair<TagSerialMappingDTO.SearchByParameters, string>(TagSerialMappingDTO.SearchByParameters.SERIAL_NUMBER_TO, serialNumberTo));
                }

                TagSerialMappingListBL tagSerialMappingListBL = new TagSerialMappingListBL(executionContext);
                List<TagSerialMappingDTO> tagSerialMappingDTOList = tagSerialMappingListBL.GetTagSerialMappingDTOList(searchParameters);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = tagSerialMappingDTOList });
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

        /// <summary>
        /// Post the JSON Object BulkUploadCards Collections.
        /// </summary>
        /// <param name="tagSerialMappingDTOList"></param>
        [HttpPost]
        [Route("api/Customer/CardSerialMappings")]
        [Authorize]
        public HttpResponseMessage Post([FromBody]List<TagSerialMappingDTO> tagSerialMappingDTOList)
        {
            try
            {
                log.LogMethodEntry(tagSerialMappingDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (tagSerialMappingDTOList.Count != 0)
                {
                    TagSerialMappingListBL tagSerialMappingListBL = new TagSerialMappingListBL(tagSerialMappingDTOList, executionContext);
                    var content = tagSerialMappingListBL.SaveBulkUploadCards();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
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

        /// <summary>
        /// Delete the JSON Object BulkUploadCards Collections.
        /// </summary>
        /// <param name="tagSerialMappingDTOList"></param>
        [HttpDelete]
        [Route("api/Customer/CardSerialMappings")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody]List<TagSerialMappingDTO> tagSerialMappingDTOList)
        {
            try
            {
                log.LogMethodEntry(tagSerialMappingDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (tagSerialMappingDTOList.Count != 0)
                {
                    TagSerialMappingListBL tagSerialMappingListBL = new TagSerialMappingListBL(tagSerialMappingDTOList, executionContext);
                    var content = tagSerialMappingListBL.SaveBulkUploadCards();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
                }
                else
                {
                    log.LogMethodExit(tagSerialMappingDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
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
