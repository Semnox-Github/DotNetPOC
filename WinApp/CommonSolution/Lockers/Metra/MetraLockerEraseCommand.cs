/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra Locker Erase Command
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

    public class MetraLockerEraseCommand : MetraLockCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext executionContext;

        /// <summary>
        /// constructor which accepts commandText and executionContext as parameter
        /// </summary>
        /// <param name="commandText"> command Text</param>
        /// <param name="executionContext"> machine User Context</param>
        protected MetraLockerEraseCommand(string commandText, ExecutionContext executionContext)
            : base(commandText, executionContext)
        {
            log.LogMethodEntry(commandText, executionContext);
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
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<MetraLockerEraseResponse> Execute()
        {
            log.LogMethodEntry();
            string response = await base.Execute();
            MetraLockerEraseResponse result = new MetraLockerEraseResponse(response);
            log.LogMethodExit(result);
            return result;
        }
    }
}
