/********************************************************************************************
* Project Name - Product
* Description  - Specification of the ModifierSet use cases. 
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   08-Mar-2021   Roshan Devadiga        Created 
*2.140.00   14-Sep-2021   Prajwal S              Modified : Get for container.
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Product
{
    public interface IModifierSetUseCases
    {
        Task<List<ModifierSetDTO>> GetModifierSets(List<KeyValuePair<ModifierSetDTO.SearchByParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveChildRecords = false, SqlTransaction sqlTransaction = null);
        Task<string> SaveModifierSets(List<ModifierSetDTO> modifierSetDTOList);
        Task<string> Delete(List<ModifierSetDTO> modifierSetDTOList);
        Task<ModifierSetContainerDTOCollection> GetModifierSetContainerDTOCollection(int siteId, string hash, bool rebuildCache);
    }
}
