/********************************************************************************************
 * Project Name - ServerCore
 * Description  - RemoteAdBroadcastUseCases
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By               Remarks          
 *********************************************************************************************
 *2.150.2     02-Dec-2022   Abhishek              Created - Game Server Cloud Movement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.ServerCore
{
    /// <summary>
    /// RemoteAdUseCases class
    /// </summary>
    public class RemoteAdUseCases : RemoteUseCases, IAdUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ADS_URL = "api/GameServer/Ads";
        private const string ADS_CONTAINER_URL = "api/Game/AdManagement/AdContainer";
        private const string AD_REFRESH_URL = "api/Game/AdManagement/{hubId}/Refresh";
        private const string AD_SHOW_CONTEXT_URL = "api/Game/AdManagement/{hubId}/AdShowContext";
        private const string ATTACH_AD_SHOW_CONTEXT_URL = "api/Game/AdManagement/{hubId}/AttachAdShowContext";
        private const string ATTACH_AD_CONTEXT_URL = "api/Game/AdManagement/{hubId}/AttachAdContext";

        // /// <summary>
        /// RemoteAdUseCases class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public RemoteAdUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Get Ads.
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns>adsDTOList</returns>
        public async Task<List<AdsDTO>> GetAds(List<KeyValuePair<AdsDTO.SearchByParameters, string>> searchParameters, bool loadActiveChild = false, bool buildChildRecords = false,
                                  bool buildImage = false, SqlTransaction sqltransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            

           
            try
            {
                List<AdsDTO> result = await Get<List<AdsDTO>>(ADS_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Save Ads.
        /// </summary>
        /// <param name="adsDTOList">adsDTOList</param>
        /// <returns>adsDTOList</returns>
        public async Task<List<AdsDTO>> SaveAds(List<AdsDTO> adsDTOList)
        {
            log.LogMethodEntry(adsDTOList);
            try
            {
                List<AdsDTO> adDTOList = await Post<List<AdsDTO>>(ADS_URL, adsDTOList);
                log.LogMethodExit(adDTOList);
                return adDTOList;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// Get the Container Data for Ads remote case.
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        //public async Task<AdContainerDTOCollection> GetAdContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        //{
        //    log.LogMethodEntry(siteId, hash, rebuildCache);
        //    AdContainerDTOCollection result = await Get<AdContainerDTOCollection>(ADS_CONTAINER_URL, new WebApiGetRequestParameterCollection("siteId",
        //                                                                                                                                                siteId,
        //                                                                                                                                                "hash",
        //                                                                                                                                                hash,
        //                                                                                                                                                "rebuildCache",
        //                                                                                                                                                rebuildCache
        //                                                                                                                                              ));
        //    log.LogMethodExit(result);
        //    return result;
        //}

        /// <summary>
        /// Ad Refresh remote case.
        /// </summary>
        /// <param name="hubId"></param>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public async Task AdRefresh(int hubId, int machineId)
        {
            log.LogMethodEntry(hubId, machineId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineId", machineId.ToString()));
            string result = await Get<string>(AD_REFRESH_URL.Replace("{hubId}",hubId.ToString()), searchParameterList);
            log.LogMethodExit(result);
        }

        /// <summary>
        /// GetAdShowContext remote case.
        /// </summary>
        /// <param name="hubId"></param>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public async Task GetAdShowContext(int hubId, int machineId)
        {
            log.LogMethodEntry(hubId, machineId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineId", machineId.ToString()));
            string result = await Get<string>(AD_SHOW_CONTEXT_URL.Replace("{hubId}", hubId.ToString()), searchParameterList);
            log.LogMethodExit(result);
        }

        /// <summary>
        /// AttachAdShowContext remote case.
        /// </summary>
        /// <param name="hubId"></param>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public async Task AttachAdShowContext(int hubId, int machineId)
        {
            log.LogMethodEntry(hubId, machineId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineId", machineId.ToString()));
            string result = await Get<string>(ATTACH_AD_SHOW_CONTEXT_URL.Replace("{hubId}", hubId.ToString()), searchParameterList);
            log.LogMethodExit(result);
        }

        /// <summary>
        /// AttachAdShowContext remote case.
        /// </summary>
        /// <param name="hubId"></param>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public async Task AttachAdContext(int hubId, int machineId)
        {
            log.LogMethodEntry(hubId, machineId);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineId", machineId.ToString()));
            string result = await Get<string>(ATTACH_AD_CONTEXT_URL.Replace("{hubId}", hubId.ToString()), searchParameterList);
            log.LogMethodExit(result);
        }
    }
}
