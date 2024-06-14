
/********************************************************************************************
 * Project Name - ScheduleCalendar
 * Description  - RemoteScheduleCalendarUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.GenericUtilities
{
    class RemoteScheduleCalendarUseCases: RemoteUseCases, IScheduleCalendarUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string ScheduleCalendar_URL = "api/Common/ScheduleCalendars";
     

        public RemoteScheduleCalendarUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }
        public async Task<List<ScheduleCalendarDTO>> GetScheduleCalendars(List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters,
               bool loadChildRecords = false, bool loadActiveChildRecord = false, SqlTransaction sqlTransaction = null)


        {
            log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecord, sqlTransaction);
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("loadChildRecords".ToString(), loadChildRecords.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("loadActiveChildRecord".ToString(), loadActiveChildRecord.ToString()));
            if (searchParameters != null)
            {
                searchParameterList.AddRange(BuildSearchParameter(searchParameters));
            }
            try
            {
                List<ScheduleCalendarDTO> result = await Get<List<ScheduleCalendarDTO>>(ScheduleCalendar_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
        private List<KeyValuePair<string, string>> BuildSearchParameter(List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> lookupSearchParams)
        {
            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string> searchParameter in lookupSearchParams)
            {
                switch (searchParameter.Key)
                {

                    case ScheduleCalendarDTO.SearchByScheduleCalendarParameters.IS_ACTIVE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("IsActive".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScheduleCalendarDTO.SearchByScheduleCalendarParameters.RECUR_END_DATE:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("RecurEndDate".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_FROM_TIME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ScheduleFromTime".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ScheduleId".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_ID_LIST:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ScheduleIdList".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_NAME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ScheduleName".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_TIME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ScheduleTime".ToString(), searchParameter.Value));
                        }
                        break;
                    case ScheduleCalendarDTO.SearchByScheduleCalendarParameters.SCHEDULE_TO_TIME:
                        {
                            searchParameterList.Add(new KeyValuePair<string, string>("ScheduleToTime".ToString(), searchParameter.Value));
                        }
                        break;

                }
            }
            log.LogMethodExit(searchParameterList);
            return searchParameterList;
        }
        public async Task<string> SaveScheduleCalendars(List<ScheduleCalendarDTO> scheduleDTOList)
        {
            log.LogMethodEntry(scheduleDTOList);
            try
            {
                string responseString = await Post<string>(ScheduleCalendar_URL, scheduleDTOList);
                log.LogMethodExit(responseString);
                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
