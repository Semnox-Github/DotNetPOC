/********************************************************************************************
 * Project Name - China ICBC Transaction Response
 * Description  - This is the response class of china ICBC.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        02-Aug-2017   Raghuveera          Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// ChinaICBCTransactionResponse
    /// </summary>
    public class ChinaICBCTransactionResponse
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// stores the response of ICBC
        /// </summary>
        public ICBCOutputStruct responseStruct;
        /// <summary>
        /// Stores the response id
        /// </summary>
        public int ccResponseId;
        /// <summary>
        /// Transaction order
        /// </summary>
        public string TransType;//= new char[2];
        /// <summary>
        /// Transaction card number
        /// </summary>
        public string CardNo;//= new char[19];
        /// <summary>
        /// Transaction amount
        /// </summary>
        public string Amount;//= new char[12];
        /// <summary>
        /// Tip amount
        /// </summary>
        public string TipAmount;//= new char[12];
        /// <summary>
        /// Transaction time
        /// </summary>
        public string TransTime;//= new char[6];
        /// <summary>
        ///  Transaction date
        /// </summary>
        public string TransDate;//= new char[8];
        /// <summary>
        ///  Card validity
        /// </summary>
        public string ExpDate;//= new char[4];
        /// <summary>
        ///  Second track information
        /// </summary>
        public string Track2;//= new char[37];
        /// <summary>
        /// Third track information
        /// </summary>
        public string Track3;//= new char[104];
        /// <summary>
        /// System search number
        /// </summary>
        public string ReferNo;//= new char[8];
        /// <summary>
        ///  Authorization number
        /// </summary>
        public string AuthNo;//= new char[6];
        /// <summary>
        /// Return code
        /// </summary>
        public string RspCode;//= new char[2];
        /// <summary>
        ///  Transaction terminal number
        /// </summary>
        public string TerminalId;//= new char[15];
        /// <summary>
        ///  Transaction merchant number
        /// </summary>
        public string MerchantId;//= new char[12];
        /// <summary>
        /// UnionPay merchant number
        /// </summary>
        public string YLMerchantId;//= new char[15];
        /// <summary>
        ///  number of monthly instalments
        /// </summary>
        public string InstallmentTimes;//= new char[2];
        /// <summary>
        /// IC card data
        /// </summary>
        public string TCData;//= new char[256];
        /// <summary>
        ///  English merchant name
        /// </summary>
        public string MerchantNameEng;//= new char[50];
        /// <summary>
        /// Chinese merchant name
        /// </summary>
        public string MerchantNameChs;//= new char[40];
        /// <summary>
        /// Terminal serial number
        /// </summary>
        public string TerminalTraceNo;//= new char[6];
        /// <summary>
        /// Terminal batch No.
        /// </summary>
        public string TerminalBatchNo;//= new char[6];
        /// <summary>
        /// IC card serial number
        /// </summary>
        public string IcCardId;//= new char[4];
        /// <summary>
        /// Issuing bank name
        /// </summary>
        public string BankName;//= new char[20];
        /// <summary>
        ///  Chinese transaction name
        /// </summary>
        public string TransName;//= new char[20];
        /// <summary>
        /// Card class    
        /// </summary>
        public string CardType;//= new char[20];
        /// <summary>
        ///  Transaction summary information，Print ledger need
        /// </summary>
        public string TotalInfo;//= new char[800];
        /// <summary>
        /// When the transaction fails，MISPOS The system returns Chinese error description information
        /// </summary>
        public string RspMessage;//= new char[100];
        /// <summary>
        /// Memo information
        /// </summary>
        public string Remark;//= new char[300];
        /// <summary>
        /// Foreign card Serial number
        /// </summary>
        public string WTrace;//= new char[24];
        /// <summary>
        /// AID(IC card data item)
        /// </summary>
        public string AIDDAT;//= new char[34];
        /// <summary>
        /// APPLABEL(IC card data item)
        /// </summary>
        public string APPLBL;//= new char[20];
        /// <summary>
        /// APPNAME(IC card data item)
        /// </summary>
        public string APPNAM;//= new char[20];
        /// <summary>
        ///  Offline transaction summary information
        /// </summary>
        public string ElecTotal;//= new char[32];
        /// <summary>
        ///  Actual debit amount
        /// </summary>
        public string SettleAmount;//= new char[12];
        /// <summary>
        /// QR code Payment number
        /// </summary>
        public string QRCardNO;//= new char[50];
        /// <summary>
        /// QR code Order number
        /// </summary>
        public string QROrderNo;//= new char[50];
        /// <summary>
        /// QR code Preferential payment information:( Integral deduction 12+ Deductible amount of electronic coupons 12+ 
        /// Coupon deduction amount 12+ Bank deduction 12+ Merchant Commission 12+ Order number 30 This field is all visual characters，
        /// In addition, ICBC's design for this field is variable in length，This field may be empty，There may be only one order number，
        /// The amount of money in the back is also undetermined，There is a possibility of an amount，There may be more than one amount，
        /// The actual return of the ICBC shall be subject to the actual return，Cashier system here to resolve the expansion of this space)
        /// </summary>
        public string QRMemo;//= new char[300];
        /// <summary>
        /// cashier number
        /// </summary>
        public string PlatId;//= new char[20];
        /// <summary>
        /// Operation number
        /// </summary>
        public string OperId;//= new char[20];
        /// <summary>
        ///Receipt Text
        /// </summary>
        public string ReceiptText;



        /// <summary>
        /// GetClass
        /// </summary>
        public void GetClass()
        {
            log.LogMethodEntry();

            TransType = new string(responseStruct.TransType).Trim() ;
            CardNo = new string(responseStruct.CardNo).Trim();
            Amount = new string(responseStruct.Amount).Trim();
            TipAmount = new string(responseStruct.TipAmount).Trim();
            TransTime = new string(responseStruct.TransTime).Trim();
            TransDate = new string(responseStruct.TransDate).Trim();
            ExpDate = new string(responseStruct.ExpDate).Trim();
            Track2 = new string(responseStruct.Track2).Trim();
            Track3 = new string(responseStruct.Track3).Trim();
            CardNo = new string(responseStruct.CardNo).Trim();
            ReferNo = new string(responseStruct.ReferNo).Trim();
            AuthNo = new string(responseStruct.AuthNo).Trim();
            RspCode = new string(responseStruct.RspCode).Trim();
            TerminalId = new string(responseStruct.TerminalId).Trim();
            InstallmentTimes = new string(responseStruct.InstallmentTimes).Trim();
            MerchantId = new string(responseStruct.MerchantId).Trim();
            YLMerchantId = new string(responseStruct.YLMerchantId).Trim();
            TCData = new string(responseStruct.TCData).Trim();
            MerchantNameEng = new string(responseStruct.MerchantNameEng).Trim();
            MerchantNameChs = new string(responseStruct.MerchantNameChs).Trim();
            TerminalTraceNo = new string(responseStruct.TerminalTraceNo).Trim();
            TerminalBatchNo = new string(responseStruct.TerminalBatchNo).Trim();
            IcCardId = new string(responseStruct.IcCardId).Trim();
            BankName = new string(responseStruct.BankName).Trim();
            TransName = new string(responseStruct.TransName).Trim();
            CardType = new string(responseStruct.CardType).Trim();
            TotalInfo = new string(responseStruct.TotalInfo).Trim();
            RspMessage = new string(responseStruct.RspMessage).Trim();
            Remark = new string(responseStruct.Remark).Trim();
            WTrace = new string(responseStruct.WTrace).Trim();
            AIDDAT = new string(responseStruct.AIDDAT).Trim();
            APPLBL = new string(responseStruct.APPLBL).Trim();
            APPNAM = new string(responseStruct.APPNAM).Trim();
            ElecTotal = new string(responseStruct.ElecTotal).Trim();
            SettleAmount = new string(responseStruct.SettleAmount).Trim();
            QRCardNO = new string(responseStruct.QRCardNO).Trim();
            QROrderNo = new string(responseStruct.QROrderNo).Trim();
            QRMemo = new string(responseStruct.QRMemo).Trim();
            PlatId = new string(responseStruct.platId).Trim();
            OperId = new string(responseStruct.operId).Trim();

            log.LogMethodExit(null);
        }

        /// <summary>
        /// ICBCOutputStruct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ICBCOutputStruct
        {
            /// <summary>
            /// Transaction order
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] TransType; // Transaction order
            /// <summary>
            /// Transaction card number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public char[] CardNo; // Transaction card number
            /// <summary>
            /// Transaction amount
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] Amount; // Transaction amount
            /// <summary>
            /// Tip amount
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] TipAmount; // Tip amount
            /// <summary>
            /// Transaction time
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] TransTime; // Transaction time
            /// <summary>
            /// Transaction date
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] TransDate; // Transaction date
            /// <summary>
            /// Card validity
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] ExpDate; // Card validity
            /// <summary>
            /// Second track information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37)]
            public char[] Track2; // Second track information
            /// <summary>
            /// Third track information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 104)]
            public char[] Track3; // Third track information
            /// <summary>
            ///  System search number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] ReferNo; // System search number
            /// <summary>
            /// Authorization number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] AuthNo; // Authorization number
            /// <summary>
            /// Return code
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] RspCode; // Return code
            /// <summary>
            ///  Transaction terminal number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public char[] TerminalId; // Transaction terminal number
            /// <summary>
            ///  Transaction merchant number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] MerchantId; // Transaction merchant number
            /// <summary>
            /// UnionPay merchant number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public char[] YLMerchantId; // UnionPay merchant number
            /// <summary>
            /// number of monthly instalments
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] InstallmentTimes; // number of monthly instalments
            /// <summary>
            /// IC card data
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] TCData; //IC card data
            /// <summary>
            /// English merchant name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public char[] MerchantNameEng; // English merchant name
            /// <summary>
            /// Chinese merchant name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
            public char[] MerchantNameChs; // Chinese merchant name
            /// <summary>
            /// Terminal serial number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] TerminalTraceNo; //Terminal serial number
            /// <summary>
            /// Terminal batch No.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] TerminalBatchNo; // Terminal batch No.
            /// <summary>
            ///  IC card serial number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] IcCardId; // IC card serial number
            /// <summary>
            /// Issuing bank name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] BankName; // Issuing bank name
            /// <summary>
            /// Chinese transaction name
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] TransName; // Chinese transaction name
            /// <summary>
            /// Card class 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] CardType; //Card class 
            /// <summary>
            /// Transaction summary information,Print ledger need
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 800)]
            public char[] TotalInfo; // Transaction summary information,Print ledger need
            /// <summary>
            ///  When the transaction fails,MISPOS The system returns Chinese error description information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
            public char[] RspMessage; // When the transaction fails,MISPOS The system returns Chinese error description information
            /// <summary>
            ///  Memo information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 300)]
            public char[] Remark; // Memo information
            /// <summary>
            /// Foreign card Serial number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            public char[] WTrace; //Foreign card Serial number
            /// <summary>
            /// AID(IC card data item)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 34)]
            public char[] AIDDAT; //AID(IC card data item)
            /// <summary>
            /// APPLABEL(IC card data item)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] APPLBL; //APPLABEL(IC card data item)
            /// <summary>
            /// APPNAME(IC card data item)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] APPNAM; //APPNAME(IC card data item)
            /// <summary>
            /// Offline transaction summary information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] ElecTotal; // Offline transaction summary information
            /// <summary>
            /// Actual debit amount
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] SettleAmount;// Actual debit amount
            /// <summary>
            /// /QR code Payment number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public char[] QRCardNO; //QR code Payment number
            /// <summary>
            /// QR code Order number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public char[] QROrderNo; //QR code Order number
            /// <summary>
            /// QR code Preferential payment information:( Integral deduction 12+ Deductible amount of electronic coupons 12+ Coupon deduction amount 12+ Bank deduction 12+ Merchant Commission 12+ Order number 30 This field is all visual characters,In addition, ICBC's design for this field is variable in length,This field may be empty,There may be only one order number,The amount of money in the back is also undetermined,There is a possibility of an amount,There may be more than one amount,The actual return of the ICBC shall be subject to the actual return,Cashier system here to resolve the expansion of this space)
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 300)]
            public char[] QRMemo; /*QR code Preferential payment information:( Integral deduction 12+ Deductible amount of electronic coupons 12+ Coupon deduction amount 12+ Bank deduction 12+ Merchant Commission 12+ Order number 30 This field is all visual characters,In addition, ICBC's design for this field is variable in length,This field may be empty,There may be only one order number,The amount of money in the back is also undetermined,There is a possibility of an amount,There may be more than one amount,The actual return of the ICBC shall be subject to the actual return,Cashier system here to resolve the expansion of this space)*/
            /// <summary>
            /// cashier number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] platId; //cashier number
            /// <summary>
            /// Operation number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] operId; // Operation number
        }        
    }
    

}
