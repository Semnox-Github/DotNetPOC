using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway.Menories
{
    internal partial class ftmTransactionType : Form
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TransactionType transactionType;
        //public double TipAmount;
        Utilities utilities;
        public ftmTransactionType(Utilities _Utilities, TransactionType trxnType)
        {
            log.LogMethodEntry(_Utilities, trxnType);

            InitializeComponent();
            utilities = _Utilities;
            utilities.setLanguage(this);
            if (trxnType.Equals(TransactionType.SALE))
            {
                btnPurchase.Enabled = btnPreauth.Enabled = true;
            }

            log.LogMethodExit(null);
        }

       

        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            this.DialogResult = DialogResult.Cancel;
            Close();

            log.LogMethodExit(null);
        }

        private void btnPreauth_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            
            transactionType = TransactionType.PRE_AUTH;
            this.DialogResult = DialogResult.OK;

            log.LogMethodExit(null);
        }

        

        private void btnPurchase_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            transactionType = TransactionType.SALE;
            this.DialogResult = DialogResult.OK;

            log.LogMethodExit(null);
        }
    }
}
