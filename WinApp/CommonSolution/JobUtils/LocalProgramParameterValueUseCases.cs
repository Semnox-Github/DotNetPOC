/********************************************************************************************
* Project Name - ProgramParameterValue
* Description  - LocalProgramParameterValueUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.1   18-May-2021   B Mahesh Pai             Created 
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
    class LocalProgramParameterValueUseCases : IProgramParameterValueUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalProgramParameterValueUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<ProgramParameterValueDTO>> GetProgramParameterValues(List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchParameters,
                                       bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<ProgramParameterValueDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                ProgramParameterValueListBL ProgramParameterValueListBL = new ProgramParameterValueListBL(executionContext);
                List<ProgramParameterValueDTO> programParameterValueDTOList = ProgramParameterValueListBL.GetAllProgramParameterValueDTOList(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);

                log.LogMethodExit(programParameterValueDTOList);
                return programParameterValueDTOList;
            });
        }

        public async Task<string> SaveProgramParameterValues(List<ProgramParameterValueDTO> programParameterValueDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;

                log.LogMethodEntry(programParameterValueDTOList);
                if (programParameterValueDTOList == null)
                {
                    throw new ValidationException("programParameterValueDTOList is Empty");
                }
                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    try
                    {
                        parafaitDBTrx.BeginTransaction();
                        ProgramParameterValueListBL ProgramParameterValueListBL = new ProgramParameterValueListBL(executionContext, programParameterValueDTOList);
                        ProgramParameterValueListBL.Save();
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
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<string> Delete(List<ProgramParameterValueDTO> programParameterValueDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(programParameterValueDTOList);
                    ProgramParameterValueListBL ProgramParameterValueListBL = new ProgramParameterValueListBL(executionContext, programParameterValueDTOList);
                    ProgramParameterValueListBL.Delete();
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
