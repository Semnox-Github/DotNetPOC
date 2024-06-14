/********************************************************************************************
 * Project Name - Facility Map Controller
 * Description  - Created to fetch, update, insert & delete in the Facility Map entity.
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.70       11-Jul-2019   Akshay Gulaganji         Created
 *2.90       24-Aug-2020   Girish Kundar            Modified: REST API statndard
*2.100.0    10-Sep-2020   Vikas Dwivedi            Modified as per the REST API Standards.
*2.120.00   10-Mar-2021   Roshan Devadiga           sModified Get,Post and Added Put method
 ***************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.CommonAPI.Products
{
    public class FacilityMapController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets the FacilityMap list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("api/Product/FacilityMaps")]
        public async Task<HttpResponseMessage> Get(int facilityId = -1, int facilityMapId = -1, int masterScheduleId = -1,
                                    int cancellationProductId = -1, string productType = null, int productId = -1,
                                    string isActive = null, bool loadChildRecords = false, bool loadActiveChildRecords = false)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(facilityId, facilityMapId, masterScheduleId, cancellationProductId, productType, productId, loadChildRecords, loadActiveChildRecords);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

                List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                if (facilityId > 0)
                {
                    searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.FACILITY_ID, facilityId.ToString()));
                }
                if (facilityMapId > 0)
                {
                    searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.FACILITY_MAP_ID, facilityMapId.ToString()));
                }
                if (masterScheduleId > 0)
                {
                    searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.MASTER_SCHEDULE_ID, masterScheduleId.ToString()));
                }
                if (cancellationProductId > 0)
                {
                    searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.CANCELLATION_PRODUCT_ID, cancellationProductId.ToString()));
                }
                if (!string.IsNullOrEmpty(productType))
                {
                    searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_IDS_IN, productType));
                }
                if (productId > 0)
                {
                    searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.ALLOWED_PRODUCT_ID, productId.ToString()));
                }
                if (string.IsNullOrEmpty(isActive) == false)
                {
                    if (isActive.ToString() == "1" || isActive.ToString() == "Y")
                    {
                        loadActiveChildRecords = true;
                        searchParameters.Add(new KeyValuePair<FacilityMapDTO.SearchByParameters, string>(FacilityMapDTO.SearchByParameters.IS_ACTIVE, "1"));
                    }
                }

                IFacilityMapUseCases facilityMapUseCases = ProductsUseCaseFactory.GetFacilityMapUseCases(executionContext);
                List<FacilityMapDTO> facilityMapDTOList = await facilityMapUseCases.GetFacilityMaps(searchParameters, loadChildRecords, loadActiveChildRecords);
                log.LogMethodExit(facilityMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = facilityMapDTOList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }

        /// <summary>
        /// Posts the facilityMapList
        /// </summary>
        /// <param name="facilityMapDTOList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Product/FacilityMaps")]
        [Authorize]
        public async Task<HttpResponseMessage> Post([FromBody] List<FacilityMapDTO> facilityMapDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;
            try
            {
                log.LogMethodEntry(facilityMapDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (facilityMapDTOList ==null)
                {
                    log.LogMethodExit(facilityMapDTOList);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IFacilityMapUseCases facilityMapUseCases = ProductsUseCaseFactory.GetFacilityMapUseCases(executionContext);
                await facilityMapUseCases.SaveFacilityMaps(facilityMapDTOList);
                log.LogMethodExit(facilityMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        /// <summary>
        /// Post the FacilityMapDTOList collection
        /// <param name="facilityMapDTOList">FacilityMapDTOList</param>
        [HttpPut]
        [Route("api/Product/FacilityMaps")]
        [Authorize]
        public async Task<HttpResponseMessage> Put([FromBody] List<FacilityMapDTO> facilityMapDTOList)
        {
            SecurityTokenDTO securityTokenDTO = null;
            ExecutionContext executionContext = null;

            try
            {
                log.LogMethodEntry(facilityMapDTOList);
                SecurityTokenBL securityTokenBL = new SecurityTokenBL();
                securityTokenBL.GenerateJWTToken();
                securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
                executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));
                if (facilityMapDTOList == null || facilityMapDTOList.Any(a => a.FacilityMapId < 0))
                {
                    log.LogMethodExit();
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = string.Empty });
                }
                IFacilityMapUseCases facilityMapUseCases = ProductsUseCaseFactory.GetFacilityMapUseCases(executionContext);
                await facilityMapUseCases.SaveFacilityMaps(facilityMapDTOList);
                log.LogMethodExit(facilityMapDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = string.Empty });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
            }
        }
        ///// <summary>
        ///// Deletes the facilityMapList
        ///// </summary>
        ///// <param name="facilityMapDTOList"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //[Route("api/Products/FacilityMap/")]
        //[Authorize]
        //public HttpResponseMessage Delete([FromBody] List<FacilityMapDTO> facilityMapDTOList)
        //{
        //    try
        //    {
        //        log.LogMethodEntry(facilityMapDTOList);
        //        securityTokenBL.GenerateJWTToken();
        //        securityTokenDTO = securityTokenBL.GetSecurityTokenDTO;
        //        executionContext = new ExecutionContext(securityTokenDTO.LoginId, securityTokenDTO.SiteId, securityTokenDTO.MachineId, -1, Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsCorporate"]), Convert.ToInt32(securityTokenDTO.LanguageId));

        //        if (facilityMapDTOList != null && facilityMapDTOList.Count != 0)
        //        {
        //            FacilityMapListBL facilityMapListBL = new FacilityMapListBL(executionContext, facilityMapDTOList);
        //            facilityMapListBL.Save();

        //            return Request.CreateResponse(HttpStatusCode.OK, new { data = ""  });
        //        }
        //        else
        //        {
        //            log.LogMethodExit(null);
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = ""  });
        //        }
        //    }
        //    catch (ValidationException valEx)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(valEx, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
        //    }
        //    catch (Exception ex)
        //    {
        //        string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
        //        log.Error(customException);
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException  });
        //    }
        //}
    }
}
