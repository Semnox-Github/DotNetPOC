/********************************************************************************************
 * Project Name - Currecny
 * Description  - Concrete implementation of monitorLog use cases.  
 *  
 **************
 **Version Log
 **************
 *Version     Date              Modified By              Remarks          
 *********************************************************************************************
 2.150.0      16-Mar-2022       Prajwal S               Created
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semnox.Parafait.logger
{
    /// <summary>
    /// Concrete implementation of MonitorLog use cases.
    /// </summary>
    public class LocalMonitorLogUseCases : LocalUseCases, IMonitorLogUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Execution Context Constructor.
        /// </summary>
        public LocalMonitorLogUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetMonitorLog
        /// </summary>
        public async Task<List<MonitorLogDTO>> GetMonitorLogs(int monitorLogId = -1, int monitorId = -1, bool isActive = false)
        {
            return await Task<List<MonitorLogDTO>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(monitorLogId, monitorId, isActive);
                List<int> monitorLogIdList = new List<int>();
                List<KeyValuePair<MonitorLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MonitorLogDTO.SearchByParameters, string>>();
                //if(monitorLogId > -1)
                //{
                //    searchParameters.Add(new KeyValuePair<MonitorLogDTO.SearchByParameters, string>(MonitorLogDTO.SearchByParameters.MONITOR_LOG_ID, monitorLogId.ToString()));
                //}
                if (monitorId > -1)
                {
                    searchParameters.Add(new KeyValuePair<MonitorLogDTO.SearchByParameters, string>(MonitorLogDTO.SearchByParameters.MONITOR_ID, monitorId.ToString()));
                }
                if (isActive)
                {
                    searchParameters.Add(new KeyValuePair<MonitorLogDTO.SearchByParameters, string>(MonitorLogDTO.SearchByParameters.ISACTIVE, "1"));
                }
                searchParameters.Add(new KeyValuePair<MonitorLogDTO.SearchByParameters, string>(MonitorLogDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                MonitorLogList monitorLogList = new MonitorLogList(executionContext);
                List<MonitorLogDTO> result = monitorLogList.GetMonitorLogDTOList(searchParameters);
                log.LogMethodExit(result);
                return result;
            });
        }

        /// <summary>
        /// Saves the MonitorLogDTO List sent through API.
        /// </summary>
        /// <param name="logMonitorDTO"></param>
        /// <returns></returns>
        public async Task<MonitorLogDTO> SaveMonitorLogs(LogMonitorDTO logMonitorDTO)
        {
            return await Task<MonitorLogDTO>.Factory.StartNew(() =>
            {

                MonitorLogDTO result = null;
                if (logMonitorDTO == null)
                {
                    string errorMessage = "MonitorLogDTO is empty or null";
                    log.LogMethodExit("Throwing Exception - " + errorMessage);
                    throw new ValidationException(errorMessage);
                }

                try
                {
                    MonitorViewDataHandler monitorViewDataHandler = new MonitorViewDataHandler();
                    List<MonitorViewDTO> monitorViewDTOList = monitorViewDataHandler.GetMonitorViewList(executionContext.GetSiteId());

                    if (monitorViewDTOList != null && monitorViewDTOList.Any())
                    {
                        MonitorViewDTO monitorViewDTO = monitorViewDTOList.Where(x => x.ApplicationName == logMonitorDTO.ApplicationName && x.ModuleName == logMonitorDTO.Appmodule &&
                                                                                            (x.AssetName == logMonitorDTO.Assetname || x.AssetHostName == logMonitorDTO.Assetname || x.MacAddress == logMonitorDTO.MacAddress || x.IpAddress == logMonitorDTO.IpAddress)
                                                                                            ).FirstOrDefault();


                        MonitorLogStatusList monitorLogStatusListBL = new MonitorLogStatusList(executionContext);
                        List<KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string>>();
                        searchParameters.Add(new KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string>(MonitorLogStatusDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                        searchParameters.Add(new KeyValuePair<MonitorLogStatusDTO.SearchByParameters, string>(MonitorLogStatusDTO.SearchByParameters.STATUS, logMonitorDTO.Logstatus.ToString()));
                        List<MonitorLogStatusDTO> monitorLogStatusDTOList = monitorLogStatusListBL.GetAllMonitorLogStatusDTO(searchParameters);
                        //monitorInfoContainerDTO = MonitorInfoContainerList.GetMonitorInfoContainerDTOList(executionContext, logMonitorDTO.ApplicationName, logMonitorDTO.Appmodule, logMonitorDTO.Assetname, logMonitorDTO.MacAddress, logMonitorDTO.IpAddress).FirstOrDefault();
                        MonitorLogDTO monitorLogDTO = new MonitorLogDTO(-1, monitorViewDTO.MonitorId, DateTime.Now, monitorLogStatusDTOList.FirstOrDefault().StatusId, logMonitorDTO.LogText, logMonitorDTO.LogKey, logMonitorDTO.LogValue, true);
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            parafaitDBTrx.BeginTransaction();
                            MonitorLog monitorLog = new MonitorLog(executionContext, monitorLogDTO);
                            monitorLog.Save(parafaitDBTrx.SQLTrx);
                            result = monitorLog.GetMonitorLogDTO;
                            parafaitDBTrx.EndTransaction();
                        }
                    }
                    log.LogMethodExit();
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                    result = null;
                }
                log.LogMethodExit(result);
                return result;
            });
        }
    }
}
