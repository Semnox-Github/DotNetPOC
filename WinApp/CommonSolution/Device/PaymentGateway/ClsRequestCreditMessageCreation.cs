using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class ClsRequestCreditMessageCreation : IRequestCreateMessage
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string CreaterequestMessage(ClsRequestMessageAttributes clsrequestattributes)
        {
            log.LogMethodEntry(clsrequestattributes);

            string xmlns = @"<?xml version=""1.0""?>";
            XDocument xdoc = new XDocument();
            XElement myxelement;
             if (clsrequestattributes.TranCode == "Return") // no partialAuth required for return
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
                                                          ("Account",
                                                              new XElement("EncryptedFormat", clsrequestattributes.EncryptedFormat),
                                                              new XElement("AccountSource", clsrequestattributes.AccountSource),
                                                              new XElement("EncryptedBlock", clsrequestattributes.EncryptedBlock), //"F40DDBA1F645CC8DB85A6459D45AFF8002C244A0F74402B479ABC9915EC9567C81BE99CE4483AF3D"),
                                                              new XElement("EncryptedKey", clsrequestattributes.EncryptedKey)//"9012090B01C4F200002B")
                                                          ),
                                                     new XElement
                                                          ("Amount",
                                                             new XElement("Purchase", clsrequestattributes.Purchase)
                                                          ),
                                                     new XElement("OperatorID", clsrequestattributes.OperatorID)
                                   )
                          );
                }
                else
                {
                    if (string.IsNullOrEmpty(clsrequestattributes.AuthCode))//Begin Modification on 09-Nov-2015:Tip Feature
                    {//End Modification on 09-Nov-2015:Tip Feature
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
                                                     new XElement("PartialAuth", clsrequestattributes.PartialAuth),
                                                     new XElement
                                                          ("Account",
                                                              new XElement("EncryptedFormat", clsrequestattributes.EncryptedFormat),
                                                              new XElement("AccountSource", clsrequestattributes.AccountSource),
                                                              new XElement("EncryptedBlock", clsrequestattributes.EncryptedBlock), //"F40DDBA1F645CC8DB85A6459D45AFF8002C244A0F74402B479ABC9915EC9567C81BE99CE4483AF3D"),
                                                              new XElement("EncryptedKey", clsrequestattributes.EncryptedKey)//"9012090B01C4F200002B")
                                                          ),
                                                     new XElement
                                                          ("Amount",
                                                             new XElement("Purchase", clsrequestattributes.Purchase),//Begin Modification on 09-Nov-2015:Tip Feature
                                                                  (!clsrequestattributes.TipAmount.Equals("0.00")) ? new XElement("Authorize", clsrequestattributes.Purchase) : null,
                                                                  (!clsrequestattributes.TipAmount.Equals("0.00")) ? new XElement("Gratuity", clsrequestattributes.TipAmount) : null//End Modification on 09-Nov-2015:Tip Feature
                                                          ),
                                                     new XElement("OperatorID", clsrequestattributes.OperatorID)
                                   )
                          );
                    }
                    else//Begin Modification on 09-Nov-2015:Tip Feature
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
                                                     new XElement("PartialAuth", clsrequestattributes.PartialAuth),

                                                     new XElement
                                                          ("Amount",
                                                             new XElement("Purchase", clsrequestattributes.Purchase),
                                                                  (!clsrequestattributes.TipAmount.Equals("0.00")) ? new XElement("Authorize", clsrequestattributes.Purchase) : null,
                                                                  (!clsrequestattributes.TipAmount.Equals("0.00")) ? new XElement("Gratuity", clsrequestattributes.TipAmount) : null
                                                          ),
                                                          new XElement
                                                              ("TranInfo",
                                                                  new XElement("AuthCode", clsrequestattributes.AuthCode),
                                                                   new XElement("AcqRefData", clsrequestattributes.AcqRefData)
                                                              ),
                                                     new XElement("OperatorID", clsrequestattributes.OperatorID)
                                        )
                          );
                    }//End Modification on 09-Nov-2015:Tip Feature
                }
                xdoc.Add(myxelement);

            log.LogVariableState("clsrequestattributes", clsrequestattributes);
            string returnValueNew = xmlns + xdoc.ToString();

            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }
    }
}