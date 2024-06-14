/********************************************************************************************
 * Project Name - Game
 * Description  - RemoteVirtualArcadeUseCases class to get the data  from API by doing remote call  
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.110.0      09-Feb-2021     Fiona                      Created for Virtual Arcade
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;


namespace Semnox.Parafait.Game.VirtualArcade
{
    /// <summary>
    /// RemoteVirtualArcadeUseCases
    /// </summary>
    public class RemoteVirtualArcadeUseCases : RemoteUseCases, IVirtualArcadeUseCases
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string GameMachineImages_URL = "api/Game/GameMachineImages";
        private const string GameMachineTranslations_URL = "api/Game/GameMachineTranslations";
        private const string GameMachineFile_URL = "api/Game/Files";

        /// <summary>
        /// RemoteVirtualArcadeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        public RemoteVirtualArcadeUseCases(ExecutionContext executionContext)
            : base(executionContext)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// GetGameMachineImages
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <returns></returns>
        public async Task<List<string>> GetGameMachineImages(string gameMachine, string fileName)
        {
            log.LogMethodEntry(gameMachine, fileName);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineName".ToString(), gameMachine.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("fileName".ToString(), fileName.ToString()));
            try
            {
                List<string> result = await Get<List<string>>(GameMachineImages_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// GetGameMachineTranslations
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <returns></returns>
        public async Task<List<string>> GetGameMachineTranslations(string gameMachine, string fileName)
        {
            log.LogMethodEntry(gameMachine, fileName);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineName".ToString(), gameMachine.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("fileName".ToString(), fileName.ToString()));
            try
            {
                List<string> result = await Get<List<string>>(GameMachineTranslations_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// GetGameMachineFile
        /// </summary>
        /// <param name="gameMachine"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<string> GetGameMachineFile(string gameMachine,string fileName)
        {
            log.LogMethodEntry(gameMachine);

            List<KeyValuePair<string, string>> searchParameterList = new List<KeyValuePair<string, string>>();
            searchParameterList.Add(new KeyValuePair<string, string>("machineName".ToString(), gameMachine.ToString()));
            searchParameterList.Add(new KeyValuePair<string, string>("fileName".ToString(), fileName.ToString()));
            try
            {
                string result = await Get<string>(GameMachineFile_URL, searchParameterList);
                log.LogMethodExit(result);
                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }
    }
}
