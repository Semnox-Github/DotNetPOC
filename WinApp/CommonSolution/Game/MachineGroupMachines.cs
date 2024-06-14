/********************************************************************************************
 * Project Name - Machine Groups
 * Description  - Bussiness logic of machine groups machines
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60        13-May-2019   Jagan Mohana Rao    Created
 *2.70.2        31-Jul-2019   Deeksha             modified:Save method returns DTO.
 *2.80        03-Jun-2020    Mushahid Faizan          Modified: 3 tier changes for Rest API.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Game
{
    public class MachineGroupMachines
    {
        /// <summary>
        /// BL class for MachineGroupMachines
        /// </summary>
        private MachineGroupMachinesDTO machineGroupMachinesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor
        /// </summary>
        private MachineGroupMachines(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="machineGroupMachinesDTO">Parameter of the type MachineGroupMachinesDTO</param>
        /// <param name="executionContext">executionContext</param>
        public MachineGroupMachines(ExecutionContext executionContext, MachineGroupMachinesDTO machineGroupMachinesDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineGroupMachinesDTO);
            this.machineGroupMachinesDTO = machineGroupMachinesDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="machineGroupMachineId">Parameter of the type machineGroupMachineId</param>
        /// <param name="SqlTransaction">SqlTransaction</param>
        public MachineGroupMachines(ExecutionContext executionContext, int machineGroupMachineId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, machineGroupMachineId, sqlTransaction);
            MachineGroupMachinesDataHandler machineGroupMachinesDataHandler = new MachineGroupMachinesDataHandler(sqlTransaction);
            machineGroupMachinesDTO = machineGroupMachinesDataHandler.GetMachineGroupMachines(machineGroupMachineId);
            log.LogMethodExit(machineGroupMachinesDTO);
        }

        /// <summary>
        /// Saves the machine group machines
        /// machine group machines will be inserted if assetid is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        /// <param name="SqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            MachineGroupMachinesDataHandler machineGroupMachinesDataHandler = new MachineGroupMachinesDataHandler(sqlTransaction);

            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            if (machineGroupMachinesDTO.IsActive)
            {
                if (machineGroupMachinesDTO.Id < 0)
                {
                    machineGroupMachinesDTO = machineGroupMachinesDataHandler.InsertMachineGroupMachines(machineGroupMachinesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    machineGroupMachinesDTO.AcceptChanges();
                }
                else
                {
                    if (machineGroupMachinesDTO.IsChanged)
                    {
                        machineGroupMachinesDTO = machineGroupMachinesDataHandler.UpdateMachineGroupMachines(machineGroupMachinesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                        machineGroupMachinesDTO.AcceptChanges();
                    }
                }
            }
            else
            {
                if (machineGroupMachinesDTO.Id >= 0)
                {
                    machineGroupMachinesDataHandler.Delete(machineGroupMachinesDTO);
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the MachineGroupMachinesDTO
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            ValidationError validationError = null;
            return validationErrorList;
            // Validation Logic here 
        }

        /// <summary>
        /// get MachineGroupMachinesDTO Object
        /// </summary>
        public MachineGroupMachinesDTO GetMachineGroupMachinesDTO
        {
            get { return machineGroupMachinesDTO; }
        }
        /// <summary>
        /// set MachineGroupMachinesDTO Object        
        /// </summary>
        public MachineGroupMachinesDTO SetMachineGroupMachinesDTO
        {
            set { machineGroupMachinesDTO = value; }
        }

    }
    /// <summary>
    /// Manages the list of MachineGroupMachines
    /// </summary>
    public class MachineGroupMachinesList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<MachineGroupMachinesDTO> machineGroupMachinesDTOList = new List<MachineGroupMachinesDTO>();
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MachineGroupMachinesList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with ExecutionContext and DTO parameter
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="machineGroupMachinesDTOList">machineGroupMachinesDTOList</param>
        public MachineGroupMachinesList(ExecutionContext executionContext, List<MachineGroupMachinesDTO> machineGroupMachinesDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineGroupMachinesDTOList);
            this.machineGroupMachinesDTOList = machineGroupMachinesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Machine Groups machines list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>machineGroupMachinesDTOs</returns>
        public List<MachineGroupMachinesDTO> GetAllMachineGroupMachines(List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, searchParameters);
            MachineGroupMachinesDataHandler machineGroupMachinesDataHandler = new MachineGroupMachinesDataHandler(sqlTransaction);
            List<MachineGroupMachinesDTO> machineGroupMachinesDTOs = new List<MachineGroupMachinesDTO>();
            machineGroupMachinesDTOs = machineGroupMachinesDataHandler.GetMachineGroupMachinesList(searchParameters, sqlTransaction);
            log.LogMethodExit(machineGroupMachinesDTOs);
            return machineGroupMachinesDTOs;
        }

        /// <summary>
        /// Saves the machineGroupMachinesDTOList
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (machineGroupMachinesDTOList == null ||
               machineGroupMachinesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < machineGroupMachinesDTOList.Count; i++)
            {
                var machineGroupMachinesDTO = machineGroupMachinesDTOList[i];
                if (machineGroupMachinesDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    MachineGroupMachines machineGroupMachines = new MachineGroupMachines(executionContext, machineGroupMachinesDTO);
                    machineGroupMachines.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving MachineGroupMachinesDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("MachineGroupMachinesDTOList", machineGroupMachinesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}