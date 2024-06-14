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

namespace Semnox.CommonAPI.SiteSetup
{
    public class SequenceController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/Sequence/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                SequencesListBL sequencesListBL = new SequencesListBL(executionContext);
                List<KeyValuePair<SequencesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<SequencesDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<SequencesDTO.SearchByParameters, string>(SequencesDTO.SearchByParameters.ISACTIVE, isActive));
                }
                var content = sequencesListBL.GetAllSequencesList(searchParameters);
                log.LogMethodExit(content);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }
        /// <summary>
        /// Performs a Post operation on SequencesDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/Sequence/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<SequencesDTO> sequencesDTOList)
        {
            try
            {
                log.LogMethodEntry(sequencesDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (sequencesDTOList != null)
                {
                    // if sequencesDTOList.sequenceId is less than zero then insert or else update
                    SequencesListBL sequencesListBL = new SequencesListBL(executionContext, sequencesDTOList);
                    sequencesListBL.SaveUpdateSequencesList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
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
        /// Performs a Delete operation on SequencesDTOs details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/Sequence/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<SequencesDTO> sequencesDTOList)
        {
            try
            {
                log.LogMethodEntry(sequencesDTOList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (sequencesDTOList != null)
                {
                    SequencesListBL sequencesListBL = new SequencesListBL(executionContext, sequencesDTOList);
                    sequencesListBL.SaveUpdateSequencesList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.NotFound, new { data = "", token = securityTokenDTO.Token });
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
