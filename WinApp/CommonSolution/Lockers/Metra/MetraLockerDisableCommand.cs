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
    /// <summary>
    /// Business logic for the base class of parafait locker lock
    /// </summary>
    public class MetraLockerDisableCommand : MetraLockCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext;

        /// <summary>
        /// constructor which accepts card type object as parameter
        /// </summary>
        /// <param name="readerDevice"> Card class reader device object</param>
        /// <param name="machineUserContext"> Execution context from parafait utils</param>
        public MetraLockerDisableCommand(string cardNumber, ExecutionContext executionContext)
            : base(BuildCommandText(cardNumber), executionContext)
        {
            log.LogMethodEntry(cardNumber, executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Sets the allocation 
        /// </summary>
        /// <param name="lockerAllocationDTO"></param>
        static string BuildCommandText(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            cardNumber = GetMetraCardNumber(cardNumber);
            string commandXML = string.Format("<?xml version=\"1.0\" standalone=\"yes\"?>" +
                                              "<package>" +
                                              "<header>" +
                                              "<name>Disable</name>" +
                                              "<version>1.0</version>" +
                                              "</header>" +
                                              "<parameters>" +
                                              "<card>{0}</card>" +
                                              "<refund>disable</refund>" +
                                              "</parameters>" +
                                              "</package>", cardNumber);
            log.LogMethodExit(commandXML);
            return commandXML;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<MetraLockerDisableResponse> Execute()
        {
            log.LogMethodEntry();
            string response = await base.Execute();
            MetraLockerDisableResponse result = new MetraLockerDisableResponse(response);
            log.LogMethodExit(result);
            return result;
        }
    }
}
