/********************************************************************************************
 * Project Name - Maintenance
 * Description  - Business Logic of the MaintenanceJobStatus class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.100.0        23-Sept-2020   Mushahid Faizan         Created.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;


namespace Semnox.Parafait.Maintenance
{
    public class MaintenanceJobStatusBL
    {
        private MaintenanceJobStatusDTO maintenanceJobStatusDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private MaintenanceJobStatusBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        ///<summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="maintenanceJobStatusDTO"></param>
        public MaintenanceJobStatusBL(ExecutionContext executionContext, MaintenanceJobStatusDTO maintenanceJobStatusDTO) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceJobStatusDTO);
            this.executionContext = executionContext;
            this.maintenanceJobStatusDTO = maintenanceJobStatusDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the maintenanceJobStatusId as the parameter
        /// Would fetch the maintenanceJobStatusDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="sqlTransaction">SqlTransaction</param>
        /// <param name="maintenanceJobStatusId">Id</param>
        public MaintenanceJobStatusBL(ExecutionContext executionContext, int maintenanceJobStatusId, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceJobStatusId, sqlTransaction);
            MaintenanceJobStatusDataHandler maintenanceJobStatusDataHandler = new MaintenanceJobStatusDataHandler(sqlTransaction);
            maintenanceJobStatusDTO = maintenanceJobStatusDataHandler.GetMaintenanceJobStatusDTO(maintenanceJobStatusId);
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the maintenanceJobStatusDTO
        /// asset will be inserted if maintenanceJobStatusId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MaintenanceJobStatusDataHandler maintenanceJobStatusDataHandler = new MaintenanceJobStatusDataHandler(sqlTransaction);
            if (maintenanceJobStatusDTO.IsChanged == false &&
                maintenanceJobStatusDTO.JobStatusId > -1)
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
            if (maintenanceJobStatusDTO.JobStatusId < 0)
            {
                maintenanceJobStatusDTO = maintenanceJobStatusDataHandler.InsertMaintenanceJobStatus(maintenanceJobStatusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                maintenanceJobStatusDTO.AcceptChanges();
            }
            else if (maintenanceJobStatusDTO.IsChanged)
            {
                maintenanceJobStatusDTO = maintenanceJobStatusDataHandler.UpdateMaintenanceJobStatus(maintenanceJobStatusDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                maintenanceJobStatusDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the maintenanceJobStatusDTO
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
        /// get MaintenanceJobStatusDTO Object
        /// </summary>
        public MaintenanceJobStatusDTO GetMaintenanceJobStatusDTO
        {
            get { return maintenanceJobStatusDTO; }
        }

        /// <summary>
        /// set MaintenanceJobStatusDTO Object        
        /// </summary>
        public MaintenanceJobStatusDTO SetMaintenanceJobStatusDTO
        {
            set { maintenanceJobStatusDTO = value; }
        }
    }

    /// <summary>
    /// Manages the list of MaintenanceJobStatusBL
    /// </summary>
    public class MaintenanceJobStatusListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<MaintenanceJobStatusDTO> maintenanceJobStatusDTOList = new List<MaintenanceJobStatusDTO>();
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MaintenanceJobStatusListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="maintenanceJobStatusDTO"></param>
        public MaintenanceJobStatusListBL(ExecutionContext executionContext, List<MaintenanceJobStatusDTO> maintenanceJobStatusDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, maintenanceJobStatusDTOList);
            this.maintenanceJobStatusDTOList = maintenanceJobStatusDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the MaintenanceJobStatusDTO  list
        /// </summary>
        public List<MaintenanceJobStatusDTO> GetAllMaintenanceJobStatusDTOList(List<KeyValuePair<MaintenanceJobStatusDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            MaintenanceJobStatusDataHandler maintenanceJobStatusDataHandler = new MaintenanceJobStatusDataHandler(sqlTransaction);
            maintenanceJobStatusDTOList = maintenanceJobStatusDataHandler.GetMaintenanceJobStatusDTOList(searchParameters);
            log.LogMethodExit();
            return maintenanceJobStatusDTOList;
        }

        /// <summary>
        /// Gets the MaintenanceJobStatusDTO List for maintenanceJobStatusIdList
        /// </summary>
        /// <param name="maintChklstdetIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of MaintenanceJobStatusDTO</returns>
        public List<MaintenanceJobStatusDTO> GetMaintenanceJobStatusDTOList(List<int> maintChklstdetIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(maintChklstdetIdList, activeRecords);
            MaintenanceJobStatusDataHandler maintenanceJobStatusDataHandler = new MaintenanceJobStatusDataHandler(sqlTransaction);
            maintenanceJobStatusDTOList = maintenanceJobStatusDataHandler.GetMaintenanceJobStatusDTOList(maintChklstdetIdList, activeRecords);
            log.LogMethodExit(maintenanceJobStatusDTOList);
            return maintenanceJobStatusDTOList;
        }

        /// <summary>
        /// Saves the MaintenanceJobStatusDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (maintenanceJobStatusDTOList == null ||
                maintenanceJobStatusDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < maintenanceJobStatusDTOList.Count; i++)
            {
                var maintenanceJobStatusDTO = maintenanceJobStatusDTOList[i];
                if (maintenanceJobStatusDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    MaintenanceJobStatusBL maintenanceJobStatusBL = new MaintenanceJobStatusBL(executionContext, maintenanceJobStatusDTO);
                    maintenanceJobStatusBL.Save(sqlTransaction);
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
                    log.Error("Error occurred while saving maintenanceJobStatusDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("maintenanceJobStatusDTO", maintenanceJobStatusDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }

    }
}
