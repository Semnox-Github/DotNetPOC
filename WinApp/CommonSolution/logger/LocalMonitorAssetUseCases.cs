/********************************************************************************************
 * Project Name -MonitorAsset
 * Description  - LocalMonitorAssetUseCases class
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
    class LocalMonitorAssetUseCases:IMonitorAssetUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        public LocalMonitorAssetUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public async Task<List<MonitorAssetDTO>> GetMonitorAssets(List<KeyValuePair<MonitorAssetDTO.SearchByMonitorAssetParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)

        {
            return await Task<List<MonitorAssetDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(searchParameters);


                MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext);
                List<MonitorAssetDTO> monitorAssetDTOList = monitorAssetList.GetAllMonitorAssets(searchParameters, sqlTransaction);

                log.LogMethodExit(monitorAssetDTOList);
                return monitorAssetDTOList;
            });
        }
        public async Task<string> SaveMonitorAssets(List<MonitorAssetDTO> monitorAssetDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(monitorAssetDTOList);
                    if (monitorAssetDTOList == null)
                    {
                        throw new ValidationException("monitorAssetDTOList is Empty");
                    }
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext, monitorAssetDTOList);
                            monitorAssetList.Save();
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
        public async Task<string> Delete(List<MonitorAssetDTO> monitorAssetDTOList)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                string result = string.Empty;
                try
                {
                    log.LogMethodEntry(monitorAssetDTOList);
                    MonitorAssetList monitorAssetList = new MonitorAssetList(executionContext, monitorAssetDTOList);
                    monitorAssetList.Delete();
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
