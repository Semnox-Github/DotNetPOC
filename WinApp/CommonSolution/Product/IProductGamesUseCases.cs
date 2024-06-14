/********************************************************************************************
* Project Name - Product
* Description  - Specification of the ProductGames use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   06-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
   public interface IProductGamesUseCases
    {
        Task<List<ProductGamesDTO>> GetProductGames(List<KeyValuePair<ProductGamesDTO.SearchByProductGamesParameters, string>> parameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveProductGames(List<ProductGamesDTO> productGamesDTOList);
        Task<string> Delete(List<ProductGamesDTO> productGamesDTOList);
    }
}
