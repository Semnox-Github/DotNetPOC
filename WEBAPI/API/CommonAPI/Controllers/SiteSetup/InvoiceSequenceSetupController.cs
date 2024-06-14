/********************************************************************************************
 * Project Name - Transactions
 * Description  - API for the InvoiceSequenceSetup details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        19-Mar-2019   Jagan Mohana Rao     Created 
              23-Apr-2019   Mushahid Faizan      Added log Method Entry & Exit &
                                                 Added isActive SearchParameter in HttpGet Method.
                                                 Declared Global ExecutionContext, SecurityTokenDTO, SecurityTokenBL.
              05-Sep-2019   Mushahid Faizan      Modified Get(),Post() and Delete() method.
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.Transaction;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace Semnox.CommonAPI.SiteSetup
{
    public class InvoiceSequenceSetupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        /// <summary>
        /// Get the JSON Object InvoiceSequenceSetup List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/InvoiceSequenceSetup/")]
        public HttpResponseMessage Get(string isActive)
        {
            try
            {
                log.LogMethodEntry(isActive);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                // The below code is use to get LookupValues based on "SYSTEM_AUTHORIZATION_NUMBER"
                LookupValuesList lookupValuesList = new LookupValuesList(executionContext);
                List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>> lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_NAME, "SYSTEM_AUTHORIZATION"));
                lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<LookupValuesDTO> invoiceTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                int lookupId = -1;
                if (invoiceTypeLookUpValueList != null && invoiceTypeLookUpValueList.Count != 0)
                {
                    lookupId = invoiceTypeLookUpValueList[0].LookupId;
                    lookupValuesSearchParams = new List<KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>>();
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.LOOKUP_VALUE, "SYSTEM_AUTHORIZATION_NUMBER"));
                    lookupValuesSearchParams.Add(new KeyValuePair<LookupValuesDTO.SearchByLookupValuesParameters, string>(LookupValuesDTO.SearchByLookupValuesParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                    invoiceTypeLookUpValueList = lookupValuesList.GetAllLookupValues(lookupValuesSearchParams);
                }

                // This below code is use to get list of InvoiceSequenceSetup
                InvoiceSequenceSetupListBL invoiceSequenceSetupListBL = new InvoiceSequenceSetupListBL(executionContext);
                List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                if (isActive == "1")
                {
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.ISACTIVE, isActive));
                }
                List<InvoiceSequenceSetupDTO> invoiceSequenceSetupList = invoiceSequenceSetupListBL.GetAllInvoiceSequenceSetupList(searchParameters);

                log.LogMethodExit(invoiceSequenceSetupList);
                return Request.CreateResponse(HttpStatusCode.OK, new { InvoiceSequenceSetupList = invoiceSequenceSetupList, LookupValueList = invoiceTypeLookUpValueList, LookupId = lookupId, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ex.Message, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Performs a Post operation on invoiceSequenceSetupDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/SiteSetup/InvoiceSequenceSetup/")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] JObject jObject)
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;

                InvoiceSequenceSetupDTO invoiceSequenceSetupDTO = null;
                LookupValuesDTO lookupValuesDTO = null;

                if (jObject != null)
                {
                    /// This below logic is to get the invoiceSequenceSetup and lookupValues details
                    string invoiceSequenceSetupDTOString = jObject.SelectToken("InvoiceSequenceSetupList").ToString();
                    string lookupValueDTOString = jObject.SelectToken("LookupValueList").ToString();

                    if (!string.IsNullOrEmpty(invoiceSequenceSetupDTOString))
                    {
                        List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList = JsonConvert.DeserializeObject<List<InvoiceSequenceSetupDTO>>(invoiceSequenceSetupDTOString);
                        if (invoiceSequenceSetupDTOList != null && invoiceSequenceSetupDTOList.Count != 0)
                        {
                            invoiceSequenceSetupDTO = invoiceSequenceSetupDTOList[0];
                        }
                    }
                    if (!string.IsNullOrEmpty(lookupValueDTOString))
                    {
                        List<LookupValuesDTO> lookupValuesDTOList = JsonConvert.DeserializeObject<List<LookupValuesDTO>>(lookupValueDTOString);
                        if (lookupValuesDTOList != null && lookupValuesDTOList.Count != 0)
                        {
                            lookupValuesDTO = lookupValuesDTOList[0];
                        }
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        try
                        {
                            if (invoiceSequenceSetupDTO != null)
                            {
                                invoiceSequenceSetupDTO.ApprovedDate = DateTime.Now;
                                InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(executionContext, invoiceSequenceSetupDTO);
                                invoiceSequenceSetupBL.Save(parafaitDBTrx.SQLTrx);
                            }
                            if (lookupValuesDTO != null)
                            {
                                LookupValues lookupValues = new LookupValues(executionContext, lookupValuesDTO);
                                lookupValues.Save(parafaitDBTrx.SQLTrx);
                            }
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            log.LogMethodExit(null, "Throwing Exception : " + valEx.Message);
                            throw;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw;
                        }
                    }
                    log.LogMethodExit();
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
        /// Performs a Delete operation on InvoiceSequenceSetupDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Route("api/SiteSetup/InvoiceSequenceSetup/")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList)
        {
            try
            {
                log.LogMethodEntry();
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId)); ;

                if (invoiceSequenceSetupDTOList != null && invoiceSequenceSetupDTOList.Count != 0)
                {
                    InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(executionContext, invoiceSequenceSetupDTOList[0]);
                    invoiceSequenceSetupBL.Save();
                    log.LogMethodExit();
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