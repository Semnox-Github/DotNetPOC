using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    [Serializable]
    public class NetEpayAccount
    {
        private string acctNo;

        public NetEpayAccount()
        {
            this.acctNo = null;
        }
        public NetEpayAccount(string acctNo)
        {
            this.acctNo = acctNo;
        }

        public string AcctNo
        {
            get
            {
                return acctNo;
            }
            set
            {
                acctNo = value;
            }
        }
    }
}