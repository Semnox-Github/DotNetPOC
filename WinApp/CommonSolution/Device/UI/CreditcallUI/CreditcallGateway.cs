using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway.CreditcallUI
{
    /// <summary>
    /// CreditCall Gateway
    /// </summary>
    public class CreditcallGateway
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// static string variables - trxStatus, authCode, returnedMessage, receiptText, referenceNo
        /// </summary>
        public static string trxStatus, authCode, returnedMessage, receiptText, referenceNo;
        /// <summary>
        /// Receipt copies
        /// </summary>
        public static string customerCopy, merchantCopy;
        /// <summary>
        /// static string variable - status
        /// </summary>
        public static string status;
        /// <summary>
        /// bool unAttendedCC
        /// </summary>
        public static bool unAttendedCC = false;

        /// <summary>
        /// Makes Payment
        /// </summary>
        /// <param name="utils"></param>
        /// <param name="amt"></param>
        /// <param name="refNo"></param>
        /// <param name="transType"></param>
        /// <param name="batchRef"></param>
        /// <param name="salesRef"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public bool doTransaction(Utilities utils,string amt, int refNo, string transType, string batchRef,string salesRef="", string langCode = "eng")
        {
            log.LogMethodEntry(utils, amt, refNo, transType, batchRef, salesRef, langCode);
            bool retValue = false;
            frmUI ccui = new frmUI(utils,amt, refNo, transType, batchRef,salesRef, langCode);
            if(ccui.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                retValue = true;                                
            }
            else
            {
                retValue = false;
            }
            ccui.chipDnaClientHelper.Dispose();
            ccui.Close();
            ccui.Dispose();
            log.LogMethodExit(retValue);
            return retValue;
        }
    }
}
