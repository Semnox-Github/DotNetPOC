/*===========================================================================================
 * 
 *  Copyright (C)   : Advanced Card System Ltd
 * 
 *  File            : PcscException.cs
 * 
 *  Description     : Contain methods and properties related pcsc exceptions
 * 
 *  Author          : Arturo Salvamante
 *  
 *  Date            : October 19, 2011
 * 
 *  Revision Traile : [Author] / [Date if modification] / [Details of Modifications done] 
 * =========================================================================================
  Modified to add Logger Methods by Deeksha on 08-Aug-2019
 * =========================================================================================*/


namespace Semnox.Parafait.Device
{
    internal class PcscException : System.Exception
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int _errorCode;
        public int errorCode
        {
            get { return _errorCode; }
        }

        private string _message;
        public override string Message
        {
            get { return _message; }
        }

        public PcscException(int errCode)
        {
            log.LogMethodEntry(errCode);
            _errorCode = errCode;
            _message = PcscProvider.GetScardErrMsg(errCode);
            log.LogMethodExit();
        }

        public PcscException(string message)
        {
            log.LogMethodEntry(message);
            _message = message;
            log.LogMethodExit();
        }
    }
}
