/********************************************************************************************
 * Project Name - Common
 * Description  -  Class for CardReader
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
    static class CardReader
    {
        // used to trigger code in calling program to receive serial port data
        // set calling program's function to setReceiveAction
        // set RequiredByOthers = true in order to trigger calling program's proc when serial port receives any data
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string lockObject = "";
        public static bool CardSwiped = false;

        public static bool RequiredByOthers = false;

        public delegate void InvokeHandle(string CardNumber);

        static InvokeHandle receiveAction;

        public static void OthersTasks(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            if (receiveAction != null)
                receiveAction.Invoke(CardNumber);
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
