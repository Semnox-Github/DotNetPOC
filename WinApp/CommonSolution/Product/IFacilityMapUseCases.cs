/********************************************************************************************
* Project Name - Product
* Description  - Specification of the FacilityMap use cases. 
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

namespace Semnox.Parafait.Product
{
    public interface IFacilityMapUseCases
    {
        Task<List<FacilityMapDTO>> GetFacilityMaps(List<KeyValuePair<FacilityMapDTO.SearchByParameters, string>> searchParameters,
                                                        bool loadChildRecords = false, bool activeChildRecords = true,
                                                        bool loadChildForOnlyProductType = false, SqlTransaction sqlTransaction = null);
        Task<string> SaveFacilityMaps(List<FacilityMapDTO> facilityMapDTOList);
    }
}
