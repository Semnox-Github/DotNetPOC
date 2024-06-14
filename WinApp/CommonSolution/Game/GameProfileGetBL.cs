/********************************************************************************************
 * Project Name - GameProfileGetBL
 * Description  - GameProfileGetBL class is used to call the business methods - Only  Get operation
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game
{
    public class GameProfileGetBL
    {
        private GameProfileDTO gameProfileDTO;
        private GameProfileDataHandler gameProfileDataHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private SqlTransaction sqlTransaction;


        private GameProfileGetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
       
        public GameProfileGetBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            gameProfileDataHandler = new GameProfileDataHandler(sqlTransaction);
            gameProfileDTO = gameProfileDataHandler.GetGameProfile(id);
            if (gameProfileDTO != null)
            {
                MachineAttributeDataHandler machineAttributeDataHandler = new MachineAttributeDataHandler(sqlTransaction);
                List<MachineAttributeDTO> machineAttributes = machineAttributeDataHandler.GetMachineAttributes(MachineAttributeDTO.AttributeContext.GAME_PROFILE, gameProfileDTO.GameProfileId, executionContext.GetSiteId());
                gameProfileDTO.SetAttributeList(machineAttributes);
            }
            log.LogMethodExit();
        }

      
        public GameProfileGetBL(ExecutionContext executionContext, GameProfileDTO gameProfileDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, gameProfileDTO);
            this.gameProfileDTO = gameProfileDTO;
            log.LogMethodExit();
        }

        public MachineAttributeDTO GetGameProfileMachineAttribute(MachineAttributeDTO.MachineAttribute attribute)
        {
            log.LogMethodEntry(attribute);
            foreach (MachineAttributeDTO currAttribute in gameProfileDTO.ProfileAttributes)
            {
                if (currAttribute.AttributeName == attribute)
                {
                    log.LogMethodExit(currAttribute);
                    return currAttribute;
                }
            }
            log.Error("The game system attribute by name " + attribute + " does not exist for game profile " + gameProfileDTO.ProfileName + ". Please check the system setup");
            throw new Exception("The game system attribute by name " + attribute + " does not exist for game profile " + gameProfileDTO.ProfileName + ". Please check the system setup");
        }
    }
}
