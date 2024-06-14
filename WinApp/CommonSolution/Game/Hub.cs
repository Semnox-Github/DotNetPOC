/********************************************************************************************
 * Project Name - Hub                                                                          
 * Description  - Hub represents grouping of machines for polling by the server. 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        12-Dec-2015   Kiran          Created
 *2.40        30-Aug-2018   Jagan          For Hub API Modifications and added new method for inserted/updated,Delete record
 *                                         Mathods - Save(List) and DeleteHub 
 *2.41        07-Nov-2018   Rajiv          Commented existing logic which does not required anymore.    
 *2.60        15-Apr-2019   Akshay Gulaganji    updated log.LogMethodEntry() and log.LogMethodExit() 
 *2.60.2      27-May-2019   Jagan Mohana    Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 *2.60.2      31-May-2019   Jagan Mohana    Created new DeleteHubsList()
 *2.70.2        29-Jul-2019   Deeksha         Save method returns DTO instead of id.
 *            25-Sept-2019  Jagan Mohana   Added AuditLog code to Save() for saving the Audits to DB Table DBAuditLog
 *2.70.3      21-Dec-2019   Archana        Modified GetHubSearchList() method to get machine count. Added UpdateHubListToGetMachineCount() method  
 *2.90.0      21-Jun-2020   Girish Kundar   Modofied : Phase -2 changes /REST API report module/Ebyte Changes Build EbyteDTO is added 
*2.100      22-Oct-2020  Girish Kundar  Modified : for POS UI redesign
* *2.110.0      21-Dec-2020   Prajwal S        Modified : added Get using searchparameters along with current page and page size.
*                                                       Added Get Count.
  ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.GenericUtilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    /// <summary>
    /// Hub represents grouping of machines for polling by the server. 
    /// In the previous generation of game readers where the nordic was used, the hub represented
    /// physical box. The hubs were addressed and the game machine readers would communicate only
    /// with the designated hubs.
    /// In the current generation readers, the hubs are logical as the readers communicate via wifi
    /// through the normal wifi APs. 
    /// The hubs grouped the machine together, for the server to create a unit which it would 
    /// circularly poll, that is, if there are 10 game machines, it would be poll from 1 to 10 and then
    /// back to 1. 
    /// </summary>
    public class Hub
    {
        private HubDTO hubDTO;
        private HubDataHandler hubDataHandler;
        private readonly ExecutionContext hubUserContext;
        private Semnox.Parafait.logger.EventLogDataHandler eventLogHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Default constructor of Hub class
        /// Initializes the data handler and execution context  
        /// </summary>
        public Hub()
        {
            log.LogMethodEntry();
            this.hubDataHandler = new HubDataHandler();
            this.hubUserContext = ExecutionContext.GetExecutionContext();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the hub id as the parameter
        /// Would fetch the hub object from the database based on the id passed. 
        /// </summary>
        /// <param name="hubId">Hub id</param>
        public Hub(int hubId)
            : this()
        {
            log.LogMethodEntry(hubId);
            this.hubDTO = hubDataHandler.GetHub(hubId);
            log.LogMethodExit(hubDTO);
        }

        /// <summary>
        /// Creates hub object using the HubDTO object
        /// </summary>
        /// <param name="hub">HubDTO</param>
        public Hub(HubDTO hub)
            : this()
        {
            log.LogMethodEntry(hub);
            this.hubDTO = hub;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor.
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="hubDTO">hubDTO</param>
        public Hub(ExecutionContext executionContext, HubDTO hubDTO)
        : this()
        {
            log.LogMethodEntry(executionContext, hubDTO);
            this.hubUserContext = executionContext;
            this.hubDTO = hubDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the hub details
        /// Checks if the hub id is -1, if it is, 
        ///     then the record does not exist in the database and the hub record is created
        /// If the hub id is not -1, then updates the hub record
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                string userId = hubUserContext.GetUserId();
                int siteId = hubUserContext.GetSiteId();
                Validate(sqlTransaction);
                HubDataHandler hubDataHandler = new HubDataHandler(sqlTransaction);
                if (hubDTO.MasterId < 0)
                {
                    hubDTO = hubDataHandler.InsertHub(hubDTO, userId, siteId);
                    hubDTO.AcceptChanges();
                }
                else
                {
                    if (hubDTO.IsChanged)
                    {
                        hubDTO = hubDataHandler.UpdateHub(hubDTO, userId, siteId);
                        if (hubDTO.RestartAP)
                        {
                            eventLogHandler = new Semnox.Parafait.logger.EventLogDataHandler();
                            eventLogHandler.InsertEventLog("ParafaitPOS", "D", hubUserContext.GetUserId(), hubUserContext.POSMachineName, "AP ID: " + hubDTO.MasterId, "AP Restarted from POS", "PARAFAITSERVER", 0,
                                hubUserContext.GetUserId(), "", false, hubUserContext.GetUserPKId().ToString(), hubUserContext.GetSiteId());
                        }
                        hubDTO.AcceptChanges();
                    }
                }
                if (!string.IsNullOrEmpty(hubDTO.Guid))
                {
                    AuditLog auditLog = new AuditLog(hubUserContext);
                    auditLog.AuditTable("masters", hubDTO.Guid, sqlTransaction);
                }
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }
        public void  Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (hubDTO.IsRadian)
            {
                if (string.IsNullOrWhiteSpace(hubDTO.Frequency) || string.IsNullOrWhiteSpace(hubDTO.IPAddress))
                {
                    log.Debug("Frequency and IPAddress cannot be empty");
                    throw new ValidationException(MessageContainerList.GetMessage(hubUserContext, 2936));
                }
                if (hubDTO.IsEByte)
                {
                    log.Debug("IsEByte and IsDirect Mode cannot be set");
                    throw new ValidationException(MessageContainerList.GetMessage(hubUserContext, 2937));
                }
                if (hubDTO.DirectMode == "Y")
                {
                    log.Debug("IsEByte and IsDirect Mode cannot be set");
                    throw new ValidationException(MessageContainerList.GetMessage(hubUserContext,2938));
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Restarts the hub
        /// Whenever game machines are added, the hub needs a restart
        /// The hub is flagged to be restarted. The server looks at this parameters
        /// and restarts the hub (and changes parameter back to false).
        /// </summary>
        public void ReStartAP()
        {
            log.LogMethodEntry();
            hubDTO.RestartAP = true;
            Save();
            string userId = hubUserContext.GetUserId();
            int siteId = hubUserContext.GetSiteId();
            eventLogHandler = new Semnox.Parafait.logger.EventLogDataHandler();
            eventLogHandler.InsertEventLog("Tablet", "I", userId, hubDTO.MasterName, "1", hubDTO.MasterName + " set to restart from Tablet", "RestartAP", 1, "", "", false, userId, siteId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the Hub based on masterId
        /// </summary>
        /// <param name="masterId"></param>        
        internal void DeleteHub(SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(sqlTransaction);

            try
            {
                HubDataHandler hubDataHandler = new HubDataHandler(sqlTransaction);
                hubDataHandler.DeleteHub(hubDTO.MasterId);
                log.LogMethodExit();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// get UserDto Object
        /// </summary>
        public HubDTO GetHubDTO
        {
            get { return hubDTO; }
        }
    }

    /// <summary>
    /// Manages the list of hubs
    /// </summary>
    public class HubList
    {
        private List<HubDTO> hubDTOList;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        public HubList() //added
        {
            log.LogMethodEntry();
            this.hubDTOList = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executioncontext">executioncontext</param>
        public HubList(ExecutionContext executioncontext)
        {
            log.LogMethodEntry(executioncontext);
            this.executionContext = executioncontext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized Constructor with hubDtoList and ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
		/// <param name="hubDTOList">hubDTOList</param>
        public HubList(ExecutionContext executionContext, List<HubDTO> hubDTOList)
        {
            log.LogMethodEntry(executionContext, hubDTOList);
            this.executionContext = executionContext;
            this.hubDTOList = hubDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        ///  Gets the Hub List matching the search key
        /// </summary>
        /// <param name="searchParameters">List of key valye pair containing the search parameter and search values</param>        
        /// <param name="sqlTransaction">sqlTransaction</param>
        public HubList(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters, SqlTransaction sqlTransaction = null, bool loadMachineCount = false)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            HubDataHandler hubDataHandler = new HubDataHandler(sqlTransaction);
            this.hubDTOList = hubDataHandler.GetHubList(searchParameters);
            if (loadMachineCount)
            {
                UpdateHubListToGetMachineCount();
            }
            log.LogMethodExit();
        }

        /// <summary>
        ///  Gets the Hub List matching the search key
        /// </summary>
        /// <param name="searchParameters">List of key valye pair containing the search parameter and search values</param>        
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<HubDTO> GetHubSearchList(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters,
                                              SqlTransaction sqlTransaction = null, bool loadMachineCount = false,
                                              bool loadChild = false, int currentPage = 0, int pageSize= 0)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            HubDataHandler hubDataHandler = new HubDataHandler(sqlTransaction);
            hubDTOList = hubDataHandler.GetHubList(searchParameters, currentPage, pageSize);
            if (loadMachineCount)
            {
                UpdateHubListToGetMachineCount();
            }
            if (loadChild && hubDTOList != null && hubDTOList.Any())
            {
                foreach (HubDTO hubDTO in hubDTOList)
                {
                    if (hubDTO.IsEByte)
                    {
                        BuildEbyteDTO(hubDTO);
                        log.LogMethodExit(hubDTO.EBYTEDTO);
                    }
                }
            }
            log.LogMethodExit(hubDTOList);
            return hubDTOList;
        }

        private void BuildEbyteDTO(HubDTO hubDTO)
        {
            try
            {
                ConfigEBYTEHubBL configEBYTEHub = new ConfigEBYTEHubBL(executionContext, hubDTO);
                List<string> ebyteConfig = configEBYTEHub.ReadAllConfigs();
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                log.LogVariableState("ebyteConfig", ebyteConfig);
                ebyteConfig.RemoveAt(0);
                char[] seperator = { ':' };
                if (hubDTO.EBYTEDTO == null)
                {
                    hubDTO.EBYTEDTO = new EBYTEDTO();
                    foreach (string config in ebyteConfig)
                    {
                        log.Debug("Config   : " + config);
                        string[] values = config.Split(seperator);
                        log.Debug("values[0] " + values[0]);
                        log.Debug("values[1] " + values[1]);
                        keyValuePairs.Add(values[0], values[1]);
                        log.Debug("keyValuePairs " + keyValuePairs.Keys);

                    }

                    if (keyValuePairs.ContainsKey("UART_PARITY"))
                    {
                        hubDTO.EBYTEDTO.UARTParity = Convert.ToInt32(keyValuePairs["UART_PARITY"]);
                    }
                    if (keyValuePairs.ContainsKey("DATA_RATE"))
                    {
                        hubDTO.EBYTEDTO.DataRate = Convert.ToInt32(keyValuePairs["DATA_RATE"]);
                    }
                    if (keyValuePairs.ContainsKey("FIXED_TRANSMISSION"))
                    {
                        hubDTO.EBYTEDTO.TransmissionMode = Convert.ToInt32(keyValuePairs["FIXED_TRANSMISSION"]);
                    }
                    if (keyValuePairs.ContainsKey("IO_DRIVE_MODE"))
                    {
                        hubDTO.EBYTEDTO.IODriveMode = Convert.ToInt32(keyValuePairs["IO_DRIVE_MODE"]);
                    }
                    if (keyValuePairs.ContainsKey("WAKEUP_TIME"))
                    {
                        hubDTO.EBYTEDTO.WakeupTime = Convert.ToInt32(keyValuePairs["WAKEUP_TIME"]);
                    }
                    if (keyValuePairs.ContainsKey("FEC_SWITCH"))
                    {
                        hubDTO.EBYTEDTO.FecSwitch = Convert.ToInt32(keyValuePairs["FEC_SWITCH"]);
                    }
                    if (keyValuePairs.ContainsKey("OUTPUT_POWER"))
                    {
                        hubDTO.EBYTEDTO.OutputPower = Convert.ToInt32(keyValuePairs["OUTPUT_POWER"]);
                    }
                    log.LogMethodExit(hubDTO);
                    log.LogMethodExit(hubDTO.EBYTEDTO);

                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.Error("Error while loading the Ebyte config value from the hub");
            }
        }

        private void UpdateHubListToGetMachineCount()
        {
            log.LogMethodEntry();
            if (hubDTOList != null && hubDTOList.Any())
            {
                List<MachineDTO> machineListDTO = new List<MachineDTO>();
                List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));
                searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                MachineList machineList = new MachineList(executionContext);
                machineListDTO = machineList.GetMachineList(searchParameters);
                if (machineListDTO != null && machineListDTO.Count > 0)
                {
                    foreach (HubDTO hub in hubDTOList)
                    {
                        hub.MachineCount = machineListDTO.Count(x => x.MasterId == hub.MasterId);
                    }
                }
                log.LogMethodExit();
            }
        }

        public List<HubDTO> GetMachines(int masterId, DateTime fromDate, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(masterId, sqlTransaction);
            HubDataHandler hubDataHandler = new HubDataHandler(sqlTransaction);
            hubDTOList = hubDataHandler.GetMasterReport(masterId, fromDate);
            log.LogMethodExit(hubDTOList);
            return hubDTOList;
        }

        /// <summary>
        /// Gets the hub list
        /// </summary>
        public List<HubDTO> GetHubList { get { return hubDTOList; } }

        /// <summary>
        /// Save or update the hubs
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                foreach (HubDTO hubdto in hubDTOList)
                {
                    Hub hubs = new Hub(executionContext, hubdto);
                    hubs.Save(sqlTransaction);
                }
                log.LogMethodExit();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 654));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 545));
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Delete the Hubs List 
        /// </summary>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            try
            {
                log.LogMethodEntry();
                if (hubDTOList != null && hubDTOList.Any())
                {
                    foreach (HubDTO hubdto in hubDTOList)
                    {
                        Hub hub = new Hub(executionContext, hubdto);
                        hub.DeleteHub(sqlTransaction);
                    }
                }
                log.LogMethodExit();
            }
            catch (SqlException ex)
            {
                log.Error(ex);
                if (ex.Number == 2601)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                }
                else if (ex.Number == 547)
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                }
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception -" + ex.Message);
                throw;
            }
        }

        public DateTime? GetHubModuleLastUpdateTime(int siteId)
        {
            log.LogMethodEntry(siteId);
            HubDataHandler hubDataHandler = new HubDataHandler();
            DateTime? result = hubDataHandler.GetHubModuleLastUpdateTime(siteId);
            log.LogMethodExit(result);
            return result;
        }
        /// <summary>
        /// Returns the no of Hub matching the search Parameters
        /// </summary>
        /// <param name="searchParameters"> search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetHubCount(List<KeyValuePair<HubDTO.SearchByHubParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)//added
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            HubDataHandler hubDataHandler = new HubDataHandler(sqlTransaction);
            int hubsCount = hubDataHandler.GetHubsCount(searchParameters);
            log.LogMethodExit(hubsCount);
            return hubsCount;
        }
    }
}
