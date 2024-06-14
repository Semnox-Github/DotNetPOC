/********************************************************************************************
 * Project Name - Inventory                                                                          
 * Description  - Barcode Reader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2       13-Aug-2019   Deeksha          Added logger methods.
 ********************************************************************************************/

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// BarcodeReader
    /// </summary>
    public static class BarcodeReader
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // used to trigger code in calling program to receive serial port data
        // set calling program's function to setReceiveAction
        // set RequiredByOthers = true in order to trigger calling program's proc when serial port receives any data

        /// <summary>
        /// BarcodeScanned
        /// </summary>
        public static bool BarcodeScanned = false;

        /// <summary>
        /// Barcode
        /// </summary>
        public static string Barcode = "";

        /// <summary>
        /// InvokeHandle
        /// </summary>
        public delegate void InvokeHandle();

        static InvokeHandle receiveAction;

        /// <summary>
        /// OthersTasks
        /// </summary>
        public static void OthersTasks()
        {
            log.LogMethodEntry();
            if (receiveAction != null)
                receiveAction.Invoke();
            log.LogMethodExit();
        }

        /// <summary>
        /// setReceiveAction
        /// </summary>
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
