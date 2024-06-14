/********************************************************************************************
 * Project Name - Utilities  
 * Description  - ILookupDataService class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;

namespace Semnox.Core.Utilities
{
    public interface ILookupDataService
    {
        List<LookupsDTO> GetLookups(List<KeyValuePair<LookupsDTO.SearchByParameters, string>> parameters, bool loadChildRecords = false);
        string PostLookups(List<LookupsDTO> lookupsDTOList);
        string DeleteLookups(List<LookupsDTO> lookupsDTOList);
    }
}
