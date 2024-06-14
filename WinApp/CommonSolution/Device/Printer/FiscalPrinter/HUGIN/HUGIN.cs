using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Printer.FiscalPrint
{
    public class HUGIN : Semnox.Parafait.Device.Printer.FiscalPrint.FiscalPrinter
    {
        Utilities Utilities;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string FISCAL_PRINTER_FILE_FOLDER = "C:\\Temp";

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_utilities"></param>
        public HUGIN(Utilities _utilities) : base(_utilities)
        {
            Utilities = _utilities;
            FISCAL_PRINTER_FILE_FOLDER = Utilities.getParafaitDefaults("FISCAL_PRINTER_FILE_FOLDER");
        }

        /// <summary>
        /// Printing receipt
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
            try
            {
                string data = Utilities.executeScalar("exec SP_CustomHuginCashRegisterData @TrxId",
                                                        SQLTrx,
                                                        new SqlParameter("@TrxId", TrxId)).ToString();
                System.IO.File.AppendAllText(FISCAL_PRINTER_FILE_FOLDER + "\\HUGIN" + TrxId.ToString() + ".DAT", data);
            }
            catch (Exception ex)
            {
                Message = "Hugin Fiscal Printer" + ex.Message;
                return false;
            }
            return true;
        }
    }
}
