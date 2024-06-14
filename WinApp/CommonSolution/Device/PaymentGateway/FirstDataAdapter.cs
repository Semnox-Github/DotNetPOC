using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Net;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// First Data Adapter Class
    /// </summary>
    public class FirstDataAdapter
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Utilities _utilities;
        /// <summary>
        ///  Whether the payment process is supervised by an attendant.
        /// </summary>
        public static bool IsUnattended = false;
        /// <summary>
        /// bool isCredit
        /// </summary>
        public bool isCredit = true;
        /// <summary>
        /// Print receipt variable 
        /// </summary>
        public bool PrintReceipt = true;
        VerifoneHandler verifonehndlr = new VerifoneHandler();
        /// <summary>
        /// Instance of class FirstDataResponse
        /// </summary>
        public FirstDataResponse FirstDataResp = new FirstDataResponse();
        ShowMessageDelegate showMessageDelegate;
        //ftmTransactionType ftmTransactionTypeObj;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inUtilities"></param>
        ///  <param name="showMessageDelegate">showMessageDelegate</param>
        public FirstDataAdapter(Utilities inUtilities, ShowMessageDelegate showMessageDelegate)
        {
            log.LogMethodEntry(inUtilities);

            _utilities = inUtilities;
            this.showMessageDelegate = showMessageDelegate;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // comparable to modern browsers

            log.LogMethodExit(null);
        }

        /// <summary>
        /// Makes Payment
        /// </summary>
        /// <param name="FirstdataTrxnRqust"></param>
        /// <returns></returns>
        public bool MakePayment(FirstDataTransactionRequest FirstdataTrxnRqust)
        {
            log.LogMethodEntry(FirstdataTrxnRqust);


            bool status = false;
            try
            {
                try
                {
                    VerifoneHandler.verifonePort = Convert.ToInt32(_utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO"));
                }
                catch(Exception ex)
                {
                    log.Error("Error when fetching valid value for Verifone Port", ex);

                    FirstDataResp.responseMessage = "The Verifone terminal port mentioned in the configuration is invalid!!!...";

                    log.LogMethodExit(false);
                    return false;
                }
                if (System.IO.Ports.SerialPort.GetPortNames().Contains("COM" + VerifoneHandler.verifonePort.ToString()))
                {
                    if (IsUnattended)
                    {
                        FirstdataTrxnRqust.isCredit = isCredit;
                    }                    
                    
                    FirstdataTrxnRqust.isPrintReceipt = PrintReceipt;
                    frmFirstDataCore FDCore = new frmFirstDataCore(FirstdataTrxnRqust, verifonehndlr, _utilities, showMessageDelegate);
                    DialogResult dr = FDCore.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                    //status = FDCore.DoTransaction("Sale");
                    FirstDataResp = FDCore.firstdataResponse;
                }
                else
                {
                    status = false;
                    FirstDataResp.responseMessage = "Device may not be connected properly or Port does not exist!!!...";
                }

                log.LogMethodExit(status);
                return status;
            }
            catch (Exception e)
            {
                log.Error("Error occured while making Paymet", e);
                FirstDataResp.responseMessage = e.ToString();

                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Reverts Payemnt
        /// </summary>
        /// <param name="trxFstdtRequest"></param>
        /// <param name="OrigCCResponseId"></param>
        /// <returns></returns>
        public bool ReverseOrVoid(FirstDataTransactionRequest trxFstdtRequest, object OrigCCResponseId)
        {
            log.LogMethodEntry(trxFstdtRequest, OrigCCResponseId);

            //All types of reversals are called through this function
            DateTime dtOrgLocal;
            int diff;
            bool status;
            double Amount;

            try
            {
                try
                {
                    VerifoneHandler.verifonePort = Convert.ToInt32(_utilities.getParafaitDefaults("CREDIT_CARD_TERMINAL_PORT_NO"));
                }
                catch(Exception ex)
                {
                    log.Error("Error ocured while fetching a valid value for Verifone Port", ex);
                    FirstDataResp.responseMessage = "The Verifone terminal port mentioned in the configuration is invalid!!!...";

                    log.LogMethodExit(false);
                    return false;
                }

                FirstDataTransactionRequest trxRequest = new FirstDataTransactionRequest();//request calss object
                trxRequest = trxFstdtRequest;
                trxRequest.isPrintReceipt = PrintReceipt;
                Amount = Math.Round(trxRequest.TransactionAmount * 100);
                getDetailsForReturnVoidSale(OrigCCResponseId, trxRequest);

                if (!string.IsNullOrEmpty(trxRequest.OrigDatetime))
                {
                    dtOrgLocal = DateTime.ParseExact(trxRequest.OrigDatetime, "yyyyMMddHHmmss", null);
                }
                else
                {
                    log.LogMethodExit(false);
                    return false;
                }
                DateTime dt = dtOrgLocal.AddMinutes(23);//as per the first data we can do void transaction in 25 min 
                DateTime dtNow = DateTime.Parse(ServerDateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                diff = dt.CompareTo(dtNow);
                if (diff >= 0 && Amount == trxRequest.OrigTransactionAmount)//deciding the reversal type
                {
                    if (!string.IsNullOrEmpty(OrigCCResponseId.ToString()))
                    {
                        if (trxRequest.OrigTransactionType.Equals("Completion"))
                        {
                            trxRequest.TransactionType = "Return";
                        }
                        else
                        {
                            trxRequest.TransactionType = "Void";
                        }
                    }
                    else
                    {

                        trxRequest.TransactionType = "Return";
                    }
                }
                else //deciding the reversal type
                {
                    trxRequest.TransactionType = "Return";
                }
                if (trxRequest.TransactionType == "Return")
                {
                    // added the following value to maintain unique reference no in case of refund 100000
                    trxRequest.ReferenceNo = (100000 + long.Parse(trxRequest.ReferenceNo)).ToString().PadLeft(12, '0');
                    if (IsUnattended)
                    {
                        trxRequest.isCredit = isCredit;
                    }
                }


                if (System.IO.Ports.SerialPort.GetPortNames().Contains("COM" + VerifoneHandler.verifonePort.ToString()) || !trxRequest.TransactionType.Equals("Return"))
                {
                    frmFirstDataCore FDCore = new frmFirstDataCore(trxRequest, verifonehndlr, _utilities, showMessageDelegate);
                    DialogResult dr = FDCore.ShowDialog();
                    if (dr == DialogResult.OK)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                    //status = FDCore.DoTransaction(trxRequest.TransactionType);
                    FirstDataResp = FDCore.firstdataResponse;

                    log.LogMethodExit(status);
                    return status;
                }
                else
                {
                    FirstDataResp.responseMessage = "Please check connectivity of the pinpad and port number mentioned in configuration.";

                    log.LogMethodExit(false);
                    return false;
                }

            }
            catch (Exception e)
            {
                log.Error("Error ocured while Reverse Or Void", e);
                FirstDataResp.responseMessage = e.ToString();

                log.LogMethodExit(false);
                return false;
            }
        }

        /// <summary>
        /// Gets the details for Return Void Sale
        /// </summary>
        /// <param name="CCResponseId"></param>
        /// <param name="ftr"></param>
        public void getDetailsForReturnVoidSale(object CCResponseId, FirstDataTransactionRequest ftr)
        {
            log.LogMethodEntry(CCResponseId, ftr);

            //geting reversal transaction details
            DataTable dt = _utilities.executeDataTable("exec SPGetVoidSaleReturnInvoiceDetails @ResponseId = @pResponseId",
                                                        new SqlParameter("@pResponseId", CCResponseId));
            if (dt.Rows.Count == 0)
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Unable to retrieve original transaction");
                throw new ApplicationException("Unable to retrieve original transaction");
            }

            if (ftr.TransactionType != "Void" && dt.Rows[0]["TranCode"].ToString().Equals("Return"))
            {
                log.LogMethodExit(null, "Throwing ApplicationException - Cannot perform " + ftr.TransactionType + " on a Sale Return");
                throw new ApplicationException("Cannot perform " + ftr.TransactionType + " on a Sale Return");
            }
            ftr.OrigSTAN = dt.Rows[0]["ProcessData"].ToString();
            ftr.OrigRef = dt.Rows[0]["InvoiceNo"].ToString();
            ftr.OrigResponseCode = dt.Rows[0]["DSIXReturnCode"].ToString();
            ftr.OrigAuthID = dt.Rows[0]["AuthCode"].ToString();
            ftr.OrigToken = dt.Rows[0]["TokenID"].ToString();
            ftr.OrigCardName = dt.Rows[0]["UserTraceData"].ToString();
            ftr.OrigGroupData = dt.Rows[0]["AcqRefData"].ToString();
            ftr.OrigCardExpiryDate = dt.Rows[0]["ResponseOrigin"].ToString();
            ftr.wasOrigSwiped = dt.Rows[0]["CaptureStatus"].ToString().Contains("1");
            ftr.OrigAccNo = dt.Rows[0]["AcctNo"].ToString();

            ftr.OrigDatetime = DateTime.Parse(dt.Rows[0]["TransactionDatetime"].ToString()).ToString("yyyyMMddHHmmss");
            ftr.OrigGMTDatetime = DateTime.Parse(dt.Rows[0]["TransactionDatetime"].ToString()).ToUniversalTime().ToString("yyyyMMddHHmmss");

            ftr.OrigTransactionType = dt.Rows[0]["TranCode"].ToString();

            ftr.OrigTransactionAmount = Convert.ToDouble(dt.Rows[0]["Authorize"]);

            ftr.isCredit = dt.Rows[0]["CardType"].ToString().Equals("Credit");

            log.LogMethodExit(null);
        }
    }
}
