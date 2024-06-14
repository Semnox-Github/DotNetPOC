/********************************************************************************************
 * Project Name - Device  
 * Description  - IAttributeEnabledTablesUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.140.0      19-Aug-2021      Fiona           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.TableAttributeSetup
{
    public interface IAttributeEnabledTablesUseCases
    {
        Task<AttributeEnabledTablesContainerDTOCollection> GetAttributeEnabledTablesContainerDTOCollection(int siteId, string hash, bool rebuildCache);
        Task<string> SaveAttributeEnabledTables(List<AttributeEnabledTablesDTO> attributeEnabledTablesDTOList);
        Task<List<AttributeEnabledTablesDTO>> GetAttributeEnabledTables(List<KeyValuePair<AttributeEnabledTablesDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false, bool loadActiveChild = false);
    }
}
