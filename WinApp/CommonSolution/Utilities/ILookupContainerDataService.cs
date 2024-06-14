/********************************************************************************************
 * Project Name - Utilities  Class
 * Description  - ILookupContainerDataService class to get the data  from API by doing remote call  
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
    //public interface ILookupViewContainerDataService
    //{
    //   // LookupViewDTOCollection Get(string hash, bool rebuildCache);
    //}
    public interface ILookupContainerDataService
    {
       List<LookupsDTO> Get(DateTime? maxLastUpdatedDate, string hash);
    }
}
