/********************************************************************************************
 * Project Name - Game
 * Description  - LocalVirtualArcadeUseCases class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By                Remarks          
 *********************************************************************************************
 *2.110.0     09-Feb-2021      Fiona                      Created for Virtual Arcade
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Semnox.Parafait.Game.VirtualArcade
{
    /// <summary>
    /// LocalVirtualArcadeUseCases
    /// </summary>
    public class LocalVirtualArcadeUseCases : IVirtualArcadeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        /// <summary>
        /// LocalVirtualArcadeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public LocalVirtualArcadeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// GetGameMachineFile
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> GetGameMachineFile(string gameMachine, string fileName)
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(gameMachine);
                VirtualArcadeBL virtualArcadeBL = new VirtualArcadeBL(executionContext);
                string file = virtualArcadeBL.GetMachineFile(gameMachine, fileName);
                log.LogMethodExit(file);
                return file;
            });
        }
        /// <summary>
        /// GetGameMachineImages
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<List<string>> GetGameMachineImages(string gameMachine, string fileName)
        {
            return await Task<List<string>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(gameMachine);
                VirtualArcadeBL virtualArcadeBL = new VirtualArcadeBL(executionContext);
                List<string> machineImagesList = virtualArcadeBL.GetGameMachineImages(gameMachine, fileName);
                log.LogMethodExit(machineImagesList);
                return machineImagesList;
            });
        }
        /// <summary>
        /// GetGameMachineTranslations
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<List<string>> GetGameMachineTranslations(string gameMachine, string fileName)
        {
            return await Task<List<string>>.Factory.StartNew(() =>
            {
                log.LogMethodEntry(gameMachine);
                VirtualArcadeBL virtualArcadeBL = new VirtualArcadeBL(executionContext);
                List<string> machineTranslationsList = virtualArcadeBL.GetGameMachineTranslations(gameMachine, fileName);
                log.LogMethodExit(machineTranslationsList);
                return machineTranslationsList;
            });
        }
    }
}
