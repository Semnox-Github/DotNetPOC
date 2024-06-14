/********************************************************************************************
 * Project Name - ServerCore
 * Description  - LocalAdUseCases
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
using Semnox.Parafait.User;

namespace Semnox.Parafait.ServerCore
{
    /// <summary>
    /// LocalAdUseCases class
    /// </summary>
    public class LocalAdUseCases : IAdUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        // <summary>
        /// LocalAdUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalAdUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// GetAds
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadActiveChild">loadActiveChild</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="buildImage">buildImage</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>adsDTOList</returns>
        public async Task<List<AdsDTO>> GetAds(List<KeyValuePair<AdsDTO.SearchByParameters, string>> searchParameters, bool loadActiveChild = false, bool buildChildRecords = false,
                                               bool buildImage = false, SqlTransaction sqlTransaction = null)
        {
            return await Task<List<AdsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, sqlTransaction);
                AdsList adsList = new AdsList(executionContext);
                List<AdsDTO> adsDTOList = adsList.GetAllAdsDTOList(searchParameters, buildChildRecords, loadActiveChild, buildImage);
                log.LogMethodExit(adsDTOList);
                return adsDTOList;
            });
        }

        /// <summary>
        /// SaveAds
        /// </summary>
        /// <param name="adsDTOList">adsDTOList</param>
        /// <returns>adsDTOList</returns>
        public async Task<List<AdsDTO>> SaveAds(List<AdsDTO> adsDTOList)
        {
            return await Task<List<AdsDTO>>.Factory.StartNew(() =>
            {
                List<AdsDTO> result = new List<AdsDTO>();
                log.LogMethodEntry(adsDTOList);
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (AdsDTO adsDTO in adsDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            Ads adsBL = new Ads(executionContext, adsDTO);
                            adsBL.Save();
                            result.Add(adsBL.GetAdsDTO);
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                }
                log.LogMethodExit(result);
                return result;
            });
        }

        /// <summary>
        /// GetAdContainerDTOCollection
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns>AdContainerDTOCollection</returns>
        //public async Task<AdContainerDTOCollection> GetAdContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        //{
        //    return await Task<AdContainerDTOCollection>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(siteId, hash, rebuildCache);
        //        if (rebuildCache)
        //        {
        //            AdContainerList.Rebuild(siteId);
        //        }
        //        AdContainerDTOCollection result = new AdContainerDTOCollection();
        //        if (hash == result.Hash)
        //        {
        //            log.LogMethodExit(null, "No changes to the cache");
        //            return null;
        //        }
        //        log.LogMethodExit(result);
        //        return result;
        //    });
        //}

        /// <summary>
        /// AdRefresh
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="machineId">machineId</param>
        /// <returns></returns>
        public async Task AdRefresh(int hubId, int machineId)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hubId, machineId);
                Utilities utilities = GetUtility();
                AdManagement adManagement = new AdManagement(executionContext, utilities);
                adManagement.AdDataRefresh(hubId, machineId);
                log.LogMethodExit();
            });
        }

        /// <summary>
        /// GetAdShowContext
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="machineId">machineId</param>
        /// <returns></returns>
        public async Task GetAdShowContext(int hubId, int machineId)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hubId, machineId);
                Utilities utilities = GetUtility();
                AdManagement adManagement = new AdManagement(hubId, executionContext, utilities, machineId);
                adManagement.AdShowContext();
                log.LogMethodExit();
            });
        }

        /// <summary>
        /// AttachAdShowContext
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="machineId">machineId</param>
        /// <returns></returns>
        public async Task AttachAdShowContext(int hubId, int machineId)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hubId, machineId);
                Utilities utilities = GetUtility();
                AdManagement adManagement = new AdManagement(hubId, executionContext, utilities, machineId);
                adManagement.AttachAdShowContext();
                log.LogMethodExit();
            });
        }

        /// <summary>
        /// AttachAdContext
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="machineId">machineId</param>
        /// <returns></returns>
        public async Task AttachAdContext(int hubId, int machineId)
        {
            await Task.Factory.StartNew(() =>
            {
                log.LogMethodEntry(hubId, machineId);
                Utilities utilities = GetUtility();
                AdManagement adManagement = new AdManagement(hubId, executionContext, utilities, machineId);
                adManagement.AttachAdContext();
                log.LogMethodExit();
            });
        }

        internal Utilities GetUtility()
        {
            log.LogMethodEntry();
            Utilities utilities = new Utilities();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.LoginID = executionContext.GetUserId();
            utilities.ParafaitEnv.POSMachineId = executionContext.GetMachineId();
            utilities.ParafaitEnv.IsCorporate = executionContext.GetIsCorporate();
            utilities.ParafaitEnv.SiteId = executionContext.GetSiteId();
            log.Debug("executionContext - siteId" + executionContext.GetSiteId());
            utilities.ExecutionContext.SetIsCorporate(executionContext.GetIsCorporate());
            utilities.ExecutionContext.SetSiteId(executionContext.GetSiteId());
            utilities.ExecutionContext.SetUserId(executionContext.GetUserId());
            UserContainerDTO user = UserContainerList.GetUserContainerDTOOrDefault(executionContext.GetUserId(), "", executionContext.GetSiteId());
            utilities.ParafaitEnv.User_Id = user.UserId;
            utilities.ParafaitEnv.RoleId = user.RoleId;
            utilities.ExecutionContext.SetUserId(user.LoginId);
            utilities.ParafaitEnv.Initialize();
            log.LogMethodExit(utilities);
            return utilities;
        }
    }
}
