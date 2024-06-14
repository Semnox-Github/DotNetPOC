/********************************************************************************************
* Project Name - Product
* Description  - Specification of the OrderTypeGroup use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   08-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public interface IOrderTypeGroupUseCases
    {
        Task<List<OrderTypeGroupDTO>> GetOrderTypeGroups(List<KeyValuePair<OrderTypeGroupDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveOrderTypeGroups(List<OrderTypeGroupDTO> orderTypeGroupDTOList);
    }
}
