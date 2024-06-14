/********************************************************************************************
 * Project Name - MachineGetBL
 * Description  - MachineGetBL class is used to call the business methods - Only  Get operation
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Aug-2020       Girish Kundar             Created : POS UI Redesign with REST API
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Game
{
    public class MachineGetBL
    {
        private MachineDTO machineDTO;
        private MachineDataHandler machineDataHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;


        private MachineGetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
       
        public MachineGetBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            machineDataHandler = new MachineDataHandler(sqlTransaction);
            machineDTO = machineDataHandler.GetMachine(id);
            if (machineDTO != null)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, machineDTO.MachineId, executionContext.GetSiteId());
                machineDTO.SetAttributeList(machineAttributes);
            }
            log.LogMethodExit();
        }

      
        public MachineGetBL(ExecutionContext executionContext, MachineDTO machineDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineDTO);
            this.machineDTO = machineDTO;
            log.LogMethodExit();
        }

        public MachineAttributeDTO GetMachineMachineAttribute(MachineAttributeDTO.MachineAttribute attribute)
        {
            log.LogMethodEntry(attribute);
            foreach (MachineAttributeDTO currAttribute in machineDTO.GameMachineAttributes)
            {
                if (currAttribute.AttributeName == attribute)
                {
                    log.LogMethodExit(currAttribute);
                    return currAttribute;
                }
            }
            log.Error("The game system attribute by name " + attribute + " does not exist for machine " + machineDTO.MachineName + ". Please check the system setup");
            throw new Exception("The game system attribute by name " + attribute + " does not exist for machine " + machineDTO.MachineName + ". Please check the system setup");
        }

       
    }
}
