/********************************************************************************************
 * Project Name - User
 * Description  - LocalAttendanceReaderUseCases class 
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
    class LocalAttendanceReaderUseCases : IAttendanceReaderUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalAttendanceReaderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<AttendanceReaderDTO>> GetAttendanceReader(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<AttendanceReaderDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AttendanceReaderListBL attendanceReadersListBL = new AttendanceReaderListBL(executionContext);
                List<AttendanceReaderDTO> attendanceReaderDTOList = attendanceReadersListBL.GetAllAttendanceReaderList(searchParameters, sqlTransaction);

                log.LogMethodExit(attendanceReaderDTOList);
                return attendanceReaderDTOList;
            });
        }

        public async Task<int> GetAttendanceReaderCount(List<KeyValuePair<AttendanceReaderDTO.SearchByParameters, string>>
                                                      searchParameters, SqlTransaction sqlTransaction = null
                             )
        {
            return await Task<int>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                AttendanceReaderListBL attendanceReadersListBL = new AttendanceReaderListBL(executionContext);
                int count = attendanceReadersListBL.GetAttendanceReaderCount(searchParameters, sqlTransaction);

                log.LogMethodExit(count);
                return count;
            });
        }

        public async Task<List<AttendanceReaderDTO>> SaveAttendanceReader(List<AttendanceReaderDTO> attendanceReaderDTOList)
        {
            return await Task<List<AttendanceReaderDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    AttendanceReaderListBL attendanceReaderList = new AttendanceReaderListBL(executionContext, attendanceReaderDTOList);
                    List<AttendanceReaderDTO> result = attendanceReaderList.SaveUpdateAttendanceReader();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<string> DeleteAttendanceReader(List<AttendanceReaderDTO> attendanceReaderDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(attendanceReaderDTOList);
                    AttendanceReaderListBL attendanceReadersList = new AttendanceReaderListBL(executionContext, attendanceReaderDTOList);
                    attendanceReadersList.Delete();
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
