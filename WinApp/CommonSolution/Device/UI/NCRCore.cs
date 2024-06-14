using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Semnox.Parafait.logging;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.Device.PaymentGateway
{
    internal partial class NCRCore : Form
    {
        public string message = "";
        public NCRResponse ncrResponse;
        bool IsThreadCompleted = false;
        bool IsCancelEnable = true;
        bool IsCashierDisplay = false;
        bool IsTransactionProcessing = false;
        /// <summary>
        /// To deside debit or credit.
        /// </summary>
        public bool IsDebitCard = false;
        Thread myThread;
        StringBuilder cashierDisplay;
        StringBuilder cashierDisplay2;
        Utilities utilities;
        int timeout = 0;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public NCRCore(Utilities _Utilities)
        {
            log.LogMethodEntry(_Utilities);
            InitializeComponent();
            utilities = _Utilities;
            log.LogMethodExit(null);
        }
        /// <summary>
        /// The Method does the sign in process of NCR Gateway
        /// </summary>
        /// <param name="CashierId"> Is integer value passed as string.</param>
        /// <param name="LaneNo">Is integer value passed as string.</param>
        public void SignOn(string CashierId, string LaneNo)
        {
            log.LogMethodEntry(CashierId, LaneNo);
            //btnCancel.Visible = false;
            //txtStatus.Text = message;
            NCRApiFunctions.CheckerSignOn(CashierId, LaneNo);
            log.LogMethodExit(null);
        }
        /// <summary>
        /// The method set the pos software version
        /// </summary>
        /// <param name="PosVersion">Pos software version is passed in the form of 'POS : 000.000.000.000' .</param>
        public void SetPOSVersion(string PosVersion)
        {
            log.LogMethodEntry(PosVersion);
            NCRApiFunctions.Set.POSTVersion(PosVersion);
            //btnCancel.Visible = true;
            //this.Close();
            log.LogMethodExit(null);
        }
        /// <summary>
        /// The method sends the sign off request to NCR.
        /// </summary>
        public void SignOff()
        {
            log.LogMethodEntry();
            NCRApiFunctions.CheckerSignOff();
            log.LogMethodExit(null);
        }

        /// <summary>
        /// The method is used to set the cashier id
        /// </summary>
        public void SetCashierID(string cashierId)
        {
            log.LogMethodEntry(cashierId);
            NCRApiFunctions.Set.CashierID(cashierId);
            log.LogMethodExit(null);
        }
        /// <summary>
        /// This method performs the sales transaction.
        /// </summary>
        /// <param name="ncrRequest">NCRRequest object</param>
        public void ProcessTransaction(NCRRequest ncrRequest)
        {
            log.LogMethodEntry(ncrRequest);
            IsThreadCompleted = false;
            IsTransactionProcessing = false;
            IsCashierDisplay = false;
            NCRApiFunctions.ResetClear();
            if (ncrRequest.TransactionType.Equals(eTransactionType.VOID_BY_MTX) || ncrRequest.TransactionType.Equals(eTransactionType.RETURN))
            {
                object o = utilities.executeScalar("select RecordNo from CCTransactionsPGW where ResponseID=@responseID", new SqlParameter("@responseID", ncrRequest.ccResponseId));
                if (o != null)
                {
                    ncrRequest.SequenceNo = new StringBuilder(o.ToString());
                }
            }            
            WaitResponseTimer.Start();
            myThread = new Thread(() =>
                {                    
                    log.Debug("Thread starts and calls the PerformTransaction(ncrRequest) method.");                    
                    ncrResponse = PerformTransaction(ncrRequest);
                    if(IsTransactionProcessing && ncrRequest.TransactionType.Equals(eTransactionType.VOID_BY_MTX))
                    {                        
                        IsTransactionProcessing = false;
                        ncrResponse = new NCRResponse();                        
                        ncrResponse = PerformTransaction(ncrRequest);
                    }
                    if (ncrResponse != null)
                    {
                        CreateResponse(ncrResponse, ncrRequest.TransactionType);
                    }
                    IsThreadCompleted = true;
                    log.Debug("Thread Ends.");
                }
                );
            myThread.Start();
            log.LogMethodExit(null);
        }
        /// <summary>
        /// Calling begin order
        /// </summary>
        public void BeginOrder()
        {
            log.LogMethodEntry();

            int status = 0;
            while (status == 0)
            {
                NCRApiFunctions.Get.SCATReady(ref status);
                Thread.Sleep(250);
            }
            NCRApiFunctions.BeginOrder();
            Thread.Sleep(250);

            log.LogMethodExit(null);
        }
        /// <summary>
        /// Calling end order 
        /// </summary>
        public void EndOrder()
        {
            log.LogMethodEntry();
            Thread.Sleep(250);
            NCRApiFunctions.EndOrder();
            Thread.Sleep(250);
            NCRApiFunctions.ResetClear();
            log.LogMethodExit(null);
        }
        
        /// <summary>
        /// Performs the transaction
        /// </summary>
        /// <param name="ncrRequest"> The ncr request object</param>
        /// <returns>returns the response object</returns>
        private NCRResponse PerformTransaction(NCRRequest ncrRequest)
        {
            log.LogMethodEntry(ncrRequest);
            int status = 0;
            StringBuilder tenderTypeMtx = new StringBuilder(32);
            StringBuilder tenderTypeMtxAfterTrxn = new StringBuilder(32);
            StringBuilder responseCode = new StringBuilder(4);  
            string respCode="";
            bool validationPass = false;
            bool printReceipt = true;
            StringBuilder fieldsMissing;
            string bits = "";
            int counter = 0;
            int exitCount = 0;
            int customerOK = 0;
            bool IsRepeat = true;
            NCRResponse ncrLResponse = null;
            
            try
            {
                NCRApiFunctions.Set.POSTTransactionNumber(ncrRequest.PostTransactionNumber);
                Thread.Sleep(250);
                NCRApiFunctions.Set.PurchaseAmount(ncrRequest.Amount);
                IsCashierDisplay = true;
                while (IsRepeat)
                {
                    IsRepeat = false;
                    Thread.Sleep(250);
                    
                    //if (NCRAdapter.IsUnattended)
                    //{
                    //    if (IsDebitCard)
                    //    {
                    //        NCRApiFunctions.Set.TenderTypePOS("80000000000000000000000000000000");
                    //    }
                    //    else
                    //    {
                    //        NCRApiFunctions.Set.TenderTypePOS("40000000000000000000000000000000");
                    //    }
                    //}
                    //else
                    //{
                        NCRApiFunctions.Set.TenderTypePOS("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
                    //}
                       // message = "Please swipe/insert your card";
                    Thread.Sleep(250);
                    status = 0;
                    while (status == 0)
                    {
                        NCRApiFunctions.Get.TenderTypeStatus(ref status);
                        Thread.Sleep(250);
                    }
                    if(status==2)
                    {
                        IsRepeat = true;
                        message = Enum.GetName(typeof(eTenderTypeStatus), status);
                    }
                    else if (status != 1 && status != 4)
                    {
                        log.Debug("Ends:PerformTransaction(ncrRequest) because TenderTypeStatus is:" + status+"- " +Enum.GetName(typeof(eTenderTypeStatus), status));
                        message = Enum.GetName(typeof(eTenderTypeStatus), status);
                        log.LogMethodExit(null);
                        return null;
                    }                    
                }
                NCRApiFunctions.Get.TenderTypeMtx(tenderTypeMtx);                
                Thread.Sleep(250);
                NCRApiFunctions.Set.TransactionType((int)ncrRequest.TransactionType);
                if (NCRAdapter.IsUnattended)
                {
                    Thread.Sleep(3000);
                }
                else
                {
                    Thread.Sleep(250);
                }
                
                status = 0;
                while (status == 0)
                {
                    NCRApiFunctions.Get.SCATStatus(ref status);
                    Thread.Sleep(250);
                }
                if (status != 1)
                {
                    log.Debug("Ends:PerformTransaction(ncrRequest) because SCATStatus is:" + status);
                    // message = Enum.GetName(typeof(eSCATStatus), status);
                    log.LogMethodExit(null);
                    return null;
                }
                while ((respCode.Equals("") || respCode.Equals("NP") || respCode.Equals("NW")) && exitCount<5)
                {
                    IsCancelEnable = true;
                    exitCount++;
                    validationPass = false;
                    while (!validationPass)
                    {
                        fieldsMissing = new StringBuilder(32);
                        NCRApiFunctions.ValidateData(ref validationPass, fieldsMissing);
                        //validationPass = false;
                        //fieldsMissing = new StringBuilder("00800000000000000000000000000000");
                        Thread.Sleep(250);
                        if (!validationPass)
                        {
                            bits = BinaryConverter(fieldsMissing);
                            ValidateInput(ncrRequest, bits);
                        }
                    }
                    while (customerOK == 0)
                    {
                        NCRApiFunctions.Get.CustomerOK(ref customerOK);
                        Thread.Sleep(250);
                    }
                    NCRApiFunctions.Get.TransactionTimeout(ref timeout);
                    Thread.Sleep(250);
                    NCRApiFunctions.Set.AmountChangeAllowed(false);
                    IsCancelEnable = false;
                    Thread.Sleep(1000);//This is to make sure that the user is not pressed cancel                    
                    IsTransactionProcessing = true;
                    status = 0;
                    IsCashierDisplay = false;
                    NCRApiFunctions.SendTransaction();
                    while (status == 0)
                    {
                        counter++;
                        Thread.Sleep(250);
                        NCRApiFunctions.Get.HostStatus(ref status);
                        IsCashierDisplay = true;
                        if (counter == 4)
                        {
                            counter = 0;
                            timeout--;
                            if (timeout <= 0)
                            {
                                TimeOutReversal();
                            }
                        }
                    }
                    ncrLResponse = new NCRResponse();
                    NCRApiFunctions.Get.ReceiptRequired(ref printReceipt);
                    try
                    {
                        ncrLResponse.TenderType = GetPaymentType(BinaryConverter(tenderTypeMtx));
                    }
                    catch(Exception ex)
                    {
                        log.Error("Error occured while fetching a valid value for TenderType", ex);
                    }
                    NCRApiFunctions.Get.ResponseCode(ncrLResponse.ResponseCode);
                    respCode = ncrLResponse.ResponseCode.ToString();
                }
                IsCashierDisplay = false;
                DefineResponse(ncrLResponse.ResponseCode.ToString(),ref ncrLResponse.ResponseText);
                NCRApiFunctions.Get.ApprovedAmount(ref ncrLResponse.ApprovedAmount);
                NCRApiFunctions.Get.TenderTypeMtx(tenderTypeMtxAfterTrxn);
                GetReceipt(ref ncrLResponse.CustomerReceiptText, ref ncrLResponse.MerchantReceiptText);
                NCRApiFunctions.Get.WinEpsCardType(ncrLResponse.CardType);
                NCRApiFunctions.Get.MTXSequenceNumber(ncrLResponse.SequenceNo);
                NCRApiFunctions.Get.TokenData(ncrLResponse.TokenData);
                NCRApiFunctions.Get.CashBackAmount(ref ncrLResponse.CashBackAmount);
                NCRApiFunctions.Get.AuthorizationNumber(ncrLResponse.AuthCode);
                NCRApiFunctions.Get.HostReferenceNumber(ncrLResponse.HostReferenceNumber);
                NCRApiFunctions.Get.CustomerName(ncrLResponse.CustomerName);
                NCRApiFunctions.Get.Track2DataMasked(ncrLResponse.CardNo);//Track2Data
                NCRApiFunctions.Get.PersonalAccountNumberMasked(ncrLResponse.AccountNumber);
                NCRApiFunctions.Get.TransactionDate(ncrLResponse.TrnsactionDate);
                NCRApiFunctions.Get.TransactionTime(ncrLResponse.TrnsactionTime);
                status = 0;
                NCRApiFunctions.Get.ERCRequired(ref status);
                if (!NCRAdapter.IsUnattended && status > 0)
                {
                    MessageBox.Show("ERCRequired Status:" + status.ToString());
                }
                IsCashierDisplay = true;
                NCRApiFunctions.TransactionComplete();
                
                IsCancelEnable = true;
                IsTransactionProcessing = false;

                log.LogMethodExit(ncrLResponse);
                return ncrLResponse;
            }
            catch (ThreadAbortException tae)
            {
                log.Error("Caught a ThreadAbortException - Error occured while performing Transaction by taking ncrRequest", tae);
                IsCashierDisplay = true;
                message = "Transaction cancelled";
                log.Fatal("Ends:PerformTransaction(ncrRequest) with exception" + tae.ToString());
                Thread.Sleep(250);
                if(IsTransactionProcessing)
                {
                    try
                    {
                        NCRApiFunctions.Get.MTXSequenceNumber(ncrRequest.SequenceNo);
                        ncrRequest.TransactionType = eTransactionType.VOID_BY_MTX;
                    }
                    catch(Exception ex)
                    {
                        log.Error("Error occured while fetching a valid value for MTX Sequence Number", ex);
                    }
                }
                NCRApiFunctions.ResetClear();
                if (IsTransactionProcessing)
                IsCancelEnable = true;
                ncrResponse = null;
                log.LogMethodExit(null);
                return null;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while performing Transaction by taking ncrRequest", ex);

                IsCashierDisplay = true;
                log.Fatal("Ends:PerformTransaction(ncrRequest) with exception" + ex.ToString());
                message = ex.ToString();
                Thread.Sleep(250);
                if (IsTransactionProcessing)
                {
                    try
                    {
                        NCRApiFunctions.Get.MTXSequenceNumber(ncrRequest.SequenceNo);
                        ncrRequest.TransactionType = eTransactionType.VOID_BY_MTX;
                    }
                    catch(Exception e)
                    {
                        log.Error("Error occured while fetching a valid value for MTX Sequence Number", e);
                    }
                }
                NCRApiFunctions.ResetClear();
                IsCancelEnable = true;
                ncrResponse = null;
                log.LogMethodExit(null);
                return null;
            }
        }
        private bool CreateResponse(NCRResponse ncrResponse, eTransactionType trantype)
        {
            log.LogMethodEntry(ncrResponse, trantype);
            //adding response to CCTransactionsPGW
            DataTable dTable = new DataTable();
            try
            {
                DateTime dt=new DateTime();
                if (ncrResponse.CardNo != null)
                    ncrResponse.CardNo = new StringBuilder(ncrResponse.CardNo.ToString().Substring(0, ncrResponse.CardNo.ToString().IndexOf("=")));
                dt=(ncrResponse.TrnsactionDate==null)?DateTime.Now:Convert.ToDateTime((DateTime.Now.Year.ToString().Substring(0,2)+ncrResponse.TrnsactionDate.ToString().Substring(4,2)+"-"+ncrResponse.TrnsactionDate.ToString().Substring(0,2)+"-"+ncrResponse.TrnsactionDate.ToString().Substring(2,2)+" "+((ncrResponse.TrnsactionTime==null)?"00:00:00":ncrResponse.TrnsactionTime.ToString().Substring(0,2)+":"+ncrResponse.TrnsactionTime.ToString().Substring(2,2)+":"+ncrResponse.TrnsactionTime.ToString().Substring(4,2))));
                
                dTable = utilities.executeDataTable(@"insert into CCTransactionsPGW 
                                               (RefNo,CardType,AuthCode,TransactionDatetime,site_id,DSIXReturnCode,AcctNo,TranCode,Authorize,InvoiceNo,RecordNo,UserTraceData,CustomerCopy,MerchantCopy)
                                        values (@refNo,@CardType,@AuthCode,@TranDatetime,@site,@RtnCode,@AccNo,@TranType,@Authorize,@invoiceNo,@recordNo,@userTraceData,@customerCopy,@merchantCopy) select @@identity",
                                        new SqlParameter("@refNo", ncrResponse.POSTransactionNo.ToString()),
                                        new SqlParameter("@CardType", ncrResponse.TenderType.ToString()),
                                        new SqlParameter("@AuthCode", ncrResponse.AuthCode.ToString()),
                                        new SqlParameter("@site", utilities.ParafaitEnv.SiteId),
                                        new SqlParameter("@RtnCode", ncrResponse.ResponseCode.ToString()),
                                        new SqlParameter("@TranDatetime",dt ),
                                        new SqlParameter("@AccNo", ncrResponse.CardNo.ToString()),
                                        new SqlParameter("@TranType", trantype.ToString()),
                                        new SqlParameter("@invoiceNo", ncrResponse.HostReferenceNumber.ToString()),
                                        new SqlParameter("@Authorize", ncrResponse.ApprovedAmount),
                                        new SqlParameter("@recordNo", ncrResponse.SequenceNo.ToString()),
                                        new SqlParameter("@userTraceData", ncrResponse.CustomerName.ToString())                                        ,
                                        new SqlParameter("@customerCopy", ncrResponse.CustomerReceiptText.ToString()),
                                        new SqlParameter("@merchantCopy", ncrResponse.MerchantReceiptText.ToString()));

                log.LogVariableState("@site", utilities.ParafaitEnv.SiteId);
                log.LogVariableState("@TranDatetime", dt);

                if (dTable.Rows.Count > 0)
                {
                    ncrResponse.ccResponseId = Convert.ToInt32(dTable.Rows[0][0].ToString());
                }
                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while creating response", ex);
                log.Fatal("Ends-CreateResponse(ncrResponse, tranCode) method with the exception " + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }
        
        /// <summary>
        /// Converting the fieldmissing data to 128 bit binary data
        /// </summary>
        /// <param name="FieldMissing">32 length string</param>
        /// <returns>Binary 128 bit data.</returns>
        private string BinaryConverter(StringBuilder FieldMissing)
        {
            log.LogMethodEntry(FieldMissing);

            string data;
            string binary = "";
            int b;
            if (FieldMissing != null)
            {
                data = FieldMissing.ToString();
                for (int i = 0; i < data.Length; i++)
                {
                    b = Convert.ToInt32(data[i].ToString());
                    binary += Convert.ToString(b, 2).PadLeft(4, '0');
                }

                log.LogMethodExit(binary);
                return binary;
            }

            log.LogMethodExit("");
            return "";
        }
        /// <summary>
        /// This function is used to identify the tendertype
        /// </summary>
        /// <param name="binary">Tender type binary data</param>
        /// <returns>returns the eTenderType</returns>
        private eTenderType GetPaymentType(string binary)
        {
            log.LogMethodEntry(binary);

            long deciNum;
            char[] charArray = binary.ToCharArray();
            Array.Reverse(charArray);
            string binaryReverse =new string(charArray);
            deciNum = Convert.ToInt64(binaryReverse, 2);

            eTenderType returnValueNew = (eTenderType)deciNum;
            log.LogMethodExit(returnValueNew);
            return returnValueNew;
        }
        private bool ValidateInput(NCRRequest ncrRequest, string BinaryData)
        {
            log.LogMethodEntry(ncrRequest, BinaryData);

            try
            {
                for (int i = 0; i < BinaryData.Length; i++)
                {
                    if (BinaryData[i].Equals('1'))
                    {
                        try
                        {
                            switch (i + 1)
                            {
                                case 1: break;//bit 1 is not required to set.
                                case 2: break;//bit 2 is not required to set.
                                case 3: break;//bit 3 is not required to set.
                                case 4: NCRApiFunctions.Set.PurchaseAmount(ncrRequest.Amount);
                                    break;
                                case 5: NCRApiFunctions.Set.CashBackAmount(0);
                                    break;
                                case 6: NCRApiFunctions.Set.FeeAmount(ncrRequest.Amount);
                                    break;
                                case 7: NCRApiFunctions.Set.AccountType("0");
                                    break;
                                case 8: break;//bit 8 is not required to set.
                                case 9: //NCRApiFunctions.Set.ManualEntryTrack2Flag(true);
                                    //NCRApiFunctions.Get.PersonalAccountNumber(ncrRequest.PersonalAccountNumber);
                                    if (!string.IsNullOrEmpty(ncrRequest.PersonalAccountNumber.ToString()))
                                    {
                                        NCRApiFunctions.Set.PersonalAccountNumber(ncrRequest.PersonalAccountNumber.ToString());
                                    }
                                    break;
                                case 10: if (!string.IsNullOrEmpty(ncrRequest.ExpirationDate))
                                    {
                                        NCRApiFunctions.Set.ExpirationDate(ncrRequest.ExpirationDate);
                                    }
                                    break;
                                case 11: break;//bit 11 is not required to set.
                                case 12: break;//bit 12 is not required to set.
                                case 13: break;//bit 13 is not required to set.
                                case 14: break;//bit 14 is not required to set.
                                case 15: if (!string.IsNullOrEmpty(ncrRequest.VoucherNumber))
                                    {
                                        NCRApiFunctions.Set.VoucherNumber(ncrRequest.VoucherNumber);
                                    }
                                    break;
                                case 16: break;//bit 13 is not required to set.
                                case 17: break;//bit 14 is not required to set.
                                case 18: 
                                        NCRApiFunctions.Set.Track2Data(ncrRequest.Track2Data);
                                    
                                    break;
                                case 19: if (!string.IsNullOrEmpty(ncrRequest.ManagerID))
                                    {
                                        NCRApiFunctions.Set.ManagerID(ncrRequest.ManagerID);
                                    }
                                    break;
                                case 20: break;//bit 20 is not required to set.
                                case 21: NCRApiFunctions.Set.MTXSequenceNumber(ncrRequest.SequenceNo.ToString()); break;//bit 21 is not required to set.
                                case 22: break;//bit 22 is not required to set.
                                case 23: break;//bit 23 is not required to set.
                                case 24: break;//bit 24 is not required to set.
                                case 25:
                                    if (ncrRequest.TransactionType.ToString().Equals(eTransactionType.FORCE.ToString()))
                                    {
                                        NCRApiFunctions.Set.AuthorizationNumber(ncrRequest.AuthorizationNumber);
                                    }
                                    break;

                                case 26: break;//bit 26 is not required to set.
                                case 27: break;//bit 27 is not required to set.
                                case 28: break;//bit 28 is not required to set.
                                case 29: break;//bit 29 is not required to set.
                                case 30: break;//bit 30 is not required to set.
                                case 31: break;//bit 31 is not required to set.
                                case 32: break;//bit 32 is not required to set.
                                case 33: break;//bit 33 is not required to set.
                                case 34: break;//bit 34 is not required to set.
                                case 35: break;//bit 35 is not required to set.
                                case 36: break;//bit 36 is not required to set.
                                case 37:
                                    if (!string.IsNullOrEmpty(ncrRequest.PostTransactionNumber))
                                    {
                                        NCRApiFunctions.Set.POSTTransactionNumber(ncrRequest.PostTransactionNumber);
                                    }
                                    break;
                                case 38: break;//bit 38 is not required to set.
                                case 39: break;//bit 39 is not required to set.
                                case 40: break;//bit 40 is not required to set.
                                case 41: break;//bit 41 is not required to set.
                                case 42: break;//bit 42 is not required to set.
                                case 43: break;//bit 43 is not required to set.
                                case 44: break;//bit 44 is not required to set.
                                case 45: break;//bit 45 is not required to set.
                                case 46: break;//bit 46 is not required to set.
                                case 47:if (!string.IsNullOrEmpty(ncrRequest.UPCCode))
                                    { 
                                    NCRApiFunctions.Set.UPCCode(ncrRequest.UPCCode);
                                }
                                    break;

                                case 48: break;//bit 48 is not required to set.
                                case 49: break;//bit 49 is not required to set.
                                case 50: break;//bit 50 is not required to set.
                                case 51: break;//bit 51 is not required to set.
                                case 52: break;//bit 52 is not required to set.
                                case 53: break;//bit 53 is not required to set.
                                case 54: break;//bit 54 is not required to set.
                                case 55: break;//bit 55 is not required to set.
                                case 56: break;//bit 56 is not required to set.
                                case 57: break;//bit 57 is not required to set.
                                case 58: break;//bit 58 is not required to set.
                                case 59: break;//bit 59 is not required to set.
                                case 60: break;//bit 60 is not required to set.
                                case 61: break;//bit 61 is not required to set.
                                case 62:if (!string.IsNullOrEmpty(ncrRequest.SecondaryIDType))
                                    { 
                                    NCRApiFunctions.Set.SecondaryIDType(ncrRequest.SecondaryIDType);
                                }
                                    break;
                                case 63: if (!string.IsNullOrEmpty(ncrRequest.SecondaryID))
                                    {
                                        NCRApiFunctions.Set.SecondaryIDType(ncrRequest.SecondaryID);
                                    }
                                    break;
                                case 64: NCRApiFunctions.Set.CashierOKCashback(false);
                                    break;
                                case 65: break;//bit 65 is not required to set.
                                case 66: break;//bit 66 is not required to set.
                                case 67: break;//bit 67 is not required to set.
                                case 68: break;//bit 68 is not required to set.
                                case 69: break;//bit 69 is not required to set.
                                case 70: break;//bit 70 is not required to set.
                                case 71: break;//bit 71 is not required to set.
                                case 72: break;//bit 72 is not required to set.
                                case 73: break;//bit 73 is not required to set.
                                case 74: break;//bit 74 is not required to set.
                                case 75: break;//bit 75 is not required to set.
                                case 76: break;//bit 76 is not required to set.
                                case 77: break;//bit 77 is not required to set.
                                case 78: break;//bit 78 is not required to set.
                                case 79: break;//bit 79 is not required to set.
                                case 80: break;//bit 80 is not required to set.
                                case 81: break;//bit 81 is not required to set.
                                case 82: break;//bit 82 is not required to set.
                                case 83: break;//bit 83 is not required to set.
                                case 84: NCRApiFunctions.Set.TaxAmount(ncrRequest.TaxAmount);
                                    break;
                                case 85: break;//bit 75 is not required to set.
                                case 86: break;//bit 76 is not required to set.
                                case 87: break;//bit 77 is not required to set.
                                case 88: break;//bit 78 is not required to set.
                                case 89: break;//bit 79 is not required to set.
                                case 90: if (NCRAdapter.IsUnattended || MessageBox.Show("Please verify the credit card.\n Are you verified?", "Credit Card Verification", MessageBoxButtons.YesNo) == DialogResult.OK)
                                    {
                                        NCRApiFunctions.Set.POSVerifyCard(true);
                                    }
                                    break;
                                case 91: break;//bit 91 is not required to set.
                                case 92: break;//bit 92 is not required to set.
                                case 93: break;//bit 93 is not required to set.
                                case 94: break;//bit 94 is not required to set.
                                case 95: break;//bit 95 is not required to set.
                                case 96: break;//bit 96 is not required to set.
                                case 97: break;//bit 97 is not required to set.
                                case 98: break;//bit 98 is not required to set.
                                case 99: break;//bit 99 is not required to set.
                                case 100: break;//bit 100 is not required to set.
                                case 101: break;//bit 101 is not required to set.
                                case 102: break;//bit 102 is not required to set.
                                case 103: break;//bit 103 is not required to set.
                                case 104: break;//bit 104 is not required to set.
                                case 105: break;//bit 105 is not required to set.
                                case 106: break;//bit 106 is not required to set.
                                case 107: break;//bit 107 is not required to set.
                                case 108: break;//bit 108 is not required to set.
                                case 109: break;//bit 109 is not required to set.
                                case 110: break;//bit 110 is not required to set.
                                case 111: break;//bit 111 is not required to set.
                                case 112: break;//bit 112 is not required to set.
                                case 113: break;//bit 113 is not required to set.
                                case 114: break;//bit 114 is not required to set.
                                case 115: break;//bit 115 is not required to set.
                                case 116: break;//bit 116 is not required to set.
                                case 117: break;//bit 117 is not required to set.
                                case 118: break;//bit 118 is not required to set.
                                case 119: break;//bit 119 is not required to set.
                                case 120: break;//bit 120 is not required to set.
                                case 121: break;//bit 121 is not required to set.
                                case 122: break;//bit 122 is not required to set.
                                case 123: break;//bit 123 is not required to set.
                                case 124: break;//bit 124 is not required to set.
                                case 125: break;//bit 125 is not required to set.
                                case 126: break;//bit 126 is not required to set.
                                case 127: break;//bit 127 is not required to set.
                                case 128: break;//bit 128 is not required to set.
                            }
                        }
                        catch(Exception ex)
                        {
                            log.Error("Error occured in EFT", ex);
                            txtStatus.Text = "EFT error, contact support";
                            txtStatus.Refresh();
                            log.LogMethodExit(false);
                            return false;
                        }
                    }
                }

                log.LogMethodExit(true);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error occured while Validating Input", ex);
                message = ex.ToString();
                log.Fatal("Ends:ValidateInput(ncrRequest," + BinaryData + ") with exception:" + ex.ToString());
                log.LogMethodExit(false);
                return false;
            }
        }

        private void DefineResponse(string Response, ref string responseText )
        {
            log.LogMethodEntry(Response, responseText);

            switch (Response)
            {
                case "AA": responseText = "Approved by the Host";
                    break;
                case "AB": responseText = "Approved by WinEPS break"; break;
                case "AC": responseText = "Approved for Electronic Check Conversion."; break;
                case "AP": responseText = "Approved; enter new Purchase Order Number."; break;
                case "AS": responseText = "Approved for ECC, but Requires Signature."; break;
                case "IC": responseText = "Invalid Cashier."; break;
                case "NA": responseText = "Additional Data Required – Additional customer information is needed"; break;
                case "NB": responseText = "Soft Decline – Declined, Insufficient Funds – Balance Remaining on Card"; break;
                case "NC": responseText = "Soft Decline – An invalid Cashback amount (too high or too low) has been entered."; break;
                case "ND": responseText = "Hard decline – cannot be overridden."; break;
                case "NE": responseText = "Soft Decline – Re-Enter MICR"; break;
                case "NF": responseText = "Soft Decline – possible override allowed."; break;
                case "NG": responseText = "Soft Decline – Re-Enter MICR and get customer ID"; break;
                case "NH": responseText = "Hard Decline – Connection to the host is down."; break;
                case "NI": responseText = "Soft Decline – Soft Decline for the void of a Debit"; break;
                case "NM": responseText = "Soft Decline – Invalid manager ID."; break;
                case "NO": responseText = "Soft Decline – possible override allowed."; break;
                case "NP":
                case "NW": responseText = "Soft Decline – The customer has entered an incorrect PIN, OR the WorkingKey changed.";
                    break;
                case "NR": responseText = "Soft Decline – possible override allowed."; break;
                case "NT": responseText = "Soft Decline – Transaction is tax exempt."; break;
                case "NV": responseText = "Soft Decline – Try for Voice Auth."; break;
            }
            log.LogMethodExit(null);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            btnCancel.Enabled = false;            
            WaitResponseTimer.Stop();
            KillTheThread();
            txtStatus.Text = message;
            Thread.Sleep(250);
            updateMessage();           
            message = "Trasnaction Cancelled.";           
            Thread.Sleep(3000);
            this.Close();
            btnCancel.Enabled = true;

            log.LogMethodExit(null);
        }
        private void GetReceipt(ref string cReceiptText, ref string mReceiptText)
        {
            log.LogMethodEntry(cReceiptText, mReceiptText);

            int lineCount = 0;
            int lineNo = 1;
            StringBuilder customerReciept = new StringBuilder(40);
            StringBuilder merchantReciept = new StringBuilder(40);
            StringBuilder line=new StringBuilder(40);
            
                try
                {
                    NCRApiFunctions.Get.ReceiptHeaderCount(ref lineCount);
                    while (lineCount != 0 && lineNo <= lineCount)
                    {
                        NCRApiFunctions.Get.ReceiptHeaderLine(lineNo, line);
                        cReceiptText += line + Environment.NewLine;
                        mReceiptText += line + Environment.NewLine;
                        lineNo++;
                    }
                    lineCount = 0;
                    lineNo = 1;
                    NCRApiFunctions.Get.ReceiptCustomerLinesCount(ref lineCount);
                    while (lineCount != 0 && lineNo <= lineCount)
                    {
                        NCRApiFunctions.Get.ReceiptCustomerLine(lineNo, customerReciept);                        
                        cReceiptText += customerReciept + Environment.NewLine;                                               
                        lineNo++;                        
                    }
                    lineCount = 0;
                    lineNo = 1;
                    NCRApiFunctions.Get.ReceiptDrawerLinesCount(ref lineCount);
                    while (lineCount != 0 && lineNo <= lineCount)
                    {
                        NCRApiFunctions.Get.ReceiptDrawerLine(lineNo, merchantReciept);
                        mReceiptText += merchantReciept.ToString() + Environment.NewLine;
                        lineNo++;
                    }
                    lineCount = 0;
                    lineNo =1;
                    NCRApiFunctions.Get.ReceiptTrailCount(ref lineCount);
                    while (lineCount != 0 && lineNo <= lineCount)
                    {
                        NCRApiFunctions.Get.ReceiptTrailLine(lineNo, line);
                        cReceiptText += line + Environment.NewLine;
                        mReceiptText += line + Environment.NewLine;
                        lineNo++;
                    }                    
                }
                catch(Exception ex)
                {
                    log.Error("Error occured while getting receipt", ex);

                }

            log.LogMethodExit(null);           
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        private void KillTheThread()
        {
            log.LogMethodEntry();
            myThread.Abort();
            log.LogMethodExit(null);
        }
        
        private void WaitResponseTimer_Tick(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);

            try
            {
                if (IsCashierDisplay)
                {
                    btnCancel.Enabled = IsCancelEnable;
                    updateMessage();
                    if (IsThreadCompleted)
                    {
                        WaitResponseTimer.Stop();
                        //Thread.Sleep(500);                    
                        Thread.Sleep(3000);
                        KillTheThread();
                        this.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured while waiting response timer", ex);
                log.Fatal("Error-WaitResponseTimer_Tick() method throwing exception");
            }

            log.LogMethodExit(null);
        }

        private void updateMessage()
        {
            log.LogMethodEntry();
            cashierDisplay = new StringBuilder(20);
            cashierDisplay2 = new StringBuilder(20);
            NCRApiFunctions.Get.CashierDisplay1(cashierDisplay);
            NCRApiFunctions.Get.CashierDisplay2(cashierDisplay2);
            message = string.IsNullOrEmpty(cashierDisplay.ToString().Trim()) ? message : cashierDisplay.ToString() +" "+ cashierDisplay2.ToString();
            if (!string.IsNullOrEmpty(message))
            {
                txtStatus.Text = message.Replace("\n\r"," ");
                txtStatus.Refresh();
            }
            log.LogMethodExit(null);
        }
        private void TimeOutReversal()
        {
            log.LogMethodEntry();
            KillTheThread();
            log.LogMethodExit(null);
        }

        private void NCRCore_Load(object sender, EventArgs e)
        {
            log.LogMethodEntry(sender, e);
            txtStatus.Text = "Pinpad is initializing... Please Wait...";
            txtStatus.Refresh();
            log.LogMethodExit(null);
        }
       
    }
}
