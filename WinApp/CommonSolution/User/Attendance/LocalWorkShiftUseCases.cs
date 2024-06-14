/********************************************************************************************
 * Project Name - User
 * Description  - LocalWorkShiftUseCases class 
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
    public class LocalWorkShiftUseCases : IWorkShiftUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalWorkShiftUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<WorkShiftDTO>> GetWorkShift(List<KeyValuePair<WorkShiftDTO.SearchByWorkShiftParameters, string>>
                          searchParameters, bool loadChildRecords, bool activeChildRecords = false, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<WorkShiftDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                WorkShiftListBL workShiftListBL = new WorkShiftListBL(executionContext);
                List<WorkShiftDTO> workShiftDTOList = workShiftListBL.GetWorkShiftDTOList(searchParameters, loadChildRecords, activeChildRecords,  sqlTransaction);

                log.LogMethodExit(workShiftDTOList);
                return workShiftDTOList;
            });
        }

        //public async Task<int> GetWorkShiftCount(List<KeyValuePair<WorkShiftDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        WorkShiftListBL WorkShiftsListBL = new WorkShiftListBL(executionContext);
        //        int count = WorkShiftsListBL.GetWorkShiftCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<WorkShiftDTO>> SaveWorkShift(List<WorkShiftDTO> workShiftDTOList)
        {
            return await Task<List<WorkShiftDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    WorkShiftListBL workShiftList = new WorkShiftListBL(executionContext, workShiftDTOList);
                    List<WorkShiftDTO> result = workShiftList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    
}
}
