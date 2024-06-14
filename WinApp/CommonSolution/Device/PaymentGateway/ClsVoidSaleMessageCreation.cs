using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class ClsVoidSaleMessageCreation
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string CreateVoidSale(ClsRequestMessageAttributes clsvoidsameattributes, bool Reversal)
        {
            log.LogMethodEntry(clsvoidsameattributes, Reversal);

            string xmlns = @"<?xml version=""1.0""?>";

            XDocument xdoc = new XDocument();

            XElement myxelement;
            if (Reversal)
            {
                myxelement = new XElement
                      (
                         "TStream", new XElement
                              (
                                  "Transaction", new XElement("MerchantID", clsvoidsameattributes.MerchantID),
                                                 new XElement("TranType", clsvoidsameattributes.TranType),
                                                 new XElement("TranCode", clsvoidsameattributes.TranCode),
                                                 new XElement("InvoiceNo", clsvoidsameattributes.InvoiceNo),
                                                 new XElement("RefNo", clsvoidsameattributes.RefNo),
                                                 new XElement("Memo", clsvoidsameattributes.Memo),
                                                 new XElement("RecordNo", clsvoidsameattributes.RecordNo),
                                                 new XElement("Frequency", clsvoidsameattributes.Frequency),
                                                 new XElement
                                                      ("Amount",
                                                         new XElement("Purchase", clsvoidsameattributes.Purchase)
                                                      ),
                                                 new XElement
                                                      ("TranInfo",
                                                          new XElement("AuthCode", clsvoidsameattributes.AuthCode)
                                                      ),
                                                 new XElement("TerminalName", clsvoidsameattributes.TerminalName),
                                                 new XElement("ShiftID", clsvoidsameattributes.ShiftID),
                                                 new XElement("OperatorID", clsvoidsameattributes.OperatorID),
                                                 new XElement("AcqRefData", clsvoidsameattributes.AcqRefData),
                                                 new XElement("ProcessData", clsvoidsameattributes.ProcessData)
                               )
                      );
            }
            else
            {
                myxelement = new XElement
                  (
                     "TStream", new XElement
                          (
                              "Transaction", new XElement("MerchantID", clsvoidsameattributes.MerchantID),
                                             new XElement("TranType", clsvoidsameattributes.TranType),
                                             new XElement("TranCode", clsvoidsameattributes.TranCode),
                                             new XElement("InvoiceNo", clsvoidsameattributes.InvoiceNo),
                                             new XElement("RefNo", clsvoidsameattributes.RefNo),
                                             new XElement("Memo", clsvoidsameattributes.Memo),
                                             new XElement("RecordNo", clsvoidsameattributes.RecordNo),
                                             new XElement("Frequency", clsvoidsameattributes.Frequency),
                                             new XElement
                                                  ("Amount",
                                                     new XElement("Purchase", clsvoidsameattributes.Purchase)
                                                  ),
                                             new XElement
                                                  ("TranInfo",
                                                      new XElement("AuthCode", clsvoidsameattributes.AuthCode)
                                                  ),
                                             new XElement("TerminalName", clsvoidsameattributes.TerminalName),
                                             new XElement("ShiftID", clsvoidsameattributes.ShiftID),
                                             new XElement("OperatorID", clsvoidsameattributes.OperatorID)
                           )
                  );
            }

            xdoc.Add(myxelement);

            string returnValueNew = xmlns + xdoc.ToString();
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }
    }
}
