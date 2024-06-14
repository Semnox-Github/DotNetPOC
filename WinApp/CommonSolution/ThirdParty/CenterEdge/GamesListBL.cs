/********************************************************************************************
 * Project Name - CenterEdge  
 * Description  - GamesListBL class - This is business layer class for GamesList
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 0.0         24-Sep-2020       Girish Kundar             Created : CenterEdge  REST API
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Game;

namespace Semnox.Parafait.ThirdParty.CenterEdge
{
    /// <summary>
    /// This class used to get the game details as  the centerEdge game object
    /// </summary>
    public class GamesListBL
    {
        private readonly ExecutionContext executionContext;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of GamesListBL class
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        public GamesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// This method builds and returns the centerEdge game response object from 
        /// Parafait MachineDTO
        /// </summary>
        /// <returns></returns>
        public Game GetMachines(int skip, int take)
        {
            log.LogMethodEntry(); // center edge GamesDTO maps to MachineDTO in parafait system
            List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>> searchParameters = new List<KeyValuePair<MachineDTO.SearchByMachineParameters, string>>();
            searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<MachineDTO.SearchByMachineParameters, string>(MachineDTO.SearchByMachineParameters.IS_ACTIVE, "Y"));
            MachineList machineList = new MachineList(executionContext);
            List<MachineDTO> machineDTOList = machineList.GetMachineList(searchParameters);
            Game ceGameResponseObject = new Game();
            if (machineDTOList != null && machineDTOList.Any())
            {
                foreach(MachineDTO machineDTO in machineDTOList)
                {
                    GameDTO gameDTO = new GameDTO(machineDTO.MachineId, machineDTO.MachineName, true);
                    ceGameResponseObject.games.Add(gameDTO);
                }
            }
            ceGameResponseObject.skipped = skip;
            if (take > 0)
            {
                take = take > ceGameResponseObject.games.Count ? ceGameResponseObject.games.Count : take;
            }
            else
            {
                take = ceGameResponseObject.games.Count;
            }
            if (take > ceGameResponseObject.games.Count - skip)
            {
                take = ceGameResponseObject.games.Count - skip;
            }
            List<GameDTO> result = ceGameResponseObject.games.GetRange(skip, take);
            ceGameResponseObject.games.Clear();
            ceGameResponseObject.games.AddRange(result);
            ceGameResponseObject.totalCount = ceGameResponseObject.games.Count;
            log.LogMethodExit(ceGameResponseObject);
            return ceGameResponseObject;
        }

        /// <summary>
        /// This method gets the MachineDTO  from the machineId
        /// </summary>
        /// <param name="MachineDTO"></param>
        /// <returns></returns>
        public MachineDTO GetMachineDTO(int machineId)
        {
            log.LogMethodEntry(machineId);
            MachineDTO machineDTO = null;
            if (machineId > 0)
            {
                Machine machineBL = new Machine(machineId, executionContext);
                machineDTO = machineBL.GetMachineDTO;
            }
            else
            {
                machineDTO = new MachineDTO();
                machineDTO.MachineId = 0;
                machineDTO.MachineName = string.Empty;
            }
            log.LogMethodExit(machineDTO);
            return machineDTO;
        }

    }
}
