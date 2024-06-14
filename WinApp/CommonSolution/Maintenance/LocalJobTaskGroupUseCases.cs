/********************************************************************************************
* Project Name - JobTaskGroup
* Description  - LocalJobTaskGroupUseCases
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    class LocalJobTaskGroupUseCases:IJobTaskGroupUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalJobTaskGroupUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<JobTaskGroupDTO>> GetJobTaskGroups(List<KeyValuePair<JobTaskGroupDTO.SearchByJobTaskGroupParameters, string>> searchParameters)

        {
            return await Task<List<JobTaskGroupDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                JobTaskGroupList maintenanceTaskGroupList = new JobTaskGroupList(executionContext);
                List<JobTaskGroupDTO> jobTaskGroupDTOList = maintenanceTaskGroupList.GetAllJobTaskGroups(searchParameters);

                log.LogMethodExit(jobTaskGroupDTOList);
                return jobTaskGroupDTOList;
            });
        }
        public async Task<string> SaveJobTaskGroups(List<JobTaskGroupDTO> jobTaskGroupDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(jobTaskGroupDTOList);
                    if (jobTaskGroupDTOList == null)
                    {
                        throw new ValidationException("assetGroupDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            JobTaskGroupList maintenanceTaskGroupList = new JobTaskGroupList(executionContext, jobTaskGroupDTOList);
                            maintenanceTaskGroupList.SaveJobTaskGroup();
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
