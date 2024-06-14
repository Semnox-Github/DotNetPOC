/********************************************************************************************
 * Project Name - Games
 * Description  - GamesGetBL class is used to call the business methods - Only  Get operation
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
    public class GamesGetBL
    {
        private GameDTO gameDTO;
        private GameDataHandler gameDataHandler;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;


        private GamesGetBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
      
        public GamesGetBL(ExecutionContext executionContext, GameDTO gameDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, gameDTO);
            this.gameDTO = gameDTO;
            log.LogMethodExit();
        }
        public MachineAttributeDTO GetGameMachineAttribute(MachineAttributeDTO.MachineAttribute attribute)
        {
            log.LogMethodEntry(attribute);
            foreach (MachineAttributeDTO currAttribute in gameDTO.GameAttributes)
            {
                if (currAttribute.AttributeName == attribute)
                {
                    log.LogMethodExit(currAttribute);
                    return currAttribute;
                }
            }
            log.Error("The game system attribute by name " + attribute + " does not exist for game " + gameDTO.GameName + ". Please check the system setup");
            throw new Exception("The game system attribute by name " + attribute + " does not exist for game " + gameDTO.GameName + ". Please check the system setup");
        }
    }
}
