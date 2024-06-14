/********************************************************************************************
* Project Name - PriceList
* Description  - Specification of the PriceList use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   10-Mar-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.PriceList
{
    public interface IPriceListUseCases
    {
        Task<List<PriceListDTO>> GetPriceLists(List<KeyValuePair<PriceListDTO.SearchByPriceListParameters, string>> searchParameters, bool loadActiveRecordsOnly = false, SqlTransaction sqlTransaction = null);
        Task<string> SavePriceLists(List<PriceListDTO> priceListDTOList);
        Task<string> Delete(List<PriceListDTO> priceListDTOList);
    }
}
