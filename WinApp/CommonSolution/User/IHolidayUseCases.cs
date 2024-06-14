/********************************************************************************************
* Project Name - User
* Description  - Interface for Holiday Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     01-Apr-2021     Prajwal S             Created : Web Inventory UI Redesign with REST API
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    public interface IHolidayUseCases
    {
        Task<List<HolidayDTO>> GetHoliday(List<KeyValuePair<HolidayDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
      //  Task<int> GetHolidayCount(List<KeyValuePair<HolidayDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null);
        Task<List<HolidayDTO>> SaveHoliday(List<HolidayDTO> holidayDTOList);
        Task<string> DeleteHoliday(List<HolidayDTO> holidayDTOList);
    }
}
