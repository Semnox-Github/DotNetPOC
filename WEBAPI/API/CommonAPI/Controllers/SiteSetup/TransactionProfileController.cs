/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the transaction profiles Controller class
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         14-Mar-2019   Jagan Mohana         Created 
               08-Apr-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                  Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
 ********************************************************************************************/
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using System;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.SiteSetup
{
    public class TransactionProfileController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object TransactionProfiles Details List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/TransactionProfile/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry();

                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<TransactionProfileDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<TransactionProfileDTO.SearchByParameters, string>(TransactionProfileDTO.SearchByParameters.IS_ACTIVE, isActive));
                }
                TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL(executionContext);
                var content = transactionProfileListBL.GetTransactionProfileDTOList(searchParameters, true);
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
        /// Performs a Post operation on TransactionProfiles  details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/TransactionProfile/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<TransactionProfileDTO> transactionProfileDTOList)
        {
            try
            {
                log.LogMethodEntry(transactionProfileDTOList);

                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;
                if (transactionProfileDTOList != null)
                {
                    // if TransactionProfileDTOs.transactionProfileId is less than zero then insert or else update
                    TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL(executionContext, transactionProfileDTOList);
                    transactionProfileListBL.SaveUpdateTransactionProfilesList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodEntry();
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
        /// Performs a Delete operation on TransactionProfiles details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/TransactionProfile/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<TransactionProfileDTO> transactionProfileDTOList)
        {
            try
            {
                log.LogMethodEntry(transactionProfileDTOList);

                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); 
                if (transactionProfileDTOList != null && transactionProfileDTOList.Count != 0)
                {
                    TransactionProfileListBL transactionProfileListBL = new TransactionProfileListBL(executionContext, transactionProfileDTOList);
                    transactionProfileListBL.DeleteTransactionProfilesList();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
                }
                else
                {
                    log.LogMethodEntry();
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
