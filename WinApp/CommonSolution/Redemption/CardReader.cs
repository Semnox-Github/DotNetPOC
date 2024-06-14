/********************************************************************************************
 * Class Name - RedemptionUtils                                                                         
 * Description - Card Reader
 * 
 * 
 **************
 **Version Log
 **************
 *Version     Date                   Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        12-Aug-2019            Deeksha        Added logger methods.
 ********************************************************************************************/

namespace Semnox.Parafait.Redemption
{
    static class CardReader
    {
        // used to trigger code in calling program to receive serial port data
        // set calling program's function to setReceiveAction
        // set RequiredByOthers = true in order to trigger calling program's proc when serial port receives any data
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool CardScanned = false;
        public static string CardNumber = "";

        public delegate void InvokeHandle();

        static InvokeHandle receiveAction;

        public static void OthersTasks()
        {
            log.LogMethodEntry();
            if (receiveAction != null)
                receiveAction.Invoke();
            log.LogMethodExit();
        }

        public static InvokeHandle setReceiveAction
        {
            get
            {
                return receiveAction;
            }
            set
            {
                receiveAction = value;
            }
        }
    }
}
