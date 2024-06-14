/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra Locker BlackList Command
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
    
    public class MetraLockerBlackListCommand : MetraLockCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Constructor for Metra Locker BlackList Command
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="action"></param>
        /// <param name="executionContext"></param>
        /// <param name="lockerMake"></param>
        public MetraLockerBlackListCommand(string cardNumber, string action, ExecutionContext executionContext, string lockerMake)
            : base(BuildCommandText(cardNumber, action), executionContext)
        {
            log.LogMethodEntry(cardNumber, action, executionContext, lockerMake);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// constructor which accepts card type object as parameter
        /// </summary>
        /// <param name="commandText"> command Text</param>
        /// <param name="executionContext"> machine User Context</param>
        protected MetraLockerBlackListCommand(string commandText, ExecutionContext executionContext)
            : base(commandText, executionContext)
        {
            log.LogMethodEntry(commandText, executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Build Command Text
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        static string BuildCommandText(string cardNumber, string action)
        {
            log.LogMethodEntry(cardNumber, action);
            if (cardNumber.Length == 10)
            {
                cardNumber = cardNumber.Substring(0, 8);
            }
            cardNumber = GetMetraCardNumber(cardNumber);
            string commandXML = string.Format("<?xml version=\"1.0\" standalone=\"yes\"?>" +
                                              "<package>" +
                                              "<header>" +
                                              "<name>BlackList</name>" +
                                              "<version>1.0</version>" +
                                              "</header>" +
                                              "<parameters>" +
                                              "<card>{0}</card>" +
                                              "<blacklist>{1}</blacklist>" +
                                              "</parameters>" +
                                              "</package>", cardNumber, action);
            log.LogMethodExit(commandXML);
            return commandXML;
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <returns></returns>
        public async new System.Threading.Tasks.Task<MetraLockerBlackListResponse> Execute()
        {
            log.LogMethodEntry();
            string response = await base.Execute();
            MetraLockerBlackListResponse result = new MetraLockerBlackListResponse(response);
            log.LogMethodExit(result);
            return result;
        }
    }
}
