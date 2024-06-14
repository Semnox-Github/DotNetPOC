/********************************************************************************************
 * Project Name - ServerCore
 * Description  - IAdUseCases
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

namespace Semnox.Parafait.ServerCore
{
    public interface IAdUseCases
    {
        /// <summary>
        /// GetAds
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="loadActiveChild">loadActiveChild</param>
        /// <param name="buildChildRecords">buildChildRecords</param>
        /// <param name="buildImage">buildImage</param>
        /// <param name="sqltransaction">sqltransaction</param>
        /// <returns>adsDTOList</returns>
        Task<List<AdsDTO>> GetAds(List<KeyValuePair<AdsDTO.SearchByParameters, string>> searchParameters, bool loadActiveChild = false, bool buildChildRecords = false,
                                  bool buildImage = false, SqlTransaction sqltransaction = null);

        /// <summary>
        /// SaveAds
        /// </summary>
        /// <param name="adsDTOList">adsDTOList</param>
        /// <returns>adsDTOList</returns>
        Task<List<AdsDTO>> SaveAds(List<AdsDTO> adsDTOList);

        /// <summary>
        /// GetAdContainerDTOCollection
        /// </summary>
        /// <param name="siteId">siteId</param>
        /// <param name="hash">hash</param>
        /// <param name="rebuildCache">rebuildCache</param>
        /// <returns>AdContainerDTOCollection</returns>
        //Task<AdContainerDTOCollection> GetAdContainerDTOCollection(int siteId, string hash, bool rebuildCache);

        /// <summary>
        /// AdRefresh
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="machineId">machineId</param>
        /// <returns></returns>
        Task AdRefresh(int hubId, int machineId);

        /// <summary>
        /// GetAdShowContext
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="machineId">machineId</param>
        /// <returns></returns>
        Task GetAdShowContext(int hubId, int machineId);

        /// <summary>
        /// AttachAdShowContext
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="machineId">machineId</param>
        /// <returns></returns>
        Task AttachAdShowContext(int hubId, int machineId);

        /// <summary>
        /// AttachAdContext
        /// </summary>
        /// <param name="hubId">hubId</param>
        /// <param name="machineId">machineId</param>
        /// <returns></returns>
        Task AttachAdContext(int hubId, int machineId);
    }
}
