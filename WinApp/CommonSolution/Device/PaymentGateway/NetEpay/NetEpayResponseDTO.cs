using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class RStream
    {
        CommandResponse cmdResponse;
        TransactionResponse tranResponse;
        NetEpayBatchReportTransaction batchReportTransaction;

        public CommandResponse CmdResponse
        {
            get
            {
                return cmdResponse;
            }
            set
            {
                cmdResponse = value;
            }
        }
        public TransactionResponse TranResponse
        {
            get
            {
                return tranResponse;
            }
            set
            {
                tranResponse = value;
            }
        }
        public NetEpayBatchReportTransaction BatchReportTransaction
        {
            get
            {
                return batchReportTransaction;
            }
            set
            {
                batchReportTransaction = value;
            }
        }
    }
}
