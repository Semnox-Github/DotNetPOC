/********************************************************************************************
* Project Name - JobTask
* Description  - LocalJobTaskUseCases
* 
**************
**Version Log
**************
*Version     Date          Modified By             Remarks          
*********************************************************************************************
*2.120.00   22-Apr-2021   B Mahesh Pai             Created 
********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    class LocalJobTaskUseCases:IJobTaskUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalJobTaskUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<JobTaskDTO>> GetJobTasks(List<KeyValuePair<JobTaskDTO.SearchByJobTaskParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<JobTaskDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                JobTaskList maintenanceTaskList = new JobTaskList(executionContext);
                List<JobTaskDTO> jobTaskDTOList = maintenanceTaskList.GetAllJobTasks(searchParameters, sqlTransaction);

                log.LogMethodExit(jobTaskDTOList);
                return jobTaskDTOList;
            });
        }
        public async Task<string> SaveJobTasks(List<JobTaskDTO> jobTaskDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(jobTaskDTOList);
                    if (jobTaskDTOList == null)
                    {
                        throw new ValidationException("jobTaskDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            JobTaskList maintenanceTaskList = new JobTaskList(executionContext, jobTaskDTOList);
                            maintenanceTaskList.SaveJobTasks();
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
