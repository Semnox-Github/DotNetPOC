/********************************************************************************************
 * Project Name - ScheduleCalendar
 * Description  - IScheduleCalendarUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
  public  interface IScheduleCalendarUseCases
    {
       Task<List<ScheduleCalendarDTO>> GetScheduleCalendars(List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters,
                     bool loadChildRecords = false, bool loadActiveChildRecord = false,
                      SqlTransaction sqlTransaction = null);
        Task<string> SaveScheduleCalendars(List<ScheduleCalendarDTO> scheduleDTOList);
    }
}
