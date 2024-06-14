/********************************************************************************************
* Project Name - Game
* Description  - Interface for GenericCalendar Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     14-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public interface IGenericCalendarUseCases
    {
        Task<List<GenericCalendarDTO>> GetGenericCalendars(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>>
                           searchParameters, string genericColIdName = "", string moduleName = "", int entityId = 0, SqlTransaction sqlTransaction = null);
        Task<string> SaveGenericCalendars(List<GenericCalendarDTO> genericCalendarDTOList);
        Task<int> GetGenericCalendarCount(List<KeyValuePair<GenericCalendarDTO.SearchByGenericCalendarParameters, string>>
                                                       searchParameters, SqlTransaction sqlTransaction = null);

    }
}
