/********************************************************************************************
* Project Name - Product
* Description  - Combo Product GET,SAVE,DELETE use cases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   14-Sep-2021    Prajwal S       Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IComboProductUseCases
    {
        Task<List<ComboProductDTO>> GetComboProduct(List<KeyValuePair<ComboProductDTO.SearchByParameters, string>> searchParameters, int currentPage = 0, int pageSize = 0, SqlTransaction sqlTransaction = null);
        //  Task<int> GetPayConfigurationCount(List<KeyValuePair<PayConfigurationDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<ComboProductDTO>> SaveComboProduct(List<ComboProductDTO> comboProductDTOList);

        Task<string> DeleteComboProduct(List<ComboProductDTO> comboProductDTOList);
    }
}
