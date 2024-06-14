using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.POS
{
    class LocalShiftUseCases : IShiftUseCases
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
            return await Task<List<ShiftDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    ShiftListBL shiftList = new ShiftListBL(executionContext, shiftDTOList);
                    List<ShiftDTO> result = shiftList.Save();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }
    }
}
