/********************************************************************************************
* Project Name - Game
* Description  - Interface for GameMachineLevel Controller.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     09-Feb-2021       Fiona               Created for Virtual Arcade
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game.VirtualArcade
{
    /// <summary>
    /// IVirtualArcadeUseCases
    /// </summary>
    public interface IVirtualArcadeUseCases
    {
        /// <summary>
        /// GetGameMachineImages
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<List<string>> GetGameMachineImages(string gameMachine, string fileName);
        /// <summary>
        /// GetGameMachineTranslations
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<List<string>> GetGameMachineTranslations(string gameMachine, string fileName);
        /// <summary>
        /// GetGameMachineFile
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<string> GetGameMachineFile(string gameMachine, string fileName);
    }
}
