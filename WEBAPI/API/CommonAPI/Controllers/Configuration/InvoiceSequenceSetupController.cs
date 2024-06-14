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
*2.90         11-May-2020   Girish Kundar        Modified : Moved to Configuration and Changes as part of the REST API  
 ********************************************************************************************/


using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class InvoiceSequenceSetupController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Object InvoiceSequenceSetup List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Configuration/InvoiceSequences")]
        public HttpResponseMessage Get(string isActive = null, int invoiceSequenceSetupId = -1, int invoiceTypeId = -1, DateTime? approvedDate = null,
                                       DateTime? expiryDate = null, int startNumber = -1, int endNumber = -1,
                                       string resolutionNumber = null, int invoiceGroupId = -1)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, invoiceGroupId, expiryDate, startNumber, endNumber, resolutionNumber, invoiceTypeId, approvedDate);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                DateTime tempApprovedDate = DateTime.Now;
                DateTime tempExpiryDate = DateTime.Now;
                DateTime tempResolutionDate = DateTime.Now;
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
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.ISACTIVE, isActive));
                    }
                }
                if (invoiceSequenceSetupId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.INVOICE_SEQUENCE_SETUP_ID, invoiceSequenceSetupId.ToString()));
                }
                if (invoiceTypeId > -1)
                {
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.INVOICE_TYPE_ID, invoiceTypeId.ToString()));
                }
                if (startNumber > -1)
                {
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.SERIES_START_NUMBER, startNumber.ToString()));
                }
                if (endNumber > -1)
                {
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.SERIES_END_NUMBER, endNumber.ToString()));
                }
                if (string.IsNullOrEmpty(resolutionNumber) == false)
                {
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.RESOLUTION_NUMBER, resolutionNumber.ToString()));
                }
                if (approvedDate != null)
                {
                    tempApprovedDate = Convert.ToDateTime(approvedDate.ToString());
                    if (tempApprovedDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.EXPIRY_DATE, tempApprovedDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

                }
                if (expiryDate != null)
                {
                    tempExpiryDate = Convert.ToDateTime(expiryDate.ToString());
                    if (tempExpiryDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                    searchParameters.Add(new KeyValuePair<InvoiceSequenceSetupDTO.SearchByParameters, string>(InvoiceSequenceSetupDTO.SearchByParameters.EXPIRY_DATE, tempExpiryDate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));

                }
                List<InvoiceSequenceSetupDTO> invoiceSequenceSetupList = invoiceSequenceSetupListBL.GetAllInvoiceSequenceSetupList(searchParameters);

                log.LogMethodExit(invoiceSequenceSetupList);
                return Request.CreateResponse(HttpStatusCode.OK, new { InvoiceSequenceSetupList = invoiceSequenceSetupList, LookupValueList = invoiceTypeLookUpValueList, LookupId = lookupId });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = ex.Message });
            }
        }

        /// <summary>
        /// Performs a Post operation on invoiceSequenceSetupDTOsList details
        /// </summary>        
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Route("api/Configuration/InvoiceSequences")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<InvoiceSequenceSetupDTO> invoiceSequenceSetupDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(invoiceSequenceSetupDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (invoiceSequenceSetupDTOList != null && invoiceSequenceSetupDTOList.Any())
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (InvoiceSequenceSetupDTO invoiceSequenceSetupDTO in invoiceSequenceSetupDTOList)
                        {
                            invoiceSequenceSetupDTO.ApprovedDate = DateTime.Now;
                            InvoiceSequenceSetupBL invoiceSequenceSetupBL = new InvoiceSequenceSetupBL(executionContext, invoiceSequenceSetupDTO);
                            invoiceSequenceSetupBL.Save(parafaitDBTrx.SQLTrx);

                        }
                        parafaitDBTrx.EndTransaction();
                    }
                    log.LogMethodExit();
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