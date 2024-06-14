using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace Semnox.Parafait.Device.PaymentGateway
{
    internal class ClasResponseValidator
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ClsResponseMessageAttributes ValidateIncomingvoidsaleMessage(ClsRequestMessageAttributes myreqmsgattributes, string responsexmlmsg)
        {
            log.LogMethodEntry(myreqmsgattributes, responsexmlmsg);

            enumCmdResponse CmdStatus;
            XDocument xdoc = XDocument.Parse(responsexmlmsg);
            List<ClsRequestMessageAttributes> objResMsgAtt;
            ClsResponseMessageAttributes objRetResMsgAtt = null;

            objResMsgAtt = (from childelements in xdoc.Descendants("CmdResponse")
                            select new ClsRequestMessageAttributes
                            {
                                CmdStatus = childelements.Element("CmdStatus").Value,
                            }).ToList<ClsRequestMessageAttributes>();

            CmdStatus = (enumCmdResponse)Enum.Parse(typeof(enumCmdResponse), objResMsgAtt[0].CmdStatus);

            if (CmdStatus != enumCmdResponse.Error)
            {
                objRetResMsgAtt = ValidateResponseMessageVoidSale(responsexmlmsg);
            }
            else
            {
                objRetResMsgAtt = ValidateResponseMessageError(responsexmlmsg);
                objRetResMsgAtt.InvoiceNo = myreqmsgattributes.InvoiceNo;
                objRetResMsgAtt.TranCode = myreqmsgattributes.TranCode;
            }

            log.LogMethodExit(objRetResMsgAtt);
            return objRetResMsgAtt;

        }
        public ClsResponseMessageAttributes ValidateIncomingResponseMessage(ClsRequestMessageAttributes myreqmsgattributes, string responsexmlmsg)
        {
            log.LogMethodEntry(myreqmsgattributes, responsexmlmsg);

            enumCmdResponse CmdStatus;
            XDocument xdoc = XDocument.Parse(responsexmlmsg);
            List<ClsResponseMessageAttributes> objResMsgAtt;
            ClsResponseMessageAttributes objRetResMsgAtt;

            objResMsgAtt = (from childelements in xdoc.Descendants("CmdResponse")
                            select new ClsResponseMessageAttributes
                            {
                                CmdStatus = childelements.Element("CmdStatus").Value,
                            }).ToList<ClsResponseMessageAttributes>();

            CmdStatus = (enumCmdResponse)Enum.Parse(typeof(enumCmdResponse), objResMsgAtt[0].CmdStatus);

            if (CmdStatus != enumCmdResponse.Error && CmdStatus != enumCmdResponse.Declined)
            {
                if (myreqmsgattributes.TranType == CardType.Credit)
                {
                    objRetResMsgAtt = ValidateResponseMessageCredit(responsexmlmsg);
                }
                else
                {
                    objRetResMsgAtt = ValidateResponseMessageDebit(responsexmlmsg);
                }
            }
            else if (CmdStatus == enumCmdResponse.Declined)
            {
                objRetResMsgAtt = ValidateDeclinedCreditResponseMessage(responsexmlmsg);
                objRetResMsgAtt.TranCode = myreqmsgattributes.TranCode;
            }
            else
            {
                objRetResMsgAtt = ValidateResponseMessageError(responsexmlmsg);
                objRetResMsgAtt.InvoiceNo = myreqmsgattributes.InvoiceNo;
                objRetResMsgAtt.TranCode = myreqmsgattributes.TranCode;
            }

            log.LogMethodExit(objRetResMsgAtt);
            return objRetResMsgAtt;
        }

        public ClsResponseMessageAttributes ValidateResponseMessageCredit(string xmlstring)
        {
            log.LogMethodEntry(xmlstring);

            XDocument xdoc = XDocument.Parse(xmlstring);
            List<ClsResponseMessageAttributes> objResMsgAtt1;
            List<ClsResponseMessageAttributes> objResMsgAtt2;
            ClsResponseMessageAttributes objRetResMsgAtt;

            objResMsgAtt1 = (from childelements in xdoc.Descendants("CmdResponse")
                             select new ClsResponseMessageAttributes
                            {
                                ResponseOrigin = childelements.Element("ResponseOrigin").Value,
                                DSIXReturnCode = childelements.Element("DSIXReturnCode").Value,
                                CmdStatus = childelements.Element("CmdStatus").Value,
                                TextResponse = childelements.Element("TextResponse").Value,
                                UserTraceData = childelements.Element("UserTraceData").Value
                            }).ToList<ClsResponseMessageAttributes>();

            objResMsgAtt2 = (from childelements in xdoc.Descendants("TranResponse")
                             select new ClsResponseMessageAttributes
                             {
                                 MerchantID = childelements.Element("MerchantID").Value,
                                 AcctNo = childelements.Element("AcctNo").Value,
                                 CardType = childelements.Element("CardType").Value,
                                 TranCode = childelements.Element("TranCode").Value,
                                 AuthCode = childelements.Element("AuthCode") != null ? childelements.Element("AuthCode").Value : string.Empty,
                                 CaptureStatus = childelements.Element("CaptureStatus").Value,
                                 RefNo = childelements.Element("RefNo").Value,
                                 InvoiceNo = childelements.Element("InvoiceNo").Value,
                                 Memo = childelements.Element("Memo").Value,
                                 AcqRefData = childelements.Element("AcqRefData") != null ? childelements.Element("AcqRefData").Value : string.Empty,
                                 RecordNo = childelements.Element("RecordNo").Value,
                                 ProcessData = childelements.Element("ProcessData") != null ? childelements.Element("ProcessData").Value : string.Empty,
                                 Purchase = childelements.Element("Amount").Element("Purchase").Value,
                                 Authorize = childelements.Element("Amount").Element("Authorize").Value,//Begin Modification on 09-Nov-2015:Tip feature

                                 Gratuity = (childelements.Element("Amount").Element("Gratuity") != null) ? childelements.Element("Amount").Element("Gratuity").Value : null//End Modification on 09-Nov-2015:Tip feature

                             }).ToList<ClsResponseMessageAttributes>();

            objRetResMsgAtt = objResMsgAtt2[0];
            objRetResMsgAtt.ResponseOrigin = objResMsgAtt1[0].ResponseOrigin;
            objRetResMsgAtt.DSIXReturnCode = objResMsgAtt1[0].DSIXReturnCode;
            objRetResMsgAtt.CmdStatus = objResMsgAtt1[0].CmdStatus;
            objRetResMsgAtt.TextResponse = objResMsgAtt1[0].TextResponse;
            objRetResMsgAtt.UserTraceData = objResMsgAtt1[0].UserTraceData;

            log.LogMethodExit(objRetResMsgAtt);
            return objRetResMsgAtt;
        }

        public ClsResponseMessageAttributes ValidateDeclinedCreditResponseMessage(string xmlstring)
        {
            log.LogMethodEntry(xmlstring);

            XDocument xdoc = XDocument.Parse(xmlstring);
            List<ClsResponseMessageAttributes> objResMsgAtt1;
            List<ClsResponseMessageAttributes> objResMsgAtt2;
            ClsResponseMessageAttributes objRetResMsgAtt;

            objResMsgAtt1 = (from childelements in xdoc.Descendants("CmdResponse")
                             select new ClsResponseMessageAttributes
                             {
                                 ResponseOrigin = childelements.Element("ResponseOrigin").Value,
                                 DSIXReturnCode = childelements.Element("DSIXReturnCode").Value,
                                 CmdStatus = childelements.Element("CmdStatus").Value,
                                 TextResponse = childelements.Element("TextResponse").Value,
                                 UserTraceData = childelements.Element("UserTraceData").Value
                             }).ToList<ClsResponseMessageAttributes>();

            objResMsgAtt2 = (from childelements in xdoc.Descendants("TranResponse")
                             select new ClsResponseMessageAttributes
                             {
                                 MerchantID = childelements.Element("MerchantID").Value,
                                 AcctNo = childelements.Element("AcctNo").Value,
                                 CardType = childelements.Element("CardType").Value,
                                 TranCode = childelements.Element("TranCode").Value,
                                 //AuthCode = childelements.Element("AuthCode").Value,
                                 //CaptureStatus = childelements.Element("CaptureStatus").Value,
                                 RefNo = childelements.Element("RefNo").Value,
                                 InvoiceNo = childelements.Element("InvoiceNo").Value,
                                 Memo = childelements.Element("Memo").Value,
                                 AcqRefData = childelements.Element("AcqRefData") != null ? childelements.Element("AcqRefData").Value : string.Empty,
                                 RecordNo = childelements.Element("RecordNo") != null ? childelements.Element("RecordNo").Value : string.Empty,
                                 ProcessData = childelements.Element("ProcessData") != null ? childelements.Element("ProcessData").Value : string.Empty,
                                 Purchase = childelements.Element("Amount").Element("Purchase").Value,
                                 Authorize = childelements.Element("Amount").Element("Authorize").Value
                             }).ToList<ClsResponseMessageAttributes>();

            objRetResMsgAtt = objResMsgAtt2[0];
            objRetResMsgAtt.ResponseOrigin = objResMsgAtt1[0].ResponseOrigin;
            objRetResMsgAtt.DSIXReturnCode = objResMsgAtt1[0].DSIXReturnCode;
            objRetResMsgAtt.CmdStatus = objResMsgAtt1[0].CmdStatus;
            objRetResMsgAtt.TextResponse = objResMsgAtt1[0].TextResponse;
            objRetResMsgAtt.UserTraceData = objResMsgAtt1[0].UserTraceData;

            log.LogMethodExit(objRetResMsgAtt);
            return objRetResMsgAtt;
        }

        public ClsResponseMessageAttributes ValidateResponseMessageDebit(string xmlstring)
        {
            log.LogMethodEntry(xmlstring);

            XDocument xdoc = XDocument.Parse(xmlstring);
            List<ClsResponseMessageAttributes> objResMsgAtt1;
            List<ClsResponseMessageAttributes> objResMsgAtt2;
            ClsResponseMessageAttributes objRetResMsgAtt;

            objResMsgAtt1 = (from childelements in xdoc.Descendants("CmdResponse")
                             select new ClsResponseMessageAttributes
                             {
                                 ResponseOrigin = childelements.Element("ResponseOrigin").Value,
                                 DSIXReturnCode = childelements.Element("DSIXReturnCode").Value,
                                 CmdStatus = childelements.Element("CmdStatus").Value,
                                 TextResponse = childelements.Element("TextResponse").Value,
                                 UserTraceData = childelements.Element("UserTraceData").Value
                             }).ToList<ClsResponseMessageAttributes>();

            objResMsgAtt2 = (from childelements in xdoc.Descendants("TranResponse")
                             select new ClsResponseMessageAttributes
                             {
                                 MerchantID = childelements.Element("MerchantID").Value,
                                 AcctNo = childelements.Element("AcctNo").Value,
                                 CardType = childelements.Element("CardType").Value,
                                 TranCode = childelements.Element("TranCode").Value,
                                 RefNo = childelements.Element("RefNo").Value,
                                 InvoiceNo = childelements.Element("InvoiceNo").Value,
                                 Memo = childelements.Element("Memo").Value,
                                 Purchase = childelements.Element("Amount").Element("Purchase").Value,
                                 Authorize = childelements.Element("Amount").Element("Authorize").Value,//Tipamount:Modification for adding the tip amount
                                 Gratuity = (childelements.Element("Amount").Element("Gratuity") != null) ? childelements.Element("Amount").Element("Gratuity").Value : null//Tipamount:Ends 
                             }).ToList<ClsResponseMessageAttributes>();

            objRetResMsgAtt = objResMsgAtt2[0];
            objRetResMsgAtt.ResponseOrigin = objResMsgAtt1[0].ResponseOrigin;
            objRetResMsgAtt.DSIXReturnCode = objResMsgAtt1[0].DSIXReturnCode;
            objRetResMsgAtt.CmdStatus = objResMsgAtt1[0].CmdStatus;
            objRetResMsgAtt.TextResponse = objResMsgAtt1[0].TextResponse;
            objRetResMsgAtt.UserTraceData = objResMsgAtt1[0].UserTraceData;
            objRetResMsgAtt.RecordNo = "N"; // If the cmdstatus is "Errro" we are setting RecordNo "N" because of Not null in DB

            log.LogMethodExit(objRetResMsgAtt);
            return objRetResMsgAtt;
        }

        public ClsResponseMessageAttributes ValidateResponseMessageVoidSale(string xmlstring)
        {
            log.LogMethodEntry(xmlstring);

            XDocument xdoc = XDocument.Parse(xmlstring);
            List<ClsResponseMessageAttributes> objResMsgAtt1;
            List<ClsResponseMessageAttributes> objResMsgAtt2;
            ClsResponseMessageAttributes objRetResMsgAtt;

            objResMsgAtt1 = (from childelements in xdoc.Descendants("CmdResponse")
                             select new ClsResponseMessageAttributes
                             {
                                 ResponseOrigin = childelements.Element("ResponseOrigin").Value,
                                 DSIXReturnCode = childelements.Element("DSIXReturnCode").Value,
                                 CmdStatus = childelements.Element("CmdStatus").Value,
                                 TextResponse = childelements.Element("TextResponse").Value,
                                 UserTraceData = childelements.Element("UserTraceData").Value
                             }).ToList<ClsResponseMessageAttributes>();

            objResMsgAtt2 = (from childelements in xdoc.Descendants("TranResponse")
                             select new ClsResponseMessageAttributes
                             {
                                 MerchantID = childelements.Element("MerchantID").Value,
                                 AcctNo = childelements.Element("AcctNo").Value,
                                 CardType = childelements.Element("CardType").Value,
                                 TranCode = childelements.Element("TranCode").Value,
                                 AuthCode = childelements.Element("AuthCode").Value,
                                 CaptureStatus = childelements.Element("CaptureStatus") != null ? childelements.Element("CaptureStatus").Value : string.Empty,
                                 RefNo = childelements.Element("RefNo").Value,
                                 InvoiceNo = childelements.Element("InvoiceNo").Value,
                                 Memo = childelements.Element("Memo").Value,
                                 AcqRefData = childelements.Element("AcqRefData") != null ? childelements.Element("AcqRefData").Value : string.Empty,
                                 RecordNo = childelements.Element("RecordNo").Value,
                                 Purchase = childelements.Element("Amount").Element("Purchase").Value,
                                 Authorize = childelements.Element("Amount").Element("Authorize").Value
                             }).ToList<ClsResponseMessageAttributes>();

            objRetResMsgAtt = objResMsgAtt2[0];
            objRetResMsgAtt.ResponseOrigin = objResMsgAtt1[0].ResponseOrigin;
            objRetResMsgAtt.DSIXReturnCode = objResMsgAtt1[0].DSIXReturnCode;
            objRetResMsgAtt.CmdStatus = objResMsgAtt1[0].CmdStatus;
            objRetResMsgAtt.TextResponse = objResMsgAtt1[0].TextResponse;
            objRetResMsgAtt.UserTraceData = objResMsgAtt1[0].UserTraceData;

            log.LogMethodExit(objRetResMsgAtt); 
            return objRetResMsgAtt;
        }

        public ClsResponseMessageAttributes ValidateResponseMessageError(string xmlstring)
        {
            log.LogMethodEntry(xmlstring);

            XDocument xdoc = XDocument.Parse(xmlstring);
            List<ClsResponseMessageAttributes> objResMsgAtt;
            ClsResponseMessageAttributes objRetResMsgAtt;

            objResMsgAtt = (from childelements in xdoc.Descendants("CmdResponse")
                            select new ClsResponseMessageAttributes
                            {
                                ResponseOrigin = childelements.Element("ResponseOrigin").Value,
                                DSIXReturnCode = childelements.Element("DSIXReturnCode").Value,
                                CmdStatus = childelements.Element("CmdStatus").Value,
                                TextResponse = childelements.Element("TextResponse").Value,
                                UserTraceData = childelements.Element("UserTraceData").Value
                            }).ToList<ClsResponseMessageAttributes>();
            objRetResMsgAtt = objResMsgAtt[0];
            objRetResMsgAtt.RecordNo = "N"; // If the cmdstatus is "Errro" we are setting RecordNo "N" because of Not null in DB

            log.LogMethodExit(objRetResMsgAtt);
            return objRetResMsgAtt;
        }

        public void Method()
        {
            log.LogMethodEntry();
            log.LogMethodExit(null, "Throwing NotImplementedException");
            throw new System.NotImplementedException();
        }
    }
}
