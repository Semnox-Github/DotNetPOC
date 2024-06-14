using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    public class OrderEventHandler
    {
        private event EventHandler<OrderEventArgs> orderSelectedEvent;
        private readonly object orderSelectedEventLock = new object();
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public event EventHandler<OrderEventArgs> OrderSelectedEvent
        {
            add
            {
                lock (orderSelectedEventLock)
                {
                    orderSelectedEvent += value;
                }
            }
            remove
            {
                lock (orderSelectedEventLock)
                {
                    orderSelectedEvent -= value;
                }
            }
        }

        public void RaiseOrderSelectedEvent(int transactionId)
        {
            log.LogMethodEntry(transactionId);
            if (transactionId >= 0)
            {
                OrderEventArgs orderSelectedEventArgs = new OrderEventArgs(transactionId);
                if (orderSelectedEvent != null)
                {
                    orderSelectedEvent(this, orderSelectedEventArgs);
                }
            }
            log.LogMethodExit();
        }
    }
}
