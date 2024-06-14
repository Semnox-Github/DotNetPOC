using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class ClsRequestDebitMessageCreation : IRequestCreateMessage
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string CreaterequestMessage(ClsRequestMessageAttributes clsrequestattributes)
        {
            log.LogMethodEntry(clsrequestattributes);

            string xmlns = @"<?xml version=""1.0""?>";
            XDocument xdoc = new XDocument();
             XElement myxelement;//Begin Modification on 09-Nov-2015: Tip Feature
                 if (string.IsNullOrEmpty(clsrequestattributes.AuthCode))
                 {
                     myxelement = new XElement//End Modification on 09-Nov-2015: Tip Feature
                       (
                          "TStream", new XElement

                               (
                                   "Transaction", new XElement("MerchantID", clsrequestattributes.MerchantID),
                                                  new XElement("TranType", clsrequestattributes.TranType),
                                                  new XElement("TranCode", clsrequestattributes.TranCode),
                                                  new XElement("InvoiceNo", clsrequestattributes.InvoiceNo),
                                                  new XElement("RefNo", clsrequestattributes.RefNo),
                                                  new XElement("Frequency", clsrequestattributes.Frequency),
                                                  new XElement("RecordNo", clsrequestattributes.RecordNo),
                                                  new XElement("Memo", clsrequestattributes.Memo),
                                                  new XElement
                                                       ("Account",
                                                           new XElement("EncryptedFormat", clsrequestattributes.EncryptedFormat),
                                                           new XElement("AccountSource", clsrequestattributes.AccountSource),
                                                           new XElement("EncryptedBlock", clsrequestattributes.EncryptedBlock),
                                                           new XElement("EncryptedKey", clsrequestattributes.EncryptedKey)
                                                       ),
                                                  new XElement
                                                       ("Amount",
                                                          new XElement("Purchase", clsrequestattributes.Purchase),//Tipamount:Modification for adding tip amount.
                                                               (!clsrequestattributes.TipAmount.Equals("0.00")) ? new XElement("Authorize", clsrequestattributes.Purchase) : null,
                                                               (!clsrequestattributes.TipAmount.Equals("0.00")) ? new XElement("Gratuity", clsrequestattributes.TipAmount) : null//Tipamount:Ends
                                                       ),
                                                  new XElement
                                                       ("PIN",
                                                          new XElement("PINBlock", clsrequestattributes.PINBlock),
                                                          new XElement("DervdKey", clsrequestattributes.DervdKey)
                                                       )
                               )
                       );
                 }//Begin Modification on 09-Nov-2015: Tip Feature
                 else
                 {
                     myxelement = new XElement
                       (
                          "TStream", new XElement

                               (
                                   "Transaction", new XElement("MerchantID", clsrequestattributes.MerchantID),
                                                  new XElement("TranType", clsrequestattributes.TranType),
                                                  new XElement("TranCode", clsrequestattributes.TranCode),
                                                  new XElement("InvoiceNo", clsrequestattributes.InvoiceNo),
                                                  new XElement("RefNo", clsrequestattributes.RefNo),
                                                  new XElement("Frequency", clsrequestattributes.Frequency),
                                                  new XElement("RecordNo", clsrequestattributes.RecordNo),
                                                  new XElement("Memo", clsrequestattributes.Memo),
                                                  new XElement
                                                       ("Amount",
                                                          new XElement("Purchase", clsrequestattributes.Purchase),//Tipamount:Modification for adding tip amount.
                                                               (!clsrequestattributes.TipAmount.Equals("0.00")) ? new XElement("Authorize", clsrequestattributes.Purchase) : null,
                                                               (!clsrequestattributes.TipAmount.Equals("0.00")) ? new XElement("Gratuity", clsrequestattributes.TipAmount) : null//Tipamount:Ends
                                                       ),
                                                       new XElement
                                                           ("TranInfo",
                                                               new XElement("AuthCode", clsrequestattributes.AuthCode),
                                                                new XElement("AcqRefData", clsrequestattributes.AcqRefData)
                                                           ),
                                                  new XElement("OperatorID", clsrequestattributes.OperatorID)
                                     )
                       );
                 }//Ends Modification on 09-Nov-2015: Tip Feature
                 xdoc.Add(myxelement);

            log.LogVariableState("clsrequestattributes", clsrequestattributes);

            string returnValueNew = xmlns + xdoc.ToString();
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }
    }
}
