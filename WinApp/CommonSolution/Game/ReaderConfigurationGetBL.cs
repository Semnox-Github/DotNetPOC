/********************************************************************************************
 * Project Name - Games
 * Description  - ReaderConfigurationGetBL class is used to call the business methods - Only  Get operation
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
    public class ReaderConfigurationGetBL
    {
        private MachineAttributeDTO machineAttributeDTO;
        private MachineAttributeDataHandler machineAttributeDataHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;


        private ReaderConfigurationGetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        public ReaderConfigurationGetBL(ExecutionContext executionContext , MachineAttributeDTO machineAttributeDTO)
            :this(executionContext)
        {
            log.LogMethodEntry(executionContext, machineAttributeDTO);
            this.machineAttributeDTO = machineAttributeDTO;
            log.LogMethodExit();
        }

        public ReaderConfigurationGetBL(ExecutionContext executionContext,int attributeId)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, attributeId);
            machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            machineAttributeDTO = machineAttributeDataHandler.GetMachineAttributeDTO(attributeId, executionContext.GetSiteId());
            log.LogMethodExit();
        }

        public int GetAttributeId(ExecutionContext executionContext, string attributeName , SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(executionContext, attributeName, sqlTransaction);
            machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            int attributeId = machineAttributeDataHandler.GetAttributeId(attributeName,executionContext.GetSiteId());
            log.LogMethodExit(attributeId);
            return attributeId;
        }


        public List<MachineAttributeDTO> GetMachineAttributes(string attributeContext, int id ,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(attributeContext, id, sqlTransaction);
            machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
            List<MachineAttributeDTO> systemAttributes = new List<MachineAttributeDTO>();
            int siteId = this.executionContext.GetSiteId();
            switch (attributeContext)
            {
                case "SYSTEM":
                    systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.SYSTEM, id, siteId);
                    break;
                case "GAMES":
                    systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME, id, siteId);
                    break;

                case "GAME_PROFILE":
                    systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME_PROFILE, id, siteId);
                    break;

                case "MACHINES":
                    systemAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.MACHINE, id, siteId);
                    break;
            }
            log.LogMethodExit(systemAttributes);
            return systemAttributes;
        }

        //public MachineAttributeDTO GetMachineAttribute(MachineAttributeDTO.MachineAttribute attribute)
        //{
        //    log.LogMethodEntry(attribute);
        //    foreach (MachineAttributeDTO currAttribute in systemAttributes)
        //    {
        //        if (currAttribute.AttributeName == attribute)
        //        {
        //            log.LogMethodExit(currAttribute);
        //            return currAttribute;
        //        }
        //    }
        //    // Ideally there should not be a case where a parameter is defined and it does not exist even at system level. 
        //    // Could only happen in case the DB is at the lower patch level than the system. 
        //    log.Info("Ends-GetMachineAttribute(attribute) Method.By throwing manual exception:\"The game system attribute by name  " + attribute + "  does not exist. Please check the system setup\"");
        //    throw new Exception("The game system attribute by name " + attribute + " does not exist. Please check the system setup");
        //}

    }
}
