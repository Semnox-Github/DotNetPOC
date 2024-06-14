/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra ELS Locker Issue Command
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

namespace Semnox.Parafait.Device.Lockers
{
    public class MetraELSLockerIssueCommand : MetraLockerIssueCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Metra ELS Locker Issue Command
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="zoneCode"></param>
        /// <param name="freeMode"></param>
        /// <param name="lockerNo"></param>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <param name="executionContext"></param>
        public MetraELSLockerIssueCommand(string cardNumber, string zoneCode, bool freeMode, string lockerNo, DateTime dateTimeFrom, DateTime dateTimeTo, ExecutionContext executionContext)
            : base(BuildCommandText(cardNumber, zoneCode, freeMode, lockerNo, dateTimeFrom, dateTimeTo), executionContext)
        {
            log.LogMethodEntry(cardNumber, zoneCode, freeMode, lockerNo, dateTimeFrom, dateTimeTo, executionContext);
            log.LogMethodExit();
        }

        /// <summary>
        /// Build Command Text
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <param name="zoneCode"></param>
        /// <param name="freeMode"></param>
        /// <param name="lockerNo"></param>
        /// <param name="dateTimeFrom"></param>
        /// <param name="dateTimeTo"></param>
        /// <returns></returns>
        static string BuildCommandText(string cardNumber, string zoneCode, bool freeMode, string lockerNo, DateTime dateTimeFrom, DateTime dateTimeTo)
        {
            log.LogMethodEntry(cardNumber, zoneCode, freeMode, lockerNo, dateTimeFrom, dateTimeTo);
            cardNumber = GetMetraCardNumber(cardNumber);
            string fromDate = dateTimeFrom.ToString("yyyy-MM-dd'T'HH:mm:ss");
            string toDate = dateTimeTo.ToString("yyyy-MM-dd'T'HH:mm:ss");
            MetraLocationCode metraLocationCode = new MetraLocationCode(zoneCode);
            int zoneCodeValue = metraLocationCode.Value;
            string commandXML = string.Format("<?xml version=\"1.0\" standalone=\"yes\"?><package>" +
                                              "<header>" +
                                              "<name>ItemIssue</name>" +
                                              "<version>1.0</version>" +
                                              "</header>" +
                                              "<parameters>" +
                                              "<pos>1</pos>" +
                                              "<user>1</user>" +
                                              "<item>{0}</item>" +
                                              "" +
                                              "<card>{1}</card>" +
                                              "" +
                                              "<datetimefrom>{2}</datetimefrom>" +
                                              "" +
                                              "<datetimeto>{3}</datetimeto>" +
                                              "" +
                                              " </parameters>" +
                                              "" +
                                              "<userdata>xyz</userdata>" +
                                              "</package>", zoneCodeValue, cardNumber, fromDate, toDate);
            log.LogMethodExit(commandXML);
            return commandXML;
        }        
    }
}
