/********************************************************************************************
* Project Name - Product
* Description  - Specification of the Facility use cases. 
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
    public interface IFacilityUseCases
    {
        Task<List<FacilityDTO>> GetFacilitys(List<KeyValuePair<FacilityDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool activeChildRecords = false,
                                              SqlTransaction sqlTransaction = null);
        Task<string> SaveFacilitys(List<FacilityDTO> facilityDTOList);
        Task<string> Delete(List<FacilityDTO> facilityDTOList);
    }
}
