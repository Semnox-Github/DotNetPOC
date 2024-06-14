/********************************************************************************************
 * Project Name - China UMS
 * Description  - This is the core class and using this the pos will comunicate with china UMS
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        29-Apr-2016   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Text;
// using Semnox.Core.Utilities;
using System.Data.SqlClient;
using System.Data;
using Semnox.Core.Utilities;
using Semnox.Parafait.Device.Printer;
using Semnox.Parafait.Device.PaymentGateway.ChinaUMSUI; // There are two frmsttaus. Hence added chinaums
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// The ChinaUMS is the chinese payament gateway called UNIONPAY MERCHANT SERVICE.
    /// The class does the payment, refund and  cancel
    /// </summary>
    public class ChinaUMS
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Semnox.Parafait.Device.PaymentGateway.ChinaUMSUI.frmStatus frmStatus = null;
        Utilities utilities;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="_Utilities"></param>
        public ChinaUMS(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);

            utilities = _Utilities;

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Makes Payment
        /// </summary>
        /// <param name="transactionRequest"></param>
        /// <param name="transactionResponse"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool PerformSale(ChinaUMSTransactionRequest transactionRequest, ref ChinaUMSTransactionResponse transactionResponse, ref string message)
        {
            log.LogMethodEntry(transactionRequest, transactionResponse, message);

            bool returnValueNew = performTransaction(transactionRequest, ref transactionResponse, ref message, "00");
            log.LogMethodExit(returnValueNew);
            return returnValueNew;//00=Consume(Sale)
        }

        /// <summary>
        /// Reverts the Payment
        /// </summary>
        /// <param name="transactionRequest"></param>
        /// <param name="transactionResponse"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool PerformRefund(ChinaUMSTransactionRequest transactionRequest, ref ChinaUMSTransactionResponse transactionResponse, ref string message)
        {
            log.LogMethodEntry(transactionRequest, transactionResponse, message);

            string tranType = "01";
            if (!GetResponse(transactionRequest, ref tranType))
            {
                message = "Last transaction response not found.";
                log.LogMethodExit(false);
                return false;
            }

            bool returnValueNew = performTransaction(transactionRequest, ref transactionResponse, ref message, tranType);
            log.LogMethodExit(returnValueNew);
            return returnValueNew;//02=Refund, cancel=01
        }
        /// <summary>
        /// Cancel Request method
        /// </summary>
        /// <param name="transactionRequest"></param>
        /// <param name="transactionResponse"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool CancelRequest(ChinaUMSTransactionRequest transactionRequest, ref ChinaUMSTransactionResponse transactionResponse, ref string message)
        {
            log.LogMethodEntry(transactionRequest, transactionResponse, message);

            bool returnValueNew = performTransaction(transactionRequest, ref transactionResponse, ref message, "01");
            log.LogMethodExit(returnValueNew);
            return returnValueNew;//01=Cancel
        }
        private bool performTransaction(ChinaUMSTransactionRequest transactionRequest, ref ChinaUMSTransactionResponse transactionResponse, ref string message, string tranType)
        {
            log.LogMethodEntry(transactionRequest, transactionResponse, message, tranType);

            try
            {
                StringBuilder requestString = new StringBuilder();
                if (transactionRequest != null)
                {
                    requestString = CreateRequest(transactionRequest, tranType);
                    transactionResponse = ProcessRequest(requestString, transactionRequest.LRC, ref message);
                    if (transactionResponse != null)
                    {
                        CreateResponse(transactionResponse, transactionRequest.TransactionType);
                        //PrintDocument prtDoc = new PrintDocument(transactionResponse, transactionRequest, utilities);
                        Printer.PrintDocument prtDoc = new Printer.PrintDocument(transactionResponse, transactionRequest, utilities);
                        transactionResponse.ReceiptText = prtDoc.getReceiptText();
                    }

                    frmStatus.Close();
                    frmStatus.Dispose();
                    if (transactionResponse != null && transactionResponse.ResponseCode.Equals("00"))
                    {
                        log.Debug("Ends-performTransaction(transactionRequest, transactionResponse, message, tranType) because transaction is approved");

                        log.LogMethodExit(true);
                        return true;
                    }
                    else
                    {
                        log.Debug("Ends-performTransaction(transactionRequest, transactionResponse, message, tranType) because transaction is not approved");

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
                log.Error("Error occured whie performing transactions", ex);

                if (frmStatus != null)
                {
                    frmStatus.Close();
                    frmStatus.Dispose();
                }
                message = ex.ToString();
                log.Fatal("Ends-performTransaction(transactionRequest, transactionResponse, message, tranType) with exception " + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }
        private ChinaUMSTransactionResponse ProcessRequest(StringBuilder requestString, string LRC, ref string message)
        {
            log.LogMethodEntry(requestString, LRC, message);

            ChinaUMSTransactionResponse transactionResponse = new ChinaUMSTransactionResponse();
            frmStatus = new Semnox.Parafait.Device.PaymentGateway.ChinaUMSUI.frmStatus(utilities);
           
            frmStatus.refreshMessage();
            frmStatus.requestStringSB = requestString;
            frmStatus.requestLRC = LRC;            
            frmStatus.ShowDialog();
            transactionResponse = frmStatus.transactionResponse;
            message = frmStatus.responseString;
            frmStatus.Dispose();
            log.LogMethodExit(transactionResponse);
            return transactionResponse;
        }

        private bool CreateResponse(ChinaUMSTransactionResponse transactionResponse, string tranCode)
        {
            log.LogMethodEntry(transactionResponse, tranCode);
            //adding response to CCTransactionsPGW
            DataTable dTable = new DataTable();
            try
            {

                dTable = utilities.executeDataTable(@"insert into CCTransactionsPGW 
                                               (RefNo,CardType,AuthCode,TransactionDatetime,site_id,DSIXReturnCode, TextResponse,AcctNo,TranCode,Authorize,InvoiceNo,RecordNo,ResponseOrigin,AcqRefData,ProcessData)
                                        values (@refNo,@CardType,@AuthCode,@TranDatetime,@site,@RtnCode,@Response,@AccNo,@TranType,@Authorize,@invoiceNo,@recordNo,@responseOrigin,@acqRefData,@processData) select @@identity",
                                        new SqlParameter("@refNo", transactionResponse.ReferenceNo),
                                        new SqlParameter("@CardType", transactionResponse.CardType),
                                        new SqlParameter("@AuthCode", transactionResponse.AuthorizationNo),
                                        new SqlParameter("@site", utilities.ParafaitEnv.SiteId),
                                        new SqlParameter("@RtnCode", transactionResponse.ReturnCode),
                                        new SqlParameter("@TranDatetime", DateTime.Now),
                                        new SqlParameter("@AccNo", transactionResponse.CardNo),
                                        new SqlParameter("@TranType", tranCode),
                                        new SqlParameter("@invoiceNo", transactionResponse.DraftNo),
                                        new SqlParameter("@Authorize", transactionResponse.TransactionAmount),
                                        new SqlParameter("@recordNo", transactionResponse.MerchantId),
                                        new SqlParameter("@acqRefData", transactionResponse.BankId),
                                        new SqlParameter("@responseOrigin", transactionResponse.BatchNo),
                                        new SqlParameter("@processData", transactionResponse.TerminalID),
                                        new SqlParameter("@Response", transactionResponse.MistakesExplanation));

                log.LogVariableState("@TranDatetime", DateTime.Now);
                log.LogVariableState("@site", utilities.ParafaitEnv.SiteId);

                if (dTable.Rows.Count > 0)
                {
                    transactionResponse.ccResponseId = Convert.ToInt32(dTable.Rows[0][0].ToString());
                }
                else
                {
                    transactionResponse.ccResponseId = -1;
                }

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
        private bool GetResponse(ChinaUMSTransactionRequest transactionRequest, ref string tranType)
        {
            log.LogMethodEntry(transactionRequest, tranType);

            //getting response to CCTransactionsPGW
            DataTable dTable = new DataTable();
            try
            {

                dTable = utilities.executeDataTable(@"SELECT RefNo,CardType,AuthCode,TransactionDatetime,site_id,DSIXReturnCode, TextResponse,AcctNo,TranCode,Authorize,InvoiceNo,RecordNo,ResponseOrigin,AcqRefData,ProcessData "
                                                     + " from CCTransactionsPGW where ResponseID=@responseID",
                                                     new SqlParameter("@responseID", transactionRequest.ccResponseId));

                log.LogVariableState("@responseID", transactionRequest.ccResponseId);

                if (dTable.Rows.Count > 0)
                {
                    transactionRequest.OldTransactionDate = (dTable.Rows[0]["TransactionDatetime"] == DBNull.Value) ? new DateTime() : Convert.ToDateTime(dTable.Rows[0]["TransactionDatetime"].ToString());
                    transactionRequest.OriginalSalesDraft = dTable.Rows[0]["InvoiceNo"].ToString();
                    transactionRequest.OriginalReferenceNo = dTable.Rows[0]["RefNo"].ToString();
                    try
                    {
                        if (transactionRequest.TransactionAmount == Convert.ToInt64(dTable.Rows[0]["Authorize"].ToString()))
                        {
                            tranType = "01"; 
                        }
                        else
                        {
                            tranType = "02"; 
                        }
                    }
                    catch(Exception ex)
                    {
                        log.Error("Error occured while calculating Transsaction Amount", ex);
                        tranType = "01";
                    }
                }
                else
                {
                    log.Debug("Ends-GetResponse(transactionRequest) method because failed to get the get response");

                    log.LogVariableState("tranType", tranType);
                    log.LogMethodExit(false);
                    return false;
                }
                log.Debug("Ends-GetResponse(transactionRequest) method by returning the response from ccTransactionPGW");

                log.LogVariableState("tranType", tranType);
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while trying to get the response", ex);
                log.Fatal("Ends-GetResponse(transactionRequest) method with the exception " + ex.ToString());
                log.LogVariableState("tranType", tranType);
                log.LogMethodExit(false);
                return false;
            }
        }
        private StringBuilder CreateRequest(ChinaUMSTransactionRequest transactionRequest, string TranType)
        {
            log.LogMethodEntry(transactionRequest, TranType);

            StringBuilder requestString = new StringBuilder();
            if (transactionRequest.TerminalID.Length > 8)
            {
                transactionRequest.TerminalID = transactionRequest.TerminalID.Substring(0, 8);
            }
            requestString.Append(transactionRequest.TerminalID.PadRight(8));
            if (transactionRequest.CashierNo.Length > 8)
            {
                transactionRequest.CashierNo = transactionRequest.CashierNo.Substring(0, 8);
            }
            requestString.Append(transactionRequest.CashierNo.PadRight(8));
            if (TranType.Length > 2)
            {
                transactionRequest.TransactionType = TranType.Substring(0, 2);
            }
            transactionRequest.TransactionType = TranType;
            requestString.Append(transactionRequest.TransactionType.PadLeft(2));
            requestString.Append(transactionRequest.TransactionAmount.ToString().PadLeft(12,'0'));
            if (transactionRequest.TransactionType.Equals("02"))
            {
                if (transactionRequest.OldTransactionDate.Equals(DateTime.MinValue))
                {
                    log.LogMethodExit(null, "Throwing Exception - Old transaction date no is not set");
                    throw (new Exception(utilities.MessageUtils.getMessage("Old transaction date no is not set.")));
                }
                else
                {
                    requestString.Append(transactionRequest.OldTransactionDate.ToString("yyyyMMdd"));
                }
            }
            else
            {
                requestString.Append("".PadRight(8));
            }
            if (transactionRequest.TransactionType.Equals("02"))
            {
                if (transactionRequest.OriginalReferenceNo.Length > 12)
                {
                    transactionRequest.OriginalReferenceNo = transactionRequest.OriginalReferenceNo.Substring(0, 12);
                }
                requestString.Append(transactionRequest.OriginalReferenceNo.PadRight(12));
            }
            else
            {
                requestString.Append(transactionRequest.OriginalReferenceNo.PadRight(12));
            }
            if (transactionRequest.TransactionType.Equals("01"))
            {
                if (transactionRequest.OriginalSalesDraft.Length > 6)
                {
                    transactionRequest.OriginalSalesDraft = transactionRequest.OriginalSalesDraft.Substring(0, 6);
                }
                requestString.Append(transactionRequest.OriginalSalesDraft.PadRight(6, '0'));
            }
            else
            {
                requestString.Append(transactionRequest.OriginalSalesDraft.PadRight(6));
            }
            if (string.IsNullOrEmpty(transactionRequest.PaymentNumber))
            {
                requestString.Append("".PadRight(32));
            }
            else
            {
                requestString.Append(transactionRequest.PaymentNumber.PadRight(32));
            }
            Random rnd = new Random();
            transactionRequest.LRC = rnd.Next(1000).ToString();
            requestString.Append(transactionRequest.LRC);
            //requestString.Append(transactionRequest.CustomInformation.PadRight(100));

            log.LogMethodExit(requestString);
            return requestString;
        }
    }
}
