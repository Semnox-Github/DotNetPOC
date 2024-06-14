/********************************************************************************************
* Project Name - DeliveryIntegration
* Description  - IOnlineOrderDeliveryIntegrationUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.150.00   13-Jul-2022    Guru S A                  Created 
********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient; 
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// IOnlineOrderDeliveryIntegrationUseCases
    /// </summary>
    public interface IOnlineOrderDeliveryIntegrationUseCases
    {
        /// <summary>
        /// GetOnlineOrderDeliveryIntegration
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        Task<List<OnlineOrderDeliveryIntegrationDTO>> GetOnlineOrderDeliveryIntegration(
                                    List<KeyValuePair<OnlineOrderDeliveryIntegrationDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, 
                                    bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null);
        /// <summary>
        /// SaveOnlineOrderDeliveryIntegration
        /// </summary>
        /// <param name="onlineOrderDeliveryIntegrationDTOList"></param>
        /// <returns></returns>
        Task<List<OnlineOrderDeliveryIntegrationDTO>> SaveOnlineOrderDeliveryIntegration(List<OnlineOrderDeliveryIntegrationDTO> onlineOrderDeliveryIntegrationDTOList);
        /// <summary>
        /// Gets the Container Object of OnlineOrderDeliveryIntegration .
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="hash"></param>
        /// <param name="rebuildCache"></param>
        /// <returns></returns>
        Task<OnlineOrderDeliveryIntegrationContainerDTOCollection> GetOnlineOrderDeliveryIntegrationContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
