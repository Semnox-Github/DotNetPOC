/*===========================================================================================
 * 
 *  Copyright (C)   : Advanced Card System Ltd
 * 
 *  File            : CardException.cs
 * 
 *  Description     : Contains Methods and Properties for smart card related exceptions
 * 
 *  Author          : Arturo Salvamante
 *  
 *  Date            : June 03, 2011
 * 
 *  Revision Traile : [Author] / [Date if modification] / [Details of Modifications done] 
  =========================================================================================
 Modified to add Logger Methods by Deeksha on 08-Aug-2019
 * =========================================================================================*/

using System;

namespace Semnox.Parafait.Device
{
    internal class CardException : Exception
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected byte[] _statusWord;
        public byte[] statusWord
        {
            get { return _statusWord; }
        }

        protected string _message;
        public override string Message
        {
            get { return _message; }
        }

        public CardException(string message, byte[] sw)
        {
            log.LogMethodEntry(message, sw);
            _message = message;
            _statusWord = sw;
            log.LogMethodExit();
        }
    }
}
