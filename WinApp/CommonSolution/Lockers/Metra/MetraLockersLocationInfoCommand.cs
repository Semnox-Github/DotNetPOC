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
    /// <summary>
    /// Business logic for the base class of parafait locker lock
    /// </summary>
    public class MetraLockersLocationInfoCommand : MetraLockCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext;
        /// <summary>
        /// constructor which accepts card type object as parameter
        /// </summary>
        /// <param name="readerDevice"> Card class reader device object</param>
        /// <param name="machineUserContext"> Execution context from parafait utils</param>
        public MetraLockersLocationInfoCommand(string zoneName, ExecutionContext executionContext)
            : base(BuildCommandText(zoneName), executionContext)
        {
            log.LogMethodEntry(zoneName, executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Sets the allocation 
        /// </summary>
        /// <param name="lockerAllocationDTO"></param>
        static string BuildCommandText(string zoneName)
        {
            string commandXML = string.Format("<?xml version=\"1.0\" standalone=\"yes\"?>" +
                                              "<package>" +
                                              "<header>" +
                                              "<name>LockersInfo</name>" +
                                              "<version>1.0</version>" +
                                              "</header>" +
                                              "<parameters>" +
                                              "<location>{0}</location>" +
                                              "</parameters>" +
                                              "<userdata>xyz</userdata>" +
                                              "</package>", zoneName);
            return commandXML;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<MetraLockersLocationInfoResponse> Execute()
        {
            log.LogMethodEntry();
            string response = await base.Execute();
            MetraLockersLocationInfoResponse result = new MetraLockersLocationInfoResponse(response);
            log.LogMethodExit(result);
            return result;
        }
    }
}
