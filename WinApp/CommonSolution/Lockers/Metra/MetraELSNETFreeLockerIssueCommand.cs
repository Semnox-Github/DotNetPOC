/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra ELS NET Free Locker Issue Command
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
 
    public class MetraELSNETFreeLockerIssueCommand : MetraLockerIssueCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Metra ELS NET Free Locker Issue Command
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="executionContext"></param>
        public MetraELSNETFreeLockerIssueCommand(string cardNumber, ExecutionContext executionContext)
            : base(BuildCommandText(cardNumber), executionContext)
        {
            log.LogMethodEntry(cardNumber, executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Build Command Text
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        static string BuildCommandText(string cardNumber)
        {
            log.LogMethodEntry(cardNumber);
            cardNumber = GetMetraCardNumber(cardNumber);
            string commandXML = string.Format("<?xml version=\"1.0\" standalone=\"yes\"?><package>" +
                                              "<header>" +
                                              "<name>WhiteList</name>" +
                                              "<version>1.0</version>" +
                                              "</header>" +
                                              "<parameters>" +
                                              "<card>{0}</card>" +
                                              "<whitelist>add</whitelist>" +
                                              " </parameters>" +
                                              "<userdata>xyz</userdata>" +
                                              "</package>", cardNumber);
            log.LogMethodExit(commandXML);
            return commandXML;
        }
    }
}
