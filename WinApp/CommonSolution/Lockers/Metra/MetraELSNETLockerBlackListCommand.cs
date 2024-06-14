/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra ELS NET Locker BlackList Command
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
    public class MetraELSNETLockerBlackListCommand : MetraLockerBlackListCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Metra ELS NET Locker BlackList Command
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="action"></param>
        /// <param name="executionContext"></param>
        /// <param name="lockerMake"></param>
        public MetraELSNETLockerBlackListCommand(string cardNumber, string action, ExecutionContext executionContext, string lockerMake)
            : base(BuildCommandText(cardNumber, action), executionContext)
        {
            log.LogMethodEntry(cardNumber, action, executionContext, lockerMake);
            this.executionContext = executionContext;
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
    }
}
