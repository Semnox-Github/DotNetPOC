using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// PinPad Response Attributes Class
    /// </summary>
    public class PinPadResponseAttributes
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// string CardReadStatus
        /// </summary>
        public string CardReadStatus;
        /// <summary>
        /// string MaskedCardNumber
        /// </summary>
        public string MaskedCardNumber;
        /// <summary>
        /// string ExpirationDate
        /// </summary>
        public string ExpirationDate;
        /// <summary>
        /// string Track1
        /// </summary>
        public string Track1;
        /// <summary>
        /// string Track2
        /// </summary>
        public string Track2;
        /// <summary>
        /// string Track3
        /// </summary>
        public string Track3;
        /// <summary>
        /// string CardSource
        /// </summary>
        public string CardSource;
        /// <summary>
        /// string PinEntryStatus
        /// </summary>
        public string PinEntryStatus;
        /// <summary>
        /// string PinData
        /// </summary>
        public string PinData;
        /// <summary>
        /// string ReasonCode
        /// </summary>
        public string ReasonCode;
        /// <summary>
        /// string DeviceStatus
        /// </summary>
        public string DeviceStatus;
        /// <summary>
        /// string DisplayText
        /// </summary>
        public string DisplayText;
        /// <summary>
        ///  int CCResponseId
        /// </summary>
        public int CCResponseId;
        /// <summary>
        /// string DeviceSerialNumber
        /// </summary>
        public string DeviceSerialNumber;
    }
}
