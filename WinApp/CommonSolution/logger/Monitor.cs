/********************************************************************************************
 * Project Name - Monitor Business Logic
 * Description  - Business logic of the monitor class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Jagan Mohana Rao        Created Save() method and new class MonitorList
 *2.60.2      17-June-2019  Jagan Mohana            Created the DeleteMonitor and Delete methods
 *2.70.2.0      15-Sep-2019   Archana                 Added Credit card payment for AppModule enum
 *2.90        28-May-2020   Mushahid Faizan         Modified : As per 3 Tier Standards.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Data;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System.Linq;
using System.Text;
using Semnox.Parafait.Communication;

namespace Semnox.Parafait.logger
{
    public class Monitor
    {
        private bool _doMonitor = false;
        private int _MonitorId = -1;
        private int _MonitorInterval;
        private DateTime LastLogTime = DateTime.Now.AddYears(-1);
        Utilities _Utils;
        private MonitorDTO monitorDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        public enum MonitorLogStatus
        {
            /// <summary>
            /// INFO = The 1st level of logging - Informational messages
            /// </summary>  
            INFO,
            /// <summary>
            /// WARN = The 2nd level of logging - Warning messages
            /// </summary>  
            WARNING,
            /// <summary>
            /// ERROR = The 3rd level of logging - Error messages
            /// </summary>  
            ERROR
        }

        public enum MonitorAssetType
        {
            SERVER,
            POS,
            MANAGER_TERMINAL,
            KIOSK,
            KDS,
            SIGNAGE,
            TABLET,
            ACCESS_POINT,
            CONTROLLER,
            COMPUTER,
            OTHER
        }

        public enum MonitorApplication
        {
            PRIMARY_SERVER,
            EXSYS_SERVER,
            ALOHA_SYNCH_SERVER,
            DATA_UPLOAD_SERVER,
            REPORTS_SERVER,
            POS,
            MANAGEMENT_STUDIO,
            KIOSK,
            KDS,
            SIGNAGE,
            TABLET_POS,
            OTHER
        }

        public enum MonitorAppModule
        {
            GENERIC,
            CARD_DISPENSER,
            BILL_ACCEPTOR,
            COIN_ACCEPTOR,
            REMOTING_SERVER,
            CREDIT_CARD_PAYMENT
        }

        public enum MonitorType
        {
            APPLICATION,
            INFRASTRUCTURE
        }

        public Monitor() :
            this(MonitorAppModule.GENERIC)
        {
        }

        public Monitor(MonitorAppModule AppModule)
        {
            _Utils = new Utilities();
            Setup(AppModule.ToString());
        }

        public Monitor(string AppModule)
        {
            _Utils = new Utilities();
            Setup(AppModule);
        }

        public Monitor(MonitorAppModule AppModule, string assemblyName, SqlConnection connection)//Modification starts on 01-Apr-2016 for adding assembly name
        {
            if (connection == null)
            {
                _Utils = new Utilities();
            }
            else
            {
                _Utils = new Utilities(connection.ConnectionString);
            }
            Setup(AppModule.ToString(), assemblyName);
        }//Modification ends on 01-Apr-2016 for adding assembly name

        void Setup(string AppModule, string applicationName = "")//Modification on 01-Apr-2016 for adding assembly name
        {
            IPAddress ipAddress = IPAddress.None;
            try
            {
                System.Net.IPAddress[] TempAd = System.Net.Dns.GetHostEntry(Dns.GetHostName()).AddressList;
                foreach (IPAddress ip in TempAd)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = ip;
                        break;
                    }
                }
            }
            catch { }

            string macAddress = NetUtils.GetMacAddress(ipAddress);

            string assemblyName = (!string.IsNullOrEmpty(applicationName)) ? applicationName : System.Reflection.Assembly.GetEntryAssembly().GetName().Name;//Modification on 01-Apr-2016 for adding assembly name

            DataTable dtMonitor = _Utils.executeDataTable(@"select m.MonitorId, m.Interval
                                                            from MonitorAsset a, MonitorType ty,
                                                                MonitorApplication ma, MonitorAppModule map,
                                                                Monitor m
                                                            where m.AssetId = a.AssetId
                                                            and m.MonitorTypeId = ty.MonitorTypeId
                                                            and ty.MonitorType = @monitorType
                                                            and ma.ApplicationId = m.ApplicationId
                                                            and ma.AppExeName = @exeName
                                                            and map.ModuleId = isnull(m.AppModuleId, map.ModuleId)
                                                            and map.ModuleName = @module
                                                            and m.Active = 1
                                                            and (a.IPAddress = @IP or MacAddress = @Mac or Hostname = @hostName)",
                                                                    new SqlParameter("@IP", ipAddress.ToString()),
                                                                    new SqlParameter("@Mac", macAddress),
                                                                    new SqlParameter("@hostName", Environment.MachineName),
                                                                    new SqlParameter("@monitorType", MonitorType.APPLICATION.ToString()),
                                                                    new SqlParameter("@exeName", assemblyName),
                                                                    new SqlParameter("@module", AppModule));
            if (dtMonitor.Rows.Count != 0)
            {
                _doMonitor = true;
                _MonitorId = Convert.ToInt32(dtMonitor.Rows[0]["MonitorId"]);
                int interval = 5;
                if (dtMonitor.Rows[0]["Interval"] != DBNull.Value)
                    interval = Convert.ToInt32(dtMonitor.Rows[0]["Interval"]);

                _MonitorInterval = interval * 60;
            }
        }

        public void Post(MonitorLogStatus logStatus, string logText)
        {
            doPost(logStatus, logText, "", "", false);
        }

        public void Post(MonitorLogStatus logStatus, string logText, string logKey, string logValue)
        {
            doPost(logStatus, logText, logKey, logValue, false);
        }

        public void PostImmediate(MonitorLogStatus logStatus, string logText)
        {
            doPost(logStatus, logText, "", "", true);
        }

        private void doPost(MonitorLogStatus logStatus, string logText, string logKey, string logValue, bool Force = false)
        {
            if (!_doMonitor)
                return;

            if (!Force && LastLogTime.AddSeconds(_MonitorInterval) > DateTime.Now)
                return;

            if (!Force)
                LastLogTime = DateTime.Now;

            _Utils.executeNonQuery(@"insert into MonitorLog 
                                    (MonitorId, Timestamp, StatusId, LogText, LogKey, LogValue)
                                    values 
                                    (@monitorId, getdate(), (select top 1 StatusId from MonitorLogStatus where Status = @status), @logText, @logKey, @logValue)",
                                    new SqlParameter("@monitorId", _MonitorId),
                                    new SqlParameter("@status", logStatus.ToString()),
                                    new SqlParameter("@logText", logText),
                                    new SqlParameter("@logKey", string.IsNullOrEmpty(logKey) ? DBNull.Value : (object)logKey),
                                    new SqlParameter("@logValue", string.IsNullOrEmpty(logValue) ? DBNull.Value : (object)logValue));
        }
        /// <summary>
        /// Parametrized consturctor
        /// </summary>
        /// <param name="executionContext"></param>
        private Monitor(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parametrized consturctor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="monitorDTO"></param>
        public Monitor(ExecutionContext executionContext, MonitorDTO monitorDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, monitorDTO);
            this.executionContext = executionContext;
            this.monitorDTO = monitorDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the monitorId as the parameter
        /// Would fetch the monitorDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="id">Id</param>
        public Monitor(ExecutionContext executionContext, int monitorId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, monitorId, sqlTransaction);
            MonitorDataHandler monitorDataHandler = new MonitorDataHandler(sqlTransaction);
            monitorDTO = monitorDataHandler.GetMonitor(monitorId);
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the monitor
        /// asset will be inserted if monitorId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            MonitorDataHandler monitorDataHandler = new MonitorDataHandler(sqlTransaction);
            if (monitorDTO.IsChangedRecursive == false)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (monitorDTO.MonitorId <= 0)
            {
                monitorDTO = monitorDataHandler.InsertMonitor(monitorDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                monitorDTO.AcceptChanges();
            }
            else if (monitorDTO.IsChanged)
            {
                monitorDTO = monitorDataHandler.UpdateMonitor(monitorDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                monitorDTO.AcceptChanges();
            }
            SaveMonitorChild(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveMonitorChild(SqlTransaction sqlTransaction)
        {
            // for Child Records : :MonitorLogDTO
            if (monitorDTO.MonitorLogDTOList != null && monitorDTO.MonitorLogDTOList.Any())
            {
                List<MonitorLogDTO> updatedMonitorLogDTOList = new List<MonitorLogDTO>();
                foreach (MonitorLogDTO monitorLogDTO in monitorDTO.MonitorLogDTOList)
                {
                    if (monitorLogDTO.MonitorId != monitorDTO.MonitorId)
                    {
                        monitorLogDTO.MonitorId = monitorDTO.MonitorId;
                    }
                    if (monitorLogDTO.IsChanged)
                    {
                        updatedMonitorLogDTOList.Add(monitorLogDTO);
                    }
                }
                if (updatedMonitorLogDTOList.Any())
                {
                    MonitorLogList monitorLogList = new MonitorLogList(executionContext, updatedMonitorLogDTOList);
                    monitorLogList.Save(sqlTransaction);
                }
            }
        }

        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (monitorDTO == null)
            {
                //Validation to be implemented.
            }

            if (monitorDTO.MonitorLogDTOList != null && monitorDTO.MonitorLogDTOList.Any())
            {
                foreach (var monitorLogDTO in monitorDTO.MonitorLogDTOList)
                {
                    if (monitorLogDTO.IsChanged)
                    {
                        MonitorLog monitorLog = new MonitorLog(executionContext, monitorLogDTO);
                        validationErrorList.AddRange(monitorLog.Validate(sqlTransaction)); //calls child validation method.
                    }
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }


        /// <summary>
        /// Delete the Monitor records from database
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete(MonitorDTO monitorDTO, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(monitorDTO, sqlTransaction);
            try
            {
                MonitorDataHandler monitorDataHandler = new MonitorDataHandler();
                if ((monitorDTO.MonitorLogDTOList != null && monitorDTO.MonitorLogDTOList.Any((x => x.IsActive == true))))
                {
                    string message = MessageContainerList.GetMessage(executionContext, 1143);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new ForeignKeyException(message);
                }
                log.LogVariableState("monitorDTO", monitorDTO);
                SaveMonitorChild(sqlTransaction);
                if (monitorDTO.MonitorId >= 0)
                {
                    monitorDataHandler.DeleteMonitor(monitorDTO.MonitorId);
                }
                monitorDTO.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// Manages the list of monitors
    /// </summary>
    public class MonitorList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MonitorDTO> monitorDTOList = new List<MonitorDTO>();
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MonitorList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.monitorDTOList = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="monitorDTOList"></param>
        /// <param name="executionContext"></param>
        public MonitorList(List<MonitorDTO> monitorDTOList, ExecutionContext executionContext)
        {
            log.LogMethodEntry(monitorDTOList, executionContext);
            this.monitorDTOList = monitorDTOList;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the no of messagesCount matching the searchParameters
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns></returns>
        public int GetMonitorDTOCount(List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            MonitorDataHandler monitorDataHandler = new MonitorDataHandler(sqlTransaction);
            int monitorDTOCount = monitorDataHandler.GetMonitorDTOCount(searchParameters);
            return monitorDTOCount;
        }

        /// <summary>
        /// Returns the monitors list
        /// </summary>
        public List<MonitorDTO> GetAllMonitorDTOList(List<KeyValuePair<MonitorDTO.SearchByParameters, string>> searchParameters, int currentPage, int pageSize, bool loadChildRecords = false, bool loadActiveRecords = false,
                                           SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            MonitorDataHandler monitorDataHandler = new MonitorDataHandler(sqlTransaction);
            monitorDTOList = monitorDataHandler.GetMonitorList(searchParameters, currentPage, pageSize);
            if (monitorDTOList != null && monitorDTOList.Any() && loadChildRecords)
            {
                Build(monitorDTOList, currentPage, pageSize, sqlTransaction);
            }
            log.LogMethodExit();
            return monitorDTOList;
        }

        private void Build(List<MonitorDTO> monitorDTOList, int currentPage, int pageSize, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(monitorDTOList, sqlTransaction);
            foreach (MonitorDTO monitorDTO in monitorDTOList)
            {
                MonitorLogList monitorLogList = new MonitorLogList(executionContext);
                List<KeyValuePair<MonitorLogDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<MonitorLogDTO.SearchByParameters, string>>();
                searchByParams.Add(new KeyValuePair<MonitorLogDTO.SearchByParameters, string>(MonitorLogDTO.SearchByParameters.MONITOR_ID, monitorDTO.MonitorId.ToString()));
                monitorDTO.MonitorLogDTOList = monitorLogList.GetMonitorLogList(searchByParams, currentPage, pageSize, sqlTransaction);

            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the  list of adsDTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (monitorDTOList == null ||
                monitorDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < monitorDTOList.Count; i++)
            {
                var monitorDTO = monitorDTOList[i];
                if (monitorDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    Monitor monitor = new Monitor(executionContext, monitorDTO);
                    monitor.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving monitorDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("monitorDTO", monitorDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the monitorDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (monitorDTOList != null && monitorDTOList.Any())
            {
                foreach (MonitorDTO monitorDTO in monitorDTOList)
                {
                    if (monitorDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                Monitor monitor = new Monitor(executionContext, monitorDTO);
                                monitor.Delete(monitorDTO, parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                                }
                                else
                                {
                                    throw;
                                }
                            }
                            catch (ValidationException valEx)
                            {
                                log.Error(valEx);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                                throw;
                            }
                            catch (Exception ex)
                            {
                                log.Error(ex.Message);
                                parafaitDBTrx.RollBack();
                                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                                throw;
                            }
                        }
                    }
                }
            }
            log.LogMethodExit();
        }
    }
}
