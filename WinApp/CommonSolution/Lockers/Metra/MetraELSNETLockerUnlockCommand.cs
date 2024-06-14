/********************************************************************************************
 * Project Name - Parafait Locker Lock
 * Description  - Metra ELS NET Locker Unlock Command
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
 
    public class MetraELSNETLockerUnlockCommand : MetraLockerUnlockCommand
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Metra ELS NET Locker Unlock Command
        /// </summary>
        /// <param name="zoneCode"></param>
        /// <param name="lockerNo"></param>
        /// <param name="executionContext"></param>
        public MetraELSNETLockerUnlockCommand(string zoneCode, string lockerNo, ExecutionContext executionContext)
            : base(BuildCommandText(zoneCode, lockerNo), executionContext)
        {
            log.LogMethodEntry(zoneCode, lockerNo, executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Metra ELS NET Locker Unlock Command
        /// </summary>
        /// <param name="commandText"> command Text</param>
        /// <param name="executionContext"> machine User Context</param>
        protected MetraELSNETLockerUnlockCommand(string commandText, ExecutionContext executionContext)
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
    }
}
