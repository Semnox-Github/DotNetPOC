using AxRCHGlobe;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    public class RCHGlobe : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        Utilities Utilities;
        AxRchEcrGlobe FiscalPrinterObject;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_utilities"></param>
        public RCHGlobe(Utilities _utilities) : base(_utilities)
        {
            Utilities = _utilities;
            FiscalPrinterObject = new AxRchEcrGlobe();
            FiscalPrinterObject.CreateControl();
        }
        public override bool OpenPort()
        {
            log.LogMethodEntry();
            string comPort = Utilities.getParafaitDefaults("FISCAL_PRINTER_PORT_NUMBER");
            int ret = FiscalPrinterObject.RCHOpen(ref comPort);
            if (ret != 0)
            {
                log.LogMethodExit(false);
                return false;
            }

            FiscalPrinterObject.AsyncMode = false;
            FiscalPrinterObject.ClearError();
            log.LogMethodExit(true);
            return true;
        }

        /// <summary>
        /// Print Receipt
        /// </summary>
        /// <param name="TrxId"></param>
        /// <param name="Message"></param>
        /// <param name="SQLTrx"></param>
        /// <param name="TenderedCash"></param>
        /// <param name="isFiscal"></param>
        /// <param name="trxReprint"></param>
        /// <returns></returns>
        public override bool PrintReceipt(int TrxId, ref string Message, SqlTransaction SQLTrx = null, decimal TenderedCash = 0, bool isFiscal = true, bool trxReprint = false)
        {
            log.LogMethodEntry(TrxId, Message, SQLTrx, TenderedCash, isFiscal, trxReprint);
            log.LogMethodEntry(TrxId, TenderedCash, SQLTrx, SQLTrx);
            bool header = true;
            string description = string.Empty;
            decimal price = 0;
            decimal amount = 0;
            decimal TotalAmount = 0;
            int quantity = 0;
            int vatInfo = 0;
            int unitPrice = 0;
            string unitName = "";

            SqlCommand cmdgetTrxDetails = Utilities.getCommand(SQLTrx);
            cmdgetTrxDetails.CommandText = "select t.amount, t.quantity, p.product_name, t.card_number from trx_lines t inner join products p on t.product_id=p.product_id where t.trxid=@trxid";
            cmdgetTrxDetails.Parameters.AddWithValue("@trxid", TrxId);
            SqlDataAdapter da = new SqlDataAdapter(cmdgetTrxDetails);
            DataTable dt = new DataTable();
            da.Fill(dt);

            FiscalPrinterObject.ClearError();
            FiscalPrinterObject.BeginFiscalReceipt(ref header);
            //setHeader();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                amount = Convert.ToDecimal(dt.Rows[i]["amount"]);
                TotalAmount += amount;
                quantity = Convert.ToInt32(dt.Rows[i]["quantity"]);
                price = amount / quantity;
                description = dt.Rows[i]["product_name"].ToString();
                if (description.Length > 12)
                    description = description.Substring(0, 12);

                int status = 0;
                if (amount > 0)
                    status = FiscalPrinterObject.PrintRecItem(ref description, ref price, ref quantity, ref vatInfo, ref unitPrice, ref unitName);
                else
                {
                    amount = amount * -1;
                    status = FiscalPrinterObject.PrintRecVoidItem(ref description, ref amount, ref quantity);
                }

                if (status != 0)
                {
                    log.LogMethodExit(false);
                    return false;
                }

            }

            FiscalPrinterObject.PrintRecSubtotal();
            string paymentDesc = "CASH";
            if (FiscalPrinterObject.PrintRecTotal(ref TotalAmount, ref TenderedCash, ref paymentDesc) != 0)
                FiscalPrinterObject.ClearError();
            FiscalPrinterObject.EndFiscalReceipt(ref header);
            log.LogMethodExit(true);
            return true;
        }

        void setHeaderRCHGlobe()
        {
            log.LogMethodEntry();
            bool doublewidth = true;
            int lineNumber1 = 1;
            string contentLine1 = "Fiscal Printer";
            FiscalPrinterObject.SetHeaderLine(ref lineNumber1, ref contentLine1, ref doublewidth);
            lineNumber1++;
            contentLine1 = "Testing Header";
            int status = FiscalPrinterObject.SetHeaderLine(ref lineNumber1, ref contentLine1, ref doublewidth);
            log.LogMethodExit(null);
        }

        public override void ClosePort()
        {
            log.LogMethodEntry();
            FiscalPrinterObject.ClearError();
            FiscalPrinterObject.RCHClose();
            log.LogMethodExit(null);
        }
    }
}