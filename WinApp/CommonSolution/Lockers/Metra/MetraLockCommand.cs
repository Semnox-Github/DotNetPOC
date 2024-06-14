/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra Lock Command
 * 
 **************
 **Version Log
 **************
 *Version     Date               Modified By       Remarks          
 *********************************************************************************************
 *2.130.00    26-May-2021        Dakshakh raj      Created 
 ********************************************************************************************/

using System;
using Semnox.Core.Utilities;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities;

namespace Semnox.Parafait.Device.Lockers
{
    public class MetraLockCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string commandText = string.Empty;
        private RemotingClient remotingClient;
        protected ExecutionContext executionContext;
        string onlineServiceUrl = string.Empty;

        /// <summary>
        /// MetraLockCommand constructor
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="executionContext"></param>
        public MetraLockCommand(string commandText, ExecutionContext executionContext)
        {
            log.LogMethodEntry(commandText, executionContext);
            this.executionContext = executionContext;
            this.commandText = commandText;
            onlineServiceUrl = ParafaitDefaultContainerList.GetParafaitDefault(this.executionContext, "ONLINE_LOCKER_SERVICE_URL");
            remotingClient = new RemotingClient();
            log.LogMethodExit();
        }

        /// <summary>
        /// Execute method to post commands
        /// </summary>
        protected async System.Threading.Tasks.Task<string> Execute()
        {
            log.LogMethodEntry();
            Semnox.Core.GenericUtilities.WebApiResponse webApiResponse = remotingClient.Post(onlineServiceUrl, new List<KeyValuePair<string, string>>(), commandText, "text/xml");
            log.LogMethodExit(webApiResponse.Response);
            return webApiResponse.Response;
        }

        /// <summary>
        /// Get Metra CardNumber
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static string GetMetraCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new Exception();
            }
            else if (cardNumber.Length == 8)
            {
                return "0009000000000000004D00000000" + cardNumber;
            }

            else if (cardNumber.Length == 16)
            {
                return "0009000000000000000000" + cardNumber;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
