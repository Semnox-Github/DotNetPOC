/********************************************************************************************
 * Project Name - Device
 * Description  - Business Logic to create Fiskaltrust ReceiptRequest object
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By            Remarks          
 *********************************************************************************************
*2.110.0     22-Dec-2020      Girish Kundar           Created :FiscalTrust changes - Shift open/Close/PayIn/PayOut to be fiscalized
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.Printer.FiscalPrinter.fiskaltrust
{
   public static class FiscaltrustDefaults
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Dictionary<string, string> defaultDictionary = new Dictionary<string, string>
        {
            {"CASH_BOX_ID_KEY", "FISCAL_CASH_REGISTER_ID" },
            {"DEFAULT_RECEIPT_CASE","4919338172267102209"},
            {"ZERO_RECEIPT_CASE","4919338167972134914"},
            {"DEFAULT_CHARGEITEM_CASE","4919338167972134912"}, //unknown type of service for DE 7 Umsatz
            {"DEFAULT_PAYITEM_CASE","4919338167972134912"}, // unknown payment type for DE This is handled like a cash payment in national currency
            {"DEFAULT_PAYITEM_CASE_DESCRIPTION","Bar"},
            {"CASE_OPEN_RECEIPT","4919338172267102211"},
            {"SIGNATURE_TYPE","4919338167972134941"},
            {"CASH","4919338167972134913"},
            {"CHEQUE","4919338167972134915"},
            {"DEBITCARD","4919338167972134916"},
            {"CREDITCARD","4919338167972134917"},
            {"VOUCHER","4919338167972134925"},
            {"CASH_DESCRIPTION","Bar"},
            {"CHEQUE_DESCRIPTION","Unbar"},
            {"DEBITCARD_DESCRIPTION","ECKarte"},
            {"CREDITCARD_DESCRIPTION","Kreditkarte"},
            {"VOUCHER_DESCRIPTION","Keine"},
            {"FISCALIZATION_ERROR_MESSAGE","Elektronisches Aufzeichnungssystem ausgefallen"},
            {"INFO_ORDER_RECEIPT_CASE","4919338167972134928"},
            {"DOWNPAYMENT_CHARGEITEM_CASE","4919338167972135041"}, 
            {"PAYIN_OUT_CHARGEITEM_CASE","4919338167972135059"},
            {"DEPOSIT_CHARGEITEM_CASE","4919338167972135056"},
            {"ASYNC_FISKATRUST_FREQUENCY","60"},
            {"START_ASYNC_FISCALIZATION_GOLIVE_DATE",""},
            {"ZERO_TAX_CHARGEITEM_CASE","4919338167972134918"}, 
            {"DOWNPAYMENT_CHARGEITEM_FULL_CASE","4919338167972135049‬"},
            {"SEVEN_PERC_TAX_CHARGEITEM_CASE","4919338167972134914‬"},
            {"NINTEEN_PERC_TAX_CHARGEITEM_CASE","4919338167972134913‬"},
            {"DEPOSIT_RECEIPT_CASE","4919338172267102228"}
            };

        public static string GetFiscaltrustDefault(string defultName)
        {
            log.LogMethodEntry();
            string result = string.Empty;
            if(defaultDictionary.ContainsKey(defultName))
            {
                result =  defaultDictionary[defultName].ToString().Trim();
            }
            log.LogMethodExit(result);
            return result;
        }
    }
}
