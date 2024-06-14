/********************************************************************************************
* Project Name - Transcation
* Description  - Specification of the OrderHeader use cases. 
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

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// IOrderHeaderUseCases
    /// </summary>
    public interface IOrderHeaderUseCases
    {
        Task<List<OrderHeaderDTO>> GetOrderHeader(List<KeyValuePair<OrderHeaderDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false, SqlTransaction sqlTransaction = null);
        Task<List<OrderHeaderDTO>> SaveOrderHeader(List<OrderHeaderDTO> orderHeaderDTOList);
    }
}
