/********************************************************************************************
 * Project Name - Products
 * Description  - Created to fetch, update and insert waivers details.   
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By              Remarks          
 *********************************************************************************************
 *2.60        24-Jan-2019   Muhammed Mehraj          Created 
 *2.60        22-Mar-2019   Nagesh Badiger           Added ExecutionContext and added Custom Generic Exception 
 *2.70.2      06-Feb-2020   Divya A                  Waiver changes for WMS and Waiver Tab
 *2.80        09-Apr-2020   Nitin                    Cobra Changes
 *2.110.0     10-Sep-2020     Girish Kundar          Modified :  REST API Standards.
 ********************************************************************************************/

using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Transaction;
using Semnox.Parafait.Waiver;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using Semnox.Parafait.POS;

namespace Semnox.CommonAPI.Products
{
    public class WaiversController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private SecurityTokenBL securityTokenBL = new SecurityTokenBL();
        private ExecutionContext executionContext;
        private SecurityTokenDTO securityTokenDTO;

        /// <summary>
        /// Gets the Waivers collection
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Product/Waivers")]
        [Authorize]
        public HttpResponseMessage Get(string isActive = null, string machineName = null, int waiverSetId = -1,bool loadActiveChild = false, bool removeIncompleteRecords = false, bool getLanguageSpecificContent = false, string waiverSetSigningOptions = null)
        {
            try
            {
                log.LogMethodEntry(isActive, machineName, waiverSetId, loadActiveChild, removeIncompleteRecords,getLanguageSpecificContent);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (!string.IsNullOrEmpty(machineName))
                {
                    List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>> searchParameters = new List<KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>>();
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.SITE_ID, Convert.ToString(securityTokenDTO.SiteId)));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.POS_OR_COMPUTER_NAME, machineName));
                    searchParameters.Add(new KeyValuePair<POSMachineDTO.SearchByPOSMachineParameters, string>(POSMachineDTO.SearchByPOSMachineParameters.ISACTIVE, "1"));
                    POSMachineList pOSMachineList = new POSMachineList(executionContext);
                    List<POSMachineDTO> machineList = pOSMachineList.GetAllPOSMachines(searchParameters, false, false);
                    if (machineList == null || !machineList.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = "POS Machine " + machineName + " is not set up."  });
                    }
                    executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, machineList[0].POSMachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), securityTokenDTO.LanguageId != -1 ? securityTokenDTO.LanguageId : ParafaitDefaultContainerList.GetParafaitDefault<int>(executionContext,"DEFAULT_LANGUAGE"));
                }

                List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> waiversSearchList = new List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>>();
                waiversSearchList.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.SITE_ID, securityTokenDTO.SiteId.ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChild = true;
                        waiversSearchList.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE, isActive));
                    }
                }
                if (waiverSetId > -1)
                {
                    waiversSearchList.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.WAIVER_SET_ID, waiverSetId.ToString()));
                }

                List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>> signingOptionsSearchParam = null;
                if(!String.IsNullOrEmpty(waiverSetSigningOptions))
                {
                    signingOptionsSearchParam = new List<KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>>();
                    signingOptionsSearchParam.Add(new KeyValuePair<WaiverSetSigningOptionsDTO.SearchByParameters, string>(WaiverSetSigningOptionsDTO.SearchByParameters.WAIVERSET_SIGNING_OPTIONS_LIST, waiverSetSigningOptions));
                }

                WaiverSetListBL waiverSetListBL = new WaiverSetListBL(executionContext);
                List<WaiverSetDTO> result = waiverSetListBL.GetWaiverSetDTOList(waiversSearchList, true, loadActiveChild, removeIncompleteRecords, true, null, signingOptionsSearchParam);
                string guid = ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "OVERRIDING_WAIVER_SET");
                if (waiverSetId <= -1 &&
                    String.IsNullOrWhiteSpace(guid) == false)
                {
                    result = result.Where(x => string.Equals(x.Guid, guid, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (getLanguageSpecificContent)
                {
                    foreach (WaiverSetDTO waiverSetDTO in result)
                    {
                        foreach (WaiversDTO waiversDTO in waiverSetDTO.WaiverSetDetailDTOList)
                        {
                            if (waiversDTO.ObjectTranslationsDTOList != null && waiversDTO.ObjectTranslationsDTOList.Any())
                            {
                                ObjectTranslationsDTO objectTranslationsDTO = waiversDTO.ObjectTranslationsDTOList.Find(otl => otl.LanguageId == executionContext.GetLanguageId() && otl.ElementGuid == waiversDTO.Guid);
                                if (objectTranslationsDTO != null)
                                {
                                    if (waiversDTO.WaiverFileName != objectTranslationsDTO.Translation)
                                    {
                                        waiversDTO.WaiverFileName = objectTranslationsDTO.Translation;
                                        WaiversBL waiversBL = new WaiversBL(executionContext, waiversDTO);
                                        waiversBL.GetWaiverFileContentInBase64Format();
                                    }
                                }
                            }
                        }
                    }
                }
                log.LogMethodExit(result);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = result });
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
        /// Post the waiver collection
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/Waivers")]
        [Authorize]
        public HttpResponseMessage Post(List<WaiverSetDTO> waiverSetDTOList)
        {
            try
            {
                log.LogMethodEntry(waiverSetDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                if (waiverSetDTOList != null || waiverSetDTOList.Count != 0)
                {                    
                    WaiverSetListBL waiverSetListBL = new WaiverSetListBL(executionContext, waiverSetDTOList);
                    waiverSetListBL.SaveUpdateWaivers();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = " "  });
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
        /// Delete the waiver collection
        /// </summary>
        /// <param name="waiverSetDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Product/Waivers")]
        [Authorize]
        public HttpResponseMessage Delete(List<WaiverSetDTO> waiverSetDTOList)
        {
            try
            {
                log.LogMethodEntry(waiverSetDTOList);
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, -1, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (waiverSetDTOList != null || waiverSetDTOList.Count != 0)
                {
                    WaiverSetListBL waiverSetListBL = new WaiverSetListBL(executionContext, waiverSetDTOList);
                    waiverSetListBL.SaveUpdateWaivers();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = " "  });
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