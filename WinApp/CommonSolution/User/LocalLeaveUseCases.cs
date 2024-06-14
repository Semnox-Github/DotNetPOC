/********************************************************************************************
 * Project Name - User
 * Description  - LocalLeaveUseCases class 
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
    class LocalLeaveUseCases : ILeaveUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLeaveUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<LeaveDTO>> GetLeave(List<KeyValuePair<LeaveDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<LeaveDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                LeaveListBL LeavesListBL = new LeaveListBL(executionContext);
                List<LeaveDTO> LeaveDTOList = LeavesListBL.GetLeaveDTOList(searchParameters, sqlTransaction);

                log.LogMethodExit(LeaveDTOList);
                return LeaveDTOList;
            });
        }

        public async Task<List<LeaveDTO>> GenerateLeave(int leaveCycleId = -1 )
        {
            return await Task<List<LeaveDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(leaveCycleId);

                LeaveListBL LeavesListBL = new LeaveListBL(executionContext);
                List<LeaveDTO> leaveDTOList = LeavesListBL.GenerateLeave(leaveCycleId);

                log.LogMethodExit(leaveDTOList);
                return leaveDTOList;
            });
        }

        public async Task<List<LeaveDTO>> PopulateInbox(int userId = -1)
        {
            return await Task<List<LeaveDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(userId);

                LeaveListBL LeavesListBL = new LeaveListBL(executionContext);
                List<LeaveDTO> leaveDTOList = LeavesListBL.PopulateInbox(userId);

                log.LogMethodExit(leaveDTOList);
                return leaveDTOList;
            });
        }

        //public async Task<int> GetLeaveCount(List<KeyValuePair<LeaveDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        LeaveListBL LeavesListBL = new LeaveListBL(executionContext);
        //        int count = LeavesListBL.GetLeaveCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<LeaveDTO>> SaveLeave(List<LeaveDTO> LeaveDTOList)
        {
            return await Task<List<LeaveDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    LeaveListBL LeaveList = new LeaveListBL(executionContext, LeaveDTOList);
                    List<LeaveDTO> result = LeaveList.SaveUpdateLeave();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<string> DeleteLeave(List<LeaveDTO> LeaveDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(LeaveDTOList);
                    LeaveListBL LeavesList = new LeaveListBL(executionContext, LeaveDTOList);
                    LeavesList.Delete();
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
