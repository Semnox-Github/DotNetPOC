using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Element Transaction Request Class
    /// </summary>
    public class ElementTransactionRequest
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// double TransactionAmount
        /// </summary>
        public double TransactionAmount;
        /// <summary>
        /// int TransactionId
        /// </summary>
        public int TransactionId;
        /// <summary>
        /// object OrigElementTransactionId
        /// </summary>
        public object OrigElementTransactionId;
        /// <summary>
        ///  double OrigTransactionAmount
        /// </summary>
        public double OrigTransactionAmount;
        /// <summary>
        /// string TransactionType
        /// </summary>
        public string TransactionType;
        /// <summary>
        /// bool DebitCardSale
        /// </summary>
        public bool DebitCardSale = false;
        /// <summary>
        /// bool PrintReceipt 
        /// </summary>
        public bool PrintReceipt = true;
    }
}
