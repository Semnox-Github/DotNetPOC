/********************************************************************************************
 * Project Name -MonitorPriority
 * Description  - LocalMonitorPriorityUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         11-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    class LocalMonitorPriorityUseCases:IMonitorPriorityUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMonitorPriorityUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MonitorPriorityDTO>> GetMonitorPriorities(List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>> searchParameters)


        {
            return await Task<List<MonitorPriorityDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);


                MonitorPriorityList monitorPriorityList = new MonitorPriorityList(executionContext);
                List<MonitorPriorityDTO> monitorPriorityDTOList = monitorPriorityList.GetAllMonitorPriorityList(searchParameters);

                log.LogMethodExit(monitorPriorityDTOList);
                return monitorPriorityDTOList;
            });
        }
        public async Task<string> SaveMonitorPriorities(List<MonitorPriorityDTO> monitorPriorityDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(monitorPriorityDTOList);
                    if (monitorPriorityDTOList == null)
                    {
                        throw new ValidationException("monitorPriorityDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MonitorPriorityList monitorPriorityList = new MonitorPriorityList(executionContext, monitorPriorityDTOList);
                            monitorPriorityList.Save();
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
        public async Task<string> Delete(List<MonitorPriorityDTO> monitorPriorityDTOList)
    
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(monitorPriorityDTOList);
                    MonitorPriorityList monitorPriorityList = new MonitorPriorityList(executionContext, monitorPriorityDTOList);
                    monitorPriorityList.Delete();
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
