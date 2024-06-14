using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal enum TransactionType
    {
        INIT,
        GET_CONFIG,
        SET_CONFIG,
        SALE,
        VOID,
        REFUND,
        PRE_AUTH,
        BATCH_CLOSE,
        IND_REFUND,
        COMPLETION
    }
    public enum PaymentGatewayTransactionType
    {
        TATokenRequest,
        SALE,
        REFUND,
        AUTHORIZATION,
        VOID,
        CAPTURE,
        PARING,
        TIPADJUST,
        USERCANCEL,
        STATUSCHECK
    }
}
