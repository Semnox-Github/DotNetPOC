using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway.Mada
{
    public class MadaRequest
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int? bPort;
        private decimal? purAmount;
        private decimal? refundAmount;
        private int printReciept;
        private string trxRrn;
        private DateTime originalRefundDate;
        private string ecrRefNumber;
        private bool isUnattended;
        private PaymentGatewayTransactionType trxType;
        private string terminalID;
        private string cashierNo;

        public MadaRequest(int? bPort, decimal? purAmount, decimal? refundAmount, int printReciept, string trxRrn, DateTime originalRefundDate,
                           string ecrRefNumber, bool isUnattended, PaymentGatewayTransactionType trxType, string terminalID, string cashierNo)
        {
            log.LogMethodEntry(bPort, purAmount, refundAmount, printReciept, trxRrn, originalRefundDate, ecrRefNumber, isUnattended, trxType, terminalID, cashierNo);
            this.bPort = bPort;
            this.purAmount = purAmount;
            this.refundAmount = refundAmount;
            this.printReciept = printReciept;
            this.trxRrn = trxRrn;
            this.originalRefundDate = originalRefundDate;
            this.ecrRefNumber = ecrRefNumber;
            this.isUnattended = isUnattended;
            this.trxType = trxType;
            this.terminalID = terminalID;
            this.cashierNo = cashierNo;
            log.LogMethodExit();
        }

        public int? BPort { get { return bPort; } set { bPort = value; } }
        public bool IsUnattended { get { return isUnattended; } set { isUnattended = value; } }
        public decimal? PurAmount { get { return purAmount; } set { purAmount = value; } }
        public decimal? RefundAmount { get { return refundAmount; } set { refundAmount = value; } }
        public int PrintReciept { get { return printReciept; } set { printReciept = value; } }
        public string TrxRrn { get { return trxRrn; } set { trxRrn = value; } }
        public DateTime OriginalRefundDate { get { return originalRefundDate; } set { originalRefundDate = value; } }
        public string EcrRefNumber { get { return ecrRefNumber; } set { ecrRefNumber = value; } }
        public string TerminalID { get { return terminalID; } set { terminalID = value; } }
        public string CashierNo { get { return cashierNo; } set { cashierNo = value; } }
        public PaymentGatewayTransactionType TrxType { get { return trxType; } set { trxType = value; } }
    }
}
