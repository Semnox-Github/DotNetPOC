/********************************************************************************************
 * Project Name - KioskCore  
 * Description  - DispenserCardReader.cs
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.80        3-Sep-2019       Deeksha            Added logger methods.
 ********************************************************************************************/

namespace Semnox.Parafait.KioskCore.CardDispenser
{
    public static class DispenserCardReader
    {
        // used to trigger code in calling program to receive serial port data
        // set calling program's function to setReceiveAction
        // set RequiredByOthers = true in order to trigger calling program's proc when serial port receives any data
         
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public delegate void InvokeHandle(string CardNumber);

        static InvokeHandle receiveAction;

        public static void handleCardRead(string CardNumber)
        {
            log.LogMethodEntry(CardNumber);
            if (receiveAction != null)
                receiveAction.Invoke(CardNumber);
            log.LogMethodExit(); ;
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
