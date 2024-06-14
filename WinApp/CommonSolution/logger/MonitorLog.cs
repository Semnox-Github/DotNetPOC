/********************************************************************************************
 * Project Name - Monitor Business Logic
 * Description  - Business logic of the monitor log class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.60        06-May-2019   Jagan Mohana Rao        Created
 *2.90        28-May-2020   Mushahid Faizan         Modified : 3 tier changes for Rest API.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.logger
{
    public class MonitorLog
    {
        private MonitorLogDTO monitorLogDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        private MonitorLog(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="monitorLogDTO">Parameter of the type MonitorLogDTO</param>
        public MonitorLog(ExecutionContext executionContext, MonitorLogDTO monitorLogDTO) : this(executionContext)
        {
            log.LogMethodEntry(monitorLogDTO, executionContext);
            this.monitorLogDTO = monitorLogDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the monitor
        /// asset will be inserted if monitor log Id is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            MonitorLogDataHandler monitorLogDataHandler = new MonitorLogDataHandler(sqlTransaction);
            if (monitorLogDTO.IsChanged == false &&
                monitorLogDTO.Id > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (monitorLogDTO.Id <= 0)
            {
                monitorLogDTO = monitorLogDataHandler.InsertMonitorLog(monitorLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                monitorLogDTO.AcceptChanges();
            }
            else if (monitorLogDTO.IsChanged)
            {
                monitorLogDTO = monitorLogDataHandler.UpdateMonitorLog(monitorLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                monitorLogDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the monitorLogDTO
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// get MonitorLogDTO Object
        /// </summary>
        public MonitorLogDTO GetMonitorLogDTO
        {
            get { return monitorLogDTO; }
        }

        /// <summary>
        /// set MonitorLogDTO Object        
        /// </summary>
        public MonitorLogDTO SetMonitorLogDTO
        {
            set { monitorLogDTO = value; }
        }
    }

    /// <summary>
    /// Manages the list of monitors
    /// </summary>
    public class MonitorLogList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MonitorLogDTO> monitorLogDTOList = new List<MonitorLogDTO>();
        private ExecutionContext executionContext;

        public MonitorLogList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="monitorLogDTOList"></param>
        /// <param name="executionContext"></param>
        public MonitorLogList(ExecutionContext executionContext, List<MonitorLogDTO> monitorLogDTOList) : this(executionContext)
        {
            log.LogMethodEntry(monitorLogDTOList, executionContext);
            this.monitorLogDTOList = monitorLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the monitors list
        /// </summary>
        public List<MonitorLogDTO> GetMonitorLogList(List<KeyValuePair<MonitorLogDTO.SearchByParameters, string>> searchParameters, int currentPage, int pageSize, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            MonitorLogDataHandler monitorLogDataHandler = new MonitorLogDataHandler(sqlTransaction);
            log.LogMethodExit();
            return monitorLogDataHandler.GetMonitorLogList(searchParameters, currentPage, pageSize);
        }

        /// <summary>
        /// Returns the monitors list
        /// </summary>
        public List<MonitorLogDTO> GetMonitorLogDTOList(List<KeyValuePair<MonitorLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            MonitorLogDataHandler monitorLogDataHandler = new MonitorLogDataHandler(sqlTransaction);
            log.LogMethodExit();
            return monitorLogDataHandler.GetMonitorLogDTOList(searchParameters);
        }


        /// <summary>
        /// Saves the monitorLog DTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (monitorLogDTOList == null ||
                monitorLogDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < monitorLogDTOList.Count; i++)
            {
                var monitorLogDTO = monitorLogDTOList[i];
                if (monitorLogDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    MonitorLog monitorLog = new MonitorLog(executionContext, monitorLogDTO);
                    monitorLog.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving monitorLogDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("monitorLogDTO", monitorLogDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}