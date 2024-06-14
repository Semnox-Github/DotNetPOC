using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parafait_POS
{
    public class OrderEventArgs
    {
        private int transactionId;

        public OrderEventArgs(int transactionId)
        {
            this.transactionId = transactionId;
        }

        public int TransactionId
        {
            get
            {
                return transactionId;
            }
        }
    }
}
