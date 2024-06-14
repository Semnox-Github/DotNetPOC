/********************************************************************************************
 * Project Name - CommonAPI
 * Description  - ExternalLocationController  API -  Get the site details
 * 
 **************
 **Version Log
 **************
 *Version     Date                  Modified By           Remarks          
 *********************************************************************************************
 *0.0        28-Sept-2020           Girish Kundar          Created 
 *2.130.7    07-Apr-2022            Ashish Bhat            Modified( External  REST API.)
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Site;
using Semnox.Parafait.ThirdParty.External;

namespace Semnox.CommonAPI.Controllers.ThirdParty.External
{
    public class ExternalLocationController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Get the JSON Sites
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/External/Locations")]
        public async Task<HttpResponseMessage> Get(int siteId = -1, bool isActive = false, int siteCode = -1,
                                                   bool excludeMasterSite = false, bool onlineEnabled = false,
                                                   bool buildChilRecords = false, bool activeChildRecords = false)
        {
            log.LogMethodEntry( siteId, siteCode, onlineEnabled);
            ExecutionContext executionContext = null;
            try
            {
                executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
                List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
                if (siteId > -1)
                {
                    searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_ID, siteId.ToString()));
                }
                if (siteCode != -1)
                {
                    searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.SITE_CODE, siteCode.ToString()));
                }
                if (onlineEnabled)
                {
                    searchParameters.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.ONLINE_ENABLED, "Y"));
                }
                ISiteUseCases siteUseCases = SiteUseCaseFactory.GetSiteUseCases(executionContext);
                List<SiteDTO> siteDTOList = await siteUseCases.GetSites(searchParameters, buildChilRecords, activeChildRecords);
                if (siteDTOList != null && siteDTOList.Count != 0 && isActive)
                {
                    // filter for InActive sites here
                    siteDTOList = siteDTOList.Where(x => (x.IsActive)).OrderBy(x => x.SiteId).ToList();

                    //if this is from external users/customers - do not show the master site
                    if (excludeMasterSite)
                    {
                        SiteList siteList = new SiteList(executionContext);
                        SiteDTO HQSite = siteList.GetMasterSiteFromHQ();
                        if (HQSite != null && HQSite.SiteId != -1)
                        {
                            siteDTOList = siteDTOList.Where(x => x.SiteId != HQSite.SiteId).ToList();
                        }
                    }
                }
                List<ExternalLocationDTO> externalLocationDTOList = new List<ExternalLocationDTO>();
                ExternalLocationDTO externalLocationDTO = new ExternalLocationDTO();
                if (siteDTOList != null && siteDTOList.Any())
                {
                    foreach (SiteDTO siteDTO in siteDTOList)
                    {
                        externalLocationDTO = new ExternalLocationDTO(siteDTO.SiteId, siteDTO.SiteName, siteDTO.SiteCode, siteDTO.IsMasterSite, siteDTO.SiteShortName,
                            siteDTO.OnlineEnabled, siteDTO.AboutUs, siteDTO.PinCode, siteDTO.Description, siteDTO.SiteURL, siteDTO.City,
                            siteDTO.State, siteDTO.Country, siteDTO.StoreRanking, siteDTO.OpenTime, siteDTO.CloseTime, siteDTO.ExternalSourceReference,
                            siteDTO.OpenDate, siteDTO.CloseDate);
                        externalLocationDTOList.Add(externalLocationDTO);
                    }
                }
                log.LogMethodExit(externalLocationDTOList);
                return Request.CreateResponse(HttpStatusCode.OK, externalLocationDTOList);
            }
            catch (ValidationException ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException, exception = ExceptionSerializer.Serialize(ex) });
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
