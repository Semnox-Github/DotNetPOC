/********************************************************************************************
 * Project Name - China ICBC
 * Description  - This is the core class and using this the pos will comunicate with china ICBC
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Aug-2017   Raghuveera          Created 
 *2.130.4     22-Feb-2022   Mathew Ninan    Modified DateTime to ServerDateTime 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
 using Semnox.Core.Utilities;
using System.Threading;
using System.Data.SqlClient;
using System.Data;
//using Semnox.Parafait.TransactionPayments;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// The ChinaICBC is the chinese payament gateway called UNIONPAY MERCHANT SERVICE.
    /// The class does the payment, refund and  cancel
    /// </summary>
    public class ChinaICBCPaymentGateway:PaymentGateway
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        frmStatus frmStatus = null;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="utilities"></param>
        /// <param name="isUnattended"></param>
        /// <param name="showMessageDelegate"></param>
        /// <param name="writeToLogDelegate"></param>
        public ChinaICBCPaymentGateway(Utilities utilities, bool isUnattended, ShowMessageDelegate showMessageDelegate, WriteToLogDelegate writeToLogDelegate)
            :base(utilities, isUnattended, showMessageDelegate,writeToLogDelegate)
        {
            log.LogMethodEntry(utilities, isUnattended, showMessageDelegate, writeToLogDelegate);
            Initialize();
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Initialize method
        /// </summary>
        public override void Initialize()
        {
            log.LogMethodEntry();
            //Call signin Method here 
            //PerformSignIn();
            log.LogMethodExit(null);
        }
        /// <summary>
        /// MAkes Payment
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO MakePayment(TransactionPaymentsDTO transactionPaymentsDTO)//MakePayment(ChinaICBCTransactionRequest transactionRequest, ref ChinaICBCTransactionResponse transactionResponse, ref string message)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            ChinaICBCTransactionRequest transactionRequest = new ChinaICBCTransactionRequest();
            ChinaICBCTransactionResponse transactionResponse = new ChinaICBCTransactionResponse();
            CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_PAYMENT);
            string message = "";
            if (string.IsNullOrEmpty(transactionPaymentsDTO.CreditCardNumber.Trim()))//transactionPaymentsDTO.QrCardNo
            {
                transactionRequest.TransType = "05";                
            }
            else
            {
                transactionRequest.QRCardNO = transactionPaymentsDTO.CreditCardNumber.PadLeft(50,' ');
                transactionRequest.TransType = "46";
            }
            transactionRequest.TransAmount = (transactionPaymentsDTO.Amount * 100).ToString().PadLeft(12,'0');
            transactionRequest.TransDate = ServerDateTime.Now.Date.ToString("yyyyMMdd");
            transactionRequest.PlatId = utilities.ParafaitEnv.LoginID.PadLeft(20, ' ');
            transactionRequest.OperId = utilities.ParafaitEnv.LoginID.PadLeft(20, ' ');
            transactionRequest.Trxid = transactionPaymentsDTO.TransactionId;
            if (performTransaction(transactionRequest, ref transactionResponse, ref message))//00=Consume(Sale)
            {
                transactionPaymentsDTO.Amount = Convert.ToDouble(transactionResponse.Amount) / 100.0;
                transactionPaymentsDTO.CreditCardNumber = (string.IsNullOrEmpty(transactionResponse.CardNo) ? "" : transactionResponse.CardNo.Substring(transactionResponse.CardNo.Length - 4).PadLeft(transactionResponse.CardNo.Length,'X'));
                transactionPaymentsDTO.Memo= transactionResponse.ReceiptText;
                transactionPaymentsDTO.CreditCardAuthorization = transactionResponse.AuthNo;
                transactionPaymentsDTO.CCResponseId = transactionResponse.ccResponseId;

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            else
            {
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Reverts Payment
        /// </summary>
        /// <param name="transactionPaymentsDTO"></param>
        /// <returns></returns>
        public override TransactionPaymentsDTO RefundAmount(TransactionPaymentsDTO transactionPaymentsDTO)
        {
            log.LogMethodEntry(transactionPaymentsDTO);

            ChinaICBCTransactionRequest transactionRequest = new ChinaICBCTransactionRequest();
            ChinaICBCTransactionResponse transactionResponse = new ChinaICBCTransactionResponse();
            string message = "";
            CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(transactionPaymentsDTO.CCResponseId);
            CCTransactionsPGWDTO ccTransactionsPGWDTO = ccTransactionsPGWBL.CCTransactionsPGWDTO;
            CreateCCRequestPGW(transactionPaymentsDTO, CREDIT_CARD_REFUND);
            if (!ccTransactionsPGWDTO.TranCode.Equals("46"))
            {
                if (ccTransactionsPGWDTO.TransactionDatetime.AddDays(1).Date.Equals(DateTime.Today))
                {
                    transactionRequest.TransType = "42";
                }
                else
                {
                    transactionRequest.TransType = "04";
                }                
            }
            else
            {                
                transactionRequest.QROrderNo = ccTransactionsPGWDTO.ProcessData.PadLeft(50, ' ');
                transactionRequest.QRCardNO = ccTransactionsPGWDTO.AcctNo.Substring(ccTransactionsPGWDTO.AcctNo.Length-4).PadLeft(50, ' ');
                transactionRequest.TransType = "47";
            }            
            transactionRequest.TransAmount = (transactionPaymentsDTO.Amount * 100).ToString().PadLeft(12, '0');
            transactionRequest.ReferNo = ccTransactionsPGWDTO.InvoiceNo.PadLeft(8, '0');//transactionPaymentsDTO.TransactionId.ToString().PadLeft(8, '0');
            transactionRequest.ReferNo = (transactionRequest.ReferNo.Length > 8) ? transactionRequest.ReferNo.Substring(transactionRequest.ReferNo.Length - 8) : transactionRequest.ReferNo;
            transactionRequest.TransDate = ccTransactionsPGWDTO.TransactionDatetime.ToString("yyyyMMdd");
            transactionRequest.PlatId = utilities.ParafaitEnv.LoginID.PadLeft(20, ' ');
            transactionRequest.OperId = utilities.ParafaitEnv.LoginID.PadLeft(20, ' ');
            transactionRequest.TerminalId = ccTransactionsPGWDTO.CaptureStatus.PadLeft(15, '0');
            transactionRequest.Trxid = transactionPaymentsDTO.TransactionId;
            if (performTransaction(transactionRequest, ref transactionResponse, ref message))
            {
                transactionPaymentsDTO.Amount = Convert.ToDouble(transactionResponse.Amount) / 100.0;
                transactionPaymentsDTO.CreditCardNumber = (string.IsNullOrEmpty(transactionResponse.CardNo) ? "" : transactionResponse.CardNo.Substring(transactionResponse.CardNo.Length - 4).PadLeft(transactionResponse.CardNo.Length, 'X'));
                transactionPaymentsDTO.Memo = transactionResponse.ReceiptText;
                transactionPaymentsDTO.CreditCardAuthorization = transactionResponse.AuthNo;

                log.LogMethodExit(transactionPaymentsDTO);
                return transactionPaymentsDTO;
            }
            else
            {
                log.LogMethodExit(message);
                throw new Exception(message);
            }
        }
        
        private void PerformSignIn()
        {
            log.LogMethodEntry();

            ChinaICBCTransactionRequest transactionRequest = new ChinaICBCTransactionRequest();
            ChinaICBCTransactionResponse transactionResponse = new ChinaICBCTransactionResponse();
            string message = "";
            transactionRequest.TransType = "09";
            transactionRequest.PlatId = utilities.ParafaitEnv.LoginID.PadLeft(20, ' ');
            transactionRequest.OperId = utilities.ParafaitEnv.LoginID.PadLeft(20, ' ');
            if (!performTransaction(transactionRequest, ref transactionResponse, ref message))
            {
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new Exception(message);
            }

            log.LogMethodExit(null);
        }
        private bool performTransaction(ChinaICBCTransactionRequest transactionRequest, ref ChinaICBCTransactionResponse transactionResponse, ref string message)
        {
            log.LogMethodEntry(transactionRequest, transactionResponse, message);
            
            try
            {
                if (transactionRequest != null)
                {
                    transactionResponse = ProcessRequest(transactionRequest, ref message);
                    log.LogVariableState("message", message);
                    if (transactionResponse != null && !string.IsNullOrEmpty(transactionResponse.RspCode) && transactionResponse.RspCode.Equals("00"))
                    {
                        if (!CreateResponse(transactionResponse, transactionRequest.TransAmount, transactionRequest.TransType))
                        {
                            message = utilities.MessageUtils.getMessage("Failed to create response.");
                        }
                        transactionResponse.ReceiptText = GetReceipt(transactionRequest.Trxid);

                    }
                    else if (transactionResponse != null && string.IsNullOrEmpty(transactionResponse.RspCode) && !transactionResponse.RspCode.Equals("00"))
                    {
                        try
                        {
                            CreateResponse(transactionResponse, transactionRequest.TransAmount, transactionRequest.TransType);
                        }
                        catch (Exception ex) { log.Fatal("Error ocured on save of response" + ex.ToString()); }
                        throw new Exception((string.IsNullOrEmpty(message) ? transactionResponse.RspMessage : message));
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(message))
                            message = utilities.MessageUtils.getMessage("Response not Available");
                        log.LogVariableState("message", message);
                        log.LogMethodExit(false);
                        throw new Exception(message);
                    }

                    frmStatus.Close();
                    frmStatus.Dispose();
                    if (transactionResponse != null && transactionResponse.RspCode.Equals("00"))
                    {
                        log.Debug("Ends-performTransaction(transactionRequest, transactionResponse, message, tranType) because transaction is approved");
                        try
                        {
                            if (utilities.getParafaitDefaults("PRINT_CUSTOMER_RECEIPT") == "Y")
                            {                                
                                print(transactionResponse.ReceiptText);
                            }
                        }
                        catch (Exception ex)
                        {
                            log.Error("Error occured wihile printing customer receipt", ex);
                        }
                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        if (transactionResponse != null)
                        {
                            message = transactionResponse.RspMessage;
                        }
                        log.LogMethodExit(false);
                        return false;
                    }
                }
                else
                {
                    log.Debug("Ends-performTransaction(transactionRequest, transactionResponse, message, tranType) because transaction request object is null");
                    message = utilities.MessageUtils.getMessage("Not a valid request");
                    log.LogMethodExit(false);
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing transaction", ex);

                if (frmStatus != null)
                {
                    frmStatus.Close();
                    frmStatus.Dispose();
                }
                message = ex.Message;
                log.LogVariableState("message", message);
                log.Fatal("Ends-performTransaction(transactionRequest, transactionResponse, message, tranType) with exception " + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }
        private ChinaICBCTransactionResponse ProcessRequest(ChinaICBCTransactionRequest transRequest, ref string message)
        {
            log.LogMethodEntry(transRequest, message);

            ChinaICBCTransactionResponse transactionResponse;            
            frmStatus = new frmStatus(utilities, transRequest);
            frmStatus.refreshMessage();
            frmStatus.ShowDialog();
            message = frmStatus.responseString;
            if (frmStatus.transactionResponse == null)
            {
                log.Debug("Response is null." + message);
                log.LogMethodExit(null);
                return null;
            }
            transactionResponse = frmStatus.transactionResponse;
            transactionResponse.GetClass();
            log.LogMethodExit(transactionResponse);
            return transactionResponse;
        }

        private bool CreateResponse(ChinaICBCTransactionResponse transactionResponse,string TranAmount, string tranCode)
        {
            log.LogMethodEntry(transactionResponse, TranAmount, tranCode);

            //adding response to CCTransactionsPGW
            //log.Debug("Starts-CreateResponse(transactionResponse, tranCode) method");          
            try
            {
                CCTransactionsPGWDTO ccTransactionsPGWDTO = new CCTransactionsPGWDTO();
                ccTransactionsPGWDTO.AcctNo = transactionResponse.CardNo;
                ccTransactionsPGWDTO.AuthCode = transactionResponse.AuthNo;
                ccTransactionsPGWDTO.Purchase = TranAmount;
                ccTransactionsPGWDTO.Authorize = transactionResponse.Amount;
                ccTransactionsPGWDTO.CardType = transactionResponse.CardType;
                ccTransactionsPGWDTO.DSIXReturnCode = transactionResponse.RspCode;
                ccTransactionsPGWDTO.TextResponse = transactionResponse.RspMessage;
                ccTransactionsPGWDTO.InvoiceNo = transactionResponse.ReferNo;
                ccTransactionsPGWDTO.RecordNo = transactionResponse.TerminalId;
                ccTransactionsPGWDTO.TipAmount = transactionResponse.TipAmount;
                ccTransactionsPGWDTO.TranCode = tranCode;
                try
                {
                    ccTransactionsPGWDTO.TransactionDatetime = Convert.ToDateTime(transactionResponse.TransDate + transactionResponse.TransTime);
                }
                catch(Exception ex)
                {
                    log.Error("Error occured while fetching the transaction date and time", ex);
                    ccTransactionsPGWDTO.TransactionDatetime = ServerDateTime.Now;
                }
                ccTransactionsPGWDTO.ResponseOrigin = transactionResponse.TerminalBatchNo  + "-" + transactionResponse.TerminalTraceNo;
                ccTransactionsPGWDTO.TokenID = transactionResponse.QRCardNO;
                ccTransactionsPGWDTO.ProcessData = transactionResponse.QROrderNo;
                ccTransactionsPGWDTO.CaptureStatus = transactionResponse.TerminalId;
                CCTransactionsPGWBL ccTransactionsPGWBL = new CCTransactionsPGWBL(ccTransactionsPGWDTO);
                ccTransactionsPGWBL.Save();
                transactionResponse.ccResponseId = ccTransactionsPGWDTO.ResponseID;

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while creating response", ex);
                transactionResponse.ccResponseId = -1;
                log.Fatal("Ends-CreateResponse(transactionResponse, tranCode) method with the exception " + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }

        private string GetReceipt(int transactionId)
        {
            log.LogMethodEntry(transactionId);

            try
            {
                string receiptText = "";
                string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!System.IO.Directory.Exists(path + "\\Receipt"))
                {
                    System.IO.Directory.CreateDirectory(path + "\\Receipt");
                }
                if (System.IO.File.Exists(path + "\\ICBCPRTTKT.txt"))
                {
                    receiptText = System.IO.File.ReadAllText(path + "\\ICBCPRTTKT.txt", Encoding.GetEncoding("gb2312"));                    
                    if (System.IO.File.Exists(path + "\\Receipt\\" + transactionId + ".txt"))
                        transactionId *= -1;
                    System.IO.File.Move(path + "\\ICBCPRTTKT.txt", path + "\\Receipt\\" + transactionId + ".txt");
                }

                log.LogMethodExit(receiptText);
                return receiptText;
            }
            catch(Exception ex)
            {
                log.Error("Error occured while getting the receipt", ex);
                System.Windows.Forms.MessageBox.Show(utilities.MessageUtils.getMessage("There is some problem while generating the receipt."));

                log.LogMethodExit(null);
                return null;
            }
        }
        void print(string printText)
        {
            log.LogMethodEntry(printText);

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            pd.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 300, 1350);
            
            pd.PrintPage += (sender, e) =>
            {
                Font f = new Font("Courier New", 9);
                e.Graphics.DrawString(printText, f, new SolidBrush(Color.Black), new RectangleF(0, 0, pd.DefaultPageSettings.PrintableArea.Width, pd.DefaultPageSettings.PrintableArea.Height));
            };
            pd.Print();

            log.LogMethodExit(null);
        }

    }
}
