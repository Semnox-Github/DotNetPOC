/********************************************************************************************
 * Project Name - User
 * Description  - LocalHolidayUseCases class 
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
    class LocalHolidayUseCases : IHolidayUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalHolidayUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<HolidayDTO>> GetHoliday(List<KeyValuePair<HolidayDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<HolidayDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                HolidayListBL holidayListBL = new HolidayListBL(executionContext);
                List<HolidayDTO> HolidayDTOList = holidayListBL.GetAllHolidayList(searchParameters, sqlTransaction);

                log.LogMethodExit(HolidayDTOList);
                return HolidayDTOList;
            });
        }

        //public async Task<int> GetHolidayCount(List<KeyValuePair<HolidayDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        HolidayListBL holidayListBL = new HolidayListBL(executionContext);
        //        int count = holidayListBL.GetHolidayCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<HolidayDTO>> SaveHoliday(List<HolidayDTO> holidayDTOList)
        {
            return await Task<List<HolidayDTO>>.Factory.StartNew(() =>
            {

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    parafaitDBTrx.BeginTransaction();
                    HolidayListBL holidayList = new HolidayListBL(executionContext, holidayDTOList);
                    List<HolidayDTO> holidayDTOLists =  holidayList.SaveUpdateHoliday(parafaitDBTrx.SQLTrx);
                    parafaitDBTrx.EndTransaction();
                    return holidayDTOLists;
                }
            });
        }

        public async Task<string> DeleteHoliday(List<HolidayDTO> HolidayDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(HolidayDTOList);
                    HolidayListBL holidayList = new HolidayListBL(executionContext, HolidayDTOList);
                    holidayList.Delete();
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
