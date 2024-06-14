/********************************************************************************************
 * Project Name - User
 * Description  - LocalLeaveCycleUseCases class 
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
    class LocalLeaveCycleUseCases : ILeaveCycleUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalLeaveCycleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<LeaveCycleDTO>> GetLeaveCycle(List<KeyValuePair<LeaveCycleDTO.SearchByParameters, string>>
                          searchParameters, SqlTransaction sqlTransaction = null
                         )
        {
            return await Task<List<LeaveCycleDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                LeaveCycleListBL LeaveCyclesListBL = new LeaveCycleListBL(executionContext);
                List<LeaveCycleDTO> LeaveCycleDTOList = LeaveCyclesListBL.GetAllLeaveCycleList(searchParameters, sqlTransaction);

                log.LogMethodExit(LeaveCycleDTOList);
                return LeaveCycleDTOList;
            });
        }

        //public async Task<int> GetLeaveCycleCount(List<KeyValuePair<LeaveCycleDTO.SearchByParameters, string>>
        //                                              searchParameters, SqlTransaction sqlTransaction = null
        //                     )
        //{
        //    return await Task<int>.Factory.StartNew(() =>
        //    {
        //        log.LogMethodEntry(searchParameters);

        //        LeaveCycleListBL LeaveCyclesListBL = new LeaveCycleListBL(executionContext);
        //        int count = LeaveCyclesListBL.GetLeaveCycleCount(searchParameters, sqlTransaction);

        //        log.LogMethodExit(count);
        //        return count;
        //    });
        //}

        public async Task<List<LeaveCycleDTO>> SaveLeaveCycle(List<LeaveCycleDTO> LeaveCycleDTOList)
        {
            return await Task<List<LeaveCycleDTO>>.Factory.StartNew(() =>
            {
                using (ParafaitDBTransaction transaction = new ParafaitDBTransaction())
                {
                    transaction.BeginTransaction();
                    LeaveCycleListBL LeaveCycleList = new LeaveCycleListBL(executionContext, LeaveCycleDTOList);
                    List<LeaveCycleDTO> result = LeaveCycleList.SaveUpdateLeaveCycle();
                    transaction.EndTransaction();
                    return result;
                }
            });
        }

        public async Task<string> DeleteLeaveCycle(List<LeaveCycleDTO> LeaveCycleDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(LeaveCycleDTOList);
                    LeaveCycleListBL LeaveCyclesList = new LeaveCycleListBL(executionContext, LeaveCycleDTOList);
                    LeaveCyclesList.Delete();
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
