/********************************************************************************************
 * Project Name - Monitor Business Logic
 * Description  - Business logic of the monitor class
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
using Semnox.Parafait.logger;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.logger
{
    public class MonitorPriority
    {
        private MonitorPriorityDTO monitorPriorityDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private MonitorPriority(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="monitorPriorityDTO"></param>
        public MonitorPriority(ExecutionContext executionContext, MonitorPriorityDTO monitorPriorityDTO) 
          : this(executionContext)
        {
            log.LogMethodEntry(executionContext, monitorPriorityDTO);
            this.monitorPriorityDTO = monitorPriorityDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the priorityId as the parameter
        /// Would fetch the monitorPriorityDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="priorityId">Id</param>
        public MonitorPriority(ExecutionContext executionContext, int priorityId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, priorityId, sqlTransaction);
            MonitorPriorityDataHandler monitorPriorityDataHandler = new MonitorPriorityDataHandler(sqlTransaction);
            monitorPriorityDTO = monitorPriorityDataHandler.GetMonitorPriority(priorityId);
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the monitor master data
        /// asset will be inserted if monitorPriorityId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MonitorPriorityDataHandler monitorPriorityDataHandler = new MonitorPriorityDataHandler();
            if (monitorPriorityDTO.IsChanged == false &&
                monitorPriorityDTO.PriorityId > -1)
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
            if (monitorPriorityDTO.IsActive)
            {
                if (monitorPriorityDTO.PriorityId <= 0)
                {
                    monitorPriorityDTO = monitorPriorityDataHandler.InsertMonitorPriority(monitorPriorityDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    monitorPriorityDTO.AcceptChanges();
                }
                else if (monitorPriorityDTO.IsChanged)
                {
                    monitorPriorityDTO = monitorPriorityDataHandler.UpdateMonitorPriority(monitorPriorityDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    monitorPriorityDTO.AcceptChanges();
                }
            }
            else
            {
                Delete(sqlTransaction);
                monitorPriorityDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Delete the MonitorPrority passing the monitorPriorityId
        /// </summary>
        /// <param name="priorityId"></param>
        public void Delete(SqlTransaction sqlTransaction)
        {
            try
            {
                log.LogMethodEntry(sqlTransaction);
                MonitorPriorityDataHandler monitorPriorityDataHandler = new MonitorPriorityDataHandler();
                if (monitorPriorityDTO.PriorityId != 0)
                {
                    monitorPriorityDataHandler.DeleteMonitorPriority(monitorPriorityDTO.PriorityId);
                    log.LogMethodExit();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Validate the monitorPriorityDTO
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
        public MonitorPriorityDTO GetMonitorPriorityDTO
        {
            get { return monitorPriorityDTO; }
        }

        /// <summary>
        /// set MonitorLogDTO Object        
        /// </summary>
        public MonitorPriorityDTO SetMonitorPriorityDTO
        {
            set { monitorPriorityDTO = value; }
        }
    }
}
/// <summary>
/// Manages the list of monitors
/// </summary>
public class MonitorPriorityList
{
    private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private List<MonitorPriorityDTO> monitorPriorityDTOList = new List<MonitorPriorityDTO>();
    private ExecutionContext executionContext;
    /// <summary>
    /// Parameterized constructor
    /// </summary>
    /// <param name="executionContext"></param>
    public MonitorPriorityList(ExecutionContext executionContext)
    {
        log.LogMethodEntry(executionContext);
        this.executionContext = executionContext;
        log.LogMethodExit();
    }
    /// <summary>
    /// Parameterized constructor
    /// </summary>
    /// <param name="executionContext"></param>
    /// <param name="monitorPriorityDTOList"></param>
    public MonitorPriorityList(ExecutionContext executionContext, List<MonitorPriorityDTO> monitorPriorityDTOList)
    {
        log.LogMethodEntry(executionContext, monitorPriorityDTOList);
        this.monitorPriorityDTOList = monitorPriorityDTOList;
        this.executionContext = executionContext;
        log.LogMethodExit();
    }

    /// <summary>
    /// Returns the monitor asset list
    /// </summary>
    public List<MonitorPriorityDTO> GetAllMonitorPriorityList(List<KeyValuePair<MonitorPriorityDTO.SearchByParameters, string>> searchParameters)
    {
        log.LogMethodEntry(searchParameters);
        MonitorPriorityDataHandler monitorPriorityDataHandler = new MonitorPriorityDataHandler();
        log.LogMethodExit();
        return monitorPriorityDataHandler.GetMonitorPriorityList(searchParameters);
    }

    /// <summary>
    /// Saves the monitorLog DTO List
    /// Checks if the  id is not less than or equal to 0
    /// If it is less than or equal to 0, then inserts
    /// else updates
    /// </summary>
    /// <param name="sqlTransaction">SqlTransaction</param>
    public void Save(SqlTransaction sqlTransaction = null)
    {
        log.LogMethodEntry(sqlTransaction);
        if (monitorPriorityDTOList == null ||
            monitorPriorityDTOList.Any() == false)
        {
            log.LogMethodExit(null, "List is empty");
            return;
        }

        for (int i = 0; i < monitorPriorityDTOList.Count; i++)
        {
            var monitorPriorityDTO = monitorPriorityDTOList[i];
            if (monitorPriorityDTO.IsChanged == false)
            {
                continue;
            }
            try
            {
                MonitorPriority monitorPriority = new MonitorPriority(executionContext, monitorPriorityDTO);
                monitorPriority.Save(sqlTransaction);
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
                log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while saving monitorPriorityDTO.", ex);
                log.LogVariableState("Record Index ", i);
                log.LogVariableState("monitorPriorityDTO", monitorPriorityDTO);
                throw;
            }
        }
        log.LogMethodExit();
    }

    /// <summary>
    /// Delete the monitorPriorityDTOList  
    /// This method is only used for Web Management Studio.
    /// </summary>
    public void Delete()
    {
        log.LogMethodEntry();
        if (monitorPriorityDTOList != null && monitorPriorityDTOList.Any())
        {
            foreach (MonitorPriorityDTO monitorPriorityDTO in monitorPriorityDTOList)
            {
                if (monitorPriorityDTO.IsChanged)
                {
                    using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                    {
                        try
                        {
                            parafaitDBTrx.BeginTransaction();
                            MonitorPriority monitorPriority = new MonitorPriority(executionContext, monitorPriorityDTO);
                            monitorPriority.Delete(parafaitDBTrx.SQLTrx);
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

