/********************************************************************************************
* Project Name - DBSynch
* Description  - Specification of the IDbSynchTableUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   05-May-2021   Roshan Devadiga          Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.DBSynch
{
    /// <summary>
    /// IDbSynchTableUseCases
    /// </summary>
    public interface IDbSynchTableUseCases
    {
        Task<List<DBSynchTableDTO>> GetDBSynchs(List<KeyValuePair<DBSynchTableDTO.SearchByParameters, string>> searchParameters);
        Task<string> SaveDBSynchs(List<DBSynchTableDTO> dBSynchDTOList);
        Task<string> Delete(List<DBSynchTableDTO> dBSynchDTOList);
    }
}
