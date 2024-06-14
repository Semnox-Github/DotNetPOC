/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Invoice Sequence Mapping  details list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         13-May-2019   Mushahid Faizan          Created 
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
    public class InvoiceSequenceMappingController : ApiController
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
        [Route("api/SiteSetup/InvoiceSequenceMapping/")]
        public HttpResponseMessage Get(string isActive, string sequenceId)
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(executionContext);
                List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.SEQUENCE_ID, sequenceId));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, isActive));
                }
                var content = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);
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
        /// Performs a Post operation on InvoiceSequenceMappingDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/InvoiceSequenceMapping/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<InvoiceSequenceMappingDTO> invoiceSequenceMappingList)
        {
            try
            {
                log.LogMethodEntry(invoiceSequenceMappingList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (invoiceSequenceMappingList != null)
                {
                    // if invoiceSequenceMappingList.Id is less than zero then insert or else update
                    InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(executionContext, invoiceSequenceMappingList);
                    invoiceSequenceMappingListBL.SaveUpdateSequenceMappingList();
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
        /// Performs a Delete operation on InvoiceSequenceMappingDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/InvoiceSequenceMapping/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<InvoiceSequenceMappingDTO> invoiceSequenceMappingList)
        {
            try
            {
                log.LogMethodEntry(invoiceSequenceMappingList);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (invoiceSequenceMappingList != null)
                {
                    // if invoiceSequenceMappingList.Id is less than zero then insert or else update
                    InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(executionContext, invoiceSequenceMappingList);
                    invoiceSequenceMappingListBL.SaveUpdateSequenceMappingList();
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
