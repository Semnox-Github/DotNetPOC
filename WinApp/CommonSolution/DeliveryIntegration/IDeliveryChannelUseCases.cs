/********************************************************************************************
* Project Name - DeliveryIntegration
* Description  - Specification of the DeliveryChannel use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   11-Mar-2021   Prajwal S                Created 
********************************************************************************************/
using System.Collections.Generic;
using System.Data.SqlClient; 
using System.Threading.Tasks;

namespace Semnox.Parafait.DeliveryIntegration
{
    /// <summary>
    /// IDeliveryChannelUseCases
    /// </summary>
    public interface IDeliveryChannelUseCases
    {
        Task<List<DeliveryChannelDTO>> GetDeliveryChannel(List<KeyValuePair<DeliveryChannelDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<DeliveryChannelDTO>> SaveDeliveryChannel(List<DeliveryChannelDTO> deliveryChannelDTOList);
    }
}
