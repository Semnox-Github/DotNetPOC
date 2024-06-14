/********************************************************************************************
* Project Name - ConcurrentProgram
* Description  - LocalConcurrentProgramsUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   27-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    class LocalConcurrentProgramsUseCases:IConcurrentProgramsUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalConcurrentProgramsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ConcurrentProgramsDTO>> GetConcurrentPrograms(List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                          SqlTransaction sqlTransaction = null)
        {
            return await Task<List<ConcurrentProgramsDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, loadActiveRecords, sqlTransaction);

                ConcurrentProgramList concurrentProgramListBL = new ConcurrentProgramList(executionContext);
                List<ConcurrentProgramsDTO> concurrentProgramDTOList = concurrentProgramListBL.GetAllConcurrentPrograms(searchParameters, loadChildRecords, loadActiveRecords, null);

                log.LogMethodExit(concurrentProgramDTOList);
                return concurrentProgramDTOList;
            });
        }
        public async Task<string> SaveConcurrentPrograms(List<ConcurrentProgramsDTO> concurrentProgramsDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(concurrentProgramsDTOList);
                    if (concurrentProgramsDTOList == null)
                    {
                        throw new ValidationException("concurrentProgramsDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(concurrentProgramsDTOList, executionContext);
                            concurrentProgramList.Save();
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
