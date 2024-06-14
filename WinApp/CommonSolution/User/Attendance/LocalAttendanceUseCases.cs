/********************************************************************************************
 * Project Name - User
 * Description  - LocalAttendanceUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.0      01-Apr-2021      Prajwal S                 Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.User
{
    class LocalAttendanceUseCases : IAttendanceUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAttendanceUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AttendanceDTO>> GetAttendance(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>
                          searchParameters, bool loadChildRecords, bool activeChildRecords, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<AttendanceDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AttendanceList attendanceListBL = new AttendanceList(executionContext);
                List<AttendanceDTO> AttendanceDTOList = attendanceListBL.GetAttendance(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(AttendanceDTOList);
                return AttendanceDTOList;
            });
        }

        public async Task<int> GetAttendanceCount(List<KeyValuePair<AttendanceDTO.SearchByParameters, string>>
                                                      searchParameters, SqlTransaction sqlTransaction = null
                             )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AttendanceList attendanceListBL = new AttendanceList(executionContext);
                int count = attendanceListBL.GetAttendanceCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<List<AttendanceDTO>> SaveAttendance(List<AttendanceDTO> attendanceDTOList)
        {
            return await Task<List<AttendanceDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    AttendanceList attendanceList = new AttendanceList(executionContext, attendanceDTOList);
                    List<AttendanceDTO> result = attendanceList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
