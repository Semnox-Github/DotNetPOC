/********************************************************************************************
 * Project Name - Device
 * Description  - BarcodeReader
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 * 2.70.2        08-Aug-2019     Deeksha             Modified to add logger methods.
 ********************************************************************************************/

namespace Semnox.Parafait.Device
{
    public class BarcodeReader
    {
        // used to trigger code in calling program to receive serial port data
        // set calling program's function to setReceiveAction
        // set RequiredByOthers = true in order to trigger calling program's proc when serial port receives any data
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static bool BarcodeScanned = false;
        public static string Barcode = "";

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
