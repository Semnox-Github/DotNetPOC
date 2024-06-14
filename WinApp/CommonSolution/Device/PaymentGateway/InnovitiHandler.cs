/********************************************************************************************
* Project Name - Semnox.Parafait.Device.PaymentGateway - InnovitiHandler
* Description  - I
* 
**************
**Version Log
**************
*Version      Date             Modified By        Remarks          
*********************************************************************************************
*2.50.0       12-Nov-2018      Archana            Created
********************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Device.PaymentGateway
{
    public class InnovitiHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        [DllImport(@"innovEFT.dll")]
        public static extern int innovEFT_GUI(int TRANSACTION_MODE, int TRANSACTION_TYPE, string REQUEST_PARAMETERS, byte[] RESPONSE_PARAMETER);

        public string MerchantReceiptText
        {
            get { return merchantReceiptText; }
        }
        public string CustomerReceiptText
        {
            get { return customerReceiptText; }
        }
        string merchantReceiptText;

        string customerReceiptText;
        
        internal InnovitiHandler()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }


        internal CCTransactionsPGWDTO ProcessTransaction(TransactionType transactionType, InnovitiRequestObject innovitiTransactionObject)
        {
            try
            {
                CCTransactionsPGWDTO cCTransactionsPGWDTO = null;
                byte[] responseXML = new byte[15000];
                XElement requestsXML = new XElement("purchase-request",
                                        new XElement("TransactionInput",
                                        new XAttribute("Id", "000001"),
                                        new XElement("Card",
                                        new XElement("IsManualEntry", "true"),
                                        new XElement("CardNumber", innovitiTransactionObject.CardNumber),
                                        new XElement("ExpirationDate",
                                        new XElement("MM", "11"),
                                        new XElement("YY", "11"))),
                                        new XElement("Amount",
                                        new XElement("BaseAmount", (innovitiTransactionObject.Amount * 100).ToString("##0")),
                                        new XElement("discount", 00),
                                        new XElement("Amnt", (innovitiTransactionObject.Amount * 100).ToString("##0"))),
                                        new XElement("POS",
                                        new XElement("POSReferenceNumber", transactionType == TransactionType.VOID ? innovitiTransactionObject.InvoiceNumber : "000000"),
                                        new XElement("TransactionTime", innovitiTransactionObject.TransactionTime.ToString("HHmmssddMM")))));
                string s = requestsXML.ToString();
                if (transactionType == TransactionType.SALE)
                {
                    int retVal = innovEFT_GUI(Convert.ToInt32(innovitiTransactionObject.TenderMode), 0, s, responseXML);
                }
                else if (transactionType == TransactionType.VOID)
                {
                    int retVal = innovEFT_GUI(Convert.ToInt32(innovitiTransactionObject.TenderMode), 1, s, responseXML);
                }
                hubresponse resonseObject = new hubresponse();
                var reader = new StringReader(Encoding.UTF8.GetString(responseXML));
                var serializer = new XmlSerializer(typeof(hubresponse));
                resonseObject = (hubresponse)serializer.Deserialize(reader);
                if (resonseObject != null)
                {
                    cCTransactionsPGWDTO = new CCTransactionsPGWDTO();
                    hubresponseTransaction hubResponseTransaction = new hubresponseTransaction();
                    hubResponseTransaction = resonseObject.Items[0];
                    cCTransactionsPGWDTO.DSIXReturnCode = hubResponseTransaction.State[0].StatusCode;
                    cCTransactionsPGWDTO.InvoiceNo = string.IsNullOrEmpty(hubResponseTransaction.State[0].InvoiceNumber)? "000000" : hubResponseTransaction.State[0].InvoiceNumber;
                    cCTransactionsPGWDTO.TextResponse = hubResponseTransaction.State[0].StatusMessage;
                    cCTransactionsPGWDTO.TokenID = hubResponseTransaction.State[0].HostResponse[0].RetrievalRefrenceNumber;
                    cCTransactionsPGWDTO.AcctNo = hubResponseTransaction.Card[0].CardNumber;
                    cCTransactionsPGWDTO.TranCode = hubResponseTransaction.State[0].TransactionType;
                    cCTransactionsPGWDTO.ProcessData = hubResponseTransaction.State[0].HostResponse[0].ApprovalCode;
                    cCTransactionsPGWDTO.Purchase = (innovitiTransactionObject.Amount).ToString();
                    cCTransactionsPGWDTO.Authorize = (string.IsNullOrEmpty(hubResponseTransaction.State[0].Amount)?"0":hubResponseTransaction.State[0].Amount).ToString();
                    cCTransactionsPGWDTO.RecordNo = innovitiTransactionObject.TransactionInputId.ToString();
                    cCTransactionsPGWDTO.ResponseOrigin = hubResponseTransaction.State[0].HostResponse[0].ResponseMessage;
                    cCTransactionsPGWDTO.AcqRefData = hubResponseTransaction.State[0].AcquirerName;
                    System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
                    try
                    {
                        cCTransactionsPGWDTO.TransactionDatetime = (string.IsNullOrEmpty(hubResponseTransaction.State[0].TransactionTime)) ? DateTime.MinValue : DateTime.ParseExact(hubResponseTransaction.State[0].TransactionTime.Replace('T', ' '), "yyyy-MM-dd HH:mm:ss", provider);
                    }
                    catch
                    {
                        cCTransactionsPGWDTO.TransactionDatetime = innovitiTransactionObject.TransactionTime;
                    }
                    if (!hubResponseTransaction.State[0].StatusCode.Equals("530"))
                    {
                        if (hubResponseTransaction.ChargeslipData[0].Receipt[0].isCustomerCopy.Equals("false"))
                        {
                            foreach (hubresponseTransactionChargeslipDataReceiptPrintline hTchargeslipPrintLine in hubResponseTransaction.ChargeslipData[0].Receipt[0].Printline)
                            {
                                merchantReceiptText += Common.AllignText(string.IsNullOrEmpty(hTchargeslipPrintLine.Value) ? " " : hTchargeslipPrintLine.Value, (!string.IsNullOrEmpty(hTchargeslipPrintLine.isCentered) && hTchargeslipPrintLine.isCentered.Equals("true")) ? Common.Alignment.Center : Common.Alignment.Left) + Environment.NewLine;
                            }
                        }
                        log.LogVariableState("merchantReceiptText", merchantReceiptText);
                        if (hubResponseTransaction.ChargeslipData[0].Receipt[1].isCustomerCopy.Equals("true"))
                        {
                            foreach (hubresponseTransactionChargeslipDataReceiptPrintline hTchargeslipPrintLine in hubResponseTransaction.ChargeslipData[0].Receipt[0].Printline)
                            {
                                customerReceiptText += Common.AllignText(string.IsNullOrEmpty(hTchargeslipPrintLine.Value) ? " " : hTchargeslipPrintLine.Value, (!string.IsNullOrEmpty(hTchargeslipPrintLine.isCentered) && hTchargeslipPrintLine.isCentered.Equals("true")) ? Common.Alignment.Center : Common.Alignment.Left) + Environment.NewLine;
                            }
                            if(!string.IsNullOrEmpty(customerReceiptText))
                            {
                                customerReceiptText = customerReceiptText.Replace("MERCHANT COPY", "CUSTOMER COPY");
                            }
                        }
                        log.LogVariableState("customerReceiptText", customerReceiptText);
                    }
                    log.LogVariableState("hubResponseTransaction", hubResponseTransaction);
                }
                return cCTransactionsPGWDTO;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
    }



    public class InnovitiRequestObject
    {
        int transactionInputId;
        string cardNumber;
        decimal amount;
        int posReferenceNumber;
        DateTime transactionTime;
        string invoiceNumber;
        string status;
        string tendermode;

        public InnovitiRequestObject()
        {
            transactionInputId = -1;
            cardNumber = "1111111111111111";
            amount = 0;
            posReferenceNumber = -1;
            transactionTime = DateTime.MinValue;
            invoiceNumber = "";
            status = "";
            tendermode = "0";
        }

        public int TransactionInputId
        {
            get { return transactionInputId; }
            set { transactionInputId = value; }
        }

        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }


        public int PosReferenceNumber
        {
            get { return posReferenceNumber; }
            set { posReferenceNumber = value; }
        }

        public DateTime TransactionTime
        {
            get { return transactionTime; }
            set { transactionTime = value; }
        }


        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { invoiceNumber = value; }
        }
        public string TenderMode
        {
            get { return tendermode; }
            set { tendermode = value; }
        }


        public string Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}

