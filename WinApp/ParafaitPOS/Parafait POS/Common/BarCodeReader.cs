/********************************************************************************************
 * Project Name - Common
 * Description  -  Class for BarCodeReader
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.80         20-Aug-2019     Girish Kundar  Modified : Removed unused namespace's and Added logger methods. 
 ********************************************************************************************/

namespace Parafait_POS
{
    static class BarCodeReader
    {
        public delegate void InvokeHandle(string scannedBarcode);
        public static bool BarCodeReaderFound = false;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static InvokeHandle receiveAction;

        public static void HandleTask(string scannedBarcode)
        {
            log.LogMethodEntry(scannedBarcode);
            if (receiveAction != null)
                receiveAction.Invoke(scannedBarcode);
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
