/********************************************************************************************
 * Project Name -Monitor
 * Description  - LocalMonitorUseCases class
 *
 **************
 ** Version Log
  **************
  * Version      Date              Modified By         Remarks
 *********************************************************************************************
 2.120.0         10-May-2021       B Mahesh Pai       Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    class LocalMonitorUseCases:IMonitorUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMonitorUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MonitorDTO>> GetMonitors(List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters, int currentPage, int pageSize, bool loadChildRecords = false, bool loadActiveRecords = false,
                                           SqlTransaction sqlTransaction = null)

        {
            return await Task<List<MonitorDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters, currentPage, pageSize, loadChildRecords,loadActiveRecords, sqlTransaction);

                MonitorList monitorList = new MonitorList(executionContext);
                List<MonitorDTO> monitorDTOList = monitorList.GetAllMonitorDTOList(searchParameters, currentPage,  pageSize, loadChildRecords, loadActiveRecords, sqlTransaction);

                log.LogMethodExit(monitorDTOList);
                return monitorDTOList;
            });
        }
        public async Task<string> SaveMonitors(List<MonitorDTO> monitorDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(monitorDTOList);
                    if (monitorDTOList == null)
                    {
                        throw new ValidationException("monitorDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MonitorList monitorList = new MonitorList(monitorDTOList, executionContext);
                            monitorList.Save();
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
        public async Task<string> Delete(List<MonitorDTO> monitorDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(monitorDTOList);
                    MonitorList monitorList = new MonitorList(monitorDTOList, executionContext);
                    monitorList.Delete();
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

