/********************************************************************************************
* Project Name - Game
* Description  - Factory class for Game.
*  
**************
**Version Log
**************
*Version     Date             Modified By          Remarks          
*********************************************************************************************
*2.150.2     12-Dec-2022      Mathew Ninan         Created : GameTransactionUseCaseFactory
********************************************************************************************/
using System;
using Semnox.Core.Utilities;
using System.Configuration;
using Semnox.Parafait.Transaction;

namespace Semnox.Parafait.ServerCore
{
   public class GameTransactionUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static IGameTransactionUseCases GetGameTransactionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IGameTransactionUseCases result;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                result = new RemoteGameTransactionUseCases(executionContext);
            }
            else
            {
                result = new LocalGameTransactionUseCases(executionContext);
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
