/********************************************************************************************
* Project Name - Product
* Description  - Specification of the SalesOfferGroup use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   11-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface ISalesOfferGroupUseCases
    {
        Task<List<SalesOfferGroupDTO>> GetSalesOfferGroups(List<KeyValuePair<SalesOfferGroupDTO.SearchBySalesOfferGroupParameters, string>> searchParameters);
        Task<string> SaveSalesOfferGroups(List<SalesOfferGroupDTO> salesOfferGroupsList);
    }
}
