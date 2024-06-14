/********************************************************************************************
* Project Name - User
* Description  -  IFacilityPOSAssignmentUseCases  class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.120.00    06-Apr-2021       B Mahesh Pai        Created : POS UI Redesign with REST API
********************************************************************************************/
using Semnox.Parafait.POS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
   public interface IFacilityPOSAssignmentUseCases
    {
        Task<List<FacilityPOSAssignmentDTO>> GetFacilityPOSAssignments(List<KeyValuePair<FacilityPOSAssignmentDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<string> SaveFacilityPOSAssignments(List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList);
        Task<string> Delete(List<FacilityPOSAssignmentDTO> facilityPOSAssignmentDTOList);
    }
}
