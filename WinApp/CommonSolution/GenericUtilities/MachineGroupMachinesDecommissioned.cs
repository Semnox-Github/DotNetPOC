/********************************************************************************************
 * Project Name - Machine Groups
 * Description  - Bussiness logic of machine groups machines
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70        13-May-2019   Jagan Mohana Rao    Created
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.Utilities;

namespace Semnox.Core.GenericUtilities
{
    public class MachineGroupMachines
    {
        private MachineGroupMachinesDTO machineGroupMachinesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public MachineGroupMachines(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.machineGroupMachinesDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="machineGroupMachinesDTO"></param>
        public MachineGroupMachines(ExecutionContext executionContext, MachineGroupMachinesDTO machineGroupMachinesDTO)
        {
            log.LogMethodEntry(machineGroupMachinesDTO, executionContext);
            this.machineGroupMachinesDTO = machineGroupMachinesDTO;
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the Id parameter
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="machineGroupMachineId"></param>
        public MachineGroupMachines(ExecutionContext executionContext, int machineGroupMachineId)
        {
            log.LogMethodEntry(executionContext, machineGroupMachineId);
            MachineGroupMachinesDataHandler machineGroupMachinesDataHandler = new MachineGroupMachinesDataHandler();
            this.executionContext = executionContext;
            this.machineGroupMachinesDTO = machineGroupMachinesDataHandler.GetMachineGroupMachines(machineGroupMachineId);
            log.LogMethodExit(machineGroupMachinesDTO);
        }

        /// <summary>
        /// Saves the machine group machines
        /// machine group machines will be inserted if assetid is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save()
        {
            log.LogMethodEntry();
            MachineGroupMachinesDataHandler machineGroupMachinesDataHandler = new MachineGroupMachinesDataHandler();
            if (machineGroupMachinesDTO.Id < 0)
            {
                int machineGroupMachineId = machineGroupMachinesDataHandler.InsertMachineGroupMachines(machineGroupMachinesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                machineGroupMachinesDTO.MachineGroupId = machineGroupMachineId;
            }
            else
            {
                if (machineGroupMachinesDTO.IsChanged == true)
                {
                    machineGroupMachinesDataHandler.UpdateMachineGroupMachines(machineGroupMachinesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    machineGroupMachinesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
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
        private ExecutionContext executionContext;
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
        /// Returns the Machine Groups machines list
        /// </summary>
        public List<MachineGroupMachinesDTO> GetAllMachineGroupMachines(List<KeyValuePair<MachineGroupMachinesDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            MachineGroupMachinesDataHandler machineGroupMachinesDataHandler = new MachineGroupMachinesDataHandler();
            log.LogMethodExit();
            return machineGroupMachinesDataHandler.GetMachineGroupMachinesList(searchParameters);
        }        
    }
}