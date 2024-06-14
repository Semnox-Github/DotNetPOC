/********************************************************************************************
 * Project Name - Game  
 * Description  - IHubDataService class to get the data  from API by doing remote call  
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

namespace Semnox.Parafait.Game
{
    public interface IHubDataService
    {
        List<HubDTO> GetHubs(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> parameters, bool loadChildRecords = false);
        string PostHubs(List<HubDTO> hubDTOList);
        string DeleteHubs(List<HubDTO> hubDTOList);
    }
}
