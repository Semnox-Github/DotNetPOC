/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - The bussiness logic for parafait locker lock
 * 
 **************
 **Version Log
 **************
 *Version     Date               Modified By       Remarks          
 *********************************************************************************************
 *2.130.00    26-May-2021        Dakshakh raj      Created 
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using Semnox.Core.Utilities;
using Semnox.Parafait.logger;

namespace Semnox.Parafait.Device.Lockers
{
    public class MetraLockerFreeCommand : MetraLockCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext;

        /// <summary>
        /// constructor which accepts commandText and executionContext as parameter
        /// </summary>
        /// <param name="commandText"> command Text</param>
        /// <param name="executionContext"> machine User Context</param>
        protected MetraLockerFreeCommand(string commandText, ExecutionContext executionContext)
            : base(commandText, executionContext)
        {
            log.LogMethodEntry(commandText, executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// cardNumber
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        static string BuildCommandText(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            cardNumber = GetMetraCardNumber(cardNumber);
            string commandXML = string.Format("<?xml version=\"1.0\" standalone=\"yes\"?>" +
                                              "<package>" +
                                              "<header>" +
                                              "<name>TicketsErase</name>" +
                                              "<version>1.0</version>" +
                                              "</header>" +
                                              "<parameters>" +
                                              "<card>{0}</card>" +
                                              "</parameters>" +
                                              "<userdata>xyz</userdata>" +
                                              "</package>", cardNumber);
            log.LogMethodExit(commandXML);
            return commandXML;
        }

        /// <summary>
        /// Execute
        /// </summary>
        public async new System.Threading.Tasks.Task<MetraLockerFreeResponse> Execute()
        {
            log.LogMethodEntry();
            string response = await base.Execute();
            MetraLockerFreeResponse result = new MetraLockerFreeResponse(response);
            log.LogMethodExit(result);
            return result;
        }
    }
}
