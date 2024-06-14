/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Sequence  details list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         17-Mar-2019   Jagan Mohana         Created 
               13-May-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
                                                  Added isActive SearchParameter in HttpGet Method.
                                                  Added HttpDeleteMethod, Modified HttpPost Method.
*2.90         11-May-2020   Girish Kundar         Modified : Moved to Configuration and Changes as part of the REST API  
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;

namespace Semnox.CommonAPI.Controllers.Configuration
{
    public class SequenceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/Sequences")]
        public HttpResponseMessage Get(string isActive = null, int sequenceId = -1, string sequenceName = null, int orderTypeGroupId = -1, int posMachineId = -1)
        {

            log.LogMethodEntry(isActive, sequenceId, sequenceName, orderTypeGroupId, posMachineId);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                SequencesListBL sequencesListBL = new SequencesListBL(executionContext);
                List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SequencesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.ISACTIVE, isActive));
                    }
                }
                if (sequenceId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SEQUENCE_ID, sequenceId.ToString()));
                }
                if (string.IsNullOrEmpty(sequenceName) == false)
                {
                    searchParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SEQUENCE_NAME, sequenceName));
                }
                if (orderTypeGroupId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.ORDER_TYPE_GROUP_ID, orderTypeGroupId.ToString()));
                }
                if (posMachineId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.POS_MACHINE_ID, posMachineId.ToString()));
                }
                var content = sequencesListBL.GetAllSequencesList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Performs a Post operation on SequencesDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Configuration/Sequences")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<SequencesDTO> sequencesDTOList)
        {
            log.LogMethodEntry(sequencesDTOList);
            SecurityTokenBL securityTokenBL = new SecurityTokenBL();
            securityTokenBL.GenerateJWTToken();
            SecurityTokenDTO securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
            ExecutionContext executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
            try
            {
                if (sequencesDTOList != null && sequencesDTOList.Any())
                {
                    SequencesListBL sequencesListBL = new SequencesListBL(executionContext, sequencesDTOList);
                    sequencesListBL.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "" });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "" });
                }
            }
            catch (ValidationException vexp)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(vexp, executionContext);
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
