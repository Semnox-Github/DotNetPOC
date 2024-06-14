/********************************************************************************************
 * Project Name - Waivers
 * Description  - Singleton container class for waiver set dto list fetch
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       15-Oct-2019    GUru S A        Created for waiver phase 2
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Site;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Waiver
{
    public sealed class WaiverSetContainer
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ConcurrentDictionary<int, List<WaiverSetDTO>> siteWaiverSetDictionaryList = new ConcurrentDictionary<int, List<WaiverSetDTO>>();
        private ExecutionContext executionContext;
        private WaiverSetContainer()
        {
            log.LogMethodEntry();
            executionContext = ExecutionContext.GetExecutionContext(); 
            siteWaiverSetDictionaryList = new ConcurrentDictionary<int, List<WaiverSetDTO>>();
            SiteList siteList = new SiteList(executionContext);
            List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> siteSearchParams = new List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>>();
            siteSearchParams.Add(new KeyValuePair<SiteDTO.SearchBySiteParameters, string>(SiteDTO.SearchBySiteParameters.IS_ACTIVE, "Y"));
            List<SiteDTO> siteDTOList = siteList.GetAllSites(siteSearchParams);
            log.LogVariableState("siteDTOList", siteDTOList);
            bool singleSite = true;
            if (siteDTOList != null && siteDTOList.Count > 1)
            {
                singleSite = false;
            } 
            for (int i = 0; i < siteDTOList.Count; i++)
            {
                WaiverSetListBL waiverSetListBL = new WaiverSetListBL(executionContext);
                List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>> searchParameters = new List<KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>>();
                searchParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.IS_ACTIVE, "1"));
                if (singleSite == false)
                {
                    searchParameters.Add(new KeyValuePair<WaiverSetDTO.SearchByWaiverParameters, string>(WaiverSetDTO.SearchByWaiverParameters.SITE_ID, siteDTOList[i].SiteId.ToString()));
                }
                if (siteWaiverSetDictionaryList != null && siteWaiverSetDictionaryList.ContainsKey(siteDTOList[i].SiteId) == false)
                {
                    List<WaiverSetDTO> waiverSetDTOList = waiverSetListBL.GetWaiverSetDTOList(searchParameters, true, true, true, true);
                    siteWaiverSetDictionaryList.TryAdd((singleSite ? -1 : siteDTOList[i].SiteId), waiverSetDTOList);
                }
            }
            log.LogMethodExit();
        } 


        private static readonly WaiverSetContainer instance = new WaiverSetContainer();
        public static WaiverSetContainer GetInstance
        {
            get
            {
                return instance;
            }
        }
        public List<WaiverSetDTO> GetWaiverSetDTOList(int siteId)
        {
            log.LogMethodEntry(siteId);
            List<WaiverSetDTO> waiverSetDTOList = null;
            log.LogVariableState("siteWaiverSetDictionaryList", siteWaiverSetDictionaryList);
            bool singleSite = true;
            if (siteWaiverSetDictionaryList != null && siteWaiverSetDictionaryList.Count > 1)
            {
                singleSite = false;
                if (siteId == -1)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2437));//"Pass site id value in HQ environment"
                }
            }

            siteWaiverSetDictionaryList.TryGetValue((singleSite ? -1 : siteId), out waiverSetDTOList);

            List<WaiverSetDTO> returnList = new List<WaiverSetDTO>();
            if (waiverSetDTOList != null)
            {
                foreach (WaiverSetDTO item in waiverSetDTOList)
                {
                    WaiverSetDTO copyWaiverSetDTO = new WaiverSetDTO(item.WaiverSetId, item.Name, item.IsActive, item.CreationDate, item.CreatedBy, item.LastUpdatedDate, item.LastUpdatedBy, item.Guid, item.Site_id, item.SynchStatus, item.MasterEntityId, item.Description);
                    copyWaiverSetDTO.WaiverSetDetailDTOList = new List<WaiversDTO>(item.WaiverSetDetailDTOList);
                    copyWaiverSetDTO.WaiverSetSigningOptionDTOList = new List<WaiverSetSigningOptionsDTO>(item.WaiverSetSigningOptionDTOList);
                    returnList.Add(copyWaiverSetDTO);
                }
            }
            return returnList;
        }
    }
}
