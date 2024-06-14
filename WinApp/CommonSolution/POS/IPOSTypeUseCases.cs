using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    public interface IPOSTypeUseCases
    {
        Task<List<POSTypeDTO>> GetPOSTypes(List<KeyValuePair<POSTypeDTO.SearchByParameters, string>> parameters, SqlTransaction sqlTransaction = null);
        Task<string> SavePOSTypes(List<POSTypeDTO> pOSTypeDTOList);
        Task<string> DeletePOSTypes(List<POSTypeDTO> pOSTypeDTOList);
    }
}
