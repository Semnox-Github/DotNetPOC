using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// 
    /// </summary>
    public enum eTransactionType : int
    {
        /// <summary>
        /// 
        /// </summary>
        PURCHASE = 1,
        /// <summary>
        /// 
        /// </summary>
        RETURN = 2,
        /// <summary>
        /// 
        /// </summary>
        FORCE = 3,
        /// <summary>
        /// 
        /// </summary>
        BALANCE_INQUIRY = 4,
        /// <summary>
        /// 
        /// </summary>
        PIN_CHANGE = 5,
        /// <summary>
        /// 
        /// </summary>
        VOID_LAST = 6,
        /// <summary>
        /// 
        /// </summary>
        VOID_BY_POST = 7,
        /// <summary>
        /// 
        /// </summary>
        VOID_BY_MTX = 8,
        /// <summary>
        /// 
        /// </summary>
        ACTIVATION = 9,
        /// <summary>
        /// 
        /// </summary>
        RECHARGE = 10,
        /// <summary>
        /// 
        /// </summary>
        DEACTIVATION = 11,
        /// <summary>
        /// 
        /// </summary>
        PRE_AUTH = 12,
        /// <summary>
        /// 
        /// </summary>
        PRE_AUTH_COMPLETION = 13,
        /// <summary>
        /// 
        /// </summary>
        PRE_ACTIVATION = 14,
        /// <summary>
        /// 
        /// </summary>
        VOUCHER_RETURN = 15,
        /// <summary>
        /// 
        /// </summary>
        REFRESH = 16,
        /// <summary>
        /// 
        /// </summary>
        PAYMENT_ON_ACCOUNT = 19,// Private Credit
        /// <summary>
        /// 
        /// </summary>
        RETURN_WITH_PAN_VALIDATION = 20,
        /// <summary>
        /// 
        /// </summary>
        REFUND_ACTIVATION = 21, // Gift Card
        /// <summary>
        /// 
        /// </summary>
        CASH_OUT = 22,          // Gift Card
        /// <summary>
        /// 
        /// </summary>
        PRE_RECHARGE = 23,      // Gift Card
        /// <summary>
        /// 
        /// </summary>
        KEY_EXCHANGE = 24,      // No Tender
        /// <summary>
        /// 
        /// </summary>
        RECEIPT_LOOKUP = 25     // No Tender
    }
}
