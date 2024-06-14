/********************************************************************************************
 * Project Name - ScheduleCalendar
 * Description  - LocalScheduleCalendarUseCases class
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
    class LocalScheduleCalendarUseCases:IScheduleCalendarUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalScheduleCalendarUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ScheduleCalendarDTO>> GetScheduleCalendars(List<KeyValuePair<ScheduleCalendarDTO.SearchByScheduleCalendarParameters, string>> searchParameters,
                     bool loadChildRecords = false, bool loadActiveChildRecord = false,
                      SqlTransaction sqlTransaction = null)

        {
            return await Task<List<ScheduleCalendarDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveChildRecord, sqlTransaction);

                ScheduleCalendarListBL scheduleList = new ScheduleCalendarListBL(executionContext);
                List<ScheduleCalendarDTO> scheduleDTOList = scheduleList.GetAllSchedule(searchParameters, loadChildRecords, loadActiveChildRecord, sqlTransaction);

                log.LogMethodExit(scheduleDTOList);
                return scheduleDTOList;
            });
        }
        public async Task<string> SaveScheduleCalendars(List<ScheduleCalendarDTO> scheduleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(scheduleDTOList);
                    if (scheduleDTOList == null)
                    {
                        throw new ValidationException("customerFeedbackSurveyDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ScheduleCalendarListBL schedule = new ScheduleCalendarListBL(executionContext, scheduleDTOList);
                            schedule.Save();
                            parafaitDBTrx.EndTransaction();
                        }
                        catch (ValidationException valEx)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(valEx);
                            throw valEx;
                        }
                        catch (Exception ex)
                        {
                            parafaitDBTrx.RollBack();
                            log.Error(ex);
                            log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                            throw ex;
                        }
                    }
                    result = "Success";
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = "Falied";
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
