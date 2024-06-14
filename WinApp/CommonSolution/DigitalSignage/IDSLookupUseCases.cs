/********************************************************************************************
* Project Name - DigitalSignage
* Description  - Specification of the DSLookup use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.140.00   22-Apr-2021   Roshan Devadiga        Created 
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DigitalSignage
{
   public interface IDSLookupUseCases
    {
        Task<List<DSLookupDTO>> GetDSLookups(List<KeyValuePair<DSLookupDTO.SearchByParameters, string>> searchParameters,
                                                                                bool loadChildRecords = false, bool activeChildRecords = true, SqlTransaction sqlTransaction = null);
        Task<string> SaveDSLookups(List<DSLookupDTO> lookupDTOList);
    }
}
