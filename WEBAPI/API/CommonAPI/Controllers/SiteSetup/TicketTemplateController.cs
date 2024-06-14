/********************************************************************************************
 * Project Name -  Printer
 * Description  -  Controller of the Ticket Template class.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.0      26-Jun-2019   Mushahid Faizan  Created
 ********************************************************************************************/
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.Core.Utilities;
using System.Web;
using System;
using Semnox.Parafait.Printer;
using Semnox.Core.GenericUtilities;
using Semnox.Parafait.User;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Semnox.CommonAPI.Helpers;
using Semnox.Parafait.Transaction;
using System.Linq;

namespace Semnox.CommonAPI.SiteSetup
{
    public class TicketTemplateController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();

        /// <summary>
        /// Get the JSON Object of Ticket Template List
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/SiteSetup/TicketTemplate/")]
        public HttpResponseMessage Get(string activityType = "TICKETTEMPLATE", int templateId = -1, string templateName = null)
        {
            try
            {
                log.LogMethodEntry(templateId, templateName);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<TicketTemplateHeaderDTO> ticketTemplateHeaderDTOList = new List<TicketTemplateHeaderDTO>();
                List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateDTOList = new List<ReceiptPrintTemplateHeaderDTO>();
                string exportQuery = string.Empty;
                if (activityType.ToUpper().ToString() == "TICKETTEMPLATE")
                {
                    ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderListBL = new ReceiptPrintTemplateHeaderListBL(executionContext);
                    receiptPrintTemplateDTOList = receiptPrintTemplateHeaderListBL.PopulateTemplate();
                    if (receiptPrintTemplateDTOList != null && receiptPrintTemplateDTOList.Count != 0)
                    {
                        foreach (ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO in receiptPrintTemplateDTOList)
                        {
                            List<KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>> searchParameters = new List<KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>>();
                            searchParameters.Add(new KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>(TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                            searchParameters.Add(new KeyValuePair<TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters, string>(TicketTemplateHeaderDTO.SearchByTicketTemplateHeaderParameters.TEMPLATE_ID, receiptPrintTemplateHeaderDTO.TemplateId.ToString()));

                            bool childRecords = true;
                            TicketTemplateHeaderListBL ticketTemplateHeaderListBL = new TicketTemplateHeaderListBL(executionContext);
                            ticketTemplateHeaderDTOList.AddRange(ticketTemplateHeaderListBL.GetTicketTemplateHeaderDTOList(searchParameters, childRecords));
                        }
                    }
                    /// To support Export option. This will give query output in text file                    
                    if (templateId > 0 && !string.IsNullOrEmpty(templateName))
                    {
                        exportQuery = receiptPrintTemplateHeaderListBL.GetExportQueries(templateId, templateName, false);
                    }
                }
                if (activityType.ToUpper().ToString() == "TICKETTEMPLATEPREVIEW" && templateId > 0)
                {
                    string ticketTemplateBase64String = PrintTransaction.TicketTemplatePreview(executionContext, templateId);
                    log.LogMethodExit(ticketTemplateBase64String);
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ticketTemplateBase64String, token = securityTokenDTO.Token });
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = receiptPrintTemplateDTOList, TicketTemplateHeaderList = ticketTemplateHeaderDTOList, ExportQuery = exportQuery, token = securityTokenDTO.Token });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, token = securityTokenDTO.Token });
            }
        }

        /// <summary>
        /// Post the JSON Object of ticketTemplateList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/SiteSetup/TicketTemplate/")]
        public HttpResponseMessage Post([FromBody] JObject jObject)
        {
            try
            {
                log.LogMethodEntry(jObject);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (jObject != null)
                {
                    List<TicketTemplateHeaderDTO> ticketTemplateHeaderList = new List<TicketTemplateHeaderDTO>();
                    List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateList = new List<ReceiptPrintTemplateHeaderDTO>();

                    string receiptPrintTemplateString = jObject.SelectToken("ReceiptPrintTemplateList").ToString();

                    // Deserialize the string to the List<ReceiptPrintTemplateHeaderDTO>
                    if (!string.IsNullOrEmpty(receiptPrintTemplateString) && !receiptPrintTemplateString.Contains("ReceiptPrintTemplateList"))
                    {
                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore
                        };
                        receiptPrintTemplateList = JsonConvert.DeserializeObject<List<ReceiptPrintTemplateHeaderDTO>>(receiptPrintTemplateString, settings);
                    }

                    string ticketTemplateHeaderString = jObject.SelectToken("TicketTemplateHeadeList").ToString();
                    // Deserialize the string to the List<TicketTemplateHeaderDTO>
                    if (!string.IsNullOrEmpty(ticketTemplateHeaderString))
                    {
                        ticketTemplateHeaderList = JsonConvert.DeserializeObject<List<TicketTemplateHeaderDTO>>(ticketTemplateHeaderString);
                    }
                    ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction();
                    try
                    {
                        if (receiptPrintTemplateList != null)
                        {
                            parafaitDBTrx.BeginTransaction();
                            ReceiptPrintTemplateHeaderListBL receiptPrintTemplateHeaderListBL = new ReceiptPrintTemplateHeaderListBL(executionContext, receiptPrintTemplateList);
                            receiptPrintTemplateHeaderListBL.SaveUpdateReceiptPrintTemplateHeaderList();
                            parafaitDBTrx.EndTransaction();
                        }
                        int templateId = receiptPrintTemplateList.Select(m => m.TemplateId).First();

                        if (templateId > 0)
                        {
                            if (ticketTemplateHeaderList != null &&
                                        ticketTemplateHeaderList.Any())
                            {
                                List<TicketTemplateHeaderDTO> updatedticketTemplateHeaderDTOList = new List<TicketTemplateHeaderDTO>();
                                foreach (TicketTemplateHeaderDTO ticketTemplateHeaderDTO in ticketTemplateHeaderList)
                                {
                                    if (ticketTemplateHeaderDTO.TemplateId != templateId)
                                    {
                                        ticketTemplateHeaderDTO.TemplateId = templateId;
                                    }
                                    if (ticketTemplateHeaderDTO.IsChanged)
                                    {
                                        updatedticketTemplateHeaderDTOList.Add(ticketTemplateHeaderDTO);
                                    }
                                }
                                if (updatedticketTemplateHeaderDTOList.Any())
                                {
                                    parafaitDBTrx.BeginTransaction();
                                    log.LogVariableState("updatedticketTemplateHeaderDTOList", updatedticketTemplateHeaderDTOList);
                                    TicketTemplateHeaderListBL ticketTemplateHeaderListBL = new TicketTemplateHeaderListBL(executionContext, updatedticketTemplateHeaderDTOList);
                                    ticketTemplateHeaderListBL.Save(parafaitDBTrx.SQLTrx);
                                    parafaitDBTrx.EndTransaction();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        parafaitDBTrx.RollBack();
                        log.Error(ex);
                        log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                        throw new Exception(ex.Message, ex);
                    }
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
        /// Delete the JSON Object of ticketTemplateList
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpDelete]
        [Authorize]
        [Route("api/SiteSetup/TicketTemplate/")]
        public HttpResponseMessage Delete([FromBody] JObject jObject)
        {
            try
            {
                log.LogMethodEntry(jObject);
                this.securityTokenBL.GenerateJWTToken();
                this.securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                this.executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (jObject != null)
                {
                    List<TicketTemplateHeaderDTO> ticketTemplateHeaderList = new List<TicketTemplateHeaderDTO>();
                    List<ReceiptPrintTemplateHeaderDTO> receiptPrintTemplateList = new List<ReceiptPrintTemplateHeaderDTO>();

                    string receiptPrintTemplateString = jObject.SelectToken("ReceiptPrintTemplateList").ToString();
                    string ticketTemplateHeaderString = jObject.SelectToken("TicketTemplateHeadeList").ToString();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    // Deserialize the string to the List<ReceiptPrintTemplateHeaderDTO>
                    if (!string.IsNullOrEmpty(receiptPrintTemplateString))
                    {
                        receiptPrintTemplateList = JsonConvert.DeserializeObject<List<ReceiptPrintTemplateHeaderDTO>>(receiptPrintTemplateString, settings);
                    }
                    // Deserialize the string to the List<TicketTemplateHeaderDTO>
                    if (!string.IsNullOrEmpty(ticketTemplateHeaderString))
                    {
                        ticketTemplateHeaderList = JsonConvert.DeserializeObject<List<TicketTemplateHeaderDTO>>(ticketTemplateHeaderString, settings);
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        parafaitDBTrx.BeginTransaction();
                        try
                        {
                            ReceiptPrintTemplateHeaderDTO receiptPrintTemplateHeaderDTO = new ReceiptPrintTemplateHeaderDTO();
                            if (receiptPrintTemplateList != null)
                            {   
                                ReceiptPrintTemplateHeaderBL receiptPrintTemplateHeaderBL = new ReceiptPrintTemplateHeaderBL(executionContext, receiptPrintTemplateList[0]);
                                receiptPrintTemplateHeaderBL.Save(parafaitDBTrx.SQLTrx);
                                receiptPrintTemplateHeaderDTO = receiptPrintTemplateHeaderBL.ReceiptPrintTemplateHeaderDTO;
                            }
                            int templateId = receiptPrintTemplateHeaderDTO.TemplateId;
                            if (templateId > 0)
                            {
                                if (ticketTemplateHeaderList != null &&
                                            ticketTemplateHeaderList.Any())
                                {
                                    List<TicketTemplateHeaderDTO> updatedticketTemplateHeaderDTOList = new List<TicketTemplateHeaderDTO>();
                                    foreach (TicketTemplateHeaderDTO ticketTemplateHeaderDTO in ticketTemplateHeaderList)
                                    {
                                        if (ticketTemplateHeaderDTO.TemplateId != templateId)
                                        {
                                            ticketTemplateHeaderDTO.TemplateId = templateId;
                                        }
                                        if (ticketTemplateHeaderDTO.IsChanged)
                                        {
                                            updatedticketTemplateHeaderDTOList.Add(ticketTemplateHeaderDTO);
                                        }
                                    }
                                    if (updatedticketTemplateHeaderDTOList.Any())
                                    {
                                        TicketTemplateHeaderListBL ticketTemplateHeaderListBL = new TicketTemplateHeaderListBL(executionContext, updatedticketTemplateHeaderDTOList);
                                        ticketTemplateHeaderListBL.Save(parafaitDBTrx.SQLTrx);
                                    }
                                }
                            }
                            parafaitDBTrx.EndTransaction();
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = "", token = securityTokenDTO.Token });
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
