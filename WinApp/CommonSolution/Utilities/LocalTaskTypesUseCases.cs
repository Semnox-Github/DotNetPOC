/********************************************************************************************
 * Project Name - Utilities
 * Description  - LocalTaskTypesUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.120.00    02-Mar-2021       Roshan Devadiga        Created : POS UI Redesign with REST API
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    /// <summary>
    /// Implementation of tasktypes use-cases
    /// </summary>
    class LocalTaskTypesUseCases : ITaskTypesUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ExecutionContext executionContext;
        public LocalTaskTypesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<TaskTypesDTO>> GetTaskTypes(List<KeyValuePair<TaskTypesDTO.SearchByParameters, string>>
                         searchParameters)
        {
            return await Task<List<TaskTypesDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);

                TaskTypesList taskTypesListBL = new TaskTypesList(executionContext);
                List<TaskTypesDTO> taskTypesDTOList = taskTypesListBL.GetAllTaskTypes(searchParameters);

                log.LogMethodExit(taskTypesDTOList);
                return taskTypesDTOList;
            });
        }
        public async Task<string> SaveTaskTypes(List<TaskTypesDTO> taskTypeDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                log.LogMethodEntry(taskTypeDTOList);
                if (taskTypeDTOList == null)
                {
                    throw new ValidationException("taskTypeDTOList is Empty");
                }

                using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                {
                    foreach (TaskTypesDTO taskTypesDTO in taskTypeDTOList)
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            TaskTypes taskTypesBL = new TaskTypes(executionContext, taskTypesDTO);
                            taskTypesBL.Save();
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
                }

                result = "Success";
                log.LogMethodExit(result);
                return result;
            });
        }
        public async Task<TaskTypesContainerDTOCollection> GetTaskTypesContainerDTOCollection(int siteId, string hash, bool rebuildCache)
        {
            return await Task<TaskTypesContainerDTOCollection>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(siteId, hash, rebuildCache);
                if (rebuildCache)
                {
                    TaskTypesContainerList.Rebuild(siteId);
                }
                List<TaskTypesContainerDTO> taskTypesContainerDTOs = TaskTypesContainerList.GetTaskTypesContainerDTOList(siteId);
                TaskTypesContainerDTOCollection result = new TaskTypesContainerDTOCollection(taskTypesContainerDTOs);
                if (hash == result.Hash)
                {
                    log.LogMethodExit(null, "No changes to the cache");
                    return null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
        
    }
}
