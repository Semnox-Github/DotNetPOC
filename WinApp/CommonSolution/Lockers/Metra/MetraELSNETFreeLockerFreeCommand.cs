/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra ELS NET Free Locker Free Command
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
    public class MetraELSNETFreeLockerFreeCommand : MetraLockerFreeCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Metra ELS NET Free Locker Free Command
        /// </summary>
        /// <param name="zoneCode"></param>
        /// <param name="lockerNumber"></param>
        /// <param name="executionContext"></param>
        public MetraELSNETFreeLockerFreeCommand(string zoneCode, string lockerNumber, ExecutionContext executionContext)
            : base(BuildCommandText(zoneCode, lockerNumber), executionContext)
        {
            log.LogMethodEntry(zoneCode, lockerNumber, executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Build Command Text
        /// </summary>
        /// <param name="zoneCode"></param>
        /// <param name="lockerNumber"></param>
        /// <returns></returns>
        static string BuildCommandText(string zoneCode, string lockerNumber)
        {
            log.LogMethodEntry(zoneCode, lockerNumber);
            MetraLocationCode metraLocationCode = new MetraLocationCode(zoneCode);
            int zoneCodeValue = metraLocationCode.Value;
            string commandXML = string.Format("<?xml version=\"1.0\"?><package>" +
                                              "<header>" +
                                              "<name>LockerELSNETFree</name>" +
                                              "<version>1.0</version>" +
                                              "</header>" +
                                              "<parameters>" +
                                              "<item>{0}</item>" +
                                              "<locker>{1}</locker>" +
                                              "</parameters>" +
                                              "<userdata>xyz</userdata>" +
                                              "</package>", zoneCodeValue, lockerNumber);
            log.LogMethodExit(commandXML);
            return commandXML;
        }
    }
}
