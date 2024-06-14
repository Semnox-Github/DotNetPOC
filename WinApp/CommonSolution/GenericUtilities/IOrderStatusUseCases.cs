/********************************************************************************************
* Project Name - Transcation
* Description  - Specification of the OrderStatus use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   11-Mar-2021   Prajwal S                Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.GenericUtilities
{
    public interface IOrderStatusUseCases
    {
        Task<List<OrderStatusDTO>> GetOrderStatus(List<KeyValuePair<OrderStatusDTO.SearchByParameters, string>> searchParameters,  SqlTransaction sqlTransaction = null);
        Task<List<OrderStatusDTO>> SaveOrderStatus(List<OrderStatusDTO> orderStatusDTOList);
    }
}
