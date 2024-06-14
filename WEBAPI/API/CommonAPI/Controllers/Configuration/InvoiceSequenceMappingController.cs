/********************************************************************************************
 * Project Name - Site Setup                                                                     
 * Description  - Controller of the Invoice Sequence Mapping  details list
 *
 **************
 **Version Log
  *Version     Date          Modified By          Remarks          
 *********************************************************************************************
 *2.60         13-May-2019   Mushahid Faizan          Created 
 *2.90         11-May-2020   Girish Kundar            Modified : Moved to Configuration and Changes as part of the REST API  
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

namespace Semnox.CommonAPI.Configuration
{
    public class InvoiceSequenceMappingController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/InvoiceSequenceMappings")]
        public HttpResponseMessage Get(string isActive = null, string sequenceId = null)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, sequenceId);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(executionContext);
                List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.ISACTIVE, isActive));
                    }
                }
                if (string.IsNullOrEmpty(sequenceId) == false)
                {
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceMappingDTO.SearchByParameters, string>(InvoiceSequenceMappingDTO.SearchByParameters.SEQUENCE_ID, sequenceId));
                }
                var content = invoiceSequenceMappingListBL.GetAllInvoiceSequenceMappingList(searchParameters);
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
        /// Performs a Post operation on InvoiceSequenceMappingDTO details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Configuration/InvoiceSequenceMappings")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<InvoiceSequenceMappingDTO> invoiceSequenceMappingList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(invoiceSequenceMappingList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (invoiceSequenceMappingList != null && invoiceSequenceMappingList.Any())
                {
                    InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(executionContext, invoiceSequenceMappingList);
                    invoiceSequenceMappingListBL.Save();
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

        ///// <summary>
        ///// Performs a Delete operation on InvoiceSequenceMappingDTO details
        ///// </summary>        
        ///// <returns>HttpResponseMessage</returns>
        //[HttpDelete]
        //[Route("api/SiteSetup/InvoiceSequenceMapping/")]
        //[Authorize]
        //public HttpResponseMessage Delete([FromBody] List<InvoiceSequenceMappingDTO> invoiceSequenceMappingList)
        //{
        //    try
        //    {
        //        log.LogMethodEntry(invoiceSequenceMappingList);
        //        this.securityTokenBL.GenerateJWTToken();
        //        this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
        //        this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
        //        if (invoiceSequenceMappingList != null)
        //        {
        //            // if invoiceSequenceMappingList.Id is less than zero then insert or else update
        //            InvoiceSequenceMappingListBL invoiceSequenceMappingListBL = new InvoiceSequenceMappingListBL(executionContext, invoiceSequenceMappingList);
        //            invoiceSequenceMappingListBL.SaveUpdateSequenceMappingList();
        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
        //        }
        //        else
        //        {
        //            log.LogMethodExit();
        //            return Request.CreateResponse(HttpStatusCode.NotFound, new { data = ""  });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException  });
        //    }
        //}
    }
}
