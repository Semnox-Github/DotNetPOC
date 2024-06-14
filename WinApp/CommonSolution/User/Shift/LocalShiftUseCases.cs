/********************************************************************************************
* Project Name - User
* Description  - Use case for Shift Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.140.0     14-Sep-2021     Deeksha               Modified : Provisional Shift changes
*2.140.0     16-Aug-2021     Girish                Modified : Multicash drawer changes
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Parafait.User;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.User
{
    public class LocalShiftUseCases : IShiftUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalShiftUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ShiftDTO>> GetShift(List<KeyValuePair<ShiftDTO.SearchByShiftParameters, string>>
                          searchParameters, bool loadChildRecords, bool buildReceipt = false, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<ShiftDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                ShiftListBL shiftListBL = new ShiftListBL(executionContext);
                List<ShiftDTO> shiftDTOList = shiftListBL.GetShiftDTOList(searchParameters, loadChildRecords, buildReceipt,  /*currentPage, pageSize,*/ sqlTransaction);

                log.LogMethodExit(shiftDTOList);
                return shiftDTOList;
            });
        }

        //public async Task<int> GetShiftCount(List<KeyValuePair<ShiftDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        ShiftListBL ShiftsListBL = new ShiftListBL(executionContext);
        //        int count = ShiftsListBL.GetShiftCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<ShiftDTO>> SaveShift(List<ShiftDTO> shiftDTOList)
        {
            log.LogMethodEntry(shiftDTOList);
            return await Task<List<ShiftDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ShiftListBL shiftList = new ShiftListBL(executionContext, shiftDTOList);
                    List<ShiftDTO> result = shiftList.Save();
                    transaction.EndTransaction();
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }
        public async Task<ShiftDTO> AssignCashdrawer(int shiftId, CashdrawerActivityDTO cashdrawerActivityDTO)
        {
            log.LogMethodEntry(shiftId, cashdrawerActivityDTO);
            return await Task<ShiftDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ShiftBL shiftBL = new ShiftBL(executionContext, shiftId);
                    ShiftDTO result = shiftBL.AssignCashdrawer(cashdrawerActivityDTO, transaction.SQLTrx);
                    transaction.EndTransaction();
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }

        public async Task<ShiftDTO> UnAssignCashdrawer(int shiftId, CashdrawerActivityDTO cashdrawerActivityDTO)
        {
            log.LogMethodEntry(shiftId, cashdrawerActivityDTO);
            return await Task<ShiftDTO>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ShiftBL shiftBL = new ShiftBL(executionContext, shiftId);
                    ShiftDTO result = shiftBL.UnAssignCashdrawer(cashdrawerActivityDTO, transaction.SQLTrx);
                    transaction.EndTransaction();
                    log.LogMethodExit(result);
                    return result;
                }
            });
        }
    }
}
