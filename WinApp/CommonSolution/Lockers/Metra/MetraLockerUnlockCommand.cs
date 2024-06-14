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

using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.Lockers
{
    public class MetraLockerUnlockCommand : MetraLockCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext;

        /// <summary>
        /// Metra Locker Unlock Command
        /// </summary>
        /// <param name="zoneCode"></param>
        /// <param name="lockerNo"></param>
        /// <param name="executionContext"></param>
        public MetraLockerUnlockCommand(string zoneCode, string lockerNo, ExecutionContext executionContext)
            : base(BuildCommandText(zoneCode, lockerNo), executionContext)
        {
            log.LogMethodEntry(zoneCode, lockerNo, executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor which accepts card type object as parameter
        /// </summary>
        /// <param name="commandText"> command Text</param>
        /// <param name="executionContext"> machine User Context</param>
        protected MetraLockerUnlockCommand(string commandText, ExecutionContext executionContext)
            : base(commandText, executionContext)
        {
            log.LogMethodEntry(commandText, executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Build Command Text
        /// </summary>
        /// <param name="zoneCode"></param>
        /// <param name="lockerNo"></param>
        /// <returns></returns>
        static string BuildCommandText(string zoneCode, string lockerNo)
        {
            log.LogMethodEntry(zoneCode, lockerNo);
            MetraLocationCode metraLocationCode = new MetraLocationCode(zoneCode);
            int zoneCodeValue = metraLocationCode.Value;
            string commandXML = string.Format("<?xml version=\"1.0\" standalone=\"yes\"?>" +
                                              "<package>" +
                                              "<header>" +
                                              "<name>Unlock</name>" +
                                              "<version>1.0</version>" +
                                              "</header>" +
                                              "<parameters>" +
                                              "<locker>{0}</locker>" +
                                              "<location>{1}</location>" +
                                              "</parameters>" +
                                              "<userdata>xyz</userdata>" +
                                              "</package>", lockerNo, zoneCodeValue);
            log.LogMethodExit(commandXML);
            return commandXML;
        }
        /// <summary>
        /// Execute
        /// </summary>
        public async System.Threading.Tasks.Task<MetraLockerUnlockResponse> Execute()
        {
            log.LogMethodEntry();
            string response = await base.Execute();
            MetraLockerUnlockResponse result = new MetraLockerUnlockResponse(response);
            log.LogMethodExit(result);
            return result;
        }
    }
}
