/********************************************************************************************
 * Project Name - China ICBC Transaction Request
 * Description  - All the transaction request can be performed using the object of this response class.
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
    /// ChinaICBCTransactionRequest class
    /// </summary>
    public class ChinaICBCTransactionRequest
    {
        private static readonly  Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// requestStruct
        /// </summary>
        public ICBCInputStruct requestStruct;
        /// <summary>
        /// Trxid
        /// </summary>
        public int Trxid;

        /// <summary>
        /// Transaction order
        /// </summary>
        public string TransType;//= new char[2]; 
        /// <summary>
        /// Branch features script ID
        /// </summary>
        public string FuncID;// = new char[4]; // Branch features script ID
        /// <summary>
        /// Transaction amount
        /// </summary>
        public string TransAmount;// = new char[12]; // Transaction amount
        /// <summary>
        /// Tip amount
        /// </summary>
        public string TipAmount;// = new char[12]; // Tip amount
        /// <summary>
        /// Transaction date
        /// </summary>
        public string TransDate;// = new char[8]; // Transaction date
        /// <summary>
        /// MIS Serial number
        /// </summary>
        public string MisTraceNo;//= new char[6];	//MIS Serial number
        /// <summary>
        /// Transaction card number
        /// </summary>
        public string CardNo;//= new char[19]; // Transaction card number
        /// <summary>
        /// Card validity
        /// </summary>
        public string ExpDate;//= new char[4]; // Card validity
        /// <summary>
        /// second track information
        /// </summary>
        public string Track2;//= new char[37]; // second track information
        /// <summary>
        ///  Third track information
        /// </summary>
        public string Track3;//= new char[104]; // Third track information
        /// <summary>
        ///  System search number
        /// </summary>
        public string ReferNo;//= new char[8]; // System search number
        /// <summary>
        ///  Authorization number
        /// </summary>
        public string AuthNo;//= new char[6]; // Authorization number
        /// <summary>
        ///  Transaction terminal number
        /// </summary>
        public string TerminalId;//= new char[15]; // Transaction terminal number
        /// <summary>
        /// number of monthly instalments 
        /// </summary>
        public string InstallmentTimes;//= new char[2]; // number of monthly instalments 
        /// <summary>
        /// Pre entry Annex description 1
        /// </summary>
        public string PreInput;//= new char[256]; // Pre entry Annex description 1
        /// <summary>
        /// Fixed entry Annex description 2
        /// </summary>
        public string AddDatas;//= new char[256]; // Fixed entry Annex description 2
        /// <summary>
        /// QR code payment number
        /// </summary>
        public string QRCardNO;//= new char[50]; // QR code payment number
        /// <summary>
        ///  QR code order number
        /// </summary>
        public string QROrderNo;//= new char[50]; // QR code order number
        /// <summary>
        /// Cash register number
        /// </summary>
        public string PlatId;//= new char[20]; // Cash register number
        /// <summary>
        /// Operator number
        /// </summary>
        public string OperId;//= new char[20]; // Operator number

        /// <summary>
        /// SetStructure
        /// </summary>


        public void SetStructure()
        {
            log.LogMethodEntry();

            requestStruct.transType = (string.IsNullOrEmpty(TransType)) ? null : TransType.ToArray<char>();
            requestStruct.funcId = (string.IsNullOrEmpty(FuncID)) ? null : FuncID.ToArray<char>();
            requestStruct.transAmount = (string.IsNullOrEmpty(TransAmount)) ? null : TransAmount.ToArray<char>();
            requestStruct.tipAmount = (string.IsNullOrEmpty(TipAmount)) ? null : TipAmount.ToArray<char>();
            requestStruct.transDate = (string.IsNullOrEmpty(TransDate)) ? null : TransDate.ToArray<char>();
            requestStruct.misTraceNo = (string.IsNullOrEmpty(MisTraceNo)) ? null : MisTraceNo.ToArray<char>();
            requestStruct.cardNo = (string.IsNullOrEmpty(CardNo)) ? null : CardNo.ToArray<char>();
            requestStruct.expDate = (string.IsNullOrEmpty(ExpDate)) ? null : ExpDate.ToArray<char>();
            requestStruct.track2 = (string.IsNullOrEmpty(Track2)) ? null : Track2.ToArray<char>();
            requestStruct.track3 = (string.IsNullOrEmpty(Track3)) ? null : Track3.ToArray<char>();
            requestStruct.referNo = (string.IsNullOrEmpty(ReferNo)) ? null : ReferNo.ToArray<char>();
            requestStruct.authNo = (string.IsNullOrEmpty(AuthNo)) ? null : AuthNo.ToArray<char>();
            requestStruct.terminalId = (string.IsNullOrEmpty(TerminalId)) ? null : TerminalId.ToArray<char>();
            requestStruct.installmentTimes = (string.IsNullOrEmpty(InstallmentTimes)) ? null : InstallmentTimes.ToArray<char>();
            requestStruct.preInput = (string.IsNullOrEmpty(PreInput)) ? null : PreInput.ToArray<char>();
            requestStruct.addDatas = (string.IsNullOrEmpty(AddDatas)) ? null : AddDatas.ToArray<char>();
            requestStruct.qRCardNO = (string.IsNullOrEmpty(QRCardNO)) ? null : QRCardNO.ToArray<char>();
            requestStruct.qROrderNo = (string.IsNullOrEmpty(QROrderNo)) ? null : QROrderNo.ToArray<char>();
            requestStruct.platId = (string.IsNullOrEmpty(PlatId)) ? null : PlatId.ToArray<char>();
            requestStruct.operId = (string.IsNullOrEmpty(OperId)) ? null : OperId.ToArray<char>();

            log.LogVariableState("requestStruct", requestStruct);
            log.LogMethodExit(null);
        }

        /// <summary>
        /// GetClass
        /// </summary>
        public void GetClass()
        {
            log.LogMethodEntry();

            TransType = requestStruct.transType.ToString();
            FuncID = requestStruct.funcId.ToString();
            TransAmount = requestStruct.transAmount.ToString();
            TipAmount = requestStruct.tipAmount.ToString();
            TransDate = requestStruct.transDate.ToString();
            MisTraceNo = requestStruct.misTraceNo.ToString();
            CardNo = requestStruct.cardNo.ToString();
            ExpDate = requestStruct.expDate.ToString();
            Track2 = requestStruct.track2.ToString();
            Track3 = requestStruct.track3.ToString();
            ReferNo = requestStruct.referNo.ToString();
            AuthNo = requestStruct.authNo.ToString();
            TerminalId = requestStruct.terminalId.ToString();
            InstallmentTimes = requestStruct.installmentTimes.ToString();
            PreInput = requestStruct.preInput.ToString();
            AddDatas = requestStruct.addDatas.ToString();
            QRCardNO = requestStruct.qRCardNO.ToString();
            QROrderNo = requestStruct.qROrderNo.ToString();
            PlatId = requestStruct.platId.ToString();
            OperId = requestStruct.operId.ToString();

            log.LogMethodExit(null);
        }
        /// <summary>
        /// ICBCInputStruct
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct ICBCInputStruct
        {
            /// <summary>
            /// transType
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] transType;
            /// <summary>
            /// funcId
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] funcId;
            /// <summary>
            /// transAmount
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] transAmount;
            /// <summary>
            /// tipAmount
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            public char[] tipAmount;
            /// <summary>
            /// transDate
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] transDate;
            /// <summary>
            /// MIS Serial number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] misTraceNo;    //MIS Serial number
            /// <summary>
            /// Transaction card number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
            public char[] cardNo; // Transaction card number.=
            /// <summary>
            /// Card validity
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public char[] expDate; // Card validity
            /// <summary>
            /// second track information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 37)]
            public char[] track2; // second track information
            /// <summary>
            ///  Third track information
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 104)]
            public char[] track3; // Third track information
            /// <summary>
            /// System search number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] referNo; // System search number
            /// <summary>
            ///  Authorization number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] authNo; // Authorization number
            /// <summary>
            ///  Transaction terminal number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
            public char[] terminalId; // Transaction terminal number
            /// <summary>
            /// number of monthly instalments 
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public char[] installmentTimes; // number of monthly instalments 
            /// <summary>
            /// Pre entry Annex description 1
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] preInput; // Pre entry Annex description 1
            /// <summary>
            /// Fixed entry Annex description 2
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public char[] addDatas; // Fixed entry Annex description 2
            /// <summary>
            ///  QR code payment number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public char[] qRCardNO; // QR code payment number
            /// <summary>
            /// QR code order number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
            public char[] qROrderNo; // QR code order number
            /// <summary>
            /// Cash register number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] platId; // Cash register number
            /// <summary>
            /// Operator number
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] operId; // Operator number
        }       
    }
}
