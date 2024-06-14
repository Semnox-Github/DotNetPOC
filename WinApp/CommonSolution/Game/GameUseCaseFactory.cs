/********************************************************************************************
* Project Name - Game
* Description  - Factory class for Game.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.110.0     06-Dec-2020     Prajwal S             Created : Web Inventory UI Redesign with REST API

********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Configuration;
using Semnox.Parafait.Game.VirtualArcade;

namespace Semnox.Parafait.Game
{
   public class GameUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IGameProfileUseCases GetGameProfileUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IGameProfileUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteGameProfileUseCases(executionContext);
            }
            else
            {
                result = new LocalGameProfileUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        public static IGamePlayUseCases GetGamePlayUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IGamePlayUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteGamePlayUseCases(executionContext);
            }
            else
            {
                result = new LocalGamePlayUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        public static IMachineGroupsUseCases GetMachineGroupsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMachineGroupsUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMachineGroupsUseCases(executionContext);
            }
            else
            {
                result = new LocalMachineGroupsUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static IGameUseCases GetGameUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IGameUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteGameUseCases(executionContext);
            }
            else
            {
                result = new LocalGameUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static IReaderConfigurationUseCases GetReaderConfigurationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IReaderConfigurationUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteReaderConfigurationUseCases(executionContext);
            }
            else
            {
                result = new LocalReaderConfigurationUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }


        public static IMachineUseCases GetMachineUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMachineUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMachineUseCases(executionContext);
            }
            else
            {
                result = new LocalMachineUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
        public static IGenericCalendarUseCases GetGenericCalendarUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IGenericCalendarUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteGenericCalendarUseCases(executionContext);
            }
            else
            {
                result = new LocalGenericCalendarUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        public static IHubUseCases GetHubUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IHubUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteHubUseCases(executionContext);
            }
            else
            {
                result = new LocalHubUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }



        public static IMachineInputDevicesUseCases GetMachineInputDevicesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMachineInputDevicesUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMachineInputDevicesUseCases(executionContext);
            }
            else
            {
                result = new LocalMachineInputDevicesUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetGameMachineLevelUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IGameMachineLevelUseCases GetGameMachineLevelUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IGameMachineLevelUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteGameMachineLevelUseCases(executionContext);
            }
            else
            {
                result = new LocalGameMachineLevelUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetVirtualArcadeUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IVirtualArcadeUseCases GetVirtualArcadeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IVirtualArcadeUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteVirtualArcadeUseCases(executionContext);
            }
            else
            {
                result = new LocalVirtualArcadeUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// GetMachineCommunicationLogUseCases
        /// </summary>
        /// <param name="executionContext"></param>
        /// <returns></returns>
        public static IMachineCommunicationLogUseCases GetMachineCommunicationLogUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IMachineCommunicationLogUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteMachineCommunicationLogUseCases(executionContext);
            }
            else
            {
                result = new LocalMachineCommunicationLogUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
