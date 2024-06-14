/********************************************************************************************
 * Project Name - Site
 * Description  - ISiteUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         10-Nov-2020       Mushahid Faizan         Created : POS UI Redesign with REST API
 2.150.0     09-Mar-2022       Abhishek                Modified : Added GetUTCDateTime() as a part of SiteDateTime Enhancement
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Parafait.Site
{
    public interface ISiteUseCases
    {
        Task<SiteContainerDTOCollection> GetSiteContainerDTOCollection(string hash, bool onlineEnabledOnly, bool fnBEnabledOnly, bool rebuildCache);
        Task<List<SiteDTO>> GetSites(List<KeyValuePair<SiteDTO.SearchBySiteParameters, string>> searchParameters, bool loadChildRecords, bool activeChildRecords);
        Task<DateTime> GetUTCDateTime(bool rebuildCache);
    }
}
