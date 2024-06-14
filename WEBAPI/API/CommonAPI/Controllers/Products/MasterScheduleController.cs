/********************************************************************************************
 * Project Name - Products Controller/MasterScheduleController
 * Description  - Created to Get, Post and Delete Attraction Master Schedule for Products-> Set Up -> Attraction Schedule  
 *  
 **************
 **Version Log
 **************
 *Version     Date            Created By          Remarks          
 ***************************************************************************************************
 *2.50        21-Feb-2019     Nagesh Badiger      Modified Get, Post and Delete methods.
 ****************************************************************************************************
 *2.60        18-Mar-2019     Akshay Gulaganji    Modified isActive (from string to bool), added Custom Generic Exception, Response
 *                          					  and added ExecutionContext 
 *2.70        27-Jun-2019     Akshay Gulaganji    modified Delete() method
 *2.70        18-Jul-2019     Akshay Gulaganji    modified Get() method
 *2.80        06-Feb-2020     Girish Kundar       modified: Renamed to MasterScheduleController and modified as per REST API standard
 *2.100.0     10-Sep-2020     Vikas Dwivedi       Modified as per the REST API Standards.
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class MasterScheduleController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the AttractionSchedule Collection based on isActive.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/MasterSchedules")]
        public HttpResponseMessage Get(string isActive = null, bool loadChildRecord = false, int facilityMapId = -1, int masterScheduleId = -1, bool activeChildRecords = true)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(isActive, loadChildRecord, facilityMapId, masterScheduleId, activeChildRecords);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MasterScheduleDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.ACTIVE_FLAG, isActive));
                    }
                }
                if (masterScheduleId != -1)
                {
                    searchParameters.Add(new KeyValuePair<MasterScheduleDTO.SearchByParameters, string>(MasterScheduleDTO.SearchByParameters.MASTER_SCHEDULE_ID, masterScheduleId.ToString()));
                }
                MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext);
                List<MasterScheduleDTO> masterScheduleDTOList = masterScheduleList.GetMasterScheduleDTOsList(searchParameters, activeChildRecords, loadChildRecord, Convert.ToInt32(facilityMapId));
                log.LogMethodExit(masterScheduleDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = masterScheduleDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
            }
        }

        /// <summary>
        /// Posts the JSON Object for AttractionSchedule
        /// </summary>
        /// <param name="masterScheduleDTOList">masterScheduleDTOList</param>
        /// <returns>HttpMessage</returns>
        [HttpPost]
        [Route("api/Product/MasterSchedules")]
        [Authorize]
        public HttpResponseMessage Post([FromBody] List<MasterScheduleDTO> masterScheduleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(masterScheduleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (masterScheduleDTOList != null && masterScheduleDTOList.Count > 0)
                {
                    MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext, masterScheduleDTOList);
                    masterScheduleList.SaveAttractionMasterScheduleList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
                }
                else
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
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
        /// Deletes the Attraction Master Schedule List
        /// </summary>
        /// <param name="masterScheduleDTOList"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("api/Product/MasterSchedules")]
        [Authorize]
        public HttpResponseMessage Delete([FromBody] List<MasterScheduleDTO> masterScheduleDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(masterScheduleDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (masterScheduleDTOList != null && masterScheduleDTOList.Count != 0)
                {
                    MasterScheduleList masterScheduleList = new MasterScheduleList(executionContext, masterScheduleDTOList);
                    masterScheduleList.DeleteAttractionMasterScheduleList();
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
                }
                else
                {
                    log.LogMethodExit(null);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
                }
            }
            catch (ValidationException valEx)
            {
                string validationException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
                log.Error(validationException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = validationException });
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
